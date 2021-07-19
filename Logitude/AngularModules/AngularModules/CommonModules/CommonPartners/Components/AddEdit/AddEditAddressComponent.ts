import {Component, OnInit} from '@angular/core';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AddressPM} from '../../../../Common/EntityPMs/AddressPM';
import {AddressItemClass} from '../EditTabs/AddressesTabComponent';
import {AppTool} from '../../../../Infrastructure/Tools';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {AddressValidator} from '../../../../Infrastructure/Validators/AddressValidator';
import {PartnersDomainService, PartnerServicePM} from '../../../../Common/Services/PartnersDomainService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import { AddressItem } from '../../../../InfrastructureModules/InfrastructureGettingStarted/Components/CompanyAddress/CompanyAddressSettingsComponent';

@Component({
    
    templateUrl: './AddEditAddressComponent.html',
})

export class AddEditAddressComponent implements OnInit {
    public ObjectTableName: string;
    public EntityPM: AddressPM = null;
    public DataContext: AddressItemClass;
    public ValidationErrorsList: string[] = [];
    public DomainService: PartnersDomainService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

    }

    ngOnInit() {
        if (this.DataContext != null) {
            this.DataContext.SetUIProperties();
        }
    }

    SetDataContext(dataContext: AddressItemClass) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.ObjectTableName = dataContext.ObjectTableName;
        this.DomainService = dataContext.fatherComponent.DomainService;

        this.Clone();

        if (dataContext.IsNewEntity) {
            dataContext.Name = dataContext.fatherComponent.EntityPM.EnglishName;
        }

        if (dataContext.IsCopyMainAddress) {
            dataContext.CopyMainAddress();
        }
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        this.CurrentSession.StartBusyIndicatorSaving();

        var isValid = this.Validate();
        if (!isValid) {
            this.CurrentSession.StopBusyIndicator();
        }

        else {
            if (!this.EntityPM.IsDirty) {
                this.CurrentSession.CloseCurrentWindow();
            }

            else {
                this.Save();
            }
        }
    }

    private errors: string[];
    private Validate() {
        var isValid = true;
        this.errors = [];

        if (this.EntityPM != null) {
            var msg: string = TextCodeTranslator.Translate("General.M.FieldIsRequired");

            this.ValidateAddress();

            var isLanguageValid = AddressValidator.IsMainAddressEnglishCharacters(this.EntityPM);
            if (!isLanguageValid) {
                this.errors.push("Main address does not allow non-english characters");
            }

            if (this.EntityPM.AddressTypeId == "O") {
                if (AppTool.IsNullOrEmpty(this.EntityPM.Description)) {
                    this.errors.push(msg.replace("%FieldName", "Description"));
                }
            }

            if (this.DataContext.Country != null) {
                if (this.DataContext.State == null) {
                    if (this.DataContext.Country.IsStateRequired) {
                        this.errors.push(msg.replace("%FieldName", "State"));
                    }
                }
            }

            if (this.DataContext.fatherComponent.Customer != null) {
                if (this.DataContext.fatherComponent.Customer.IsCustomer) {
                    if (this.DataContext.fatherComponent.Customer.PartnerTypeId == "CS") {
                        if (SessionLocator.TenantPM.IsCustomerTelRequired) {
                            if (AppTool.IsNullOrEmpty(this.EntityPM.PhoneNumber)) {
                                this.errors.push("Phone Number is required");
                            }
                        }

                        if (SessionLocator.TenantPM.IsCustomerFaxRequired) {
                            if (AppTool.IsNullOrEmpty(this.EntityPM.FaxNumber)) {
                                this.errors.push("Fax Number is required");
                            }
                        }
                    }

                    else if (this.DataContext.fatherComponent.Customer.PartnerTypeId == "PO") {
                        if (SessionLocator.TenantPM.IsPotentialTelRequired) {
                            if (AppTool.IsNullOrEmpty(this.EntityPM.PhoneNumber)) {
                                this.errors.push("Phone Number is required");
                            }
                        }

                        if (SessionLocator.TenantPM.IsPotentialFaxRequired) {
                            if (AppTool.IsNullOrEmpty(this.EntityPM.FaxNumber)) {
                                this.errors.push("Fax Number is required");
                            }
                        }
                    }
                }
            }
        }

        isValid = this.errors.length == 0 ? true : false;
        this.ValidationErrorsList = this.errors;
        return isValid;
    }

    private ValidateAddress() {
      if (this.DataContext.fatherComponent.Customer != null) {
        var newPotentialAddressCity = this.EntityPM.City;
        if (AppTool.IsNullOrEmpty(this.EntityPM.City) && this.DataContext.fatherComponent.Customer.PartnerTypeId == "PO") {
            this.EntityPM.City = (AppTool.IsNullOrEmpty(this.EntityPM.City) ? " Potential city " : this.EntityPM.City);
        }

        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, this.errors);

        if (this.DataContext.fatherComponent.Customer.PartnerTypeId == "PO") {
            this.EntityPM.City = newPotentialAddressCity;
        }
      }
    }

    private LoadCompletedEvent: any = null;
    private Save() {
        var args = new PartnerServicePM();
        args.Tenant = this.EntityPM.Tenant;
        args.AddressId = this.EntityPM.Id;
        args.PartnerId = this.EntityPM.CardId;
        args.Address = this.EntityPM;
        args.IsAddressDirty = this.EntityPM.IsDirty;

        if (this.DataContext.fatherComponent) {
            args.IsPartnerDirty = this.DataContext.fatherComponent.EntityPM.IsDirty;
            args.PartnerTypeId = this.DataContext.fatherComponent.PartnerTypeId;
        }

        this.DomainService.SetPartner(args, this.DataContext.fatherComponent.EntityPM);

        this.DomainService.PostPartnerAddress(args).subscribe((myResponse: ServiceResponse) => {

            this.CurrentSession.StopBusyIndicator();

            if (myResponse.HasError) {
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }

            else {
                this.DataContext.EntityPM = myResponse.Result.Address;

                if (!this.LoadCompletedEvent) {
                    this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isSuccess: boolean) => {

                        AppTool.KillEventEmitter(this.LoadCompletedEvent);
                        this.LoadCompletedEvent = null;

                        if (isSuccess == false) {
                            this.CurrentSession.StopBusyIndicator();
                        }

                        else {
                            if (this.DataContext.IsNewEntity) {
                                this.DataContext.fatherComponent.DomainService.GetAllAddressesPMsbyCardId(this.EntityPM.CardId).subscribe((myResult: any) => {
                                    this.DataContext.fatherComponent.AllAddresses = myResult;
                                    this.DataContext.fatherComponent.BuildItemsSource();
                                    this.CurrentSession.CloseCurrentWindow();
                                });
                            }

                            else {
                                this.DataContext.fatherComponent.BuildItemsSource();
                                this.CurrentSession.CloseCurrentWindow();
                            }
                        }                       
                    });

                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                }
            }            
        });
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('Description');
        this.myCloner.AddField('Name');
        this.myCloner.AddField('Address1');
        this.myCloner.AddField('Address2');
        this.myCloner.AddField('City');
        this.myCloner.AddField('StateId');
        this.myCloner.AddField('CountryId');
        this.myCloner.AddField('ZipCode');
        this.myCloner.AddField('PhoneNumber');
        this.myCloner.AddField('FaxNumber');
        this.myCloner.AddField('ATTN');
        this.myCloner.AddField('InActive');
        this.myCloner.AddEntity(this.EntityPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
