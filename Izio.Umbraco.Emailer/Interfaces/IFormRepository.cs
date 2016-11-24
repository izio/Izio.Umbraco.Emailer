using System;
using System.Collections.Generic;
using Izio.Umbraco.Emailer.Models;

namespace Izio.Umbraco.Emailer.Interfaces
{
    public interface IFormRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<Form> GetAll();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Form GetById(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Form GetByReference(Guid reference);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        Form Save(Form form);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);
    }
}
