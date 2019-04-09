import {Component} from '@angular/core';
import {ContactPM} from '../../../../Common/EntityPMs/ContactPM';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ContactDatePicker} from '../../../../Controls/ContactDatePicker';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {AppTool, FormatTool} from '../../../../Infrastructure/Tools';
import {PartnersDomainService} from '../../../../Common/Services/PartnersDomainService';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';

@Component({
    moduleId: module.id,
    templateUrl: './ContactInputTemplate.html',
})

export class ContactInputTemplate extends BaseComponent {
    private args: ContactInputTemplateArgs;
    public ObjectTableName: string = "Contact";
    public EntityPM: ContactPM = null;
    public DataContext: ContactInputTemplate = this;
    public CardId: string = null;
    public IsNewEntity: boolean = false;
    public IsCustomerVisible: boolean = false;
    public CardDependencyProperty1: string = "CS";
    public CustomerLable: string = "Customer";
    public DependencyFilter1IsList: boolean = false;
    public DomainService: PartnersDomainService;
    public ShowSearchContacts: boolean = false;
    public ShowSecondPartOfWindow: boolean = true;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.EntityPM = new ContactPM();
        this.DomainService = new PartnersDomainService();

        this.Listen();
    }


    private LoadEntityCompletedEvent: any = null;
    Listen() {
        if (this.CurrentSession.CurrentEditComponent != null && this.CurrentSession.CurrentEditComponent.ObjectTableName == "Contact") {
            if (this.LoadEntityCompletedEvent == null) {
                this.LoadEntityCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }

        }
    }


    ngOnDestroy() {
        AppTool.KillEventEmitter(this.LoadEntityCompletedEvent);

    }

    CopyDomain() {
        let selBox = document.createElement('textarea');
        selBox.style.position = 'fixed';
        selBox.style.left = '0';
        selBox.style.top = '0';
        selBox.style.opacity = '0';
        selBox.value = this.Email;
        document.body.appendChild(selBox);
        selBox.focus();
        selBox.select();
        document.execCommand('copy');
        document.body.removeChild(selBox);
    }

    public InitTemplate(args: ContactInputTemplateArgs) {
        this.args = args;
        this.EntityPM = args.EntityPM;
        this.IsNewEntity = args.IsNewEntity;
        this.IsCustomerVisible = args.IsCustomerVisible;
        this.CardId = args.CardId;
        this.ShowSearchContacts = args.ShowSearchContacts;

       
        if (!AppTool.IsNullOrEmpty(args.CustomerId)) {
            this.CustomerId = args.CustomerId;
        }

        if (args.ComponentName == "Ticket") {
            this.CardId = args.CustomerId;
            this.EntityPM.CustomerId = args.CustomerId;
            this.CardDependencyProperty1 = args.CustomerId;
            this.CustomerLable = args.CustomerLable;
            this.DependencyFilter1IsList = args.CardDependencyProperty1IsList;
            this.UIProperties.SetEnabled("CustomerId", this.ObjectTableName, false);
        }

        if (args.ShowSecondPartOfWindow == "yes") {
            this.ShowSecondPartOfWindow = false;
        }
        else {
            this.ShowSecondPartOfWindow = true;
        }

        this.SetUIProperties();
    }

    public IsEditingEnabled: boolean = false;
    public IsEditingEmailEnabled: boolean = false;
    public IsBlockingUnifreightCustomer: boolean = false;
    private SetUIProperties() {

        this.IsEditingEnabled = true;
        this.IsEditingEmailEnabled = true;

        if (!this.IsNewEntity) {
            if (this.args.BlockEditingEmail) {
                this.IsEditingEmailEnabled = false;
            }
        }

        //this.IsBlockingUnifreightCustomer = this.fatherComponent.IsBlockingUnifreightCustomer;

        this.UIProperties.SetEnabled("Email", this.ObjectTableName, this.IsEditingEmailEnabled);
        this.UIProperties.SetEnabled("EnglishName", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("LocalName", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Position", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("BusinessPhone", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Mobile", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Fax", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Birthday", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Anniversary", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Notes", this.ObjectTableName, this.IsEditingEnabled);

        this.UIProperties.SetEnabled("InActive", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetVisibility("InActive", this.ObjectTableName, !this.IsNewEntity);

        //this.UIProperties.SetEnabled("IsAll", this.ObjectTableName, this.IsEditingEnabled);
        //this.UIProperties.SetEnabled("IsAirExport", this.ObjectTableName, this.IsEditingEnabled);
        //this.UIProperties.SetEnabled("IsAirImport", this.ObjectTableName, this.IsEditingEnabled);
        //this.UIProperties.SetEnabled("IsOceanExport", this.ObjectTableName, this.IsEditingEnabled);
        //this.UIProperties.SetEnabled("IsOceanImport", this.ObjectTableName, this.IsEditingEnabled);
        //this.UIProperties.SetEnabled("IsInlandDomestic", this.ObjectTableName, this.IsEditingEnabled);
        //this.UIProperties.SetEnabled("IsCustomsImport", this.ObjectTableName, this.IsEditingEnabled);

        this.SetUIProperties_Dates();
    }
    private SetUIProperties_Dates() {
        var isBirthdayReminderEnabled = this.IsEditingEnabled;
        var isAnniversaryReminderEnabled = this.IsEditingEnabled;
        if (this.IsEditingEnabled) {
            if (this.Birthday == null) {
                isBirthdayReminderEnabled = false;
            }

            if (this.Anniversary == null) {
                isAnniversaryReminderEnabled = false;
            }
        }

        this.UIProperties.SetEnabled("BirthdayReminder", this.ObjectTableName, isBirthdayReminderEnabled);
        this.UIProperties.SetEnabled("AnniversaryReminder", this.ObjectTableName, isAnniversaryReminderEnabled);
    }

    SelectEmailClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 725;
        logWindow.Height = 520;
        logWindow.WindowArgs = this;
        logWindow.Title = TextCodeTranslator.TranslateTablePlural("Contact") + " Search";;
        logWindow.Show('./CommonModules/CommonPartners/Components/Templates/SearchContactsComponent');
    }

    // Properties
    get Email() { return this.EntityPM.Email; }
    set Email(newValue: string) {
        if (this.EntityPM.Email != newValue) {
            this.EntityPM.Email = newValue;
        }
    }

    get EnglishName() { return this.EntityPM.EnglishName; }
    set EnglishName(newValue: string) {
        if (this.EntityPM.EnglishName != newValue) {
            this.EntityPM.EnglishName = newValue;
            this.LocalName = this.EntityPM.EnglishName;
        }
    }

    get LocalName() { return this.EntityPM.LocalName; }
    set LocalName(newValue: string) {
        if (this.EntityPM.LocalName != newValue) {
            this.EntityPM.LocalName = newValue;
        }
    }

    get Position() { return this.EntityPM.Position; }
    set Position(newValue: string) {
        if (this.EntityPM.Position != newValue) {
            this.EntityPM.Position = newValue;
        }
    }

    get BusinessPhone() { return this.EntityPM.BusinessPhone; }
    set BusinessPhone(newValue: string) {
        if (this.EntityPM.BusinessPhone != newValue) {
            this.EntityPM.BusinessPhone = newValue;
        }
    }

    get Mobile() { return this.EntityPM.Mobile; }
    set Mobile(newValue: string) {
        if (this.EntityPM.Mobile != newValue) {
            this.EntityPM.Mobile = newValue;
        }
    }

    get Fax() { return this.EntityPM.Fax; }
    set Fax(newValue: string) {
        if (this.EntityPM.Fax != newValue) {
            this.EntityPM.Fax = newValue;
        }
    }

    get Birthday() { return this.EntityPM.Birthday; }
    set Birthday(newValue: Date) {
        if (this.EntityPM.Birthday != newValue) {
            this.EntityPM.Birthday = newValue;
            this.SetUIProperties_Dates();
        }
    }

    get Anniversary() { return this.EntityPM.Anniversary; }
    set Anniversary(newValue: Date) {
        if (this.EntityPM.Anniversary != newValue) {
            this.EntityPM.Anniversary = newValue;
            this.SetUIProperties_Dates();
        }
    }

    get Notes() { return this.EntityPM.Notes; }
    set Notes(newValue: string) {
        if (this.EntityPM.Notes != newValue) {
            this.EntityPM.Notes = newValue;
        }
    }

    get InActive() { return this.EntityPM.InActive; }
    set InActive(newValue: boolean) {
        if (this.EntityPM.InActive != newValue) {
            this.EntityPM.InActive = newValue;
        }
    }

    get BirthdayReminder() { return this.EntityPM.BirthdayReminder; }
    set BirthdayReminder(newValue: boolean) {
        if (this.EntityPM.BirthdayReminder != newValue) {
            this.EntityPM.BirthdayReminder = newValue;
        }
    }

    get AnniversaryReminder() { return this.EntityPM.AnniversaryReminder; }
    set AnniversaryReminder(newValue: boolean) {
        if (this.EntityPM.AnniversaryReminder != newValue) {
            this.EntityPM.AnniversaryReminder = newValue;
        }
    }

    get CustomerId() { return this.EntityPM.CustomerId; }
    set CustomerId(newValue: string) {
        if (this.EntityPM.CustomerId != newValue) {
            this.EntityPM.CustomerId = newValue;
            this.CardId = newValue;
            this.ValidateContactExist();
        }
    }

    // Products
    public IsProductsVisible: boolean = false;
    private SetIsProductsVisible() {
        var isProductsVisible = false;

        if (FeatureLocator.HasFeaturePermession("Contact", "ACTIVEPRODUCTTYPES")) {
            if (this.HasCardContact) {
                isProductsVisible = true;
            }
        }

        this.IsProductsVisible = isProductsVisible;
    }

    get IsAll() { return this.EntityPM.IsAll; }
    set IsAll(newValue: boolean) {
        if (this.EntityPM.IsAll != newValue) {
            this.EntityPM.IsAll = newValue;
        }
    }

    get IsAirExport() { return this.EntityPM.IsAirExport; }
    set IsAirExport(newValue: boolean) {
        if (this.EntityPM.IsAirExport != newValue) {
            this.EntityPM.IsAirExport = newValue;
        }
    }

    get IsAirImport() { return this.EntityPM.IsAirImport; }
    set IsAirImport(newValue: boolean) {
        if (this.EntityPM.IsAirImport != newValue) {
            this.EntityPM.IsAirImport = newValue;
        }
    }

    get IsInlandDomestic() { return this.EntityPM.IsInlandDomestic; }
    set IsInlandDomestic(newValue: boolean) {
        if (this.EntityPM.IsInlandDomestic != newValue) {
            this.EntityPM.IsInlandDomestic = newValue;
        }
    }

    get IsCustomsImport() { return this.EntityPM.IsCustomsImport; }
    set IsCustomsImport(newValue: boolean) {
        if (this.EntityPM.IsCustomsImport != newValue) {
            this.EntityPM.IsCustomsImport = newValue;
        }
    }

    get IsOceanExport() { return this.EntityPM.IsOceanExport; }
    set IsOceanExport(newValue: boolean) {
        if (this.EntityPM.IsOceanExport != newValue) {
            this.EntityPM.IsOceanExport = newValue;
        }
    }

    get IsOceanImport() { return this.EntityPM.IsOceanImport; }
    set IsOceanImport(newValue: boolean) {
        if (this.EntityPM.IsOceanImport != newValue) {
            this.EntityPM.IsOceanImport = newValue;
        }
    }

    get HasCardContact() { return this.EntityPM.HasCardContact; }
    set HasCardContact(newValue: boolean) {
        if (this.EntityPM.HasCardContact != newValue) {
            this.EntityPM.HasCardContact = newValue;
        }
    }

    public Info1Text: string = null;
    public Info2Text: string = null;
    private loadedContact: ContactPM = null;
    private allCardContacts: any[] = [];
    public EmailLostFocus(email: string) {

        this.Info1Text = null;
        this.Info2Text = null;
        this.loadedContact = null;
        this.allCardContacts = [];
        this.ValidateContactExist();

        if (!AppTool.IsNullOrEmpty(email)) {
            if (email.indexOf('@') > -1 && email.indexOf('.') > -1) {
                this.DomainService.GetContactsByEmail(email).subscribe(myResult => {
                    if (myResult != null) {
                        this.loadedContact = myResult[0];

                        if (this.loadedContact != null) {
                            this.EnglishName = this.loadedContact.EnglishName;
                            this.LocalName = this.loadedContact.LocalName;
                            this.Email = this.loadedContact.Email;
                            this.BusinessPhone = this.loadedContact.BusinessPhone;
                            this.Mobile = this.loadedContact.Mobile;
                            this.Fax = this.loadedContact.Fax;
                            this.Position = this.loadedContact.Position;
                            this.Birthday = this.loadedContact.Birthday;
                            this.Anniversary = this.loadedContact.Anniversary;
                            this.BirthdayReminder = this.loadedContact.BirthdayReminder;
                            this.AnniversaryReminder = this.loadedContact.AnniversaryReminder;
                            this.InActive = this.loadedContact.InActive;
                            this.IsAirImport = this.loadedContact.IsAirImport;
                            this.IsAirExport = this.loadedContact.IsAirExport;
                            this.IsAll = this.loadedContact.IsAll;
                            this.IsInlandDomestic = this.loadedContact.IsInlandDomestic;
                            this.IsCustomsImport = this.loadedContact.IsCustomsImport;
                            this.IsOceanExport = this.loadedContact.IsOceanExport;
                            this.IsOceanImport = this.loadedContact.IsOceanImport;
                            this.HasCardContact = this.loadedContact.HasCardContact;
                            this.SetIsProductsVisible();

                            if (!AppTool.IsNullOrEmpty(this.CardId)) {
                                this.DomainService.GetCardContactsByContact(this.loadedContact.Id).subscribe((myResponse: ServiceResponse) => {
                                    if (!myResponse.HasError) {

                                        this.allCardContacts = myResponse.Result;
                                        this.ValidateContactExist();

                                        var otherCardContacts = this.allCardContacts.filter(d => d.ContactId == this.loadedContact.Id && d.CardId != this.CardId);

                                        if (otherCardContacts.length == 0) {
                                            this.Info2Text = null;
                                        }

                                        else {
                                            this.Info2Text = "This Contact is Already Added for " + otherCardContacts.length + " other Partners!";
                                        }
                                    }
                                });
                            }

                            else {
                                this.ValidateContactExist();
                            }
                        }
                    }
                });
            }
        }
    }

    public Validate() {
        var errors: string[] = [];

        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (!FormatTool.IsEmail(this.Email)) {
            errors.push("Invalid email format!");
        }

        if (this.ValidateContactExist()) {
            if (this.CardId != null) {
                errors.push("This Contact is Already added for you");
            }

            else {
                errors.push("Sorry this contact already exists!");
            }
        }

        return errors;
    }

    private ValidateContactExist() {
        var isContactAlreadyExist = false;

        if (this.loadedContact != null) {
            if (this.CardId != null) {
                if (this.allCardContacts != null) {
                    if (this.allCardContacts.filter(d => d.ContactId == this.loadedContact.Id && d.CardId == this.CardId).length > 0) {
                        isContactAlreadyExist = true;
                    }
                }

                this.Info1Text = isContactAlreadyExist ? TextCodeTranslator.Translate("Contact.M.ContactAddedForYou") : null;
            }

            else {
                isContactAlreadyExist = true;
            }
        }

        return isContactAlreadyExist;
    }
}

export class ContactInputTemplateArgs {
    public IsNewEntity: boolean = false;
    public EntityPM: ContactPM = null;
    public CardId: string = null;
    public IsCustomerVisible: boolean = false;
    public CardDependencyProperty1: string = null;
    public CustomerId: string = null;
    public CustomerLable: string = null;
    public CardDependencyProperty1IsList: boolean = false;
    public ComponentName: string = null;
    public BlockEditingEmail: boolean = false;
    public ShowSearchContacts: boolean = false;
    public ShowSecondPartOfWindow: string = null;
}
