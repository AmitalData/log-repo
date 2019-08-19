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
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var Cloner_1 = require("../../../Infrastructure/Utilities/Cloner");
var QuoteEventNotesComponent = /** @class */ (function (_super) {
    __extends(QuoteEventNotesComponent, _super);
    function QuoteEventNotesComponent() {
        var _this = _super.call(this) || this;
        _this.EntityPM = null;
        _this.DataContext = _this;
        _this.ObjectTableName = "Quote";
        _this.NotesHeader = "Notes";
        _this.ShowClosingReason = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsConvertQuoteType = false;
        return _this;
    }
    QuoteEventNotesComponent.prototype.SetWindowArgs = function (args) {
        this.EntityPM = args.EntityPM;
        this.NotesHeader = args.NotesHeader;
        this.ShowClosingReason = args.ShowClosingReason;
        this.IsConvertQuoteType = args.IsConvertQuoteType;
        this.EventNote = null;
        this.Clone();
    };
    Object.defineProperty(QuoteEventNotesComponent.prototype, "QuoteClosingReasonCode", {
        get: function () { return this.EntityPM.QuoteClosingReasonCode; },
        set: function (value) {
            if (this.EntityPM.QuoteClosingReasonCode != value) {
                this.EntityPM.QuoteClosingReasonCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteEventNotesComponent.prototype, "EventNote", {
        get: function () { return this.EntityPM.EventNote; },
        set: function (value) {
            if (this.EntityPM.EventNote != value) {
                this.EntityPM.EventNote = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    QuoteEventNotesComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    QuoteEventNotesComponent.prototype.OkButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("OK");
    };
    QuoteEventNotesComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.EntityPM);
        this.myCloner.AddField('EventNote');
        this.myCloner.AddEntity(this.EntityPM);
    };
    QuoteEventNotesComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    QuoteEventNotesComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './QuoteEventNotesComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], QuoteEventNotesComponent);
    return QuoteEventNotesComponent;
}(BaseComponent_1.BaseComponent));
exports.QuoteEventNotesComponent = QuoteEventNotesComponent;
//# sourceMappingURL=QuoteEventNotesComponent.js.map