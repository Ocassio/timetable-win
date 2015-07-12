using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Timetable.Utils
{
    public class NetworkUtils
    {
        private NetworkUtils() { }

        public static async Task<HtmlDocument> LoadDocument(string url)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlNode.ElementsFlags.Remove("option");

            return await web.LoadFromWebAsync(url);
        }

        public static async Task<HtmlDocument> LoadDocument(string url, string method, Dictionary<string, string> parameters)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = method;
            request.ContentType = "application/x-www-form-urlencoded";

            byte[] byteParams = getParamsBytes(parameters);
            using (Stream requestStream = await request.GetRequestStreamAsync())
            {
                requestStream.Write(byteParams, 0, byteParams.Length);
            }

            HtmlDocument doc = new HtmlDocument();
            WebResponse response = await request.GetResponseAsync();
            doc.Load(response.GetResponseStream(), Encoding.GetEncoding("windows-1251"));

            return doc;
        }

        private static string getParamsString(Dictionary<string, string> parameters)
        {
            StringBuilder builder = new StringBuilder();

            if (parameters.Count > 0)
            {
                Dictionary<string, string>.Enumerator enumerator = parameters.GetEnumerator();
                while(enumerator.MoveNext())
                {
                    builder.AppendFormat("{0}={1}&", enumerator.Current.Key, enumerator.Current.Value);
                }
                builder.Remove(builder.Length - 1, 1);
            }

            return builder.ToString();            
        }

        private static byte[] getParamsBytes(Dictionary<string, string> parameters)
        {
            return Encoding.GetEncoding("windows-1251").GetBytes(getParamsString(parameters));
        }
    }
}
