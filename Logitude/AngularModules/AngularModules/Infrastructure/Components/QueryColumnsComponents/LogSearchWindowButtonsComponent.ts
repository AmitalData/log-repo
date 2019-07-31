declare var window: any;
import {Component, ViewContainerRef, OnInit, ViewChildren, QueryList, Output, EventEmitter, ChangeDetectorRef} from '@angular/core';
import {CommonDomainService} from '../../../Common/Services/CommonDomainService';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {PartnersDomainService} from '../../../Common/Services/PartnersDomainService';
import {EntityListService} from '../../../Infrastructure/Services/EntityListService';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {CardList} from '../../../Common/EntityLists/CardList';
import {CachedDataManager} from '../../../Infrastructure/Utilities/CachedDataManager';
import {AppTool} from '../../../Infrastructure/Tools';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {TextCodeTranslator} from '../../Utilities/TextCodeTranslator';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {FeatureLocator} from '../../Utilities/FeatureLocator';
import {ObjectTablePM} from '../../EntityPMs/ObjectTablePM';

@Component({
    moduleId: module.id,

    selector: 'LogSearchWindowButtonsComponent',
    templateUrl: './LogSearchWindowButtonsComponent.html',
})

export class LogSearchWindowButtonsComponent implements OnInit {

    public rowData: any;
    public fieldName: any;
    public LookUpTableName: any;
    public LookUpTable: ObjectTablePM;
    private PartnerTypes: Array<any> = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private CD: ChangeDetectorRef, private _entityListService: EntityListService) {
    }

    setVariables(rowData: any, fieldName: string) {
        this.rowData = rowData;

        var arry = fieldName.split(',');
        this.fieldName = arry[0];
        this.LookUpTableName = arry[1];
        this.LookUpTable = window.ObjectTables.filter(d => d.Name === this.LookUpTableName)[0];

        var apiQueryFilter: ApiQueryFilters;
        apiQueryFilter = new ApiQueryFilters();
        var partnerTypes: any[];
        this._entityListService.getAllFromCache("PartnerType", apiQueryFilter).then((res3: any) => {
            res3.subscribe(res4 => {
                this.PartnerTypes = res4.Result;
            })
        });

        var isDestroyed: boolean = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    }

    ngOnInit() {

    }
    
    EditButtonClicked22() {
        this.CurrentSession.PseventRowSelectEvent.emit(this.LookUpTable.Name);
        var id = this.rowData['Id'];
        if (!AppTool.IsNullOrEmpty(id) && !AppTool.IsNullOrEmpty(this.LookUpTableName)) {
            console.log("Editing: " + this.LookUpTableName + " " + id); 
            var logWindow = new LogitudeWindow();
            logWindow.Title = "Edit " + TextCodeTranslator.TranslateTable(this.LookUpTableName);
            logWindow.ShowEditComponent(id, this.LookUpTableName);
            //logWindow.WindowClosed.subscribe(($event: any) => {
            //    this.FireEvent("ok from LSWBC");
            //});

        }

    }

    EditButtonClicked() {
        this.CurrentSession.PseventRowSelectEvent.emit(this.LookUpTable.Name);
        var id = this.rowData['Id'];
        if (!AppTool.IsNullOrEmpty(id) && !AppTool.IsNullOrEmpty(this.LookUpTableName)) {
            if (!FeatureLocator.HasFeaturePermession(this.LookUpTableName, "UPDATE") && this.LookUpTable.EnableSecurity) {
                var messageWindow: MessageWindow = new MessageWindow();
                messageWindow.Show("You have no permission to edit an entity of this type.");
                return;
            }

            if (!FeatureLocator.HasFeaturePermession(this.LookUpTableName, "Module") && this.LookUpTable.EnableSecurity) {
                var messageWindow: MessageWindow = new MessageWindow();
                messageWindow.Show("Your package doesn't include this module..");
                return;
            }

            if (this.LookUpTableName == "Card" || this.LookUpTableName == "Carrier") {
                this.LookUpTableName = this.GetObjectTableNameForDependency(this.rowData["PartnerTypeId"], this.LookUpTableName);
            }

            if (!AppTool.IsNullOrEmpty(id)) {
                var logWindow = new LogitudeWindow();
                logWindow.Title = "Edit " + TextCodeTranslator.TranslateTable(this.LookUpTableName);
                logWindow.ShowEditComponent(id, this.LookUpTableName);
                logWindow.WindowClosed.subscribe(($event: any) => {
                    this.OnEditCompleted();
                });
            }
        }

    }

    public OnEditCompleted() {
        this.CurrentSession.SessionEvent.emit(this.rowData['Id']);
    }

    private GetObjectTableNameForDependency(dependency: string, parentObjectName: string) {

        if (parentObjectName == "Card" && dependency == "PO") {
            dependency = "CS";
        }

        var partnerType = this.PartnerTypes.filter(p => p.Id.toLowerCase() == dependency.toLowerCase())[0];
        if (partnerType != null && partnerType != undefined) {
            var name: string = partnerType.Name.replace(" ", "");
            var table: ObjectTablePM = window.ObjectTables.filter(d => d.Name.toLowerCase() === name.toLocaleLowerCase())[0];
            if (table != null) {
                return table.Name;
            }
            else {
                return parentObjectName;
            }
        }
        else {
            return parentObjectName;
        }
       
    }


}
