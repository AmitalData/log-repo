


import { Response} from '@angular/http';
import {UserLoginLogList} from '../../../Common/EntityLists/UserLoginLogList';
import {Component, OnInit}  from '@angular/core';

import {UserExtendedPMService} from '../../../Common/Services/ExtendedPMs/UserExtendedPMService';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {UsersWorkspaceRecentItem} from  './ViewModel/UsersWorkspaceRecentItem';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';


@Component({
    moduleId: module.id,

    selector: 'UserLoginHistory',
    templateUrl: './UserLoginHistoryComponent.html',
    providers: [UserExtendedPMService]
})
export class UserLoginHistoryComponent implements OnInit {
    SelectedUserLoginHistory: UserLoginLogList;
    public UserLoginHistoryLists: UserLoginLogList[];
    Username: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _userExtendedPMService: UserExtendedPMService) {


    }
    
    ngOnInit(


    ) {

    }


    SetDataContext(usersWorkspaceRecentItem: any) {
        this.Username = usersWorkspaceRecentItem.Username;
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
        this.LoadData(usersWorkspaceRecentItem.Id);
    }



    LoadData(userId:string) {
       
        this._userExtendedPMService.GetUserLoginHistory(userId, SessionLocator.Tenant).subscribe((res:any)=> {

            var pmResponse: ServiceResponse = res;

            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    this.UserLoginHistoryLists = myResult;

                }

            }

            this.CurrentSession.CurrentWindow.StopBusyIndicator();




        });
    }


    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }



   
}
