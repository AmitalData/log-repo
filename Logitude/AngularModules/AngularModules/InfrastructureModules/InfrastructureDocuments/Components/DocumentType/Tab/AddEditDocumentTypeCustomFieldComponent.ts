declare var System: any;
declare var window: any;
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Component, OnInit}  from '@angular/core';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {DocumentTypeCustomFieldPM} from '../../../../../Common/EntityPMs/DocumentTypeCustomFieldPM';
import {DocumentTypeCustomFieldService} from '../../../../../Common/Services/ExtendedPMs/DocumentTypeCustomFieldService';
import {FieldDataTypePM} from '../../../../../Common/EntityPMs/FieldDataTypePM';
import {ClassLevelValidator} from '../../../../../Infrastructure/Validators/ClassLevelValidator';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';
import {UIProperty, UIProperties}  from '../../../../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {DocumentTypeCustomFieldsViewModel} from '../ViewModel/DocumentTypeCustomFieldsViewModel';
import {Cloner} from '../../../../../Infrastructure/Utilities/Cloner';

@Component({
    moduleId: module.id,
    selector: 'AddEditDocumentTypeCustomField',
    templateUrl: './AddEditDocumentTypeCustomFieldComponent.html',
    providers: [DocumentTypeCustomFieldService],
})

export class AddEditDocumentTypeCustomFieldComponent extends BaseComponent implements OnInit {
    public EntityPM: DocumentTypeCustomFieldPM;
    public DataViewModel: any;
    public Mode: string;
    SelectedFieldDataTypeCode: FieldDataTypePM;
    validator: ClassLevelValidator;
    ShowMultiLineCheckBox: boolean;
    MultiLine: boolean;
    public FieldDataTypeLists: FieldDataTypePM[];
    public ValidationErrorsList: string[];

    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs , public _documentTypeCustomFieldService: DocumentTypeCustomFieldService) {
        super();
        this.validator = new ClassLevelValidator();

    }

    ngOnInit() {
       

    }



    Run() {
    



    }

    name: string;
    datatype: string;
    defultvalue: string;
    inactive: boolean;
    required: boolean;
   
    SetWindowArgs(args: any) {
    
        this.EntityPM = args.EntityPM;
        this.DataViewModel = args.DataViewModel;
        this.Mode = args.Mode;
        this.FieldDataTypeLists = args.FieldDataTypeLists;
        
        this.MultiLine = this.EntityPM.MultiLine;
        if (this.FieldDataTypeLists) {
            this.SelectedFieldDataTypeCode = this.FieldDataTypeLists.filter(d=> d.Code == this.EntityPM.FieldDataTypeCode)[0];
            if (this.Mode == "Add") {
                this.SelectedFieldDataTypeCode = this.FieldDataTypeLists[0];
                if (this.SelectedFieldDataTypeCode) {
                    this.EntityPM.FieldDataTypeCode = this.SelectedFieldDataTypeCode.Code;
                }
            }
        }
        this.ShowMultiLineCheckBox = this.MultiLine;


        this.name = this.EntityPM.Name;
        this.datatype = this.EntityPM.FieldDataTypeCode;
        this.defultvalue = this.EntityPM.DefaultValue;
        this.inactive = this.EntityPM.InActive;
        this.required = this.EntityPM.IsRequired;
      
    }

    ComboBoxFieldDataTypeCodeValueChanged(event) {
        if (event) {
            this.SelectedFieldDataTypeCode = event;
            this.EntityPM.FieldDataTypeCode = event.Code;
            if (this.EntityPM.FieldDataTypeCode == "Text") {
                this.ShowMultiLineCheckBox = true;

            }
            else {
                this.MultiLine = false;
                this.ShowMultiLineCheckBox = false;

            }
        }
    }


    MultiLineChange() {
  
        this.MultiLine = !this.MultiLine;
    }




    SaveButtonClicked() {
        this.ValidationErrorsList = [];

        if (this.EntityPM.Name) {
            if (!this.EntityPM.FieldCode) {
                this.EntityPM.FieldCode = this.EntityPM.Name.replace(" ", "");
            }
        }


        var errorsArray = this.validator.Validate("DocumentTypeCustomField", this.EntityPM);
        if (errorsArray.length > 0) {
            errorsArray.forEach((item) => {
                this.ValidationErrorsList.push(item);
            });
        }



        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");
            if (this.Mode == "Add") {
                this._documentTypeCustomFieldService.Insert(this.EntityPM).subscribe(res => {

                    var pmResponse: ServiceResponse = res;
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;

                        if (this.DataViewModel && this.DataViewModel.CustomFieldsLists) {
                            var item = new DocumentTypeCustomFieldsViewModel(myResult);
                            this.DataViewModel.CustomFieldsLists.push(item);
                            this.DataViewModel.SelectedDocumentTypeCustomFieldsViewModel = item;
                        }

                    }
                    this.Close();
                });


            }
            else if (this.Mode == "Edit") {
                this._documentTypeCustomFieldService.update(this.EntityPM).subscribe(res => {

                    var pmResponse: ServiceResponse = res;
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        if (this.DataViewModel && this.DataViewModel.CustomFieldsLists) {
                            this.DataViewModel.CustomFieldsLists = this.DataViewModel.CustomFieldsLists.filter(d=> d.Id != myResult.Id);
                            var item = new DocumentTypeCustomFieldsViewModel(myResult);
                            this.DataViewModel.CustomFieldsLists.push(item);
                            this.DataViewModel.SelectedDocumentTypeCustomFieldsViewModel = item;
                        }

                    }
                    this.Close();
                });
            }
        }

    }
    

    CancelButtonClicked() {
        if (this.Mode == "Edit") {
            this.EntityPM.Name = this.name;
            this.EntityPM.FieldDataTypeCode = this.datatype;
            this.EntityPM.DefaultValue = this.defultvalue;
            this.EntityPM.InActive = this.inactive;
            this.EntityPM.IsRequired = this.required;

        }
        this.Close();
    }


    Close() {
        this.CurrentSession.CurrentWindow.StopBusyIndicator();
        this.CurrentSession.CloseCurrentWindow();
    }



}






