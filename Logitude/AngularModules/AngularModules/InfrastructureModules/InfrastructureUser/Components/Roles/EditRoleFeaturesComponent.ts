import {Component} from '@angular/core';
import {AppTool} from '../../../../Infrastructure/Tools';
import {RolePM} from '../../../../Common/EntityPMs/RolePM';
import {FeaturePM} from '../../../../Infrastructure/EntityPMs/FeaturePM';
import {ObjectTablePM} from '../../../../Infrastructure/EntityPMs/ObjectTablePM';
import {RolePMService} from '../../../../Common/Services/StandardPMs/RolePMService';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {InfrastructureDomainService, FeaturesUpdateHelper} from '../../../../Infrastructure/Services/InfrastructureDomainService';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
declare var window: any;

@Component({
    
    templateUrl: './EditRoleFeaturesComponent.html',
})

export class EditRoleFeaturesComponent {
    public EntityPM: RolePM;
    public DataContext = this;
    public ObjectTableName: string = "Role";
    public ValidationErrorsList: string[] = [];
    public IsResourcesReady: boolean = false;
    public ItemsSource1: TableRoleFeatureClass[] = [];
    public ItemsSource2: TableRoleFeatureClass[] = [];
    public ItemsSource1Hidden: boolean = false;
    public ItemsSource2Hidden: boolean = false;
    public MenusList: RoleFeatureClass[] = [];
    public OthersList: RoleFeatureClass[] = [];
    public IsCustomRole: boolean = false;
    public IsEventsButtonVisible: boolean = false;
    private myDomainService: InfrastructureDomainService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService) {
        this.myDomainService = new InfrastructureDomainService();

        if (FeatureLocator.HasFeaturePermession("General", "FeaturesChanges")) {
            this.IsEventsButtonVisible = true;
        }
    }

    SetWindowArgs(args: any) {
        this.EntityPM = args['RolePM'];
        this.IsCustomRole = this.EntityPM.IsCustomRole;

        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((res: any) => {
            this.IsResourcesReady = true;
            this.SetUIProperties();
            this.LoadFeatures();
        });
    }

    public IsEditingEnabled: boolean = false;
    public IsOkButtonVisible: boolean = false;
    public CancelButtonTextCode: string = "General.B.Close";
    public IsShowNewFeaturesButtonVisible: boolean = false;
    SetUIProperties() {
        if (SessionLocator.Tenant == 0) {
            this.IsEditingEnabled = true;
            this.IsShowNewFeaturesButtonVisible = true;
        }

        else if (!AppTool.IsNullOrEmpty(this.EntityPM.ParentRoleId)) {
            if (this.EntityPM.Tenant == SessionLocator.Tenant) {
                this.IsEditingEnabled = true;
            }
        }

        if (SessionLocator.Tenant == 0 || this.IsCustomRole) {
            this.IsOkButtonVisible = true;
            this.CancelButtonTextCode = "General.B.Cancel";
        }
    }

    private mySearchText: string = null;
    SearchTextChanged(text: string) {
        this.mySearchText = text;
        this.BuildCollections();
    }

    private allFeatures: FeaturePM[] = [];
    private allFeaturesItems: RoleFeatureClass[] = [];
    private LoadFeatures() {
        this.CurrentSession.StartBusyIndicatorLoading();

        this.myDomainService.GetSelectedAndUnselectedRoleFeatures(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {

            if (myResponse.HasError) {
                this.BuildCollections();
                this.CurrentSession.StopBusyIndicator();
            }

            var loadedFeatures: FeaturePM[] = myResponse.Result;
            if (SessionLocator.Tenant == 0) {
                this.allFeatures = loadedFeatures;
                this.Build();
                return;
            }

            if (!this.IsCustomRole) {
                this.allFeatures = loadedFeatures;
                this.Build();
                return;
            }

            this.myDomainService.GetAllowedFeaturesForRole(this.EntityPM.ParentRoleId).subscribe(roleFeatures => {
                if (roleFeatures.HasError) {
                    this.Build();
                    return;
                }
                var parentRoleFeaturesDectionary = this.GetRoleFeaturesDectionary(roleFeatures.Result);
                loadedFeatures.forEach(item => {
                    if (parentRoleFeaturesDectionary[this.GetFeatureDictionaryKey(item)]) {
                        this.allFeatures.push(item);
                    }
                });
                this.Build();
            });
        });
    }
    Build() {
        
        this.CurrentSession.StopBusyIndicator();
        this.allFeatures.forEach(itemFeature => {
            this.allFeaturesItems.push(new RoleFeatureClass(itemFeature, this));
        });
        this.BuildCollections();
    }

    GetRoleFeaturesDectionary(roleFeatures: Array<FeaturePM>) {
        var dictionary:{[key:string]:FeaturePM} = {};
        roleFeatures.forEach(features => {
            if(!dictionary[this.GetFeatureDictionaryKey(features)]){
                dictionary[this.GetFeatureDictionaryKey(features)] = features;
            }
        });
        return dictionary;
    }
    GetFeatureDictionaryKey(features: FeaturePM) {
        return features.ObjectTableId + features.Code.toLowerCase();
    }
    private BuildCollections() {
        this.BuildTablesLists();
        this.BuildMenusList();
        this.BuildOthersList();
    }
    private BuildTablesLists() {

        var items: ObjectTablePM[] = window.ObjectTables.filter(d => d.IsMain == true && d.EnableSecurity == true && d.IsClosed == false && d.IsComposition == false);

        if (!AppTool.IsNullOrEmpty(this.mySearchText)) {
            items = items.filter(f => f.Name != null && f.Name.toLowerCase().indexOf(this.mySearchText.toLowerCase()) > -1);
        }

        items = items.sort(function (a, b) { return a.Name.toLowerCase() == b.Name.toLowerCase() ? 0 : a.Name.toLowerCase() < b.Name.toLowerCase() ? -1 : 1; });

        var allTablesItems: TableRoleFeatureClass[] = [];

        items.forEach(item => {
            if (allTablesItems.filter(f => f.ObjectTableId == item.Id).length == 0) {
                if (this.allFeatures.filter(d => d.ObjectTableId == item.Id && d.FeatureTypeCode == "MODL").length > 0) {
                    allTablesItems.push(new TableRoleFeatureClass(item, this.allFeatures.filter(d => d.ObjectTableId == item.Id), this));
                }
            }
        });

        this.ItemsSource1 = allTablesItems.filter(f => f.ObjectTableTypeCode != "MD");
        this.ItemsSource2 = allTablesItems.filter(f => f.ObjectTableTypeCode == "MD");
    }
    private BuildMenusList() {

        var items: RoleFeatureClass[] = [];
        items = this.allFeaturesItems.filter(d => d.FeatureTypeCode.toUpperCase() == "MENU");
        items = items.sort(function (a, b) { return a.Code.toLowerCase() == b.Code.toLowerCase() ? 0 : a.Code.toLowerCase() < b.Code.toLowerCase() ? -1 : 1; });

        if (!AppTool.IsNullOrEmpty(this.mySearchText)) {
            items = items.filter(f => f.Name != null && f.Name.toLowerCase().indexOf(this.mySearchText.toLowerCase()) > -1);
        }

        this.MenusList = items;
    }
    private BuildOthersList() {

        var items: RoleFeatureClass[] = [];
        items = this.allFeaturesItems.filter(d => d.FeatureTypeCode == "OTH");
        items = items.sort(function (a, b) { return a.Code.toLowerCase() == b.Code.toLowerCase() ? 0 : a.Code.toLowerCase() < b.Code.toLowerCase() ? -1 : 1; });

        if (!AppTool.IsNullOrEmpty(this.mySearchText)) {
            items = items.filter(f => f.Name != null && f.Name.toLowerCase().indexOf(this.mySearchText.toLowerCase()) > -1);
        }

        this.OthersList = items;
    }

    ShowNewFeaturesClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "New Features";
        logWindow.Show('./InfrastructureModules/InfrastructureUser/Components/Roles/ShowNewFeaturesComponent');
    }
    ShowEventsClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Role Events";
        logWindow.WindowArgs = this.EntityPM.Id;
        logWindow.Show('./InfrastructureModules/InfrastructureUser/Components/Roles/RoleFeaturesEventsComponent');
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {    
        var items: FeaturePM[] = this.allFeatures.filter(f => f.IsDirty == true);

        if (items.length == 0) {
            this.CurrentSession.CloseCurrentWindow();
        }

        else {
            if (this.IsCustomRole) {
                var confirmWindow = new ConfirmWindow();
                confirmWindow.Show("The user who will be assigned this role will need to logout and login so the changes will take place");
                confirmWindow.WindowClosed.subscribe((event: any) => {
                    if (confirmWindow.Yes) {
                        this.Save(items);
                    }
                });
            }

            else {
                this.Save(items);
            }
        }
    }
    Save(items: FeaturePM[]) {
        this.CurrentSession.StartBusyIndicatorSaving();

        var myServiceHelper = new FeaturesUpdateHelper();
        myServiceHelper.Tenant = SessionLocator.Tenant;
        myServiceHelper.RoleId = this.EntityPM.Id;
        myServiceHelper.Items = items;

        this.myDomainService.UpdateFeatures(myServiceHelper).subscribe((myResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();

            if (myResponse.HasError) {
                this.ValidationErrorsList = myResponse.ErrorsArray;
                this.CurrentSession.StopBusyIndicator();
            }

            else {
                this.myDomainService.GetAllowedFeaturesForLoggedUser().subscribe((myResponse1: ServiceResponse) => {

                    this.CurrentSession.StopBusyIndicator();

                    if (myResponse1.HasError) {
                        this.ValidationErrorsList = myResponse1.ErrorsArray;
                    }

                    else {
                        this.CurrentSession.CloseCurrentWindowEmit("Ok");
                    }
                });
            }
        });
    }
}
export class RoleFeatureClass {
    public Feature: FeaturePM;
    public Code: string = null;
    public Name: string = null;
    public FeatureTypeCode: string = null;
    public IsEditingEnabled: boolean = false;
    public IsCustomRoleFeature: boolean = false;
    private OldAccessLevelCode: string = "NO";
    constructor(entityPM: FeaturePM, private fatherComponent: EditRoleFeaturesComponent) {
        this.Feature = entityPM;
        this.Code = entityPM.Code;
        this.Name = entityPM.TranslatedName;
        this.FeatureTypeCode = entityPM.FeatureTypeCode.toUpperCase();
        this.IsEditingEnabled = fatherComponent.IsEditingEnabled;
        this.IsCustomRoleFeature = entityPM.IsCustomRoleFeature;

        if (this.Feature.AccessLevelCode) {
            this.OldAccessLevelCode = this.Feature.AccessLevelCode;
        }
    }

    public get AccessLevelCode() { return this.Feature.AccessLevelCode; }
    public set AccessLevelCode(value: string) {
        if (this.Feature.AccessLevelCode != value) {
            this.Feature.AccessLevelCode = value;
            this.Feature.RoleId = this.fatherComponent.EntityPM.Id;

            if (value == this.OldAccessLevelCode) {
                this.Feature.IsDirty = false;
            }
        }
    }
}
export class TableRoleFeatureClass {
    public Name: string = null;
    public ObjectTableId: string = null;
    public ObjectTableTypeCode: string = null;
    public ObjectTableTypeName: string = null;
    public ModuleFeature: FeaturePM = null;
    public ReadFeature: FeaturePM = null;
    public UpdateFeature: FeaturePM = null;
    public AddNewFeature: FeaturePM = null;
    public AreasFeatures: FeaturePM[] = [];
    public ActionsFeatures: FeaturePM[] = [];
    public QueriesFeatures: FeaturePM[] = [];
    public IsCustomRoleFeature_Read: boolean = false;
    public IsCustomRoleFeature_Update: boolean = false;
    public IsCustomRoleFeature_AddNew: boolean = false;
    private OldModuleFeatureAccessLevelCode: string = "NO";
    private OldReadFeatureAccessLevelCode: string = "NO";
    private OldUpdateFeatureAccessLevelCode: string = "NO";
    private OldAddNewFeatureAccessLevelCode: string = "NO";
    constructor(objectTablePM: ObjectTablePM, myFeatures: FeaturePM[], private fatherComponent: EditRoleFeaturesComponent) {
        this.Name = TextCodeTranslator.Translate(objectTablePM.Name);
        this.ObjectTableId = objectTablePM.Id;
        this.ObjectTableTypeCode = objectTablePM.ObjectTableTypeCode;
        this.ObjectTableTypeName = this.ObjectTableTypeCode == "MD" ? "Master Data" : "Buisness Records";

        this.ModuleFeature = myFeatures.filter(d => d.FeatureTypeCode.toUpperCase() == "MODL")[0];
        this.ReadFeature = myFeatures.filter(d => d.FeatureTypeCode.toUpperCase() == "READ")[0];
        this.UpdateFeature = myFeatures.filter(d => d.FeatureTypeCode.toUpperCase() == "UPDT")[0];
        this.AddNewFeature = myFeatures.filter(d => d.FeatureTypeCode.toUpperCase() == "NEW")[0];
        this.AreasFeatures = myFeatures.filter(d => d.FeatureTypeCode.toUpperCase() == "AREA");
        this.ActionsFeatures = myFeatures.filter(d => d.FeatureTypeCode.toUpperCase() == "ACT");
        this.QueriesFeatures = myFeatures.filter(d => d.FeatureTypeCode.toUpperCase() == "QUER");

        if (this.ModuleFeature) {
            if (this.ModuleFeature.AccessLevelCode) {
                this.OldModuleFeatureAccessLevelCode = this.ModuleFeature.AccessLevelCode;
            }
        }

        if (this.ReadFeature) {
            this.IsCustomRoleFeature_Read = this.ReadFeature.IsCustomRoleFeature;

            if (this.ReadFeature.AccessLevelCode) {
                this.OldReadFeatureAccessLevelCode = this.ReadFeature.AccessLevelCode;
            }
        }

        if (this.UpdateFeature) {
            this.IsCustomRoleFeature_Update = this.UpdateFeature.IsCustomRoleFeature;

            if (this.UpdateFeature.AccessLevelCode) {
                this.OldUpdateFeatureAccessLevelCode = this.UpdateFeature.AccessLevelCode;
            }
        }

        if (this.AddNewFeature) {
            this.IsCustomRoleFeature_AddNew = this.AddNewFeature.IsCustomRoleFeature;

            if (this.AddNewFeature.AccessLevelCode) {
                this.OldAddNewFeatureAccessLevelCode = this.AddNewFeature.AccessLevelCode;
            }
        }

        this.SetHyperlinks();
        this.SetUIProperties();
    }

    public AreasLinkText: string;
    public ActionsLinkText: string;
    public QueriesLinkText: string;
    public AreasLinkColor: string;
    public ActionsLinkColor: string;
    public QueriesLinkColor: string;
    SetHyperlinks() {
        this.AreasLinkText = this.AreasFeatures.filter(d => d.AccessLevelCode == "OR").length + "/" + this.AreasFeatures.length;
        this.ActionsLinkText = this.ActionsFeatures.filter(d => d.AccessLevelCode == "OR").length + "/" + this.ActionsFeatures.length;
        this.QueriesLinkText = this.QueriesFeatures.filter(d => d.AccessLevelCode == "OR").length + "/" + this.QueriesFeatures.length;
        this.AreasLinkColor = this.AreasFeatures.filter(d => d.AccessLevelCode != "OR").length > 0 ? "#1E4AC4" : "#009161";
        this.ActionsLinkColor = this.ActionsFeatures.filter(d => d.AccessLevelCode != "OR").length > 0 ? "#1E4AC4" : "#009161";
        this.QueriesLinkColor = this.QueriesFeatures.filter(d => d.AccessLevelCode != "OR").length > 0 ? "#1E4AC4" : "#009161";
    }

    public IsEditingEnabled: boolean = false;
    public IsUpdateFeatureEnabled: boolean = false;
    public IsAddNewFeatureEnabled: boolean = false;
    SetUIProperties() {
        this.IsEditingEnabled = this.fatherComponent.IsEditingEnabled;

        var isUpdateFeatureEnabled = true;
        var isAddNewFeatureEnabled = true;

        if (!this.IsEditingEnabled) {
            isUpdateFeatureEnabled = false;
            isAddNewFeatureEnabled = false;
        }

        else {
            if (this.ReadFeature == null) {
                isUpdateFeatureEnabled = false;
                isAddNewFeatureEnabled = false;
            }

            else if (this.ReadFeature.AccessLevelCode == "NO" || AppTool.IsNullOrEmpty(this.ReadFeature.AccessLevelCode)) {
                isUpdateFeatureEnabled = false;
                isAddNewFeatureEnabled = false;
            }

            else {
                if (this.AddNewFeature == null) {
                    isUpdateFeatureEnabled = false;
                }

                else if (this.AddNewFeature.AccessLevelCode != "NO" && !AppTool.IsNullOrEmpty(this.AddNewFeature.AccessLevelCode)) {
                    isUpdateFeatureEnabled = false;
                }
            }
        }

        this.IsUpdateFeatureEnabled = isUpdateFeatureEnabled;
        this.IsAddNewFeatureEnabled = isAddNewFeatureEnabled;
    }

    // Access Level
    public get AccessLevelCode_Read() { return this.ReadFeature == null ? null : this.ReadFeature.AccessLevelCode; }
    public set AccessLevelCode_Read(value: string) {
        if (this.ReadFeature) {
            if (this.ReadFeature.AccessLevelCode != value) {
                this.ReadFeature.AccessLevelCode = value;
                this.ReadFeature.RoleId = this.fatherComponent.EntityPM.Id;
                this.SetUIProperties();

                if (value == this.OldReadFeatureAccessLevelCode) {
                    this.ReadFeature.IsDirty = false;
                }
            }
        }
    }

    public get AccessLevelCode_Update() { return this.UpdateFeature == null ? null : this.UpdateFeature.AccessLevelCode; }
    public set AccessLevelCode_Update(value: string) {
        if (this.UpdateFeature) {
            if (this.UpdateFeature.AccessLevelCode != value) {
                this.UpdateFeature.AccessLevelCode = value;
                this.UpdateFeature.RoleId = this.fatherComponent.EntityPM.Id;

                if (value == this.OldUpdateFeatureAccessLevelCode) {
                    this.UpdateFeature.IsDirty = false;
                }
            }
        }
    }

    public get AccessLevelCode_AddNew() { return this.AddNewFeature == null ? null : this.AddNewFeature.AccessLevelCode; }
    public set AccessLevelCode_AddNew(value: string) {
        if (this.AddNewFeature) {
            if (this.AddNewFeature.AccessLevelCode != value) {
                this.AddNewFeature.AccessLevelCode = value;
                this.AddNewFeature.RoleId = this.fatherComponent.EntityPM.Id;

                if (value != "NO" && !AppTool.IsNullOrEmpty(value)) {
                    this.AccessLevelCode_Update = value;
                }

                this.SetUIProperties();

                if (value == this.OldAddNewFeatureAccessLevelCode) {
                    this.AddNewFeature.IsDirty = false;
                }
            }
        }
    }

    HyperlinkClicked(typeCode: string) {
        var myFeatures: FeaturePM[] = [];
        var myFeaturesItems: RoleFeatureClass[] = [];
        switch (typeCode) {
            case "Areas": { myFeatures = this.AreasFeatures; break; }
            case "Actions": { myFeatures = this.ActionsFeatures; break; }
            case "Queries": { myFeatures = this.QueriesFeatures; break; }
        }

        myFeatures.forEach(item => {
            myFeaturesItems.push(new RoleFeatureClass(item, this.fatherComponent));
        });

        var logWindow = new LogitudeWindow();
        logWindow.Title = "Edit " + typeCode + " Features";
        logWindow.WindowArgs = { Items: myFeaturesItems };
        logWindow.Show('./InfrastructureModules/InfrastructureUser/Components/Roles/EditFeaturesRoleLinkComponent');

        logWindow.WindowClosed.subscribe(s => {
            if (s) {
                this.SetHyperlinks();
            }
        });
    }
}
