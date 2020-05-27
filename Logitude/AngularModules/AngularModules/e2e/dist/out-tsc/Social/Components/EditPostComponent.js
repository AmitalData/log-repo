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
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../Infrastructure/Tools");
var PostPMService_1 = require("../Services/StandardPMs/PostPMService");
var MessageWindow_1 = require("../../Controls/Windows/MessageWindow");
var EditPostComponent = /** @class */ (function () {
    function EditPostComponent() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.bodyText = "";
        this.postPMService = new PostPMService_1.PostPMService();
    }
    EditPostComponent.prototype.ngOnInit = function () {
    };
    EditPostComponent.prototype.SetWindowArgs = function (args) {
        this.PostViewModelData = args.PostViewModelData;
        if (this.PostViewModelData != null) {
            this.BodyText = this.PostViewModelData.EntityPM.BodyText;
        }
    };
    Object.defineProperty(EditPostComponent.prototype, "BodyText", {
        get: function () {
            return this.bodyText;
        },
        set: function (newValue) {
            if (this.bodyText != newValue) {
                this.bodyText = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    EditPostComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    EditPostComponent.prototype.SaveButtonClick = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.BodyText)) {
            if (this.BodyText.length <= 4000) {
                if (this.BodyText != this.PostViewModelData.EntityPM.BodyText) {
                    this.PostViewModelData.EntityPM.BodyText = this.BodyText;
                    this.PostViewModelData.BodyText = this.PostViewModelData.ViewMode.ConvertBodyText(this.BodyText);
                    this.CurrentSession.StartBusyIndicatorSaving();
                    this.postPMService.update(this.PostViewModelData.EntityPM).subscribe(function (res) {
                        var pmResponse = res;
                        _this.CurrentSession.StopBusyIndicator();
                        if (!pmResponse.HasError && pmResponse.Result) {
                            _this.CurrentSession.CloseCurrentWindow();
                        }
                    });
                }
                else
                    this.CurrentSession.CloseCurrentWindow();
            }
            else {
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Show("Post maximum charachters should be less than 4000!");
            }
        }
        else {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Show("Post Body field is required");
        }
        // this.CurrentSession.CloseCurrentWindow();
    };
    EditPostComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'EditPostComponent',
            templateUrl: './EditPostComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], EditPostComponent);
    return EditPostComponent;
}());
exports.EditPostComponent = EditPostComponent;
//# sourceMappingURL=EditPostComponent.js.map