declare var System: any;
declare var window: any;
import {AppTool} from '../../../Infrastructure/Tools';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import { Component, Input, ViewContainerRef, OnInit, ChangeDetectorRef, EventEmitter, Output, ViewChild} from '@angular/core';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {PostsArgs} from '../../../Infrastructure/DataContracts/PostsArgs';

@Component({
    moduleId: module.id,

    selector: 'SocialComponent',
    templateUrl: './SocialComponent.html', 
    inputs: ['QueryName', 'SubQueryName', 'EntityId', 'ObjectTableName', 'ScreenCode', 'EntityDescription', 'IsEntityMode', 'InsideEntity', 'RegardingEntity'],

})


export class SocialComponent implements OnInit {

    @ViewChild('Child', { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;
    QueryName: string = "";
    SubQueryName: string = "";
    EntityId: string = "";
    ObjectTableName: string = "";
    ScreenCode: string = "";
    EntityDescription: string = "";
    IsEntityMode: boolean = false;
    RegardingEntity: string = "";
    ObjectTableId: string = "";
    InsideEntity: boolean = false;

    constructor() {

        if (FeatureLocator.HasFeaturePermession("General", "SOCIALGENERAL")) {

            this.RunComponent();
        }

    }
    ngOnInit() {


    }


    RunComponent() {
        if (this.viewContainerRef) {
            
            this.LoadSocialComponent();

        }

        else {
            this.RunComponentTimer();
        }
    }



    LoadSocialComponent() {
        SessionLocator.DynamicLoader.Load('./Social/Components/SocialMainComponent', this.viewContainerRef)
            .then(cmpRef => {
                this.SocialPeopleComponent = cmpRef.instance;

                var table = window.ObjectTables.filter(d => d.Name == this.ObjectTableName)[0];
                if (table) {
                    this.ObjectTableId = table.Id;
                } 

                var postsArgs: PostsArgs = new PostsArgs();
                postsArgs.QueryName = this.QueryName;
                postsArgs.SubQueryName = this.SubQueryName;
                postsArgs.UserId = SessionLocator.LoggedUserId;
                postsArgs.ScreenCode = this.ScreenCode;
                postsArgs.InsideEntity = this.InsideEntity;
                postsArgs.EntityId = this.EntityId;
                postsArgs.ObjectTableId = this.ObjectTableId;
                postsArgs.EntityDescription = this.EntityDescription;
                postsArgs.RegardingEntity = this.RegardingEntity;
                postsArgs.IsEntityMode = this.IsEntityMode;
                postsArgs.HideLeftArea = true;

                this.SocialPeopleComponent.InitializeSocialMainComponent(postsArgs);

            });
    }


    private timerToken: any;
    private Retries: number = 0;
    private SocialPeopleComponent: any;

    RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 100) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }


}


   