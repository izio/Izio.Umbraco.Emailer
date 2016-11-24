using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Izio.Umbraco.Emailer.ViewModels
{
    public class EmailerViewModel
    {
        private string _name;
        private string _subject;
        private string _body;

        [Required]
        public string Name
        {
            get { return _name; }
            set { _name = HttpUtility.HtmlEncode(value); }
        }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required]
        public string Subject
        {
            get { return _subject; }
            set { _subject = HttpUtility.HtmlEncode(value); }
        }

        [Required]
        [AllowHtml]
        public string Body
        {
            get { return _body; }
            set { _body = Security.SanitiseHtml(value); }
        }

        [Required]
        public string SubmissionStamp { get; set; }

        [Required]
        public string FormReference { get; set; }
    }
}
