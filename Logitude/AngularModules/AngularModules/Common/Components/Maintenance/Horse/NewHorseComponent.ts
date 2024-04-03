import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { CachedDataManager } from '../../../../Infrastructure/Utilities/CachedDataManager';
import { DateTool } from '../../../../Infrastructure/Tools';
import { HorsePM } from '../../../EntityPMs/HorsePM';
import { HorsePMService } from '../../../../Common/Services/StandardPMs/HorsePMService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { SessionInfo } from '../../../../Infrastructure/Utilities/SessionInfo';

@Component({
    templateUrl: './NewHorseComponent.html',
})

export class NewHorseComponent extends BaseComponent {
    public DataContext: NewHorseComponent = this;
    public ObjectTableName: string = "Horse";
    public EntityPM: HorsePM;
    private CurrentSession = SessionLocator.SelectedSession;
    public Years: number[];
    constructor() {
        super();

        this.EntityPM = new HorsePM();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.EntityPM.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
        this.EntityPM.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
        this.EntityPM.CreatedByUserId = SessionInfo.LoggedUserId;
        this.EntityPM.UpdatedByUserId = SessionInfo.LoggedUserId;

        this.FillComboBoxes();
        this.SetUIProperties();
    }

    private FillComboBoxes() {
        this.Years = [];

        for (var i = new Date().getFullYear(); i >= 1950; i--) {
            this.Years.push(i);
        }
    }

    private SetUIProperties() {
        
    }

    get Name() { return this.EntityPM.Name; }
    set Name(newValue: string) {
        if (this.EntityPM.Name != newValue) {
            this.EntityPM.Name = newValue;
        }
    }

    get YearOfBirth() { return this.EntityPM.YearOfBirth; }
    set YearOfBirth(newValue: number) {
        if (this.EntityPM.YearOfBirth != newValue) {
            this.EntityPM.YearOfBirth = newValue;
        }
    }

    get Color() { return this.EntityPM.Color; }
    set Color(newValue: string) {
        if (this.EntityPM.Color != newValue) {
            this.EntityPM.Color = newValue;
        }
    }

    get GenderCode() { return this.EntityPM.GenderCode; }
    set GenderCode(newValue: string) {
        if (this.EntityPM.GenderCode != newValue) {
            this.EntityPM.GenderCode = newValue;
        }
    }

    get Breed() { return this.EntityPM.Breed; }
    set Breed(newValue: string) {
        if (this.EntityPM.Breed != newValue) {
            this.EntityPM.Breed = newValue;
        }
    }

    get Discipline() { return this.EntityPM.Discipline; }
    set Discipline(newValue: string) {
        if (this.EntityPM.Discipline != newValue) {
            this.EntityPM.Discipline = newValue;
        }
    }

    get TravelBehavior() { return this.EntityPM.TravelBehavior; }
    set TravelBehavior(newValue: string) {
        if (this.EntityPM.TravelBehavior != newValue) {
            this.EntityPM.TravelBehavior = newValue;
        }
    }

    get MicochipNumber() { return this.EntityPM.MicochipNumber; }
    set MicochipNumber(newValue: string) {
        if (this.EntityPM.MicochipNumber != newValue) {
            this.EntityPM.MicochipNumber = newValue;
        }
    }

    get PassportNumber() { return this.EntityPM.PassportNumber; }
    set PassportNumber(newValue: string) {
        if (this.EntityPM.PassportNumber != newValue) {
            this.EntityPM.PassportNumber = newValue;
        }
    }

    get CountryOfBirthId() { return this.EntityPM.CountryOfBirthId }
    set CountryOfBirthId(newValue: string) {
        if (this.EntityPM.CountryOfBirthId != newValue) {
            this.EntityPM.CountryOfBirthId = newValue;
        }
    }

    get CurrentStable() { return this.EntityPM.CurrentStable; }
    set CurrentStable(newValue: string) {
        if (this.EntityPM.CurrentStable != newValue) {
            this.EntityPM.CurrentStable = newValue;
        }
    }

    get Owner() { return this.EntityPM.Owner; }
    set Owner(newValue: string) {
        if (this.EntityPM.Owner != newValue) {
            this.EntityPM.Owner = newValue;
        }
    }

    get Remarks() { return this.EntityPM.Remarks; }
    set Remarks(newValue: string) {
        if (this.EntityPM.Remarks != newValue) {
            this.EntityPM.Remarks = newValue;
        }
    }

    get Inactive() { return this.EntityPM.Inactive; }
    set Inactive(newValue: boolean) {
        if (this.EntityPM.Inactive != newValue) {
            this.EntityPM.Inactive = newValue;
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    public ValidationErrorsList: string[];
    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();

            var myService: HorsePMService = new HorsePMService();
            myService.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {

                this.CurrentSession.StopBusyIndicator();

                if (!myResponse.HasError) {
                    CachedDataManager.RefreshTableData(this.ObjectTableName, true);

                    this.CurrentSession.CloseCurrentWindowEmit(this.EntityPM.Id);
                }

                else {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }
            });
        }
    }
}

