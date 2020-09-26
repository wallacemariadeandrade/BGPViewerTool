using System.Net;
using System.IO;

namespace BGPViewerCore.Service
{
    public static class WebService
    {
        public static string GetContentFrom(string url)
            => new StreamReader(WebRequest.Create(url).GetResponse().GetResponseStream())
                .ReadToEnd();
    }
}