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
            var recordCount = Db.ExecuteScalar<int>("SELECT COUNT(*) FROM umbracoUser2app WHERE [user] = @0 AND app = 'izioEmailer'", UmbracoContext.Current.Security.CurrentUser.Id);

            if (recordCount == 0)
            {
                Db.Execute("INSERT INTO umbracoUser2app ([user], app) VALUES  (@0, 'izioEmailer')", UmbracoContext.Current.Security.CurrentUser.Id);
            }
        }

        public static void RevokePermissions()
        {
            Db.Execute("DELETE FROM umbracoUser2app WHERE app = 'izioEmailer';");
        }
    }
}
