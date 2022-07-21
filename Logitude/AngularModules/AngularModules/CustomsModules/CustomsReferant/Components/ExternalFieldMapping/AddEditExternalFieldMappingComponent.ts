import { Component, OnInit } from '@angular/core';
import { ExternalFieldMappingPM } from 'Customs/EntityPMs/ExternalFieldMappingPM';
import { ExternalFieldMappingPMService } from 'Customs/Services/StandardPMs/ExternalFieldMappingPMService';
import { GTBFUSTATUWebService } from 'Customs/Services/WebServices/GTBFUSTATUWebService';
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ServiceArgs } from 'Infrastructure/DataContracts/ServiceArgs';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { FieldsTranslations, FieldsUpdateHelper, GeneralDomainService } from 'Infrastructure/Services/GeneralDomainService';
import { TextCodePMService } from 'Infrastructure/Services/StandardPMs/TextCodePMService';
import { AppTool } from 'Infrastructure/Tools';
import { CachedDataManager } from 'Infrastructure/Utilities/CachedDataManager';
import { ServiceHelper } from 'Infrastructure/Utilities/ServiceHelper';
import { StandardFieldItem } from 'InfrastructureModules/InfrastructureCustomization/Components/Customization/StandardFieldsComponent';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { Validator } from '../../../../Infrastructure/Validators/Validator';


@Component({
    templateUrl: './AddEditExternalFieldMappingComponent.html',
})
export class AddEditExternalFieldMappingComponent
    extends BaseComponent
    implements OnInit {


    public DataContext: any = this;
    public ObjectTableName: string = "Customs.ExternalFieldMapping";
    isWindowMode: boolean = false;
    isNewRecord: boolean = false;
    ValidationErrorsList: any[] = [];
    private _EntityResourceService: EntityResourceService = new EntityResourceService();
    private _GTBFUSTATUWebService: GTBFUSTATUWebService = new GTBFUSTATUWebService();
    Loaded: boolean = false;
    _ExternalFieldMappingPMService: ExternalFieldMappingPMService = new ExternalFieldMappingPMService();


    constructor(public entityArgs: EntityArgs) {
        super();
        this._EntityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((response: any) => {
            this.Loaded = true;
            if (AppTool.IsNullOrEmpty(entityArgs.EntityPM) || !(entityArgs.EntityPM instanceof ExternalFieldMappingPM)) {
                this.EntityPM = new ExternalFieldMappingPM();
                this.EntityPM.Tenant = SessionLocator.Tenant;
                this.isWindowMode = true;
                this.isNewRecord = true;

            } else {
                this.EntityPM = this.entityArgs.EntityPM;
            }
        });
    }

    SetUIProperties() {
        this.UIProperties.SetEnabled("StatusName", this.ObjectTableName, false);
        if (!this.isWindowMode) {
            this.UIProperties.SetEnabled("StatusFieldType", this.ObjectTableName, false);
        }
    }
    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.isWindowMode = true;
            this.isNewRecord = true;
            this.EntityPM = new ExternalFieldMappingPM();
            this.EntityPM.Tenant = SessionLocator.Tenant;

        }
    }

    private _WarningMessage: string;
    public get WarningMessage() { return this._WarningMessage; }
    public set WarningMessage(newValue: string) {
        this._WarningMessage = newValue;
    }

    public get StatusFieldType() { return this.EntityPM.StatusFieldType; }
    public set StatusFieldType(newValue: string) {
        this.EntityPM.StatusFieldType = newValue;
        this.UpdateStatusName();
    }

    public get StatusCode() { return this.EntityPM.StatusCode; }
    public set StatusCode(newValue: string) {
        this.EntityPM.StatusCode = newValue;
        this.UpdateStatusName();
    }

    public get StatusName() { return this.EntityPM.StatusName }
    public set StatusName(newValue: string) {
        this.EntityPM.StatusName = newValue;
    }

    UpdateStatusName() {
        var value = this.EntityPM.StatusCode;
        if (value != null && this.StatusFieldType != null) {
            switch (this.StatusFieldType) {
                case "1": {
                    value = "קוד "+value;
                    break;
                }
                case "2": {
                    value = "תאריך "+value;
                    break;
                }
                case "3": {
                    value = "הערות "+value;
                    break;
                }
                default: {
                    //statements; 
                    break;
                }
            }
            this.StatusName = value;
        }
    }

    public get InActive() { return this.EntityPM.InActive }
    public set InActive(newValue: string) {
        this.EntityPM.InActive = newValue;
    }

    ngOnInit() {
        this.SetUIProperties();
    }

    private serviceArgs: ServiceArgs;
    async CreateFieldsTranslationsList(fieldCode: string) {
        this.serviceArgs = new ServiceArgs();
        this.serviceArgs.http = ServiceHelper.HttpClient;
        var list: FieldsTranslations[] = [];
        list.push(await this.CreateFieldsTranslations("Customs.DeclarationReferantData.CH." + fieldCode + "ListLable", fieldCode));
        list.push(await this.CreateFieldsTranslations("Customs.DeclarationReferantData.F." + fieldCode, fieldCode));
        var myServiceHelper = new FieldsUpdateHelper();
        myServiceHelper.Tenant = SessionLocator.Tenant;
        myServiceHelper.Items = list;
        var generalService: GeneralDomainService = new GeneralDomainService();
        generalService.UpdateFieldsTranslations(myServiceHelper).subscribe((myResponse: ServiceResponse) => {
            if (myResponse.HasError) {
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }

            else {
                CachedDataManager.RefreshTenantTextCodes().subscribe((response: any) => {
                });
            }
        });
    }
    async CreateFieldsTranslations(code: string, defaultText: string) {
        var fieldsTranslation = new FieldsTranslations();
        fieldsTranslation.Code = code;
        fieldsTranslation.DefaultText = defaultText;
        fieldsTranslation.TranslationLanguageCode = "HE";
        fieldsTranslation.TranslationTenent = SessionLocator.Tenant;
        fieldsTranslation.TranslatedText = this.StatusName;
        var textCodesService: TextCodePMService = new TextCodePMService();
        textCodesService.setServiceArgs(this.serviceArgs);
        await new Promise<void>((resolve, reject) => {
            textCodesService.getByCode(code, 0).subscribe((res: any) => {
                if (res != null) {
                    fieldsTranslation.TextCodeId = res.TextCodeId;
                    fieldsTranslation.ObjectTableID = res.ObjectTableID;
                    fieldsTranslation.TypeCode = res.TypeCode;
                    fieldsTranslation.ObjectTableTypeCode = res.ObjectTableTypeCode;
                    fieldsTranslation.ObjectTableName = res.ObjectTableName;
                }
                resolve();
            })
        });
        return fieldsTranslation;
    }

    OkButtonClicked() {
        var errors = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (this.isNewRecord) {
            this._ExternalFieldMappingPMService.insert(this.EntityPM).subscribe(myResult => {
                if (myResult.HasError) {
                    this.ValidationErrorsList = [];
                    this.ValidationErrorsList.push(myResult.ErrorsArray[0]);
                    return;
                } else {
                    this.CancelButtonClicked();
                    var myServiceHelper = new FieldsUpdateHelper();
                    myServiceHelper.Tenant = SessionLocator.Tenant;
                    this.CreateFieldsTranslationsList(myResult?.Result?.field);
                    //myServiceHelper.Items = list;
                }
            });
        }
        else {
            this._ExternalFieldMappingPMService.update(this.EntityPM).subscribe(myResult => {
                if (myResult.HasError) {
                    this.ValidationErrorsList = [];
                    this.ValidationErrorsList.push(myResult.ErrorsArray[0]);
                    return;
                }
                this.CancelButtonClicked();
            });
        }

    }
    CancelButtonClicked() {
        SessionLocator.SelectedSession.CloseCurrentWindow();
    }
}