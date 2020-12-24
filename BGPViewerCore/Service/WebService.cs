using System.Net;
using System.IO;

namespace BGPViewerCore.Service
{
    public static class WebService
    {
        public static string GetContentFrom(string url)
        {
            using(var stream = new StreamReader(WebRequest.Create(url).GetResponse().GetResponseStream()))
            {
                return stream.ReadToEnd();   
            }
        }
    }
}