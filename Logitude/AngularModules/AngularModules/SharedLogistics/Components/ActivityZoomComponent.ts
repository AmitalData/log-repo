import {Component, OnInit}  from '@angular/core';
import {FeatureLocator} from '../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {Guid} from '../../Infrastructure/Utilities/Guid';
import {SharedLogisticsService} from '../Services/Others/SharedLogisticsService';

import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {SessionInfo} from '../../Infrastructure/Utilities/SessionInfo';
import {ActivityItemDetailsViewModel} from './ViewModel/ActivityItemDetailsViewModel';
import {ActivityZoomItemViewModel} from './ViewModel/ActivityZoomItemViewModel';
import {TenantPMService} from '../../Common/Services/StandardPMs/TenantPMService';
import {TenantPM} from '../../Common/EntityPMs/TenantPM';

@Component({
    moduleId: module.id,
    selector: 'ActivityZoomControl',
    templateUrl: './ActivityZoomComponent.html',
    inputs: ['PartnerTypeId', 'DateParameter', 'DataContext', 'OnCloseWindowEvent'],
    providers: [SharedLogisticsService],
})

export class ActivityZoomComponent implements OnInit {

    ActivityList: ActivityZoomItemViewModel[];
    ActivityDetailsList: ActivityItemDetailsViewModel[];


    ActivityZoomSelectedItemViewModel: ActivityZoomItemViewModel;
    ActivityDetailsSelectedItemViewModel: ActivityItemDetailsViewModel;
    public ComboList: CodeNameClass[];
    ComboListSelectedItem: CodeNameClass;
    PartnerTypeId: string;
    DateParameter: string;
    DataContext: any;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _sharedLogisticsService: SharedLogisticsService) {
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
    }

    ngOnInit(

    ) {

        this.Run();

    }


    Run() {
        this.BuildFilters();
    }



    BuildFilters() {

        this.ComboList = [];
        this.ComboList.push(new CodeNameClass("T", "Today")); 
        this.ComboList.push(new CodeNameClass("W", "Last 7 Days"));
        this.ComboList.push(new CodeNameClass("M", "Last Month"));
        this.ComboListSelectedItem = this.ComboList.filter(d => d.Code == this.DateParameter)[0];
        this.GetZoomDetails();
    }


   ComboListSelectedValueChanged(item) {

        if (this.ComboListSelectedItem != item)
        {
            this.ComboListSelectedItem = item;
            this.DateParameter = this.ComboListSelectedItem.Code; 
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
            this.ActivityZoomSelectedItemViewModel = null;
            this.GetZoomDetails();

        }
    }

    //GetZoomDetails
   GetZoomDetails() {
       
       this.ActivityList = [];
       this.ActivityDetailsList = [];
       this._sharedLogisticsService.getCardLogDetails(this.PartnerTypeId, this.DateParameter , SessionInfo.LoggedUserTenant).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    myResult.forEach((item) => {
                        this.ActivityList.push(new ActivityZoomItemViewModel(item));
        });
                    this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    if (this.ActivityList && this.ActivityList.length > 0) {

                        this.GetActivityDetailsList(this.ActivityList[0]);
                    }
                   

                }

            }
            else this.CurrentSession.CurrentWindow.StopBusyIndicator();
        });

    }


    //GetActivityDetailsList

    GetActivityDetailsList(item: ActivityZoomItemViewModel) {


        if (this.ActivityZoomSelectedItemViewModel != item) {
            this.ActivityZoomSelectedItemViewModel = item;
       
            if (this.ActivityZoomSelectedItemViewModel) {
                this.ActivityDetailsList = [];
                this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
                this._sharedLogisticsService.getCardLogActivityDetailsList(item.LogDetails.CardId, item.LogDetails.ContactId, this.PartnerTypeId, this.DateParameter, SessionInfo.LoggedUserTenant).subscribe(res => {
                    var pmResponse: ServiceResponse = res;

             
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        if (myResult) {
                            myResult.forEach((item) => {
                                this.ActivityDetailsList.push(new ActivityItemDetailsViewModel(item, this.ActivityZoomSelectedItemViewModel));
                            });
                        }

                    }
                    this.CurrentSession.CurrentWindow.StopBusyIndicator();

                });
            }
        }
    }


    CloseButtonClicked() {

        this.CurrentSession.CloseCurrentWindow();
    }





    SetWindowArgs(args: any) {
        this.PartnerTypeId = args.PartnerTypeId;
        this.DateParameter = args.DateParameter;
        this.DataContext = args.DataContext;

    }





}
class CodeNameClass {
    Code: string;
    Name: string;
    constructor(code: string , name:string) {
        this.Code = code;
        this.Name = name;
    } 

}
