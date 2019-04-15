import { Component, ViewChild, ViewContainerRef,  } from '@angular/core';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { TariffPM } from '../../../../TariffModule/EntityPMs/TariffPM';

@Component({
    moduleId: module.id,
    templateUrl: './TariffGeneralTabComponent.html',
})

export class TariffGeneralTabComponent extends BaseComponent {
    public EntityPM: TariffPM = new TariffPM();
    public ObjectTableName: string = "Tariff";

    public DataContext = this;

    @ViewChild('Child', { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        
    }

}
