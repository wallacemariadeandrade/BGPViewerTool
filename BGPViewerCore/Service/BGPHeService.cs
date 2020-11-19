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

        private IWebDriver GetDriverWithValidatedResponseFrom(string url)
        {
            // We've got to use a WebDriverWait to wait bgp.he.net page redirection
            var wb = new WebDriverWait(_driver, _timeout);
            _driver.Navigate().GoToUrl(url);
            wb.Until(x => x.Url == url);
            if(CheckIfDataExists(_driver))
                throw new KeyNotFoundException("Your query did not return any results.");
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

        private bool CheckIfDataExists(IWebDriver driver) => driver.FindElements(By.Id("error")).Count > 0;

        #endregion

        public AsnDetailsModel GetAsnDetails(int asNumber)
        {
            var driver = GetDriverWithValidatedResponseFrom($"https://bgp.he.net/AS{asNumber}");

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
            // https://bgp.he.net doesn't provide downstreams information,
            // so we'll just return null object
            return new Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>>(
                new AsnDetailsModel[]{}, 
                new AsnDetailsModel[]{}
            );    
        }

        public IEnumerable<IxModel> GetAsnIxs(int asNumber)
        {
            var driver = GetDriverWithValidatedResponseFrom($"https://bgp.he.net/AS{asNumber}");
            var brTagSplitArg = new string[]{"<br>"};
            if(driver.PageSource.Contains("exchange_table"))
            {
                foreach(var item in driver.FindElement(By.Id("exchange_table"))
                    .FindElement(By.TagName("tbody")).FindElements(By.TagName("tr")).Select(x => x.FindElements(By.TagName("td"))))
                {
                    var ixName = item.ElementAt(0).FindElement(By.TagName("a")).GetAttribute("innerHTML").TrimStart().TrimEnd();
                    var ixCountryCode = item.ElementAt(1).GetAttribute("innerHTML").TrimStart().TrimEnd();
                    var ixIpv4Addresses = item.ElementAt(3).GetAttribute("innerHTML").TrimStart().TrimEnd().Split(brTagSplitArg, StringSplitOptions.None);
                    var ixIpv6Addresses = item.ElementAt(4).GetAttribute("innerHTML").TrimStart().TrimEnd().Split(brTagSplitArg, StringSplitOptions.None);
                    var ixIpv6AddressesCount = ixIpv6Addresses.Count();
                    // We loop by IPv4 cause it'll be always equal to or 
                    // greater than IPv6
                    for(int index=0; index<ixIpv4Addresses.Count(); index++)
                    {
                        yield return new IxModel {
                            Name = ixName,
                            FullName = ixName,
                            CountryCode = ixCountryCode,
                            AsnSpeed = 0, // Information not given by bgp.he.net
                            IPv4 = ixIpv4Addresses.ElementAt(index),
                            IPv6 = index < ixIpv6AddressesCount ? ixIpv6Addresses.ElementAt(index) : null
                        };             
                    }
                }   
            }
        }

        public Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>> GetAsnPeers(int asNumber)
        {
            var driver = GetDriverWithValidatedResponseFrom($"https://bgp.he.net/AS{asNumber}");
            var hasIPv4Peers = driver.PageSource.Contains("table_peers4");
            var hasIPv6Peers = driver.PageSource.Contains("table_peers6");
            
            var ipv4Peers = hasIPv4Peers ? ExtractAsnsInfoFromPeerTable(driver.FindElement(By.Id("peers")))
                .Select(x => new AsnModel{
                    ASN = int.Parse(x[0]),
                    Name = x[1],
                    Description = x[2],
                    CountryCode = null
                }) : new AsnModel[]{};
                
            var ipv6Peers = hasIPv4Peers ? ExtractAsnsInfoFromPeerTable(driver.FindElement(By.Id("peers6")))
                .Select(x => new AsnModel{
                    ASN = int.Parse(x[0]),
                    Name = x[1],
                    Description = x[2],
                    CountryCode = null
                }) : new AsnModel[]{};
            
            return new Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>>(ipv4Peers, ipv6Peers);
        }

        public AsnPrefixesModel GetAsnPrefixes(int asNumber)
        {
            var allHtmlTables = GetDriverWithValidatedResponseFrom($"https://bgp.he.net/AS{asNumber}").FindElements(By.TagName("table"));
            var hasIpv4Prefixes = allHtmlTables.Count(table => table.GetAttribute("id") == "table_prefixes4") == 1;
            var hasIpv6Prefixes = allHtmlTables.Count(table => table.GetAttribute("id") == "table_prefixes6") == 1;
            var ipv4Prefixes = hasIpv4Prefixes ? ExtractPrefixesFromTablePrefixes(allHtmlTables.Single(table => table.GetAttribute("id") == "table_prefixes4"), IPV4_PREFIX_PATTERN) : new string[]{};
            var ipv6Prefixes = hasIpv6Prefixes ? ExtractPrefixesFromTablePrefixes(allHtmlTables.Single(table => table.GetAttribute("id") == "table_prefixes6"), IPV6_PREFIX_PATTERN) : new string[]{};
            return new AsnPrefixesModel
            {
                ASN = asNumber,
                IPv4 = ipv4Prefixes,
                IPv6 = ipv6Prefixes
            };
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