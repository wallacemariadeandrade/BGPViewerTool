using System.Text.Json;
using System.Threading.Tasks;
using BGPViewerCore.Service;

namespace BGPViewerCore.UnitTests.ArinServiceTests
{
    public class ArinMockWebApi : ArinWebApi
    {
        public override JsonDocument RetrieveAsnDetails(int asNumber)
        {
            if(asNumber == 53181)
                return JsonDocument.Parse(@"{""asn"":{""@xmlns"":{""ns3"":""http:\/\/www.arin.net\/whoisrws\/netref\/v2"",""ns2"":""http:\/\/www.arin.net\/whoisrws\/rdns\/v1"",""$"":""http:\/\/www.arin.net\/whoisrws\/core\/v1""},""@copyrightNotice"":""Copyright 1997-2021, American Registry for Internet Numbers, Ltd."",""@inaccuracyReportUrl"":""https:\/\/www.arin.net\/resources\/registry\/whois\/inaccuracy_reporting\/"",""@termsOfUse"":""https:\/\/www.arin.net\/resources\/registry\/whois\/tou\/"",""registrationDate"":{""$"":""2009-03-11T00:00:00-04:00""},""rdapRef"":{""$"":""https:\/\/rdap.arin.net\/registry\/autnum\/52224""},""ref"":{""$"":""https:\/\/whois.arin.net\/rest\/asn\/AS52224""},""endAsNumber"":{""$"":""53247""},""handle"":{""$"":""AS52224""},""name"":{""$"":""LACNIC-52224""},""resources"":{""@copyrightNotice"":""Copyright 1997-2021, American Registry for Internet Numbers, Ltd."",""@inaccuracyReportUrl"":""https:\/\/www.arin.net\/resources\/registry\/whois\/inaccuracy_reporting\/"",""@termsOfUse"":""https:\/\/www.arin.net\/resources\/registry\/whois\/tou\/"",""limitExceeded"":{""@limit"":""256"",""$"":""false""},""resource"":[{""@description"":""Web Page"",""@media_type"":""text\/html"",""@rel"":""SELF"",""@url"":""http:\/\/lacnic.net\/cgi-bin\/lacnic\/whois""},{""@description"":""Whois"",""@media_type"":""application\/x-whois"",""@rel"":""SELF"",""@url"":""whois.lacnic.net""}]},""orgRef"":{""@handle"":""LACNIC"",""@name"":""Latin American and Caribbean IP address Regional Registry"",""$"":""https:\/\/whois.arin.net\/rest\/org\/LACNIC""},""comment"":{""line"":[{""@number"":""0"",""$"":""This AS is under LACNIC responsibility for further allocations to""},{""@number"":""1"",""$"":""users in LACNIC region.""},{""@number"":""2"",""$"":""Please see http:\/\/www.lacnic.net\/ for further details, or check the""},{""@number"":""3"",""$"":""WHOIS server located at http:\/\/whois.lacnic.net""}]},""startAsNumber"":{""$"":""52224""},""updateDate"":{""$"":""2009-03-17T15:29:07-04:00""}}}");
            if(asNumber == 3356)
                return JsonDocument.Parse(@"{""asn"":{""@xmlns"":{""ns3"":""http:\/\/www.arin.net\/whoisrws\/netref\/v2"",""ns2"":""http:\/\/www.arin.net\/whoisrws\/rdns\/v1"",""$"":""http:\/\/www.arin.net\/whoisrws\/core\/v1""},""@copyrightNotice"":""Copyright 1997-2021, American Registry for Internet Numbers, Ltd."",""@inaccuracyReportUrl"":""https:\/\/www.arin.net\/resources\/registry\/whois\/inaccuracy_reporting\/"",""@termsOfUse"":""https:\/\/www.arin.net\/resources\/registry\/whois\/tou\/"",""registrationDate"":{""$"":""2000-03-10T00:00:00-05:00""},""rdapRef"":{""$"":""https:\/\/rdap.arin.net\/registry\/autnum\/3356""},""ref"":{""$"":""https:\/\/whois.arin.net\/rest\/asn\/AS3356""},""endAsNumber"":{""$"":""3356""},""handle"":{""$"":""AS3356""},""name"":{""$"":""LEVEL3""},""resources"":{""@copyrightNotice"":""Copyright 1997-2021, American Registry for Internet Numbers, Ltd."",""@inaccuracyReportUrl"":""https:\/\/www.arin.net\/resources\/registry\/whois\/inaccuracy_reporting\/"",""@termsOfUse"":""https:\/\/www.arin.net\/resources\/registry\/whois\/tou\/"",""limitExceeded"":{""@limit"":""256"",""$"":""false""}},""orgRef"":{""@handle"":""LPL-141"",""@name"":""Level 3 Parent, LLC"",""$"":""https:\/\/whois.arin.net\/rest\/org\/LPL-141""},""startAsNumber"":{""$"":""3356""},""updateDate"":{""$"":""2018-02-20T12:38:56-05:00""}}}");
 
            return  DefaultReturn();
        }

        public override Task<JsonDocument> RetrieveAsnDetailsAsync(int asNumber)
            => Task.FromResult(RetrieveAsnDetails(asNumber));
    }
}