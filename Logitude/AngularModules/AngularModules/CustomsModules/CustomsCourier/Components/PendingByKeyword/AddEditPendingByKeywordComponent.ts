import { Component, OnInit } from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {LocationDirective} from '../../../../Infrastructure/Utilities/LocationDirective';
import {ApiQueryFilters, FilterItem} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {AppTool, ArrayTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { PendingByKeywordPM } from '../../../../Customs/EntityPMs/PendingByKeywordPM';
import { PendingByKeywordPMService } from '../../../../Customs/Services/StandardPMs/PendingByKeywordPMService';
//import { PendingByKeywordExtendedListService } from '../../../../Customs/Services/ExtendedLists/PendingByKeywordExtendedListService';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';





import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';


@Component({
    moduleId: module.id,
    templateUrl: './AddEditPendingByKeywordComponent.html',
})

export class AddEditPendingByKeywordComponent
    extends BaseComponent
    implements OnInit{

    public DataContext: any = this;
    public ObjectTableName: string = "Customs.PendingByKeyword";
    public EntityPM: PendingByKeywordPM;
    isWindowMode: boolean = false;
    isNewRecord: boolean = false;
    isFromUnifreight: boolean = false;
    ValidationErrorsList: any[] = [];
    private _EntityResourceService: EntityResourceService = new EntityResourceService();

    _PendingByKeywordPMService: PendingByKeywordPMService = new PendingByKeywordPMService();
    //_PendingByKeywordExtendedListService: PendingByKeywordExtendedListService = new PendingByKeywordExtendedListService();

    constructor(public entityArgs: EntityArgs) {
        super();

        SessionLocator.CurrentSession.StartBusyIndicator("");
        this._EntityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(response => {

            SessionLocator.CurrentSession.StopBusyIndicator();
            if (AppTool.IsNullOrEmpty(entityArgs.EntityPM)) {
                this.EntityPM = new PendingByKeywordPM();
                this.EntityPM.Tenant = SessionLocator.Tenant;
                this.isWindowMode = true;
                this.isNewRecord = true;

            } else {
                this.EntityPM = this.entityArgs.EntityPM;
                //this.UnifreightStatusCode = this.EntityPM.UnifreightStatusCode;
            }
            this.UIProperties.SetEnabled("UnifreightStatusCode", this.ObjectTableName, false);
        });
    }

    Loaded: boolean = false;
    ngOnInit() {
        SessionLocator.CurrentSession.StartBusyIndicatorLoading();
        this._EntityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(response => {
            SessionLocator.CurrentSession.StopBusyIndicator();
            this.Loaded = true;
        });
    }

    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.isWindowMode = true;
            this.isFromUnifreight = true;

            if (args.FromUnifreight && !AppTool.IsNullOrEmpty(args.UnifreightStatusCode)) {
                this.isNewRecord = true;
                this.EntityPM = new PendingByKeywordPM();
                this.EntityPM.Tenant = SessionLocator.Tenant;
                //this.UnifreightStatusCode = args.UnifreightStatusCode;
                this.UIProperties.SetEnabled("UnifreightStatusCode", this.ObjectTableName, false);

                /*this._PendingByKeywordExtendedListService.GetPendingByKeywordByUnifreightStatus(this.UnifreightStatusCode).subscribe(response => {
                    var PendingByKeywordResult: PendingByKeywordPM[] = response.Result;
                    if (PendingByKeywordResult != null && PendingByKeywordResult.length > 0) {
                        this.isNewRecord = false;
                        this.EntityPM = PendingByKeywordResult[0];
                        if (PendingByKeywordResult.length > 1) {
                            this.WarningMessage = "סטטוס " + this.UnifreightStatusCode + " מקושר למספר קודים. מוצגת הרשומה הראשונה בלבד";
                        }
                    }
                });*/
            }
        }
    }


    //#region Properties
    private _WarningMessage: string;
    public get WarningMessage() { return this._WarningMessage; }
    public set WarningMessage(newValue: string) {
        this._WarningMessage = newValue;
    }

    public get CourierPendingReasonCode() { return this.EntityPM.CourierPendingReasonCode; }
    public set CourierPendingReasonCode(newValue: string) {
        this.EntityPM.CourierPendingReasonCode = newValue;
    }
    /*
    public get PendingCode() { return this.EntityPM.Code; }
    public set PendingCode(newValue: string) {
        if (this.isFromUnifreight) {
            if (this.EntityPM != null && !AppTool.IsNullOrEmpty(this.EntityPM.Code)) {
                this.EntityPM.UnifreightStatusCode = null;
                SessionLocator.CurrentSession.StartBusyIndicatorLoading();
                this._PendingByKeywordPMService.update(this.EntityPM).subscribe(myResult => {
                    SessionLocator.CurrentSession.StopBusyIndicator();
                    if (myResult.HasError) {
                        this.ValidationErrorsList = [];
                        this.ValidationErrorsList.push(myResult.ErrorsArray[0]);
                        return;
                    }
                    this.EntityPM = new PendingByKeywordPM();
                });
            }
            if (newValue) {
                SessionLocator.CurrentSession.StartBusyIndicatorLoading();
                this._PendingByKeywordPMService.get(newValue).subscribe(response => {
                    if (!response.HasError && response.Result != null) {
                        SessionLocator.CurrentSession.StopBusyIndicator();
                        if (!AppTool.IsNullOrEmpty(response.Result.UnifreightStatusCode) && response.Result.UnifreightStatusCode != this.UnifreightStatusCode) {
                            var confirm = new ConfirmWindow();
                            confirm.Width = 350;
                            confirm.Height = 200;
                            confirm.Title = "קישור Pending לסטטוס";
                            confirm.YesButtonText = TextCodeTranslator.Translate("General.B.Yes");
                            confirm.ShowNoButton = true;
                            confirm.Show("לקוד זה כבר קושר סטטוס " + response.Result.UnifreightStatusCode + " האם להחליף לסטטוס " + this.UnifreightStatusCode + "?");
                            confirm.WindowClosed.subscribe((event: any) => {
                                if (confirm.No) {
                                    confirm.Close();
                                    this.EntityPM = new PendingByKeywordPM();
                                    return;
                                }
                                confirm.Close();
                            });
                        }
                        this.isNewRecord = false;
                        this.EntityPM = response.Result;
                        this.EntityPM.UnifreightStatusCode = this.UnifreightStatusCode;
                    }
                });
            }
        }
    }
    */

    public get CourierPendingReasonName() { return this.EntityPM.CourierPendingReasonName; }
    public set CourierPendingReasonName(newValue: string) {
        this.EntityPM.CourierPendingReasonName = newValue;
    }

    public get KeywordsList() { return this.EntityPM.KeywordsList; }
    public set KeywordsList(newValue: string) {
        this.EntityPM.KeywordsList = newValue;
    }
    

    //#endregion\

    OkButtonClicked() {

        var errors = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (this.isNewRecord) {
            
        }

        if (errors.length > 0) {
            this.ValidationErrorsList = [];
            this.ValidationErrorsList = errors;
        } else {
            if (this.EntityPM == null || (this.EntityPM != null && AppTool.IsNullOrEmpty(this.EntityPM.CourierPendingReasonCode))){
                this.CancelButtonClicked();
            }
            if (this.isNewRecord) {
                this._PendingByKeywordPMService.insert(this.EntityPM).subscribe(myResult => {
                    if (myResult.HasError) {
                        this.ValidationErrorsList = [];
                        this.ValidationErrorsList.push(myResult.ErrorsArray[0]);
                        return;
                    }
                    this.CancelButtonClicked();
                });
            }
            else {
                this._PendingByKeywordPMService.update(this.EntityPM).subscribe(myResult => {
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
