import { Component, Output, EventEmitter, AfterViewInit} from '@angular/core';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ListComponentArgs } from '../../../../Infrastructure/Args';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';

@Component({
    moduleId: module.id,
    templateUrl: './MiscPageComponent.html',
})

export class MiscPageComponent implements AfterViewInit {

    private _entityResourceService: EntityResourceService = new EntityResourceService();
    @Output() ReloadUserQueries = new EventEmitter();
    public isRTL: boolean = false;
    public isScreenLoaded: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this._entityResourceService.getEntityResourceByTableName("OpenFormatReport").subscribe((response: any) => {


         this.isScreenLoaded = true;
         this.CurrentSession.StopBusyIndicator();
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
                        listArgs.Perspective = "OpenFormatReportMain";
                        break;
                    }
                    case "ALLTAXREPORTS":
                        {
                            displayTitle = "Tax Reports";
                            tableName = "TaxReport";
                            break;
                        }

                default: { break; }
            }

            listArgs.QueryCode = myQueryCode;
            listArgs.Filters = filters;
            listArgs.ObjectTableName = tableName;
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = TextCodeTranslator.Translate("Accounting.General.O.Main");
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


    RunNewOpenFormatReportWizard() {
        var windowTitle = TextCodeTranslator.Translate("Accounting.General.O.NewOpenFormatReport");
        //var windowArgs: BookingWizardArgs = new BookingWizardArgs();
        //windowArgs.IsNewEntity = true;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 400;
        logWindow.Height = 180;
        logWindow.Title = windowTitle;
        //logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => this.LoadAllScreenData());
        logWindow.Show('./Accounting/Components/NewEntity/NewOpenFormatReportComponent');
    }


}
