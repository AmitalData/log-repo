import { Component, ViewChild, ViewContainerRef } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { QuestionnairePM } from '../../../../CRM/EntityPMs/QuestionnairePM';
import { QuestionnaireQuestionPM } from '../../../../CRM/EntityPMs/QuestionnaireQuestionPM';
import { ServiceArgs } from '../../../../Infrastructure/DataContracts/ServiceArgs';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { QuestionnairePMService } from '../../../../CRM/Services/StandardPMs/QuestionnairePMService';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools'; 
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { Guid } from '../../../../Infrastructure/Utilities/Guid';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditQuestionnaireComponent.html',
    providers: [QuestionnairePMService],
})
export class AddEditQuestionnaireComponent extends BaseComponent{
    private CurrentSession = SessionLocator.SelectedSession;
    private _entityResourceService: EntityResourceService = new EntityResourceService();

    public ValidationErrorsList: string[] = [];
    public EntityPM: QuestionnairePM;
    public SelectedQuestion: QuestionnaireQuestionViewModel;
    //public QuestionsList: QuestionnaireQuestionViewModel[] = [];
    public ObjectTableName: string = "Questionnaire";
    public DataContext: AddEditQuestionnaireComponent = this;
    public ItemsSource: ObservableCollection;
    public IsNewEntity: boolean = false;
    public IsResourcesReady: boolean = false;


    private name: string = "";
    public get Name() {
        if (this.EntityPM) {
            this.name = this.EntityPM.Name;
        }

        return this.name;

    }
    public set Name(newValue: string) {
        if (this.Name != newValue) {
            this.name = newValue;
            this.EntityPM.Name = newValue;

        }
    }

    private rightToLeft: boolean = false;
    public get RightToLeft() {
        if (this.EntityPM) {
            this.rightToLeft = this.EntityPM.RightToLeft;
        }

        return this.rightToLeft;

    }
    public set RightToLeft(newValue: boolean) {
        if (this.RightToLeft != newValue) {
            this.rightToLeft = newValue;
            this.EntityPM.RightToLeft = newValue;

        }
    }


    private hasTwoColumn: boolean = false;
    public get HasTwoColumn() {
        if (this.EntityPM) {
            this.hasTwoColumn = this.EntityPM.HasTwoColumn;
        }

        return this.hasTwoColumn;

    }
    public set HasTwoColumn(newValue: boolean) {
        if (this.HasTwoColumn != newValue) {
            this.hasTwoColumn = newValue;
            this.EntityPM.HasTwoColumn = newValue;

        }
    }



    private inActive: boolean = false;
    public get InActive() {
        if (this.EntityPM) {
            this.inActive = this.EntityPM.InActive;
        }

        return this.inActive;

    }
    public set InActive(newValue: boolean) {
        if (this.InActive != newValue) {
            this.inActive = newValue;
            this.EntityPM.InActive = newValue;

        }
    }

    constructor(private _QuestionnairePMService: QuestionnairePMService) {
        super();
    }
    private CurrentVersionNumber: number;
    SetWindowArgs(args: any) {
        if (args != null) {
        
            this._entityResourceService.getEntityResourceByTableName("QuestionnaireQuestion").subscribe(response => {
                if (args.IsNew) {

                    this.IsNewEntity = true;
                    this.EntityPM = this._QuestionnairePMService.GetNewEntityPM();
                    this.EntityPM.Tenant = SessionLocator.Tenant;
                    this.EntityPM.Name = this.Name;
                    this.EntityPM.CreateDate = DateTool.GetCurrentDateAsUtc();
                    this.EntityPM.CreatedByUserId = SessionLocator.LoggedUserId;
                    this.EntityPM.UpdateDate = DateTool.GetCurrentDateAsUtc();
                    this.EntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
                    this.EntityPM.VersionNumber = 1;
                    this.EntityPM.RightToLeft = false;
                    this.EntityPM.HasTwoColumn = false;

                    this.ItemsSource = new ObservableCollection([]);
                    //this.BuildItemsSource();
                    this.IsResourcesReady = true;
                }
                else {

                    this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Loading"));
                    this._QuestionnairePMService.get(args.EntityId).subscribe(response => {
                        this.CurrentSession.CurrentWindow.StopBusyIndicator();

                        this.EntityPM = response.Result;
                        //this.CurrentSession.CurrentWindow.Title = this.EntityPM.Name;
                        this.CurrentVersionNumber = this.EntityPM.VersionNumber;
                        this.ItemsSource = new ObservableCollection([]);
                        this.BuildItemsSource();
                        this.IsResourcesReady = true;

                    });
                }

               
            });
        }
    }


    BuildItemsSource() {
        var itemsCollection: QuestionnaireQuestionViewModel[] = [];
        this.EntityPM.QuestionnaireQuestions.forEach((questionnaireQuestionPM) => {

            var questionnaireQuestionPMCopy: QuestionnaireQuestionPM = new QuestionnaireQuestionPM(this.EntityPM);
            questionnaireQuestionPMCopy.Tenant = questionnaireQuestionPM.Tenant;
            questionnaireQuestionPMCopy.QuestionTypeCode = questionnaireQuestionPM.QuestionTypeCode;
            questionnaireQuestionPMCopy.QuestionNumber = questionnaireQuestionPM.QuestionNumber;
            questionnaireQuestionPMCopy.Question = questionnaireQuestionPM.Question;
            questionnaireQuestionPMCopy.IsMandatory = questionnaireQuestionPM.IsMandatory;
            questionnaireQuestionPMCopy.CreateDate = DateTool.GetCurrentDateAsUtc();
            questionnaireQuestionPMCopy.CreatedByUserId = SessionLocator.LoggedUserId;
            questionnaireQuestionPMCopy.UpdateDate = DateTool.GetCurrentDateAsUtc();
            questionnaireQuestionPMCopy.UpdatedByUserId = SessionLocator.LoggedUserId;
            questionnaireQuestionPMCopy.QuestioneerId = "DUM";
            questionnaireQuestionPMCopy.PickListCode = questionnaireQuestionPM.PickListCode;
            questionnaireQuestionPMCopy.IsAddOther = questionnaireQuestionPM.IsAddOther;
            questionnaireQuestionPMCopy.QuestionTypeCode = questionnaireQuestionPM.QuestionTypeCode;
            //QuestionnaireQuestionsViewModelList.Add(new QuestionnaireQuestionsViewModel(questionnaireQuestionPMCopy, this, null));


            itemsCollection.push(new QuestionnaireQuestionViewModel(questionnaireQuestionPMCopy, this));
        });

        this.ItemsSource.InsertCollection(itemsCollection);
         
    }



    OnQuestionSelected(item: QuestionnaireQuestionViewModel) {
        this.SelectedQuestion = item;
        this.ItemsSource.Collection.forEach((item) => {
            item.IsShowArrowUpDown = false;
        });

        this.SelectedQuestion.IsShowArrowUpDown = true;
    }



    ArrowUpButtonClicked(item: QuestionnaireQuestionViewModel) {
        var i = this.ItemsSource.Collection.indexOf(item);
        var upColumn = this.ItemsSource.Collection[i - 1];
        if (i > 0) {
            this.ItemsSource.Collection = this.ItemsSource.Collection.filter(d => d.Id != upColumn.Id);
            var tempOrder = item.QuestionNumber;
            item.EntityPM.QuestionNumber = item.QuestionNumber = upColumn.Order;
            upColumn.Order = upColumn.EntityPM.QuestionNumber = tempOrder;
            this.ItemsSource.Collection.splice(i, 0, upColumn);
        }
    }


    ArrowDownButtonClicked(item: QuestionnaireQuestionViewModel) {
        var i = this.ItemsSource.Collection.indexOf(item);
        var downColumn = this.ItemsSource.Collection[i + 1];
        if (i < this.ItemsSource.Collection.length - 1) {
            this.ItemsSource.Collection = this.ItemsSource.Collection.filter(d => d.Id != downColumn.Id);
            var tempOrder = item.QuestionNumber;
            item.EntityPM.QuestionNumber = item.QuestionNumber = downColumn.Order;
            downColumn.Order = downColumn.EntityPM.Order = tempOrder;
            this.ItemsSource.Collection.splice(i, 0, downColumn);
        }
    }


    OnAddEditQuestion(questionnaireQuestionViewModel: QuestionnaireQuestionViewModel , type:string) {
        if (!questionnaireQuestionViewModel) {
           var questionnaireQuestionPM = new QuestionnaireQuestionPM(null);
            questionnaireQuestionViewModel = new QuestionnaireQuestionViewModel(questionnaireQuestionPM, this, true);
        }

        var logWindow = new LogitudeWindow();
        logWindow.Title = type == "Add" ? "Add Question" : "Edit Question";
        logWindow.Width = 900;
        logWindow.Height = 550;
        var windowArgs: any = {};
        windowArgs.QuestionnaireQuestionViewModel = questionnaireQuestionViewModel
        logWindow.WindowArgs = windowArgs;

        logWindow.Show('./CRMModules/CRMOthers/Components/Questionnaire/AddEditQuestionnaireQuestionComponent');

    }



    IsDeleteAnyQuestion: boolean = false;

    DeleteQuestion(item: QuestionnaireQuestionViewModel) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("Are you sure you want to delete this question?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {

                if (item != null) {
                    var index = this.ItemsSource.Collection.indexOf(item);
                    if (index > -1) {
                        this.ItemsSource.Collection.splice(index, 1);
                        this.IsDeleteAnyQuestion = true;
                    }
                }


               
            }
        });
    }

    OkButtonClicked() {

        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        var hasChanges: boolean = this.ItemsSource.Collection.filter(f => f.EntityPM.IsDirty == true || f.IsNewEntity).length > 0
        var questionNumber: number = 1;

        if (this.IsDeleteAnyQuestion) hasChanges = true;

        if (this.IsNewEntity || hasChanges) {
            if (!this.IsNewEntity && hasChanges && this.EntityPM.VersionNumber == this.CurrentVersionNumber) {
                this.EntityPM.VersionNumber += 1;
            }
            this.ItemsSource.Collection.forEach(item => {
                if (item != null) {

                    item.VersionNumber = this.EntityPM.VersionNumber;
                    item.QuestionNumber = questionNumber;
                    this.EntityPM.AddQuestionnaireQuestion(item.EntityPM);
                    //questionnairePM.QuestionnaireQuestions.Add(questionnaireQuestionpm.CurrentEntityPM);
                    ++questionNumber;
                }
            });
        }
        //this.EntityPM.QuestionnaireQuestions.forEach(questionnaireQuestionpm => {
        //    if (questionnaireQuestionpm != null) {

        //        questionnaireQuestionpm.VersionNumber = this.EntityPM.VersionNumber;
        //        questionnaireQuestionpm.QuestionNumber = questionNumber;
        //        //questionnairePM.QuestionnaireQuestions.Add(questionnaireQuestionpm.CurrentEntityPM);
        //        ++questionNumber;
        //    }
        //});


        this.DataContext.EntityPM.QuestionnaireQuestions.forEach(item => {
            if (item != null) {
                Validator.TryValidateObject(item, "QuestionnaireQuestion", errors);
            }
        });

        if (this.EntityPM.QuestionnaireQuestions.length < 1) {
            errors.push("Please add at least one question");
        }

        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {

            this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
            if (this.IsNewEntity) {
                this._QuestionnairePMService.insert(this.EntityPM).subscribe(response => {

                    this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    if (!response.HasError) {
                        this.CurrentSession.CloseCurrentWindowEmit(response.Result.Id);
                    }
                    else {
                        this.ValidationErrorsList = response.ErrorsArray;
                    }

                });
            } else {
                this._QuestionnairePMService.update(this.EntityPM).subscribe(response => {

                    this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    if (!response.HasError) {
                        this.CurrentSession.CloseCurrentWindowEmit(response.Result.Id);
                    }
                    else {
                        this.ValidationErrorsList = response.ErrorsArray;
                    }

                });
            }
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}

export class QuestionnaireQuestionViewModel extends BaseComponent {

    public Id: string;
    public EntityPM: QuestionnaireQuestionPM;
    public QuestionnairePM: QuestionnairePM;
    constructor(entityPM: QuestionnaireQuestionPM, public parentComponent: AddEditQuestionnaireComponent, isNew: boolean = false) {
        super();

        this.EntityPM = entityPM;
        this.QuestionnairePM = parentComponent.EntityPM;
        this.IsNewEntity = isNew;
        this.Id = Guid.newGuid();
        if (this.IsNewEntity) {
            this.QuestionTypeCode = "TE";
        }
        this.EntityPM.IsDirty = false;
    }

    public IsNewEntity: boolean = false;

    public IsShowArrowUpDown: boolean = false;

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

    private isAddOther: boolean = false;
    public get IsAddOther() {
        if (this.EntityPM) {
            this.isAddOther = this.EntityPM.IsAddOther;
        }

        return this.isAddOther;

    }
    public set IsAddOther(newValue: boolean) {
        if (this.isAddOther != newValue) {
            this.isAddOther = newValue;
            this.EntityPM.IsAddOther = newValue;

        }
    }

    private questionNumber: number;
    public get QuestionNumber() {
        if (this.EntityPM) {
            this.questionNumber = this.EntityPM.QuestionNumber;
        }

        return this.questionNumber;

    }
    public set QuestionNumber(newValue: number) {
        if (this.QuestionNumber != newValue) {
            this.questionNumber = newValue;
            this.EntityPM.QuestionNumber = newValue;

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

    
    private questionTypeName: string = "";
    public get QuestionTypeName() {
        if (this.EntityPM) {
            switch (this.EntityPM.QuestionTypeCode) {
                case "YN":
                    this.questionTypeName = "Yes/No";
                    break;
                case "MC":
                    this.questionTypeName = "Multiple choice";
                    break;
                case "TE":
                    this.questionTypeName = "Text";
                    break;
                case "CB":
                    this.questionTypeName = "Check boxes";
                    break;
                case "CL":
                    this.questionTypeName = "Choose from a list";
                    break;
                case "ES":
                    this.questionTypeName = "Text (Multiline)";
                    break;
                case "DA":
                    this.questionTypeName = "Date";
                    break;
                case "DS":
                    this.questionTypeName = "Decimal";
                    break;
                case "HL":
                    this.questionTypeName = "Header line";
                    break;

            }

            return this.questionTypeName;

        }
    }

}
