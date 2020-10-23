using System.Text;
using BGPViewerCore.Model;
using System.Linq;
using System.Collections.Generic;

namespace BGPViewerConsoleTool
{
    public static class Printer
    {
        public static string PrintAsnModel(AsnModel model)
            => PrintAsnModelWithPadding(model, 0);

        private static string PrintAsnModelWithPadding(AsnModel model, int padding)
            => new StringBuilder()
                .AppendLine()
                .AppendLine($"{new string(' ', padding)}ASN: {model.ASN}")
                .AppendLine($"{new string(' ', padding)}Name: {model.Name}")
                .AppendLine($"{new string(' ', padding)}Description: {model.Description}")
                .AppendLine($"{new string(' ', padding)}Country Code: {model.CountryCode}")
                .ToString();

        public static string PrintAsnWithContacts(AsnWithContactsModel model)
            => PrintAsnModelWithContactsAndPadding(model, 0);

        private static string PrintAsnModelWithContactsAndPadding(AsnWithContactsModel model, int padding)
            => new StringBuilder()
                .AppendLine(PrintAsnModelWithPadding(model, padding))
                .AppendLine($"{new string(' ', padding)}Email Contacts: {string.Join(", ", model.EmailContacts)}")
                .AppendLine($"{new string(' ', padding)}Abuse Contacts: {string.Join(", ", model.AbuseContacts)}")
                .ToString();

        public static string PrintAsnDetails(AsnDetailsModel model)
            => new StringBuilder()
                .AppendLine(PrintAsnWithContacts(model))
                .AppendLine($"Looking Glass: {model.LookingGlassUrl}")
                .ToString();

        public static string PrintAsnsSet(IEnumerable<AsnModel> models)
            => string.Join(System.Environment.NewLine, models.Select(asn => PrintAsnModel(asn)));

        public static string PrintAsnPrefixes(AsnPrefixesModel model)
        {
            var builder = new StringBuilder()
                .AppendLine()
                .AppendLine($"ASN: {model.ASN}")
                .AppendLine($"IPv4:");
            var padding = new string(' ', 2);
            for (int i = 0; i < model.IPv4.Count(); i++) builder.AppendLine($"{padding}{model.IPv4.ElementAt(i)}");
            builder.AppendLine("IPv6:");
            for (int i = 0; i < model.IPv6.Count(); i++) builder.AppendLine($"{padding}{model.IPv6.ElementAt(i)}");
            return builder.ToString();
        }
            
        public static string PrintPrefixModel(PrefixModel model)
            => PrintPrefixModelWithPadding(model, 0);

        private static string PrintPrefixModelWithPadding(PrefixModel model, int padding)
            => new StringBuilder()
                .AppendLine($"{new string(' ', padding)}Prefix: {model.Prefix}")
                .AppendLine($"{new string(' ', padding)}Name: {model.Name}")
                .AppendLine($"{new string(' ', padding)}Description: {model.Description}")
                .ToString();

        public static string PrintPrefixDetails(PrefixDetailModel model)
            => new StringBuilder()
                .AppendLine()
                .AppendLine(PrintPrefixModel(model))
                .AppendLine("Parent asns:")
                .AppendLine($"{string.Join(System.Environment.NewLine, model.ParentAsns.Select(asn => PrintAsnModelWithPadding(asn, 2)))}")
                .ToString();

        public static string PrintIPDetails(IpDetailModel model)
        {
            var builder = new StringBuilder()
                .AppendLine()
                .AppendLine($"IP address: {model.IPAddress}")
                .AppendLine($"Allocation Prefix: {model.RIRAllocationPrefix}")
                .AppendLine($"Country Code: {model.CountryCode}")
                .AppendLine($"PTR Record: {model.PtrRecord}")
                .AppendLine("Related Prefixes: ")
                .AppendLine();
            
            var padding = new string(' ', 2);
            for (int i = 0; i < model.RelatedPrefixes.Count(); i++)
            {
                builder
                .AppendLine($"{padding}Prefix: {model.RelatedPrefixes.ElementAt(i).Prefix}")
                .AppendLine($"{padding}Name: {model.RelatedPrefixes.ElementAt(i).Name}")
                .AppendLine($"{padding}Description: {model.RelatedPrefixes.ElementAt(i).Description}")
                .AppendLine($"{padding}Parent asns:")
                .AppendLine();

                for (int j = 0; j < model.RelatedPrefixes.ElementAt(i).ParentAsns.Count(); j++)
                    builder.AppendLine(PrintAsnModelWithPadding(model.RelatedPrefixes.ElementAt(i).ParentAsns.ElementAt(j), 4));    
            }

            return builder.ToString();
        }
            
        public static string PrintIx(IxModel model)
            => new StringBuilder()
                .AppendLine()
                .AppendLine($"{nameof(model.Name)}: {model.Name}")
                .AppendLine($"{nameof(model.FullName)}: {model.FullName}")
                .AppendLine($"{nameof(model.CountryCode)}: {model.CountryCode}")
                .AppendLine($"{nameof(model.IPv4)}: {model.IPv4}")
                .AppendLine($"{nameof(model.IPv6)}: {model.IPv6}")
                .AppendLine($"{nameof(model.AsnSpeed)}: {model.AsnSpeed}")
                .ToString();

        public static string PrintIxSet(IEnumerable<IxModel> models)
            => string.Join(System.Environment.NewLine, models.Select(ix => PrintIx(ix)));

        public static string PrintSearch(SearchModel model)
            => new StringBuilder()
                .AppendLine()
                .AppendLine($"Related asns:")
                .AppendLine(string.Join(System.Environment.NewLine, model.RelatedAsns.Select(asn => PrintAsnModelWithContactsAndPadding(asn, 2))))
                .AppendLine("IPv4 related prefixes: ")
                .AppendLine(string.Join(System.Environment.NewLine, model.IPv4.Select(prefix => PrintPrefixModelWithPadding(prefix, 2))))
                .AppendLine("IPv6 related prefixes: ")
                .AppendLine(string.Join(System.Environment.NewLine, model.IPv6.Select(prefix => PrintPrefixModelWithPadding(prefix, 2))))
                .ToString();
    }
}