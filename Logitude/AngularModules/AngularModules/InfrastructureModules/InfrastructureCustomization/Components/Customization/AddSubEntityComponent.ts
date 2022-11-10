import { Component } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObjectTablePM } from '../../../../Infrastructure/EntityPMs/ObjectTablePM';
import { ObjectTablePMService } from '../../../../Infrastructure/Services/StandardPMs/ObjectTablePMService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { SubEntitiesComponent } from './SubEntitiesComponent';
import { ObjectFieldPMExtendedService } from '../../../../Infrastructure/Services/ExtendedPMs/ObjectFieldPMExtendedService';
import { CachedDataManager } from '../../../../Infrastructure/Utilities/CachedDataManager';
import { AppTool } from '../../../../Infrastructure/Tools';

const valdationMessageOfDisplayLabelSingular = 'Please fill the Display Label (Singular)';
const valdationMessageOfDisplayLabelPlural = 'Please fill the Display Label (Plural)';
const valdationMessageOfDuplicateTableName = 'Another sub object with same Display Label(Singular) is already exist';
declare var window: any;

@Component({

    templateUrl: './AddSubEntityComponent.html',
})

export class AddSubEntityComponent extends BaseComponent {

    private CurrentSession = SessionLocator.SelectedSession;

    ValidationErrorsList: any[];
    dataContext = this;
    private parentObjectTableId: string;
    private parentObjectTable: any;
    private objectTablePM: ObjectTablePM;
    private objectTablePMService: ObjectTablePMService;
    private objectFieldPMExtendedService: ObjectFieldPMExtendedService;
    private customizationSubEntitiesComponent: SubEntitiesComponent;
    private objectTableName: string;
    constructor() {
        super();
        this.objectTablePMService = new ObjectTablePMService();
        this.objectFieldPMExtendedService = new ObjectFieldPMExtendedService();
        this.objectTablePM = new ObjectTablePM();
        this.UIProperties.SetRequired("DisplayLabelSingular", "ObjectTable", true);
        this.UIProperties.SetRequired("DisplayLabelPlural", "ObjectTable", true);
    }

    SetWindowArgs(args: any) {
        this.customizationSubEntitiesComponent = args['CustomizationSubEntitiesComponent'];
        this.parentObjectTableId = this.customizationSubEntitiesComponent.ObjectTableId;
        this.parentObjectTable = window.ObjectTables.filter((table: any) => table.Id === this.parentObjectTableId)[0];;
    }

    private displayLabelSingular: string;
    get DisplayLabelSingular() { return this.displayLabelSingular; }
    set DisplayLabelSingular(newValue: string) {
        if (this.displayLabelSingular != newValue) {
            this.displayLabelSingular = newValue;
            this.objectTableName = this.parentObjectTable.Name + "." + SessionLocator.Tenant + "." + newValue.replace(/\s/g, "");
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


    SaveButtonClicked() {
        let errors = [];

        if (!this.displayLabelSingular)
            errors.push(valdationMessageOfDisplayLabelSingular);

        if (!this.displayLabelPlural)
            errors.push(valdationMessageOfDisplayLabelPlural);

        if (this.IsNotValidTableName()) {
            errors.push(valdationMessageOfDuplicateTableName);
        }

        if (errors.length > 0)
            return this.ValidationErrorsList = errors;

        this.CurrentSession.StartBusyIndicator("Saving ...");
        this.MapCustomObjectTableFields();

    }

    private IsNotValidTableName() {
        let objectTable = window.ObjectTables.filter(t => t.Name == this.objectTableName)[0];
        return !AppTool.IsNullOrEmpty(objectTable);
    }

    private MapCustomObjectTableFields() {
        this.objectTablePM.ParentObjectTableId = this.parentObjectTableId;
        this.objectTablePM.ClientModuleName = this.parentObjectTable?.ClientModuleName;
        this.objectTablePM.IsCustom = true;
        this.objectTablePM.Tenant = SessionLocator.Tenant;

        this.objectTablePM.Name = this.objectTableName;
        this.objectTablePM.DefaultText = this.DisplayLabelSingular;
        this.objectTablePM.DefaultTextPlural = this.DisplayLabelPlural;
        this.objectTablePM.Description = this.Description;

        this.objectTablePMService.insert(this.objectTablePM).subscribe((response: ServiceResponse) => {

            if (response.HasError) return;

            this.CurrentSession.StopBusyIndicator();
            response.Result.IsNew = true;
            window.ObjectTables.push(response.Result);
            this.GetObjectFields();
            this.customizationSubEntitiesComponent.ApplyChanges(response.Result);
            this.CurrentSession.CloseCurrentWindow();

        });
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
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }



}
