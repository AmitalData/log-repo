import {Component, OnInit, Output, EventEmitter, ChangeDetectorRef}  from '@angular/core';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {FollowerExtendedPMService} from '../../Services/ExtendedPMs/FollowerExtendedPMService';


@Component({
    moduleId: module.id,
    selector: 'SocialPeopleFollowComponent',
    templateUrl: './SocialPeopleFollowComponent.html',

})

export class SocialPeopleFollowComponent implements OnInit {
    IsFollowed: boolean = false;
    rowData: any;
    followerExtendedPMService: FollowerExtendedPMService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private cd: ChangeDetectorRef) {
        this.followerExtendedPMService = new FollowerExtendedPMService();
    }
   
    ngOnInit(

    ) {



    }


    

    setVariables(rowData: any, fieldName: string) {
        if (rowData != null) {
            this.rowData = rowData;
            //this.IsFollowed = rowData.IsFollowed;
            this.Destroyed();
        }
    }



    FollowingButtonClick( user: any) {
        
        this.AddDeleteFollower(user);
    }


    AddDeleteFollower(user: any) {
        var isdelete: boolean = user.IsFollowed;
        this.CurrentSession.StartBusyIndicatorSaving();
        this.followerExtendedPMService.AddDeleteFollower(user.Id, SessionLocator.LoggedUserId, isdelete, SessionLocator.Tenant).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            this.CurrentSession.StopBusyIndicator();

            if (!pmResponse.HasError) {
                this.rowData.IsFollowed = !isdelete;
                this.Destroyed();
            }
        });

    }


    Destroyed() {
        var isDestroyed: boolean = this.cd['destroyed'];
        if (!isDestroyed) {
            this.cd.detectChanges();
        }
    }









}
