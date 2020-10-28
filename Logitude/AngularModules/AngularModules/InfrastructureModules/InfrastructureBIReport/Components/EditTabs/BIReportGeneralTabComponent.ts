import { Component, OnInit, AfterViewInit } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { BIReportPM } from '../../../../Infrastructure/EntityPMs/BIReportPM';
import { BIReportPMService } from '../../../../Infrastructure/Services/StandardPMs/BIReportPMService';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { AppTool} from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { BIReportFolderExtendedListService } from '../../../../Infrastructure/Services/ExtendedLists/BIReportFolderExtendedListService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    selector: 'BIReportGeneralTabComponent',
    
    templateUrl: './BIReportGeneralTabComponent.html',
})

export class BIReportGeneralTabComponent extends BaseComponent {

    public EntityPM: BIReportPM;
    public ObjectTableName ="BIReport";
    public DataContext: BIReportGeneralTabComponent = this;
    public BIReportFolders: string[] = [];
    public SelectdBIReportFolder: string;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;
    private BIReportFolderExtendedListService: BIReportFolderExtendedListService;

    constructor(public entityArgs: EntityArgs) {
        super();
        this._entityResourceService.getEntityResourceByTableName("BIReport").subscribe((response: any) => { });
        this.BIReportFolderExtendedListService = new BIReportFolderExtendedListService();
        this.EntityPM = this.entityArgs.EntityPM;
        this.SetUIProperties();
        this.FillBIReportFolderNamesList();
    }

    SetUIProperties() {
        this.UIProperties.SetEnabled("TypeCode", this.ObjectTableName, false);
        this.UIProperties.SetRequired("DWQueryId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.DWQueryId));
    }


    FillBIReportFolderNamesList() {
        this.CurrentSession.StartBusyIndicatorSaving();
        this.BIReportFolderExtendedListService.GetPermittedFolders(SessionLocator.LoggedUserId).subscribe((myResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {
                this.BIReportFolders = myResponse.Result;
                var selectedBIReport: string = myResponse.Result.filter(bi => bi.Id == this.EntityPM.BIReportFolderId)[0];
                if (!AppTool.IsNullOrEmpty(selectedBIReport)) this.BIReportFolderSelectionChanged(selectedBIReport);
            }
        });
    }

    BIReportFolderSelectionChanged(selectControl: any) {
        if (selectControl) {
            this.SelectdBIReportFolder = selectControl;
            this.BIReportFolderId = selectControl.Id;
            this.UIProperties.SetRequired("BIReportFolderId", this.ObjectTableName, false);
        }
        else {
            this.SelectdBIReportFolder = "";
            this.BIReportFolderId = "";
            this.UIProperties.SetRequired("BIReportFolderId", this.ObjectTableName, true);
        }
    }


    get Name() { return this.EntityPM.Name; }
    set Name(newValue: string) {
        if (this.EntityPM.Name != newValue) {
            this.EntityPM.Name = newValue;
        }
    }

    get Description() { return this.EntityPM.Description; }
    set Description(newValue: string) {
        if (this.EntityPM.Description != newValue) {
            this.EntityPM.Description = newValue;
        }
    }

    get BIReportFolderId() { return this.EntityPM.BIReportFolderId; }
    set BIReportFolderId(newValue: string) {
        if (this.EntityPM.BIReportFolderId != newValue) {
            this.EntityPM.BIReportFolderId = newValue;
        }
    }

    get DWQueryId() { return this.EntityPM.DWQueryId; }
    set DWQueryId(newValue: string) {
        if (this.EntityPM.DWQueryId != newValue) {
            this.EntityPM.DWQueryId = newValue;
        }
    }

    get TypeCode() { return this.EntityPM.TypeCode; }
    set TypeCode(newValue: string) {
        if (this.EntityPM.TypeCode != newValue) {
            this.EntityPM.TypeCode = newValue;
        }
    }
}
