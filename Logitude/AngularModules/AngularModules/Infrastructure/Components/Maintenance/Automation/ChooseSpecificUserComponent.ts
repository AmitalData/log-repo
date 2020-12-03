import { Component } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { UserList } from '../../../../Common/EntityLists/UserList';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { AppTool } from '../../../Tools';

@Component({
    
    templateUrl: './ChooseSpecificUserComponent.html',
})

export class ChooseSpecificUserComponent {
    UsersItemsSource: UserItem[] = [];
    private args: ChooseUserArgs;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
    }

    public MyChoosenSpecificUsers: string;
    SetWindowArgs(args: ChooseUserArgs) {
        this.args = args;
        this.MyChoosenSpecificUsers = args.MyChoosenSpecificUsers == null ? "" : args.MyChoosenSpecificUsers;
        this.myUsersList = args.AllUsers;

        this.SaveData(); 
        this.BuildData();
    }

    private savedMyChoosenSpecificUsersList: string;
    SaveData() {
        this.savedMyChoosenSpecificUsersList = this.MyChoosenSpecificUsers;             
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
            var myChoosenUser: string = this.MyChoosenSpecificUsers.split(";").filter(d => d == item.Id)[0];
            this.UsersItemsSource.push(new UserItem(item, myChoosenUser, this));
        });
    }

    private mySearchText: string;
    onSearchTextChangeEvent(searchText) {
        if (!searchText) searchText = "";

        this.mySearchText = searchText;
        this.BuildData();
    }

    CloseButtonClicked() {
        this.MyChoosenSpecificUsers = "";
        this.MyChoosenSpecificUsers = this.savedMyChoosenSpecificUsersList;

        this.CurrentSession.CloseCurrentWindowEmit(this.MyChoosenSpecificUsers);
    }

    public ValidationErrorsList: string[];
    SaveButtonClicked() {
        this.ValidationErrorsList = [];

        //if (!AppTool.IsNullOrEmpty(this.MyChoosenSpecificUsers)) {
            this.CurrentSession.CloseCurrentWindowEmit(this.MyChoosenSpecificUsers);
        //}

        //else {
        //    this.ValidationErrorsList.push("Please choose at least one user");
        //}
    }
}

export class UserItem extends BaseComponent {
    private userList: UserList;
    private MyChoosenUser: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(entity: UserList, myChoosenUser: string, public fatherCompo: ChooseSpecificUserComponent) {
        super();
        this.userList = entity;

        this.MyChoosenUser = myChoosenUser;

        if (!AppTool.IsNullOrEmpty(this.MyChoosenUser)) {
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
            if (this.fatherCompo.MyChoosenSpecificUsers.indexOf(this.userList.Id) == -1) {
                this.fatherCompo.MyChoosenSpecificUsers += ";" + this.userList.Id;
            }
        }

        else {
            var index = this.fatherCompo.MyChoosenSpecificUsers.indexOf(this.userList.Id);
            if (index > -1) {
                var MyChoosenSpecificUsersSplit = this.fatherCompo.MyChoosenSpecificUsers.split(";" + this.userList.Id);
                this.fatherCompo.MyChoosenSpecificUsers = MyChoosenSpecificUsersSplit[0] + MyChoosenSpecificUsersSplit[1];
            }
        }
    }
}

export class ChooseUserArgs {
    public MyChoosenSpecificUsers: string;
    public AllUsers: UserList[];
}
