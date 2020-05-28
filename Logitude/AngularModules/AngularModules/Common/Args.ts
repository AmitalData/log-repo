import {MAWBStackPM} from './EntityPMs/MAWBStackPM';
import {CustomerPM} from './EntityPMs/CustomerPM';
import {Injectable} from '@angular/core';

export class CitySelectionArgs {
    public IsCitySelected: boolean = false;
    public CityName: string = null;
    public CityLocalName: string = null;
    public CountryId: string = null;
    public StateId: string = null;
    constructor(myCountryId: string) {
        this.CountryId = myCountryId;
    }
}

export class GetStackWindowArgs {
    public CardId: string;
    public CardName: string;
    public ShipperId: string;
    public SelectedStack: MAWBStackPM;
    public HasAssignedStocks: boolean;
    public SelectedFBLStock: any;
}

export class GetAccountingSystemWindowArgs {
    constructor() {
        this.ReceivableCard = false;
        this.PayableCard = false;
        this.CardName = "";
        this.LogitudeCardName = "";
        this.SearchField = "";
    }
    public CardId: string;
    public CardName: string;
    public SelectedEntity: any;
    public SearchField: string;
    public LogitudeCardName: string;
    public ReceivableCard: boolean;
    public PayableCard: boolean;
}

export class CustomerAccountManagerByProductSplitComponentARGS {
    public EntityPM: any;
    public Parent: any;    
}

export class NewGLAccountArgs {
    public ChartOfAccountType: string;
    public DisplayNo: string;
    public LocalName: string;
    public EnglishName: string;
    public AccountType: string;
    public CardId: string;
    public RevenueExpenseType: string;
    public EntityName: string;
    public PartnerType: string;
}

export class CustomerActivationArgs {
    public EntityPM: CustomerPM;
    public Message: string;
    public ActivatedFromQuoteSide: boolean;
    public ActivatedPartnerType: string;
}
