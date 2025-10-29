import { Component } from '@angular/core';
import { GeneralDomainService, FieldsTranslations } from '../../../../Infrastructure/Services/GeneralDomainService';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { ObjectTablePM } from '../../../../Infrastructure/EntityPMs/ObjectTablePM';
import { AppTool } from '../../../../Infrastructure/Tools';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { ObjectsLocator } from 'Infrastructure/Locators/ObjectsLocator';
import { ConfirmWindow } from 'Controls/Windows/ConfirmWindow';
import { CustomizationPermissionService } from '../../ExternalService/CustomizationPermissionService';
declare var window: any;

@Component({

    templateUrl: './CustomizationMainComponent.html',
})

export class CustomizationMainComponent {

    public ItemsSource1: FieldsTranslations[] = [];
    public ItemsSource2: FieldsTranslations[] = [];
    public ItemsSource1Hidden: boolean = false;
    public ItemsSource2Hidden: boolean = false;
    public IsButtonEnabled: boolean = false;
    public IsCustomFieldsMenue: boolean = false;
    public IsReady: boolean = false;
    private myService: GeneralDomainService;
    private entityResourceService: EntityResourceService
    private CurrentSession = SessionLocator.SelectedSession;
    public NumberOfItems: number = 0;
    public IsEnabledCreatingCustomObjects: boolean = false;
    private _entityResourceService: EntityResourceService = new EntityResourceService();

    constructor() {
        this.myService = new GeneralDomainService();
        this.entityResourceService = new EntityResourceService();
        this.LoadTableTranslations();
        this.LoadPermessions();
    }
    SetWindowArgs(args: any) {
        this.IsCustomFieldsMenue = args.IsCustomFieldsMenue;
    }
    private searchText: string = null;
    public get SearchText() { return this.searchText; }
    public set SearchText(value: string) {
        if (this.searchText != value) {
            this.searchText = value;
        }
    }

    SearchTextChanged(text: string) {
        this.SearchText = text;
        this.BuildItemsSources(true);
    }

    IsObjectTableFilterEnabled: boolean;
    IsCustomizationToggleActive: boolean;
    LoadPermessions() {
        this.IsObjectTableFilterEnabled = this.SetIsObjectTableFilterEnabled();
        this.IsCustomizationToggleActive = CustomizationPermissionService.HasToggleFeaturePermession("CUS");
        this.IsEnabledCreatingCustomObjects = CustomizationPermissionService.HasFeaturePermession("General", "Customization.CreateObjects");
    }

    SetIsObjectTableFilterEnabled(): boolean {
        if (SessionLocator.LoggedUserPM.IsCustomerCare || ObjectsLocator.GlobalSetting?.DeploymentStage == "Dev" || SessionLocator.LoggedUserPM.IsDistributor) {
            return false;
        }
        return true;
    }

    private allTablesItems: FieldsTranslations[];
    private LoadTableTranslations() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.myService.GetTranslationsByParam("T", "", SessionLocator.TenantPM.Language).subscribe((myResult: ServiceResponse) => {
            var myResponse: ServiceResponse = myResult;
            if (!myResponse.HasError) {

                this.allTablesItems = myResponse.Result;
                if (this.allTablesItems != null) {
                    this.BuildItemsSources();
                }
            }
            this.CurrentSession.StopBusyIndicator();
        });
    }

    private BuildItemsSources(fromSearch: boolean = false) {
        var myTablesItems: FieldsTranslations[];

        if (!AppTool.IsNullOrEmpty(this.SearchText)) {
            myTablesItems = this.allTablesItems.filter(f => f.DefaultText != null && f.DefaultText.toLowerCase().indexOf(this.SearchText.toLowerCase()) != -1
                || f.TranslatedText != null && f.TranslatedText.toLowerCase().indexOf(this.SearchText.toLowerCase()) != -1
                || f.TranslatedTextPlural != null && f.TranslatedTextPlural.toLowerCase().indexOf(this.SearchText.toLowerCase()) != -1);
        }

        else {
            myTablesItems = this.allTablesItems;
        }

        var tablesList: ObjectTablePM[] = window.ObjectTables.filter(d => (d.IsMain && d.EnableSecurity && !d.IsClosed && !d.IsComposition) || d.Name == "Address" || d.Name == "Card" || (d.IsComposition && d.AllowCustomFields) || (d.IsCustom && AppTool.IsNullOrEmpty(d.ParentObjectTableId)));
        var myData: FieldsTranslations[] = [];

        myTablesItems.forEach(field => {
            var table: ObjectTablePM = tablesList.filter(d => d.Id == field.ObjectTableID)[0];
            if (table != null && this.HasEntityPermessions(table) && this.HaveObjectTableAccess(table)) {
                myData.push(field);
            }
        });

        this.ItemsSource1 = myData.filter(f => f.ObjectTableTypeCode != "MD");
        this.ItemsSource2 = myData.filter(f => f.ObjectTableTypeCode == "MD");

        if ((!myData || myData.length == 0) && this.IsObjectTableFilterEnabled && !fromSearch) {
            this.ShowPackageMessage();
        }
        this.IsReady = true;
        this.NumberOfItems = this.ItemsSource1.length + this.ItemsSource2.length;
    }

    HasEntityPermessions(objectTable) {
        return CustomizationPermissionService.HasEntityPermessions(objectTable.Name, "READ", false) && objectTable.ParentObjectTableId == null;
    }

    ShowPackageMessage() {
        var window = new ConfirmWindow();
        window.Width = 450;
        window.Height = 190;
        window.Title = "You have no permession";
        window.YesButtonText = "Ok";
        window.ShowNoButton = false;
        window.Show("Your package doesn't include this module..");
    }

    HaveObjectTableAccess(table: ObjectTablePM): boolean {
        if (this.IsCustomFieldsMenue) return true;
        if (!this.IsObjectTableFilterEnabled) return true;
        if (this.IsCustomizationToggleActive) return true;
        return false;
    }

    public selectedRow: FieldsTranslations;
    OpenCustomizationEditComponent(fieldsTranslations: FieldsTranslations) {

        this.selectedRow = fieldsTranslations;

        var table: ObjectTablePM = window.ObjectTables.filter(d => d.Id == this.selectedRow.ObjectTableID)[0];
        this.CurrentSession.StartBusyIndicator("Loading ...");
        if (table.IsCustom) {
            this.CurrentSession.StopBusyIndicator();
            this.ShowCustomizationEditComponent(table.Id);
            return;
        }
        this._entityResourceService.getEntityResourceByTableName(table.Name).subscribe((response: any) => {
            if (!response.HasError) {
                this.CurrentSession.StopBusyIndicator();
                this.ShowCustomizationEditComponent(table.Id);
            }
        });
    }

    private ShowCustomizationEditComponent(objectTableId: string) {
        var logWindow = new LogitudeWindow();
        logWindow.IsFillScreen = true;
        logWindow.IsShowCloseButton = false;
        logWindow.Title = "";
        logWindow.WindowArgs = {
            Title: this.selectedRow.DefaultText,
            IsCustomFieldsMenue: this.IsCustomFieldsMenue,
            IsObjectTableFilterEnabled: this.IsObjectTableFilterEnabled,
            ObjectTableId: objectTableId,
            IsSubEntity: false
        };
        logWindow.Show('./InfrastructureCustomization/Components/Customization/CustomizationEditComponent');
    }

    NewCustomObjectClicked() {
        var window = new LogitudeWindow();
        window.Width = 600;
        window.Height = 400;
        window.Title = "New Custom Object";
        window.WindowArgs = {
            CustomizationSubEntitiesComponent: null,
            CustomizationMainComponent: this,
            IsSubObject: false
        };
        window.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/AddCustomObjectComponent');
    }
    RefreshObjectTables() {
        this.LoadTableTranslations();
    }
    CloseClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    IsCustomObjectTableById(objectTableId : string) {
        let objectTable = window.ObjectTables.filter(objectTable => objectTable.Id == objectTableId)[0];
        return objectTable.IsCustom;
    }
}
