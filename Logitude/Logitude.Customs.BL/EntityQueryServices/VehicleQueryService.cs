using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
     public partial class VehicleQueryService
    {

         public override void GetComposition(EntityKeyFields entityKeys, VehiclePM entityPM)
         {
             ICustomContext context = MainContext as CustomContext;
             VehicleKeys vehicleKeys = entityKeys as VehicleKeys;
             VehicleOwnerQueryService vehicleOwnerQueryService = new VehicleOwnerQueryService(context);
             entityPM.VehicleOwners = vehicleOwnerQueryService.GetMulti(vehicleKeys, true);
             VehicleSafetyAccessoryQueryService VehicleSafetyAccessoryQueryService = new VehicleSafetyAccessoryQueryService(context);
             entityPM.VehicleSafetyAccessories = VehicleSafetyAccessoryQueryService.GetMulti(vehicleKeys, true);

             if (entityPM.VehicleOwners == null)
             {
                 entityPM.VehicleOwners = new List<VehicleOwnerPM>();
             }

             if (entityPM.VehicleOwners.Count > 0)
             {
                 entityPM.LastOwnerLineNumber = entityPM.VehicleOwners.Max(m => m.LineNumber);
             }

             if (entityPM.VehicleSafetyAccessories == null)
             {
                 entityPM.VehicleSafetyAccessories = new List<VehicleSafetyAccessoryPM>();
             }

             if (entityPM.VehicleSafetyAccessories.Count > 0)
             {
                 entityPM.LastSaftyLineNumber = entityPM.VehicleSafetyAccessories.Max(m => m.LineNumber);
             }



             base.GetComposition(entityKeys, entityPM);
         }

         public VehiclePM GetVehicleByVehicleChassisNumberOrRichbitFileNumber(string vehicleChassisNumber, string richbitFileNumber, int tenant)
          {
             Vehicle entity = repository.GetSingleVehicleByVehicleChassisNumberOrRichbitFileNumber(vehicleChassisNumber, richbitFileNumber, tenant);
             VehiclePM entityPM = null;
             if (entity != null)
             {
                entityPM = new VehiclePM();
                VehicleDataMapping mappingClass = new EntityDataMappings.VehicleDataMapping();
                mappingClass.CustomPOCOToPM(entityPM, entity);
                mappingClass.POCOToPM(entityPM, entity);
                 //entityPM = new VehiclePM()
                 //{
                 //    Id = entity.Id,
                 //    Tenant = entity.Tenant,
                 //    VehicleChassisNumber = entity.VehicleChassisNumber,
                 //    RichbitFileNumber = entity.RichbitFileNumber,
                 //    StatusCode = entity.StatusCode,
                 //    StatusName = entity.VehicleStatus != null ? entity.VehicleStatus.LocalName : null,
                 //    DeclarationId = entity.DeclarationId,
                 //    CustomFileNumber = entity.Declaration != null ? entity.Declaration.CustomFileNo : null,
                 //    VehicleManufacturerCode = entity.VehicleManufacturerCode,
                 //    ManufactureCountryCode = entity.ManufactureCountryCode,
                 //    TotalVehicleWeight = entity.TotalVehicleWeight,
                 //    VehicleManufactureDate = entity.VehicleManufactureDate,
                 //    ImporterIdentityId = entity.ImporterIdentityId,
                 //    VehiclePoolTypeCode = entity.VehiclePoolTypeCode,
                 //    EngineCapacity = entity.EngineCapacity,
                 //    ModelDescription = entity.ModelDescription,
                 //    NumberOfWheels = entity.NumberOfWheels,
                 //    VehicleTypeCode = entity.VehicleTypeCode,
                 //    ConcurrencyGUID = entity.ConcurrencyGUID ,
                 //    NewConcurrencyGUID = Guid.NewGuid().ToString(),
                     
                 //};
             }

             return entityPM;
         }

        //public List<VehiclePM> GetVehiclesByRichbitFileNumber(string declarationId, string[] richbitFileNumbers, string[] chassissNumbers, int tenant)
        //{
        //    List<Vehicle> vehicles = repository.GetVehiclesByRichbitFileNumbers(declarationId, richbitFileNumbers, chassissNumbers, tenant);

        //    List<VehiclePM> vehiclesList = new List<VehiclePM>();
        //    VehiclePM vehicleList = null;
        //    if (vehicles.Count > 0)
        //    {
        //        foreach (Vehicle item in vehicles)
        //        {
        //            vehicleList = new VehiclePM()
        //            {
        //                Id = item.Id,
        //                Tenant = item.Tenant,
        //                VehicleChassisNumber = item.VehicleChassisNumber,
        //                RichbitFileNumber = item.RichbitFileNumber,
        //                StatusCode = item.StatusCode,
        //                StatusName = item.VehicleStatus != null ? item.VehicleStatus.LocalName : null,
        //                DeclarationId = item.DeclarationId,
        //                CustomFileNumber = item.Declaration != null ? item.Declaration.CustomFileNo : null,
        //                VehicleManufacturerCode = item.VehicleManufacturerCode,
        //                ManufactureCountryCode = item.ManufactureCountryCode,
        //                TotalVehicleWeight = item.TotalVehicleWeight,
        //                VehicleManufactureDate = item.VehicleManufactureDate,
        //                ImporterIdentityId = item.ImporterIdentityId,
        //                VehiclePoolTypeCode = item.VehiclePoolTypeCode,
        //                EngineCapacity = item.EngineCapacity,
        //                ModelDescription = item.ModelDescription,
        //                NumberOfWheels = item.NumberOfWheels,
        //                VehicleTypeCode = item.VehicleTypeCode,
        //                ConcurrencyGUID = item.ConcurrencyGUID,
        //                NewConcurrencyGUID = Guid.NewGuid().ToString(),
        //            };

        //            vehiclesList.Add(vehicleList);
        //        }

        //    }

        //    return vehiclesList;
        //}

        public List<VehiclePM> GetVehiclesByRichbitFileNumbers(string[] richbitFileNumbers, int tenant)
        {
            List<Vehicle> vehicles = repository.GetVehiclesByRichbitFileNumbers(richbitFileNumbers, tenant);

            List<VehiclePM> vehiclesList = new List<VehiclePM>();
            VehiclePM vehicleList = null;
            if (vehicles.Count > 0)
            {
                foreach (Vehicle item in vehicles)
                {
                    vehicleList = new VehiclePM()
                    {
                        Id = item.Id,
                        Tenant = item.Tenant,
                        VehicleChassisNumber = item.VehicleChassisNumber,
                        RichbitFileNumber = item.RichbitFileNumber,
                        StatusCode = item.StatusCode,
                        StatusName = item.VehicleStatus != null ? item.VehicleStatus.LocalName : null,
                        DeclarationId = item.DeclarationId,
                        CustomFileNumber = item.Declaration != null ? item.Declaration.CustomFileNo : null,
                        VehicleManufacturerCode = item.VehicleManufacturerCode,
                        ManufactureCountryCode = item.ManufactureCountryCode,
                        TotalVehicleWeight = item.TotalVehicleWeight,
                        VehicleManufactureDate = item.VehicleManufactureDate,
                        ImporterIdentityId = item.ImporterIdentityId,
                        VehiclePoolTypeCode = item.VehiclePoolTypeCode,
                        EngineCapacity = item.EngineCapacity,
                        ModelDescription = item.ModelDescription,
                        NumberOfWheels = item.NumberOfWheels,
                        VehicleTypeCode = item.VehicleTypeCode,
                        ConcurrencyGUID = item.ConcurrencyGUID,
                        NewConcurrencyGUID = Guid.NewGuid().ToString(),
                    };

                    vehiclesList.Add(vehicleList);
                }

            }

            return vehiclesList;
        }


        public List<VehicleList> GetVehiclesForSelection(int tenant)
         {
             List<Vehicle> vehicles = repository.GetVehiclesForSelection(tenant);
             List<VehicleList> vehiclesList = new List<VehicleList>();
             VehicleList vehicleList = null;
             if (vehicles.Count > 0)
             {
                 foreach (Vehicle item in vehicles)
                 {
                     vehicleList = new VehicleList()
                     {
                         Id = item.Id,
                         DeclarationId = item.DeclarationId,
                         CustomFileNumber = item.Declaration != null ? item.Declaration.CustomFileNo : null,
                         VehicleChassisNumber = item.VehicleChassisNumber,
                         RichbitFileNumber = item.RichbitFileNumber,
                         Tenant = item.Tenant,
                         StatusCode = item.StatusCode,
                         StatusName = item.VehicleStatus != null ? item.VehicleStatus.LocalName : null,
                         ImporterName = item.Client != null? item.Client.FullName : null,
                         VehiclePoolTypeName = item.VehiclePoolType != null? item.VehiclePoolType.LocalName : null,
                         VehicleManufacturerName = item.VehicleManufacturer != null? item.VehicleManufacturer.LocalName : null,
                         ModelCode = item.ModelCode,
                         VehicleWindowNumber = item.VehicleWindowNumber,
                 
                     };
             
                     vehiclesList.Add(vehicleList);
                 }

             }
             return vehiclesList;
         }

         public string GetVehicleIdByChassisNumber(string vehicleChassisNumber, int tenant)
         {
             if (string.IsNullOrEmpty(vehicleChassisNumber)) return "";
             return repository.GetVehicleIdByChassisNumber(vehicleChassisNumber, tenant);
         }
        
         public string GetVehicleIdByRichbitFileNumber(string richbitFileNumber, int tenant)
         {
             if (string.IsNullOrEmpty(richbitFileNumber)) return "";
             return repository.GetVehicleIdByRichbitFileNumber(richbitFileNumber, tenant);
         }
    }
}
