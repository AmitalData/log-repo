import { Component } from '@angular/core';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { AppTool, ArrayTool } from '../../../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { LogTab } from '../../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ClaimPM } from '../../../../../Customs/EntityPMs/ClaimPM';
import { ClaimImporterDeclarsPage3PM } from '../../../../../Customs/EntityPMs/ClaimImporterDeclarsPage3PM';
import { ClaimImporterDeclarsPage3APM } from '../../../../../Customs/EntityPMs/ClaimImporterDeclarsPage3APM';
import { ClaimImporterDeclarsP3LoiPM } from '../../../../../Customs/EntityPMs/ClaimImporterDeclarsP3LoiPM';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { MessageWindow } from '../../../../../Controls/Windows/MessageWindow';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';


@Component({
    moduleId: module.id,
    templateUrl: './ClaimImporterDeclATabComponent.html',
})

export class ClaimImporterDeclATabComponent extends BaseComponent {
    public DataContext: ClaimImporterDeclATabComponent = this;
    public EntityPM: ClaimPM = new ClaimPM();
    public ObjectTableName: string = "Customs.Claim";

    public ClaimImporterDeclAlist: ObservableCollection;
    public CommercialSalelist: ObservableCollection;

    public CurrentEditComponentId: string;
    private isControlEnabled: boolean = true;

    IsLoaded: boolean = false;

    private _ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, private EntityResourceService: EntityResourceService) {
        super();
        this.ClaimImporterDeclAlist = new ObservableCollection([]);
        this.CommercialSalelist = new ObservableCollection([]);
        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];

        this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(response => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.Claim").subscribe(response => {
                this.EntityResourceService.getEntityResourceByTableName("Customs.ClaimImporterDeclarsP3Loi").subscribe(response => {
                    this.EntityResourceService.getEntityResourceByTableName("Customs.ClaimImporterDeclarsPage3A").subscribe(response => {
                        this.EntityResourceService.getEntityResourceByTableName("Customs.ClaimImporterDeclarsPage3").subscribe(response => {
                            if (this.entityArgs.EntityPM != null) {
                                this.EntityPM = this.entityArgs.EntityPM;
                                this.BuildImporterDeclareList();
                                this.BuildCommercialSaleList();
                            }
                            this.Listen();
                            this.IsLoaded = true;
                        });
                    });
                });
            });
        });

    }

    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {

            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.BuildImporterDeclareList();
                        this.BuildCommercialSaleList();
                    }
                })
            );
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.BuildImporterDeclareList();
                        this.BuildCommercialSaleList();
                    }
                })
            );
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                    if (this.CurrentEditComponentId == this.CurrentSession.CurrentEditComponent.ComponentId) {
                        if (tabCode == "CLMA") {
                            this.BuildImporterDeclareList();
                            this.BuildCommercialSaleList();
                        }
                    }
                })
            );
        }
    }

    RefreshEntity() {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    }

    getItemTitle(Item: any) {//task 41087
        return Item.ImporterLoiDeclarationTypeCode + "," + Item.ImporterDeclarationTypeName;
    }

    selectedTab: LogTab;
    public get SelectedTab() { return this.selectedTab; }
    public set SelectedTab(tab: LogTab) {
        this.selectedTab = tab;
    }

    public SetTabArgs(args: any, valdationErrorList: any[] = null) {
        this.EntityPM = args.EntityPM;
        console.log("EntityPM", this.EntityPM);
    }

    public get IsControlEnabled() { return this.isControlEnabled; }
    public set IsControlEnabled(newValue: boolean) { this.isControlEnabled = newValue; }

    public get ValidationErrorsList() { return this._ValidationErrorsList; }
    public set ValidationErrorsList(newValue: string[]) { this._ValidationErrorsList = newValue; }

    public get ImporterAffidavit() { return this.EntityPM.ImporterAffidavit; }
    public set ImporterAffidavit(newValue: string) { this.EntityPM.ImporterAffidavit = newValue; }

    public get AccountCurrencyTypeCode() { return this.EntityPM.AccountCurrencyTypeCode; }
    public set AccountCurrencyTypeCode(newValue: string) { this.EntityPM.AccountCurrencyTypeCode = newValue; }

    public get BeneficiaryExternalID() { return this.EntityPM.BeneficiaryExternalID; }
    public set BeneficiaryExternalID(newValue: string) { this.EntityPM.BeneficiaryExternalID = newValue; }


    BuildImporterDeclareList() {
        this.ClaimImporterDeclAlist = new ObservableCollection([]);

        if (this.EntityPM.ClaimImporterDeclarsPage3 != null && this.EntityPM.ClaimImporterDeclarsPage3.length > 0) {
            for (let item of this.EntityPM.ClaimImporterDeclarsPage3) {
                this.ClaimImporterDeclAlist.Insert(new ClaimImporterDeclarsPage3LineComponent(item, this.EntityPM));
            }
        }
    }

    AddImporterDeclareCommand() {
        if (!this.IsControlEnabled) return;

        this.ValidationErrorCheck();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }

        var newClaimImporterDeclarsPage3PM = new ClaimImporterDeclarsPage3PM(this.EntityPM);
        newClaimImporterDeclarsPage3PM.Tenant = this.EntityPM.Tenant;
        newClaimImporterDeclarsPage3PM.ClaimId = this.EntityPM.Id;
        newClaimImporterDeclarsPage3PM.LineNo = (ArrayTool.Max(this.EntityPM.ClaimImporterDeclarsPage3, "LineNo") + 1);

        this.ClaimImporterDeclAlist.Insert(new ClaimImporterDeclarsPage3LineComponent(newClaimImporterDeclarsPage3PM, this.EntityPM));
        this.EntityPM.AddClaimImporterDeclarsPage3(newClaimImporterDeclarsPage3PM);
    }

    DeleteImporterDeclareCommand(item: ClaimImporterDeclarsPage3LineComponent) {
        if (!this.IsControlEnabled) return;

        if (!AppTool.IsNullOrEmpty(item)) {
            this.ClaimImporterDeclAlist.Remove(item);
            this.EntityPM.RemoveClaimImporterDeclarsPage3(item.entityPM);
        }

        this.ValidationErrorCheck();
    }

    MyDeclarationListkeyUp($event, item: ClaimImporterDeclarsPage3LineComponent) {
        //if (!isNaN(parseFloat(item.MyDeclarationList)) && isFinite(item.MyDeclarationList)) {
        //    return true;
        //}
        //if ((!AppTool.IsNullOrEmpty(item.MyDeclarationList) && isNaN(item.MyDeclarationList))
        //    || (AppTool.IsNullOrEmpty(item.MyDeclarationList) && $event.code.includes("Key") && isNaN($event.key))) {
        //    return false;
        //}

        if (!AppTool.IsNullOrEmpty(item.MyDeclarationList)){
            if (isNaN(item.MyDeclarationList)) {
                return false;
            }
        }
        else {
            if ($event.code.includes("Key") && isNaN($event.key)) {
                return false;
            }
        }

        return true;
    }

    IsNumberKey(event) {
        var charCode = (event.which) ? event.which : event.keyCode
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        return true;
    }

    EditImporterDeclareCommand(item: ClaimImporterDeclarsPage3LineComponent) {
        if (!this.IsControlEnabled) return;

        if (!AppTool.IsNullOrEmpty(item)) {
            this.ValidationErrorsList = [];
            if (AppTool.IsNullOrEmpty(item.ImporterLoiDeclarationTypeCode)) {
                this.ValidationErrorsList.push("חובה להזין הצהרת יבואן");
                return;
            }

            var windowArgs: any = {};
            windowArgs.ClaimImporterDeclarsP3Loilist = item.ClaimImporterDeclarsP3Loilist;

            var logitudeWindow = new LogitudeWindow();
            logitudeWindow.Width = 350;
            logitudeWindow.Height = 400;
            logitudeWindow.IsShowCloseButton = false;
            logitudeWindow.Title = TextCodeTranslator.Translate("Customs.Claim.g.CreateDecList2") + " " + item.ImporterLoiDeclarationTypeCode;
            logitudeWindow.WindowArgs = windowArgs;
            logitudeWindow.WindowClosed.subscribe(($event: any) => this.SelectionCompleted($event, item));
            logitudeWindow.Show('./CustomsModules/CustomsClaim/Components/EditTabs/ImporterDeclaration/ClaimImporterDeclAP3LoisComponent');
        }
    }

    SelectionCompleted(msg: string, item: ClaimImporterDeclarsPage3LineComponent) {
        if (msg != "Cancel") {
            if (item.entityPM.ClaimImporterDeclarsP3Loi != null && item.entityPM.ClaimImporterDeclarsP3Loi.length > 0) {
                item.entityPM.ClaimImporterDeclarsP3Loi.forEach((declarationitem) => {
                    item.entityPM.RemoveClaimImporterDeclarsP3Loi(declarationitem);
                });
            } 
            item.ClaimImporterDeclarsP3Loilist.Clear();
            item.entityPM.ClaimImporterDeclarsP3Loi = [];

            if (!AppTool.IsNullOrEmpty(msg)) {
                var listOfDeclarations = msg.split(",");
                for (let declarationNumber of listOfDeclarations) {
                    if (!AppTool.IsNullOrEmpty(declarationNumber) && declarationNumber != "null") {
                        item.AddNewImporterDeclarsP3Loi(declarationNumber);
                    }
                }
            }
            item.BuildClaimImporterDeclAP3LoisList();
        }
    }

    ValidationErrorCheck() {

        this.ValidationErrorsList = [];

        //Importer Declaration
        if (this.ClaimImporterDeclAlist != null && this.ClaimImporterDeclAlist.Length > 0) {
            var nullVM = this.ClaimImporterDeclAlist.Collection.filter(vm => vm.ImporterLoiDeclarationTypeCode == null);
            if (nullVM.length > 0) {
                this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.Claim.O.UseEmptyRow") + " (תצהיר יבואן)");
                return;
            }
        }

        if (this.ClaimImporterDeclAlist != null && this.ClaimImporterDeclAlist.Length >= 3) {
            this.ValidationErrorsList.push("לא ניתן להוסיף יותר מ 3 שורות לתצהיר היבואן");
            return;
        }
    }

    BuildCommercialSaleList() {
        this.CommercialSalelist = new ObservableCollection([]);

        if (this.EntityPM.ClaimImporterDeclarsPage3A != null && this.EntityPM.ClaimImporterDeclarsPage3A.length > 0) {
            for (let item of this.EntityPM.ClaimImporterDeclarsPage3A) {
                this.CommercialSalelist.Insert(new CommercialSaleComponent(item));
            }
        }
    }

    AddCommercialSaleCommand() {
        if (!this.IsControlEnabled) return;

        this.CommercialValidationErrorCheck();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }

        var newClaimImporterDeclarsPage3APM = new ClaimImporterDeclarsPage3APM(this.EntityPM);
        newClaimImporterDeclarsPage3APM.Tenant = this.EntityPM.Tenant;
        newClaimImporterDeclarsPage3APM.ClaimId = this.EntityPM.Id;
        newClaimImporterDeclarsPage3APM.LineNo = (ArrayTool.Max(this.EntityPM.ClaimImporterDeclarsPage3, "LineNo") + 1);

        this.CommercialSalelist.Insert(new CommercialSaleComponent(newClaimImporterDeclarsPage3APM));
        this.EntityPM.AddClaimImporterDeclarsPage3A(newClaimImporterDeclarsPage3APM);
    }

    DeleteCommercialSaleCommand(item: CommercialSaleComponent) {
        if (!this.IsControlEnabled) return;

        if (!AppTool.IsNullOrEmpty(item)) {
            this.CommercialSalelist.Remove(item);
            this.EntityPM.RemoveClaimImporterDeclarsPage3A(item.entityPM);
        }

        this.CommercialValidationErrorCheck();
    }

    CommercialValidationErrorCheck() {

        this.ValidationErrorsList = [];
        if (this.CommercialSalelist != null && this.CommercialSalelist.Length > 0) {
            var nullVM = this.CommercialSalelist.Collection.filter(vm => vm.CommercialSaleTypeCode == null);
            if (nullVM.length > 0) {
                this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.Claim.O.UseEmptyRow") + " (פרטי המישור המסחרי)");
                return;
            }
        }

        if (this.CommercialSalelist != null && this.CommercialSalelist.Length >= 4) {
            this.ValidationErrorsList.push("לא ניתן להוסיף יותר מ 4 שורות לפרטי המישור המסחרי");
            return;
        }

    }
}


export class ClaimImporterDeclarsPage3LineComponent extends BaseComponent {
    public ObjectTableName = "Customs.ClaimImporterDeclarsPage3";
    public DataContext: ClaimImporterDeclarsPage3LineComponent = this;
    public ClaimImporterDeclarsP3Loilist: ObservableCollection;

    constructor(public entityPM: ClaimImporterDeclarsPage3PM, public claimPM: ClaimPM) {
        super();
        this.ClaimImporterDeclarsP3Loilist = new ObservableCollection([]);
        this.BuildClaimImporterDeclAP3LoisList();
    }

    public get ImporterLoiDeclarationTypeCode() { return this.entityPM.ImporterLoiDeclarationTypeCode; }
    public set ImporterLoiDeclarationTypeCode(newValue: string) { this.entityPM.ImporterLoiDeclarationTypeCode = newValue; }

    public get ImporterDeclarationTypeName() { return this.entityPM.ImporterDeclarationTypeName; }
    public set ImporterDeclarationTypeName(newValue: string) { this.entityPM.ImporterDeclarationTypeName = newValue; }

    private _MyDeclarationList: string;
    public get MyDeclarationList() { return this._MyDeclarationList; }
    public set MyDeclarationList(newValue: any) {
        if (this._MyDeclarationList == "List"){
            return;
        }
        this._MyDeclarationList = newValue;
    }

    public SetLocalName(entity, fieldName) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        } else {
            this[fieldName] = null;
        }
    }

    BuildClaimImporterDeclAP3LoisList() {
        this.ClaimImporterDeclarsP3Loilist = new ObservableCollection([]);
        this._MyDeclarationList = "";

        if (this.entityPM.ClaimImporterDeclarsP3Loi != null) {
            for (let item of this.entityPM.ClaimImporterDeclarsP3Loi) {
                this.ClaimImporterDeclarsP3Loilist.Insert(item.DeclarationNumber);
            }
            if (this.entityPM.ClaimImporterDeclarsP3Loi.length == 1) {
                this.MyDeclarationList = this.entityPM.ClaimImporterDeclarsP3Loi[0].DeclarationNumber;
                this.UIProperties.SetEnabled("MyDeclarationList", null, false);
            }
            else if (this.entityPM.ClaimImporterDeclarsP3Loi.length > 1) {
                this.MyDeclarationList = "List";
                this.UIProperties.SetEnabled("MyDeclarationList", null, true);
            }
        }
    }

    ImporterDeclarationLostFocus(event) {

        if (AppTool.IsNullOrEmpty(this.ImporterLoiDeclarationTypeCode)
            || this.MyDeclarationList == "List") {
            return;
        }

        if (isNaN(this.MyDeclarationList)) {
            var messageWindow = new MessageWindow();
            messageWindow.Title = TextCodeTranslator.Translate("Customs.General.O.Warning");
            messageWindow.Width = 250;
            messageWindow.Height = 150;
            messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
            messageWindow.Show("יש להזין מספר הצהרה");

            this.MyDeclarationList = "";
            return;
        }

        if (AppTool.IsNullOrEmpty(this.MyDeclarationList) && this.ClaimImporterDeclarsP3Loilist != null && this.ClaimImporterDeclarsP3Loilist.Length == 1) {
            this.ClaimImporterDeclarsP3Loilist.Clear();
            this.entityPM.RemoveClaimImporterDeclarsP3Loi(this.entityPM.ClaimImporterDeclarsP3Loi[0]);
            return;
        }

        this.ClaimImporterDeclarsP3Loilist.Clear();
        if (this.entityPM.ClaimImporterDeclarsP3Loi == null || this.entityPM.ClaimImporterDeclarsP3Loi.length == 0) {
            this.AddNewImporterDeclarsP3Loi(this.MyDeclarationList);
        }
        else {
            this.entityPM.ClaimImporterDeclarsP3Loi[0].DeclarationNumber = this.MyDeclarationList;
        }
        this.ClaimImporterDeclarsP3Loilist.Insert(this.MyDeclarationList);    
    }

    AddNewImporterDeclarsP3Loi(declarationNumber: string){
        var newClaimImporterDeclarsP3LoiPM = new ClaimImporterDeclarsP3LoiPM(this.EntityPM);
        newClaimImporterDeclarsP3LoiPM.Tenant = this.entityPM.Tenant;
        newClaimImporterDeclarsP3LoiPM.ClaimId = this.entityPM.ClaimId;
        newClaimImporterDeclarsP3LoiPM.CounterKey = this.entityPM.LineNo;
        newClaimImporterDeclarsP3LoiPM.LineNo = (ArrayTool.Max(this.entityPM.ClaimImporterDeclarsP3Loi, "LineNo") + 1);
        if (!AppTool.IsNullOrEmpty(declarationNumber)) {
            newClaimImporterDeclarsP3LoiPM.DeclarationNumber = declarationNumber;
        }

        this.entityPM.AddClaimImporterDeclarsP3Loi(newClaimImporterDeclarsP3LoiPM);
    }
}

export class CommercialSaleComponent extends BaseComponent {
    public ObjectTableName = "Customs.ClaimImporterDeclarsPage3A";
    public DataContext: CommercialSaleComponent = this;

    constructor(public entityPM: ClaimImporterDeclarsPage3APM) {
        super();
    }

    public get CommercialSaleTypeCode() { return this.entityPM.CommercialSaleTypeCode; }
    public set CommercialSaleTypeCode(newValue: string) { this.entityPM.CommercialSaleTypeCode = newValue; }

    public get CommercialSaleTypeName() { return this.entityPM.CommercialSaleTypeName; }
    public set CommercialSaleTypeName(newValue: string) { this.entityPM.CommercialSaleTypeName = newValue; }

    public SetLocalName(entity, fieldName) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        } else {
            this[fieldName] = null;
        }
    }

}
