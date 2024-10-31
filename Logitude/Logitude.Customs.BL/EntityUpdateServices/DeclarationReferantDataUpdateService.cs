using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Customs.Data;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class DeclarationReferantDataUpdateService
    {

        protected override void OnCreating(DeclarationReferantDataPM entityPM, EntityPM entityParentPM)
        {
            if(!string.IsNullOrEmpty(entityPM.DeclarationId)) entityPM.DeclarationIdToDisplay = entityPM.DeclarationId;
            base.OnCreating(entityPM, entityParentPM);
        }


        protected override void OnUpdating(DeclarationReferantDataPM entityPM)
        {
            if( entityPM != null && entityPM.IsCloseOrOpenFromUser)
            {
                if (entityPM.IsClosedForFollowUp == "1")
                    SendUnifreightEventParam(entityPM, "REFFC");
                if (entityPM.IsClosedForFollowUp == "0")
                    SendUnifreightEventParam(entityPM, "REFFO");
            }
            base.OnUpdating(entityPM);
        }
		protected override void OnUpdating(DeclarationReferantDataPM entityPM, DeclarationReferantData entityPOCO)
		{
            CreateEventsCustomShipment(entityPM, entityPOCO);
			base.OnUpdating(entityPM, entityPOCO);
		}


		private void SendUnifreightEventParam(DeclarationReferantDataPM declarationReferantDataPM,string code)
        {
            try
            {
                var declarationQueryService = new DeclarationQueryService(declarationReferantDataPM.Tenant);
                DeclarationPM connectedDeclarationPM = declarationQueryService.GetSingle(declarationReferantDataPM.DeclarationId, false, false);


                string loggedContactId = null;
                ContactRepository contactRepository = new ContactRepository(Tenant);
                var loggedContact = contactRepository.GetSingleContactByEmail(AuthenticationUtil.ResolveLoggingUserId(Tenant), Tenant);
                if (loggedContact != null)
                {
                    loggedContactId = loggedContact.Id;
                }

                var MyUnifreightEventParam = new UnifreightEventParam()
                {
                    Code = code,
                    Mode = UnifreightEventMode.@new,
                    EventDateTime = DateTime.Now,
                    Entname = "CFIFILEM",
                    PrimaryNum = connectedDeclarationPM.CustomFileNo,
                    EventRemarks = "",
                };
                LogMessagingUtil.Instance.AppendLine("MyUnifreightEventParam = " + MyUnifreightEventParam ?? "NULL");
                var myOpenUnifreighTask = new UnifreightEventTaskService();
                myOpenUnifreighTask.UpsertEventLE2U(
                    declarationReferantDataPM.Tenant,
                   loggedContactId,
                    MyUnifreightEventParam);

            }
            catch (System.Exception)
            {
                // TODO: BL Stop Execute or Cuntinue - Ask IHAB
                throw;
            }
        }

		public void CreateEventsCustomShipment(DeclarationReferantDataPM entityPM, DeclarationReferantData entityPOCO)
		{
			DeclarationRepository declarationRepository = new DeclarationRepository(Tenant);
			Declaration declaration = declarationRepository.GetSingle(entityPM.DeclarationId, entityPM.Tenant);
			if(declaration.SystemConnection != "N")
			{
				return;
			}
			string loggedContactId = null;
			ContactRepository contactRepository = new ContactRepository(Tenant);
			var loggedContact = contactRepository.GetSingleContactByEmail(AuthenticationUtil.ResolveLoggingUserId(Tenant), Tenant);
			if (loggedContact != null)
			{
				loggedContactId = loggedContact.Id;
			}
			if (entityPM.EstimatedArrivalDate != null && entityPM.EstimatedArrivalDate != entityPOCO.EstimatedArrivalDate)
			{
				EventTracer.DeleteTraceEvent("ETA", entityPM.Tenant, "Shipment", declaration.ShipmentId);
				EventTracerArgs eventTracerArgs = new EventTracerArgs()
				{
					EntityId = declaration.ShipmentId,
					Tenant = entityPM.Tenant,
					UserId = loggedContact.Id,
					ObjectTableName = "Shipment",
					IsAddedManually = false,
					EventTypeCode = "ETA",
					EventDateTime = new DateTime(entityPM.EstimatedArrivalDate.Value.Year, entityPM.EstimatedArrivalDate.Value.Month, entityPM.EstimatedArrivalDate.Value.Day, 0, 0, 0)
				};
				EventTracer.CreateTraceEvent(eventTracerArgs);
			}
            if(entityPOCO.EstimatedArrivalDate != null && entityPM.EstimatedArrivalDate == null)
			{
				EventTracer.DeleteTraceEvent("ETA", entityPM.Tenant, "Shipment", declaration.ShipmentId);
			}

			if (entityPM.ArrivalDate != null && entityPM.ArrivalDate != entityPOCO.ArrivalDate)
			{
				EventTracer.DeleteTraceEvent("ARR", entityPM.Tenant, "Shipment", declaration.ShipmentId);
				EventTracerArgs eventTracerArgs = new EventTracerArgs()
				{
					EntityId = declaration.ShipmentId,
					Tenant = entityPM.Tenant,
					UserId = loggedContact.Id,
					ObjectTableName = "Shipment",
					IsAddedManually = false,
					EventTypeCode = "ARR",
					EventDateTime = new DateTime(entityPM.ArrivalDate.Value.Year, entityPM.ArrivalDate.Value.Month, entityPM.ArrivalDate.Value.Day, 0, 0, 0)
				};
				EventTracer.CreateTraceEvent(eventTracerArgs);
			}
			if (entityPOCO.ArrivalDate != null && entityPM.ArrivalDate == null)
			{
				EventTracer.DeleteTraceEvent("ARR", entityPM.Tenant, "Shipment", declaration.ShipmentId);
			}
		}

	}
}
