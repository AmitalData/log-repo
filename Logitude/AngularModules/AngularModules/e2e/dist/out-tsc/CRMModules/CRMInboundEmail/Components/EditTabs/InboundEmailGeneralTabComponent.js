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
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var InboundEmailGeneralTabComponent = /** @class */ (function () {
    function InboundEmailGeneralTabComponent(entityArgs) {
        this.entityArgs = entityArgs;
        //public ObjectTableName = "InboundEmail";
        this.DataContext = this;
        this.EmailLineSelectedItem = null;
        this.entityPM = entityArgs.EntityPM;
        this.EmailLinesList = new ObservableCollection_1.ObservableCollection([]);
        this.BuildData();
    }
    InboundEmailGeneralTabComponent.prototype.BuildData = function () {
        this.EmailLinesList.Clear();
        var list = [];
        this.entityPM.InboundEmailLines.forEach(function (item) {
            list.push(new InboundEmailLineData(item));
        });
        this.EmailLinesList.InsertCollection(list);
    };
    InboundEmailGeneralTabComponent.prototype.EmailLineSelected = function (item) {
        this.EmailLineSelectedItem = item;
    };
    Object.defineProperty(InboundEmailGeneralTabComponent.prototype, "ObjectTableName", {
        get: function () { return this.entityPM.ObjectTableName; },
        set: function (value) { this.entityPM.ObjectTableName = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InboundEmailGeneralTabComponent.prototype, "ObjectTableId", {
        get: function () { return this.entityPM.ObjectTableId; },
        set: function (value) { this.entityPM.ObjectTableId = value; },
        enumerable: true,
        configurable: true
    });
    InboundEmailGeneralTabComponent = __decorate([
        core_1.Component({
            selector: 'TicketDocsOutTabComponent',
            moduleId: module.id,
            templateUrl: './InboundEmailGeneralTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], InboundEmailGeneralTabComponent);
    return InboundEmailGeneralTabComponent;
}());
exports.InboundEmailGeneralTabComponent = InboundEmailGeneralTabComponent;
var InboundEmailLineData = /** @class */ (function () {
    function InboundEmailLineData(item) {
        this.ObjectTableName = "InboundEmailLine";
        this.entityPM = item;
    }
    Object.defineProperty(InboundEmailLineData.prototype, "Sender", {
        get: function () { return this.entityPM.Sender; },
        set: function (value) { this.entityPM.Sender = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InboundEmailLineData.prototype, "Recepient", {
        get: function () { return this.entityPM.Recepient; },
        set: function (value) { this.entityPM.Recepient = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InboundEmailLineData.prototype, "Subject", {
        get: function () { return this.entityPM.Subject; },
        set: function (value) { this.entityPM.Subject = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InboundEmailLineData.prototype, "CCs", {
        get: function () { return this.entityPM.CCs; },
        set: function (value) { this.entityPM.CCs = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InboundEmailLineData.prototype, "InternalUsers", {
        get: function () { return this.entityPM.InternalUsers; },
        set: function (value) { this.entityPM.InternalUsers = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InboundEmailLineData.prototype, "Body", {
        get: function () { return this.entityPM.Body; },
        set: function (value) { this.entityPM.Body = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InboundEmailLineData.prototype, "FullBody", {
        get: function () { return this.entityPM.FullBody; },
        set: function (value) { this.entityPM.FullBody = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InboundEmailLineData.prototype, "CreateDate", {
        get: function () { return this.entityPM.CreateDate; },
        set: function (value) { this.entityPM.CreateDate = value; },
        enumerable: true,
        configurable: true
    });
    InboundEmailLineData.prototype.ViewBody = function (arg) {
        var title = "";
        var description = null;
        if (arg == 'B') {
            title = "Body";
            description = this.entityPM.Body;
        }
        else {
            title = "Full Body";
            description = this.entityPM.FullBody;
        }
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = title;
        logWindow.WindowArgs = description;
        logWindow.Show("./CRMModules/CRMInboundEmail/Components/EditTabs/ViewInboundLineBodyComponent");
    };
    return InboundEmailLineData;
}());
exports.InboundEmailLineData = InboundEmailLineData;
//# sourceMappingURL=InboundEmailGeneralTabComponent.js.map