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
import {DocumentTypePMExtendedService} from '../../../../Common/Services/ExtendedPMs/DocumentTypePMExtendedService';
import {DocumentTypePMService} from '../../../../Common/Services/StandardPMs/DocumentTypePMService';
import {DocumentTypePM} from '../../../../Common/EntityPMs/DocumentTypePM';

@Component({
    
    templateUrl: './DigitalSignDocTypeComponent.html'
})

export class DigitalSignDocTypeComponent extends BaseComponent implements OnInit, AfterViewInit {

    DataContext: DigitalSignDocTypeComponent = this;
    ValidationErrorsList: any[];
    DocTypes: any[];
    UpdateDocTypes: any[] = [];
    _DocumentTypeListService: DocumentTypePMExtendedService;
    _DocumentTypePMService: DocumentTypePMService
    private CurrentSession = SessionLocator.SelectedSession;
    SystemName: string;
    constructor() {
        super();
        this._DocumentTypeListService = new DocumentTypePMExtendedService();
        this._DocumentTypePMService = new DocumentTypePMService();
    }
    ngOnInit() {

        var objectTablePm = window.ObjectTables.filter(d => d.Name == "Shipment")[0];
        this._DocumentTypeListService.GetDocumentTypesByObjectTableAndTenant(objectTablePm.Id,SessionLocator.Tenant).subscribe((res:any) => {
            this.DocTypes = res.Result;
            this.DocTypes = this.DocTypes.sort((a, b) => { return (a.OrderBy === b.OrderBy) ? 0 : (a.OrderBy < b.OrderBy) ? -1 : 1 });
        });
        this.FillSystemName();
    }
    ngAfterViewInit() {

    }
    SetWindowArgs(args: any) {
        //this.AdditionalData = args.AdditionalData;
    } 

    FillSystemName() {
        if (SessionLocator.PrivateLableSettings) {
            this.SystemName = SessionLocator.PrivateLableSettings.PrivateLabelName;
            return;
        }
        this.SystemName = "Logbox";
    }

    onCheckBoxChecked(Type: any) {
        if (this.UpdateDocTypes.filter(a => a.Id == Type.Id).length > 0) {
            this.UpdateDocTypes = this.UpdateDocTypes.filter(a => a.Id != Type.Id);
        }
        this.UpdateDocTypes.push(this.CustomMapJsonToEntityPM(Type));
    }


    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OKButtonClicked() {
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving ..");
        //var itemsProcessed = 0;
        //this.UpdateDocTypes.forEach((Type) => {

            this._DocumentTypeListService.update(this.UpdateDocTypes).subscribe((myResult:any) => {
                //itemsProcessed++;
                //if (itemsProcessed === this.UpdateDocTypes.length) {
                    this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    this.CurrentSession.CloseCurrentWindow();
                   
                //}
            });
        //});
            
    }
    CustomMapJsonToEntityPM(jsonPM: any, entityPM: DocumentTypePM = null) {


        if (!entityPM) {

            entityPM = new DocumentTypePM();
        }
         
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "PropertyChanged") {

                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }



        entityPM.IsDirty = false;
        return entityPM;
    }
    
    //public get DenyReason() { return this.AdditionalData.DenyReason }
    //public set DenyReason(newValue: string) { this.AdditionalData.DenyReason = newValue; }
        
}
