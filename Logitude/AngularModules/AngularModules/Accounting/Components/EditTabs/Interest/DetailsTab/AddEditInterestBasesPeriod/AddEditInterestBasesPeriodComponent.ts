import { Component } from '@angular/core';
import { InterestBasesPeriodItem } from '../InterestBasesTypeDetailsTabComponent';
import { InterestBasesPeriodPM } from '../../../../../EntityPMs/InterestBasesPeriodPM';
import { BaseComponent } from '../../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../../../Infrastructure/Utilities/SessionLocator';
import { Validator } from '../../../../../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from '../../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { DateTool } from '../../../../../../Infrastructure/Tools';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditInterestBasesPeriodComponent.html',
})

export class AddEditInterestBasesPeriodComponent extends BaseComponent {
    public ObjectTableName: string = "InterestBasesPeriod";
    public DataContext: InterestBasesPeriodItem;
    public EntityPM: InterestBasesPeriodPM;
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.UIProperties.SetEnabled("InterestRate", "InterestBasesPeriod", false);
    }

    SetDataContext(dataContext: InterestBasesPeriodItem) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.DataContext.EntityPM, this.DataContext.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (this.DataContext.EntityPM.InterestRate && !this.CheckInterestRateValid(this.DataContext.EntityPM.InterestRate))
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("Accounting.General.O.Theratepercentageshouldbeformattedas00.00"));
        if (this.DataContext.EntityPM.InterestBaseStartDate && !this.CheckInterestBaseStartDateExist(this.DataContext.EntityPM.InterestBaseStartDate, this.DataContext.EntityPM.CreateDate))
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("Accounting.General.O.AbaseperiodwiththesamestartdateisalreadyAdded"));
        if (this.ValidationErrorsList.length == 0) {
            if (this.DataContext.IsNewEntity)
                if (this.DataContext.InterestBasesTypePM.InterestBasesPeriods.indexOf(this.EntityPM) == -1) {
                    this.DataContext.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
                    this.DataContext.fatherComponent.InterestBasesPeriodsList.Insert(this.DataContext);
                    this.EntityPM.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
                    this.DataContext.InterestBasesTypePM.AddInterestBasesPeriod(this.EntityPM);
                    this.DataContext.fatherComponent.BuildData();
                }
                this.CurrentSession.CloseCurrentWindow();
        }
    }

    CheckInterestRateValid(InterestRate: number): boolean {
        if ((InterestRate.toFixed()).length > 2) {
            return false;
        }
        return true;
    }
    CheckInterestBaseStartDateExist(InterestBaseStartDate: Date, CreateDate:Date): boolean {
        for (let i = 0; i < this.DataContext.fatherComponent.InterestBasesPeriodsList.Length; i++) {
            if (new Date(this.DataContext.fatherComponent.InterestBasesPeriodsList.Collection[i].InterestBaseStartDate).getTime() === new Date(InterestBaseStartDate).getTime()) {
                if (!this.DataContext.IsNewEntity) {
                    if (new Date(this.DataContext.fatherComponent.InterestBasesPeriodsList.Collection[i].CreateDate).getTime() != new Date(CreateDate).getTime()) {
                        return false;
                    }
                }
                else {
                    return false;
                }
            }
        }
        return true;
    }


}
