using System;
using System.Collections.Generic;
using System.Web.Http;
using Izio.Umbraco.Emailer.Models;
using Izio.Umbraco.Emailer.Repositories;
using Umbraco.Web.WebApi;

namespace Izio.Umbraco.Emailer.Controllers
{
    public class EmailerApiController : UmbracoAuthorizedApiController
    {
        private readonly FormRepsoitory _repository;

        public EmailerApiController()
        {
            _repository = new FormRepsoitory();
        }

        [HttpGet]
        public IEnumerable<Form> GetAll()
        {
            return _repository.GetAll();
        }

        [HttpGet]
        public Form GetById(int id)
        {
            return _repository.GetById(id);
        }

        [HttpGet]
        public Form GetByReference(Guid reference)
        {
            return _repository.GetByReference(reference);
        }

        [HttpPost]
        public Form Save(Form form)
        {
            _repository.Save(form);

            return form;
        }

        [HttpPost]
        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}
