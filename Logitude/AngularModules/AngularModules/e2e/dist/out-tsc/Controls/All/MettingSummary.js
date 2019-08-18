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
var Tools_1 = require("../../Infrastructure/Tools");
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var CRMControlsService_1 = require("../Services/CRMControlsService");
var MettingSummary = /** @class */ (function () {
    function MettingSummary() {
        this.ComponentId = null;
        this.ComponentButtonId = null;
        this.ComponentContentId = null;
        this.ComponentTemplateId = null;
        this.Width = 380;
        this.Height = 280;
        this.Updated = new core_1.EventEmitter();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.text = null;
        this.isToRight = false;
        this.entityId = null;
        this.isChecked = false;
        this.isOpened = false;
        this.IsMouseOver = false;
        this.IsMouseOverButton = false;
        this.IsMouseOverInput = false;
        this.ValidationErrorsList = [];
        var idIndex = this.CurrentSession.GetNewId("MettingSummary");
        this.ComponentId = "MettingSummary_" + idIndex;
        this.ComponentButtonId = "MettingSummaryButton_" + idIndex;
        this.ComponentContentId = "MettingSummaryContent_" + idIndex;
        this.ComponentTemplateId = "MettingSummaryTemplate_" + idIndex;
    }
    Object.defineProperty(MettingSummary.prototype, "Text", {
        get: function () { return this.text; },
        set: function (value) {
            if (this.text != value) {
                this.text = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MettingSummary.prototype, "IsToRight", {
        get: function () { return this.isToRight; },
        set: function (value) {
            if (this.isToRight != value) {
                this.isToRight = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MettingSummary.prototype, "EntityId", {
        get: function () { return this.entityId; },
        set: function (value) {
            if (this.entityId != value) {
                this.entityId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MettingSummary.prototype, "IsChecked", {
        get: function () { return this.isChecked; },
        set: function (value) {
            if (this.isChecked != value) {
                this.isChecked = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MettingSummary.prototype, "IsOpened", {
        get: function () { return this.isOpened; },
        set: function (value) {
            if (value != undefined) {
                if (this.isOpened != value) {
                    this.isOpened = value;
                    if (value) {
                        if (document.getElementById(this.ComponentContentId).style.visibility != "visible") {
                            document.getElementById(this.ComponentContentId).style.visibility = "visible";
                        }
                    }
                    else {
                        if (document.getElementById(this.ComponentContentId).style.visibility != "hidden") {
                            document.getElementById(this.ComponentContentId).style.visibility = "hidden";
                        }
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    MettingSummary.prototype.OnButtonClicked = function () {
        if (this.IsOpened) {
            this.IsOpened = false;
        }
        else {
            var item = document.getElementById(this.ComponentId);
            var itemRect = item.getBoundingClientRect();
            if (this.IsToRight) {
                document.getElementById(this.ComponentContentId).style.top = (itemRect.top + 26) + 'px';
                document.getElementById(this.ComponentContentId).style.left = (itemRect.left) + 'px';
            }
            else {
                document.getElementById(this.ComponentContentId).style.top = (itemRect.top + 26) + 'px';
                document.getElementById(this.ComponentContentId).style.left = (itemRect.left + 75 - this.Width) + 'px';
            }
            this.IsOpened = true;
        }
    };
    MettingSummary.prototype.OnButtonLostFocus = function () {
        if (!this.IsMouseOverButton) {
            if (this.IsMouseOver || this.IsMouseOverInput) {
                if (!this.IsMouseOverInput) {
                    document.getElementById(this.ComponentButtonId).focus();
                }
            }
            else {
                this.IsOpened = false;
            }
        }
    };
    MettingSummary.prototype.OnTextBoxLostFocus = function () {
        if (!this.IsMouseOverButton) {
            if (this.IsMouseOver) {
                document.getElementById(this.ComponentButtonId).focus();
            }
            else {
                this.IsOpened = false;
            }
        }
    };
    MettingSummary.prototype.OnTextBoxKeydown = function (e) {
        var keyCode = e.keyCode || e.which;
        if (keyCode == 9) {
            e.preventDefault();
        }
    };
    // Commands
    MettingSummary.prototype.CancelButtonClicked = function () {
        this.ValidationErrorsList = [];
        this.Text = null;
        this.IsOpened = false;
    };
    MettingSummary.prototype.OkButtonClicked = function () {
        var errors = [];
        if (!Tools_1.AppTool.IsNullOrEmpty(this.Text)) {
            if (this.Text.length > 5000) {
                errors.push("Meeting Summary must be less\nthan 5000 char");
            }
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.UpdatingActivityMettingSummary();
        }
    };
    MettingSummary.prototype.UpdatingActivityMettingSummary = function () {
        var _this = this;
        var service = new CRMControlsService_1.CRMControlsService();
        var summary = new CRMControlsService_1.MeetingSummary();
        summary.ActivityId = this.EntityId;
        summary.Summary = this.Text;
        summary.Post = this.IsChecked;
        service.PutCompleteActivity(summary).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.Updated.emit(true);
            }
        });
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], MettingSummary.prototype, "Updated", void 0);
    MettingSummary = __decorate([
        core_1.Component({
            selector: "MettingSummary",
            moduleId: module.id,
            templateUrl: './MettingSummary.html',
            inputs: ['IsChecked', 'Text', 'EntityId', 'IsToRight'],
        }),
        __metadata("design:paramtypes", [])
    ], MettingSummary);
    return MettingSummary;
}());
exports.MettingSummary = MettingSummary;
//# sourceMappingURL=MettingSummary.js.map