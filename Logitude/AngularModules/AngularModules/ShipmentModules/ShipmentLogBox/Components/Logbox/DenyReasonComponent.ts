declare var System: any, window: any;
import {ShipmentArchiveFilter} from '../../../../Controls/ShipmentArchiveFilter';
import {TransportsFilter} from '../../../../Controls/TransportsFilter';
import {Component, Output, EventEmitter, OnInit, AfterViewInit} from '@angular/core';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {SearchTextBox} from '../../../../Controls/SearchTextBox';
import {IconButton} from '../../../../Controls/IconButton';
import {LogGridComponent} from '../../../../Infrastructure/Components/LogitudeComponents/LogGridComponent/LogGridComponent'
import {Http, Response} from '@angular/http';
import {ServiceArgs} from '../../../../Infrastructure/DataContracts/ServiceArgs';
import {EntityListService} from '../../../../Infrastructure/Services/EntityListService';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {LogBoxDocumentsComponent} from './LogBoxDocumentsComponent';
import {ShipmentDomainService, ImporterQueriesDataCounts} from '../../../../Shipment/Services/ShipmentDomainService';
import {AppTool} from '../../../../Infrastructure/Tools';
import {CustomNumbersPipe} from '../../../../Infrastructure/Pipes/CustomNumbersPipe';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {EntityStatusListService} from '../../../../Infrastructure/Services/StandardLists/EntityStatusListService';
import {BranchListService} from '../../../../Common/Services/StandardLists/BranchListService';
import {DepartmentListService} from '../../../../Common/Services/StandardLists/DepartmentListService';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {Guid} from '../../../../Infrastructure/Utilities/Guid';
import {ShipmentPMService} from '../../../../Shipment/Services/StandardPMs/ShipmentPMService';
import {DocumentsFilingExtendedPMService} from '../../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';
import {GroupByPipe} from '../../../../Infrastructure/Pipes/GroupByPipe';
import {ShipmentAdditionalCloudDataService} from '../../../../Shipment/Services/Others/ShipmentAdditionalCloudDataService';
import {ImageLibraryService} from '../../../../Common/Services/Others/ImageLibraryService';
import {ServiceHelper} from '../../../../Infrastructure/Utilities/ServiceHelper';

@Component({
    moduleId: module.id,
    templateUrl: './DenyReasonComponent.html'
})

export class DenyReasonComponent extends BaseComponent implements OnInit, AfterViewInit {

    DataContext: DenyReasonComponent = this;
    ValidationErrorsList: any[];
    AdditionalData: any;
    public _ShipmentAdditionalCloudDataService: ShipmentAdditionalCloudDataService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this._ShipmentAdditionalCloudDataService = new ShipmentAdditionalCloudDataService();
    }
    ngOnInit() {
        
    }
    ngAfterViewInit() {

    }
    SetWindowArgs(args: any) {
        this.AdditionalData = args.AdditionalData;
    } 
   
    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    SendButtonClicked() {
        this.ValidationErrorsList = [];
        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (AppTool.IsNullOrEmpty(this.DenyReason)) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", "DenyReason"));
        }
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving ...");
            this.AdditionalData.IsImporterApprovalRequried = false;
            this.DenyReason = SessionLocator.LoggedUserPM.EnglishName + ", " + SessionLocator.LoggedUserPM.LocalName + ", " + SessionLocator.LoggedUserPM.Email + ", " + this.DenyReason + ", " + this.AdditionalData.VersionApproved;
            this._ShipmentAdditionalCloudDataService.update(this.AdditionalData).subscribe(AdditionalResult => {
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
                this.CurrentSession.CloseCurrentWindowEmit("Denied");
            });
        }
    }
    
    public get DenyReason() { return this.AdditionalData.DenyReason }
    public set DenyReason(newValue: string) { this.AdditionalData.DenyReason = newValue; }
        
}
