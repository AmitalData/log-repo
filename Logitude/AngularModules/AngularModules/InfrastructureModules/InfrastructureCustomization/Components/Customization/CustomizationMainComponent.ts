import {Component} from '@angular/core';
import {GeneralDomainService, FieldsTranslations} from '../../../../Infrastructure/Services/GeneralDomainService';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {ObjectTablePM} from '../../../../Infrastructure/EntityPMs/ObjectTablePM';
import {AppTool} from '../../../../Infrastructure/Tools';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
declare var window: any;

@Component({
    moduleId: module.id,
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
 
    private allTablesItems: FieldsTranslations[];
    private LoadTableTranslations() {
        this.myService.GetTranslationsByParam("T", "", SessionLocator.TenantPM.Language).subscribe(myResult => {
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
            if (table != null) {
                myData.push(field);
            }
        });

        this.ItemsSource1 = myData.filter(f => f.ObjectTableTypeCode != "MD");
        this.ItemsSource2 = myData.filter(f => f.ObjectTableTypeCode == "MD");
    }

    public selectedRow: FieldsTranslations;
    Selecting(item: FieldsTranslations) {
        this.selectedRow = item;

        if (item == null) {
            this.IsButtonEnabled = false;
        }

        else {
            this.IsButtonEnabled = true;
        }
    }

    StandardFieldsClicked() {
        var table: ObjectTablePM = window.ObjectTables.filter(d => d.Id == this.selectedRow.ObjectTableID)[0];
        if (table != null) {          
        this.entityResourceService.getEntityResourceByTableName(table.Name, 0).subscribe(response => {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Standard Fields: " + this.selectedRow.DefaultText;
        logWindow.IsFillScreen_115 = true;
        logWindow.WindowArgs = { ObjectTableId: this.selectedRow.ObjectTableID};
        logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/StandardFieldsComponent');                                    
            });
        }
    }
    
    LabelsClicked() {
        var table: ObjectTablePM = window.ObjectTables.filter(d => d.Id == this.selectedRow.ObjectTableID)[0];

        if (table != null) {
            this.entityResourceService.getEntityResourceByTableName(table.Name, 0).subscribe(response => {
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
            this.entityResourceService.getEntityResourceByTableName(table.Name, 0).subscribe(response => {
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
            this.entityResourceService.getEntityResourceByTableName(table.Name, 0).subscribe(response => {
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
