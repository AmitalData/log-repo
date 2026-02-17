import {Component} from '@angular/core';
import {AppTool} from '../../../../../Infrastructure/Tools';
import {CounterPM} from '../../../../../Common/EntityPMs/CounterPM';
import {CounterDefinitionPM} from '../../../../../Common/EntityPMs/CounterDefinitionPM';
import {TenantSettingPM} from '../../../../../Infrastructure/EntityPMs/TenantSettingPM';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../../../Infrastructure/Utilities/FeatureLocator';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {CountersDomainService, CounterAPIHelper} from '../../../../../Common/Services/CountersDomainService';
import {Validator} from '../../../../../Infrastructure/Validators/Validator';
import {MessageWindow} from '../../../../../Controls/Windows/MessageWindow';

@Component({
    moduleId: module.id,
    templateUrl: './CounterHAWBComponent.html',
})

export class CounterHAWBComponent extends BaseComponent {
    public CounterId: string;
    public EntityPM: CounterPM;
    public DataContext = this;
    public ObjectTableName = "CounterDefinition";
    public CounterAPIHelper: CounterAPIHelper;
    public SettingHWB: TenantSettingPM;
    public SettingFBL: TenantSettingPM;
    public SettingHBL: TenantSettingPM;
    public CounterDefinitionHWB: CounterDefinitionPM;
    public CounterDefinitionFBL: CounterDefinitionPM;
    public CounterDefinitionHBL: CounterDefinitionPM;
    public IsFBLVisible: boolean = false;
    public IsHBLVisible: boolean = false;
    public HasAllTransportsFeature: boolean = false;
    public IsResourcesReady: boolean = false;
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();

        if (FeatureLocator.HasFeaturePermession("Shipment", "ALLTRANSPORTMODES")) {
            this.HasAllTransportsFeature = true;
        }
    }

    SetWindowArgs(args: any) {
        this.CounterId = args["CounterId"];

        if (this.CounterId) {
            this.CurrentSession.StartBusyIndicatorLoading();

            var myService = new CountersDomainService();
            myService.GetCounterAPIHelper(this.CounterId).subscribe((myResponse: ServiceResponse) => {

                if (myResponse.HasError) {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }

                else {
                    this.CounterAPIHelper = myResponse.Result;

                    if (this.CounterAPIHelper) {
                        this.EntityPM = this.CounterAPIHelper.CounterPM;
                        this.InitializeDefinitions();
                        this.InitializeTenantSettings();
                    }
                }

                this.IsResourcesReady = true;
                this.CurrentSession.StopBusyIndicator();
            });
        }
    }
    InitializeDefinitions() {
        this.CounterDefinitionHWB = this.CounterAPIHelper.CounterDefinitions.filter(f => f.Parameter1 == "A")[0];
        if (this.CounterDefinitionHWB == null) {
            this.CounterDefinitionHWB = new CounterDefinitionPM();
            this.CounterDefinitionHWB.Tenant = this.EntityPM.Tenant;
            this.CounterDefinitionHWB.CounterId = this.EntityPM.Id;
            this.CounterDefinitionHWB.Parameter1 = "A";
            this.CounterDefinitionHWB.StartNumber = 1000;
            this.CounterDefinitionHWB.StartNumber_Old = 0;
            this.CounterDefinitionHWB.UniquePerPrefix = false;
        }

        if (this.HasAllTransportsFeature) {
            this.CounterDefinitionFBL = this.CounterAPIHelper.CounterDefinitions.filter(f => f.Parameter1 == "O")[0];
            this.CounterDefinitionHBL = this.CounterAPIHelper.CounterDefinitions.filter(f => f.Parameter1 == "I")[0];

            if (this.CounterDefinitionFBL == null) {
                this.CounterDefinitionFBL = new CounterDefinitionPM();
                this.CounterDefinitionFBL.Tenant = this.EntityPM.Tenant;
                this.CounterDefinitionFBL.CounterId = this.EntityPM.Id;
                this.CounterDefinitionFBL.Parameter1 = "O";
                this.CounterDefinitionFBL.StartNumber = 1000;
                this.CounterDefinitionFBL.StartNumber_Old = 0;
                this.CounterDefinitionFBL.UniquePerPrefix = false;
            }

            if (this.CounterDefinitionHBL == null) {
                this.CounterDefinitionHBL = new CounterDefinitionPM();
                this.CounterDefinitionHBL.Tenant = this.EntityPM.Tenant;
                this.CounterDefinitionHBL.CounterId = this.EntityPM.Id;
                this.CounterDefinitionHBL.Parameter1 = "I";
                this.CounterDefinitionHBL.StartNumber = 1000;
                this.CounterDefinitionHBL.StartNumber_Old = 0;
                this.CounterDefinitionHBL.UniquePerPrefix = false;
            }
        }
    }
    InitializeTenantSettings() {
        this.SettingHWB = this.CounterAPIHelper.TenantSettings.filter(f => f.SettingCode == "HAWBCounterA_E_D")[0];
        if (this.SettingHWB == null) {
            this.SettingHWB = new TenantSettingPM();
            this.SettingHWB.Tenant = this.EntityPM.Tenant;
            this.SettingHWB.ObjectTableId = this.EntityPM.ObjectTableId;
            this.SettingHWB.SettingCode = "HAWBCounterA_E_D";
            this.SettingHWB.SettingValue = "None";
            this.CounterAPIHelper.TenantSettings.push(this.SettingHWB);
        }

        if (this.HasAllTransportsFeature) {
            this.SettingFBL = this.CounterAPIHelper.TenantSettings.filter(f => f.SettingCode == "HAWBCounterO_E_D")[0];
            this.SettingHBL = this.CounterAPIHelper.TenantSettings.filter(f => f.SettingCode == "HAWBCounterI_E_D")[0];

            if (this.SettingFBL == null) {
                this.SettingFBL = new TenantSettingPM();
                this.SettingFBL.Tenant = this.EntityPM.Tenant;
                this.SettingFBL.ObjectTableId = this.EntityPM.ObjectTableId;
                this.SettingFBL.SettingCode = "HAWBCounterO_E_D";
                this.SettingFBL.SettingValue = "None";
                this.CounterAPIHelper.TenantSettings.push(this.SettingFBL);
            }

            if (this.SettingHBL == null) {
                this.SettingHBL = new TenantSettingPM();
                this.SettingHBL.Tenant = this.EntityPM.Tenant;
                this.SettingHBL.ObjectTableId = this.EntityPM.ObjectTableId;
                this.SettingHBL.SettingCode = "HAWBCounterI_E_D";
                this.SettingHBL.SettingValue = "None";
                this.CounterAPIHelper.TenantSettings.push(this.SettingHBL);
            }

            this.IsFBLVisible = true;
            this.IsHBLVisible = true;
        }
    }

    // HWB
    public get HWBSettingValue() { return this.SettingHWB.SettingValue; }
    public set HWBSettingValue(value: string) {
        if (this.SettingHWB.SettingValue != value) {
            this.SettingHWB.SettingValue = value;
        }
    }

    public get HWBPrefix() { return this.SettingHWB.Prefix; }
    public set HWBPrefix(value: string) {
        if (this.SettingHWB.Prefix != value) {
            this.SettingHWB.Prefix = value;

            //this.UIProperties.SetValidity("HWBPrefix", "CounterDefinition", true, null);

            //if (!AppTool.IsNullOrEmpty(value)) {
            //    if (value.length > 10) {
            //        this.UIProperties.SetValidity("HWBPrefix", null, false, "Prefix Field must be less than 10");
            //    }
            //}
        }
    }

    public get HWBSize() { return this.SettingHWB.Size; }
    public set HWBSize(value: number) {
        if (this.SettingHWB.Size != value) {
            this.SettingHWB.Size = value;
        }
    }

    public get HWBDontIncludeDirects() { return this.SettingHWB.DontIncludeDirects; }
    public set HWBDontIncludeDirects(value: boolean) {
        if (this.SettingHWB.DontIncludeDirects != value) {
            this.SettingHWB.DontIncludeDirects = value;
        }
    }

    public get HWBStartNumber() { return this.CounterDefinitionHWB.StartNumber; }
    public set HWBStartNumber(value: number) {
        if (this.CounterDefinitionHWB.StartNumber != value) {
            this.CounterDefinitionHWB.StartNumber = value;
        }
    }


    // FBL
    public get FBLSettingValue() { return this.SettingFBL.SettingValue; }
    public set FBLSettingValue(value: string) {
        if (this.SettingFBL.SettingValue != value) {
            this.SettingFBL.SettingValue = value;
        }
    }

    public get FBLPrefix() { return this.SettingFBL.Prefix; }
    public set FBLPrefix(value: string) {
        if (this.SettingFBL.Prefix != value) {
            this.SettingFBL.Prefix = value;
        }
    }

    public get FBLSize() { return this.SettingFBL.Size; }
    public set FBLSize(value: number) {
        if (this.SettingFBL.Size != value) {
            this.SettingFBL.Size = value;
        }
    }

    public get FBLDontIncludeDirects() { return this.SettingFBL.DontIncludeDirects; }
    public set FBLDontIncludeDirects(value: boolean) {
        if (this.SettingFBL.DontIncludeDirects != value) {
            this.SettingFBL.DontIncludeDirects = value;
        }
    }

    public get FBLStartNumber() { return this.CounterDefinitionFBL.StartNumber; }
    public set FBLStartNumber(value: number) {
        if (this.CounterDefinitionFBL.StartNumber != value) {
            this.CounterDefinitionFBL.StartNumber = value;
        }
    }


    // HBL
    public get HBLSettingValue() { return this.SettingHBL.SettingValue; }
    public set HBLSettingValue(value: string) {
        if (this.SettingHBL.SettingValue != value) {
            this.SettingHBL.SettingValue = value;
        }
    }

    public get HBLPrefix() { return this.SettingHBL.Prefix; }
    public set HBLPrefix(value: string) {
        if (this.SettingHBL.Prefix != value) {
            this.SettingHBL.Prefix = value;
        }
    }

    public get HBLSize() { return this.SettingHBL.Size; }
    public set HBLSize(value: number) {
        if (this.SettingHBL.Size != value) {
            this.SettingHBL.Size = value;
        }
    }

    public get HBLDontIncludeDirects() { return this.SettingHBL.DontIncludeDirects; }
    public set HBLDontIncludeDirects(value: boolean) {
        if (this.SettingHBL.DontIncludeDirects != value) {
            this.SettingHBL.DontIncludeDirects = value;
        }
    }

    public get HBLStartNumber() { return this.CounterDefinitionHBL.StartNumber; }
    public set HBLStartNumber(value: number) {
        if (this.CounterDefinitionHBL.StartNumber != value) {
            this.CounterDefinitionHBL.StartNumber = value;
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        var isDirty: boolean = false;

        if (this.EntityPM.IsDirty) {
            isDirty = true;
        }

        if (this.CounterAPIHelper.TenantSettings.filter(f => f.IsDirty).length > 0) {
            isDirty = true;
        }

        if (this.CounterDefinitionHWB.Id != null && this.CounterDefinitionHWB.IsDirty) {
            isDirty = true;
        }

        if (this.CounterDefinitionFBL.Id != null && this.CounterDefinitionFBL.IsDirty) {
            isDirty = true;
        }

        if (this.CounterDefinitionHBL.Id != null && this.CounterDefinitionHBL.IsDirty) {
            isDirty = true;
        }

        if (this.HWBSettingValue == "Counter") {
            if (this.CounterAPIHelper.CounterDefinitions.filter(f => f.Parameter1 == "A").length == 0) {
                this.CounterAPIHelper.CounterDefinitions.push(this.CounterDefinitionHWB);
                isDirty = true;
            }
        }

        if (this.HasAllTransportsFeature) {
            if (this.FBLSettingValue == "Counter") {
                if (this.CounterAPIHelper.CounterDefinitions.filter(f => f.Parameter1 == "O").length == 0) {
                    this.CounterAPIHelper.CounterDefinitions.push(this.CounterDefinitionFBL);
                    isDirty = true;
                }
            }

            if (this.HBLSettingValue == "Counter") {
                if (this.CounterAPIHelper.CounterDefinitions.filter(f => f.Parameter1 == "I").length == 0) {
                    this.CounterAPIHelper.CounterDefinitions.push(this.CounterDefinitionHBL);
                    isDirty = true;
                }
            }
        }

        if (!isDirty) {
            this.CurrentSession.CloseCurrentWindow();
        }

        else {
            var isValidGreaterStartNumber: boolean = true;
            var isFieldLengthValid_HWB: boolean = true;
            var isFieldLengthValid_FBL: boolean = true;
            var isFieldLengthValid_HBL: boolean = true;

            if (this.HWBSettingValue == "Counter") {
                if (!AppTool.IsNullOrEmpty(this.CounterDefinitionHWB.StartNumber)) {

                    if (this.CounterDefinitionHWB.StartNumber < this.CounterDefinitionHWB.StartNumber_Old) {
                        isValidGreaterStartNumber = false;
                    }

                    isFieldLengthValid_HWB = this.ValidateFieldLength(this.CounterDefinitionHWB.StartNumber, this.HWBSize, this.HWBPrefix);                    
                }                
            }

            if (this.HasAllTransportsFeature) {
                if (this.FBLSettingValue == "Counter") {
                    if (!AppTool.IsNullOrEmpty(this.CounterDefinitionFBL.StartNumber)) {

                        if (this.CounterDefinitionFBL.StartNumber < this.CounterDefinitionFBL.StartNumber_Old) {
                            isValidGreaterStartNumber = false;
                        }

                        isFieldLengthValid_FBL = this.ValidateFieldLength(this.CounterDefinitionFBL.StartNumber, this.FBLSize, this.FBLPrefix);
                    }
                }

                if (this.HBLSettingValue == "Counter") {
                    if (!AppTool.IsNullOrEmpty(this.CounterDefinitionHBL.StartNumber)) {

                        if (this.CounterDefinitionHBL.StartNumber < this.CounterDefinitionHBL.StartNumber_Old) {
                            isValidGreaterStartNumber = false;
                        }

                        isFieldLengthValid_HBL = this.ValidateFieldLength(this.CounterDefinitionHBL.StartNumber, this.HBLSize, this.HBLPrefix);
                    }
                }
            }

            if (!isValidGreaterStartNumber) {
                var messageWindow = new MessageWindow();
                messageWindow.Show("The new start number must be greater than current start number!");
            }

            else if (!isFieldLengthValid_HWB) {
                var messageWindow = new MessageWindow();
                messageWindow.Show("HAWB number max length is 20");
            }

            else if (!isFieldLengthValid_FBL) {
                var messageWindow = new MessageWindow();
                messageWindow.Show("FBL number max length is 20");
            }

            else if (!isFieldLengthValid_HBL) {
                var messageWindow = new MessageWindow();
                messageWindow.Show("HBL number max length is 20");
            }

            else {
                var errors: string[] = [];
                Validator.TryValidateObject(this.CounterDefinitionHWB, this.ObjectTableName, errors);
                Validator.TryValidateObject(this.CounterDefinitionFBL, this.ObjectTableName, errors);
                Validator.TryValidateObject(this.CounterDefinitionHBL, this.ObjectTableName, errors);

                var isPrefixLengthValid: boolean = this.ValidatePrefixLength();
                if (!isPrefixLengthValid) {
                    errors.push("Prefix Field must be less than 10");
                }

                this.ValidationErrorsList = errors;

                if (errors.length == 0) {
                    this.CurrentSession.StartBusyIndicatorSaving();

                    var myService = new CountersDomainService();
                    myService.Post(this.CounterAPIHelper).subscribe((myResponse: ServiceResponse) => {

                        this.CurrentSession.StopBusyIndicator();

                        if (myResponse.HasError) {
                            this.ValidationErrorsList = myResponse.ErrorsArray;
                        }

                        else {
                            SessionLocator.TenantSettings = this.CounterAPIHelper.TenantSettings;
                            this.CurrentSession.CloseCurrentWindowEmit("Ok");
                        }
                    });
                }
            }
        }
    }

    ValidatePrefixLength() {
        var isValid: boolean = true;

        if (this.HWBPrefix) {
            if (this.HWBPrefix.length > 10) {
                isValid = false;
            }
        }

        if (this.FBLPrefix) {
            if (this.FBLPrefix.length > 10) {
                isValid = false;
            }
        }

        if (this.HBLPrefix) {
            if (this.HBLPrefix.length > 10) {
                isValid = false;
            }
        }

        return isValid;
    }

    ValidateFieldLength(myStartNumber: number, mySize: number, myPrefix: string) {
        var isValid: boolean = true;
        var dbFieldLength: number = 20;
        var expectedField: string = "";

        if (!AppTool.IsNullOrZero(this.CounterAPIHelper.LastDBValue)) {
            expectedField = this.CounterAPIHelper.LastDBValue + 1 + "";
        }

        else {
            expectedField = myStartNumber + 1 + "";
        }

        if (!AppTool.IsNullOrZero(mySize)) {
            expectedField = AppTool.PadLeft(expectedField, mySize, "0");
        }

        if (!AppTool.IsNullOrEmpty(myPrefix)) {
            expectedField = myPrefix + expectedField;
        }

        if (expectedField) {
            if (expectedField.length > dbFieldLength) {
                isValid = false;
            }
        }

        return isValid;
    }
}
