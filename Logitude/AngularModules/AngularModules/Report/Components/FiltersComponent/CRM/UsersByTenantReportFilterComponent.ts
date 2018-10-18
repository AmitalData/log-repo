import {Component, OnInit, Output, EventEmitter}  from '@angular/core';
import {AppTool} from '../../../../Infrastructure/Tools';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ReportFliter} from '../../../Components/Filters/ReportFliter';
import {QueryFilterItem} from '../../../Components/Filters/QueryFilterItem';

@Component({
    moduleId: module.id,
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


    RunButtonClicked(arg: boolean) {
      
            var myFilterItems: QueryFilterItem[] = [];
            myFilterItems.push(new QueryFilterItem("DistributorCode", this.DistributorCode));
            myFilterItems.push(new QueryFilterItem("PackageCode", this.PackageCode));
            myFilterItems.push(new QueryFilterItem("AddOnPackageCode", this.AddOnPackageCode));
            myFilterItems.push(new QueryFilterItem("IncludeInactiveUsers", this.IncludeInactiveUsers));
            myFilterItems.push(new QueryFilterItem("IncludeInactiveTenants", this.IncludeInactiveTenants));


            var myReportFliter: ReportFliter = new ReportFliter();
            myReportFliter.NumberOfPage = 1;
            myReportFliter.ProcessType = "GenerateReport";
            myReportFliter.QueryFilterItemLists = myFilterItems;
            this.RunReportEvent.emit(myReportFliter);
     
    }
}