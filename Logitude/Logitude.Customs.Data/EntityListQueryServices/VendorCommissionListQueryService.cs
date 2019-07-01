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

    public partial class VendorCommissionListQueryService
    {
        private IQueryable<VendorCommissionList> GetIqueryableList(IQueryable<VendorCommission> iQueryable)
        {
            IQueryable<VendorCommissionList> query = (from a in iQueryable
                                                      select new VendorCommissionList()
                                                      {

                                                          VendorId = a.VendorId,
                                                          CustomerId = a.CustomerId,
                                                          //ModificationsTypeCode = a.ModificationsTypeCode,
                                                          //ModificationsTypeName = a.ModificationAndDiscountType != null ? a.ModificationAndDiscountType.LocalName : null,
                                                          CommisionPercentage = a.CommisionPercentage,
                                                          Tenant = a.Tenant,
                                                      });
            return query;
        }

        private IQueryable<VendorCommission> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<VendorCommission> iQueryable, int tenant)
        {
            return iQueryable;

            //throw new NotImplementedException();
        }

        public List<VendorCommissionList> GetCommissionsForCustomer(string customerId, int tenant)
        {
            return (from a in context.VendorCommissions.Where(d => d.CustomerId == customerId && d.Tenant == tenant)
                    select new VendorCommissionList()
                    {
                        VendorId = a.VendorId,
                        CustomerId = a.CustomerId,
                        ModificationsTypeCode = a.ModificationsTypeCode,
                        CommisionPercentage = a.CommisionPercentage,
                        ModificationsTypeName = a.ModificationAndDiscountType != null ? (a.ModificationAndDiscountType.LocalName != null ? a.ModificationAndDiscountType.LocalName : a.ModificationAndDiscountType.EnglishName) : null,
                    }).ToList();
        }

    }


}
	