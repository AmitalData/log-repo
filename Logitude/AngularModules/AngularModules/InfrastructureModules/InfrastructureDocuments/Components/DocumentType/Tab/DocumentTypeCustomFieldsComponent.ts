declare var System: any;
declare var window: any;
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Component, OnInit}  from '@angular/core';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {DocumentTypePM} from '../../../../../Common/EntityPMs/DocumentTypePM';
import {DocumentTypeCopyPM} from '../../../../../Common/EntityPMs/DocumentTypeCopyPM';
import {LogitudeWindow} from '../../../../../Controls/Windows/LogitudeWindow';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';
import {DocumentTypeCustomFieldsViewModel} from '../ViewModel/DocumentTypeCustomFieldsViewModel';
import {DocumentTypeCustomFieldPM} from '../../../../../Common/EntityPMs/DocumentTypeCustomFieldPM';
import {UIProperty, UIProperties}  from '../../../../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';
import {FieldDataTypeService} from '../../../../../Common/Services/ExtendedPMs/FieldDataTypeService';
import {FieldDataTypePM} from '../../../../../Common/EntityPMs/FieldDataTypePM';

@Component({
    moduleId: module.id,
    selector: 'DocumentTypeCustomFields',
    templateUrl: './DocumentTypeCustomFieldsComponent.html',
    providers: [FieldDataTypeService],
})

export class DocumentTypeCustomFieldsComponent extends BaseComponent implements OnInit {
    public EntityPM: DocumentTypePM;
    public IsShowButtonDelete: boolean = false;
    public EditingIsEnabled: boolean = false;
    public CustomFieldsLists: DocumentTypeCustomFieldsViewModel[];
    public FullCustomFieldsLists: DocumentTypeCustomFieldsViewModel[];
    public FieldDataTypeLists: FieldDataTypePM[];
    

    SelectedDocumentTypeCustomFieldsViewModel: DocumentTypeCustomFieldsViewModel;


    private _entityResourceService: EntityResourceService = new EntityResourceService();
    public IsVisibile: boolean = false;
    constructor(public entityArgs: EntityArgs, public _fieldDataTypeService: FieldDataTypeService ) {
        super();


    }

    ngOnInit() {
        this.FullCustomFieldsLists = [];
        this._entityResourceService.getEntityResourceByTableName("DocumentTypeCustomField", 0).subscribe(response => {
            this.IsVisibile = true;
            this.EntityPM = this.entityArgs.EntityPM;
            if (this.EntityPM) {
                this.Run();

            }
        });



    }



    Run() {

        this.FieldDataTypeLists = [];
        this.CustomFieldsLists = [];
        this.FullCustomFieldsLists = [];
        if (this.EntityPM.DocumentTypeCustomFields) {
  
            this.EntityPM.DocumentTypeCustomFields.forEach((customFields) => {
                var item = new DocumentTypeCustomFieldsViewModel(customFields);
                this.CustomFieldsLists.push(item);
                this.FullCustomFieldsLists.push(item);
                

            });
        }

        this._fieldDataTypeService.GetFieldDataTypes(this.EntityPM.Tenant).subscribe(res => {
            var pmResponse: ServiceResponse = res;

            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    this.FieldDataTypeLists = myResult;

                }
            }



        });

    }


    onSearchTextChangeEvent(search) {

        if (search) {
            if (search != "Search" && this.FullCustomFieldsLists) {
                this.CustomFieldsLists = this.FullCustomFieldsLists.filter(d => d.EntityPM.Name && d.EntityPM.Name.toUpperCase().indexOf(search.toUpperCase()) > -1);
            
            }
        }
        else this.CustomFieldsLists = this.FullCustomFieldsLists;
    }
 
    AddCustomFieldButtonClicked() {

        var entityPM: DocumentTypeCustomFieldPM = new DocumentTypeCustomFieldPM();
        entityPM.Tenant = SessionLocator.Tenant;
        entityPM.DocumentTypeId = this.EntityPM.Id;
        this.ShowAddEditDocumentTypeCustomFieldComponent(entityPM, "Add", "Add Custom Fields");

    }

    EditCustomFieldButtonClicked() {



        if (this.SelectedDocumentTypeCustomFieldsViewModel) {
            this.ShowAddEditDocumentTypeCustomFieldComponent(this.SelectedDocumentTypeCustomFieldsViewModel.EntityPM , "Edit" , "Edit Custom Fields");
        }
    }

    ShowAddEditDocumentTypeCustomFieldComponent(entityPM: DocumentTypeCustomFieldPM, mode: string, title: string) {

        var windowArgs: any = {};
        windowArgs.EntityPM = entityPM;
        windowArgs.DataViewModel = this;
        windowArgs.Mode = mode;
        windowArgs.FieldDataTypeLists = this.FieldDataTypeLists;
        
        var logWindow = new LogitudeWindow();
        logWindow.Width = 600;
        logWindow.Height = 400;
        logWindow.Title = title;
        
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentType/Tab/AddEditDocumentTypeCustomFieldComponent");
    }
    







}






