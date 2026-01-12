import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { AppTool } from '../../../Infrastructure/Tools';
import { ObjectsLocator } from '../../../Infrastructure/Locators/ObjectsLocator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { DateTimePipe } from '../../../Controls/Pipes/DateTimePipe';
import { ChangeDetectorRef, Component } from '@angular/core';
import { TaxReportPMService } from 'Accounting/Services/StandardPMs/TaxReportPMService';
import { TaxReportLinePMService } from 'Accounting/Services/StandardPMs/TaxReportLinePMService';
import { TaxReportLineList } from 'Accounting/EntityLists/TaxReportLineList';

@Component({

    templateUrl: './MasavInterfaceListTemplate.html',
})

export class MasavInterfaceListTemplate {

    public rowData: any;
    public fieldName: any;
    public AdditionalData: any;
    public UpdateMessage: string;

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
        //var DatePipe = new DateTimePipe();
        //this.UpdateMessage = TextCodeTranslator.Translate("TaxReportLine.O.LastUpdatedBy") + " {" + this.rowData.UpdatedBUserName + " } " + TextCodeTranslator.Translate("TaxReportLine.O.On") + " {" + DatePipe.transform(this.rowData.LastUpdateDateTime, "DT") + " }";

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
    GetUpdateMessage(){
        var DatePipe = new DateTimePipe();
       return   TextCodeTranslator.Translate("TaxReportLine.O.LastUpdatedBy") + " {" + this.rowData.UpdatedBUserName + " } " + TextCodeTranslator.Translate("TaxReportLine.O.On") + " {" + DatePipe.transform(this.rowData.LastUpdateDateTime, "DT") + " }";
    }
    EditLine() {
        var lineEntity: TaxReportLineList = this.rowData;
        if (lineEntity) {
            this.CurrentSession.StartBusyIndicatorLoading();

            var windowTitle = TextCodeTranslator.Translate("Accounting.O.EditLine") + " " + lineEntity.Line;

            this._TaxReportPMService.get(lineEntity.TaxReportId).subscribe((myResult:any) => {

                var mm: ServiceResponse = myResult;
                if (!mm.HasError) {
                    var report = mm.Result;
                    this._TaxReportLinePMService.get(report.Id, lineEntity.Line).subscribe((myResult:any) => {

                        var mm: ServiceResponse = myResult;
                        if (!mm.HasError) {
                            this.CurrentSession.StopBusyIndicator();

                            var linePM = mm.Result;

                            var windowArgs: any = {};
                            windowArgs.TaxReportPM = report;
                            windowArgs.TaxReportLinePM = linePM;

                            var logWindow = new LogitudeWindow();
                            logWindow.Width = 450;
                            logWindow.Height = 450;
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
