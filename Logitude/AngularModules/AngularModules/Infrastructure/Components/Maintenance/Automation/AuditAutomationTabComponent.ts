declare var window: any;
import {Component, OnInit, AfterViewInit, ChangeDetectorRef}  from '@angular/core';
import {EntityChangePM} from '../../../../Common/EntityPMs/EntityChangePM';
import {EntityChangeExtendedPMService} from '../../../../Common/Services/ExtendedPMs/EntityChangeExtendedPMService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {EntityChangeAutomationsSummary} from '../../../../Common/DataContracts/EntityChangeAutomationsSummary';
import {ChangeField} from '../../../../Common/DataContracts/ChangeField';
import {EntityChangeAutomation} from '../../../../Common/DataContracts/EntityChangeAutomation';
import {AppTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { AutomationConditionsDetailsComponent } from './AutomationConditionsDetailsComponent';

@Component({
    
    selector: 'AuditAutomationTabComponent',
    templateUrl: './AuditAutomationTabComponent.html',
    inputs: ['ObjectTableName','EntityId'],
    providers: [EntityChangeExtendedPMService],
})

export class AuditAutomationTabComponent implements OnInit, AfterViewInit {
    public EntityId: string;
    public ObjectTableName = "";
    public ObjectTableId = "";
    public DataContext: this;
    EntityChangeListSelected: EntityChangePM;
    EntityChangeLists: EntityChangePM[];
    public AutomationTypeFilterSelectedValue: string = "All";
    public ConditionChangedSelectedValue: string = "All";
    IsConditionAll: boolean = true;
    IsCondition: boolean;
    ChangeFieldsList: ChangeField[];
    SelectedChangeFieldsList: ChangeField;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    SelectedTabCode: string;
    private AutomationChangedEvent: any = null;
    AutomationList: EntityChangeAutomation[];
    EntityAutomationList: EntityChangeAutomation[];
    SelectedEntityAutomationList: EntityChangeAutomation;
    IsCustomerCareUser: boolean = true;
    private CurrentSession = SessionLocator.SelectedSession;
    ShowConditionsDetailsLink: boolean = true;

    constructor(private _entityChangeExtendedPMService: EntityChangeExtendedPMService, private cd: ChangeDetectorRef) {

        if (SessionLocator.LoggedUserPM.IsCustomerCare) {
            this.IsCustomerCareUser = true;
        }

        this.Listen();
    }

    SetWindowArgs(windowArgs) {
        //this.ReportGroupList = windowArgs.ReportGroupList;
        //this.ReportList = windowArgs.ReportList;
    }

    ShowCondithionsDetails(item) { 
        let windowArgs: any = {};  
        let logWindow = new LogitudeWindow(); 
        logWindow.DataContext = this;
        logWindow.Height = 800;
        logWindow.Width = 840;
        logWindow.Title = "Conditions Statuses";
        logWindow.DataContext = this;
        windowArgs.CurrentEntityPM = item; 
        windowArgs.AutomationHistoryPM = item;
        windowArgs.DataViewModel = this;
        logWindow.WindowArgs = windowArgs;
        logWindow.IsShowCloseButton = true;
        logWindow.Show('./Infrastructure/Components/Maintenance/Automation/AutomationConditionsDetailsComponent');
        logWindow.WindowClosed.subscribe(closed => { 
        });
    }

    Listen() {
        if (!this.AutomationChangedEvent) {
            this.AutomationChangedEvent = this.CurrentSession.SessionEvent.subscribe(s => {
                if (s == this.ObjectTableName) {
                    this.LoadData();
                }
            });
        }
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.AutomationChangedEvent);
    }

    ngOnInit() {
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((response:any) => {
            var table = window.ObjectTables.filter(d => d.Name == this.ObjectTableName)[0];
            if (table) {
                this.ObjectTableId = table.Id;
                this.LoadData();
            }
        });

        this.cd.detectChanges();
    }

    ngAfterViewInit() {

    }

    LoadData() {
        this.EntityChangeLists = [];
        this.AutomationList = [];
        this.ChangeFieldsList = [];

        this._entityChangeExtendedPMService.getEntityChangePMsByEntityIdAndObjectTable(this.EntityId, this.ObjectTableId, SessionLocator.Tenant).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            this.CurrentSession.StopBusyIndicator();
            this.IsConditionAll = true;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                this.EntityChangeLists = myResult;
                this.SelectedTabCode = "FCH";

                if (this.EntityChangeLists && this.EntityChangeLists.length > 0) {
                    this.EntityChangeListSelected = this.EntityChangeLists[0];

                    this.LoadAutomationAndChangeFelids();
                }
            }
        });
    }


    EntityChangeListsChangeSelected(item: EntityChangePM) {

        if (item != this.EntityChangeListSelected) {
            this.EntityChangeListSelected = item;
            this.LoadAutomationAndChangeFelids();

        }

    }
    LoadAutomationAndChangeFelids() {
        this.AutomationList = [];
        this.ChangeFieldsList = [];
        this.CurrentSession.StartBusyIndicator("Loading...");
        this._entityChangeExtendedPMService.getEntityChangeAutomationsSummaryByEntityChangeId(this.EntityChangeListSelected.Id, this.ObjectTableName, SessionLocator.Tenant).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            this.CurrentSession.StopBusyIndicator();
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;

                var entityChangeAutomationsSummary: EntityChangeAutomationsSummary = myResult;

                if (entityChangeAutomationsSummary) {
                    this.ChangeFieldsList = entityChangeAutomationsSummary.ChangeFieldsList;
                    this.AutomationList = this.EntityAutomationList = entityChangeAutomationsSummary.EntityChangeAutomationList;
                    if (this.AutomationList.length && !this.AutomationList[0].ConditionsList.length) {
                        this.ShowConditionsDetailsLink = false;
                    } else {
                        this.ShowConditionsDetailsLink = true;
                    }

                }
            }

        });
    }



    AutomationTypeChangedMethod(type: string) {

        this.AutomationTypeFilterSelectedValue = type;
        this.RefreshEntityAutomationList();
    }


    ConditionChangedMethod(type: string) {

        this.ConditionChangedSelectedValue = type;
        this.IsConditionAll = false;
        this.IsCondition = false;

        switch (type) {
            case 'All':
                this.IsConditionAll = true;
                break;
            case 'true':
                this.IsCondition = true;

                break;
            case 'false':
                this.IsCondition = false;
                break;
        }

        this.RefreshEntityAutomationList();

    }

    RefreshButtonClicked() {
        this.LoadData();

    }




    RefreshEntityAutomationList() {
        if (AppTool.IsNullOrEmpty(this.AutomationTypeFilterSelectedValue) || this.AutomationTypeFilterSelectedValue == "All") {
            this.EntityAutomationList = this.AutomationList;
        }

        else this.EntityAutomationList = this.AutomationList.filter(d => d.AutomationType == this.AutomationTypeFilterSelectedValue);


        if (!this.IsConditionAll) {
            this.EntityAutomationList = this.EntityAutomationList.filter(d => d.IsConditionTrue == this.IsCondition);
        }

    }



}
