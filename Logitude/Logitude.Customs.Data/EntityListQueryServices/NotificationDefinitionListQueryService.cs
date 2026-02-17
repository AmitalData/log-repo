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
using Logitude.Customs.Data.Repsitories;

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class NotificationDefinitionListQueryService
    {
        public int Tenant { get; set; }
        private IQueryable<NotificationDefinitionList> GetIqueryableList(IQueryable<NotificationDefinition> iQueryable)
        {

            NotificationTenantDefinitionRepository definitionRep = new NotificationTenantDefinitionRepository(context);
            Tenant = InjectionUtil.Instance.GetTenantFromToken();
            IQueryable<NotificationTenantDefinition> definitions = definitionRep.GetAll(Tenant);

            IQueryable<NotificationDefinitionList> query = (from a in iQueryable.Include("AssigneeNotificationType")

                                                            join d in definitions
                                                     on a.Code equals d.Code into xy
                                                            from s in xy.DefaultIfEmpty()
                                                            select new NotificationDefinitionList()
                                                            {
                                                                AssigneeNotificationTypeCode = a.AssigneeNotificationTypeCode,
                                                                AssigneeNotificationTypeName = a.AssigneeNotificationType != null ? a.AssigneeNotificationType.LocalName : null,
                                                                Code = a.Code,
                                                                DefaultAssigneeId = s.DefaultAssigneeId,
                                                                DefaultAssigneeName = s.User != null ? s.User.Contact.LocalName : null,
                                                                EnglishName = a.EnglishName,
                                                                LocalName = a.LocalName,
                                                                SearchFields = a.SearchFields,

                                                            });
            return query;
		}

		private IQueryable<NotificationDefinition> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<NotificationDefinition> iQueryable)
        {
            return iQueryable;
		}
	}


}
	