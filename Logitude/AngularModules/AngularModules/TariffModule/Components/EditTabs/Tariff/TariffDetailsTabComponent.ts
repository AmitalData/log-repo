import { Component, OnInit, ViewChild, ViewContainerRef, OnDestroy } from '@angular/core';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { TariffPM } from '../../../../TariffModule/EntityPMs/TariffPM';
import { AppTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';

@Component({
    moduleId: module.id,
    templateUrl: './TariffDetailsTabComponent.html',
})

export class TariffDetailsTabComponent implements OnInit, OnDestroy {
    public EntityPM: TariffPM;
    @ViewChild("Child", { read: ViewContainerRef }) location: ViewContainerRef;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, private entityResourceService: EntityResourceService) {
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent != null) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.CurrentSession.FireEvent("LoadEventTabData");
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.RunComponent();
                }
            });
        }
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    private isComponentInited: boolean = false;
    ngOnInit() {
        this.entityResourceService.getEntityResourceByTableName("TariffLine").subscribe((res1: any) => {
            this.isComponentInited = true;
            this.RunComponent();
        });
    }

    RunComponent() {
        if (this.isComponentInited) {
            this.ClearLocation();

            SessionLocator.DynamicLoader.Load("./TariffModule/Components/EditTabs/Tariff/TariffTabsContentComponent", this.location)
                .then(cmpRef => {
                    cmpRef.instance.Run({ EntityPM: this.EntityPM, });
                });
        }
    }

    private ClearLocation() {
        if (this.location) {
            this.location.clear();
        }
    }
}
