using Logitude.BL.Helpers;
using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.TariffModule.BL.EntityPMs;
using Logitude.TariffModule.BL.EntityQueryServices;
using Logitude.TariffModule.BL.Helpers;
using Logitude.TariffModule.Data;
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.Data.Repositories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TariffModule.BL.EntityUpdateServices
{
    public partial class TariffUpdateService
    {
        protected override void OnCreating(TariffPM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.Id = IdCounter.GetNumber("Tariff", entityPM.Tenant);
                entityPM.TariffNumber = CodeCounter.GetNumber("Tariff", entityPM.Tenant).ToString();
                entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

                if (entityPM.PriceSteps == null && (entityPM.TypeCode == "AFC" || entityPM.TypeCode == "OLC"))
                {
                    ITariffModuleContext iContext = TariffModuleContext.GetContext(entityPM.Tenant);
                    IInfrastructureContext iInfrastructureContext = InfrastructureContext.GetContext(entityPM.Tenant);
                    TariffSetting iTariffSetting = (from d in iContext.TariffSettings where d.Tenant == entityPM.Tenant select d).FirstOrDefault();
                    
                    if (iTariffSetting != null)
                    {
                        if(entityPM.TypeCode== "AFC")
                        {
                            PriceStep iPriceSteps = (from d in iInfrastructureContext.PriceSteps where d.Tenant == entityPM.Tenant && d.Id == iTariffSetting.AirDefaultStepsId select d).FirstOrDefault();
                            entityPM.PriceSteps = iPriceSteps!= null? iPriceSteps.Steps: null;
                        }
                        else if(entityPM.TypeCode == "OLC")
                        {
                            PriceStep iPriceSteps = (from d in iInfrastructureContext.PriceSteps where d.Tenant == entityPM.Tenant && d.Id == iTariffSetting.LCLDefaultStepsId select d).FirstOrDefault();
                            entityPM.PriceSteps = iPriceSteps != null ? iPriceSteps.Steps : null;
                        }
                    }
                }

                if(entityPM.TypeCode == "OFS")
                {
                    string BCNTId = this.GetBCNTMeasurements(entityPM.Tenant);

                    if(!string.IsNullOrEmpty(BCNTId))
                    {
                        this.FillBCNTMeasurements(entityPM, BCNTId, 1);
                        this.FillBCNTMeasurements(entityPM, BCNTId, 2);
                        this.FillBCNTMeasurements(entityPM, BCNTId, 3);
                        this.FillBCNTMeasurements(entityPM, BCNTId, 4);
                        this.FillBCNTMeasurements(entityPM, BCNTId, 5);
                        this.FillBCNTMeasurements(entityPM, BCNTId, 6);
                        this.FillBCNTMeasurements(entityPM, BCNTId, 7);
                        this.FillBCNTMeasurements(entityPM, BCNTId, 8);
                        this.FillBCNTMeasurements(entityPM, BCNTId, 9);
                        this.FillBCNTMeasurements(entityPM, BCNTId, 10);
                    }

                    else
                    {
                        throw new ApplicationException("BCNT Measurement is not found");
                    }
                }

                this.ValidateSurchargeUniqueSeller(entityPM);
                this.ValidateFCLSurchargeUniqueSeller(entityPM);
            }
        }
        
        protected override void OnUpdating(TariffPM entityPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            }

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

            string myLoggedUserId = null;
            Contact contact = contactRep.GetSingleContactByEmail(email, entityPM.Tenant);
            if (contact != null)
            {
                myLoggedUserId = contact.Id;
            }

            entityPM.UpdatedByUserId = myLoggedUserId;

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                if (entityPM.CreatedByUserId == null)
                {
                    entityPM.CreatedByUserId = myLoggedUserId;
                }

                this.CreateTariffVersion(entityPM);
            }

            if (entityPM.TypeCode == "ASC" || entityPM.TypeCode == "OSC" || entityPM.TypeCode == "OFS")
            {
                if (entityPM.TariffVersions.Where(d => d.StartDate != null || d.ExpirationDate != null).Any())
                {
                    throw new ApplicationException("Dates are not allowed in Surcharges versions");
                }

                if (entityPM.ActiveVersions.Where(d => d.StartDate != null || d.ExpirationDate != null).Any())
                {
                    throw new ApplicationException("Dates are not allowed in Surcharges versions");
                }
            }

            this.ValidateSurchargeUniqueSeller(entityPM);
            this.ValidateFCLSurchargeUniqueSeller(entityPM);

            if (entityPM.IsApprovingDraftVersion)
            {
                this.ApproveDraftVersion(entityPM);
                entityPM.IsApprovingDraftVersion = false;
            }

            if (entityPM.IsUpdatingMissingPorts)
            {
                this.UpdateMissingPorts(entityPM);
                entityPM.IsUpdatingMissingPorts = false;
            }

            this.InsertTariffSurchargeLog(entityPM);
        }
        
        List<ChargesType> ChargeTypes;
        private void InsertTariffSurchargeLog(TariffPM tariff)
        {
            if ((tariff.TypeCode == "ASC" || tariff.TypeCode == "OSC" || tariff.TypeCode == "OFS") && !tariff.IsFromUpdateScreen && !tariff.IsFromCopy)
            {
                TariffSurchargesUpdateUpdateService tariffSurchargeUpdateService = new TariffSurchargesUpdateUpdateService(TariffModuleContext.GetContext(tariff.Tenant), new Dictionary<string, IContext>(), tariff.Tenant);
                TariffVersionPM version = tariff.TariffVersions.Where(prop => prop.IsDraft == true).FirstOrDefault();
                if (version != null && version.TariffLines != null)
                {
                    ChargesTypeRepository chargesTypeRepository = new ChargesTypeRepository(tariff.Tenant);
                    ChargeTypes = chargesTypeRepository.GetChargesTypes(tariff.Tenant).ToList();
                    List<TariffLinePM> tariffUpdatedLines = version.TariffLines.Where(p => p.ChangeSetOp == ChangeSetOperation.Update || p.ChangeSetOp == ChangeSetOperation.Insert).ToList();
                    TariffLineRepository tariffLineRepository = new TariffLineRepository(tariff.Tenant);
                    List<TariffLine> tariffUpdatedLines_POCO = tariffLineRepository.GetTariffLinesByTariff(tariff.Id, tariff.Tenant);
                    foreach (var item in tariffUpdatedLines)
                    {
                        TariffSurchargesUpdatePM tariffSurchageLog = new TariffSurchargesUpdatePM();
                        tariffSurchageLog.TariffId = tariff.Id;
                        tariffSurchageLog.Version = item.Version;
                        tariffSurchageLog.Surcharges = this.GetUpdatedSurcharges(tariff, item, tariffUpdatedLines_POCO);
                        tariffSurchageLog.LinesUpdated = 1;
                        tariffSurchageLog.StartDate = item.StartDate;
                        tariffSurchageLog.UpdateMethodCode = "MA";
                        tariffSurchageLog.ChangeSetOp = ChangeSetOperation.Insert;
                        tariffSurchageLog.Tenant = item.Tenant;
                        tariffSurchageLog.To = item.DestinationPortCode;
                        tariffSurchageLog.From = item.OriginPortCode;
                        tariffSurchargeUpdateService.Update(tariffSurchageLog, true);
                    }
                }
            }
        }

        private string GetUpdatedSurcharges(TariffPM tariff, TariffLinePM itemPM, List<TariffLine> tariffUpdatedLines_POCO)
        {
            var surcharges = "";
            if (tariffUpdatedLines_POCO != null && tariffUpdatedLines_POCO.Count > 0)
            {
                var itemPOCO = tariffUpdatedLines_POCO.Where(a => a.Id == itemPM.Id).FirstOrDefault();
                if (itemPOCO != null)
                {
                    for (int i = 1; i <= 10; i++)
                    {
                        PropertyInfo pMPricePropInfoPM = itemPM.GetType().GetProperty("Surcharge" + i + "Price");
                        PropertyInfo pMMinPricePropInfoPM = itemPM.GetType().GetProperty("Surcharge" + i + "MinPrice");
                        PropertyInfo pMPricePropInfoPOCO = itemPOCO.GetType().GetProperty("Surcharge" + i + "Price");
                        PropertyInfo pMMinPricePropInfoPOCO = itemPOCO.GetType().GetProperty("Surcharge" + i + "MinPrice");

                        var pMPricevalue = (decimal?)pMPricePropInfoPM.GetValue(itemPM);
                        var pMMinPricevalue = (decimal?)pMMinPricePropInfoPM.GetValue(itemPM);
                        var pOCOPricevalue = (decimal?)pMPricePropInfoPOCO.GetValue(itemPOCO);
                        var pOCOMinPricevalue = (decimal?)pMMinPricePropInfoPOCO.GetValue(itemPOCO);

                        if (pMPricevalue != pOCOPricevalue || pMMinPricevalue != pOCOMinPricevalue)
                        {
                            PropertyInfo chargeIdPropInfo = tariff.GetType().GetProperty("Surcharge" + i + "Id");
                            string chargeIdValue = chargeIdPropInfo.GetValue(tariff).ToString();
                            surcharges += ChargeTypes.Where(a => a.Id == chargeIdValue).Select(d => d.Code).FirstOrDefault() + ", ";
                        }
                    }
                    surcharges = surcharges.TrimEnd(',');
                }
                else
                {
                    surcharges = GetUpdatedSurchargesInNew(tariff, itemPM);
                }
            }
            else
            {
                surcharges = GetUpdatedSurchargesInNew(tariff, itemPM);
            }
            return surcharges;
        }

        private string GetUpdatedSurchargesInNew(TariffPM tariff, TariffLinePM itemPM)
        {
            var surcharges = "";
            for (int i = 1; i <= 10; i++)
            {
                PropertyInfo pMPricePropInfoPM = itemPM.GetType().GetProperty("Surcharge" + i + "Price");
                PropertyInfo pMMinPricePropInfoPM = itemPM.GetType().GetProperty("Surcharge" + i + "MinPrice");
                var pMPricevalue = (decimal?)pMPricePropInfoPM.GetValue(itemPM);
                var pMMinPricevalue = (decimal?)pMMinPricePropInfoPM.GetValue(itemPM);

                if (pMPricevalue != null || pMMinPricevalue != null)
                {
                    PropertyInfo chargeIdPropInfo = tariff.GetType().GetProperty("Surcharge" + i + "Id");
                    string chargeIdValue = chargeIdPropInfo.GetValue(tariff).ToString();
                    surcharges += ChargeTypes.Where(a => a.Id == chargeIdValue).Select(d => d.Code).FirstOrDefault() + ", ";
                }
            }
            return surcharges.TrimEnd(',');
        }

        protected override void UpdateComposition(TariffPM entityPM)
        {
            TariffVersionUpdateService tariffVersionUpdateService = new TariffVersionUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            tariffVersionUpdateService.UpdateMulti(entityPM.TariffVersions, entityPM.DeletedTariffVersions, entityPM, false);
        }

        protected override void Trace(TariffPM entityPM, Tariff entityPOCO, string changesXml)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);
            ContactRepository contactRep = new ContactRepository(commonContext);
            Contact contact = contactRep.GetSingleContactByEmail(AuthenticationUtil.GetAuthenticatedUser(), entityPM.Tenant);


            if (!string.IsNullOrEmpty(entityPM.FileUploadedName))
            {
                int numberOfLines = 0;
                TariffVersionPM version = entityPM.TariffVersions.Where(prop => prop.IsDraft == true).FirstOrDefault();

                if (version != null)
                {
                    numberOfLines = version.TariffLines.Where(p => p.ChangeSetOp != ChangeSetOperation.Delete).ToList().Count;
                }
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "TUPL",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Tariff",
                    Notes = entityPM.FileUploadedName + " uploaded (" + numberOfLines + " lines)"
                });

            }
            if (entityPM.TariffLinesAdded)
            {
                TariffVersionPM tariffVersion = entityPM.TariffVersions.Where(p => p.IsDraft).FirstOrDefault();
                if (tariffVersion != null)
                {
                    int count = tariffVersion.TariffLines.Where(a => a.AddedManually == true).Count();
                    if (count > 0)
                    {
                        EventTracer.CreateTraceEvent(new EventTracerArgs()
                        {
                            Tenant = entityPM.Tenant,
                            EventTypeCode = "TLAD",
                            UserId = contact.Id,
                            EntityId = entityPM.Id,
                            ObjectTableName = "Tariff",
                            Notes = "Tariff Lines manually added (" + count + " lines)"
                        });
                    }
                }

            }

            if (entityPM.SetAsInActive)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "SAIN",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Tariff",
                    Notes = changesXml
                });
            }

            if (entityPM.SetAsReActive)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "REAC",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Tariff",
                    Notes = changesXml
                });
            }

            if (entityPM.IsSurchargeUpdate)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "SUCU",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Tariff",
                    Notes = changesXml
                });
            }

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPEV",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Tariff",
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
                    ObjectTableName = "Tariff",
                    Notes = changesXml
                });
            }
        }

        protected override void CheckConcurrency(TariffPM entityPM, Tariff entityPOCO)
        {
            if (!entityPM.ConcurrencyGUID.Equals(entityPOCO.ConcurrencyGUID) && !entityPM.NewConcurrencyGUID.Equals(entityPOCO.ConcurrencyGUID))
            {
                string msg = TranslateTextsClass.Translate("General.M.CantUpdateRecord", entityPM.Tenant);
                throw new OptimisticConcurrencyException(msg);
            }
        }

        private void CreateTariffVersion(TariffPM entityPM)
        {
            entityPM.LastVersion += 1;
            entityPM.LastStartDate = entityPM.StartDate;
            entityPM.LastExpirationDate = entityPM.ExpirationDate;

            TariffVersionPM tariffVersionPM = new TariffVersionPM()
            {
                TariffId = entityPM.Id,
                Tenant = entityPM.Tenant,
                CreateDate = entityPM.CreateDate,
                CreatedByUserId = entityPM.CreatedByUserId,
                StartDate = entityPM.StartDate,
                ExpirationDate = entityPM.ExpirationDate,
                Version = entityPM.LastVersion,
                SearchFields = entityPM.LastVersion.ToString(),
                ChangeSetOp = ChangeSetOperation.Insert,
                IsDraft = true,
            };

            if (entityPM.TypeCode == "ASC" || entityPM.TypeCode == "OSC" || entityPM.TypeCode == "OFA")
            {
                tariffVersionPM.StartDate = null;
                tariffVersionPM.ExpirationDate = null;
            }

            entityPM.TariffVersions.Add(tariffVersionPM);
        }

        private TariffLineRepository iTariffLineRepository;
        private void ApproveDraftVersion(TariffPM entityPM)
        {
            if (entityPM.IsApprovingDraftVersion)
            {
                this.iTariffLineRepository = new TariffLineRepository(entityPM.Tenant);

                TariffVersionPM iDraftVersion = entityPM.TariffVersions.Where(d => d.IsDraft).FirstOrDefault();
                List<TariffLinePM> iDraftVersionLines = new List<TariffLinePM>();

                if (iDraftVersion == null)
                {
                    throw new ApplicationException("No Draft version to approve");
                }

                else
                {
                    iDraftVersionLines = iDraftVersion.TariffLines;
                }

                if (iDraftVersionLines.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete && d.HasErrors).Count() > 0)
                {
                    throw new ApplicationException("Invalid Tariff Lines");
                }

                if (entityPM.TypeCode == "AFC" || entityPM.TypeCode == "OLC")
                {
                    if (iDraftVersion.ExpirationDate != null)
                    {
                        if (iDraftVersion.ExpirationDate.Value.Date < entityPM.UpdateDate.Date)
                        {
                            throw new ApplicationException("Approving past version is not allowed, please update the dates");
                        }
                    }
                    else
                    {
                        if (iDraftVersion.InitialEnddate != null)
                        {
                            if (iDraftVersion.InitialEnddate.Value.Date < entityPM.UpdateDate.Date)
                            {
                                throw new ApplicationException("Approving past version is not allowed, please update the dates");
                            }
                        }
                    }
                }

                iDraftVersion.IsDraft = false;
                iDraftVersion.ApprovedByUserId = entityPM.UpdatedByUserId;
                iDraftVersion.ApproveDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                iDraftVersion.ChangeSetOp = ChangeSetOperation.Update;

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "VNAP",
                    UserId = iDraftVersion.ApprovedByUserId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Tariff",
                    Notes = "Version " + iDraftVersion.Version + " approved",
                });

                if (entityPM.TypeCode == "ASC" || entityPM.TypeCode == "OSC" || entityPM.TypeCode == "OFS")
                {
                    TariffVersionPM iPreviousVersion = entityPM.ActiveVersions.OrderByDescending(o => o.CreateDate).FirstOrDefault();
                    if (iPreviousVersion != null)
                    {
                        List<TariffLine> iPreviousVersionLines = iTariffLineRepository.GetTariffLinesByTariffAndVersion(entityPM.Id, iPreviousVersion.Version, entityPM.Tenant);

                        if (entityPM.DeletedLinesExpirationDates.Count > 0)
                        {
                            foreach (TariffLineExpirationDatePM tariffLineExpirationDateItem in entityPM.DeletedLinesExpirationDates)
                            {
                                TariffLine previousLine = iPreviousVersionLines.Where(d => d.OriginPortId == tariffLineExpirationDateItem.OriginPortId && d.DestinationPortId == tariffLineExpirationDateItem.DestinationPortId).FirstOrDefault();
                                if (previousLine != null)
                                {
                                    this.UpdateVersionPreviousLineExpirationDate(tariffLineExpirationDateItem, previousLine);
                                }
                            }
                        }

                        foreach (TariffLinePM tariffLinePM in iDraftVersionLines)
                        {
                            TariffLine previousLine = iPreviousVersionLines.Where(d => d.OriginPortId == tariffLinePM.OriginPortId && d.DestinationPortId == tariffLinePM.DestinationPortId).FirstOrDefault();

                            if (previousLine != null)
                            {
                                if (tariffLinePM.ChangeSetOp == ChangeSetOperation.Delete)
                                {

                                }

                                else
                                {
                                    if (tariffLinePM.StartDate != null)
                                    {
                                        this.UpdateVersionPreviousLineSatrtDate(tariffLinePM, previousLine);
                                    }
                                }
                            }
                        }

                        iTariffLineRepository.SubmitChanges();
                    }
                }

                else
                {
                    this.ComputeTariffLinesExpirationDates(iDraftVersion, entityPM);
                }
            }
        }

        private void ComputeTariffLinesExpirationDates(TariffVersionPM iDraftVersion, TariffPM entityPM)
        {
            iDraftVersion.ExpirationDate = iDraftVersion.InitialEnddate;
            TariffVersionRepository tariffVersionRepository = new TariffVersionRepository(iDraftVersion.Tenant);
            TariffVersion iPreviousVersion = tariffVersionRepository.GetAllVersions(entityPM.Id, entityPM.Tenant).Where(o => o.Version != iDraftVersion.Version).OrderByDescending(o => o.CreateDate).FirstOrDefault();
            if (iPreviousVersion != null)
            {
                if (iPreviousVersion.StartDate.Value.Date >= iDraftVersion.StartDate.Value.Date)
                {
                    throw new ApplicationException("Start date is smaller than start date of the previous version");
                }
                if (iPreviousVersion.Version != iDraftVersion.Version)
                {
                    List<TariffLine> iPreviousVersionLines = iTariffLineRepository.GetTariffLinesByTariffAndVersion(entityPM.Id, iPreviousVersion.Version, entityPM.Tenant);
                    iPreviousVersion.ExpirationDate = iDraftVersion.StartDate.Value.AddDays(-1);
                    tariffVersionRepository.Update(iPreviousVersion);
                    foreach (TariffLine line in iPreviousVersionLines)
                    {
                        line.ExpirationDate = iPreviousVersion.ExpirationDate;
                        iTariffLineRepository.Update(line);
                    }
                    foreach (TariffLinePM line in iDraftVersion.TariffLines.Where(p => p.ChangeSetOp != ChangeSetOperation.Delete).ToList())
                    {
                        line.ExpirationDate = iDraftVersion.ExpirationDate;
                    }
                    iTariffLineRepository.SubmitChanges();
                    tariffVersionRepository.SubmitChanges();
                }
            }
        }

        private void UpdateVersionPreviousLineExpirationDate(TariffLineExpirationDatePM tariffLineExpirationDateItem, TariffLine previousLine)
        {
            bool isExpirationDateValid = this.ValidatePreviousLineDates(new { DateField = "expiration", TariffLineExpirationDateItem = tariffLineExpirationDateItem, PreviousLine = previousLine });

            if (isExpirationDateValid)
            {
                previousLine.ExpirationDate = tariffLineExpirationDateItem.ExpirationDate;
                iTariffLineRepository.Update(previousLine);
            }

            else
            {
                throw new ApplicationException("Expiration date can't be less than start date in the previous version line");
            }
        }
        private void UpdateVersionPreviousLineSatrtDate(TariffLinePM tariffLinePM, TariffLine previousLine)
        {
            bool isExpirationDateValid = this.ValidatePreviousLineDates(new { DateField = "start", TariffLinePM = tariffLinePM, PreviousLine = previousLine });

            if (isExpirationDateValid)
            {
                previousLine.ExpirationDate = tariffLinePM.StartDate.Value.AddDays(-1);
                iTariffLineRepository.Update(previousLine);
            }

            else
            {
                string msg = "Line (" + tariffLinePM.OriginPortCode + " > " + tariffLinePM.DestinationPortCode + ") Start Date is less than or equal the previous version line";
                throw new ApplicationException(msg);
            }
        }
        private bool ValidatePreviousLineDates(dynamic previousLineDatesArgs)
        {
            bool isExpirationDateValid = true;

            if (previousLineDatesArgs.DateField == "expiration")
            {
                if (previousLineDatesArgs.TariffLineExpirationDateItem.ExpirationDate < previousLineDatesArgs.PreviousLine.StartDate)
                {
                    isExpirationDateValid = false;
                }
            }

            else if (previousLineDatesArgs.DateField == "start")
            {
                if (previousLineDatesArgs.TariffLinePM.StartDate <= previousLineDatesArgs.PreviousLine.StartDate)
                {
                    isExpirationDateValid = false;
                }
            }

            return isExpirationDateValid;
        }

        private void ValidateSurchargeUniqueSeller(TariffPM entityPM)
        {
            if (entityPM.TypeCode == "ASC" || entityPM.TypeCode == "OSC")
            {
                ITariffModuleContext iContext = TariffModuleContext.GetContext(entityPM.Tenant);
                int iCount = (from d in iContext.Tariffs
                              where d.Tenant == entityPM.Tenant
                              && d.Id != entityPM.Id
                              && d.SellerId == entityPM.SellerId
                              && (d.TypeCode == "ASC" || d.TypeCode == "OSC")
                              select d).Count();

                if (iCount >= 1)
                {
                    throw new ApplicationException("Tariff surcharge seller should be unique");
                }
            }
        }
        private void ValidateFCLSurchargeUniqueSeller(TariffPM entityPM)
        {
            if (entityPM.TypeCode == "OFS")
            {
                ITariffModuleContext iContext = TariffModuleContext.GetContext(entityPM.Tenant);
                int iCount = (from d in iContext.Tariffs
                              where d.Tenant == entityPM.Tenant
                              && d.Id != entityPM.Id
                              && d.SellerId == entityPM.SellerId
                              && d.TypeCode == "OFS"
                              select d).Count();

                if (iCount >= 1)
                {
                    throw new ApplicationException("Tariff surcharge seller should be unique");
                }
            }
        }

        private void UpdateMissingPorts(TariffPM entityPM)
        {
            this.iTariffLineRepository = new TariffLineRepository(entityPM.Tenant);
            PortRepository portRepository = new PortRepository(entityPM.Tenant);

            TariffVersionPM iDraftVersion = entityPM.TariffVersions.Where(d => d.IsDraft).FirstOrDefault();
            List<TariffLine> iDraftVersionLines = new List<TariffLine>();

            if (iDraftVersion == null)
            {
                throw new ApplicationException("No Draft version found");
            }

            else
            {
                iDraftVersionLines = this.iTariffLineRepository.GetTariffLinesByTariffAndVersion(entityPM.Id, iDraftVersion.Version, entityPM.Tenant);
            }

            if (iDraftVersionLines != null)
            {
                iDraftVersionLines = iDraftVersionLines.Where(d => d.HasErrors && (string.IsNullOrEmpty(d.OriginPortId) || string.IsNullOrEmpty(d.DestinationPortId))).ToList();

                if (iDraftVersionLines.Count > 0)
                {
                    foreach (TariffLine tariffLine in iDraftVersionLines)
                    {
                        bool tariffLineUpdated = false;

                        if (string.IsNullOrEmpty(tariffLine.OriginPortId) && !string.IsNullOrEmpty(tariffLine.OriginPortText))
                        {
                            Port fromPort = null;
                            if (entityPM.TypeCode == "AFC")
                            {
                                fromPort = portRepository.GetAirlinePortByCode(entityPM.Tenant, tariffLine.OriginPortText.Trim(), true);
                            }
                            else
                            {
                                fromPort = portRepository.GetOceanPortByCode(entityPM.Tenant, tariffLine.OriginPortText.Trim(), true);
                            }
                            if (fromPort == null)
                            {
                                Port portZero = null;
                                if (entityPM.TypeCode == "AFC")
                                {
                                    portZero = portRepository.GetAirlinePortByCode(0, tariffLine.OriginPortText.Trim(), true);
                                }
                                else
                                {
                                    portZero = portRepository.GetOceanPortByCode(0, tariffLine.OriginPortText.Trim(), true);
                                }
                               
                                if (portZero != null)
                                {
                                    fromPort = this.GetPortCopyToCurrentTenant(portZero, entityPM.Tenant, portRepository);
                                }
                            }

                            if (fromPort != null)
                            {
                                tariffLine.OriginPortId = fromPort.Id;
                                tariffLine.OriginPortText = null;
                                tariffLineUpdated = true;
                            }
                        }

                        if (string.IsNullOrEmpty(tariffLine.DestinationPortId) && !string.IsNullOrEmpty(tariffLine.DestinationPortText))
                        {
                            Port toPort = null;
                            if (entityPM.TypeCode == "AFC")
                            {
                                toPort = portRepository.GetAirlinePortByCode(entityPM.Tenant, tariffLine.DestinationPortText.Trim(), true);
                            }
                            else
                            {
                                toPort = portRepository.GetOceanPortByCode(entityPM.Tenant, tariffLine.DestinationPortText.Trim(), true);
                            }

                            if (toPort == null)
                            {
                                Port portZero = null;
                                if (entityPM.TypeCode == "AFC")
                                {
                                    portZero = portRepository.GetAirlinePortByCode(0, tariffLine.DestinationPortText.Trim(), true);
                                }
                                else
                                {
                                    portZero = portRepository.GetOceanPortByCode(0, tariffLine.DestinationPortText.Trim(), true);
                                }
                                if (portZero != null)
                                {
                                    toPort = this.GetPortCopyToCurrentTenant(portZero, entityPM.Tenant, portRepository);
                                }
                            }

                            if (toPort != null)
                            {
                                tariffLine.DestinationPortId = toPort.Id;
                                tariffLine.DestinationPortText = null;
                                tariffLineUpdated = true;
                            }
                        }

                        if (tariffLineUpdated)
                        {
                            tariffLine.ErrorText = TariffLineHelper.ComputeTariffLineErrorText(tariffLine);

                            if(string.IsNullOrEmpty(tariffLine.ErrorText))
                            {
                                tariffLine.HasErrors = false;
                            }
                            else
                            {
                                tariffLine.HasErrors = true;
                            }

                            iTariffLineRepository.Update(tariffLine);
                        }
                    }

                    iTariffLineRepository.SubmitChanges();
                }
            }
        }
        private Port GetPortCopyToCurrentTenant(Port ZeroPort, int tenant, PortRepository portRepository)
        {
            ICommonDataContext objectContext = portRepository.context;

            CountryRepository countryRepository = new CountryRepository(objectContext);
            GlobalZoneRepository globalZoneRepository = new GlobalZoneRepository(objectContext);

            Country country = countryRepository.GetSingleCountryByCode(ZeroPort.Country.Code, tenant, false);

            if (country == null)
            {
                GlobalZone globalzone = globalZoneRepository.GetSingleGlobalZoneByCode(ZeroPort.Country.GlobalZone.Code, tenant);

                if (globalzone == null)
                {
                    GlobalZone oldZone = globalZoneRepository.GetSingleGlobalZone(ZeroPort.Country.GlobalZoneId, 0);
                    globalzone = new GlobalZone()
                    {
                        Id = IdCounter.GetNumber("GlobalZone", tenant).ToString(),
                        Code = oldZone.Code,
                        EnglishName = oldZone.EnglishName,
                        LocalName = oldZone.LocalName,
                        Notes = oldZone.Notes,
                        SearchFields = oldZone.SearchFields,
                        Tenant = tenant,
                    };

                    globalZoneRepository.Add(globalzone);
                    globalZoneRepository.SubmitChanges();
                }

                Country oldCountry = CountryRepository.GetSingleCountry(ZeroPort.CountryId, 0, false);
                country = new Country()
                {
                    Id = IdCounter.GetNumber("Country", tenant).ToString(),
                    Tenant = tenant,
                    GlobalZoneId = oldCountry.GlobalZoneId,
                    EC = oldCountry.EC,
                    EnglishName = oldCountry.EnglishName,
                    Code = oldCountry.Code,
                    InActive = oldCountry.InActive,
                    Notes = oldCountry.Notes,
                    LocalName = oldCountry.LocalName,
                    SearchFields = oldCountry.SearchFields,
                };

                countryRepository.Add(country);
                countryRepository.SubmitChanges();
            }

            Port newPort = new Port()
            {
                Id = IdCounter.GetNumber("Port", tenant).ToString(),
                Code = ZeroPort.Code,
                EnglishName = ZeroPort.EnglishName,
                LocalName = ZeroPort.LocalName,
                Tenant = tenant,
                AddedManually = false,
                InActive = false,
                CountryId = country.Id,
                IsAir = ZeroPort.IsAir,
                IsInland = ZeroPort.IsInland,
                IsOcean = ZeroPort.IsOcean,
                Latitude = ZeroPort.Latitude,
                Longtitude = ZeroPort.Longtitude,
                SearchFields = ZeroPort.SearchFields,
                Notes = ZeroPort.Notes,
            };

            portRepository.Add(newPort);
            portRepository.SubmitChanges();

            TableLastUpdateClass.UpdateTableHistory(tenant, "Port");

            return newPort;
        }

        private string GetBCNTMeasurements(int tenant)
        {
            MeasurementRepository measurementRepository = new MeasurementRepository(tenant);
            string id = measurementRepository.GetMeasurementIdbyCode("BCNT", tenant);

            return id;
        }
        private void FillBCNTMeasurements(TariffPM entityPM, string BCNTId, int index)
        {
            PropertyInfo valuePropInfo1 = entityPM.GetType().GetProperty("Surcharge" + index + "Id");
            string value1 = valuePropInfo1.GetValue(entityPM).ToString();

            if (!string.IsNullOrEmpty(value1))
            {
                PropertyInfo valuePropInfo2 = entityPM.GetType().GetProperty("Surcharge" + index + "UOM");
                valuePropInfo2.SetValue(entityPM, BCNTId);
            }
        }
    }
}
