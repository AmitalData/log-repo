import { Component, OnInit} from '@angular/core';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { AppTool } from '../../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { InterestBasesTypePM } from '../../../../EntityPMs/InterestBasesTypePM';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';
import { InterestBasesPeriodPM } from '../../../../EntityPMs/InterestBasesPeriodPM';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ObjectsLocator } from '../../../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    moduleId: module.id,
    templateUrl: './InterestBasesTypeDetailsTabComponent.html',
})

export class InterestBasesTypeDetailsTabComponent extends BaseComponent implements OnInit {
    public EntityPM: InterestBasesTypePM;
    public ObjectTableName: string = "InterestBasesType";
    public DataContext: InterestBasesTypeDetailsTabComponent = this;
    public InterestBasesPeriodsList: ObservableCollection;
    private CurrentSession = SessionLocator.SelectedSession;
    public isRTL: boolean = false;
    constructor(public entityArgs: EntityArgs) {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.EntityPM = entityArgs.EntityPM;
        this.InterestBasesPeriodsList = new ObservableCollection([]);
        this.BuildData();
        this.SetUIProperties();
        this.Listen();
    }
    ngOnInit() {
    }
    SetUIProperties() {
        if (this.EntityPM.InActive) { 
            this.UIProperties.SetEnabled("Code", "InterestBasesType", false);
            this.UIProperties.SetEnabled("EnglishName", "InterestBasesType", false);
            this.UIProperties.SetEnabled("LocalName", "InterestBasesType", false);
            this.UIProperties.SetEnabled("Description", "InterestBasesType", false);
        }
        else{
            this.UIProperties.SetEnabled("Code", "InterestBasesType", true);
            this.UIProperties.SetEnabled("EnglishName", "InterestBasesType", true);
            this.UIProperties.SetEnabled("LocalName", "InterestBasesType", true);
            this.UIProperties.SetEnabled("Description", "InterestBasesType", true);
        }
    }
    //Grid Header Label
    public InterestBaseStartDateHeader = TextCodeTranslator.Translate("InterestBasesPeriod.CH.InterestBaseStartDateListLable");
    public UpdatedByUserIdHeader = TextCodeTranslator.Translate("InterestBasesPeriod.CH.UpdatedByUserIdListLable");
    public UpdateDateHeader = TextCodeTranslator.Translate("InterestBasesPeriod.CH.UpdateDateListLable");
    public InterestRateHeader = TextCodeTranslator.Translate("InterestBasesPeriod.CH.InterestRateListLable");

    //Add Edit Interest Bases Period Title
    public EditInterestBasesPeriod = TextCodeTranslator.Translate("Accounting.General.O.Edit");
    public AddInterestBasesPeriod = TextCodeTranslator.Translate("Accounting.General.B.Add");

    AddPeriodClicked() {
        var itemPM = new InterestBasesPeriodPM(null);
        itemPM.Tenant = SessionLocator.Tenant;
        itemPM.InterestBaseTypeId = this.EntityPM.Id;
        itemPM.LineNumber = this.EntityPM.InterestBasesPeriods.length + 1;
        var itemComponent = new InterestBasesPeriodItem(itemPM, true, this);
        this.LogWindowShow(this.AddInterestBasesPeriod, itemComponent);
    }

    EditPeriodClicked(itemComponent: InterestBasesPeriodItem) {
        itemComponent.IsNewEntity = false;
        this.LogWindowShow(this.EditInterestBasesPeriod, itemComponent);
    }

    LogWindowShow(title: string, itemComponent) {
        var logWindow = new LogitudeWindow();
        logWindow.Title = title;
        var myPath = "./Accounting/Components/Packages/EditTabs/Interest/DetailsTab/AddEditInterestBasesPeriod/AddEditInterestBasesPeriodComponent";
        logWindow.Width = 400;
        logWindow.Height = 250;
        logWindow.DataContext = itemComponent;
        logWindow.Show(myPath);
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent != null) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.BuildData();
                    this.SetUIProperties();
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.BuildData();
                    this.SetUIProperties();
                }
            });
        }
    }

    public BuildData() {
        this.InterestBasesPeriodsList.Clear();
        var list = [];
        this.EntityPM.InterestBasesPeriods.forEach(item => {
            list.push(new InterestBasesPeriodItem(item, true, this));
        });
        this.InterestBasesPeriodsList.InsertCollection(list);
    }


    get Description() {
        if (this.EntityPM != null) {
            return this.EntityPM.Description;
        }
        else
            return null;
    }
    set Description(newValue: string) {
        if (this.EntityPM.Description != newValue) {
            this.EntityPM.Description = newValue;
        }
    }
    get EnglishName() {
        if (this.EntityPM != null) {
            return this.EntityPM.EnglishName;
        }
        else
            return null;
    }
    set EnglishName(newValue: string) {
        if (this.EntityPM.EnglishName != newValue) {
            this.EntityPM.EnglishName = newValue;
        }
    }
    get LocalName() {
        if (this.EntityPM != null) {
            return this.EntityPM.LocalName;
        }
        else
            return null;
    }
    set LocalName(newValue: string) {
        if (this.EntityPM.LocalName != newValue) {
            this.EntityPM.LocalName = newValue;
        }
    }
    get Code() {
        if (this.EntityPM != null) {
            return this.EntityPM.Code;
        }
        else
            return null;
    }
    set Code(newValue: string) {
        if (this.EntityPM.Code != newValue) {
            this.EntityPM.Code = newValue;
        }
    }
    get InActive() {
        if (this.EntityPM != null) {
            return this.EntityPM.InActive;
        }
        else
            return null;
    }
    set InActive(newValue: boolean) {
        if (this.EntityPM.InActive != newValue) {
            this.EntityPM.InActive = newValue;
        }
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }
}

export class InterestBasesPeriodItem extends BaseComponent {
    public DataContext: InterestBasesPeriodItem = this;
    public EntityPM: InterestBasesPeriodPM;
    public InterestBasesTypePM: InterestBasesTypePM;
    public ObjectTableName: string = "InterestBasesPeriod";
    public IsNewEntity: boolean = false;

    constructor(entityPM: InterestBasesPeriodPM, isNew: boolean, public fatherComponent: InterestBasesTypeDetailsTabComponent) {
        super();
        this.EntityPM = entityPM;
        this.InterestBasesTypePM = fatherComponent.EntityPM;
        this.IsNewEntity = isNew;
    }

    get InterestBaseStartDate() { return this.EntityPM.InterestBaseStartDate; }
    set InterestBaseStartDate(newValue: Date) {
        if (this.EntityPM.InterestBaseStartDate != newValue) {
            this.EntityPM.InterestBaseStartDate = newValue;
        }
    }

    get InterestRate() { return this.EntityPM.InterestRate; }
    set InterestRate(newValue: number) {
        if (this.EntityPM.InterestRate != newValue) {
            this.EntityPM.InterestRate = newValue;
        }
    }

    get LineNumber() { return this.EntityPM.LineNumber; }
    set LineNumber(newValue: number) {
        if (this.EntityPM.LineNumber != newValue) {
            this.EntityPM.LineNumber = newValue;
        }
    }

    get UpdatedByUserId() { return this.EntityPM.UpdatedByUserId; }
    set UpdatedByUserId(newValue: string) {
        if (this.EntityPM.UpdatedByUserId != newValue) {
            this.EntityPM.UpdatedByUserId = newValue;
        }
    }

    get UpdateDate() { return this.EntityPM.UpdateDate; }
    set UpdateDate(newValue: Date) {
        if (this.EntityPM.UpdateDate != newValue) {
            this.EntityPM.UpdateDate = newValue;
        }
    }

}
