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
using Logitude.BL.QuoteModel.EntityLists;
using Logitude.BL.QuoteModel.EntityQueries;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class ProductTypeModificationQuery
    {
        ProductTypeModificationRepository repository;



        public ProductTypeModificationQuery(int tenant)
        {
            repository = new ProductTypeModificationRepository(tenant);
        }

        public ProductTypeModificationQuery(ProductTypeModificationRepository repository)
        {
            this.repository = repository;
        }

        public ProductTypeModificationPM GetSinglePM(string code, int tenant)
        {
            ProductTypeModification entityPoco = repository.GetSingleProductTypeModification(code,tenant);
            if (entityPoco != null)
            {
                ProductTypeModificationPM entityPM = new ProductTypeModificationPM()
                {
                    ProductTypeCode = entityPoco.ProductTypeCode,
                    InActive = entityPoco.InActive,
                    Tenant = tenant,
                    QuotationDefaultTemplateId = entityPoco.QuotationDefaultTemplateId,
                    RoutingRQuoteDefaultTemplateId = entityPoco.RoutingRQuoteDefaultTemplateId,
                    CostTariffUse = entityPoco.CostTariffUse,
                    SaleTariffUse = entityPoco.SaleTariffUse,
                };

                ProductTypeModificationRepository modificationRep = new ProductTypeModificationRepository(tenant);
                ProductTypeModification modification = modificationRep.GetSingleProductTypeModification(code, tenant);

                entityPM.InActive = modification != null ? modification.InActive : false;
                entityPM.QuotationDefaultTemplateId = modification != null ? modification.QuotationDefaultTemplateId : null;
                entityPM.RoutingRQuoteDefaultTemplateId = modification != null ? modification.RoutingRQuoteDefaultTemplateId : null;
                return entityPM;
            }
            return null;
        }
    }
}
