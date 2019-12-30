import { Component, Output, EventEmitter, AfterViewInit} from '@angular/core';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ListComponentArgs } from '../../../../Infrastructure/Args';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { EntityPMService } from '../../../../Infrastructure/Services/EntityPMService';

declare var window: any;
@Component({
    moduleId: module.id,
    templateUrl: './InterestPageComponent.html',
    providers: [EntityPMService],

})

export class InterestPageComponent implements AfterViewInit {

    private _entityResourceService: EntityResourceService = new EntityResourceService();
    @Output() ReloadUserQueries = new EventEmitter();
    private CurrentSession = SessionLocator.SelectedSession;
    public isRTL: boolean = false;
    public ObjectTableName = "InterestBasesType";
    constructor() {
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

    RunNewInterestBasesTypeWizard() {
        var windowArgs: any = {};
        windowArgs.IsNew = true;
        var windowTitle = TextCodeTranslator.Translate("Accounting.General.O.NewInterestBases");
        var logWindow = new LogitudeWindow();
        logWindow.Width = 680;
        logWindow.Height = 400;
        logWindow.WindowArgs = windowArgs;
        logWindow.Title = windowTitle;
        logWindow.WindowClosed.subscribe(($event: any) => this.LoadAllScreenData());
        logWindow.Show('./Accounting/Components/EditTabs/Interest/DetailsTab/InterestBasesTypeDetailsTabComponent');
    }

    RunNewInterestReportWizard() {
        var windowArgs: any = {};
        windowArgs.IsNew = true;
        var windowTitle = TextCodeTranslator.Translate("InterestReport");
        var logWindow = new LogitudeWindow();
        logWindow.Width = 320;
        logWindow.Height = 200;
        logWindow.WindowArgs = windowArgs;
        logWindow.Title = windowTitle;
        logWindow.WindowClosed.subscribe(($event: any) => this.LoadAllScreenData());
        logWindow.Show('./Accounting/Components/NewEntity/NewInterestReportComponent');
    }

        ViewAccountingQuery(myQueryCode: string) {
        if (myQueryCode != null) {

            var displayTitle = "";

            var filters = new ApiQueryFilters();

            var tableName = "";
            var listArgs = new ListComponentArgs();

            switch (myQueryCode) {
                case "Interest Bases":
                    {
                        displayTitle = TextCodeTranslator.Translate("InterestBasesType.Q.InterestBases");
                        tableName = "InterestBasesType";
                     
                        break;
                    }
                case "Interest Report":
                    {
                        displayTitle = TextCodeTranslator.Translate("InterestReport.Q.InterestReport");
                        tableName = "InterestReport";

                        break;
                    }
                 default: { break; }
            }

            listArgs.QueryCode = myQueryCode;
            listArgs.Filters = filters;
            listArgs.ObjectTableName = tableName;
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = TextCodeTranslator.Translate("Accounting.General.O.Interest");
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

}
