import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { TariffVersionPM } from '../../../EntityPMs/TariffVersionPM';
import { UpdateTariffArgs } from '../../../Args';
import { CodeNameClass } from '../../../../Infrastructure/DataContracts/CodeNameClass';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { PortList } from '../../../../Common/EntityLists/PortList';

@Component({
    moduleId: module.id,
    templateUrl: './UpdateSurchargesComponent.html',
})

export class UpdateSurchargesComponent extends BaseComponent {
    private CurrentSession = SessionLocator.SelectedSession;
    public DataContext = this;
    public ObjectTableName = "Tariff";
    public EntityPM: TariffVersionPM;
    public ValidationErrorsList: string[] = [];
    public TariffChargesObsList: TariffCharge[];
    constructor() {
        super();
    }

    SetWindowArgs(arg: UpdateTariffArgs) {
        this.EntityPM = arg.Version;

        this.FillTariffCharges(arg.TariffCharges);
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

    AddArea(type: string) {


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


    //IsChargeChecked
    //NewPrice
}

export class DestinationClass extends BaseComponent{
    public Indication: string;
    public DisplayText: string;
    public Type: string;
    constructor(public fatherComponent: UpdateSurchargesComponent, type: string, Port: PortList) {
        super();

        this.Type = type;

        if (Port != null) {
            this.Indication = "Port";
            this.DisplayText = Port.EnglishName;
        }
    }
}
