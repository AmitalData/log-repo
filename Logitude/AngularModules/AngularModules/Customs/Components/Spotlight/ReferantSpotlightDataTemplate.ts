import { Component, AfterViewInit, ChangeDetectorRef, ViewContainerRef, OnChanges, SimpleChanges } from '@angular/core';
import { AppTool, ArrayTool } from '../../../Infrastructure/Tools';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { PhysicalCheckPMService } from '../../Services/StandardPMs/PhysicalCheckPMService'
import { DeclarationReferantDataPMService } from '../../Services/StandardPMs/DeclarationReferantDataPMService';
import { DeclarationReferantDataPM } from '../../EntityPMs/DeclarationRefernatDataPM';
import { BaseRequestsSheetMassaging } from '../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ReferantExceptionPM } from '../../EntityPMs/ReferantExceptionPM';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { ReferantExceptionPMService } from '../../Services/StandardPMs/ReferantExceptionPMService';
import { EntityResourceService } from "../../../Infrastructure/Services/EntityResourceService";
import { ExceptionReasonPM } from '../../EntityPMs/ExceptionReasonPM';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { KeyValuePair } from '../../../CustomsModules/CustomsCourier/Components/CourierWorkSheet/CourierWorksheetComponent';
import { ReferantExceptionExtendedPMService } from '../../Services/ExtendedPMs/ReferantExceptionExtendedPMService';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { SpotlightSharedDataService } from '../../../Customs/Services/DataChange/SpotlightSharedDataService';
import { ExceptionReasonPMService } from '../../Services/StandardPMs/ExceptionReasonPMService';
import { ExceptionReasonExtendedListService } from '../../Services/ExtendedLists/ExceptionReasonExtendedListService';

@Component({
    templateUrl: './ReferantSpotlightDataTemplate.html',
})


export class ReferantSpotlightDataTemplate
    extends BaseComponent
    implements AfterViewInit{
    RowIndex: any;
    ngAfterViewInit(): void { 
        this.ShowBusyIndicator = false;
    }
    public DataContext: any = this;
    IsDisplayOnly: boolean;
    DeletedCodeList: string[] = [];
    public ReferantExceptionItemsSource: ObservableCollection;
    public ObjectTableName: string = "Customs.DeclarationReferantData";
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    private _declarationReferantDataPMService: DeclarationReferantDataPMService = new DeclarationReferantDataPMService();
    private _referantExceptionPMService: ReferantExceptionPMService = new ReferantExceptionPMService();
    private _referantExceptionExtendedPMService: ReferantExceptionExtendedPMService = new ReferantExceptionExtendedPMService();


    public spotlightSharedDataService = new SpotlightSharedDataService();
    _IsReady: boolean = false;
    constructor(private EntityResourceService: EntityResourceService) {
        super();
        this.EntityResourceService.getEntityResourceByTableName("Customs.DeclarationReferantData").subscribe((response:any) => {
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
            this.spotlightSharedDataService.IsDirty = false;
        }
    }
    spotLightViewContainerRef: ViewContainerRef;

    Run(entity: DeclarationReferantDataPM, SpotLightViewContainerRef: ViewContainerRef, RowIndex) {

        this.EntityPM = entity;

        this.RowIndex = RowIndex;

        this.spotLightViewContainerRef = SpotLightViewContainerRef;
        this.LoadReferantException();
        this.IsDisplayOnly = false;
        this.spotlightSharedDataService.IsDirty = false;

    }
    private LoadReferantException() {
        this.ShowBusyIndicator = true;
        this.SplitExceptionList(this.EntityPM.ExceptionReasonsList);
        this.getData(this.ExceptionsList);
        this.ShowBusyIndicator = false;
        this.DeletedCodeList = [];  // list of items need to be deleted
        this._IsReady = true;
    }

    private ReferantExceptionListPM : ReferantExceptionPM[] = [];
    getData(ExceptionsList: string[]) {
            this._referantExceptionExtendedPMService.GetByDecId(this.EntityPM.DeclarationId)
                .subscribe((response: any) => {
                    for (let item of response.Result) {
                        this.ReferantExceptionListPM.push(item);
                        this.ReferantExceptionItemsSource.Insert(new ExceptionReason(item, this, this.spotlightSharedDataService));

                    }
                });
        
    }
    Add() {
        var item: ReferantExceptionPM = new ReferantExceptionPM();
        item.DeclarationId = this.EntityPM.DeclarationId;
        item.Tenant = this.EntityPM.Tenant;
        item.Status = "A";
        this.ReferantExceptionListPM.includes(item);
        var _exceptionReason = new ExceptionReason(item, this.EntityPM, this.spotlightSharedDataService);
        _exceptionReason.IsNew = true;
        _exceptionReason.ShowCode = false;
        this.ReferantExceptionItemsSource.Insert(_exceptionReason);


    }

    private ExceptionsList: string[] = [];
    private SplitExceptionList(exception: string) {
        if (exception != null) {
            this.ExceptionsList = exception.split(',');
            this.ExceptionsList.shift();
        } 
    }

    private BuildExceptionReasonsList() {
        var newValue: string="";
        this.ReferantExceptionItemsSource.Collection.forEach((item: ExceptionReason) => {
            if (item.EntityPM.Status == "A") {
                newValue = newValue + "," + item.EntityPM.ExceptionReasonsCode;
            }
        });
        this.EntityPM.ExceptionReasonsList = newValue;
    }

    public get FollowUpDate() { return this.EntityPM.FollowUpDate; }
    public set FollowUpDate(newValue: Date) {
        if (newValue != null) {
            newValue.setUTCHours(6);
            this.EntityPM.FollowUpDate = newValue;
        } else {
            this.EntityPM.FollowUpDate = newValue;
        }
        this.spotlightSharedDataService.IsDirty = true;
    }

    public get ExceptionReasonsList() { return this.EntityPM.ExceptionReasonsList; }
    public set ExceptionReasonsList(newValue: string) { this.EntityPM.ExceptionReasonsList = newValue; }

    OnRowEnded($event) {
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

    FIELD_IS_REQUIERD: string;
    existCodeList: string[] = [];
    errors: string[] = [];
    CheckForDuplicate() {
        this.ReferantExceptionItemsSource.Collection.forEach((item: ExceptionReason) => {
            if (AppTool.IsNullOrEmpty(item.ExceptionReasonsCode)) {
                this.errors.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("Customs.DeclarationReferantData.O.ExceptionReasonsCode")));
            } else {
                if (this.existCodeList.indexOf(item.ExceptionReasonsCode) >= 0) {
                    this.errors.push("כבר קיימת רשומה עם קוד חריג  " + item.ExceptionReasonsCode);
                    this.ReferantExceptionItemsSource.Remove(item);
                    this.ExceptionsList = this.ExceptionsList.filter(e => e !== item.ExceptionReasonsCode);
                } else {
                    this.existCodeList.push(item.ExceptionReasonsCode);
                }
            }
        });
    }

    OkButtonClicked() {
        if (this.spotlightSharedDataService.IsDirty) { 
            this.ValidationErrorsList = [];
            this.errors = [];
            this.existCodeList = [];
            this.CheckForDuplicate()
            if (this.errors.length != 0) {
                this.ValidationErrorsList = this.errors;
            }
            if (this.errors.length == 0) {
                this.ShowBusyIndicator = true;
                this.BuildExceptionReasonsList();
                this.DeletedCodeList.forEach((item: string) => {
                    this._referantExceptionExtendedPMService.Delete(this.EntityPM.DeclarationId, item).subscribe((response: any) => {
                        this.DeletedCodeList.splice(this.DeletedCodeList.indexOf(item), 1);
                    });
                });
                this._declarationReferantDataPMService.update(this.EntityPM).subscribe((response: any) => {
                    this.ReferantExceptionItemsSource.Collection.forEach((item: ExceptionReason) => {
                        if (item.IsNew == true) {
                            this._referantExceptionPMService.insert(item.EntityPM).subscribe((response: any) => {
                                item.IsNew = false;
                                item.ShowCode = true;
                            });
                        } else if (item.EntityPM.IsDirty == true) {
                            this._referantExceptionPMService.update(item.EntityPM).subscribe();
                        }
                    });
                    SessionLocator.SelectedSession.CurrentListComponent.OnBackFromEdit(this.EntityPM.DeclarationId, { rowIndex: this.RowIndex });
                    //SessionLocator.SelectedSession.CurrentListComponent.DoRefresh();
                });
            }
            this.spotlightSharedDataService.IsDirty = false;
            this.ShowBusyIndicator = false;

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
                    if (!this.ExceptionsList.includes(item.ExceptionReasonsCode)) {
                        this.DeletedCodeList.push(item.ExceptionReasonsCode);
                        this.spotlightSharedDataService.IsDirty = true;
                    }
                }
            });
        }
    }

}
export class ExceptionReason extends BaseComponent {
    public DataContext = this;
    public ObjectTableName: string = "Customs.ReferantException";
    public EntityPM: ReferantExceptionPM;
    public IsNew: boolean=false;
    public parent: ReferantSpotlightDataTemplate;
    public ShowCode: boolean = true;
    _StatusItems: KeyValuePair[] = [];
    exceptionReasonExtendedListService: ExceptionReasonExtendedListService = new ExceptionReasonExtendedListService();
    constructor(entity: ReferantExceptionPM, Parent: ReferantSpotlightDataTemplate,public spotlightSharedDataService: SpotlightSharedDataService) {
        super();
        this.parent = Parent;
        this.EntityPM = entity;
        this._StatusItems.push({ 'Key': "A", 'Value': "Active" });
        this._StatusItems.push({ 'Key': "S", 'Value': "Solved" });
        if (this.EntityPM.ExceptionReasonsCode == null) {
            this.ShowCode = false;
        }
        if (this.EntityPM.ExceptionReasonsCode != null) {
            this.exceptionReasonExtendedListService.get(this.EntityPM.ExceptionReasonsCode).subscribe(response => {
                this.ExceptionReason = response.Result;
            });
        }
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
            this.spotlightSharedDataService.IsDirty = true;

        }

    }
    get ExceptionReasonsCode() { return this.EntityPM.ExceptionReasonsCode; }
    set ExceptionReasonsCode(value: string) {
        if (this.EntityPM.ExceptionReasonsCode != null ) {
        }
        if (this.EntityPM.ExceptionReasonsCode != value) {
            this.EntityPM.ExceptionReasonsCode = value;
            this.spotlightSharedDataService.IsDirty = true;

        }

    }

    get ExceptionRemarks() { return this.EntityPM.ExceptionRemarks; }
    set ExceptionRemarks(value: string) {
        if (this.EntityPM.ExceptionRemarks != value) {
            this.EntityPM.ExceptionRemarks = value;
            this.spotlightSharedDataService.IsDirty = true;

        }

    }

    reasonName:any;
    get ExceptionReasonName() {
        return this.reasonName;
    }
    set ExceptionReasonName(value: string) {
        if (this.reasonName != value) {
            this.reasonName = value;
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
        if (this.exceptionReason != value) {
            this.exceptionReason = value;
        }
        if (!AppTool.IsNullOrEmpty(value)) {
            this.reasonName = value.LocalName;
        } else {
            this.ExceptionReasonsCode = null;
            this.reasonName = null;
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
