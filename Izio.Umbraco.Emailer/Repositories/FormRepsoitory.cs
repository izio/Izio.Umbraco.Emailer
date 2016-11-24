using System;
using System.Collections.Generic;
using Izio.Umbraco.Emailer.Interfaces;
using Izio.Umbraco.Emailer.Models;
using Umbraco.Core;
using Umbraco.Core.Persistence;

namespace Izio.Umbraco.Emailer.Repositories
{
    public class FormRepsoitory : IFormRepository
    {
        private readonly Database _db;

        public FormRepsoitory()
        {
            _db = ApplicationContext.Current.DatabaseContext.Database;
        }

        #region IFormRepository

        public IEnumerable<Form> GetAll()
        {
            return _db.Fetch<Form>("SELECT * FROM izioEmailerForms");
        }

        public Form GetById(int id)
        {
            return _db.SingleOrDefault<Form>("SELECT * FROM izioEmailerForms WHERE Id = @0", id);
        }

        public Form GetByReference(Guid reference)
        {
            return _db.SingleOrDefault<Form>("SELECT * FROM izioEmailerForms WHERE Reference = @0", reference);
        }

        public Form Save(Form form)
        {
            if (form.IsNew || form.IsDirty)
            {
                _db.Save(form);
            }

            return form;
        }

        public void Delete(int id)
        {
            _db.Execute("DELETE FROM izioEmailerForms WHERE Id = @0", id);
        }

        #endregion
    }
}
