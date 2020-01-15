import { Component} from '@angular/core';
import { InterestReportPM } from '../../../../EntityPMs/InterestReportPM';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { ObjectsLocator } from '../../../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    moduleId: module.id,
    templateUrl: './InterestReportGeneralTabComponent.html',
})

export class InterestReportGeneralTabComponent extends BaseComponent {
    public EntityPM: InterestReportPM;
    public ObjectTableName: string = "InterestReport";
    public DataContext: InterestReportGeneralTabComponent = this;
    private CurrentSession = SessionLocator.SelectedSession;
    public IsNew: boolean = false;
    public isRTL: boolean = false;
    public ValidationErrorsList: string[] = [];
     constructor(public entityArgs: EntityArgs) {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.EntityPM = entityArgs.EntityPM;
    }
}

