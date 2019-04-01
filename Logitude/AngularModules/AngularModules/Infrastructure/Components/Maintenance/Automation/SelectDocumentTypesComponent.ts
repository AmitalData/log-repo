import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import 'rxjs/add/operator/map';
import {Component, OnInit }  from '@angular/core';
import {DocumentTypeList} from '../../../../Common/EntityLists/DocumentTypeList';
import {AutomationCondition} from '../../../../Infrastructure/DataContracts/AutomationCondition';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {AppTool} from '../../../../Infrastructure/Tools'
import {AddEditAutomationsComponent} from '../../../../Infrastructure/Components/Maintenance/Automation/AddEditAutomationsComponent';
import {Guid} from '../../../../Infrastructure/Utilities/Guid';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {FollowUpDocumentTypeList} from '../../../../Infrastructure/DataContracts/AutomationFollowUp';



@Component({
    moduleId: module.id,
    selector: 'SelectDocumentTypesComponent',
    templateUrl: './SelectDocumentTypesComponent.html',


})

export class SelectDocumentTypesComponent extends BaseComponent implements OnInit {

    DocumentTypes: DocumentTypeList[] = [];

    DocumentTypeLists: SelectDocumentTypeViewModel[] = [];
    AddEditAutomationsComponent: AddEditAutomationsComponent;

    SelectedDocumentTypeLists: FollowUpDocumentTypeList[] = [];

    Area: string = "";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    
    
    }

    ngOnInit() {


    }
    SetWindowArgs(args: any) {
    
        this.AddEditAutomationsComponent = args.AddEditAutomationsComponent;

        if (this.AddEditAutomationsComponent) {
            if (this.AddEditAutomationsComponent.ResultCodeSelected) {
                if (this.AddEditAutomationsComponent.ResultCodeSelected.Code == "DOCOUTFOLLOWUP") this.Area = "DocOut";
                else if (this.AddEditAutomationsComponent.ResultCodeSelected.Code == "DOCINFOLLOWUP") this.Area = "DocIn";

            }

            if (this.AddEditAutomationsComponent.AutomationFollowUp) {
                this.SelectedDocumentTypeLists = this.AddEditAutomationsComponent.AutomationFollowUp.DocumentTypeLists;
                if (!this.SelectedDocumentTypeLists) this.SelectedDocumentTypeLists = [];
                this.SelectedDocumentTypeLists = this.SelectedDocumentTypeLists.filter(d => d.Area == this.Area);
            }

            if (this.AddEditAutomationsComponent.AllDocumentTypeLists) {
                if (this.Area == "DocIn") {
                    this.DocumentTypes = this.AddEditAutomationsComponent.AllDocumentTypeLists.filter(d => d.IsDocIn && d.ObjectTableId == this.AddEditAutomationsComponent.ObjectTableId);
                } else {

                    this.DocumentTypes = this.AddEditAutomationsComponent.AllDocumentTypeLists.filter(d => d.IsDocOut && d.ObjectTableId == this.AddEditAutomationsComponent.ObjectTableId);
                }

                this.DocumentTypes.forEach((item) => {
                    this.DocumentTypeLists.push(new SelectDocumentTypeViewModel(item, this.Area, this));

                });

            }

        }

      
    }

   




    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    SaveButtonClicked() {
        this.AddEditAutomationsComponent.AutomationFollowUp.DocumentTypeLists = this.SelectedDocumentTypeLists;

        this.CurrentSession.CurrentWindow.Close("Save");
     
    }





}

class SelectDocumentTypeViewModel {

    Name: string; 
    DocumentType: DocumentTypeList;
    ViewModel: SelectDocumentTypesComponent;
    constructor(documentType: DocumentTypeList, area: string, viewModel: SelectDocumentTypesComponent ) {
        this.Name = documentType.Name;
        this.ViewModel = viewModel;
        
        this.DocumentType = documentType;

    

    }
    get IsSelect() {

        var isSelect: boolean = false;
        if (this.ViewModel.SelectedDocumentTypeLists && this.DocumentType) {
            if (this.ViewModel.SelectedDocumentTypeLists.filter(d => d.Id == this.DocumentType.Id)[0]) {
                isSelect = true;
            }

        }
        return isSelect;


    }
    set IsSelect(newValue: boolean) {
        this.ViewModel.AddEditAutomationsComponent.IsChangeAutomation = true;

        if (this.ViewModel.SelectedDocumentTypeLists.filter(d => d.Id == this.DocumentType.Id)[0]) {
            this.ViewModel.SelectedDocumentTypeLists = this.ViewModel.SelectedDocumentTypeLists.filter(d => d.Id != this.DocumentType.Id);
        }
        else {
            var document: FollowUpDocumentTypeList = new FollowUpDocumentTypeList();
            document.Id = this.DocumentType.Id;
            document.Name = this.DocumentType.Name;
            document.Area = this.ViewModel.Area;
            this.ViewModel.SelectedDocumentTypeLists.push(document);
        }
    }





}
