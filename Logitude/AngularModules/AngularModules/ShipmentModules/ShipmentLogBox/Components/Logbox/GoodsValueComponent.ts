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
import {LogBoxDocumentsComponent} from '../LogBox/LogBoxDocumentsComponent';
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
    templateUrl: './GoodsValueComponent.html'
})

export class GoodsValueComponent implements OnInit, AfterViewInit {

    DataContext: GoodsValueComponent = this;
   
    AdditionalData: any;

    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {


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
    
    public get SupAccount() { return new CustomNumbersPipe().transform(this.AdditionalData.SupAccount,0) }
    public set SupAccount(newValue: string) { this.AdditionalData.SupAccount = newValue; }

    public get IncotermId() { return this.AdditionalData.IncotermId }
    public set IncotermId(newValue: string) { this.AdditionalData.IncotermId = newValue; }

    public get Value() { return new CustomNumbersPipe().transform(this.AdditionalData.Value,0) }
    public set Value(newValue: string) { this.AdditionalData.Value = newValue; }

    public get CurrencyName() { return this.AdditionalData.CurrencyName }
    public set CurrencyName(newValue: string) { this.AdditionalData.CurrencyName = newValue; }

    public get CountryName() { return this.AdditionalData.CountryName }
    public set CountryName(newValue: string) { this.AdditionalData.CountryName = newValue; }

    public get SupplierName() { return this.AdditionalData.SupplierName }
    public set SupplierName(newValue: string) { this.AdditionalData.SupplierName = newValue; }

    public get SupplierFreight() { return new CustomNumbersPipe().transform(this.AdditionalData.SupplierFreight,0) }
    public set SupplierFreight(newValue: string) { this.AdditionalData.SupplierFreight = newValue; }
    
}
