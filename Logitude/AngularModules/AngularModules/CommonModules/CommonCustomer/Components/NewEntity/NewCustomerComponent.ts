import {Component, ViewChild, ViewContainerRef} from '@angular/core';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {NewPartnerTamplate} from '../../../CommonPartners/Components/Templates/NewPartnerTamplate';
import {PartnersDomainService, PartnerServicePM} from '../../../../Common/Services/PartnersDomainService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {CustomerPM} from '../../../../Common/EntityPMs/CustomerPM';
import {NewEntityArgs} from '../../../../Infrastructure/Args';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {AppTool} from '../../../../Infrastructure/Tools';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';

@Component({
    
    templateUrl: './NewCustomerComponent.html',
})

export class NewCustomerComponent {
    public EntityPM: CustomerPM;
    public PartnerTypeId: string = "CS";
    public ObjectTableName: string = "Customer";
    public ValidationErrorsList: string[] = [];
    public DomainService: PartnersDomainService;
    private PartnerTamplate: NewPartnerTamplate;
    @ViewChild('Child', { read: ViewContainerRef, static: false }) viewContainerRef: ViewContainerRef;
    private args: NewEntityArgs;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService) {
        this.EntityPM = new CustomerPM();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.EntityPM.PartnerTypeId = this.PartnerTypeId;
        this.EntityPM.CustomerStatusCode = "ACT";
        this.EntityPM.CreatedByUserId = SessionLocator.LoggedUserId;
        this.IsCustomer = false;

        if (SessionLocator.LoggedUserPM.IsSalesman) {
            this.EntityPM.SalesmanUserId = SessionLocator.LoggedUserId;
        }

        this.DomainService = new PartnersDomainService();
        this.RunComponent();
    }

    SetWindowArgs(args: NewEntityArgs) {
        if (args != null) {
            this.args = args;

            if (args.Perspective == "ShippersAndConsignees") {
                this.IsCustomer = false;
            }
            else {
                this.IsCustomer = true;
            }

        }
    }

    RunComponent() {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }

        else {
            this.RunComponentTimer();
        }
    }

    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }

    LoadChildComponent() {
        this.entityResourceService.getEntityResourceByTableName("Address", 0).subscribe(response=> {
            this.entityResourceService.getEntityResourceByTableName("Customer").subscribe(response2 => {
                SessionLocator.DynamicLoader.Load("./CommonModules/CommonPartners/Components/Templates/NewPartnerTamplate", this.viewContainerRef)
                    .then(cmpRef => {
                        this.PartnerTamplate = cmpRef.instance;
                        this.PartnerTamplate.EntityPM = this.EntityPM;
                        this.PartnerTamplate.CardTableName = this.ObjectTableName;
                        this.PartnerTamplate.PartnerTypeId = this.PartnerTypeId;
                        this.PartnerTamplate.DomainService = this.DomainService;
                        this.PartnerTamplate.IsCustomer = this.IsCustomer;

                        if (this.args) {
                            if (!AppTool.IsNullOrEmpty(this.args.DefaultValues)) {
                                this.PartnerTamplate.DefaultValues = this.args.DefaultValues;
                            }

                            if (this.args.Address) {
                                this.SetPartnerTamplateProperties();
                            }
                        }

                        this.PartnerTamplate.InitTemplate();
                        this.SetEnabled();
                    });
            });
        });
    }

    private SetPartnerTamplateProperties() {
        this.PartnerTamplate.City = this.args.Address.City;
        this.PartnerTamplate.CountryId = this.args.Address.CountryId;
        this.PartnerTamplate.CountryCode = this.args.Address.CountryCode;
        this.PartnerTamplate.StateId = this.args.Address.StateId;
        this.PartnerTamplate.Address1 = this.args.Address.Address1;
        this.PartnerTamplate.Address2 = this.args.Address.Address2;
        this.PartnerTamplate.ZipCode = this.args.Address.ZipCode;
        this.PartnerTamplate.PhoneNumber = this.args.Address.PhoneNumber;
        this.PartnerTamplate.FaxNumber = this.args.Address.FaxNumber;
        this.PartnerTamplate.Name = this.args.Address.Name;
    }

    public IsCustomerRadioEnabled: boolean = true;
    public IsOkButtonEnabled: boolean = true;
    public IsMessageVisible: boolean = false;
    private SetEnabled() {
        if (!FeatureLocator.HasFeaturePermession("Customer", "CREATEACTIVECUSTOMER")) {
            if (this.IsCustomer) {
                //this.IsCustomerRadioEnabled = false;
                this.IsOkButtonEnabled = false;
                this.IsMessageVisible = true;
            }

            else {
                //this.IsCustomerRadioEnabled = true;
                this.IsOkButtonEnabled = true;
                this.IsMessageVisible = false;
            }
        }        
    }

    SetIsCustomer(isCustomer: boolean) {
        this.IsCustomer = isCustomer;
    }
    get IsCustomer() { return this.EntityPM.IsCustomer; }
    set IsCustomer(newValue: boolean) {
        if (this.EntityPM.IsCustomer != newValue) {
            this.EntityPM.IsCustomer = newValue;

            this.SetEnabled();

            if (this.PartnerTamplate) {
                this.PartnerTamplate.IsCustomer = this.IsCustomer;
                this.PartnerTamplate.SetUIProperties();
            }
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = this.PartnerTamplate.Validate();

        if (errors.length == 0) {
            this.EntityPM.Code = this.PartnerTamplate.CardCode;
            this.EntityPM.EnglishName = this.PartnerTamplate.Name;
            this.EntityPM.LocalName = !AppTool.IsNullOrEmpty(this.PartnerTamplate.LocalName) ? this.PartnerTamplate.LocalName : this.PartnerTamplate.Name;
            this.EntityPM.VatNumber = this.PartnerTamplate.VatNumber;
            this.EntityPM.SalesmanUserId = this.PartnerTamplate.SalesmanUserId;
            this.EntityPM.ExistedContactId = this.PartnerTamplate.ExistedContactId;
            
            Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        }

        this.ValidationErrorsList = errors;

        if (this.IsCustomer) {
            if (!FeatureLocator.HasFeaturePermession("Customer", "CREATEACTIVECUSTOMER")) {
                errors.push("You are not allowed to add a new active customer");
            }
        }

        if (errors.length == 0) {

            this.CurrentSession.StartBusyIndicatorSaving();

            //this.EntityPM.UIProperties = null;

            var args = new PartnerServicePM();
            args.Tenant = this.EntityPM.Tenant;
            args.PartnerTypeId = this.PartnerTypeId;
            args.Customer = this.EntityPM;
            args.Address = this.PartnerTamplate.Address;            
            if (this.PartnerTamplate.IsAddContactChecked) {
                this.PartnerTamplate.Contact.SetAsPrimaryForCard = true;
                args.Contact = this.PartnerTamplate.Contact;        
            }

            this.DomainService.PostPartnerAddress(args).subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();
                if (!myResponse.HasError) {
                    this.EntityPM = myResponse.Result.Customer;
                    this.CurrentSession.CloseCurrentWindowEmit(this.EntityPM.Id);
                }

                else {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }
            });
        }
    }
}
