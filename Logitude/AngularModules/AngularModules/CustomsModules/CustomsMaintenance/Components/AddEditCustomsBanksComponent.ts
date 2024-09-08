import { Component, } from '@angular/core';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { AppTool } from '../../../Infrastructure/Tools';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
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
import { CardPMService } from 'Common/Services/StandardPMs/CardPMService';
import { CardPM } from 'Common/EntityPMs/CardPM';


@Component({
    templateUrl: './AddEditCustomsBanksComponent.html',
    providers: [AmitalAPIRequestsComponent],
})


export class AddEditCustomsBanksComponent extends BaseComponent {
    public ObjectTableName: string = "Customs.CustomBank";
    public EntityPM: CustomBankPM;
    public CustomBankListService = new CustomBankPMService();
    public AmitalAPIRequestsComponent: AmitalAPIRequestsComponent = new AmitalAPIRequestsComponent();
    private _requiredFields: string[] = ["BankCode", "BranchCode", "AccountNumber", "LocalName", "PayerTypeCode"];

    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;

    isWindowMode: boolean = true;
    ValidationErrorsList: any[] = [];
    CustomBankPMService: CustomBankPMService = new CustomBankPMService();
    CustomBankList: CustomBankList;
    CustomBankCardExtendedPMService: CustomBankCardExtendedPMService = new CustomBankCardExtendedPMService();
    CustomBankCardItems = new ObservableCollection([]);
    cardPMService: CardPMService = new CardPMService();
    cardsList = [];
    currentCardList: any = [];


    constructor(public entityArgs: EntityArgs) {
        super();
        if (AppTool.IsNullOrEmpty(entityArgs.EntityPM)) {
            this.EntityPM = new CustomBankPM();
            this.EntityPM.Tenant = SessionLocator.Tenant;
            this.isWindowMode = true;
        }
        this._requiredFields.forEach((field) => {
            this.UIProperties.SetRequired(field, this.ObjectTableName);
        });

    }


    SetWindowArgs(_WindowArgs) {

        this._entityResourceService.getEntityResourceByTableName("Customs.CustomBank").subscribe((response: any) => {
            this.CurrentSession.StartBusyIndicatorLoading();
            this.CustomBankListService
                .get
                (_WindowArgs)
                .subscribe((rsp: any) => {
                    Object.keys(rsp.Result).forEach((key) => {
                        this.EntityPM[key] = rsp.Result[key]
                    });
                    new CardListService().getAll().subscribe((response: any) => {
                        this.cardsList = response.Result;

                        const cardMap = new Map();
                        this.cardsList.forEach((card) => {
                            cardMap.set(card.Id, card);
                        });

                        this.EntityPM.CustomBanksCards.forEach((Bcard: CustomBanksCardPM) => {
                            const card = cardMap.get(Bcard.CardId);
                            this.currentCardList[Bcard.CardId] = card?.Code;
                            Bcard.CardName = card?.LocalName || card?.EnglishName;
                        });

                        this.CurrentSession.StopBusyIndicator();
                    });

                    this.CustomBankCardItems = new ObservableCollection(this.EntityPM.CustomBanksCards || [])
                });
        })
    }

    onInputChange(event, ObjectFieldName) {
        if (event === null) return;
        if (ObjectFieldName === "BankCode") {
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


    OkButtonClicked() {
        var errors = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (errors.length > 0) {
            this.ValidationErrorsList = [];
            this.ValidationErrorsList = errors;
        } else {
            if (this.CustomBankCardItems.Length > 0 && this.EntityPM.PayerTypeCode !== "3") {
                this.EntityPM.CustomBanksCards.forEach((card: CustomBanksCardPM) => {
                    this.EntityPM.AddCustomBanksCard(card);
                });
            }

            this.CustomBankPMService.get(this.EntityPM.Id).subscribe((response: any) => {
                if (response.Result === null || response.Result === undefined) {
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
            });
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }


    //Grid logic
    getCardDetails(cardId): CardPM {

        console.log(this.cardsList.find(card => card.Id === cardId))
        return this.cardsList.find(card => card.Id === cardId);
    }

    onCardChange(event, item) {

        if (event === null) return;

        this.EntityPM?.CustomBanksCards.filter(card => card.CardId === item.CardId).forEach(card => {
            card.CardName = event.LocalName || event.EnglishName;
            card.CardId = event.Id;
            this.currentCardList[card.CardId] = event.Code;
        })
        this.CustomBankCardItems = new ObservableCollection(this.EntityPM.CustomBanksCards || [])

        return;

    }

    AddBankCardLine() {
        let row = new CustomBanksCardPM(this.EntityPM);
        row.CustomBankId = this.EntityPM.Id;
        row.CustomsBankName = this.EntityPM.LocalName;
        this.EntityPM.CustomBanksCards.push(row);
        this.CustomBankCardItems = new ObservableCollection(this.EntityPM.CustomBanksCards || [])

        let index = this.customBanksCards.indexOf(row);
        if (index == -1) {
            row.EntityParentPM = this;
            this.EntityPM.CustomBanksCards.push(row);
        }

    }

    RemoveBankCardLine(card: CustomBanksCardPM) {
        const confirmWindow = new ConfirmWindow();
        confirmWindow.Show("האם למחוק את השורה?");
        confirmWindow.WindowClosed.subscribe(() => {
            if (confirmWindow.Yes) {
                if (card != null) {
                    this.CustomBankCardItems.RemoveFromIndex(this.CustomBankCardItems.GetIndex(card));
                    let index = this.customBanksCards.indexOf(card);
                    if (index > -1) {
                        this.customBanksCards.splice(index, 1)
                    }
                }
            }
        });
    }
    //// End of Grid logic

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
