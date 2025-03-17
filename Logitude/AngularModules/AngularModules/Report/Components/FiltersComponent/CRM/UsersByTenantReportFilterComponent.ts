import {Component, OnInit, Output, EventEmitter}  from '@angular/core';
import {AppTool} from '../../../../Infrastructure/Tools';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ReportFliter} from '../../../Components/Filters/ReportFliter';
import {QueryFilterItem} from '../../../Components/Filters/QueryFilterItem';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';

@Component({
    
    templateUrl: './UsersByTenantReportFilterComponent.html',
})

export class UsersByTenantReportFilterComponent extends BaseComponent implements OnInit {
    public DataContext = this;
    public ValidationErrorsList = [];
    @Output() RunReportEvent: EventEmitter<ReportFliter> = new EventEmitter<ReportFliter>();
    constructor() {
        super();
      
    }

    ngOnInit() {
      
    }

  

    // Filters
    private distributorCode: string = null;
    public get DistributorCode() { return this.distributorCode; }
    public set DistributorCode(value: string) {
        if (this.distributorCode != value) {
            this.distributorCode = value;
        }
    }


    private packageCode: string = null;
    public get PackageCode() { return this.packageCode; }
    public set PackageCode(value: string) {
        if (this.packageCode != value) {
            this.packageCode = value;
        }
    }


    private addOnPackageCode: string = null;
    public get AddOnPackageCode() { return this.addOnPackageCode; }
    public set AddOnPackageCode(value: string) {
        if (this.addOnPackageCode != value) {
            this.addOnPackageCode = value;
        }
    }



    private includeInactiveUsers: boolean = false;
    public get IncludeInactiveUsers() { return this.includeInactiveUsers; }
    public set IncludeInactiveUsers(value: boolean) {
        if (this.includeInactiveUsers != value) {
            this.includeInactiveUsers = value;
        }
    }

    private includeInactiveTenants: boolean = false;
    public get IncludeInactiveTenants() { return this.includeInactiveTenants; }
    public set IncludeInactiveTenants(value: boolean) {
        if (this.includeInactiveTenants != value) {
            this.includeInactiveTenants = value;
        }
    }
    public IsSchedulerReport: boolean = false;  
    SetQueryFilterItems(queryFilterItems: Array<QueryFilterItem>,isSchedulerReport:boolean=true) {
        this.IsSchedulerReport = isSchedulerReport; 
        if (queryFilterItems) {
            queryFilterItems.forEach(queryFilterItem => {
                this.SetFilterItem(queryFilterItem);
            });
        }
    }
    public RunReportTitle: string = 'Run Report';
    SetRunReportTitle() {
         
            if (this.IsSchedulerReport) {
                this.RunReportTitle = TextCodeTranslator.Translate("AgingReport.O.PreviewReport");
            }
            else {
                this.RunReportTitle = TextCodeTranslator.Translate("AgingReport.O.RunReport");
            }
       
    }
    private SetFilterItem(queryFilterItem: QueryFilterItem) {
        if (queryFilterItem) {
            switch (queryFilterItem.FieldName) {
                case "DistributorCode":
                    this.DistributorCode = queryFilterItem.FieldValue;
                    break;
               
                case "PackageCode":
                    this.PackageCode = queryFilterItem.FieldValue;
                    break;                          
                case "AddOnPackageCode":
                    this.AddOnPackageCode =   queryFilterItem.FieldValue;
                        break; 
                case "IncludeInactiveUsers":
                    this.IncludeInactiveUsers = queryFilterItem.FieldValue;
                    break;
                case "IncludeInactiveTenants":
                    this.IncludeInactiveTenants = queryFilterItem.FieldValue;
                    break;
                           
            }
    
        }
    }
    ValidateSelectedFilters(){
        return true;
    }
    RunButtonClicked(arg: boolean) {
      
            var myReportFliter: ReportFliter = new ReportFliter();
            myReportFliter.NumberOfPage = 1;
            myReportFliter.ProcessType = "GenerateReport";
            myReportFliter.QueryFilterItemLists = this.GetQueryFilterItems();
            this.RunReportEvent.emit(myReportFliter);
     
    }

    GetQueryFilterItems(){
        var myFilterItems: QueryFilterItem[] = [];
        myFilterItems.push(new QueryFilterItem("DistributorCode", this.DistributorCode));
        myFilterItems.push(new QueryFilterItem("PackageCode", this.PackageCode));
        myFilterItems.push(new QueryFilterItem("AddOnPackageCode", this.AddOnPackageCode));
        myFilterItems.push(new QueryFilterItem("IncludeInactiveUsers", this.IncludeInactiveUsers));
        myFilterItems.push(new QueryFilterItem("IncludeInactiveTenants", this.IncludeInactiveTenants));

        return myFilterItems;
    }
}