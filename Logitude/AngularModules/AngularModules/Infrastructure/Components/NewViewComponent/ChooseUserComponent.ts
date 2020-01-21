import { Component } from '@angular/core';
import { BaseComponent } from '../../Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../Utilities/SessionLocator';
import { UserList } from '../../../Common/EntityLists/UserList';
import { QueryPM } from '../../EntityPMs/QueryPM';
import { SharedUserQueryPM } from '../../EntityPMs/SharedUserQueryPM';

@Component({
    moduleId: module.id,
    templateUrl: './ChooseUserComponent.html',
})

export class ChooseUserComponent {
    UsersItemsSource: UserItem[] = [];
    private args: ChooseUserArgs;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
    }

    public MyQuery: QueryPM;
    SetWindowArgs(args: ChooseUserArgs) {
        this.args = args;
        this.MyQuery = args.MyQuery;
        this.myUsersList = args.AllUsers;

        this.SaveData(); 
        this.BuildData();
    }

    private savedList: SharedUserQueryPM[] = [];
    SaveData() {
        this.MyQuery.SharedUserQueries.forEach(item => {
            var newItem = new SharedUserQueryPM(null);
            newItem.Id = item.Id;
            newItem.UserId = item.UserId;
            newItem.QueryId = item.QueryId;            
            newItem.QueryCode = item.QueryCode;            

            this.savedList.push(newItem);
        });               
    }

    myUsersList: UserList[];
    private BuildData() {
        var myList: UserList[] = [];
        this.UsersItemsSource = [];

        if (!this.mySearchText) {
            myList = this.myUsersList;
        }
        else {

            myList = this.myUsersList.filter(d => (d.Email && d.Email.toLowerCase().indexOf(this.mySearchText.toLowerCase()) > -1) || (d.EnglishName && d.EnglishName.toLowerCase().indexOf(this.mySearchText.toLowerCase()) > -1));
        }

        myList.forEach((item) => {
            var mySharedUser: SharedUserQueryPM = this.MyQuery.SharedUserQueries.filter(d => d.UserId == item.Id)[0];
            this.UsersItemsSource.push(new UserItem(item, mySharedUser, this));
        });
    }

    private mySearchText: string;
    onSearchTextChangeEvent(searchText) {
        if (!searchText) searchText = "";

        this.mySearchText = searchText;
        this.BuildData();
    }

    CloseButtonClicked() {
        this.MyQuery.SharedUserQueries = [];

        this.savedList.forEach(item => {
            this.MyQuery.AddSharedUserQueryPM(item);
        });

        this.CurrentSession.CloseCurrentWindow();
    }

    public ValidationErrorsList: string[];
    SaveButtonClicked() {
        this.ValidationErrorsList = [];
        
        if (this.MyQuery.SharedUserQueries.length > 0) {
            this.CurrentSession.CloseCurrentWindow();
        }

        else {
            this.ValidationErrorsList.push("Please choose at least one user");
        }
    }
}

export class UserItem extends BaseComponent {
    private userList: UserList;
    private sharedUserQuery: SharedUserQueryPM;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(entity: UserList, mySharedUser: SharedUserQueryPM, public fatherCompo: ChooseUserComponent) {
        super();
        this.userList = entity;
        this.sharedUserQuery = mySharedUser

        if (mySharedUser != null) {
            this.isChecked = true;
        }
    }

    get Email() { return this.userList.Email; }
    get Name() { return this.userList.EnglishName; }
    get Notes() { return this.userList.Notes; }

    private isChecked: boolean;
    public get IsChecked() { return this.isChecked; }
    public set IsChecked(value: boolean) {
        if (this.isChecked != value) {
            this.isChecked = value;

            this.SetIsChecked();
        }
    }

    private SetIsChecked() {
        if (this.IsChecked) {
            var newItem: SharedUserQueryPM = new SharedUserQueryPM(this.fatherCompo.MyQuery);
            newItem.Tenant = SessionLocator.Tenant;
            newItem.UserId = this.userList.Id;

            if (this.fatherCompo.MyQuery.SharedUserQueries.indexOf(newItem) == -1) {
                this.fatherCompo.MyQuery.AddSharedUserQueryPM(newItem);
            }
        }

        else {
            var index = this.fatherCompo.MyQuery.SharedUserQueries.indexOf(this.sharedUserQuery);
            if (index > -1) {
                this.fatherCompo.MyQuery.RemoveSharedUserQueryPM(this.sharedUserQuery);
            }
        }
    }
}

export class ChooseUserArgs {
    public MyQuery: QueryPM;
    public AllUsers: UserList[];
}
