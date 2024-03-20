import { Component } from '@angular/core';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { AppTool, ArrayTool } from '../../../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { LogTab } from '../../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ClaimPM } from '../../../../../Customs/EntityPMs/ClaimPM';
import { ClaimsRelatedEntityPM } from '../../../../../Customs/EntityPMs/ClaimsRelatedEntityPM';
import { ClaimsRelatedEntitiesAmountPM } from '../../../../../Customs/EntityPMs/ClaimsRelatedEntitiesAmountPM';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { CustomsSettingListService } from '../../../../../Customs/Services/StandardLists/CustomsSettingListService';
import { ClientList } from '../../../../../Customs/EntityLists/ClientList';
import { MessageWindow } from '../../../../../Controls/Windows/MessageWindow';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { ConfirmWindow } from '../../../../../Controls/Windows/ConfirmWindow';
import { DeclarationExtendedListService } from '../../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';

@Component({
    
    templateUrl: './ClaimRelatedEntityGeneralTabComponent.html',
})

export class ClaimRelatedEntityGeneralTabComponent extends BaseComponent {
  public IsDisplayOnly: boolean = false;

    public DataContext: ClaimRelatedEntityGeneralTabComponent = this;
    public EntityPM: ClaimsRelatedEntityPM = new ClaimsRelatedEntityPM(null); // added null because it demands a parameter parent.
    public ClaimPM: ClaimPM = new ClaimPM();
    public ObjectTableName: string = "Customs.ClaimsRelatedEntity";

    public ClaimsRelatedEntitiesAmountsList: ObservableCollection;

    public CurrentEditComponentId: string;
    private isControlEnabled: boolean = true;
    public Total: number = 0;
    FooterMethods: any;// html component requires this property. AOT

    private DeclarationExtendedListService: DeclarationExtendedListService = new DeclarationExtendedListService();

    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, private EntityResourceService: EntityResourceService) {
        super();
        this.ClaimsRelatedEntitiesAmountsList = new ObservableCollection([]);
        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        this.CalcClaimsRelatedEntitiesAmountsTotal();
    }

    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {

            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                }
                })
            );;

            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.BuildPaymentAmountList();
                    }
                })
            );
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                    if (this.CurrentEditComponentId == this.CurrentSession.CurrentEditComponent.ComponentId) {
                        if (tabCode == "CLMG" && !this.CurrentSession.CurrentEditComponent.EntityPM.notSavedEntity) {
                            this.RefreshEntity();
                            this.BuildPaymentAmountList();
                        }
                    }
                })
            );
        }
    }

    InitTab(entityPM: ClaimsRelatedEntityPM, claimPM: ClaimPM, isEnable: boolean) {

        this.EntityPM = entityPM;
        this.ClaimPM = claimPM;
        this.isControlEnabled = isEnable;

        this.EntityResourceService.getEntityResourceByTableName("Customs.Claim").subscribe((response:any) => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.ClaimsRelatedEntity").subscribe((response:any) => {
                this.EntityResourceService.getEntityResourceByTableName("Customs.ClaimsRelatedEntitiesAmount").subscribe((response:any) => {
                    this.BuildPaymentAmountList();
                    this.Listen();
                });
            });
        });
    }


    RefreshEntity() {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
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

    public get ClaimEntityTypeCode() { return this.EntityPM.ClaimEntityTypeCode; }
    public set ClaimEntityTypeCode(newValue: string) { this.EntityPM.ClaimEntityTypeCode = newValue; }

    public get ExternalClaimNumber() { return this.EntityPM.ExternalClaimNumber; }
    public set ExternalClaimNumber(newValue: string) { this.EntityPM.ExternalClaimNumber = newValue; }

    public get ClaimEntityNumber() { return this.EntityPM.ClaimEntityNumber; }
    public set ClaimEntityNumber(newValue: string) { this.EntityPM.ClaimEntityNumber = newValue;}

    public get DeclarationVersion() { return this.EntityPM.DeclarationVersion; }
    public set DeclarationVersion(newValue: number) { this.EntityPM.DeclarationVersion = newValue; }

    public get ClaimAmount() { return this.EntityPM.ClaimAmount; }
    public set ClaimAmount(newValue: number) { this.EntityPM.ClaimAmount = newValue; }

    public get IsFinancialRefundDemand() { return this.EntityPM.IsFinancialRefundDemand; }
    public set IsFinancialRefundDemand(newValue: boolean) { this.EntityPM.IsFinancialRefundDemand = newValue; }

    BuildPaymentAmountList() {
        this.ClaimsRelatedEntitiesAmountsList = new ObservableCollection([]);

        if (this.EntityPM.ClaimsRelatedEntitiesAmounts != null && this.EntityPM.ClaimsRelatedEntitiesAmounts.length > 0) {
            for (let item of this.EntityPM.ClaimsRelatedEntitiesAmounts) {
                this.ClaimsRelatedEntitiesAmountsList.Insert(new ClaimRelatedEntityAmountComponent(item, this.EntityPM));
            }
        }
        this.CalcClaimsRelatedEntitiesAmountsTotal();
    }

    ClaimEntityNumberLostFocus() {
        if (AppTool.IsNullOrEmpty(this.ClaimEntityNumber) || this.ClaimEntityTypeCode != "1055") {
            return;
        }
        
        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        this.CurrentSession.StartBusyIndicator("")
        this.DeclarationExtendedListService.GetSingleDeclarationByNumber(this.ClaimEntityNumber, SessionLocator.Tenant)
            .subscribe((myResponse: ServiceResponse) => {
                this.FetchDeclaration(myResponse, false);
            });
    }

    FetchDeclaration(myResponse: ServiceResponse, sourceIsCostomFile: boolean) {
        var lastFetchDeclarationList = myResponse.Result;
        if (lastFetchDeclarationList != null) {
            if (AppTool.IsNullOrEmpty(this.ExternalClaimNumber)) {
                this.ExternalClaimNumber = lastFetchDeclarationList.CustomFileNo;
            }
            if (!AppTool.IsNullOrEmpty(lastFetchDeclarationList.DeclarationVersionId)) {
                this.DeclarationVersion = lastFetchDeclarationList.DeclarationVersionId;
            }
            this.UIProperties.SetValidity("ExternalClaimNumber", this.ObjectTableName, true, "");
            this.UIProperties.SetValidity("ClaimEntityNumber", this.ObjectTableName, true, "");
        }
        this.CurrentSession.StopBusyIndicator();
    }

    ExternalClaimNumberLostFocus() {
        if (AppTool.IsNullOrEmpty(this.ExternalClaimNumber) || this.ClaimEntityTypeCode != "1055")
        {
            return;
        }
        this.UIProperties.SetValidity("ClaimEntityNumber", this.ObjectTableName, true, "");
        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        this.CurrentSession.StartBusyIndicator("")
        this.DeclarationExtendedListService.GetSingleDeclarationByCustomFileNo(this.ExternalClaimNumber)
            .subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();
                this.FetchCustomFileNo(myResponse, false);
            });
    }

    FetchCustomFileNo(myResponse: ServiceResponse, sourceIsCostomFile: boolean){
        var lastFetchDeclarationList = myResponse.Result;
        if (lastFetchDeclarationList != null) {
            if (AppTool.IsNullOrEmpty(this.ClaimEntityNumber)) {
                this.ClaimEntityNumber = lastFetchDeclarationList.DeclarationNumber;
                this.UIProperties.SetRequired("ClaimEntityNumber", this.ObjectTableName, false);
            }
            if (!AppTool.IsNullOrEmpty(lastFetchDeclarationList.DeclarationVersionId)) {
                this.DeclarationVersion = lastFetchDeclarationList.DeclarationVersionId;
            }
            this.UIProperties.SetValidity("ExternalClaimNumber", this.ObjectTableName, true, "");
            this.UIProperties.SetValidity("ClaimEntityNumber", this.ObjectTableName, true, "");
        }
    }

    IsFinancialRefundDemandUnchecked(item: any) {
        if (item || this.ClaimsRelatedEntitiesAmountsList == null || (this.ClaimsRelatedEntitiesAmountsList != null && this.ClaimsRelatedEntitiesAmountsList.Length == 0)) {
            return;
        }

        var rfundDemandUncheckedWindow: ConfirmWindow = new ConfirmWindow();
        rfundDemandUncheckedWindow.Title = TextCodeTranslator.Translate("Customs.Claim.F.IsFinancialRefundDemand");
        rfundDemandUncheckedWindow.Width = 250;
        rfundDemandUncheckedWindow.Height = 150;
        rfundDemandUncheckedWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
        rfundDemandUncheckedWindow.NoButtonText = TextCodeTranslator.Translate("Customs.General.B.Cancel");
        rfundDemandUncheckedWindow.ShowCancelButton = false;
        rfundDemandUncheckedWindow.Show(TextCodeTranslator.Translate("Customs.General.O.BillingItemsDeleted"));
        rfundDemandUncheckedWindow.WindowClosed.subscribe((event: any) => {
            if (rfundDemandUncheckedWindow.Yes) {
                this.DeletePaymentAmoutLines();
            }
            else {
                this.IsFinancialRefundDemand = true;
            }
        });
    }

    DeletePaymentAmoutLines() {

        if (this.EntityPM.ClaimsRelatedEntitiesAmounts != null && this.EntityPM.ClaimsRelatedEntitiesAmounts.length > 0) {
            for (let item of this.EntityPM.ClaimsRelatedEntitiesAmounts) {
                this.EntityPM.RemoveClaimsRelatedEntitiesAmount(item);
            }
            this.ClaimsRelatedEntitiesAmountsList.Clear();
            this.ClaimsRelatedEntitiesAmountsList = new ObservableCollection([]);
        }
        this.CalcClaimsRelatedEntitiesAmountsTotal();
    }

    AddPaymentAmountCommand() {
        if (!this.IsControlEnabled || !this.IsFinancialRefundDemand) return;

        var newClaimsRelatedEntitiesAmountPM = new ClaimsRelatedEntitiesAmountPM(this.EntityPM);
        newClaimsRelatedEntitiesAmountPM.ClaimId = this.EntityPM.ClaimId;
        newClaimsRelatedEntitiesAmountPM.CounterKey = this.EntityPM.EntityCounterKey;
        newClaimsRelatedEntitiesAmountPM.Tenant = this.EntityPM.Tenant;
        newClaimsRelatedEntitiesAmountPM.LineNo = (ArrayTool.Max(this.EntityPM.ClaimsRelatedEntitiesAmounts, "LineNo") + 1);

        this.ClaimsRelatedEntitiesAmountsList.Insert(new ClaimRelatedEntityAmountComponent(newClaimsRelatedEntitiesAmountPM, this.EntityPM));
        this.EntityPM.AddClaimsRelatedEntitiesAmount(newClaimsRelatedEntitiesAmountPM);
        this.CalcClaimsRelatedEntitiesAmountsTotal();
    }

    DeletePaymentAmountCommand(item) {
        if (!this.IsControlEnabled) return;

        if (!AppTool.IsNullOrEmpty(item)) {
            this.ClaimsRelatedEntitiesAmountsList.Remove(item);
            this.EntityPM.RemoveClaimsRelatedEntitiesAmount(item.entityPM);
        }
        this.CalcClaimsRelatedEntitiesAmountsTotal();
    }

    Dispose() {
        //this.ClaimsRelatedEntitiesAmountsList.Collection.forEach((item) => {
        //    item.Dispose();
        //});
    }

    CalcClaimsRelatedEntitiesAmountsTotal() {
        this.Total = 0;
        if (this.ClaimsRelatedEntitiesAmountsList != null && this.ClaimsRelatedEntitiesAmountsList.Length > 0) {
            this.ClaimsRelatedEntitiesAmountsList.Collection.forEach((item) => {
                if (item.Amount != null)this.Total = this.Total + item.Amount;
            });

        }

        this.EntityPM.ClaimAmount = this.Total;    
    }
    //#endregion
}

export class ClaimRelatedEntityAmountComponent extends BaseComponent {
    public ObjectTableName = "Customs.ClaimsRelatedEntitiesAmount";
    public DataContext: ClaimRelatedEntityAmountComponent = this;

    constructor(public entityPM: ClaimsRelatedEntitiesAmountPM, public claimsRelatedEntity: ClaimsRelatedEntityPM) {
        super();
    }

    public get PaymentTypeCode() { return this.entityPM.PaymentTypeCode; }
    public set PaymentTypeCode(newValue: string) { this.entityPM.PaymentTypeCode = newValue; }

    public get PaymentTypeName() { return this.entityPM.PaymentTypeName; }
    public set PaymentTypeName(newValue: string) { this.entityPM.PaymentTypeName = newValue; }

    public get Amount() { return this.entityPM.Amount; }
    public set Amount(newValue: number) { this.entityPM.Amount = newValue; }

    public SetLocalName(entity, fieldName) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        } else {
            this[fieldName] = null;
        }
    }
}
