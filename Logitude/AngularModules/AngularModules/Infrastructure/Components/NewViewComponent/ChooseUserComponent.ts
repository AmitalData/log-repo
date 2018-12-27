import { Component, Query } from '@angular/core';
import { BaseComponent } from '../../Components/LogitudeComponents/BaseComponent';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { SessionLocator } from '../../Utilities/SessionLocator';
import { ApiQueryFilters } from '../../DataContracts/ApiQueryFilters';
import { UserListService } from '../../../Common/Services/StandardLists/UserListService';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { UserList } from '../../../Common/EntityLists/UserList';
import { QueryPM } from '../../EntityPMs/QueryPM';
import { SharedUserQueryPM } from '../../EntityPMs/SharedUserQueryPM';

@Component({
    moduleId: module.id,
    templateUrl: './ChooseUserComponent.html',
})

export class ChooseUserComponent {
    UsersItemsSource: UserItem[] = [];
    userService: UserListService;
    mySelectedUsers: UserList[];
    private args: ChooseUserArgs;
    constructor() {
        this.userService = new UserListService();
        this.mySelectedUsers = [];        
    }

    public MyQuery: QueryPM;
    private mySharedUsersQueriesList: SharedUserQueryPM[];
    SetWindowArgs(args: ChooseUserArgs) {
        this.args = args;
        this.MyQuery = args.MyQuery;

        this.LoadUsers();

        if (args.MyQuery != null) {
            this.mySharedUsersQueriesList = args.MyQuery.SharedUserQueries;
        }
    }

    myUsersList: UserList[];
    private LoadUsers() {
        SessionLocator.CurrentSession.StartBusyIndicatorLoading();

        var filters: ApiQueryFilters = new ApiQueryFilters();        
        filters.SortBy = "EnglishName";
        filters.SortDirection = "Ascending";
        filters.PageIndex = 0;
        filters.PageSize = 100;
        filters.Tenant = SessionLocator.Tenant;
        
        filters.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "boolean");

        this.userService.getByFilters(filters).subscribe(res => {           
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                this.myUsersList = pmResponse.Result;

                this.BuildData();
            }

            SessionLocator.CurrentSession.StopBusyIndicator();
        });
    }

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
            var mySharedUser: SharedUserQueryPM = this.mySharedUsersQueriesList.filter(d => d.UserId == item.Id)[0];
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
        SessionLocator.CurrentSession.CloseCurrentWindow();
    }

    public ValidationErrorsList: string[];
    SaveButtonClicked() {
        this.ValidationErrorsList = [];

        if (this.mySelectedUsers.length > 0) {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Show("Are you sure you want to share this view with the selected users?");
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.args.SelectedUsers = this.mySelectedUsers;
                    this.CloseButtonClicked();
                }

                else {
                    this.CloseButtonClicked();
                }
            });

        }
        else {

            this.ValidationErrorsList.push("Please choose at least one user");
        }
    }
       
    AddUser(user: UserList) {
        if (this.mySelectedUsers.filter(d => d.Id == user.Id).length == 0) {
            this.mySelectedUsers.push(user);
        }
    }

    RemoveUser(user: UserList) {
        var item: UserList = this.mySelectedUsers.filter(d => d.Id == user.Id)[0];
        if (item != null) {
            var index = this.mySelectedUsers.indexOf(item);

            if (index > -1) {
                this.mySelectedUsers.splice(index, 1);
            }
        }  
    }
}

export class UserItem extends BaseComponent {
    private userList: UserList;
    private sharedUserQuery: SharedUserQueryPM;
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
        if (this.fatherCompo.MyQuery != null) {
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
        
        else {
            if (this.IsChecked) {
                this.fatherCompo.AddUser(this.userList);
            }
            else {
                this.fatherCompo.RemoveUser(this.userList);
            }
        }        
    }
}

export class ChooseUserArgs {
    public MyQuery: QueryPM;
    public SelectedUsers: UserList[];
}
