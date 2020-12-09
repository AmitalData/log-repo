declare var window: any;
import {ObjectTablePM} from '../../../../Infrastructure/EntityPMs/ObjectTablePM';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {DocumentObjectFieldsRowViewModel} from './ViewModel/DocumentObjectFieldsRowViewModel';
import {ObjectFieldPM} from '../../../../Infrastructure/EntityPMs/ObjectFieldPM';
import {Component, OnInit, Output, EventEmitter}  from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {SessionInfo} from '../../../../Infrastructure/Utilities/SessionInfo';
import {AppTool} from '../../../../Infrastructure/Tools';
import {FormControl}   from '@angular/forms'; 
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';

@Component({
    
    selector: 'DocumentObjectFields',
    templateUrl: './DocumentObjectFieldsComponent.html',
})

export class DocumentObjectFieldsComponent implements OnInit {

    public ObjectTablesList: ObjectTablePM[];
    SelectedObjectTable: ObjectTablePM;
    ObjectTypeField: string = "";
    ObjectTableId: string;
    SelectSystemDataObjectFieldsRowViewModel: DocumentObjectFieldsRowViewModel;
    SelectObjectDataFieldsRowViewModel: DocumentObjectFieldsRowViewModel;
    public ObsList: DocumentObjectFieldsRowViewModel[];
    public ObsListAll: DocumentObjectFieldsRowViewModel[];
    public DataSource: DocumentObjectFieldsRowViewModel[];
    IsTextSearchEnabled: boolean;
    public SystemObsList: DocumentObjectFieldsRowViewModel[];
    public SystemDataSource: DocumentObjectFieldsRowViewModel[];

    public AllSystemDataSourceViewsLists: DocumentObjectFieldsRowViewModel[];
    public AllObjectDataSourceViewsLists: DocumentObjectFieldsRowViewModel[];

    public EntityResourceService: EntityResourceService;

    IsShowTabObjectField = false;
    public SearchTextValue: FormControl;
    SearchText: string;
    SelectedTabCode: string;
    InSertDataFieldType: string;
    public HideSystemDataTab: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
      
        
    }

    ngOnInit(


    ) {

      this.SearchTextValue = new FormControl();
      this.SearchTextValue.valueChanges.pipe(
            debounceTime(500),
            distinctUntilChanged())
            .subscribe((search: string): any => {
                this.Search(search);
            });

 

    }


    SetWindowArgs(args: any) {

        this.ObjectTableId = args.ObjectTableId;
        this.ObjectTypeField = args.ObjectTypeField;
        this.InSertDataFieldType = args.InSertDataFieldType;
        this.HideSystemDataTab = args.HideSystemDataTab;

     if (AppTool.IsNullOrEmpty(this.ObjectTypeField)) {
         if (this.InSertDataFieldType == "From" || this.InSertDataFieldType == "ReplyTo" || this.InSertDataFieldType == "CC" || this.InSertDataFieldType == "BCC" || this.InSertDataFieldType == "To") this.ObjectTypeField = "Emails"; 
        } 
          

        this.Run();


    }

    Run() {

        this.ObjectTablesList = window.ObjectTables;
        this.ObsList = new Array<DocumentObjectFieldsRowViewModel>();
        this.ObsListAll = new Array<DocumentObjectFieldsRowViewModel>();
        this.SystemObsList = new Array<DocumentObjectFieldsRowViewModel>();
        this.SystemDataSource = new Array<DocumentObjectFieldsRowViewModel>();  
        this.AllSystemDataSourceViewsLists = new Array<DocumentObjectFieldsRowViewModel>();
        this.AllObjectDataSourceViewsLists = new Array<DocumentObjectFieldsRowViewModel>();
        this.EntityResourceService = new EntityResourceService();
        if (!this.ObjectTypeField) {
            this.ObjectTypeField = null;
        }
        // Abed



        this.SelectedObjectTable = window.ObjectTables.filter(d=> d.Id == this.ObjectTableId)[0];

        this.ObjectTableSelectionChangedMethod(this.SelectedObjectTable);
    }
    objectFieldsList: ObjectFieldPM[];  
    ObjectTableSelectionChangedMethod(objectTable: ObjectTablePM) {
        this.objectFieldsList = new Array<ObjectFieldPM>();

        //Tab Entity
        if (objectTable != null) {

            this.IsShowTabObjectField = true;
            this.SelectedTabCode = "DAF";
            this.ObsList = new Array<DocumentObjectFieldsRowViewModel>();
            if (this.ObjectTypeField) {
                if (this.ObjectTypeField == "Emails") {

                    this.objectFieldsList = window.ObjectFields.filter(f => f.ObjectTableId == objectTable.Id && (f.PMPropertyPath != null || f.ListPropertyPath != null) && (f.DataTypeCode == this.ObjectTypeField || f.ObjectTable_LookUpTableName == "User" || f.ObjectTable_LookUpTableName == "Contact") && f.FieldName != "TimeFrameFilter" && !f.DisplayOnly && f.DisplayInEntityVariables == true);
                }
                else if (this.ObjectTypeField == "LookUpContactOrUser") {

                    this.objectFieldsList = window.ObjectFields.filter(f => f.ObjectTableId == objectTable.Id && (f.PMPropertyPath || f.ListPropertyPath) && (f.FieldName == "TicketReplyto" || f.ObjectTable_LookUpTableName == "User" || f.ObjectTable_LookUpTableName == "Contact") && f.FieldName != "TimeFrameFilter" && !f.DisplayOnly && f.DisplayInEntityVariables == true);
                }
                else if (this.ObjectTypeField == "DocuemntFileName") {
                    this.CurrentSession.StartBusyIndicator("Loading...");
                    this.EntityResourceService.getEntityResourceByTableName("DocumentsFiling").subscribe((response: any) => {
                        this.FillDocumentTableObjectFieldsList(objectTable);
                    });
                }
            }

            else {
                if (objectTable.Name == "LogitudeMessagesTransmissionLog") {
                  
                    this.objectFieldsList = window.ObjectFields.filter(f=> f.ObjectTableId == objectTable.Id && (f.PMPropertyPath != null || f.ListPropertyPath != null) && f.FieldName != "TimeFrameFilter" && f.DataTypeCode != "Emails" && f.DisplayInEntityVariables == true);
                }

                else {
                    this.objectFieldsList = window.ObjectFields.filter(f=> f.ObjectTableId == objectTable.Id && (f.PMPropertyPath != null || f.ListPropertyPath != null) && f.FieldName != "TimeFrameFilter" && f.DataTypeCode != "Emails" && !f.DisplayOnly && f.DisplayInEntityVariables == true);
                   
                }
            }

            this.FillDataSource();
        }
        else this.SelectedTabCode = "SAF";

        this.TextSearchEnabled();

        //Tab SystemData
        if (!this.HideSystemDataTab) {
            var systemDataObjectFieldsList = new Array<ObjectFieldPM>();
            var table = window.ObjectTables.filter(d => d.Name == "SystemData")[0];
            if (this.ObjectTypeField) {

                if (this.ObjectTypeField == "Emails") {
                    systemDataObjectFieldsList = window.ObjectFields.filter(f => f.ObjectTableId == table.Id && (f.Tenant == SessionInfo.LoggedUserTenant || f.Tenant == 0) && (f.DataTypeCode == this.ObjectTypeField || f.ObjectTable_LookUpTableName == "User" || f.ObjectTable_LookUpTableName == "Contact" || f.FieldName == "Supportemail"));
                }

                if (this.ObjectTypeField == "LookUpContactOrUser") {

                    systemDataObjectFieldsList = window.ObjectFields.filter(f => f.ObjectTableId == table.Id && (f.Tenant == SessionInfo.LoggedUserTenant || f.Tenant == 0) && (f.ObjectTable_LookUpTableName == "User" || f.ObjectTable_LookUpTableName == "Contact" || f.FieldName == "Supportemail"));
                }


            }
            else {



                systemDataObjectFieldsList = window.ObjectFields.filter(f => f.ObjectTableId == table.Id && (f.Tenant == SessionInfo.LoggedUserTenant || f.Tenant == 0) && f.DataTypeCode != "Emails");

            }
            var order = 0;
            systemDataObjectFieldsList.forEach((field) => {
                var view = new DocumentObjectFieldsRowViewModel(field, field.FieldName, this.ObjectTypeField);
                view.Order = order;
                order += 1;
                this.SystemObsList.push(view);
            });

            if (this.InSertDataFieldType == "TextArea") {

                this.SystemObsList = this.SystemObsList.filter(d => d.FieldName.toLowerCase() != "logo" && d.FieldName.toLowerCase() != "signature" && d.FieldName.toLowerCase() != "smalllogo" && d.FieldName.toLowerCase() != "widelogo");
            }
        
            var smailLogo = this.SystemObsList.filter(d => d.FieldName == "SmallLogo")[0];
            if (smailLogo) {
                var wideLogo = this.SystemObsList.filter(d => d.FieldName == "WideLogo")[0];
                if (wideLogo) {
                    wideLogo.Order = (smailLogo.Order + 1);
                    order = (wideLogo.Order + 1);
                    this.SystemObsList.filter(d => d.Order > smailLogo.Order && d.FieldName != "WideLogo").forEach((field) => {
                        field.Order = order;
                        order += 1;
                    });

                }
            }
            this.SystemDataSource = this.SystemObsList.sort((a, b) => { return a.Order - b.Order });
   

        }

        //  Search

    }




    private FillDocumentTableObjectFieldsList(objectTable: ObjectTablePM) {
        this.objectFieldsList = window.ObjectFields.filter(f => f.ObjectTableId == objectTable.Id && (f.PMPropertyPath != null || f.ListPropertyPath != null) && f.DisplayInDocumentReferences);
        let documentsObjectTable = window.ObjectTables.filter(t => t.Name == "DocumentType" || t.Name == "DocumentsFiling");
        documentsObjectTable.forEach(documentTable => {
            let documentTableObjectFieldsList = window.ObjectFields.filter(f => f.ObjectTableId == documentTable.Id && f.DisplayInDocumentReferences);
            this.objectFieldsList = this.objectFieldsList.concat(documentTableObjectFieldsList);
        });
        this.CurrentSession.StopBusyIndicator();
        this.FillDataSource();
        this.TextSearchEnabled();
    }

    private TextSearchEnabled() {
        if (this.objectFieldsList.length > 0)
            this.IsTextSearchEnabled = true;
        else
            this.IsTextSearchEnabled = false;
    }

    private FillDataSource() {
        this.ObsList = [];
        this.ObsListAll = [];
        this.objectFieldsList.forEach((field) => {

            if (field.FieldName == "OBLTypeCode") {
                var d = "f";
            }

            var view = new DocumentObjectFieldsRowViewModel(field, field.FieldName, this.ObjectTypeField);
            this.ObsList.push(view);
            this.ObsListAll.push(view);
            this.DataSource = this.ObsList;
        });
    }

    SystemDataSourceChangeSelected(selectedItem: DocumentObjectFieldsRowViewModel) {

        this.SelectSystemDataObjectFieldsRowViewModel = selectedItem;
        this.SelectObjectDataFieldsRowViewModel = null;
        


        var item = this.AllSystemDataSourceViewsLists.filter(d=> d.Id == selectedItem.Id)[0];

        if (!item) {
            this.AllSystemDataSourceViewsLists.push(selectedItem);
        }

        this.AllSystemDataSourceViewsLists.forEach((field) => {
            field.DivSelectBackgroud = "#ffffff";
        });
        selectedItem.DivSelectBackgroud = "#B6E0F5";


    }
  

    ObjectDataSourceChangeSelected(selectedItem: DocumentObjectFieldsRowViewModel) {
        this.SelectSystemDataObjectFieldsRowViewModel = null;
        this.SelectObjectDataFieldsRowViewModel = selectedItem;
        var item = this.AllSystemDataSourceViewsLists.filter(d=> d.Id == selectedItem.Id)[0];

        if (!item) {
            this.AllSystemDataSourceViewsLists.push(selectedItem);
        }

        this.AllSystemDataSourceViewsLists.forEach((field) => {
            field.DivSelectBackgroud = "#ffffff";
        });
        selectedItem.DivSelectBackgroud = "#B6E0F5";


    }








    Search(textsearch: string) {
        this.SearchText = textsearch;
        this.ObsList = new Array<DocumentObjectFieldsRowViewModel>(); 
        var views = new Array<DocumentObjectFieldsRowViewModel>(); 

        if (textsearch) {
            this.DataSource = this.ObsList = this.ObsListAll.filter(d => this.IsExistInSearchText(d, textsearch));
        }
        else {
            this.DataSource = this.ObsListAll;
        }


    }




    displayListOnly: boolean = false;
  
    private IsExistInSearchText(d: DocumentObjectFieldsRowViewModel, textsearch: string) {
        let isExistField: boolean = false;
        let fieldTextCode: string = TextCodeTranslator.Translate(d.FullNameTextCodeCode);
        if (d.ObjectTableId != this.ObjectTableId) {
            fieldTextCode = d.ObjectTableName + ' ' + TextCodeTranslator.Translate(d.FullNameTextCodeCode)
        }
        isExistField = fieldTextCode.toLowerCase().indexOf(textsearch.toLowerCase()) > -1

        return isExistField;
    }

    DisplayListFieldsOnly() {

        this.displayListOnly = true;
        this.ObsList = new Array<DocumentObjectFieldsRowViewModel>();
        this.ObsListAll = new Array<DocumentObjectFieldsRowViewModel>();

        if (this.SelectedObjectTable) {
            var table = window.ObjectTables.filter(d=> d.Name == this.SelectedObjectTable.Name)[0];
            this.objectFieldsList = window.ObjectFields.filter(f => f.ObjectTableId == table.Id && (f.PMPropertyPath != null || f.ListPropertyPath != null) && f.FieldName != "TimeFrameFilter" && f.IsMulti == false && f.DisplayOnly == false && f.DisplayInEntityVariables == true);
            var views = new Array<DocumentObjectFieldsRowViewModel>();

            this.objectFieldsList.forEach((field) => {

                var view = new DocumentObjectFieldsRowViewModel(field, field.FieldName, this.ObjectTypeField);
                view.DisplayListOnly = true;
                views.push(view);
                this.ObsList.push(view);
                this.ObsListAll.push(view);
            });
            this.DataSource = this.ObsList;

            if (this.objectFieldsList.length > 0) {
                this.IsTextSearchEnabled = true;
                return;
            }
        }
    }

    IsSearchIconVisible: boolean = true;
    OnFucos() {
        this.IsSearchIconVisible = false;
    }
    OnLostFucos() {

        if (this.SearchText) {
            this.IsSearchIconVisible = false;
        }
        else this.IsSearchIconVisible = true;

    }




    TextSelected: string;
    SaveButtonClicked() {
        this.TextSelected = "";

            if (this.SelectObjectDataFieldsRowViewModel) {

                var selectedField = this.SelectObjectDataFieldsRowViewModel;
                if (selectedField.ObjectTableId != this.ObjectTableId)
                    this.TextSelected = "[" + selectedField.ObjectTableName + selectedField.ResultFieldName + "]";
                else
                    this.TextSelected = "[" + selectedField.ResultFieldName + "]";

                if (this.InSertDataFieldType == "FroalaEditor") this.TextSelected = "<span>" + this.TextSelected + "</span>";
            }
            else if (this.SelectSystemDataObjectFieldsRowViewModel) {
                var selectedField = this.SelectSystemDataObjectFieldsRowViewModel;

                if (this.InSertDataFieldType != "FroalaEditor") {
                    if (selectedField.FieldName && (selectedField.FieldName.toLowerCase() == "logo" || selectedField.FieldName.toLowerCase() == "smalllogo" || selectedField.FieldName.toLowerCase() == "signature")) {
                        return;
                    }
                    this.TextSelected = "[SystemData." + selectedField.ResultFieldName + "]"
                }

                else {
                    this.TextSelected = "[SystemData." + selectedField.ResultFieldName + "]"

                    this.TextSelected=   "<span>" + this.TextSelected + "</span>"
                }

            }

            this.CurrentSession.CurrentWindow.Close(this.TextSelected);
    }



    CloseButtonClicked() {
        this.CurrentSession.CurrentWindow.Close("");
    }





}
