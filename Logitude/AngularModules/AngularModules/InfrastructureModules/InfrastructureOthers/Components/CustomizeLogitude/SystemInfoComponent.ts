import {Component}  from '@angular/core';
import {ExportDocumentService} from '../../../../Common/Services/DocumentServices/ExportDocumentService';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    moduleId: module.id,
    selector: 'SystemInfo',
    templateUrl: './SystemInfoComponent.html',   
    providers: [ExportDocumentService],
})

export class SystemInfoComponent {
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
    TemporalPackageVisibility: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _exportDocumentService: ExportDocumentService) {

    }

    SetDataContext(data: any) {
        this.ID = SessionLocator.Tenant.toString();
        this.PackageName = SessionLocator.TenantManagementJS.PackageName;
        this.NumberOfUsers = SessionLocator.TenantManagementJS.NumberOfUsers.toString();
        this.IsTrial = SessionLocator.TenantManagementJS.IsTrial ? "Yes" : "No";

        if (SessionLocator.TenantManagementJS.TemporalPackageCode) {
            this.TemporalPackageVisibility = true;
        }
        this.IsRecurring = SessionLocator.TenantManagementJS.IsRecurring ? "Yes" : "No";
        if (SessionLocator.TenantManagementJS.PaidUntilDate) {
            this.PaidVisibility = true;
        }

        if (SessionLocator.TenantManagementJS.BluesnapAccount) {
            this.BluesnapVisibility = true;
        }

        if (SessionLocator.TenantManagementJS.IsTrial) {
            this.IsTrailVisibility = true;
        }

        this.GetUsedSpaceFromServer();
    }

    GetUsedSpaceFromServer() {
        this._exportDocumentService.GetUsedSpaceForTenant(SessionLocator.Tenant).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    this.UsedSpace = myResult;
                }
            }

            else {

                if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                    var messageWindow: MessageWindow = new MessageWindow();
                    messageWindow.Title = "Logitude Message";
                    messageWindow.Show(pmResponse.ErrorsArray[0]);
                }
            }
        });
    }

    CloseButtonClicked() {    
        this.CurrentSession.CloseCurrentWindow();
    }
}
