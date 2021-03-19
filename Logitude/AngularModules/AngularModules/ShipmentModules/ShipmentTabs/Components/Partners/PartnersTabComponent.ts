import {Component, OnInit, OnDestroy}  from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {UIProperty, UIProperties}  from '../../../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceHelper} from '../../../../Infrastructure/Utilities/ServiceHelper';
import {CardList} from '../../../../Common/EntityLists/CardList';
import {AddressList} from '../../../../Common/EntityLists/AddressList';
import {ContactList} from '../../../../Common/EntityLists/ContactList';
import {CardListService} from '../../../../Common/Services/StandardLists/CardListService';
import {AddressListService} from '../../../../Common/Services/StandardLists/AddressListService';
import {ContactListService} from '../../../../Common/Services/StandardLists/ContactListService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {NewEntityArgs} from '../../../../Infrastructure/Args';
import {AppTool} from '../../../../Infrastructure/Tools';
import {AddressPM} from '../../../../Common/EntityPMs/AddressPM';
import {AddEditPartnerArgs} from '../../../../Shipment/Args';
import {ShipmentTool} from '../../../../Shipment/Tools';
import { ContactInputTemplateArgs } from '../../../../CommonModules/CommonPartners/Components/Templates/ContactInputTemplate';

@Component({
    
    templateUrl: './PartnersTabComponent.html',
})

export class PartnersTabComponent implements OnInit, OnDestroy {
    public EntityPM: ShipmentPM;
    public ObjectTableName: string;
    public ItemsCollection: PartnerItem[];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = this.entityArgs.EntityPM;
        this.ObjectTableName = this.entityArgs.ObjectTableName;
        this.InitializeServices();
        this.Listen();
    }

    private SessionEvent: any = null;
    private TabChangedEvent: any = null;
    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;  
    private Listen() {
        if (this.entityArgs.EditComponent) {

            this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(s => {
                if (s == "AWBWizardClosed") {
                    this.UpdateScreen();
                }
            });

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

            this.TabChangedEvent = this.entityArgs.EditComponent.TabChanged.subscribe((tabCode: string) => {
                this.IsUpdateSalesmanAccountManagerVisible = false;                
            });
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SessionEvent);
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
        this.IsEditingEnabled = ShipmentTool.IsEditingEnabled(this.EntityPM);
    }

    public AllAddresses: AddressList[] = [];
    public AllContacts: ContactList[] = [];
    BuildItemsCollection() {

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

        if (this.EntityPM.IssuingCarrierAgentId != null) {
            this.ItemsCollection.push(new PartnerItem(this, "ISSAG"));
        }

        if (this.EntityPM.CustomAgentExportId != null) {
            this.ItemsCollection.push(new PartnerItem(this, "CSAEX"));
        }

        if (this.EntityPM.CustomAgentImportId != null) {
            this.ItemsCollection.push(new PartnerItem(this, "CSAIM"));
        }

        if (this.EntityPM.Notify1Id != null) {
            this.ItemsCollection.push(new PartnerItem(this, "NOTF1"));
        }

        if (this.EntityPM.Notify2Id != null) {
            this.ItemsCollection.push(new PartnerItem(this, "NOTF2"));
        }

        if (this.EntityPM.ShipperNotExporterId != null) {
            this.ItemsCollection.push(new PartnerItem(this, "SHPNT"));
        }

        if (this.EntityPM.ConsigneeNotImporterId != null) {
            this.ItemsCollection.push(new PartnerItem(this, "CONNT"));
        }

        if (this.EntityPM.FreightForwarderId != null) {
            this.ItemsCollection.push(new PartnerItem(this, "FRTFR"));
        }

        if (this.EntityPM.ColoaderId != null) {
            this.ItemsCollection.push(new PartnerItem(this, "COLOD"));
        }

        if (this.EntityPM.CustomClearancePointId != null) {
            this.ItemsCollection.push(new PartnerItem(this, "CLERN"));
        }

        if (this.EntityPM.ConsolidatorId != null) {
            this.ItemsCollection.push(new PartnerItem(this, "CONSL"));
        }

        if (this.EntityPM.ReleasingAgentId != null) {
            this.ItemsCollection.push(new PartnerItem(this, "REAGT"));
        }
    }

    get ShowAddPartners() {
        return SessionLocator.TenantPM.IsHybrid ? false : true;
    }
    get IsEditDisabled() {
        return SessionLocator.TenantPM.IsHybrid ? true : false;
    }
   
    public IsAddDisabled_SHIPR: boolean = false;
    public IsAddDisabled_CONSI: boolean = false;
    public IsAddDisabled_AGENT: boolean = false;
    public IsAddDisabled_ISSAG: boolean = false;
    public IsAddDisabled_CSAIM: boolean = false;
    public IsAddDisabled_CSAEX: boolean = false;
    public IsAddDisabled_NOTF1: boolean = false;
    public IsAddDisabled_NOTF2: boolean = false;
    public IsAddDisabled_SHPNT: boolean = false;
    public IsAddDisabled_CONNT: boolean = false;
    public IsAddDisabled_FRTFR: boolean = false;
    public IsAddDisabled_COLOD: boolean = false;
    public IsAddDisabled_CLERN: boolean = false;
    public IsAddDisabled_CONSL: boolean = false;
    public IsAddDisabled_REAGT: boolean = false;
    SetAddButtonsIsDisabled() {
        this.IsAddDisabled_SHIPR = this.EntityPM.ShipperId == null ? false : true;
        this.IsAddDisabled_CONSI = this.EntityPM.ConsigneeId == null ? false : true;
        this.IsAddDisabled_AGENT = this.EntityPM.AgentId == null ? false : true;
        this.IsAddDisabled_ISSAG = this.EntityPM.IssuingCarrierAgentId == null ? false : true;
        this.IsAddDisabled_CSAIM = this.EntityPM.CustomAgentImportId == null ? false : true;
        this.IsAddDisabled_CSAEX = this.EntityPM.CustomAgentExportId == null ? false : true;
        this.IsAddDisabled_NOTF1 = this.EntityPM.Notify1Id == null ? false : true;
        this.IsAddDisabled_NOTF2 = this.EntityPM.Notify2Id == null ? false : true;
        this.IsAddDisabled_SHPNT = this.EntityPM.ShipperNotExporterId == null ? false : true;
        this.IsAddDisabled_CONNT = this.EntityPM.ConsigneeNotImporterId == null ? false : true;
        this.IsAddDisabled_FRTFR = this.EntityPM.FreightForwarderId == null ? false : true;
        this.IsAddDisabled_COLOD = this.EntityPM.ColoaderId == null ? false : true;
        this.IsAddDisabled_CLERN = this.EntityPM.CustomClearancePointId == null ? false : true;
        this.IsAddDisabled_CONSL = this.EntityPM.ConsolidatorId == null ? false : true;
        this.IsAddDisabled_REAGT = this.EntityPM.ReleasingAgentId == null ? false : true;
    }

    AddPartner(myCode: string) {

        var newPartnerItem: PartnerItem = new PartnerItem(this, myCode);

        var myWindowTitle: string;
        switch (myCode) {
            case "COLOD": { myWindowTitle = "Add Coloader"; break }
            case "CLERN": { myWindowTitle = "Add Custom Clearance Point"; break }
            case "CONSL": { myWindowTitle = "Add Consolidator"; break }
            case "REAGT": { myWindowTitle = "Add Releasing Agent"; break }
            default: {
                myWindowTitle = TextCodeTranslator.Translate("Shipment.S.Partners.Add" + newPartnerItem.FullCode);
                break;
            }
        }

        this.RunAddEditPartner(newPartnerItem, myWindowTitle);       
    };
    EditPartner(myPartnerItem: PartnerItem) {

        var myWindowTitle: string;
        switch (myPartnerItem.Code) {
            case "COLOD": { myWindowTitle = "Edit Coloader"; break }
            case "CLERN": { myWindowTitle = "Edit Custom Clearance Point"; break }
            case "CONSL": { myWindowTitle = "Edit Consolidator"; break }
            case "REAGT": { myWindowTitle = "Edit Releasing Agent"; break }
            default: {
                myWindowTitle = TextCodeTranslator.Translate("Shipment.S.Partners.Edit" + myPartnerItem.FullCode);
                break;
            }
        }

        this.RunAddEditPartner(myPartnerItem, myWindowTitle);
    }
    RunAddEditPartner(myPartnerItem: PartnerItem, myWindowTitle: string) {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = myWindowTitle;
        logitudeWindow.DataContext = myPartnerItem;
        logitudeWindow.Show("./ShipmentModules/ShipmentTabs/Components/Partners/AddEditPartnerComponent");
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
            confirmWindow.Show(TextCodeTranslator.Translate("Shipment.M.DeleteThisPartner"));
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
                    this.CurrentSession.FireEvent("ShipmentPartnersChanged");
                }
            });
        }
    }
    SetDefaultCustomer() {
        if (this.EntityPM.ShipmentLevelCode == "C") {
            this.EntityPM.CustomerId = null;
            this.EntityPM.CustomerName = null;
            this.EntityPM.CustomerNote = null;
            this.EntityPM.CustomerReference1 = null;
            this.EntityPM.CustomerReference2 = null;
            this.EntityPM.CustomerAddressId = null;
            this.EntityPM.CustomerContactId = null;
            this.EntityPM.ShipmentCustomerTypeCode = null;
        }

        else {
            if (this.EntityPM.DirectionId == "I") {
                this.EntityPM.ShipmentCustomerTypeCode = "CON";
                this.EntityPM.CustomerId = this.EntityPM.ConsigneeId;
                this.EntityPM.CustomerName = this.EntityPM.ConsigneeName;
                this.EntityPM.CustomerNote = this.EntityPM.ConsigneeNote;
                this.EntityPM.CustomerAddressId = this.EntityPM.ConsigneeAddressId;
                this.EntityPM.CustomerContactId = this.EntityPM.ConsigneeContactId;
                this.EntityPM.CustomerReference1 = this.EntityPM.ConsigneeReference1;
                this.EntityPM.CustomerReference2 = this.EntityPM.ConsigneeReference2;
            }

            else {
                this.EntityPM.ShipmentCustomerTypeCode = "SHI";
                this.EntityPM.CustomerId = this.EntityPM.ShipperId;
                this.EntityPM.CustomerName = this.EntityPM.ShipperName;
                this.EntityPM.CustomerNote = this.EntityPM.ShipperNote;
                this.EntityPM.CustomerAddressId = this.EntityPM.ShipperAddressId;
                this.EntityPM.CustomerContactId = this.EntityPM.ShipperContactId;
                this.EntityPM.CustomerReference1 = this.EntityPM.ShipperReference1;
                this.EntityPM.CustomerReference2 = this.EntityPM.ShipperReference2;
            }
        }

        this.OnCustomerChanged();
    }

    public UpdateSalesmanId: string = null;
    public UpdateSalesmanName: string = null;
    public UpdateSalesmanAccountManagerText: string = null;

    public UpdateAccountManagerId: string = null;
    public UpdateAccountManagerName: string = null;
    public IsUpdateSalesmanAccountManagerVisible: boolean = false;
    private SalesmanUpdated: boolean = false;
    private AccountManagerUpdated: boolean = false;
    OnCustomerChanged() {
        this.UpdateSalesmanId = null;
        this.UpdateSalesmanName = null;
        this.UpdateSalesmanAccountManagerText = null;
        this.UpdateAccountManagerId = null;
        this.UpdateAccountManagerName = null;
        this.IsUpdateSalesmanAccountManagerVisible = false;
        this.SalesmanUpdated = false;
        this.AccountManagerUpdated = false;
        if (this.EntityPM.ShipmentLevelCode != "C") {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.CustomerId)) {
                this.CardListService.getSingle(this.EntityPM.CustomerId).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CardList = myResponse.Result;
                        if (list) {                            
                            if (list.SalesmanUserId != this.EntityPM.SalesmanUserId) {
                                if (AppTool.IsNullOrEmpty(list.SalesmanUserId)) {
                                    this.UpdateSalesmanId = null;
                                    this.UpdateSalesmanName = null;
                                    this.UpdateSalesmanAccountManagerText = "Customer changed, update the salesman to Empty ?";
                                    if (list.AccountManagerUserId != null ? list.AccountManagerUserId != this.EntityPM.AccountManagerUserId : SessionLocator.LoggedUserId != this.EntityPM.AccountManagerUserId) {
                                        if (!AppTool.IsNullOrEmpty(list.AccountManagerUserId)) {
                                            this.UpdateAccountManagerId = list.AccountManagerUserId;
                                            this.UpdateAccountManagerName = list.AccountManagerUserName;
                                            this.UpdateSalesmanAccountManagerText = "Customer changed, update the salesman to Empty , update the account manager to " + list.AccountManagerUserName + " ?";
                                        }
                                        else {
                                            this.UpdateAccountManagerId = SessionLocator.LoggedUserId;
                                            this.UpdateAccountManagerName = SessionLocator.LoggedUserPM.EnglishName;
                                            this.UpdateSalesmanAccountManagerText = "Customer changed, update the salesman to Empty , update the account manager to " + this.UpdateAccountManagerName + " ?";
                                        }
                                        this.AccountManagerUpdated = true;
                                    }
                                }

                                else {
                                    this.UpdateSalesmanId = list.SalesmanUserId;
                                    this.UpdateSalesmanName = list.SalesmanUserEnglishName;
                                    this.UpdateSalesmanAccountManagerText = "Customer changed, update the salesman to " + list.SalesmanUserEnglishName + " ?";
                                    if (list.AccountManagerUserId != null ? list.AccountManagerUserId != this.EntityPM.AccountManagerUserId : SessionLocator.LoggedUserId != this.EntityPM.AccountManagerUserId) {
                                        if (!AppTool.IsNullOrEmpty(list.AccountManagerUserId)) {
                                            this.UpdateAccountManagerId = list.AccountManagerUserId;
                                            this.UpdateAccountManagerName = list.AccountManagerUserName;
                                            this.UpdateSalesmanAccountManagerText = "Customer changed, update the salesman to " + list.SalesmanUserEnglishName + ", update the account manager to " + list.AccountManagerUserName + " ?";
                                        }
                                        else {
                                            this.UpdateAccountManagerId = SessionLocator.LoggedUserId;
                                            this.UpdateAccountManagerName = SessionLocator.LoggedUserPM.EnglishName;
                                            this.UpdateSalesmanAccountManagerText = "Customer changed, update the salesman to " + list.SalesmanUserEnglishName + ", update the account manager to " + this.UpdateAccountManagerName + " ?";
                                        }
                                        this.AccountManagerUpdated = true;
                                    }
                                }

                                this.IsUpdateSalesmanAccountManagerVisible = true;
                                this.SalesmanUpdated = true;
                            }

                            if ((list.AccountManagerUserId != null ? list.AccountManagerUserId != this.EntityPM.AccountManagerUserId : SessionLocator.LoggedUserId != this.EntityPM.AccountManagerUserId) && !this.IsUpdateSalesmanAccountManagerVisible) {
                                if (!AppTool.IsNullOrEmpty(list.AccountManagerUserId)) {
                                    this.UpdateAccountManagerId = list.AccountManagerUserId;
                                    this.UpdateAccountManagerName = list.AccountManagerUserName;
                                    this.UpdateSalesmanAccountManagerText = "Customer changed, update the account manager to " + this.UpdateAccountManagerName + " ?";
                                }
                                else {
                                    this.UpdateAccountManagerId = SessionLocator.LoggedUserId;
                                    this.UpdateAccountManagerName = SessionLocator.LoggedUserPM.EnglishName;
                                    this.UpdateSalesmanAccountManagerText = "Customer changed, update the account manager to " + this.UpdateAccountManagerName + " ?";                                    
                                }
                                this.IsUpdateSalesmanAccountManagerVisible = true;
                                this.AccountManagerUpdated = true;
                            }
                        }
                    }
                });
            }
        }
    }
    UpdateSalesmanClicked() {
        if (this.SalesmanUpdated) {
            this.EntityPM.SalesmanUserId = this.UpdateSalesmanId;
            this.EntityPM.SalesmanUserName = this.UpdateSalesmanName;
        }
        if (this.AccountManagerUpdated) {
            this.EntityPM.AccountManagerUserId = this.UpdateAccountManagerId;
            this.EntityPM.AccountManagerUserName = this.UpdateAccountManagerName;
        }
        this.IsUpdateSalesmanAccountManagerVisible = false;
    }
}
export class PartnerItem extends BaseComponent {
    public EntityPM: ShipmentPM;
    public Code: string;
    public ObjectTableName: string = "Shipment";
    constructor(public fatherComponent: PartnersTabComponent, typeCode: string) {
        super();
        this.EntityPM = fatherComponent.EntityPM;
        this.IsEditingEnabled = fatherComponent.IsEditingEnabled;
        this.Code = typeCode;
        this.InitializeProperties();
        this.SetRemoveButtonVisibility();
        this.GetPartnerAddress();
        this.GetPartnerContact();
    }

    public IsEditingEnabled: boolean = true;
    SetUIProperties() {

        var isPartnerFilled = this.PartnerId == null ? false : true;
        var isFieldsEnabled = false;
        if (this.IsEditingEnabled) {
            if (isPartnerFilled) {
                isFieldsEnabled = true;
            }
        }

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
            case "CSTMR": { this.PartnerIndex = 2; this.FullCode = "Customer"; break }
            case "AGENT": { this.PartnerIndex = 3; this.FullCode = "Agent"; break }
            case "ISSAG": { this.PartnerIndex = 4; this.FullCode = "IssuingCarrierAgent"; break }
            case "CSAEX": { this.PartnerIndex = 5; this.FullCode = "CustomAgentExport"; break }
            case "CSAIM": { this.PartnerIndex = 6; this.FullCode = "CustomAgentImport"; break }
            case "NOTF1": { this.PartnerIndex = 7; this.FullCode = "Notify1"; break }
            case "NOTF2": { this.PartnerIndex = 8; this.FullCode = "Notify2"; break }
            case "SHPNT": { this.PartnerIndex = 9; this.FullCode = "ShipperNotExporter"; break }
            case "CONNT": { this.PartnerIndex = 10; this.FullCode = "ConsigneeNotImporter"; break }
            case "FRTFR": { this.PartnerIndex = 11; this.FullCode = "FreightForwarder"; break }
            case "COLOD": { this.PartnerIndex = 12; this.FullCode = "Coloader"; break }
            case "CLERN": { this.PartnerIndex = 13; this.FullCode = "CustomClearancePoint"; break }
            case "CONSL": { this.PartnerIndex = 14; this.FullCode = "Consolidator"; break }
            case "REAGT": { this.PartnerIndex = 15; this.FullCode = "ReleasingAgent"; break }
        }

        this.PartnerTypeName = TextCodeTranslator.Translate("Shipment.F." + this.FullCode + "Id");
    }

    get CardDependencyProperty1() {
        var myResult: string = null;

        switch (this.Code) {
            case "SHIPR":
            case "CONSI":
            case "CSTMR":
                {
                    myResult = "CS";

                    if (this.EntityPM.ShipmentLevelCode == "C") {
                        myResult = "AG";

                        if (SessionLocator.TenantPM.AllowCustomersInAgentsLOV) {
                            myResult = "CS,AG";
                        }
                    }

                    else {
                        if (SessionLocator.TenantPM.AllowAgentInCustomersLOV) {
                            myResult = "CS,AG";
                        }
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

            case "ISSAG":
            case "FRTFR":
            case "COLOD":
                {
                    myResult = "AG";
                    break;
                }

            case "CSAEX":
            case "CSAIM":
                {
                    myResult = "CG";
                    break;
                }

            case "REAGT": //ReleasingAgent
            case "NOTF1":
            case "NOTF2":
                {
                    myResult = "AG,AL,CG,CS,SG,SL,TR,VD,WH";
                    break;
                }

            case "SHPNT":
            case "CONNT":
                {
                    myResult = "AG,CS";
                    break;
                }
            case "CONSL":
                {
                    myResult = "AG,CS,SG";
                    break;
                }

            case "CLERN":
                {
                    myResult = "WH";
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
                {
                    if (this.EntityPM.ShipmentLevelCode == "C") {
                        if (SessionLocator.TenantPM.AllowCustomersInAgentsLOV) {
                            myResult = true;
                        }
                    }

                    else {
                        if (SessionLocator.TenantPM.AllowAgentInCustomersLOV) {
                            myResult = true;
                        }
                    }

                    break
                }

            case "AGENT":
                {
                    if (SessionLocator.TenantPM.AllowCustomersInAgentsLOV) {
                        myResult = true;
                    }

                    break;
                }

            case "REAGT":
            case "NOTF1":
            case "NOTF2":
            case "SHPNT":
            case "CONNT":
            case "CONSL":
                {
                    myResult = true;
                    break;
                }

            default: {
                myResult = false;
                break;
            }
        }

        return myResult;
    }

    get Name() {
        switch (this.Code) {
            case "SHIPR": { return this.EntityPM.ShipperName; }
            case "CONSI": { return this.EntityPM.ConsigneeName; }
            case "AGENT": { return this.EntityPM.AgentName; }
            case "CSTMR": { return this.EntityPM.CustomerName; }
            case "ISSAG": { return this.EntityPM.IssuingCarrierAgentName; }
            case "CSAEX": { return this.EntityPM.CustomAgentExportName; }
            case "CSAIM": { return this.EntityPM.CustomAgentImportName; }
            case "NOTF1": { return this.EntityPM.Notify1Name; }
            case "NOTF2": { return this.EntityPM.Notify2Name; }
            case "SHPNT": { return this.EntityPM.ShipperNotExporterName; }
            case "CONNT": { return this.EntityPM.ConsigneeNotImporterName; }
            case "FRTFR": { return this.EntityPM.FreightForwarderName; }
            case "COLOD": { return this.EntityPM.ColoaderName; }
            case "CLERN": { return this.EntityPM.CustomClearancePointName; }
            case "CONSL": { return this.EntityPM.ConsolidatorName; }
            case "REAGT": { return this.EntityPM.ReleasingAgentName; }
            default: { return null; }
        }
    }
    set Name(newValue: string) {
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

            case "ISSAG": {
                if (this.EntityPM.IssuingCarrierAgentName != newValue) {
                    this.EntityPM.IssuingCarrierAgentName = newValue
                }

                break;
            }

            case "CSAEX": {
                if (this.EntityPM.CustomAgentExportName != newValue) {
                    this.EntityPM.CustomAgentExportName = newValue
                    break;
                }
            }

            case "CSAIM": {
                if (this.EntityPM.CustomAgentImportName != newValue) {
                    this.EntityPM.CustomAgentImportName = newValue
                }

                break;
            }

            case "NOTF1": {
                if (this.EntityPM.Notify1Name != newValue) {
                    this.EntityPM.Notify1Name = newValue
                    break;
                }
            }

            case "NOTF2": {
                if (this.EntityPM.Notify2Name != newValue) {
                    this.EntityPM.Notify2Name = newValue
                }

                break;
            }

            case "SHPNT": {
                if (this.EntityPM.ShipperNotExporterName != newValue) {
                    this.EntityPM.ShipperNotExporterName = newValue
                }

                break;
            }

            case "CONNT": {
                if (this.EntityPM.ConsigneeNotImporterName != newValue) {
                    this.EntityPM.ConsigneeNotImporterName = newValue
                }

                break;
            }

            case "FRTFR": {
                if (this.EntityPM.FreightForwarderName != newValue) {
                    this.EntityPM.FreightForwarderName = newValue
                }

                break;
            }

            case "COLOD": {
                if (this.EntityPM.ColoaderName != newValue) {
                    this.EntityPM.ColoaderName = newValue
                }

                break;
            }

            case "CLERN": {
                if (this.EntityPM.CustomClearancePointName != newValue) {
                    this.EntityPM.CustomClearancePointName = newValue
                }

                break;
            }

            case "CONSL": {
                if (this.EntityPM.ConsolidatorName != newValue) {
                    this.EntityPM.ConsolidatorName = newValue
                }

                break;
            }
            case "REAGT": {
                if (this.EntityPM.ReleasingAgentName != newValue) {
                    this.EntityPM.ReleasingAgentName = newValue
                }

                break;
            }
        }
    }

    get Note() {
        switch (this.Code) {
            case "SHIPR": { return this.EntityPM.ShipperNote; }
            case "CONSI": { return this.EntityPM.ConsigneeNote; }
            case "AGENT": { return this.EntityPM.AgentNote; }
            case "CSTMR": { return this.EntityPM.CustomerNote; }
            case "ISSAG": { return this.EntityPM.IssuingCarrierAgentNote; }
            case "CSAEX": { return this.EntityPM.CustomAgentExportNote; }
            case "CSAIM": { return this.EntityPM.CustomAgentImportNote; }
            case "NOTF1": { return this.EntityPM.Notify1Note; }
            case "NOTF2": { return this.EntityPM.Notify2Note; }
            case "SHPNT": { return this.EntityPM.ShipperNotExporterNote; }
            case "CONNT": { return this.EntityPM.ConsigneeNotImporterNote; }
            case "FRTFR": { return this.EntityPM.FreightForwarderNote; }
            case "COLOD": { return this.EntityPM.ColoaderNote; }
            case "CLERN": { return this.EntityPM.CustomClearancePointNote; }
            case "CONSL": { return this.EntityPM.ConsolidatorNote; }
            case "REAGT": { return this.EntityPM.ReleasingAgentNote; }
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

            case "AGENT": {
                if (this.EntityPM.AgentNote != newValue) {
                    this.EntityPM.AgentNote = newValue
                }

                break;
            }

            case "CSTMR": {
                if (this.EntityPM.CustomerNote != newValue) {
                    this.EntityPM.CustomerNote = newValue
                }

                break;
            }

            case "ISSAG": {
                if (this.EntityPM.IssuingCarrierAgentNote != newValue) {
                    this.EntityPM.IssuingCarrierAgentNote = newValue
                }

                break;
            }

            case "CSAEX": {
                if (this.EntityPM.CustomAgentExportNote != newValue) {
                    this.EntityPM.CustomAgentExportNote = newValue
                }

                break;
            }

            case "CSAIM": {
                if (this.EntityPM.CustomAgentImportNote != newValue) {
                    this.EntityPM.CustomAgentImportNote = newValue
                }

                break;
            }

            case "NOTF1": {
                if (this.EntityPM.Notify1Note != newValue) {
                    this.EntityPM.Notify1Note = newValue
                }

                break;
            }

            case "NOTF2": {
                if (this.EntityPM.Notify2Note != newValue) {
                    this.EntityPM.Notify2Note = newValue
                }

                break;
            }

            case "SHPNT": {
                if (this.EntityPM.ShipperNotExporterNote != newValue) {
                    this.EntityPM.ShipperNotExporterNote = newValue
                }

                break;
            }

            case "CONNT": {
                if (this.EntityPM.ConsigneeNotImporterNote != newValue) {
                    this.EntityPM.ConsigneeNotImporterNote = newValue
                }

                break;
            }

            case "FRTFR": {
                if (this.EntityPM.FreightForwarderNote != newValue) {
                    this.EntityPM.FreightForwarderNote = newValue
                }

                break;
            }

            case "COLOD": {
                if (this.EntityPM.ColoaderNote != newValue) {
                    this.EntityPM.ColoaderNote = newValue
                }

                break;
            }

            case "CLERN": {
                if (this.EntityPM.CustomClearancePointNote != newValue) {
                    this.EntityPM.CustomClearancePointNote = newValue
                }

                break;
            }

            case "CONSL": {
                if (this.EntityPM.ConsolidatorNote != newValue) {
                    this.EntityPM.ConsolidatorNote = newValue
                }

                break;
            }
            case "REAGT": {
                if (this.EntityPM.ReleasingAgentNote != newValue) {
                    this.EntityPM.ReleasingAgentNote = newValue
                }

                break;
            }
        }
    }

    get IsCustomer() {
        return (this.PartnerId == this.EntityPM.CustomerId) ? true : false;
    }

    SetAsCustomer() {

        //|| this.CardDependencyProperty1 != "CS"
        if (this.EntityPM.ShipmentLevelCode == "C") {
            this.EntityPM.CustomerName = null;
            this.EntityPM.CustomerNote = null;
            this.EntityPM.CustomerId = null;
            this.EntityPM.CustomerAddressId = null;
            this.EntityPM.CustomerContactId = null;
            this.EntityPM.CustomerReference1 = null;
            this.EntityPM.CustomerReference2 = null;
            this.EntityPM.CustomerRankName = null;
            this.EntityPM.CustomerShipmentNumber = null;
            this.EntityPM.ShipmentCustomerTypeCode = null;
        }

        else {
            this.EntityPM.CustomerName = this.Name;
            this.EntityPM.CustomerNote = this.Note;
            this.EntityPM.CustomerId = this.PartnerId;
            this.EntityPM.CustomerAddressId = this.AddressId;
            this.EntityPM.CustomerContactId = this.ContactId;
            this.EntityPM.CustomerReference1 = this.Reference1;
            this.EntityPM.CustomerReference2 = this.Reference2;
            this.EntityPM.CustomerShipmentNumber = this.EntityPM.ShipmentNumber;

            switch (this.Code) {
                case "SHIPR": { this.EntityPM.ShipmentCustomerTypeCode = "SHI"; break; }
                case "CONSI": { this.EntityPM.ShipmentCustomerTypeCode = "CON"; break; }
                case "AGENT": { this.EntityPM.ShipmentCustomerTypeCode = "AGT"; break; }
                case "ISSAG": { this.EntityPM.ShipmentCustomerTypeCode = "IGT"; break; }
                case "CSAEX": { this.EntityPM.ShipmentCustomerTypeCode = "CAE"; break; }
                case "CSAIM": { this.EntityPM.ShipmentCustomerTypeCode = "CAI"; break; }
                case "NOTF1": { this.EntityPM.ShipmentCustomerTypeCode = "NT1"; break; }
                case "NOTF2": { this.EntityPM.ShipmentCustomerTypeCode = "NT2"; break; }
                case "SHPNT": { this.EntityPM.ShipmentCustomerTypeCode = "SNE"; break; }
                case "CONNT": { this.EntityPM.ShipmentCustomerTypeCode = "CNI"; break; }
                case "FRTFR": { this.EntityPM.ShipmentCustomerTypeCode = "FOR"; break; }
                case "COLOD": { this.EntityPM.ShipmentCustomerTypeCode = "COL"; break; }
                case "CLERN": { this.EntityPM.ShipmentCustomerTypeCode = "CCP"; break; }
                case "CONSL": { this.EntityPM.ShipmentCustomerTypeCode = "CSD"; break; }
                case "REAGT": { this.EntityPM.ShipmentCustomerTypeCode = "REA"; break; }
                default: { this.EntityPM.ShipmentCustomerTypeCode = "OTH"; break; }
            }
        }

        this.fatherComponent.OnCustomerChanged();
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
    public PartnerCardList: CardList = null;

    get PartnerIdProperty() {
        switch (this.Code) {
            case "SHIPR": { return "ShipperId"; }
            case "CONSI": { return "ConsigneeId"; }
            case "AGENT": { return "AgentId"; }
            case "CSTMR": { return "CustomerId"; }
            case "ISSAG": { return "IssuingCarrierAgentId"; }
            case "CSAEX": { return "CustomAgentExportId"; }
            case "CSAIM": { return "CustomAgentImportId"; }
            case "NOTF1": { return "Notify1Id"; }
            case "NOTF2": { return "Notify2Id"; }
            case "SHPNT": { return "ShipperNotExporterId"; }
            case "CONNT": { return "ConsigneeNotImporterId"; }
            case "FRTFR": { return "FreightForwarderId"; }
            case "COLOD": { return "ColoaderId"; }
            case "CLERN": { return "CustomClearancePointId"; }
            case "CONSL": { return "ConsolidatorId"; }
            case "REAGT": { return "ReleasingAgentId"; }
            default: { return null; }
        }
    }

    get PartnerId() {
        switch (this.Code) {
            case "SHIPR": { return this.ShipperId; }
            case "CONSI": { return this.ConsigneeId; }
            case "AGENT": { return this.AgentId; }
            case "CSTMR": { return this.CustomerId; }
            case "ISSAG": { return this.IssuingCarrierAgentId; }
            case "CSAEX": { return this.CustomAgentExportId; }
            case "CSAIM": { return this.CustomAgentImportId; }
            case "NOTF1": { return this.Notify1Id; }
            case "NOTF2": { return this.Notify2Id; }
            case "SHPNT": { return this.ShipperNotExporterId; }
            case "CONNT": { return this.ConsigneeNotImporterId; }
            case "FRTFR": { return this.FreightForwarderId; }
            case "COLOD": { return this.ColoaderId; }
            case "CLERN": { return this.CustomClearancePointId; }
            case "CONSL": { return this.ConsolidatorId; }
            case "REAGT": { return this.ReleasingAgentId; }
            default: { return null; }
        }
    }
    set PartnerId(newValue: string) {
        switch (this.Code) {
            case "SHIPR": { this.ShipperId = newValue; break; }
            case "CONSI": { this.ConsigneeId = newValue; break; }
            case "AGENT": { this.AgentId = newValue; break; }
            case "CSTMR": { this.CustomerId = newValue; break; }
            case "ISSAG": { this.IssuingCarrierAgentId = newValue; break; }
            case "CSAEX": { this.CustomAgentExportId = newValue; break; }
            case "CSAIM": { this.CustomAgentImportId = newValue; break; }
            case "NOTF1": { this.Notify1Id = newValue; break; }
            case "NOTF2": { this.Notify2Id = newValue; break; }
            case "SHPNT": { this.ShipperNotExporterId = newValue; break; }
            case "CONNT": { this.ConsigneeNotImporterId = newValue; break; }
            case "FRTFR": { this.FreightForwarderId = newValue; break; }
            case "COLOD": { this.ColoaderId = newValue; break; }
            case "CLERN": { this.CustomClearancePointId = newValue; break; }
            case "CONSL": { this.ConsolidatorId = newValue; break; }
            case "REAGT": { this.ReleasingAgentId = newValue; break; }
        }
    }

    get ShipperId() {
        return this.EntityPM.ShipperId;
    }
    set ShipperId(newValue: string) {
        if (this.EntityPM.ShipperId != newValue) {
            this.EntityPM.ShipperId = newValue;
            this.GetPartnerCard();
        }
    }

    get ConsigneeId() {
        return this.EntityPM.ConsigneeId;
    }
    set ConsigneeId(newValue: string) {
        if (this.EntityPM.ConsigneeId != newValue) {
            this.EntityPM.ConsigneeId = newValue;
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

    private isPartnerChanged_Issuing: boolean = false;
    get IssuingCarrierAgentId() {
        return this.EntityPM.IssuingCarrierAgentId;
    }
    set IssuingCarrierAgentId(newValue: string) {
        if (this.EntityPM.IssuingCarrierAgentId != newValue) {
            this.EntityPM.IssuingCarrierAgentId = newValue;
            this.isPartnerChanged_Issuing = true;
            this.GetPartnerCard();
        }
    }

    get CustomAgentExportId() {
        return this.EntityPM.CustomAgentExportId;
    }
    set CustomAgentExportId(newValue: string) {
        if (this.EntityPM.CustomAgentExportId != newValue) {
            this.EntityPM.CustomAgentExportId = newValue;
            this.GetPartnerCard();
        }
    }

    get CustomAgentImportId() {
        return this.EntityPM.CustomAgentImportId;
    }
    set CustomAgentImportId(newValue: string) {
        if (this.EntityPM.CustomAgentImportId != newValue) {
            this.EntityPM.CustomAgentImportId = newValue;
            this.GetPartnerCard();
        }
    }

    get Notify1Id() {
        return this.EntityPM.Notify1Id;
    }
    set Notify1Id(newValue: string) {
        if (this.EntityPM.Notify1Id != newValue) {
            this.EntityPM.Notify1Id = newValue;
            this.GetPartnerCard();
        }
    }

    get Notify2Id() {
        return this.EntityPM.Notify2Id;
    }
    set Notify2Id(newValue: string) {
        if (this.EntityPM.Notify2Id != newValue) {
            this.EntityPM.Notify2Id = newValue;
            this.GetPartnerCard();
        }
    }

    get ShipperNotExporterId() {
        return this.EntityPM.ShipperNotExporterId;
    }
    set ShipperNotExporterId(newValue: string) {
        if (this.EntityPM.ShipperNotExporterId != newValue) {
            this.EntityPM.ShipperNotExporterId = newValue;
            this.GetPartnerCard();
        }
    }

    get ConsigneeNotImporterId() {
        return this.EntityPM.ConsigneeNotImporterId;
    }
    set ConsigneeNotImporterId(newValue: string) {
        if (this.EntityPM.ConsigneeNotImporterId != newValue) {
            this.EntityPM.ConsigneeNotImporterId = newValue;
            this.GetPartnerCard();
        }
    }

    get FreightForwarderId() {
        return this.EntityPM.FreightForwarderId;
    }
    set FreightForwarderId(newValue: string) {
        if (this.EntityPM.FreightForwarderId != newValue) {
            this.EntityPM.FreightForwarderId = newValue;
            this.GetPartnerCard();
        }
    }

    get ColoaderId() {
        return this.EntityPM.ColoaderId;
    }
    set ColoaderId(newValue: string) {
        if (this.EntityPM.ColoaderId != newValue) {
            this.EntityPM.ColoaderId = newValue;
            this.GetPartnerCard();
        }
    }

    get CustomClearancePointId() {
        return this.EntityPM.CustomClearancePointId;
    }
    set CustomClearancePointId(newValue: string) {
        if (this.EntityPM.CustomClearancePointId != newValue) {
            this.EntityPM.CustomClearancePointId = newValue;
            this.GetPartnerCard();
        }
    }

    get ConsolidatorId() {
        return this.EntityPM.ConsolidatorId;
    }
    set ConsolidatorId(newValue: string) {
        if (this.EntityPM.ConsolidatorId != newValue) {
            this.EntityPM.ConsolidatorId = newValue;
            this.GetPartnerCard();
        }
    }

    get ReleasingAgentId() {
        return this.EntityPM.ReleasingAgentId;
    }
    set ReleasingAgentId(newValue: string) {
        if (this.EntityPM.ReleasingAgentId != newValue) {
            this.EntityPM.ReleasingAgentId = newValue;
            this.GetPartnerCard();
        }
    }

    // AddressId
    get PartnerAddressIdProperty() {
        switch (this.Code) {
            case "SHIPR": { return "ShipperAddressId"; }
            case "CONSI": { return "ConsigneeAddressId"; }
            case "AGENT": { return "AgentAddressId"; }
            case "CSTMR": { return "CustomerAddressId"; }
            case "ISSAG": { return "IssuingCarrierAddressId"; }
            case "CSAEX": { return "CustomAgentExportAddressId"; }
            case "CSAIM": { return "CustomAgentImportAddressId"; }
            case "NOTF1": { return "Notify1AddressId"; }
            case "NOTF2": { return "Notify2AddressId"; }
            case "SHPNT": { return "ShipperNotExporterAddressId"; }
            case "CONNT": { return "ConsigneeNotImporterAddressId"; }
            case "FRTFR": { return "FreightForwarderAddressId"; }
            case "COLOD": { return "ColoaderAddressId"; }
            case "CLERN": { return "CustomClearancePointAddressId"; }
            case "CONSL": { return "ConsolidatorAddressId"; }
            case "REAGT": { return "ReleasingAgentAddressId"; }
            default: { return null; }
        }
    }

    get AddressId() {
        switch (this.Code) {
            case "SHIPR": { return this.ShipperAddressId; }
            case "CONSI": { return this.ConsigneeAddressId; }
            case "AGENT": { return this.AgentAddressId; }
            case "CSTMR": { return this.CustomerAddressId; }
            case "ISSAG": { return this.IssuingCarrierAddressId; }
            case "CSAEX": { return this.CustomAgentExportAddressId; }
            case "CSAIM": { return this.CustomAgentImportAddressId; }
            case "NOTF1": { return this.Notify1AddressId; }
            case "NOTF2": { return this.Notify2AddressId; }
            case "SHPNT": { return this.ShipperNotExporterAddressId; }
            case "CONNT": { return this.ConsigneeNotImporterAddressId; }
            case "FRTFR": { return this.FreightForwarderAddressId; }
            case "COLOD": { return this.ColoaderAddressId; }
            case "CLERN": { return this.CustomClearancePointAddressId; }
            case "CONSL": { return this.ConsolidatorAddressId; }
            case "REAGT": { return this.ReleasingAgentAddressId; }

            default: { return null; }
        }
    }
    set AddressId(newValue: string) {
        switch (this.Code) {
            case "SHIPR": { this.ShipperAddressId = newValue; break; }
            case "CONSI": { this.ConsigneeAddressId = newValue; break; }
            case "AGENT": { this.AgentAddressId = newValue; break; }
            case "CSTMR": { this.CustomerAddressId = newValue; break; }
            case "ISSAG": { this.IssuingCarrierAddressId = newValue; break; }
            case "CSAEX": { this.CustomAgentExportAddressId = newValue; break; }
            case "CSAIM": { this.CustomAgentImportAddressId = newValue; break; }
            case "NOTF1": { this.Notify1AddressId = newValue; break; }
            case "NOTF2": { this.Notify2AddressId = newValue; break; }
            case "SHPNT": { this.ShipperNotExporterAddressId = newValue; break; }
            case "CONNT": { this.ConsigneeNotImporterAddressId = newValue; break; }
            case "FRTFR": { this.FreightForwarderAddressId = newValue; break; }
            case "COLOD": { this.ColoaderAddressId = newValue; break; }
            case "CLERN": { this.CustomClearancePointAddressId = newValue; break; }
            case "CONSL": { this.ConsolidatorAddressId = newValue; break; }
            case "REAGT": { this.ReleasingAgentAddressId = newValue; break; }

        }
    }

    get ShipperAddressId() {
        return this.EntityPM.ShipperAddressId;
    }
    set ShipperAddressId(newValue: string) {
        if (this.EntityPM.ShipperAddressId != newValue) {
            this.EntityPM.ShipperAddressId = newValue;
            this.GetPartnerAddress();
        }
    }

    get ConsigneeAddressId() {
        return this.EntityPM.ConsigneeAddressId;
    }
    set ConsigneeAddressId(newValue: string) {
        if (this.EntityPM.ConsigneeAddressId != newValue) {
            this.EntityPM.ConsigneeAddressId = newValue;
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

    get CustomerAddressId() {
        return this.EntityPM.CustomerAddressId;
    }
    set CustomerAddressId(newValue: string) {
        if (this.EntityPM.CustomerAddressId != newValue) {
            this.EntityPM.CustomerAddressId = newValue;
            this.GetPartnerAddress();
        }
    }

    get IssuingCarrierAddressId() {
        return this.EntityPM.IssuingCarrierAddressId;
    }
    set IssuingCarrierAddressId(newValue: string) {
        if (this.EntityPM.IssuingCarrierAddressId != newValue) {
            this.EntityPM.IssuingCarrierAddressId = newValue;
            this.GetPartnerAddress();
        }
    }

    get CustomAgentExportAddressId() {
        return this.EntityPM.CustomAgentExportAddressId;
    }
    set CustomAgentExportAddressId(newValue: string) {
        if (this.EntityPM.CustomAgentExportAddressId != newValue) {
            this.EntityPM.CustomAgentExportAddressId = newValue;
            this.GetPartnerAddress();
        }
    }

    get CustomAgentImportAddressId() {
        return this.EntityPM.CustomAgentImportAddressId;
    }
    set CustomAgentImportAddressId(newValue: string) {
        if (this.EntityPM.CustomAgentImportAddressId != newValue) {
            this.EntityPM.CustomAgentImportAddressId = newValue;
            this.GetPartnerAddress();
        }
    }

    get Notify1AddressId() {
        return this.EntityPM.Notify1AddressId;
    }
    set Notify1AddressId(newValue: string) {
        if (this.EntityPM.Notify1AddressId != newValue) {
            this.EntityPM.Notify1AddressId = newValue;
            this.GetPartnerAddress();
        }
    }

    get Notify2AddressId() {
        return this.EntityPM.Notify2AddressId;
    }
    set Notify2AddressId(newValue: string) {
        if (this.EntityPM.Notify2AddressId != newValue) {
            this.EntityPM.Notify2AddressId = newValue;
            this.GetPartnerAddress();
        }
    }

    get ShipperNotExporterAddressId() {
        return this.EntityPM.ShipperNotExporterAddressId;
    }
    set ShipperNotExporterAddressId(newValue: string) {
        if (this.EntityPM.ShipperNotExporterAddressId != newValue) {
            this.EntityPM.ShipperNotExporterAddressId = newValue;
            this.GetPartnerAddress();
        }
    }

    get ConsigneeNotImporterAddressId() {
        return this.EntityPM.ConsigneeNotImporterAddressId;
    }
    set ConsigneeNotImporterAddressId(newValue: string) {
        if (this.EntityPM.ConsigneeNotImporterAddressId != newValue) {
            this.EntityPM.ConsigneeNotImporterAddressId = newValue;
            this.GetPartnerAddress();
        }
    }

    get FreightForwarderAddressId() {
        return this.EntityPM.FreightForwarderAddressId;
    }
    set FreightForwarderAddressId(newValue: string) {
        if (this.EntityPM.FreightForwarderAddressId != newValue) {
            this.EntityPM.FreightForwarderAddressId = newValue;
            this.GetPartnerAddress();
        }
    }

    get ColoaderAddressId() {
        return this.EntityPM.ColoaderAddressId;
    }
    set ColoaderAddressId(newValue: string) {
        if (this.EntityPM.ColoaderAddressId != newValue) {
            this.EntityPM.ColoaderAddressId = newValue;
            this.GetPartnerAddress();
        }
    }

    get CustomClearancePointAddressId() {
        return this.EntityPM.CustomClearancePointAddressId;
    }
    set CustomClearancePointAddressId(newValue: string) {
        if (this.EntityPM.CustomClearancePointAddressId != newValue) {
            this.EntityPM.CustomClearancePointAddressId = newValue;
            this.GetPartnerAddress();
        }
    }

    get ConsolidatorAddressId() {
        return this.EntityPM.ConsolidatorAddressId;
    }
    set ConsolidatorAddressId(newValue: string) {
        if (this.EntityPM.ConsolidatorAddressId != newValue) {
            this.EntityPM.ConsolidatorAddressId = newValue;
            this.GetPartnerAddress();
        }
    }
    get ReleasingAgentAddressId() {
        return this.EntityPM.ReleasingAgentAddressId;
    }
    set ReleasingAgentAddressId(newValue: string) {
        if (this.EntityPM.ReleasingAgentAddressId != newValue) {
            this.EntityPM.ReleasingAgentAddressId = newValue;
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
            //case "ISSAG": { return "IssuingCarrierContactId"; }
            case "CSAEX": { return "CustomAgentExportContactId"; }
            case "CSAIM": { return "CustomAgentImportContactId"; }
            case "NOTF1": { return "Notify1ContactId"; }
            case "NOTF2": { return "Notify2ContactId"; }
            case "SHPNT": { return "ShipperNotExporterContactId"; }
            case "CONNT": { return "ConsigneeNotImporterContactId"; }
            case "FRTFR": { return "FreightForwarderContactId"; }
            case "COLOD": { return "ColoaderContactId"; }
            case "CLERN": { return "CustomClearancePointContactId"; }
            case "CONSL": { return "ConsolidatorContactId"; }
            case "REAGT": { return "ReleasingAgentContactId"; }
            default: { return null; }
        }
    }

    get ContactId() {
        switch (this.Code) {
            case "SHIPR": { return this.ShipperContactId; }
            case "CONSI": { return this.ConsigneeContactId; }
            case "AGENT": { return this.AgentContactId; }
            case "CSTMR": { return this.CustomerContactId; }
            //case "ISSAG": { return this.IssuingCarrierContactId; }
            case "CSAEX": { return this.CustomAgentExportContactId; }
            case "CSAIM": { return this.CustomAgentImportContactId; }
            case "NOTF1": { return this.Notify1ContactId; }
            case "NOTF2": { return this.Notify2ContactId; }
            case "SHPNT": { return this.ShipperNotExporterContactId; }
            case "CONNT": { return this.ConsigneeNotImporterContactId; }
            case "FRTFR": { return this.FreightForwarderContactId; }
            case "COLOD": { return this.ColoaderContactId; }
            case "CLERN": { return this.CustomClearancePointContactId; }
            case "CONSL": { return this.ConsolidatorContactId; }
            case "REAGT": { return this.ReleasingAgentContactId; }

            default: { return null; }
        }
    }
    set ContactId(newValue: string) {
        switch (this.Code) {
            case "SHIPR": { this.ShipperContactId = newValue; break; }
            case "CONSI": { this.ConsigneeContactId = newValue; break; }
            case "AGENT": { this.AgentContactId = newValue; break; }
            case "CSTMR": { this.CustomerContactId = newValue; break; }
            //case "ISSAG": { this.IssuingCarrierContactId = newValue; break; }
            case "CSAEX": { this.CustomAgentExportContactId = newValue; break; }
            case "CSAIM": { this.CustomAgentImportContactId = newValue; break; }
            case "NOTF1": { this.Notify1ContactId = newValue; break; }
            case "NOTF2": { this.Notify2ContactId = newValue; break; }
            case "SHPNT": { this.ShipperNotExporterContactId = newValue; break; }
            case "CONNT": { this.ConsigneeNotImporterContactId = newValue; break; }
            case "FRTFR": { this.FreightForwarderContactId = newValue; break; }
            case "COLOD": { this.ColoaderContactId = newValue; break; }
            case "CLERN": { this.CustomClearancePointContactId = newValue; break; }
            case "CONSL": { this.ConsolidatorContactId = newValue; break; }
            case "REAGT": { this.ReleasingAgentContactId = newValue; break; }
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

    get CustomAgentExportContactId() {
        return this.EntityPM.CustomAgentExportContactId;
    }
    set CustomAgentExportContactId(newValue: string) {
        if (this.EntityPM.CustomAgentExportContactId != newValue) {
            this.EntityPM.CustomAgentExportContactId = newValue;
            this.GetPartnerContact();
        }
    }

    get CustomAgentImportContactId() {
        return this.EntityPM.CustomAgentImportContactId;
    }
    set CustomAgentImportContactId(newValue: string) {
        if (this.EntityPM.CustomAgentImportContactId != newValue) {
            this.EntityPM.CustomAgentImportContactId = newValue;
            this.GetPartnerContact();
        }
    }

    get Notify1ContactId() {
        return this.EntityPM.Notify1ContactId;
    }
    set Notify1ContactId(newValue: string) {
        if (this.EntityPM.Notify1ContactId != newValue) {
            this.EntityPM.Notify1ContactId = newValue;
            this.GetPartnerContact();
        }
    }

    get Notify2ContactId() {
        return this.EntityPM.Notify2ContactId;
    }
    set Notify2ContactId(newValue: string) {
        if (this.EntityPM.Notify2ContactId != newValue) {
            this.EntityPM.Notify2ContactId = newValue;
            this.GetPartnerContact();
        }
    }

    get ShipperNotExporterContactId() {
        return this.EntityPM.ShipperNotExporterContactId;
    }
    set ShipperNotExporterContactId(newValue: string) {
        if (this.EntityPM.ShipperNotExporterContactId != newValue) {
            this.EntityPM.ShipperNotExporterContactId = newValue;
            this.GetPartnerContact();
        }
    }

    get ConsigneeNotImporterContactId() {
        return this.EntityPM.ConsigneeNotImporterContactId;
    }
    set ConsigneeNotImporterContactId(newValue: string) {
        if (this.EntityPM.ConsigneeNotImporterContactId != newValue) {
            this.EntityPM.ConsigneeNotImporterContactId = newValue;
            this.GetPartnerContact();
        }
    }

    get FreightForwarderContactId() {
        return this.EntityPM.FreightForwarderContactId;
    }
    set FreightForwarderContactId(newValue: string) {
        if (this.EntityPM.FreightForwarderContactId != newValue) {
            this.EntityPM.FreightForwarderContactId = newValue;
            this.GetPartnerContact();
        }
    }

    get ColoaderContactId() {
        return this.EntityPM.ColoaderContactId;
    }
    set ColoaderContactId(newValue: string) {
        if (this.EntityPM.ColoaderContactId != newValue) {
            this.EntityPM.ColoaderContactId = newValue;
            this.GetPartnerContact();
        }
    }

    get CustomClearancePointContactId() {
        return this.EntityPM.CustomClearancePointContactId;
    }
    set CustomClearancePointContactId(newValue: string) {
        if (this.EntityPM.CustomClearancePointContactId != newValue) {
            this.EntityPM.CustomClearancePointContactId = newValue;
            this.GetPartnerContact();
        }
    }

    get ConsolidatorContactId() {
        return this.EntityPM.ConsolidatorContactId;
    }
    set ConsolidatorContactId(newValue: string) {
        if (this.EntityPM.ConsolidatorContactId != newValue) {
            this.EntityPM.ConsolidatorContactId = newValue;
            this.GetPartnerContact();
        }
    }

    get ReleasingAgentContactId() {
        return this.EntityPM.ReleasingAgentContactId;
    }
    set ReleasingAgentContactId(newValue: string) {
        if (this.EntityPM.ReleasingAgentContactId != newValue) {
            this.EntityPM.ReleasingAgentContactId = newValue;
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
            case "ISSAG":
            case "CSAEX":
            case "CSAIM":
            case "FRTFR":
            case "COLOD":
            case "CLERN":
            case "REAGT":
            case "CONSL":
            case "NOTF1":
            case "NOTF2":
            case "SHPNT":
            case "CONNT":
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
            case "ISSAG": { return "IssuingCarrierReference1"; }
            case "CSAEX": { return "CustomAgentExportReference"; }
            case "CSAIM": { return "CustomAgentImportReference"; }
            case "FRTFR": { return "FreightForwarderReference"; }
            case "COLOD": { return "ColoaderReference1"; }
            case "CLERN": { return "CustomClearancePointReference1"; }
            case "CONSL": { return "ConsolidatorReference"; }
            case "REAGT": { return "ReleasingAgentReference1"; }
            case "NOTF1": { return "Notify1Reference"; }
            case "NOTF2": { return "Notify2Reference"; }
            case "SHPNT": { return "ShipperNotExporterReference"; }
            case "CONNT": { return "ConsigneeNotImporterReference"; }
            default: { return null; }
        }
    }
    get Reference1() {
        switch (this.Code) {
            case "SHIPR": { return this.ShipperReference1; }
            case "CONSI": { return this.ConsigneeReference1; }
            case "AGENT": { return this.AgentReference1; }
            case "CSTMR": { return this.CustomerReference1; }
            case "ISSAG": { return this.IssuingCarrierReference1; }
            case "CSAEX": { return this.CustomAgentExportReference; }
            case "CSAIM": { return this.CustomAgentImportReference; }
            case "FRTFR": { return this.FreightForwarderReference; }
            case "COLOD": { return this.ColoaderReference1; }
            case "CLERN": { return this.CustomClearancePointReference1; }
            case "CONSL": { return this.ConsolidatorReference; }
            case "REAGT": { return this.ReleasingAgentReference1; }
            case "NOTF1": { return this.Notify1Reference; }
            case "NOTF2": { return this.Notify2Reference; }
            case "SHPNT": { return this.ShipperNotExporterReference; }
            case "CONNT": { return this.ConsigneeNotImporterReference; }
            default: { return null; }
        }
    }
    set Reference1(newValue: string) {
        switch (this.Code) {
            case "SHIPR": { this.ShipperReference1 = newValue; break; }
            case "CONSI": { this.ConsigneeReference1 = newValue; break; }
            case "AGENT": { this.AgentReference1 = newValue; break; }
            case "CSTMR": { this.CustomerReference1 = newValue; break; }
            case "ISSAG": { this.IssuingCarrierReference1 = newValue; break; }
            case "CSAEX": { this.CustomAgentExportReference = newValue; break; }
            case "CSAIM": { this.CustomAgentImportReference = newValue; break; }
            case "FRTFR": { this.FreightForwarderReference = newValue; break; }
            case "COLOD": { this.ColoaderReference1 = newValue; break; }
            case "CLERN": { this.CustomClearancePointReference1 = newValue; break; }
            case "CONSL": { this.ConsolidatorReference = newValue; break; }
            case "REAGT": { this.ReleasingAgentReference1 = newValue; break; }
            case "NOTF1": { this.Notify1Reference = newValue; break; }
            case "NOTF2": { this.Notify2Reference = newValue; break; }
            case "SHPNT": { this.ShipperNotExporterReference = newValue; break; }
            case "CONNT": { this.ConsigneeNotImporterReference = newValue; break; }
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

    get IssuingCarrierReference1() {
        return this.EntityPM.IssuingCarrierReference1;
    }
    set IssuingCarrierReference1(newValue: string) {
        if (this.EntityPM.IssuingCarrierReference1 != newValue) {
            this.EntityPM.IssuingCarrierReference1 = newValue;
        }
    }

    get CustomAgentExportReference() { return this.EntityPM.CustomAgentExportReference; }
    set CustomAgentExportReference(newValue: string) {
        if (this.EntityPM.CustomAgentExportReference != newValue) {
            this.EntityPM.CustomAgentExportReference = newValue;
        }
    }

    get CustomAgentImportReference() { return this.EntityPM.CustomAgentImportReference; }
    set CustomAgentImportReference(newValue: string) {
        if (this.EntityPM.CustomAgentImportReference != newValue) {
            this.EntityPM.CustomAgentImportReference = newValue;
        }
    }

    get FreightForwarderReference() {
        return this.EntityPM.FreightForwarderReference;
    }
    set FreightForwarderReference(newValue: string) {
        if (this.EntityPM.FreightForwarderReference != newValue) {
            this.EntityPM.FreightForwarderReference = newValue;
        }
    }

    get ColoaderReference1() {
        return this.EntityPM.ColoaderReference1;
    }
    set ColoaderReference1(newValue: string) {
        if (this.EntityPM.ColoaderReference1 != newValue) {
            this.EntityPM.ColoaderReference1 = newValue;
        }
    }

    get CustomClearancePointReference1() {
        return this.EntityPM.CustomClearancePointReference1;
    }
    set CustomClearancePointReference1(newValue: string) {
        if (this.EntityPM.CustomClearancePointReference1 != newValue) {
            this.EntityPM.CustomClearancePointReference1 = newValue;
        }
    }

    get ConsolidatorReference() {
        return this.EntityPM.ConsolidatorReference;
    }
    set ConsolidatorReference(newValue: string) {
        if (this.EntityPM.ConsolidatorReference != newValue) {
            this.EntityPM.ConsolidatorReference = newValue;
        }
    }

    get ReleasingAgentReference1() {
        return this.EntityPM.ReleasingAgentReference1;
    }
    set ReleasingAgentReference1(newValue: string) {
        if (this.EntityPM.ReleasingAgentReference1 != newValue) {
            this.EntityPM.ReleasingAgentReference1 = newValue;
        }
    }

    get Notify1Reference() { return this.EntityPM.Notify1Reference; }
    set Notify1Reference(value: string) {
        if (this.EntityPM.Notify1Reference != value) {
            this.EntityPM.Notify1Reference = value;
        }
    }

    get Notify2Reference() { return this.EntityPM.Notify2Reference; }
    set Notify2Reference(value: string) {
        if (this.EntityPM.Notify2Reference != value) {
            this.EntityPM.Notify2Reference = value;
        }
    }

    get ShipperNotExporterReference() { return this.EntityPM.ShipperNotExporterReference; }
    set ShipperNotExporterReference(value: string) {
        if (this.EntityPM.ShipperNotExporterReference != value) {
            this.EntityPM.ShipperNotExporterReference = value;
        }
    }

    get ConsigneeNotImporterReference() { return this.EntityPM.ConsigneeNotImporterReference; }
    set ConsigneeNotImporterReference(value: string) {
        if (this.EntityPM.ConsigneeNotImporterReference != value) {
            this.EntityPM.ConsigneeNotImporterReference = value;
        }
    }

    // Reference2
    get HasReference2() {
        var myResult: boolean = false;

        switch (this.Code) {
            case "SHIPR":
            case "CONSI":
            case "AGENT":
            case "REAGT":
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
            case "REAGT": { return "ReleasingAgentReference2"; }
            default: { return null; }
        }
    }
    get Reference2() {
        switch (this.Code) {
            case "SHIPR": { return this.ShipperReference2; }
            case "CONSI": { return this.ConsigneeReference2; }
            case "AGENT": { return this.AgentReference2; }
            case "CSTMR": { return this.CustomerReference2; }
            case "REAGT": { return this.ReleasingAgentReference2; }
            default: { return null; }
        }
    }
    set Reference2(newValue: string) {
        switch (this.Code) {
            case "SHIPR": { this.ShipperReference2 = newValue; break; }
            case "CONSI": { this.ConsigneeReference2 = newValue; break; }
            case "AGENT": { this.AgentReference2 = newValue; break; }
            case "CSTMR": { this.CustomerReference2 = newValue; break; }
            case "REAGT": { this.ReleasingAgentReference2 = newValue; break; }
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

    get ReleasingAgentReference2() {
        return this.EntityPM.ReleasingAgentReference2;
    }
    set ReleasingAgentReference2(newValue: string) {
        if (this.EntityPM.ReleasingAgentReference2 != newValue) {
            this.EntityPM.ReleasingAgentReference2 = newValue;
        }
    }

    public IsReseting: boolean = false;
    private isAddressLoaded = false;
    private isContactLoaded = false;
    public ShowNoTemplateText = false;
    public AddressCityText: string = null;
    public PartnerAddressList: AddressList;
    public PartnerContactList: ContactList;
    GetPartnerCard() {

        this.SetUIProperties();

        var myCardId: string = this.PartnerId;

        if (AppTool.IsNullOrEmpty(myCardId)) {
            this.Name = null;
            this.Note = null;
            this.PartnerCardList = null;
            this.AddressId = null;
            this.ContactId = null;

            if (this.Code == "SHIPR") {
                if (this.EntityPM.KnownConsignorNumber != null) {
                    this.EntityPM.KnownConsignorNumber = null;
                }

                if (this.EntityPM.KCExpirationDate != null) {
                    this.EntityPM.KCExpirationDate = null;
                }
            }
        }

        else {
            var myService = this.fatherComponent.CardListService;
            myService.getSingle(myCardId).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        this.PartnerCardList = myResponse.Result;

                        if (this.PartnerCardList != null) {

                            this.Name = this.PartnerCardList.EnglishName;
                            this.Note = this.PartnerCardList.Notes;

                            if (this.Code == "ISSAG") {
                                if (this.isPartnerChanged_Issuing) {
                                    this.AddressId = this.PartnerCardList.MainAddressId;
                                }

                                else {
                                    this.GetPartnerAddress();
                                }

                                this.isPartnerChanged_Issuing = false;
                            }

                            if (this.Code == "SHIPR") {

                                if (this.EntityPM.KnownConsignorNumber != this.PartnerCardList.KnownConsignor) {
                                    this.EntityPM.KnownConsignorNumber = this.PartnerCardList.KnownConsignor;
                                }

                                if (this.EntityPM.KCExpirationDate != this.PartnerCardList.KCExpirationDate) {
                                    this.EntityPM.KCExpirationDate = this.PartnerCardList.KCExpirationDate;
                                }
                            }

                            if (!this.IsReseting) {
                                this.AddressId = this.PartnerCardList.MainAddressId;
                                this.ContactId = this.PartnerCardList.PrimaryContactId;
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

            var list: AddressList = this.fatherComponent.AllAddresses.filter(f => f.Id == myAddressId)[0];
            if (list) {
                this.PartnerAddressList = list;
                this.isAddressLoaded = true;
                this.BuildAddressCityText();
                this.OnLoadCompleted();
            }

            else {
                var myService = this.fatherComponent.AddressListService;
                myService.getSingle(myAddressId).subscribe((myResponse: ServiceResponse) => {

                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            this.PartnerAddressList = myResponse.Result;
                            this.BuildAddressCityText();

                            if (this.PartnerAddressList) {
                                this.fatherComponent.AllAddresses.push(this.PartnerAddressList);
                            }
                        }
                    }

                    this.isAddressLoaded = true;
                    this.OnLoadCompleted();
                });
            }
        }

        else {
            this.isAddressLoaded = true;
            this.OnLoadCompleted();
        }
    }
    GetPartnerContact() {

        this.isContactLoaded = false;
        this.PartnerContactList = null;
        this.ShowNoTemplateText = false;

        var myContactId: string = this.ContactId;

        if (myContactId != null) {

            var list: ContactList = this.fatherComponent.AllContacts.filter(f => f.Id == myContactId)[0];
            if (list) {
                this.PartnerContactList = list;
                this.isContactLoaded = true;
                this.OnLoadCompleted();
            }

            var myService = this.fatherComponent.ContactListService;
            myService.getSingle(myContactId).subscribe((myResponse: ServiceResponse) => {

                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        this.PartnerContactList = myResponse.Result;

                        if (this.PartnerContactList) {
                            this.fatherComponent.AllContacts.push(this.PartnerContactList);
                        }
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

    // Add|Edit Partner
    private isEditButtonClicked: boolean = false;
    AddPartnerClicked() {

        if (this.Code == "ISSAG") {
            this.AddEditIssuingCarrierAgent(true);
        }

        else {
            var myPerspective: string = null;
            var myComponentPath: string = null;

            if (this.Code == "AGENT") {
                myComponentPath = "./CommonModules/CommonAgent/Components/NewEntity/NewAgentComponent";
            }

            else if (this.Code == "CLERN") {
                myComponentPath = "./CommonModules/CommonPartners/Components/NewEntity/NewWarehouseComponent";
            }

            else if (this.Code == "CSAIM" || this.Code == "CSAEX") {
                myComponentPath = "./CommonModules/CommonPartners/Components/NewEntity/NewCustomAgentComponent";
            }

            else {
                myComponentPath = "./CommonModules/CommonCustomer/Components/NewEntity/NewCustomerComponent";

                if (!this.IsCustomer) {
                    myPerspective = "ShippersAndConsignees";
                }
            }

            if (myComponentPath != null) {

                var logWindow = new LogitudeWindow();
                logWindow.Width = 960;
                logWindow.Height = 600;
                logWindow.Title = "New " + this.PartnerTypeName;

                if (!AppTool.IsNullOrEmpty(myPerspective)) {
                    var args = new NewEntityArgs();
                    args.Perspective = myPerspective;
                    logWindow.WindowArgs = args;
                }

                logWindow.Show(myComponentPath);

                logWindow.ComponentLoaded.subscribe(comp => {
                    logWindow.WindowClosed.subscribe(s => {
                        if (s) {
                            this.PartnerId = comp.EntityPM.Id;
                        }
                    });
                });
            }
        }
    }
    EditPartnerClicked() {

        if (this.Code == "ISSAG") {
            this.AddEditIssuingCarrierAgent(false);
        }

        else {
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

                                            this.Name = comp.EntityPM.EnglishName;
                                            this.Note = comp.EntityPM.Notes;

                                            if (this.Code == "SHIPR") {

                                                if (this.EntityPM.KnownConsignorNumber != comp.EntityPM.KnownConsignor) {
                                                    this.EntityPM.KnownConsignorNumber = comp.EntityPM.KnownConsignor;
                                                }

                                                if (this.EntityPM.KCExpirationDate != comp.EntityPM.KCExpirationDate) {
                                                    this.EntityPM.KCExpirationDate = comp.EntityPM.KCExpirationDate;
                                                }
                                            }

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
    }

    // Add|Edit Address
    AddAddressClicked() {

        var entityPM: AddressPM = new AddressPM();
        entityPM.Tenant = SessionLocator.Tenant;
        entityPM.AddressTypeId = "O";
        entityPM.CardId = this.PartnerId;

        var myPartnerTypeId: string = null;
        var isCustomer: boolean;
        if (this.PartnerCardList != null) {
            myPartnerTypeId = this.PartnerCardList.PartnerTypeId;
            isCustomer = this.PartnerCardList.IsCustomer;
        }

        if (entityPM != null) {
            var logeWindow = new LogitudeWindow();
            logeWindow.Width = 630;
            logeWindow.Height = 430;
            logeWindow.Title = "Add Address";
            logeWindow.WindowArgs = { EntityPM: entityPM };
            logeWindow.Show("./CommonPartners/Components/AddEdit/AddEditPartnerAddressComponent");
            logeWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.AddressId = null;
                    this.AddressId = entityPM.Id;
                }
            });
        }
    }
    EditAddressClicked() {
        var myAddressId = this.AddressId;
        var myPartnerTypeId: string = null;
        var isCustomer: boolean;
        if (this.PartnerCardList != null) {
            myPartnerTypeId = this.PartnerCardList.PartnerTypeId;
            isCustomer = this.PartnerCardList.IsCustomer;
        }

        if (!AppTool.IsNullOrEmpty(myAddressId)) {
            var logeWindow = new LogitudeWindow();
            logeWindow.Width = 630;
            logeWindow.Height = 430;
            logeWindow.Title = "Edit Address";
            logeWindow.WindowArgs = { EntityId: myAddressId };
            logeWindow.Show("./CommonPartners/Components/AddEdit/AddEditPartnerAddressComponent");
            logeWindow.WindowClosed.subscribe(s => {
                if (s) {

                    var list = this.fatherComponent.AllAddresses.filter(f => f.Id == myAddressId)[0];
                    if (list) {
                        var indexOfList = this.fatherComponent.AllAddresses.indexOf(list);
                        this.fatherComponent.AllAddresses.splice(indexOfList, 1);
                    }

                    this.AddressId = null;
                    this.AddressId = myAddressId;
                }
            });
        }
    }

    private isPartnerWindowOpened: boolean;
    AddEditIssuingCarrierAgent(isNewPartner: boolean) {
        if (!this.isPartnerWindowOpened) {
            this.isPartnerWindowOpened = true

            var myTitle = isNewPartner ? "Add " : "Edit ";
            myTitle += "Issuing Carrier's Agent";

            var windowArgs = new AddEditPartnerArgs();
            windowArgs.EntityPM = this.EntityPM;
            windowArgs.IsNewEntity = isNewPartner;
            windowArgs.PartnerTypeCode = "AGT";

            var logWindow = new LogitudeWindow();
            logWindow.Title = myTitle;
            logWindow.WindowArgs = windowArgs;
            logWindow.Show('./ShipmentModules/ShipmentAWB/Components/AWBWizard/Partners/AWBAddEditPartnerComponent');

            logWindow.ComponentLoaded.subscribe(cmp => {
                logWindow.WindowClosed.subscribe(($event: any) => {
                    this.isPartnerWindowOpened = false;

                    if (cmp.IsUpdatingPartner) {

                        var myPartnerId = cmp.CurrentPartnerId;
                        var myAddressId = cmp.CurrentAddressId;

                        if (isNewPartner) {
                            if (this.IssuingCarrierAgentId != myPartnerId) {
                                this.IssuingCarrierAgentId = myPartnerId;
                            }

                            else {
                                this.EntityPM.IssuingCarrierAddressId = myAddressId;
                                this.GetPartnerCard();
                            }
                        }

                        else {

                            if (this.Name != cmp.CardEnglishName) {
                                this.Name = cmp.CardEnglishName;
                            }

                            if (this.IssuingCarrierAddressId != myAddressId) {
                                this.IssuingCarrierAddressId = myAddressId;
                            }

                            else {
                                this.GetPartnerAddress();
                            }
                        }
                    }
                });
            });
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
