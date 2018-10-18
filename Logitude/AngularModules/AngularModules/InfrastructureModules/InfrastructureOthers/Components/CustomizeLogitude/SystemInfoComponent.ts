
declare var System: any;
declare var window: any;


import {TenantManagementPM} from '../../../../Infrastructure/EntityPMs/TenantManagementPM';
import {Component, OnInit, Output}  from '@angular/core';

import {ExportDocumentService} from '../../../../Common/Services/DocumentServices/ExportDocumentService';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';

import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
@Component({
    moduleId: module.id,

    selector: 'SystemInfo',
    templateUrl: './SystemInfoComponent.html',
    

    providers: [ExportDocumentService],
})
export class SystemInfoComponent implements OnInit {
    Tenant: TenantManagementPM;
    ID: string;
    PackageName: string;
    TemporalPackageName: string;

    TemporalStartDate
    TemporalEndDate
    NumberOfUsers: string;
    IsTrial: string;
    TrialStartDate: string;
    TrialEndDate: string;
    IsRecurring: string;
    PaidUntilDate: string;
    UsedSpace: string;
    BluesnapID: string;


    BluesnapVisibility: boolean=false;
    PaidVisibility: boolean=false;

    IsTrailVisibility: boolean=false;
    TemporalPackageVisibility: boolean=false;


    constructor(public _exportDocumentService: ExportDocumentService) {


    }

    ngOnInit(


    ) {





    }



    GetUsedSpaceFromServer() {

        this._exportDocumentService.GetUsedSpaceForTenant(this.Tenant.Id).subscribe(res => {

            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    this.UsedSpace = myResult;
                }
            } else {

                if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                    var messageWindow: MessageWindow = new MessageWindow();
                    messageWindow.Title = "Logitude Message";
                    messageWindow.Show(pmResponse.ErrorsArray[0]);
                }

            }
        });

    }
    SetDataContext(data: any) {
        this.Tenant = InfraSettings.TenantManagementPM;
        this.ID = this.Tenant.Id.toString();
        this.PackageName = this.Tenant.PackageName;
        this.NumberOfUsers = this.Tenant.NumberOfUsers.toString();
        this.IsTrial = this.Tenant.IsTrial ? "Yes" : "No";

        if (this.Tenant.TemporalPackageCode) {
            this.TemporalPackageVisibility = true;
        }
        this.IsRecurring = this.Tenant.IsRecurring ? "Yes" : "No"; 
        if (this.Tenant.PaidUntilDate) {
            this.PaidVisibility = true;
        }

        if (this.Tenant.BluesnapAccount) {
            this.BluesnapVisibility = true;
        }
        
        if (this.Tenant.IsTrial) {
            this.IsTrailVisibility = true;
        }

        this.GetUsedSpaceFromServer();
    }


    CloseButtonClicked() {

    
        SessionLocator.CurrentSession.CloseCurrentWindow();

    }




 


  





}