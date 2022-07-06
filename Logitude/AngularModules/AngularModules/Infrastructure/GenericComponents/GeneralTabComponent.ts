import { Component, AfterViewInit, ViewChild } from '@angular/core';
import {EntityArgs} from '../DataContracts/EntityArgs';
import {SessionLocator} from '../Utilities/SessionLocator';
import { ChildDirective } from '../Directives/ChildDirective';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { ObjectTableTabPM } from 'Infrastructure/EntityPMs/ObjectTableTabPM';
declare var window:any;

const generalTabTitleTextCode = 'General.O.General';
@Component({
  templateUrl: './GeneralTabComponent.html',
})

export class GeneralTabComponent implements AfterViewInit {
  IsNewEntity: boolean = false;
  private ObjectTableName: string = null;
  @ViewChild(ChildDirective) Child: ChildDirective;
    title: string;

  constructor(private entityArgs: EntityArgs) {
    this.IsNewEntity = entityArgs.IsNewEntity;
    this.ObjectTableName = entityArgs.ObjectTableName;
  }

  ngAfterViewInit() {
    SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.Child.Location)
      .then(cmpRef => {

        var ScreenCode = this.ObjectTableName + ".GeneralTabScreen";

        if (this.ObjectTableName == "Shipment" || this.ObjectTableName == "Master") {
          if (this.entityArgs.EntityPM.ShipmentLevelCode == "C") {
            ScreenCode = "Master.GeneralTabScreen";
          }
        }
        if (this.ObjectTableName == "BatchTaskExecution") {
          ScreenCode = "BatchTaskExecutionGeneralTabScreen";
        }

        this.SetTabTitle();

        cmpRef.instance.EntityArgs = this.entityArgs;
        cmpRef.instance.Run(this.entityArgs.EntityPM, this.ObjectTableName, ScreenCode, this.IsNewEntity);
      });
  }

    private SetTabTitle()
    {
        const tab: ObjectTableTabPM = window.ObjectTableTabs.find(d => d.Code == this.entityArgs.SelectedTabCode);
        if (tab?.TabNameTextCodeDefaultText)
            return this.title = tab.TabNameTextCodeDefaultText;

        this.title = TextCodeTranslator.Translate(generalTabTitleTextCode);
    }
}
