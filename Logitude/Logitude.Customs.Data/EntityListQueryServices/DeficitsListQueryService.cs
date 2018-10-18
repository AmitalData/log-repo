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

    public partial class DeficitListQueryService
    {
	    private IQueryable<DeficitList> GetIqueryableList(IQueryable<Deficit> iQueryable)
        {
            IQueryable<DeficitList> query = (from a in iQueryable.Include("DebtNotificationType")
                                             select new DeficitList()
                                                    {
                                                        DebtNotificationNumber = a.DebtNotificationNumber,
                                                        DebtNotificationReason = a.DebtNotificationReason,
                                                        Id = a.Id,
                                                        NotificationTypeCode = a.NotificationTypeCode,
                                                        NotificationTypeName = a.DebtNotificationType.LocalName != null ? a.DebtNotificationType.LocalName : a.DebtNotificationType.EnglishName,
                                                        ProductionDate = a.ProductionDate,
                                                        RealesGoodsDescription = a.RealesGoodsDescription,
                                                        Tenant = a.Tenant,
                                                        ValidityDateTo = a.ValidityDateTo,
                                                       

                                                    });
            return query;
		}

        private IQueryable<Deficit> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<Deficit> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	