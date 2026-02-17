import {Component, ChangeDetectorRef}  from '@angular/core';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';
import {BatchTaskExecutionPM} from '../../../../EntityPMs/BatchTaskExecutionPM';
import {ApiQueryFilters} from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {AppTool} from '../../../../../Infrastructure/Tools';
import {ObjectsLocator} from '../../../../../Infrastructure/Locators/ObjectsLocator';
import { EntityResourceService } from '../../../../Services/EntityResourceService';

@Component({
    moduleId: module.id,
    templateUrl: './BTEParameterTabComponent.html'
})

export class BTEParameterTabComponent extends BaseComponent {
    public EntityPM: BatchTaskExecutionPM = null;
    public ObjectTableName = 'BatchTaskExecution';
    public DataContext = this;

    public isRTL = false;
    public isReady = false;

    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityArgs: EntityArgs) {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");

        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName, 0).subscribe(response => {
            this.isReady = true;
        });

        // Set Entity
        this.EntityPM = entityArgs.EntityPM;
        this.SetUIProperties();

        this.Listen();
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }

            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        console.log("Entity Reloaded");
                    }
                });
            }
        }
    }

    //#region Properties
    get PrametersXml() { return this.EntityPM.PrametersXml; }
    //#endregion

    SetUIProperties() {
        //if (!this.EntityPM.TypeCode) {
        this.UIProperties.SetEnabled("PrametersXml", this.ObjectTableName, false);
        //}

    }

}
