import {Component, OnInit, Output, EventEmitter, ChangeDetectorRef}  from '@angular/core';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';

@Component({
    moduleId: module.id,
    selector: 'SocialContactNameLink',
    templateUrl: './SocialContactNameLink.html',

})

export class SocialContactNameLink implements OnInit {
    UserName: string = "";
    rowData: any;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private cd: ChangeDetectorRef) {

    }

    ngOnInit(

    ) {



    }




    setVariables(rowData: any, fieldName: string) {
        if (rowData != null) {
            this.rowData = rowData;
            this.UserName = rowData.EnglishName;
            this.Destroyed();
        }
    }



    ViewPostUserFeedsButtonClick(item: any) {

        //this.IsChecked = !this.IsChecked;

        var data: any[] = [];
        data.push("SocialContactLinkEvent");
        data.push(item);
        this.Destroyed();
        this.CurrentSession.SessionEvent.emit(data);
      
    }
 

    Destroyed() {
        var isDestroyed: boolean = this.cd['destroyed'];
        if (!isDestroyed) {
            this.cd.detectChanges();
        }
    }









}
