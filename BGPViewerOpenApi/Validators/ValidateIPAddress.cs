using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using static BGPViewerCore.Service.RegexPatterns;

namespace BGPViewerOpenApi.Validators
{
    public class ValidateIPAddress : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var ipAddress = value as string;

            if(Regex.IsMatch(ipAddress, IPV4_ADDRESS_PATTERN) 
                || Regex.IsMatch(ipAddress, IPV6_ADDRESS_PATTERN))
                return true;

            return false;
        }
    }
}