import {Component, OnDestroy} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {BranchPM} from '../../../EntityPMs/BranchPM';
import {AddressPM} from '../../../EntityPMs/AddressPM';
import {EntityArgs} from  '../../../../Infrastructure/DataContracts/EntityArgs';
import {AppTool} from '../../../../Infrastructure/Tools';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {AddressPMService} from '../../../Services/StandardPMs/AddressPMService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {AddEditBranchAddressComponent} from './AddEditBranchAddressComponent';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';

@Component({
    moduleId: module.id,
    templateUrl: './BranchGeneralTabComponent.html',
})

export class BranchGeneralTabComponent extends BaseComponent implements OnDestroy {
    public EntityPM: BranchPM;
    public Address: AddressPM;
    public DataContext: BranchGeneralTabComponent = this;
    public IsNewEntity: boolean = true;
    public ObjectTableName: string = "Branch";
    private addressService: AddressPMService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public args: EntityArgs) {
        super();

        this.EntityPM = args.EntityPM;
        this.addressService = new AddressPMService();
        this.Listen();

        if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            this.IsNewEntity = true;
        }

        else {
            this.IsNewEntity = false;
            this.LoadAddress();
        }      
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.LoadAddress();
                    }
                });
            }

            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.LoadAddress();
                    }
                });
            }
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    get EnglishName() { return this.EntityPM.EnglishName; }
    set EnglishName(value: string) {
        if (this.EntityPM.EnglishName != value) {
            this.EntityPM.EnglishName = value;
        }
    }

    get LocalName() { return this.EntityPM.LocalName; }
    set LocalName(value: string) {
        if (this.EntityPM.LocalName != value) {
            this.EntityPM.LocalName = value;
        }
    }

    get Code() { return this.EntityPM.Code; }
    set Code(value: string) {
        if (this.EntityPM.Code != value) {
            this.EntityPM.Code = value;
        }
    }

    get Signature() { return this.EntityPM.Signature; }
    set Signature(value: string) {
        if (this.EntityPM.Signature != value) {
            this.EntityPM.Signature = value;
        }
    }

    get CounterCode() { return this.EntityPM.CounterCode; }
    set CounterCode(value: string) {
        if (this.EntityPM.CounterCode != value) {
            this.EntityPM.CounterCode = value;
        }
    }

    get InActive() { return this.EntityPM.InActive; }
    set InActive(value: boolean) {
        if (this.EntityPM.InActive != value) {
            this.EntityPM.InActive = value;
        }
    }

    private LoadAddress() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.AddressId)) {
            this.addressService.get(this.EntityPM.AddressId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.Address = myResponse.Result;

                    this.FillAddressProperties();
                }
            });
        }
    }

    public AddressName: string;
    public Address1: string;
    public Address2: string;
    public CityLineText: string;
    public CountryName: string;
    public PhoneNumber: string;
    public FaxNumber: string;
    public FlagSrc: string;
    private FillAddressProperties() {
        if (this.Address != null) {
            this.AddressName = this.Address.Name;
            this.Address1 = this.Address.Address1;
            this.Address2 = this.Address.Address2;            
            this.CountryName = this.Address.CountryName;
            this.PhoneNumber = this.Address.PhoneNumber;
            this.FaxNumber = this.Address.FaxNumber;

            if (!AppTool.IsNullOrEmpty(this.Address.CountryCode)) {
                this.FlagSrc = "./Images/Flags/" + this.Address.CountryCode + ".png";
            }
            
            this.BuildCityString();            
        }
    }

    private BuildCityString() {
        var myResult = this.Address.City;

        if (!AppTool.IsNullOrEmpty(this.Address.StateEnglishName)) {
            myResult += ", " + this.Address.StateEnglishName;
        }

        if (!AppTool.IsNullOrEmpty(this.Address.ZipCode)) {
            myResult += ", " + this.Address.ZipCode;
        }

        this.CityLineText = myResult;
    }

    EditAddressClicked() {
        var service: EntityResourceService = new EntityResourceService();
        service.getEntityResourceByTableName("Address", 0).subscribe((response1: any) => {
            var logWindow = new LogitudeWindow();

            if (AppTool.IsNullOrEmpty(this.EntityPM.AddressId)) {
                logWindow.Title = "Add Address";

                var address: AddressPM = this.addressService.GetNewEntityPM();
                address.AddressTypeId = "M";
                address.Description = "Main Address";
                address.BranchId = this.EntityPM.Id;

                logWindow.WindowArgs = address;
            }

            else {
                logWindow.Title = "Edit Address";
                logWindow.WindowArgs = this.Address;
            }

            logWindow.Show('./Common/Components/Maintenance/Branch/AddEditBranchAddressComponent');

            logWindow.WindowClosed.subscribe(s => {
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                //this.CurrentSession.FireEvent("LoadAddress");
            });
        });        
    }  
}
