	using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityLists;

namespace Logitude.Accounting.Data.EntityListQueryServices
{

    public partial class GLAccountCurrencyListQueryService
    {
        private IQueryable<GLAccountCurrencyList> GetIqueryableList(IQueryable<GLAccountCurrency> iQueryable)
        {
            IQueryable<GLAccountCurrencyList> query = (from a in iQueryable
                                                       select new GLAccountCurrencyList()
                                                       {

                                                           Id = a.Id,

                                                           Tenant = a.Tenant,

                                                           GLAccountId = a.GLAccountId,

                                                           MainGLAccountId = a.MainGLAccountId,
                                                           CurrencyId = a.CurrencyId,
                                                           CurrencyName = a.Currency != null ? a.Currency.EnglishName : null,
                                                           CurrencyCode = a.Currency != null ? a.Currency.Code : null,
                                                           GLAccountName = a.GLAccount != null ? a.GLAccount.LocalName : null,
                                                           GLAccountNumber = a.GLAccount != null ? a.GLAccount.DisplayNumber : null,


                                                       });
            return query;
        }

        public GLAccountCurrencyList GetByAccountNumber(string number, int tenant)
        {

            IQueryable<GLAccountCurrency> glAccountCurrencyQuery = (from a in context.GLAccountCurrencies
                                                                    where a.Tenant == tenant && a.MainGLAccountId == number
                                                select a);

            IQueryable<GLAccountCurrencyList> glAccountCurrencyListQuery = GetIqueryableList(glAccountCurrencyQuery);
            var myList = glAccountCurrencyListQuery.FirstOrDefault();
            return myList;
        }

        public List<GLAccountCurrencyList> GetByAccountId(string AccountId, int tenant)
        {
            IQueryable<GLAccountCurrency> query = (from a in context.GLAccountCurrencies
                                                                    where a.Tenant == tenant && a.MainGLAccountId == AccountId
                                                                    select a);

            IQueryable<GLAccountCurrencyList> result = GetIqueryableList(query);
            var myList = result.ToList();
            return myList;
        }



        private IQueryable<GLAccountCurrency> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<GLAccountCurrency> iQueryable, int tenant)
        {
            return iQueryable;
        }
        private IQueryable<GLAccountCurrency> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<GLAccountCurrency> iQueryable, int tenant)
        {
            return iQueryable;
        }

    }


}
	