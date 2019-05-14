declare var window: any;
import {Component, OnInit, AfterViewInit} from '@angular/core';
import {ServiceArgs} from '../../../../Infrastructure/DataContracts/ServiceArgs';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ValidationSummary} from '../../../../Controls/All/ValidationSummary';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {AppTool} from '../../../../Infrastructure/Tools';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {CustomsRequiredFieldList} from '../../../../Customs/EntityLists/CustomsRequiredFieldList';
import { CustomsRequierdFieldsWebService } from '../../../../Customs/Services/WebServices/CustomsRequierdFieldsWebService';


@Component({
    moduleId: module.id,
    selector: 'AddEditRequiredFieldsComponent',
    templateUrl: './AddEditRequiredFieldsComponent.html',
})

export class AddEditRequiredFieldsComponent extends BaseComponent {
    public DataContext: AddEditRequiredFieldsComponent = this;
    public ObjectTableName: string;
    public ValidationErrorsList: string[];
    OriginalFieldsList: ObservableCollection;
    FieldsList: ObservableCollection;
    SelectedObjectFields: CustomsRequiredFieldList[] = [];

    customsRequierdFieldsWebService: CustomsRequierdFieldsWebService = new CustomsRequierdFieldsWebService();
    _EntityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _entityResourceService: EntityResourceService) {
        super();
        this.FieldsList = new ObservableCollection([]);
        this.OriginalFieldsList = new ObservableCollection([]);

    }

    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.ObjectTableName = args.SelectedObjectTableName;
            //this.SelectedObjectFields = args.SelectedObjectFields;
            if (args.SelectedObjectFields) {
                args.SelectedObjectFields.forEach((el) => {
                    this.SelectedObjectFields.push(el);
                });
            }

            //this.CurrentSession.StartBusyIndicatorLoading();
            this._EntityResourceService.getEntityResourceByTableName(this.ObjectTableName, 0).subscribe((res: any) => {
                this.GetObjectFields();
            });
        }
    }

    GetObjectFields() {
        var objectTable = window.ObjectTables.filter(x => x.Name === this.ObjectTableName)[0];
        var objectFields: any[] = window.ObjectFields.filter(x => x.ObjectTableId == objectTable.Id && (!x.IsMulti && x.FieldName != "ImporterId" && x.FieldName != "TransferImporterId" && x.FieldName != "EntitleImporterId"));

        this.FieldsList.Clear();
        var items = [];

        objectFields.forEach((objectField) => {

            var requierdField = new CustomsRequiredFieldList();
            requierdField.ObjectfieldId = objectField.Id;
            requierdField.ObjectFieldName = objectField.FieldName;
            requierdField.ObjectTableId = objectField.ObjectTableId
            requierdField.Tenant = SessionLocator.Tenant;

            var item = new RequiredFieldItemModel(objectField, requierdField);
            item.TranslatedName = TextCodeTranslator.Translate(objectField.FullNameTextCodeCode);

            if (this.SelectedObjectFields.find(d => d.ObjectfieldId == objectField.Id)) {
                item.Active = true;
            }
            items.push(item);

        });

        this.FieldsList.InsertCollection(items);
        this.OriginalFieldsList.InsertCollection(items);
        //this.CurrentSession.StopBusyIndicator();

        //BuildSelectedList();
    }

    TextChanged(text: string) {
        if (text) {
            //console.log("Searching for " + text + " ...");
            var originalList = this.OriginalFieldsList.Collection;
            var filteredList = originalList.filter(d => d.TranslatedName.toLocaleLowerCase().includes(text.trim().toLocaleLowerCase()));
            this.FieldsList.Clear();
            this.FieldsList.InsertCollection(filteredList);
        } else {
            this.FieldsList.InsertCollection(this.OriginalFieldsList.Collection);
        }
    }
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        this.CurrentSession.StartBusyIndicatorSaving();

        var items: RequierdFieldObject[] = [];
        // 1- 
        this.OriginalFieldsList.Collection.forEach((field: RequiredFieldItemModel) => {
            var i = new RequierdFieldObject();
            i.ObjectfieldId = field.ObjectfieldId;
            i.ObjectTableId = field.ObjectField.ObjectTableId;
            i.ObjectFieldName = field.ObjectField.FieldName;
            i.Active = field.Active;
            items.push(i);
        });


        this.customsRequierdFieldsWebService.PostRequiredFields(items).subscribe((response: ServiceResponse) => {
            var res = response.Result;
            console.log("[Response] customsRequierdFieldsWebService.PostRequiredFields: ", res);


            this.CurrentSession.StopBusyIndicator();
            this.CurrentSession.CloseCurrentWindow();
        });

    }

    //row selection on grid
    public SelectedRow: any = null;
    OnRowSelected(itemComponent: any) {
        this.SelectedRow = itemComponent;
    }

    CheckBoxChanged(event, itemModel: RequiredFieldItemModel) {
        console.log("<CheckBoxChanged> ", event, itemModel);
        if (itemModel) {

            if (event == true) {
                this.SelectedObjectFields.push(itemModel.RequierdField);
                this.FieldsList.Collection.find(d => d.Name == itemModel.ObjectField.FullNameTextCodeCode).Active = true;
                this.OriginalFieldsList.Collection.find(d => d.Name == itemModel.ObjectField.FullNameTextCodeCode).Active = true;
            }
            else {
                var index = this.SelectedObjectFields.findIndex(d => d.ObjectFieldName == itemModel.ObjectField.FieldName);
                this.SelectedObjectFields.splice(index, 1);
                this.FieldsList.Collection.find(d => d.Name == itemModel.ObjectField.FullNameTextCodeCode).Active = false;
                this.OriginalFieldsList.Collection.find(d => d.Name == itemModel.ObjectField.FullNameTextCodeCode).Active = false;


            }

        }
    }

}

export class RequiredFieldItemModel extends BaseComponent {
    
    public DataContext = this;

    constructor(public ObjectField: any, public RequierdField: CustomsRequiredFieldList) {
        super();
        this.ObjectfieldId = ObjectField.Id;
    }

    //#region Properties

    objectfieldId: string;
    get ObjectfieldId() { return this.objectfieldId; }
    set ObjectfieldId(value: string) {
        if (this.objectfieldId != value) {
            this.objectfieldId = value;
        }
    }

    get Name() { return this.ObjectField.FullNameTextCodeCode; }
    set Name(value: string) {
        if (this.ObjectField.FullNameTextCodeCode != value) {
            this.ObjectField.FullNameTextCodeCode = value;
        }
    }

    translatedName: string;
    get TranslatedName() { return this.translatedName; }
    set TranslatedName(value: string) {
        if (this.translatedName != value) {
            this.translatedName = value;

        }
    }

    active: boolean = false;
    get Active() { return this.active; }
    set Active(value: boolean) {
        if (this.active != value) {
            this.active = value;

        }
    }

    //#endregion
}

export class RequierdFieldObject {
    ObjectfieldId: string;
    ObjectTableId: string;
    ObjectFieldName: string;
    Active: boolean;
}
