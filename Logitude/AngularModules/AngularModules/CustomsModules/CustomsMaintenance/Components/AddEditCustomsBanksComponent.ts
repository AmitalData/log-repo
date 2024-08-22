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
import { CustomSendOptionsArgs, SendRequestVIA } from '../../../Customs/DataContract/RequestParams/RequestParamsBase';

import { CustomBankPM } from '../../../Customs/EntityPMs/CustomBankPM';

import { CustomBankPMService } from '../../../Customs/Services/StandardPMs/CustomBankPMService';
import { CustomBankList } from '../../../Customs/EntityLists/CustomBankList';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';

import { ClassLevelValidator } from '../../../Infrastructure/Validators/ClassLevelValidator';
import { EntityListService } from 'Infrastructure/Services/EntityListService';
import { AmitalAPIRequestsComponent } from 'InfrastructureModules/InfrastructureOthers/AmitalAPI/AmitalAPIRequestsComponent';
import { CustomBankCardExtendedPMService } from '../../../Customs/Services/ExtendedPMs/CustomBankCardExtendedPMService';
import { ngModuleJitUrl } from '@angular/compiler';
import { CardListService } from 'Common/Services/StandardLists/CardListService';
import { get } from 'cypress/types/lodash';




@Component({

    templateUrl: './AddEditCustomsBanksComponent.html',
    providers: [AmitalAPIRequestsComponent]
})



export class AddEditCustomsBanksComponent extends BaseComponent {
    public DataContext: any = this;
    // public EntityId: string = null;
    public ObjectTableName: string = "Customs.CustomBank";
    public EntityPM: CustomBankPM;
    private _isNew: boolean = true;
    isWindowMode: boolean = true;
    ValidationErrorsList: any[] = [];
    _CustomBankPMService: CustomBankPMService = new CustomBankPMService();
    private _CustomBankListService = new CustomBankPMService();
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    _CustomBankList: CustomBankList;
    private CurrentSession = SessionLocator.SelectedSession;
    public AmitalAPIRequestsComponent: AmitalAPIRequestsComponent = new AmitalAPIRequestsComponent();

    CustomBankCardExtendedPMService: CustomBankCardExtendedPMService = new CustomBankCardExtendedPMService();
    CustomBankCardItems = new ObservableCollection([]);


    constructor(public entityArgs: EntityArgs) {
        super();
        if (AppTool.IsNullOrEmpty(entityArgs.EntityPM)) {
            this.EntityPM = new CustomBankPM();
            this.EntityPM.Tenant = SessionLocator.Tenant;
            this.isWindowMode = true;
        }
        this.UIProperties.SetRequired("InternalCode", this.ObjectTableName, true);
        this.UIProperties.SetRequired("BankCode", this.ObjectTableName, true);
        this.UIProperties.SetRequired("BranchCode", this.ObjectTableName, true);
        this.UIProperties.SetRequired("AccountNumber", this.ObjectTableName, true);
        this.UIProperties.SetRequired("LocalName", this.ObjectTableName, true);
        this.UIProperties.SetRequired("PayerTypeCode", this.ObjectTableName, true)
    }


    ngOnInit() {
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((response: any) => {
            this._entityResourceService.getEntityResourceByTableName("Customs.CustomBank").subscribe((response: any) => {
            });
            this._isNew = true;

        });

    }

    SetWindowArgs(_WindowArgs) {

        if (!AppTool.IsNullOrEmpty(_WindowArgs)) {
            this.isWindowMode = true;
        }
        this.CurrentSession.StartBusyIndicatorLoading();
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((response: any) => {
            this._entityResourceService.getEntityResourceByTableName("Customs.CustomBank").subscribe((response: any) => {
                this._CustomBankListService
                    .get
                    (_WindowArgs)
                    .subscribe((rsp: any) => {
                        this._isNew = false;
                        this.EntityPM = rsp.Result;
                        this.EntityPM.CustomBanksCards.forEach((card, index) => {

                            var filters = new ApiQueryFilters(true);
                            filters.addAdditionalFilter('Id', card.CardId, null, null, "InListExact", false, false, false, "string");

                            new CardListService().getByFilters(filters).subscribe((response: any) => {
                                this.EntityPM.CustomBanksCards[index].CardName = response.Result[index].ENGLISHNAME;
                                this.EntityPM.CustomBanksCards[index].CardId = response.Result[index].VATNUMBER;
                            }
                            );

                            this.EntityPM.CustomBanksCards ? this.CustomBankCardItems = new ObservableCollection(this.EntityPM.CustomBanksCards) : this.CustomBankCardItems = new ObservableCollection([]);
                            this.CurrentSession.StopBusyIndicator();
                        });
                    });
            });

        }
        )
    }




    onInputChange(event, ObjectFieldName) {
        this.EntityPM[ObjectFieldName] = event;
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
        console.log(newValue)
        this.EntityPM.CustomBanksCards = newValue;
    }


    valid = new ClassLevelValidator();

    onCellSelected(event) {
        console.log(event)
    }
    IsDisplayOnly = false;

    OkButtonClicked() {


        var errors = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (errors.length > 0) {
            this.ValidationErrorsList = [];
            this.ValidationErrorsList = errors;
        } else {
            if (this._isNew) {
                this._CustomBankPMService.insert(this.EntityPM).subscribe((myResult: any) => {

                    var mm: ServiceResponse = myResult;
                    if (!mm.HasError) {

                        this.CurrentSession.CloseCurrentWindowEmit("ok");

                    }
                    else {
                        this.ValidationErrorsList = mm.ErrorsArray;
                        this.CurrentSession.StopBusyIndicator();
                    }
                });

            } else {

                this._CustomBankPMService.update(this.EntityPM).subscribe((myResult: any) => {

                    var mm: ServiceResponse = myResult;
                    if (!mm.HasError) {

                        this.CurrentSession.CloseCurrentWindowEmit("ok");

                    }
                    else {
                        this.ValidationErrorsList = mm.ErrorsArray;
                        this.CurrentSession.StopBusyIndicator();
                    }
                });

            }
        }

    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }


}
