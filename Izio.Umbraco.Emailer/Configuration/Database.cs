using Umbraco.Core;
using Umbraco.Core.Persistence;

namespace Izio.Umbraco.Emailer.Configuration
{
    public class Database : ApplicationEventHandler
    {
        private const string Sql = @"CREATE TABLE [izioEmailerForms](
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
        )";

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            var db = applicationContext.DatabaseContext.Database;

            if (!db.TableExist("izioEmailerForms"))
            {
                db.Execute(Sql);
            }
        }
    }
}
