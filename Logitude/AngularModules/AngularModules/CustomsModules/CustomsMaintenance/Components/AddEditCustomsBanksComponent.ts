import { Component, NgModule } from '@angular/core';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { LocationDirective } from '../../../Infrastructure/Utilities/LocationDirective';
import { ApiQueryFilters, FilterItem } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { AppTool, ArrayTool } from '../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { CustomBankPM } from '../../../Customs/EntityPMs/CustomBankPM';
import { CustomBanksCardPM } from '../../../Customs/EntityPMs/CustomBanksCardPM';
import { CustomBankPMService } from '../../../Customs/Services/StandardPMs/CustomBankPMService';
import { CustomBankList } from '../../../Customs/EntityLists/CustomBankList';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { AmitalAPIRequestsComponent } from 'InfrastructureModules/InfrastructureOthers/AmitalAPI/AmitalAPIRequestsComponent';
import { CustomBankCardExtendedPMService } from '../../../Customs/Services/ExtendedPMs/CustomBankCardExtendedPMService';

import { CardListService } from 'Common/Services/StandardLists/CardListService';
// import { CustomsBranchListService } from 'Customs/Services/StandardLists/CustomsBranchListService';
import { CardPMService } from 'Common/Services/StandardPMs/CardPMService';
// import { CardPM } from 'Common/EntityPMs/CardPM';


@Component({

    templateUrl: './AddEditCustomsBanksComponent.html',
    providers: [AmitalAPIRequestsComponent],
})


export class AddEditCustomsBanksComponent extends BaseComponent {
    public DataContext: any = this;
    public ObjectTableName: string = "Customs.CustomBank";
    public EntityPM: CustomBankPM;
    isWindowMode: boolean = true;
    ValidationErrorsList: any[] = [];
    CustomBankPMService: CustomBankPMService = new CustomBankPMService();
    public CustomBankListService = new CustomBankPMService();
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    CustomBankList: CustomBankList;
    private CurrentSession = SessionLocator.SelectedSession;
    public AmitalAPIRequestsComponent: AmitalAPIRequestsComponent = new AmitalAPIRequestsComponent();
    CustomBankCardExtendedPMService: CustomBankCardExtendedPMService = new CustomBankCardExtendedPMService();
    CustomBankCardItems = new ObservableCollection([]);
    cardPMService: CardPMService = new CardPMService();
    cardTempData: any = {};

    constructor(public entityArgs: EntityArgs) {
        super();
        if (AppTool.IsNullOrEmpty(entityArgs.EntityPM)) {
            this.EntityPM = new CustomBankPM();
            this.EntityPM.Tenant = SessionLocator.Tenant;
            this.isWindowMode = true;
        }
    }

    SetWindowArgs(_WindowArgs) {

        this._entityResourceService.getEntityResourceByTableName("Customs.CustomBank").subscribe((response: any) => {
            this.CurrentSession.StartBusyIndicatorLoading();
            this.CustomBankListService
                .get
                (_WindowArgs)
                .subscribe((rsp: any) => {

                    Object.keys(rsp.Result).forEach((key) => {
                        this.EntityPM[key] = rsp.Result[key];
                    });

                    this.EntityPM.LocalName = rsp.Result.BankName;

                    if (this.EntityPM.CustomBanksCards.length > 0) {
                        this.EntityPM.CustomBanksCards.forEach((card, index) => {

                            let filters = new ApiQueryFilters(true);
                            filters.addAdditionalFilter('Id', card.CardId, null, null, "InListExact", false, false, false, "string");


                            new CardListService().getByFilters(filters).subscribe((response: any) => {
                                this.cardTempData[card.CardId] = response.Result[0]?.Code;
                                this.EntityPM.CustomBanksCards[index].CardName ? this.EntityPM.CustomBanksCards[index].CardName : response.Result[0]?.LocalName || response.Result[0]?.LocalName || response.Result[0].EnglishName || "";
                                this.EntityPM.CustomBanksCards[index].EntityParentPM = null;

                            },
                            );
                        });
                    }
                    this.EntityPM.CustomBanksCards ? this.CustomBankCardItems = new ObservableCollection(this.EntityPM.CustomBanksCards) : this.CustomBankCardItems = new ObservableCollection([]);
                    this.CurrentSession.StopBusyIndicator();
                });
        })

        this.DataContext = this.EntityPM;
    }

    onInputChange(event, ObjectFieldName) {

        if (event === null) return;

        if (ObjectFieldName === "BankCode") {
            this.EntityPM.LocalName = event?.LocalName || null;
            this.EntityPM.EnglishName = event?.EnglishName || null;
            this.EntityPM.BankCode = event?.Code;
            return;
        }
        if (ObjectFieldName === "BranchCode") {
            this.EntityPM.BranchCode = event.Code;
            this.EntityPM.CustomsBranchId = event.Id;
            this.EntityPM.BranchName = event.LocalName;
            return;
        }

        this.EntityPM[ObjectFieldName] = event;

    }


    onCardChange(event, item, ObjectFieldName) {

        this.EntityPM.CustomBanksCards[this.EntityPM.CustomBanksCards.indexOf(item)][ObjectFieldName] = event;
    }

    OkButtonClicked() {
        var errors = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (errors.length > 0) {
            this.ValidationErrorsList = [];
            this.ValidationErrorsList = errors;
        } else {

            if (this.CustomBankPMService.get(this.EntityPM.Id) == null) {
                this.CustomBankPMService.insert(this.EntityPM).subscribe((myResult: any) => {
                    let mm: ServiceResponse = myResult;
                    if (!mm.HasError) {
                        this.CurrentSession.CloseCurrentWindowEmit("ok");
                    }
                    else {
                        this.ValidationErrorsList = mm.ErrorsArray;
                        this.CurrentSession.StopBusyIndicator();
                    }
                });

            } else {

                this.CustomBankPMService.update(this.EntityPM).subscribe((myResult: any) => {

                    let mm: ServiceResponse = myResult;
                    if (!mm.HasError) {
                        this.CurrentSession.CloseCurrentWindowEmit("ok");
                    }
                    else {
                        this.ValidationErrorsList = mm.ErrorsArray;
                        this.CurrentSession.StopBusyIndicator();
                    }
                });
            }

            if (this.CustomBankCardItems.Length > 0) {

                this.EntityPM.CustomBanksCards = this.CustomBankCardItems.Collection;
                this.EntityPM.CustomBanksCards.forEach((card: CustomBanksCardPM) => {
                    this.EntityPM.AddCustomBanksCard(card);
                });
            }
        }
    }


    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    public AddBankCardLine() {
        let line = new CustomBanksCardPM(this.EntityPM);
        line.CustomBankId = this.EntityPM.Id;
        line.CustomsBankName = this.EntityPM.LocalName;
        line.CustomBankId = this.EntityPM.Id;

        this.CustomBankCardItems.Insert(line);
        let index = this.customBanksCards.indexOf(line);

        if (index == -1) {
            line.EntityParentPM = this;
            this.EntityPM.CustomBanksCards.push(line);
        }
    }


    public AddCustomerCard(card: CustomBanksCardPM) {
        if (card != null) {
            let index = this.customBanksCards.indexOf(card);
            if (index == -1) {
                card.EntityParentPM = this;
                this.EntityPM.CustomBanksCards.push(card);
            }
        }
    }

    public RemoveBankCardLine(card: CustomBanksCardPM) {
        const _ConfirmWindow = new ConfirmWindow();
        _ConfirmWindow.Show("האם למחוק את השורה?");
        _ConfirmWindow.WindowClosed.subscribe(() => {
            if (_ConfirmWindow.Yes) {
                if (card != null) {
                    this.CustomBankCardItems.RemoveFromIndex(this.CustomBankCardItems.GetIndex(card));
                    let index = this.customBanksCards.indexOf(card);
                    if (index > -1) {
                        this.customBanksCards.splice(index, 1)
                    }
                }
            }
        });
        window.focus();
    }


    public get BankId() { return this.EntityPM.Id; }
    public set BankId(newValue: string) {
        this.EntityPM.Id = newValue;
    }

    public get BankAddress() { return this.EntityPM.BankAddress; }
    public set BankAddress(newValue: string) {
        this.EntityPM.BankAddress = newValue;
    }


    public get internalCode() { return this.EntityPM.InternalCode; }
    public set internalCode(newValue: string) {
        this.EntityPM.InternalCode = newValue;

    }

    public get bankCode() { return this.EntityPM.BankCode; }
    public set bankCode(newValue: string) {
        this.EntityPM.BankCode = newValue;
    }

    public get branchCode() { return this.EntityPM.BranchCode; }
    public set branchCode(newValue: string) {
        this.EntityPM.BranchCode = newValue;
    }

    public get localName() { return this.EntityPM.LocalName; }
    public set localName(newValue: string) {
        this.EntityPM.LocalName = newValue;
    }

    public get englishName() { return this.EntityPM.EnglishName; }
    public set englishName(newValue: string) {
        this.EntityPM.EnglishName = newValue;
    }

    public get accountNumber() { return this.EntityPM.AccountNumber; }
    public set accountNumber(newValue: string) {
        this.EntityPM.AccountNumber = newValue;
    }

    public get inActive() { return this.EntityPM.InActive; }
    public set inActive(newValue: boolean) {
        this.EntityPM.InActive = newValue;
    }

    public get payerTypeCode() { return this.EntityPM.PayerTypeCode; }
    public set payerTypeCode(newValue: string) {
        this.EntityPM.PayerTypeCode = newValue;
    }

    public get customBanksCards() { return this.EntityPM.CustomBanksCards; }
    public set customBanksCards(newValue: any) {
        this.EntityPM.CustomBanksCards = newValue;
    }
}
