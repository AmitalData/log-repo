import {Component} from '@angular/core';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {ActivityPM} from '../../EntityPMs/ActivityPM';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../Infrastructure/Tools';
import {CustomerList} from '../../../Common/EntityLists/CustomerList';
import {CustomerListService} from '../../../Common/Services/StandardLists/CustomerListService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {CRMTool} from '../../Tools';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';

@Component({
    moduleId: module.id,
    templateUrl: "ActivityShortTitleComponent.html",
})

export class ActivityShortTitleComponent {
    public EntityPM: ActivityPM;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = this.entityArgs.EntityPM;

        if (this.EntityPM != null) {
            this.BuildComponent();
        }
    }

    BuildComponent(){
        this.ImageSrc = CRMTool.GetActivityImageSrc(this.EntityPM.ActivityTypePathCode);
    }
    public ImageSrc: string;
   

    //Properties
    get CustomerDataVisibility() {
        var myResult = false;

        if (this.EntityPM != null) {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.CustomerId)) {
                myResult = true;
            }
        }

        return myResult;
    }
    get CancelledVisibility() {
        var result = false;

        if (this.EntityPM != null) {
            if (this.EntityPM.ActivityStatusCode == "X") {
                result = true;
            }
        }
        return result;
    }
    get ControlBackground() {
        var result = null;

        if (this.EntityPM != null) {
            result = "red"; //new SolidColorBrush(Colors.Red) { Opacity = 0.1 };
        }
        return result;
    }
    get ActivityTypePathCode() {
        var result = "";

        if (this.EntityPM != null) {
            result = this.EntityPM.ActivityTypePathCode;
        }

        return result;
    }
    get ActivityTypeName() {
        var result = "";

        if (this.EntityPM != null) {
            result = this.EntityPM.ActivityTypeName;
        }

        return result;
    }
    get Subject() {
        var result = "";

        if (this.EntityPM != null) {
            result = this.EntityPM.Subject;
        }

        return result;
    }
    get CustomerName() {
        var myResult = "";

        if (this.EntityPM != null) {
            myResult = this.EntityPM.CustomerName;
        }

        return myResult;
    }
    get IsCustomerBlockedBusinessUnit()
    {
        var myResult = false;

        if (this.EntityPM != null) {
            myResult = this.EntityPM.IsCustomerBlockedBusinessUnit;
        }
        return myResult;
    }

    public ViewCustomerMethod() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.CustomerId)) {
            if (this.EntityPM.IsCustomerBlockedBusinessUnit) {
                var service = new CustomerListService();
                service.getSingleFromCache(this.EntityPM.CustomerId).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var customerList = myResponse.Result;
                        if (customerList != null) {
                            this.EditBlockedCustomer(customerList);
                        }
                    }
                });
            }
            else {
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({ EntityId: this.EntityPM.CustomerId, ObjectTableName: "Customer", BackButtonLabel: "Activity" });
                    });
            }
        }
    }

    private EditBlockedCustomer(customerList: CustomerList) {
        var windowTitle = "View Customer";
        var windowArgs: any = {};
        windowArgs.CustomerList = customerList;
        var logWindow = new LogitudeWindow();
        logWindow.Title = windowTitle;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./CRMModules/CRMOthers/Components/BlockedCustomer/BlockedCustomerComponent');
    }
}
