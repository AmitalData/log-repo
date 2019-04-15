
import { Component, ViewChild, ViewContainerRef } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { QuestionnairePM } from '../../../../CRM/EntityPMs/QuestionnairePM';
import { QuestionnaireQuestionPM } from '../../../../CRM/EntityPMs/QuestionnaireQuestionPM';
import { ServiceArgs } from '../../../../Infrastructure/DataContracts/ServiceArgs';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';

import { QuestionnaireQuestionViewModel } from './AddEditQuestionnaireComponent';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { Cloner } from '../../../../Infrastructure/Utilities/Cloner';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import {CustomPickListList} from '../../../../Infrastructure/EntityLists/CustomPickListList';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import {ApiQueryFilters, FilterItem} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {CustomPickListListService} from '../../../../Infrastructure/Services/StandardLists/CustomPickListListService';


@Component({
    moduleId: module.id,
    templateUrl: './AddEditQuestionnaireQuestionComponent.html',
})
export class AddEditQuestionnaireQuestionComponent {
    private _customPickListListService = new CustomPickListListService();
    public LabelColumnWidth: number = 170;
    public ControlColumnWidth: number = 220;

    public ValidationErrorsList: string[] = [];
    public EntityPM: QuestionnaireQuestionPM;
    public DataContext: QuestionnaireQuestionViewModel;

    public ObjectTableName: string = "QuestionnaireQuestion";

    public QuestionTypesList: QuestionType[];
    public TenantCustomPickLists: CustomPickListList[] = [];
    public CustomPickLists: string[] = [];
    //public SelectedType: QuestionType;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

        
    }

    private questionTypeCode: string = "";
    public get QuestionTypeCode() {
        if (this.EntityPM) {
            this.questionTypeCode = this.EntityPM.QuestionTypeCode;
        }

        return this.questionTypeCode;

    }
    public set QuestionTypeCode(newValue: string) {
        if (this.QuestionTypeCode != newValue) {
            this.questionTypeCode = newValue;
            this.EntityPM.QuestionTypeCode = newValue;

        }
    }


    private isMandatory: boolean = false;
    public get IsMandatory() {
        if (this.EntityPM) {
            this.isMandatory = this.EntityPM.IsMandatory;
        }

        return this.isMandatory;

    }
    public set IsMandatory(newValue: boolean) {
        if (this.IsMandatory != newValue) {
            this.isMandatory = newValue;
            this.EntityPM.IsMandatory = newValue;

        }
    }

    private question: string = "";
    public get Question() {
        if (this.EntityPM) {
            this.question = this.EntityPM.Question;
        }

        return this.question;

    }
    public set Question(newValue: string) {
        if (this.Question != newValue) {
            this.question = newValue;
            this.EntityPM.Question = newValue;

        }
    }



    private pickListCode: string = "";
    public get PickListCode() {
        if (this.EntityPM) {
            this.pickListCode = this.EntityPM.PickListCode;
        }

        return this.pickListCode;

    }
    public set PickListCode(newValue: string) {
        if (this.pickListCode != newValue) {
            this.pickListCode = newValue;
            this.EntityPM.PickListCode = newValue;

        }
    }
  

    SetWindowArgs(args: any) {

        if (args != null) {

            this.CurrentSession.StartBusyIndicatorLoading();
            this.DataContext = args.QuestionnaireQuestionViewModel;
            this.EntityPM = this.DataContext.EntityPM;

            this.BuildQuestionTypes();

            var filters = new ApiQueryFilters();
            filters.Tenant = SessionLocator.Tenant;
            filters.SortDirection = "Ascending";
            filters.PageIndex = 0;
            filters.SortBy = 'Value';
            filters.GetAll = true;
            filters.addAdditionalFilter("IsMultipleChoice", true, null, null, "Equals", false, false, false, null, false, true);
            this._customPickListListService.getByFilters(filters).subscribe(response => {
                this.TenantCustomPickLists = response.Result;
                this.CurrentSession.StopBusyIndicator();
                if (this.TenantCustomPickLists != null) {
                    this.TenantCustomPickLists.forEach(p => {
                        if (this.CustomPickLists.indexOf(p.Code) === - 1) {
                            this.CustomPickLists.push(p.Code);
                        }
                    });
                }


            });

        }
    }


    private selectedType: QuestionType;
    public get SelectedType() {
        if (this.EntityPM) {
            this.selectedType = this.QuestionTypesList.filter(t => t.Code === this.EntityPM.QuestionTypeCode)[0];
        }

        return this.selectedType;

    }
    public set SelectedType(newValue: QuestionType) {
        if (this.SelectedType != newValue) {
            this.selectedType = newValue;
            this.EntityPM.QuestionTypeCode = newValue.Code;
            //this.DataContext.QuestionTypeName = 

        }
    }

    onTypeSelected(code: string) {
        this.DataContext.QuestionTypeCode = code;
    }

    //public QuestionTypeCodeLabel: string;
    //public QuestionLabel: string;
    //public IsMandetoryLabel: string;
    
    //SetLabels() {
    //    this.QuestionTypeCodeLabel = TextCodeTranslator.Translate('QuestionnaireQuestion.F.QuestionTypeCode');
    //    this.QuestionLabel = TextCodeTranslator.Translate('QuestionnaireQuestion.F.Question');
    //    this.IsMandetoryLabel = TextCodeTranslator.Translate('QuestionnaireQuestion.F.IsMandetory');
        
    //}


    AddEditPickListCommand(type:string) {

        var logWindow = new LogitudeWindow();
        logWindow.Title = (type + " Custom Pick List");
        logWindow.Width = 780;
        logWindow.Height = 500;

        var windowArgs: any = {};
        windowArgs.IsMultipleChoice = true;
        windowArgs.IsNewMode = true;

        if (type == "Edit") {
            windowArgs.IsNewMode = false;
            if (this.PickListCode != null) {
                windowArgs.PickListCode = this.PickListCode;
            }
        }

        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./CRMModules/CRMOthers/Components/Questionnaire/AddEditPickListComponent');
        logWindow.WindowClosed.subscribe(($event: any) => {
            if ($event && this.CustomPickLists) {

                if (this.CustomPickLists.indexOf($event) === - 1) {
                    this.CustomPickLists.push($event);
                    this.PickListCode = $event;
                }
            }
        });



     
    }

   
    OkButtonClicked() {

        if (this.DataContext.IsNewEntity) {
            this.EntityPM.Tenant = SessionLocator.Tenant;
            this.EntityPM.CreateDate = DateTool.GetCurrentDateAsUtc();
            this.EntityPM.CreatedByUserId = SessionLocator.LoggedUserId;
            this.EntityPM.UpdateDate = DateTool.GetCurrentDateAsUtc();
            this.EntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
            this.EntityPM.QuestionNumber = 1;
            this.EntityPM.QuestioneerId = "DUM";
        }
        else {
            this.EntityPM.UpdateDate = DateTool.GetCurrentDateAsUtc();
            this.EntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
        }

        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

         
        if (this.EntityPM.QuestionTypeCode == "MC" || this.EntityPM.QuestionTypeCode == "CL" || this.EntityPM.QuestionTypeCode == "CB") {

            if (AppTool.IsNullOrEmpty(this.EntityPM.PickListCode)) {
                errors.push("PickListCode field is required");
            }
        }
     
        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {

            if (this.DataContext.IsNewEntity) {
                //this.DataContext.IsNewEntity = false;
                //this.DataContext.QuestionnairePM.AddQuestionnaireQuestion(this.EntityPM);
                this.DataContext.parentComponent.ItemsSource.Insert(this.DataContext);
               
            }
            
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }


    private myCloner: Cloner;
    private Clone() {
        //this.myCloner = new Cloner(this.DataContext);
        //this.myCloner.AddField('PackageTypeId');
        //this.myCloner.AddField('Quantity');
        //this.myCloner.AddField('Length');
        //this.myCloner.AddField('Width');
        //this.myCloner.AddField('Height');
        //this.myCloner.AddField('Volume');
        //this.myCloner.AddField('VolumetricWeight');
        //this.myCloner.AddField('Weight');
        //this.myCloner.AddEntity(this.EntityPM);
        //this.myCloner.AddEntity(this.DataContext.ShipmentPM);
    }
    private RejectChanges() {
        //this.DataContext.ResetPackageItems();
        //this.myCloner.RejectChanges();
    }


    BuildQuestionTypes() {

        this.QuestionTypesList = [];

        var type1: QuestionType = new QuestionType("YN", "Yes/No");
        this.QuestionTypesList.push(type1);

        var type2: QuestionType = new QuestionType("HL", "Header line");
        this.QuestionTypesList.push(type2);

        var type3: QuestionType = new QuestionType("MC", "Multiple choice");
        this.QuestionTypesList.push(type3);

        var type4: QuestionType = new QuestionType("TE", "Text");
        this.QuestionTypesList.push(type4);

        var type5: QuestionType = new QuestionType("ES", "Text (Multiline)");
        this.QuestionTypesList.push(type5);

        var type6: QuestionType = new QuestionType("DA", "Date");
        this.QuestionTypesList.push(type6);

        var type7: QuestionType = new QuestionType("CB", "Check boxes");
        this.QuestionTypesList.push(type7);

        var type8: QuestionType = new QuestionType("CL", "Choose from a list");
        this.QuestionTypesList.push(type8);

        var type9: QuestionType = new QuestionType("DS", "Decimal");
        this.QuestionTypesList.push(type9);

        if (this.DataContext.IsNewEntity) {
            this.DataContext.QuestionTypeCode = this.QuestionTypesList[3].Code;
        }

    }
}

export class QuestionType {
    constructor(public Code: string, public Name: string)
    {

    }
}

