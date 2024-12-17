using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class PaymentTermDateTypeQuery
    {
        PaymentTermDateTypeRepository repository;

        public PaymentTermDateTypeQuery()
        {
            repository = new PaymentTermDateTypeRepository(); 
        }
        public PaymentTermDateTypeQuery(int tenant)
        {
            repository = new PaymentTermDateTypeRepository(tenant);
        }
        public PaymentTermDateTypeQuery(PaymentTermDateTypeRepository repository)
        {
            this.repository = repository;
        }

        public PaymentTermDateTypePM GetSinglePM(string code)
        {
            PaymentTermDateTypePM entityPM = null;
            PaymentTermDateType entityPOCO = repository.GetSinglePaymentTermDateType(code);

            if (entityPOCO != null)
            {
                entityPM = new PaymentTermDateTypePM()
                          {
                              Code = entityPOCO.Code,
                              Name = entityPOCO.Name,
                              SearchFields = entityPOCO.SearchFields,
                          };
            }

            return entityPM;
        }

        public IQueryable<PaymentTermDateTypePM> GetPaymentTermDateTypes()
        {
            IQueryable<PaymentTermDateTypePM> myResult = from a in repository.context.PaymentTermDateTypes
                                                         select new PaymentTermDateTypePM()
                                                     {
                                                         Code = a.Code,
                                                         Name = a.Name,
                                                         SearchFields = a.SearchFields,
                                                     };
            return myResult;
        }


        public IQueryable<PaymentTermDateTypeList> GetIQueryableEntityList(IQueryable<PaymentTermDateType> iQueryable)
        {
            var result = from a in iQueryable
                         select new PaymentTermDateTypeList()
                         {
                             Code = a.Code,
                             Name = a.Name,
                             SearchFields = a.SearchFields,
                         };

          
            return result;
        }
    }
}
