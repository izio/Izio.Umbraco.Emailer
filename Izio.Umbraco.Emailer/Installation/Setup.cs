using System;
using System.Xml;
using umbraco.interfaces;

namespace Izio.Umbraco.Emailer.Installation
{
    public class Setup : IPackageAction
    {
        public string Alias()
        {
            return "Emailer_Setup";
        }

        public bool Execute(string packageName, XmlNode xmlData)
        {
            DatabaseHelper.CreateDatabase();
            PermissionHelper.GrantPermissions();
            LanguageHelper.AddLanguageTranslations();
            DashboardHelper.AddDashboardSection();

            return true;
        }

        public bool Undo(string packageName, XmlNode xmlData)
        {
            DatabaseHelper.DeleteDatabase();
            LanguageHelper.RemoveLanguageFiles();
            PermissionHelper.RevokePermissions();
            DashboardHelper.RemoveDashboardSection();

            return true;
        }

        public XmlNode SampleXml()
        {
            var sample = new XmlDocument();
            sample.LoadXml(string.Format("<Action runat=\"install\" undo=\"true\" alias=\"{0}\" />", Alias()));

            return sample;
        }
    }
}
