import {Component} from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import { TariffPM } from '../../EntityPMs/TariffPM';
import { TariffPMService } from '../../Services/StandardPMs/TariffPMService';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {DateTool} from '../../../Infrastructure/Tools';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { AppTool } from '../../../Infrastructure/Tools';
import { ChargesTypeListService } from '../../../Common/Services/StandardLists/ChargesTypeListService';
import { ChargesTypeList } from '../../../Common/EntityLists/ChargesTypeList';
import { ClassLevelValidator } from '../../../Infrastructure/Validators/ClassLevelValidator'
@Component({
    selector: 'NewAirFreightCostComponent',
    moduleId: module.id,
    templateUrl: './NewAirFreightCostComponent.html',
})

export class NewAirFreightCostComponent extends BaseComponent {
    private CurrentSession = SessionLocator.SelectedSession;

    public DataContext = this;
    public ObjectTableName = "Tariff";
    public EntityPM: TariffPM;
    public SelectedLocationFilter: any;
    public VisibileSurchargesArea: boolean = false;
    public ChargeTypesQueryFilters: ApiQueryFilters;
    private chargesTypePMService: ChargesTypeListService;
    private IdProps: string[] = [];
    private UOMProps: string[] = [];

    constructor() {
        super();
        this.chargesTypePMService = new ChargesTypeListService();
        var todayDate: Date = DateTool.GetCurrentDateAsUtc();
        this.EntityPM = new TariffPM();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.EntityPM.CreatedByUserId = SessionLocator.LoggedUserId;
        this.EntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
        this.FillChargesIDsAndUOMS();

    }

    FillChargesIDsAndUOMS() {
        for (var index = 1; index <= 10; index++) {
            this.IdProps.push("Surcharge" + index + "Id");
            this.UOMProps.push("Surcharge" + index + "UOM");
        }
    }

    BuildQueryFilters() {
        this.ChargeTypesQueryFilters = new ApiQueryFilters();
        this.ChargeTypesQueryFilters.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "Boolean");
        this.ChargeTypesQueryFilters.addAdditionalFilter("IsAir", true, null, null, "Equals", false, false, false, "Boolean");
        this.ChargeTypesQueryFilters.addAdditionalFilter("ChargesGroupCode", "FRT", null, null, "NotEqual", false, false, false, "string");
        this.Validate(true);
    }


    Validate(initial: boolean=false) {
        for (var index = 1; index <= 10; index++) {          
            if (initial) {
                if (index != 1) {
                    this.UIProperties.SetEnabled(this.IdProps[index - 1], this.ObjectTableName, false);
                    this.UIProperties.SetEnabled(this.UOMProps[index - 1], this.ObjectTableName, false);
                }
                else {
                    this.UIProperties.SetEnabled(this.IdProps[index - 1], this.ObjectTableName, true);
                    this.UIProperties.SetEnabled(this.UOMProps[index - 1], this.ObjectTableName, false);
                    this.UIProperties.SetRequired(this.IdProps[index - 1], this.ObjectTableName, true);
                    this.UIProperties.SetRequired(this.UOMProps[index - 1], this.ObjectTableName, true);
                }
            }
            if (!initial) {
                if (AppTool.IsNullOrEmpty(this[this.IdProps[index - 1]])) {
                    this[this.UOMProps[index - 1]] = null;
                    
                    this.UIProperties.SetEnabled(this.UOMProps[index - 1], this.ObjectTableName, false);
                    if (index > 1) {
                        if (!AppTool.IsNullOrEmpty(this[this.UOMProps[index - 2]]) && !AppTool.IsNullOrEmpty(this[this.IdProps[index - 2]])) {
                            this.UIProperties.SetEnabled(this.IdProps[index - 1], this.ObjectTableName, true);
                            this.UIProperties.SetEnabled(this.UOMProps[index - 1], this.ObjectTableName, false);
                        }
                    }
                    else {
                        this.UIProperties.SetRequired(this.IdProps[index - 1], this.ObjectTableName, true);
                        this.UIProperties.SetRequired(this.UOMProps[index - 1], this.ObjectTableName, true);
                    }



                }
                else {
                    this.UIProperties.SetEnabled(this.UOMProps[index - 1], this.ObjectTableName, true);
                    if (index == 1) {
                        this.UIProperties.SetRequired(this.IdProps[index - 1], this.ObjectTableName, false);
                        if (AppTool.IsNullOrEmpty(this[this.UOMProps[index - 1]])) {
                            this.UIProperties.SetRequired(this.UOMProps[index - 1], this.ObjectTableName, true);
                        }
                        else {
                            this.UIProperties.SetRequired(this.UOMProps[index - 1], this.ObjectTableName, false);
                        }
                    }
                }
            }
           
        }       
    }


    SetDefaultUOM(index:number) {
            this.chargesTypePMService.getSingleFromCache(this[this.IdProps[index]]).subscribe(res => {
                if (!res.HasError) {
                    if (res.Result) {
                        var ChargesType: ChargesTypeList = res.Result;
                        this[this.UOMProps[index]] = ChargesType.MeasurementId;
                    }
                }
            });        
    }



    public MeasurementId: string;
    public ChargesTypeId: string;

    SetWindowArgs(args) {
        this.EntityPM.TypeCode = args.TypeCode;
        if (this.EntityPM.TypeCode == "ASC") {
            this.VisibileSurchargesArea = true;
            this.BuildQueryFilters();
        }
        else {
            this.VisibileSurchargesArea = false;
        }
    }

    get Name() {
        return this.EntityPM.Name;
    }
    set Name(value: string) {
        if (this.EntityPM.Name != value) {
            this.EntityPM.Name = value;
        }
    }

    get StartDate() {
        return this.EntityPM.StartDate;
    }
    set StartDate(value: Date) {
        if (this.EntityPM.StartDate != value) {
            this.EntityPM.StartDate = value;
        }
    }

    get ExpirationDate() {
        return this.EntityPM.ExpirationDate;
    }
    set ExpirationDate(value: Date) {
        if (this.EntityPM.ExpirationDate != value) {
            this.EntityPM.ExpirationDate = value;
        }
    }

    get Surcharge1Id() {
        return this.EntityPM.Surcharge1Id;
    }
    set Surcharge1Id(value: string) {
        if (this.EntityPM.Surcharge1Id != value) {            
            this.EntityPM.Surcharge1Id = value;
            this.Validate();
            if (value != null) {
                this.SetDefaultUOM(0);
            }
        }
    }


    get Surcharge2Id() {
        return this.EntityPM.Surcharge2Id;
    }
    set Surcharge2Id(value: string) {
        if (this.EntityPM.Surcharge2Id != value) {
            this.EntityPM.Surcharge2Id = value;
            this.Validate();
            if (value != null) {
                this.SetDefaultUOM(1);
            }
        }
    }


    get Surcharge3Id() {
        return this.EntityPM.Surcharge3Id;
    }
    set Surcharge3Id(value: string) {
        if (this.EntityPM.Surcharge3Id != value) {
            this.EntityPM.Surcharge3Id = value;
            this.Validate();
            if (value != null) {
                this.SetDefaultUOM(2);
            }
        }
    }



    get Surcharge4Id() {
        return this.EntityPM.Surcharge4Id;
    }
    set Surcharge4Id(value: string) {
        if (this.EntityPM.Surcharge4Id != value) {
            this.EntityPM.Surcharge4Id = value;
            this.Validate();
            if (value != null) {
                this.SetDefaultUOM(3);
            }
        }
    }



    get Surcharge5Id() {
        return this.EntityPM.Surcharge5Id;
    }
    set Surcharge5Id(value: string) {
        if (this.EntityPM.Surcharge5Id != value) {
            this.EntityPM.Surcharge5Id = value;
            this.Validate();
            if (value != null) {
                this.SetDefaultUOM(4);
            }
        }
    }


    get Surcharge6Id() {
        return this.EntityPM.Surcharge6Id;
    }
    set Surcharge6Id(value: string) {
        if (this.EntityPM.Surcharge6Id != value) {
            this.EntityPM.Surcharge6Id = value;
            this.Validate();
            if (value != null) {
                this.SetDefaultUOM(5);
            }
        }
    }


    get Surcharge7Id() {
        return this.EntityPM.Surcharge7Id;
    }
    set Surcharge7Id(value: string) {
        if (this.EntityPM.Surcharge7Id != value) {
            this.EntityPM.Surcharge7Id = value;
            this.Validate();
            if (value != null) {
                this.SetDefaultUOM(6);
            }
        }
    }


    get Surcharge8Id() {
        return this.EntityPM.Surcharge8Id;
    }
    set Surcharge8Id(value: string) {
        if (this.EntityPM.Surcharge8Id != value) {
            this.EntityPM.Surcharge8Id = value;
            this.Validate();
            if (value != null) {
                this.SetDefaultUOM(7);
            }
        }
    }


    get Surcharge9Id() {
        return this.EntityPM.Surcharge9Id;
    }
    set Surcharge9Id(value: string) {
        if (this.EntityPM.Surcharge9Id != value) {
            this.EntityPM.Surcharge9Id = value;
            this.Validate();
            if (value != null) {
                this.SetDefaultUOM(8);
            }
        }
    }


    get Surcharge10Id() {
        return this.EntityPM.Surcharge10Id;
    }
    set Surcharge10Id(value: string) {
        if (this.EntityPM.Surcharge10Id != value) {
            this.EntityPM.Surcharge10Id = value;
            this.Validate();
            if (value != null) {
                this.SetDefaultUOM(9);
            }
        }
    }



    get Surcharge1UOM() {
        return this.EntityPM.Surcharge1UOM;
    }
    set Surcharge1UOM(value: string) {
        if (this.EntityPM.Surcharge1UOM != value) {
            this.EntityPM.Surcharge1UOM = value;
            this.Validate();


        }
    }



    get Surcharge2UOM() {
        return this.EntityPM.Surcharge2UOM;
    }
    set Surcharge2UOM(value: string) {
        if (this.EntityPM.Surcharge2UOM != value) {
            this.EntityPM.Surcharge2UOM = value;
            this.Validate();

        }
    }

    get Surcharge3UOM() {
        return this.EntityPM.Surcharge3UOM;
    }
    set Surcharge3UOM(value: string) {
        if (this.EntityPM.Surcharge3UOM != value) {
            this.EntityPM.Surcharge3UOM = value;
            this.Validate();

        }
    }

    get Surcharge4UOM() {
        return this.EntityPM.Surcharge4UOM;
    }
    set Surcharge4UOM(value: string) {
        if (this.EntityPM.Surcharge4UOM != value) {
            this.EntityPM.Surcharge4UOM = value;
            this.Validate();

        }
    }


    get Surcharge5UOM() {
        return this.EntityPM.Surcharge5UOM;
    }
    set Surcharge5UOM(value: string) {
        if (this.EntityPM.Surcharge5UOM != value) {
            this.EntityPM.Surcharge5UOM = value;
            this.Validate();
    
        }
    }

    get Surcharge6UOM() {
        return this.EntityPM.Surcharge6UOM;
    }
    set Surcharge6UOM(value: string) {
        if (this.EntityPM.Surcharge6UOM != value) {
            this.EntityPM.Surcharge6UOM = value;
            this.Validate();

        }
    }

    get Surcharge7UOM() {
        return this.EntityPM.Surcharge7UOM;
    }
    set Surcharge7UOM(value: string) {
        if (this.EntityPM.Surcharge7UOM != value) {
            this.EntityPM.Surcharge7UOM = value;
            this.Validate();

        }
    }

    get Surcharge8UOM() {
        return this.EntityPM.Surcharge8UOM;
    }
    set Surcharge8UOM(value: string) {
        if (this.EntityPM.Surcharge8UOM != value) {
            this.EntityPM.Surcharge8UOM = value;
            this.Validate();

        }
    }

    get Surcharge9UOM() {
        return this.EntityPM.Surcharge9UOM;
    }
    set Surcharge9UOM(value: string) {
        if (this.EntityPM.Surcharge9UOM != value) {
            this.EntityPM.Surcharge9UOM = value;
            this.Validate();

        }
    }

    get Surcharge10UOM() {
        return this.EntityPM.Surcharge10UOM;
    }
    set Surcharge10UOM(value: string) {
        if (this.EntityPM.Surcharge10UOM != value) {
            this.EntityPM.Surcharge10UOM = value;
            this.Validate();

        }
    }
    


    get Description() {
        return this.EntityPM.Description;
    }
    set Description(value: string) {
        if (this.EntityPM.Description != value) {
            this.EntityPM.Description = value;
        }
    }



    get CurrencyId() {
        return this.EntityPM.CurrencyId;
    }
    set CurrencyId(value: string) {
        if (this.EntityPM.CurrencyId != value) {
            this.EntityPM.CurrencyId = value;
        }
    }


    get SellerId() {
        return this.EntityPM.SellerId;
    }
    set SellerId(value: string) {
        if (this.EntityPM.SellerId != value) {
            this.EntityPM.SellerId = value;
        }
    }


    get ContractNumber() {
        return this.EntityPM.ContractNumber;
    }
    set ContractNumber(value: number) {
        if (this.EntityPM.ContractNumber != value) {
            this.EntityPM.ContractNumber = value;
        }
    }



    // Commands
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    public ValidationErrorsList: string[] = [];

    ValidateSurcharge() {
        var IdProps: string[] = [];
        var UOMProps: string[] = [];
        var IdPropsName: string[] = [];
        var UOMPropsName: string[] = [];
        var EmptyIndex = 1;
        for (var index = 1; index <= 10; index++) {
            IdProps.push("Surcharge" + index + "Id");
            UOMProps.push("Surcharge" + index + "UOM");

            IdPropsName.push("Charge Type " + index );
            UOMPropsName.push("UOM " + index);
            if (index == 1) {
                if (AppTool.IsNullOrEmpty(this[IdProps[index - 1]])) {
                    this.ValidationErrorsList.push(IdPropsName[index - 1] + " is required");
                }

                if (AppTool.IsNullOrEmpty(this[UOMProps[index - 1]])) {
                    this.ValidationErrorsList.push(UOMPropsName[index - 1] + " is required");
                }
            }

            else {
                if (AppTool.IsNullOrEmpty(this[IdProps[index - 1]])) {
                    if (EmptyIndex == 1) {
                        EmptyIndex = index;
                    }
                }

                if (AppTool.IsNullOrEmpty(this[UOMProps[index - 1]]) && !AppTool.IsNullOrEmpty(this[IdProps[index - 1]])) {
                    this.ValidationErrorsList.push(IdPropsName[index - 1] + " is filled without a UOM");
                }

                if (index >= 3) {
                    if (!AppTool.IsNullOrEmpty(this[UOMProps[index - 1]]) && !AppTool.IsNullOrEmpty(this[IdProps[index - 1]])) {
                        if (EmptyIndex != 1) {
                            this.ValidationErrorsList.push("No empty charge lines between line " + (EmptyIndex - 1) + " and line " + index);
                            EmptyIndex = 1;
                        }
                        if (AppTool.IsNullOrEmpty(this[IdProps[index - 2]])) {
                         //   this.ValidationErrorsList.push("no empty line between 2 charges in line "+index+ " and "+(index-2));
                        }
                    }
                }
            }
        }
    }
  

    OkButtonClicked() {
        this.ValidationErrorsList = [];
        if (this.StartDate != null && this.ExpirationDate != null) {
            if (this.ExpirationDate < this.StartDate) {
                this.ValidationErrorsList.push("Expiration date must be less than start date");
            }
        }
        if (this.EntityPM.TypeCode == "ASC") {
            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = validator.Validate("Tariff", this.EntityPM);
            this.ValidationErrorsList = errorsArray;
           this.ValidateSurcharge();
        }
      
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicator("Creating...");
            var myService: TariffPMService = new TariffPMService();
            myService.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();
                if (!myResponse.HasError) {
                    this.CurrentSession.CloseCurrentWindowEmit('OK');
                }
                else {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }
            });
        }
    }
}
