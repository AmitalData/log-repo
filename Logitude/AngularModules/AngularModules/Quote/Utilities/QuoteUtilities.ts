import {QuotePM} from '../EntityPMs/QuotePM';
import {QuotePackagePM} from '../EntityPMs/QuotePackagePM';
import {QuoteChargePM} from '../EntityPMs/QuoteChargePM';
import {QuotePriceStepsPM} from '../EntityPMs/QuotePriceStepsPM';
import {QuoteStageList} from '../EntityLists/QuoteStageList';
import {QuoteStageListService} from '../Services/StandardLists/QuoteStageListService';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../Infrastructure/DataContracts/ApiQueryFilters';
import {InfraSettings} from '../../Infrastructure/Utilities/InfraSettings';
import {AppTool, ArrayTool} from '../../Infrastructure/Tools';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {PackageTypeListService} from '../../Common/Services/StandardLists/PackageTypeListService';
import {PackageTypeList} from '../../Common/EntityLists/PackageTypeList';
import {ShipmentPM} from '../../Shipment/EntityPMs/ShipmentPM';
import {ShipmentOrderPackagePM} from '../../Shipment/EntityPMs/ShipmentOrderPackagePM';
import {VatTypeList} from '../../Common/EntityLists/VatTypeList';
import {VatTypeListService} from '../../Common/Services/StandardLists/VatTypeListService';
import {VatTypesValidator} from '../../Infrastructure/Validators/VatTypesValidator';

export class QuoteUtilities {
    public static IsQuoteEditEnabled(entityPM: QuotePM) {
        var myResult: boolean = true;
        
        if (entityPM != null) {

            if (entityPM.IsClosed) {
                myResult = false;
            }

            else if (entityPM.IsCancelled) {
                myResult = false;
            }

            else if (entityPM.IsQuoteDataExternal && entityPM.IsQuoteDocumentExternal) {
                myResult = false;
            }

            else {

                var allStages = [];
                var myService: QuoteStageListService = new QuoteStageListService();
                myService.getAllFromCache().subscribe((resp: any) => {
                    if (!resp.HasError) {
                        allStages = resp.Result;
                    }
                });

                var mySentStageId = "";
                var mySentStage: QuoteStageList = allStages.filter(d => d.Code == "QTST")[0];

                if (mySentStage != null) {
                    mySentStageId = mySentStage.Id;
                }

                if (entityPM.StageId == mySentStageId) {
                    myResult = false;
                }
            }
        }

        return myResult;
    }

    public static IsLCLQuote(entityPM: QuotePM ) {
        var myResult = false;

        if (entityPM != null) {
            if (entityPM.TransportModeId.toUpperCase() == "A") {
                myResult = true;
            }

            else if (entityPM.TransportModeId.toUpperCase() == "O" && entityPM.ShipmentTypeId.toUpperCase() == "LCLD") {
                myResult = true;
            }

            else if (entityPM.TransportModeId.toUpperCase() == "I" && entityPM.ShipmentTypeId.toUpperCase() == "LTL") {
                myResult = true;
            }
        }

        return myResult;
    }

    public static IsInlandDomestic(entityPM: QuotePM) {
        var isInlandDomestic = false;

        if (entityPM.DirectionId == "D" && entityPM.TransportModeId == "I") {
            isInlandDomestic = true;
        }

        return isInlandDomestic;
    }

    public static RecalculateQuoteFields(entityPM: QuotePM) {
        if (entityPM != null) {
            if (entityPM.Ratio == null) {
                entityPM.Ratio = AppTool.GetRatio(entityPM.DirectionId, entityPM.TransportModeId, entityPM.ShipmentTypeId, InfraSettings.TenantPM.CountryCode);
            }

            entityPM.QuotePackages.forEach((item) => {
                item.Volume = AppTool.ComputePackageVolume(item.Quantity, item.Width, item.Height, item.Length, item.GrossWeight, entityPM.Ratio, entityPM.DimensionsUnitCode, entityPM.VolumeUnitCode, entityPM.GrossWeightUnitCode);
                item.VolumetricWeight = AppTool.ComputePackageVolumetricWeight(item.Quantity, item.Width, item.Height, item.Length, item.Volume, item.GrossWeight, entityPM.Ratio, entityPM.DimensionsUnitCode, entityPM.VolumeUnitCode, entityPM.GrossWeightUnitCode, entityPM.ChargeableWeightUnitCode);
            });
            
            if (entityPM.QuotePackages.length == 0) {
                entityPM.NumberOfPackages = null;
                entityPM.GrossWeight = null;
                entityPM.Volume = null;
                entityPM.VolumetricWeight = null;
                entityPM.ChargeableWeight = null;
            }

            else {                
                var myVolume: number = 0;
                var myQuantity: number = 0;
                var myVolumetricWeight: number = 0;
                var myGrossWeight: number = 0;
                
                entityPM.QuotePackages.forEach((item) => {
                    if (item.Volume != null) {
                        myVolume += item.Volume;
                    }

                    if (item.Quantity != null) {
                        myQuantity += item.Quantity;
                    }
                    
                    if (item.VolumetricWeight != null) {
                        myVolumetricWeight += item.VolumetricWeight;
                    }

                    if (item.GrossWeight != null) {
                        myGrossWeight += item.GrossWeight;
                    }
                })

                entityPM.Volume = myVolume;
                entityPM.NumberOfPackages = myQuantity;
                entityPM.VolumetricWeight = myVolumetricWeight;
                entityPM.GrossWeight = myGrossWeight;
                entityPM.ChargeableWeight = AppTool.CalculateChargeableWeight(entityPM.GrossWeight, entityPM.VolumetricWeight, entityPM.GrossWeightUnitCode, entityPM.ChargeableWeightUnitCode, entityPM.DirectionId, entityPM.TransportModeId);
            }
        }
    }
    public static OnQuoteRatioChanged(entityPM: QuotePM) {
        if (entityPM) {
            if (entityPM.Ratio == null) {
                entityPM.Ratio = AppTool.GetRatio(entityPM.DirectionId, entityPM.TransportModeId, entityPM.ShipmentTypeId, InfraSettings.TenantPM.CountryCode);
            }

            if (entityPM.QuotePackages.length == 0) {
                entityPM.NumberOfPackages = null;
                entityPM.GrossWeight = null;
                entityPM.Volume = null;
                entityPM.VolumetricWeight = null;
                entityPM.ChargeableWeight = null;
            }

            else {
                entityPM.QuotePackages.forEach((item) => {
                    if (item.Volume) {
                        item.VolumetricWeight = AppTool.GetWeightFromVolume(entityPM.VolumeUnitCode, entityPM.ChargeableWeightUnitCode, item.Volume, entityPM.Ratio);
                    }
                });

                entityPM.VolumetricWeight = AppTool.Round(ArrayTool.Sum(entityPM.QuotePackages, "VolumetricWeight"), 3);
                entityPM.ChargeableWeight = AppTool.CalculateChargeableWeight(entityPM.GrossWeight, entityPM.VolumetricWeight, entityPM.GrossWeightUnitCode, entityPM.ChargeableWeightUnitCode, entityPM.DirectionId, entityPM.TransportModeId);
            }
        }
    }

    public static CopyQuote(entityPM: QuotePM, copiedEntityPM: QuotePM) {
        entityPM.DirectionId = copiedEntityPM.DirectionId;
        entityPM.TransportModeId = copiedEntityPM.TransportModeId;
        entityPM.ShipmentTypeId = copiedEntityPM.ShipmentTypeId;
        entityPM.BranchId = copiedEntityPM.BranchId;
        entityPM.DepartmentId = copiedEntityPM.DepartmentId;
        entityPM.QuoteTypeCode = copiedEntityPM.QuoteTypeCode;
        entityPM.IncotermId = copiedEntityPM.IncotermId;
        entityPM.MoveTypeId = copiedEntityPM.MoveTypeId;
        entityPM.IsByKG = copiedEntityPM.IsByKG;
        entityPM.IsByContainer = copiedEntityPM.IsByContainer;
        entityPM.BaseShipmentNumber = copiedEntityPM.QuoteNumber;
        entityPM.IsAutomaticallyClosed = copiedEntityPM.IsAutomaticallyClosed;
        entityPM.AutomaticallyCloseDate = copiedEntityPM.AutomaticallyCloseDate;
        entityPM.AutomaticallyCloseDays = copiedEntityPM.AutomaticallyCloseDays;
        entityPM.VolumeUnitCode = copiedEntityPM.VolumeUnitCode;
        entityPM.DimensionsUnitCode = copiedEntityPM.DimensionsUnitCode;
        entityPM.GrossWeightUnitCode = copiedEntityPM.GrossWeightUnitCode;
        entityPM.ChargeableWeightInKG = copiedEntityPM.ChargeableWeightInKG;
        entityPM.ChargeableWeightUnitCode = copiedEntityPM.ChargeableWeightUnitCode;
        entityPM.Ratio = copiedEntityPM.Ratio;
        entityPM.DimFactor = copiedEntityPM.DimFactor;
        entityPM.IsDangerous = copiedEntityPM.IsDangerous;
        entityPM.DescriptionOfGoods = copiedEntityPM.DescriptionOfGoods;
        entityPM.PackageType1Id = copiedEntityPM.PackageType1Id;
        entityPM.PackageType2Id = copiedEntityPM.PackageType2Id;
        entityPM.PackageType3Id = copiedEntityPM.PackageType3Id;
        entityPM.PackageType4Id = copiedEntityPM.PackageType4Id;
        entityPM.PackageType5Id = copiedEntityPM.PackageType5Id;
        entityPM.PackageType1Quantity = copiedEntityPM.PackageType1Quantity;
        entityPM.PackageType2Quantity = copiedEntityPM.PackageType2Quantity;
        entityPM.PackageType3Quantity = copiedEntityPM.PackageType3Quantity;
        entityPM.PackageType4Quantity = copiedEntityPM.PackageType4Quantity;
        entityPM.PackageType5Quantity = copiedEntityPM.PackageType5Quantity;
        entityPM.Volume = copiedEntityPM.Volume;
        entityPM.VolumetricWeight = copiedEntityPM.VolumetricWeight;
        entityPM.GrossWeight = copiedEntityPM.GrossWeight;
        entityPM.GrossWeightInKG = copiedEntityPM.GrossWeightInKG;
        entityPM.GrossWeightPerTon = copiedEntityPM.GrossWeightPerTon;
        entityPM.ChargeableWeight = copiedEntityPM.ChargeableWeight;
        entityPM.NumberOfContainers = copiedEntityPM.NumberOfContainers;
        entityPM.NumberOfPackages = copiedEntityPM.NumberOfPackages;
        entityPM.ValueOfGoods = copiedEntityPM.ValueOfGoods;
        entityPM.ValueOfGoodsCurrencyId = copiedEntityPM.ValueOfGoodsCurrencyId;
        entityPM.IsChargesByVAT = copiedEntityPM.IsChargesByVAT;
        entityPM.GrossWeightEdited = copiedEntityPM.GrossWeightEdited;
        entityPM.ChargeableWeightEdited = copiedEntityPM.ChargeableWeightEdited;

        entityPM.IsSaleCurrencySameAsCost = copiedEntityPM.IsSaleCurrencySameAsCost;
        entityPM.SaleCurrencyId = copiedEntityPM.SaleCurrencyId;
        entityPM.ExchangeRate = copiedEntityPM.ExchangeRate;
        entityPM.IsFixedPrice = copiedEntityPM.IsFixedPrice;

        //entityPM.ShipperId = copiedEntityPM.ShipperId;
        //entityPM.ShipperContactId = copiedEntityPM.ShipperContactId;
        //entityPM.ShipperReference1 = copiedEntityPM.ShipperReference1;
        //entityPM.ShipperReference2 = copiedEntityPM.ShipperReference2;
        //entityPM.ShipperNote = copiedEntityPM.ShipperNote;
        //entityPM.ShipperMainAddressId = copiedEntityPM.ShipperMainAddressId;
        //entityPM.ShipperPickAddressId = copiedEntityPM.ShipperPickAddressId;

        //entityPM.ConsigneeId = copiedEntityPM.ConsigneeId;
        //entityPM.ConsigneeContactId = copiedEntityPM.ConsigneeContactId;
        //entityPM.ConsigneeReference1 = copiedEntityPM.ConsigneeReference1;
        //entityPM.ConsigneeReference2 = copiedEntityPM.ConsigneeReference2;
        //entityPM.ConsigneeNote = copiedEntityPM.ConsigneeNote;
        //entityPM.ConsigneeMainAddressId = copiedEntityPM.ConsigneeMainAddressId;
        //entityPM.ConsigneePickAddressId = copiedEntityPM.ConsigneePickAddressId;

        //entityPM.FromPort = copiedEntityPM.FromPort;
        //entityPM.FromPortCountry = copiedEntityPM.FromPortCountry;
        //entityPM.FromPortId = copiedEntityPM.FromPortId;
        //entityPM.FromPortName = copiedEntityPM.FromPortName;
        //entityPM.ToPort = copiedEntityPM.ToPort;
        //entityPM.ToPortCountry = copiedEntityPM.ToPortCountry;
        //entityPM.ToPortId = copiedEntityPM.ToPortId;
        //entityPM.ToPortName = copiedEntityPM.ToPortName;

        entityPM.MainCarriageCarrierId = copiedEntityPM.MainCarriageCarrierId;
    }
    public static CopyQuotePackages(entityPM: QuotePM, copiedEntityPM: QuotePM) {
        copiedEntityPM.QuotePackages.forEach(item => {
            var newPackage: QuotePackagePM = new QuotePackagePM(null);

            newPackage.Tenant = item.Tenant;
            newPackage.PackageTypeId = item.PackageTypeId;
            newPackage.PackageTypeName = item.PackageTypeName;
            newPackage.Quantity = item.Quantity;
            newPackage.GrossWeight = item.GrossWeight;
            newPackage.Volume = item.Volume;
            newPackage.Height = item.Height;
            newPackage.Length = item.Length;
            newPackage.Width = item.Width;

            entityPM.AddQuotePackagePM(newPackage);
        });
    }
    public static CopyQuoteCharges(entityPM: QuotePM, oldEntityPM: QuotePM, isCopySales: boolean, isCopyCost: boolean) {

        if (entityPM.QuoteCharges.length > 0) {
            entityPM.QuoteCharges.forEach(itemCharge => {
                itemCharge.QuoteChargePriceSteps.forEach((priceItem) => {
                    itemCharge.RemoveQuotePriceStepsPM(priceItem);
                });

                entityPM.RemoveQuoteChargePM(itemCharge);
            });
        }

        var allVatTypes: VatTypeList[] = VatTypesValidator.GetAllVatTypes();

        oldEntityPM.QuoteCharges.forEach(item => {

            var newChargePM: QuoteChargePM = new QuoteChargePM(null);
            newChargePM.Tenant = item.Tenant;
            newChargePM.ChargesTypeId = item.ChargesTypeId;
            newChargePM.VendorId = item.VendorId;
            newChargePM.SaleCurrencyId = item.SaleCurrencyId;
            newChargePM.CostCurrencyId = item.CostCurrencyId;
            newChargePM.SaleExchangeRate = item.SaleExchangeRate;
            newChargePM.CostExchangeRate = item.CostExchangeRate;
            newChargePM.SaleIsFixedRate = item.SaleIsFixedRate;
            newChargePM.CostIsFixedRate = item.CostIsFixedRate;
            newChargePM.SaleMeasurementId = item.SaleMeasurementId;
            newChargePM.SaleMeasurementCode = item.SaleMeasurementCode;
            newChargePM.CostMeasurementId = item.CostMeasurementId;
            newChargePM.CostMeasurementCode = item.CostMeasurementCode;
            newChargePM.IsAllIN = item.IsAllIN;
            newChargePM.ViewOrder = item.ViewOrder;
            newChargePM.UpdateDate = item.UpdateDate;
            newChargePM.UpdatedByUserId = item.UpdatedByUserId;
            newChargePM.CostMaxAmount = item.CostMaxAmount;
            newChargePM.CostMinAmount = item.CostMinAmount;
            newChargePM.SaleMinAmount = item.SaleMinAmount;
            newChargePM.SaleMaxAmount = item.SaleMaxAmount;
            newChargePM.MarkUpValue = 0;
            newChargePM.ContainerType1MarkUpValue = 0;
            newChargePM.ContainerType2MarkUpValue = 0;
            newChargePM.ContainerType3MarkUpValue = 0;
            newChargePM.ContainerType4MarkUpValue = 0;
            newChargePM.ContainerType5MarkUpValue = 0;
            newChargePM.MarkUpTypeCode = "F";
            newChargePM.ContainerType1MarkUpTypeCode = "F";
            newChargePM.ContainerType2MarkUpTypeCode = "F";
            newChargePM.ContainerType3MarkUpTypeCode = "F";
            newChargePM.ContainerType4MarkUpTypeCode = "F";
            newChargePM.ContainerType5MarkUpTypeCode = "F";
            newChargePM.IsChargeBySteps = item.IsChargeBySteps;                       
            newChargePM.ChargesGroupCode = item.ChargesGroupCode;
            newChargePM.Notes = item.Notes;
            if (isCopyCost) {
                newChargePM.CostUnitPrice = item.CostUnitPrice;
                newChargePM.CostQuantity = item.CostQuantity;
                newChargePM.CostUnitPrice1InSaleCurrency = item.CostUnitPrice1InSaleCurrency;
                newChargePM.CostUnitPrice2InSaleCurrency = item.CostUnitPrice2InSaleCurrency;
                newChargePM.CostUnitPrice3InSaleCurrency = item.CostUnitPrice3InSaleCurrency;
                newChargePM.CostUnitPrice4InSaleCurrency = item.CostUnitPrice4InSaleCurrency;
                newChargePM.CostUnitPrice5InSaleCurrency = item.CostUnitPrice5InSaleCurrency;
                newChargePM.CostUnitPriceInSaleCurrency = item.CostUnitPriceInSaleCurrency;
                newChargePM.CostContainerType1UnitPrice = item.CostContainerType1UnitPrice;
                newChargePM.CostContainerType2UnitPrice = item.CostContainerType2UnitPrice;
                newChargePM.CostContainerType3UnitPrice = item.CostContainerType3UnitPrice;
                newChargePM.CostContainerType4UnitPrice = item.CostContainerType4UnitPrice;
                newChargePM.CostContainerType5UnitPrice = item.CostContainerType5UnitPrice;
                newChargePM.CostTotalAmount = item.CostTotalAmount;
                newChargePM.CostTotalAmountLocal = item.CostTotalAmountLocal;
                newChargePM.CostAmountInSaleCurrency = item.CostAmountInSaleCurrency;
            }

            if (isCopySales) {
                newChargePM.MarkUpValue = item.MarkUpValue;
                newChargePM.ContainerType1MarkUpValue = item.ContainerType1MarkUpValue;
                newChargePM.ContainerType2MarkUpValue = item.ContainerType2MarkUpValue;
                newChargePM.ContainerType3MarkUpValue = item.ContainerType3MarkUpValue;
                newChargePM.ContainerType4MarkUpValue = item.ContainerType4MarkUpValue;
                newChargePM.ContainerType5MarkUpValue = item.ContainerType5MarkUpValue;
                newChargePM.MarkUpTypeCode = item.MarkUpTypeCode;
                newChargePM.ContainerType1MarkUpTypeCode = item.ContainerType1MarkUpTypeCode;
                newChargePM.ContainerType2MarkUpTypeCode = item.ContainerType2MarkUpTypeCode;
                newChargePM.ContainerType3MarkUpTypeCode = item.ContainerType3MarkUpTypeCode;
                newChargePM.ContainerType4MarkUpTypeCode = item.ContainerType4MarkUpTypeCode;
                newChargePM.ContainerType5MarkUpTypeCode = item.ContainerType5MarkUpTypeCode;
                newChargePM.SaleUnitPrice = item.SaleUnitPrice;
                newChargePM.SaleQuantity = item.SaleQuantity;
                newChargePM.SaleContainerType1UnitPrice = item.SaleContainerType1UnitPrice;
                newChargePM.SaleContainerType2UnitPrice = item.SaleContainerType2UnitPrice;
                newChargePM.SaleContainerType3UnitPrice = item.SaleContainerType3UnitPrice;
                newChargePM.SaleContainerType4UnitPrice = item.SaleContainerType4UnitPrice;
                newChargePM.SaleContainerType5UnitPrice = item.SaleContainerType5UnitPrice;

                if (oldEntityPM.IsChargesByVAT) {
                    newChargePM.VatTypeId = item.VatTypeId;
                    newChargePM.VatTypeName = item.VatTypeName;
                    newChargePM.VatAmount = item.VatAmount;
                    newChargePM.VatPercentage = item.VatPercentage;
                    newChargePM.VatIsMultiPercentage = item.VatIsMultiPercentage;
                    newChargePM.ExternalVATCard = item.ExternalVATCard;
                }

                newChargePM.SaleTotalAmount = item.SaleTotalAmount;
                newChargePM.SaleTotalAmountLocal = item.SaleTotalAmountLocal;
            }

            if (item.IsChargeBySteps) {
                item.QuoteChargePriceSteps.forEach(step => {
                    var stepItem: QuotePriceStepsPM = new QuotePriceStepsPM(item);

                    stepItem.Tenant = step.Tenant;
                    stepItem.CostUnitPrice = step.CostUnitPrice;
                    stepItem.MarkupValue = step.MarkupValue;
                    stepItem.SaleUnitPrice = step.SaleUnitPrice;
                    stepItem.Step = step.Step;

                    newChargePM.AddQuotePriceStepsPM(stepItem);
                });
            }

            entityPM.AddQuoteChargePM(newChargePM);
        });
    }


    public static ComputeChargeableWeight(entityPM: QuotePM) {        
        var myResult = 0;
        myResult = AppTool.CalculateChargeableWeight(entityPM.GrossWeight, entityPM.VolumetricWeight, entityPM.GrossWeightUnitCode, entityPM.ChargeableWeightUnitCode, entityPM.DirectionId, entityPM.TransportModeId);

        return myResult;
    }
    public static ComputeVolumetricWeight(entityPM: QuotePM) {
        var myResult = 0;

        if (entityPM.Volume != null) {
            myResult = AppTool.GetWeightFromVolume(entityPM.VolumeUnitCode, entityPM.ChargeableWeightUnitCode, entityPM.Volume, entityPM.Ratio);
        }

        else if (entityPM.GrossWeight != null) {
            myResult = AppTool.GetWeightFromWeight(entityPM.GrossWeightUnitCode, entityPM.ChargeableWeightUnitCode, entityPM.GrossWeight);
        }

        return myResult;
    }
    public static ComputeQuoteTEU(entityPM: QuotePM) {
        var totalTEU = 0;

        var packageTypeService = new PackageTypeListService();

        if (!AppTool.IsNullOrEmpty(entityPM.PackageType1Id) && entityPM.PackageType1Quantity != null) {
            packageTypeService.getSingleFromCache(entityPM.PackageType1Id).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    var myPackageTypeList: PackageTypeList = myResponse.Result;
                    if (myPackageTypeList != null) {
                        totalTEU = totalTEU + (myPackageTypeList.TEU * entityPM.PackageType1Quantity);
                    }
                }
            });
        }

        if (!AppTool.IsNullOrEmpty(entityPM.PackageType2Id) && entityPM.PackageType2Quantity != null) {
            packageTypeService.getSingleFromCache(entityPM.PackageType2Id).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    var myPackageTypeList: PackageTypeList = myResponse.Result;
                    if (myPackageTypeList != null) {
                        totalTEU = totalTEU + (myPackageTypeList.TEU * entityPM.PackageType2Quantity);
                    }
                }
            });
        }

        if (!AppTool.IsNullOrEmpty(entityPM.PackageType3Id) && entityPM.PackageType3Quantity != null) {
            packageTypeService.getSingleFromCache(entityPM.PackageType3Id).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    var myPackageTypeList: PackageTypeList = myResponse.Result;
                    if (myPackageTypeList != null) {
                        totalTEU = totalTEU + (myPackageTypeList.TEU * entityPM.PackageType3Quantity);
                    }
                }
            });
        }

        if (!AppTool.IsNullOrEmpty(entityPM.PackageType4Id) && entityPM.PackageType4Quantity != null) {
            packageTypeService.getSingleFromCache(entityPM.PackageType4Id).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    var myPackageTypeList: PackageTypeList = myResponse.Result;
                    if (myPackageTypeList != null) {
                        totalTEU = totalTEU + (myPackageTypeList.TEU * entityPM.PackageType4Quantity);
                    }
                }
            });
        }

        if (!AppTool.IsNullOrEmpty(entityPM.PackageType5Id) && entityPM.PackageType5Quantity != null) {
            packageTypeService.getSingleFromCache(entityPM.PackageType5Id).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    var myPackageTypeList: PackageTypeList = myResponse.Result;
                    if (myPackageTypeList != null) {
                        totalTEU = totalTEU + (myPackageTypeList.TEU * entityPM.PackageType5Quantity);
                    }
                }
            });
        }

        return totalTEU;
    }

    public static BuildShipment(entityPM: QuotePM) {
        var packageTypeService: PackageTypeListService = new PackageTypeListService();

        var shipmentPM: ShipmentPM = new ShipmentPM();

        shipmentPM.IsBuildFromQuote = true;
        shipmentPM.Tenant = SessionLocator.Tenant;
        shipmentPM.TransportModeId = entityPM.TransportModeId;
        shipmentPM.DirectionId = entityPM.DirectionId;
        shipmentPM.QuoteId = entityPM.Id;
        shipmentPM.QuoteNumber = entityPM.QuoteNumber;
        shipmentPM.DepartmentId = entityPM.DepartmentId;
        shipmentPM.BranchId = entityPM.BranchId;
        shipmentPM.IncotermId = entityPM.IncotermId;
        shipmentPM.SalesmanUserId = entityPM.SalesmanUserId;
        shipmentPM.EstimateProfitInLocalCurrency = entityPM.EstimateProfit;
        shipmentPM.ProfitCurrencyId = SessionLocator.TenantPM.ProfitCurrencyId;
        shipmentPM.AWBCurrencyId = SessionLocator.TenantPM.FreightCurrencyId;
        shipmentPM.FreightPrepaidCollectId = SessionLocator.TenantPM.ExportFreightPrepaidCollectId;
        shipmentPM.OtherPrepaidCollectId = SessionLocator.TenantPM.ExportOtherPrepaidCollectId;
        shipmentPM.ShipmentTypeId = (entityPM.TransportModeId == "A") ? null : entityPM.ShipmentTypeId;
        shipmentPM.ValueOfGoods = entityPM.ValueOfGoods;
        shipmentPM.ValueOfGoodsCurrencyId = entityPM.ValueOfGoodsCurrencyId;
        shipmentPM.MoveTypeId = entityPM.MoveTypeId;

        //Partners
        shipmentPM.ShipperId = entityPM.ShipperId;
        shipmentPM.ShipperContactId = entityPM.ShipperContactId;
        shipmentPM.ShipperName = entityPM.ShipperName;
        shipmentPM.ShipperNote = entityPM.ShipperNote;
        shipmentPM.ShipperReference1 = entityPM.ShipperReference1;
        shipmentPM.ShipperReference2 = entityPM.ShipperReference2;

        shipmentPM.ConsigneeId = entityPM.ConsigneeId;
        shipmentPM.ConsigneeContactId = entityPM.ConsigneeContactId;
        shipmentPM.ConsigneeName = entityPM.ConsigneeName;
        shipmentPM.ConsigneeNote = entityPM.ConsigneeNote;
        shipmentPM.ConsigneeReference1 = entityPM.ConsigneeReference1;
        shipmentPM.ConsigneeReference2 = entityPM.ConsigneeReference2;

        shipmentPM.CustomerId = entityPM.CustomerId;
        shipmentPM.CustomerContactId = entityPM.CustomerContactId;
        shipmentPM.CustomerReference1 = entityPM.CustomerReference1;
        shipmentPM.CustomerReference2 = entityPM.CustomerReference2;
        shipmentPM.CustomerName = entityPM.CustomerName;
        shipmentPM.CustomerNote = entityPM.CustomerNote;

        if (entityPM.QuoteCustomerTypeCode == "NOT") {
            shipmentPM.ShipmentCustomerTypeCode = "NT1";
        }

        else {
            shipmentPM.ShipmentCustomerTypeCode = entityPM.QuoteCustomerTypeCode;
        }

        shipmentPM.AgentId = entityPM.AgentId;
        shipmentPM.AgentName = entityPM.AgentName;
        shipmentPM.AgentAddressId = entityPM.AgentAddressId;
        shipmentPM.AgentContactId = entityPM.AgentContactId;
        shipmentPM.AgentReference1 = entityPM.AgentReference1;
        shipmentPM.AgentReference2 = entityPM.AgentReference2;

        shipmentPM.Notify1Id = entityPM.NotifyId;
        shipmentPM.Notify1Name = entityPM.NotifyName;
        shipmentPM.Notify1AddressId = entityPM.NotifyAddressId;
        shipmentPM.Notify1ContactId = entityPM.NotifyContactId;

        //Routing
        shipmentPM.MainCarriageFromPartnerId = entityPM.FromPartnerId;
        shipmentPM.MainCarriageFromAddressId = entityPM.FromPartnerAddressId;
        shipmentPM.MainCarriageToPartnerId = entityPM.ToPartnerId;
        shipmentPM.MainCarriageToAddressId = entityPM.ToPartnerAddressId;

        shipmentPM.MainCarriageFromPortId = entityPM.FromPortId;
        shipmentPM.MainCarriageToPortId = entityPM.ToPortId;
        shipmentPM.FromPortId = entityPM.FromPortId;
        shipmentPM.ToPortId = entityPM.ToPortId;
        shipmentPM.MainCarriageCarrierId = entityPM.MainCarriageCarrierId;
        shipmentPM.MainCarriageFinalDestinationPortId = entityPM.ToPortId;

        //Measurments
        shipmentPM.VolumeUnitCode = entityPM.VolumeUnitCode;
        shipmentPM.DimensionsUnitCode = entityPM.DimensionsUnitCode;
        shipmentPM.GrossWeightUnitCode = entityPM.GrossWeightUnitCode;
        shipmentPM.ChargeableWeightInKG = entityPM.ChargeableWeightInKG;
        shipmentPM.ChargeableWeightUnitCode = entityPM.ChargeableWeightUnitCode;
        shipmentPM.BookingVolume = entityPM.Volume;
        shipmentPM.OrderVolumetricWeight = entityPM.VolumetricWeight;
        shipmentPM.OrderGrossWeight = entityPM.GrossWeight;
        shipmentPM.OrderChargeableWeight = entityPM.ChargeableWeight;
        shipmentPM.OrderGrossWeightEdited = entityPM.GrossWeightEdited;
        shipmentPM.OrderChargeableWeightEdited = entityPM.ChargeableWeightEdited;
        shipmentPM.Ratio = entityPM.Ratio;
        shipmentPM.DimFactor = entityPM.DimFactor;
        shipmentPM.BookingNumberOfPackages = entityPM.NumberOfPackages;
        shipmentPM.OrderIsDangerouseGoods = entityPM.IsDangerous;
        shipmentPM.DescriptionOfGoods = entityPM.DescriptionOfGoods;

        //PickUp Delivery
        shipmentPM.IncludePickUp = entityPM.IncludePickUp;
        shipmentPM.FromAddressCity = entityPM.FromAddressCity;
        shipmentPM.FromAddressZipCode = entityPM.FromAddressZipCode;
        shipmentPM.FromAddressCountryId = entityPM.FromAddressCountryId;
        shipmentPM.PickUpAddressId = entityPM.PickUpAddressId;
        shipmentPM.ShipperMainAddressId = entityPM.ShipperMainAddressId;
        shipmentPM.ShipperPickAddressId = entityPM.ShipperPickAddressId;

        shipmentPM.IncludeDelivery = entityPM.IncludeDelivery;
        shipmentPM.ToAddressCity = entityPM.ToAddressCity;
        shipmentPM.ToAddressZipCode = entityPM.ToAddressZipCode;
        shipmentPM.ToAddressCountryId = entityPM.ToAddressCountryId;
        shipmentPM.DeliveryAddressId = entityPM.DeliveryAddressId;
        shipmentPM.ConsigneeMainAddressId = entityPM.ConsigneeMainAddressId;
        shipmentPM.ConsigneePickAddressId = entityPM.ConsigneePickAddressId;
        
        //Order Packages
        if (this.IsLCLQuote(entityPM)) {
            entityPM.QuotePackages.forEach(item => {
                var newItem: ShipmentOrderPackagePM = new ShipmentOrderPackagePM(shipmentPM);

                packageTypeService.getSingleFromCache(item.PackageTypeId).subscribe((myResponse: ServiceResponse) => {
                    if (myResponse != null) {
                        var myPackageTypeList: PackageTypeList = myResponse.Result;
                        if (myPackageTypeList != null) {
                            newItem.PackageTypeName = myPackageTypeList.EnglishName;
                        }
                    }
                });

                newItem.Tenant = SessionLocator.Tenant;
                newItem.PackageTypeId = item.PackageTypeId;
                newItem.Quantity = item.Quantity;
                newItem.GrossWeight = item.GrossWeight;
                newItem.Volume = item.Volume;
                newItem.Height = item.Height;
                newItem.Length = item.Length;
                newItem.Width = item.Width;
                newItem.VolumetricWeight = item.VolumetricWeight;             
                
                shipmentPM.ShipmentOrderPackages.push(newItem);
            });
        }

        else {
            shipmentPM.Quantity1 = entityPM.PackageType1Quantity;
            shipmentPM.Quantity2 = entityPM.PackageType2Quantity;
            shipmentPM.Quantity3 = entityPM.PackageType3Quantity;
            shipmentPM.Quantity4 = entityPM.PackageType4Quantity;
            shipmentPM.Quantity5 = entityPM.PackageType5Quantity;
            shipmentPM.PackageTypeId1 = entityPM.PackageType1Id;
            shipmentPM.PackageTypeId2 = entityPM.PackageType2Id;
            shipmentPM.PackageTypeId3 = entityPM.PackageType3Id;
            shipmentPM.PackageTypeId4 = entityPM.PackageType4Id;
            shipmentPM.PackageTypeId5 = entityPM.PackageType5Id;
        }

        return shipmentPM;
    }
}
