import { Component, ViewChild, ViewContainerRef } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { QuestionnairePM } from '../../../../CRM/EntityPMs/QuestionnairePM';
import { QuestionnaireQuestionPM } from '../../../../CRM/EntityPMs/QuestionnaireQuestionPM';
import { QuestionnaireAnswerPM } from '../../../../CRM/EntityPMs/QuestionnaireAnswerPM';
import { QuestionnaireAnswerLinePM } from '../../../../CRM/EntityPMs/QuestionnaireAnswerLinePM';

import { ServiceArgs } from '../../../../Infrastructure/DataContracts/ServiceArgs';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { QuestionnairePMService } from '../../../../CRM/Services/StandardPMs/QuestionnairePMService';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';

 import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { Cloner } from '../../../../Infrastructure/Utilities/Cloner';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { UIProperties, UIProperty } from '../../../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {ApiQueryFilters, FilterItem} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {CustomPickListListService} from '../../../../Infrastructure/Services/StandardLists/CustomPickListListService';
import {CustomPickListList} from '../../../../Infrastructure/EntityLists/CustomPickListList';
import {QuestionnaireAnswerPMService} from  '../../../../CRM/Services/StandardPMs/QuestionnaireAnswerPMService';
import { SessionInfo } from '../../../../Infrastructure/Utilities/SessionInfo';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {Guid} from '../../../../Infrastructure/Utilities/Guid';
@Component({
    moduleId: module.id,
    templateUrl: './QuestionnaireAnswersComponent.html',
    providers: [QuestionnairePMService, QuestionnaireAnswerPMService],
})
export class QuestionnaireAnswersComponent extends BaseComponent{


    public ValidationErrorsList: string[] = [];
    public EntityPM: QuestionnairePM;
    public UIProperties: UIProperties;
    public TenantCustomPickLists: CustomPickListList[]= [];
    //public ObjectTableName: string = "QuestionnaireQuestion";

    //public QuestionTypesList: QuestionType[];
    //public SelectedType: QuestionType;
    private ObjectTableId: string;
    private EntityId: string;
    public ItemsSource: ObservableCollection;
    public ItemsSource2: ObservableCollection;

    public IsResourcesReady: boolean = false;
    private customPickListListService: CustomPickListListService;
    public SessionIndex: number;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _QuestionnairePMService: QuestionnairePMService, private _QuestionnaireAnswerPMService: QuestionnaireAnswerPMService) {
        super();
        this.ItemsSource = new ObservableCollection([]);
        this.ItemsSource2 = new ObservableCollection([]);
        this.UIProperties = new UIProperties;
        this.customPickListListService = new CustomPickListListService();
        this.SessionIndex = SessionLocator.Index;  
    }

    private simplogWindow: LogitudeWindow;
    SetWindowArgs(args: any) {
        this.simplogWindow = this.CurrentSession.CurrentWindow;
        if (args != null) {
            this.EntityId = args.EntityId;
            this.ObjectTableId = args.ObjectTableId;
            this.EntityPM = args.EntityPM;

            this.BuildItemsSource();
           // this.LoadData();
        }
    }

    //LoadData() {
    //    this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Loading"));
    //    this._QuestionnairePMService.get(SessionLocator.TenantPM.DefaultQuestionnaireId).subscribe(response => {

    //        this.CurrentSession.CurrentWindow.StopBusyIndicator();
    //        this.EntityPM = response.Result;
    //        if (this.EntityPM) {
    //            this.simplogWindow.Title = this.EntityPM.Name;
    //            this.BuildItemsSource();
    //        }

    //    });


    //    //
    //}

    BuildItemsSource() {
        var filters = new ApiQueryFilters();
        filters.Tenant = SessionLocator.Tenant;
        filters.SortDirection = "Ascending";
        filters.PageIndex = 0;
        filters.SortBy = 'Value';
        filters.addAdditionalFilter("IsMultipleChoice", true, null, null, "Equals", false, false, false, null, false, true);

        this.customPickListListService.getAllFromCache(filters).subscribe(response => {

            var AllQuestionsArr: QuestionnaireQuestionPM[] = this.EntityPM.QuestionnaireQuestions;
           
            this.TenantCustomPickLists = response.Result;
            this.ItemsSource = new ObservableCollection([]);
            this.ItemsSource2 = new ObservableCollection([]);

            var itemsCollection: QuestionnaireAnswerViewModel[] = [];
            var itemsCollection2: QuestionnaireAnswerViewModel[] = [];


            if (!this.HasTwoColumn) {
                AllQuestionsArr.forEach((questionnaireQuestionPM) => {
                    itemsCollection.push(new QuestionnaireAnswerViewModel(questionnaireQuestionPM, this));
                });


            }
            else {
                var count = 0;
                AllQuestionsArr.forEach((questionnaireQuestionPM) => {
                    var questionVM = null;

                    if (count % 2 === 0) {
                        questionVM = new QuestionnaireAnswerViewModel(questionnaireQuestionPM, this);
                    }
                    else
                        questionVM = new QuestionnaireAnswerViewModel(questionnaireQuestionPM, this, 1);

                    itemsCollection.push(questionVM);

                    count++;
                });
            }
            this.ItemsSource.InsertCollection(itemsCollection);


            //this.ItemsSource.InsertCollection(itemsCollection);

            //if (!this.HasTwoColumn) {
            //    AllQuestionsArr.forEach((questionnaireQuestionPM) => {
            //        itemsCollection.push(new QuestionnaireAnswerViewModel(questionnaireQuestionPM, this));
            //    });

            //    this.ItemsSource.InsertCollection(itemsCollection);
            //}
            //else {
            //    var count = 0;
            //    AllQuestionsArr.forEach((questionnaireQuestionPM) => {
            //        if (count % 2 === 0) {
            //            itemsCollection.push(new QuestionnaireAnswerViewModel(questionnaireQuestionPM, this));
            //        }
            //        else
            //            itemsCollection2.push(new QuestionnaireAnswerViewModel(questionnaireQuestionPM, this));

            //        count++;
            //    });

            //    this.ItemsSource.InsertCollection(itemsCollection);
            //    this.ItemsSource2.InsertCollection(itemsCollection2);
            //}
            //AllQuestionsArr.forEach((questionnaireQuestionPM) => {
            //    itemsCollection.push(new QuestionnaireAnswerViewModel(questionnaireQuestionPM, this));
            //});

            //this.ItemsSource.InsertCollection(itemsCollection);
          

            this.IsResourcesReady = true;
        });
    }

    private hasTwoColumn: boolean = false;
    public get HasTwoColumn() {
        if (this.EntityPM) {
            this.hasTwoColumn = this.EntityPM.HasTwoColumn;
        }

        return this.hasTwoColumn;

    }
    public set HasTwoColumn(newValue: boolean) {
        if (this.hasTwoColumn != newValue) {
            this.hasTwoColumn = newValue;
            this.EntityPM.HasTwoColumn = newValue;

        }
    }

    private flowDirection: string = "ltr";
    public get FlowDirection() {
        if (this.EntityPM) {
            if (this.EntityPM.RightToLeft) {
                this.flowDirection = "rtl";
            }
        }

        return this.flowDirection;

    }
    
     

    OkButtonClicked() {

        var errors: string[] = [];
        var hasRequiredFields: boolean = false;
        var otherFieldsRequired: boolean = false;
        var textOutOfRange: boolean = false;
        this.ItemsSource.Collection.forEach((questionAnswerModel: QuestionnaireAnswerViewModel) => {
            if (questionAnswerModel.IsMandatory && AppTool.IsNullOrEmpty(questionAnswerModel.AnswerValue)) {
                hasRequiredFields = true;
            }

            if ((questionAnswerModel.QuestionTypeCode === "CB" || questionAnswerModel.QuestionTypeCode === "MC")
                && questionAnswerModel.AnswerValue === "Other" && AppTool.IsNullOrEmpty(questionAnswerModel.OtherValue)) {
                otherFieldsRequired = true;
            }

            if ((questionAnswerModel.QuestionTypeCode === "TE" || questionAnswerModel.QuestionTypeCode === "ES")
                && !AppTool.IsNullOrEmpty(questionAnswerModel.AnswerValue) && questionAnswerModel.AnswerValue.length > 1000) {
                textOutOfRange = true;
            }

        });

        if (hasRequiredFields) {
            errors.push("Some fields are required");
        }
        if (otherFieldsRequired) {
            errors.push("Please fill (other)  field");
        }
        if (textOutOfRange) {
            errors.push("Text field must be 0 - 1000  characters");
        }

        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length === 0) {

            var questionnaireAnswerPM: QuestionnaireAnswerPM = new QuestionnaireAnswerPM();
            questionnaireAnswerPM.Id = "DUM";
            
            questionnaireAnswerPM.Tenant = SessionInfo.LoggedUserTenant;
            questionnaireAnswerPM.QuestioneerId = this.EntityPM.Id;
            questionnaireAnswerPM.CreateDate = new Date();
            questionnaireAnswerPM.CreatedByUserId = SessionInfo.LoggedUserId;
            questionnaireAnswerPM.VersionNumber = this.EntityPM.VersionNumber;
            questionnaireAnswerPM.ObjectTableId = this.ObjectTableId;
            questionnaireAnswerPM.EntityId = this.EntityId;
            questionnaireAnswerPM.HasTwoColumn = this.EntityPM.HasTwoColumn;
            var Id: number = 0;
            this.ItemsSource.Collection.forEach((questionAnswerModel: QuestionnaireAnswerViewModel) => {

                var answerValue: string = questionAnswerModel.AnswerValue;

                if (questionAnswerModel.QuestionTypeCode === "MC" && questionAnswerModel.AnswerValue === "Other" && !AppTool.IsNullOrEmpty(questionAnswerModel.OtherValue)) {
                    answerValue = questionAnswerModel.OtherValue;
                }
                if (!AppTool.IsNullOrEmpty(answerValue)) {

                    var questionnaireAnswerLinePM: QuestionnaireAnswerLinePM = new QuestionnaireAnswerLinePM(questionAnswerModel.EntityPM);

                    questionnaireAnswerLinePM.Tenant = SessionInfo.LoggedUserTenant;
                    questionnaireAnswerLinePM.QuestionNumber = questionAnswerModel.EntityPM.QuestionNumber;
                    if (questionAnswerModel.QuestionTypeCode === "CB") {
                        questionAnswerModel.CheckedValues.forEach(value => {
                            if (value && value != "Other") {
                                questionnaireAnswerLinePM.AnswerValue += value + ",";
                            }
                            else
                                questionnaireAnswerLinePM.AnswerValue += questionAnswerModel.OtherValue + ",";
                        });

                        //questionnaireAnswerLinePM.AnswerValue.(",");
                    }
                    else {
                        questionnaireAnswerLinePM.AnswerValue = answerValue;
                    }

                    questionnaireAnswerLinePM.QuestionnaireAnswerId = "Dom" + Id;
                    questionnaireAnswerPM.QuestionnaireAnswerLines.push(questionnaireAnswerLinePM);

                    Id++;
                }
            });

            this._QuestionnaireAnswerPMService.insert(questionnaireAnswerPM).subscribe(response => {
                if (response.HasError === false) {
                    this.CurrentSession.CurrentWindow.Close("ok");
                }
                else {
                    this.ValidationErrorsList = response.ErrorsArray;
                }
            });
       
        }
      
    }

    CancelButtonClicked() {
        this.CurrentSession.CurrentWindow.Close("cancelled");
    }


}

export class QuestionnaireAnswerViewModel extends BaseComponent
{
    public EntityPM: QuestionnaireQuestionPM;
    public UIProperties: UIProperties;
    public Parent: QuestionnaireAnswersComponent;
    public CustomPickListItems: CustomPickListList[] = [];
    public IndexKey: string;
    constructor(questionPM: QuestionnaireQuestionPM, parent: QuestionnaireAnswersComponent, columnNumber:number = 0) {
        super();
        this.EntityPM = questionPM;
        this.UIProperties = new UIProperties;
        this.Parent = parent;
        this.ColumnNumber = columnNumber;
        this.IndexKey = Guid.newGuid();

        if (!AppTool.IsNullOrEmpty(this.EntityPM.PickListCode)) {
            this.CustomPickListItems = this.Parent.TenantCustomPickLists.filter(p => p.Code === this.EntityPM.PickListCode);
            //if (this.EntityPM.IsAddOther) {
            //    var customPickList = new CustomPickListList();
            //    customPickList.Id = "Other";
            //    customPickList.Code = "Other";
            //    customPickList.Value = "Other";
            //    customPickList.Tenant = 0;
            //    customPickList.IsMultipleChoice = true;
            //    this.CustomPickListItems.push(customPickList);
                
            //}
        }
    }

    onTextChange(newValue: any) {
        if (this.QuestionTypeCode === "MC" || this.QuestionTypeCode === "CB") {
            if (this.QuestionTypeCode === "CB") {
                this.OtherValue = newValue;
            }
            if (this.QuestionTypeCode === "MC" && this.AnswerValue === "Other") {
                this.OtherValue = newValue;
            }
        }
        else {
            this.AnswerValue = newValue;
        }
    }
    
    
    OnItemChecked(item: string) {
        this.AnswerValue = "";
        var index = this.CheckedValues.indexOf(item);
        if (index > -1) {
            if (index > -1) {
                this.CheckedValues.splice(index, 1);
            }
        }
        else {
            this.CheckedValues.push(item);
        }

        this.CheckedValues.forEach(value => {
            if (value) {
                this.AnswerValue += value + ",";
            }
        });

    }
     
    pickListItemChanged($event) {
        if ($event) {
            this.AnswerValue = $event.Value;
        }
        else {
            this.AnswerValue = null;
        }
    }


    public OtherValue: string = "";
    public AnswerValue: string = "";

    public CheckedValues: string[] = [];


    private columnNumber: number;
    public get ColumnNumber() {
        return this.columnNumber;
    }
    public set ColumnNumber(newValue: number) {
        if (this.columnNumber != newValue) {
            this.columnNumber = newValue;
        }
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


    private isAddOther: boolean = false;
    public get IsAddOther() {

        if (this.EntityPM) {
            this.isAddOther = this.EntityPM.IsAddOther;
        }

        return this.isAddOther;

    }

    
    
} 
