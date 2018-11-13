import {Component, AfterViewInit, ChangeDetectorRef, ViewChildren, QueryList } from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {LocationDirective} from '../../../../Infrastructure/Utilities/LocationDirective';
import {ApiQueryFilters, FilterItem} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {AppTool, ArrayTool} from '../../../../Infrastructure/Tools';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {CustomSendOptionsArgs, SendRequestVIA} from '../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import {CourierPendingReasonPM} from '../../../../Customs/EntityPMs/CourierPendingReasonPM';
import { CourierPendingReasonPMService } from '../../../../Customs/Services/StandardPMs/CourierPendingReasonPMService';
import { CourierPendingReasonExtendedListService } from '../../../../Customs/Services/ExtendedLists/CourierPendingReasonExtendedListService';
import { List } from '../../../../Infrastructure/DataContracts/Dashboard/List';


@Component({
    moduleId: module.id,
    templateUrl: './AddEditCourierPendingReasonComponent.html',
})

export class AddEditCourierPendingReasonComponent extends BaseComponent {
    public DataContext: any = this;
    public ObjectTableName: string = "Customs.CourierPendingReason";
    public EntityPM: CourierPendingReasonPM;
    isWindowMode: boolean = false;
    isNewRecord: boolean = false;
    ValidationErrorsList: any[] = [];

    _CourierPendingReasonPMService: CourierPendingReasonPMService = new CourierPendingReasonPMService();
    _CourierPendingReasonExtendedListService: CourierPendingReasonExtendedListService = new CourierPendingReasonExtendedListService();

    constructor(public entityArgs: EntityArgs) {
        super();
        if (AppTool.IsNullOrEmpty(entityArgs.EntityPM)) {
            this.EntityPM = new CourierPendingReasonPM();
            this.EntityPM.Tenant = SessionLocator.Tenant;
            this.isWindowMode = true;
            this.isNewRecord = true;
        } else {
            this.EntityPM = this.entityArgs.EntityPM;
        }
        this.UIProperties.SetEnabled("UnifreightStatusCode", this.ObjectTableName, false);

    }

    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.isWindowMode = true;

            if (args.FromUnifreight && !AppTool.IsNullOrEmpty(args.UnifreightStatusCode)) {
                this.isNewRecord = true;
                this.EntityPM = new CourierPendingReasonPM();
                this.UnifreightStatusCode = args.UnifreightStatusCode;
                this.EntityPM.Code = args.UnifreightStatusCode;
                this.UIProperties.SetEnabled("UnifreightStatusCode", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("Code", this.ObjectTableName, false);

                this._CourierPendingReasonExtendedListService.GetCourierPendingReasonByUnifreightStatus(this.UnifreightStatusCode).subscribe(response => {
                    var courierPendingReasonResult: CourierPendingReasonPM[] = response.Result;
                    if (courierPendingReasonResult != null && courierPendingReasonResult.length > 0) {
                        this.isNewRecord = false;
                        this.EntityPM = courierPendingReasonResult[0];
                        if (courierPendingReasonResult.length > 1) {
                            this.WarningMessage = "סטטוס " + this.UnifreightStatusCode + " מקושר למספר קודים. מוצגת הרשומה הראשונה בלבד";
                        }
                    }
                });
            }
        }
    }


    //#region Properties
    private _WarningMessage: string;
    public get WarningMessage() { return this._WarningMessage; }
    public set WarningMessage(newValue: string) {
        this._WarningMessage = newValue;
    }

    public get Code() { return this.EntityPM.Code; }
    public set Code(newValue: string) {
        this.EntityPM.Code = newValue;
    }

    public get EnglishName() { return this.EntityPM.EnglishName; }
    public set EnglishName(newValue: string) {
        this.EntityPM.EnglishName = newValue;
    }

    public get LocalName() { return this.EntityPM.LocalName; }
    public set LocalName(newValue: string) {
        this.EntityPM.LocalName = newValue;
    }

    public get Inactive() { return this.EntityPM.Inactive; }
    public set Inactive(newValue: boolean) {
        this.EntityPM.Inactive = newValue;
    }

    public get ErrorPlace() { return this.EntityPM.ErrorPlace; }
    public set ErrorPlace(newValue: string) {
        this.EntityPM.ErrorPlace = newValue;
    }

    public get UnifreightStatusCode() { return this.EntityPM.UnifreightStatusCode; }
    public set UnifreightStatusCode(newValue: string) {
        this.EntityPM.UnifreightStatusCode = newValue;
    }

    //#endregion\

    OkButtonClicked() {

        var errors = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (errors.length > 0) {
            this.ValidationErrorsList = [];
            this.ValidationErrorsList = errors;
        } else {
            if (this.isNewRecord) {
                this._CourierPendingReasonPMService.insert(this.EntityPM).subscribe(myResult => {
                    if (myResult.HasError) {
                        this.ValidationErrorsList = [];
                        this.ValidationErrorsList.push(myResult.ErrorsArray[0]);
                        return;
                    }
                    this.CancelButtonClicked();
                });
            }
            else {
                this._CourierPendingReasonPMService.update(this.EntityPM).subscribe(myResult => {
                    if (myResult.HasError) {
                        this.ValidationErrorsList = [];
                        this.ValidationErrorsList.push(myResult.ErrorsArray[0]);
                        return;
                    }
                    this.CancelButtonClicked();
                });
            }
        }
    }

    CancelButtonClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindow();
    }


}
