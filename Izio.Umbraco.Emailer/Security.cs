using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using HtmlAgilityPack;

namespace Izio.Umbraco.Emailer
{
    public class Security
    {
        private static readonly byte[] Entropy = { 24, 64, 98, 30, 63, 53, 81, 95, 35, 87, 32, 17, 69, 61, 04 };
        private static readonly Dictionary<string, string[]> WhiteList = new Dictionary<string, string[]>
            {
                {"p", null},
                {"#text", null},
                {"span", null},
                {"br", null},
                {"strong", null},
                {"b", null},
                {"em", null},
                {"i", null},
                {"u", null},
                {"ol", null},
                {"ul", null},
                {"li", null}
            };

        /// <summary>
        /// generates an encrypted security token
        /// </summary>
        /// <returns>string</returns>
        public static string GenerateSecurityToken()
        {
            return Convert.ToBase64String(ProtectedData.Protect(Encoding.Unicode.GetBytes(DateTime.Now.ToString()), Entropy, DataProtectionScope.LocalMachine));
        }

        /// <summary>
        /// validates that the security token is valid for the specified time period
        /// </summary>
        /// <param name="token">the encrypted security token</param>
        /// <param name="period">the time period in seconds</param>
        /// <returns>bool</returns>
        public static bool ValidateSecurityToken(string token, int period)
        {
            try
            {
                var decryptedToken = Encoding.Unicode.GetString(ProtectedData.Unprotect(Convert.FromBase64String(token), Entropy, DataProtectionScope.LocalMachine));

                return ((DateTime.Now - Convert.ToDateTime(decryptedToken)).TotalSeconds > period);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// sanitises the given html string removing any tags not contained within the whitelist and stripping any
        /// </summary>
        /// <param name="input">the html string to sanitise</param>
        /// <returns>string</returns>
        public static string SanitiseHtml(string input)
        {
            return SanitiseHtml(input, WhiteList);
        }

        /// <summary>
        /// sanitises the given html string removing any tags not contained within the whitelist
        /// </summary>
        /// <param name="input">the html string to sanitise</param>
        /// <param name="whiteList">a dictionary list of whitelisted tags and associated attributes</param>
        /// <returns>string</returns>
        public static string SanitiseHtml(string input, Dictionary<string, string[]> whiteList)
        {
            var html = new HtmlDocument
            {
                OptionFixNestedTags = true,
                OptionAutoCloseOnEnd = true,
                OptionCheckSyntax = true,
                OptionWriteEmptyNodes = true,
                OptionOutputAsXml = true,
                OptionDefaultStreamEncoding = Encoding.UTF8
            };

            html.LoadHtml(input);

            IList<HtmlNode> nodes = html.DocumentNode.Descendants().ToList();

            for (var i = nodes.Count - 1; i >= 0; i--)
            {
                var htmlNode = nodes[i];

                //check node tag is in whitelist
                if (!WhiteList.ContainsKey(htmlNode.Name.ToLower()))
                {
                    htmlNode.Remove();

                    continue;
                }

                for (var att = htmlNode.Attributes.Count - 1; att >= 0; att--)
                {
                    //get node attributes
                    var attributes = htmlNode.Attributes[att];

                    //get attribute whitelist for node
                    var attributeWhiteList = WhiteList.SingleOrDefault(t => t.Key == htmlNode.Name.ToLower()).Value;

                    //remove any attribute that is not in the whitelist
                    if (!attributeWhiteList.Contains(attributes.Name.ToLower()))
                    {
                        attributes.Remove();
                    }

                    //strip expressions from any style attributes
                    if (attributes.Value.ToLower().Contains("expression") && attributes.Name.ToLower() == "style")
                    {
                        attributes.Value = string.Empty;
                    }

                    //disable any image or links to non-http sources
                    if (attributes.Name.ToLower() == "src" || attributes.Name.ToLower() == "href")
                    {
                        if (!attributes.Value.StartsWith("http")) attributes.Value = "#";
                    }
                }
            }

            return html.DocumentNode.WriteTo().Substring(38);
        }
    }
}
