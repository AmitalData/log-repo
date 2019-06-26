import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { TariffVersionPM } from '../../../EntityPMs/TariffVersionPM';
import { UpdateTariffArgs } from '../../../Args';
import { CodeNameClass } from '../../../../Infrastructure/DataContracts/CodeNameClass';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { PortList } from '../../../../Common/EntityLists/PortList';
import { AirlineAreaList } from '../../../../Common/EntityLists/AirlineAreaList';
import { CommonDomainService } from '../../../../Common/Services/CommonDomainService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { TariffDomainService, UpdateSurchargeArgs } from '../../../Services/TariffDomainService';

@Component({
    moduleId: module.id,
    templateUrl: './UpdateSurchargesComponent.html',
})

export class UpdateSurchargesComponent extends BaseComponent {
    private CurrentSession = SessionLocator.SelectedSession;
    public DataContext = this;
    public ObjectTableName = "Tariff";
    public EntityPM: TariffVersionPM;
    public Logs= [];
    public ValidationErrorsList: string[] = [];
    public TariffChargesObsList: TariffCharge[];
    public AirlineAreas: AirlineAreaClass[];
    public SearchAreaButtonId: string = "SearchAreaButtonId";
    constructor() {
        super();        
    }
    
    SetWindowArgs(arg: UpdateTariffArgs) {
        this.EntityPM = arg.Version;

        this.FillTariffCharges(arg.TariffCharges);
        this.LoadAirlineAreas(arg.AirlineId);
    }

    private LoadAirlineAreas(airlineId: string) {
        var service: CommonDomainService = new CommonDomainService();
        service.GetAirlineAreas(airlineId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                //this.AirlineAreas = myResponse.Result;
            }
        });
    }

    FillTariffCharges(myList: CodeNameClass[]) {
        this.TariffChargesObsList = [];

        myList.sort(p => p.Code_Int).forEach(item => {
            this.TariffChargesObsList.push(new TariffCharge(item));
        });
    }
    
    public FromObsList: DestinationClass[] = [];
    public ToObsList: DestinationClass[] = [];

    private startDate: Date;
    get StartDate() {
        return this.startDate;
    }
    set StartDate(value: Date) {
        if (this.startDate != value) {
            this.startDate = value;
        }
    }

    setToggleButtonMenuTemp() {
        var ToggleBTN = document.getElementById(this.SearchAreaButtonId) as HTMLDivElement;
        ToggleBTN.className = "ToggleButtonMenuTemp";
    }
    setToggleButtonMenu() {
        var ToggleBTN = document.getElementById(this.SearchAreaButtonId) as HTMLDivElement;
        ToggleBTN.className = "ToggleButtonMenu";
    }

    AddArea(item: AirlineAreaClass, i: number, type: string) {
        if (item.IsChecked == true) {
            if (type == "From") {
                if (!this.FromObsList.filter(d => d.Indication == "Area" && d.DisplayText == item.Name)) {
                    //this.BuildToggleButtonList();


                }
            }
        }
    }

    AddPort(type: string) {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 320;
        logWindow.Height = 170;

        var itemComponent = new DestinationClass(this, type, null);
        logWindow.DataContext = itemComponent;

        logWindow.Title = "Add " + type + " Port";
        logWindow.Show('./TariffModule/Components/EditTabs/Tariff/ChoosePortComponent');
    }

    DeleteDestination(item: DestinationClass, type: string) {
        if (type == "From") {
            var index = this.FromObsList.indexOf(item);
            if (index > -1) {
                this.FromObsList.splice(index);
            }
        }

        else {
            var index = this.ToObsList.indexOf(item);
            if (index > -1) {
                this.ToObsList.splice(index);
            }
        }
    }

    UpdateButtonClicked() {
        var errors: string[] = [];

        if (this.FromObsList.length == 0) {
            errors.push("You have to choose from ports/areas");
        }

        if (this.ToObsList.length == 0) {
            errors.push("You have to choose to ports/areas");
        }

        if (this.StartDate == null) {
            errors.push("Start date is required");
        }

        if (this.TariffChargesObsList.filter(d => d.IsChargeChecked).length == 0) {
            errors.push("No surcharges updated");
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();

            var args: UpdateSurchargeArgs = new UpdateSurchargeArgs();
            args.TariffId = this.EntityPM.TariffId;
            args.StartDate = this.StartDate;

            this.FromObsList.forEach(item => {
                args.From.push(item.Indication + "," + item.Code);
            });

            this.ToObsList.forEach(item => {
                args.To.push(item.Indication + "," + item.Code);
            });

            this.TariffChargesObsList.filter(d => d.IsChargeChecked).forEach(item => {
                args.Surcharge.push(item.ChargeCode + "," + item.NewPrice);
            });

            var myService: TariffDomainService = new TariffDomainService();
            myService.PostUpdateSurcharge(args).subscribe((response: ServiceResponse) => {
                if (!response.HasError) {
                    
                }

                this.CurrentSession.StopBusyIndicator();
            });
        }
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }    
}

export class TariffCharge {
    public ChargeCode: string;
    public MeasurmentCode: string;

    constructor(charge: CodeNameClass) {
        this.ChargeCode = charge.Code;
        this.MeasurmentCode = charge.Name;
    }

    private isChargeChecked: boolean;
    get IsChargeChecked() {
        return this.isChargeChecked;
    }
    set IsChargeChecked(value: boolean) {
        if (this.isChargeChecked != value) {
            this.isChargeChecked = value;
        }
    }

    private newPrice: number;
    get NewPrice() {
        return this.newPrice;
    }
    set NewPrice(value: number) {
        if (this.newPrice != value) {
            this.newPrice = value;
        }
    }
}

export class DestinationClass extends BaseComponent{
    public Indication: string;
    public DisplayText: string;
    public Code: string;
    public Type: string;
    constructor(public fatherComponent: UpdateSurchargesComponent, type: string, Port: PortList) {
        super();

        this.Type = type;

        if (Port != null) {
            this.Indication = "Port";
            this.DisplayText = Port.EnglishName;
            this.Code = Port.Code;
        }
    }
}

export class AirlineAreaClass {
    private entityList: AirlineAreaList;

    public get Name() { return this.entityList.Name; }

    public get Foreground() { return this.IsChecked ? "#FF6E7172" : "#FF282E30"; }   

    public ProductTypesByTenantList = [];

    constructor(itemList: AirlineAreaList, private Parent: any, private productTypeList: Array<any>) {       
        this.ProductTypesByTenantList = productTypeList;
        this.entityList = itemList;
       
        //var isChecked = null;
        //var productPM = this.entityPM.CustomerProducts.filter(d => d.ProductTypeCode == this.entityList.Code)[0];
        //this.isChecked = false;
        //if (productPM != null) {
        //    isChecked = true;
        //    this.IsChecked = true;
        //}
    }
    
    private isChecked: boolean;
    public get IsChecked() { return this.isChecked; }
    public set IsChecked(value: boolean) {
        if (this.isChecked != value) {
            this.isChecked = value;

            //if (value) {
            //    var newItem: CustomerProductPM = new CustomerProductPM(null);
            //    newItem.Tenant = this.TenantPM.Id;
            //    newItem.CustomerId = this.entityPM.Id;
            //    newItem.ProductTypeCode = this.Code;
            //    newItem.CommitmentChargeableWeight = 0;
            //    newItem.PotentialChargeableWeight = 0;
            //    newItem.CommitmentTEU = 0;
            //    newItem.PotentialTEU = 0;
            //    newItem.CommitmentNumberOfShipments = 0;
            //    newItem.PotentialNumberOfShipments = 0;
            //    newItem.CommitmentRevenue = 0;
            //    newItem.PotentialRevenue = 0;


            //    var type: string = null;

            //    var ProductsToggleButtonList = [];

            //    this.ProductTypesByTenantList.forEach((i) => {
            //        if (!i.InActive) {
            //            var item = new ProductTypeList();
            //            item.Code = i.Code;
            //            item.Name = i.Name;
            //            item.InActive = i.InActive;
            //            item.Id = i.Id;
            //            item.SearchFields = i.SearchFields;

            //            ProductsToggleButtonList.push(i);

            //        }
            //        else {


            //        }
            //    });

            //    ProductsToggleButtonList.sort((a, b) => { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1 });
            //    var typelist: ProductTypeList = ProductsToggleButtonList.filter(d => d.Code == this.Code)[0];
            //    if (typelist != null) {
            //        type = typelist.Name;
            //    }
            //    newItem.ProductTypeName = type;
            //    var flag: boolean = true;
            //    for (var i = 0; i < this.entityPM.CustomerProducts.length; i++) {
            //        if (this.entityPM.CustomerProducts[i].ProductTypeCode == newItem.ProductTypeCode) {
            //            flag = false; break;
            //        }
            //    }
            //    if (flag) {
            //        this.entityPM.AddCustomerProductPM(newItem);
            //    }

            //    if (!this.entityPM.ActivityWatch)
            //        this.entityPM.ActivityWatch = true;
            //}
            //else {
            //    var item: CustomerProductPM = this.entityPM.CustomerProducts.filter(d => d.ProductTypeCode == this.Code)[0];
            //    if (item != null) {
            //        if (this.entityPM.CustomerProducts.includes(item)) {
            //            var CustomerProdArr: Array<CustomerProductPM> = [];
            //            this.entityPM.CustomerProducts.forEach(i => {
            //                if (i.ProductTypeCode != item.ProductTypeCode) {
            //                    CustomerProdArr.push(i);
            //                }
            //            });
            //            this.entityPM.RemoveCustomerProductPM(this.entityPM.CustomerProducts.filter(p => p.ProductTypeCode == this.Code)[0]);
            //        }
            //    }
            //}
        }
    }
}
