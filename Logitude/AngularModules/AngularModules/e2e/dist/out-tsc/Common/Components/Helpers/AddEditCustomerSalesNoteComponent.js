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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var Cloner_1 = require("../../../Infrastructure/Utilities/Cloner");
var Tools_1 = require("../../../Infrastructure/Tools");
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var AddEditCustomerSalesNoteComponent = /** @class */ (function (_super) {
    __extends(AddEditCustomerSalesNoteComponent, _super);
    function AddEditCustomerSalesNoteComponent(entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityResourceService = entityResourceService;
        _this.ObjectTableName = "CustomerSalesNote";
        _this.DataContext = _this;
        _this.IsNewEntity = false;
        _this.ValidationErrorsList = [];
        _this.IsResourcesReady = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        return _this;
    }
    AddEditCustomerSalesNoteComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.EntityPM = args['EntityPM'];
        this.CustomerPM = args['CustomerPM'];
        this.IsNewEntity = args['IsNewEntity'];
        this.oldNotesField = this.EntityPM.Notes;
        this.SetInfoData();
        this.Clone();
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(function (res) {
            _this.IsResourcesReady = true;
        });
    };
    AddEditCustomerSalesNoteComponent.prototype.SetInfoData = function () {
        this.CreateByUser = this.EntityPM.CreatedByUserName;
        this.UpdateByUser = this.EntityPM.UpdatedByUserName;
        this.CreateDateString = "(" + Tools_1.DateTool.GetDateFormats(this.EntityPM.CreateDate).DateString + ")";
        this.UpdateDateString = "(" + Tools_1.DateTool.GetDateFormats(this.EntityPM.UpdateDate).DateString + ")";
        var width = this.CurrentSession.CurrentWindow.Width - 10;
        this.CreateDateStringWidth = Tools_1.AppTool.GetTextWidth(this.CreateDateString, 11) + 5;
        this.UpdateDateStringWidth = Tools_1.AppTool.GetTextWidth(this.UpdateDateString, 11) + 5;
        var createByUserWidth = Tools_1.AppTool.GetTextWidth(this.CreateByUser, 12) + 5;
        var updateByUserWidth = Tools_1.AppTool.GetTextWidth(this.UpdateByUser, 12) + 5;
        var emptySpaceWidth_Create = width - (70 + this.CreateDateStringWidth);
        var emptySpaceWidth_Update = width - (70 + this.UpdateDateStringWidth);
        this.CreateByUserWidth = emptySpaceWidth_Create < createByUserWidth ? emptySpaceWidth_Create : createByUserWidth;
        this.UpdateByUserWidth = emptySpaceWidth_Update < updateByUserWidth ? emptySpaceWidth_Update : updateByUserWidth;
    };
    Object.defineProperty(AddEditCustomerSalesNoteComponent.prototype, "Notes", {
        get: function () { return this.EntityPM.Notes; },
        set: function (value) {
            if (this.EntityPM.Notes != value) {
                this.EntityPM.Notes = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditCustomerSalesNoteComponent.prototype, "PostToFollowers", {
        get: function () { return this.EntityPM.PostToFollowers; },
        set: function (value) {
            if (this.EntityPM.PostToFollowers != value) {
                this.EntityPM.PostToFollowers = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    AddEditCustomerSalesNoteComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditCustomerSalesNoteComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (errors.length == 0) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.Notes)) {
                errors.push("Notes is required");
            }
        }
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            if (this.IsNewEntity) {
                this.CustomerPM.AddCustomerSalesNotePM(this.EntityPM);
            }
            else {
                if (this.oldNotesField != this.EntityPM.Notes) {
                    this.EntityPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                    this.EntityPM.UpdatedByUserName = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
                    var maxDate = null;
                    var maxDateTicks = 0;
                    if (this.CustomerPM.SalesNotes.length > 0) {
                        this.CustomerPM.SalesNotes.forEach(function (item) {
                            var itemDateTicks = Tools_1.DateTool.GetDateParts(item.UpdateDate).DateTicks;
                            if (itemDateTicks > maxDateTicks) {
                                maxDateTicks = itemDateTicks;
                                maxDate = item.UpdateDate;
                            }
                        });
                    }
                    if (maxDate == null) {
                        maxDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                    }
                    else {
                        maxDate = Tools_1.DateTool.AddHours(maxDate, 1);
                    }
                    this.EntityPM.UpdateDate = maxDate;
                }
            }
            this.CurrentSession.CloseCurrentWindowEmit("Ok");
            this.CurrentSession.FireEvent("CustomerSalesNotesChanged");
        }
    };
    AddEditCustomerSalesNoteComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('Notes');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.CustomerPM);
    };
    AddEditCustomerSalesNoteComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    AddEditCustomerSalesNoteComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: "./AddEditCustomerSalesNoteComponent.html",
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], AddEditCustomerSalesNoteComponent);
    return AddEditCustomerSalesNoteComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditCustomerSalesNoteComponent = AddEditCustomerSalesNoteComponent;
//# sourceMappingURL=AddEditCustomerSalesNoteComponent.js.map