import {Component} from '@angular/core';
import {AppTool, ArrayTool} from '../../../Infrastructure/Tools';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator'; 
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import {PhysicalCheckPMService} from '../../Services/StandardPMs/PhysicalCheckPMService'

@Component({
    moduleId: module.id,
    templateUrl: './CustomsSpotlightComponent.html',
})

export class CustomsSpotlightComponent {
    public EntityId: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

    }

    private showBusyIndicator: boolean = false;
    get ShowBusyIndicator() { return this.showBusyIndicator; }
    set ShowBusyIndicator(value: boolean) {
        if (this.showBusyIndicator != value) {
            this.showBusyIndicator = value;
            this.CurrentSession.FireEvent("SpotLightDetectChanges");
        }
    }
    MyEntityArg: EntityArgs;
    Run(entityId: string) {
        this.EntityId = entityId;
        this.LoadPhysicalCheckPM();
    }

    private LoadPhysicalCheckPM() {
       
        this.ShowBusyIndicator = true;

        var physicalCheckPMService = new PhysicalCheckPMService();
        physicalCheckPMService.get(this.EntityId).subscribe(response => {
            var result = response.Result;
            if (!AppTool.IsNullOrEmpty(result)) {
                this.MyEntityArg = new EntityArgs();
                this.MyEntityArg.EntityPM = result;
                this.MyEntityArg.ObjectTableName = "Customs.PhysicalCheck";
            }
            this.ShowBusyIndicator = false;
        });
    }
} 
