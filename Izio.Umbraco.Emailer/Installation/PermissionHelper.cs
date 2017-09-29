using Umbraco.Core;
using Umbraco.Core.Persistence;
using Umbraco.Web;

namespace Izio.Umbraco.Emailer.Installation
{
    public class PermissionHelper
    {
        private static readonly Database Db;

        static PermissionHelper()
        {
            Db = ApplicationContext.Current.DatabaseContext.Database;
        }

        public static void GrantPermissions()
        {
            if (UmbracoContext.Current.Security.CurrentUser != null)
            {
                var recordCount = Db.ExecuteScalar<int>("SELECT COUNT(*) FROM umbracoUserGroup2App WHERE userGroupId = 1 AND app = 'izioEmailer'");

                if (recordCount == 0)
                {
                    Db.Execute("INSERT INTO umbracoUserGroup2App (userGroupId, app) VALUES  (@0, 'izioEmailer')", 1);
                }
            }
        }

        public static void RevokePermissions()
        {
            Db.Execute("DELETE FROM umbracoUserGroup2App WHERE app = 'izioEmailer';");
        }
    }
}
