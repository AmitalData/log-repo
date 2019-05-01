import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { TimeManagementDomainService } from '../../../Services/TimeManagementDomainService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    moduleId: module.id,
    templateUrl: './VacationsComponent.html',
})

export class VacationsComponent extends BaseComponent {
    public Years: number[] = [];
    public Types: string[] = [];
    public ItemsSource: any[] = [];
    private mySerive: TimeManagementDomainService = null;
    constructor() {
        super();
        this.mySerive = new TimeManagementDomainService();

        this.Years = [];
        this.Types = [];

        for (var i = new Date().getFullYear(); i >= 2018; i--) {
            this.Years.push(i);
        }

        this.Types.push("Holidays");
        this.Types.push("Vacations");
        this.Types.push("Half Vacations");
        this.Types.push("Unpaid Vacations");
        this.Types.push("Sickness Vacations");
        this.Types.push("Sick Leaves");

        this.selectedYear = this.Years[0];
        this.selectedType = this.Types[0];       
    }

    private selectedYear: number = null;
    get SelectedYear() { return this.selectedYear; }
    set SelectedYear(value: number) {
        if (this.selectedYear != value) {
            this.selectedYear = value;
            this.LoadAllScreenData();
        }
    }

    private selectedType: string = null;
    get SelectedType() { return this.selectedType; }
    set SelectedType(value: string) {
        if (this.selectedType != value) {
            this.selectedType = value;
            this.GetVacationsDetails();
        }
    }

    InitTab() {
        this.LoadAllScreenData();
    }

    RefreshButtonClicked() {
        this.LoadAllScreenData();
    }

    LoadAllScreenData() {
        this.GetVacationsSummary();
        this.GetVacationsDetails();
    }

    public Holidays: number = 0;
    public Vacations: number = 0;
    public HalfVacations: number = 0;
    public UnpaidVacations: number = 0;
    public SicknessVacations: number = 0;
    public SickLeaves: string = "0";
    GetVacationsSummary() {

        this.Holidays = 0;
        this.Vacations = 0;
        this.HalfVacations = 0;
        this.UnpaidVacations = 0;
        this.SicknessVacations = 0;
        this.SickLeaves = "0";

        this.mySerive.GetVacationsSummary(this.SelectedYear).subscribe((myResponse: ServiceResponse) => {

            if (myResponse.HasError) {
                //this.ShowMessage(myResponse.ErrorsArray[0]);
            }

            else {
                this.Holidays = myResponse.Result.Holidays;
                this.Vacations = myResponse.Result.Vacations;
                this.HalfVacations = myResponse.Result.HalfVacations;
                this.UnpaidVacations = myResponse.Result.UnpaidVacations;
                this.SicknessVacations = myResponse.Result.SicknessVacations;
                this.SickLeaves = myResponse.Result.SickLeaves;
            }
        });
    }

    GetVacationsDetails() {

        this.ItemsSource = [];

        this.mySerive.GetVacationsDetails(this.SelectedYear, this.SelectedType).subscribe((myResponse: ServiceResponse) => {

            this.ItemsSource = [];

            if (myResponse.HasError) {
                //this.ShowMessage(myResponse.ErrorsArray[0]);
            }

            else {
                this.ItemsSource = myResponse.Result;
            }
        });
    }
}
