import {Component} from '@angular/core';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {ReconciliationPM} from '../../EntityPMs/ReconciliationPM';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../Infrastructure/Tools';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';

@Component({
    moduleId: module.id,
    templateUrl: "./ReconciliationShortTitleComponent.html",
}) 
export class ReconciliationShortTitleComponent {
    public EntityPM: ReconciliationPM;
    public isRTL: boolean = false;
    public IsVisibile: boolean;
    private _entityResourceService: EntityResourceService = new EntityResourceService();

    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = this.entityArgs.EntityPM;
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");

        this._entityResourceService.getEntityResourceByTableName("Reconciliation", 0).subscribe((response: any) => {
            this.IsVisibile = true;
        });
    } }