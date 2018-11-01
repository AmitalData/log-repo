import {Component,ChangeDetectorRef} from '@angular/core';
import {WebFreightDomainService} from '../../../Infrastructure/Services/WebFreightDomainService';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {OnInit, Output, EventEmitter, ComponentRef, QueryList} from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
//import {JournalExtendedListService} from '../../Services/ExtendedLists/JournalExtendedListService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {AppTool} from '../../../Infrastructure/Tools';
import {ReconcileEventManager} from '../../Utilities/ReconcileEventManager';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';

@Component({
    moduleId: module.id,
    templateUrl: './TaxReportListTemplate.html',
})

export class TaxReportListTemplate {

    public rowData: any;
    public fieldName: any;
    public AdditionalData: any;


    public isRTL: boolean = false;


    constructor(private CD: ChangeDetectorRef) {
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
    }

    setVariables(rowData: any, fieldName: string, MyAdditionalData: any) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        this.AdditionalData = MyAdditionalData;

        var isDestroyed: boolean = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    }

    Abs(number: number) {
        return number < 0 ? number * -1 : number;
    }

    OpenJournal(id) {
        if (!AppTool.IsNullOrEmpty(id)) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', SessionLocator.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'Journal' });
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                    });
                });
        }
    }

    EditLine() {
        var entity = this.rowData;
        if (entity) {
            var windowTitle = TextCodeTranslator.Translate("Accounting.O.EditLine") + " " + entity.Line;

            var windowArgs: any = {};
            //windowArgs.TaxReportPM = this.EntityPM;
            windowArgs.TaxReportLinePM = entity;

            var logWindow = new LogitudeWindow();
            logWindow.Width = 450;
            logWindow.Height = 350;
            logWindow.Title = windowTitle;
            logWindow.WindowArgs = windowArgs;
            logWindow.WindowClosed.subscribe((event: any) => {
                if (event == "ok")
                    SessionLocator.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            });
            logWindow.Show('./Accounting/Components/EditTabs/TaxReport/EditTaxReportLine/EditTaxReportLineComponent');
        }




    }
}
