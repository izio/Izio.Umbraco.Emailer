using Umbraco.Core;
using Umbraco.Core.Persistence;

namespace Izio.Umbraco.Emailer.Installation
{
    public class DatabaseHelper
    {
        private static readonly Database Db;

        private const string ScriptCreate = @"CREATE TABLE [izioEmailerForms](
	        [Id] [int] IDENTITY(1,1) PRIMARY KEY,
	        [Name] [nvarchar](255) NOT NULL,
	        [Reference] [uniqueidentifier] NOT NULL,
	        [DestinationAddress] [nvarchar](255) NULL,
	        [SubmissionLimit] [int] NOT NULL,
	        [ConfirmationMessage] [ntext] NULL,
            [TemplateSubject] [nvarchar](255) NULL,
	        [TemplateBody] [ntext] NULL,
	        [ResponderEnabled] [bit] NOT NULL,
	        [ResponderAddress] [nvarchar](255) NULL,
	        [ResponderSubject] [nvarchar](255) NULL,
	        [ResponderBody] [ntext] NULL
        );";

        private const string ScriptDelete = @"DROP TABLE [izioEmailerForms];";

        static DatabaseHelper()
        {
            Db = ApplicationContext.Current.DatabaseContext.Database;
        }

        public static void CreateDatabase()
        {
            if (Db.TableExist("izioEmailerForms") == false)
            {
                Db.Execute(ScriptCreate);
            }
        }

        public static void DeleteDatabase()
        {
            if (Db.TableExist("izioEmailerForms"))
            {
                Db.Execute(ScriptDelete);
            }
        }
    }
}