using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveApp
{
    public static class UriExtensions
    {        
        public static Dictionary<string, string> ParseQueryString(this Uri uri)
        {
            var toReturn = new Dictionary<string, string>();

            foreach (var vp in (uri.Query ?? string.Empty).Split('&'))
            {
                AddPairFrom(toReturn, vp);
            }
            return toReturn;
        }

        private static void AddPairFrom(Dictionary<string, string> toReturn, string vp)
        {
            string[] singlePair = vp.Split('=');
            if (singlePair.Length == 2)
            {
                toReturn[singlePair[0]] = WebUtility.UrlDecode(singlePair[1]);
            }
            else
            {
                toReturn[singlePair[0]] = string.Empty;
            }
        }
    }
}
