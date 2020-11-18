using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BGPViewerCore.Model;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.IO;
using System.Text;

namespace BGPViewerCore.Service
{
    public class BGPHeService : IBGPViewerService
    {
        private readonly IWebDriver _driver;
        private readonly TimeSpan _timeout;

        public BGPHeService(IWebDriver driver, int timeout)
        {
            _timeout = TimeSpan.FromSeconds(timeout);
            _driver = driver;
        }

        #region Constants
        private const string ASN_PATTERN = "(AS[0-9][0-9][0-9][0-9][0-9][0-9][0-9]|AS[0-9][0-9][0-9][0-9][0-9][0-9]|AS[0-9][0-9][0-9][0-9][0-9]|AS[0-9][0-9][0-9][0-9]|AS[0-9][0-9][0-9]|AS[0-9][0-9]|AS[0-9])";
        private const string IPV4_ADDRESS_PATTERN = @"((?:[0-9]{1,3}\.){3}[0-9]{1,3})";
        private const string IPV4_PREFIX_PATTERN = @"((?:[0-9]{1,3}\.){3}[0-9]{1,3}\/([0-9][0-9]|[0-9]))";
        private const string IPV6_PREFIX_PATTERN = @"(([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,}|::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]))\/([0-9][0-9][0-9]|[0-9][0-9]|[0-9])";
        private const string EMAIL_PATTERN = @"([a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*)";
        #endregion

        #region Routines

        private IWebDriver GetDriverWithResponseFrom(string url)
        {
            // We've got to use a WebDriverWait to wait bgp.he.net page redirection
            var wb = new WebDriverWait(_driver, _timeout);
            _driver.Navigate().GoToUrl(url);
            wb.Until(x => x.Url == url);
            return _driver;
        }

        private IEnumerable<string> ExtractEmailsFrom(string content)
            => Regex.Matches(content, EMAIL_PATTERN)
            .Cast<Match>() // This is necessary to access LINQ methods
            .Select(match => match.Value);
                
        
        private string ExtractDivWhoisFrom(string content)
        {
            var whoisDivStartIndex = content.IndexOf(@"<div id=""whois"" class=""tabdata hidden"">");
            if(whoisDivStartIndex < 0) throw new System.ArgumentException($"{content} doesn't contains div whois.");
            var whoisDivlength = content.IndexOf("</div>", whoisDivStartIndex) - whoisDivStartIndex;
            return  content.Substring(whoisDivStartIndex, whoisDivlength);
        }

        // At bgp.he.net/ASxxx upstreams are inside two  
        // HTML tables (IPv4 and IPv6) styled with "toppeertable" class
        private IEnumerable<string[]> ExtractAsnsAndNamesFromToppertable(string toppeertableText)
        {            
            using(var reader = new StringReader(toppeertableText.Split(new string[]{"ASN Name"}, StringSplitOptions.None)[1].TrimStart()))
            {
                var separator = new string[]{" "};
                while(reader.Peek() > 0)
                {
                    var line = reader.ReadLine();
                    var asnAndName = line.Split(separator, StringSplitOptions.None);
                    var asn = asnAndName[0].Substring(2);
                    var name = line.Replace($"AS{asn}", "").TrimStart();
                    yield return new string[] {
                        asn,
                        name
                    };
                }
            }
        }

        // At bgp.he.net/ASxxx IPv4 and IPv6 prefixes are inside two  
        // HTML tables (IPv4: table with id="table_prefixes4"; IPv6: table with id="table_prefixes6")
        private IEnumerable<string> ExtractPrefixesFromTablePrefixes(IWebElement tablePrefixes, string prefixPattern)
            => tablePrefixes.FindElements(By.ClassName("nowrap"))
                .Select(td => Regex.Match(td.FindElement(By.TagName("a")).GetAttribute("href"), prefixPattern).Value);

        // At bgp.he.net/ASxxx IPv4 and IPv6 peers are inside two  
        // HTML tables (IPv4: table with id="table_peers4"; IPv6: table with id="table_peers6"), 
        // within divs "peers" and "peers6"
        private IEnumerable<string[]> ExtractAsnsInfoFromPeerTable(IWebElement peerTable)
        {
            var tdList = peerTable.FindElement(By.TagName("tbody"))
                .FindElements(By.TagName("tr"))
                .Select(tr => tr.FindElements(By.TagName("td")));
            foreach (var td in tdList)
            {
                var asn = td.ElementAt(3).FindElement(By.TagName("a")).GetAttribute("innerHTML").Substring(2);
                // Extracting name and country
                var tdWithNameAndCountry = td.ElementAt(1);
                var innerHtmlOfTdWithNameAndCountry = tdWithNameAndCountry.GetAttribute("innerHTML");
                var name = innerHtmlOfTdWithNameAndCountry.Substring(0, innerHtmlOfTdWithNameAndCountry.IndexOf("<div")); // Gets only the portion with asn name
                var description = name;
                var country = tdWithNameAndCountry.FindElement(By.TagName("img")).GetAttribute("title");
                yield return new string[]{
                    asn, name, description, country
                };
            }
        }
            
        #endregion

        public AsnDetailsModel GetAsnDetails(int asNumber)
        {
            var driver = GetDriverWithResponseFrom($"https://bgp.he.net/AS{asNumber}");

            // h1 tag is always present and always contains the AS name/description
            var h1TitleElement = driver.FindElement(By.TagName("h1"));
            var asn = h1TitleElement.Text.Split(new string[]{" "}, StringSplitOptions.None)[0].Substring(2);
            var name = h1TitleElement.Text.Replace($"AS{asn} ", "");
            var description = name;

            // divs with classes "asleft" and "asright" are always present,
            // so we can use them to get country code and looking glass url
            var leftDivs = driver.FindElements(By.ClassName("asleft"));
            var rightDivs = driver.FindElements(By.ClassName("asright"));

            // splits "https://bgp.he.net/country/XX" string and gets XX
            var countryCode = rightDivs
                .SingleOrDefault(div => div.FindElements(By.TagName("a")).Count > 0 && 
                        div.FindElements(By.TagName("a")).ElementAt(0)
                        .GetAttribute("href").Contains("/country"))
                        ?.FindElement(By.TagName("a"))
                        ?.GetAttribute("href").Split('/')[4];
            
            // Extracts looking glass url
            var indexOfLookingGlassDiv = leftDivs.IndexOf(leftDivs.SingleOrDefault(div => div.Text.Contains("Looking Glass")));
            var lookingGlass = indexOfLookingGlassDiv != -1 ? rightDivs.ElementAt(indexOfLookingGlassDiv).Text : null;

            var emails = ExtractEmailsFrom(ExtractDivWhoisFrom(driver.PageSource));

            return new AsnDetailsModel
            {
                ASN = int.Parse(asn),
                Name = name,
                Description = description,
                CountryCode = countryCode,
                EmailContacts = emails,
                AbuseContacts = emails,
                LookingGlassUrl = lookingGlass
            };
        }

        public Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>> GetAsnDownstreams(int asNumber)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IxModel> GetAsnIxs(int asNumber)
        {
            throw new NotImplementedException();
        }

        public Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>> GetAsnPeers(int asNumber)
        {
            throw new NotImplementedException();
        }

        public AsnPrefixesModel GetAsnPrefixes(int asNumber)
        {
            throw new NotImplementedException();
        }

        public Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>> GetAsnUpstreams(int asNumber)
        {
            throw new NotImplementedException();
        }

        public IpDetailModel GetIpDetails(string ipAddress)
        {
            throw new NotImplementedException();
        }

        public PrefixDetailModel GetPrefixDetails(string prefix, byte cidr)
        {
            throw new NotImplementedException();
        }

        public SearchModel SearchBy(string queryTerm)
        {
            throw new NotImplementedException();
        }
    }
}