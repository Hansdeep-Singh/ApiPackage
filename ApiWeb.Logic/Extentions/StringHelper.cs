using Logic.Efficacy;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Logic.Extentions
{
    public static class StringHelper
    {
        public static string ChangeFirstLetterCase(this string inputString)
        {
            if (inputString.Length > 0)
            {
                char[] charArray = inputString.ToCharArray();
                charArray[0] = char.IsUpper(charArray[0]) ? char.ToLower(charArray[0]) : char.ToUpper(charArray[0]);
                return new string(charArray);
            }
            return inputString;
        }

        public static string PrefixId(this string prefix, string id) => $"{prefix}{id}";

        public static string[] TrimArray(this string[] arr) => (arr.ToList().Select(x => x.Trim())).ToArray();

        public static List<string> StringToList(this string values, char seperator = ',')
        {
            List<string> list = new List<string>();
            if (!string.IsNullOrEmpty(values))
            {
                foreach (var s in values.Split(seperator))
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        list.Add(s);
                    }
                }
            }
            return list;
        }
       

        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value[..maxLength];
        }
        public static bool Between<T>(this T item, T start, T end) =>
             Comparer<T>.Default.Compare(item, start) >= 0
                && Comparer<T>.Default.Compare(item, end) <= 0;

        public static DateTime ToMelbLocalTime(this DateTime dateTime) => ((DateTime)dateTime).ToLocalTime();

        public static string? NullIfEmpty(this string s)
        {
            return string.IsNullOrEmpty(s) ? null : s;
        }
        public static string? NullIfWhiteSpace(this string s)
        {
            return string.IsNullOrWhiteSpace(s) ? null : s;
        }
        //public static string EmbedTemplate(this string template, string toReplace, string replacement)
        //{
        //    if (!string.IsNullOrEmpty(template))
        //    {
        //        var valueMap = new Dictionary<string, string>
        //        {
        //            {$"{Constants.Signature}{toReplace}{Constants.Signature}",replacement}
        //        };
        //        foreach (var c in valueMap)
        //        {
        //            template = template.Replace(c.Key, c.Value);
        //        }
        //    }
        //    return template;
        //}

        //public static string TemplateReady(this string template, Dictionary<string, string> embedments)
        //{
        //    if (!string.IsNullOrEmpty(template))
        //    {
        //        foreach (var e in embedments)
        //        {
        //            template = template.EmbedTemplate(e.Key, e.Value);
        //        }
        //    }
        //    return template;
        //}

        public static string ReplaceSpecialChars(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                var charMap = new Dictionary<string, string>
                {
                    {"","&trade;"},{"'","&#39;" }
                };
                foreach (var c in charMap)
                {
                    value = value.Replace(c.Key, c.Value);
                }
            }
            return value;
        }
        public static string EncodeNonAsciiCharacters(this string value)
        {
            var sb = new StringBuilder();
            //foreach (var c in value)
            foreach (char c in value)
            {
                if (c > 127)
                {
                    var encodedValue = "\\u" + ((int)c).ToString("x4");
                    sb.Append(encodedValue);
                }
                else { sb.Append(c); }

            }
            return sb.ToString();
        }
        public static string DecodeEncodedNonAsciiCharacters(this string value)
        {
            return Regex.Replace(
                value,
                @"\\u(?<Value>)[a-zA-Z0-9]{4}",
                m => ((char)int.Parse(m.Groups["Value"].Value, System.Globalization.NumberStyles.HexNumber)).ToString()
                );
        }

        public static string TemplateBuilder(this string template, string[] values)
        {
            return string.Format(template, values);
        }
        public static Uri AddQuery(this Uri uri, string name, string value)
        {
            var httpValueCollection = HttpUtility.ParseQueryString(uri.Query);

            httpValueCollection.Remove(name);
            httpValueCollection.Add(name, value);

            var ub = new UriBuilder(uri);
            ub.Query = httpValueCollection.ToString();

            return ub.Uri;
        }

        public static string ValidEmail(this string email)
        {
            string regex = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            return (Regex.IsMatch(email, regex, RegexOptions.IgnoreCase)) ? email : "Invalid Email";
        }

        public static string ArrayToString(this Array array, string separator)
        {
            return array.Length != 0 ? string.Join(separator, array) : string.Empty;
        }

        //public static Uri BuildUri(this string host, bool ssl, string path)
        //{
        //    UriBuilder ub = new()
        //    {
        //        Host = host,
        //        Path = path,
        //        Scheme = ssl ? Constants.SSL : Constants.NoSSL
        //    };
        //    return ub.Uri;
        //}

        public static Uri AddQueryB(this Uri uri, string name, string value)
        {
            var httpValueCollection = HttpUtility.ParseQueryString(uri.Query);

            httpValueCollection.Remove(name);
            httpValueCollection.Add(name, value);

            var ub = new UriBuilder(uri);

            // this code block is taken from httpValueCollection.ToString() method
            // and modified so it encodes strings with HttpUtility.UrlEncode
            if (httpValueCollection.Count == 0)
                ub.Query = String.Empty;
            else
            {
                var sb = new StringBuilder();

                for (int i = 0; i < httpValueCollection.Count; i++)
                {
                    string text = httpValueCollection.GetKey(i)!;
                    {
                        text = HttpUtility.UrlEncode(text);

                        string val = (text != null) ? (text + "=") : string.Empty;
                        string[] vals = httpValueCollection.GetValues(i)!;

                        if (sb.Length > 0)
                            sb.Append('&');

                        if (vals == null || vals.Length == 0)
                            sb.Append(val);
                        else
                        {
                            if (vals.Length == 1)
                            {
                                sb.Append(val);
                                sb.Append(HttpUtility.UrlEncode(vals[0]));
                            }
                            else
                            {
                                for (int j = 0; j < vals.Length; j++)
                                {
                                    if (j > 0)
                                        sb.Append('&');

                                    sb.Append(val);
                                    sb.Append(HttpUtility.UrlEncode(vals[j]));
                                }
                            }
                        }
                    }
                }

                ub.Query = sb.ToString();
            }

            return ub.Uri;
        }

    }
}
