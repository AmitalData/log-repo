import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { TariffVersionPM } from '../../../EntityPMs/TariffVersionPM';
import { TariffSurchargesUpdatePM } from '../../../EntityPMs/TariffSurchargesUpdatePM';
import { UpdateTariffArgs } from '../../../Args';
import { CodeNameClass } from '../../../../Infrastructure/DataContracts/CodeNameClass';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { PortList } from '../../../../Common/EntityLists/PortList';
import { CarrierAreaList } from '../../../../Common/EntityLists/CarrierAreaList';
import { CommonDomainService } from '../../../../Common/Services/CommonDomainService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { TariffDomainService, UpdateSurchargeArgs } from '../../../Services/TariffDomainService';
import { AppTool } from '../../../../Infrastructure/Tools';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { CountryList } from '../../../../Common/EntityLists/CountryList';

@Component({
    
    templateUrl: './UpdateSurchargesComponent.html',
})

export class UpdateSurchargesComponent extends BaseComponent {
    private CurrentSession = SessionLocator.SelectedSession;
    public DataContext = this;
    public ObjectTableName = "Tariff";
    public EntityPM: TariffVersionPM;
    public Logs: ObservableCollection;
    public ValidationErrorsList: string[] = [];
    public TariffChargesObsList: TariffCharge[];
    public FromCarrierAreas: CarrierAreaClass[];
    public ToCarrierAreas: CarrierAreaClass[];
    public FromTariffAreaDropButton: string = "FromTariffAreaDropButton";
    public ToTariffAreaDropButton: string = "ToTariffAreaDropButton";
    public FromSearchAreaId: string = "FromSearchAreaId";
    public ToSearchAreaId: string = "ToSearchAreaId";
    public TypeCode: string;
    public FatherComponent: any;
    public ContainerPricesItemsSource: ContainerPriceClass[];
    constructor() {
        super();
        this.FromTariffAreaDropButton += this.CurrentSession.GetNewId("FromTariffAreaDropButton_1");
        this.FromSearchAreaId += this.CurrentSession.GetNewId("FromSearchAreaId_1");
        this.ToTariffAreaDropButton += this.CurrentSession.GetNewId("ToTariffAreaDropButton_1");
        this.ToSearchAreaId += this.CurrentSession.GetNewId("ToSearchAreaId_1");
        this.SetUIProperties();
    }
    
    SetWindowArgs(arg: UpdateTariffArgs) {
        this.EntityPM = arg.Version;
        this.TypeCode = arg.TypeCode;
        this.FatherComponent = arg.FatherComponent;

        this.FillLogs();
        this.FillTariffCharges(arg.TariffCharges);
        this.FillTariffContainerPrices(arg.TariffCharges);
        this.LoadCarrierAreas(arg.CarrierId);
    }

    SetUIProperties() {
        var isStartDateRequired: boolean = false;
        if (this.StartDate == null || this.StartDate == undefined) {
            isStartDateRequired = true;
        }
        this.UIProperties.SetRequired("StartDate", null, isStartDateRequired);
    }

    private AreasList: CarrierAreaList[] =[];
    private LoadCarrierAreas(carrierId: string) {
        var service: CommonDomainService = new CommonDomainService();
        service.GetCarrierAreas(carrierId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.AreasList = myResponse.Result;
                this.FillCarrierAreas("From");
                this.FillCarrierAreas("To");
            }
        });
    }

    private FillLogs() {
        this.Logs  = new ObservableCollection([]);
        var service: TariffDomainService = new TariffDomainService();
        service.GetTariffsLogsByTariffId(this.EntityPM.TariffId, this.EntityPM.Version).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var list: TariffSurchargesUpdateItem[] = [];
                var index = 1;
                if (myResponse.Result != null) {
                    myResponse.Result.forEach(item => {
                        list.push(new TariffSurchargesUpdateItem(item, index));
                        index = index + 1;
                    });
                    this.Logs.InsertCollection(list);
                }             
            }
        });
    }
    FillCarrierAreas(type: string) {
        if (type == "From") {
            var data: CarrierAreaList[] = [];
            if (this.FromSearchText == null || this.FromSearchText == "") {
                data = this.AreasList;
            }

            else {
                data = this.AreasList.filter(f => f.Name.toLowerCase().indexOf(this.FromSearchText.toLowerCase()) > -1);

            }

            this.FromCarrierAreas = [];

            data.forEach((i) => {
                var itemTogleButton: CarrierAreaClass = new CarrierAreaClass(i, this);
                this.FromCarrierAreas.push(itemTogleButton);
            });
        }

        else if (type == "To") {
            var data: CarrierAreaList[] = [];
            if (this.ToSearchText == null || this.ToSearchText == "") {
                data = this.AreasList;
            }

            else {
                data = this.AreasList.filter(f => f.Name.toLowerCase().indexOf(this.ToSearchText.toLowerCase()) > -1);

            }

            this.ToCarrierAreas = [];

            data.forEach((i) => {
                var itemTogleButton: CarrierAreaClass = new CarrierAreaClass(i, this);
                this.ToCarrierAreas.push(itemTogleButton);
            });
        }
    }

    FillTariffContainerPrices(myList: CodeNameClass[]) {
        this.ContainerPricesItemsSource = [];

        if (this.TypeCode == "OFS") {
            for (var i = 1; i <= 10; i++) {
                var charge: CodeNameClass = myList.filter(d => d.Code == this.FatherComponent.EntityPM['Surcharge' + i + 'Id'])[0];

                if (charge != null) {
                    this.ContainerPricesItemsSource.push(new ContainerPriceClass(charge, this.FatherComponent));
                }
            }
        }
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
            this.SetUIProperties();
        }
    }

    setToggleButtonMenuTemp(type: string) {
        if (type == "From") {
            var ToggleBTN = document.getElementById(this.FromTariffAreaDropButton) as HTMLDivElement;
            ToggleBTN.className = "ToggleButtonMenuTemp";
        }

        else if (type == "To") {
            var ToggleBTN = document.getElementById(this.ToTariffAreaDropButton) as HTMLDivElement;
            ToggleBTN.className = "ToggleButtonMenuTemp";
        }
    }
    setToggleButtonMenu(type: string) {
        if (type == "From") {
            var ToggleBTN = document.getElementById(this.FromTariffAreaDropButton) as HTMLDivElement;
            ToggleBTN.className = "ToggleButtonMenu";
        }

        else if (type == "To") {
            var ToggleBTN = document.getElementById(this.ToTariffAreaDropButton) as HTMLDivElement;
            ToggleBTN.className = "ToggleButtonMenu";
        }
    }
    ClearPlaceHolder(type: string) {
        if (type == "From") {
            var temp = document.getElementById(this.FromSearchAreaId) as HTMLInputElement;
            temp.placeholder = "";
            temp.style.background = "rgba(0, 0, 0, 0)";
            var ToggleBTN = document.getElementById(this.FromTariffAreaDropButton) as HTMLDivElement;
            ToggleBTN.className = "ToggleButtonMenuTemp";
        }

        else if (type == "To") {
            var temp = document.getElementById(this.ToSearchAreaId) as HTMLInputElement;
            temp.placeholder = "";
            temp.style.background = "rgba(0, 0, 0, 0)";
            var ToggleBTN = document.getElementById(this.ToTariffAreaDropButton) as HTMLDivElement;
            ToggleBTN.className = "ToggleButtonMenuTemp";
        }
    }
    FillPlaceHolder(type: string) {
        if (type == "From") {
            if (!this.FromSearchText) {
                var temp = document.getElementById(this.FromSearchAreaId) as HTMLInputElement;
                temp.placeholder = "Search";
                temp.style.background = "url(Images/Search.png) no-repeat scroll";
                temp.style.backgroundPosition = "right center";
                temp.style.paddingRight = "30px";
            }
            var ToggleBTN = document.getElementById(this.FromTariffAreaDropButton) as HTMLDivElement;
            ToggleBTN.className = "ToggleButtonMenu";
        }

        else if (type == "To") {
            if (!this.ToSearchText) {
                var temp = document.getElementById(this.ToSearchAreaId) as HTMLInputElement;
                temp.placeholder = "Search";
                temp.style.background = "url(Images/Search.png) no-repeat scroll";
                temp.style.backgroundPosition = "right center";
                temp.style.paddingRight = "30px";
            }
            var ToggleBTN = document.getElementById(this.ToTariffAreaDropButton) as HTMLDivElement;
            ToggleBTN.className = "ToggleButtonMenu";
        }
    }
    OnDeleteValue(type: string) {
        if (type == "From") {
            var temp = document.getElementById(this.FromSearchAreaId) as HTMLInputElement;
            temp.value = null;
            this.FromSearchText = null;
            temp.focus();
        }

        else if (type == "To") {
            var temp = document.getElementById(this.ToSearchAreaId) as HTMLInputElement;
            temp.value = null;
            this.ToSearchText = null;
            temp.focus();
        }
    }
    
    public fromSearchText: string = null;
    public get FromSearchText() { return this.fromSearchText; }
    public set FromSearchText(newValue: string) {
        this.fromSearchText = newValue;
        this.FillCarrierAreas("From");
    }

    public toSearchText: string = null;
    public get ToSearchText() { return this.toSearchText; }
    public set ToSearchText(newValue: string) {
        this.toSearchText = newValue;
        this.FillCarrierAreas("To");
    }

    AddArea(item: CarrierAreaClass, i, type: string) {
        if (item.IsChecked) {
            if (type == "From") {
                if (this.FromObsList.filter(d => d.Id == item.Id && d.Indication == "Area").length == 0) {
                    var newItem: DestinationClass = new DestinationClass(this, "From", null, item.entityList)
                    this.FromObsList.push(newItem);
                }
            }

            else if (type == "To") {
                if (this.ToObsList.filter(d => d.Id == item.Id && d.Indication == "Area").length == 0) {
                    var newItem: DestinationClass = new DestinationClass(this, "To", null, item.entityList)
                    this.ToObsList.push(newItem);
                }
            }
        }

        else {
            if (type == "From") {
                var deleteItem: DestinationClass = this.FromObsList.filter(d => d.Id == item.Id && d.Indication == "Area")[0];
                var index = this.FromObsList.indexOf(deleteItem);
                if (index > -1) {
                    this.FromObsList.splice(index);
                }
            }

            else if (type == "To") {
                var deleteItem: DestinationClass = this.ToObsList.filter(d => d.Id == item.Id && d.Indication == "Area")[0];
                var index = this.ToObsList.indexOf(deleteItem);
                if (index > -1) {
                    this.ToObsList.splice(index);
                }
            }
        }
    }

    AddPort(type: string) {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 320;
        logWindow.Height = 170;

        var itemComponent = new DestinationClass(this, type, null, null);
        logWindow.DataContext = itemComponent;

        logWindow.Title = "Add " + type + " Port";
        logWindow.Show('./TariffModule/Components/EditTabs/Tariff/ChoosePortComponent');
    }

    AddCountry(type: string) {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 320;
        logWindow.Height = 170;

        var itemComponent = new DestinationClass(this, type, null, null);
        logWindow.DataContext = itemComponent;

        logWindow.Title = "Add " + type + " Country";
        logWindow.Show('./TariffModule/Components/EditTabs/Tariff/ChooseCountryComponent');
    }

    DeleteDestination(item: DestinationClass, type: string) {
        if (type == "From") {
            var index = this.FromObsList.indexOf(item);
            if (index > -1) {
                this.FromObsList.splice(index, 1);
            }

            if (item.Indication == "Area") {
                this.FillCarrierAreas("From");
            }
        }

        else {
            var index = this.ToObsList.indexOf(item);
            if (index > -1) {
                this.ToObsList.splice(index, 1);
            }

            if (item.Indication == "Area") {
                this.FillCarrierAreas("To");
            }
        }
    }

    private isUpdateDone: boolean = false;
    UpdateButtonClicked() {
        var errors: string[] = [];

        if (this.FromObsList.length == 0) {
            errors.push("You have to choose from ports/areas/countries");
        }

        if (this.ToObsList.length == 0) {
            errors.push("You have to choose to ports/areas/countries");
        }

        if (this.StartDate == null || this.StartDate == undefined) {
            errors.push("Start date is required");
        }

        if (this.TypeCode == "OFS") {
            if (this.ContainerPricesItemsSource.filter(d => d.IsChargeChecked).length == 0) {
                errors.push("No surcharges updated");
            }

            else {
                if (this.ContainerPricesItemsSource.filter(d => d.IsChargeChecked &&
                    AppTool.IsNullOrZero(d.Price1) && AppTool.IsNullOrZero(d.Price2) && AppTool.IsNullOrZero(d.Price3) && AppTool.IsNullOrZero(d.Price4) && AppTool.IsNullOrZero(d.Price5)
                    && AppTool.IsNullOrZero(d.CostPrice)
                ).length > 0) {
                    errors.push("No surcharges updated");
                }
            }
        }

        else {
            if (this.TariffChargesObsList.filter(d => d.IsChargeChecked).length == 0) {
                errors.push("No surcharges updated");
            }

            else {
                if (this.TariffChargesObsList.filter(d => d.IsChargeChecked && AppTool.IsNullOrZero(d.NewPrice)).length > 0) {
                    errors.push("No surcharges updated");
                }
            }
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();

            var args: UpdateSurchargeArgs = new UpdateSurchargeArgs();
            args.TariffId = this.EntityPM.TariffId;
            args.VersionNumber = this.EntityPM.Version;
            args.StartDate = this.StartDate;

            this.FromObsList.forEach(item => {
                args.From.push(item.Indication + "," + item.Id + "," + item.Code + "," + item.CombinedCode);
            });

            this.ToObsList.forEach(item => {
                args.To.push(item.Indication + "," + item.Id + "," + item.Code + "," + item.CombinedCode);
            });

            if (this.TypeCode == "OFS") {
                this.ContainerPricesItemsSource.filter(d => d.IsChargeChecked).forEach(item => {
                    args.Surcharge.push(item.ChargeId + "," + item.ChargeCode + "," + item.Price1 + "," + item.Price2 + "," + item.Price3 + "," + item.Price4 + "," + item.Price5 + "," + item.CostPrice);
                });
            }

            else {
                this.TariffChargesObsList.filter(d => d.IsChargeChecked).forEach(item => {
                    args.Surcharge.push(item.ChargeId + "," + item.NewPrice + "," + item.NewMinPrice + "," + item.Index + "," + item.ChargeCode);
                });
            }

            var myService: TariffDomainService = new TariffDomainService();
            myService.PostUpdateSurcharge(args).subscribe((response: ServiceResponse) => {
                if (!response.HasError) {
                    this.isUpdateDone = true;
                    this.FillLogs();
                }

                else {
                    this.ValidationErrorsList = response.ErrorsArray;
                }

                this.CurrentSession.StopBusyIndicator();
            });
        }
    }

    CloseButtonClicked() {
        if (this.isUpdateDone) {
            this.CurrentSession.CloseCurrentWindowEmit("ok");
            this.isUpdateDone = true;
        }

        else {
            this.CurrentSession.CloseCurrentWindow();
        }        
    }    
}

export class TariffCharge extends BaseComponent{
    public ChargeCode: string;
    public ChargeId: string;
    public DisplayText: string;
    public DataContext = this;
    public Index: number;
    public MeasurementCode: string;
    constructor(charge: CodeNameClass) {
        super();

        this.ChargeId = charge.Code;
        this.ChargeCode = charge.Name;
        this.DisplayText = charge.DisplyText;
        this.Index = charge.Code_Int;
        this.MeasurementCode = charge.AdditionalField;

        this.SetUIProperties();
    }

    SetUIProperties() {
        var isPriceRequired: boolean = false;
        if (this.IsChargeChecked) {
            if (AppTool.IsNullOrZero(this.NewPrice)) {
                isPriceRequired = true;
            }
        }

        this.UIProperties.SetEnabled("NewPrice", null, this.IsChargeChecked);       
        this.UIProperties.SetRequired("NewPrice", null, isPriceRequired);        

        this.SetUIProperties_MinPrice();
    }

    private SetUIProperties_MinPrice() {
        var isMinPriceEnabled: boolean = false;

        if (this.IsChargeChecked) {
            if (this.MeasurementCode != "FIXD") {
                isMinPriceEnabled = true
            }
        }

        this.UIProperties.SetEnabled("NewMinPrice", null, isMinPriceEnabled);
    }

    private isChargeChecked: boolean;
    get IsChargeChecked() {
        return this.isChargeChecked;
    }
    set IsChargeChecked(value: boolean) {
        if (this.isChargeChecked != value) {
            this.isChargeChecked = value;

            this.SetUIProperties();
        }
    }

    private newPrice: number;
    get NewPrice() {
        return this.newPrice;
    }
    set NewPrice(value: number) {
        if (this.newPrice != value) {
            this.newPrice = value;

            if (this.MeasurementCode != "FIXD") {
                this.MinPricePlaceHolder = "";
                this.NewMinPrice = null;
            }

            this.SetUIProperties();
        }
    }

    private newMinPrice: number;
    get NewMinPrice() {
        return this.newMinPrice;
    }
    set NewMinPrice(value: number) {
        if (this.newMinPrice != value) {
            this.newMinPrice = value;
        }
    }

    private minPricePlaceHolder: string = "No Update";
    get MinPricePlaceHolder() {
        return this.minPricePlaceHolder;
    }
    set MinPricePlaceHolder(value: string) {
        if (this.minPricePlaceHolder != value) {
            this.minPricePlaceHolder = value;            
        }
    }
}

export class DestinationClass extends BaseComponent{
    public Indication: string;
    public DisplayText: string;
    public Code: string;
    public CombinedCode: string;
    public Id: string;
    public Type: string;
    constructor(public fatherComponent: UpdateSurchargesComponent, type: string, Port: PortList, carrierArea: CarrierAreaList) {
        super();

        this.Type = type;

        if (Port != null) {
            this.Indication = "Port";
            this.DisplayText = Port.EnglishName;
            this.Code = Port.Code;
            this.CombinedCode = Port.CombinedCode;
            this.Id = Port.Id;
        }

        //if (Country != null) {
        //    this.Indication = "Country";
        //    this.DisplayText = Country.EnglishName;
        //    this.Code = Country.Code;
        //    this.Id = Country.Id;
        //}

        if (carrierArea != null) {
            this.Indication = "Area";
            this.DisplayText = carrierArea.Name;
            this.Id = carrierArea.Id;
        }
    }
}

export class CarrierAreaClass {
    public entityList: CarrierAreaList;
    public get Name() { return this.entityList.Name; }

    public get Foreground() { return this.IsChecked ? "#FF6E7172" : "#FF282E30"; }

    public get Id() { return this.entityList.Id; }

    constructor(itemList: CarrierAreaList, private Parent: UpdateSurchargesComponent) {
        this.entityList = itemList;
        this.isChecked = Parent.FromObsList.filter(d => d.Id == this.entityList.Id && d.Indication == "Area")[0] != null;
    }

    private isChecked: boolean;
    public get IsChecked() { return this.isChecked; }
    public set IsChecked(value: boolean) {
        if (this.isChecked != value) {
            this.isChecked = value;            
        }
    }
}

export class TariffSurchargesUpdateItem {
    public EntityPM: TariffSurchargesUpdatePM;
    public Index;
    constructor(entity: TariffSurchargesUpdatePM, index: number) {
        this.EntityPM = entity;
        this.Index = index;
    }
    public get To() { return this.EntityPM.To; }
    public get From() { return this.EntityPM.From; }
    public get CreateDate() { return this.EntityPM.CreateDate; }
    public get StartDate() { return this.EntityPM.StartDate; }
    public get LinesUpdated() { return this.EntityPM.LinesUpdated; }
    public get Surcharges() { return this.EntityPM.Surcharges; }
    public get UpdateMethodCode() { return this.EntityPM.UpdateMethodCode; }
    public get UpdateMethodName() { return this.EntityPM.UpdateMethodName; }
}

export class ContainerPriceClass extends BaseComponent {
    public ChargeLabel: string;
    public ChargeId: string;
    public ChargeCode: string;
    public DataContext: ContainerPriceClass = this;
    private measurementCode: string;
    private isEnabled: boolean;
    constructor(charge: CodeNameClass, public mainComponent: any) {
        super();
        this.ChargeLabel = charge.DisplyText;
        this.ChargeId = charge.Code;
        this.ChargeCode = charge.Name;
        this.measurementCode = charge.AdditionalField;
        this.SetUIProperties();
    }

    SetUIProperties() {
        if (this.measurementCode == "FIXD" || this.measurementCode == "BTEU") {
            this.isEnabled = false;
            this.SetUIProperties_TariffLinesContainersPrice();
            this.UIProperties.SetEnabled("CostPrice", null, this.IsChargeChecked && true);
        }
        else if (this.measurementCode == "BCNT") {
            this.isEnabled = true;
            this.SetUIProperties_TariffLinesContainersPrice();
            this.UIProperties.SetEnabled("CostPrice", null, false);
        }
    }

    SetUIProperties_TariffLinesContainersPrice() {
        this.SetUIProperties_Price(1);
        this.SetUIProperties_Price(2);
        this.SetUIProperties_Price(3);
        this.SetUIProperties_Price(4);
        this.SetUIProperties_Price(5);
    }
    private SetUIProperties_Price(index: number) {        
        this.UIProperties.SetEnabled(("Price" + index), null, this.IsChargeChecked && this.isEnabled);
    }
    
    private isChargeChecked: boolean;
    get IsChargeChecked() {
        return this.isChargeChecked;
    }
    set IsChargeChecked(value: boolean) {
        if (this.isChargeChecked != value) {
            this.isChargeChecked = value;

            this.SetUIProperties();
        }
    }

    private costPrice: number;
    get CostPrice() {
        return this.costPrice;
    }
    set CostPrice(value: number) {
        if (this.costPrice != value) {
            this.costPrice = value;
            this.UIProperties.SetEnabled("CostPrice", null, this.IsChargeChecked);
        }
    }

    private price1: number;
    get Price1() {
        return this.price1;
    }
    set Price1(value: number) {
        if (this.price1 != value) {
            this.price1 = value;

            this.SetUIProperties_Price(1);
        }
    }

    private price2: number;
    get Price2() {
        return this.price2;
    }
    set Price2(value: number) {
        if (this.price2 != value) {
            this.price2 = value;

            this.SetUIProperties_Price(2);
        }
    }

    private price3: number;
    get Price3() {
        return this.price3;
    }
    set Price3(value: number) {
        if (this.price3 != value) {
            this.price3 = value;

            this.SetUIProperties_Price(3);
        }
    }

    private price4: number;
    get Price4() {
        return this.price4;
    }
    set Price4(value: number) {
        if (this.price4 != value) {
            this.price4 = value;

            this.SetUIProperties_Price(4);
        }
    }

    private price5: number;
    get Price5() {
        return this.price5;
    }
    set Price5(value: number) {
        if (this.price5 != value) {
            this.price5 = value;

            this.SetUIProperties_Price(5);
        }
    }
}
