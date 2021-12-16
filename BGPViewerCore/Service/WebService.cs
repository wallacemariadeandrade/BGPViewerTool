using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace BGPViewerCore.Service
{
    public static class WebService
    {
        /// <summary>
        /// Performs a web request to provided URL.
        /// </summary>
        /// <param name="url">The URL to call</param>
        /// <param name="headers">(Optional) A set of tuples where item1 describes the header name and item2 the header value. These will be added to default headers.</param>
        /// <returns> 
        /// A string containing the response body.
         /// </returns>
        public static string GetContentFrom(string url, IEnumerable<Tuple<string,string>> headers = null)
        {
            var request = BuildRequest(url, headers);
            using(var stream = new StreamReader(request.GetResponse().GetResponseStream()))
            {
                return stream.ReadToEnd();   
            }
        }

        /// <summary>
        /// Performs a web request to provided URL.
        /// </summary>
        /// <param name="url">The URL to call</param>
        /// <param name="headers">(Optional) A set of tuples where item1 describes the header name and item2 the header value. These will be added to default headers.</param>
        /// <returns> 
        /// A stream containing the response body.
         /// </returns>
        public static async Task<Stream> GetContentStreamFromAsync(string url, IEnumerable<Tuple<string,string>> headers = null)
        {
            var response = await BuildRequest(url, headers).GetResponseAsync();
            // Do not closes the stream, just returns it!
            return response.GetResponseStream();
        }

        private static WebRequest BuildRequest(string url, IEnumerable<Tuple<string,string>> headers = null)
        {
            var request = WebRequest.Create(url);

            if(headers != null)
                foreach (var header in headers)
                    request.Headers.Add(header.Item1, header.Item2);

            return request;   
        }
    }
}