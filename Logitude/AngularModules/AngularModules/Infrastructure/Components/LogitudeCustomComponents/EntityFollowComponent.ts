declare var System: any;
declare var window: any;
import {Component, Input, ViewContainerRef, OnInit, ChangeDetectorRef, EventEmitter, Output, ViewChild} from '@angular/core';
import {AppTool} from '../../../Infrastructure/Tools';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {ModulesService} from '../../../Infrastructure/Services/ModulesService';

@Component({
    moduleId: module.id,
    selector: 'EntityFollowComponent',
    templateUrl: './EntityFollowComponent.html',
    inputs: ['EntityId',  'ObjectTableName'],
})

export class EntityFollowComponent implements OnInit {
    myModulesService: ModulesService;
    IsFollowed: boolean = false;
    FollowersCount: number;


    EntityId: string = "";
    ObjectTableName: string = "";
    FollowEntityLists: any[] = [];
    ObjectTableId: string = "";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.myModulesService = new ModulesService();

    }
    ngOnInit() {

        var table = window.ObjectTables.filter(d => d.Name == this.ObjectTableName)[0];
        if (table) {
            this.ObjectTableId = table.Id;
        } 

        this.LoadEntityFollowers();
    }

    ToolTipMessage: string = "";
    LoadEntityFollowers() {
        this.FollowEntityLists = [];
        this.ToolTipMessage = "";
       // this.CurrentSession.StartBusyIndicatorSaving();
        this.myModulesService.GetUserFollowEntityLists(this.EntityId, this.ObjectTableId).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            //this.CurrentSession.StopBusyIndicator();

            if (!pmResponse.HasError && pmResponse.Result) {
                pmResponse.Result.forEach((item) => {
                    this.FollowEntityLists.push(item);
                    this.ToolTipMessage += item.FollowerName + " \n";
                });
          
                this.FollowersCount = this.FollowEntityLists.length;

              
                var followEntityList = this.FollowEntityLists.filter(f => f.FollowerUserId == SessionLocator.LoggedUserId)[0];
   
                if (followEntityList) this.IsFollowed = true;
                else this.IsFollowed = false;
            }
        });
        

    }

    AddFollowEntity(user: any) {
 
        this.CurrentSession.StartBusyIndicatorSaving();
        this.myModulesService.AddFollowEntity(this.EntityId, this.ObjectTableId, SessionLocator.LoggedUserId).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            this.CurrentSession.StopBusyIndicator();

            if (!pmResponse.HasError) {
                this.IsFollowed = true;
                this.FollowersCount += 1;
                this.LoadEntityFollowers();
            }
        });

    }
 
    DeleteFollowEntity(user: any) {
        this.CurrentSession.StartBusyIndicatorSaving();
        this.myModulesService.DeleteFollowEntity(SessionLocator.LoggedUserId).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            this.CurrentSession.StopBusyIndicator();

            if (!pmResponse.HasError) {
                this.IsFollowed = false;
                this.FollowersCount -= 1;
                this.LoadEntityFollowers();
            }
        });

    }


}


