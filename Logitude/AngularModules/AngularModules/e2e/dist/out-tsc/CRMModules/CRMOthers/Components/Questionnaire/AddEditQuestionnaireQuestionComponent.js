"use strict";
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
var Tools_1 = require("../../../../Infrastructure/Tools");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var CustomPickListListService_1 = require("../../../../Infrastructure/Services/StandardLists/CustomPickListListService");
var AddEditQuestionnaireQuestionComponent = /** @class */ (function () {
    function AddEditQuestionnaireQuestionComponent() {
        this._customPickListListService = new CustomPickListListService_1.CustomPickListListService();
        this.LabelColumnWidth = 170;
        this.ControlColumnWidth = 220;
        this.ValidationErrorsList = [];
        this.ObjectTableName = "QuestionnaireQuestion";
        this.TenantCustomPickLists = [];
        this.CustomPickLists = [];
        //public SelectedType: QuestionType;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.questionTypeCode = "";
        this.isMandatory = false;
        this.question = "";
        this.pickListCode = "";
    }
    Object.defineProperty(AddEditQuestionnaireQuestionComponent.prototype, "QuestionTypeCode", {
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
    Object.defineProperty(AddEditQuestionnaireQuestionComponent.prototype, "IsMandatory", {
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
    Object.defineProperty(AddEditQuestionnaireQuestionComponent.prototype, "Question", {
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
    Object.defineProperty(AddEditQuestionnaireQuestionComponent.prototype, "PickListCode", {
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
    AddEditQuestionnaireQuestionComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        if (args != null) {
            this.CurrentSession.StartBusyIndicatorLoading();
            this.DataContext = args.QuestionnaireQuestionViewModel;
            this.EntityPM = this.DataContext.EntityPM;
            this.BuildQuestionTypes();
            var filters = new ApiQueryFilters_1.ApiQueryFilters();
            filters.Tenant = SessionLocator_1.SessionLocator.Tenant;
            filters.SortDirection = "Ascending";
            filters.PageIndex = 0;
            filters.SortBy = 'Value';
            filters.GetAll = true;
            filters.addAdditionalFilter("IsMultipleChoice", true, null, null, "Equals", false, false, false, null, false, true);
            this._customPickListListService.getByFilters(filters).subscribe(function (response) {
                _this.TenantCustomPickLists = response.Result;
                _this.CurrentSession.StopBusyIndicator();
                if (_this.TenantCustomPickLists != null) {
                    _this.TenantCustomPickLists.forEach(function (p) {
                        if (_this.CustomPickLists.indexOf(p.Code) === -1) {
                            _this.CustomPickLists.push(p.Code);
                        }
                    });
                }
            });
        }
    };
    Object.defineProperty(AddEditQuestionnaireQuestionComponent.prototype, "SelectedType", {
        get: function () {
            var _this = this;
            if (this.EntityPM) {
                this.selectedType = this.QuestionTypesList.filter(function (t) { return t.Code === _this.EntityPM.QuestionTypeCode; })[0];
            }
            return this.selectedType;
        },
        set: function (newValue) {
            if (this.SelectedType != newValue) {
                this.selectedType = newValue;
                this.EntityPM.QuestionTypeCode = newValue.Code;
                //this.DataContext.QuestionTypeName = 
            }
        },
        enumerable: true,
        configurable: true
    });
    AddEditQuestionnaireQuestionComponent.prototype.onTypeSelected = function (code) {
        this.DataContext.QuestionTypeCode = code;
    };
    //public QuestionTypeCodeLabel: string;
    //public QuestionLabel: string;
    //public IsMandetoryLabel: string;
    //SetLabels() {
    //    this.QuestionTypeCodeLabel = TextCodeTranslator.Translate('QuestionnaireQuestion.F.QuestionTypeCode');
    //    this.QuestionLabel = TextCodeTranslator.Translate('QuestionnaireQuestion.F.Question');
    //    this.IsMandetoryLabel = TextCodeTranslator.Translate('QuestionnaireQuestion.F.IsMandetory');
    //}
    AddEditQuestionnaireQuestionComponent.prototype.AddEditPickListCommand = function (type) {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = (type + " Custom Pick List");
        logWindow.Width = 780;
        logWindow.Height = 500;
        var windowArgs = {};
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
        logWindow.WindowClosed.subscribe(function ($event) {
            if ($event && _this.CustomPickLists) {
                if (_this.CustomPickLists.indexOf($event) === -1) {
                    _this.CustomPickLists.push($event);
                    _this.PickListCode = $event;
                }
            }
        });
    };
    AddEditQuestionnaireQuestionComponent.prototype.OkButtonClicked = function () {
        if (this.DataContext.IsNewEntity) {
            this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
            this.EntityPM.CreateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
            this.EntityPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            this.EntityPM.UpdateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
            this.EntityPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            this.EntityPM.QuestionNumber = 1;
            this.EntityPM.QuestioneerId = "DUM";
        }
        else {
            this.EntityPM.UpdateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
            this.EntityPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        }
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (this.EntityPM.QuestionTypeCode == "MC" || this.EntityPM.QuestionTypeCode == "CL" || this.EntityPM.QuestionTypeCode == "CB") {
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PickListCode)) {
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
    };
    AddEditQuestionnaireQuestionComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditQuestionnaireQuestionComponent.prototype.Clone = function () {
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
    };
    AddEditQuestionnaireQuestionComponent.prototype.RejectChanges = function () {
        //this.DataContext.ResetPackageItems();
        //this.myCloner.RejectChanges();
    };
    AddEditQuestionnaireQuestionComponent.prototype.BuildQuestionTypes = function () {
        this.QuestionTypesList = [];
        var type1 = new QuestionType("YN", "Yes/No");
        this.QuestionTypesList.push(type1);
        var type2 = new QuestionType("HL", "Header line");
        this.QuestionTypesList.push(type2);
        var type3 = new QuestionType("MC", "Multiple choice");
        this.QuestionTypesList.push(type3);
        var type4 = new QuestionType("TE", "Text");
        this.QuestionTypesList.push(type4);
        var type5 = new QuestionType("ES", "Text (Multiline)");
        this.QuestionTypesList.push(type5);
        var type6 = new QuestionType("DA", "Date");
        this.QuestionTypesList.push(type6);
        var type7 = new QuestionType("CB", "Check boxes");
        this.QuestionTypesList.push(type7);
        var type8 = new QuestionType("CL", "Choose from a list");
        this.QuestionTypesList.push(type8);
        var type9 = new QuestionType("DS", "Decimal");
        this.QuestionTypesList.push(type9);
        if (this.DataContext.IsNewEntity) {
            this.DataContext.QuestionTypeCode = this.QuestionTypesList[3].Code;
        }
    };
    AddEditQuestionnaireQuestionComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditQuestionnaireQuestionComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditQuestionnaireQuestionComponent);
    return AddEditQuestionnaireQuestionComponent;
}());
exports.AddEditQuestionnaireQuestionComponent = AddEditQuestionnaireQuestionComponent;
var QuestionType = /** @class */ (function () {
    function QuestionType(Code, Name) {
        this.Code = Code;
        this.Name = Name;
    }
    return QuestionType;
}());
exports.QuestionType = QuestionType;
//# sourceMappingURL=AddEditQuestionnaireQuestionComponent.js.map