import { Component, OnInit } from '@angular/core';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { LocationDirective } from '../../../../Infrastructure/Utilities/LocationDirective';
import { ApiQueryFilters, FilterItem } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { AppTool, ArrayTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';





import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { ExceptionReasonPM } from '../../../../Customs/EntityPMs/ExceptionReasonPM';
import { ExceptionReasonPMService } from '../../../../Customs/Services/StandardPMs/ExceptionReasonPMService';
import { ExceptionReasonExtendedListService } from '../../../../Customs/Services/ExtendedLists/ExceptionReasonExtendedListService';


@Component({
    templateUrl: './AddEditReferantExceptionReasonComponent.html',
})

export class AddEditReferantExceptionReasonComponent
    extends BaseComponent
    implements OnInit {

    public DataContext: any = this;
    public ObjectTableName: string = "Customs.ExceptionReason";
    public EntityPM: ExceptionReasonPM;
    isWindowMode: boolean = false;
    isNewRecord: boolean = false;
    isFromUnifreight: boolean = false;
    ValidationErrorsList: any[] = [];
    private _EntityResourceService: EntityResourceService = new EntityResourceService();

    _ExceptionReasonPMService: ExceptionReasonPMService = new ExceptionReasonPMService();
    _ExceptionReasonExtendedListService: ExceptionReasonExtendedListService = new ExceptionReasonExtendedListService();

    constructor(public entityArgs: EntityArgs) {
        super();

        SessionLocator.SelectedSession.StartBusyIndicator("");
        this._EntityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(response => {

            SessionLocator.SelectedSession.StopBusyIndicator();
            if (AppTool.IsNullOrEmpty(entityArgs.EntityPM) || !(entityArgs.EntityPM instanceof ExceptionReasonPM)) {
                this.EntityPM = new ExceptionReasonPM();
                this.EntityPM.Tenant = SessionLocator.Tenant;
                this.isWindowMode = true;
                this.isNewRecord = true;

            } else {
                this.EntityPM = this.entityArgs.EntityPM;
                this.UnifreightStatusCode = this.EntityPM.UnifreightStatusCode;
            }
            this.UIProperties.SetEnabled("UnifreightStatusCode", this.ObjectTableName, false);
        });
    }

    Loaded: boolean = false;
    ngOnInit() {
        SessionLocator.SelectedSession.StartBusyIndicatorLoading();
        this._EntityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(response => {
            SessionLocator.SelectedSession.StopBusyIndicator();
            this.Loaded = true;
        });
    }

    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.isWindowMode = true;
            this.isFromUnifreight = true;

            if (args.FromUnifreight && !AppTool.IsNullOrEmpty(args.UnifreightStatusCode)) {
                this.isNewRecord = true;
                this.EntityPM = new ExceptionReasonPM();
                this.EntityPM.Tenant = SessionLocator.Tenant;
                this.UnifreightStatusCode = args.UnifreightStatusCode;
                this.UIProperties.SetEnabled("UnifreightStatusCode", this.ObjectTableName, false);

                this._ExceptionReasonExtendedListService.GetExceptionReasonByUnifreightStatus(this.UnifreightStatusCode).subscribe((response:any) => {
                    var exceptionReasonResult: ExceptionReasonPM[] = response.Result;
                    if (exceptionReasonResult != null && exceptionReasonResult.length > 0) {
                        this.isNewRecord = false;
                        this.EntityPM = exceptionReasonResult[0];
                        if (exceptionReasonResult.length > 1) {
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

    public get IsActive() { return this.EntityPM.IsActive; }
    public set IsActive(newValue: boolean) {
        this.EntityPM.IsActive = newValue;
    }


    private _UnifreightStatusCode: string;
    public get UnifreightStatusCode() { return this._UnifreightStatusCode; }
    public set UnifreightStatusCode(newValue: string) {
        this._UnifreightStatusCode = newValue;
    }

    //#endregion\

    OkButtonClicked() {
        var errors = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (this.isNewRecord) {
            var first3LettersNumber: number = +this.EntityPM.Code.substring(0, 3);
            if (first3LettersNumber >= 900) {
                errors.push("לא ניתן להגדיר קוד שמתחיל ב 900");
            }
        }

        if (errors.length > 0) {
            this.ValidationErrorsList = [];
            this.ValidationErrorsList = errors;
        } else {
            if (this.EntityPM == null || (this.EntityPM != null && AppTool.IsNullOrEmpty(this.EntityPM.Code))) {
                this.CancelButtonClicked();
            }
            this.EntityPM.UnifreightStatusCode = this._UnifreightStatusCode;
            if (this.isNewRecord) {
                this._ExceptionReasonPMService.insert(this.EntityPM).subscribe(myResult => {
                    if (myResult.HasError) {
                        this.ValidationErrorsList = [];
                        this.ValidationErrorsList.push(myResult.ErrorsArray[0]);
                        return;
                    }
                    this.CancelButtonClicked();
                });
            }
            else {
                this._ExceptionReasonPMService.update(this.EntityPM).subscribe(myResult => {
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
        SessionLocator.SelectedSession.CloseCurrentWindow();
    }


}
