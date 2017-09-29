using System;
using System.IO;
using System.Web.Hosting;
using System.Xml;
using Umbraco.Core.Logging;

namespace Izio.Umbraco.Emailer.Installation
{
    public class DashboardHelper
    {
        public static bool AddDashboardSection()
        {
            try
            {
                var plugin = new XmlDocument() { PreserveWhitespace = true };
                plugin.LoadXml(File.ReadAllText(HostingEnvironment.MapPath("~/App_Plugins/Emailer/dashboard.config")));

                var dashboard = new XmlDocument() { PreserveWhitespace = true };
                dashboard.LoadXml(File.ReadAllText(HostingEnvironment.MapPath("~/config/dashboard.config")));

                var pluginSections = plugin.SelectNodes("//section");

                foreach (XmlNode section in pluginSections)
                {
                    var existingSection = dashboard.SelectSingleNode($"//section [@alias='{section.Attributes["alias"].Value}']");

                    if (existingSection == null)
                    {
                        var import = dashboard.ImportNode(section, true);

                        dashboard.DocumentElement.AppendChild(import);
                    }
                }

                dashboard.Save(HostingEnvironment.MapPath("~/config/dashboard.config"));
            }
            catch (Exception ex)
            {
                LogHelper.Error<DashboardHelper>("Failed to add section to dashboard", ex);

                return false;
            }

            return true;
        }

        public static bool RemoveDashboardSection()
        {
            try
            {
                var plugin = new XmlDocument() { PreserveWhitespace = true };
                plugin.LoadXml(File.ReadAllText(HostingEnvironment.MapPath("~/App_Plugins/Emailer/dashboard.config")));

                var dashboard = new XmlDocument() { PreserveWhitespace = true };
                dashboard.LoadXml(File.ReadAllText(HostingEnvironment.MapPath("~/config/dashboard.config")));

                var pluginSections = plugin.SelectNodes("//section");

                foreach (XmlNode section in pluginSections)
                {
                    var existingSection = dashboard.SelectSingleNode($"//section [@alias='{section.Attributes["alias"].Value}']");

                    if (existingSection != null)
                    {
                        dashboard.DocumentElement.RemoveChild(existingSection);
                    }
                }

                dashboard.Save(HostingEnvironment.MapPath("~/config/dashboard.config"));
            }
            catch (Exception ex)
            {
                LogHelper.Error<DashboardHelper>("Failed to remove section from dashboard", ex);

                return false;
            }

            return true;
        }
    }
}