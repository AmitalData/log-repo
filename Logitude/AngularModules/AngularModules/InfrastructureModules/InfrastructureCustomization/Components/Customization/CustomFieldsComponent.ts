import {Component} from '@angular/core';
import {GeneralDomainService, FieldsTranslations} from '../../../../Infrastructure/Services/GeneralDomainService';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {ObjectTablePM} from '../../../../Infrastructure/EntityPMs/ObjectTablePM';
import {ObjectFieldPM} from '../../../../Infrastructure/EntityPMs/ObjectFieldPM';
import {TextCodePM} from '../../../../Infrastructure/EntityPMs/TextCodePM';
import {AppTool} from '../../../../Infrastructure/Tools';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';
import {ObjectFieldPMService} from '../../../../Infrastructure/Services/StandardPMs/ObjectFieldPMService';

declare var window: any;

@Component({
    moduleId: module.id,
    templateUrl: './CustomFieldsComponent.html',
})

export class CustomFieldsComponent {
    private myService: GeneralDomainService;    
    public CustomFieldsCollection: ObservableCollection;
    private loadedFields: ObjectFieldPM[];
    private _ObjectFieldPMService: ObjectFieldPMService;
    public IsAddButtonEnabled: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.myService = new GeneralDomainService();
        this.CustomFieldsCollection = new ObservableCollection([]);
        this._ObjectFieldPMService = new ObjectFieldPMService();   
    }

    private ObjectTableId: string;
    private ObjectTableName: string;
    SetWindowArgs(args: any) {

        this.ObjectTableId = args['ObjectTableId'];
        this.ObjectTableName = args['ObjectTableName'];

        this.myService.GetCustomFieldsByTableId(this.ObjectTableId).subscribe(myResult => {
            var myResponse: ServiceResponse = myResult;
            if (!myResponse.HasError) {

                this.loadedFields = myResponse.Result;
                if (this.loadedFields != null) {
                    this.BuildItemsSource();
                }
            }
        });
        //this.BuildItemsSource();
    }
    
    private BuildItemsSource() {

        this.CustomFieldsCollection = new ObservableCollection(this.loadedFields);

        var fieldsCount = (this.ObjectTableName == "Shipment" || this.ObjectTableName == "Master") ? 40 : 10;
        if (this.ObjectTableName == "Quote") fieldsCount = 20;

        this.IsAddButtonEnabled = this.CustomFieldsCollection.Length < fieldsCount ? true : false;

        //var objectTablePM: ObjectTablePM;
        //var tableName: string;
        //this.Tabs = [];

        //objectTablePM = window.ObjectTables.filter(d => d.Id == this.ObjecttableId)[0];
        //if (objectTablePM != null) {
        //    this.Tabs.push(new TabItem(objectTablePM));
        //}

        //var tableIds: string[] = [];
        //var mulityList: ObjectFieldPM[] = window.ObjectFields.filter(d => d.ObjectTableId == this.ObjecttableId && d.IsMulti);

        //mulityList.forEach((item) => {
        //    var index = tableIds.indexOf(item.MultiTableId);

        //    if (index == -1) {
        //        tableIds.push(item.MultiTableId);

        //        objectTablePM = window.ObjectTables.filter(d => d.Id == item.MultiTableId)[0];
        //        if (objectTablePM != null) {
        //            this.Tabs.push(new TabItem(objectTablePM));
        //        }
        //    }
        //});

        //this.SelectedTabItem = this.Tabs[0];
    }

    AddCustomField() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Add New Custom Field"; 
        var windowArgs: any = {};
        this.myService = new GeneralDomainService();
        this.myService.GetFieldDataTypes().subscribe(myResult => {
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
                logWindow.WindowArgs = windowArgs;
                logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/AddEditCustomFieldComponent');
                logWindow.WindowClosed.subscribe((event: any) => {
                    this.myService.GetCustomFieldsByTableId(this.ObjectTableId).subscribe(myResult => {
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
        logWindow.Title = "Add New Custom Field";
        var windowArgs: any = {};
        this.myService = new GeneralDomainService();
        this.myService.GetFieldDataTypes().subscribe(myResult => {
            var myResponse: ServiceResponse = myResult;
            if (!myResponse.HasError) {
                this._ObjectFieldPMService.get(item.Id).subscribe(field => {
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
                    logWindow.WindowArgs = windowArgs;
                    logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/AddEditCustomFieldComponent');
                    logWindow.WindowClosed.subscribe((event: any) => {
                        this.myService.GetCustomFieldsByTableId(this.ObjectTableId).subscribe(myResult => {
                            var myResponse: ServiceResponse = myResult;
                            if (!myResponse.HasError) {

                                this.loadedFields = myResponse.Result;
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

    CancelClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    
}
