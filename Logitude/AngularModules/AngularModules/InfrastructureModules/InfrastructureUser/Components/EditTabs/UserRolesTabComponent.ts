import {Component, OnDestroy}  from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {UserPM} from '../../../../Common/EntityPMs/UserPM';
import {RolePM} from '../../../../Common/EntityPMs/RolePM';
import {UserRolesPM} from '../../../../Common/EntityPMs/UserRolesPM';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {ExcelExportService} from '../../../../Common/Services/Others/ExcelExportService';
import {RoleExtendedPMService} from '../../../../Common/Services/ExtendedPMs/RoleExtendedPMService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {Guid} from '../../../../Infrastructure/Utilities/Guid';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {AppTool} from '../../../../Infrastructure/Tools';
import {ImageParameter} from '../../../../Infrastructure/DataContracts/ImageParameter';
declare var UploadLogoFile, base64ToArrayBuffer, saveByteArray, ArrayBufferToBase64: any;
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    
    templateUrl: './UserRolesTabComponent.html',
})

export class UserRolesTabComponent extends BaseComponent implements OnDestroy {
    public EntityPM: UserPM;
    public ObjectTableName: string = "User";
    public DataContext = this;
    public ObsList: UserRolesItemClass[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.InitializeServices();
        this.SetUIProperties();
        this.LoadUserRoles();
        this.Listen();
    }

    private excelExportService: ExcelExportService;
    private roleExtendedPMService: RoleExtendedPMService;
    InitializeServices() {
        this.excelExportService = new ExcelExportService();
        this.roleExtendedPMService = new RoleExtendedPMService();
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.SetUIProperties();

                        if (this.isEditingRoleRequested) {
                            if (this.editedRole) {
                                this.EditRole(this.editedRole);
                            }
                        }

                        if (this.isEditingCustomRoleRequested) {
                            this.EditCustomRole();
                        }
                    }

                    this.isEditingCustomRoleRequested = false;
                    this.isEditingRoleRequested = false;
                });
            }

            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.SetUIProperties();
                    }

                    this.isEditingRoleRequested = false;
                });
            }
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    public IsEditingEnabled: boolean = false;
    public IsExportButtonVisible: boolean = false;
    public IsNewRoleButtonVisible: boolean = false;
    SetUIProperties() {

        var isEditingEnabled = true;
        if (ObjectsLocator.IsDemoTenant(SessionLocator.Tenant.toString())) {
            if (!SessionLocator.LoggedUserPM.IsCustomerCare) {
                isEditingEnabled = false;
            }

            else if (this.EntityPM.Tenant == 0 && !this.EntityPM.IsDistributor) {
                isEditingEnabled = false;
            }
        }

        else {
            if (SessionLocator.Tenant != 0) {
                if (this.EntityPM.Tenant == 0 && !this.EntityPM.IsDistributor) {
                    isEditingEnabled = false;
                }
            }
        }

        this.IsEditingEnabled = isEditingEnabled;

        if (SessionLocator.Tenant == 0) {
            this.IsExportButtonVisible = true;
        }

        if (FeatureLocator.HasFeaturePermession("User", "User.Feature.CustomRoles") && SessionLocator.Tenant != 0) {
            this.IsNewRoleButtonVisible = true;
        }
    }

    public ImportFeaturesFileHtmlId: string = Guid.NewRandomString();
    ExportFeaturesToFileButtonClicked() {

        this.CurrentSession.StartBusyIndicator("Initializing...");

        this.excelExportService.ExportRoleFeaturesToCSVFile().subscribe((myResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {
                var myResult = myResponse.Result;
                if (myResult) {

                    var defaultname: string = "Role features" + "_" + new Date().toLocaleDateString();
                    var data = base64ToArrayBuffer(myResult);
                    saveByteArray("defaultname", data, ".csv");

                    // this.ShowMessage("Export completed successfully");

                    //                    SaveFileDialog dlg = new SaveFileDialog();
                    //                    dlg.Filter = "Microsoft Excel (*.csv)|*.csv";
                    //                    dlg.DefaultExt = "csv";

                    //                    string defaultname = "Role features" + "_" + DateTime.Now.ToShortDateString();
                    //                    dlg.DefaultFileName = defaultname.Replace("/", "-");

                    //                    if ((bool)dlg.ShowDialog())
                    //        {

                    //            Stream fs = (Stream)dlg.OpenFile();
                    //            fs.Write(fileBytes, 0, fileBytes.Length);
                    //            fs.Close();
                    //        }

                    //                else
                    //                {
                    //    return;

                }
            }
        });
    }
    ImportFeaturesToFileuttonClicked() {
        document.getElementById(this.ImportFeaturesFileHtmlId).click();
    }
    ImportFeaturesFile(event: any) {
        var file: any = UploadLogoFile(this.ImportFeaturesFileHtmlId);
        //   && (file.type == "image/png" || file.type == "image/Png")
        if (file && file.name && file.name.toLowerCase().indexOf("csv") != -1) {
            this.ArrayBufferToBase64(file, this);
        }
    }

    ImportFeatures(data: any) {
        var service: ExcelExportService = new ExcelExportService();
        var file: ImageParameter = new ImageParameter();
        file.Base64String = data;

        service.ImportRoleFeatures(file).subscribe((res:any) => {
            this.CurrentSession.StopBusyIndicator();

            var wind = new MessageWindow();
            wind.Show("Import completed successfully");
        });
    }

    ArrayBufferToBase64(file: any, viewmode: any) {
        if (file) {
            var reader: FileReader = new FileReader();

            var reader = new FileReader();
            reader.onload = function (e) {
                var binary = '';
                var result = ArrayBufferToBase64(e);
                var bytes = new Uint8Array(result);
                var len = bytes.byteLength;
                for (var i = 0; i < len; i++) {
                    binary += String.fromCharCode(bytes[i]);
                }

                viewmode.ImportFeatures(window.btoa(binary));

            };

            reader.onerror = function (e) {
                SessionLocator.SelectedSession.StopBusyIndicator();

                var wind = new MessageWindow();
                wind.Show("Error Importing file");
            };

            reader.readAsArrayBuffer(file);
        }
    }

    private allRoles: RolePM[] = [];
    LoadUserRoles(StartBusyIndicator: boolean = true) {
        if (StartBusyIndicator) {
            this.CurrentSession.StartBusyIndicatorLoading();
        }

        this.roleExtendedPMService.GetRolesForUser(this.EntityPM.Id, SessionLocator.Tenant).subscribe((myResponse: ServiceResponse) => {            
            this.CurrentSession.StopBusyIndicator();

            if (!myResponse.HasError) {
                this.allRoles = myResponse.Result;
                this.BuildItemsSource();
            }
        });
    }
    private BuildItemsSource() {
        this.ObsList = [];

        if (this.allRoles != null) {
            this.allRoles.filter(f => f.Exists == true).forEach(item => {
                this.AddRoleItem(item);
            });

            this.allRoles.filter(f => f.Exists == false).forEach(item => {
                this.AddRoleItem(item);
            });

            if (!this.ShowInactiveRoles) {
                this.ObsList = this.ObsList.filter(f => !f.Inactive);
            }
        }
    }

    AddRoleItem(item: RolePM) {
        switch (item.Code) {
            case "DIST":
            case "CUCA":
                {
                    if (SessionLocator.Tenant == 0) {
                        this.ObsList.push(new UserRolesItemClass(item, this.EntityPM, this));
                    }

                    break;
                }

            case "HRAD":
                {
                    if (SessionLocator.Tenant == 0 || SessionLocator.Tenant == 1489 || FeatureLocator.IsPackage_DVMT()) {
                        this.ObsList.push(new UserRolesItemClass(item, this.EntityPM, this));
                    }

                    break;
                }

            case "BILL":
                {
                    if (ObjectsLocator.GlobalSetting &&
                        (  ObjectsLocator.GlobalSetting?.DeploymentStage == "Dev"
                        || ObjectsLocator.GlobalSetting?.DeploymentStage == "Test2"
                        || ObjectsLocator.GlobalSetting?.DeploymentStage == "Simplog")) {
                        this.ObsList.push(new UserRolesItemClass(item, this.EntityPM, this));
                    }

                    break;
                }

            default:
                {
                    if (item.IsCustomRole) {
                        if (item.Tenant == SessionLocator.Tenant) {
                            this.ObsList.push(new UserRolesItemClass(item, this.EntityPM, this));
                        }
                    }

                    else {
                        this.ObsList.push(new UserRolesItemClass(item, this.EntityPM, this));
                    }

                    break;
                }
        }
    }

    private showInactiveRoles: boolean = false;
    public get ShowInactiveRoles() { return this.showInactiveRoles; }
    public set ShowInactiveRoles(value: boolean) {
        if (this.showInactiveRoles != value) {
            this.showInactiveRoles = value;
            this.BuildItemsSource();
        }
    }

    private editedRole: RolePM;
    private editedCustomRole: UserRolesItemClass;
    private isEditingRoleRequested: boolean = false;
    private isEditingCustomRoleRequested: boolean = false;
    NewRoleButtonClicked() {
        var myCustomRolePM = new RolePM();
        myCustomRolePM.Added = true;
        myCustomRolePM.Exists = true;
        myCustomRolePM.Removed = false;
        myCustomRolePM.UserId = this.EntityPM.Id;
        myCustomRolePM.Tenant = SessionLocator.Tenant;
        myCustomRolePM.IsCustomRole = true;

        var logWindow = new LogitudeWindow();
        logWindow.Title = "New Custom Role";
        logWindow.WindowArgs = { RolePM: myCustomRolePM, IsNew: true };
        logWindow.Show('./InfrastructureModules/InfrastructureUser/Components/Roles/NewRoleComponent');

        logWindow.ComponentLoaded.subscribe(comp => {
            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.LoadUserRoles(false);
                    this.EditRole(comp.EntityPM);
                }
            });
        });
    }
    EditRolePropertiesClicked(item: UserRolesItemClass) {
        this.isEditingCustomRoleRequested = true;
        this.editedCustomRole = item;
        this.CurrentSession.CurrentEditComponent.SaveChanges();
    }
    private EditCustomRole() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Edit Custom Role";
        logWindow.WindowArgs = { RolePM: this.editedCustomRole.EntityPM, IsNew: false };
        logWindow.Show('./InfrastructureModules/InfrastructureUser/Components/Roles/NewRoleComponent');

        logWindow.ComponentLoaded.subscribe(comp => {
            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.LoadUserRoles(false);
                }
            });
        });
    }

    EditRoleButtonClicked(item: UserRolesItemClass) {
        this.editedRole = item.EntityPM;
        this.isEditingRoleRequested = true;
        this.CurrentSession.CurrentEditComponent.SaveChanges();
    }
    EditRole(myRole: RolePM) {
        var logWindow = new LogitudeWindow();
        logWindow.IsFillScreen = true;
        logWindow.Title = "Edit " + myRole.Name + " Role";
        logWindow.WindowArgs = { RolePM: myRole };
        logWindow.Show('./InfrastructureModules/InfrastructureUser/Components/Roles/EditRoleFeaturesComponent');
    }
    UpdateRoles() {
        this.EntityPM.UserRolesNamesList = "";
        this.ObsList.filter(a => a.IsActive).forEach(item => {
            this.EntityPM.UserRolesNamesList += item.Name + ",";
        });
    }
    test() {
        if (AppTool.IsNullOrEmpty(this.EntityPM.UserRolesNamesList_db)) {
            this.EntityPM.UserRolesNamesList_db = "";
            this.ObsList.filter(a => a.IsActive).forEach(item => {
                this.EntityPM.UserRolesNamesList_db += item.Name + ",";
            });
        }
    }
}
export class UserRolesItemClass {
    public EntityPM: RolePM;
    public UserPM: UserPM;
    private isNewUserMode: boolean = false;
    constructor(entityPM: RolePM, private myUserPM: UserPM, public father: any) {
        this.EntityPM = entityPM;
        this.UserPM = myUserPM;
        this.isActive = entityPM.Exists;

        if (myUserPM == null) {
            this.isNewUserMode = true;
        }

        else if (myUserPM.Id == null) {
            this.isNewUserMode = true;
        }
    }

    public get Inactive() { return this.EntityPM.Inactive; }
    public get Name() { return this.EntityPM.Name; }
    public get Description() { return this.EntityPM.Description; }

    public get Code() { return this.EntityPM.Code; }
    public get Id() { return this.EntityPM.Id; }
    public get IsCustomRole() { return this.EntityPM.IsCustomRole; }

    private isActive: boolean = false;
    public get IsActive() { return this.isActive; }
    public set IsActive(value: boolean) {  

        var isUserDirty: boolean = this.UserPM.IsDirty;

        if (!this.isNewUserMode) {
            this.father.test();
        }

        if (this.isActive != value) {
            this.isActive = value;


            this.UserPM.IsDirty = true;
            this.EntityPM.UserId = this.UserPM.Id;

            if (!this.UserPM.RolePMLists) {
                this.UserPM.RolePMLists = [];
            }

            var index = this.UserPM.RolePMLists.indexOf(this.EntityPM);
            if (index > -1) {
                this.UserPM.RolePMLists.splice(index, 1);
            }

            if (this.isNewUserMode) {
                this.EntityPM.UserId = null;
            }

            if (value) {
                if (this.EntityPM.IsCustomRole) {
                    var confirmWindow = new ConfirmWindow();
                    confirmWindow.Show("The user who will be assigned this role will need to logout and login so the changes will take place");
                    confirmWindow.WindowClosed.subscribe((event: any) => {
                        if (confirmWindow.Yes) {
                            this.EntityPM.Exists = true;
                            this.EntityPM.Added = true;
                            this.EntityPM.Removed = false;

                            if (this.isNewUserMode) {
                                var userRole = new UserRolesPM(null);
                                userRole.Added = this.EntityPM.Added;
                                userRole.Exists = this.EntityPM.Exists;
                                userRole.Id = this.EntityPM.Id;
                                userRole.Name = this.EntityPM.Name;
                                userRole.Removed = this.EntityPM.Removed;
                                userRole.Tenant = this.EntityPM.Tenant;

                                this.UserPM.AddUserRolesPM(userRole);
                            }
                        }

                        else {
                            this.isActive = false;

                            if (!isUserDirty) {
                                this.UserPM.IsDirty = false;
                            }
                        }
                    });
                }

                else {
                    this.EntityPM.Exists = true;
                    this.EntityPM.Added = true;
                    this.EntityPM.Removed = false;

                    if (this.isNewUserMode) {
                        var userRole = new UserRolesPM(null);
                        userRole.Added = this.EntityPM.Added;
                        userRole.Exists = this.EntityPM.Exists;
                        userRole.Id = this.EntityPM.Id;
                        userRole.Name = this.EntityPM.Name;
                        userRole.Removed = this.EntityPM.Removed;
                        userRole.Tenant = this.EntityPM.Tenant;

                        this.UserPM.AddUserRolesPM(userRole);
                    }
                }
            }

            else {
                this.EntityPM.Exists = false;
                this.EntityPM.Added = false;
                this.EntityPM.Removed = true;
             
                if (this.isNewUserMode) {
                    var userRole: UserRolesPM = this.UserPM.Roles.filter(r => r.Id == this.EntityPM.Id)[0];
                    if (userRole) {
                        this.UserPM.RemoveUserRolesPM(userRole);
                    }
                }
            }

            this.UserPM.RolePMLists.push(this.EntityPM);

            if (!this.isNewUserMode) {
                this.father.UpdateRoles();
            }
        }
    }
}
