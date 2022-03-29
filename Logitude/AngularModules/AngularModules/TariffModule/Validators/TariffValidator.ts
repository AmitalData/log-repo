import {AppTool} from '../../Infrastructure/Tools';
import { ChargesTypeListService } from '../../Common/Services/StandardLists/ChargesTypeListService';
import { ChargesTypeList } from '../../Common/EntityLists/ChargesTypeList';
import { TariffPM } from '../EntityPMs/TariffPM';
import { TariffVersionPM } from '../EntityPMs/TariffVersionPM';
import { Validator } from '../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from '../../Infrastructure/Utilities/TextCodeTranslator';
import { PackageTypeListService } from '../../Common/Services/StandardLists/PackageTypeListService';
import { PackageTypeList } from '../../Common/EntityLists/PackageTypeList';
import { MeasurementListService } from '../../Common/Services/StandardLists/MeasurementListService';
import { MeasurementList } from '../../Common/EntityLists/MeasurementList';

export class TariffValidator {
    private IdProps: string[] = [];
    private UOMProps: string[] = [];
    private chargesTypePMService: ChargesTypeListService;
    private packageTypeListService: PackageTypeListService;
    private measurementPMService: MeasurementListService;
    private Errors: string[] = [];
    private entityPM: TariffPM;
    public HasAContainerTypeUOM: boolean = false;

    public Validate = (entityPM: TariffPM): any[] => {
        this.Errors = [];
        this.entityPM = entityPM;

        if (entityPM != null) {
            Validator.TryValidateObject(this.entityPM, "Tariff", this.Errors);

            if (entityPM.TypeCode == "ICS" || entityPM.TypeCode == "ECS") {
                if (AppTool.IsNullOrEmpty(entityPM.CustomerGroupId)) {
                    this.Errors.push("Customer Group Field is Required");
                }
            }
            else {
                if (AppTool.IsNullOrEmpty(entityPM.SellerId)) {
                    this.Errors.push("Seller Field is Required");
                }
            }

            if (entityPM.TypeCode == "ASC" || entityPM.TypeCode == "OSC" || entityPM.TypeCode == "ICC" || entityPM.TypeCode == "ECC" || entityPM.TypeCode == "ICS" || entityPM.TypeCode == "ECS") {
                this.chargesTypePMService = new ChargesTypeListService();
                this.FillChargesIDsAndUOMS();
                this.ValidateSurcharge();
            }
            else if (entityPM.TypeCode == "OFC") {
                this.packageTypeListService = new PackageTypeListService();
                this.FillContainersIDs();
                this.ValidateContainers();
                this.HasAContainerTypeUOM = true;
            }
            else if (entityPM.TypeCode == "OFS" || entityPM.TypeCode == "IFT") {
                this.chargesTypePMService = new ChargesTypeListService();
                this.packageTypeListService = new PackageTypeListService();
                this.measurementPMService = new MeasurementListService();
                this.HasAContainerTypeUOM = false;
                this.FillChargesIDsAndUOMS();
                this.ValidateSurcharge();
                this.FillContainersIDs();
                this.ValidateContainers();
            }
            else if (entityPM.TypeCode == "AFC") {
                this.ValidateProduct();
            }

            this.ValidateTariffLines();
        }

        return this.Errors;
    }

    FillContainersIDs() {
        for (var index = 1; index <= 5; index++) {
            this.IdProps.push("ContainerType" + index + "Id");
        }
    }

    FillChargesIDsAndUOMS() {
        for (var index = 1; index <= 10; index++) {
            this.IdProps.push("Surcharge" + index + "Id");
            this.UOMProps.push("Surcharge" + index + "UOM");
        }
    }

    ValidateProduct() {
        if (AppTool.IsNullOrEmpty(this.entityPM.TariffProductId)) {
            this.Errors.push("Product Field is Required");
        }
    }

    ValidateSurcharge() {
        var IdProps: string[] = [];
        var UOMProps: string[] = [];
        var IdPropsName: string[] = [];
        var UOMPropsName: string[] = [];
        var DuplicatedChargesIds: string[] = [];
        var EmptyIndex = 1;
        var emptyLines: boolean = false;
        var FirstLineEmpty: boolean = false;
        var tempErrors: Array<string> = [];
        this.HasAContainerTypeUOM = false;

        for (var index = 1; index <= 10; index++) {
            IdProps.push("Surcharge" + index + "Id");
            UOMProps.push("Surcharge" + index + "UOM");
            IdPropsName.push("Charge Type " + index);
            UOMPropsName.push("UOM " + index);
            if (this.IdProps.filter(p => this.entityPM[p + ""] == this.entityPM[IdProps[index - 1]] && (p + "" != IdProps[index - 1] + "") && this.entityPM[IdProps[index - 1]] != null)[0] != null) {
                var chargresType = this.IdProps.filter(p => this.entityPM[p + ""] == this.entityPM[IdProps[index - 1]] && (p + "" != IdProps[index - 1] + "") && this.entityPM[IdProps[index - 1]] != null)[0];
                if (!DuplicatedChargesIds.includes(this.entityPM[chargresType + ""])) {
                    DuplicatedChargesIds.push(this.entityPM[chargresType + ""]);
                    this.chargesTypePMService.getSingleFromCache(this.entityPM[chargresType + ""]).subscribe((res: any) => {
                        if (!res.HasError) {
                            var chargesTypeList: ChargesTypeList = res.Result;
                            if (res) {
                                this.Errors.push("Charge type " + chargesTypeList.EnglishName + " is duplicated");
                            }
                        }
                    });
                }
            }

            if (index == 1) {
                if (AppTool.IsNullOrEmpty(this.entityPM[IdProps[index - 1]])) {
                    tempErrors.push(IdPropsName[index - 1] + " is required");
                    FirstLineEmpty = true;
                }

                if (AppTool.IsNullOrEmpty(this.entityPM[UOMProps[index - 1]])) {
                    tempErrors.push(UOMPropsName[index - 1] + " is required");
                }
            }

            else {
                if (AppTool.IsNullOrEmpty(this.entityPM[IdProps[index - 1]])) {
                    if (EmptyIndex == 1) {
                        EmptyIndex = index;
                    }
                }

                if (AppTool.IsNullOrEmpty(this.entityPM[UOMProps[index - 1]]) && !AppTool.IsNullOrEmpty(this.entityPM[IdProps[index - 1]])) {
                    this.Errors.push(IdPropsName[index - 1] + " is filled without a UOM");
                }

                if (index == 2) {
                    if (!AppTool.IsNullOrEmpty(this.entityPM[UOMProps[index - 1]]) && !AppTool.IsNullOrEmpty(this.entityPM[IdProps[index - 1]])) {
                        if (FirstLineEmpty) {
                            emptyLines = true;
                            this.Errors.push("Empty Charge Lines aren't allowed between line 1 and line 2");
                            EmptyIndex = 1;
                        }
                    }
                }

                if (index >= 3) {
                    if (!AppTool.IsNullOrEmpty(this.entityPM[UOMProps[index - 1]]) && !AppTool.IsNullOrEmpty(this.entityPM[IdProps[index - 1]])) {
                        if (EmptyIndex != 1) {
                            emptyLines = true;
                            this.Errors.push("Empty Charge Lines aren't allowed between line " + (EmptyIndex - 1) + " and line " + index);
                            EmptyIndex = 1;
                        }

                        if (AppTool.IsNullOrEmpty(this.entityPM[IdProps[index - 2]])) {
                            //   this.ValidationErrorsList.push("no empty line between 2 charges in line "+index+ " and "+(index-2));
                        }
                    }
                }
            }

            if (this.entityPM.TypeCode == "OFS") {
                this.HasAContainerTypeUOM = true;
            }
        }

        if (!emptyLines) {
            tempErrors.forEach(error => {
                this.Errors.push(error);
            });
        }
    }

    ValidateContainers() {
        var IdProps: string[] = [];
        var IdPropsName: string[] = [];
        var DuplicatedContainersIds: string[] = [];
        var EmptyIndex = 1;
        var emptyLines: boolean = false;
        var FirstLineEmpty: boolean = false;
        var tempErrors: Array<string> = [];

        for (var index = 1; index <= 5; index++) {
            IdProps.push("ContainerType" + index + "Id");
            IdPropsName.push("Container Type " + index);

            if (this.IdProps.filter(p => this.entityPM[p + ""] == this.entityPM[IdProps[index - 1]] && (p + "" != IdProps[index - 1] + "") && this.entityPM[IdProps[index - 1]] != null)[0] != null) {
                var packageType = this.IdProps.filter(p => this.entityPM[p + ""] == this.entityPM[IdProps[index - 1]] && (p + "" != IdProps[index - 1] + "") && this.entityPM[IdProps[index - 1]] != null)[0];
                if (!DuplicatedContainersIds.includes(this.entityPM[packageType + ""])) {
                    DuplicatedContainersIds.push(this.entityPM[packageType + ""]);
                    this.packageTypeListService.getSingleFromCache(this.entityPM[packageType + ""]).subscribe((res: any) => {
                        if (!res.HasError) {
                            var packageTypeList: PackageTypeList = res.Result;
                            if (packageTypeList) {
                                this.Errors.push("Container type " + packageTypeList.EnglishName + " is duplicated");
                            }
                        }
                    });
                }
            }

            if (index == 1) {
                if (AppTool.IsNullOrEmpty(this.entityPM[IdProps[index - 1]])) {
                    if (this.HasAContainerTypeUOM) {
                        tempErrors.push(IdPropsName[index - 1] + " is required");
                    }
                    FirstLineEmpty = true;
                }
            }

            else {
                if (AppTool.IsNullOrEmpty(this.entityPM[IdProps[index - 1]])) {
                    if (EmptyIndex == 1) {
                        EmptyIndex = index;
                    }
                }

                if (index == 2) {
                    if (!AppTool.IsNullOrEmpty(this.entityPM[IdProps[index - 1]])) {
                        if (FirstLineEmpty) {
                            emptyLines = true;
                            this.Errors.push("Empty Container Lines aren't allowed between line 1 and line 2");
                            EmptyIndex = 1;
                        }
                    }
                }

                if (index >= 3) {
                    if (!AppTool.IsNullOrEmpty(this.entityPM[IdProps[index - 1]])) {
                        if (EmptyIndex != 1) {
                            emptyLines = true;
                            this.Errors.push("Empty Container Lines aren't allowed between line " + (EmptyIndex - 1) + " and line " + index);
                            EmptyIndex = 1;
                        }
                    }
                }
            }
        }

        if (!emptyLines) {
            tempErrors.forEach(error => {
                this.Errors.push(error);
            });
        }
    }

    private ValidateTariffLines() {
        var msg: string = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        var draftVersion: TariffVersionPM = this.entityPM.TariffVersions.filter(d => d.IsDraft)[0];

        if (draftVersion != null) {
            draftVersion.TariffLines.forEach(item => {
                Validator.TryValidateObject(item, "TariffLine", this.Errors);

                if (this.entityPM.TypeCode == "ASC" || this.entityPM.TypeCode == "OSC" || this.entityPM.TypeCode == "OFS") {
                    if (item.IsDifferentCurrenciesPerCharge) {
                        if (this.entityPM.TypeCode == "OFS") {
                            if (item.ContainersPrices.filter(d => AppTool.IsNullOrEmpty(d.CurrencyId)).length > 0) {
                                this.Errors.push("Some Containers Prices missing Currency");
                            }
                        }

                        else {
                            for (var i = 1; i <= 10; i++) {
                                if (this.entityPM["Surcharge" + i + "Id"]) {
                                    if (AppTool.IsNullOrEmpty(item["Surcharge" + i + "CurrencyId"])) {
                                        this.Errors.push("Surcharge " + i + " Currency Field is Required");
                                    }
                                }
                            }
                        }
                    }
                    else {
                        if (AppTool.IsNullOrEmpty(item.CurrencyId)) {
                            this.Errors.push("Currency Field is Required");
                        }
                    }

                    if (AppTool.IsNullOrEmpty(item.DestinationPortId) && !item.IsToAllOtherPorts) {
                        this.Errors.push("To port or To All Other Ports is Required");
                    }

                    if (AppTool.IsNullOrEmpty(item.OriginPortId) && !item.IsFromAllOtherPorts) {
                        this.Errors.push("From port or From All Other Ports is Required");
                    }
                }

                else if (this.entityPM.TypeCode == "ICC" || this.entityPM.TypeCode == "ECC") {
                    if (item.IsDifferentCurrenciesPerCharge) {
                        for (var i = 1; i <= 10; i++) {
                            if (this.entityPM["Surcharge" + i + "Id"]) {
                                if (AppTool.IsNullOrEmpty(item["Surcharge" + i + "CurrencyId"])) {
                                    this.Errors.push("Surcharge " + i + " Currency Field is Required");
                                }
                            }
                        }
                    }
                    else {
                        if (AppTool.IsNullOrEmpty(item.CurrencyId)) {
                            this.Errors.push("Currency Field is Required");
                        }
                    }

                    if (AppTool.IsNullOrEmpty(item.ToCountryId) && !item.IsToAllOtherCountries) {
                        this.Errors.push("To country or to all other countries is required");
                    }

                    if (AppTool.IsNullOrEmpty(item.FromCountryId) && !item.IsFromAllOtherCountries) {
                        this.Errors.push("From country or from all other countries is required");
                    }
                }

                else if (this.entityPM.TypeCode == "AFC" || this.entityPM.TypeCode == "OLC" || this.entityPM.TypeCode == "OFC") {
                    if (this.entityPM.IsApprovingDraftVersion) {
                        if (AppTool.IsNullOrEmpty(item.DestinationPortId)) {
                            this.Errors.push(msg.replace("%FieldName", "To"));
                        }

                        if (AppTool.IsNullOrEmpty(item.OriginPortId)) {
                            this.Errors.push(msg.replace("%FieldName", "From"));
                        }
                    }
                }
            });
        }
    }
}
