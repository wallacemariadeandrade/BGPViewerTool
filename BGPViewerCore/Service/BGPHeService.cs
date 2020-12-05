using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BGPViewerCore.Model;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.IO;

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
        private const string IPV6_ADDRESS_PATTERN = @"^\s*((([0-9A-Fa-f]{1,4}:){7}([0-9A-Fa-f]{1,4}|:))|(([0-9A-Fa-f]{1,4}:){6}(:[0-9A-Fa-f]{1,4}|((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3})|:))|(([0-9A-Fa-f]{1,4}:){5}(((:[0-9A-Fa-f]{1,4}){1,2})|:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3})|:))|(([0-9A-Fa-f]{1,4}:){4}(((:[0-9A-Fa-f]{1,4}){1,3})|((:[0-9A-Fa-f]{1,4})?:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){3}(((:[0-9A-Fa-f]{1,4}){1,4})|((:[0-9A-Fa-f]{1,4}){0,2}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){2}(((:[0-9A-Fa-f]{1,4}){1,5})|((:[0-9A-Fa-f]{1,4}){0,3}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){1}(((:[0-9A-Fa-f]{1,4}){1,6})|((:[0-9A-Fa-f]{1,4}){0,4}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(:(((:[0-9A-Fa-f]{1,4}){1,7})|((:[0-9A-Fa-f]{1,4}){0,5}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:)))(%.+)?\s*$";
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
            System.Diagnostics.Debug.WriteLine(_driver.PageSource);
            wb.Until(x => x.Url == url);
            if(!CheckIfDataExists(_driver))
                throw new KeyNotFoundException("Your query did not return any results.");
            return _driver;
        }

        private bool CheckIfDataExists(IWebDriver driver) => driver.FindElements(By.Id("error")).Count <= 0;

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
        // Each string array returned contains the as number (pos 0) and its name (pos 1)
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
        private IEnumerable<PrefixModel> ExtractPrefixesFromTablePrefixes(IWebElement tablePrefixes, string prefixPattern)
            // => tablePrefixes.FindElements(By.ClassName("nowrap"))
            //     .Select(td => Regex.Match(td.FindElement(By.TagName("a")).GetAttribute("href"), prefixPattern).Value);
            => tablePrefixes.FindElement(By.TagName("tbody"))
                            .FindElements(By.TagName("tr"))
                            .Select(tr => tr.FindElements(By.TagName("td")))
                            .Select(tds => new PrefixModel {
                                Prefix = Regex.Match(tds.ElementAt(0).FindElement(By.TagName("a")).GetAttribute("href"), prefixPattern).Value,
                                Name = tds.ElementAt(1).Text,
                                Description = tds.ElementAt(1).Text
                            });

        private Tuple<IEnumerable<PrefixModel>, IEnumerable<PrefixModel>> ExtractAsPrefixesFromDriver(IWebDriver driver)
        {
            var allHtmlTables = driver.FindElements(By.TagName("table"));
            
            var hasIpv4Prefixes = allHtmlTables.Count(table => table.GetAttribute("id") == "table_prefixes4") == 1;
            var hasIpv6Prefixes = allHtmlTables.Count(table => table.GetAttribute("id") == "table_prefixes6") == 1;

            var ipv4Prefixes = hasIpv4Prefixes ? ExtractPrefixesFromTablePrefixes(allHtmlTables.Single(table => table.GetAttribute("id") == "table_prefixes4"), IPV4_PREFIX_PATTERN) : new PrefixModel[]{};
            var ipv6Prefixes = hasIpv6Prefixes ? ExtractPrefixesFromTablePrefixes(allHtmlTables.Single(table => table.GetAttribute("id") == "table_prefixes6"), IPV6_PREFIX_PATTERN) : new PrefixModel[]{};

            return new Tuple<IEnumerable<PrefixModel>, IEnumerable<PrefixModel>>(ipv4Prefixes, ipv6Prefixes);
        }

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

        private IEnumerable<string[]> ExtractPrefixAndAsnInfoFromNetInfoDivTable(IWebDriver driver) 
        {
            var tables = driver.FindElement(By.Id("netinfo"))
                .FindElements(By.TagName("table"));

            foreach (var table in tables)
            {
                var matches = Regex.Matches(table.GetAttribute("innerHTML"), @"<a.*>.*<\/a>|<td>.*<\/td>");
                var isMatchCorrect = matches.Count % 3 == 0; // Each table has to match 3 rows (asn, prefix and name)
                if(!isMatchCorrect) throw new ArgumentException($"Incorrect input HTML. It was expected 3 matches and got {matches.Count}.", "tableInnerHtml");

                for (int i = 0; i < matches.Count; i+=3)
                {
                    var result = new string[3];
                    // Extract ASxxxx
                    result[0] = Regex.Match(matches[i].Value, ASN_PATTERN).Value.Substring(2);

                    // Extract prefix
                    result[1] = Regex.IsMatch(matches[i+1].Value, IPV4_PREFIX_PATTERN) ? 
                        Regex.Match(matches[i+1].Value, IPV4_PREFIX_PATTERN).Value : Regex.Match(matches[i+1].Value, IPV6_PREFIX_PATTERN).Value;

                    // Extract name/description
                    result[2] =  matches[i+2].Value.Replace("<td>", "").Replace("</td>", "");

                    yield return result;
                }
            }
        }

        private string ExtractCountryCodeFromDivWhois(string divWhois)
        {
            using(var reader = new StringReader(divWhois))
            {
                while(reader.Peek() > 0)
                {
                    var line = reader.ReadLine();
                    System.Diagnostics.Debug.WriteLine(line);
                    if(Regex.IsMatch(line, "Country:") || Regex.IsMatch(line, "country:"))
                    {
                        var splitted = line.Split(new string[]{" "}, StringSplitOptions.None);
                        return splitted.Last();
                    }
                }
                return null;
            }
        }

        private AsnDetailsModel ExtractAsDetailsFromDriver(IWebDriver driver, int asNumber)
        {
            // h1 tag is always present and always contains the AS name/description
            var h1TitleElement = driver.FindElement(By.TagName("h1"));
            var asn = h1TitleElement.Text.Split(new string[]{" "}, StringSplitOptions.None)[0].Substring(2);
            var name = h1TitleElement.Text.Replace($"AS{asn} ", "");
            var description = name;

            // divs with classes "asleft" and "asright" are always present,
            // so we can use them to get country code and looking glass url
            var leftDivs = driver.FindElements(By.ClassName("asleft"));
            var rightDivs = driver.FindElements(By.ClassName("asright"));

            var divWhois = ExtractDivWhoisFrom(driver.PageSource);
            var countryCode = ExtractCountryCodeFromDivWhois(divWhois);
            
            // Extracts looking glass url
            var indexOfLookingGlassDiv = leftDivs.IndexOf(leftDivs.SingleOrDefault(div => div.Text.Contains("Looking Glass")));
            var lookingGlass = indexOfLookingGlassDiv != -1 ? rightDivs.ElementAt(indexOfLookingGlassDiv).Text : null;

            var emails = ExtractEmailsFrom(divWhois);

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

        private IEnumerable<string[]> ExtractAsDataFromIpInfoElement(IWebElement ipInfoElement)
            => ipInfoElement.FindElement(By.TagName("table"))
                .FindElement(By.TagName("tbody"))
                .FindElements(By.TagName("tr"))
                .Select(tr => tr.FindElements(By.TagName("td")))
                .Select(tds => new string[]{
                    tds.ElementAt(0).FindElement(By.TagName("a")).GetAttribute("innerHTML").Substring(2), // AS number
                    tds.ElementAt(1).FindElement(By.TagName("a")).GetAttribute("innerHTML"), // Prefix
                    tds.ElementAt(2).GetAttribute("innerHTML") // Name/Description
                });

        #endregion

        public AsnDetailsModel GetAsnDetails(int asNumber)
        {
            var driver = GetDriverWithValidatedResponseFrom($"https://bgp.he.net/AS{asNumber}");
            return ExtractAsDetailsFromDriver(driver, asNumber);
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
            var driver = GetDriverWithValidatedResponseFrom($"https://bgp.he.net/AS{asNumber}");
            
            var prefixes = ExtractAsPrefixesFromDriver(driver);

            return new AsnPrefixesModel
            {
                ASN = asNumber,
                IPv4 = prefixes.Item1.Select(model => model.Prefix),
                IPv6 = prefixes.Item2.Select(model => model.Prefix)
            };  
        }

        public Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>> GetAsnUpstreams(int asNumber)
        {
            var toppeertables = GetDriverWithValidatedResponseFrom($"https://bgp.he.net/AS{asNumber}")
                .FindElements(By.ClassName("toppeertable"))
                .Select(table => table.Text);   
            
            var hasIpv4Upstreams = toppeertables.Count() >= 1;
            var hasIpv6Upstreams = toppeertables.Count() >= 2;

            var ipv4Upstreams = hasIpv4Upstreams ? ExtractAsnsAndNamesFromToppertable(toppeertables.ElementAt(0))
                    .Select(x => new AsnModel{
                        ASN = int.Parse(x[0]),
                        Name = x[1],
                        Description = x[1],
                        CountryCode = null
                    }) : new AsnModel[]{};

            var ipv6Upstreams = hasIpv6Upstreams ? ExtractAsnsAndNamesFromToppertable(toppeertables.ElementAt(1))
                    .Select(x => new AsnModel{
                        ASN = int.Parse(x[0]),
                        Name = x[1],
                        Description = x[1],
                        CountryCode = null
                    }) : new AsnModel[]{};

            return new Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>>(ipv4Upstreams, ipv6Upstreams);
        }

        public IpDetailModel GetIpDetails(string ipAddress)
        {
            var driver = GetDriverWithValidatedResponseFrom($"https://bgp.he.net/ip/{ipAddress}");
            var ipInfoElement = driver.FindElement(By.Id("ipinfo"));

            var ipDetails = new IpDetailModel { IPAddress = ipAddress };

            using(var reader = new StringReader(ipInfoElement.Text))
            {
                // reads the first line, which contains ip address and, sometimes, dns record between ()
                // Ex 8.8.8.8 (dns.google)
                var ipAndPtrRecord = reader.ReadLine();
                var hasPtrRecord = ipAndPtrRecord.Contains("(");
                ipDetails.PtrRecord = hasPtrRecord ? Regex.Match(ipAndPtrRecord, @"\(([^\)]+)\)").Value.Trim('(', ')') : null;
            }
            
            ipDetails.RelatedPrefixes = ExtractAsDataFromIpInfoElement(ipInfoElement)
                .GroupBy(x => x[1]) // Groups asns by prefixes
                .Select(px => new PrefixDetailModel {
                    Prefix = px.Key,
                    Name = px.ElementAt(0)[2],
                    Description = px.ElementAt(0)[2],
                    ParentAsns = px.Select(array => new AsnModel {
                        ASN = int.Parse(array[0]),
                        Name = array[2],
                        Description = array[2],
                        CountryCode = null
                    })
                });

            // Allocation prefix is always the first on the list, which is the major prefix
            ipDetails.RIRAllocationPrefix = ipDetails.RelatedPrefixes.ElementAt(0).Prefix;
            ipDetails.CountryCode = ExtractCountryCodeFromDivWhois(ExtractDivWhoisFrom(driver.PageSource));
            
            return ipDetails;
        }

        public PrefixDetailModel GetPrefixDetails(string prefix, byte cidr)
        {
            var driver = GetDriverWithValidatedResponseFrom($"https://bgp.he.net/net/{prefix}/{cidr}");
            
            var asnDetailsExtractedFromNetinfoDiv = ExtractPrefixAndAsnInfoFromNetInfoDivTable(driver)
                .GroupBy(detailsArray => detailsArray[0]) // Group to remove duplicated ASN entries
                .Select(grouped => new AsnModel {
                    ASN = int.Parse(grouped.ElementAt(0)[0]),
                    Name = grouped.ElementAt(0)[2],
                    Description = grouped.ElementAt(0)[2],
                    CountryCode = null
                }).ToArray();

            // Set prefix owner country code, that is,
            // first parent ASN
            var ownerPrefix = asnDetailsExtractedFromNetinfoDiv[0];
            ownerPrefix.CountryCode = ExtractCountryCodeFromDivWhois(ExtractDivWhoisFrom(driver.PageSource));

            return new PrefixDetailModel {
                Prefix = $"{prefix}/{cidr}",
                Name = ownerPrefix.Name,
                Description = ownerPrefix.Description,
                ParentAsns = asnDetailsExtractedFromNetinfoDiv
            };
        }

        public SearchModel SearchBy(string queryTerm)
        {
            if(int.TryParse(queryTerm, out int asNumber))
            {
                var driver = GetDriverWithValidatedResponseFrom($"https://bgp.he.net/AS{asNumber}");
                var prefixes = ExtractAsPrefixesFromDriver(driver);
                var asDetails = ExtractAsDetailsFromDriver(driver, asNumber);
                return new SearchModel
                {
                    RelatedAsns = new AsnDetailsModel[] { asDetails },
                    IPv4 = prefixes.Item1,
                    IPv6 = prefixes.Item2
                };
            }
            else if(Regex.IsMatch(queryTerm, IPV4_PREFIX_PATTERN))
            {
                var prefix = queryTerm.Split('/')[0];
                var cidr = queryTerm.Split('/')[1];
                var driver = GetDriverWithValidatedResponseFrom($"https://bgp.he.net/net/{prefix}/{cidr}");
                var data = ExtractPrefixAndAsnInfoFromNetInfoDivTable(driver);

                return new SearchModel
                {
                    RelatedAsns = data.Select(x => new AsnWithContactsModel {
                        ASN = int.Parse(x.ElementAt(0)),
                        Name = x.ElementAt(2),
                        Description = x.ElementAt(2),
                        EmailContacts = Enumerable.Empty<string>(),
                        AbuseContacts = Enumerable.Empty<string>()
                    }),
                    IPv4 = data.Select(x => new PrefixModel {
                        Prefix = x.ElementAt(1),
                        Name = x.ElementAt(2),
                        Description = x.ElementAt(2)
                    }),
                    IPv6 = Enumerable.Empty<PrefixModel>()
                };
            }
            else if(Regex.IsMatch(queryTerm, IPV6_PREFIX_PATTERN))
            {
                var prefix = queryTerm.Split('/')[0];
                var cidr = queryTerm.Split('/')[1];
                var driver = GetDriverWithValidatedResponseFrom($"https://bgp.he.net/net/{prefix}/{cidr}");
                var data = ExtractPrefixAndAsnInfoFromNetInfoDivTable(driver);

                return new SearchModel
                {
                    RelatedAsns = data.Select(x => new AsnWithContactsModel {
                        ASN = int.Parse(x.ElementAt(0)),
                        Name = x.ElementAt(2),
                        Description = x.ElementAt(2),
                        EmailContacts = Enumerable.Empty<string>(),
                        AbuseContacts = Enumerable.Empty<string>()
                    }),
                    IPv4 = Enumerable.Empty<PrefixModel>(),
                    IPv6 = data.Select(x => new PrefixModel {
                        Prefix = x.ElementAt(1),
                        Name = x.ElementAt(2),
                        Description = x.ElementAt(2)                      
                    })
                };
            }
            else if(Regex.IsMatch(queryTerm, IPV4_ADDRESS_PATTERN))
            {
                var driver = GetDriverWithValidatedResponseFrom($"https://bgp.he.net/ip/{queryTerm}");
                var data = ExtractAsDataFromIpInfoElement(driver.FindElement(By.Id("ipinfo")));
                return new SearchModel
                {
                    RelatedAsns = data.Select(x => new AsnWithContactsModel {
                        ASN = int.Parse(x.ElementAt(0)),
                        Name = x.ElementAt(2),
                        Description = x.ElementAt(2),
                        EmailContacts = Enumerable.Empty<string>(),
                        AbuseContacts = Enumerable.Empty<string>()
                    }),
                    IPv4 = data.Select(x => new PrefixModel {
                        Prefix = x.ElementAt(1),
                        Name = x.ElementAt(2),
                        Description = x.ElementAt(2)
                    }),
                    IPv6 = Enumerable.Empty<PrefixModel>()
                };
            }
            else if(Regex.IsMatch(queryTerm, IPV6_ADDRESS_PATTERN))
            {
                var driver = GetDriverWithValidatedResponseFrom($"https://bgp.he.net/ip/{queryTerm}");
                var data = ExtractAsDataFromIpInfoElement(driver.FindElement(By.Id("ipinfo")));
                return new SearchModel
                {
                    RelatedAsns = data.Select(x => new AsnWithContactsModel {
                        ASN = int.Parse(x.ElementAt(0)),
                        Name = x.ElementAt(2),
                        Description = x.ElementAt(2),
                        EmailContacts = Enumerable.Empty<string>(),
                        AbuseContacts = Enumerable.Empty<string>()
                    }),
                    IPv4 = Enumerable.Empty<PrefixModel>(),
                    IPv6 = data.Select(x => new PrefixModel {
                        Prefix = x.ElementAt(1),
                        Name = x.ElementAt(2),
                        Description = x.ElementAt(2)                      
                    })
                };
            }
            else
            {
                var driver = GetDriverWithValidatedResponseFrom($"https://bgp.he.net/search?search%5Bsearch%5D={queryTerm.Replace(' ', '+')}&commit=Search");
                var relatedAsns = new List<AsnWithContactsModel>();
                var ipv4Prefixes = new List<PrefixModel>();
                var ipv6Prefixes = new List<PrefixModel>();


                var trs = driver.FindElement(By.TagName("tbody"))
                    .FindElements(By.TagName("tr"))
                    .Skip(1) // The first row is irrelevant, so skip it
                    .Select(tr => tr.FindElements(By.TagName("td")));
                
                foreach(var tds in trs)
                {
                    var firstTdAnchorHref = tds.ElementAt(0).FindElement(By.TagName("a")).GetAttribute("href");
                    var name = tds.ElementAt(1).Text;
                    
                    if(Regex.IsMatch(firstTdAnchorHref, ASN_PATTERN))
                    {
                        relatedAsns.Add(new AsnWithContactsModel {   
                            ASN = int.Parse(Regex.Match(firstTdAnchorHref, ASN_PATTERN).Value.Substring(2)),
                            Name = name,
                            Description = name,
                            CountryCode = null,
                            AbuseContacts = Enumerable.Empty<string>(),
                            EmailContacts = Enumerable.Empty<string>()
                        });
                    }
                    else if(Regex.IsMatch(firstTdAnchorHref, IPV6_PREFIX_PATTERN))
                    {
                        ipv6Prefixes.Add(new PrefixModel {
                            Prefix = Regex.Match(firstTdAnchorHref, IPV6_PREFIX_PATTERN).Value,
                            Name = name,
                            Description = name
                        });
                    }
                    else 
                    {
                        ipv4Prefixes.Add(new PrefixModel {
                            Prefix = Regex.Match(firstTdAnchorHref, IPV4_PREFIX_PATTERN).Value,
                            Name = name,
                            Description = name
                        });
                    }
                }

                return new SearchModel
                {
                    RelatedAsns = relatedAsns,
                    IPv4 = ipv4Prefixes,
                    IPv6 = ipv6Prefixes
                };
            }
        }
    }
}