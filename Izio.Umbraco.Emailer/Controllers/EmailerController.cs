using System;
using System.Web.Helpers;
using System.Web.Mvc;
using Izio.Umbraco.Emailer.Repositories;
using Izio.Umbraco.Emailer.ViewModels;
using umbraco;
using Umbraco.Core.Logging;
using Umbraco.Web.Mvc;

namespace Izio.Umbraco.Emailer.Controllers
{
    [PluginController("Emailer")]
    public class EmailerController : SurfaceController
    {
        private readonly FormRepsoitory _repository;

        public EmailerController()
        {
            _repository = new FormRepsoitory();
        }

        [ChildActionOnly]
        public ActionResult Render(string formReference)
        {
            var form = _repository.GetByReference(new Guid(formReference));

            if (form != null)
            {
                return PartialView("Form", new EmailerViewModel { SubmissionStamp = Security.GenerateSecurityToken(), FormReference = form.Reference.ToString()});   
            }

            return null;
        }

        [HttpPost]
        [NotChildAction]
        public ActionResult Submit(EmailerViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    AntiForgery.Validate();

                    if (Convert.ToBoolean(Request.Form["Confirmation"]))
                    {
                        var form = _repository.GetByReference(new Guid(model.FormReference));

                        if (form != null)
                        {
                            var address = form.DestinationAddress;
                            var submissionLimit = form.SubmissionLimit;

                            if (Security.ValidateSecurityToken(model.SubmissionStamp, submissionLimit))
                            {
                                library.SendMail(model.Email, address, FormatTemplate(form.TemplateSubject, model), FormatTemplate(form.TemplateBody, model), true);

                                try
                                {
                                    if (form.ResponderEnabled)
                                    {
                                        library.SendMail(form.ResponderAddress, model.Email, FormatTemplate(form.ResponderSubject, model), FormatTemplate(form.ResponderBody, model), true);
                                    }
                                }
                                catch (Exception)
                                {
                                    //hide exception
                                }
                                
                                TempData["EmailSent"] = true;
                                TempData["ConfirmationMessage"] = form.ConfirmationMessage;

                                return RedirectToCurrentUmbracoPage();
                            }

                            ModelState.AddModelError("", $"Submission throttle breached, you must wait {form.SubmissionLimit} seconds before submitting the form.");
                        }
                        else
                        {
                            ModelState.AddModelError("", "There was an error sending your email, please refresh the page and try again.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to confirm you are not a spam bot");
                    }
                }
                catch (HttpAntiForgeryException ex)
                {
                    LogHelper.Error(GetType(), "Anti forgery validation failed sending email", ex);

                    ModelState.AddModelError("", "There was an error sending your email, please refresh the page and try again.");
                }
                catch (Exception ex)
                {
                    LogHelper.Error(GetType(), "An error occurred sending email", ex);

                    ModelState.AddModelError("", "There was an error sending your email, please refresh the page and try again.");
                }
            }

            return CurrentUmbracoPage();
        }

        static string FormatTemplate(string template, EmailerViewModel model)
        {
            return template.Replace("[name]", model.Name).Replace("[email]", model.Email).Replace("[subject]", model.Subject).Replace("[body]", model.Body);
        }
    }
}
