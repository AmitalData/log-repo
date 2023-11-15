using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.VehicleInServiceReference;
using Logitude.Customs.Data.Repsitories;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class VP_NG_2690_VehicleInRequestService : RequestServiceBase
        <VP_NG_2690_MSG100_VehicleIn, UpdateDeleteVehicleRequestParams>
    {
        public VehiclePM _MyVehicle { get; set; }
        private ICustomContext _CustomContext { get; set; }

        public override VP_NG_2690_MSG100_VehicleIn GetRequest(UpdateDeleteVehicleRequestParams requestParams)
        {
            //Build request 2690- Send Vehicle Details
            _CustomContext = CustomContext.GetContext(requestParams.Tenant);
            var myVehicleQueryService = new VehicleQueryService(_CustomContext);
            string description = null;

            var myVP_NG_2690_MSG100_VehicleIn = new VP_NG_2690_MSG100_VehicleIn();
            myVP_NG_2690_MSG100_VehicleIn.RequestContentHeader = new RequestContentHeader() { Convertor = "1", RecieverID = new int[] { 1 } };
            myVP_NG_2690_MSG100_VehicleIn.VP_NG_2690_MSG100_Vehicle = new VP_MSG100_VehicleIn();

            _MyVehicle = myVehicleQueryService.GetSingle(requestParams.VehicleId, true, false);

            if (!requestParams.IsDelete)
            {
                List<VP_MSG100_VehicleInVehicleDetails> vehicleDetailsList = new List<VP_MSG100_VehicleInVehicleDetails>();
                VP_MSG100_VehicleInVehicleDetails vehicleDetails = GetVehicleDetails();
                vehicleDetailsList.Add(vehicleDetails);

                myVP_NG_2690_MSG100_VehicleIn.VP_NG_2690_MSG100_Vehicle.VehicleDetails = vehicleDetailsList.ToArray();
                if (string.IsNullOrWhiteSpace(_MyVehicle.RichbitFileNumber))
                {
                    description = "יצירת רכב " + _MyVehicle.VehicleChassisNumber;
                }
                else
                {
                    description = "עדכון פרטי רכב " + _MyVehicle.VehicleChassisNumber;
                }
            }
            else
            {
                List<long> vehicleToDeleteList = new List<long>();
                long richbitFileNumber;
                long.TryParse(_MyVehicle.RichbitFileNumber, out richbitFileNumber);
                vehicleToDeleteList.Add(richbitFileNumber);

                myVP_NG_2690_MSG100_VehicleIn.VP_NG_2690_MSG100_Vehicle.VehicleToDelete = vehicleToDeleteList.ToArray();
                description = "מחיקת רכב " + _MyVehicle.VehicleChassisNumber;
            }

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Vehicle");
            this.MyRequestSheetParam.EntityId1 = this._MyVehicle.Id;
            this.MyRequestSheetParam.RequestDescription = description;

            return myVP_NG_2690_MSG100_VehicleIn;
        }

        private VP_MSG100_VehicleInVehicleDetails GetVehicleDetails()
        {
            var clientQueryService = new ClientQueryService(_CustomContext);

            VP_MSG100_VehicleInVehicleDetails vehicleDetails = new VP_MSG100_VehicleInVehicleDetails();
            if (!string.IsNullOrWhiteSpace(_MyVehicle.RichbitFileNumber))
            {
                long richbitFileNumber;
                long.TryParse(_MyVehicle.RichbitFileNumber, out richbitFileNumber);
                vehicleDetails.richbitFileNumber = richbitFileNumber;
                vehicleDetails.richbitFileNumberSpecified = true;
            }
            vehicleDetails.vehicleChassisNumber = _MyVehicle.VehicleChassisNumber;
            int vehiclePoolType;
            int.TryParse(_MyVehicle.VehiclePoolTypeCode, out vehiclePoolType);
            vehicleDetails.vehiclePoolType = vehiclePoolType;
            if (!string.IsNullOrWhiteSpace(_MyVehicle.VehiclePriceListTypeCode))
            {
                int vehiclePriceListType;
                int.TryParse(_MyVehicle.VehiclePriceListTypeCode, out vehiclePriceListType);
                vehicleDetails.vehiclePriceListType = vehiclePriceListType;
                vehicleDetails.vehiclePriceListTypeSpecified = true;
            }
            int vehicleManufacturerId;
            int.TryParse(_MyVehicle.VehicleManufacturerCode, out vehicleManufacturerId);
            vehicleDetails.vehicleManufacturerId = vehicleManufacturerId;
            vehicleDetails.vehicleManufacturerIdSpecified = vehicleManufacturerId > 0 ? true : false;
            if (!string.IsNullOrWhiteSpace(_MyVehicle.ModelCode))
            {
                int modelCode;
                int.TryParse(_MyVehicle.ModelCode, out modelCode);
                vehicleDetails.modelCode = modelCode;
                vehicleDetails.modelCodeSpecified = true;
            }
            vehicleDetails.isABS = _MyVehicle.IsABS;
            vehicleDetails.isABSSpecified = _MyVehicle.IsABS == true ? true : false;
            vehicleDetails.airBags = _MyVehicle.AirBagsNumber;
            vehicleDetails.airBagsSpecified = vehicleDetails.airBags > 0 ? true : false;
            if (!string.IsNullOrWhiteSpace(_MyVehicle.ConverterTypeCode))
            {
                int converterType;
                int.TryParse(_MyVehicle.ConverterTypeCode, out converterType);
                vehicleDetails.converterType = converterType;
                vehicleDetails.converterTypeSpecified = true;
            }
            vehicleDetails.isArmoredVehicle = _MyVehicle.IsArmoredVehicle;
            vehicleDetails.isArmoredVehicleSpecified = _MyVehicle.IsArmoredVehicle == true ? true : false;
            vehicleDetails.isLoweringVehicleForInvalid = _MyVehicle.IsLoweringVehicleForInvalid;
            vehicleDetails.isLoweringVehicleForInvalidSpecified = _MyVehicle.IsLoweringVehicleForInvalid == true ? true : false;
            vehicleDetails.greenIndex = _MyVehicle.GreenIndex;
            vehicleDetails.greenIndexSpecified = _MyVehicle.GreenIndex > 0 ? true : false;
            vehicleDetails.greenIndexGroup = _MyVehicle.GreenIndexGroup;
            vehicleDetails.greenIndexGroupSpecified = _MyVehicle.GreenIndexGroup > 0 ? true : false;
            vehicleDetails.isStabilityControl = _MyVehicle.IsStabilityControl;
            vehicleDetails.isStabilityControlSpecified = _MyVehicle.IsStabilityControl = true ? true : false;
            vehicleDetails.israelEnterDate = _MyVehicle.IsraelEnterDate;
            vehicleDetails.israelEnterDateSpecified = _MyVehicle.IsraelEnterDate != null ? true : false;
            if (_MyVehicle.EngineCapacity.HasValue)
            {
                vehicleDetails.engineCapacity = (int)_MyVehicle.EngineCapacity;
                vehicleDetails.engineCapacitySpecified = true;
            }
            if (_MyVehicle.VehiclePowerKW.HasValue)
            {
                int vehiclePower = Convert.ToInt32(_MyVehicle.VehiclePowerKW);
                vehicleDetails.vehiclePowerKW = vehiclePower;
                vehicleDetails.vehiclePowerKWSpecified = vehiclePower > 0 ? true : false;
            }

            //vehicleDetails.vehiclePowerKWSpecified = _MyVehicle.VehiclePowerKW > 0 ? true : false;
            if (!string.IsNullOrWhiteSpace(_MyVehicle.VehicleTecnologyTypeCode))
            {
                int vehicleTecnologyType;
                int.TryParse(_MyVehicle.VehicleTecnologyTypeCode, out vehicleTecnologyType);
                vehicleDetails.vehicleTecnologyType = vehicleTecnologyType;
                vehicleDetails.vehicleTecnologyTypeSpecified = true;
            }
            if (!string.IsNullOrWhiteSpace(_MyVehicle.FuelTypeCode))
            {
                int fuelTypeCode;
                int.TryParse(_MyVehicle.FuelTypeCode, out fuelTypeCode);
                vehicleDetails.fuelTypeCode = fuelTypeCode;
                vehicleDetails.fuelTypeCodeSpecified = true;
            }
            vehicleDetails.vehicleWindowNumber = _MyVehicle.VehicleWindowNumber;
            //vehicleDetails.richbitOpenDate // not exist in DB
            //vehicleDetails.richbitOpenDateSpecified

            CustomsCountryQueryService customsCountryQueryService = new CustomsCountryQueryService(_CustomContext);
            CustomsCountryPM customsCountryPM = customsCountryQueryService.GetSingleCustomsCountryWithTenant(_MyVehicle.ManufactureCountryCode, _MyVehicle.Tenant);
            if (customsCountryPM != null)
            {
                int malamId;
                int.TryParse(customsCountryPM.MalamId, out malamId);
                vehicleDetails.manufactureCountry = malamId;
                vehicleDetails.manufactureCountrySpecified = malamId > 0 ? true : false;
            }

            int medalNumber;
            int.TryParse(_MyVehicle.MedalNumber, out medalNumber);
            vehicleDetails.medalNumber = medalNumber;
            vehicleDetails.medalNumberSpecified = medalNumber > 0 ? true : false;
            int taxiMedalOwner;
            int.TryParse(_MyVehicle.TaxiMedalOwner, out taxiMedalOwner);
            vehicleDetails.taxiMedalOwner = taxiMedalOwner;
            vehicleDetails.taxiMedalOwnerSpecified = taxiMedalOwner > 0 ? true : false;
            vehicleDetails.commercialNickname = _MyVehicle.CommercialNickname;
            vehicleDetails.modelDescription = _MyVehicle.ModelDescription;
            vehicleDetails.numberOfSeats = _MyVehicle.NumberOfSeats;
            vehicleDetails.numberOfSeatsSpecified = _MyVehicle.NumberOfSeats > 0 ? true : false;
            if (_MyVehicle.TotalVehicleWeight > 0)
            {
                vehicleDetails.totalVehicleWeight = (int)_MyVehicle.TotalVehicleWeight;
                vehicleDetails.totalVehicleWeightSpecified = true;
            }
            vehicleDetails.selfVehicleWeight = _MyVehicle.SelfVehicleWeight;
            vehicleDetails.selfVehicleWeightSpecified = _MyVehicle.SelfVehicleWeight > 0 ? true : false;
            if (_MyVehicle.NumberOfWheels > 0)
            {
                vehicleDetails.numberOfWheels = (int)_MyVehicle.NumberOfWheels;
                vehicleDetails.numberOfWheelsSpecified = true;
            }
            if (_MyVehicle.VehicleManufactureDate != null)
            {
                vehicleDetails.vehicleManufactureDate = (DateTime)_MyVehicle.VehicleManufactureDate;
            }
            int vehicleTypeId;
            int.TryParse(_MyVehicle.VehicleTypeCode, out vehicleTypeId);
            vehicleDetails.vehicleTypeId = vehicleTypeId;
            vehicleDetails.vehicleTypeIdSpecified = vehicleTypeId > 0 ? true : false;
            if (_MyVehicle.TransmissionDateWithoutTax != null)
            {
                vehicleDetails.transmissionDateWithoutTax = _MyVehicle.TransmissionDateWithoutTax;
                vehicleDetails.transmissionDateWithoutTaxSpecified = true;
            }
            if (!string.IsNullOrWhiteSpace(_MyVehicle.ImporterIdentityId))
            {
                var clientPM = clientQueryService.GetSingle(_MyVehicle.ImporterIdentityId, false, false);
                int importerIdentity = 0;
                if (clientPM != null && !string.IsNullOrWhiteSpace(clientPM.Code))
                {
                    int.TryParse(clientPM.Code, out importerIdentity);
                    if (importerIdentity > 0)
                    {
                        vehicleDetails.importerIdentity = importerIdentity;
                        vehicleDetails.importerName = clientPM.FullName;
                    }
                }
                vehicleDetails.importerIdentitySpecified = importerIdentity > 0 ? true : false;
            }
            else
            {
                vehicleDetails.importerName = _MyVehicle.PassportName;
            }
            if (_MyVehicle.DateOnRoadAbroad != null)
            {
                vehicleDetails.dateOnRoadAbroad = _MyVehicle.DateOnRoadAbroad;
                vehicleDetails.dateOnRoadAbroadSpecified = true;
            }
            vehicleDetails.vehicleSafetyAccessoryPoints = _MyVehicle.VehicleSafetyAccessoryPoints;
            vehicleDetails.vehicleSafetyAccessoryPointsSpecified = _MyVehicle.VehicleSafetyAccessoryPoints > 0 ? true : false;
            ///vehicleDetails.ExternalAttachmentID = new string[] { "CUSD-30240-1" };// _MyVehicle.ExternalAttachmentID; // to do- send dociment
            vehicleDetails.ImporterPassportNumber = _MyVehicle.ImporterPassportNumber;
            vehicleDetails.ImporterPassportCountry = _MyVehicle.ImporterPassCountryCode;
            if (!string.IsNullOrWhiteSpace(_MyVehicle.ImporterPassportTypeCode))
            {
                int importerPassportTypeCode;
                if (int.TryParse(_MyVehicle.ImporterPassportTypeCode, out importerPassportTypeCode))
                {
                    vehicleDetails.ImporterPassportType = importerPassportTypeCode;
                    vehicleDetails.ImporterPassportTypeSpecified = true;
                }
            }
            vehicleDetails.IsSlipperClutch = _MyVehicle.IsSlipperClutch;
            vehicleDetails.IsSlipperClutchSpecified = _MyVehicle.IsSlipperClutch == true ? true : false;
            vehicleDetails.IsCBS = _MyVehicle.IsCBS;
            vehicleDetails.IsCBSSpecified = _MyVehicle.IsCBS == true ? true : false;
            vehicleDetails.IsSteeringDamper = _MyVehicle.IsSteeringDamper;
            vehicleDetails.IsSteeringDamperSpecified = _MyVehicle.IsSteeringDamper == true ? true : false;
            vehicleDetails.IsTCS = _MyVehicle.IsTCS;
            vehicleDetails.IsTCSSpecified = _MyVehicle.IsTCS == true ? true : false;
            vehicleDetails.IsTPS = _MyVehicle.IsTPS;
            vehicleDetails.IsTPSSpecified = _MyVehicle.IsTPS == true ? true : false;
            vehicleDetails.VehicleCategory = _MyVehicle.VehicleCategory;
            vehicleDetails.VehicleMaxPowerKW = _MyVehicle.VehicleMaxPowerKW;
            vehicleDetails.VehicleMaxPowerKWSpecified = _MyVehicle.VehicleMaxPowerKW > 0 ? true : false;
            vehicleDetails.isThreeWheeledForReduction = _MyVehicle.IsThreeWheeledForReduction;
            vehicleDetails.isThreeWheeledForReductionSpecified = _MyVehicle.IsThreeWheeledForReduction == true ? true : false;
            //Get VehicleSafetyAccessoryInstallation Details
            if (_MyVehicle.VehicleSafetyAccessories != null && _MyVehicle.VehicleSafetyAccessories.Count() > 0)
            {
                List<VP_MSG100_VehicleInVehicleDetailsVehicleSafetyAccessoryInstallation> vehicleSafetyAccessoryList = new List<VP_MSG100_VehicleInVehicleDetailsVehicleSafetyAccessoryInstallation>();
                foreach (var safetyItem in _MyVehicle.VehicleSafetyAccessories)
                {
                    VP_MSG100_VehicleInVehicleDetailsVehicleSafetyAccessoryInstallation vehicleSafetyAccessory = new VP_MSG100_VehicleInVehicleDetailsVehicleSafetyAccessoryInstallation();
                    int vehicleSafetyAccessoryId;
                    int.TryParse(safetyItem.VehicleSafetyAccessoryCode, out vehicleSafetyAccessoryId);
                    vehicleSafetyAccessory.VehicleSafetyAccessoryId = vehicleSafetyAccessoryId;
                    int vehicleSafetyAccessoryInstallationTypeId;
                    int.TryParse(safetyItem.VehicleSafAccessoryInstlTypCod, out vehicleSafetyAccessoryInstallationTypeId);
                    vehicleSafetyAccessory.VehicleSafetyAccessoryInstallationTypeId = vehicleSafetyAccessoryInstallationTypeId;
                    vehicleSafetyAccessoryList.Add(vehicleSafetyAccessory);
                }
                vehicleDetails.VehicleSafetyAccessoryInstallation = vehicleSafetyAccessoryList.ToArray();
            }

            //Get VehicleOwner Details
            if (_MyVehicle.VehicleOwners != null && _MyVehicle.VehicleOwners.Count() > 0)
            {
                List<VP_MSG100_VehicleInVehicleDetailsVehicleOwner> vehicleOwnerList = new List<VP_MSG100_VehicleInVehicleDetailsVehicleOwner>();
                foreach (var ownerItem in _MyVehicle.VehicleOwners)
                {
                    VP_MSG100_VehicleInVehicleDetailsVehicleOwner vehicleOwner = new VP_MSG100_VehicleInVehicleDetailsVehicleOwner();
                    int externalID;
                    var clientIsFreeText = true;
                    if (clientIsFreeText)
                    {
                        int.TryParse(ownerItem.ClientId, out externalID);
                    }
                    else
                    {
                        var clientOwnerPM = clientQueryService.GetSingle(ownerItem.ClientId, false, false);
                        int.TryParse(clientOwnerPM.Code, out externalID);
                    }


                    vehicleOwner.externalID = externalID;
                    vehicleOwner.externalIDSpecified = externalID > 0 ? true : false;
                    vehicleOwner.lastNameOrCorporationName = ownerItem.LastNameOrCorporationName;
                    vehicleOwner.firstName = ownerItem.FirstName;
                    vehicleOwner.isMain = ownerItem.IsMain;
                    vehicleOwner.passportNumber = ownerItem.PassportNumber;
                    vehicleOwner.passportCountry = ownerItem.PassCountryCode;
                    if (!string.IsNullOrWhiteSpace(ownerItem.ImporterPassportTypeCode))
                    {
                        int importerPassportTypeCode;
                        if (int.TryParse(ownerItem.ImporterPassportTypeCode, out importerPassportTypeCode))
                        {
                            vehicleOwner.passportType = importerPassportTypeCode;
                            vehicleOwner.passportTypeSpecified = true;
                        }
                    }
                    vehicleOwnerList.Add(vehicleOwner);
                }
                vehicleDetails.VehicleOwner = vehicleOwnerList.ToArray();
            }

            //Get Connected Documents
            vehicleDetails.ExternalAttachmentID = GetVehicleAttachment();

            return vehicleDetails;
        }

        private string[] GetVehicleAttachment()
        {
            var vehicleAttachmentList = new List<string>();
            var customsDocumentQueryService = new CustomsDocumentQueryService(_CustomContext);

            //Get Vehicle Attachments
            var customsDocumentPMList = customsDocumentQueryService.GetCustomsDocumentPMListWithoutRequestedDoc(new GetTicketsParams() { ParentEntityId = this._MyVehicle.Id, ParentEntityCode = "Vehicle" }, this._MyVehicle.Tenant);
            int counter = 1;
            foreach (CustomsDocumentPM customsDocumentPM in customsDocumentPMList)
            {
                if (counter > 5) break;
                if (!string.IsNullOrWhiteSpace(customsDocumentPM.CustomsDocId))
                {
                    vehicleAttachmentList.Add(customsDocumentPM.ExternalAttachmentId);
                }
                counter++;
            }

            return vehicleAttachmentList.ToArray();
        }
    }
}
