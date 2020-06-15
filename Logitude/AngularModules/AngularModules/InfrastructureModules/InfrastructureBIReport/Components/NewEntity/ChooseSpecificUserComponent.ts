import { Component } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { BIReportFolderPM } from '../../../../Infrastructure/EntityPMs/BIReportFolderPM';
import { BIFoldersPermissionPM } from '../../../../Infrastructure/EntityPMs/BIFoldersPermissionPM';
import { UserList } from '../../../../Common/EntityLists/UserList';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';

@Component({
    
    templateUrl: './ChooseSpecificUserComponent.html',
})

export class ChooseSpecificUserComponent {
    UsersItemsSource: UserItem[] = [];
    private args: ChooseUserArgs;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
    }

    public MyFolder: BIReportFolderPM;
    SetWindowArgs(args: ChooseUserArgs) {
        this.args = args;
        this.MyFolder = args.MyFolder;
        this.myUsersList = args.AllUsers;

        this.SaveData(); 
        this.BuildData();
    }

    private savedList: BIFoldersPermissionPM[] = [];
    SaveData() {
        this.MyFolder.PermittedBIFolders.forEach(item => {
            var newItem = new BIFoldersPermissionPM(null);
            newItem.Id = item.Id;
            newItem.UserId = item.UserId;
            newItem.FolderId = item.FolderId;          

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
            var mySharedUser: BIFoldersPermissionPM = this.MyFolder.PermittedBIFolders.filter(d => d.UserId == item.Id)[0];
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
        this.MyFolder.PermittedBIFolders = [];

        this.savedList.forEach(item => {
            this.MyFolder.AddBIFoldersPermission(item);
        });

        this.CurrentSession.CloseCurrentWindow();
    }

    public ValidationErrorsList: string[];
    SaveButtonClicked() {
        this.ValidationErrorsList = [];

        if (this.MyFolder.PermittedBIFolders.length > 0) {
            this.CurrentSession.CloseCurrentWindow();
        }

        else {
            this.ValidationErrorsList.push("Please choose at least one user");
        }
    }
}

export class UserItem extends BaseComponent {
    private userList: UserList;
    private BIFoldersPermission: BIFoldersPermissionPM;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(entity: UserList, BIFoldersPermission: BIFoldersPermissionPM, public fatherCompo: ChooseSpecificUserComponent) {
        super();
        this.userList = entity;
        this.BIFoldersPermission = BIFoldersPermission;

        if (BIFoldersPermission != null) {
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
            var newItem: BIFoldersPermissionPM = new BIFoldersPermissionPM(this.fatherCompo.MyFolder);
            newItem.Tenant = SessionLocator.Tenant;
            newItem.UserId = this.userList.Id;

            if (this.fatherCompo.MyFolder.PermittedBIFolders.indexOf(newItem) == -1) {
                this.fatherCompo.MyFolder.AddBIFoldersPermission(newItem);
            }
        }

        else {
            var index = this.fatherCompo.MyFolder.PermittedBIFolders.indexOf(this.BIFoldersPermission);
            if (index > -1) {
                this.fatherCompo.MyFolder.RemoveBIFoldersPermission(this.BIFoldersPermission);
            }
        }
    }
}

export class ChooseUserArgs {
    public MyFolder: BIReportFolderPM;
    public AllUsers: UserList[];
}
