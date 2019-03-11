import {Component, OnInit}  from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../../Infrastructure/Tools';
import {ChargesTypePM} from '../../../EntityPMs/ChargesTypePM';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import {ChargesGroupListService} from '../../../../Infrastructure/Services/StandardLists/ChargesGroupListService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    selector: 'ChargesTypeGeneralTabComponent',
    moduleId: module.id,
    templateUrl: './ChargesTypeGeneralTabComponent.html',
})

export class ChargesTypeGeneralTabComponent extends BaseComponent implements OnInit {
    public DataContext: ChargesTypeGeneralTabComponent = this;
    public ObjectTableName: string = "ChargesType";
    public EntityPM: ChargesTypePM;
    public DisplaySATSettings: boolean = false;
    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
        if (SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
            this.DisplaySATSettings = true;
        }
    }

    ngOnInit() {
        if (this.EntityPM != null) {
            this.SetUIProperties();
            this.CheckWarnings();
        }
    }

    public CustomsFieldsIsVisible: boolean = false;
    private SetUIProperties() {
        var fieldsEnabled = true;  
        var awbFieldsEnabled = true;      
        if (InfraSettings.TenantPM.Id == 65) {
            if (!SessionLocator.LoggedUserPM.IsCustomerCare) {
                fieldsEnabled = false;
                awbFieldsEnabled = false;
            }
        }

        else {
            if (!this.IsAir || (this.ChargesGroupCode == "FRT")) {
                awbFieldsEnabled = false;
            }
        }        

        if (ObjectsLocator.CustomsInterfaceSettingPM != null) {
            if (ObjectsLocator.CustomsInterfaceSettingPM.ActivateCustomsManagementInShipments) {
                this.CustomsFieldsIsVisible = true;
            }
        }

        //this.UIProperties.SetEnabled("Code", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("EnglishName", this.ObjectTableName, fieldsEnabled);
        this.UIProperties.SetEnabled("LocalName", this.ObjectTableName, fieldsEnabled);
        this.UIProperties.SetEnabled("ChargesGroupId", this.ObjectTableName, fieldsEnabled);
        this.UIProperties.SetEnabled("MeasurementCode", this.ObjectTableName, fieldsEnabled);
        this.UIProperties.SetEnabled("ContainerMeasurementId", this.ObjectTableName, fieldsEnabled);
        this.UIProperties.SetEnabled("VatTypeId", this.ObjectTableName, fieldsEnabled);
        this.UIProperties.SetEnabled("ViewOrder", this.ObjectTableName, fieldsEnabled);
        this.UIProperties.SetEnabled("Description", this.ObjectTableName, fieldsEnabled);
        this.UIProperties.SetEnabled("IsReceivable", this.ObjectTableName, fieldsEnabled);
        this.UIProperties.SetEnabled("IsPayable", this.ObjectTableName, fieldsEnabled);
        this.UIProperties.SetEnabled("IsCustoms", this.ObjectTableName, fieldsEnabled);
        //this.UIProperties.SetEnabled("IsBackToBack", this.ObjectTableName, fieldsEnabled);
        this.UIProperties.SetEnabled("IsExpense", this.ObjectTableName, fieldsEnabled);
        this.UIProperties.SetEnabled("InActive", this.ObjectTableName, fieldsEnabled);
        this.UIProperties.SetEnabled("IsAir", this.ObjectTableName, fieldsEnabled);
        this.UIProperties.SetEnabled("IsInland", this.ObjectTableName, fieldsEnabled);
        this.UIProperties.SetEnabled("IsOcean", this.ObjectTableName, fieldsEnabled);
        this.UIProperties.SetEnabled("IsAutoDisplayInQuote", this.ObjectTableName, fieldsEnabled);
        this.UIProperties.SetEnabled("IsAutoDisplayInShipment", this.ObjectTableName, fieldsEnabled);
        this.UIProperties.SetEnabled("IsAutoDisplayInConsolidation", this.ObjectTableName, fieldsEnabled);
        this.UIProperties.SetEnabled("IsAutoDisplayInCustoms", this.ObjectTableName, fieldsEnabled);
        this.UIProperties.SetEnabled("DueTypeCode", this.ObjectTableName, awbFieldsEnabled);
        this.UIProperties.SetEnabled("IATACodeId", this.ObjectTableName, awbFieldsEnabled);
        this.UIProperties.SetEnabled("AWBPrintDescription", this.ObjectTableName, awbFieldsEnabled);
        this.UIProperties.SetEnabled("SATExternalId", this.ObjectTableName, fieldsEnabled);

        //if (this.IsBackToBack) {
        //    this.UIProperties.SetEnabled("IsReceivable", this.ObjectTableName, false);
        //}
    }

    public ValidationWarningsList: string[];
    private CheckWarnings() {
        this.ValidationWarningsList = [];

        if (!this.IsAir) {
            this.ValidationWarningsList.push("This Charge Type will not be used in Air Transport Mode");
        }

        if (this.ChargesGroupCode == "FRT") {
            this.ValidationWarningsList.push("Charges that belong to (Freight) group will not be printed on AWB");
        }
    }

    // Properties
    get Code() { return this.EntityPM.Code; }
    set Code(newValue: string) {
        if (this.EntityPM.Code != newValue) {
            this.EntityPM.Code = newValue;
        }
    }

    get EnglishName() { return this.EntityPM.EnglishName; }
    set EnglishName(newValue: string) {
        if (this.EntityPM.EnglishName != newValue) {
            this.EntityPM.EnglishName = newValue;
        }
    }

    get LocalName() { return this.EntityPM.LocalName; }
    set LocalName(newValue: string) {
        if (this.EntityPM.LocalName != newValue) {
            this.EntityPM.LocalName = newValue;
        }
    }

    get ChargesGroupCode() { return this.EntityPM.ChargesGroupCode; }
    set ChargesGroupCode(newValue: string) {
        if (this.EntityPM.ChargesGroupCode != newValue) {
            this.EntityPM.ChargesGroupCode = newValue;

            this.SetUIProperties();
            this.CheckWarnings();
        }
    }

    get ChargesGroupId() { return this.EntityPM.ChargesGroupId; }
    set ChargesGroupId(newValue: string) {
        if (this.EntityPM.ChargesGroupId != newValue) {
            this.EntityPM.ChargesGroupId = newValue;
            if (!AppTool.IsNullOrEmpty(newValue)) {
                var myService: ChargesGroupListService = new ChargesGroupListService();
                myService.getSingleFromCache(this.EntityPM.ChargesGroupId).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError && myResponse.Result) {
                        this.ChargesGroupCode = myResponse.Result.Code;
                    }
                });
            }
            else this.ChargesGroupCode = newValue;

        }
    }


    get MeasurementId() { return this.EntityPM.MeasurementId; }
    set MeasurementId(newValue: string) {
        if (this.EntityPM.MeasurementId != newValue) {
            this.EntityPM.MeasurementId = newValue;

            this.SetUIProperties();
        }
    }

    get ContainerMeasurementId() { return this.EntityPM.ContainerMeasurementId; }
    set ContainerMeasurementId(newValue: string) {
        if (this.EntityPM.ContainerMeasurementId != newValue) {
            this.EntityPM.ContainerMeasurementId = newValue;
        }
    }

    get VatTypeId() { return this.EntityPM.VatTypeId; }
    set VatTypeId(newValue: string) {
        if (this.EntityPM.VatTypeId != newValue) {
            this.EntityPM.VatTypeId = newValue;
        }
    }

    get ViewOrder() { return this.EntityPM.ViewOrder; }
    set ViewOrder(newValue: number) {
        if (this.EntityPM.ViewOrder != newValue) {
            this.EntityPM.ViewOrder = newValue;
        }
    } 

    get Description() { return this.EntityPM.Description; }
    set Description(newValue: string) {
        if (this.EntityPM.Description != newValue) {
            this.EntityPM.Description = newValue;
        }
    }

    get IsReceivable() { return this.EntityPM.IsReceivable; }
    set IsReceivable(newValue: boolean) {
        if (this.EntityPM.IsReceivable != newValue) {
            this.EntityPM.IsReceivable = newValue;
        }
    }

    get IsPayable() { return this.EntityPM.IsPayable; }
    set IsPayable(newValue: boolean) {
        if (this.EntityPM.IsPayable != newValue) {
            this.EntityPM.IsPayable = newValue;
        }
    }

    get IsCustoms() { return this.EntityPM.IsCustoms; }
    set IsCustoms(newValue: boolean) {
        if (this.EntityPM.IsCustoms != newValue) {
            this.EntityPM.IsCustoms = newValue;
        }
    }

    //get IsBackToBack() { return this.EntityPM.IsBackToBack; }
    //set IsBackToBack(newValue: boolean) {
    //    if (this.EntityPM.IsBackToBack != newValue) {
    //        this.EntityPM.IsBackToBack = newValue;
    //        if (newValue) {
    //            this.IsReceivable = false;
    //            this.UIProperties.SetEnabled("IsReceivable", this.ObjectTableName, false);
    //        }
    //        else {
    //            this.UIProperties.SetEnabled("IsReceivable", this.ObjectTableName, true);
    //        }
    //    }
    //}
    
    get IsExpense() { return this.EntityPM.IsExpense; }
    set IsExpense(newValue: boolean) {
        if (this.EntityPM.IsExpense != newValue) {
            this.EntityPM.IsExpense = newValue;
        }
    }

    get InActive() { return this.EntityPM.InActive; }
    set InActive(newValue: boolean) {
        if (this.EntityPM.InActive != newValue) {
            this.EntityPM.InActive = newValue;
        }
    }
    
    get IsAir() { return this.EntityPM.IsAir; }
    set IsAir(newValue: boolean) {
        if (this.EntityPM.IsAir != newValue) {
            this.EntityPM.IsAir = newValue;

            this.SetUIProperties();
            this.CheckWarnings();
        }
    }

    get IsInland() { return this.EntityPM.IsInland; }
    set IsInland(newValue: boolean) {
        if (this.EntityPM.IsInland != newValue) {
            this.EntityPM.IsInland = newValue;
        }
    }

    get IsOcean() { return this.EntityPM.IsOcean; }
    set IsOcean(newValue: boolean) {
        if (this.EntityPM.IsOcean != newValue) {
            this.EntityPM.IsOcean = newValue;
        }
    }

    get IsAutoDisplayInQuote() { return this.EntityPM.IsAutoDisplayInQuote; }
    set IsAutoDisplayInQuote(newValue: boolean) {
        if (this.EntityPM.IsAutoDisplayInQuote != newValue) {
            this.EntityPM.IsAutoDisplayInQuote = newValue;
        }
    }

    get IsAutoDisplayInShipment() { return this.EntityPM.IsAutoDisplayInShipment; }
    set IsAutoDisplayInShipment(newValue: boolean) {
        if (this.EntityPM.IsAutoDisplayInShipment != newValue) {
            this.EntityPM.IsAutoDisplayInShipment = newValue;
        }
    }

    get IsAutoDisplayInConsolidation() { return this.EntityPM.IsAutoDisplayInConsolidation; }
    set IsAutoDisplayInConsolidation(newValue: boolean) {
        if (this.EntityPM.IsAutoDisplayInConsolidation != newValue) {
            this.EntityPM.IsAutoDisplayInConsolidation = newValue;
        }
    }

    get IsAutoDisplayInCustoms() { return this.EntityPM.IsAutoDisplayInCustoms; }
    set IsAutoDisplayInCustoms(newValue: boolean) {
        if (this.EntityPM.IsAutoDisplayInCustoms != newValue) {
            this.EntityPM.IsAutoDisplayInCustoms = newValue;
        }
    }

    get DueTypeCode() { return this.EntityPM.DueTypeCode; }
    set DueTypeCode(newValue: string) {
        if (this.EntityPM.DueTypeCode != newValue) {
            this.EntityPM.DueTypeCode = newValue;
        }
    }

    get IATACodeId() { return this.EntityPM.IATACodeId; }
    set IATACodeId(newValue: string) {
        if (this.EntityPM.IATACodeId != newValue) {
            this.EntityPM.IATACodeId = newValue;
        }
    }

    get AWBPrintDescription() { return this.EntityPM.AWBPrintDescription; }
    set AWBPrintDescription(newValue: boolean) {
        if (this.EntityPM.AWBPrintDescription != newValue) {
            this.EntityPM.AWBPrintDescription = newValue;
        }
    }       

    get SATExternalId() { return this.EntityPM.SATExternalId; }
    set SATExternalId(newValue: string) {
        if (this.EntityPM.SATExternalId != newValue) {
            this.EntityPM.SATExternalId = newValue;
        }
    }

    get IsImport() { return this.EntityPM.IsImport; }
    set IsImport(newValue: boolean) {
        if (this.EntityPM.IsImport != newValue) {
            this.EntityPM.IsImport = newValue;
        }
    }

    get IsExport() { return this.EntityPM.IsExport; }
    set IsExport(newValue: boolean) {
        if (this.EntityPM.IsExport != newValue) {
            this.EntityPM.IsExport = newValue;
        }
    }

    get IsDrop() { return this.EntityPM.IsDrop; }
    set IsDrop(newValue: boolean) {
        if (this.EntityPM.IsDrop != newValue) {
            this.EntityPM.IsDrop = newValue;
        }
    }

    get IsDomestic() { return this.EntityPM.IsDomestic; }
    set IsDomestic(newValue: boolean) {
        if (this.EntityPM.IsDomestic != newValue) {
            this.EntityPM.IsDomestic = newValue;
        }
    }
}
