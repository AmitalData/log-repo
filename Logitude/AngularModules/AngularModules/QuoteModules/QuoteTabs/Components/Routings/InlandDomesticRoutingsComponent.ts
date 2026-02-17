import {Component, OnDestroy} from '@angular/core';
import {QuotePM} from '../../../../Quote/EntityPMs/QuotePM';
import {QuoteDomainService, QuoteSubjectArgs} from '../../../../Quote/Services/QuoteDomainService';
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

@Component({
    selector: 'InlandDomesticRoutingsComponent',
    moduleId: module.id,
    templateUrl: './InlandDomesticRoutingsComponent.html',
})

export class InlandDomesticRoutingsComponent extends BaseComponent implements OnDestroy {
    public EntityPM: QuotePM = null;
    public ObjectTableName: string = "Quotes";
    public DataContext = this;
    public IsSubjectVisible: boolean = false;
    constructor(private entityArgs: EntityArgs) {
        super();
        this.IsSubjectVisible = SessionLocator.TenantPM.IsQuoteSubjectEdited ? true : false;
        this.Listen();
    }

    InitTab(entityPM: QuotePM, tableName: string) {
        this.EntityPM = entityPM;
        this.ObjectTableName = tableName;
        this.SetLabels();
        this.SetUIProperties();
        this.GetFromPartnerData();
        this.GetToPartnerData();
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

        this.UIProperties.SetEnabled("Subject", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("ETD", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("ETA", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("MainCarriageCarrierId", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("FromPartnerAddressId", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("ToPartnerAddressId", this.ObjectTableName, this.IsQuoteEditEnabled);
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
                    var myArgs: QuoteSubjectArgs = myResponse.Result;

                    if (myArgs != null) {
                        this.EntityPM.Subject = myArgs.Subject;
                    }
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
    get FromPartnerId() { return this.EntityPM.FromPartnerId; }
    set FromPartnerId(newValue: string) {
        if (this.EntityPM.FromPartnerId != newValue) {
            this.EntityPM.FromPartnerId = newValue;
        }
    }

    get FromPartnerAddressId() { return this.EntityPM.FromPartnerAddressId; }
    set FromPartnerAddressId(newValue: string) {
        if (this.EntityPM.FromPartnerAddressId != newValue) {
            this.EntityPM.FromPartnerAddressId = newValue;

            this.GetFromPartnerData();
            this.SetUIProperties();
        }
    }

    get FromPartnerName() { return this.EntityPM.FromPartnerName; }
    set FromPartnerName(newValue: string) {
        if (this.EntityPM.FromPartnerName != newValue) {
            this.EntityPM.FromPartnerName = newValue;
        }
    }

    public FromAddressList: AddressList;
    private GetFromPartnerData() {
        if (!AppTool.IsNullOrEmpty(this.FromPartnerId)) {
            if (AppTool.IsNullOrEmpty(this.EntityPM.FromPartnerAddressId)) {
                this.FromAddressList = null;
            }

            else {
                var myService: AddressListService = new AddressListService();
                myService.getSingle(this.FromPartnerAddressId).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        this.FromAddressList = myResponse.Result;
                    }
                });
            }
        }
    }

    get IsEditFromAddressEnabled() {
        var myResult = false;

        if (this.IsQuoteEditEnabled) {
            if (!AppTool.IsNullOrEmpty(this.FromPartnerAddressId)) {
                myResult = true;
            }
        }

        return myResult;
    }

    //////////// To Partner ////////////
    get ToPartnerId() { return this.EntityPM.ToPartnerId; }
    set ToPartnerId(newValue: string) {
        if (this.EntityPM.ToPartnerId != newValue) {
            this.EntityPM.ToPartnerId = newValue;
        }
    }

    get ToPartnerAddressId() { return this.EntityPM.ToPartnerAddressId; }
    set ToPartnerAddressId(newValue: string) {
        if (this.EntityPM.ToPartnerAddressId != newValue) {
            this.EntityPM.ToPartnerAddressId = newValue;

            this.GetToPartnerData();
            this.SetUIProperties();
        }
    }

    get ToPartnerName() { return this.EntityPM.ToPartnerName; }
    set ToPartnerName(newValue: string) {
        if (this.EntityPM.ToPartnerName != newValue) {
            this.EntityPM.ToPartnerName = newValue;
        }
    }

    public ToAddressList: AddressList;
    private GetToPartnerData() {
        if (!AppTool.IsNullOrEmpty(this.ToPartnerId)) {
            if (AppTool.IsNullOrEmpty(this.EntityPM.ToPartnerAddressId)) {
                this.ToAddressList = null;
            }

            else {
                var myService: AddressListService = new AddressListService();
                myService.getSingle(this.ToPartnerAddressId).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        this.ToAddressList = myResponse.Result;
                    }
                });
            }
        }
    }

    get IsEditToAddressEnabled() {
        var myResult = false;

        if (this.IsQuoteEditEnabled) {
            if (!AppTool.IsNullOrEmpty(this.ToPartnerAddressId)) {
                myResult = true;
            }
        }

        return myResult;
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
            logeWindow.WindowArgs = { EntityId: myAddressId, CardId: myPartnerId };
            logeWindow.Show("./QuoteModules/QuoteTabs/Components/Routings/RoutingsAddEditAddressComponent");
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
            logeWindow.WindowArgs = { EntityPM: entityPM, CardId: myPartnerId };
            logeWindow.Show("./QuoteModules/QuoteTabs/Components/Routings/RoutingsAddEditAddressComponent");
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