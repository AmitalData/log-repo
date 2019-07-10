declare var window: any;

import { Component, AfterViewInit, ChangeDetectorRef, ViewChildren, QueryList } from '@angular/core';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { LocationDirective } from '../../../../Infrastructure/Utilities/LocationDirective';
import { ApiQueryFilters, FilterItem } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { AppTool, ArrayTool } from '../../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { CustomSendOptionsArgs, SendRequestVIA } from '../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { DeclarationCourierStatusPM } from '../../../../Customs/EntityPMs/DeclarationCourierStatusPM';
import { DeclarationCourierStatusPMService } from '../../../../Customs/Services/StandardPMs/DeclarationCourierStatusPMService';
import { AmitalGatewayUtil, UnifreightMessageM } from '../../../../Infrastructure/Utilities/AmitalGatewayUtil';
import { DeclarationPendingPM } from '../../../../Customs/EntityPMs/DeclarationPendingPM';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { DeclarationPendingPMService } from '../../../../Customs/Services/StandardPMs/DeclarationPendingPMService';
import { DeclarationPM } from '../../../../Customs/EntityPMs/DeclarationPM';
import { DeclarationPMService } from '../../../../Customs/Services/StandardPMs/DeclarationPMService';
import { CourierPendingReasonPM } from '../../../../Customs/EntityPMs/CourierPendingReasonPM';

@Component({
    moduleId: module.id,
    templateUrl: './DeclarationPendingsGeneralComponent.html',
})



export class DeclarationPendingsGeneralComponent extends BaseComponent {
    public ObjectTableName: string = "Customs.DeclarationPending";
    public DataContext = this;
    public DeclarationPendingItemsSource: ObservableCollection;
    DeclarationPendingsList: DeclarationPendingPM[] = [];
    public declarationPM: DeclarationPM;
    FIELD_IS_REQUIERD: string;
    IsDisplayOnly: boolean;
    public ValidationErrorsList: string[] = [];
    public entityResourceService: EntityResourceService = new EntityResourceService();

    declarationPendingPMService: DeclarationPendingPMService = new DeclarationPendingPMService();
    declarationPMService: DeclarationPMService = new DeclarationPMService();
    IsVisibile: boolean;
    _IsNewPending: boolean = true;
    IsHeaderVisible: boolean = false;
    IsChanged: boolean = false;

    constructor() {
        super();
        this.DeclarationPendingItemsSource = new ObservableCollection([]);
        this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        console.log("....|| DeclarationPendingsGeneralComponent ||....");
    }

    parent;

    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.entityResourceService.getEntityResourceByTableName("Customs.DeclarationPending").subscribe(response => {
                //this.entityResourceService.getEntityResourceByTableName("Customs.DeclarationPending").subscribe(response => {


                this.IsVisibile = true;
                this.DeclarationPendingsList = args.DeclarationIdList;
                this.BuildDeclarationPendingList();
                this.IsDisplayOnly = args.IsDisplayOnly;
                this.parent = args.parent;
                var table = window.ObjectTables.filter(d => d.Name === 'Customs.DeclarationPending')[0];


                // });
            });

            SessionLocator.CurrentSession.StartBusyIndicatorLoading();
            this.declarationPMService.get(args.DeclarationId).subscribe((response: ServiceResponse) => {
                SessionLocator.CurrentSession.StopBusyIndicator();
                this.declarationPM = response.Result;
            });
            //this.CourierHawb = args.CourierHawb;
            if (args.Mode == "FromDeclaration") {
                SessionLocator.CurrentSession.StartBusyIndicatorLoading();
                this.declarationPendingPMService.get(args.DeclarationId, "").subscribe((response: ServiceResponse) => {
                    SessionLocator.CurrentSession.StopBusyIndicator();
                    var declarationPendingPM: DeclarationPendingPM = response.Result;
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

    Add() {
        if (!this.IsDisplayOnly) {

            var item: DeclarationPendingPM = new DeclarationPendingPM();

            item.DeclarationID = this.declarationPM.Id;
            item.Tenant = this.declarationPM.Tenant;
            item.IsDirty = true;
            item.Status = "A";

            if (!this.DeclarationPendingsList.includes(item)) {
                this.DeclarationPendingItemsSource.Insert(new DeclarationPendingLine(item, this));
            }
        }
    }

    BuildDeclarationPendingList() {
        this.DeclarationPendingItemsSource.Clear();
        for (let item of this.DeclarationPendingsList) {
            this.DeclarationPendingItemsSource.Insert(new DeclarationPendingLine(item, this));
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
                    SessionLocator.CurrentSession.CloseCurrentWindow();
                }

            });

        }
        else {
            SessionLocator.CurrentSession.CloseCurrentWindowEmit('cancel');
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

                if (item.PendingRemarks == null) {
                    errors.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.PendingRemarks")));
                    this.inValid = true;
                    this.isValid = false;
                    break;
                }

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
                        if (isSave == 1) {
                            SessionLocator.CurrentSession.StartBusyIndicatorSaving();
                            this.DeclarationPendingsList.forEach((declarationPendingPM: DeclarationPendingPM) => {
                                //declarationPendingPM.CourierPendingReasonCode = this.CourierPendingReasonCode;
                                //declarationPendingPM.PendingRemarks = this.PendingRemarks;
                                //declarationPendingPM.Status = this.Status;
                                //declarationPendingPM.IsDirty = true;
                                //declarationPendingPM.DeclarationID = this.declarationPM.Id;
                                //declarationPendingPM.Tenant = this.declarationPM.Tenant;
                                this.declarationPendingPMService.update(declarationPendingPM).subscribe((response: ServiceResponse) => {
                                    SessionLocator.CurrentSession.StopBusyIndicator();
                                    SessionLocator.CurrentSession.CloseCurrentWindow();
                                });
                            });

                        }
                        else {
                            SessionLocator.CurrentSession.CloseCurrentWindow();
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
                if (isSave == 1) {
                    SessionLocator.CurrentSession.StartBusyIndicatorSaving();
                    this.DeclarationPendingsList.forEach((declarationPendingPM: DeclarationPendingPM) => {
                        //declarationPendingPM.CourierPendingReasonCode = this.CourierPendingReasonCode;
                        //declarationPendingPM.PendingRemarks = this.PendingRemarks;
                        //declarationPendingPM.Status = this.Status;
                        //declarationPendingPM.IsDirty = true;
                        //declarationPendingPM.DeclarationID = this.declarationPM.Id;
                        //declarationPendingPM.Tenant = this.declarationPM.Tenant;
                        this.declarationPendingPMService.update(declarationPendingPM).subscribe((response: ServiceResponse) => {
                            SessionLocator.CurrentSession.StopBusyIndicator();
                            SessionLocator.CurrentSession.CloseCurrentWindow();
                        });
                    });
                }
                else {
                    SessionLocator.CurrentSession.CloseCurrentWindow();
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
        this.IsChanged = true;
    }

    OnRowEnded($event) {
        console.log("this.ItemsSource.Length : " + this.DeclarationPendingItemsSource.Length);
        if (($event) == this.DeclarationPendingItemsSource.Length) {
            this.Add();

        }
    }

    OnFocus() {
        if (this.DeclarationPendingItemsSource.Length == 0) {
            this.Add();
        }
    }

}
export class DeclarationPendingLine extends BaseComponent {
    public DataContext = this;
    public ObjectTableName: string = "Customs.DeclarationPending";
    public entityPM: DeclarationPendingPM;
    public declaration: DeclarationPM;
    public parent: DeclarationPendingsGeneralComponent;
    constructor(EntityPM: DeclarationPendingPM, Parent: DeclarationPendingsGeneralComponent) {
        super();
        this.entityPM = EntityPM;

        this.parent = Parent;

    }

    //#region properties



    get CourierPendingReasonCode() { return this.entityPM.CourierPendingReasonCode; }
    set CourierPendingReasonCode(value: string) {
        if (this.entityPM.CourierPendingReasonCode != value) {
            this.entityPM.CourierPendingReasonCode = value;

        }
    }

    get CourierPendingReasonName() { return this.entityPM.CourierPendingReasonName; }
    set CourierPendingReasonName(value: string) {
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
    
    //#endregion

    SetLocalName(entity, fieldName) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        } else {
            this[fieldName] = null;
        }

    }


    DeleteButtonClicked() {

        this.parent.DeclarationPendingItemsSource.Remove(this);
        if (this.parent.DeclarationPendingsList.includes(this.entityPM)) {
            const index = this.parent.DeclarationPendingsList.indexOf(this.entityPM, 0);
            if (index > -1) {
                this.parent.DeclarationPendingsList.splice(index, 1);
            }
        }
    }

    valid: boolean = true;

    CourierPendingReasonKeyUp(event, logCellTemplate: any, CourierPendingReasonLovBox: any) {
        var key = event.keyCode;
        if (key == 13) {
            this.OnCourierPendingReasonLostFocus(logCellTemplate, CourierPendingReasonLovBox);
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
            if (this.parent.DeclarationPendingsList.find(d => d.CourierPendingReasonCode == newValue) != null) {
                this.valid = false;
                this.UIProperties.SetValidity("CourierPendingReasonCode", "Customs.DeclarationPending", false, "כבר קיימת רשומה עם קוד עיכוב " + newValue);
            }
        }
        if (this.valid != true) {
            SessionLocator.SustainFocusOnCell = true;
            SessionLocator.CurrentSession.SessionEvent.emit({ FocusNow: true, OuterDivId: logCellTemplate.OuterDivId, LogTextBoxId: CourierPendingReasonLovBox.InputId });

        }
    }
}
