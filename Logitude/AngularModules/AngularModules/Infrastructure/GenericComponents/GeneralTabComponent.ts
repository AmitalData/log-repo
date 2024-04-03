import { Component, AfterViewInit, ViewChild } from '@angular/core';
import { EntityArgs } from '../DataContracts/EntityArgs';
import { SessionLocator } from '../Utilities/SessionLocator';
import { ChildDirective } from '../Directives/ChildDirective';
 import { FeatureLocator } from '../Utilities/FeatureLocator';

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
    public DisplayTabNameInScreen: boolean = true;
  constructor(private entityArgs: EntityArgs) {
 
        this.IsNewEntity = entityArgs.IsNewEntity;
        this.ObjectTableName = entityArgs.ObjectTableName;
    }

    OPObjectTablesName = ["ChargesGroup"];

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
                 if (FeatureLocator.HasFeaturePermession("QuoteOP", "QuoteOPMaintence") && this.OPObjectTablesName.includes(this.ObjectTableName)){
                    var ScreenCode = this.ObjectTableName +"OP"+ ".GeneralTabScreen";
                }
 
          this.SetTabTitle();
 
                cmpRef.instance.EntityArgs = this.entityArgs;
                cmpRef.instance.Run(this.entityArgs.EntityPM, this.ObjectTableName, ScreenCode, this.IsNewEntity);
            });
    }

    private SetTabTitle()
    {
        const tab: ObjectTableTabPM = window.ObjectTableTabs.find(d => d.Code == this.entityArgs.SelectedTabCode);
        this.DisplayTabNameInScreen = !tab?.HideTabNameInScreen;
        if (!this.DisplayTabNameInScreen) return;
        if (tab?.TabNameTextCodeDefaultText)
            return this.title = tab.TabNameTextCodeDefaultText;

        this.title = TextCodeTranslator.Translate(generalTabTitleTextCode);
    }


    //SetTabVisibility() {

    //    const tab: ObjectTableTabPM = window.ObjectTableTabs.find(d => d.Code == this.entityArgs.SelectedTabCode);
    //    const objectTableId = window.ObjectTables.filter((x: any) => x.Name === this.entityArgs.ObjectTableName)[0].Id;
    //    if (!tab || !tab.ScreenCode) return;

    //    let screen: any = window.Screens.filter((x: any) => x.ObjectTableId === objectTableId && x.Code.toLowerCase() == tab.ScreenCode.toLowerCase())[0];
    //    if (!screen || screen.Type != "LIGHTENING") return;
    //    this.IsShowTitle = false;
    //}

}

