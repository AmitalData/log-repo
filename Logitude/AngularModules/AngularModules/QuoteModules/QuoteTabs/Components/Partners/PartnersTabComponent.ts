import {Component, OnInit, OnDestroy} from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {QuotePM} from '../../../../Quote/EntityPMs/QuotePM';
import {QuoteUtilities} from '../../../../Quote/../Quote/Utilities/QuoteUtilities';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {CardList} from '../../../../Common/EntityLists/CardList';
import {AddressList} from '../../../../Common/EntityLists/AddressList';
import {ContactList} from '../../../../Common/EntityLists/ContactList';
import {CardListService} from '../../../../Common/Services/StandardLists/CardListService';
import {AddressListService} from '../../../../Common/Services/StandardLists/AddressListService';
import {ContactListService} from '../../../../Common/Services/StandardLists/ContactListService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {NewEntityArgs} from '../../../../Infrastructure/Args';
import {AppTool} from '../../../../Infrastructure/Tools';
import { ContactInputTemplateArgs } from '../../../../CommonModules/CommonPartners/Components/Templates/ContactInputTemplate';

@Component({
    selector: 'PartnersTabComponent',
    
    templateUrl: './PartnersTabComponent.html',
})

export class PartnersTabComponent implements OnInit, OnDestroy {
    public EntityPM: QuotePM;
    public ObjectTableName: string = "Quote";
    public ItemsCollection: PartnerItem[];
    public IsInlandDomestic: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = this.entityArgs.EntityPM;
        this.InitializeServices();
        this.Listen();
    }

    private TabSelectedEvent: any = null;
    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null; 
    private TabChangedEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent) {

            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.UpdateScreen();
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.UpdateScreen();
                }
            });

            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe((tabCode: string) => {
                if (tabCode == "QTPA") {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    this.UpdateScreen();
                }
            });

            this.TabChangedEvent = this.entityArgs.EditComponent.TabChanged.subscribe((tabCode: string) => {
                this.IsUpdateSalesmanVisible = false;
            });
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.TabSelectedEvent);
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
        AppTool.KillEventEmitter(this.TabChangedEvent);
    }

    public CardListService: CardListService;
    public AddressListService: AddressListService;
    public ContactListService: ContactListService;
    InitializeServices() {
        this.CardListService = new CardListService();
        this.AddressListService = new AddressListService();
        this.ContactListService = new ContactListService();
    }

    ngOnInit() {
        if (this.EntityPM != null) {
            this.UpdateScreen();
        }
    }

    UpdateScreen() {
        this.SetUIProperties();
        this.BuildItemsCollection();
        this.SetAddButtonsIsDisabled();
    }

    public IsEditingEnabled: boolean = true;
    SetUIProperties() {
        this.IsEditingEnabled = QuoteUtilities.IsQuoteEditEnabled(this.EntityPM);
    }

    private BuildItemsCollection() {
        this.ItemsCollection = [];

        if (this.EntityPM.ShipperId != null) {
            this.ItemsCollection.push(new PartnerItem(this, "SHIPR"));
        }

        if (this.EntityPM.ConsigneeId != null) {
            this.ItemsCollection.push(new PartnerItem(this, "CONSI"));
        }

        if (this.EntityPM.AgentId != null) {
            this.ItemsCollection.push(new PartnerItem(this, "AGENT"));
        }

        if (this.EntityPM.NotifyId != null) {
            this.ItemsCollection.push(new PartnerItem(this, "NOTFY"));
        }
    }

    public IsAddDisabled_SHIPR: boolean = false;
    public IsAddDisabled_CONSI: boolean = false;
    public IsAddDisabled_AGENT: boolean = false;
    public IsAddDisabled_NOTFY: boolean = false;
    SetAddButtonsIsDisabled() {
        this.IsAddDisabled_SHIPR = this.EntityPM.ShipperId == null ? false : true;
        this.IsAddDisabled_CONSI = this.EntityPM.ConsigneeId == null ? false : true;
        this.IsAddDisabled_AGENT = this.EntityPM.AgentId == null ? false : true;
        this.IsAddDisabled_NOTFY = this.EntityPM.NotifyId == null ? false : true;
    }

    AddPartner(myCode: string) {
        var newPartnerItem: PartnerItem = new PartnerItem(this, myCode);
        newPartnerItem.IsNewAdded = true;

        var myWindowTitle: string = TextCodeTranslator.Translate("Quote.S.Partners.Add" + newPartnerItem.FullCode);        
        this.RunAddEditPartner(newPartnerItem, myWindowTitle);
    }
    EditPartner(myPartnerItem: PartnerItem) {
        myPartnerItem.IsNewAdded = false;
        myPartnerItem.CopyOriginData();

        var myWindowTitle: string = TextCodeTranslator.Translate("Quote.S.Partners.Edit" + myPartnerItem.FullCode); 
        this.RunAddEditPartner(myPartnerItem, myWindowTitle);
    }
    RunAddEditPartner(myPartnerItem: PartnerItem, myWindowTitle: string) {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = myWindowTitle;
        logitudeWindow.DataContext = myPartnerItem;
        logitudeWindow.Show("./QuoteModules/QuoteTabs/Components/Partners/AddEditPartnerComponent");
        logitudeWindow.WindowClosed.subscribe(s => {
            if (s == "OK") {
                this.BuildItemsCollection();
                this.SetAddButtonsIsDisabled();
            }
        });
    }   
    DeletePartner(myPartnerItem: PartnerItem) {
        if (myPartnerItem.IsEditingEnabled) {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Show(TextCodeTranslator.Translate("Quote.M.DeleteThisPartner"));

            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    if (myPartnerItem.IsCustomer) {
                        this.SetDefaultCustomer();
                    }

                    var index = this.ItemsCollection.indexOf(myPartnerItem);
                    if (index != -1) {
                        this.ItemsCollection.splice(index, 1);
                    }

                    myPartnerItem.PartnerId = null;
                    myPartnerItem.AddressId = null;
                    myPartnerItem.ContactId = null;
                    myPartnerItem.Reference1 = null;
                    myPartnerItem.Reference2 = null;
                    this.SetAddButtonsIsDisabled();
                    this.CurrentSession.FireEvent("QuotePartnersChanged");
                }
            });
        }
    }
    SetDefaultCustomer() {
        this.EntityPM.CustomerId = null;
        this.EntityPM.CustomerName = null;
        this.EntityPM.CustomerNote = null;
        this.EntityPM.CustomerContactId = null;
        this.EntityPM.CustomerReference1 = null;
        this.EntityPM.CustomerReference2 = null;
        this.EntityPM.QuoteCustomerTypeCode = null;

        if (this.EntityPM.DirectionId == "I") {
            this.EntityPM.QuoteCustomerTypeCode = "CON";
            this.EntityPM.CustomerId = this.EntityPM.ConsigneeId;
            this.EntityPM.CustomerName = this.EntityPM.ConsigneeName;
            this.EntityPM.CustomerNote = this.EntityPM.ConsigneeNote;
            this.EntityPM.CustomerContactId = this.EntityPM.ConsigneeContactId;
            this.EntityPM.CustomerReference1 = this.EntityPM.ConsigneeReference1;
            this.EntityPM.CustomerReference2 = this.EntityPM.ConsigneeReference2;
        }

        else {
            this.EntityPM.QuoteCustomerTypeCode = "SHI";
            this.EntityPM.CustomerId = this.EntityPM.ShipperId;
            this.EntityPM.CustomerName = this.EntityPM.ShipperName;
            this.EntityPM.CustomerNote = this.EntityPM.ShipperNote;
            this.EntityPM.CustomerContactId = this.EntityPM.ShipperContactId;
            this.EntityPM.CustomerReference1 = this.EntityPM.ShipperReference1;
            this.EntityPM.CustomerReference2 = this.EntityPM.ShipperReference2;
        }

        this.OnCustomerChanged();
    }

    public UpdateSalesmanId: string = null;
    public UpdateSalesmanName: string = null;
    public UpdateSalesmanText: string = null;    
    public IsUpdateSalesmanVisible: boolean = false;
    private SalesmanUpdated: boolean = false;
    OnCustomerChanged() {
        this.UpdateSalesmanId = null;
        this.UpdateSalesmanName = null;
        this.UpdateSalesmanText = null;
        this.IsUpdateSalesmanVisible = false;
        this.SalesmanUpdated = false;
        if (!AppTool.IsNullOrEmpty(this.EntityPM.CustomerId)) {
            this.CardListService.getSingle(this.EntityPM.CustomerId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var list: CardList = myResponse.Result;
                    if (list) {
                        if (list.SalesmanUserId != this.EntityPM.SalesmanUserId) {
                            if (AppTool.IsNullOrEmpty(list.SalesmanUserId)) {
                                this.UpdateSalesmanId = null;
                                this.UpdateSalesmanName = null;
                                this.UpdateSalesmanText = "Customer changed, update the salesman to Empty ?";                                
                            }

                            else {
                                this.UpdateSalesmanId = list.SalesmanUserId;
                                this.UpdateSalesmanName = list.SalesmanUserEnglishName;
                                this.UpdateSalesmanText = "Customer changed, update the salesman to " + list.SalesmanUserEnglishName + " ?";                                
                            }

                            this.IsUpdateSalesmanVisible = true;
                            this.SalesmanUpdated = true;
                        }
                    }
                }
            });
        }
    }
    UpdateSalesmanClicked() {
        if (this.SalesmanUpdated) {
            this.EntityPM.SalesmanUserId = this.UpdateSalesmanId;
            this.EntityPM.SalesmanName = this.UpdateSalesmanName;
        }
        
        this.IsUpdateSalesmanVisible = false;
    }
}
export class PartnerItem extends BaseComponent {
    public IsNewAdded: boolean;
    public EntityPM: QuotePM;
    public Code: string;
    public ObjectTableName: string = "Quote";
    public IsMyCustomer: boolean = false;
    public PartnerId_Origin: string;
    public AddressId_Origin: string;
    public ContactId_Origin: string;
    public Reference1_Origin: string;
    public Reference2_Origin: string;
    public IsInlandDomestic: boolean = false;
    constructor(public fatherComponent: PartnersTabComponent, typeCode: string) {
        super();
        this.EntityPM = fatherComponent.EntityPM;
        this.IsEditingEnabled = fatherComponent.IsEditingEnabled;
        this.Code = typeCode;
        this.InitializeProperties();
        this.SetRemoveButtonVisibility();
        this.GetPartnerAddress();
        this.GetPartnerContact();
        if (this.EntityPM != null) {
            this.IsInlandDomestic = this.EntityPM.TransportModeId == "I" && this.EntityPM.DirectionId == "D" ? true : false;
        }
    }

    public EditPartnerIsEnabled: boolean = false;
    public IsEditingEnabled: boolean = true;
    SetUIProperties() {
        var isPartnerFilled = this.PartnerId == null ? false : true;
        var isFieldsEnabled = false;
        if (this.IsEditingEnabled) {
            if (isPartnerFilled) {
                isFieldsEnabled = true;
            }
        }

        this.EditPartnerIsEnabled = isFieldsEnabled;
        this.UIProperties.SetRequired(this.PartnerIdProperty, this.ObjectTableName, !isPartnerFilled);
        this.UIProperties.SetEnabled(this.PartnerIdProperty, this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled(this.PartnerAddressIdProperty, this.ObjectTableName, isFieldsEnabled);
        this.UIProperties.SetEnabled(this.PartnerContactIdProperty, this.ObjectTableName, isFieldsEnabled);
        this.UIProperties.SetEnabled(this.Reference1Property, this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled(this.Reference2Property, this.ObjectTableName, this.IsEditingEnabled);
    }

    public FullCode: string;
    public PartnerIndex: number;
    public PartnerTypeName: string;
    InitializeProperties() {
        switch (this.Code) {
            case "SHIPR": { this.PartnerIndex = 0; this.FullCode = "Shipper"; break }
            case "CONSI": { this.PartnerIndex = 1; this.FullCode = "Consignee"; break }
            case "AGENT": { this.PartnerIndex = 2; this.FullCode = "Agent"; break }
            case "NOTFY": { this.PartnerIndex = 3; this.FullCode = "Notify"; break }
        }

        this.PartnerTypeName = TextCodeTranslator.Translate("Quote.F." + this.FullCode + "Id");
    }

    get PartnerName() {
        switch (this.Code) {
            case "SHIPR": { return this.EntityPM.ShipperName; }
            case "CONSI": { return this.EntityPM.ConsigneeName; }
            case "AGENT": { return this.EntityPM.AgentName; }
            case "NOTFY": { return this.EntityPM.NotifyName; }
            default: { return null; }
        }
    }
    set PartnerName(newValue: string) {
        switch (this.Code) {
            case "SHIPR": {
                if (this.EntityPM.ShipperName != newValue) {
                    this.EntityPM.ShipperName = newValue
                }

                break;
            }

            case "CONSI": {
                if (this.EntityPM.ConsigneeName != newValue) {
                    this.EntityPM.ConsigneeName = newValue
                }

                break;
            }

            case "AGENT": {
                if (this.EntityPM.AgentName != newValue) {
                    this.EntityPM.AgentName = newValue
                }

                break;
            }

            case "CSTMR": {
                if (this.EntityPM.CustomerName != newValue) {
                    this.EntityPM.CustomerName = newValue
                }

                break;
            }

            case "NOTFY": {
                if (this.EntityPM.NotifyName != newValue) {
                    this.EntityPM.NotifyName = newValue
                    break;
                }
            }
        }
    }
       
    get CardDependencyProperty1() {
        var myResult: string = null;

        switch (this.Code) {
            case "SHIPR":
            case "CONSI":
            case "CSTMR":
                {
                    myResult = "CS,PO";
                    
                    if (SessionLocator.TenantPM.AllowAgentInCustomersLOV) {
                        myResult = "CS,PO,AG";
                    }

                    if (this.IsInlandDomestic) {
                        myResult = myResult + ",WH";
                    }
                    break
                }

            case "AGENT":
                {
                    myResult = "AG";

                    if (SessionLocator.TenantPM.AllowCustomersInAgentsLOV) {
                        myResult = "CS,AG";
                    } 
                    break;
                }
                
            case "NOTFY":
                {
                    myResult = "AG,AL,CG,CS,SG,SL,TR,VD,WH";
                    break;
                }

            default: {
                myResult = "CS";
                break;
            }
        }

        return myResult;
    }
    get CardDependencyProperty1IsList() {
        var myResult: boolean = false;

        switch (this.Code) {   
            case "SHIPR":
            case "CONSI":
            case "CSTMR":
            case "NOTFY":
                {
                    myResult = true;
                    break
                }

            case "AGENT":
                {
                    if (SessionLocator.TenantPM.AllowCustomersInAgentsLOV) {
                        myResult = true;
                    }
                    break;
                }

            default: {
                myResult = false;
                break;
            }
        }

        return myResult;
    } 
    
    get Note() {
        switch (this.Code) {
            case "SHIPR": { return this.EntityPM.ShipperNote; }
            case "CONSI": { return this.EntityPM.ConsigneeNote; }
            //case "AGENT": { return this.EntityPM.AgentNote; }
            case "CSTMR": { return this.EntityPM.CustomerNote; }
            case "NOTFY": { return this.EntityPM.NotifyNote; }
            default: { return null; }
        }
    }
    set Note(newValue: string) {
        switch (this.Code) {
            case "SHIPR": {
                if (this.EntityPM.ShipperNote != newValue) {
                    this.EntityPM.ShipperNote = newValue
                }

                break;
            }

            case "CONSI": {
                if (this.EntityPM.ConsigneeNote != newValue) {
                    this.EntityPM.ConsigneeNote = newValue
                }

                break;
            }

            //case "AGENT": {
            //    if (this.EntityPM.AgentNote != newValue) {
            //        this.EntityPM.AgentNote = newValue
            //    }
            //  break;
            //}

            case "CSTMR": {
                if (this.EntityPM.CustomerNote != newValue) {
                    this.EntityPM.CustomerNote = newValue
                }

                break;
            }

            case "NOTFY": {
                if (this.EntityPM.NotifyNote != newValue) {
                    this.EntityPM.NotifyNote = newValue
                }

                break;
            }
        }
    }

    get IsCustomer() {
        return (this.PartnerId == this.EntityPM.CustomerId) ? true : false;
    }

    SetAsCustomer() {
        this.EntityPM.CustomerId = null;
        this.EntityPM.CustomerName = null;
        this.EntityPM.CustomerNote = null;
        this.EntityPM.CustomerContactId = null;
        this.EntityPM.CustomerReference1 = null;
        this.EntityPM.CustomerReference2 = null;

        switch (this.Code) {
            case "SHIPR":
                {
                    this.EntityPM.QuoteCustomerTypeCode = "SHI";
                    break;
                }

            case "CONSI":
                {
                    this.EntityPM.QuoteCustomerTypeCode = "CON";
                    break;
                }

            case "AGENT":
                {
                    this.EntityPM.QuoteCustomerTypeCode = "AGT";
                    break;
                }

            case "NOTFY":
                {
                    this.EntityPM.QuoteCustomerTypeCode = "NOT";
                    break;
                }

            default:
                {
                    this.EntityPM.QuoteCustomerTypeCode = "OTH";
                    break;
                }
        }

        this.EntityPM.CustomerId = this.PartnerId;
        this.EntityPM.CustomerName = this.PartnerName;
        this.EntityPM.CustomerNote = this.Note;
        this.EntityPM.CustomerContactId = this.ContactId;
        this.EntityPM.CustomerReference1 = this.Reference1;
        this.EntityPM.CustomerReference2 = this.Reference2; 

        this.fatherComponent.OnCustomerChanged();
    }

    CopyOriginData() {
        this.PartnerId_Origin = this.PartnerId;
        this.AddressId_Origin = this.AddressId;
        this.ContactId_Origin = this.ContactId;
        this.Reference1_Origin = this.Reference1;
        this.Reference2_Origin = this.Reference2;
    }

    public IsRemoveButtonVisible: boolean = false;
    SetRemoveButtonVisibility() {
        switch (this.Code) {
            case "SHIPR": {
                this.IsRemoveButtonVisible = (this.EntityPM.DirectionId == "I") ? true : false;
                break;
            }

            case "CONSI": {
                this.IsRemoveButtonVisible = (this.EntityPM.DirectionId == "E") ? true : false;
                break;
            }

            default: {
                this.IsRemoveButtonVisible = true;
                break;
            }
        }
    }

    // PartnerId
    get PartnerIdProperty() {
        switch (this.Code) {
            case "SHIPR": { return "ShipperId"; }
            case "CONSI": { return "ConsigneeId"; }
            case "AGENT": { return "AgentId"; }
            case "CSTMR": { return "CustomerId"; }
            case "NOTFY": { return "NotifyId"; }
            default: { return null; }
        }
    }

    get PartnerId() {
        switch (this.Code) {
            case "SHIPR": { return this.ShipperId; }
            case "CONSI": { return this.ConsigneeId; }
            case "AGENT": { return this.AgentId; }
            case "CSTMR": { return this.CustomerId; }
            case "NOTFY": { return this.NotifyId; }
            default: { return null; }
        }
    }
    set PartnerId(newValue: string) {
        switch (this.Code) {
            case "SHIPR": { this.ShipperId = newValue; break; }
            case "CONSI": { this.ConsigneeId = newValue; break; }
            case "AGENT": { this.AgentId = newValue; break; }
            case "CSTMR": { this.CustomerId = newValue; break; }
            case "NOTFY": { this.NotifyId = newValue; break; }
        }
    }

    get ShipperId() {
        return this.EntityPM.ShipperId;
    }
    set ShipperId(newValue: string) {
        if (this.EntityPM.ShipperId != newValue) {
            this.EntityPM.ShipperId = newValue;

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.EntityPM.ShipperMainAddressId = null;
                this.EntityPM.ShipperPickAddressId = null;
                this.EntityPM.PickUpAddressId = null;
                this.EntityPM.FromPartnerId = null;
                this.EntityPM.FromPartnerAddressId = null;
            }

            else {
                if (this.IsInlandDomestic) {
                    this.EntityPM.FromPartnerId = newValue;
                }

                this.EntityPM.FromAddressCity = null;
                this.EntityPM.FromAddressZipCode = null;
                this.EntityPM.FromAddressCountryId = null;
            }

            this.GetPartnerCard();
        }
    }

    get ConsigneeId() {
        return this.EntityPM.ConsigneeId;
    }
    set ConsigneeId(newValue: string) {
        if (this.EntityPM.ConsigneeId != newValue) {
            this.EntityPM.ConsigneeId = newValue;

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.EntityPM.ConsigneeMainAddressId = null;
                this.EntityPM.ConsigneePickAddressId = null;
                this.EntityPM.DeliveryAddressId = null;
                this.EntityPM.ToPartnerId = null;
                this.EntityPM.ToPartnerAddressId = null;
            }

            else {
                if (this.IsInlandDomestic) {
                    this.EntityPM.ToPartnerId = newValue;
                }

                this.EntityPM.ToAddressCity = null;
                this.EntityPM.ToAddressZipCode = null;
                this.EntityPM.ToAddressCountryId = null;
            }
            
            this.GetPartnerCard();
        }
    }

    get CustomerId() {
        return this.EntityPM.CustomerId;
    }
    set CustomerId(newValue: string) {
        if (this.EntityPM.CustomerId != newValue) {
            this.EntityPM.CustomerId = newValue;
            this.GetPartnerCard();
        }
    }

    get AgentId() {
        return this.EntityPM.AgentId;
    }
    set AgentId(newValue: string) {
        if (this.EntityPM.AgentId != newValue) {
            this.EntityPM.AgentId = newValue;
            this.GetPartnerCard();
        }
    }

    get NotifyId() {
        return this.EntityPM.NotifyId;
    }
    set NotifyId(newValue: string) {
        if (this.EntityPM.NotifyId != newValue) {
            this.EntityPM.NotifyId = newValue;
            this.GetPartnerCard();
        }
    }

    // AddressId
    get PartnerAddressIdProperty() {
        switch (this.Code) {
            case "SHIPR": { return "ShipperAddressId"; }
            case "CONSI": { return "ConsigneeAddressId"; }
            case "AGENT": { return "AgentAddressId"; }
            case "NOTFY": { return "NotifyAddressId"; }
            default: { return null; }
        }
    }

    get AddressId() {
        switch (this.Code) {
            case "SHIPR": { return this.ShipperAddressId; }
            case "CONSI": { return this.ConsigneeAddressId; }
            case "AGENT": { return this.AgentAddressId; }
            case "NOTFY": { return this.NotifyAddressId; }
            default: { return null; }
        }
    }
    set AddressId(newValue: string) {
        switch (this.Code) {
            case "SHIPR": { this.ShipperAddressId = newValue; break; }
            case "CONSI": { this.ConsigneeAddressId = newValue; break; }
            case "AGENT": { this.AgentAddressId = newValue; break; }
            case "NOTFY": { this.NotifyAddressId = newValue; break; }
        }
    }

    get ShipperAddressId() {
        return this.EntityPM.ShipperMainAddressId;
    }
    set ShipperAddressId(newValue: string) {
        if (this.EntityPM.ShipperMainAddressId != newValue) {
            this.EntityPM.ShipperMainAddressId = newValue;
            this.GetPartnerAddress();
        }
    }

    get ConsigneeAddressId() {
        return this.EntityPM.ConsigneeMainAddressId;
    }
    set ConsigneeAddressId(newValue: string) {
        if (this.EntityPM.ConsigneeMainAddressId != newValue) {
            this.EntityPM.ConsigneeMainAddressId = newValue;
            this.GetPartnerAddress();
        }
    }

    get AgentAddressId() {
        return this.EntityPM.AgentAddressId;
    }
    set AgentAddressId(newValue: string) {
        if (this.EntityPM.AgentAddressId != newValue) {
            this.EntityPM.AgentAddressId = newValue;
            this.GetPartnerAddress();
        }
    }

    get NotifyAddressId() {
        return this.EntityPM.NotifyAddressId;
    }
    set NotifyAddressId(newValue: string) {
        if (this.EntityPM.NotifyAddressId != newValue) {
            this.EntityPM.NotifyAddressId = newValue;
            this.GetPartnerAddress();
        }
    }

    // ContactId
    get PartnerContactIdProperty() {
        switch (this.Code) {
            case "SHIPR": { return "ShipperContactId"; }
            case "CONSI": { return "ConsigneeContactId"; }
            case "AGENT": { return "AgentContactId"; }
            case "CSTMR": { return "CustomerContactId"; }
            case "NOTFY": { return "NotifyContactId"; }
            default: { return null; }
        }
    }

    get ContactId() {
        switch (this.Code) {
            case "SHIPR": { return this.ShipperContactId; }
            case "CONSI": { return this.ConsigneeContactId; }
            case "AGENT": { return this.AgentContactId; }
            case "CSTMR": { return this.CustomerContactId; }
            case "NOTFY": { return this.NotifyContactId; }
            default: { return null; }
        }
    }
    set ContactId(newValue: string) {
        switch (this.Code) {
            case "SHIPR": { this.ShipperContactId = newValue; break; }
            case "CONSI": { this.ConsigneeContactId = newValue; break; }
            case "AGENT": { this.AgentContactId = newValue; break; }
            case "CSTMR": { this.CustomerContactId = newValue; break; }
            case "NOTFY": { this.NotifyContactId = newValue; break; }
        }
    }

    get ShipperContactId() {
        return this.EntityPM.ShipperContactId;
    }
    set ShipperContactId(newValue: string) {
        if (this.EntityPM.ShipperContactId != newValue) {
            this.EntityPM.ShipperContactId = newValue;
            this.GetPartnerContact();
        }
    }

    get ConsigneeContactId() {
        return this.EntityPM.ConsigneeContactId;
    }
    set ConsigneeContactId(newValue: string) {
        if (this.EntityPM.ConsigneeContactId != newValue) {
            this.EntityPM.ConsigneeContactId = newValue;
            this.GetPartnerContact();
        }
    }

    get AgentContactId() {
        return this.EntityPM.AgentContactId;
    }
    set AgentContactId(newValue: string) {
        if (this.EntityPM.AgentContactId != newValue) {
            this.EntityPM.AgentContactId = newValue;
            this.GetPartnerContact();
        }
    }

    get CustomerContactId() {
        return this.EntityPM.CustomerContactId;
    }
    set CustomerContactId(newValue: string) {
        if (this.EntityPM.CustomerContactId != newValue) {
            this.EntityPM.CustomerContactId = newValue;
            this.GetPartnerContact();
        }
    }

    get NotifyContactId() {
        return this.EntityPM.NotifyContactId;
    }
    set NotifyContactId(newValue: string) {
        if (this.EntityPM.NotifyContactId != newValue) {
            this.EntityPM.NotifyContactId = newValue;
            this.GetPartnerContact();
        }
    }

    // Reference1
    get HasReference1() {
        var myResult: boolean = false;

        switch (this.Code) {
            case "SHIPR":
            case "CONSI":
            case "AGENT":
            case "CSTMR":
                {
                    myResult = true;
                }
        }

        return myResult;
    }
    get Reference1Property() {
        switch (this.Code) {
            case "SHIPR": { return "ShipperReference1"; }
            case "CONSI": { return "ConsigneeReference1"; }
            case "AGENT": { return "AgentReference1"; }
            case "CSTMR": { return "CustomerReference1"; }
            default: { return null; }
        }
    }
    get Reference1() {
        switch (this.Code) {
            case "SHIPR": { return this.ShipperReference1; }
            case "CONSI": { return this.ConsigneeReference1; }
            case "AGENT": { return this.AgentReference1; }
            case "CSTMR": { return this.CustomerReference1; }
            default: { return null; }
        }
    }
    set Reference1(newValue: string) {
        switch (this.Code) {
            case "SHIPR": { this.ShipperReference1 = newValue; break; }
            case "CONSI": { this.ConsigneeReference1 = newValue; break; }
            case "AGENT": { this.AgentReference1 = newValue; break; }
            case "CSTMR": { this.CustomerReference1 = newValue; break; }
        }
    }

    get ShipperReference1() {
        return this.EntityPM.ShipperReference1;
    }
    set ShipperReference1(newValue: string) {
        if (this.EntityPM.ShipperReference1 != newValue) {
            this.EntityPM.ShipperReference1 = newValue;
        }
    }

    get ConsigneeReference1() {
        return this.EntityPM.ConsigneeReference1;
    }
    set ConsigneeReference1(newValue: string) {
        if (this.EntityPM.ConsigneeReference1 != newValue) {
            this.EntityPM.ConsigneeReference1 = newValue;
        }
    }

    get AgentReference1() {
        return this.EntityPM.AgentReference1;
    }
    set AgentReference1(newValue: string) {
        if (this.EntityPM.AgentReference1 != newValue) {
            this.EntityPM.AgentReference1 = newValue;
        }
    }

    get CustomerReference1() {
        return this.EntityPM.CustomerReference1;
    }
    set CustomerReference1(newValue: string) {
        if (this.EntityPM.CustomerReference1 != newValue) {
            this.EntityPM.CustomerReference1 = newValue;
        }
    }
    
    // Reference2
    get HasReference2() {
        var myResult: boolean = false;

        switch (this.Code) {
            case "SHIPR":
            case "CONSI":
            case "AGENT":
            case "CSTMR":
                {
                    myResult = true;
                }
        }

        return myResult;
    }
    get Reference2Property() {
        switch (this.Code) {
            case "SHIPR": { return "ShipperReference2"; }
            case "CONSI": { return "ConsigneeReference2"; }
            case "AGENT": { return "AgentReference2"; }
            case "CSTMR": { return "CustomerReference2"; }
            default: { return null; }
        }
    }
    get Reference2() {
        switch (this.Code) {
            case "SHIPR": { return this.ShipperReference2; }
            case "CONSI": { return this.ConsigneeReference2; }
            case "AGENT": { return this.AgentReference2; }
            case "CSTMR": { return this.CustomerReference2; }
            default: { return null; }
        }
    }
    set Reference2(newValue: string) {
        switch (this.Code) {
            case "SHIPR": { this.ShipperReference2 = newValue; break; }
            case "CONSI": { this.ConsigneeReference2 = newValue; break; }
            case "AGENT": { this.AgentReference2 = newValue; break; }
            case "CSTMR": { this.CustomerReference2 = newValue; break; }
        }
    }

    get ShipperReference2() {
        return this.EntityPM.ShipperReference2;
    }
    set ShipperReference2(newValue: string) {
        if (this.EntityPM.ShipperReference2 != newValue) {
            this.EntityPM.ShipperReference2 = newValue;
        }
    }

    get ConsigneeReference2() {
        return this.EntityPM.ConsigneeReference2;
    }
    set ConsigneeReference2(newValue: string) {
        if (this.EntityPM.ConsigneeReference2 != newValue) {
            this.EntityPM.ConsigneeReference2 = newValue;
        }
    }

    get AgentReference2() {
        return this.EntityPM.AgentReference2;
    }
    set AgentReference2(newValue: string) {
        if (this.EntityPM.AgentReference2 != newValue) {
            this.EntityPM.AgentReference2 = newValue;
        }
    }

    get CustomerReference2() {
        return this.EntityPM.CustomerReference2;
    }
    set CustomerReference2(newValue: string) {
        if (this.EntityPM.CustomerReference2 != newValue) {
            this.EntityPM.CustomerReference2 = newValue;
        }
    }

    // Add|Edit Partner
    private isEditButtonClicked: boolean = false;
    AddPartnerClicked() {
        var myComponentPath: string = null;

        if (this.Code == "AGENT") {
            myComponentPath = "./CommonModules/CommonAgent/Components/NewEntity/NewAgentComponent";
        }

        else {
            myComponentPath = "./CommonModules/CommonCustomer/Components/NewEntity/NewCustomerComponent";           
        }
        
        if (myComponentPath != null) {
            var logWindow = new LogitudeWindow();
            logWindow.Width = 900;
            logWindow.Height = 570;
            logWindow.Title = "New " + this.PartnerTypeName;

            if (this.Code != "AGENT") {
                var args = new NewEntityArgs();
                args.Perspective = "ShippersAndConsignees";
                logWindow.WindowArgs = args;
            }

            logWindow.Show(myComponentPath);

            logWindow.ComponentLoaded.subscribe(comp => {
                logWindow.WindowClosed.subscribe(s => {
                    if (s) {
                        this.PartnerId = comp.EntityPM.Id;
                        this.PartnerName = comp.EntityPM.EnglishName;
                    }
                });
            });
        }
    }
    EditPartnerClicked() {
        if (!this.isEditButtonClicked) {
            if (!AppTool.IsNullOrEmpty(this.PartnerId)) {
                this.isEditButtonClicked = true;

                var myService = this.fatherComponent.CardListService;
                myService.getSingle(this.PartnerId).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CardList = myResponse.Result;
                        if (list) {
                            var objectTableName: string = null;

                            switch (list.PartnerTypeId) {
                                case "CO":
                                case "AG":
                                    {
                                        objectTableName = "Agent";
                                        break;
                                    }


                                case "AL": { objectTableName = "Airline"; break; }
                                case "TR": { objectTableName = "Trucker"; break; }
                                case "CG": { objectTableName = "CustomAgent"; break; }
                                case "SG": { objectTableName = "ShippingAgent"; break; }
                                case "SL": { objectTableName = "ShippingLine"; break; }
                                case "VD": { objectTableName = "Vendor"; break; }

                                case "CS":
                                    {
                                        objectTableName = "Customer";
                                        break;
                                    }

                                case "WH":
                                case "CC":
                                    {
                                        objectTableName = "Warehouse";
                                        break;
                                    }
                            }

                            if (objectTableName != null) {
                                var logWindow = new LogitudeWindow();
                                logWindow.Title = "Edit " + this.PartnerTypeName;
                                logWindow.IsFillScreen = true;
                                logWindow.ShowEditComponent(this.PartnerId, objectTableName);

                                logWindow.ComponentLoaded.subscribe(comp => {
                                    logWindow.WindowClosed.subscribe(s => {

                                        this.PartnerName = comp.EntityPM.EnglishName;
                                        this.Note = comp.EntityPM.Notes;

                                        this.GetPartnerAddress();
                                        this.GetPartnerContact();
                                        this.isEditButtonClicked = false;
                                    });
                                });
                            }

                            else {
                                this.isEditButtonClicked = false;
                            }
                        }

                        else {
                            this.isEditButtonClicked = false;
                        }
                    }

                    else {
                        this.isEditButtonClicked = false;
                    }
                });
            }
        }
    }

    public IsReseting: boolean = false;
    private isAddressLoaded = false;
    private isContactLoaded = false;
    public ShowNoTemplateText = false;
    public AddressCityText: string = null;
    public PartnerCardList: CardList;
    public PartnerAddressList: AddressList;
    public PartnerContactList: ContactList;
    GetPartnerCard() {
        this.SetUIProperties();

        var myCardId: string = this.PartnerId;

        if (AppTool.IsNullOrEmpty(myCardId)) {
            this.PartnerName = null;
            this.Note = null;
            this.AddressCityText = null;
            this.PartnerCardList = null;
            this.PartnerAddressList = null;
            this.PartnerContactList = null;
            this.AddressId = null;
            this.ContactId = null;            
        }

        else {
            var myService = this.fatherComponent.CardListService;
            myService.getSingle(myCardId).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        var list: CardList = myResponse.Result;

                        this.PartnerCardList = list;

                        if (list) {

                            this.PartnerName = list.EnglishName;
                            this.Note = list.Notes;
                            this.ContactId = list.PrimaryContactId;
                            this.AddressId = list.MainAddressId;

                            switch (this.Code) {
                                case "SHIPR": {
                                    this.EntityPM.ShipperPickAddressId = list.PickAddressId;

                                    if (list.PartnerTypeId == "PO") {
                                        this.EntityPM.IsPotentialShipper = true;
                                    }

                                    if (this.IsInlandDomestic) {
                                        this.EntityPM.FromPartnerAddressId = this.EntityPM.ShipperMainAddressId;
                                    }

                                    if (this.EntityPM.IncludePickUp) {
                                        if (!AppTool.IsNullOrEmpty(this.EntityPM.ShipperPickAddressId)) {
                                            this.EntityPM.PickUpAddressId = this.EntityPM.ShipperPickAddressId;
                                        }

                                        else {
                                            this.EntityPM.PickUpAddressId = this.EntityPM.ShipperMainAddressId;
                                        }

                                        if (!AppTool.IsNullOrEmpty(this.EntityPM.PickUpAddressId)) {
                                            this.EntityPM.FromAddressCity = null;
                                            this.EntityPM.FromAddressZipCode = null;
                                            this.EntityPM.FromAddressCountryId = null;
                                        }
                                    }

                                    break;
                                }

                                case "CONSI": {
                                    this.EntityPM.ConsigneePickAddressId = list.PickAddressId;

                                    if (list.PartnerTypeId == "PO") {
                                        this.EntityPM.IsPotentialConsignee = true;
                                    }

                                    if (this.IsInlandDomestic) {
                                        this.EntityPM.ToPartnerAddressId = this.EntityPM.ConsigneeMainAddressId;
                                    }

                                    if (this.EntityPM.IncludeDelivery) {
                                        if (!AppTool.IsNullOrEmpty(this.EntityPM.ConsigneePickAddressId)) {
                                            this.EntityPM.DeliveryAddressId = this.EntityPM.ConsigneePickAddressId;
                                        }

                                        else {
                                            this.EntityPM.DeliveryAddressId = this.EntityPM.ConsigneeMainAddressId;
                                        }

                                        if (!AppTool.IsNullOrEmpty(this.EntityPM.DeliveryAddressId)) {
                                            this.EntityPM.ToAddressCity = null;
                                            this.EntityPM.ToAddressZipCode = null;
                                            this.EntityPM.ToAddressCountryId = null;
                                        }
                                    }

                                    break;
                                }
                            }
                        }
                    }
                }
            });
        }
    }
    GetPartnerAddress() {

        this.isAddressLoaded = false;
        this.PartnerAddressList = null;
        this.ShowNoTemplateText = false;
        this.AddressCityText = null;

        var myAddressId: string = this.AddressId;

        if (myAddressId != null) {
            var myService = this.fatherComponent.AddressListService;
            myService.getSingle(myAddressId).subscribe((myResponse: ServiceResponse) => {

                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        this.PartnerAddressList = myResponse.Result;
                    }
                }

                this.isAddressLoaded = true;
                this.BuildAddressCityText();
                this.OnLoadCompleted();
            });
        }

        else {
            this.isAddressLoaded = true;
            this.BuildAddressCityText();
            this.OnLoadCompleted();
        }
    }
    GetPartnerContact() {

        this.isContactLoaded = false;
        this.PartnerContactList = null;
        this.ShowNoTemplateText = false;

        var myContactId: string = this.ContactId;

        if (myContactId != null) {
            var myService = this.fatherComponent.ContactListService;
            myService.getSingle(myContactId).subscribe((myResponse: ServiceResponse) => {

                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        this.PartnerContactList = myResponse.Result;
                    }
                }

                this.isContactLoaded = true;
                this.OnLoadCompleted();
            });
        }

        else {
            this.isContactLoaded = true;
            this.OnLoadCompleted();
        }
    }
    BuildAddressCityText() {
        var myResult = null;

        if (this.PartnerAddressList) {

            if (!AppTool.IsNullOrEmpty(this.PartnerAddressList.City)) {
                myResult = this.PartnerAddressList.City;
            }

            if (!AppTool.IsNullOrEmpty(this.PartnerAddressList.StateName)) {
                myResult = AppTool.IsNullOrEmpty(myResult) ? this.PartnerAddressList.StateName : myResult + ", " + this.PartnerAddressList.StateName;
            }

            if (!AppTool.IsNullOrEmpty(this.PartnerAddressList.ZipCode)) {
                myResult = AppTool.IsNullOrEmpty(myResult) ? this.PartnerAddressList.ZipCode : myResult + ", " + this.PartnerAddressList.ZipCode;
            }
        }

        this.AddressCityText = myResult;
    }

    OnLoadCompleted() {
        if (this.isAddressLoaded && this.isContactLoaded) {
            if (this.PartnerAddressList == null && this.PartnerContactList == null) {
                this.ShowNoTemplateText = true;
            }

            else {
                this.ShowNoTemplateText = false;
            }
        }
    }
    ResetOriginData() {
        if (this.IsNewAdded) {
            this.PartnerId = null;
            this.AddressId = null;
            this.ContactId = null;
            this.Reference1 = null;
            this.Reference2 = null;
        }

        else {
            this.IsReseting = true;
            this.PartnerId = this.PartnerId_Origin;
            this.AddressId = this.AddressId_Origin;
            this.ContactId = this.ContactId_Origin;
            this.Reference1 = this.Reference1_Origin;
            this.Reference2 = this.Reference2_Origin;
            this.IsReseting = false;
        }
    }

    AddContact() {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.Title = "New Contact";
        var args = new ContactInputTemplateArgs();
        args.CustomerId = this.PartnerId;
        args.CardDependencyProperty1 = this.CardDependencyProperty1;
        args.CustomerLable = this.PartnerTypeName;
        args.ComponentName = "Partners";
        logWindow.WindowArgs = args;
        logWindow.Show('./CommonModules/CommonPartners/Components/NewEntity/NewContactComponent');
        logWindow.WindowClosed.subscribe(($event: any) => this.OnNewContactWindowClosed($event));
    }

    OnNewContactWindowClosed(arg: any) {
        if (arg != 'cancel') {
            this.ContactId = arg;
        }
    }
}
