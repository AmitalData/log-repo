import { Component, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { AppTool, ArrayTool } from '../../../Infrastructure/Tools';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { PhysicalCheckPMService } from '../../Services/StandardPMs/PhysicalCheckPMService'
import { DeclarationReferantDataPMService } from '../../Services/StandardPMs/DeclarationReferantDataPMService';
import { DeclarationReferantDataPM } from '../../EntityPMs/DeclarationReferantDataPM';
import { BaseRequestsSheetMassaging } from '../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ReferantExceptionPM } from '../../EntityPMs/ReferantExceptionPM';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { KeyValuePair } from '../../../CustomsModules/CustomsCourier/Components/CourierWorkSheet/CourierWorksheetComponent';
import { ReferantExceptionPMService } from '../../Services/StandardPMs/ReferantExceptionPMService';
import { EntityResourceService } from "../../../Infrastructure/Services/EntityResourceService";
import { ExceptionReasonPM } from '../../EntityPMs/ExceptionReasonPM';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';

@Component({
    moduleId: module.id,
    templateUrl: './ReferantSpotlightDataTemplate.html',
})


export class ReferantSpotlightDataTemplate
    extends BaseComponent
    implements AfterViewInit{
    ngAfterViewInit(): void { 
        this.ShowBusyIndicator = false;
    }
    public DataContext: any = this;
    public ReferantExceptionItemsSource: ObservableCollection;
    public ObjectTableName: string = "Customs.DeclarationReferantData";
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    private _DeclarationReferantDataPMService: DeclarationReferantDataPMService = new DeclarationReferantDataPMService();
    private referantExceptionPmService: ReferantExceptionPMService = new ReferantExceptionPMService();

    _IsReady: boolean = false;
    constructor(private EntityResourceService: EntityResourceService) {
        super();
        this.EntityResourceService.getEntityResourceByTableName("Customs.DeclarationReferantData").subscribe(response => {
            this._IsReady = true;
        });
        this.ReferantExceptionItemsSource = new ObservableCollection([]);
        this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");

    }

    IsChanged: boolean = false;
    private showBusyIndicator: boolean = false;
    get ShowBusyIndicator() { return this.showBusyIndicator; }
    set ShowBusyIndicator(value: boolean) {
        if (this.showBusyIndicator != value) {
            this.showBusyIndicator = value;
            this.CurrentSession.FireEvent("SpotLightDetectChanges");
        }
    }
    Run(entity: DeclarationReferantDataPM) {
        this.EntityPM = entity;
        this.LoadReferantException();
    }
    private LoadReferantException() {
        this.ShowBusyIndicator = true;
        this.SplitExceptionList(this.EntityPM.ExceptionReasonsList);
        this.getData(this.ExceptionsList);
    }

    private ReferantExceptionListPM : ReferantExceptionPM[] = [];
    getData(ExceptionsList: string[]) {
        for (let exceptionReasonsCode of this.ExceptionsList) {
            this.referantExceptionPmService.get(this.EntityPM.DeclarationId, exceptionReasonsCode)
                .subscribe((response: any) => {
                    this.ReferantExceptionListPM.push = response.Result;
                    this.ReferantExceptionItemsSource.Insert(new ExceptionReason(response.Result, this));
                });
        }
    }
    Add() {
        var item: ReferantExceptionPM = new ReferantExceptionPM();
        item.DeclarationId = this.EntityPM.DeclarationId;
        item.Tenant = this.EntityPM.Tenant;
        item.IsNew = true;
        item.Status = "A";
        this.ReferantExceptionListPM.includes(item);
        this.ReferantExceptionItemsSource.Insert(new ExceptionReason(item, this.EntityPM));
    }

    private ExceptionsList: string[] = [];
    private SplitExceptionList(exception: string) {
        if (exception != null) {
            this.ExceptionsList = exception.split(',');
        } else {
            this.Add();
        }
    }
    private BuildExceptionReasonsList(ExceptionsList: string[]) {
        var newValue: string;
        for (let item of ExceptionsList) {
            newValue = newValue + "," + item;
        }
    }

    public get FollowUpDate() { return this.EntityPM.FollowUpDate; }
    public set FollowUpDate(newValue: Date) { this.EntityPM.FollowUpDate = newValue; }

    public get ExceptionReasonsList() { return this.EntityPM.ExceptionReasonsList; }
    public set ExceptionReasonsList(newValue: string) { this.EntityPM.ExceptionReasonsList = newValue; }

    nRowEnded($event) {
        console.log("this.ReferantExceptionItemsSource.Length : " + this.ReferantExceptionItemsSource.Length);
        if (this.ReferantExceptionItemsSource != null && ($event) == this.ReferantExceptionItemsSource.Length) {
            this.Add();

        }
    }
    OnFocus() {
        if (this.ReferantExceptionItemsSource == null || this.ReferantExceptionItemsSource.Length == 0) {
            this.Add();
        }
    }
    public SelectedRow: any = null;
    OnRowSelected(itemComponent: any) {
        this.SelectedRow = itemComponent;
        this.IsChanged = true;
    }

    isValid: boolean;
    inValid: boolean;
    FIELD_IS_REQUIERD: string;

    OkButtonClicked() {
        debugger;
        this.ValidationErrorsList = [];
        var errors: string[] = [];
        this.isValid = true;
        this.inValid = false;
        for (let item of this.ReferantExceptionListPM) {
            if (AppTool.IsNullOrEmpty(item.ExceptionReasonsCode)) {
                errors.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.CourierPendingReasonCode")));
                this.inValid = true;
                this.isValid = false;
                break;
            }
            else {
                var existCodeList: string[] = [];
                this.ReferantExceptionItemsSource.Collection.forEach((item: ExceptionReason) => {
                    if (existCodeList != null && item != null && existCodeList.indexOf(item.ExceptionReasonsCode)) {
                        errors.push("כבר קיימת רשומה עם קוד חריג  " + item.ExceptionReasonsCode);
                        this.inValid = true;
                        this.isValid = false;
                    }
                    existCodeList.push(item.ExceptionReasonsCode);
                });
            }
            if (errors.length != 0) {
                this.ValidationErrorsList = errors;
            }
            if (this.inValid) {
                this.isValid = false;
                var confirm = new ConfirmWindow();
                confirm.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                confirm.WindowClosed.subscribe((event: any) => {
                    if (confirm.Yes) {
                        if (errors.length == 0) {
                            var isSave = 1;
                            this.BuildExceptionReasonsList(this.ExceptionsList);
                           if (isSave == 1) {
                               /*this._DeclarationReferantDataPMService.update(this.EntityPM).subscribe((response: ServiceResponse) => {

                                });*/
                               this.ReferantExceptionItemsSource.Collection.forEach((item: ExceptionReason) => {
                                   if (item.EntityPM.IsNew == true) {
                                       debugger;
                                   } else if (item.EntityPM.IsDirty == true) {
                                       debugger;
                                   }
                               });

                            }
                        }
                    }
                });
            }
        }
    }
    
    DeleteButtonClicked(item) {
        if (!AppTool.IsNullOrEmpty(item)) {
            var msg = "שורה זו תמחק, האם להמשיך?" 
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Width = 400;
            confirmWindow.Height = 150;
            confirmWindow.Show(msg);
            confirmWindow.WindowClosed.subscribe((event: any) => {

                if (confirmWindow.Yes) { // YES
                    this.ReferantExceptionItemsSource.Remove(item);
                    this.ExceptionsList = this.ExceptionsList.filter(e => e !== item.ExceptionReasonsCode);
                    this.ReferantExceptionListPM.splice(this.ReferantExceptionListPM.indexOf(item), 1);
                }
            });
        }
    }
}
export class ExceptionReason extends BaseComponent {
    public DataContext = this;
    public ObjectTableName: string = "Customs.ReferantException";
    public EntityPM: ReferantExceptionPM;
    public parent: ReferantSpotlightDataTemplate;
    _StatusItems: KeyValuePair[] = [];
    constructor(entity: ReferantExceptionPM, Parent: ReferantSpotlightDataTemplate) {
        super();
        this.parent = Parent;
        this.EntityPM = entity;
        this._StatusItems.push({ 'Key': "A", 'Value': "Active" });
        this._StatusItems.push({ 'Key': "S", 'Value': "Solved" });

    }
   
    _SelectedItemStatus: KeyValuePair;
    get SelectedItemStatus() {
        if (this.Status == "S") {
            this._SelectedItemStatus = this._StatusItems[1];
        } else {
            this._SelectedItemStatus = this._StatusItems[0];
        }
        return this._SelectedItemStatus;
    }
    set SelectedItemStatus(value) {
        if (this._SelectedItemStatus != value) {
            this._SelectedItemStatus = value;
            this.Status = this._SelectedItemStatus.Key;
        }
    }
    StatusChanged(val) {
        this.SelectedItemStatus = val;
    }
    get Status() { return this.EntityPM.Status; }
    set Status(value: string) {
        if (this.EntityPM.Status != value) {
            this.EntityPM.Status = value;

        }
    }
    get ExceptionReasonsCode() { return this.EntityPM.ExceptionReasonsCode; }
    set ExceptionReasonsCode(value: string) {
        if (this.EntityPM.ExceptionReasonsCode != value) {
            this.EntityPM.ExceptionReasonsCode = value;

        }
    }
    get ExceptionRemarks() { return this.EntityPM.ExceptionRemarks; }
    set ExceptionRemarks(value: string) {
        if (this.EntityPM.ExceptionRemarks != value) {
            this.EntityPM.ExceptionRemarks = value;

        }
    }


    SetLocalName(entity, fieldName) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        } else {
            this[fieldName] = null;
        }

    }
    exceptionReason: ExceptionReasonPM;
    get ExceptionReason() { return this.exceptionReason; }
    set ExceptionReason(value: ExceptionReasonPM) {

        if (this.ExceptionReason != value) {
            this.ExceptionReason = value;
        }
        if (!AppTool.IsNullOrEmpty(value)) {
       //     this.CourierPendingReasonName = value.LocalName;


        } else {
            this.ExceptionReasonsCode = null;
          //  this.CourierPendingReasonName = null;
        }
    }

    valid: boolean = true;

    ExceptionReasonKeyUp(event, logCellTemplate: any, ReferantReasonLovBox: any) {
        if (!AppTool.IsNullOrEmpty(event)) {
            var key = event.keyCode;
            if (key == 13) {
                this.OnExceptionReasonLostFocus(logCellTemplate, ReferantReasonLovBox);
            }
        }
    }

    OnExceptionReasonLostFocus(logCellTemplate: any, ReferantReasonLovBox: any) {
        var newValue = this.ExceptionReasonsCode;
        this.valid = true;

        if (AppTool.IsNullOrEmpty(newValue)) {
            this.UIProperties.SetValidity("ExceptionReasonsCode", "Customs.DeclarationReferantData", true, "");
        }
        else {
            this.UIProperties.SetValidity("ExceptionReasonsCode", "Customs.DeclarationReferantData", true, "");
            if (this.parent.ReferantExceptionItemsSource != null && this.parent.ReferantExceptionItemsSource.Collection.find(d => d.ExceptionReasonsCode == newValue) != null) {
                this.valid = false;
                this.UIProperties.SetValidity("ExceptionReasonsCode", "Customs.DeclarationReferantData", false, "כבר קיימת רשומה עם קוד  " + newValue);
            }
        }
        if (this.valid != true && logCellTemplate != null && ReferantReasonLovBox != null) {
            SessionLocator.SustainFocusOnCell = true;
            SessionLocator.SelectedSession.SessionEvent.emit({ FocusNow: true, OuterDivId: logCellTemplate.OuterDivId, LogTextBoxId: ReferantReasonLovBox.InputId });

        }
    }

}
