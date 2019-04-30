import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { TimeManagementDomainService } from '../../../Services/TimeManagementDomainService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    moduleId: module.id,
    templateUrl: './VacationsComponent.html',
})

export class VacationsComponent extends BaseComponent {
    public Years: number[];
    private mySerive: TimeManagementDomainService = null;
    constructor() {
        super();
        this.mySerive = new TimeManagementDomainService();

        this.Years = [];
        for (var i = new Date().getFullYear(); i >= 2019; i--) {
            this.Years.push(i);
        }

        this.Year = this.Years[0];
    }

    public Holidays: number = 0;
    public Vacations: number = 0;
    public HalfVacations: number = 0;
    public UnpaidVacations: number = 0;
    public SicknessVacations: number = 0;
    public SickLeaves: number = 0;

    InitTab() {
        this.mySerive.GetVacations().subscribe((myResponse: ServiceResponse) => {
            
            //this.ItemSource.Clear();

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

    private year: number = null;
    get Year() { return this.year; }
    set Year(value: number) {
        if (this.year != value) {
            this.year = value;                        
        }
    }
}
