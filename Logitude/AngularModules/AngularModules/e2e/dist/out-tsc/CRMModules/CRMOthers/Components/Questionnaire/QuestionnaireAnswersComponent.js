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
var QuestionnaireAnswerPM_1 = require("../../../../CRM/EntityPMs/QuestionnaireAnswerPM");
var QuestionnaireAnswerLinePM_1 = require("../../../../CRM/EntityPMs/QuestionnaireAnswerLinePM");
var QuestionnairePMService_1 = require("../../../../CRM/Services/StandardPMs/QuestionnairePMService");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../../Infrastructure/Tools");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var UIProperties_1 = require("../../../../Infrastructure/Components/LogitudeComponents/UIProperties");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var CustomPickListListService_1 = require("../../../../Infrastructure/Services/StandardLists/CustomPickListListService");
var QuestionnaireAnswerPMService_1 = require("../../../../CRM/Services/StandardPMs/QuestionnaireAnswerPMService");
var SessionInfo_1 = require("../../../../Infrastructure/Utilities/SessionInfo");
var Guid_1 = require("../../../../Infrastructure/Utilities/Guid");
var QuestionnaireAnswersComponent = /** @class */ (function (_super) {
    __extends(QuestionnaireAnswersComponent, _super);
    function QuestionnaireAnswersComponent(_QuestionnairePMService, _QuestionnaireAnswerPMService) {
        var _this = _super.call(this) || this;
        _this._QuestionnairePMService = _QuestionnairePMService;
        _this._QuestionnaireAnswerPMService = _QuestionnaireAnswerPMService;
        _this.ValidationErrorsList = [];
        _this.TenantCustomPickLists = [];
        _this.IsResourcesReady = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.hasTwoColumn = false;
        _this.flowDirection = "ltr";
        _this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        _this.ItemsSource2 = new ObservableCollection_1.ObservableCollection([]);
        _this.UIProperties = new UIProperties_1.UIProperties;
        _this.customPickListListService = new CustomPickListListService_1.CustomPickListListService();
        _this.SessionIndex = SessionLocator_1.SessionLocator.Index;
        return _this;
    }
    QuestionnaireAnswersComponent.prototype.SetWindowArgs = function (args) {
        this.simplogWindow = this.CurrentSession.CurrentWindow;
        if (args != null) {
            this.EntityId = args.EntityId;
            this.ObjectTableId = args.ObjectTableId;
            this.EntityPM = args.EntityPM;
            this.BuildItemsSource();
            // this.LoadData();
        }
    };
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
    QuestionnaireAnswersComponent.prototype.BuildItemsSource = function () {
        var _this = this;
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.Tenant = SessionLocator_1.SessionLocator.Tenant;
        filters.SortDirection = "Ascending";
        filters.PageIndex = 0;
        filters.SortBy = 'Value';
        filters.addAdditionalFilter("IsMultipleChoice", true, null, null, "Equals", false, false, false, null, false, true);
        this.customPickListListService.getAllFromCache(filters).subscribe(function (response) {
            var AllQuestionsArr = _this.EntityPM.QuestionnaireQuestions;
            _this.TenantCustomPickLists = response.Result;
            _this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
            _this.ItemsSource2 = new ObservableCollection_1.ObservableCollection([]);
            var itemsCollection = [];
            var itemsCollection2 = [];
            if (!_this.HasTwoColumn) {
                AllQuestionsArr.forEach(function (questionnaireQuestionPM) {
                    itemsCollection.push(new QuestionnaireAnswerViewModel(questionnaireQuestionPM, _this));
                });
            }
            else {
                var count = 0;
                AllQuestionsArr.forEach(function (questionnaireQuestionPM) {
                    var questionVM = null;
                    if (count % 2 === 0) {
                        questionVM = new QuestionnaireAnswerViewModel(questionnaireQuestionPM, _this);
                    }
                    else
                        questionVM = new QuestionnaireAnswerViewModel(questionnaireQuestionPM, _this, 1);
                    itemsCollection.push(questionVM);
                    count++;
                });
            }
            _this.ItemsSource.InsertCollection(itemsCollection);
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
            _this.IsResourcesReady = true;
        });
    };
    Object.defineProperty(QuestionnaireAnswersComponent.prototype, "HasTwoColumn", {
        get: function () {
            if (this.EntityPM) {
                this.hasTwoColumn = this.EntityPM.HasTwoColumn;
            }
            return this.hasTwoColumn;
        },
        set: function (newValue) {
            if (this.hasTwoColumn != newValue) {
                this.hasTwoColumn = newValue;
                this.EntityPM.HasTwoColumn = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuestionnaireAnswersComponent.prototype, "FlowDirection", {
        get: function () {
            if (this.EntityPM) {
                if (this.EntityPM.RightToLeft) {
                    this.flowDirection = "rtl";
                }
            }
            return this.flowDirection;
        },
        enumerable: true,
        configurable: true
    });
    QuestionnaireAnswersComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        var hasRequiredFields = false;
        var otherFieldsRequired = false;
        var textOutOfRange = false;
        this.ItemsSource.Collection.forEach(function (questionAnswerModel) {
            if (questionAnswerModel.IsMandatory && Tools_1.AppTool.IsNullOrEmpty(questionAnswerModel.AnswerValue)) {
                hasRequiredFields = true;
            }
            if ((questionAnswerModel.QuestionTypeCode === "CB" || questionAnswerModel.QuestionTypeCode === "MC")
                && questionAnswerModel.AnswerValue === "Other" && Tools_1.AppTool.IsNullOrEmpty(questionAnswerModel.OtherValue)) {
                otherFieldsRequired = true;
            }
            if ((questionAnswerModel.QuestionTypeCode === "TE" || questionAnswerModel.QuestionTypeCode === "ES")
                && !Tools_1.AppTool.IsNullOrEmpty(questionAnswerModel.AnswerValue) && questionAnswerModel.AnswerValue.length > 1000) {
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
            var questionnaireAnswerPM = new QuestionnaireAnswerPM_1.QuestionnaireAnswerPM();
            questionnaireAnswerPM.Id = "DUM";
            questionnaireAnswerPM.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
            questionnaireAnswerPM.QuestioneerId = this.EntityPM.Id;
            questionnaireAnswerPM.CreateDate = new Date();
            questionnaireAnswerPM.CreatedByUserId = SessionInfo_1.SessionInfo.LoggedUserId;
            questionnaireAnswerPM.VersionNumber = this.EntityPM.VersionNumber;
            questionnaireAnswerPM.ObjectTableId = this.ObjectTableId;
            questionnaireAnswerPM.EntityId = this.EntityId;
            questionnaireAnswerPM.HasTwoColumn = this.EntityPM.HasTwoColumn;
            var Id = 0;
            this.ItemsSource.Collection.forEach(function (questionAnswerModel) {
                var answerValue = questionAnswerModel.AnswerValue;
                if (questionAnswerModel.QuestionTypeCode === "MC" && questionAnswerModel.AnswerValue === "Other" && !Tools_1.AppTool.IsNullOrEmpty(questionAnswerModel.OtherValue)) {
                    answerValue = questionAnswerModel.OtherValue;
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(answerValue)) {
                    var questionnaireAnswerLinePM = new QuestionnaireAnswerLinePM_1.QuestionnaireAnswerLinePM(questionAnswerModel.EntityPM);
                    questionnaireAnswerLinePM.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                    questionnaireAnswerLinePM.QuestionNumber = questionAnswerModel.EntityPM.QuestionNumber;
                    if (questionAnswerModel.QuestionTypeCode === "CB") {
                        questionAnswerModel.CheckedValues.forEach(function (value) {
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
            this._QuestionnaireAnswerPMService.insert(questionnaireAnswerPM).subscribe(function (response) {
                if (response.HasError === false) {
                    _this.CurrentSession.CurrentWindow.Close("ok");
                }
                else {
                    _this.ValidationErrorsList = response.ErrorsArray;
                }
            });
        }
    };
    QuestionnaireAnswersComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CurrentWindow.Close("cancelled");
    };
    QuestionnaireAnswersComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './QuestionnaireAnswersComponent.html',
            providers: [QuestionnairePMService_1.QuestionnairePMService, QuestionnaireAnswerPMService_1.QuestionnaireAnswerPMService],
        }),
        __metadata("design:paramtypes", [QuestionnairePMService_1.QuestionnairePMService, QuestionnaireAnswerPMService_1.QuestionnaireAnswerPMService])
    ], QuestionnaireAnswersComponent);
    return QuestionnaireAnswersComponent;
}(BaseComponent_1.BaseComponent));
exports.QuestionnaireAnswersComponent = QuestionnaireAnswersComponent;
var QuestionnaireAnswerViewModel = /** @class */ (function (_super) {
    __extends(QuestionnaireAnswerViewModel, _super);
    function QuestionnaireAnswerViewModel(questionPM, parent, columnNumber) {
        if (columnNumber === void 0) { columnNumber = 0; }
        var _this = _super.call(this) || this;
        _this.CustomPickListItems = [];
        _this.OtherValue = "";
        _this.AnswerValue = "";
        _this.CheckedValues = [];
        _this.questionTypeCode = "";
        _this.isMandatory = false;
        _this.question = "";
        _this.pickListCode = "";
        _this.isAddOther = false;
        _this.EntityPM = questionPM;
        _this.UIProperties = new UIProperties_1.UIProperties;
        _this.Parent = parent;
        _this.ColumnNumber = columnNumber;
        _this.IndexKey = Guid_1.Guid.newGuid();
        if (!Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.PickListCode)) {
            _this.CustomPickListItems = _this.Parent.TenantCustomPickLists.filter(function (p) { return p.Code === _this.EntityPM.PickListCode; });
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
        return _this;
    }
    QuestionnaireAnswerViewModel.prototype.onTextChange = function (newValue) {
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
    };
    QuestionnaireAnswerViewModel.prototype.OnItemChecked = function (item) {
        var _this = this;
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
        this.CheckedValues.forEach(function (value) {
            if (value) {
                _this.AnswerValue += value + ",";
            }
        });
    };
    QuestionnaireAnswerViewModel.prototype.pickListItemChanged = function ($event) {
        if ($event) {
            this.AnswerValue = $event.Value;
        }
        else {
            this.AnswerValue = null;
        }
    };
    Object.defineProperty(QuestionnaireAnswerViewModel.prototype, "ColumnNumber", {
        get: function () {
            return this.columnNumber;
        },
        set: function (newValue) {
            if (this.columnNumber != newValue) {
                this.columnNumber = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuestionnaireAnswerViewModel.prototype, "QuestionTypeCode", {
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
    Object.defineProperty(QuestionnaireAnswerViewModel.prototype, "IsMandatory", {
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
    Object.defineProperty(QuestionnaireAnswerViewModel.prototype, "Question", {
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
    Object.defineProperty(QuestionnaireAnswerViewModel.prototype, "PickListCode", {
        get: function () {
            if (this.EntityPM) {
                this.pickListCode = this.EntityPM.PickListCode;
            }
            return this.pickListCode;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuestionnaireAnswerViewModel.prototype, "IsAddOther", {
        get: function () {
            if (this.EntityPM) {
                this.isAddOther = this.EntityPM.IsAddOther;
            }
            return this.isAddOther;
        },
        enumerable: true,
        configurable: true
    });
    return QuestionnaireAnswerViewModel;
}(BaseComponent_1.BaseComponent));
exports.QuestionnaireAnswerViewModel = QuestionnaireAnswerViewModel;
//# sourceMappingURL=QuestionnaireAnswersComponent.js.map