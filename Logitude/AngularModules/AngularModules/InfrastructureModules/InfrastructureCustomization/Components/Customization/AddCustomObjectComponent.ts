import { Component } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObjectTablePM } from '../../../../Infrastructure/EntityPMs/ObjectTablePM';
import { ObjectTablePMService } from '../../../../Infrastructure/Services/StandardPMs/ObjectTablePMService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { SubEntitiesComponent } from './SubEntitiesComponent';
import { ObjectFieldPMExtendedService } from '../../../../Infrastructure/Services/ExtendedPMs/ObjectFieldPMExtendedService';
import { CachedDataManager } from '../../../../Infrastructure/Utilities/CachedDataManager';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { CustomizationMainComponent } from './CustomizationMainComponent';
import { LoginService } from '../../../../Infrastructure/Services/LoginService';

const valdationMessageOfDisplayLabelSingular = 'Please fill the Display Label (Singular)';
const valdationMessageOfDisplayLabelPlural = 'Please fill the Display Label (Plural)';
const validationMessageOfDuplicateCustomSubObjectTableName = 'Another sub object with same Display Label(Singular) is already exist';
const validationMessageOfDuplicateCustomObjectTableName = 'Another custom object with same Display Label(Singular) is already exist';
const validationMessageOfCustomObjectTableNameLength = 'Maximum Length of Display Label(Singular) is 100';
declare var window: any;

@Component({

    templateUrl: './AddCustomObjectComponent.html',
})

export class AddCustomObjectComponent extends BaseComponent {

    private CurrentSession = SessionLocator.SelectedSession;

    ValidationErrorsList: any[];
    dataContext = this;
    private parentObjectTableId: string;
    private parentObjectTable: any;
    private objectTablePM: ObjectTablePM;
    private objectTablePMService: ObjectTablePMService;
    private objectFieldPMExtendedService: ObjectFieldPMExtendedService;
    private customizationSubEntitiesComponent: SubEntitiesComponent;
    private customizationMainComponent: CustomizationMainComponent;
    private objectTableName: string;
    public IsSubObject: boolean = true;
    public ObjectTableTypes: object[] = [];
    public TypeHelpText: string;
    
    constructor(private loginService: LoginService) {
        super();
        this.objectTablePMService = new ObjectTablePMService();
        this.objectFieldPMExtendedService = new ObjectFieldPMExtendedService();
        this.objectTablePM = new ObjectTablePM();
        this.UIProperties.SetRequired("DisplayLabelSingular", "ObjectTable", true);
        this.UIProperties.SetRequired("DisplayLabelPlural", "ObjectTable", true);
        this.loginService.CurrentTenant = SessionLocator.Tenant;
    }

    SetWindowArgs(args: any) {
        this.IsSubObject = args['IsSubObject'];
        this.customizationMainComponent = args['CustomizationMainComponent'];
        this.customizationSubEntitiesComponent = args['CustomizationSubEntitiesComponent'];
        this.parentObjectTableId = this.customizationSubEntitiesComponent?.ObjectTableId;
        this.parentObjectTable = this.parentObjectTableId ? window.ObjectTables.filter((table: any) => table.Id === this.parentObjectTableId)[0] : null;
        this.FillObjectTableTypes();
    }

    private FillObjectTableTypes() {
        if (this.IsSubObject) return;
        this.ObjectTableTypes.push({
            Record: "Buisness Record (Data)",
            Code: "BR"
        });
        this.ObjectTableTypes.push({
            Record: "Master Data (Reference)",
            Code: "MD"
        });
        this.ObjectTableType = this.ObjectTableTypes[0];
        this.SetTypeHelpText(this.ObjectTableType["Code"]);
    }

    private displayLabelSingular: string;
    get DisplayLabelSingular() { return this.displayLabelSingular; }
    set DisplayLabelSingular(newValue: string) {
        if (this.displayLabelSingular != newValue) {
            this.displayLabelSingular = newValue;
            this.objectTableName = newValue;
        }
    }

    private displayLabelPlural: string;
    get DisplayLabelPlural() { return this.displayLabelPlural; }
    set DisplayLabelPlural(newValue: string) {
        if (this.displayLabelPlural != newValue) {
            this.displayLabelPlural = newValue;
        }
    }

    private description: string;
    get Description() { return this.description; }
    set Description(newValue: string) {
        if (this.description != newValue) {
            this.description = newValue;
        }
    }

    private objectTableTypeCode: string="BR";
    private objectTableType: object;
    get ObjectTableType() { return this.objectTableType; }
    set ObjectTableType(newValue: object) {
        if (this.objectTableType == newValue) return;
        this.objectTableType = newValue;
        this.objectTableTypeCode = newValue["Code"];
        this.SetTypeHelpText(this.objectTableTypeCode);
    }

    SetTypeHelpText(objectTableTypeCode: string) {
        if (AppTool.IsNullOrEmpty(objectTableTypeCode)) return;
        if (objectTableTypeCode == "BR") {
            this.TypeHelpText = "Using this Type your object will be considered as a main object";
            return;
        }
        if (objectTableTypeCode == "MD") {
            this.TypeHelpText = "Using this Type your object can be used as a reference in other objects";
            return;
        }
    }
    SaveButtonClicked() {
        let errors = [];

        if (!this.displayLabelSingular)
            errors.push(valdationMessageOfDisplayLabelSingular);

        if (!this.displayLabelPlural)
            errors.push(valdationMessageOfDisplayLabelPlural);

        if (this.IsNotValidCustomTableName()) {
            errors.push(validationMessageOfDuplicateCustomObjectTableName);
        }

        if (this.IsNotValidCustomSubTableName()) {
            errors.push(validationMessageOfDuplicateCustomSubObjectTableName);
        }

        if (!this.ValidTableNameLength()) {
            errors.push(validationMessageOfCustomObjectTableNameLength);
        }

        if (errors.length > 0)
            return this.ValidationErrorsList = errors;

        this.CurrentSession.StartBusyIndicator("Saving ...");
        this.MapCustomObjectTableFields();
        this.CreateObjectTable();

    }

    private ValidTableNameLength() {
        return this.DisplayLabelSingular.length < 100;
    }

    private IsNotValidCustomTableName() {
        if (this.IsSubObject) return;
        let objectTable = window.ObjectTables.filter(t => t.FullNameTextCodeDefaultText?.toLowerCase() == this.objectTableName.toLowerCase() && t.IsCustom && AppTool.IsNullOrEmpty(t.ParentObjectTableId))[0];
        return !AppTool.IsNullOrEmpty(objectTable);
    }

    private IsNotValidCustomSubTableName() {
        if (!this.IsSubObject) return;
        let objectTable = window.ObjectTables.filter(t => t.FullNameTextCodeDefaultText?.toLowerCase() == this.objectTableName.toLowerCase() && t.IsCustom && t.ParentObjectTableId == this.parentObjectTableId)[0];
        return !AppTool.IsNullOrEmpty(objectTable);
    }

    private MapCustomObjectTableFields() {
        this.objectTablePM.ParentObjectTableId = this.parentObjectTableId;
        this.objectTablePM.ClientModuleName = this.parentObjectTable?.ClientModuleName;
        this.objectTablePM.IsCustom = true;
        this.objectTablePM.Tenant = SessionLocator.Tenant;
        this.objectTablePM.LastUpdateDate = DateTool.GetCurrentDateTimeAsUtc();
        this.objectTablePM.Name = "Custom Table";
        this.objectTablePM.DefaultText = this.DisplayLabelSingular;
        this.objectTablePM.FullNameTextCodeDefaultText = this.DisplayLabelSingular;
        this.objectTablePM.DefaultTextPlural = this.DisplayLabelPlural;
        this.objectTablePM.Description = this.Description;
        this.objectTablePM.ObjectTableTypeCode = this.objectTableTypeCode;
        this.objectTablePM.SupportSubEntity = this.IsSubObject ? false : true;
        this.objectTablePM.AvailableInDocumentTypes = this.IsSubObject ? false : true;
       
    }
    CreateObjectTable() {
        this.objectTablePMService.insert(this.objectTablePM).subscribe((response: ServiceResponse) => {
            if (response.HasError) return;
            response.Result.IsNew = true;
            this.LoadData(response.Result);      
            this.CurrentSession.StopBusyIndicator();
            this.CurrentSession.CloseCurrentWindow();
        });
    }

    LoadData(objectTable: ObjectTablePM) {
        window.ObjectTables.push(objectTable);
        this.objectTablePM.Name = objectTable?.Name;
        this.GetObjectFields();
        this.GetScreens();
        this.GetTabs();
        if (this.IsSubObject) this.customizationSubEntitiesComponent.AddObjectTable(objectTable);
        else this.customizationMainComponent.RefreshObjectTables();
    }
    GetObjectFields() {
        this.objectFieldPMExtendedService.GetObjectFieldsByObjectTable(this.objectTablePM.Name).subscribe((response: any) => {
            if (!response) return;
            window.ObjectFields = window.ObjectFields.concat(response);
            response.forEach(item => {
                CachedDataManager.RefreshTenantTextCodes().subscribe((res: any) => {
                    var oldItem = window.ObjectFields.filter(t => t.Id == item.Id)[0];
                    if (oldItem) {
                        var index = window.ObjectFields.indexOf(oldItem);
                        window.ObjectFields.splice(index, 1);
                    }
                    window.ObjectFields.push(item);
                });
            })
        });
    }
    GetScreens() {
        this.loginService.GetScreens().subscribe((screens: any) => {
            window.Screens = screens;
        });
    }
    GetTabs() { 
        this.loginService.GetObjectTableTabs().subscribe((tabs: any) => {
            window.ObjectTableTabs = tabs;
        });
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }



}
