using System.Net;
using System.IO;
using System.Threading.Tasks;

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

        public static async Task<Stream> GetContentStreamFromAsync(string url)
        {
            var response = await WebRequest.Create(url).GetResponseAsync();
            // Do not closes the stream, just returns it!
            return response.GetResponseStream();
        }
    }
}