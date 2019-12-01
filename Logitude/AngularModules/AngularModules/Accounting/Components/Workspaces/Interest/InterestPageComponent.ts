import { Component, Output, EventEmitter, AfterViewInit} from '@angular/core';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ListComponentArgs } from '../../../../Infrastructure/Args';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';
import { ServiceLocator } from '../../../../Infrastructure/Locators/ServiceLocator';
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
    constructor(private entityPMService: EntityPMService) {
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
    //RunNewInterestBasesTypeWizard() {
    //    var windowTitle = TextCodeTranslator.Translate("Accounting.General.O.NewInterestBases");
    //    var logWindow = new LogitudeWindow();
    //    logWindow.Width = 720;
    //    logWindow.Height = 400;
    //    logWindow.Title = windowTitle;
    //    logWindow.WindowClosed.subscribe(($event: any) => this.LoadAllScreenData());
    //    logWindow.Show('./Accounting/Components/NewEntity/NewInterestBasesTypeComponent');
    //}

    RunNewInterestBasesTypeWizard() {
        if (!FeatureLocator.HasEntityPermessions(this.ObjectTableName, "NEW", true)) {
            return;
        }
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName, 0).subscribe(response => {
            this.RunNewGenaricEntity();
            ServiceLocator.SendTotangoUserActivity(this.ObjectTableName, "New" + this.ObjectTableName);
        });
    }
    private RunNewGenaricEntity() {
        var componentPath = "./Infrastructure/GenericComponents/NewEntityComponent";
        this.entityPMService.getNewEntity(this.ObjectTableName).then(response => {
            var args = new EntityArgs();
            args.EntityPM = response;
            args.ObjectTableName = this.ObjectTableName;
            var logWindow = new LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;
            var windowTitle = TextCodeTranslator.Translate("Accounting.General.O.NewInterestBaseType");
            logWindow.WindowArgs = args;
            logWindow.Title = windowTitle;
            logWindow.Show(componentPath);
        });
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
                 default: { break; }
            }

            listArgs.QueryCode = myQueryCode;
            listArgs.Filters = filters;
            listArgs.ObjectTableName = tableName;
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = TextCodeTranslator.Translate("Accounting.General.O.Interest");
            listArgs.NewButtonLabel = TextCodeTranslator.Translate("Accounting.General.O.NewInterestBases");
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
