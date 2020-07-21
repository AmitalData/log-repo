import { Component, OnInit} from '@angular/core';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { ObjectsLocator } from 'Infrastructure/Locators/ObjectsLocator';
import { DateTool, AppTool } from 'Infrastructure/Tools';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ConfirmWindow } from 'Controls/Windows/ConfirmWindow';
import { CargoTrackingExtendedPMService, CargoTrackingArgs } from 'Accounting/Services/ExtendedPMs/CargoTrackingExtendedPMService';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { EntityListService } from 'Infrastructure/Services/EntityListService';
import { CargoTrackingIncrementalStatExtendedListService } from 'Accounting/Services/ExtendedLists/CargoTrackingIncrementalStatExtendedListService';
 

@Component({
    
    templateUrl: './CargoTrackingIncrementalStatistics.html',
})

export class CargoTrackingIncrementalStatistics implements OnInit {
    private _entityListService: CargoTrackingIncrementalStatExtendedListService;
    public ValidationErrorsList: string[] = [];
    constructor(){
        this._entityListService = new CargoTrackingIncrementalStatExtendedListService();
    }

    public GridHeaderText:string = "Incremental Records Details";

    ngOnInit() {
        this.BuildColumns();
    }

    public DataSource = {
        pageSize: 50,
        rowCount: null,
        sortingDir: "Ascending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;
        },
    };

    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        var filters = new ApiQueryFilters();
        filters.PageSize = take;
        filters.PageIndex = skip;
        filters.GetAll = false;
        filters.GetCount = true;
        // filters.addAdditionalFilter("InterestReportId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");
        return this._entityListService.getByFilters(filters); 
    }


    public columns: any[] = null;
    BuildColumns() {
        this.columns = [];
        this.columns.push({
            FieldName: 'Id',
            DataTypeCode: 'Number',
            Display: 'Number', 
            Styles: { width: '100px' },
            HtmlListComponentName: 'CargoTrackingIncrementalStatListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/CargoTrackingIncrementalStatListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'StartDate',
            DataTypeCode: 'DateTime',
            Display: "Start Date",
            Styles: { width: '100px' },
            HtmlListComponentName: 'CargoTrackingIncrementalStatListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/CargoTrackingIncrementalStatListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'EndDate',
            DataTypeCode: 'DateTime',
            Display: 'EndDate',  
            Styles: { width: '100px' },
            HtmlListComponentName: 'CargoTrackingIncrementalStatListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/CargoTrackingIncrementalStatListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'Shipments',
            DataTypeCode: 'Number',
            Display: 'Shipments',  
            Styles: { width: '100px' },
            HtmlListComponentName: 'CargoTrackingIncrementalStatListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/CargoTrackingIncrementalStatListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'Cards',
            DataTypeCode: 'Number',
            Display: 'Cards',  
            Styles: { width: '100px' },
            HtmlListComponentName: 'CargoTrackingIncrementalStatListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/CargoTrackingIncrementalStatListTemplate',
            IsCustomTemplate: true
        });

        this.columns.push({
            FieldName: 'Ports',
            DataTypeCode: 'Number',
            Display: 'Ports',  
            Styles: { width: '100px' },
            HtmlListComponentName: 'CargoTrackingIncrementalStatListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/CargoTrackingIncrementalStatListTemplate',
            IsCustomTemplate: true
        });

        this.columns.push({
            FieldName: 'Countries',
            DataTypeCode: 'Number',
            Display: 'Countries',  
            Styles: { width: '100px' },
            HtmlListComponentName: 'CargoTrackingIncrementalStatListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/CargoTrackingIncrementalStatListTemplate',
            IsCustomTemplate: true
        });

        this.columns.push({
            FieldName: 'TransportModes',
            DataTypeCode: 'Number', 
            Display: 'TransportModes',  
            Styles: { width: '100px' },
            HtmlListComponentName: 'CargoTrackingIncrementalStatListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/CargoTrackingIncrementalStatListTemplate',
            IsCustomTemplate: true
        });
    }
}



 
