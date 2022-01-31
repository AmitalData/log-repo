declare var window: any;
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { AppTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { DeclarationCourierStatusPM } from '../../../../Customs/EntityPMs/DeclarationCourierStatusPM';
import { DeclarationCourierStatusPMService } from '../../../../Customs/Services/StandardPMs/DeclarationCourierStatusPMService';
import { DeclarationPendingPM } from '../../../../Customs/EntityPMs/DeclarationPendingPM';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { DeclarationPM } from '../../../../Customs/EntityPMs/DeclarationPM';
import { DeclarationPMService } from '../../../../Customs/Services/StandardPMs/DeclarationPMService';
import { CourierPendingReasonPM } from '../../../../Customs/EntityPMs/CourierPendingReasonPM';
import { DeclarationExtendedListService } from '../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { KeyValuePair } from '../CourierWorkSheet/CourierWorksheetComponent';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { PendingWebService } from '../../../../Customs/Services/WebServices/PendingWebService';

@Component({
    templateUrl: './DeclarationPendingsBulkFeedingComponent.html',
    selector: 'app-declaration-pendings-bulk-feeding'
})

export class DeclarationPendingsBulkFeedingComponent extends BaseComponent {
    // @Output() OkClick = new EventEmitter<DeclarationCourierStatusPM>();
    @Output() cancelClicked = new EventEmitter<DeclarationCourierStatusPM>();

    public ObjectTableName: string = "Customs.DeclarationPending";
    public DataContext = this;
    public DeclarationPendingItemsSource: ObservableCollection;
    DeclarationPendingsList: DeclarationPendingPM[] = [];
    DeclarationCourierStatus: DeclarationCourierStatusPM = new DeclarationCourierStatusPM();
    _DeclarationExtendedListService: DeclarationExtendedListService = new DeclarationExtendedListService();
    public DeclarationId: string;
    //public declarationPM: DeclarationPM;
    FIELD_IS_REQUIERD: string;
    IsDisplayOnly: boolean;
    public ValidationErrorsList: string[] = [];
    public entityResourceService: EntityResourceService = new EntityResourceService();
    private _DeclarationCourierStatusPMService: DeclarationCourierStatusPMService = new DeclarationCourierStatusPMService();

    //declarationPendingPMService: DeclarationPendingPMService = new DeclarationPendingPMService();
    //declarationPMService: DeclarationPMService = new DeclarationPMService();
    IsVisibile: boolean = false;
    _IsNewPending: boolean = true;
    IsHeaderVisible: boolean = false;
    IsChanged: boolean = false;
    notUpdateSelf: boolean = false;
    parent;
    declarationIdsList: string[] = [];
    allWithoutdeclarationIdsList: string[] = [];

    checkboxAll: boolean;
    courierMasterId: string;
    constructor(private pendingWebService: PendingWebService) {
        super();
        this.DeclarationPendingItemsSource = new ObservableCollection([]);
        this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        console.log("....|| DeclarationPendingsGeneralComponent ||....");
    }


    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.entityResourceService.getEntityResourceByTableName("Customs.DeclarationPending").subscribe(response => {
                //this.entityResourceService.getEntityResourceByTableName("Customs.DeclarationPending").subscribe(response => {


                this.IsVisibile = true;
                this.notUpdateSelf = args.notUpdateSelf;
                this.declarationIdsList = args.declarationIdsList;
                this.allWithoutdeclarationIdsList = args.allWithoutdeclarationIdsList;

                this.checkboxAll = args.checkboxAll;
                this.courierMasterId = args.courierMasterId;
                //this.DeclarationPendingsList = args.DeclarationIdList;
                this.DeclarationCourierStatus = args.DeclarationCourierStatus;
                if (!AppTool.IsNullOrEmpty(this.DeclarationCourierStatus?.DeclarationPendings)) {
                    this.DeclarationPendingsList = this.DeclarationCourierStatus.DeclarationPendings;
                }
                this.DeclarationId = args.DeclarationId;
                this.BuildDeclarationPendingList();
                this.IsDisplayOnly = args.IsDisplayOnly;
                this.parent = args.parent;
                var table = window.ObjectTables.filter(d => d.Name === 'Customs.DeclarationPending')[0];
                /*
                if (!AppTool.IsNullOrEmpty(args.DeclarationId)) {
                    this._DeclarationExtendedListService.GetDeclarationPendingListPMByDeclarationId(args.DeclarationId).subscribe((response: ServiceResponse) => {
                        if (!response.HasError) {
                            this.DeclarationPendingsList.push(response.Result);
                        }
                    });
                }
                */
            });

            //SessionLocator.SelectedSession.StartBusyIndicatorLoading();
            //this.declarationPMService.get(args.DeclarationId).subscribe((response: ServiceResponse) => {
            //    SessionLocator.SelectedSession.StopBusyIndicator();
            //    this.declarationPM = response.Result;
            //});
            //this.CourierHawb = args.CourierHawb;
            /*
            if (args.Mode == "FromDeclaration") {
                SessionLocator.SelectedSession.StartBusyIndicatorLoading();
                //this.declarationPendingPMService.get(args.DeclarationId, "").subscribe((response: ServiceResponse) => {
                //    SessionLocator.SelectedSession.StopBusyIndicator();
                  //  var declarationPendingPM: DeclarationPendingPM = response.Result;
                var declarationPendingPM: DeclarationPendingPM = this.DeclarationCourierStatus.;
                    if (declarationPendingPM != null) {
                        this.DeclarationPendingsList.push(declarationPendingPM);
                        if (!AppTool.IsNullOrEmpty(declarationPendingPM.CourierPendingReasonCode) || !AppTool.IsNullOrEmpty(declarationPendingPM.PendingRemarks)) {
                            this._IsNewPending = false;
                            //this.CourierPendingReasonCode = declarationPendingPM.CourierPendingReasonCode;
                            //this.PendingRemarks = declarationPendingPM.PendingRemarks;
                        }
                    }
                });
            }
            else {
                //if (args.Mode == "Update") {
                //   this._IsNewPending = false;
                //   this.CourierPendingReasonCode = args.CourierPendingReasonCode;
                //   this.PendingRemarks = args.PendingRemarks;
                //}
                if (args.DeclarationIdList != null) {
                    args.DeclarationIdList.forEach((declarationPendingPM: DeclarationPendingPM) => {
                        this.DeclarationPendingsList.push(declarationPendingPM);
                    });
                }
            }
            */
        }
    }
    /*
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

    private _Status: string;
    public get Status() { return this._Status; }
    public set Status(newValue: string) {
        this._Status = newValue;
    }
    */
    //#endregion

    haveDuplicates(arr: any[]): boolean { return arr.some((item, index) => arr.indexOf(item) != index) }


    async onOkClick() {
        const listPending = this.DeclarationPendingItemsSource.Collection.map(x => x.CourierPendingReasonCode)
        const listPendingRemark = this.DeclarationPendingItemsSource.Collection.map(x => x.PendingRemarks)

        if (this.haveDuplicates(listPending))
            return this.showMessage("יש שתי רשומות עם אותו קוד עיכוב");

        SessionLocator.SelectedSession.StartBusyIndicatorSaving();
        const msg: string =  await this.pendingWebService.postBulkFeeding(listPending, listPendingRemark, this.declarationIdsList, this.courierMasterId, this.checkboxAll, this.allWithoutdeclarationIdsList)
        SessionLocator.SelectedSession.StopBusyIndicator();

        this.showMessage(msg);
        SessionLocator.SelectedSession.CloseCurrentWindow();
        // this.CancelButtonClicked()
    }


    showMessage(msg: string): void {
        const myMessageWindow = new MessageWindow();
        myMessageWindow.Width = 250;
        myMessageWindow.Height = 150;
        myMessageWindow.Show(msg);
    }


    Add() {

        if (!this.IsDisplayOnly) {

            var item: DeclarationPendingPM = new DeclarationPendingPM(this.DeclarationCourierStatus);

            item.DeclarationID = ''; //this.DeclarationCourierStatus.DeclarationId;
            item.Tenant = SessionLocator.Tenant;//  this.DeclarationCourierStatus.Tenant;
            item.IsDirty = true;
            item.Status = "A";
            // this.DeclarationCourierStatus.AddDeclarationPending(item);
            //if (!this.DeclarationPendingsList.includes(item)) {
            var item1 = new DeclarationPendingLine(item, this);
            this.DeclarationPendingItemsSource.Insert(item1);
            this.IsChanged = true;
            //}
        }
    }

    DeleteButtonClicked(item) {
        if (!AppTool.IsNullOrEmpty(item)) {
            var msg = "שורה זו תמחק, האם להמשיך?" // TextCodeTranslator.Translate("Customs.Declaration.O.DeleteCondition");
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Width = 400;
            confirmWindow.Height = 150;
            confirmWindow.Show(msg);
            confirmWindow.WindowClosed.subscribe((event: any) => {

                if (confirmWindow.Yes) { // YES
                    this.DeclarationPendingItemsSource.Remove(item);
                    if (this.DeclarationPendingItemsSource.Collection.length == 0) {
                        this.IsChanged = false;
                    }
                    //     this.DeclarationCourierStatus.RemoveDeclarationPending(item.entityPM);
                }
            });
            /*
            this.parent.DeclarationPendingItemsSource.Remove(this);
            if (this.parent.DeclarationPendingsList.includes(this.entityPM)) {
                const index = this.parent.DeclarationPendingsList.indexOf(this.entityPM, 0);
                if (index > -1) {
                    this.parent.DeclarationPendingsList.splice(index, 1);
                }
            }*/
        }
    }

    BuildDeclarationPendingList() {
        this.DeclarationPendingItemsSource.Clear();
        for (let item of this.DeclarationPendingsList) {
            // this.DeclarationPendingItemsSource.Insert(new DeclarationPendingLine(item, this));
        }
    }

    CancelButtonClicked() {

        if (!this.IsDisplayOnly && this.IsChanged) {
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
                    //this.RejectChanges();
                    SessionLocator.SelectedSession.CloseCurrentWindow();
                    this.cancelClicked.emit()
                }

            });

        }
        else {
            SessionLocator.SelectedSession.CloseCurrentWindowEmit('cancel');
            this.cancelClicked.emit()
        }


    }

    isValid: boolean;
    inValid: boolean;
    hasRequest: boolean;
    notMandatoryIsNotEmpty: boolean = false;
    OkButtonClicked() {
        this.ValidationErrorsList = [];
        var errors: string[] = [];
        this.isValid = true;
        this.inValid = false;

        for (let item of this.DeclarationPendingsList) {

            if (AppTool.IsNullOrEmpty(item.CourierPendingReasonCode)) {
                errors.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.CourierPendingReasonCode")));
                this.inValid = true;
                this.isValid = false;
                break;
            }


            else {
                var existCodeList: string[] = [];
                this.DeclarationPendingItemsSource.Collection.forEach((item: DeclarationPendingLine) => {
                    if (existCodeList != null && item != null && existCodeList.indexOf(item.CourierPendingReasonCode) > -1) {
                        errors.push("כבר קיימת רשומה עם קוד עיכוב " + item.CourierPendingReasonName);
                        this.inValid = true;
                        this.isValid = false;
                    }
                    existCodeList.push(item.CourierPendingReasonCode);
                });

                /*
                if (item.PendingRemarks == null) {
                    errors.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.PendingRemarks")));
                    this.inValid = true;
                    this.isValid = false;
                    break;
                }
                */
            }
        }
        if (errors.length != 0) {
            this.ValidationErrorsList = errors;
        }
        if (this.inValid) {
            this.isValid = false;

            var confirm = new ConfirmWindow();


            confirm.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
            //confirm.ShowNoButton = true;

            confirm.WindowClosed.subscribe((event: any) => {
                if (confirm.Yes) {

                    if (errors.length == 0) {
                        var isSave = 1;

                        if (this.notUpdateSelf)
                            //this.OkClick.emit(this.DeclarationCourierStatus)
                            this.onOkClick();
                        else if (isSave == 1) {
                            SessionLocator.SelectedSession.StartBusyIndicatorSaving();
                            this._DeclarationCourierStatusPMService.update(this.DeclarationCourierStatus).subscribe((response: ServiceResponse) => {
                                //this.DeclarationPendingsList.forEach((declarationPendingPM: DeclarationPendingPM) => {
                                //declarationPendingPM.CourierPendingReasonCode = this.CourierPendingReasonCode;
                                //declarationPendingPM.PendingRemarks = this.PendingRemarks;
                                //declarationPendingPM.Status = this.Status;
                                //declarationPendingPM.IsDirty = true;
                                //declarationPendingPM.DeclarationID = this.declarationPM.Id;
                                //declarationPendingPM.Tenant = this.declarationPM.Tenant;
                                //this.declarationPendingPMService.update(declarationPendingPM).subscribe((response: ServiceResponse) => {
                                SessionLocator.SelectedSession.StopBusyIndicator();
                                SessionLocator.SelectedSession.CloseCurrentWindow();
                            });

                        }
                        else {
                            SessionLocator.SelectedSession.CloseCurrentWindow();
                        }
                    }

                }
                else {
                    this.ValidationErrorsList = errors;
                }

                confirm.Close();
            });

        }

        else {

            if (errors.length == 0) {

                var isSave = 1;

                if (this.notUpdateSelf)
                    // this.OkClick.emit(this.DeclarationCourierStatus)
                    this.onOkClick();
                else if (isSave == 1) {
                    SessionLocator.SelectedSession.StartBusyIndicatorSaving();
                    this._DeclarationCourierStatusPMService.update(this.DeclarationCourierStatus).subscribe((response: ServiceResponse) => {
                        SessionLocator.SelectedSession.StopBusyIndicator();
                        SessionLocator.SelectedSession.CloseCurrentWindow();
                    });
                }
                else {
                    SessionLocator.SelectedSession.CloseCurrentWindow();
                }
            }

            else {
                this.ValidationErrorsList = errors;
            }
        }

        return this.isValid;

    }

    public SelectedRow: any = null;
    OnRowSelected(itemComponent: any) {
        this.SelectedRow = itemComponent;
        // this.IsChanged = true;
    }

    OnRowEnded($event) {
        console.log("this.DeclarationPendingItemsSource.Length : " + this.DeclarationPendingItemsSource.Length);
        if (this.DeclarationPendingItemsSource != null && ($event) == this.DeclarationPendingItemsSource.Length) {
            this.Add();

        }
    }

    OnFocus() {
        //if (this.DeclarationPendingItemsSource == null || this.DeclarationPendingItemsSource.Length == 0) {
        //    this.Add();
        //}
    }

}
export class DeclarationPendingLine extends BaseComponent {
    public DataContext = this;
    public ObjectTableName: string = "Customs.DeclarationPending";
    public entityPM: DeclarationPendingPM;
    public declaration: DeclarationPM;
    public parent: DeclarationPendingsBulkFeedingComponent;
    _StatusItems: KeyValuePair[] = [];
    constructor(EntityPM: DeclarationPendingPM, Parent: DeclarationPendingsBulkFeedingComponent) {
        super();
        this.entityPM = EntityPM;

        this._StatusItems.push({ 'Key': "A", 'Value': "Active" });
        this._StatusItems.push({ 'Key': "S", 'Value': "Solved" });
        this.parent = Parent;
    }

    //#region properties


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
    get CourierPendingReasonCode() { return this.entityPM.CourierPendingReasonCode; }
    set CourierPendingReasonCode(value: string) {
        if (this.entityPM.CourierPendingReasonCode != value) {
            this.entityPM.CourierPendingReasonCode = value;

        }
    }

    get CourierPendingReasonName() { return this.entityPM.CourierPendingReasonName; }
    set CourierPendingReasonName(value: string) {
        DeclarationExtendedListService
        if (this.entityPM.CourierPendingReasonName != value) {
            this.entityPM.CourierPendingReasonName = value;

        }
    }

    courierPendingReason: CourierPendingReasonPM;
    get CourierPendingReason() { return this.CourierPendingReason; }
    set CourierPendingReason(value: CourierPendingReasonPM) {

        if (this.CourierPendingReason != value) {
            this.CourierPendingReason = value;
        }
        if (!AppTool.IsNullOrEmpty(value)) {
            this.CourierPendingReasonName = value.LocalName;


        } else {
            this.CourierPendingReasonCode = null;
            this.CourierPendingReasonName = null;
        }
    }

    get PendingRemarks() { return this.entityPM.PendingRemarks; }
    set PendingRemarks(value: string) {
        if (this.entityPM.PendingRemarks != value) {
            this.entityPM.PendingRemarks = value;

        }
    }

    get Status() { return this.entityPM.Status; }
    set Status(value: string) {
        if (this.entityPM.Status != value) {
            this.entityPM.Status = value;

        }
    }

    //#endregion

    SetLocalName(entity, fieldName) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        } else {
            this[fieldName] = null;
        }

    }




    valid: boolean = true;

    CourierPendingReasonKeyUp(event, logCellTemplate: any, CourierPendingReasonLovBox: any) {
        if (!AppTool.IsNullOrEmpty(event)) {
            var key = event.keyCode;
            if (key == 13) {
                this.OnCourierPendingReasonLostFocus(logCellTemplate, CourierPendingReasonLovBox);
            }
        }
    }

    OnCourierPendingReasonLostFocus(logCellTemplate: any, CourierPendingReasonLovBox: any) {
        var newValue = this.CourierPendingReasonCode;
        this.valid = true;

        if (AppTool.IsNullOrEmpty(newValue)) {
            this.UIProperties.SetValidity("CourierPendingReasonCode", "Customs.DeclarationPending", true, "");
        }
        else {
            this.UIProperties.SetValidity("CourierPendingReasonCode", "Customs.DeclarationPending", true, "");
            if (this.parent.DeclarationPendingItemsSource != null && this.parent.DeclarationPendingItemsSource.Collection.find(d => d.CourierPendingReasonCode == newValue) != null) {
                this.valid = false;
                this.UIProperties.SetValidity("CourierPendingReasonCode", "Customs.DeclarationPending", false, "כבר קיימת רשומה עם קוד עיכוב " + newValue);
            }
        }
        if (this.valid != true && logCellTemplate != null && CourierPendingReasonLovBox != null) {
            SessionLocator.SustainFocusOnCell = true;
            SessionLocator.SelectedSession.SessionEvent.emit({ FocusNow: true, OuterDivId: logCellTemplate.OuterDivId, LogTextBoxId: CourierPendingReasonLovBox.InputId });

        }
    }
}
