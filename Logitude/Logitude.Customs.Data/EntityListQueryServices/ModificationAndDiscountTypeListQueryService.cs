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

using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityLists;

namespace Logitude.Customs.Data.EntityListQueryServices
{

    public partial class ModificationAndDiscountTypeListQueryService
    {
        private IQueryable<ModificationAndDiscountTypeList> GetIqueryableList(IQueryable<ModificationAndDiscountType> iQueryable)
        {
            IQueryable<ModificationAndDiscountTypeList> query = (from a in iQueryable
                                                                 where a.Code != "67" 
                                                                 select new ModificationAndDiscountTypeList()
                                                                 {
                                                                     Code = a.Code,
                                                                     EnglishName = a.EnglishName,
                                                                     LocalName = a.LocalName,
                                                                     SearchFields = a.SearchFields,
                                                                     Inactive = a.Inactive,
                                                                     IsRelevantGoodsItem = a.IsRelevantGoodsItem,
                                                                     IsRelevantInvoice = a.IsRelevantInvoice,
                                                                     IsRelevantGoodsItemExport= a.IsRelevantGoodsItemExport,
                                                                     IsRelevantInvoiceExport= a.IsRelevantInvoiceExport,
                                                                     ExtraNumericData = 
                                                                        (a.ExtraNumericData == "1") ? "תוספת" :
                                                                        (a.ExtraNumericData == "2") ? "הפחתה" :
                                                                        (a.ExtraNumericData == "3") ? "ללא השפעה" : null,
                                                                     IsCustomsValueComponent = a.IsCustomsValueComponent,
                                                                     IsCustomsValueComponentExport = a.IsCustomsValueComponentExport,
                                                                     CurrencyMustBeSameAsInvoice = a.CurrencyMustBeSameAsInvoice,
                                                                     CurrencyMustSameInvoiceExport = a.CurrencyMustSameInvoiceExport,
                                                                     IsCustomUseExport = a.IsCustomUseExport,      


                                                                 });
            return query;
        }

        private IQueryable<ModificationAndDiscountType> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<ModificationAndDiscountType> iQueryable)
        {
            return iQueryable;
        }
    }


}
