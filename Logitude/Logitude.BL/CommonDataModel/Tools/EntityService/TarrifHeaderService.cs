using System.Collections.Generic;
using System.ServiceModel.DomainServices.Server;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class TarrifHeaderService : DomainService
    {          
        bool isNewEntity;
        private int tenant;
        public TarrifHeader Poco { get; set; }

        private TarrifHeaderPM entityPm;
        private ICommonDataContext objectContext;
        private TarrifHeaderRepository entityRepository;
        private TarrifChargeRepository tarrifChargeRepository;
        private TarrifFromToRepository tarrifFromToRepository;
        
        public TarrifHeaderService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new TarrifHeaderRepository(objectContext);
            this.tarrifChargeRepository = new TarrifChargeRepository(objectContext);
            this.tarrifFromToRepository = new TarrifFromToRepository(objectContext);
        }

        public void Create(TarrifHeaderPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;           

            this.entityPm.Id = IdCounter.GetNumber("TarrifHeader", tenant).ToString();
            this.Poco = new TarrifHeader();
            this.Poco.Id = this.entityPm.Id;

            if (entityPM.TarrifCharges != null)
            {
                foreach (TarrifChargePM charge in entityPM.TarrifCharges)
                {
                    TarrifCharge newCharge = new TarrifCharge()
                    {
                        ChargesTypeId = charge.ChargesTypeId,
                        CurrencyId = charge.CurrencyId,
                        Id = IdCounter.GetNumber("TarrifCharge", entityPM.Tenant).ToString(),
                        MaxPrice = charge.MaxPrice,
                        MeasurementId = charge.MeasurementId,
                        MinPrice = charge.MinPrice,
                        TarrifHeaderId = this.Poco.Id,
                        Tenant = entityPM.Tenant,
                        UnitPrice = charge.UnitPrice,

                    };

                    charge.Id = newCharge.Id;
                    tarrifChargeRepository.Add(newCharge);
                }
            }

            if (entityPM.TarrifFromToes != null)
            {
                foreach (TarrifFromToPM fromTo in entityPM.TarrifFromToes)
                {
                    TarrifFromTo newFromTo = new TarrifFromTo()
                    {
                        CountryId = fromTo.CountryId,
                        Id = IdCounter.GetNumber("TarrifFromTo", entityPM.Tenant).ToString(),
                        PortId = fromTo.PortId,
                        TarrifFromToTypeCode = fromTo.TarrifFromToTypeCode,
                        TarrifHeaderId = this.Poco.Id,
                        Tenant = entityPM.Tenant,

                    };

                    fromTo.Id = newFromTo.Id;
                    tarrifFromToRepository.Add(newFromTo);
                }
            }

            TarrifHeaderValidating.Validate(entityPM);
            TarrifHeaderTracing.Trace(entityPM, Poco, isNewEntity);
            TarrifHeaderMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        private List<TarrifChargePM> tarrifChrgeList;
        private List<TarrifFromToPM> tarrrifFromToList;
        public void SetChangeSet(List<TarrifChargePM> tarrifChrgeList, List<TarrifFromToPM> tarrrifFromToList)
        {
            this.tarrifChrgeList = tarrifChrgeList;
            this.tarrrifFromToList = tarrrifFromToList;
        }

        public void Update(TarrifHeaderPM entityPM, bool mapComposition = false)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleTarrifHeader(entityPM.Id);

            if (mapComposition)
            {
                this.tarrifChrgeList = entityPM.TarrifCharges;
                this.tarrrifFromToList = entityPM.TarrifFromToes;
            }

            this.UpdateTarrifCharges();
            this.TarrifFromToes();

            TarrifHeaderValidating.Validate(entityPM);
            TarrifHeaderTracing.Trace(entityPM, Poco, isNewEntity);        
            TarrifHeaderMapping.MapEntity(entityPM, Poco, isNewEntity);
                  
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

        private void UpdateTarrifCharges()
        {
            if (tarrifChrgeList != null)
            {
                TarrifChargeService chargesService = new TarrifChargeService(objectContext, entityPm.Tenant);

                foreach (TarrifChargePM r in tarrifChrgeList)
                {
                    switch (r.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                chargesService.Create(r);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                chargesService.Update(r);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                TarrifCharge ch = tarrifChargeRepository.GetSingleTarrifCharge(r.Id);
                                tarrifChargeRepository.Remove(ch);
                                break;
                            }

                        case ChangeSetOperation.None:
                            {
                                break;
                            }

                        default:
                            {
                                break;
                            }
                    }
                }
            }
        }

        private void TarrifFromToes()
        {
            if (tarrrifFromToList != null)
            {
                TarrifFromToService fromToService = new TarrifFromToService(objectContext, entityPm.Tenant);

                foreach (TarrifFromToPM r in tarrrifFromToList)
                {
                    switch (r.changeOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                fromToService.Create(r);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                fromToService.Update(r);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                TarrifFromTo ft = tarrifFromToRepository.GetSingleTarrifFromTo(r.Id);
                                tarrifFromToRepository.Remove(ft);
                                break;
                            }

                        case ChangeSetOperation.None:
                            {
                                break;
                            }

                        default:
                            {
                                break;
                            }
                    }
                }
            }
        }
    }
}
