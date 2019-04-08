import {Component, OnInit, OnDestroy} from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {WarehouseEntryPM} from '../../../EntityPMs/WarehouseEntryPM';
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

@Component({
    selector: 'WarehouseEntryPartnersTabComponent',
    moduleId: module.id,
    templateUrl: './WarehouseEntryPartnersTabComponent.html',
})

export class WarehouseEntryPartnersTabComponent implements OnInit, OnDestroy {
    public EntityPM: WarehouseEntryPM;
    public ObjectTableName: string = "WarehouseEntry";
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
                if (tabCode == "PAEY") {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    this.UpdateScreen();
                }
            });
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.TabSelectedEvent);
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
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
       
    }

    private BuildItemsCollection() {
        this.ItemsCollection = [];

        if (this.EntityPM.ShipperId != null) {
            this.ItemsCollection.push(new PartnerItem(this, "SHIPR"));
        }

        if (this.EntityPM.ConsigneeId != null) {
            this.ItemsCollection.push(new PartnerItem(this, "CONSI"));
        }
    }

    public IsAddDisabled_SHIPR: boolean = false;
    public IsAddDisabled_CONSI: boolean = false;

    SetAddButtonsIsDisabled() {
        this.IsAddDisabled_SHIPR = this.EntityPM.ShipperId == null ? false : true;
        this.IsAddDisabled_CONSI = this.EntityPM.ConsigneeId == null ? false : true;
    }

    AddPartner(myCode: string) {
        var newPartnerItem: PartnerItem = new PartnerItem(this, myCode);
        newPartnerItem.IsNewAdded = true;

        var myWindowTitle: string = TextCodeTranslator.Translate("WarehouseEntry.S.Partners.Add" + newPartnerItem.FullCode);
        this.RunAddEditPartner(newPartnerItem, myWindowTitle);
    }
    EditPartner(myPartnerItem: PartnerItem) {
        myPartnerItem.IsNewAdded = false;
        myPartnerItem.CopyOriginData();

        var myWindowTitle: string = TextCodeTranslator.Translate("WarehouseEntry.S.Partners.Edit" + myPartnerItem.FullCode);
        this.RunAddEditPartner(myPartnerItem, myWindowTitle);
    }
    RunAddEditPartner(myPartnerItem: PartnerItem, myWindowTitle: string) {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = myWindowTitle;
        logitudeWindow.DataContext = myPartnerItem;
        logitudeWindow.Show("./Warehouse/Components/EditTabs/PartnersTab/AddEditPartnerComponent");
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
            confirmWindow.Show(TextCodeTranslator.Translate("WarehouseEntry.M.DeleteThisPartner"));

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
                    myPartnerItem.Reference1 = null;
                    myPartnerItem.Reference2 = null;
                    this.SetAddButtonsIsDisabled();
                }
            });
        }
    }
    public WarehouseEntryCustomerTypeCode = null;
    SetDefaultCustomer() {
        this.EntityPM.CustomerId = null;
        this.EntityPM.CustomerName = null;
        this.EntityPM.CustomerRef1 = null;
        this.EntityPM.CustomerRef2 = null;
        this.WarehouseEntryCustomerTypeCode = null;

        if (this.EntityPM.DirectionId == "I") {
            this.WarehouseEntryCustomerTypeCode = "CON";
            this.EntityPM.CustomerId = this.EntityPM.ConsigneeId;
            this.EntityPM.CustomerName = this.EntityPM.ConsigneeName;
            this.EntityPM.CustomerRef1 = this.EntityPM.ConsigneeReference1;
            this.EntityPM.CustomerRef2 = this.EntityPM.ConsigneeReference2;
        }

        else {
            this.WarehouseEntryCustomerTypeCode = "SHI";
            this.EntityPM.CustomerId = this.EntityPM.ShipperId;
            this.EntityPM.CustomerName = this.EntityPM.ShipperName;
            this.EntityPM.CustomerRef1 = this.EntityPM.ShipperReference1;
            this.EntityPM.CustomerRef2 = this.EntityPM.ShipperReference2;
        }
    }
}

export class PartnerItem extends BaseComponent {
    public IsNewAdded: boolean;
    public EntityPM: WarehouseEntryPM;
    public Code: string;
    public ObjectTableName: string = "WarehouseEntry";
    public IsMyCustomer: boolean = false;
    public PartnerId_Origin: string;
    public AddressId_Origin: string;
    public ContactId_Origin: string;
    public Reference1_Origin: string;
    public Reference2_Origin: string;
    public IsInlandDomestic: boolean = false;
    constructor(public fatherComponent: WarehouseEntryPartnersTabComponent, typeCode: string) {
        super();
        this.EntityPM = fatherComponent.EntityPM;
        this.IsEditingEnabled = fatherComponent.IsEditingEnabled;
        this.Code = typeCode;
        this.InitializeProperties();
        this.SetRemoveButtonVisibility();
        this.GetPartnerAddress();
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
        }

        this.PartnerTypeName = TextCodeTranslator.Translate("WarehouseEntry.F." + this.FullCode + "Id");
    }

    get PartnerName() {
        switch (this.Code) {
            case "SHIPR": { return this.EntityPM.ShipperName; }
            case "CONSI": { return this.EntityPM.ConsigneeName; }
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
        }
    }

    get CardDependencyProperty1() {
        var myResult: string = null;

        switch (this.Code) {
            case "SHIPR":
            case "CONSI":
                {
                    if (SessionLocator.TenantPM.AllowAgentInCustomersLOV) {
                        myResult = "CS,AG";
                    }
                    else {
                        myResult = "CS";
                    }
                    break
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
                {
                    myResult = true;
                    break
                }

            default: {
                myResult = false;
                break;
            }
        }

        return myResult;
    }

  
    get IsCustomer() {
        return (this.PartnerId == this.EntityPM.CustomerId) ? true : false;
    }

    SetAsCustomer() {
        this.EntityPM.CustomerId = null;
        this.EntityPM.CustomerName = null;
        this.EntityPM.CustomerRef1 = null;
        this.EntityPM.CustomerRef2 = null;

        switch (this.Code) {
            case "SHIPR":
                {
                    this.fatherComponent.WarehouseEntryCustomerTypeCode = "SHI";
                    break;
                }

            case "CONSI":
                {
                    this.fatherComponent.WarehouseEntryCustomerTypeCode = "CON";
                    break;
                }

            default:
                {
                    this.fatherComponent.WarehouseEntryCustomerTypeCode = "OTH";
                    break;
                }
        }

        this.EntityPM.CustomerId = this.PartnerId;
        this.EntityPM.CustomerName = this.PartnerName;
        this.EntityPM.CustomerRef1 = this.Reference1;
        this.EntityPM.CustomerRef2 = this.Reference2;
    }

    CopyOriginData() {
        this.PartnerId_Origin = this.PartnerId;
        this.AddressId_Origin = this.AddressId;
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
            default: { return null; }
        }
    }

    get PartnerId() {
        switch (this.Code) {
            case "SHIPR": { return this.ShipperId; }
            case "CONSI": { return this.ConsigneeId; }
            default: { return null; }
        }
    }
    set PartnerId(newValue: string) {
        switch (this.Code) {
            case "SHIPR": { this.ShipperId = newValue; break; }
            case "CONSI": { this.ConsigneeId = newValue; break; }
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


    // AddressId
    get PartnerAddressIdProperty() {
        switch (this.Code) {
            case "SHIPR": { return "ShipperAddressId"; }
            case "CONSI": { return "ConsigneeAddressId"; }
            default: { return null; }
        }
    }

    get AddressId() {
        switch (this.Code) {
            case "SHIPR": { return this.ShipperAddressId; }
            case "CONSI": { return this.ConsigneeAddressId; }
            default: { return null; }
        }
    }
    set AddressId(newValue: string) {
        switch (this.Code) {
            case "SHIPR": { this.ShipperAddressId = newValue; break; }
            case "CONSI": { this.ConsigneeAddressId = newValue; break; }
        }
    }

    get ShipperAddressId() {
        return this.EntityPM.FromAddressId;
    }
    set ShipperAddressId(newValue: string) {
        if (this.EntityPM.FromAddressId != newValue) {
            this.EntityPM.FromAddressId = newValue;
            this.GetPartnerAddress();
        }
    }

    get ConsigneeAddressId() {
        return this.EntityPM.ToAddressId;
    }
    set ConsigneeAddressId(newValue: string) {
        if (this.EntityPM.ToAddressId != newValue) {
            this.EntityPM.ToAddressId = newValue;
            this.GetPartnerAddress();
        }
    }

    // ContactId
    get PartnerContactIdProperty() {
        switch (this.Code) {
            case "SHIPR": { return "ShipperContactId"; }
            case "CONSI": { return "ConsigneeContactId"; }
            default: { return null; }
        }
    }


    // Reference1
    get HasReference1() {
        var myResult: boolean = false;

        switch (this.Code) {
            case "SHIPR":
            case "CONSI":
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
            default: { return null; }
        }
    }
    get Reference1() {
        switch (this.Code) {
            case "SHIPR": { return this.ShipperReference1; }
            case "CONSI": { return this.ConsigneeReference1; }
            default: { return null; }
        }
    }
    set Reference1(newValue: string) {
        switch (this.Code) {
            case "SHIPR": { this.ShipperReference1 = newValue; break; }
            case "CONSI": { this.ConsigneeReference1 = newValue; break; }
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

  

    // Reference2
    get HasReference2() {
        var myResult: boolean = false;

        switch (this.Code) {
            case "SHIPR":
            case "CONSI":
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
            default: { return null; }
        }
    }
    get Reference2() {
        switch (this.Code) {
            case "SHIPR": { return this.ShipperReference2; }
            case "CONSI": { return this.ConsigneeReference2; }
            default: { return null; }
        }
    }
    set Reference2(newValue: string) {
        switch (this.Code) {
            case "SHIPR": { this.ShipperReference2 = newValue; break; }
            case "CONSI": { this.ConsigneeReference2 = newValue; break; }
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

    
    get CustomerReference2() {
        return this.EntityPM.CustomerRef2;
    }
    set CustomerReference2(newValue: string) {
        if (this.EntityPM.CustomerRef2 != newValue) {
            this.EntityPM.CustomerRef2 = newValue;
        }
    }

    // Add|Edit Partner
    AddPartnerClicked() {
        var myComponentPath: string = null;

        myComponentPath = "./CommonModules/CommonCustomer/Components/NewEntity/NewCustomerComponent";
        if (myComponentPath != null) {
            var logWindow = new LogitudeWindow();
            logWindow.Width = 900;
            logWindow.Height = 570;
            logWindow.Title = "New " + this.PartnerTypeName;

            var args = new NewEntityArgs();
            args.Perspective = "ShippersAndConsignees";
            logWindow.WindowArgs = args;

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
        var objectTableName: string = null;
        objectTableName = "Customer";
        if (objectTableName != null) {
            var logWindow = new LogitudeWindow();
            logWindow.Title = "Edit " + this.PartnerTypeName;
            logWindow.IsFillScreen = true;
            logWindow.ShowEditComponent(this.PartnerId, objectTableName);

            logWindow.ComponentLoaded.subscribe(comp => {
                logWindow.WindowClosed.subscribe(s => {

                    this.PartnerName = comp.EntityPM.EnglishName;
                    this.GetPartnerAddress();
                });
            });
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
            this.AddressCityText = null;
            this.PartnerCardList = null;
            this.PartnerAddressList = null;
            this.PartnerContactList = null;
            this.AddressId = null;
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
                            this.AddressId = list.MainAddressId;

                            switch (this.Code) {
                                case "SHIPR": {
                                  //  this.EntityPM.FromAddressId = list.PickAddressId;
                                    break;
                                }

                                case "CONSI": {
                                  //  this.EntityPM.ToAddressId = list.PickAddressId;
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
            this.Reference1 = null;
            this.Reference2 = null;
        }

        else {
            this.IsReseting = true;
            this.PartnerId = this.PartnerId_Origin;
            this.AddressId = this.AddressId_Origin;
            this.Reference1 = this.Reference1_Origin;
            this.Reference2 = this.Reference2_Origin;
            this.IsReseting = false;
        }
    }
}
