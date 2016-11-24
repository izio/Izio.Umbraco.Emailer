using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Xml;
using Umbraco.Core.Logging;

namespace Izio.Umbraco.Emailer.Installation
{
    public class LanguageHelper
    {
        private const string UmbracoLanguagePath = "~/umbraco/config/lang/";
        private const string PluginsLanguagePath = "~/App_Plugins/Emailer/Lang/";

        public static bool AddLanguageTranslations()
        {
            var pluginFiles = GetLanguageFiles(PluginsLanguagePath);
            var pluginFilesArray = pluginFiles as FileInfo[] ?? pluginFiles.ToArray();
            var existingLanguages = GetLanguageFiles(UmbracoLanguagePath);

            foreach (var lang in existingLanguages)
            {
                var plugin = new XmlDocument() {PreserveWhitespace = true};
                var umbraco = new XmlDocument() {PreserveWhitespace = true};

                try
                {
                    var match = pluginFilesArray.FirstOrDefault(x => x.Name == lang.Name);

                    if (match != null)
                    {
                        plugin.LoadXml(File.ReadAllText(match.FullName));
                        umbraco.LoadXml(File.ReadAllText(lang.FullName));

                        var areas = plugin.DocumentElement.SelectNodes("//area");

                        foreach (XmlNode area in areas)
                        {
                            var existingArea =
                                umbraco.SelectSingleNode($"//area [@alias='{area.Attributes["alias"].Value}']");

                            if (existingArea == null)
                            {
                                var import = umbraco.ImportNode(area, true);

                                umbraco.DocumentElement.AppendChild(import);
                            }
                            else
                            {
                                foreach (XmlNode key in area.ChildNodes)
                                {
                                    if (key.NodeType == XmlNodeType.Element)
                                    {
                                        var existingKey =
                                            existingArea.SelectSingleNode(
                                                $"./key [@alias='{key.Attributes["alias"].Value}']");

                                        if (existingKey == null)
                                        {
                                            var add = umbraco.ImportNode(key, true);

                                            existingArea.AppendChild(add);
                                        }
                                    }
                                }
                            }
                        }

                        umbraco.Save(lang.FullName);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error<LanguageHelper>("Failed to add localization values to language file", ex);

                    return false;
                }

            }

            return true;
        }

        public static bool RemoveLanguageFiles()
        {
            var pluginFiles = GetLanguageFiles(PluginsLanguagePath);
            var pluginFilesArray = pluginFiles as FileInfo[] ?? pluginFiles.ToArray();
            var existingLanguages = GetLanguageFiles(UmbracoLanguagePath);

            foreach (var lang in existingLanguages)
            {
                var plugin = new XmlDocument() {PreserveWhitespace = true};
                var umbraco = new XmlDocument() {PreserveWhitespace = true};

                try
                {
                    var match = pluginFilesArray.FirstOrDefault(x => x.Name == lang.Name);

                    if (match != null)
                    {
                        plugin.LoadXml(File.ReadAllText(match.FullName));
                        umbraco.LoadXml(File.ReadAllText(lang.FullName));

                        var areas = plugin.DocumentElement.SelectNodes("//area");

                        foreach (XmlNode area in areas)
                        {
                            var existingArea =
                                umbraco.SelectSingleNode($"//area [@alias='{area.Attributes["alias"].Value}']");

                            if (existingArea != null)
                            {
                                foreach (XmlNode key in area.ChildNodes)
                                {
                                    if (key.NodeType == XmlNodeType.Element)
                                    {
                                        var existingKey =
                                            existingArea.SelectSingleNode(
                                                $"./key [@alias='{key.Attributes["alias"].Value}']");

                                        if (existingKey != null)
                                        {
                                            existingArea.RemoveChild(existingKey);
                                        }
                                    }
                                }

                                if (!area.HasChildNodes)
                                {
                                    umbraco.DocumentElement.RemoveChild(existingArea);
                                }
                            }
                        }

                        umbraco.Save(lang.FullName);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error<LanguageHelper>("Failed to remove localization values from language file", ex);

                    return false;

                }

            }
            return true;
        }

        private static IEnumerable<FileInfo> GetLanguageFiles(string path)
        {
            var languagePath = HostingEnvironment.MapPath(path);

            try
            {
                var directory = new DirectoryInfo(languagePath);

                return directory.GetFiles("*.xml");
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}