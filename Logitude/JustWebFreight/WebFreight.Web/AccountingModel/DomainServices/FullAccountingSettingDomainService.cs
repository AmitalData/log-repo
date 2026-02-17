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
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Interfaces;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using System.ServiceModel.DomainServices.Hosting;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data;
using Logitude.Accounting.BL;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.BL.EntityQueryServices;

namespace WebFreight.Web.AccountingModel.DomainServices
{
    [EnableClientAccess()]
    public partial class FullAccountingSettingDomainService : LogitudeDomainService
    {
        IDomainServiceUpdateClass<FullAccountingSettingPM> service;
        IAccountingContext MyContext;
        public FullAccountingSettingDomainService()
        {
            //service = ContainerAccessor.Container.Resolve(typeof(IDomainServiceUpdateClass<FullAccountingSettingPM>), "AccountingDomainServiceUpdateClass", new ParameterOverride("", 1)) as IDomainServiceUpdateClass<FullAccountingSettingPM>;
        }

        public FullAccountingSettingPM GetSingleFullAccountingSettingPM(string id, int tenant)
        {
            if (MyContext == null)
            {
                MyContext = AccountingContext.GetContext(tenant);
            }

            FullAccountingSettingQueryService fullAccountingSettingQuery = new FullAccountingSettingQueryService(MyContext);
            FullAccountingSettingPM fullAccountingSettingPM = fullAccountingSettingQuery.GetSingle(id, false, false);
            return fullAccountingSettingPM;

        }


        public FullAccountingSettingList GetSingleFullAccountingSettingList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("FullAccountingSetting", "READ", tenant);

            if (MyContext == null)
            {
                MyContext = AccountingContext.GetContext(tenant);
            }


            FullAccountingSettingListQueryService listService = new FullAccountingSettingListQueryService(MyContext);
            return listService.GetSingle(id);
        }

        public List<FullAccountingSettingList> GetFullAccountingSettingLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("FullAccountingSetting", "READ", tenant);
            if (MyContext == null)
            {
                MyContext = AccountingContext.GetContext(tenant);
            }
            FullAccountingSettingListQueryService listService = new FullAccountingSettingListQueryService(MyContext);
            return listService.GetList(tenant);
        }

        public List<FullAccountingSettingList> GetFullAccountingSettingsFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("FullAccountingSetting", "READ", tenant);

            if (MyContext == null)
            {
                MyContext = AccountingContext.GetContext(tenant);
            };
            FullAccountingSettingListQueryService listService = new FullAccountingSettingListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetFullAccountingSettingFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("FullAccountingSetting", "READ", tenant);
            if (MyContext == null)
            {
                MyContext = AccountingContext.GetContext(tenant);
            };
            FullAccountingSettingListQueryService queryService = new FullAccountingSettingListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }

        public void InsertFullAccountingSetting(FullAccountingSettingPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("FullAccountingSetting", "NEW", entityPM.Tenant);
            if (MyContext == null)
            {
                MyContext = AccountingContext.GetContext(entityPM.Tenant);
            };
            FullAccountingSettingUpdateService service = new FullAccountingSettingUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            service.Update(entityPM, true);

            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("FullAccountingSetting", 0, true);
            string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            if (loggedContact != null)
            {
                ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id);
            }
        }


        public void UpdateFullAccountingSetting(FullAccountingSettingPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("FullAccountingSetting", "UPDATE", entityPM.Tenant);
            if (MyContext == null)
            {
                MyContext = AccountingContext.GetContext(entityPM.Tenant);
            };
            FullAccountingSettingUpdateService service = new FullAccountingSettingUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

            service.Update(entityPM, true);

            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("FullAccountingSetting", 0, true);
            string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            if (loggedContact != null)
            {
                ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id);
            }
        }




    }
}