import {Component, OnDestroy} from '@angular/core';
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
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';

@Component({
    
    templateUrl: './ContactsTabComponent.html',
})

export class ContactsTabComponent extends BaseComponent implements OnDestroy {
    public ItemsSource: ContactItemClass[];
    public EntityPM: any = null;
    public EntityId: string = null;
    public ObjectTableName: string;
    public DataContext = this;
    public Customer: CustomerPM = null;
    public PartnerTypeId: string = null;
    public DomainService: PartnersDomainService;
    public IsCustomerPartner: boolean = false;
    public IsNoDataVisible: boolean = false;
    public IsVisibile: boolean = false;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;
    public HasExternalId: boolean = false;
    constructor(public entityArgs: EntityArgs) {
        super();
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
            this.LoadData();
        });
    }


    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent != null) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.LoadData();
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.LoadData();
                }
            });
        }
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }


    public IsEditingEnabled: boolean = false;
    public IsBlockingUnifreightCustomer: boolean = false;
    private SetUIProperties() {
        var isBlockingUnifreightCustomer = false;
        if (this.HasExternalId) {
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
            this.SetContactForAccounting(myResult);
            this.BuildItemsSource(myResult);
            this.CurrentSession.StopBusyIndicator();
            this.HasExternalId = this.IsAllContactsHaveExternalId(myResult);
            this.SetUIProperties();

        });


    }


    private IsAllContactsHaveExternalId(contacts: any): boolean{

        return contacts.filter(d => AppTool.IsNullOrEmpty(d.ExternalId))[0] ? false : true;
       
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

    SetContactForAccounting(items: ContactPM[]){

        items.forEach(item => {
            if(item.ContactForAccounting == true){
                this.EntityPM.ContactForAccounting=item.Id;
                this.EntityPM.IsDirty=false;
                return;
            }
        });
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
                    args.ExternalId = myContactPM.ExternalId;

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
export class   ContactItemClass extends BaseComponent{
    public ObjectTableName = "Contact";
    public EntityPM: ContactPM;
    public IsNewEntity: boolean = false; 
    constructor(item: ContactPM, public fatherComponent: ContactsTabComponent, isNewEntity: boolean) {
        super();
        this.EntityPM = item;
        this.IsNewEntity = isNewEntity; 
        this.CheckPrimary();
        this.CheckContactForAccounting();
        if(fatherComponent != null) {
            
             this.CheckEmailForSending(this.fatherComponent.EntityPM['EmailForSendingSingArinvoice'])
             if(this.fatherComponent.EntityPM['SendingInterestReport'])
               this.CheckSendingInterestReport(this.fatherComponent.EntityPM['EmailForSendingSingArinvoice']);

        }
      
    }
    GetIsHasExternalId() {
        return !AppTool.IsNullOrEmpty(this.EntityPM.ExternalId);
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
    get DontShowLocalLabels() { return this.EntityPM.DontShowLocalLabels; }
    public get ExternalId() { return this.EntityPM.ExternalId; }
    public get ContactForAccounting() { return this.EntityPM.ContactForAccounting; }
    public set ContactForAccounting(value: boolean) {
        if (this.EntityPM.ContactForAccounting != value) {
            this.EntityPM.ContactForAccounting = value;
        }
    }    
    public IsPrimary: boolean = false;
    public EmailForSending : boolean = false;
    public SendingInterestReport : boolean = false;
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
    CheckContactForAccounting(){
        var ContactForAccounting = false;
        var myCardContactForAccounting=null;
        
        if (this.fatherComponent && this.fatherComponent.EntityPM) {
            myCardContactForAccounting = this.fatherComponent.EntityPM['ContactForAccounting'];
            if (!AppTool.IsNullOrEmpty(myCardContactForAccounting)) {
                if (myCardContactForAccounting == this.Id) {
                    ContactForAccounting = true;
                }
            }
        }
        this.ContactForAccounting = ContactForAccounting;
    }

    setContactForAccounting(){
        
        this.fatherComponent.EntityPM['ContactForAccounting']=this.Id;
        this.fatherComponent.ItemsSource.forEach(item => {
            item.CheckContactForAccounting();
        });
    }
    SetPrimary() {
        this.fatherComponent.EntityPM['PrimaryContactId'] = this.Id;
        this.fatherComponent.EntityPM['PrimaryContactName'] = this.EnglishName;
        this.fatherComponent.EntityPM['PrimaryContactPhone'] = this.BusinessPhone;

        this.fatherComponent.ItemsSource.forEach(item => {
            item.CheckPrimary();
        });
    }

    


    SetEmailForSendingSingArinvoices() {
        this.fatherComponent.EntityPM['SendingInterestReport'] = false;
        this.fatherComponent.EntityPM['EmailForSendingSingArinvoice'] = this.Id;
        var myCardContactId = this.Id;
        this.fatherComponent.ItemsSource.forEach(item => {
            item.CheckEmailForSending(myCardContactId);
        });
    }

    SetSendingInterestReport() {
        
        this.fatherComponent.EntityPM['SendingInterestReport'] = true;
        var myCardContactId = this.Id;
        this.fatherComponent.ItemsSource.forEach(item => {
           item.CheckSendingInterestReport(myCardContactId);
        });
    }

    CheckEmailForSending(myCardContactId: string=null) {
        var emailForSending = false; 
        if (this.fatherComponent && this.fatherComponent.EntityPM) {
            if (!AppTool.IsNullOrEmpty(myCardContactId)) {
                if (myCardContactId == this.Id) {
                    emailForSending = true;
                }
            }
        }
         
        this.EmailForSending = emailForSending;
    }
    CheckSendingInterestReport(myCardContactId: string=null) {
        var sendingInterestReport = false; 
        if (this.fatherComponent && this.fatherComponent.EntityPM) {
            if (!AppTool.IsNullOrEmpty(myCardContactId)) {
                if (myCardContactId == this.Id  ) {
                    sendingInterestReport = true;
                }
            }
        }
        
        this.SendingInterestReport = sendingInterestReport;
    }
}

