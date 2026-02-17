import {Component, Output, EventEmitter} from '@angular/core';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AddressPM} from '../../../EntityPMs/AddressPM';
import {AddressPMService} from '../../../Services/StandardPMs/AddressPMService';
import {AppTool} from '../../../../Infrastructure/Tools';
import {AddressValidator} from '../../../../Infrastructure/Validators/AddressValidator';
import {PartnersDomainService, PartnerServicePM} from '../../../../Common/Services/PartnersDomainService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {CitySelectionArgs} from '../../../Args';
import {StateList} from '../../../EntityLists/StateList';
import {CountryList} from '../../../EntityLists/CountryList';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditBranchAddressComponent.html',
})

export class AddEditBranchAddressComponent extends BaseComponent {
    public AddressPM: AddressPM;
    public DataContext: AddEditBranchAddressComponent = this;
    public ObjectTableName: string = "Address";
    public ValidationErrorsList: string[] = [];
    public DomainService: PartnersDomainService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }

    SetWindowArgs(windowArgs: AddressPM) {
        this.AddressPM = windowArgs;
        this.SetUIProperties();
        this.Clone();
    }

    public SetUIProperties() {        
        this.SetUIProperties_StateEnabled();
        this.SetUIProperties_StateRequired();
    }
    private SetUIProperties_StateEnabled() {
        var isEnabled = false;

        if (this.Country != null) {
            if (this.Country.HasStates) {
                isEnabled = true;
            }
        }

        this.UIProperties.SetEnabled("StateId", this.ObjectTableName, isEnabled);
    }
    private SetUIProperties_StateRequired() {
        var isRequired = false;

        if (this.Country != null) {
            if (this.State == null) {
                if (this.Country.IsStateRequired) {
                    isRequired = true;
                }
            }
        }

        this.UIProperties.SetRequired("StateId", this.ObjectTableName, isRequired);
    }

    get Name() { return this.AddressPM.Name; }
    set Name(newValue: string) {
        if (this.AddressPM.Name != newValue) {
            this.AddressPM.Name = newValue;
        }
    }

    get Address1() { return this.AddressPM.Address1; }
    set Address1(newValue: string) {
        if (this.AddressPM.Address1 != newValue) {
            this.AddressPM.Address1 = newValue;
        }
    }

    get Address2() { return this.AddressPM.Address2; }
    set Address2(newValue: string) {
        if (this.AddressPM.Address2 != newValue) {
            this.AddressPM.Address2 = newValue;
        }
    }

    get City() { return this.AddressPM.City; }
    set City(newValue: string) {
        if (this.AddressPM.City != newValue) {
            this.AddressPM.City = newValue;
        }
    }

    get ZipCode() { return this.AddressPM.ZipCode; }
    set ZipCode(newValue: string) {
        if (this.AddressPM.ZipCode != newValue) {
            this.AddressPM.ZipCode = newValue;
        }
    }

    get PhoneNumber() { return this.AddressPM.PhoneNumber; }
    set PhoneNumber(newValue: string) {
        if (this.AddressPM.PhoneNumber != newValue) {
            this.AddressPM.PhoneNumber = newValue;
        }
    }

    get FaxNumber() { return this.AddressPM.FaxNumber; }
    set FaxNumber(newValue: string) {
        if (this.AddressPM.FaxNumber != newValue) {
            this.AddressPM.FaxNumber = newValue;
        }
    }

    get ATTN() { return this.AddressPM.ATTN; }
    set ATTN(newValue: string) {
        if (this.AddressPM.ATTN != newValue) {
            this.AddressPM.ATTN = newValue;
        }
    }
    
    get CountryId() { return this.AddressPM.CountryId; }
    set CountryId(newValue: string) {
        if (this.AddressPM.CountryId != newValue) {
            this.AddressPM.CountryId = newValue;
            this.StateId = null;
        }
    }
   
    get StateId() { return this.AddressPM.StateId; }
    set StateId(newValue: string) {
        if (this.AddressPM.StateId != newValue) {
            this.AddressPM.StateId = newValue;
        }
    }

    private country: CountryList = null;
    get Country() { return this.country; }
    set Country(newValue: CountryList) {
        if (this.country != newValue) {
            this.country = newValue;
            this.OnCountryChanged(newValue);
        }
    }

    private state: StateList = null;
    get State() { return this.state; }
    set State(newValue: StateList) {
        if (this.state != newValue) {
            this.state = newValue;
            this.OnStateChanged(newValue);
        }
    }

    private OnCountryChanged(list: CountryList) {        
        this.SetUIProperties();
    }
    private OnStateChanged(list: StateList) {        
        this.SetUIProperties_StateRequired();
    }

    // Commands
    SelectCityCommand() {
        var args = new CitySelectionArgs(this.CountryId);
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Select City";
        logWindow.WindowArgs = args;
        logWindow.Show("./CommonModules/CommonOthers/Components/CitySelection/CitySelectionComponent");
        logWindow.WindowClosed.subscribe(($event: any) => {
            if (args.IsCitySelected) {

                var mySelectedCity: string = args.CityName;
                if (this.AddressPM.IsLocalLanguage && args.CityLocalName != null) {
                    mySelectedCity = args.CityLocalName;
                }

                this.City = mySelectedCity;
                this.CountryId = args.CountryId;
                this.StateId = args.StateId;
            }
        });
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
            if (!this.AddressPM.IsDirty) {
                this.CurrentSession.CloseCurrentWindow();
            }

            else {
                this.Save();
            }
        }
    }
    private Validate() {
        var isValid = true;
        var errors: string[] = [];

        if (this.AddressPM != null) {
            var msg: string = TextCodeTranslator.Translate("General.M.FieldIsRequired");

            Validator.TryValidateObject(this.AddressPM, this.ObjectTableName, errors);
            
            if (this.Country != null) {
                if (this.State == null) {
                    if (this.Country.IsStateRequired) {
                        errors.push(msg.replace("%FieldName", "State"));
                    }
                }
            }
        }

        isValid = errors.length == 0 ? true : false;
        this.ValidationErrorsList = errors;
        return isValid;
    }
    
    private Save() {
        var myService: AddressPMService = new AddressPMService();

        if (AppTool.IsNullOrEmpty(this.AddressPM.Id)) {
            myService.insert(this.AddressPM).subscribe(myResult => {
                var mm: ServiceResponse = myResult;
                if (!mm.HasError) {
                    this.CurrentSession.StopBusyIndicator();
                    this.CurrentSession.CloseCurrentWindowEmit(this.AddressPM.Id);
                }

                else {
                    this.ValidationErrorsList = mm.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }
            });
        }

        else {
            myService.update(this.AddressPM).subscribe(myResult => {
                var mm: ServiceResponse = myResult;
                if (!mm.HasError) {
                    this.CurrentSession.StopBusyIndicator();
                    this.CurrentSession.CloseCurrentWindowEmit(this.AddressPM.Id);
                }

                else {
                    this.ValidationErrorsList = mm.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }
            });
        }
    }  

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
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
        this.myCloner.AddEntity(this.AddressPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
