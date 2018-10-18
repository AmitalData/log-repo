
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.Repsitories;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class VehicleDataMapping: IMapping<VehiclePM, Vehicle>
   {

        public void CustomPMToPOCO(VehiclePM entityPM, Vehicle entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
          
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
             
            }



            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.ConcurrencyGUID);
            entityPOCO.ConcurrencyGUID = entityPM.NewConcurrencyGUID;//Guid.NewGuid().ToString();

            entityPM.ConcurrencyGUID = entityPOCO.ConcurrencyGUID;      
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
        }

        private static void BuildSearchFields(VehiclePM entityPM, Vehicle poco, bool isNewEntity)
        {
            string result = "";

            result = entityPM.ImporterName + "," + entityPM.MedalNumber + "," + entityPM.ModelCode + "," + entityPM.ModelDescription + "," + entityPM.RichbitFileNumber + "," + entityPM.StatusName + "," + entityPM.VehicleChassisNumber + "," + entityPM.VehicleManufacturerName + "," + entityPM.VehiclePoolTypeName + "," + entityPM.VehiclePriceListTypeName + "," + entityPM.VehicleTecnologyTypeName + "," + entityPM.VehicleTypeName
                 + "," + entityPM.TaxiMedalOwner + "," + entityPM.ImporterPassportNumber + "," + entityPM.ImporterPassCountryCode + "," + entityPM.ImporterPassCountryName + "," + entityPM.ImporterPassportTypeCode + "," + entityPM.ImporterPassportTypeName + "," + entityPM.VehicleCategory;

            DeclarationQueryService declarationQuery = new DeclarationQueryService(poco.Tenant);
            string customFileNo = declarationQuery.GetCustomFileNoByDeclarationId(entityPM.DeclarationId, entityPM.Tenant);

            result = result + "," + customFileNo;

            entityPM.SearchFields = result.ToLower();
            poco.SearchFields = entityPM.SearchFields;

        }
        public void CustomPOCOToPM(VehiclePM entityPM, Vehicle entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.VehiclePoolTypeName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.VehiclePriceListTypeName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.VehicleManufacturerName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ConverterTypeName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.VehicleTecnologyTypeName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ManufactureCountryName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ImporterName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.StatusName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.FuelTypeName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.VehicleTypeName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.NewConcurrencyGUID);


            VehiclePoolTypeQueryService vehiclePoolTypeQueryService = new VehiclePoolTypeQueryService(entityPOCO.Tenant);
            VehiclePoolTypePM vehiclePoolType = vehiclePoolTypeQueryService.GetSingle(entityPOCO.VehiclePoolTypeCode, false, true);
            if (vehiclePoolType != null)
            {
                entityPM.VehiclePoolTypeName = vehiclePoolType.LocalName;

            }

            VehiclePriceListTypeQueryService vehiclePriceListTypeQueryService = new VehiclePriceListTypeQueryService(entityPOCO.Tenant);
            VehiclePriceListTypePM vehiclePriceListType = vehiclePriceListTypeQueryService.GetSingle(entityPOCO.VehiclePriceListTypeCode, false, true);
            if (vehiclePriceListType != null)
            {
                entityPM.VehiclePriceListTypeName = vehiclePriceListType.LocalName;

            }

            VehicleManufacturerQueryService vehicleManufacturerQueryService = new VehicleManufacturerQueryService(entityPOCO.Tenant);
            VehicleManufacturerPM vehicleManufacturer = vehicleManufacturerQueryService.GetSingle(entityPOCO.VehicleManufacturerCode, false, true);
            if (vehicleManufacturer != null)
            {
                entityPM.VehicleManufacturerName = vehicleManufacturer.LocalName;

            }

            ConverterTypeQueryService converterTypeQueryService = new ConverterTypeQueryService(entityPOCO.Tenant);
            ConverterTypePM converterType = converterTypeQueryService.GetSingle(entityPOCO.ConverterTypeCode, false, true);
            if (converterType != null)
            {
                entityPM.ConverterTypeName = converterType.LocalName;

            }

            VehicleTecnologyTypeQueryService vehicleTecnologyTypeQueryService = new VehicleTecnologyTypeQueryService(entityPOCO.Tenant);
            VehicleTecnologyTypePM vehicleTecnologyType = vehicleTecnologyTypeQueryService.GetSingle(entityPOCO.VehicleTecnologyTypeCode, false, true);
            if (vehicleTecnologyType != null)
            {
                entityPM.VehicleTecnologyTypeName = vehicleTecnologyType.LocalName;

            }

            CustomsCountryQueryService customsCountryQueryService = new CustomsCountryQueryService(entityPOCO.Tenant);
            CustomsCountryPM customsCountry = customsCountryQueryService.GetSingle(entityPOCO.ManufactureCountryCode, false, true);
            if (customsCountry != null)
            {
                entityPM.ManufactureCountryName = customsCountry.LocalName;

            }

            ClientQueryService clientQueryService = new ClientQueryService(entityPOCO.Tenant);
            ClientPM client = clientQueryService.GetSingle(entityPOCO.ImporterIdentityId, false, true);
            if (client != null)
            {
                entityPM.ImporterName = client.FullName;

            }



            VehicleStatusQueryService vehicleStatusQueryService = new VehicleStatusQueryService(entityPOCO.Tenant);
            VehicleStatusPM vehicleStatus = vehicleStatusQueryService.GetSingle(entityPOCO.StatusCode, false, true);
            if (vehicleStatus != null)
            {
                entityPM.StatusName = vehicleStatus.LocalName;

            }


            FuelTypeQueryService fuelTypeQueryService = new FuelTypeQueryService(entityPOCO.Tenant);
            FuelTypePM fuelType = fuelTypeQueryService.GetSingle(entityPOCO.FuelTypeCode, false, true);
            if (fuelType != null)
            {
                entityPM.FuelTypeName = fuelType.LocalName;

            }

            VehicleTypeQueryService vehicleTypeQueryService = new VehicleTypeQueryService(entityPOCO.Tenant);
            VehicleTypePM vehicleType = vehicleTypeQueryService.GetSingle(entityPOCO.VehicleTypeCode, false, true);
            if (vehicleType != null)
            {
                entityPM.VehicleTypeName = vehicleType.LocalName;

            }

            if (entityPOCO.ImporterPassCountryCode != null)
            {
                CustomsCountryQueryService passCustomsCountryQueryService = new CustomsCountryQueryService(entityPOCO.Tenant);
                CustomsCountryPM passCustomsCountry = customsCountryQueryService.GetSingle(entityPOCO.ImporterPassCountryCode, false, true);
                if (passCustomsCountry != null)
                {
                    entityPM.ImporterPassCountryName = passCustomsCountry.LocalName;

                }
            }

            if (entityPOCO.ImporterPassportTypeCode != null)
            {
                PassportTypeQueryService passportTypeQueryService = new PassportTypeQueryService(entityPOCO.Tenant);
                PassportTypePM passportType = passportTypeQueryService.GetSingle(entityPOCO.ImporterPassportTypeCode, false, true);
                if (passportType != null)
                {
                    entityPM.ImporterPassportTypeName = passportType.LocalName;

                }
            }

            entityPM.NewConcurrencyGUID = Guid.NewGuid().ToString();

        }
   }


}
   