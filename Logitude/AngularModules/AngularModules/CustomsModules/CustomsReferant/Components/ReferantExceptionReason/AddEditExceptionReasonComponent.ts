import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { Component } from '@angular/core';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { DeclarationReferantDataPMService } from '../../../../Customs/Services/StandardPMs/DeclarationReferantDataPMService';
import { ReferantExceptionPMService } from '../../../../Customs/Services/StandardPMs/ReferantExceptionPMService';
import { ReferantExceptionExtendedPMService } from '../../../../Customs/Services/ExtendedPMs/ReferantExceptionExtendedPMService';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { AppTool } from '../../../../Infrastructure/Tools';
import { ReferantExceptionPM } from '../../../../Customs/EntityPMs/ReferantExceptionPM';
import { SpotlightSharedDataService } from '../../../../Customs/Services/DataChange/SpotlightSharedDataService';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { KeyValuePair } from '../../../CustomsCourier/Components/CourierWorkSheet/CourierWorksheetComponent';
import { ExceptionReasonExtendedListService } from '../../../../Customs/Services/ExtendedLists/ExceptionReasonExtendedListService';
import { ExceptionReasonPM } from '../../../../Customs/EntityPMs/ExceptionReasonPM';


@Component({
    templateUrl: './AddEditExceptionReasonComponent.html',
})

export class AddEditExceptionReasonComponent extends BaseComponent {
    public ObjectTableName: string = "Customs.DeclarationReferantData";
    public DataContext = this;
    public ReferantExceptionItemsSource: ObservableCollection;
    IsDisplayOnly: boolean;
    DeletedCodeList: string[] = [];
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    private _declarationReferantDataPMService: DeclarationReferantDataPMService = new DeclarationReferantDataPMService();
    private _referantExceptionPMService: ReferantExceptionPMService = new ReferantExceptionPMService();
    private _referantExceptionExtendedPMService: ReferantExceptionExtendedPMService = new ReferantExceptionExtendedPMService();
    public isActiveFilterItems: ApiQueryFilters;
    FIELD_IS_REQUIERD: string;
    _IsReady: boolean = false;
    private ReferantExceptionListPM: ReferantExceptionPM[] = [];
    public exceptionReasonSharedDataService = new SpotlightSharedDataService();
    private ExceptionsList: string[] = [];
    errors: string[] = [];
    existCodeList: string[] = [];
    IsChanged: boolean = false;
    rowIndex: any;

    constructor(private entityResourceService: EntityResourceService) {
        super();
        this.entityResourceService.getEntityResourceByTableName("Customs.DeclarationReferantData").subscribe(response => {
            this.entityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(response => {
                this._IsReady = true;
            });
        });
        this.ReferantExceptionItemsSource = new ObservableCollection([]);
        this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        this.isActiveFilterItems = new ApiQueryFilters();
        this.isActiveFilterItems.addAdditionalFilter("IsActive", true, null, null, "Equals", false, false, false, "boolean");
    }

    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.EntityPM = args.EntityPM;
            this.LoadReferantException();
            this.exceptionReasonSharedDataService.IsDirty = false;
            this.rowIndex = args.rowIndex;
        }
    }

    public get ExceptionReasonsList() { return this.EntityPM.ExceptionReasonsList; }
    public set ExceptionReasonsList(newValue: string) { this.EntityPM.ExceptionReasonsList = newValue; }

    private LoadReferantException() {
        this.SplitExceptionList(this.EntityPM.ExceptionReasonsList);
        this.getData(this.ExceptionsList);
        this.DeletedCodeList = [];
        this._IsReady = true;
    }

    getData(ExceptionsList: string[]) {
        this._referantExceptionExtendedPMService.GetByDecId(this.EntityPM.DeclarationId)
            .subscribe((response: any) => {
                for (let item of response.Result) {
                    this.ReferantExceptionListPM.push(item);
                    this.ReferantExceptionItemsSource.Insert(new ExceptionReason(item, this, this.exceptionReasonSharedDataService));
                }
            });

    }

    private SplitExceptionList(exception: string) {
        if (exception != null) {
            this.ExceptionsList = exception.split(',');
            this.ExceptionsList.shift();
        }
    }

    public get FollowUpDate() { return this.EntityPM.FollowUpDate; }
    public set FollowUpDate(newValue: Date) {
        if (newValue != null) {
            newValue.setUTCHours(6);
            this.EntityPM.FollowUpDate = newValue;
        } else {
            this.EntityPM.FollowUpDate = newValue;
        }
        this.exceptionReasonSharedDataService.IsDirty = true;
    }

    Add() {
        var item: ReferantExceptionPM = new ReferantExceptionPM();
        item.DeclarationId = this.EntityPM.DeclarationId;
        item.Tenant = this.EntityPM.Tenant;
        item.Status = "A";
        this.ReferantExceptionListPM.includes(item);
        var _exceptionReason = new ExceptionReason(item, this.EntityPM, this.exceptionReasonSharedDataService);
        _exceptionReason.IsNew = true;
        _exceptionReason.ShowCode = false;
        this.ReferantExceptionItemsSource.Insert(_exceptionReason);
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
                        this.exceptionReasonSharedDataService.IsDirty = true;
                    }
                }
            });
        }
    }

    private BuildExceptionReasonsList() {
        var newValue: string = "";
        this.ReferantExceptionItemsSource.Collection.forEach((item: ExceptionReason) => {
            if (item.EntityPM.Status == "A") {
                newValue = newValue + "," + item.EntityPM.ExceptionReasonsCode;
            }
        });
        this.EntityPM.ExceptionReasonsList = newValue;
    }

    CancelButtonClicked() {
        if (this.exceptionReasonSharedDataService.IsDirty) {
            var confirm = new ConfirmWindow();
            confirm.YesButtonText = TextCodeTranslator.Translate("General.B.Yes");
            confirm.ShowNoButton = true;
            confirm.Show(TextCodeTranslator.Translate("Customs.General.O.CancelMessage"));
            confirm.WindowClosed.subscribe((event: any) => {
                if (confirm.Yes) {
                    confirm.Close();
                    this.OkButtonClicked();

                }
                else {
                    SessionLocator.SelectedSession.CloseCurrentWindow();
                }

            });
        }
        else {
            SessionLocator.SelectedSession.CloseCurrentWindowEmit('cancel');
        }
    }

    CheckForDuplicate() {
        this.ReferantExceptionItemsSource.Collection.forEach((item: ExceptionReason) => {
            if (AppTool.IsNullOrEmpty(item.ExceptionReasonsCode)) {
                this.errors.push(this.FIELD_IS_REQUIERD.replace("%FieldName", " קוד חריג הוא"));
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
        if (this.exceptionReasonSharedDataService.IsDirty) {
            this.ValidationErrorsList = [];
            this.errors = [];
            this.existCodeList = [];
            this.CheckForDuplicate()
            if (this.errors.length != 0) {
                this.ValidationErrorsList = this.errors;
            }
            if (this.errors.length == 0) {
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
                                this.IsLastItemInCollection(item);
                            });
                        } else if (item.EntityPM.IsDirty == true) {
                            this._referantExceptionPMService.update(item.EntityPM).subscribe((response: any) => {
                                this.IsLastItemInCollection(item);
                            });
                        } else {
                            this.IsLastItemInCollection(item);
                        }
                    });
                    if (this.ReferantExceptionItemsSource.Collection.length == 0) {
                        SessionLocator.SelectedSession.CloseCurrentWindow();
                    }
                });
            }
            this.exceptionReasonSharedDataService.IsDirty = false;
        } else {
            SessionLocator.SelectedSession.CloseCurrentWindow();

        }
    }

    public SelectedRow: any = null;
    OnRowSelected(itemComponent: any) {
        this.SelectedRow = itemComponent;
        this.IsChanged = true;
    }

    IsLastItemInCollection(item: any) {
        var lastItem = this.ReferantExceptionItemsSource.Collection[this.ReferantExceptionItemsSource.Collection.length-1]
        if (item == lastItem) {
            SessionLocator.SelectedSession.CloseCurrentWindow();
        }
    }

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

}
export class ExceptionReason extends BaseComponent {
    public DataContext = this;
    public ObjectTableName: string = "Customs.ReferantException";
    public EntityPM: ReferantExceptionPM;
    public IsNew: boolean = false;
    public parent: AddEditExceptionReasonComponent;
    public ShowCode: boolean = true;
    _StatusItems: KeyValuePair[] = [];
    exceptionReasonExtendedListService: ExceptionReasonExtendedListService = new ExceptionReasonExtendedListService();
    constructor(entity: ReferantExceptionPM, Parent: AddEditExceptionReasonComponent, public spotlightSharedDataService: SpotlightSharedDataService) {
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
        if (this.EntityPM.ExceptionReasonsCode != null) {
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

    reasonName: any;
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

