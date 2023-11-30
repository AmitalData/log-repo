declare var System: any, window: any;
import {ShipmentArchiveFilter} from '../../../../Controls/ShipmentArchiveFilter';
import {TransportsFilter} from '../../../../Controls/TransportsFilter';
import {Component, Output, EventEmitter, OnInit, AfterViewInit} from '@angular/core';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {SearchTextBox} from '../../../../Controls/SearchTextBox';
import {IconButton} from '../../../../Controls/IconButton';
import {LogGridComponent} from '../../../../Infrastructure/Components/LogitudeComponents/LogGridComponent/LogGridComponent'

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
    
    templateUrl: './LogBoxPackagesComponent.html'
})

export class LogBoxPackagesComponent implements OnInit, AfterViewInit {

    DataContext: LogBoxPackagesComponent = this;
   
    ShipmentPM: any;
    Title: string = "";
    public showShipmentOrderPackages: boolean = false;

    private CurrentSession = SessionLocator.SelectedSession;
    private AirTransportModeId = 'A';
    private LCLShipmentTypeId = 'LCLD';
    private ExportDirectionId = 'E';

    constructor() {


    }
    ngOnInit() {
        if ((!this.ShipmentPM.ForwarderShipmentNumber) || (this.ShipmentPM && this.ShipmentPM.ShipmentPackages.length == 0)) {
            this.showShipmentOrderPackages = true;
        }
    }
    ngAfterViewInit() {

    }
    SetWindowArgs(args: any) {
        this.ShipmentPM = args.ShipmentPM;
        this.Title = args.Title;
     
    } 
   
    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    } 
    
    get IsPackageList(): boolean {
        return this.ShipmentPM.DirectionId == this.ExportDirectionId && (this.ShipmentPM.TransportModeId == this.AirTransportModeId || this.ShipmentPM.ShipmentTypeId == this.LCLShipmentTypeId);
    }
}
