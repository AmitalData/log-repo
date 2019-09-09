using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.TariffModule.BL.EntityPMs;
using Logitude.TariffModule.BL.EntityQueryServices;
using Logitude.TariffModule.Data;
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.Data.Repositories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
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
                if (entityPM.PriceSteps == null)
                {
                    ITariffModuleContext iContext = TariffModuleContext.GetContext(entityPM.Tenant);
                    TariffSetting iTariffSetting = (from d in iContext.TariffSettings where d.Tenant == entityPM.Tenant select d).FirstOrDefault();
                    if (iTariffSetting != null)
                    {
                        entityPM.PriceSteps = iTariffSetting.DefaultPriceSteps;
                    }
                }

                this.ValidateSurchargeUniqueSeller(entityPM);
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

            if (entityPM.TypeCode == "ASC")
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

            if (entityPM.IsApprovingDraftVersion)
            {
                this.ApproveDraftVersion(entityPM);
                entityPM.IsApprovingDraftVersion = false;
            }

            this.InsertTariffSurchargeLog(entityPM);
        }


        List<Measurement> Measurements; 
        private void InsertTariffSurchargeLog(TariffPM tariff)
        {
            if (tariff.TypeCode == "ASC" && !tariff.IsFromUpdateScreen)
            {
                MeasurementRepository measurementRepository = new MeasurementRepository(tariff.Tenant);
                Measurements = measurementRepository.GetMeasurementsByTenant(tariff.Tenant).ToList();

                TariffSurchargesUpdateUpdateService tariffSurchargeUpdateService = new TariffSurchargesUpdateUpdateService(TariffModuleContext.GetContext(tariff.Tenant), new Dictionary<string, IContext>(), tariff.Tenant);
                TariffVersionPM version = tariff.TariffVersions.Where(prop => prop.IsDraft == true).FirstOrDefault();
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
                    tariffSurchageLog.StartDate = tariff.StartDate;
                    tariffSurchageLog.UpdateMethodCode = "MA";
                    tariffSurchageLog.ChangeSetOp = ChangeSetOperation.Insert;
                    tariffSurchageLog.Tenant = item.Tenant;
                    tariffSurchageLog.To = item.DestinationPortCode;
                    tariffSurchageLog.From = item.OriginPortCode;
                    tariffSurchargeUpdateService.Update(tariffSurchageLog, true);
                }
            }
        }

        private string GetUpdatedSurcharges(TariffPM tariff, TariffLinePM itemPM, List<TariffLine> tariffUpdatedLines_POCO)
        {
            var surcharges = "";
            var itemPOCO = tariffUpdatedLines_POCO.Where(a=>a.Id == itemPM.Id).FirstOrDefault();
            if(itemPM.Surcharge1Price != itemPOCO.Surcharge1Price || itemPM.Surcharge1MinPrice != itemPOCO.Surcharge1MinPrice)
            {
                surcharges += Measurements.Where(a=>a.Id == tariff.Surcharge1UOM).Select(d=>d.Code).FirstOrDefault()  + ",";
            }
            if (itemPM.Surcharge2Price != itemPOCO.Surcharge2Price || itemPM.Surcharge2MinPrice != itemPOCO.Surcharge2MinPrice)
            {
                surcharges += Measurements.Where(a => a.Id == tariff.Surcharge2UOM).Select(d=>d.Code).FirstOrDefault() + ",";
            }
            if (itemPM.Surcharge3Price != itemPOCO.Surcharge3Price || itemPM.Surcharge3MinPrice != itemPOCO.Surcharge3MinPrice)
            {
                surcharges += Measurements.Where(a => a.Id == tariff.Surcharge3UOM).Select(d=>d.Code).FirstOrDefault() + ",";
            }
            if (itemPM.Surcharge4Price != itemPOCO.Surcharge4Price || itemPM.Surcharge4MinPrice != itemPOCO.Surcharge4MinPrice)
            {
                surcharges += Measurements.Where(a => a.Id == tariff.Surcharge4UOM).Select(d=>d.Code).FirstOrDefault() + ",";
            }
            if (itemPM.Surcharge5Price != itemPOCO.Surcharge5Price || itemPM.Surcharge5MinPrice != itemPOCO.Surcharge5MinPrice)
            {
                surcharges += Measurements.Where(a => a.Id == tariff.Surcharge5UOM).Select(d=>d.Code).FirstOrDefault() + ",";
            }
            if (itemPM.Surcharge6Price != itemPOCO.Surcharge6Price || itemPM.Surcharge6MinPrice != itemPOCO.Surcharge6MinPrice)
            {
                surcharges += Measurements.Where(a => a.Id == tariff.Surcharge6UOM).Select(d=>d.Code).FirstOrDefault() + ",";
            }
            if (itemPM.Surcharge7Price != itemPOCO.Surcharge7Price || itemPM.Surcharge7MinPrice != itemPOCO.Surcharge7MinPrice)
            {
                surcharges += Measurements.Where(a => a.Id == tariff.Surcharge7UOM).Select(d=>d.Code).FirstOrDefault() + ",";
            }
            if (itemPM.Surcharge8Price != itemPOCO.Surcharge8Price || itemPM.Surcharge8MinPrice != itemPOCO.Surcharge8MinPrice)
            {
                surcharges += Measurements.Where(a => a.Id == tariff.Surcharge8UOM).Select(d=>d.Code).FirstOrDefault() + ",";
            }
            if (itemPM.Surcharge9Price != itemPOCO.Surcharge9Price || itemPM.Surcharge9MinPrice != itemPOCO.Surcharge9MinPrice)
            {
                surcharges += Measurements.Where(a => a.Id == tariff.Surcharge9UOM).Select(d=>d.Code).FirstOrDefault() + ",";
            }
            if (itemPM.Surcharge10Price != itemPOCO.Surcharge10Price || itemPM.Surcharge10MinPrice != itemPOCO.Surcharge10MinPrice)
            {
                surcharges += Measurements.Where(a => a.Id == tariff.Surcharge10UOM).Select(d=>d.Code).FirstOrDefault() + ",";
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

            //if (entityPM.ChangeSetOp != ChangeSetOperation.Insert)
            //{
            //    if (!entityPM.ConcurrencyGUID.Equals(entityPOCO.ConcurrencyGUID))
            //    {
            //        string msg = CRMTranslateTextsClass.Translate("General.M.CantUpdateRecord", entityPM.Tenant);
            //        throw new OptimisticConcurrencyException(msg);
            //    }
            //}

            //base.CheckConcurrency(entityPM, entityPOCO);
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

            if (entityPM.TypeCode == "ASC")
            {
                tariffVersionPM.StartDate = null;
                tariffVersionPM.ExpirationDate = null;
            }

            entityPM.TariffVersions.Add(tariffVersionPM);

            //TariffVersionUpdateService tariffVersionUpdateService = new TariffVersionUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            //tariffVersionUpdateService.Update(tariffVersionPM, true);
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

                if (entityPM.TypeCode == "AFC")
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

                if (entityPM.TypeCode == "ASC")
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
                if(iPreviousVersion.StartDate.Value.Date>= iDraftVersion.StartDate.Value.Date)
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

            if(isExpirationDateValid)
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
            if (entityPM.TypeCode == "ASC")
            {
                ITariffModuleContext iContext = TariffModuleContext.GetContext(entityPM.Tenant);
                int iCount = (from d in iContext.Tariffs
                              where d.Tenant == entityPM.Tenant
                              && d.Id != entityPM.Id
                              && d.SellerId == entityPM.SellerId
                              && d.TypeCode == "ASC"
                              select d).Count();

                if (iCount >= 1)
                {
                    throw new ApplicationException("Tariff surcharge seller should be unique");
                }
            }
        }
    }
}
