import { Component } from '@angular/core';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { AppTool, ArrayTool } from '../../../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { LogTab } from '../../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ClaimPM } from '../../../../../Customs/EntityPMs/ClaimPM';
import { ClaimsRelatedEntityPM } from '../../../../../Customs/EntityPMs/ClaimsRelatedEntityPM';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { CustomBankList } from '../../../../../Customs/EntityLists/CustomBankList';
import { CustomBankListService } from '../../../../../Customs/Services/StandardLists/CustomBankListService';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';


@Component({
    
    templateUrl: './ClaimRefundDetailsTabComponent.html',
})

export class ClaimRefundDetailsTabComponent extends BaseComponent {
    public DataContext: ClaimRefundDetailsTabComponent = this;
    public EntityPM: ClaimPM = new ClaimPM();
    public ObjectTableName: string = "Customs.Claim";

    public banksList: CustomBankList[] = [];

    public CurrentEditComponentId: string;
    private _IsControlEnabled: boolean = true;
    private _IsraelBankFieldsEnabled: boolean = false;
    private _ForeignBankFieldsEnabled: boolean = false;

    _CustomBankListService: CustomBankListService = new CustomBankListService();

    IsLoaded: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, private EntityResourceService: EntityResourceService) {
        super();
        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];

        this.EntityResourceService.getEntityResourceByTableName("Customs.Claim").subscribe((response:any) => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.ClaimsRelatedEntity").subscribe((response:any) => {
                   if (this.entityArgs.EntityPM != null) {
                       this.EntityPM = this.entityArgs.EntityPM;
                       this.SetBankFieldsEnabled();
                       this.LoadBanks();
                    }
                    this.Listen();
                    this.IsLoaded = true;
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
                    }
                })
            );
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                })
            );

            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                    if (this.CurrentEditComponentId == this.CurrentSession.CurrentEditComponent.ComponentId) {
                        if (tabCode == "CLMR") {
                            //this.RefreshEntity();
                        }
                    }
                })
            );
        }
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

    public get IsControlEnabled() { return this._IsControlEnabled; }
    public set IsControlEnabled(newValue: boolean) { this._IsControlEnabled = newValue; }

    public get IsraelBankFieldsEnabled() { return this._IsraelBankFieldsEnabled; }
    public set IsraelBankFieldsEnabled(newValue: boolean) { this._IsraelBankFieldsEnabled = newValue; }

    public get ForeignBankFieldsEnabled() { return this._ForeignBankFieldsEnabled; }
    public set ForeignBankFieldsEnabled(newValue: boolean) { this._ForeignBankFieldsEnabled = newValue; }

    public get BeneficiaryActivityTypeCode() { return this.EntityPM.BeneficiaryActivityTypeCode; }
    public set BeneficiaryActivityTypeCode(newValue: string) { this.EntityPM.BeneficiaryActivityTypeCode = newValue; }

    public get AccountCurrencyTypeCode() { return this.EntityPM.AccountCurrencyTypeCode; }
    public set AccountCurrencyTypeCode(newValue: string) { this.EntityPM.AccountCurrencyTypeCode = newValue; }

    public get BeneficiaryExternalID() { return this.EntityPM.BeneficiaryExternalID; }
    public set BeneficiaryExternalID(newValue: string) { this.EntityPM.BeneficiaryExternalID = newValue; }

    public get BankTypeCode() { return this.EntityPM.BankTypeCode; }
    public set BankTypeCode(newValue: string) { this.EntityPM.BankTypeCode = newValue; }

    public get ForeignBank() { return this.EntityPM.ForeignBank; }
    public set ForeignBank(newValue: string) { this.EntityPM.ForeignBank = newValue; }

    public get AccountCountryCode() { return this.EntityPM.AccountCountryCode; }
    public set AccountCountryCode(newValue: string) {
        this.EntityPM.AccountCountryCode = newValue;
        this.SetBankFieldsEnabled();
    }

    public get AccountBranchCode() { return this.EntityPM.AccountBranchCode; }
    public set AccountBranchCode(newValue: string) { this.EntityPM.AccountBranchCode = newValue; }

    public get ForeignBranch() { return this.EntityPM.ForeignBranch; }
    public set ForeignBranch(newValue: string) { this.EntityPM.ForeignBranch = newValue; }

    public get AccountNumber() { return this.EntityPM.AccountNumber; }
    public set AccountNumber(newValue: string) { this.EntityPM.AccountNumber = newValue; }

    public get ForeignAccountNumber() { return this.EntityPM.ForeignAccountNumber; }
    public set ForeignAccountNumber(newValue: string) { this.EntityPM.ForeignAccountNumber = newValue; }

    _SelectedBankIndex: CustomBankList;
    get SelectedBankIndex() { return this._SelectedBankIndex; }
    set SelectedBankIndex(value: CustomBankList) {
        if (this._SelectedBankIndex != value) {
            this._SelectedBankIndex = value;

            if (value != null) {
                this.BankTypeCode = value.BankCode;
                this.AccountBranchCode = Number(value.BranchCode).toString() + "," + value.BankCode;
                this.AccountNumber = value.AccountNumber;
            }
            else {
                this.BankTypeCode = null;
                this.AccountBranchCode = null;
                this.AccountNumber = null;
            }
        }
    }

    LoadBanks() {
        this._CustomBankListService.getAllFromCache().subscribe((response: ServiceResponse) => {
            if (response) {
                if (!response.HasError) {
                    this.banksList = response.Result.filter(d => d.PayerTypeCode == "3" && !d.InActive);
                }
            }
        });
    }

    DeleteBankDetailsCommand(){
        this.SelectedBankIndex = null;
        this.BankTypeCode = null;
        this.AccountBranchCode = null;
        this.AccountNumber = null;
    }

    SetBankFieldsEnabled() {
        if (AppTool.IsNullOrEmpty(this.AccountCountryCode)) {
            this.SetAllBankFieldsEnabled();
            return;
        }

        switch (this.AccountCountryCode) {
            case "IL": // Israel
                this.SetIsraelBankFieldsEnabled();
                break;
            default: // Foreign
                this.SetForeignBankFieldsEnabled();
                break;
        }
    }

    SetAllBankFieldsEnabled() {
        this.IsraelBankFieldsEnabled = false;
        this.ForeignBankFieldsEnabled = false;

        this.UIProperties.SetEnabled("AccountBranchCode", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("BankTypeCode", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("AccountNumber", this.ObjectTableName, false);

        this.UIProperties.SetEnabled("AccountCurrencyTypeCode", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ForeignBank", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ForeignBranch", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ForeignAccountNumber", this.ObjectTableName, false);
    }

    SetIsraelBankFieldsEnabled() {
        this.IsraelBankFieldsEnabled = true;
        this.ForeignBankFieldsEnabled = false;

        this.UIProperties.SetEnabled("AccountBranchCode", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("BankTypeCode", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("AccountNumber", this.ObjectTableName, true);

        this.UIProperties.SetEnabled("AccountCurrencyTypeCode", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ForeignBank", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ForeignBranch", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ForeignAccountNumber", this.ObjectTableName, false);

        this.AccountCurrencyTypeCode = null;
        this.ForeignBank = null;
        this.ForeignBranch = null;
        this.ForeignAccountNumber = null;
    }

    SetForeignBankFieldsEnabled() {
        this.IsraelBankFieldsEnabled = false;
        this.ForeignBankFieldsEnabled = true;

        this.UIProperties.SetEnabled("AccountBranchCode", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("BankTypeCode", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("AccountNumber", this.ObjectTableName, false);

        this.UIProperties.SetEnabled("AccountCurrencyTypeCode", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("ForeignBank", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("ForeignBranch", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("ForeignAccountNumber", this.ObjectTableName, true);

        this.DeleteBankDetailsCommand();
    }

}
