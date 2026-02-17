	using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityLists;

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class VehicleListQueryService
    {
	    private IQueryable<VehicleList> GetIqueryableList(IQueryable<Vehicle> iQueryable)
        {
            IQueryable<VehicleList> query = (from a in iQueryable.Include("VehicleManufacturer").Include("Client").Include("VehiclePoolType")
                                             .Include("ConverterType").Include("CustomsCountry").Include("VehicleStatus").Include("VehiclePriceListType").Include("VehicleTecnologyType").Include("VehicleType").Include("PassportCountry").Include("PassportType")
                                             join d in context.Declarations
                                             on a.DeclarationId equals d.Id into xy
                                             from s in xy.DefaultIfEmpty()

                                             select new VehicleList()
                                             {
                                                 Id = a.Id,
                                                 Tenant = a.Tenant,
                                                 AirBagsNumber = a.AirBagsNumber,
                                                 DateOnRoadAbroad = a.DateOnRoadAbroad,
                                                 IsABS = a.IsABS,
                                                 IsArmoredVehicle = a.IsArmoredVehicle,
                                                 VehicleSafetyAccessoryPoints = a.VehicleSafetyAccessoryPoints,
                                                 CommercialNickname = a.CommercialNickname,
                                                 ConverterTypeCode = a.ConverterTypeCode,
                                                 EngineCapacity = a.EngineCapacity,
                                                 FuelTypeCode = a.FuelTypeCode,
                                                 GreenIndex = a.GreenIndex,
                                                 GreenIndexGroup = a.GreenIndexGroup,
                                                 ImporterIdentityId = a.ImporterIdentityId,
                                                 ImporterName = a.Client != null ? a.Client.FullName : null,
                                                 IsLoweringVehicleForInvalid = a.IsLoweringVehicleForInvalid,
                                                 IsraelEnterDate = a.IsraelEnterDate,
                                                 IsStabilityControl = a.IsStabilityControl,
                                                 ManufactureCountryCode = a.ManufactureCountryCode,
                                                 MedalNumber = a.MedalNumber,
                                                 ModelCode = a.ModelCode,
                                                 ModelDescription = a.ModelDescription,
                                                 NumberOfSeats = a.NumberOfSeats,
                                                 NumberOfWheels = a.NumberOfWheels,
                                                 RichbitFileNumber = a.RichbitFileNumber,
                                                 SelfVehicleWeight = a.SelfVehicleWeight,
                                                 StatusCode = a.StatusCode,
                                                 TotalVehicleWeight = a.TotalVehicleWeight,
                                                 TransmissionDateWithoutTax = a.TransmissionDateWithoutTax,
                                                 VehicleChassisNumber = a.VehicleChassisNumber,
                                                 VehicleManufactureDate = a.VehicleManufactureDate,
                                                 VehicleManufacturerCode = a.VehicleManufacturerCode,
                                                 VehicleManufacturerName = a.VehicleManufacturer != null ? a.VehicleManufacturer.LocalName : null,
                                                 VehiclePoolTypeCode = a.VehiclePoolTypeCode,
                                                 VehiclePoolTypeName = a.VehiclePoolType != null ? a.VehiclePoolType.LocalName : null,
                                                 VehiclePowerKW = a.VehiclePowerKW,
                                                 VehiclePriceListTypeCode = a.VehiclePriceListTypeCode,
                                                 VehicleTecnologyTypeCode = a.VehicleTecnologyTypeCode,
                                                 VehicleTypeCode = a.VehicleTypeCode,
                                                 VehicleWindowNumber = a.VehicleWindowNumber,
                                                 ConverterTypeName = a.ConverterType != null ? a.ConverterType.LocalName : null,
                                                 ManufactureCountryName = a.CustomsCountry != null ? a.CustomsCountry.LocalName : null,
                                                 StatusName = a.VehicleStatus != null ? a.VehicleStatus.LocalName : null,
                                                 VehiclePriceListTypeName = a.VehiclePriceListType != null ? a.VehiclePriceListType.LocalName : null,
                                                 VehicleTecnologyTypeName = a.VehicleTecnologyType != null ? a.VehicleTecnologyType.LocalName : null,
                                                 VehicleTypeName = a.VehicleType != null ? a.VehicleType.LocalName : null,
                                                 SearchFields = a.SearchFields,
                                                 CustomFileNumber = s.CustomFileNo,
                                                 IsThreeWheeledForReduction = a.IsThreeWheeledForReduction,
                                                 TaxiMedalOwner = a.TaxiMedalOwner,
                                                 ImporterPassportNumber = a.ImporterPassportNumber,
                                                 ImporterPassCountryCode = a.ImporterPassCountryCode,
                                                 ImporterPassCountryName = a.PassportCountry != null ? a.PassportCountry.LocalName : null,
                                                 ImporterPassportTypeCode = a.ImporterPassportTypeCode,
                                                 ImporterPassportTypeName = a.PassportType != null ? a.PassportType.LocalName : null,
                                                 IsCBS = a.IsCBS,
                                                 IsSlipperClutch = a.IsSlipperClutch,
                                                 IsSteeringDamper = a.IsSteeringDamper,
                                                 IsTCS = a.IsTCS,
                                                 IsTPS = a.IsTPS,
                                                 VehicleCategory = a.VehicleCategory,
                                                 VehicleMaxPowerKW = a.VehicleMaxPowerKW,
                                                 PassportName = a.PassportName,
                                             });
            return query;
		}

        private IQueryable<Vehicle> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<Vehicle> iQueryable, int tenant)
        {
            return iQueryable;
		}
	}


}
	