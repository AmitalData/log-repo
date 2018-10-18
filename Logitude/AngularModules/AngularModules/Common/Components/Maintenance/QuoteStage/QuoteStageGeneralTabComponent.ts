import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {QuoteStagePM} from '../../../../Quote/EntityPMs/QuoteStagePM';
import {AppTool} from '../../../../Infrastructure/Tools';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {EntityArgs} from  '../../../../Infrastructure/DataContracts/EntityArgs';

@Component({
    moduleId: module.id,
    templateUrl: './QuoteStageGeneralTabComponent.html',
})

export class QuoteStageGeneralTabComponent extends BaseComponent {
    public EntityPM: QuoteStagePM;
    public DataContext: QuoteStageGeneralTabComponent = this;
    public IsNewEntity: boolean = true;
    public ObjectTableName: string = "QuoteStage";

    constructor(public args: EntityArgs) {
        super();
        this.EntityPM = args.EntityPM;
        this.UIProperties.SetEnabled("Code", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("Name", this.ObjectTableName, false);
    }

    get Code() { return this.EntityPM.Code; }
    set Code(value: string) {
        if (this.EntityPM.Code != value) {
            this.EntityPM.Code = value;
        }
    }

    get Name() { return this.EntityPM.Name; }
    set Name(value: string) {
        if (this.EntityPM.Name != value) {
            this.EntityPM.Name = value;
        }
    }

    get MaxDays() { return this.EntityPM.MaxDays; }
    set MaxDays(value: number) {
        if (this.EntityPM.MaxDays != value) {
            this.EntityPM.MaxDays = value;
        }
    }

    get InActive() { return this.EntityPM.InActive; }
    set InActive(value: boolean) {
        if (this.EntityPM.InActive != value) {
            this.EntityPM.InActive = value;
        }
    }

}