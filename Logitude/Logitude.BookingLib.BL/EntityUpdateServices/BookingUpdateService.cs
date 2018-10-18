using Logitude.BookingLib.BL.EntityPMs;
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.Server.Tools.Counters;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Transactions;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BookingLib.Data.Repositories;
using System.ServiceModel.DomainServices.Server;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BookingLib.BL.Helpers;
using System.Data.Entity.Core;
using Logitude.BookingLib.BL.Validators;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Web;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;

namespace Logitude.BookingLib.BL.EntityUpdateServices
{
    public partial class BookingUpdateService
    {
        protected override void OnCreating(BookingPM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.Id = IdCounter.GetNumber("Booking", entityPM.Tenant);
                entityPM.BookingNumber = CodeCounter.GetNumber("Booking", entityPM.Tenant).ToString();

                DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

                entityPM.CreateDate = todayDateTime;
                entityPM.UpdateDate = todayDateTime;

                if (string.IsNullOrEmpty(entityPM.BookingStatusCode))
                {
                    entityPM.BookingStatusCode = "CRT";
                }

                if (string.IsNullOrEmpty(entityPM.SpaceAllocationCode))
                {
                    entityPM.SpaceAllocationCode = "NN";
                }

                if (!string.IsNullOrEmpty(entityPM.Transshipment1FromPortId))
                {
                    if (string.IsNullOrEmpty(entityPM.Transshipment1SpaceAllocationCode))
                    {
                        entityPM.Transshipment1SpaceAllocationCode = "NN";
                    }
                }

                if (!string.IsNullOrEmpty(entityPM.Transshipment2FromPortId))
                {
                    if (string.IsNullOrEmpty(entityPM.Transshipment2SpaceAllocationCode))
                    {
                        entityPM.Transshipment2SpaceAllocationCode = "NN";
                    }
                }

                string email = HttpContext.Current.User.Identity.Name;
                ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
                Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
                if (loggedContact != null)
                {
                    entityPM.CreatedByUserId = loggedContact.Id;
                    entityPM.UpdatedByUserId = loggedContact.Id;
                }

                ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
                ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("Booking", 0, true);
                ActivityLogger.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "N", entityPM.CreatedByUserId);
            }
        }

        protected override void OnUpdating(BookingPM entityPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

                string email = "";
                if (entityPM.IsUpdatedByChampAnalyzer)
                {
                    email = "system@tenant" + entityPM.Tenant + ".com";
                }

                else
                {
                    email = HttpContext.Current.User.Identity.Name;                    
                }

                ContactQuery contactQuery = new ContactQuery(entityPM.Tenant);
                ContactPM loggedContact = contactQuery.GetContactByNameAndTenant(email, entityPM.Tenant, true);

                if (loggedContact == null)
                {
                    loggedContact = contactQuery.GetContactByEmailOnly(email, entityPM.Tenant);
                }

                if (loggedContact != null)
                {
                    entityPM.UpdatedByUserId = loggedContact.Id;
                }

                if (entityPM.IsUpdatedByChampAnalyzer)
                {
                    entityPM.UpdatedByPartner = "Airline Transmission";
                }

                else if (loggedContact != null)
                {
                    entityPM.UpdatedByPartner = loggedContact.EnglishName;
                }

                ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
                ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("Booking", 0, true);
                ActivityLogger.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", entityPM.UpdatedByUserId);
            }
        }

        protected override void OnUpdating(BookingPM entityPM, Booking entityPOCO)
        {
            this.InitializeMAWBStack(entityPM, entityPOCO);

            entityPM.GrossWeightInKG = GetWeightInKG(entityPM.GrossWeightUnitCode, entityPM.GrossWeight);
            entityPM.ChargeableWeightInKG = GetWeightInKG(entityPM.ChargeableWeightUnitCode, entityPM.ChargeableWeight);

            this.InitializeCarrierPrefix(entityPM);
            this.ValidateAirlineRestriction(entityPM);
            this.ValidateMasterNumber(entityPM);
        }

        private void InitializeCarrierPrefix(BookingPM entityPM)
        {
            if (entityPM.TransportModeCode == "A")
            {
                if (!string.IsNullOrEmpty(entityPM.InterlineId))
                {
                    //AirlineRepository airlineRepository = new AirlineRepository(entityPM.Tenant);
                    //Airline myAirline = airlineRepository.GetSingleAirline(entityPM.InterlineId, entityPM.Tenant);
                    //if (myAirline != null)
                    //{
                    //    if (!string.IsNullOrEmpty(myAirline.Prefix))
                    //    {
                    //        entityPM.AirlinePrefix = myAirline.Prefix.PadLeft(3, '0');
                    //    }
                    //}
                }

                else if (!string.IsNullOrEmpty(entityPM.MainCarriageCarrierId))
                {
                    AirlineRepository airlineRepository = new AirlineRepository(entityPM.Tenant);
                    Airline myAirline = airlineRepository.GetSingleAirline(entityPM.MainCarriageCarrierId, entityPM.Tenant);
                    if (myAirline != null)
                    {
                        if (!string.IsNullOrEmpty(myAirline.Prefix))
                        {
                            entityPM.AirlinePrefix = myAirline.Prefix.PadLeft(3, '0');
                        }
                    }
                }

                if (string.IsNullOrEmpty(entityPM.MainCarriageCarrierPrefix))
                {
                    Card card = CardRepository.GetSingleCard(entityPM.MainCarriageCarrierId, entityPM.Tenant, true);

                    if (card != null)
                    {
                        entityPM.MainCarriageCarrierPrefix = card.Code;
                    }
                }
            }
        }

        protected override void UpdateComposition(BookingPM entityPM)
        {
            BookingPackageUpdateService packagesUpdateService = new BookingPackageUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            packagesUpdateService.UpdateMulti(entityPM.BookingPackages, entityPM.DeletedBookingPackages, entityPM, false);

            BookingAnswerUpdateService myBookingAnswerUpdateService = new BookingAnswerUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            myBookingAnswerUpdateService.UpdateMulti(entityPM.BookingAnswers, entityPM.DeletedBookingAnswers, entityPM, false);
        }

        protected override void Trace(BookingPM entityPM, Booking entityPOCO, string changesXml)
        {
            BookingEmailAlertsHelper helper = new BookingEmailAlertsHelper();

            ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);
            ContactRepository contactRep = new ContactRepository(commonContext);

            string email = "";
            if (AuthenticationUtil.IsAuthenticatedUserExists())
            {
                email = AuthenticationUtil.GetAuthenticatedUser();
            }
            else
            {
                email = "system@tenant" + entityPM.Tenant + ".com";
            }

            Contact contact = contactRep.GetSingleContactByEmail(email, entityPM.Tenant);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CRBO",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Booking",
                    Notes = changesXml
                });
            }
            else
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPBO",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Booking",
                    Notes = changesXml
                });

                if (entityPM.BookingStatusCode != entityPOCO.BookingStatusCode)
                {
                    
                    BookingStatusRepository statusRep = new BookingStatusRepository(entityPM.Tenant);

                    BookingStatus newStatus = statusRep.GetSingle(entityPM.BookingStatusCode);
                    BookingStatus oldStatus = statusRep.GetSingle(entityPOCO.BookingStatusCode);

                    string notes = "Booking status changed from " + oldStatus.Name + " to " + newStatus.Name;

                    if (entityPM.IsFSU)
                    {
                        notes = notes + Environment.NewLine + "Updated by FSA/FSU";
                    }

                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "BOKS",
                        UserId = contact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "Booking",
                        Notes = notes,
                    });
                }

                if (entityPM.FFRStatusCode != entityPOCO.FFRStatusCode)
                {
                    FFRStatusRepository statusRep = new FFRStatusRepository(entityPM.Tenant);

                    FFRStatus newStatus = statusRep.GetSingle(entityPM.FFRStatusCode);
                    FFRStatus oldStatus = statusRep.GetSingle(entityPOCO.FFRStatusCode);

                    string notes = "Messaging status changed from " + oldStatus.Name + " to " + newStatus.Name;

                    if (entityPM.IsFSU)
                    {
                        notes = notes + Environment.NewLine + "Updated by FSA/FSU";
                    }

                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "BOKF",
                        UserId = contact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "Booking",
                        Notes = notes,
                    });
                }
            }

            if (entityPM.BookingStatusCode == "CNF" && entityPOCO.BookingStatusCode != "CNF")
            {
                if (entityPM.FFRStatusCode != "CFM")
                {
                    helper.SendEmailAlert(entityPM, entityPM.Tenant, "BOKC");
                }
            }
        }

        private void InitializeMAWBStack(BookingPM entityPM, Booking entityPOCO)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);
            MAWBStackRepository mawStackRepository = new MAWBStackRepository(commonContext);

            //if ((entityPOCO.Master != entityPM.Master || entityPM.MainCarriageCarrierId != entityPOCO.MainCarriageCarrierId) && entityPM.MAWBTakenFromStack == false)
            //{
            //    if (!String.IsNullOrEmpty(entityPM.Master) && entityPM.MainCarriageCarrierId != null)
            //    {
            //        MAWBStack stack = mawStackRepository.GetSingleMAWBStackByNumberAirline(long.Parse(entityPM.Master), entityPM.Tenant, entityPM.MainCarriageCarrierId);
            //        if (stack != null)
            //        {
            //            string msg = TranslateTextsClass.Translate("Shipment.M.ThisAirlineMAWBStackFoundInStack", entityPM.Tenant);
            //            throw new ApplicationException(msg);
            //        }
            //    }
            //}

            if (entityPM.TransportModeCode == "A")
            {
                if (!string.IsNullOrEmpty(entityPM.Master))
                {
                    this.SetMAWBAirline(entityPM);

                    if (string.IsNullOrEmpty(entityPM.MAWBStackAirlineId))
                    {
                        if (entityPM.MAWBTakenFromStack == false && entityPM.MainCarriageIsFromStack == false)
                        {
                            if (!string.IsNullOrEmpty(entityPM.MAWBStackAirlineId))
                            {
                                MAWBStack stack = mawStackRepository.GetSingleMAWBStackByNumberAirline(long.Parse(entityPM.Master), entityPM.Tenant, entityPM.MAWBStackAirlineId);
                                if (stack != null)
                                {
                                    string msg = TranslateTextsClass.Translate("Shipment.M.ThisAirlineMAWBStackFoundInStack", entityPM.Tenant);
                                    throw new ApplicationException(msg);
                                }
                            }
                        }
                    }
                }
            }

            if (entityPM.MAWBTakenFromStack)
            {
                this.SetMAWBAirline(entityPM);

                MAWBStack stack = mawStackRepository.GetSingleMAWBStackByNumberAirline(int.Parse(entityPM.MAWBStackNumber), entityPM.Tenant, entityPM.MAWBStackAirlineId);
                if (stack != null)
                {
                    entityPM.MAWBStackNumber = null;
                    entityPM.MAWBStackAirlineId = null;
                    entityPM.MAWBTakenFromStack = false;
                    entityPM.MainCarriageIsFromStack = true;

                    entityPM.Master = stack.Number.ToString().PadLeft(8, '0');

                    stack.IsUsed = true;
                    mawStackRepository.Update(stack);
                    mawStackRepository.SubmitChanges();
                }

                else
                {
                    string msg = TranslateTextsClass.Translate("Shipment.M.ThisAirlineMAWBStackNotExists", entityPM.Tenant);
                    throw new ApplicationException(msg);
                }
            }

            if (entityPM.MAWBReturnedToStack)
            {
                this.SetMAWBAirline(entityPM);

                MAWBStack stack = mawStackRepository.GetSingleMAWBStackByNumberAirline(int.Parse(entityPM.MAWBStackNumber), entityPM.Tenant, entityPM.MAWBStackAirlineId);
                if (stack != null)
                {
                    entityPM.MAWBStackNumber = null;
                    entityPM.MAWBStackAirlineId = null;
                    entityPM.MAWBReturnedToStack = false;
                    entityPM.MainCarriageIsFromStack = false;

                    if (!entityPM.MAWBReturnedToStackWithCancel)
                    {
                        entityPM.Master = null;
                    }

                    stack.Notes = "Returned from booking";
                    stack.IsUsed = false;
                    stack.InsertionDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                    mawStackRepository.Update(stack);
                    mawStackRepository.SubmitChanges();
                }

                else
                {
                    string msg = TranslateTextsClass.Translate("Shipment.M.ThisAirlineMAWBStackNotExists", entityPM.Tenant);
                    throw new ApplicationException(msg);
                }
            }
        }

        private void SetMAWBAirline(BookingPM entityPM)
        {
            entityPM.MAWBStackAirlineId = entityPM.MainCarriageCarrierId;

            if (!string.IsNullOrEmpty(entityPM.InterlineId))
            {
                entityPM.MAWBStackAirlineId = entityPM.InterlineId;
            }
        }

        public static decimal? GetWeightInKG(string weightCode, decimal? weight)
        {
            decimal? myResult = null;

            if (weight != null)
            {
                decimal? factorOfConvert = 1;

                if (!string.IsNullOrEmpty(weightCode))
                {
                    switch (weightCode.ToUpper())
                    {
                        case "KG": { factorOfConvert = 1; break; }
                        case "LB": { factorOfConvert = (decimal)0.45359237; break; }     // 1 LB = 0.45359237 KG
                        case "MT": { factorOfConvert = 1000; break; }           // 1 mt = 1000 KG
                    }
                }

                myResult = weight * factorOfConvert;
            }

            if (myResult != null)
            {
                myResult = Math.Round((decimal)(myResult.Value), 3);
            }

            return myResult;
        }

        private void ValidateAirlineRestriction(BookingPM entityPM)
        {
            if (entityPM.TransportModeCode == "A")
            {
                int tenant = entityPM.Tenant;

                bool isRestrictedByAirline = false;

                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                    TenantManagement tenantManagement = tenantManagementRepository.GetSingleTenantManagement(tenant);
                    if (tenantManagement != null)
                    {
                        isRestrictedByAirline = tenantManagement.IsRestrictedByAirline;
                    }
                }

                if (isRestrictedByAirline)
                {
                    AirlineRepository airlineRepository = new AirlineRepository(tenant);

                    if (MethodHelper.IsAirlineRestricted(entityPM.MainCarriageCarrierId, airlineRepository, tenant))
                    {
                        List<Airline> allowedAirlines = airlineRepository.GetAllAllowedAirlinesInRestriction(tenant);

                        string airlineCodes = "";

                        foreach (Airline airline in allowedAirlines)
                        {
                            if (string.IsNullOrEmpty(airlineCodes))
                            {
                                airlineCodes = airline.Card.Code;
                            }
                            else
                            {
                                airlineCodes = airlineCodes + " - " + airline.Card.Code;
                            }
                        }
                        
                        throw new ApplicationException("You are restricted for " + airlineCodes + " Airlines only");
                    }
                }
            }
        }

        private void ValidateMasterNumber(BookingPM entityPM)
        {
            if (entityPM.Tenant != 343 && entityPM.Tenant != 528)
            {
                if (!string.IsNullOrEmpty(entityPM.Master) && !string.IsNullOrEmpty(entityPM.AirlinePrefix) && !entityPM.IsCancelled)
                {
                    BookingRepository myBookingRepository = new BookingRepository(entityPM.Tenant);
                    bool isMasterFieldUsed = myBookingRepository.IsMasterFieldUsed(entityPM.Master, entityPM.AirlinePrefix, entityPM.Id, entityPM.Tenant, entityPM.DirectionCode, entityPM.TransportModeCode);
                    if (isMasterFieldUsed)
                    {
                        throw new ApplicationException("Master field already used in another Booking");
                    }
                }
            }
        }

        protected override void CheckConcurrency(BookingPM entityPM, Booking entityPOCO)
        {
            if (entityPM.ChangeSetOp != ChangeSetOperation.Insert)
            {
                if (!entityPM.ConcurrencyGUID.Equals(entityPOCO.ConcurrencyGUID))
                {
                    string msg = BookingTranslateTextsClass.Translate("General.M.CantUpdateRecord", entityPM.Tenant);

                    if (entityPOCO.UpdatedByPartner != null)
                    {
                        msg = msg.Replace("another user", entityPOCO.UpdatedByPartner);
                    }

                    throw new OptimisticConcurrencyException(msg);
                }
            }

            base.CheckConcurrency(entityPM, entityPOCO);
        }
    }
}
