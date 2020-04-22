import { Component, AfterViewInit, ViewChild, OnDestroy } from '@angular/core';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { TariffPM } from '../../../../TariffModule/EntityPMs/TariffPM';
import { AppTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ChildDirective } from '../../../../Infrastructure/Directives/ChildDirective';

@Component({
  templateUrl: './TariffDetailsTabComponent.html',
})

export class TariffDetailsTabComponent implements AfterViewInit, OnDestroy {
  public EntityPM: TariffPM;
  private CurrentSession = SessionLocator.SelectedSession;
  @ViewChild(ChildDirective) Child: ChildDirective;
  constructor(public entityArgs: EntityArgs, private entityResourceService: EntityResourceService) {
    this.EntityPM = entityArgs.EntityPM;
    this.Listen();
  }

  ngAfterViewInit() {
    this.entityResourceService.getEntityResourceByTableName("TariffLine").subscribe((res1: any) => {
      this.LoadChildComponent();
    });
  }
  LoadChildComponent() {
    this.ClearLocation();

    SessionLocator.DynamicLoader.Load("./TariffModule/Components/EditTabs/Tariff/TariffTabsContentComponent", this.Child.Location)
      .then(cmpRef => {
        cmpRef.instance.Run({ EntityPM: this.EntityPM, });
      });
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
          this.LoadChildComponent();
        }
      });
    }
  }

  ngOnDestroy() {
    AppTool.KillEventEmitter(this.SaveCompletedEvent);
    AppTool.KillEventEmitter(this.LoadCompletedEvent);
  }

  private ClearLocation() {
    if (this.Child.Location) {
      this.Child.Location.clear();
    }
  }
}
