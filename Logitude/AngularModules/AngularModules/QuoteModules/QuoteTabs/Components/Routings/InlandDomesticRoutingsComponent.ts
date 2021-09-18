import {Component, OnDestroy} from '@angular/core';
import {QuotePM} from '../../../../Quote/EntityPMs/QuotePM';
import {QuoteDomainService} from '../../../../Quote/Services/QuoteDomainService';
import {QuoteUtilities} from '../../../../Quote/Utilities/QuoteUtilities';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {AppTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {AddressList} from '../../../../Common/EntityLists/AddressList';
import {AddressPM} from '../../../../Common/EntityPMs/AddressPM';
import {AddressListService} from '../../../../Common/Services/StandardLists/AddressListService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import { PortList } from '../../../../Common/EntityLists/PortList';
import { PortListService } from '../../../../Common/Services/StandardLists/PortListService';
import { CardListService } from '../../../../Common/Services/StandardLists/CardListService';
import { CitySelectionArgs } from '../../../../Common/Args';
import { CardList } from '../../../../Common/EntityLists/CardList';
import { CountryList } from '../../../../Common/EntityLists/CountryList';

@Component({
    selector: 'InlandDomesticRoutingsComponent',
    templateUrl: './InlandDomesticRoutingsComponent.html',
})

export class InlandDomesticRoutingsComponent extends BaseComponent implements OnDestroy {
    public EntityPM: QuotePM = null;
    public ObjectTableName: string = "Quotes";
    public DataContext = this;
    public IsSubjectVisible: boolean = false;
    public CardLOVDependencyProperty1: string = null;
    constructor(private entityArgs: EntityArgs) {
        super();
        this.IsSubjectVisible = SessionLocator.TenantPM.IsQuoteSubjectEdited ? true : false;
        this.Listen();
    }

    InitTab(entityPM: QuotePM, tableName: string) {
        this.EntityPM = entityPM;
        this.ObjectTableName = tableName;
        this.CardLOVDependencyProperty1 = "CS,PO,WH";

        if (SessionLocator.TenantPM.AllowAgentInCustomersLOV) {
            this.CardLOVDependencyProperty1 = "CS,PO,AG,WH";
        }

        this.InitializePartners();
        this.SetLabels();
        this.SetUIProperties();
    }

    private myCardListService: CardListService;
    private myAddressListService: AddressListService;
    private myPortListService: PortListService;
    InitializePartners() {
        this.myCardListService = new CardListService();
        this.myAddressListService = new AddressListService();
        this.myPortListService = new PortListService();

        this.FromAddressList = null;
        this.ToAddressList = null;

        if (!AppTool.IsNullOrEmpty(this.FromPartnerId) && !AppTool.IsNullOrEmpty(this.EntityPM.FromPartnerAddressId)) {
            this.myAddressListService.getSingle(this.FromPartnerAddressId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.FromAddressList = myResponse.Result;
                }
            });
        }

        if (!AppTool.IsNullOrEmpty(this.ToPartnerId) && !AppTool.IsNullOrEmpty(this.EntityPM.ToPartnerAddressId)) {
            this.myAddressListService.getSingle(this.ToPartnerAddressId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.ToAddressList = myResponse.Result;
                }
            });
        }
    }

    private TabSelectedEvent: any = null;
    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null; 
    private Listen() {
        if (this.entityArgs.EditComponent) {

            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.SetUIProperties();
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.SetUIProperties();
                }
            });

            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe((tabCode: string) => {
                if (tabCode == "QTRT") {
                    this.GetFromPartnerData();
                    this.GetToPartnerData();
                }
            });
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.TabSelectedEvent);
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    public IsQuoteEditEnabled: boolean = false;
    SetUIProperties() {
        this.IsQuoteEditEnabled = QuoteUtilities.IsQuoteEditEnabled(this.EntityPM);

        if (this.IsQuoteEditEnabled) {
            this.SetUIProperties_From();
            this.SetUIProperties_To();
        }

        else {
            this.UIProperties.SetEnabled("FromPartnerId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("FromPartnerAddressId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("FromPortId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("MainCarriageFromPortAddress", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("InlandDomesticFromCity", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("InlandDomesticFromCountryId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("InlandDomesticFromZipCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ToPartnerId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ToPartnerAddressId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ToPortId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("MainCarriageToPortAddress", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("InlandDomesticToCity", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("InlandDomesticToCountryId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("InlandDomesticToZipCode", this.ObjectTableName, false);
        }
        
        this.UIProperties.SetEnabled("Subject", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("ETD", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("ETA", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("MainCarriageCarrierId", this.ObjectTableName, this.IsQuoteEditEnabled);
    }
    SetUIProperties_From() {
        switch (this.InlandDomesticFromTypeCode) {
            case "PART": {
                this.UIProperties.SetRequired("FromPartnerId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.FromPartnerId) ? true : false);
                this.UIProperties.SetEnabled("FromPartnerId", this.ObjectTableName, this.IsQuoteEditEnabled);

                var isAddressIdEnabled = false;
                if (this.IsQuoteEditEnabled) {
                    if (!AppTool.IsNullOrEmpty(this.FromPartnerId)) {
                        isAddressIdEnabled = true;
                    }
                }

                this.UIProperties.SetEnabled("FromPartnerAddressId", this.ObjectTableName, isAddressIdEnabled);
                break;
            }

            case "PORT": {
                this.UIProperties.SetRequired("FromPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.FromPortId) ? true : false);
                this.UIProperties.SetEnabled("MainCarriageFromPortAddress", this.ObjectTableName, false);
                break;
            }

            case "CASL": {
                this.UIProperties.SetRequired("InlandDomesticFromCity", this.ObjectTableName, AppTool.IsNullOrEmpty(this.InlandDomesticFromCity) && AppTool.IsNullOrEmpty(this.InlandDomesticFromZipCode) ? true : false);
                this.UIProperties.SetRequired("InlandDomesticFromCountryId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.InlandDomesticFromCountryId) ? true : false);
                break;
            }
        }
    }
    SetUIProperties_To() {
        switch (this.InlandDomesticToTypeCode) {
            case "PART": {
                this.UIProperties.SetRequired("ToPartnerId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.ToPartnerId) ? true : false);
                this.UIProperties.SetEnabled("ToPartnerId", this.ObjectTableName, this.IsQuoteEditEnabled);

                var isAddressIdEnabled = false;
                if (this.IsQuoteEditEnabled) {
                    if (!AppTool.IsNullOrEmpty(this.ToPartnerId)) {
                        isAddressIdEnabled = true;
                    }
                }

                this.UIProperties.SetEnabled("ToPartnerAddressId", this.ObjectTableName, isAddressIdEnabled);
                break;
            }

            case "PORT": {
                this.UIProperties.SetRequired("ToPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.ToPortId) ? true : false);
                this.UIProperties.SetEnabled("MainCarriageToPortAddress", this.ObjectTableName, false);
                break;
            }

            case "CASL": {
                this.UIProperties.SetRequired("InlandDomesticToCity", this.ObjectTableName, AppTool.IsNullOrEmpty(this.InlandDomesticToCity) && AppTool.IsNullOrEmpty(this.InlandDomesticToZipCode) ? true : false);
                this.UIProperties.SetRequired("InlandDomesticToCountryId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.InlandDomesticToCountryId) ? true : false);
                break;
            }
        }
    }

    // Subject
    get IsSubjectEdited() { return this.EntityPM.IsSubjectEdited; }
    set IsSubjectEdited(value: boolean) {
        if (this.EntityPM.IsSubjectEdited != value) {
            this.EntityPM.IsSubjectEdited = value;
        }
    }

    get Subject() { return this.EntityPM.Subject; }
    set Subject(newValue: string) {
        if (this.EntityPM.Subject != newValue) {
            this.EntityPM.Subject = newValue;
            this.IsSubjectEdited = true;
        }
    }

    ResetSubjectEdited() {
        this.IsSubjectEdited = false;
        this.GetSubjectField();
    }

    GetSubjectField() {
        if (SessionLocator.TenantPM.IsQuoteSubjectEdited) {
            var myQuoteDomainService = new QuoteDomainService();

            myQuoteDomainService.ComputeQuoteAutomaticSubject(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.EntityPM.Subject = myResponse.Result;                    
                }
            });
        }
    }

    //////////// Dates ////////////
    get ETD() { return this.EntityPM.ETD; }
    set ETD(newValue: Date) {
        if (this.EntityPM.ETD != newValue) {
            this.EntityPM.ETD = newValue;
        }
    }

    get ETA() { return this.EntityPM.ETA; }
    set ETA(newValue: Date) {
        if (this.EntityPM.ETA != newValue) {
            this.EntityPM.ETA = newValue;
        }
    }

    //////////// Carrier ////////////
    public CarrierLabel: string;
    public CarrierDependencyProperty1: string;
    private SetLabels() {
        this.CarrierLabel = "Quote.S.NewQuote.Carrier";

        switch (this.EntityPM.TransportModeId) {
            case "A": {
                this.CarrierLabel = "Quote.S.NewQuote.Airline";
                this.CarrierDependencyProperty1 = "AL";
                break;
            }

            case "O": {
                this.CarrierLabel = "Quote.S.NewQuote.Shippingline";
                this.CarrierDependencyProperty1 = "SL";
                break;
            }

            case "I": {
                this.CarrierLabel = "Quote.S.NewQuote.Trucker";
                this.CarrierDependencyProperty1 = "TR";
                break;
            }
        }
    }

    get TransportModeId() { return this.EntityPM.TransportModeId; }

    get MainCarriageCarrierId() { return this.EntityPM.MainCarriageCarrierId; }
    set MainCarriageCarrierId(newValue: string) {
        if (this.EntityPM.MainCarriageCarrierId != newValue) {
            this.EntityPM.MainCarriageCarrierId = newValue;
        }
    }
    
    //////////// From Partner ////////////
    get InlandDomesticFromTypeCode() { return this.EntityPM.InlandDomesticFromTypeCode; }
    set InlandDomesticFromTypeCode(value: string) {
        if (this.EntityPM.InlandDomesticFromTypeCode != value) {
            this.EntityPM.InlandDomesticFromTypeCode = value;

            this.FromPartnerId = null;
            this.FromPartnerAddressId = null;
            this.FromPortId = null;
            this.InlandDomesticFromCity = null;
            this.InlandDomesticFromZipCode = null;
            this.InlandDomesticFromCountryId = null;
            this.MainCarriageFromPortAddress = null;
            this.fromAddressList = null;
            this.SetUIProperties_From();
        }
    }

    get FromPartnerId() { return this.EntityPM.FromPartnerId; }
    set FromPartnerId(newValue: string) {
        if (this.EntityPM.FromPartnerId != newValue) {
            this.EntityPM.FromPartnerId = newValue;

            this.SetUIProperties_From();

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.FromPartnerAddressId = null;
            }

            else {
                this.myCardListService.getSingle(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CardList = myResponse.Result;
                        if (list) {
                            this.FromPartnerAddressId = list.MainAddressId;
                        }
                    }
                });
            }
        }
    }

    get FromPartnerAddressId() { return this.EntityPM.FromPartnerAddressId; }
    set FromPartnerAddressId(newValue: string) {
        if (this.EntityPM.FromPartnerAddressId != newValue) {
            this.EntityPM.FromPartnerAddressId = newValue;
            this.GetFromPartnerData();
        }
    }

    get FromPartnerName() { return this.EntityPM.FromPartnerName; }
    set FromPartnerName(newValue: string) {
        if (this.EntityPM.FromPartnerName != newValue) {
            this.EntityPM.FromPartnerName = newValue;
        }
    }

    private fromAddressList: AddressList;
    get FromAddressList() { return this.fromAddressList; }
    set FromAddressList(newValue: AddressList) {
        if (this.fromAddressList != newValue) {
            this.fromAddressList = newValue;            
        }
    }

    get FromPortId() { return this.EntityPM.FromPortId; }
    set FromPortId(value: string) {
        if (this.EntityPM.FromPortId != value) {
            this.EntityPM.FromPortId = value;            
            this.EntityPM.FromCountryCode = null;
            this.EntityPM.FromPortCountry = null;
            this.EntityPM.FromCountryName = null;

            this.SetUIProperties_From();

            if (AppTool.IsNullOrEmpty(value)) {
                this.MainCarriageFromPortAddress = null;
            }

            else {
                this.myPortListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PortList = myResponse.Result;
                        if (list) {
                            this.MainCarriageFromPortAddress = "Port Of: " + list.EnglishName;
                            this.EntityPM.FromCountryCode = list.CountryCode;                            
                            this.EntityPM.FromPortCountry = list.CountryName;
                            this.EntityPM.FromCountryName = list.CountryName;
                        }
                    }
                });
            }
        }
    }

    get MainCarriageFromPortAddress() { return this.EntityPM.MainCarriageFromPortAddress; }
    set MainCarriageFromPortAddress(value: string) {
        if (this.EntityPM.MainCarriageFromPortAddress != value) {
            this.EntityPM.MainCarriageFromPortAddress = value;
        }
    }

    get InlandDomesticFromCity() { return this.EntityPM.InlandDomesticFromCity; }
    set InlandDomesticFromCity(value: string) {
        if (this.EntityPM.InlandDomesticFromCity != value) {
            this.EntityPM.InlandDomesticFromCity = value;
            this.SetUIProperties_From();
        }
    }

    get InlandDomesticFromZipCode() { return this.EntityPM.InlandDomesticFromZipCode; }
    set InlandDomesticFromZipCode(value: string) {
        if (this.EntityPM.InlandDomesticFromZipCode != value) {
            this.EntityPM.InlandDomesticFromZipCode = value;
            this.SetUIProperties_From();
        }
    }

    get InlandDomesticFromCountryId() { return this.EntityPM.InlandDomesticFromCountryId; }
    set InlandDomesticFromCountryId(value: string) {
        if (this.EntityPM.InlandDomesticFromCountryId != value) {
            this.EntityPM.InlandDomesticFromCountryId = value;
            this.SetUIProperties_From();
        }
    }

    private inlandDomesticFromCountry: CountryList;
    get InlandDomesticFromCountry() { return this.inlandDomesticFromCountry; }
    set InlandDomesticFromCountry(value: CountryList) {
        if (this.inlandDomesticFromCountry != value) {
            this.inlandDomesticFromCountry = value;
            this.EntityPM.FromCountryCode = null;
            this.EntityPM.FromPortCountry = null;
            this.EntityPM.FromCountryName = null;

            if (value != null) {
                this.EntityPM.FromCountryCode = value.Code;
                this.EntityPM.FromPortCountry = value.EnglishName;
                this.EntityPM.FromCountryName = value.EnglishName;
            }
        }
    }

    private GetFromPartnerData() {
        if (!AppTool.IsNullOrEmpty(this.FromPartnerId)) {
            if (AppTool.IsNullOrEmpty(this.EntityPM.FromPartnerAddressId)) {
                this.FromAddressList = null;
                this.EntityPM.FromCountryCode = null;
                this.EntityPM.FromPortCountry = null;
                this.EntityPM.FromCountryName = null;
            }

            else {
                this.myAddressListService.getSingle(this.FromPartnerAddressId).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        this.FromAddressList = myResponse.Result;
                        this.EntityPM.FromCountryCode = this.FromAddressList.CountryCode;
                        this.EntityPM.FromPortCountry = this.FromAddressList.CountryName;
                        this.EntityPM.FromCountryName = this.FromAddressList.CountryName;
                    }
                });
            }
        }
    }

    //////////// To Partner ////////////
    get InlandDomesticToTypeCode() { return this.EntityPM.InlandDomesticToTypeCode; }
    set InlandDomesticToTypeCode(value: string) {
        if (this.EntityPM.InlandDomesticToTypeCode != value) {
            this.EntityPM.InlandDomesticToTypeCode = value;

            this.ToPartnerId = null;
            this.ToPartnerAddressId = null;
            this.ToPortId = null;
            this.InlandDomesticToCity = null;
            this.InlandDomesticToZipCode = null;
            this.InlandDomesticToCountryId = null;
            this.MainCarriageToPortAddress = null;
            this.toAddressList = null;
            this.SetUIProperties_To();
        }
    }

    get ToPartnerId() { return this.EntityPM.ToPartnerId; }
    set ToPartnerId(newValue: string) {
        if (this.EntityPM.ToPartnerId != newValue) {
            this.EntityPM.ToPartnerId = newValue;

            this.SetUIProperties_To();

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.ToPartnerAddressId = null;
            }

            else {
                this.myCardListService.getSingle(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CardList = myResponse.Result;
                        if (list) {
                            this.ToPartnerAddressId = list.MainAddressId;
                        }
                    }
                });
            }
        }
    }

    get ToPartnerAddressId() { return this.EntityPM.ToPartnerAddressId; }
    set ToPartnerAddressId(newValue: string) {
        if (this.EntityPM.ToPartnerAddressId != newValue) {
            this.EntityPM.ToPartnerAddressId = newValue;
            this.GetToPartnerData();
        }
    }

    get ToPartnerName() { return this.EntityPM.ToPartnerName; }
    set ToPartnerName(newValue: string) {
        if (this.EntityPM.ToPartnerName != newValue) {
            this.EntityPM.ToPartnerName = newValue;
        }
    }

    private toAddressList: AddressList;
    get ToAddressList() { return this.toAddressList; }
    set ToAddressList(newValue: AddressList) {
        if (this.toAddressList != newValue) {
            this.toAddressList = newValue;            
        }
    }

    get ToPortId() { return this.EntityPM.ToPortId; }
    set ToPortId(value: string) {
        if (this.EntityPM.ToPortId != value) {
            this.EntityPM.ToPortId = value;
            this.EntityPM.ToCountryCode = null;
            this.EntityPM.ToPortCountry = null;
            this.EntityPM.ToCountryName = null;

            this.SetUIProperties_To();

            if (AppTool.IsNullOrEmpty(value)) {
                this.MainCarriageToPortAddress = null;
            }

            else {
                this.myPortListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PortList = myResponse.Result;
                        if (list) {
                            this.MainCarriageToPortAddress = "Port Of: " + list.EnglishName;
                            this.EntityPM.ToCountryCode = list.CountryCode;
                            this.EntityPM.ToPortCountry = list.CountryName;
                            this.EntityPM.ToCountryName = list.CountryName;
                        }
                    }
                });
            }
        }
    }

    get MainCarriageToPortAddress() { return this.EntityPM.MainCarriageToPortAddress; }
    set MainCarriageToPortAddress(value: string) {
        if (this.EntityPM.MainCarriageToPortAddress != value) {
            this.EntityPM.MainCarriageToPortAddress = value;
        }
    }

    get InlandDomesticToCity() { return this.EntityPM.InlandDomesticToCity; }
    set InlandDomesticToCity(value: string) {
        if (this.EntityPM.InlandDomesticToCity != value) {
            this.EntityPM.InlandDomesticToCity = value;
            this.SetUIProperties_To();
        }
    }

    get InlandDomesticToZipCode() { return this.EntityPM.InlandDomesticToZipCode; }
    set InlandDomesticToZipCode(value: string) {
        if (this.EntityPM.InlandDomesticToZipCode != value) {
            this.EntityPM.InlandDomesticToZipCode = value;
            this.SetUIProperties_To();
        }
    }

    get InlandDomesticToCountryId() { return this.EntityPM.InlandDomesticToCountryId; }
    set InlandDomesticToCountryId(value: string) {
        if (this.EntityPM.InlandDomesticToCountryId != value) {
            this.EntityPM.InlandDomesticToCountryId = value;
            this.SetUIProperties_To();
        }
    }

    private inlandDomesticToCountry: CountryList;
    get InlandDomesticToCountry() { return this.inlandDomesticToCountry; }
    set InlandDomesticToCountry(value: CountryList) {
        if (this.inlandDomesticToCountry != value) {
            this.inlandDomesticToCountry = value;
            this.EntityPM.ToCountryCode = null;
            this.EntityPM.ToPortCountry = null;
            this.EntityPM.ToCountryName = null;

            if (value != null) {
                this.EntityPM.ToCountryCode = value.Code;
                this.EntityPM.ToPortCountry = value.EnglishName;
                this.EntityPM.ToCountryName = value.EnglishName;
            }
        }
    }

    private GetToPartnerData() {
        if (!AppTool.IsNullOrEmpty(this.ToPartnerId)) {
            if (AppTool.IsNullOrEmpty(this.EntityPM.ToPartnerAddressId)) {
                this.ToAddressList = null;
                this.EntityPM.ToCountryCode = null;
                this.EntityPM.ToPortCountry = null;
                this.EntityPM.ToCountryName = null;
            }

            else {
                this.myAddressListService.getSingle(this.ToPartnerAddressId).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        this.ToAddressList = myResponse.Result;
                        this.EntityPM.ToCountryCode = this.ToAddressList.CountryCode;
                        this.EntityPM.ToPortCountry = this.ToAddressList.CountryName;
                        this.EntityPM.ToCountryName = this.ToAddressList.CountryName;
                    }
                });
            }
        }
    }

    SelectCityCommand(myAddressCode: string) {
        var mySourceCountryId: string = myAddressCode == "F" ? this.InlandDomesticFromCountryId : this.InlandDomesticToCountryId;

        var args = new CitySelectionArgs(mySourceCountryId);
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Select City";
        logWindow.WindowArgs = args;
        logWindow.Show("./CommonModules/CommonOthers/Components/CitySelection/CitySelectionComponent");
        logWindow.WindowClosed.subscribe(($event: any) => {
            if (args.IsCitySelected) {
                if (myAddressCode == "F") {
                    this.InlandDomesticFromCity = args.CityName;
                    this.InlandDomesticFromCountryId = args.CountryId;
                }

                else if (myAddressCode == "T") {
                    this.InlandDomesticToCity = args.CityName;
                    this.InlandDomesticToCountryId = args.CountryId;
                }
            }
        });
    }

    EditAddressClicked(myAddressCode: string) {
        var myAddressId: string = null;
        var myPartnerId: string = null;

        switch (myAddressCode) {
            case "F": {
                if (!AppTool.IsNullOrEmpty(this.FromPartnerId)) {
                    myPartnerId = this.FromPartnerId;
                    myAddressId = this.FromPartnerAddressId;                    
                }

                break;
            }

            case "T": {
                if (!AppTool.IsNullOrEmpty(this.ToPartnerId)) {
                    myPartnerId = this.ToPartnerId;
                    myAddressId = this.ToPartnerAddressId;
                }

                break;
            }
        }

        if (!AppTool.IsNullOrEmpty(myAddressId) && !AppTool.IsNullOrEmpty(myPartnerId)) {
            var logeWindow = new LogitudeWindow();
            logeWindow.Width = 630;
            logeWindow.Height = 430;
            logeWindow.Title = "Edit Address";
            logeWindow.WindowArgs = { EntityId: myAddressId };
            logeWindow.Show("./CommonPartners/Components/AddEdit/AddEditPartnerAddressComponent");
            logeWindow.WindowClosed.subscribe(s => {
                if (s) {
                    switch (myAddressCode) {
                        case "F": {
                            this.GetFromPartnerData();
                            break;
                        }

                        case "T": {
                            this.GetToPartnerData();
                            break;
                        }
                    }
                }
            });
        }
    }
    AddAddressClicked(myAddressCode: string) {
        var entityPM: AddressPM = null;
        var myPartnerId: string = null;

        switch (myAddressCode) {
            case "F": {
                if (!AppTool.IsNullOrEmpty(this.FromPartnerId)) {
                    myPartnerId = this.FromPartnerId;

                    entityPM = new AddressPM();
                    entityPM.Tenant = SessionLocator.Tenant;
                    entityPM.AddressTypeId = "O";
                    entityPM.CardId = myPartnerId;
                }

                break;
            }

            case "T": {
                if (!AppTool.IsNullOrEmpty(this.ToPartnerId)) {
                    myPartnerId = this.ToPartnerId;

                    entityPM = new AddressPM();
                    entityPM.Tenant = SessionLocator.Tenant;
                    entityPM.AddressTypeId = "O";
                    entityPM.CardId = myPartnerId;
                }

                break;
            }                
        }

        if (entityPM != null && !AppTool.IsNullOrEmpty(myPartnerId)) {
            var logeWindow = new LogitudeWindow();
            logeWindow.Width = 630;
            logeWindow.Height = 430;
            logeWindow.Title = "Add Address";
            logeWindow.WindowArgs = { EntityPM: entityPM };
            logeWindow.Show("./CommonPartners/Components/AddEdit/AddEditPartnerAddressComponent");
            logeWindow.WindowClosed.subscribe(s => {
                if (s) {
                    switch (myAddressCode) {
                        case "P": {
                            this.FromPartnerAddressId = null;
                            this.FromPartnerAddressId = entityPM.Id;
                            break;
                        }

                        case "D": {
                            this.ToPartnerAddressId = null;
                            this.ToPartnerAddressId = entityPM.Id;
                            break;
                        }
                    }
                }
            });
        }
    }    
}
