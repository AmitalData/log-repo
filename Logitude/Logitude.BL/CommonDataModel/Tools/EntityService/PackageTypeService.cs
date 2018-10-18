using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.BL.Helpers;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Linq;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class PackageTypeService
    {
        bool isNewEntity;
        private int tenant;
        public PackageType Poco { get; set; }
        private PackageTypePM entityPM;
        private ICommonDataContext objectContext;
        private PackageTypeRepository entityRepository;
        private MeasurementRepository measurementRepository;
        public PackageTypeService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new PackageTypeRepository(objectContext);
            this.measurementRepository = new MeasurementRepository(objectContext);
        }

        public void Create(PackageTypePM entityPM)
        {
            this.isNewEntity = true;
            this.entityPM = entityPM;

            this.CheckIfExists();

            this.Poco = new PackageType();
            this.entityPM.Id = IdCounter.GetNumber("PackageType", tenant).ToString();
            this.Poco.Id = entityPM.Id;

            PackageTypeValidating.Validate(entityPM);

            this.CheckMeasurement();

            if (!entityPM.IsHybrid)
            {
                PackageTypeTracing.Trace(entityPM, Poco, isNewEntity);
            }

            PackageTypeMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
            TableLastUpdateClass.UpdateTableHistory(tenant, "PackageType");
        }

        public void Update(PackageTypePM entityPM)
        {
            this.isNewEntity = false;
            this.entityPM = entityPM;
            this.Poco = entityRepository.GetSinglePackageType(entityPM.Id, entityPM.Tenant);

            PackageTypeValidating.Validate(entityPM);

            this.CheckMeasurement();

            string entityName = "PackageType" + entityPM.Id + tenant;
            string entityCode = "PackageType" + entityPM.Code + tenant;
            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }

            if (CacheManager.CacheWrapper.Get(entityCode) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityCode);
            }

            if (!entityPM.IsHybrid)
            {
                PackageTypeTracing.Trace(entityPM, Poco, isNewEntity);
            }

            PackageTypeMapping.MapEntity(entityPM, Poco, isNewEntity);

            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
            TableLastUpdateClass.UpdateTableHistory(tenant, "PackageType");
        }

        private void CheckMeasurement()
        {
            if (entityPM.IsContainer)
            {
                if (this.isNewEntity)
                {
                    this.CreateMeasurement();
                }

                else
                {
                    if (entityPM.MeasurementId == null)
                    {
                        this.CreateMeasurement();
                    }

                    if (entityPM.InActive)
                    {
                        if (entityPM.MeasurementId != null)
                        {
                            Measurement measurement = measurementRepository.GetSingleMeasurement(entityPM.MeasurementId, entityPM.Tenant);
                            measurement.InActive = true;
                            measurementRepository.Update(measurement);
                            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "Measurement");
                        }
                    }
                }
            }
        }

        private void CreateMeasurement()
        {
            Measurement newMeasurement = new Measurement();
            newMeasurement.Id = IdCounter.GetNumber("Measurement", entityPM.Tenant).ToString();
            newMeasurement.Tenant = entityPM.Tenant;
            newMeasurement.IsContainer = true;
            newMeasurement.Name = entityPM.EnglishName;
            newMeasurement.Code = entityPM.Code;
            newMeasurement.ShortName = entityPM.EnglishName;
            newMeasurement.SearchFields = newMeasurement.Code + "," + newMeasurement.Name + "," + newMeasurement.ShortName;
            measurementRepository.Add(newMeasurement);
            measurementRepository.SubmitChanges();
            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "Measurement");

            entityPM.MeasurementId = newMeasurement.Id;
            this.Poco.MeasurementId = newMeasurement.Id;
        }

        private void CheckIfExists()
        {
            if (this.isNewEntity)
            {
                bool exist = (from a in entityRepository.GetPackageTypes(tenant)
                              where a.Code == entityPM.Code && a.Tenant == entityPM.Tenant
                              select a).Any();

                if (exist)
                {
                    string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyexists", tenant);
                    msg = msg.Replace("%Entity", "PackageType");
                    throw new Exception(msg);
                }
            }
        }
    }
}
