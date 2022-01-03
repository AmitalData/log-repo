import { Component } from '@angular/core';
import { GeneralDomainService, FieldsTranslations } from '../../../../Infrastructure/Services/GeneralDomainService';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { ObjectTablePM } from '../../../../Infrastructure/EntityPMs/ObjectTablePM';
import { AppTool } from '../../../../Infrastructure/Tools';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { ObjectsLocator } from 'Infrastructure/Locators/ObjectsLocator';
import { FeatureLocator } from 'Infrastructure/Utilities/FeatureLocator';
import { ConfirmWindow } from 'Controls/Windows/ConfirmWindow';
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
    private myService: GeneralDomainService;
    private entityResourceService: EntityResourceService
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.myService = new GeneralDomainService();
        this.entityResourceService = new EntityResourceService();
        this.LoadTableTranslations();
        this.LoadPermessions();
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
        this.BuildItemsSources();
    }

    IsObjectTableFilterEnabled: boolean;
    IsCustomizationToggleActive: boolean;
    LoadPermessions() {
        this.IsObjectTableFilterEnabled = this.SetIsObjectTableFilterEnabled();
        this.IsCustomizationToggleActive = SessionLocator.FeatureToggles.some(d => d.ToggleCode == "CUS");
    }

    SetIsObjectTableFilterEnabled(): boolean {
        if (SessionLocator.LoggedUserPM.IsCustomerCare || ObjectsLocator.GlobalSetting.DeploymentStage == "Dev") {
            return false;
        }
        return true;
    }

    private allTablesItems: FieldsTranslations[];
    private LoadTableTranslations() {
        this.myService.GetTranslationsByParam("T", "", SessionLocator.TenantPM.Language).subscribe((myResult: ServiceResponse) => {
            var myResponse: ServiceResponse = myResult;
            if (!myResponse.HasError) {

                this.allTablesItems = myResponse.Result;
                if (this.allTablesItems != null) {
                    this.BuildItemsSources();
                }
            }
        });
    }

    private BuildItemsSources() {
        var myTablesItems: FieldsTranslations[];

        if (!AppTool.IsNullOrEmpty(this.SearchText)) {
            myTablesItems = this.allTablesItems.filter(f => f.DefaultText != null && f.DefaultText.toLowerCase().startsWith(this.SearchText.toLowerCase())
                || f.TranslatedText != null && f.TranslatedText.toLowerCase().startsWith(this.SearchText.toLowerCase())
                || f.TranslatedTextPlural != null && f.TranslatedTextPlural.toLowerCase().startsWith(this.SearchText.toLowerCase()));
        }

        else {
            myTablesItems = this.allTablesItems;
        }

        var tablesList: ObjectTablePM[] = window.ObjectTables.filter(d => (d.IsMain && d.EnableSecurity && !d.IsClosed && !d.IsComposition) || d.Name == "Address");
        var myData: FieldsTranslations[] = [];

        myTablesItems.forEach(field => {
            var table: ObjectTablePM = tablesList.filter(d => d.Id == field.ObjectTableID)[0];
            if (table != null && this.HaveObjectTableAccess(table)) {
                myData.push(field);
            }
        });

        this.ItemsSource1 = myData.filter(f => f.ObjectTableTypeCode != "MD");
        this.ItemsSource2 = myData.filter(f => f.ObjectTableTypeCode == "MD");

        if(myData || myData.length ==0){
            this.ShowPackageMessage();
        }
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
        if (!this.IsObjectTableFilterEnabled) return true;
        if (!this.IsCustomizationToggleActive) return false;
        return this.HaveFieldsCustomization(table.Name) || this.HaveRulesCustomization(table.Name);
    }

    HaveFieldsCustomization(objectTableName: string): boolean {
        return FeatureLocator.HasFeaturePermession(objectTableName, "FIELDSCUSTOMIZATION");
    }

    HaveRulesCustomization(objectTableName: string): boolean {
        return FeatureLocator.HasFeaturePermession(objectTableName, "RULESCUSTOMIZATION");
    }

    public selectedRow: FieldsTranslations;
    public IsFieldsCustomizationEnabled: boolean = false;
    public IsRulesCustomizationEnabled: boolean = false;
    
    Selecting(fieldsTranslations: FieldsTranslations) {
        this.selectedRow = fieldsTranslations;
        this.IsFieldsCustomizationEnabled = false;
        this.IsRulesCustomizationEnabled = false;

        if (fieldsTranslations == null) {
            this.IsButtonEnabled = false;
            return;
        }
        this.IsButtonEnabled = true;
        this.IsFieldsCustomizationEnabled = this.HaveFieldsCustomization(fieldsTranslations.ObjectTableName);
        this.IsRulesCustomizationEnabled = this.HaveRulesCustomization(fieldsTranslations.ObjectTableName);
    }

    StandardFieldsClicked() {
        var table: ObjectTablePM = window.ObjectTables.filter(d => d.Id == this.selectedRow.ObjectTableID)[0];
        if (table != null) {
            this.entityResourceService.getEntityResourceByTableName(table.Name, 0).subscribe((response: any) => {
                var logWindow = new LogitudeWindow();
                logWindow.Title = "Standard Fields: " + this.selectedRow.DefaultText;
                logWindow.IsFillScreen_115 = true;
                logWindow.WindowArgs = { ObjectTableId: this.selectedRow.ObjectTableID };
                logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/StandardFieldsComponent');
            });
        }
    }

    LabelsClicked() {
        var table: ObjectTablePM = window.ObjectTables.filter(d => d.Id == this.selectedRow.ObjectTableID)[0];

        if (table != null) {
            this.entityResourceService.getEntityResourceByTableName(table.Name, 0).subscribe((response: any) => {
                var logWindow = new LogitudeWindow();
                logWindow.Title = "Object Labels: " + this.selectedRow.DefaultText;
                logWindow.IsFillScreen = true;
                logWindow.WindowArgs = this.selectedRow;
                logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/ObjectLabelsComponent');
            });
        }
    }

    ScreensLayoutClicked() {
        var table: ObjectTablePM = window.ObjectTables.filter(d => d.Id == this.selectedRow.ObjectTableID)[0];

        if (table != null) {
            this.entityResourceService.getEntityResourceByTableName(table.Name, 0).subscribe((response: any) => {
                var logWindow = new LogitudeWindow();
                logWindow.Title = "Screens Layout: " + this.selectedRow.DefaultText;
                logWindow.IsFillScreen = true;
                logWindow.WindowArgs = { ObjectTableID: this.selectedRow.ObjectTableID };
                logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/ScreenLayoutComponent');
            });
        }
    }

    CustomFieldsClicked() {
        var table: ObjectTablePM = window.ObjectTables.filter(d => d.Id == this.selectedRow.ObjectTableID)[0];

        if (table != null) {
            this.entityResourceService.getEntityResourceByTableName(table.Name, 0).subscribe((response: any) => {
                var logWindow = new LogitudeWindow();
                logWindow.Title = "Custom Fields: " + this.selectedRow.DefaultText;
                logWindow.IsFillScreen_115 = true;
                logWindow.WindowArgs = { ObjectTableId: table.Id, ObjectTableName: table.Name }; //this.selectedRow.ObjectTableID;
                logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/CustomFieldsComponent');
            });
        }
    }

    RulesClicked() {
        //RulesMainComponent
        var table: ObjectTablePM = window.ObjectTables.filter(d => d.Id == this.selectedRow.ObjectTableID)[0];

        if (table != null) {
            var logWindow = new LogitudeWindow();
            logWindow.Title = "Object Rules: " + this.selectedRow.DefaultText;
            logWindow.IsFillScreen = true;
            logWindow.WindowArgs = this.selectedRow;
            logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/RulesComponents/RulesMainComponent');
        }
    }

    CloseClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
