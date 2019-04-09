import {Component} from '@angular/core';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {OpportunityPM} from '../../EntityPMs/OpportunityPM';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../Infrastructure/Tools';
import {CustomerList} from '../../../Common/EntityLists/CustomerList';
import {CustomerListService} from '../../../Common/Services/StandardLists/CustomerListService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {CRMTool} from '../../Tools';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';

@Component({
    moduleId: module.id,
    templateUrl: "OpportunityShortTitleComponent.html",
})

export class OpportunityShortTitleComponent {
    public EntityPM: OpportunityPM;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = this.entityArgs.EntityPM;
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
                        cmpRef.instance.Run({ EntityId: this.EntityPM.CustomerId, ObjectTableName: "Customer", BackButtonLabel: "Opportunity" });
                    });
            }
        }
    }


    get EnglishName() {
        var myResult: string = null;

        if (this.EntityPM != null) {
            myResult = this.EntityPM.CustomerName;
        }

        return myResult;
    }
    get RankName() {
        var myResult: string = null;

        if (this.EntityPM != null) {
            myResult = this.EntityPM.CustomerRankName;
        }

        return myResult;
    }
    get RankSource1() {
        var myResult: string = null;
        // silver to lower
        if (this.EntityPM != null) {
            var RankCode = this.EntityPM.CustomerRankCode;

            switch (RankCode) {
                case "1":
                case "2":
                case "3": {
                    myResult = "./Images/Icons/StarOrange.png";
                    break;
                }

                default: {
                    myResult = "./Images/Icons/StarGray.png";
                    break;
                }
            }
        }

        return myResult;
    }
    get RankSource2() {
        var myResult: string = null;

        if (this.EntityPM != null) {
            var RankCode = this.EntityPM.CustomerRankCode;

            switch (RankCode) {
                case "2":
                case "3": {
                    myResult = "./Images/Icons/StarOrange.png";
                    break;
                }

                default: {
                    myResult = "./Images/Icons/StarGray.png";
                    break;
                }
            }
        }

        return myResult;
    }
    get RankSource3() {
        var myResult: string = null;

        if (this.EntityPM != null) {
            var RankCode = this.EntityPM.CustomerRankCode;

            switch (RankCode) {
                case "3": {
                    myResult = "./Images/Icons/StarOrange.png";
                    break;
                }

                default: {
                    myResult = "./Images/Icons/StarGray.png";
                    break;
                }
            }
        }

        return myResult;
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
