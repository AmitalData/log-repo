declare var window: any;
import { Component } from '@angular/core';
import { ConfirmWindow } from 'Controls/Windows/ConfirmWindow';
import { CourierPendingReasonPM } from 'Customs/EntityPMs/CourierPendingReasonPM';
import { DeclarationCourierStatusPM } from 'Customs/EntityPMs/DeclarationCourierStatusPM';
import { DeclarationPendingPM } from 'Customs/EntityPMs/DeclarationPendingPM';
import { DeclarationPM } from 'Customs/EntityPMs/DeclarationPM';
import { DeclarationExtendedListService } from 'Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { DeclarationCourierStatusPMService } from 'Customs/Services/StandardPMs/DeclarationCourierStatusPMService';
import { DeclarationEventManager } from 'Customs/Utilities/DeclarationEventManager';
import { KeyValuePair } from 'CustomsModules/CustomsCourier/Components/CourierWorkSheet/CourierWorksheetComponent';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { AppTool } from 'Infrastructure/Tools';

import { FeatureLocator } from 'Infrastructure/Utilities/FeatureLocator';
import { ObservableCollection } from 'Infrastructure/Utilities/ObservableCollection';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { Subscription } from 'rxjs';

@Component({
    selector: "DeclarationPendingsGeneral",
    templateUrl: './DeclarationPendingsGeneralComponent.html',
})

export class DeclarationPendingsGeneralComponent extends BaseComponent {
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


    IsVisibile: boolean;
    _IsNewPending: boolean = true;
    IsHeaderVisible: boolean = false;
    IsChanged: boolean = false;
    HasRequiresApprovalFeature:boolean=false;
    decPM:DeclarationPM;
    private CurrentSession = SessionLocator.SelectedSession;
    public CurrentEditComponentId: string;
    subscription: Subscription = null;
    constructor() {
       
        super(); 
        
        this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        this.Listen()
        this.InitTab();
        this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
       
    }
    Listen(){
          this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
             this.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                 if (this.CurrentEditComponentId == this.CurrentSession.CurrentEditComponent.ComponentId) {
                     if (tabCode == "DCCF") {
                         this.InitTab();
                     }
                 }
             })
         );

        this.subscription = DeclarationEventManager.SavePendingAfterDeclarationSaved.subscribe(data => {
            this._DeclarationCourierStatusPMService.get(this.decPM.Id).subscribe((response: ServiceResponse) => {
                if (!response.HasError) {
                    this.DeclarationCourierStatus.IsCourierMissingClassification = (response.Result as DeclarationCourierStatusPM).IsCourierMissingClassification;
                    this.DeclarationCourierStatus.CourierDeclarationStatusCode= (response.Result as DeclarationCourierStatusPM).CourierDeclarationStatusCode;
                    this.OkButtonClicked();
                }
            });
            });


            
    }
    ngOnDestroy(){
        this.subscription.unsubscribe();
    }
    
    InitTab(){

        this.DeclarationPendingItemsSource = new ObservableCollection([]);
       
        console.log("....|| DeclarationPendingsGeneralComponent ||...."); 
        if(this.CurrentSession.CurrentEditComponent?.EntityPM != null){
            this.decPM= this.CurrentSession.CurrentEditComponent?.EntityPM;
        }
        var windowArgs: any = {};
        this._DeclarationCourierStatusPMService.get(this.decPM.Id).subscribe((response: ServiceResponse) => {
            
            if (!response.HasError) {
                windowArgs.DeclarationCourierStatus = response.Result
                windowArgs.Mode = "FromDeclaration";
                windowArgs.DeclarationId = this.decPM.Id;
                windowArgs.CourierHawb = this.decPM.MAWBCourierMaster;
                this.SetWindowArgs(windowArgs);
                   
            }
        });
    }
    parent;

    SetWindowArgs(args: any) {
        
        if (FeatureLocator.HasFeaturePermession("Customs.CourierPendingReason", "PendingRequiresApproval")) {
            this.HasRequiresApprovalFeature = true;
        }

        if (!AppTool.IsNullOrEmpty(args)) {
            this.entityResourceService.getEntityResourceByTableName("Customs.DeclarationPending").subscribe(response => {

                this.IsVisibile = true;
             
                this.DeclarationCourierStatus = args.DeclarationCourierStatus;
                if (!AppTool.IsNullOrEmpty(this.DeclarationCourierStatus.DeclarationPendings)) {
                    this.DeclarationPendingsList = this.DeclarationCourierStatus.DeclarationPendings;
                }
                this.DeclarationId = args.DeclarationId;
                this.BuildDeclarationPendingList();
                this.IsDisplayOnly = args.IsDisplayOnly;
                this.parent = args.parent;
                var table = window.ObjectTables.filter(d => d.Name === 'Customs.DeclarationPending')[0];
            });

          
        }
    }
   
  

    Add() {
        if (!this.IsDisplayOnly) {

            var item: DeclarationPendingPM = new DeclarationPendingPM(this.DeclarationCourierStatus);
            
            item.DeclarationID = this.DeclarationCourierStatus.DeclarationId;
            item.Tenant = this.DeclarationCourierStatus.Tenant;
            item.IsDirty = true;
            item.Status = "A";
            this.DeclarationCourierStatus.AddDeclarationPending(item);
            //if (!this.DeclarationPendingsList.includes(item)) {
            var item1 = new DeclarationPendingLine(item, this);
            this.DeclarationPendingItemsSource.Insert(item1);
            this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty = true;
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
                    this.DeclarationCourierStatus.RemoveDeclarationPending(item.entityPM);
                    this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty = true
                }
            });
           
        }
    }

    BuildDeclarationPendingList() {
        this.DeclarationPendingItemsSource.Clear();
        for (let item of this.DeclarationPendingsList) {
            this.DeclarationPendingItemsSource.Insert(new DeclarationPendingLine(item, this));
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
        this.DeclarationCourierStatus.ApprovedCourierPendingList="";

        for (let item of this.DeclarationPendingsList) {
            if(item.WasApproved == false && item.Approval){
                this.DeclarationCourierStatus.ApprovedCourierPendingList+=","+item.CourierPendingReasonCode
            }

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
                                    this.InitTab();
                            });

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
                    SessionLocator.SelectedSession.StartBusyIndicatorSaving();
                    this._DeclarationCourierStatusPMService.update(this.DeclarationCourierStatus).subscribe((response: ServiceResponse) => {
                        this.InitTab();
                        SessionLocator.SelectedSession.StopBusyIndicator();
                    });
                }
                else {
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
        console.log("this.DeclarationPendingItemsSource.Length : " + this.DeclarationPendingItemsSource.Length);
        if (this.DeclarationPendingItemsSource != null && ($event) == this.DeclarationPendingItemsSource.Length) {
            this.Add();

        }
    }

    OnFocus() {
        if (this.DeclarationPendingItemsSource == null || this.DeclarationPendingItemsSource.Length == 0) {
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
    _StatusItems: KeyValuePair[] = [];
    private CurrentSession = SessionLocator.SelectedSession;

    constructor(EntityPM: DeclarationPendingPM, Parent: DeclarationPendingsGeneralComponent) {
        super();
        this.entityPM = EntityPM;
        this.entityPM.WasApproved=this.entityPM.Approval;
        this._StatusItems.push({ 'Key': "A", 'Value': "Active" });
        this._StatusItems.push({ 'Key': "S", 'Value': "Solved" });
        this.parent = Parent;
    }

    //#region properties


    _SelectedItemStatus: KeyValuePair;
    get SelectedItemStatus() {
        if (this.Status == "S") {
            this._SelectedItemStatus =this._StatusItems[1];
        } else {
            this._SelectedItemStatus =this._StatusItems[0];
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
            this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty = true;


        }
    }

    get CourierPendingReasonName() { return this.entityPM.CourierPendingReasonName; }
    set CourierPendingReasonName(value: string) {
        if (this.entityPM.CourierPendingReasonName != value) {
            this.entityPM.CourierPendingReasonName = value;
            this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty = true;


        }
    }

    get CourierPendingRequireApr() { return this.entityPM.CourierPendingRequireApr; }
    set CourierPendingRequireApr(value: boolean) {
        if (this.entityPM.CourierPendingRequireApr != value) {
            this.entityPM.CourierPendingRequireApr = value;
            this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty = true;

        }
    }
    
    get WasApproved(){ return this.entityPM.WasApproved}
    set WasApproved(value: boolean) {
        if (this.entityPM.WasApproved != value) {
            this.entityPM.WasApproved = value;
            this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty = true;

        }    }

    courierPendingReason: CourierPendingReasonPM;
    get CourierPendingReason() { return this.CourierPendingReason; }
    set CourierPendingReason(value: CourierPendingReasonPM) {
        if (this.CourierPendingReason != value) {
            this.CourierPendingReason = value;
            this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty = true;

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
            this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty = true;


        }
    }

    get Status() { return this.entityPM.Status; }
    set Status(value: string) {
        if (this.entityPM.Status != value) {
            this.entityPM.Status = value;
            this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty = true;

        }
    }

    get Approval() { return this.entityPM.Approval; }
    set Approval(value: boolean) {
        if (this.entityPM.Approval != value) {
            this.entityPM.Approval = value;
            this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty = true;

        }
    }

     
    //#endregion

    SetLocalName(entity, fieldName,fieldName2) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
            this[fieldName2] = entity.RequiresApproval;
        } else {
            this[fieldName] = null;
        }

    }

    itemApproved(item:any,$event){;
        this.entityPM.Approval=true;
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
