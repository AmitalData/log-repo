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
    templateUrl: './TaxScreenComponent.html'
})

export class TaxScreenComponent implements OnInit, AfterViewInit {

    DataContext: TaxScreenComponent = this;
   
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
        var ammount = 0;
        var basis = 0;
        var topay = 0;
        var post = 0;
        this.AdditionalData.TaxesDetails.forEach((item, key) => {
            ammount += +(item.TaxAmount);
            basis += +(item.TaxBasis);
            topay += +(item.TaxToPay);
            post += +(item.TaxPostponed);
        });

        this.TotalAmount = ammount;
        this.TotalTaxToPay = topay;
        this.TotalTaxBasis = basis;
        this.TotalPostboned = post;
    } 
   
    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    
    public get Taxtypename() { return this.AdditionalData.Taxtypename }
    public set Taxtypename(newValue: string) { this.AdditionalData.Taxtypename = newValue; }

    public get TaxBasis() { return new CustomNumbersPipe().transform(this.AdditionalData.TaxBasis, 0) }
    public set TaxBasis(newValue: string) { this.AdditionalData.TaxBasis = newValue; }

    public get TaxToPay() { return new CustomNumbersPipe().transform(this.AdditionalData.TaxToPay, 0) }
    public set TaxToPay(newValue: string) { this.AdditionalData.TaxToPay = newValue; }

    public get TaxAmount() { return new CustomNumbersPipe().transform(this.AdditionalData.TaxAmount, 0) }
    public set TaxAmount(newValue: string) { this.AdditionalData.TaxAmount = newValue; }

    public get TaxPostponed() { return new CustomNumbersPipe().transform(this.AdditionalData.TaxPostponed, 0) }
    public set TaxPostponed(newValue: string) { this.AdditionalData.TaxPostponed = newValue; }

    private totalTaxBasis: number = 0;
    public get TotalTaxBasis() { return this.totalTaxBasis }
    public set TotalTaxBasis(newValue: number) { this.totalTaxBasis = newValue; }

    private totalTaxToPay: number = 0;
    public get TotalTaxToPay() { return this.totalTaxToPay }
    public set TotalTaxToPay(newValue: number) { this.totalTaxToPay = newValue; }

    private totalAmount: number = 0;
    public get TotalAmount() { return this.totalAmount }
    public set TotalAmount(newValue: number) { this.totalAmount = newValue; }

    private totalPostboned: number = 0;
    public get TotalPostboned() { return this.totalPostboned }
    public set TotalPostboned(newValue: number) { this.totalPostboned = newValue; }
    
}
