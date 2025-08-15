using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class IncotermQuery
    {
        IncotermRepository repository;

 

        public IncotermQuery(int tenant)
        {
            repository = new IncotermRepository(tenant);
        }

        public IncotermQuery(IncotermRepository repository)
        {
            this.repository = repository;
        }

        public IncotermPM GetSinglePM(string id, int tenant)
        {
            IncotermPM incoterm = (from a in repository.context.Incoterms
                                   where a.Tenant == tenant && a.Id == id
                                   select new IncotermPM()
                                   {
                                       AddedManually = a.AddedManually,
                                       Code = a.Code,
                                       Freight = a.Freight,
                                       Id = a.Id,
                                       InActive = a.InActive,
                                       LocalName = a.LocalName,
                                       Name = a.Name,
                                       OtherCharges = a.OtherCharges,
                                       Notes = a.Notes,
                                       Tenant = a.Tenant,
                                       ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.Name : a.LocalName,
                                       SearchFields = a.SearchFields,
                                   }).FirstOrDefault();

            IncotermPM securedPm = new IncotermPM();
            SecuredMapping.GetMappedPM(incoterm, securedPm, "Incoterm", tenant);

            return securedPm;
        }



        public string GetIncotermIdByCode(string code, int tenant)
        {
            return (from a in repository.context.Incoterms
                    where a.Tenant == tenant && a.Code == code
                    select a.Id).FirstOrDefault();


        }


        public IncotermPM GetIncotermPMById(string id, int tenant)
        {
            IncotermPM incoterm = (from a in repository.context.Incoterms
                                   where a.Tenant == tenant && a.Id == id
                                   select new IncotermPM()
                                   {
                                       AddedManually = a.AddedManually,
                                       Code = a.Code,
                                       Id = a.Id,

                                   }).FirstOrDefault();



            return incoterm;
        }




        public IQueryable<IncotermPM> GetIncotermPMsByTenant(int tenant)
        {
            IQueryable<IncotermPM> incoterms = from a in repository.context.Incoterms
                                               where a.Tenant == tenant
                                               select new IncotermPM()
                                               {
                                                   AddedManually = a.AddedManually,
                                                   Code = a.Code,
                                                   Freight = a.Freight,
                                                   Id = a.Id,
                                                   InActive = a.InActive,
                                                   LocalName = a.LocalName,
                                                   Name = a.Name,
                                                   OtherCharges = a.OtherCharges,
                                                   Notes = a.Notes,
                                                   Tenant = a.Tenant,
                                                   ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.Name : a.LocalName,
                                                   SearchFields = a.SearchFields,
                                               };
            return incoterms;
        }

        public IQueryable<IncotermPM> GetIncotermsByCodeOrName(string code, string name, int tenant)
        {
            string codeNew = "";
            codeNew = code;

            string nameNew = "";
            nameNew = name;

            var query = (from a in repository.context.Incoterms
                         where a.Tenant == tenant
                         select new IncotermPM()
                         {
                             AddedManually = a.AddedManually,
                             Code = a.Code,
                             Freight = a.Freight,
                             Id = a.Id,
                             InActive = a.InActive,
                             LocalName = a.LocalName,
                             Name = a.Name,
                             OtherCharges = a.OtherCharges,
                             Notes = a.Notes,
                             Tenant = a.Tenant,
                             ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.Name : a.LocalName,
                             SearchFields = a.SearchFields,
                         });

            IQueryable<IncotermPM> query2 = null;
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

        public IQueryable<IncotermList> GetIQueryableEntityList(IQueryable<Incoterm> iQueryable)
        {
            IQueryable<IncotermList> result = from incoterm in iQueryable
                                              select new IncotermList()
                                              {
                                                  Tenant = incoterm.Tenant,
                                                  Notes = incoterm.Notes,
                                                  OtherCharges = incoterm.OtherCharges,
                                                  Name = incoterm.Name,
                                                  LocalName = incoterm.LocalName,
                                                  InActive = incoterm.InActive,
                                                  Id = incoterm.Id,
                                                  Freight = incoterm.Freight,
                                                  Code = incoterm.Code,
                                                  AddedManually = incoterm.AddedManually,
                                                  SearchFields = incoterm.SearchFields,
                                              };
            return result;
        }

        public IncotermPM GetSinglePMByCode(string Code, int Tenant)
        {
            IncotermPM incoterm = (from a in repository.context.Incoterms
                                   where a.Tenant == Tenant && a.Code == Code
                                   select new IncotermPM()
                                   {
                                       AddedManually = a.AddedManually,
                                       Code = a.Code,
                                       Freight = a.Freight,
                                       Id = a.Id,
                                       InActive = a.InActive,
                                       LocalName = a.LocalName,
                                       Name = a.Name,
                                       OtherCharges = a.OtherCharges,
                                       Notes = a.Notes,
                                       Tenant = a.Tenant,
                                       ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.Name : a.LocalName,
                                       SearchFields = a.SearchFields,

                                   }).FirstOrDefault();
            return incoterm;
        }

        public IQueryable<IncotermList> GetTenantZeroIncoterms()
        {
            IQueryable<IncotermList> result = (from incoterm in repository.context.Incoterms
                                               where incoterm.Tenant == 0
                                               select new IncotermList()
                                               {
                                                   Tenant = incoterm.Tenant,
                                                   Notes = incoterm.Notes,
                                                   OtherCharges = incoterm.OtherCharges,
                                                   Name = incoterm.Name,
                                                   LocalName = incoterm.LocalName,
                                                   InActive = incoterm.InActive,
                                                   Id = incoterm.Id,
                                                   Freight = incoterm.Freight,
                                                   Code = incoterm.Code,
                                                   AddedManually = incoterm.AddedManually,
                                                   SearchFields = incoterm.SearchFields,
                                               });
            return result;
        }
    }
}