import { Component } from '@angular/core';
import { GeneralDomainService, FieldsTranslations } from '../../../../Infrastructure/Services/GeneralDomainService';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { ObjectTablePM } from '../../../../Infrastructure/EntityPMs/ObjectTablePM';
import { ObjectFieldPM } from '../../../../Infrastructure/EntityPMs/ObjectFieldPM';
import { TextCodePM } from '../../../../Infrastructure/EntityPMs/TextCodePM';
import { AppTool } from '../../../../Infrastructure/Tools';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { ObjectFieldPMService } from '../../../../Infrastructure/Services/StandardPMs/ObjectFieldPMService';

declare var window: any;

@Component({

    templateUrl: './CustomFieldsComponent.html',
})

export class CustomFieldsComponent {
    private myService: GeneralDomainService;
    public CustomFieldsCollection: ObservableCollection;
    private loadedFields: ObjectFieldPM[];
    private _ObjectFieldPMService: ObjectFieldPMService;
    public IsAddButtonEnabled: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    private MaxNumberOfCustomFields: number;
    public IsCustomFieldsMenue: boolean = false;
    public AllowCustomFields: boolean = false;
    constructor() {
        this.myService = new GeneralDomainService();
        this.CustomFieldsCollection = new ObservableCollection([]);
        this._ObjectFieldPMService = new ObjectFieldPMService();
    }

    private ObjectTableId: string;
    private ObjectTableName: string;
    SetWindowArgs(args: any) {

        this.IsCustomFieldsMenue = args['IsCustomFieldsMenue'];
        this.AllowCustomFields = args['AllowCustomFields'];
        if (!this.AllowCustomFields) return;

        this.ObjectTableId = args['ObjectTableId'];
        this.ObjectTableName = args['ObjectTableName'];
        this.MaxNumberOfCustomFields = args['MaxNumberOfCustomFields'];

        this.myService.GetCustomFieldsByTableId(this.ObjectTableId).subscribe((myResult: ServiceResponse) => {
            var myResponse: ServiceResponse = myResult;
            if (!myResponse.HasError) {

                this.loadedFields = myResponse.Result;
                this.filterPickListCustomFields();
                if (this.loadedFields != null) {
                    this.BuildItemsSource();
                }
            }
        });
        //this.BuildItemsSource();
    }

    private filterPickListCustomFields() {
        if (this.IsCustomFieldsMenue)
            this.loadedFields = this.loadedFields.filter(a => a.DataTypeCode == "PickList");
    }

    private BuildItemsSource() {

        this.CustomFieldsCollection = new ObservableCollection(this.loadedFields);

        var fieldsCount = this.GetCustomFieldsCount();
        this.IsAddButtonEnabled = this.CustomFieldsCollection.Length < fieldsCount ? true : false;

    }

    GetCustomFieldsCount(): number {
        if (this.MaxNumberOfCustomFields && this.MaxNumberOfCustomFields != 0) return this.MaxNumberOfCustomFields;
        if (this.ObjectTableName == "Shipment" || this.ObjectTableName == "Master") return 70;
        if (this.ObjectTableName == "Quote") return 20;
        return 10;
    }

    AddCustomField() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Add New Custom Field";
        logWindow.Width = 800;
        logWindow.Height = 600;
        var windowArgs: any = {};
        this.myService = new GeneralDomainService();
        this.myService.GetFieldDataTypes().subscribe((myResult: ServiceResponse) => {
            var myResponse: ServiceResponse = myResult;
            if (!myResponse.HasError) {
                windowArgs.IsNew = true;
                var objectField = new ObjectFieldPM();
                objectField.Tenant = SessionLocator.Tenant;
                objectField.ObjectTableId = this.ObjectTableId;
                objectField.IsCustom = true;
                objectField.DisplayInEntityVariables = true;
                objectField.DisplayInList = true;
                objectField.CanFilter = true;
                windowArgs.objectField = objectField;
                windowArgs.DataTypeCollection = myResponse.Result;
                windowArgs.ObjectTableName = this.ObjectTableName;
                logWindow.WindowArgs = windowArgs;
                logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/AddEditCustomFieldComponent');
                logWindow.WindowClosed.subscribe((event: any) => {
                    this.myService.GetCustomFieldsByTableId(this.ObjectTableId).subscribe((myResult: ServiceResponse) => {
                        var myResponse: ServiceResponse = myResult;
                        if (!myResponse.HasError) {

                            this.loadedFields = myResponse.Result;
                            if (this.loadedFields != null) {
                                this.BuildItemsSource();
                            }
                        }
                    });
                });

            }
        });

    }

    EditLine(item) {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Edit Custom Field";
        logWindow.Width = 800;
        logWindow.Height = 600;
        var windowArgs: any = {};
        this.myService = new GeneralDomainService();
        this.myService.GetFieldDataTypes().subscribe((myResult: ServiceResponse) => {
            var myResponse: ServiceResponse = myResult;
            if (!myResponse.HasError) {
                this._ObjectFieldPMService.get(item.Id).subscribe((field: any) => {
                    windowArgs.IsNew = false;
                    var objectField = field.Result;
                    //objectField.Tenant = SessionLocator.Tenant;
                    //objectField.ObjectTableId = this.ObjecttableId;
                    //objectField.IsCustom = true;
                    //objectField.DisplayInEntityVariables = true;
                    //objectField.DisplayInList = true;
                    //objectField.CanFilter = true;
                    windowArgs.objectField = objectField;
                    windowArgs.DataTypeCollection = myResponse.Result;
                    windowArgs.ObjectTableName = this.ObjectTableName;
                    logWindow.WindowArgs = windowArgs;
                    logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/AddEditCustomFieldComponent');
                    logWindow.WindowClosed.subscribe((event: any) => {
                        this.myService.GetCustomFieldsByTableId(this.ObjectTableId).subscribe((myResult: ServiceResponse) => {
                            var myResponse: ServiceResponse = myResult;
                            if (!myResponse.HasError) {

                                this.loadedFields = myResponse.Result;
                                this.filterPickListCustomFields();
                                if (this.loadedFields != null) {
                                    this.BuildItemsSource();
                                }
                            }
                        });
                    });

                });
            }
        });
    }

}
