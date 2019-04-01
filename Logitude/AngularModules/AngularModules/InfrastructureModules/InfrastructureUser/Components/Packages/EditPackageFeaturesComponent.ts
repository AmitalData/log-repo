import {Component} from '@angular/core';
import {AppTool} from '../../../../Infrastructure/Tools';
import {FeaturePM} from '../../../../Infrastructure/EntityPMs/FeaturePM';
import {ObjectTablePM} from '../../../../Infrastructure/EntityPMs/ObjectTablePM';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {InfrastructureDomainService, FeaturesUpdateHelper} from '../../../../Infrastructure/Services/InfrastructureDomainService';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
declare var window: any;

@Component({
    moduleId: module.id,
    templateUrl: './EditPackageFeaturesComponent.html',
})

export class EditPackageFeaturesComponent {
    public PackageCode: string;
    public ValidationErrorsList: string[] = [];
    public ItemsSource1: TablePackageFeatureClass[] = [];
    public ItemsSource2: TablePackageFeatureClass[] = [];
    public ItemsSource1Hidden: boolean = false;
    public ItemsSource2Hidden: boolean = false;
    public MenusList: PackageFeatureClass[] = [];
    public OthersList: PackageFeatureClass[] = [];
    public SettingsList: PackageFeatureClass[] = [];
    public IsEventsButtonVisible: boolean = false;
    private myDomainService: InfrastructureDomainService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.myDomainService = new InfrastructureDomainService();

        if (FeatureLocator.HasFeaturePermession("General", "FeaturesChanges")) {
            this.IsEventsButtonVisible = true;
        }
    }

    SetWindowArgs(args: any) {
        this.PackageCode = args['PackageCode'];
        this.SetUIProperties();
        this.LoadFeatures();
    }

    public IsEditingEnabled: boolean = false;
    SetUIProperties() {
        if (SessionLocator.Tenant == 0) {
            this.IsEditingEnabled = true;
        }
    }

    private mySearchText: string = null;
    SearchTextChanged(text: string) {
        this.mySearchText = text;
        this.BuildCollections();
    }

    private allFeatures: FeaturePM[] = [];
    private allFeaturesItems: PackageFeatureClass[] = [];
    private LoadFeatures() {
        this.CurrentSession.StartBusyIndicatorLoading();

        this.myDomainService.GetSelectedAndUnselectedPackageFeatures(this.PackageCode).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.allFeatures = myResponse.Result;

                this.allFeatures.forEach(itemFeature => {
                    this.allFeaturesItems.push(new PackageFeatureClass(itemFeature, this));
                });
            }

            this.BuildCollections();
            this.CurrentSession.StopBusyIndicator();
        });
    }
    private BuildCollections() {
        this.BuildTablesLists();
        this.BuildMenusList();
        this.BuildOthersList();
        this.BuildSettingsList();
    }
    private BuildTablesLists() {

        var items: ObjectTablePM[] = window.ObjectTables.filter(d => d.IsMain == true && d.IsClosed == false && d.IsComposition == false);

        if (!AppTool.IsNullOrEmpty(this.mySearchText)) {
            items = items.filter(f => f.Name != null && f.Name.toLowerCase().indexOf(this.mySearchText.toLowerCase()) > -1);
        }

        items = items.sort(function (a, b) { return a.Name.toLowerCase() == b.Name.toLowerCase() ? 0 : a.Name.toLowerCase() < b.Name.toLowerCase() ? -1 : 1; });

        var allTablesItems: TablePackageFeatureClass[] = [];

        items.forEach(item => {
            if (allTablesItems.filter(f => f.ObjectTableId == item.Id).length == 0) {
                if (this.allFeatures.filter(d => d.ObjectTableId == item.Id && d.FeatureTypeCode == "MODL").length > 0) {
                    allTablesItems.push(new TablePackageFeatureClass(item, this.allFeatures.filter(d => d.ObjectTableId == item.Id), this));;
                }
            }
        });

        this.ItemsSource1 = allTablesItems.filter(f => f.ObjectTableTypeCode != "MD");
        this.ItemsSource2 = allTablesItems.filter(f => f.ObjectTableTypeCode == "MD");
    }
    private BuildMenusList() {

        var items: PackageFeatureClass[] = [];
        items = this.allFeaturesItems.filter(d => d.FeatureTypeCode.toUpperCase() == "MENU");
        items = items.sort(function (a, b) { return a.Code.toLowerCase() == b.Code.toLowerCase() ? 0 : a.Code.toLowerCase() < b.Code.toLowerCase() ? -1 : 1; });

        if (!AppTool.IsNullOrEmpty(this.mySearchText)) {
            items = items.filter(f => f.Name != null && f.Name.toLowerCase().indexOf(this.mySearchText.toLowerCase()) > -1);
        }

        this.MenusList = items;
    }
    private BuildOthersList() {

        var items: PackageFeatureClass[] = [];
        items = this.allFeaturesItems.filter(d => d.FeatureTypeCode == "OTH");
        items = items.sort(function (a, b) { return a.Code.toLowerCase() == b.Code.toLowerCase() ? 0 : a.Code.toLowerCase() < b.Code.toLowerCase() ? -1 : 1; });

        if (!AppTool.IsNullOrEmpty(this.mySearchText)) {
            items = items.filter(f => f.Name != null && f.Name.toLowerCase().indexOf(this.mySearchText.toLowerCase()) > -1);
        }

        this.OthersList = items;
    }
    private BuildSettingsList() {

        var items: PackageFeatureClass[] = [];
        items = this.allFeaturesItems.filter(d => d.FeatureTypeCode == "SET");
        items = items.sort(function (a, b) { return a.Code.toLowerCase() == b.Code.toLowerCase() ? 0 : a.Code.toLowerCase() < b.Code.toLowerCase() ? -1 : 1; });

        if (!AppTool.IsNullOrEmpty(this.mySearchText)) {
            items = items.filter(f => f.Name != null && f.Name.toLowerCase().indexOf(this.mySearchText.toLowerCase()) > -1);
        }

        this.SettingsList = items;
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
            this.CurrentSession.StartBusyIndicatorSaving();

            var myServiceHelper = new FeaturesUpdateHelper();
            myServiceHelper.Tenant = SessionLocator.Tenant;
            myServiceHelper.PackageCode = this.PackageCode;
            myServiceHelper.Items = items;

            this.myDomainService.UpdateFeatures(myServiceHelper).subscribe((myResponse: ServiceResponse) => {

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

    ShowEventsClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Package Events";
        logWindow.WindowArgs = this.PackageCode;
        logWindow.Show('./InfrastructureModules/InfrastructureUser/Components/Packages/PackageFeaturesEventsComponent');
    }
}
export class PackageFeatureClass {
    public Feature: FeaturePM;
    public Code: string = null;
    public Name: string = null;
    public FeatureTypeCode: string = null;
    public IsEditingEnabled: boolean = false;
    private OldIsActive: boolean = false;
    constructor(entityPM: FeaturePM, private fatherComponent: EditPackageFeaturesComponent) {
        this.Feature = entityPM;
        this.Code = entityPM.Code;
        this.Name = entityPM.TranslatedName;
        this.FeatureTypeCode = entityPM.FeatureTypeCode.toUpperCase();
        this.IsEditingEnabled = fatherComponent.IsEditingEnabled;
        this.isActive = entityPM.Exists;
        this.OldIsActive = entityPM.Exists;
    }

    private isActive: boolean = false;
    public get IsActive() { return this.isActive; }
    public set IsActive(value: boolean) {
        if (this.isActive != value) {
            this.isActive = value;

            if (value == true) {
                this.Feature.Exists = true;
                this.Feature.IsAdded = true;
                this.Feature.IsRemoved = false;
                this.Feature.PackageCode = this.fatherComponent.PackageCode;
            }

            else {
                this.Feature.Exists = false;
                this.Feature.IsAdded = false;
                this.Feature.IsRemoved = true;
                this.Feature.PackageCode = this.fatherComponent.PackageCode;
            }

            if (value == this.OldIsActive) {
                this.Feature.IsDirty = false;
            }
        }
    }
}
export class TablePackageFeatureClass {
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
    private OldIsActive_ModuleFeature: boolean = false;
    private OldIsActive_ReadFeature: boolean = false;
    private OldIsActive_UpdateFeature: boolean = false;
    private OldIsActive_AddNewFeature: boolean = false;
    constructor(objectTablePM: ObjectTablePM, myFeatures: FeaturePM[], private fatherComponent: EditPackageFeaturesComponent) {
        this.Name = TextCodeTranslator.Translate(objectTablePM.Name);
        this.ObjectTableId = objectTablePM.Id;
        this.ObjectTableTypeCode = objectTablePM.ObjectTableTypeCode;
        this.ObjectTableTypeName = this.ObjectTableTypeCode == "MD" ? "Master Data" : "Buisness Records";

        this.ModuleFeature = myFeatures.filter(d => d.FeatureTypeCode.toUpperCase() == "MODL")[0];
        this.ReadFeature = myFeatures.filter(d => d.FeatureTypeCode.toUpperCase() == "READ")[0];
        this.UpdateFeature = myFeatures.filter(d => d.FeatureTypeCode.toUpperCase() == "UPDT")[0];
        this.AddNewFeature = myFeatures.filter(d => d.FeatureTypeCode.toUpperCase() == "NEW")[0];
        this.AreasFeatures = myFeatures.filter(d => d.FeatureTypeCode.toUpperCase() == "AREA" && d.Packagable == true);
        this.ActionsFeatures = myFeatures.filter(d => d.FeatureTypeCode.toUpperCase() == "ACT" && d.Packagable == true);
        this.QueriesFeatures = myFeatures.filter(d => d.FeatureTypeCode.toUpperCase() == "QUER");

        if (this.ModuleFeature) {
            this.isActive_ModuleFeature = this.ModuleFeature.Exists;
            this.OldIsActive_ModuleFeature = this.ModuleFeature.Exists;
        }

        if (this.ReadFeature) {
            this.isActive_ReadFeature = this.ReadFeature.Exists;
            this.OldIsActive_ReadFeature = this.ReadFeature.Exists;
        }

        if (this.UpdateFeature) {
            this.isActive_UpdateFeature = this.UpdateFeature.Exists;
            this.OldIsActive_UpdateFeature = this.UpdateFeature.Exists;
        }

        if (this.AddNewFeature) {
            this.isActive_AddNewFeature = this.AddNewFeature.Exists;
            this.OldIsActive_AddNewFeature = this.AddNewFeature.Exists;
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
        this.AreasLinkText = this.AreasFeatures.filter(d => d.Exists == true).length + "/" + this.AreasFeatures.length;
        this.ActionsLinkText = this.ActionsFeatures.filter(d => d.Exists == true).length + "/" + this.ActionsFeatures.length;
        this.QueriesLinkText = this.QueriesFeatures.filter(d => d.Exists == true).length + "/" + this.QueriesFeatures.length;
        this.AreasLinkColor = this.AreasFeatures.filter(d => d.Exists != true).length > 0 ? "#1E4AC4" : "#009161";
        this.ActionsLinkColor = this.ActionsFeatures.filter(d => d.Exists != true).length > 0 ? "#1E4AC4" : "#009161";
        this.QueriesLinkColor = this.QueriesFeatures.filter(d => d.Exists != true).length > 0 ? "#1E4AC4" : "#009161";
    }

    public IsEditingEnabled: boolean = false;
    public IsEditingEnabled_Read: boolean = false;
    public IsEditingEnabled_Update: boolean = false;
    public IsEditingEnabled_AddNew: boolean = false;
    public IsModuleEnabled: boolean = false;
    SetUIProperties() {
        var isEditingEnabled = true;
        var isEditingEnabled_Read = true;
        var isEditingEnabled_Update = true;
        var isEditingEnabled_AddNew = true;
        var isModuleEnabled = true;

        if (SessionLocator.Tenant != 0) {
            isEditingEnabled = false;
            isEditingEnabled_Read = false;
            isEditingEnabled_Update = false;
            isEditingEnabled_AddNew = false;
        }

        if (this.ModuleFeature == null) {
            isEditingEnabled = false;
            isEditingEnabled_Update = false;
            isEditingEnabled_AddNew = false;
        }

        if (this.IsActive_ModuleFeature == false) {
            isEditingEnabled_Read = false;
            isEditingEnabled_Update = false;
            isEditingEnabled_AddNew = false;
            isModuleEnabled = false;
        }

        if (this.IsActive_ReadFeature == false) {
            isEditingEnabled_AddNew = false;
        }

        if (this.IsActive_AddNewFeature == true) {
            isEditingEnabled_Update = false;
        }

        this.IsEditingEnabled = isEditingEnabled;
        this.IsEditingEnabled_Read = isEditingEnabled_Read;
        this.IsEditingEnabled_Update = isEditingEnabled_Update;
        this.IsEditingEnabled_AddNew = isEditingEnabled_AddNew;
        this.IsModuleEnabled = isModuleEnabled;
    }

    private isActive_ModuleFeature: boolean = false;
    public get IsActive_ModuleFeature() { return this.isActive_ModuleFeature; }
    public set IsActive_ModuleFeature(value: boolean) {
        if (this.isActive_ModuleFeature != value) {
            this.isActive_ModuleFeature = value;
            this.SetUIProperties();

            if (this.ModuleFeature) {
                if (value == true) {
                    this.ModuleFeature.Exists = true;
                    this.ModuleFeature.IsAdded = true;
                    this.ModuleFeature.IsRemoved = false;
                    this.ModuleFeature.PackageCode = this.fatherComponent.PackageCode;
                }

                else {
                    this.ModuleFeature.Exists = false;
                    this.ModuleFeature.IsAdded = false;
                    this.ModuleFeature.IsRemoved = true;
                    this.ModuleFeature.PackageCode = this.fatherComponent.PackageCode;
                }

                if (value == this.OldIsActive_ModuleFeature) {
                    this.ModuleFeature.IsDirty = false;
                }
            }
        }
    }

    private isActive_ReadFeature: boolean = false;
    public get IsActive_ReadFeature() { return this.isActive_ReadFeature; }
    public set IsActive_ReadFeature(value: boolean) {
        if (this.isActive_ReadFeature != value) {
            this.isActive_ReadFeature = value;
            this.SetUIProperties();

            if (this.ReadFeature) {
                if (value == true) {
                    this.ReadFeature.Exists = true;
                    this.ReadFeature.IsAdded = true;
                    this.ReadFeature.IsRemoved = false;
                    this.ReadFeature.PackageCode = this.fatherComponent.PackageCode;
                }

                else {
                    this.ReadFeature.Exists = false;
                    this.ReadFeature.IsAdded = false;
                    this.ReadFeature.IsRemoved = true;
                    this.ReadFeature.PackageCode = this.fatherComponent.PackageCode;
                }

                if (value == this.OldIsActive_ReadFeature) {
                    this.ReadFeature.IsDirty = false;
                }
            }
        }
    }

    private isActive_UpdateFeature: boolean = false;
    public get IsActive_UpdateFeature() { return this.isActive_UpdateFeature; }
    public set IsActive_UpdateFeature(value: boolean) {
        if (this.isActive_UpdateFeature != value) {
            this.isActive_UpdateFeature = value;

            if (this.UpdateFeature) {
                if (value == true) {
                    this.UpdateFeature.Exists = true;
                    this.UpdateFeature.IsAdded = true;
                    this.UpdateFeature.IsRemoved = false;
                    this.UpdateFeature.PackageCode = this.fatherComponent.PackageCode;
                }

                else {
                    this.UpdateFeature.Exists = false;
                    this.UpdateFeature.IsAdded = false;
                    this.UpdateFeature.IsRemoved = true;
                    this.UpdateFeature.PackageCode = this.fatherComponent.PackageCode;
                }

                if (value == this.OldIsActive_UpdateFeature) {
                    this.UpdateFeature.IsDirty = false;
                }
            }
        }
    }

    private isActive_AddNewFeature: boolean = false;
    public get IsActive_AddNewFeature() { return this.isActive_AddNewFeature; }
    public set IsActive_AddNewFeature(value: boolean) {
        if (this.isActive_AddNewFeature != value) {
            this.isActive_AddNewFeature = value;
            this.SetUIProperties();

            if (this.AddNewFeature) {
                if (value == true) {
                    this.AddNewFeature.Exists = true;
                    this.AddNewFeature.IsAdded = true;
                    this.AddNewFeature.IsRemoved = false;
                    this.AddNewFeature.PackageCode = this.fatherComponent.PackageCode;
                }

                else {
                    this.AddNewFeature.Exists = false;
                    this.AddNewFeature.IsAdded = false;
                    this.AddNewFeature.IsRemoved = true;
                    this.AddNewFeature.PackageCode = this.fatherComponent.PackageCode;
                }

                if (value == this.OldIsActive_AddNewFeature) {
                    this.AddNewFeature.IsDirty = false;
                }
            }
        }
    }

    HyperlinkClicked(typeCode: string) {
        var myFeatures: FeaturePM[] = [];
        var myFeaturesItems: PackageFeatureClass[] = [];
        switch (typeCode) {
            case "Areas": { myFeatures = this.AreasFeatures; break; }
            case "Actions": { myFeatures = this.ActionsFeatures; break; }
            case "Queries": { myFeatures = this.QueriesFeatures; break; }
        }

        myFeatures.forEach(item => {
            myFeaturesItems.push(new PackageFeatureClass(item, this.fatherComponent));
        });

        var logWindow = new LogitudeWindow();
        logWindow.Title = "Edit " + typeCode + " Features";
        logWindow.WindowArgs = { Items: myFeaturesItems };
        logWindow.Show('./InfrastructureModules/InfrastructureUser/Components/Packages/EditFeaturesPackageLinkComponent');

        logWindow.WindowClosed.subscribe(s => {
            if (s) {
                this.SetHyperlinks();
            }
        });
    }
}
