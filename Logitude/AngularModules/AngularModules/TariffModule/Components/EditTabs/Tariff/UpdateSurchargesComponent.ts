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
    public FromAirlineAreas: AirlineAreaClass[];
    public ToAirlineAreas: AirlineAreaClass[];
    public FromTariffAreaDropButton: string = "FromTariffAreaDropButton";
    public ToTariffAreaDropButton: string = "ToTariffAreaDropButton";
    public FromSearchAreaId: string = "FromSearchAreaId";
    public ToSearchAreaId: string = "ToSearchAreaId";
    constructor() {
        super();

        this.FromTariffAreaDropButton += this.CurrentSession.GetNewId("FromTariffAreaDropButton_1");
        this.FromSearchAreaId += this.CurrentSession.GetNewId("FromSearchAreaId_1");
        this.ToTariffAreaDropButton += this.CurrentSession.GetNewId("ToTariffAreaDropButton_1");
        this.ToSearchAreaId += this.CurrentSession.GetNewId("ToSearchAreaId_1");
    }
    
    SetWindowArgs(arg: UpdateTariffArgs) {
        this.EntityPM = arg.Version;

        this.FillTariffCharges(arg.TariffCharges);
        this.LoadAirlineAreas(arg.AirlineId);
    }

    private AreasList: AirlineAreaList[] =[];
    private LoadAirlineAreas(airlineId: string) {
        var service: CommonDomainService = new CommonDomainService();
        service.GetAirlineAreas(airlineId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.AreasList = myResponse.Result;
                this.FillAirlineAreas("From");
                this.FillAirlineAreas("To");
            }
        });
    }

    FillAirlineAreas(type: string) {
        if (type == "From") {
            var data: AirlineAreaList[] = [];
            if (this.FromSearchText == null || this.FromSearchText == "") {
                data = this.AreasList;
            }

            else {
                data = this.AreasList.filter(f => f.Name.toLowerCase().indexOf(this.FromSearchText.toLowerCase()) > -1);

            }

            this.FromAirlineAreas = [];

            data.forEach((i) => {
                var itemTogleButton: AirlineAreaClass = new AirlineAreaClass(i, this);
                this.FromAirlineAreas.push(itemTogleButton);
            });
        }

        else if (type == "To") {
            var data: AirlineAreaList[] = [];
            if (this.ToSearchText == null || this.ToSearchText == "") {
                data = this.AreasList;
            }

            else {
                data = this.AreasList.filter(f => f.Name.toLowerCase().indexOf(this.ToSearchText.toLowerCase()) > -1);

            }

            this.ToAirlineAreas = [];

            data.forEach((i) => {
                var itemTogleButton: AirlineAreaClass = new AirlineAreaClass(i, this);
                this.ToAirlineAreas.push(itemTogleButton);
            });
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
        this.FillAirlineAreas("From");
    }

    public toSearchText: string = null;
    public get ToSearchText() { return this.toSearchText; }
    public set ToSearchText(newValue: string) {
        this.toSearchText = newValue;
        this.FillAirlineAreas("To");
    }

    AddArea(item: AirlineAreaClass, i, type: string) {
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

    DeleteDestination(item: DestinationClass, type: string) {
        if (type == "From") {
            var index = this.FromObsList.indexOf(item);
            if (index > -1) {
                this.FromObsList.splice(index);
            }

            if (item.Indication == "Area") {
                this.FillAirlineAreas("From");
            }
        }

        else {
            var index = this.ToObsList.indexOf(item);
            if (index > -1) {
                this.ToObsList.splice(index);
            }

            if (item.Indication == "Area") {
                this.FillAirlineAreas("To");
            }
        }
    }

    private isUpdateDone: boolean = false;
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
            args.VersionNumber = this.EntityPM.Version;
            args.StartDate = this.StartDate;

            this.FromObsList.forEach(item => {
                args.From.push(item.Indication + "," + item.Id);
            });

            this.ToObsList.forEach(item => {
                args.To.push(item.Indication + "," + item.Id);
            });

            this.TariffChargesObsList.filter(d => d.IsChargeChecked).forEach(item => {
                args.Surcharge.push(item.ChargeId + "," + item.NewPrice + "," + item.Index);
            });

            var myService: TariffDomainService = new TariffDomainService();
            myService.PostUpdateSurcharge(args).subscribe((response: ServiceResponse) => {
                if (!response.HasError) {
                    this.isUpdateDone = true;
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
    constructor(charge: CodeNameClass) {
        super();

        this.ChargeId = charge.Code;
        this.ChargeCode = charge.Name;
        this.DisplayText = charge.DisplyText;
        this.Index = charge.Code_Int;

        this.SetUIProperties();
    }

    SetUIProperties() {
        var newPriceEnabled: boolean = false;

        if (this.IsChargeChecked) {
            newPriceEnabled = true;
        }

        this.UIProperties.SetEnabled("NewPrice", null, newPriceEnabled);
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
        }
    }
}

export class DestinationClass extends BaseComponent{
    public Indication: string;
    public DisplayText: string;
    public Code: string;
    public Id: string;
    public Type: string;
    constructor(public fatherComponent: UpdateSurchargesComponent, type: string, Port: PortList, airlineArea: AirlineAreaList) {
        super();

        this.Type = type;

        if (Port != null) {
            this.Indication = "Port";
            this.DisplayText = Port.EnglishName;
            this.Code = Port.Code;
            this.Id = Port.Id;
        }

        if (airlineArea != null) {
            this.Indication = "Area";
            this.DisplayText = airlineArea.Name;
            this.Id = airlineArea.Id;
        }
    }
}

export class AirlineAreaClass {
    public entityList: AirlineAreaList;
    public get Name() { return this.entityList.Name; }

    public get Foreground() { return this.IsChecked ? "#FF6E7172" : "#FF282E30"; }

    public get Id() { return this.entityList.Id; }

    constructor(itemList: AirlineAreaList, private Parent: UpdateSurchargesComponent) {
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
