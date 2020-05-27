"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var QuestionnaireQuestionPM_1 = require("../../../../CRM/EntityPMs/QuestionnaireQuestionPM");
var QuestionnairePMService_1 = require("../../../../CRM/Services/StandardPMs/QuestionnairePMService");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../../Infrastructure/Tools");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var Guid_1 = require("../../../../Infrastructure/Utilities/Guid");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var AddEditQuestionnaireComponent = /** @class */ (function (_super) {
    __extends(AddEditQuestionnaireComponent, _super);
    function AddEditQuestionnaireComponent(_QuestionnairePMService) {
        var _this = _super.call(this) || this;
        _this._QuestionnairePMService = _QuestionnairePMService;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.ValidationErrorsList = [];
        //public QuestionsList: QuestionnaireQuestionViewModel[] = [];
        _this.ObjectTableName = "Questionnaire";
        _this.DataContext = _this;
        _this.IsNewEntity = false;
        _this.IsResourcesReady = false;
        _this.name = "";
        _this.rightToLeft = false;
        _this.hasTwoColumn = false;
        _this.inActive = false;
        _this.IsDeleteAnyQuestion = false;
        return _this;
    }
    Object.defineProperty(AddEditQuestionnaireComponent.prototype, "Name", {
        get: function () {
            if (this.EntityPM) {
                this.name = this.EntityPM.Name;
            }
            return this.name;
        },
        set: function (newValue) {
            if (this.Name != newValue) {
                this.name = newValue;
                this.EntityPM.Name = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditQuestionnaireComponent.prototype, "RightToLeft", {
        get: function () {
            if (this.EntityPM) {
                this.rightToLeft = this.EntityPM.RightToLeft;
            }
            return this.rightToLeft;
        },
        set: function (newValue) {
            if (this.RightToLeft != newValue) {
                this.rightToLeft = newValue;
                this.EntityPM.RightToLeft = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditQuestionnaireComponent.prototype, "HasTwoColumn", {
        get: function () {
            if (this.EntityPM) {
                this.hasTwoColumn = this.EntityPM.HasTwoColumn;
            }
            return this.hasTwoColumn;
        },
        set: function (newValue) {
            if (this.HasTwoColumn != newValue) {
                this.hasTwoColumn = newValue;
                this.EntityPM.HasTwoColumn = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditQuestionnaireComponent.prototype, "InActive", {
        get: function () {
            if (this.EntityPM) {
                this.inActive = this.EntityPM.InActive;
            }
            return this.inActive;
        },
        set: function (newValue) {
            if (this.InActive != newValue) {
                this.inActive = newValue;
                this.EntityPM.InActive = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    AddEditQuestionnaireComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        if (args != null) {
            this._entityResourceService.getEntityResourceByTableName("QuestionnaireQuestion").subscribe(function (response) {
                if (args.IsNew) {
                    _this.IsNewEntity = true;
                    _this.EntityPM = _this._QuestionnairePMService.GetNewEntityPM();
                    _this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                    _this.EntityPM.Name = _this.Name;
                    _this.EntityPM.CreateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
                    _this.EntityPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                    _this.EntityPM.UpdateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
                    _this.EntityPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                    _this.EntityPM.VersionNumber = 1;
                    _this.EntityPM.RightToLeft = false;
                    _this.EntityPM.HasTwoColumn = false;
                    _this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
                    //this.BuildItemsSource();
                    _this.IsResourcesReady = true;
                }
                else {
                    _this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Loading"));
                    _this._QuestionnairePMService.get(args.EntityId).subscribe(function (response) {
                        _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        _this.EntityPM = response.Result;
                        //this.CurrentSession.CurrentWindow.Title = this.EntityPM.Name;
                        _this.CurrentVersionNumber = _this.EntityPM.VersionNumber;
                        _this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
                        _this.BuildItemsSource();
                        _this.IsResourcesReady = true;
                    });
                }
            });
        }
    };
    AddEditQuestionnaireComponent.prototype.BuildItemsSource = function () {
        var _this = this;
        var itemsCollection = [];
        this.EntityPM.QuestionnaireQuestions.forEach(function (questionnaireQuestionPM) {
            var questionnaireQuestionPMCopy = new QuestionnaireQuestionPM_1.QuestionnaireQuestionPM(_this.EntityPM);
            questionnaireQuestionPMCopy.Tenant = questionnaireQuestionPM.Tenant;
            questionnaireQuestionPMCopy.QuestionTypeCode = questionnaireQuestionPM.QuestionTypeCode;
            questionnaireQuestionPMCopy.QuestionNumber = questionnaireQuestionPM.QuestionNumber;
            questionnaireQuestionPMCopy.Question = questionnaireQuestionPM.Question;
            questionnaireQuestionPMCopy.IsMandatory = questionnaireQuestionPM.IsMandatory;
            questionnaireQuestionPMCopy.CreateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
            questionnaireQuestionPMCopy.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            questionnaireQuestionPMCopy.UpdateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
            questionnaireQuestionPMCopy.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            questionnaireQuestionPMCopy.QuestioneerId = "DUM";
            questionnaireQuestionPMCopy.PickListCode = questionnaireQuestionPM.PickListCode;
            questionnaireQuestionPMCopy.IsAddOther = questionnaireQuestionPM.IsAddOther;
            questionnaireQuestionPMCopy.QuestionTypeCode = questionnaireQuestionPM.QuestionTypeCode;
            //QuestionnaireQuestionsViewModelList.Add(new QuestionnaireQuestionsViewModel(questionnaireQuestionPMCopy, this, null));
            itemsCollection.push(new QuestionnaireQuestionViewModel(questionnaireQuestionPMCopy, _this));
        });
        this.ItemsSource.InsertCollection(itemsCollection);
    };
    AddEditQuestionnaireComponent.prototype.OnQuestionSelected = function (item) {
        this.SelectedQuestion = item;
        this.ItemsSource.Collection.forEach(function (item) {
            item.IsShowArrowUpDown = false;
        });
        this.SelectedQuestion.IsShowArrowUpDown = true;
    };
    AddEditQuestionnaireComponent.prototype.ArrowUpButtonClicked = function (item) {
        var i = this.ItemsSource.Collection.indexOf(item);
        var upColumn = this.ItemsSource.Collection[i - 1];
        if (i > 0) {
            this.ItemsSource.Collection = this.ItemsSource.Collection.filter(function (d) { return d.Id != upColumn.Id; });
            var tempOrder = item.QuestionNumber;
            item.EntityPM.QuestionNumber = item.QuestionNumber = upColumn.Order;
            upColumn.Order = upColumn.EntityPM.QuestionNumber = tempOrder;
            this.ItemsSource.Collection.splice(i, 0, upColumn);
        }
    };
    AddEditQuestionnaireComponent.prototype.ArrowDownButtonClicked = function (item) {
        var i = this.ItemsSource.Collection.indexOf(item);
        var downColumn = this.ItemsSource.Collection[i + 1];
        if (i < this.ItemsSource.Collection.length - 1) {
            this.ItemsSource.Collection = this.ItemsSource.Collection.filter(function (d) { return d.Id != downColumn.Id; });
            var tempOrder = item.QuestionNumber;
            item.EntityPM.QuestionNumber = item.QuestionNumber = downColumn.Order;
            downColumn.Order = downColumn.EntityPM.Order = tempOrder;
            this.ItemsSource.Collection.splice(i, 0, downColumn);
        }
    };
    AddEditQuestionnaireComponent.prototype.OnAddEditQuestion = function (questionnaireQuestionViewModel, type) {
        if (!questionnaireQuestionViewModel) {
            var questionnaireQuestionPM = new QuestionnaireQuestionPM_1.QuestionnaireQuestionPM(null);
            questionnaireQuestionViewModel = new QuestionnaireQuestionViewModel(questionnaireQuestionPM, this, true);
        }
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = type == "Add" ? "Add Question" : "Edit Question";
        logWindow.Width = 900;
        logWindow.Height = 550;
        var windowArgs = {};
        windowArgs.QuestionnaireQuestionViewModel = questionnaireQuestionViewModel;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./CRMModules/CRMOthers/Components/Questionnaire/AddEditQuestionnaireQuestionComponent');
    };
    AddEditQuestionnaireComponent.prototype.DeleteQuestion = function (item) {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Show("Are you sure you want to delete this question?");
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                if (item != null) {
                    var index = _this.ItemsSource.Collection.indexOf(item);
                    if (index > -1) {
                        _this.ItemsSource.Collection.splice(index, 1);
                        _this.IsDeleteAnyQuestion = true;
                    }
                }
            }
        });
    };
    AddEditQuestionnaireComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        var hasChanges = this.ItemsSource.Collection.filter(function (f) { return f.EntityPM.IsDirty == true || f.IsNewEntity; }).length > 0;
        var questionNumber = 1;
        if (this.IsDeleteAnyQuestion)
            hasChanges = true;
        if (this.IsNewEntity || hasChanges) {
            if (!this.IsNewEntity && hasChanges && this.EntityPM.VersionNumber == this.CurrentVersionNumber) {
                this.EntityPM.VersionNumber += 1;
            }
            this.ItemsSource.Collection.forEach(function (item) {
                if (item != null) {
                    item.VersionNumber = _this.EntityPM.VersionNumber;
                    item.QuestionNumber = questionNumber;
                    _this.EntityPM.AddQuestionnaireQuestion(item.EntityPM);
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
        this.DataContext.EntityPM.QuestionnaireQuestions.forEach(function (item) {
            if (item != null) {
                Validator_1.Validator.TryValidateObject(item, "QuestionnaireQuestion", errors);
            }
        });
        if (this.EntityPM.QuestionnaireQuestions.length < 1) {
            errors.push("Please add at least one question");
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Saving"));
            if (this.IsNewEntity) {
                this._QuestionnairePMService.insert(this.EntityPM).subscribe(function (response) {
                    _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    if (!response.HasError) {
                        _this.CurrentSession.CloseCurrentWindowEmit(response.Result.Id);
                    }
                    else {
                        _this.ValidationErrorsList = response.ErrorsArray;
                    }
                });
            }
            else {
                this._QuestionnairePMService.update(this.EntityPM).subscribe(function (response) {
                    _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    if (!response.HasError) {
                        _this.CurrentSession.CloseCurrentWindowEmit(response.Result.Id);
                    }
                    else {
                        _this.ValidationErrorsList = response.ErrorsArray;
                    }
                });
            }
        }
    };
    AddEditQuestionnaireComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditQuestionnaireComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditQuestionnaireComponent.html',
            providers: [QuestionnairePMService_1.QuestionnairePMService],
        }),
        __metadata("design:paramtypes", [QuestionnairePMService_1.QuestionnairePMService])
    ], AddEditQuestionnaireComponent);
    return AddEditQuestionnaireComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditQuestionnaireComponent = AddEditQuestionnaireComponent;
var QuestionnaireQuestionViewModel = /** @class */ (function (_super) {
    __extends(QuestionnaireQuestionViewModel, _super);
    function QuestionnaireQuestionViewModel(entityPM, parentComponent, isNew) {
        if (isNew === void 0) { isNew = false; }
        var _this = _super.call(this) || this;
        _this.parentComponent = parentComponent;
        _this.IsNewEntity = false;
        _this.IsShowArrowUpDown = false;
        _this.question = "";
        _this.questionTypeCode = "";
        _this.isMandatory = false;
        _this.isAddOther = false;
        _this.pickListCode = "";
        _this.questionTypeName = "";
        _this.EntityPM = entityPM;
        _this.QuestionnairePM = parentComponent.EntityPM;
        _this.IsNewEntity = isNew;
        _this.Id = Guid_1.Guid.newGuid();
        if (_this.IsNewEntity) {
            _this.QuestionTypeCode = "TE";
        }
        _this.EntityPM.IsDirty = false;
        return _this;
    }
    Object.defineProperty(QuestionnaireQuestionViewModel.prototype, "Question", {
        get: function () {
            if (this.EntityPM) {
                this.question = this.EntityPM.Question;
            }
            return this.question;
        },
        set: function (newValue) {
            if (this.Question != newValue) {
                this.question = newValue;
                this.EntityPM.Question = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuestionnaireQuestionViewModel.prototype, "QuestionTypeCode", {
        get: function () {
            if (this.EntityPM) {
                this.questionTypeCode = this.EntityPM.QuestionTypeCode;
            }
            return this.questionTypeCode;
        },
        set: function (newValue) {
            if (this.QuestionTypeCode != newValue) {
                this.questionTypeCode = newValue;
                this.EntityPM.QuestionTypeCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuestionnaireQuestionViewModel.prototype, "IsMandatory", {
        get: function () {
            if (this.EntityPM) {
                this.isMandatory = this.EntityPM.IsMandatory;
            }
            return this.isMandatory;
        },
        set: function (newValue) {
            if (this.IsMandatory != newValue) {
                this.isMandatory = newValue;
                this.EntityPM.IsMandatory = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuestionnaireQuestionViewModel.prototype, "IsAddOther", {
        get: function () {
            if (this.EntityPM) {
                this.isAddOther = this.EntityPM.IsAddOther;
            }
            return this.isAddOther;
        },
        set: function (newValue) {
            if (this.isAddOther != newValue) {
                this.isAddOther = newValue;
                this.EntityPM.IsAddOther = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuestionnaireQuestionViewModel.prototype, "QuestionNumber", {
        get: function () {
            if (this.EntityPM) {
                this.questionNumber = this.EntityPM.QuestionNumber;
            }
            return this.questionNumber;
        },
        set: function (newValue) {
            if (this.QuestionNumber != newValue) {
                this.questionNumber = newValue;
                this.EntityPM.QuestionNumber = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuestionnaireQuestionViewModel.prototype, "PickListCode", {
        get: function () {
            if (this.EntityPM) {
                this.pickListCode = this.EntityPM.PickListCode;
            }
            return this.pickListCode;
        },
        set: function (newValue) {
            if (this.pickListCode != newValue) {
                this.pickListCode = newValue;
                this.EntityPM.PickListCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuestionnaireQuestionViewModel.prototype, "QuestionTypeName", {
        get: function () {
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
        },
        enumerable: true,
        configurable: true
    });
    return QuestionnaireQuestionViewModel;
}(BaseComponent_1.BaseComponent));
exports.QuestionnaireQuestionViewModel = QuestionnaireQuestionViewModel;
//# sourceMappingURL=AddEditQuestionnaireComponent.js.map