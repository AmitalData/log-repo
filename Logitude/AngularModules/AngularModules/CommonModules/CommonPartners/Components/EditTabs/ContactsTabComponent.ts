import {Component} from '@angular/core';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ContactPM} from '../../../../Common/EntityPMs/ContactPM';
import {PartnersDomainService, PartnerServicePM} from '../../../../Common/Services/PartnersDomainService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {AppTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {CustomerPM} from '../../../../Common/EntityPMs/CustomerPM';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    moduleId: module.id,
    templateUrl: './ContactsTabComponent.html',
})

export class ContactsTabComponent {
    public ItemsSource: ContactItemClass[];
    public EntityPM: any = null;
    public EntityId: string = null;
    public ObjectTableName: string;
    public Customer: CustomerPM = null;
    public PartnerTypeId: string = null;
    public DomainService: PartnersDomainService;
    public IsCustomerPartner: boolean = false;
    public IsNoDataVisible: boolean = false;
    public IsVisibile: boolean = false;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        this._entityResourceService.getEntityResourceByTableName("Contact", 0).subscribe(response=> {
            this.IsVisibile = true;
            this.ItemsSource = [];
            this.EntityPM = entityArgs.EntityPM;
            this.EntityId = entityArgs.EntityPM == null ? null : entityArgs.EntityPM.Id;
            this.ObjectTableName = entityArgs.ObjectTableName;
            this.PartnerTypeId = this.EntityPM.PartnerTypeId;

            if (this.EntityPM instanceof CustomerPM) {
                this.Customer = this.EntityPM;
                this.IsCustomerPartner = true;
            }

            if (this.DomainService == null) {
                this.DomainService = new PartnersDomainService();
            }

            this.Listen();
            this.SetUIProperties();
            this.LoadData();
        });
    }

    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    this.SetUIProperties();
                    this.LoadData();
                }
            });

            this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    this.SetUIProperties();
                    this.LoadData();
                }
            });
        }
    }

    public IsEditingEnabled: boolean = false;
    public IsBlockingUnifreightCustomer: boolean = false;
    private SetUIProperties() {
        var isBlockingUnifreightCustomer = false;

        if (!AppTool.IsNullOrEmpty(this.EntityPM.ExternalId)) {
            if (this.Customer != null) {
                if (SessionLocator.TenantPM.IsHybrid && (this.Customer.CustomerStatusCode == "ACT" || this.Customer.CustomerStatusCode == "WAC")) {
                    isBlockingUnifreightCustomer = true;
                }
            }
        }

        this.IsBlockingUnifreightCustomer = isBlockingUnifreightCustomer;
        this.IsEditingEnabled = !isBlockingUnifreightCustomer;
    }

    private LoadData() {
        this.CurrentSession.StartBusyIndicatorLoading();

        this.DomainService.GetAllContactsPMsbyCardId(this.EntityPM.Id).subscribe((myResult:any) => {
            this.BuildItemsSource(myResult);
            this.CurrentSession.StopBusyIndicator();
        });
    }
    private BuildItemsSource(items: ContactPM[]) {
        this.ItemsSource = [];

        if (items == null) {
            items = [];
        }

        items.forEach(item => {
            this.ItemsSource.push(new ContactItemClass(item, this, false));
        });

        this.SetIsNoDataVisible();
    }

    SetIsNoDataVisible() {
        var isNoDataVisible = false;

        if (this.ItemsSource.length == 0) {
            isNoDataVisible = true;
        }

        this.IsNoDataVisible = isNoDataVisible;
    }

    NewEntityClicked() {
        var item = new ContactPM();
        item.Tenant = this.EntityPM.Tenant;
        item.CardId = this.EntityPM.Id;

        var itemComponent = new ContactItemClass(item, this, true);
        this.RunWindow(itemComponent, TextCodeTranslator.Translate("Contact.O.AddContact"));
    }
    EditEntityClicked(itemComponent: ContactItemClass) {
        this.RunWindow(itemComponent, TextCodeTranslator.Translate("Contact.O.EditContact"));
    }
    private RunWindow(itemViewModel: ContactItemClass, windowTitle: string) {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.Title = windowTitle;
        logWindow.DataContext = itemViewModel;
        logWindow.Show('./CommonModules/CommonPartners/Components/AddEdit/AddEditContactComponent');
    }

    DisconnectClicked(itemComponent: ContactItemClass) {
        if (!itemComponent.IsPrimary || this.ItemsSource.length == 1) {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Title = "Disconnect";
            confirmWindow.Show('Are you sure you want to disconnect?');
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {                   

                    this.CurrentSession.StartBusyIndicatorSaving();

                    var isCardEntityDirty: boolean = this.EntityPM['IsDirty'];
                    if (itemComponent.IsPrimary && this.ItemsSource.length == 1) {
                        this.EntityPM['PrimaryContactId'] = null;
                        isCardEntityDirty = true;
                    }

                    var indexOfItemComponent = this.ItemsSource.indexOf(itemComponent);
                    if (indexOfItemComponent > -1) {
                        this.ItemsSource.splice(indexOfItemComponent, 1);
                        this.SetIsNoDataVisible();
                    }

                    var myContactPM: ContactPM = itemComponent.EntityPM;
                    myContactPM.DisconectFromCard = true;

                    var args = new PartnerServicePM();
                    args.PartnerId = this.EntityId;
                    args.PartnerTypeId = this.PartnerTypeId;
                    args.Tenant = myContactPM.Tenant;
                    args.ContactId = myContactPM.Id;
                    args.PartnerId = myContactPM.CardId;
                    args.Contact = myContactPM;
                    args.IsContactDirty = true;
                    args.IsPartnerDirty = isCardEntityDirty;

                    this.DomainService.SetPartner(args, this.EntityPM);

                    this.DomainService.PostPartnerAddress(args).subscribe((myResponse: ServiceResponse) => {

                        if (!myResponse.HasError) {
                            this.EntityPM['IsDirty'] = isCardEntityDirty;                            
                        }

                        this.CurrentSession.StopBusyIndicator();
                    });
                }
            });
        }

        else {
            var messageWindow = new MessageWindow();
            messageWindow.Show("Please set another contact as the primary before disconnecting the primary contact.");
        }
    }
}
export class ContactItemClass {
    public ObjectTableName = "Contact";
    public EntityPM: ContactPM;
    public IsNewEntity: boolean = false;

    constructor(item: ContactPM, public fatherComponent: ContactsTabComponent, isNewEntity: boolean) {
        this.EntityPM = item;
        this.IsNewEntity = isNewEntity;
        this.CheckPrimary();
    }

    // Properties
    get Id() { return this.EntityPM.Id; }
    get Name() { return this.EntityPM.EnglishName; }   
    get Email() { return this.EntityPM.Email; }
    get EnglishName() { return this.EntityPM.EnglishName; }
    get LocalName() { return this.EntityPM.LocalName; }
    get Position() { return this.EntityPM.Position; }
    get BusinessPhone() { return this.EntityPM.BusinessPhone; }
    get Mobile() { return this.EntityPM.Mobile; }
    get Fax() { return this.EntityPM.Fax; }
    get Birthday() { return this.EntityPM.Birthday; }
    get Anniversary() { return this.EntityPM.Anniversary; }
    get Notes() { return this.EntityPM.Notes; }
    get InActive() { return this.EntityPM.InActive; }
    get BirthdayReminder() { return this.EntityPM.BirthdayReminder; }
    get AnniversaryReminder() { return this.EntityPM.AnniversaryReminder; }

    public IsPrimary: boolean = false;
    CheckPrimary() {

        var isPrimary = false;
        var myCardPrimaryContactId = null;
        if (this.fatherComponent && this.fatherComponent.EntityPM) {
            myCardPrimaryContactId = this.fatherComponent.EntityPM['PrimaryContactId'];
            if (!AppTool.IsNullOrEmpty(myCardPrimaryContactId)) {
                if (myCardPrimaryContactId == this.Id) {
                    isPrimary = true;
                }
            }
        }
     
        this.IsPrimary = isPrimary;
    }
    SetPrimary() {
        this.fatherComponent.EntityPM['PrimaryContactId'] = this.Id;
        this.fatherComponent.EntityPM['PrimaryContactName'] = this.EnglishName;
        this.fatherComponent.EntityPM['PrimaryContactPhone'] = this.BusinessPhone;

        this.fatherComponent.ItemsSource.forEach(item => {
            item.CheckPrimary();
        });
    }
}
