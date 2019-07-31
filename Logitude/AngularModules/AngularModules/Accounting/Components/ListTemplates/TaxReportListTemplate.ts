import { TaxReportLinePMService } from './../../Services/StandardPMs/TaxReportLinePMService';
import { TaxReportLineList } from './../../EntityLists/TaxReportLineList';
import { TaxReportPMService } from './../../Services/StandardPMs/TaxReportPMService';
import { Component, ChangeDetectorRef } from '@angular/core';
import { WebFreightDomainService } from '../../../Infrastructure/Services/WebFreightDomainService';
import { ServiceArgs } from '../../../Infrastructure/DataContracts/ServiceArgs';
import { OnInit, Output, EventEmitter, ComponentRef, QueryList } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
//import {JournalExtendedListService} from '../../Services/ExtendedLists/JournalExtendedListService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { AppTool } from '../../../Infrastructure/Tools';
import { ReconcileEventManager } from '../../Utilities/ReconcileEventManager';
import { ObjectsLocator } from '../../../Infrastructure/Locators/ObjectsLocator';
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
    public showLocal: boolean = false;

    private _TaxReportPMService: TaxReportPMService = new TaxReportPMService();
    private _TaxReportLinePMService: TaxReportLinePMService = new TaxReportLinePMService();
    private CurrentSession = SessionLocator.SelectedSession;

    private currentEntityPM: any;

    taxReportStatusCode: string;

    constructor(private CD: ChangeDetectorRef) {
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.showLocal = !SessionLocator.LoggedUserPM.DontShowLocal;
    }

    setVariables(rowData: any, fieldName: string, MyAdditionalData: any) {
        this.rowData = rowData;
        this.AdditionalData = MyAdditionalData;

        if (fieldName.includes(';')) {
            var temp = fieldName.split(';');
            if (temp.length == 2) {
                this.fieldName = temp[0];
                this.taxReportStatusCode = temp[1];
            }
        } else {
            this.fieldName = fieldName;
        }

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
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'Journal' });
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                    });
                });
        }
    }

    EditLine() {
        var lineEntity: TaxReportLineList = this.rowData;
        if (lineEntity) {
            this.CurrentSession.StartBusyIndicatorLoading();

            var windowTitle = TextCodeTranslator.Translate("Accounting.O.EditLine") + " " + lineEntity.Line;

            this._TaxReportPMService.get(lineEntity.TaxReportId).subscribe(myResult => {

                var mm: ServiceResponse = myResult;
                if (!mm.HasError) {
                    var report = mm.Result;




                    this._TaxReportLinePMService.get(report.Id, lineEntity.Line).subscribe(myResult => {

                        var mm: ServiceResponse = myResult;
                        if (!mm.HasError) {
                            this.CurrentSession.StopBusyIndicator();

                            var linePM = mm.Result;

                            var windowArgs: any = {};
                            windowArgs.TaxReportPM = report;
                            windowArgs.TaxReportLinePM = linePM;

                            var logWindow = new LogitudeWindow();
                            logWindow.Width = 450;
                            logWindow.Height = 350;
                            logWindow.Title = windowTitle;
                            logWindow.WindowArgs = windowArgs;
                            logWindow.WindowClosed.subscribe((event: any) => {
                                if (event == "ok")
                                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                            });
                            logWindow.Show('./Accounting/Components/EditTabs/TaxReport/EditTaxReportLine/EditTaxReportLineComponent');


                        }
                        else {
                            this.CurrentSession.StopBusyIndicator();
                        }
                    });








                }
                else {
                    this.CurrentSession.StopBusyIndicator();
                }
            });







        }




    }
}
