import { Component } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { BIReportFolderPM } from '../../../../Infrastructure/EntityPMs/BIReportFolderPM';
import { BIReportFolderPMService } from '../../../../Infrastructure/Services/StandardPMs/BIReportFolderPMService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { CodeNameClass } from '../../../../Infrastructure/DataContracts/CodeNameClass';
import { UserList } from '../../../../Common/EntityLists/UserList';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { UserListService } from '../../../../Common/Services/StandardLists/UserListService';
import { BIFoldersPermissionPM } from '../../../../Infrastructure/EntityPMs/BIFoldersPermissionPM';
import { ChooseUserArgs } from './ChooseSpecificUserComponent';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';

@Component({
    
    templateUrl: './NewBIReportFolderComponent.html',
})

export class NewBIReportFolderComponent extends BaseComponent {
    public EntityPM: BIReportFolderPM;
    public ValidationErrorsList: string[] = [];
    private myService: BIReportFolderPMService;
    public DataContext: NewBIReportFolderComponent = this;
    public ObjectTableName: string = "BIReportFolder";
    public IsNewQuery = true;
    public IsShareFolderAvailable = false;
    public IsReady:boolean = false;
    private IsNew: boolean = true;
    private FolderId: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();        
        this.myService = new BIReportFolderPMService();
        this.SetUIProperties();
    }

    SetWindowArgs(args: any) {
        this.IsNew = args.IsNew;
        this.FolderId = args.FolderId;
        this.SetEntityPM();
    }

    SetEntityPM() {
        if (this.IsNew) {
            this.EntityPM = new BIReportFolderPM();
            this.EntityPM.Tenant = SessionLocator.Tenant;
            this.EntityPM.CreatedByUserId = SessionLocator.LoggedUserId;
            this.EntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
            this.EntityPM.PermissionForAll = true;
            this.IsReady = true;
            this.SetShareFolderDetails();
        }
        else {
            this.myService.get(this.FolderId).subscribe((myResponse: ServiceResponse) => {
                if (myResponse.HasError) {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }
                else {
                    this.EntityPM = myResponse.Result;
                    this.EntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
                    this.IsReady = true;
                    this.SetShareFolderDetails();
                }
            });
        }
    }

    SetShareFolderDetails() {
        if (!FeatureLocator.HasFeaturePermession("BIReportFolder", "UPDATE")) return;
        this.IsShareFolderAvailable = true;
        this.LoadUsers();
        this.FillShareValuesList();
        this.SetSelectedSharedValue();
    }

    SetUIProperties() {
        
    }

    private myUsersList: UserList[] = [];
    private LoadUsers() {
        var filters: ApiQueryFilters = new ApiQueryFilters();
        filters.SortBy = "EnglishName";
        filters.SortDirection = "Ascending";
        filters.PageIndex = 0;
        filters.PageSize = 1000;
        filters.Tenant = SessionLocator.Tenant;

        var userService: UserListService = new UserListService();
        this.CurrentSession.StartBusyIndicator('Loading...');
        userService.getByFilters(filters).subscribe((res: any) => {
            this.CurrentSession.StopBusyIndicator();
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                this.myUsersList = pmResponse.Result;

                if (!this.IsNew) {
                    //this.ShareWithUsersCount = this.EntityPM.SharedUserQueries.length;
                    //this.SharedByUserName = this.EntityPM.SharedByUserName;
                    //this.SharedByUserEmail = this.EntityPM.SharedByUserEmail;

                    this.FillSharedWithUsersItemsSource();
                    this.SetSelectedSharedValue();
                }
            }
        });
    }

    public IsChooseUsersVisible: boolean = false;
    public ShareValuesList: CodeNameClass[] = [];
    private FillShareValuesList() {
        this.ShareValuesList = [];

        var obj1: CodeNameClass = new CodeNameClass();
        obj1.Code = "ALL";
        obj1.Name = "All Users";

        var obj2: CodeNameClass = new CodeNameClass();
        obj2.Code = "SPF";
        obj2.Name = "Specific Users";

        this.ShareValuesList.push(obj1);
        this.ShareValuesList.push(obj2);
    }
    private SetSelectedSharedValue() {

        if (this.IsNew) {
            this.shareValueSelectedItem = this.ShareValuesList.filter(d => d.Code == "ALL")[0];
        }

        else {
            if (this.EntityPM != null) {

                this.ShareWithUsersCount = this.EntityPM.PermittedBIFolders.length;

                if (this.EntityPM.PermissionForAll) {
                    this.shareValueSelectedItem = this.ShareValuesList.filter(d => d.Code == "ALL")[0];
                }

                else {
                    this.shareValueSelectedItem = this.ShareValuesList.filter(d => d.Code == "SPF")[0];
                    this.IsChooseUsersVisible = true;
                }
            }
        }
    }

    private shareValueSelectedItem: CodeNameClass;
    get ShareValueSelectedItem() { return this.shareValueSelectedItem; }
    set ShareValueSelectedItem(value: CodeNameClass) {
        if (this.shareValueSelectedItem != value) {
            this.shareValueSelectedItem = value;

            if (value.Code == "SPF") {
                this.IsChooseUsersVisible = true;
                this.EntityPM.PermissionForAll = false;
            }
            else {
                this.IsChooseUsersVisible = false;
                this.EntityPM.PermissionForAll = true;
            }
        }
    }

    public ShareWithUsersCount: number = 0;
    public SharedByUserName: string;
    public SharedByUserEmail: string;
    ChooseUsers() {
        var args = new ChooseUserArgs();
        args.MyFolder = this.EntityPM;
        args.AllUsers = this.myUsersList;

        var logWindow = new LogitudeWindow();
        logWindow.Title = "Users List";
        logWindow.Width = 725;
        logWindow.Height = 520;
        logWindow.WindowArgs = args;
        logWindow.Show("./InfrastructureModules/InfrastructureBIReport/Components/NewEntity/ChooseSpecificUserComponent");
        logWindow.WindowClosed.subscribe(($event: any) => {
            this.ShareWithUsersCount = this.EntityPM.PermittedBIFolders.length;
            this.FillSharedWithUsersItemsSource();
        });
    }

    public SharedWithUsersItemsSource: PermittedWithUserItem[] = [];
    FillSharedWithUsersItemsSource() {
        this.SharedWithUsersItemsSource = [];

        this.EntityPM.PermittedBIFolders.forEach((item) => {
            var user: UserList = this.myUsersList.filter(d => d.Id == item.UserId)[0];
            this.SharedWithUsersItemsSource.push(new PermittedWithUserItem(item, user));
        });
    }

    DeleteUser(user: PermittedWithUserItem) {
        var itemIndex = this.SharedWithUsersItemsSource.indexOf(user);
        if (itemIndex > -1) {
            this.SharedWithUsersItemsSource.splice(itemIndex, 1);
        }

        var index = this.EntityPM.PermittedBIFolders.indexOf(user.myEnity);
        if (index > -1) {
            this.EntityPM.RemoveBIFoldersPermission(user.myEnity);
        }

        this.ShareWithUsersCount = this.EntityPM.PermittedBIFolders.length;
    }

    get Name() { return this.EntityPM.Name; }
    set Name(newValue: string) {
        if (this.EntityPM.Name != newValue) {
            this.EntityPM.Name = newValue;
        }
    }

    get Description() { return this.EntityPM.Description; }
    set Description(newValue: string) {
        if (this.EntityPM.Description != newValue) {
            this.EntityPM.Description = newValue;
        }
    }

    get Index() { return this.EntityPM.Index; }
    set Index(newValue: number) {
        if (this.EntityPM.Index != newValue) {
            this.EntityPM.Index = newValue;
        }
    }
    
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        this.ValidationErrorsList = [];
        if (!this.EntityPM.PermissionForAll && this.ShareWithUsersCount == 0) {
            this.ValidationErrorsList.push("Please choose at least one user");
        }
        if (AppTool.IsNullOrEmpty(this.EntityPM.Name)) {
            this.ValidationErrorsList.push("Name Field is Required");
        }
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();

            if (this.ShareValueSelectedItem) {
                switch (this.ShareValueSelectedItem.Code) {
                    case "ALL": {
                        this.EntityPM.PermissionForAll = true;
                        this.EntityPM.PermittedByUserId = SessionLocator.LoggedUserId;

                        if (this.EntityPM.PermittedBIFolders != null && this.EntityPM.PermittedBIFolders.length > 0) {
                            for (var i = this.EntityPM.PermittedBIFolders.length - 1; i >= 0; i--) {
                                var item = this.EntityPM.PermittedBIFolders[i];
                                this.EntityPM.RemoveBIFoldersPermission(item);
                            }
                        }
                        break;
                    }

                    case "SPF": {
                        this.EntityPM.PermissionForAll = false;
                        this.EntityPM.PermittedByUserId = SessionLocator.LoggedUserId;
                        break;
                    }
                }
            }

            if (this.IsNew) {
                this.EntityPM.CreateDate = DateTool.GetCurrentDateAsUtc();
                this.EntityPM.UpdateDate = DateTool.GetCurrentDateAsUtc();
                this.myService.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
                    this.CurrentSession.StopBusyIndicator();
                    if (myResponse.HasError) {
                        this.ValidationErrorsList = myResponse.ErrorsArray;
                    }
                    else {
                        this.CurrentSession.CloseCurrentWindowEmit(this.EntityPM.Id);
                    }
                });
            }
            else {
                this.EntityPM.UpdateDate = DateTool.GetCurrentDateAsUtc();
                this.myService.update(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
                    this.CurrentSession.StopBusyIndicator();
                    if (myResponse.HasError) {
                        this.ValidationErrorsList = myResponse.ErrorsArray;
                    }
                    else {
                        this.CurrentSession.CloseCurrentWindowEmit(this.EntityPM.Name);
                    }
                });
            }
        }
    }
}

export class PermittedWithUserItem {
    public myEnity: BIFoldersPermissionPM;
    private myUser: UserList;
    constructor(entity: BIFoldersPermissionPM, user: UserList) {
        this.myEnity = entity;
        this.myUser = user;
    }

    get Email() { return this.myUser.Email; }
    get Name() { return this.myUser.EnglishName; }
}
