import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ReportsPreviewComponent} from '../../Components/ReportsPreviewComponent';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {ReportFliter} from '../../Components/Filters/ReportFliter';
import {QueryFilterItem} from '../../Components/Filters/QueryFilterItem';
import {Component, OnInit, Output, ElementRef}  from '@angular/core';
import {FormBuilder, FormGroup, FormsModule} from '@angular/forms';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {DateTool} from '../../../Infrastructure/Tools';
import {LastFilter} from '../../../Infrastructure/Utilities/LastFilter';
import {CodeNameClass} from './CodeNameClass';
import {AppTool} from '../../../Infrastructure/Tools';
import {LogitudeListBoxComponent} from '../../../Infrastructure/Components/LogitudeComponents/LogitudeListBox/LogitudeListBoxComponent';

@Component({

    moduleId: module.id,
    selector: 'ContainerTruckingFilterComponent',
    templateUrl: './ContainerTruckingFilterComponent.html',
    inputs: ['ReportsPreview'],
    entryComponents: [LogitudeListBoxComponent]


})



export class ContainerTruckingFilterComponent extends BaseComponent implements OnInit {
    public ReportsPreview: ReportsPreviewComponent;

    public myForm: FormGroup;
    public queryFilterItems: QueryFilterItem[];
    public queryFilterItem: QueryFilterItem;
    public DateType: string;
    public AgentId = null;
    public CustomerId = null;

    public reportFliter: ReportFliter;
    public DataContext: ContainerTruckingFilterComponent = this;
    public ObjectTableName: string = "FlightsSchedulesResponse";



    private mySelectedDirectionFilter: string = "All";
    public get MySelectedDirectionFilter() { return this.mySelectedDirectionFilter; }
    public set MySelectedDirectionFilter(newValue: string) {
        if (this.mySelectedDirectionFilter != newValue) {
            this.mySelectedDirectionFilter = newValue;
        }
    }

    constructor(fb: FormBuilder) {
        super();
        this.myForm = fb.group({});
    }

    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;
    }

    ngOnInit() {

        //if (!this.ReportsPreview.FilterConrolHeight) {
        //    this.ReportsPreview.SetFilterCotrolHeight(65);
        //}

    }


    onSelectedItemChanged(item) {
    }


    onSelectedItemShowChanged(item) {
    }

   

    RunReport() {
        this.queryFilterItems = new Array<QueryFilterItem>();

        if (this.MySelectedDirectionFilter != "All") {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "DirectionId";
            this.queryFilterItem.FieldValue = this.MySelectedDirectionFilter;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }
        if (this.AgentId) {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "AgentId";
            this.queryFilterItem.FieldValue = this.AgentId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);

        }
        if (this.CustomerId) {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "CustomerId";
            this.queryFilterItem.FieldValue = this.CustomerId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }



       


        if (!this.DateType) {
            this.DateType = "CreateDate";
        }


        this.reportFliter = new ReportFliter();
        this.reportFliter.DateType = this.DateType;
        this.reportFliter.Tenant = SessionInfo.LoggedUserTenant;
        this.reportFliter.QueryFilterItemLists = this.queryFilterItems;
        this.reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
        this.reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
        this.reportFliter.ReportCode = this.ReportsPreview.Report.Code;
        this.reportFliter.NumberOfPage = 1;
        this.reportFliter.ProcessType = "GenerateReport";




        this.ReportsPreview.CleanPartnersObslist();
        if (!AppTool.IsNullOrEmpty(this.CustomerId)) this.ReportsPreview.AddPartner("Customer", this.CustomerId);
        if (!AppTool.IsNullOrEmpty(this.AgentId)) this.ReportsPreview.AddPartner("Agent", this.AgentId);
        


        this.ReportsPreview.GenerateReport(this.reportFliter, true);
    }



}

