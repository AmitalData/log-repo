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
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class CustomsSettingListQueryService
    {
	    private IQueryable<CustomsSettingList> GetIqueryableList(IQueryable<CustomsSetting> iQueryable)
        {
            IQueryable<CustomsSettingList> query = (from a in iQueryable.Include("User").Include("User.Contact").Include("CustomsEnvoirmentType")
                                                    select new CustomsSettingList()
                                                    {
                                                       Id = a.Id,
                                                       CustomsAgentId = a.CustomsAgentId,
                                                       DCAServiceAddress = a.DCAServiceAddress,
                                                       IIGServiceAddress = a.IIGServiceAddress,
                                                       SignServiceAddress = a.SignServiceAddress,
                                                       IsConnectedToUniFreight = a.IsConnectedToUniFreight,
                                                       Tenant = a.Tenant,
                                                       UServerServiceAddress = a.UServerServiceAddress,
                                                      // DefaultNotificationAssignee = a.DefaultNotificationAssignee,
                                                       DefaultNotificationAssigneeName = a.User != null ? a.User.Contact.LocalName : null,
                                                       DCAPartnerVault = a.DCAPartnerVault,
                                                       SearchFields = a.SearchFields,
                                                       CustomsEnvoirmentTypeCode = a.CustomsEnvoirmentTypeCode,
                                                       PaymentOrderAccCard = a.PaymentOrderAccCard,
                                                       CustomsEnvoirmentTypeName = a.CustomsEnvoirmentType != null? a.CustomsEnvoirmentType.LocalName : null,
                                                       UnifreightCertificateActivated = a.UnifreightCertificateActivated,
                                                       AutoFillAccountType = a.AutoFillAccountType,
                                                       AutoUnitMeasurement = a.AutoUnitMeasurement,
                                                    });
            return query;
		}

        private IQueryable<CustomsSetting> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<CustomsSetting> iQueryable, int tenant)
        {
            return iQueryable;
		}
	}


}
	