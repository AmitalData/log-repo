declare var window: any;
import {Component, OnInit, ViewChild, ViewContainerRef, ChangeDetectorRef, OnDestroy} from '@angular/core';
import {SessionLocator} from '../../Utilities/SessionLocator';
import {AppTool} from '../../Tools';
import {ObjectTablePM} from '../../EntityPMs/ObjectTablePM';
import {ObjectFieldPM} from '../../EntityPMs/ObjectFieldPM';
import {CustomFieldClass} from '../../DataContracts/CustomFieldClass';
import {FieldValueResolver} from '../../Utilities/FieldValueResolver';
import {ObjectsLocator} from '../../Locators/ObjectsLocator';

@Component({
    moduleId: module.id,
    templateUrl: "./ObjectFieldTemplate.html",
    selector: 'ObjectFieldTemplate',
    inputs: ['ObjectTable', 'ObjectField', 'FieldName', 'Entity', 'IsHeaderScreenTemplate', 'IsListColumnCellTemplate', 'IsListColumnHeaderTemplate', 'IsSpotLightTemplate','SpotlightDataTemplate'],
})

// https://github.com/angular/angular/issues/10762
// http://stackoverflow.com/questions/39794156/angular2-dynamically-added-elements

export class ObjectFieldTemplate implements OnInit, OnDestroy  {
    public Entity: any = null;
    public CustomField: any = null;
    public ObjectTable: ObjectTablePM = null;
    public ObjectField: ObjectFieldPM = null;
    public FieldName: string = null;
    public FieldValue: any = null;
    public HasTemplate: boolean = false;
    public DataTypeCode: string = null;
    public DigitsAfterPoints: string = "n0";
    public IsAutoFormat: boolean = false;    
    public IsLookUp: boolean = false;
    public LookUpFieldValue: string = null;
    public IsHeaderScreenTemplate: boolean = false;
    public IsListColumnCellTemplate: boolean = false;
    public IsListColumnHeaderTemplate: boolean = false;
    public IsSpotLightTemplate: boolean = false;
    public SpotlightDataTemplate: string;
    public Direction: string = ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;
    public TextAlign = this.Direction == 'rtl' ? 'right' : 'left';
    public NumberFieldTextAlign: string = "right";
    @ViewChild('Template', { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;
    public isRTL: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private CD: ChangeDetectorRef) {
    }

    ngOnInit() {
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");

        if (this.ObjectField != null && this.IsSpotLightTemplate == false) {
            //this.IsCustom = this.ObjectField.IsCustom;
            this.HasTemplate = this.ObjectField.HasTemplate;
            this.DataTypeCode = this.ObjectField.DataTypeCode;
            this.DigitsAfterPoints = "n"+this.ObjectField.DigitsAfterPoint;

            if (this.IsHeaderScreenTemplate) {
                this.FieldName = this.ObjectField.PMPropertyPath;
                this.NumberFieldTextAlign = "left";
            }

            else {
                this.FieldName = this.ObjectField.FieldName;
            }

            if (AppTool.IsNullOrEmpty(this.FieldName)) {
                this.FieldName = this.ObjectField.FieldName;
            }

            switch (this.DataTypeCode) {
                case "Decimal":
                case "Double":
                case "Boolean":
                case "Date":
                case "DateTime":
                case "Integer":
                    {
                        if (this.HasTemplate != true) {
                            this.IsAutoFormat = true;
                        }

                        break;
                    }

                case "LookUp":
                case "PickList":
                    {
                        this.IsLookUp = true;
                        break;
                    }
            }

            if (this.HasTemplate) {
                this.LoadTemplate();
            }

            else {
                if (this.Entity != null) {
                    if (this.IsHeaderScreenTemplate && this.IsLookUp) {

                        if (this.ObjectField.IsCustom) {
                            this.CustomField = this.Entity[this.FieldName];
                            if (this.CustomField) {
                                this.FieldValue = this.CustomField.Value;
                            }
                        }

                        else {
                            this.FieldValue = this.Entity[this.FieldName];
                        }

                        if (!AppTool.IsNullOrEmpty(this.FieldValue)) {
                            if (this.ObjectField.LookUpTableId) {

                                var ObjectTable = window.ObjectTables.filter(x => x.Id === this.ObjectField.LookUpTableId)[0];
                                if (ObjectTable) {
                                    var moduleName = ObjectTable.ClientModuleName;
                                    var objectTableName = ObjectTable.Name;
                                    if (objectTableName.indexOf('Customs.') > -1) {
                                        objectTableName = objectTableName.split('.')[1];
                                    }
                                    var servicename = objectTableName + "ListService"; 
                                    var servicelink = './' + moduleName + '/Services/StandardLists/' + servicename;

                                    return new Promise((resolve, reject) => {
                                        SessionLocator.DynamicLoader.GetInstance(servicelink).then((myService: any) => {
                                            if (myService) {
                                                if (ObjectTable.CacheOnClient)
                                                    myService.getSingleFromCache(this.FieldValue).subscribe((myResponse: any) => {
                                                        if (!myResponse.HasError) {
                                                            if (myResponse.Result) {

                                                                var myResultValue = myResponse.Result["Name"];

                                                                if (AppTool.IsNullOrEmpty(myResultValue)) {
                                                                    myResultValue = myResponse.Result["EnglishName"];
                                                                }

                                                                this.LookUpFieldValue = myResultValue;
                                                            }
                                                        }
                                                    });
                                                else
                                                    myService.getSingle(this.FieldValue).subscribe((myResponse: any) => {
                                                        if (!myResponse.HasError) {
                                                            if (myResponse.Result) {

                                                                var myResultValue = myResponse.Result["Name"];

                                                                if (AppTool.IsNullOrEmpty(myResultValue)) {
                                                                    myResultValue = myResponse.Result["EnglishName"];
                                                                }

                                                                this.LookUpFieldValue = myResultValue;
                                                            }
                                                        }
                                                    });
                                            }
                                        });
                                    });
                                }
                            }
                        }
                    }

                    else {
                        if (this.ObjectField.IsCustom) {
                            this.CustomField = this.Entity[this.FieldName];
                            if (this.CustomField) {

                                if (this.ObjectField.DataTypeCode == 'Boolean') {
                                    if (this.IsHeaderScreenTemplate) {
                                        this.FieldValue = (this.CustomField.Value == "True" || this.CustomField.Value == "true") ? true : false;
                                    }

                                    else {
                                        this.FieldValue = (this.CustomField == "True" || this.CustomField == "true") ? true : false;
                                    }
                                }

                                else {
                                    if (this.IsHeaderScreenTemplate) {
                                        this.FieldValue = this.CustomField['Value'];
                                    }

                                    else {
                                        this.FieldValue = this.CustomField;
                                    }
                                }
                            }
                        }

                        else {
                            this.FieldValue = this.Entity[this.FieldName];
                        }
                    }

                    this.DetectChanges();
                }
            }
        }

        if (this.IsSpotLightTemplate == true) {
            this.LoadSpotLightTemplate();
        }
    }

    LoadTemplate() {
        if (this.HasTemplate) {
            if (this.viewContainerRef) {
                if (this.Entity != null && this.ObjectTable != null && this.ObjectField != null) {

                    var myComponentPath = null;
                    if (this.ObjectTable.ClientModuleName == "Infrastructure") {
                        myComponentPath = "./" + this.ObjectTable.ClientModuleName + "/Components/Templates/InfrastructureFieldTemplateComponent";
                    }

                    else {
                        myComponentPath = "./" + this.ObjectTable.ClientModuleName + "/Components/Templates/FieldTemplateComponent";
                    }

                    SessionLocator.DynamicLoader.Load(myComponentPath, this.viewContainerRef)
                        .then(cmpRef => {
                            cmpRef.instance.Run({ Entity: this.Entity, FieldName: this.FieldName, ObjectTableName: this.ObjectTable.Name, IsHeaderScreenTemplate: this.IsHeaderScreenTemplate });
                            this.DetectChanges();                           
                        });
                }
            }
        }
    }

    LoadSpotLightTemplate() {
            if (this.viewContainerRef) {
                if (this.Entity != null && this.ObjectTable != null) {

                    var myComponentPath = null;
                    if (this.ObjectTable.ClientModuleName == "Infrastructure") {
                        myComponentPath = "./" + this.ObjectTable.ClientModuleName + "/Components/Templates/InfrastructureFieldTemplateComponent";
                    }

                    else {
                        myComponentPath = "./" + this.ObjectTable.ClientModuleName + "/Components/Templates/FieldTemplateComponent";
                    }

                    SessionLocator.DynamicLoader.Load(myComponentPath, this.viewContainerRef)
                        .then(cmpRef => {
                            cmpRef.instance.Run({ Entity: this.Entity, FieldName: this.FieldName, ObjectTableName: this.ObjectTable.Name, IsHeaderScreenTemplate: this.IsHeaderScreenTemplate, IsSpotLightTemplate: this.IsSpotLightTemplate, SpotlightDataTemplate: this.SpotlightDataTemplate });

                            this.DetectChanges();

                            this.CurrentSession.SessionEvent.subscribe(s => {
                                if (s == "SpotLightDetectChanges") {
                                    this.DetectChanges();
                                }
                            });

                        });
                }
            }
    }

    DetectChanges() {
        if (this.CD) { 
            var isDestroyed: boolean = this.CD['destroyed'];
            if (!isDestroyed) {
                //console.log("DetectChanges Templates") 
                this.CD.detectChanges();
            }
        }
    }

    ngOnDestroy() {
        this.CD = null;
    }
}
