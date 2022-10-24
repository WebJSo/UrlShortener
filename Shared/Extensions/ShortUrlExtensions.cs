using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Shared.Extensions
{
    /// <summary>
    /// Extension/utility methods used for creating short url from id, 
    /// formatting url to add correct uri schemas for redirecting, 
    /// and validating url format against regular expression
    /// </summary>
    public static class ShortUrlExtensions
    {
        public static string ShortUrl(this int id)
        {
            // get byte array from id
            byte[] byteArray = Encoding.UTF8.GetBytes(id.ToString());

            // encode byte array to base 64 string (short url)
            string shortUrl = Convert.ToBase64String(byteArray).Trim('=');

            return shortUrl;
        }

        public static string FormatUrl(this string url)
        {
            // cant format url
            if (string.IsNullOrEmpty(url))
                return null;

            // contains :// or http
            bool containsDelimeter = url.Contains(Uri.SchemeDelimiter),
                 startsWithHttp = url.StartsWith(Uri.UriSchemeHttp);

            // check if url can be converted into correct format
            if (containsDelimeter && !startsWithHttp)
            {
                url = string.Concat(Uri.UriSchemeHttps, url);
            }
            else if (!containsDelimeter && !startsWithHttp)
            {
                url = string.Concat(Uri.UriSchemeHttps, Uri.SchemeDelimiter, url);
            }

            return url;
        }

        public static bool ValidUrl(this string url)
        {
            // not valid url
            if (string.IsNullOrEmpty(url))
                return false;

            // http://google.co
            string pattern = @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$";
            Regex rgx = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            // is reg ex match and well formed url
            bool isValid = rgx.IsMatch(url) && Uri.IsWellFormedUriString(url, UriKind.Absolute);

            return isValid;
        }
    }
}
