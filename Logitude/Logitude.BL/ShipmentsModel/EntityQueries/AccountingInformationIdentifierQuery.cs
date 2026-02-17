using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.ShipmentsModel.EntityLists;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class AccountingInformationIdentifierQuery
    {
        private AccountingInformationIdentifierRepository entityRepository;

        public AccountingInformationIdentifierQuery(int tenant)
        {
            this.entityRepository = new AccountingInformationIdentifierRepository(tenant);
        }

        public AccountingInformationIdentifierQuery(AccountingInformationIdentifierRepository repository)
        {
            this.entityRepository = repository;
        }

        public AccountingInformationIdentifierList GetSingleAccountingInformationIdentifierList(string code)
        {
            AccountingInformationIdentifierList myResult = null;

            AccountingInformationIdentifier entity = entityRepository.GetSingleAccountingInformationIdentifier(code);
            if (entity != null)
            {
                myResult = new AccountingInformationIdentifierList()
                {
                    Code = entity.Code,
                    Name = entity.Name,
                    SearchFields = entity.SearchFields
                };
            }

            return myResult;
        }

        public IQueryable<AccountingInformationIdentifierList> GetIQueryableEntityList(IQueryable<AccountingInformationIdentifier> iQueryable)
        {
            IQueryable<AccountingInformationIdentifierList>
                result = from a in iQueryable
                         select new AccountingInformationIdentifierList()
                         {
                             Code = a.Code,
                             Name = a.Name,
                             SearchFields = a.SearchFields
                         };

            return result;
        }


    }
}