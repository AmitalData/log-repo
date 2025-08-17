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
    public class VatTypePercentageQuery
    {
        VatTypePercentageRepository repository;



        public VatTypePercentageQuery(int tenant)
        {
            repository = new VatTypePercentageRepository(tenant);
        }

        public VatTypePercentageQuery(VatTypePercentageRepository repository)
        {
            this.repository = repository;
        }

        public VatTypePercentagePM GetSingleVatTypePercentagePM(string id)
        {
            return (from a in repository.context.VatTypePercentages
                    where a.Id == id
                    select new VatTypePercentagePM()
                    {
                        FromDate = a.FromDate,
                        Id = a.Id,
                        Percentage = a.Percentage,
                        Tenant = a.Tenant,
                        VatTypeId = a.VatTypeId,
                    }).FirstOrDefault();
        }

        public IQueryable<VatTypePercentagePM> GetVatTypePercentagePMsByTenant(int tenant)
        {
            return from a in repository.context.VatTypePercentages
                   where a.Tenant == tenant
                   select new VatTypePercentagePM()
                   {
                       FromDate = a.FromDate,
                       Id = a.Id,
                       Percentage = a.Percentage,
                       Tenant = a.Tenant,
                       VatTypeId = a.VatTypeId,
                   };
        }

        public IQueryable<VatTypePercentagePM> GetVatTypePercentagesForVatType(int tenant, string vatTypeId)
        {
            return from a in repository.context.VatTypePercentages
                   where a.Tenant == tenant && a.VatTypeId == vatTypeId
                   select new VatTypePercentagePM()
                   {
                       FromDate = a.FromDate,
                       Id = a.Id,
                       Percentage = a.Percentage,
                       Tenant = a.Tenant,
                       VatTypeId = a.VatTypeId,
                   };
        }


        public VatTypePercentagePM GetVatTypePercentagesForVatTypeDate(int tenant, string vatTypeId, DateTime date)
        {
            VatTypePercentagePM rv = new VatTypePercentagePM();

            VatTypePercentage resultItem = this.repository.GetVatTypePercentageByDate(vatTypeId, tenant, date);
            if (resultItem != null)
            {
                rv= new VatTypePercentagePM()
                {
                    Id = resultItem.Id,
                    Tenant = resultItem.Tenant,
                    VatTypeId = resultItem.VatTypeId,
                    FromDate = resultItem.FromDate,
                    Percentage = resultItem.Percentage
                };
            }

            return rv;
        }

        public List<VatTypePercentagePM> GetVatTypePercentagePMByDate(int tenant, DateTime? date)
        {
            List<VatTypePercentagePM> myResult = new List<VatTypePercentagePM>();

            if(date != null)
            {
                date = date.Value.Date;
            }

            VatTypeRepository vatTypeRepository = new VatTypeRepository(this.repository.context);
            List<VatType> vatTypes = vatTypeRepository.GetVatTypes(tenant).ToList();

            foreach (VatType item in vatTypes)
            {
                VatTypePercentage resultItem = this.repository.GetVatTypePercentageByDate(item.Id, tenant, date);
                if (resultItem != null)
                {
                    myResult.Add(new VatTypePercentagePM()
                    {
                        Id = resultItem.Id,
                        Tenant = resultItem.Tenant,
                        VatTypeId = resultItem.VatTypeId,
                        FromDate = resultItem.FromDate,
                        Percentage = resultItem.Percentage
                    });
                }
            }

            return myResult;
        }
    }
}