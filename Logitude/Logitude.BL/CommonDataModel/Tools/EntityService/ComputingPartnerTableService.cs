using System;
using System.Linq;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.Security;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class ComputingPartnerTableService
    {
        bool isNewEntity;
        private int tenant;
        private string loggedContactId;
        public ComputingPartnerTable Poco { get; set; }
        private ComputingPartnerTablePM entityPM;
        private ICommonDataContext objectContext;
        private ComputingPartnerTableRepository entityRepository;
        public ComputingPartnerTableService(ICommonDataContext objectContext, ComputingPartnerTablePM entityPM)
        {
            this.tenant = entityPM.Tenant;
            this.entityPM = entityPM;
            this.objectContext = objectContext;
            this.entityRepository = new ComputingPartnerTableRepository(objectContext);
            this.GetLoggedContact();
        }

        private void GetLoggedContact()
        {
            ContactRepository contactRepository = new ContactRepository(tenant);

            string email = HttpContext.Current.User.Identity.Name;
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);

            if (loggedContact == null)
            {
                loggedContact = contactRepository.GetSingleContactByEmail("system@tenant" + tenant + ".com", tenant);
                this.loggedContactId = loggedContact.Id;
            }

            else
            {
                if (loggedContact.Tenant != tenant)
                {
                    loggedContact = contactRepository.GetSingleContactByEmail("system@tenant" + tenant + ".com", tenant);
                    this.loggedContactId = loggedContact.Id;
                }
                else
                {
                    this.loggedContactId = loggedContact.Id;
                }
            }

        }

        public void Create()
        {
            this.isNewEntity = true;

            bool exist = this.IsEntityExists();

            if (exist)
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyexists", tenant);
                msg = msg.Replace("%Entity", "Computing Partner Table: " + entityPM.Name);
                throw new ApplicationException(msg);
            }

            else
            {
                DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);

                //this.entityPM.Id = IdCounter.GetNumber("ComputingPartner", tenant).ToString();
                this.entityPM.CreateDate = entityPM.UpdateDate = todayDateTime;
                this.entityPM.CreatedByUserId = entityPM.UpdatedByUserId = this.loggedContactId;

                this.Poco = new ComputingPartnerTable()
                {
                    Tenant = entityPM.Tenant,
                    ObjectTableId = entityPM.ObjectTableId,
                    ComputingPartnerId = entityPM.ComputingPartnerId,
                };

                ComputingPartnerTableMapping.MapEntity(entityPM, Poco, isNewEntity);

                entityRepository.Add(Poco);
                entityRepository.SubmitChanges();
            }
        }

        public void Update()
        {
            this.isNewEntity = false;

            bool exist = this.IsEntityExists();

            if (exist)
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyexists", tenant);
                msg = msg.Replace("%Entity", "Computing Partner Table: " + entityPM.Name);
                throw new ApplicationException(msg);
            }

            else
            {
                DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);

                this.entityPM.UpdateDate = todayDateTime;
                this.entityPM.UpdatedByUserId = this.loggedContactId;

                this.Poco = entityRepository.GetSingleComputingPartnerTable(tenant, entityPM.ObjectTableId, entityPM.ComputingPartnerId);

                ComputingPartnerTableMapping.MapEntity(entityPM, Poco, isNewEntity);

                entityRepository.Update(Poco);
                entityRepository.SubmitChanges();
            }
        }

        private bool IsEntityExists()
        {
            bool myResult = false;

            if (isNewEntity)
            {
                myResult = (from a in entityRepository.GetComputingPartnerTablesByPartnerId(tenant, entityPM.ComputingPartnerId)
                            where a.Name == entityPM.Name
                            select a).Any();
            }

            else
            {
                myResult = (from a in entityRepository.GetComputingPartnerTablesByPartnerId(tenant, entityPM.ComputingPartnerId)
                            where a.Name == entityPM.Name && a.ComputingPartnerId != entityPM.ComputingPartnerId
                            select a).Any();
            }

            return myResult;
        }
    }
}
