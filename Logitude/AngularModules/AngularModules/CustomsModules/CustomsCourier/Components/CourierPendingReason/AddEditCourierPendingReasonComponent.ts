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
import { CourierPendingReasonPM } from '../../../../Customs/EntityPMs/CourierPendingReasonPM';
import { CourierPendingReasonPMService } from '../../../../Customs/Services/StandardPMs/CourierPendingReasonPMService';
import { CourierPendingReasonExtendedListService } from '../../../../Customs/Services/ExtendedLists/CourierPendingReasonExtendedListService';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';





import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { FeatureLocator } from 'Infrastructure/Utilities/FeatureLocator';

 
@Component({
    
    templateUrl: './AddEditCourierPendingReasonComponent.html',
})

export class AddEditCourierPendingReasonComponent
    extends BaseComponent
    implements OnInit{

    public DataContext: any = this;
    public ObjectTableName: string = "Customs.CourierPendingReason";
    isWindowMode: boolean = false;
    isNewRecord: boolean = false;
    isFromUnifreight: boolean = false;
    ValidationErrorsList: any[] = [];
    private _EntityResourceService: EntityResourceService = new EntityResourceService();
    HasRequiresApprovalFeature:boolean=false;

    _CourierPendingReasonPMService: CourierPendingReasonPMService = new CourierPendingReasonPMService();
    _CourierPendingReasonExtendedListService: CourierPendingReasonExtendedListService = new CourierPendingReasonExtendedListService();
    private currentSession=SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        super();
        if (FeatureLocator.HasFeaturePermession("Customs.CourierPendingReason", "PendingRequiresApproval")) {
            this.HasRequiresApprovalFeature = true;
        }
        this.currentSession.StartBusyIndicator("");
        this._EntityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((response:any) => {

            SessionLocator.SelectedSession.StopBusyIndicator();
            if (AppTool.IsNullOrEmpty(entityArgs.EntityPM) || !(entityArgs.EntityPM instanceof CourierPendingReasonPM)) {
                this.EntityPM = new CourierPendingReasonPM();
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
        this.currentSession.StartBusyIndicatorLoading();
        this._EntityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((response:any) => {
            this.currentSession.StopBusyIndicator();
            this.Loaded = true;
        });
    }

    SetWindowArgs(args: any) {
        
        if (!AppTool.IsNullOrEmpty(args)) {
            this.isWindowMode = true;
            this.isFromUnifreight = true;

            if (args.FromUnifreight && !AppTool.IsNullOrEmpty(args.UnifreightStatusCode)) {
                this.isNewRecord = true;
                this.EntityPM = new CourierPendingReasonPM();
                this.EntityPM.Tenant = SessionLocator.Tenant;
                this.UnifreightStatusCode = args.UnifreightStatusCode;
                this.UIProperties.SetEnabled("UnifreightStatusCode", this.ObjectTableName, false);

                this._CourierPendingReasonExtendedListService.GetCourierPendingReasonByUnifreightStatus(this.UnifreightStatusCode).subscribe((response:any) => {
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

    public get PendingCode() { return this.EntityPM.Code; }
    public set PendingCode(newValue: string) {
        if (this.isFromUnifreight) {
            if (this.EntityPM != null && !AppTool.IsNullOrEmpty(this.EntityPM.Code)) {
                this.EntityPM.UnifreightStatusCode = null;
                this.currentSession.StartBusyIndicatorLoading();
                this._CourierPendingReasonPMService.update(this.EntityPM).subscribe((myResult:any) => {
                    this.currentSession.StopBusyIndicator();
                    if (myResult.HasError) {
                        this.ValidationErrorsList = [];
                        this.ValidationErrorsList.push(myResult.ErrorsArray[0]);
                        return;
                    }
                    this.EntityPM = new CourierPendingReasonPM();
                });
            }
            if (newValue) {
                this.currentSession.StartBusyIndicatorLoading();
                this._CourierPendingReasonPMService.get(newValue).subscribe((response:any) => {
                    if (!response.HasError && response.Result != null) {
                        SessionLocator.SelectedSession.StopBusyIndicator();
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
                                    this.EntityPM = new CourierPendingReasonPM();
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

    public get MamanSuspendedCode() { return this.EntityPM.MamanSuspendedCode; }
    public set MamanSuspendedCode(newValue: string) {
        this.EntityPM.MamanSuspendedCode = newValue;
    }

    public get SwissportSuspendedCode() { return this.EntityPM.SwissportSuspendedCode; }
    public set SwissportSuspendedCode(newValue: string) {
        this.EntityPM.SwissportSuspendedCode = newValue;
    }

    public get OverseasSuspendedCode() { return this.EntityPM.OverseasSuspendedCode; }
    public set OverseasSuspendedCode(newValue: string) {
        this.EntityPM.OverseasSuspendedCode = newValue;
    }

    public get ErrorPlace() { return this.EntityPM.ErrorPlace; }
    public set ErrorPlace(newValue: string) {
        this.EntityPM.ErrorPlace = newValue;
    }

    public get RequiresApproval() { return this.EntityPM.RequiresApproval; }
    public set RequiresApproval(newValue: boolean) {
        this.EntityPM.RequiresApproval = newValue;
    }

    public get RequiresPayment() { return this.EntityPM.RequiresPayment; }
    public set RequiresPayment(newValue: boolean) {
        this.EntityPM.RequiresPayment = newValue;
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
            if (this.EntityPM == null || (this.EntityPM != null && AppTool.IsNullOrEmpty(this.EntityPM.Code))){
                this.CancelButtonClicked();
            }
            this.EntityPM.UnifreightStatusCode = this._UnifreightStatusCode;
            if (this.isNewRecord) {
                this._CourierPendingReasonPMService.insert(this.EntityPM).subscribe((myResult:any) => {
                    if (myResult.HasError) {
                        this.ValidationErrorsList = [];
                        this.ValidationErrorsList.push(myResult.ErrorsArray[0]);
                        return;
                    }
                    this.CancelButtonClicked();
                });
            }
            else {
                this._CourierPendingReasonPMService.update(this.EntityPM).subscribe((myResult:any) => {
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
