
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Server.Infrastructure;
using Logitude.Accounting.BL.EntityQueryServices;
using System.Web;
using Logitude.BL.Interfaces;
using Microsoft.Practices.Unity;
using Logitude.BL.Helpers;
using Logitude.BL.Resolvers;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class ReconcileExternalPageDataMapping: IMapping<ReconcileExternalPagePM, ReconcileExternalPage>
   {

        public void CustomPMToPOCO(ReconcileExternalPagePM entityPM, ReconcileExternalPage entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }

            //entityPOCO.SearchFields = "";// entityPM.PageNo; //filled by generated way
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
            entityPOCO.SearchFields = entityPM.SearchFields;
        }

        public void CustomPOCOToPM(ReconcileExternalPagePM entityPM, ReconcileExternalPage entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.CreatedByUserName);
            CustomMappedPMProperties.Add(PMPropertyNames.StatusName);

            // Get user
            if (entityPOCO.CreatedByUserId != null)
            {
                ContactQuery query = new ContactQuery(entityPOCO.Tenant);
                ContactPM contact = query.GetSinglePM(entityPOCO.CreatedByUserId, entityPOCO.Tenant);
                if (contact != null)
                {
                    entityPM.CreatedByUserName = contact.LocalName;
                }
            }

            if (entityPOCO.StatusCode != null)
            {
                ReconcileExternalPageStatusQueryService statusQuery = new ReconcileExternalPageStatusQueryService(entityPOCO.Tenant);
                ReconcileExternalPageStatusPM status = statusQuery.GetSingle(entityPOCO.StatusCode, false, false);

                ContactPM user = GetLoggedContact(entityPOCO.Tenant);


                if (status != null)
                {
                    entityPM.StatusName = user.DontShowLocal ? status.EnglishName : status.LocalName;
                }
            }

            if (entityPOCO.EntryTypeCode != null)
            {
                BankPageEntryTypeQueryService query = new BankPageEntryTypeQueryService(entityPOCO.Tenant);
                BankPageEntryTypePM type = query.GetSingle(entityPOCO.EntryTypeCode, false, false);

                if (type != null)
                {
                    entityPM.EntryTypeEnglishName = type.EnglishName;
                    entityPM.EntryTypeLocalName = type.LocalName;
                }
            }


            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
            entityPOCO.SearchFields = entityPM.SearchFields;


        }


        private ContactPM GetLoggedContact(int tenant)
        {
            //ILoggedContactUtil loggedContactUtil = ContainerAccessor.Container.Resolve(typeof(ILoggedContactUtil), "LoggedContactUtil", new ParameterOverride("", tenant)) as ILoggedContactUtil;
            //ContactPM loggedcontact = loggedContactUtil.GetLoggedContact(tenant);

            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }


        private static void BuildSearchFields(ReconcileExternalPagePM entityPM, ReconcileExternalPage poco, bool isNewEntity)
        {
            string searchText = "";

            searchText = entityPM.PageNo.ToString();

            entityPM.SearchFields = searchText;
            poco.SearchFields = searchText;

        }

    }


}
   