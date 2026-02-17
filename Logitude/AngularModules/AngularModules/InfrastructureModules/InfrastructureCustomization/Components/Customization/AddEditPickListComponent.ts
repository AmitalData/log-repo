import {Component} from '@angular/core';
import {GeneralDomainService, FieldsTranslations} from '../../../../Infrastructure/Services/GeneralDomainService';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {ObjectTablePM} from '../../../../Infrastructure/EntityPMs/ObjectTablePM';
import {ObjectFieldPM} from '../../../../Infrastructure/EntityPMs/ObjectFieldPM';
import {CustomPickListPM} from '../../../../Infrastructure/EntityPMs/CustomPickListPM';
import {TextCodePM} from '../../../../Infrastructure/EntityPMs/TextCodePM';
import {AppTool} from '../../../../Infrastructure/Tools';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';
import {PickListGeneralEntitiesArgs} from '../../../../Infrastructure/DataContracts/PickListGeneralEntitiesArgs';
import { CachedDataManager } from '../../../../Infrastructure/Utilities/CachedDataManager';

declare var window: any;

@Component({
    moduleId: module.id,
    templateUrl: './AddEditPickListComponent.html',
})

export class AddEditPickListComponent extends BaseComponent {
    DataContext: AddEditPickListComponent = this;
    GeneralEntitiesArgs: PickListGeneralEntitiesArgs = new PickListGeneralEntitiesArgs();
    private myService: GeneralDomainService;
    public PickListsList: ObservableCollection;
    private loadedFields: ObjectFieldPM[];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.myService = new GeneralDomainService();
        this.PickListsList = new ObservableCollection([]);
        this.GeneralEntitiesArgs = new PickListGeneralEntitiesArgs();
        this.GeneralEntitiesArgs.CustomPickListPMs = [];
        this.GeneralEntitiesArgs.RemovedCustomPickListPMs = [];
    }

    private PickListCode: string;
    isNew: boolean = true;
    IsMultipleChoice: boolean = false;
    SetWindowArgs(windowArgs: any) {
        this.PickListCode = windowArgs.Code;
        this.isNew = windowArgs.isNew;
        if (this.isNew) {
            this.PickListsList = new ObservableCollection([]);
        }
        else {
            this.myService.GetCustomPickListsByCode(this.PickListCode).subscribe(myResult => {
                var myResponse: ServiceResponse = myResult;
                if (!myResponse.HasError) {

                    this.loadedFields = myResponse.Result;
                    if (this.loadedFields != null) {
                        this.BuildItemsSource();
                    }
                }
            });
        }

    }

    private BuildItemsSource() {

        this.PickListsList = new ObservableCollection(this.loadedFields);
    }

    AddPickListItem() {
        var pm: CustomPickListPM = new CustomPickListPM();


        pm.Code = this.PickListCode;
        pm.Tenant = SessionLocator.Tenant;
        pm.IsMultipleChoice = this.IsMultipleChoice;
        this.PickListsList.Insert(pm);

        //this.context.CustomPickListPMs.Add(pm);
        //RefreshPickListsList();
        //var logWindow = new LogitudeWindow();
        //logWindow.Title = "Add New Custom Field";
        //var windowArgs: any = {};
        //this.myService = new GeneralDomainService();
        //this.myService.GetFieldDataTypes().subscribe(myResult => {
        //    var myResponse: ServiceResponse = myResult;
        //    if (!myResponse.HasError) {
        //        windowArgs.IsNew = true;
        //        var objectField = new ObjectFieldPM();
        //        objectField.Tenant = SessionLocator.Tenant;
        //        objectField.ObjectTableId = this.ObjecttableId;
        //        objectField.IsCustom = true;
        //        objectField.DisplayInEntityVariables = true;
        //        objectField.DisplayInList = true;
        //        objectField.CanFilter = true;
        //        windowArgs.objectField = objectField;
        //        windowArgs.DataTypeCollection = myResponse.Result;
        //        logWindow.WindowArgs = windowArgs;
        //        logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/AddEditCustomFieldComponent');
        //        logWindow.WindowClosed.subscribe((event: any) => {
        //            this.myService.GetCustomFieldsByTableId(this.ObjecttableId).subscribe(myResult => {
        //                var myResponse: ServiceResponse = myResult;
        //                if (!myResponse.HasError) {

        //                    this.loadedFields = myResponse.Result;
        //                    if (this.loadedFields != null) {
        //                        this.BuildItemsSource();
        //                    }
        //                }
        //            });
        //        });

        //    }
        //});

    }

    DeleteLine(item) {
        this.PickListsList.Remove(item);
        if (this.GeneralEntitiesArgs == null) {
            this.GeneralEntitiesArgs = new PickListGeneralEntitiesArgs();
        }
        if (this.GeneralEntitiesArgs.RemovedCustomPickListPMs == null){
            this.GeneralEntitiesArgs.RemovedCustomPickListPMs = [];
        }
        var mappedEntity: CustomPickListPM;
        mappedEntity = this.MapJsonToEntityPM(item, false);
        this.GeneralEntitiesArgs.RemovedCustomPickListPMs.push(mappedEntity);
        //var logWindow = new LogitudeWindow();
        //logWindow.Title = "Add New Custom Field";
        //var windowArgs: any = {};
        //this.myService = new GeneralDomainService();
        //this.myService.GetFieldDataTypes().subscribe(myResult => {
        //    var myResponse: ServiceResponse = myResult;
        //    if (!myResponse.HasError) {
        //        windowArgs.IsNew = false;
        //        var objectField = item;
        //        //objectField.Tenant = SessionLocator.Tenant;
        //        //objectField.ObjectTableId = this.ObjecttableId;
        //        //objectField.IsCustom = true;
        //        //objectField.DisplayInEntityVariables = true;
        //        //objectField.DisplayInList = true;
        //        //objectField.CanFilter = true;
        //        windowArgs.objectField = objectField;
        //        windowArgs.DataTypeCollection = myResponse.Result;
        //        logWindow.WindowArgs = windowArgs;
        //        logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/AddEditCustomFieldComponent');
        //        logWindow.WindowClosed.subscribe((event: any) => {
        //            this.myService.GetCustomFieldsByTableId(this.ObjecttableId).subscribe(myResult => {
        //                var myResponse: ServiceResponse = myResult;
        //                if (!myResponse.HasError) {

        //                    this.loadedFields = myResponse.Result;
        //                    if (this.loadedFields != null) {
        //                        this.BuildItemsSource();
        //                    }
        //                }
        //            });
        //        });

        //    }
        //});
    }

    CancelClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    ValidationErrorsList: any[];
    SaveChanges() {
        this.ValidationErrorsList = [];
        if (this.PickListsList != null && this.PickListsList.Length > 0) {
            this.ValidationErrorsList = [];
            this.PickListsList.Collection.forEach(list => {
                if (this.PickListsList.Collection.filter(p => p.Value == list.Value && p.Id != list.Id).length > 0) {
                    this.ValidationErrorsList.push("Some values are duplicated!");
                }

                if (AppTool.IsNullOrEmpty(list.Value)) {
                    this.ValidationErrorsList.push("PickList value is required");
                }
                else if (list.Value.Length > 1000) {
                    this.ValidationErrorsList.push("PickList value length should be less than 1000 character");
                }
            });

            if (this.ValidationErrorsList.length == 0) {
                this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving changes....");
                if (this.GeneralEntitiesArgs == null) {
                    this.GeneralEntitiesArgs = new PickListGeneralEntitiesArgs();
                }
                this.GeneralEntitiesArgs.CustomPickListPMs = [];
                //this.GeneralEntitiesArgs.RemovedCustomPickListPMs = [];
                this.PickListsList.Collection.forEach(list => {
                    var mappedEntity: CustomPickListPM;
                    mappedEntity = this.MapJsonToEntityPM(list, false);
                    this.GeneralEntitiesArgs.CustomPickListPMs.push(mappedEntity);
                });
                if (this.isNew) {
                    this.myService.insertPickListGeneralEntities(this.GeneralEntitiesArgs).subscribe(myResult => {
                        this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        this.CurrentSession.CloseCurrentWindow();
                        CachedDataManager.RefreshTableData("CustomPickList", true);
                        //var myResponse: ServiceResponse = myResult;
                        //if (!myResponse.HasError) {

                        //    this.loadedFields = myResponse.Result;
                        //    if (this.loadedFields != null) {
                        //        this.BuildItemsSource();
                        //    }
                        //}
                    });
                }
                else {
                    this.myService.updatePickListGeneralEntities(this.GeneralEntitiesArgs).subscribe(myResult => {
                        this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        this.CurrentSession.CloseCurrentWindow();
                        CachedDataManager.RefreshTableData("CustomPickList", true);

                        this.GeneralEntitiesArgs = new PickListGeneralEntitiesArgs();
                        //var myResponse: ServiceResponse = myResult;
                        //if (!myResponse.HasError) {

                        //    this.loadedFields = myResponse.Result;
                        //    if (this.loadedFields != null) {
                        //        this.BuildItemsSource();
                        //    }
                        //}
                    });
                }
                //SubmitOperation op = this.context.SubmitChanges();
                //op.Completed += new EventHandler(op_Completed);
            }
        }
        else {
            this.ValidationErrorsList.push("The pick list must have at least one item!");
        }
    }

    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: CustomPickListPM = null) {


        if (!entityPM) {

            entityPM = new CustomPickListPM();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {

                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


        entityPM.IsDirty = false;

         
            entityPM.OldEntityPM = null;
         
        return entityPM;
    }

}
