using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Models;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityLists;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class ClaimUpdateService
    {
        protected override void OnCreating(ClaimPM entityPM, EntityPM entityParentPM)
        {
            ICustomContext context = MainContext as CustomContext;
            TapagQueryService tapagQueryService = new TapagQueryService(context);

            if (entityPM.CreateDate == null)
            {
                entityPM.CreateDate = DateTime.Now;
            }
            
            TapagPM tapag = new TapagPM()
            {
                ChangeSetOp = ChangeSetOperation.Insert,
                Tenant = entityPM.Tenant,
                CustomerId = entityPM.CustomerId,
                CustomsBranchCode = entityPM.CustomsBranchCode,
                ImporterId = entityPM.ImporterId,
                FollowDate = entityPM.FollowDate,
                IsClosed = false,
                LeadingFileNumber = entityPM.LeadingFileNumber,
                //ProfessionUnitTypeCode = entityPM.ProfessionUnitTypeCode,
                //SpecializationTypeCode = entityPM.SpecializationTypeCode,
                TapagNumber = entityPM.TapagNumber,
                TapagTypeCode = "3",
                ValidityDate = entityPM.ValidityDate,
                CreateDate = entityPM.CreateDate,
                ReferantId = entityPM.ReferantId,
            };

            TapagUpdateService tapagUpdate = new TapagUpdateService(context, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), entityPM.Tenant);
            tapagUpdate.Update(tapag, false);

            entityPM.ImporterClaimTypeCode = "7";
            entityPM.ClaimSubmiterTypeCode = "3";
            entityPM.SubmitDate = DateTime.Now;
            entityPM.TapagId = tapag.Id;
            entityPM.TapagNumber = tapag.TapagNumber;
            entityPM.Id = tapag.Id;

            if (!string.IsNullOrEmpty(entityPM.CustomerId) && string.IsNullOrEmpty(entityPM.ClientId))
            {
                CardRepository cardRep = new CardRepository(entityPM.Tenant);
                Card card = cardRep.GetSingleCard(entityPM.CustomerId, entityPM.Tenant);
                if (card != null)
                {
                    ClientQueryService clientQueryService = new ClientQueryService(entityPM.Tenant);
                    entityPM.ClientId = clientQueryService.GetIdByCode(card.VatNumber, entityPM.Tenant);
                }
            }

            var setting = CustomsSettingQueryService.GetSettingByTenant(entityPM.Tenant);
            if (setting != null)
            {
                entityPM.ClaimSubmiterNumber = setting.CustomsAgentId.Length <= 9 ? setting.CustomsAgentId : null;
                if (!string.IsNullOrEmpty(entityPM.ClaimSubmiterNumber))
                {
                    ClientQueryService clientQueryService = new ClientQueryService(entityPM.Tenant);
                    ClientPM clientPM = clientQueryService.GetClientByCode(entityPM.ClaimSubmiterNumber, entityPM.Tenant);
                    if (clientPM != null && !string.IsNullOrWhiteSpace(clientPM.LocalCorporationName))
                    {
                        entityPM.HebrewCorporationName = clientPM.LocalCorporationName;
                    }
                }

            }

            var myUpdateEventContextTagModel = new EventContextTagModel() // Create Event OPN - “Claim Opened”
            {
                CallProccessID = EventContextTagModel.ProccessEnum.CreateNewClaim,
                EventCode = "OPNC",
                EventRemarks = "Claim Opened",
                FUStatusRemarks = "Claim Opened",
            };
            entityPM.CurrentContextTag = myUpdateEventContextTagModel;

            base.OnCreating(entityPM, entityParentPM);
        }
        
        protected override void UpdateComposition(ClaimPM entityPM)
        {
            ClaimImporterDeclarsPage3UpdateService claimImporterDeclarsPage3UpdateService = new ClaimImporterDeclarsPage3UpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            claimImporterDeclarsPage3UpdateService.UpdateMulti(entityPM.ClaimImporterDeclarsPage3, entityPM.DeletedClaimImporterDeclarsPage3, entityPM, false);
            
            ClaimsRelatedEntityUpdateService claimsRelatedEntityUpdateService = new ClaimsRelatedEntityUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            claimsRelatedEntityUpdateService.UpdateMulti(entityPM.ClaimsRelatedEntities, entityPM.DeletedClaimsRelatedEntities, entityPM, false);

            var claimImporterDeclarsPage3AUpdateService = new ClaimImporterDeclarsPage3AUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            claimImporterDeclarsPage3AUpdateService.UpdateMulti(entityPM.ClaimImporterDeclarsPage3A, entityPM.DeletedClaimImporterDeclarsPage3A, entityPM, false);

            ClaimImporterDeclarsPage3BUpdateService claimImporterDeclarsPage3BUpdateService = new ClaimImporterDeclarsPage3BUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            claimImporterDeclarsPage3BUpdateService.UpdateMulti(entityPM.ClaimImporterDeclarsPage3B, entityPM.DeletedClaimImporterDeclarsPage3B, entityPM, false);

            OnUpdatingTapag(entityPM);

            base.UpdateComposition(entityPM);
        }

        private void OnUpdatingTapag(ClaimPM entityPM)
        {
            ICustomContext context = MainContext as CustomContext;
            var tapagQueryService = new TapagQueryService(context);
            var tapagUpdateService = new TapagUpdateService(context, new Dictionary<string, IContext>(), entityPM.Tenant);

            TapagPM tapagPM = tapagQueryService.GetSingle(entityPM.Id, true, false);

            if (tapagPM != null)
            {
                if (tapagPM.IsClosed != entityPM.IsClosed)
                {
                    var myUpdateEventContextTagModel = new EventContextTagModel();
                    var statuseCode = "";
                    if (entityPM.IsClosed == true && tapagPM.IsClosed == false) // Close claim
                    {
                        statuseCode = "CLSC";
                        myUpdateEventContextTagModel.CallProccessID = EventContextTagModel.ProccessEnum.SentClaimToCustoms;
                        myUpdateEventContextTagModel.EventCode = "CLSC";
                        myUpdateEventContextTagModel.EventRemarks = "Claim Closed";
                        myUpdateEventContextTagModel.FUStatusRemarks = "Claim Closed";
                    }
                    else // Claim Re-Opened
                    {
                        statuseCode = "ROPC";
                        myUpdateEventContextTagModel.CallProccessID = EventContextTagModel.ProccessEnum.SentClaimToCustoms;
                        myUpdateEventContextTagModel.EventCode = "ROPC";
                        myUpdateEventContextTagModel.EventRemarks = "Claim Re-Opened";
                        myUpdateEventContextTagModel.FUStatusRemarks = "Claim Re-Opened";
                    }
                    entityPM.CurrentContextTag = myUpdateEventContextTagModel;
                    RaiseClaimEventAndStatus(statuseCode, statuseCode, entityPM, null, true);
                }

                tapagPM.ChangeSetOp = ChangeSetOperation.Update;
                tapagPM.CustomerId = entityPM.CustomerId;
                tapagPM.ReferantId = entityPM.ReferantId;
                tapagPM.IsClosed = entityPM.IsClosed;
                // tapagPM.CustomsBranchCode = entityPM.CustomsBranchCode;
                tapagUpdateService.Update(tapagPM, false);
            }
        }

        protected override void OnUpdating(ClaimPM entityPM)
        {
            // Update CustomsFiles Fiels List
            if (entityPM.ClaimsRelatedEntities != null && entityPM.ClaimsRelatedEntities.Count() > 0)
            {
                entityPM.CustomsFiles = "";

                foreach (var relatedEntitiyItem in entityPM.ClaimsRelatedEntities)
                {
                    if (relatedEntitiyItem.ClaimEntityTypeCode == "1055" && !string.IsNullOrWhiteSpace(relatedEntitiyItem.ExternalClaimNumber))
                    {
                        if (string.IsNullOrWhiteSpace(entityPM.CustomsFiles))
                        {
                            entityPM.CustomsFiles = relatedEntitiyItem.ExternalClaimNumber;
                        }
                        else
                        {
                            entityPM.CustomsFiles = string.Concat(entityPM.CustomsFiles, " *");
                            break;
                        }
                    }
                }
            }

          
            UpdateUnifreight(entityPM);
           
        }

        protected override void AfterUpdating(ClaimPM entityPM, EntityPM entityParentPM)
        {
            if (!string.IsNullOrWhiteSpace(entityPM.CustomerId))
            {
                CardRepository rep = new CardRepository(entityPM.Tenant);
                Card customerCard = rep.GetSingleCard(entityPM.CustomerId, entityPM.Tenant);
                if (customerCard != null)
                {
                    entityPM.CustomerName = customerCard.LocalName != null ? customerCard.LocalName : customerCard.EnglishName;
                }
            }
            else
            {
                entityPM.CustomerName = "";
            }

            if (!string.IsNullOrWhiteSpace(entityPM.ReferantId))
            {
                UserRepository rep = new UserRepository(entityPM.Tenant);
                User referantUser = rep.GetSingleUser(entityPM.ReferantId, entityPM.Tenant);
                if (referantUser != null)
                {
                    entityPM.ReferantName = referantUser.Contact.LocalName != null ? referantUser.Contact.LocalName : referantUser.Contact.EnglishName;
                }
            }
            else
            {
                entityPM.ReferantName = "";
            }
        }

    }
}
