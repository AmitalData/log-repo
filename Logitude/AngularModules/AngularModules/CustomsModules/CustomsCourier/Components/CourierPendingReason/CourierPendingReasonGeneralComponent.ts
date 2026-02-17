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
import {DeclarationCourierStatusPM} from '../../../../Customs/EntityPMs/DeclarationCourierStatusPM';
import {DeclarationCourierStatusPMService} from '../../../../Customs/Services/StandardPMs/DeclarationCourierStatusPMService';


@Component({
    moduleId: module.id,
    templateUrl: './CourierPendingReasonGeneralComponent.html',
})

export class CourierPendingReasonGeneralComponent extends BaseComponent {
    public DataContext: any = this;
    public ObjectTableName: string = "Customs.DeclarationCourierStatus";
    DeclarationsList: DeclarationCourierStatusPM[] = [];
    ValidationErrorsList: any[] = [];
    _DeclarationCourierStatusPMService: DeclarationCourierStatusPMService = new DeclarationCourierStatusPMService();
    _IsNewPending: boolean = true;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();

    }

    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.CourierHawb = args.CourierHawb;
            if (args.Mode == "FromDeclaration") {
                this.CurrentSession.StartBusyIndicatorLoading();
                this._DeclarationCourierStatusPMService.get(args.DeclarationId).subscribe((response: ServiceResponse) => {
                    this.CurrentSession.StopBusyIndicator();
                    var declarationCourierStatusPM: DeclarationCourierStatusPM = response.Result;
                    if (declarationCourierStatusPM != null) {
                        this.DeclarationsList.push(declarationCourierStatusPM);
                        if (!AppTool.IsNullOrEmpty(declarationCourierStatusPM.CourierPendingReasonCode) || !AppTool.IsNullOrEmpty(declarationCourierStatusPM.PendingRemarks)) {
                            this._IsNewPending = false;
                            this.CourierPendingReasonCode = declarationCourierStatusPM.CourierPendingReasonCode;
                            this.PendingRemarks = declarationCourierStatusPM.PendingRemarks;
                        }
                    }
                });
            }
            else {
                if (args.Mode == "Update") {
                    this._IsNewPending = false;
                    this.CourierPendingReasonCode = args.CourierPendingReasonCode;
                    this.PendingRemarks = args.PendingRemarks;
                }
                if (args.DeclarationIdList != null) {
                    args.DeclarationIdList.forEach((declarationCourierStatusPM: DeclarationCourierStatusPM) => {
                        this.DeclarationsList.push(declarationCourierStatusPM);
                    });
                }
            }
        }
    }

    //#region Properties
    private _CourierHawb: string;
    public get CourierHawb() { return this._CourierHawb; }
    public set CourierHawb(newValue: string) {
        this._CourierHawb = newValue;
    }

    private _CourierPendingReasonCode: string;
    public get CourierPendingReasonCode() { return this._CourierPendingReasonCode; }
    public set CourierPendingReasonCode(newValue: string) {
        this._CourierPendingReasonCode = newValue;
    }

    private _CourierPendingReasonName: string;
    public get CourierPendingReasonName() { return this._CourierPendingReasonName; }
    public set CourierPendingReasonName(newValue: string) {
        this._CourierPendingReasonName = newValue;
    }

    private _PendingRemarks: string;
    public get PendingRemarks() { return this._PendingRemarks; }
    public set PendingRemarks(newValue: string) {
        this._PendingRemarks = newValue;
    }

    //#endregion\

    OkButtonClicked() {
       
        this.CurrentSession.StartBusyIndicatorSaving();
        this.DeclarationsList.forEach((declarationCourierStatusPM: DeclarationCourierStatusPM) => {
            declarationCourierStatusPM.CourierPendingReasonCode = this.CourierPendingReasonCode;
            declarationCourierStatusPM.PendingRemarks = this.PendingRemarks;
            this._DeclarationCourierStatusPMService.update(declarationCourierStatusPM).subscribe((response: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();
                this.CurrentSession.CloseCurrentWindow();
            });
        });

    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
