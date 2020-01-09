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
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {AppTool} from '../../../../Infrastructure/Tools';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';

import {CustomsRequiredFieldList} from '../../../../Customs/EntityLists/CustomsRequiredFieldList';
import { CustomsRequierdFieldsWebService } from '../../../../Customs/Services/WebServices/CustomsRequierdFieldsWebService';



@Component({
    moduleId: module.id,
    selector: 'RequiredFieldsComponent',
    templateUrl: 'RequiredFieldsComponent.html',
})

export class RequiredFieldsComponent extends BaseComponent {
    public DataContext: RequiredFieldsComponent = this;
    public ObjectTableName: string = "Customs.CustomsRequiredField";
    public ValidationErrorsList: string[];

    customsRequierdFieldsWebService: CustomsRequierdFieldsWebService = new CustomsRequierdFieldsWebService();
    _EntityResourceService: EntityResourceService = new EntityResourceService();

    SelectedTable: any;
    OriginalTablesList: any[] = [];
    TablesList: any[] = [];

    FieldsList: CustomsRequiredFieldList[] = [];

    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _entityResourceService: EntityResourceService) {
        super();
        this.GetCustomsObjectTables();
        //this.BuildTablesList();
    }
    GetCustomsObjectTables() {
        this.customsRequierdFieldsWebService.GetSomeObjectTables().subscribe((response: ServiceResponse) => {
            var res = response.Result;
            console.log("[Response] customsRequierdFieldsWebService.GetSomeObjectTables: ", res);
            if (!AppTool.IsNullOrEmpty(res)) {
                this.TablesList = [];
                this.TablesList = res;
                this.BuildTablesList();
            }
        });
    }

    BuildTablesList() {
        //this.TablesList = [];
        //this.TablesList.push({ Name: "demo Declaration"});
        //this.TablesList.push({ Name: "demo Supplier Invoices Modification"});
        //this.TablesList.push({ Name: "demo Supplier invoices connected Declaration"});
        //this.TablesList.push({ Name: "demo Physical Checks"});
        //this.TablesList.push({ Name: "demo Procedural Faults"});
        //this.TablesList.push({ Name: "demo Bla Bla Bla"});
        //this.TablesList.push({ Name: "table number 7" });
        this.TablesList.forEach((el) => {
            el["TranslatedName"] = TextCodeTranslator.TranslateTable(el.Name);
        });
        this.OriginalTablesList = this.TablesList;
    }

    AddRemoveFieldsButtonClicked() {
        var window = new LogitudeWindow();

        if (this.SelectedTable) {
            window.Title = TextCodeTranslator.Translate("Customs.General.O.AddRemoveRequiredFields");
            window.ShowCloseButton = true;
            window.WindowArgs = {
                SelectedObjectTableName: this.SelectedTable.Name,
                SelectedObjectFields: this.FieldsList,
            }
            window.Show("./CustomsModules/CustomsMaintenance/Components/RequiredFields/AddEditRequiredFieldsComponent");
            window.WindowClosed.subscribe(($event: any) => {
                this.TableNameClicked(this.SelectedTable);
            });
        }
        else {
            var msg = new MessageWindow();
            msg.Show(TextCodeTranslator.Translate("Customs.General.O.SelectObjectTable"));
        }
    }

    TableNameSearchTextChanged(event: string) {
        if (!AppTool.IsNullOrEmpty(event)) {
            this.TablesList = [];
            this.OriginalTablesList.forEach((el) => {
                if (el.TranslatedName.toLowerCase().includes(event.toLowerCase()))
                    this.TablesList.push(el);
            });

        } else {
            this.TablesList = this.OriginalTablesList;
        }
    }
    IsNoFields: boolean = false;

    TableNameClicked(table: any) {
        if (!AppTool.IsNullOrEmpty(table)) {
            this.SelectedTable = table;

            this._EntityResourceService.getEntityResourceByTableName(table.Name, 0).subscribe((res: any) => {
                this.customsRequierdFieldsWebService.GetCustomsRequiredFieldListsByObjectTable(table.Id)
                    .subscribe((response: ServiceResponse) => {
                        var res = response.Result;
                        this.IsNoFields = false;

                        console.log("[Response] GetCustomsRequiredFieldListsByObjectTable: ", res);
                        if (!AppTool.IsNullOrEmpty(res)) {
                            this.FieldsList = [];
                            this.FieldsList = res;

                            if (this.FieldsList.length == 0) {
                                this.IsNoFields = true;
                            } else {
                                this.TranslateFieldsNames();
                            }

                        }
                });
            });


        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }


    TranslateFieldsNames() {
        this.FieldsList.forEach((field) => {
            var objectField = window.ObjectFields.find(x => x.FieldCode == field.ObjectfieldCode);
            field.ObjectFieldName = TextCodeTranslator.Translate(objectField.FullNameTextCodeCode);
        });
    }


}
