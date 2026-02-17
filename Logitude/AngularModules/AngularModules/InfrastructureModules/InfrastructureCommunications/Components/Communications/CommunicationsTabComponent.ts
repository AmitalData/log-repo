declare var window: any;
import {Component, OnDestroy} from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {DownloadManager} from '../../../../Infrastructure/Utilities/DownloadManager';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {CommunicationLogList} from '../../../../Common/EntityLists/CommunicationLogList';
import {CommunicationLogListService} from '../../../../Common/Services/StandardLists/CommunicationLogListService';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {AppTool} from '../../../../Infrastructure/Tools';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';

@Component({
    moduleId: module.id,
    templateUrl: './CommunicationsTabComponent.html',
})

export class CommunicationsTabComponent implements OnDestroy {
    private EntityId: string;
    private ObjectTableId: string;
    private ObjectTableName: string = "CommunicationLog";
    public IsTitleHidden: boolean = false;
    public IsForINTTRA: boolean = false;
    public ItemsSource: Array<CommunicationLogList>;
    private EntityPM: any;
    public IsResourcesReady: boolean = false;
    public TabHeaderTextCode: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, private entityResourceService: EntityResourceService) {
        this.ItemsSource = [];

        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((res: any) => {
            this.IsResourcesReady = true;
            this.Listen();
            this.InitTab();
        });
    }

    private SessionEvent: any = null;
    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {

            this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(event => {
                if (event == "CommunicationRefresh") {
                    this.LoadData();
                }
            });
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SessionEvent);
    }

    private InitTab() {
        this.EntityId = this.entityArgs.EntityPM.Id;
        this.EntityPM = this.entityArgs.EntityPM;
        this.ObjectTableName = this.entityArgs.ObjectTableName;

        if (this.ObjectTableName == "Master") {
            this.ObjectTableName = "Shipment";
        }

        // Ayman:
        // we need this for Translation
        // Please don't remove it
        this.TabHeaderTextCode = this.ObjectTableName + ".TH.Communications";

        this.ObjectTableId = window.ObjectTables.filter(x => x.Name === this.ObjectTableName)[0].Id;
        this.LoadData();
    }

    private myService: CommunicationLogListService;
    private LoadData() {
        if (this.EntityId != null && this.ObjectTableId != null) {

            if (this.myService == null) {
                this.myService = new CommunicationLogListService();
            }

            var filters = new ApiQueryFilters();
            filters.PageIndex = 0;
            filters.PageSize = 100;
            filters.SortBy = "CreateDate";
            filters.SortDirection = "Descending";

            filters.Filter1Name = "EntityId";
            filters.Filter1Value = this.EntityId;
            filters.Filter1Operator = "Equals";

            filters.Filter2Name = "ObjectTableId";
            filters.Filter2Value = this.ObjectTableId;
            filters.Filter2Operator = "Equals";
            if (this.EntityPM != null && this.EntityPM.IsForINTTRA == true) {
                filters.addAdditionalFilter("FromTo", "true", null, null, "Contains", true, false, false, "string");
            }

            this.myService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
                if (myResponse == null) {
                    this.ItemsSource = [];
                }

                else {
                    this.ItemsSource = myResponse.Result;
                }
            });
        }
    }

    RefreshButtonClicked() {
        this.LoadData();
    }

    ViewXMLClicked(item: CommunicationLogList) {
        DownloadManager.DownloadCommunicationLogXML(item);
    }
    EditItemClicked(item: CommunicationLogList) {
        var entityId = item.Id;

        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: entityId, ObjectTableName: 'CommunicationLog' });
            });
    }
}
