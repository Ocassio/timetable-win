using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Timetable.Encodings;

namespace Timetable.Utils
{
    public class NetworkUtils
    {
        private static readonly Encoding PAGE_ENCODING = new Win1251Encoding();

        private NetworkUtils() { }

        public static async Task<HtmlDocument> LoadDocument(string url)
        {
            HtmlNode.ElementsFlags.Remove("option");
            var request = (HttpWebRequest) WebRequest.Create(url);

            var doc = new HtmlDocument { OptionReadEncoding = false };
            var response = await request.GetResponseAsync();
            doc.Load(response.GetResponseStream(), PAGE_ENCODING);

            return doc;
        }

        public static async Task<HtmlDocument> LoadDocument(string url, string method, Dictionary<string, string> parameters)
        {
            var request = (HttpWebRequest) WebRequest.Create(url);
            request.Method = method;
            request.ContentType = "application/x-www-form-urlencoded";

            var byteParams = GetParamsBytes(parameters);
            using (var requestStream = await request.GetRequestStreamAsync())
            {
                requestStream.Write(byteParams, 0, byteParams.Length);
            }

            var doc = new HtmlDocument { OptionReadEncoding = false };
            var response = await request.GetResponseAsync();
            doc.Load(response.GetResponseStream(), PAGE_ENCODING);

            return doc;
        }

        private static string GetParamsString(Dictionary<string, string> parameters)
        {
            var builder = new StringBuilder();

            if (parameters.Count > 0)
            {
                var enumerator = parameters.GetEnumerator();
                while(enumerator.MoveNext())
                {
                    builder.AppendFormat("{0}={1}&", enumerator.Current.Key, enumerator.Current.Value);
                }
                builder.Remove(builder.Length - 1, 1);
            }

            return builder.ToString();            
        }

        private static byte[] GetParamsBytes(Dictionary<string, string> parameters)
        {
            return PAGE_ENCODING.GetBytes(GetParamsString(parameters));
        }
    }
}
