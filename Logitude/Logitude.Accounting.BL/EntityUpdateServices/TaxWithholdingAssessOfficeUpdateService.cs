using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
   public partial class TaxWithholdingAssessOfficeUpdateService
    {

        protected override void OnCreating(TaxWithholdingAssessOfficePM entityPM, EntityPM entityParentPM)
        {

            entityPM.Id = IdCounter.GetNumber("TaxWithholdingAssessOffice", entityPM.Tenant);
          
        }

        internal void ValidateEntity(TaxWithholdingAssessOfficePM entityPM)
        {
            IAccountingContext context = MainContext as AccountingContext;
            TaxWithholdingAssessOfficeQueryService service = new TaxWithholdingAssessOfficeQueryService(context);
            bool exist = service.CheckIfTaxOfficeExists(entityPM.Code, entityPM.Tenant);
            if (exist)
            {
                throw new ApplicationException("already exist");
             //   throw new ApplicationException(TranslateTextsClass.Translate("Accounting.General.O.PaymentChequeExist", entityPM.Tenant, true));
            }

        }

        protected override void Trace(TaxWithholdingAssessOfficePM entityPM, TaxWithholdingAssessOffice entityPOCO, string changesXml)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);
            ContactRepository contactRep = new ContactRepository(commonContext);
            Contact contact = contactRep.GetSingleContactByEmail(AuthenticationUtil.GetAuthenticatedUser(), entityPM.Tenant);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPEV",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "TaxWithholdingAssessOffice",
                    Notes = changesXml
                });

              
            }
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CREV",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "TaxWithholdingAssessOffice",
                    Notes = changesXml
                });

               
            }




        }

    }
}
