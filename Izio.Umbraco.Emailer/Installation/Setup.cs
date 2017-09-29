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
            DashboardHelper.AddDashboardSection();
            PermissionHelper.GrantPermissions();
            

            return true;
        }

        public bool Undo(string packageName, XmlNode xmlData)
        {
            PermissionHelper.RevokePermissions();
            DashboardHelper.RemoveDashboardSection();
            DatabaseHelper.DeleteDatabase();

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
