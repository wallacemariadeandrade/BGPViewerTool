using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BGPViewerCore.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.IO;
using static BGPViewerCore.Service.RegexPatterns;
using System.Threading.Tasks;

namespace BGPViewerCore.Service
{
    public class BGPHeService : IBGPViewerService
    {
        protected class PrefixWithAsNumber : PrefixModel
        {
            public string AsNumberStr { get; set; }
        }

        private const string BASE_ENDPOINT_URL = "https://bgp.he.net";
        private const string EMAIL_PATTERN = @"([a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*)";
        private readonly IWebDriver _driver;
        private readonly TimeSpan _timeout;
        
        public BGPHeService(IWebDriver driver, int timeout)
        {
            _timeout = TimeSpan.FromSeconds(timeout);
            _driver = driver;
        }

        #region Routines

        private string BuildAsnDetailsEndpoint(int asNumber) => $"{BASE_ENDPOINT_URL}/AS{asNumber}";
        private string BuildPrefixDetailsEndpoint(string prefix, byte cidr) => $"{BASE_ENDPOINT_URL}/net/{prefix}/{cidr}";

        private IWebDriver GetDriverWithValidatedResponseFrom(string url)
        {
            var wb = new WebDriverWait(_driver, _timeout);
            _driver.Navigate().GoToUrl(url);
            wb.Until(x => x.Url == url);
            if(!CheckIfDataExists(_driver))
                throw new KeyNotFoundException("Your query did not return any results.");
            return _driver;
        }

        private bool CheckIfDataExists(IWebDriver driver) => driver.FindElements(By.Id("error")).Count <= 0;

        private IEnumerable<string> ExtractEmailsFrom(string content)
            => Regex.Matches(content, EMAIL_PATTERN)
            .Cast<Match>()
            .Select(match => match.Value);
                
        private string ExtractDivWhoisFrom(string content)
        {
            var whoisDivStartIndex = content.IndexOf(@"<div id=""whois"" class=""tabdata hidden"">");
            if(whoisDivStartIndex < 0) throw new System.ArgumentException($"{content} doesn't contains div whois.");
            var whoisDivlength = content.IndexOf("</div>", whoisDivStartIndex) - whoisDivStartIndex;
            return  content.Substring(whoisDivStartIndex, whoisDivlength);
        }

        private IEnumerable<AsnModel> ExtractAsnsAndNamesFromToppertable(string toppeertableText)
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
                    yield return new AsnModel {
                        ASN = int.Parse(asn),
                        Name = name,
                        Description = name,
                        CountryCode = null
                    };
                }
            }
        }

        private async Task<IEnumerable<AsnModel>> ExtractAsnsAndNamesFromToppertableAsync(string toppeertableText)
        {            
            using(var reader = new StringReader(toppeertableText.Split(new string[]{"ASN Name"}, StringSplitOptions.None)[1].TrimStart()))
            {
                var separator = new string[]{" "};
                var set = new HashSet<AsnModel>();
                while(reader.Peek() > 0)
                {
                    var line = await reader.ReadLineAsync();
                    var asnAndName = line.Split(separator, StringSplitOptions.None);
                    var asn = asnAndName[0].Substring(2);
                    var name = line.Replace($"AS{asn}", "").TrimStart();
                    set.Add(new AsnModel {
                        ASN = int.Parse(asn),
                        Name = name,
                        Description = name,
                        CountryCode = null
                    });
                }
                return set;
            }
        }

        private IEnumerable<PrefixModel> ExtractPrefixesFromTablePrefixes(IWebElement tablePrefixes)
            => tablePrefixes.FindElement(By.TagName("tbody"))
                            .FindElements(By.TagName("tr"))
                            .Select(tr => tr.FindElements(By.TagName("td")))
                            .Select(tds => new PrefixModel {
                                Prefix = tds.ElementAt(0).FindElement(By.TagName("a")).GetAttribute("innerHTML"),
                                Name = tds.ElementAt(1).Text,
                                Description = tds.ElementAt(1).Text
                            });

        // For better performance while querying only prefixes
        private IEnumerable<string> ExtractPrefixesFromTablePrefixes(IWebElement tablePrefixes, string pattern)
            => Regex.Matches(tablePrefixes.GetAttribute("innerHTML"), pattern)
                    .Cast<Match>()
                    .Select(x => x.Value)
                    .Distinct();

        private IEnumerable<AsnModel> ExtractAsnsInfoFromPeerTable(IWebElement peerTable)
        {
            var tdList = peerTable.FindElement(By.TagName("tbody"))
                .FindElements(By.TagName("tr"))
                .Select(tr => tr.FindElements(By.TagName("td")));
            foreach (var td in tdList)
            {
                var asn = td.ElementAt(3).FindElement(By.TagName("a")).GetAttribute("innerHTML").Substring(2);
                var tdWithNameAndCountry = td.ElementAt(1);
                var innerHtmlOfTdWithNameAndCountry = tdWithNameAndCountry.GetAttribute("innerHTML");
                var name = innerHtmlOfTdWithNameAndCountry.Substring(0, innerHtmlOfTdWithNameAndCountry.IndexOf("<div")); // Gets only the portion with asn name
                var description = name;
                yield return new AsnModel {
                    ASN = int.Parse(asn),
                    Name = name,
                    Description = name,
                    CountryCode = null
                };
            }
        }

        private IEnumerable<PrefixWithAsNumber> ExtractPrefixAndAsnInfoFromNetInfoDivTable(IWebDriver driver) 
        {
            var tables = driver.FindElement(By.Id("netinfo"))
                .FindElements(By.TagName("table"));

            int correctNumberOfTableRows = 3; // Each table has to match 3 rows (asn, prefix and name)

            foreach (var table in tables)
            {
                var matches = Regex.Matches(table.GetAttribute("innerHTML"), @"<a.*>.*<\/a>|<td>.*<\/td>");
                var isNumberOfRowsCorrect = matches.Count % correctNumberOfTableRows == 0; 
                if(!isNumberOfRowsCorrect) throw new ArgumentException($"Incorrect input HTML. It was expected 3 matches and got {matches.Count}.", "tableInnerHtml");

                for (int i = 0; i < matches.Count; i+=3)
                {
                    var asnStr = Regex.Match(matches[i].Value, ASN_PATTERN).Value.Substring(2);

                    var prefix = Regex.IsMatch(matches[i+1].Value, IPV4_PREFIX_PATTERN) ? 
                        Regex.Match(matches[i+1].Value, IPV4_PREFIX_PATTERN).Value : Regex.Match(matches[i+1].Value, IPV6_PREFIX_PATTERN).Value;

                    var nameOrDescription = matches[i+2].Value.Replace("<td>", "").Replace("</td>", "");

                    yield return new PrefixWithAsNumber {
                        AsNumberStr = asnStr,
                        Name = nameOrDescription,
                        Description = nameOrDescription,
                        Prefix = prefix
                    };
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

        private async Task<string> ExtractCountryCodeFromDivWhoisAsync(string divWhois)
        {
            using(var reader = new StringReader(divWhois))
            {
                while(reader.Peek() > 0)
                {
                    var line = await reader.ReadLineAsync();
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

        private async Task<AsnDetailsModel> ExtractAsDetailsFromDriverAsync(IWebDriver driver, int asNumber)
        {
            var h1TitleElement = driver.FindElement(By.TagName("h1"));
            var asn = h1TitleElement.Text.Split(new string[]{" "}, StringSplitOptions.None)[0].Substring(2);
            var name = h1TitleElement.Text.Replace($"AS{asn} ", "");
            var description = name;

            // divs with classes "asleft" and "asright" are always present,
            // so we can use them to get country code and looking glass url
            var leftDivs = driver.FindElements(By.ClassName("asleft"));
            var rightDivs = driver.FindElements(By.ClassName("asright"));

            var divWhois = ExtractDivWhoisFrom(driver.PageSource);
            var countryCode = await ExtractCountryCodeFromDivWhoisAsync(divWhois);
            
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

        private IEnumerable<PrefixWithAsNumber> ExtractAsDataFromIpInfoElement(IWebElement ipInfoElement)
            => ipInfoElement.FindElement(By.TagName("table"))
                .FindElement(By.TagName("tbody"))
                .FindElements(By.TagName("tr"))
                .Select(tr => tr.FindElements(By.TagName("td")))
                .Select(tds => new PrefixWithAsNumber {
                    AsNumberStr = tds.ElementAt(0).FindElement(By.TagName("a")).GetAttribute("innerHTML").Substring(2),
                    Prefix = tds.ElementAt(1).FindElement(By.TagName("a")).GetAttribute("innerHTML"),
                    Name = tds.ElementAt(2).GetAttribute("innerHTML"),
                    Description = tds.ElementAt(2).GetAttribute("innerHTML")
                });

        #endregion

        public AsnDetailsModel GetAsnDetails(int asNumber)
        {
            var driver = GetDriverWithValidatedResponseFrom($"https://bgp.he.net/AS{asNumber}");
            return ExtractAsDetailsFromDriver(driver, asNumber);
        }

        // Not supported by bgp.he.net
        public Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>> GetAsnDownstreams(int asNumber)
        {
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
                    for(int index=0; index < ixIpv4Addresses.Count(); index++)
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
                : new AsnModel[]{};

            var ipv6Peers = hasIPv6Peers ? ExtractAsnsInfoFromPeerTable(driver.FindElement(By.Id("peers6")))
                : new AsnModel[]{};
            
            return new Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>>(ipv4Peers, ipv6Peers);
        }

        public AsnPrefixesModel GetAsnPrefixes(int asNumber)
        {
            var driver = GetDriverWithValidatedResponseFrom($"https://bgp.he.net/AS{asNumber}");

            var hasIpv4Prefixes = driver.PageSource.Contains("table_prefixes4");
            var hasIpv6Prefixes = driver.PageSource.Contains("table_prefixes6");

            var ipv4Prefixes = hasIpv4Prefixes ? ExtractPrefixesFromTablePrefixes(driver.FindElement(By.Id("table_prefixes4")), IPV4_PREFIX_PATTERN) : Enumerable.Empty<string>();
            var ipv6Prefixes = hasIpv6Prefixes ? ExtractPrefixesFromTablePrefixes(driver.FindElement(By.Id("table_prefixes6")), IPV6_PREFIX_PATTERN) : Enumerable.Empty<string>();

            return new AsnPrefixesModel
            {
                ASN = asNumber,
                IPv4 = ipv4Prefixes,
                IPv6 = ipv6Prefixes
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
                    : new AsnModel[]{};

            var ipv6Upstreams = hasIpv6Upstreams ? ExtractAsnsAndNamesFromToppertable(toppeertables.ElementAt(1))
                    : new AsnModel[]{};

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
                .GroupBy(data => data.Prefix)
                .Select(px => new PrefixDetailModel {
                    Prefix = px.Key,
                    Name = px.ElementAt(0).Name,
                    Description = px.ElementAt(0).Description,
                    ParentAsns = px.Select(x => new AsnModel {
                        ASN = int.Parse(x.AsNumberStr),
                        Name = x.Name,
                        Description = x.Description,
                        CountryCode = null
                    })});

            ipDetails.RIRAllocationPrefix = ipDetails.RelatedPrefixes.ElementAt(0).Prefix;
            ipDetails.CountryCode = ExtractCountryCodeFromDivWhois(ExtractDivWhoisFrom(driver.PageSource));
            
            return ipDetails;
        }

        public PrefixDetailModel GetPrefixDetails(string prefix, byte cidr)
        {
            var driver = GetDriverWithValidatedResponseFrom($"https://bgp.he.net/net/{prefix}/{cidr}");
            
            var asnDetailsExtractedFromNetinfoDiv = ExtractPrefixAndAsnInfoFromNetInfoDivTable(driver)
                .GroupBy(data => data.AsNumberStr)
                .Select(grouped => new AsnModel {
                    ASN = int.Parse(grouped.ElementAt(0).AsNumberStr),
                    Name = grouped.ElementAt(0).Name,
                    Description = grouped.ElementAt(0).Description,
                    CountryCode = null
                }).ToArray();

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
                
                var hasIpv4Prefixes = driver.PageSource.Contains("table_prefixes4");
                var hasIpv6Prefixes = driver.PageSource.Contains("table_prefixes6");

                var ipv4Prefixes = hasIpv4Prefixes ? ExtractPrefixesFromTablePrefixes(driver.FindElement(By.Id("table_prefixes4"))) : Enumerable.Empty<PrefixModel>();
                var ipv6Prefixes = hasIpv6Prefixes ? ExtractPrefixesFromTablePrefixes(driver.FindElement(By.Id("table_prefixes6"))) : Enumerable.Empty<PrefixModel>();
                
                var asDetails = ExtractAsDetailsFromDriver(driver, asNumber);
                return new SearchModel
                {
                    RelatedAsns = new AsnDetailsModel[] { asDetails },
                    IPv4 = ipv4Prefixes,
                    IPv6 = ipv6Prefixes
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
                        ASN = int.Parse(x.AsNumberStr),
                        Name = x.Name,
                        Description = x.Description,
                        EmailContacts = Enumerable.Empty<string>(),
                        AbuseContacts = Enumerable.Empty<string>()
                    }),
                    IPv4 = data.Select(x => new PrefixModel {
                        Prefix = x.Prefix,
                        Name = x.Name,
                        Description = x.Description
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
                        ASN = int.Parse(x.AsNumberStr),
                        Name = x.Name,
                        Description = x.Description,
                        EmailContacts = Enumerable.Empty<string>(),
                        AbuseContacts = Enumerable.Empty<string>()
                    }),
                    IPv4 = Enumerable.Empty<PrefixModel>(),
                    IPv6 = data.Select(x => new PrefixModel {
                        Prefix = x.Prefix,
                        Name = x.Name,
                        Description = x.Description
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
                        ASN = int.Parse(x.AsNumberStr),
                        Name = x.Name,
                        Description = x.Description,
                        EmailContacts = Enumerable.Empty<string>(),
                        AbuseContacts = Enumerable.Empty<string>()
                    }),
                    IPv4 = data.Select(x => new PrefixModel {
                        Prefix = x.Prefix,
                        Name = x.Name,
                        Description = x.Description
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
                        ASN = int.Parse(x.AsNumberStr),
                        Name = x.Name,
                        Description = x.Description,
                        EmailContacts = Enumerable.Empty<string>(),
                        AbuseContacts = Enumerable.Empty<string>()
                    }),
                    IPv4 = Enumerable.Empty<PrefixModel>(),
                    IPv6 = data.Select(x => new PrefixModel {
                        Prefix = x.Prefix,
                        Name = x.Name,
                        Description = x.Description
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

        public void Dispose() => _driver.Dispose();

        public Task<AsnDetailsModel> GetAsnDetailsAsync(int asNumber)
        {
            var driver = GetDriverWithValidatedResponseFrom(BuildAsnDetailsEndpoint(asNumber));
            return ExtractAsDetailsFromDriverAsync(driver, asNumber);
        }

        public Task<Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>>> GetAsnDownstreamsAsync(int asNumber)
            => Task.FromResult(new Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>>(
                new AsnDetailsModel[]{}, 
                new AsnDetailsModel[]{}
            ));    
        

        public Task<IEnumerable<IxModel>> GetAsnIxsAsync(int asNumber) => Task.FromResult(GetAsnIxs(asNumber));

        public Task<Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>>> GetAsnPeersAsync(int asNumber)
            => Task.FromResult(GetAsnPeers(asNumber));
        public Task<AsnPrefixesModel> GetAsnPrefixesAsync(int asNumber) => Task.FromResult(GetAsnPrefixes(asNumber));

        public async Task<Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>>> GetAsnUpstreamsAsync(int asNumber)
        {
            var toppeertables = GetDriverWithValidatedResponseFrom(BuildAsnDetailsEndpoint(asNumber))
            .FindElements(By.ClassName("toppeertable"))
            .Select(table => table.Text);   
            
            var hasIpv4Upstreams = toppeertables.Count() >= 1;
            var hasIpv6Upstreams = toppeertables.Count() >= 2;

            var ipv4Upstreams = hasIpv4Upstreams ? await ExtractAsnsAndNamesFromToppertableAsync(toppeertables.ElementAt(0))
                    : new AsnModel[]{};

            var ipv6Upstreams = hasIpv6Upstreams ? await ExtractAsnsAndNamesFromToppertableAsync(toppeertables.ElementAt(1))
                    : new AsnModel[]{};

            return new Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>>(ipv4Upstreams, ipv6Upstreams);
        }

        public async Task<IpDetailModel> GetIpDetailsAsync(string ipAddress)
        {
            var driver = GetDriverWithValidatedResponseFrom($"https://bgp.he.net/ip/{ipAddress}");
            var ipInfoElement = driver.FindElement(By.Id("ipinfo"));

            var ipDetails = new IpDetailModel { IPAddress = ipAddress };

            using(var reader = new StringReader(ipInfoElement.Text))
            {
                // reads the first line, which contains ip address and, sometimes, dns record between ()
                // Ex 8.8.8.8 (dns.google)
                var ipAndPtrRecord = await reader.ReadLineAsync();
                var hasPtrRecord = ipAndPtrRecord.Contains("(");
                ipDetails.PtrRecord = hasPtrRecord ? Regex.Match(ipAndPtrRecord, @"\(([^\)]+)\)").Value.Trim('(', ')') : null;
            }
            
            ipDetails.RelatedPrefixes = ExtractAsDataFromIpInfoElement(ipInfoElement)
                .GroupBy(data => data.Prefix)
                .Select(px => new PrefixDetailModel {
                    Prefix = px.Key,
                    Name = px.ElementAt(0).Name,
                    Description = px.ElementAt(0).Description,
                    ParentAsns = px.Select(x => new AsnModel {
                        ASN = int.Parse(x.AsNumberStr),
                        Name = x.Name,
                        Description = x.Description,
                        CountryCode = null
                    })});

            ipDetails.RIRAllocationPrefix = ipDetails.RelatedPrefixes.ElementAt(0).Prefix;
            ipDetails.CountryCode = await ExtractCountryCodeFromDivWhoisAsync(ExtractDivWhoisFrom(driver.PageSource));
            
            return ipDetails;
        }

        public async Task<PrefixDetailModel> GetPrefixDetailsAsync(string prefix, byte cidr)
        {
            var driver = GetDriverWithValidatedResponseFrom(BuildPrefixDetailsEndpoint(prefix, cidr));
            
            var asnDetailsExtractedFromNetinfoDiv = ExtractPrefixAndAsnInfoFromNetInfoDivTable(driver)
                .GroupBy(data => data.AsNumberStr)
                .Select(grouped => new AsnModel {
                    ASN = int.Parse(grouped.ElementAt(0).AsNumberStr),
                    Name = grouped.ElementAt(0).Name,
                    Description = grouped.ElementAt(0).Description,
                    CountryCode = null
                }).ToArray();

            var ownerPrefix = asnDetailsExtractedFromNetinfoDiv[0];
            ownerPrefix.CountryCode = await ExtractCountryCodeFromDivWhoisAsync(ExtractDivWhoisFrom(driver.PageSource));

            return new PrefixDetailModel {
                Prefix = $"{prefix}/{cidr}",
                Name = ownerPrefix.Name,
                Description = ownerPrefix.Description,
                ParentAsns = asnDetailsExtractedFromNetinfoDiv
            };
        }

        public async Task<SearchModel> SearchByAsync(string queryTerm)
        {
            if(int.TryParse(queryTerm, out int asNumber))
            {
                var driver = GetDriverWithValidatedResponseFrom($"https://bgp.he.net/AS{asNumber}");
                
                var hasIpv4Prefixes = driver.PageSource.Contains("table_prefixes4");
                var hasIpv6Prefixes = driver.PageSource.Contains("table_prefixes6");

                var ipv4Prefixes = hasIpv4Prefixes ? ExtractPrefixesFromTablePrefixes(driver.FindElement(By.Id("table_prefixes4"))) : Enumerable.Empty<PrefixModel>();
                var ipv6Prefixes = hasIpv6Prefixes ? ExtractPrefixesFromTablePrefixes(driver.FindElement(By.Id("table_prefixes6"))) : Enumerable.Empty<PrefixModel>();
                
                var asDetails = await ExtractAsDetailsFromDriverAsync(driver, asNumber);
                return new SearchModel
                {
                    RelatedAsns = new AsnDetailsModel[] { asDetails },
                    IPv4 = ipv4Prefixes,
                    IPv6 = ipv6Prefixes
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
                        ASN = int.Parse(x.AsNumberStr),
                        Name = x.Name,
                        Description = x.Description,
                        EmailContacts = Enumerable.Empty<string>(),
                        AbuseContacts = Enumerable.Empty<string>()
                    }),
                    IPv4 = data.Select(x => new PrefixModel {
                        Prefix = x.Prefix,
                        Name = x.Name,
                        Description = x.Description
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
                        ASN = int.Parse(x.AsNumberStr),
                        Name = x.Name,
                        Description = x.Description,
                        EmailContacts = Enumerable.Empty<string>(),
                        AbuseContacts = Enumerable.Empty<string>()
                    }),
                    IPv4 = Enumerable.Empty<PrefixModel>(),
                    IPv6 = data.Select(x => new PrefixModel {
                        Prefix = x.Prefix,
                        Name = x.Name,
                        Description = x.Description
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
                        ASN = int.Parse(x.AsNumberStr),
                        Name = x.Name,
                        Description = x.Description,
                        EmailContacts = Enumerable.Empty<string>(),
                        AbuseContacts = Enumerable.Empty<string>()
                    }),
                    IPv4 = data.Select(x => new PrefixModel {
                        Prefix = x.Prefix,
                        Name = x.Name,
                        Description = x.Description
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
                        ASN = int.Parse(x.AsNumberStr),
                        Name = x.Name,
                        Description = x.Description,
                        EmailContacts = Enumerable.Empty<string>(),
                        AbuseContacts = Enumerable.Empty<string>()
                    }),
                    IPv4 = Enumerable.Empty<PrefixModel>(),
                    IPv6 = data.Select(x => new PrefixModel {
                        Prefix = x.Prefix,
                        Name = x.Name,
                        Description = x.Description
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