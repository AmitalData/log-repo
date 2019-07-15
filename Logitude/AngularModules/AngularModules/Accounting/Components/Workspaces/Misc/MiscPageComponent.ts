import { Component, Output, EventEmitter, AfterViewInit} from '@angular/core';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ListComponentArgs } from '../../../../Infrastructure/Args';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';
declare var window: any;
@Component({
    moduleId: module.id,
    templateUrl: './MiscPageComponent.html',
})

export class MiscPageComponent implements AfterViewInit {

    private _entityResourceService: EntityResourceService = new EntityResourceService();
    @Output() ReloadUserQueries = new EventEmitter();
    public isRTL: boolean = false;
    public isScreenLoaded: boolean = false;
    IsYEARTRANSFERVisibile: boolean = false;
    IsGEN1000MENUVisibile: boolean = false;
    IsRECV1000MENUVisibile: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this._entityResourceService.getEntityResourceByTableName("OpenFormatReport").subscribe((response: any) => {
            this._entityResourceService.getEntityResourceByTableName("TaxReport").subscribe((response: any) => {
                this._entityResourceService.getEntityResourceByTableName("TaxDeductionReport").subscribe((response: any) => {

                    var yearTransFeature = FeatureLocator.HasFeaturePermession("GLAccount", "YEARTRANSFERMENU");
                    console.log("YEARTRANSFERMENU Feature:" + yearTransFeature);
                    if (yearTransFeature) {
                        this.IsYEARTRANSFERVisibile = true;
                    }
                    var IsGEN1000MENUVisibile = FeatureLocator.HasFeaturePermession("GLAccount", "GEN1000MENU");
                    console.log("GEN1000MENU Feature:" + IsGEN1000MENUVisibile);
                    if (IsGEN1000MENUVisibile) {
                        this.IsGEN1000MENUVisibile = true;
                    }
                    var IsRECV1000MENUVisibile = FeatureLocator.HasFeaturePermession("GLAccount", "RECV1000MENU");
                    console.log("GEN1000MENU Feature:" + IsRECV1000MENUVisibile);
                    if (IsRECV1000MENUVisibile) {
                        this.IsRECV1000MENUVisibile = true;
                    }

         this.isScreenLoaded = true;
         this.CurrentSession.StopBusyIndicator();
                });
            });
        });



        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
    }

    ngAfterViewInit() {
        this.LoadAllScreenData();
    }
    public LoadAllScreenData() {

        this.ReloadUsersQuery();
    }
    ReloadUsersQuery() {
        this.ReloadUserQueries.emit();
    }

    InitComponent() {
        this.LoadAllScreenData();

    }

    ViewAccountingQuery(myQueryCode: string) {
        if (myQueryCode != null) {

            var displayTitle = "";
            var queryCode = myQueryCode;

            var filters = new ApiQueryFilters();

            var tableName = "";
            var listArgs = new ListComponentArgs();

            switch (myQueryCode) {
                case "AllOpenFormats":
                    {
                        displayTitle = TextCodeTranslator.Translate("OpenFormatReport");
                        tableName = "OpenFormatReport";
                     
                        break;
                    }
                    case "ALLTAXREPORTS":
                        {
                        displayTitle = TextCodeTranslator.Translate("TaxReport");
                        tableName = "TaxReport";
                      
                            break;
                    }
                case "ALLTaxDeductionReports": {

                    displayTitle = TextCodeTranslator.Translate("TaxDeductionReport");
                    tableName = "TaxDeductionReport";
                    break;

                }

                case "ACPD": {
                    this._entityResourceService.getEntityResourceByTableName("AccountingPeriod", 0).subscribe(response => {
                        var logitudeWindow = new LogitudeWindow();
                        logitudeWindow.Width = 750;
                        logitudeWindow.Height = 500;
                        logitudeWindow.Title = TextCodeTranslator.Translate("Accounting.O.AccountingPeriods");
                        logitudeWindow.Show('./Accounting/Components/Maintenance/AccountingPeriodsComponent');
                    });
                    break;
                }
                    
                case "ACYTC":
                    {
                        this.YearTransferMethod(true);
                        break;
                    }
           case "ACYT":{

                    this.YearTransferMethod(false);
                       break;

                       }
                default: { break; }
            }

            listArgs.QueryCode = myQueryCode;
            listArgs.Filters = filters;
            listArgs.ObjectTableName = tableName;
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = TextCodeTranslator.Translate("Accounting.General.O.Misc");
            listArgs.IgnoreSelectedPerspective = true;
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(response => {
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run(listArgs);
                        cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadAllScreenData());
                        this.CurrentSession.AddMenuReference(cmpRef);
                    });
            });
        }
    }
    YearTransferMethod(cancelYearTransfer: boolean) {
        this._entityResourceService.getEntityResourceByTableName("AccountingPeriod", 0).subscribe(response => {
            var logitudeWindow = new LogitudeWindow();
            logitudeWindow.Width = 500;
            logitudeWindow.Height = 300;

            logitudeWindow.Title = TextCodeTranslator.Translate("Accounting.O.YearTransfer");
            if (cancelYearTransfer) {
                logitudeWindow.Title = TextCodeTranslator.Translate("Accounting.O.CancelYearTransfer");
            }
            logitudeWindow.WindowArgs = { "CancelYearTransfer": cancelYearTransfer };
            logitudeWindow.Show('./Accounting/Components/Maintenance/YearTransferComponent');
        });
    }
    Generate1000() {


        this._entityResourceService.getEntityResourceByTableName("GLAccount", 0).subscribe(response => {
            var logitudeWindow = new LogitudeWindow();
            logitudeWindow.Width = 500;
            logitudeWindow.Height = 300;
            logitudeWindow.Title = TextCodeTranslator.Translate("Accounting.General.O.Generate1000");
            logitudeWindow.Show('./Accounting/Components/Maintenance/Generate1000Component');
        });

    }

    Receiving1000() {
        this._entityResourceService.getEntityResourceByTableName("GLAccount", 0).subscribe(response => {
            var logitudeWindow = new LogitudeWindow();
            logitudeWindow.Width = 650;
            logitudeWindow.Height = 350;
            logitudeWindow.Title = TextCodeTranslator.Translate("Accounting.General.O.Receiving1000");
            logitudeWindow.Show('./Accounting/Components/Maintenance/Receiving1000Component');
        });

    }

    RunNewOpenFormatReportWizard() {
        var windowTitle = TextCodeTranslator.Translate("Accounting.General.O.NewOpenFormatReport");
        //var windowArgs: BookingWizardArgs = new BookingWizardArgs();
        //windowArgs.IsNewEntity = true;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 400;
        logWindow.Height = 220;
        logWindow.Title = windowTitle;
        //logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => this.LoadAllScreenData());
        logWindow.Show('./Accounting/Components/NewEntity/NewOpenFormatReportComponent');
    }

    RunNewTaxReport() {


        var windowTitle = TextCodeTranslator.Translate("Accounting.General.O.NewOpenFormatReport");
        //var windowArgs: BookingWizardArgs = new BookingWizardArgs();
        //windowArgs.IsNewEntity = true;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 400;
        logWindow.Height = 200;
        logWindow.Title = windowTitle;
        //logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => this.LoadAllScreenData());
        logWindow.Show('./Accounting/Components/NewEntity/NewTaxReportComponent');

    }

    RunNewTaxDeduction() {


        var windowTitle = TextCodeTranslator.Translate("Accounting.General.O.NewOpenFormatReport");
        //var windowArgs: BookingWizardArgs = new BookingWizardArgs();
        //windowArgs.IsNewEntity = true;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 400;
        logWindow.Height = 200;
        logWindow.Title = windowTitle;
        //logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => this.LoadAllScreenData());
        logWindow.Show('./Accounting/Components/NewEntity/NewTaxDeductionReportComponent');

    }


}
