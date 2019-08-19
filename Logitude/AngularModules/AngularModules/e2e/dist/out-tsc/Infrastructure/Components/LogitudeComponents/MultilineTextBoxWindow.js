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
var SessionLocator_1 = require("../../Utilities/SessionLocator");
var MultilineTextBoxWindow = /** @class */ (function () {
    function MultilineTextBoxWindow() {
        this.DisplayMode = false;
        this.PreventNewLine = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    MultilineTextBoxWindow.prototype.SetWindowArgs = function (args) {
        this.Text = args.TextValue;
        if (args.DisplayMode) {
            this.DisplayMode = args.DisplayMode;
        }
        this.RowsCount = args.RowsCount;
        this.PreventNewLine = this.RowsCount == 1;
    };
    Object.defineProperty(MultilineTextBoxWindow.prototype, "Text", {
        get: function () { return this.text; },
        set: function (newValue) {
            if (this.text != newValue) {
                this.text = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    MultilineTextBoxWindow.prototype.OnKeyDown = function (event) {
        var ENTER = 13;
        var key = event.keyCode;
        var keyChar = event.key;
        if (key == ENTER && this.PreventNewLine) {
            event.preventDefault();
            return;
        }
    };
    MultilineTextBoxWindow.prototype.ngOnInit = function () { };
    MultilineTextBoxWindow.prototype.OkButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit(this.text);
    };
    MultilineTextBoxWindow.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("<!#cancelled>");
    };
    MultilineTextBoxWindow = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'MultilineTextBoxWindow',
            templateUrl: "./MultilineTextBoxWindow.html",
        }),
        __metadata("design:paramtypes", [])
    ], MultilineTextBoxWindow);
    return MultilineTextBoxWindow;
}());
exports.MultilineTextBoxWindow = MultilineTextBoxWindow;
//# sourceMappingURL=MultilineTextBoxWindow.js.map