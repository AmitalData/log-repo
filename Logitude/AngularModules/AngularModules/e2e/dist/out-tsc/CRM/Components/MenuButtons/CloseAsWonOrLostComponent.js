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
var Tools_1 = require("../../../Infrastructure/Tools");
var OpportunityClosingReasonListService_1 = require("../../Services/StandardLists/OpportunityClosingReasonListService");
var StageListService_1 = require("../../Services/StandardLists/StageListService");
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var CloseAsWonOrLostComponent = /** @class */ (function (_super) {
    __extends(CloseAsWonOrLostComponent, _super);
    function CloseAsWonOrLostComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "Opportunity";
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.ValidationErrorsList = [];
        _this.lostVisi = false;
        return _this;
    }
    CloseAsWonOrLostComponent.prototype.SetWindowArgs = function (args) {
        this.entityPM = args;
        this.ActualClosingDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
    };
    CloseAsWonOrLostComponent.prototype.SetRequiredClosingReason = function () {
        this.UIProperties.SetRequired("ClosingReasonId", "Opportunity", Tools_1.AppTool.IsNullOrEmpty(this.ClosingReasonId));
        this.UIProperties.SetEnabled("ClosingReasonId", this.ObjectTableName, this.IsClosedLost);
    };
    Object.defineProperty(CloseAsWonOrLostComponent.prototype, "ClosingReasonId", {
        get: function () { return this.entityPM.ClosingReasonId; },
        set: function (value) {
            var _this = this;
            this.entityPM.ClosingReasonId = value;
            this.ClosedToCompetitorId = null;
            this.SetRequiredClosingReason();
            var closingListService = new OpportunityClosingReasonListService_1.OpportunityClosingReasonListService();
            closingListService.getAllFromCache().subscribe(function (result) {
                var list = result.Result.filter(function (d) { return d.Id == value; })[0];
                if (list != null) {
                    _this.ClosingReasonCode = list.Code;
                    if (_this.ClosingReasonCode == "LC") {
                        _this.LostToCompetitionVisibility = true;
                    }
                    else {
                        _this.LostToCompetitionVisibility = false;
                    }
                    if ((_this.ClosingReasonCode == "WN" || _this.ClosingReasonCode == "LC" || _this.ClosingReasonCode == "LO")) {
                        _this.PostToFollowersAsWon = true;
                    }
                }
            });
        },
        enumerable: true,
        configurable: true
    });
    CloseAsWonOrLostComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("cancle");
    };
    CloseAsWonOrLostComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        if (Tools_1.AppTool.IsNullOrEmpty(this.ClosingReasonId)) {
            this.ValidationErrorsList.push("Closing reason is required");
        }
        Validator_1.Validator.TryValidateObject(this.entityPM, "Opportunity", this.ValidationErrorsList);
        if (this.ValidationErrorsList.length == 0) {
            this.entityPM.IsClosed = true;
            this.entityPM.StageDueDate = null;
            if (!this.IsClosedLost) {
                this.entityPM.Probability = 100;
                this.ComputeValue();
            }
            else {
                this.entityPM.Probability = 0;
                this.ComputeValue();
            }
            this.SetStageId();
            this.CurrentSession.CurrentEditComponent.SaveChanges("Closing Opportunity...");
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (event) {
                _this.CurrentSession.StopBusyIndicator();
                _this.CurrentSession.CloseCurrentWindow();
            });
        }
    };
    Object.defineProperty(CloseAsWonOrLostComponent.prototype, "SendPostVisibility", {
        get: function () {
            return ((this.ClosingReasonCode == "WN" || this.ClosingReasonCode == "LC" || this.ClosingReasonCode == "LO" || this.IsClosedLost) ? true : false);
        },
        enumerable: true,
        configurable: true
    });
    CloseAsWonOrLostComponent.prototype.ComputeValue = function () {
        var field1 = parseFloat(this.entityPM.Probability + "");
        var field2 = this.entityPM.NumberOfShipments == null ? 0 : parseFloat(this.entityPM.NumberOfShipments + "");
        var myValue = field1 * field2 / 100;
        this.entityPM.ValueField = myValue;
    };
    CloseAsWonOrLostComponent.prototype.SetStageId = function () {
        var _this = this;
        this.entityPM.LastStageIdBeforeClosure = this.entityPM.StageId;
        this.entityPM.LastStageDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        if (!this.IsClosedLost) {
            var stageListService = new StageListService_1.StageListService();
            stageListService.getAllFromCache().subscribe(function (result) {
                var stage = result.Result.filter(function (s) { return s.Code == "CWN"; })[0];
                if (stage != null) {
                    _this.entityPM.StageId = stage.Id;
                    _this.entityPM.StageName = stage.Name;
                }
            });
        }
        else {
            var stageListService = new StageListService_1.StageListService();
            stageListService.getAllFromCache().subscribe(function (result) {
                var stage = result.Result.filter(function (s) { return s.Code == "CLS"; })[0];
                if (stage != null) {
                    _this.entityPM.StageId = stage.Id;
                    _this.entityPM.StageName = stage.Name;
                }
            });
        }
    };
    Object.defineProperty(CloseAsWonOrLostComponent.prototype, "ClosingDescription", {
        get: function () { return this.entityPM.ClosingDescription; },
        set: function (value) { this.entityPM.ClosingDescription = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CloseAsWonOrLostComponent.prototype, "PostToFollowersAsWon", {
        get: function () { return this.entityPM.PostToFollowersAsWon; },
        set: function (value) { this.entityPM.PostToFollowersAsWon = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CloseAsWonOrLostComponent.prototype, "LostToCompetitionVisibility", {
        get: function () { return this.lostVisi; },
        set: function (value) { this.lostVisi = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CloseAsWonOrLostComponent.prototype, "ClosedToCompetitorId", {
        get: function () { return this.entityPM.ClosedToCompetitorId; },
        set: function (value) { this.entityPM.ClosedToCompetitorId = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CloseAsWonOrLostComponent.prototype, "ClosingReasonCode", {
        get: function () { return this.entityPM.ClosingReasonCode; },
        set: function (value) { this.entityPM.ClosingReasonCode = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CloseAsWonOrLostComponent.prototype, "ActualClosingDate", {
        get: function () { return this.entityPM.ActualClosingDate; },
        set: function (value) {
            var myValue = value;
            if (myValue != null) {
                //  myValue = Date.SpecifyKind(myValue.Value, DateTimeKind.Utc);
            }
            this.entityPM.ActualClosingDate = myValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CloseAsWonOrLostComponent.prototype, "IsClosedLost", {
        get: function () {
            return this.isClosedLost;
        },
        set: function (value) {
            this.isClosedLost = value;
            if (value) {
                this.PostToFollowersAsWon = true;
                this.SetRequiredClosingReason();
            }
        },
        enumerable: true,
        configurable: true
    });
    CloseAsWonOrLostComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CloseAsWonOrLostComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], CloseAsWonOrLostComponent);
    return CloseAsWonOrLostComponent;
}(BaseComponent_1.BaseComponent));
exports.CloseAsWonOrLostComponent = CloseAsWonOrLostComponent;
//# sourceMappingURL=CloseAsWonOrLostComponent.js.map