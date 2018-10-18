using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;

namespace Logitude.BL.InvoiceModel.EntityQueries
{
    public class ExternalSystemsTablesCodeQuery
    {
         ExternalSystemsTablesCodeRepository repository;
        public ExternalSystemsTablesCodeQuery()
        {
            repository = new ExternalSystemsTablesCodeRepository(); 
        }


        public ExternalSystemsTablesCodeQuery(ExternalSystemsTablesCodeRepository externalSystemsTablesCodeRepository)
        {
            repository = externalSystemsTablesCodeRepository;
        }

        public ExternalSystemsTablesCodeQuery(int tenant)
        {
            repository = new ExternalSystemsTablesCodeRepository(tenant);
        }

        public IQueryable<ExternalSystemsTablesCodePM> GetExternalSystemsTablesCodePMs()
        {
            return from a in repository.context.ExternalSystemsTablesCodes
                   select new ExternalSystemsTablesCodePM()
                   {
                       Id = a.Id,
                       Tenant = a.Tenant,
                       Code = a.Code,
                       Name = a.Name,
                    
                       LogitudeTable = a.LogitudeTable,
                       CreatedDate = a.CreatedDate,
                       UpdatedDate = a.UpdatedDate,
                       SearchFields = a.SearchFields,
                   };
        }

        public ExternalSystemsTablesCodePM GetSinglePM(string id, int tenant)
        {
            return (from a in repository.context.ExternalSystemsTablesCodes
                    where a.Id == id && a.Tenant == tenant
                    select new ExternalSystemsTablesCodePM()
                    {
                        Id = a.Id,
                       Tenant = a.Tenant,
                       Code = a.Code,
                       Name = a.Name,
                    
                       LogitudeTable = a.LogitudeTable,
                       CreatedDate = a.CreatedDate,
                       UpdatedDate = a.UpdatedDate,
                       SearchFields = a.SearchFields,
                    }).FirstOrDefault();
        }

        public IQueryable<ExternalSystemsTablesCodePM> GetExternalSystemsTablesCodePMsByTenant(int tenant)
        {
            IQueryable<ExternalSystemsTablesCodePM> ExternalSystemsTablesCodes = (from a in repository.context.ExternalSystemsTablesCodes
                                              where a.Tenant == tenant
                                              select new ExternalSystemsTablesCodePM()
                                              {
                                                    Id = a.Id,
                                                    Tenant = a.Tenant,
                                                    Code = a.Code,
                                                    Name = a.Name,
                    
                                                   LogitudeTable = a.LogitudeTable,
                                                   CreatedDate = a.CreatedDate,
                                                   UpdatedDate = a.UpdatedDate,
                                                   SearchFields = a.SearchFields,
                                              });
            return ExternalSystemsTablesCodes;
        }

        public IQueryable<ExternalSystemsTablesCodePM> GetExternalSystemsTablesCodeByCodeOrName(string code, string name, int tenant)
        {
            string codeNew = "";
            codeNew = code;

            string nameNew = "";
            nameNew = name;

            var query = (from a in repository.context.ExternalSystemsTablesCodes
                         where a.Tenant == tenant
                         select new ExternalSystemsTablesCodePM()
                         {
                                                    Id = a.Id,
                                                    Tenant = a.Tenant,
                                                    Code = a.Code,
                                                    Name = a.Name,
                    
                                                   LogitudeTable = a.LogitudeTable,
                                                   CreatedDate = a.CreatedDate,
                                                   UpdatedDate = a.UpdatedDate,
                                                   SearchFields = a.SearchFields,
                         }).AsQueryable();

            IQueryable<ExternalSystemsTablesCodePM> query2 = null;
            if (!string.IsNullOrEmpty(code))
            {
                query2 = query.Where(d => d.Code.ToUpper().StartsWith(code.ToUpper()));
            }
            if (!string.IsNullOrEmpty(name))
            {
                if (query2 != null)
                {
                    if (query2.Count() == 0)
                    {
                        query2 = query.Where(d => d.Name.ToUpper().StartsWith(name.ToUpper()));
                    }
                }
                else
                {
                    query2 = query.Where(d => d.Name.ToUpper().StartsWith(name.ToUpper()));

                }
            }
            if (query2 != null)
            {
                return query2;
            }
            else
                return query;
        }

        public IQueryable<ExternalSystemsTablesCodeList> GetIQueryableEntityList(IQueryable<ExternalSystemsTablesCode> iQueryable)
        {
            IQueryable<ExternalSystemsTablesCodeList> result = from ExternalSystemsTablesCode in iQueryable
                                             select new ExternalSystemsTablesCodeList()
                                             {
                                                 Id = ExternalSystemsTablesCode.Id,
                                                 Tenant =ExternalSystemsTablesCode.Tenant,
                                                 Code =ExternalSystemsTablesCode.Code,
                                                 Name =ExternalSystemsTablesCode.Name,
                                                 LogitudeTable =ExternalSystemsTablesCode.LogitudeTable,
                                                CreatedDate = ExternalSystemsTablesCode.CreatedDate,
                                                UpdatedDate = ExternalSystemsTablesCode.UpdatedDate,
                                                 SearchFields =ExternalSystemsTablesCode.SearchFields,
                                             };
            return result;
        }


       
    }
}