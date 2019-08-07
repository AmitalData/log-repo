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
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var ClaimsRelatedEntityPM_1 = require("../../../../Customs/EntityPMs/ClaimsRelatedEntityPM");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var PointersFromClaimRelatedEntitiesSelectionComponent = /** @class */ (function () {
    function PointersFromClaimRelatedEntitiesSelectionComponent() {
        this.entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsVisibile = false;
        this.ClaimRelatedEntitiesList = new ObservableCollection_1.ObservableCollection([ClaimsRelatedEntityPM_1.ClaimsRelatedEntityPM]);
        this.SelectedClaimRelatedEntities = new ObservableCollection_1.ObservableCollection([]);
    }
    PointersFromClaimRelatedEntitiesSelectionComponent.prototype.SetWindowArgs = function (args) {
        this.ClaimPM = args.ClaimPM;
        this.CustomsDocumentsTicket = args.CustomsDocumentsTicket;
        this.BuildClaimsRelatedEntitiesList();
        if (this.CustomsDocumentsTicket.RequestedCustomsDocId) {
            this.IsDisplayOnly = true;
            this.DisplayOnlyMessage = "מסמך לתצוגה בלבד";
        }
        else {
            this.IsDisplayOnly = false;
            this.DisplayOnlyMessage = "";
        }
    };
    PointersFromClaimRelatedEntitiesSelectionComponent.prototype.BuildClaimsRelatedEntitiesList = function () {
        var _this = this;
        this.ClaimRelatedEntitiesList.Clear();
        for (var _i = 0, _a = this.ClaimPM.ClaimsRelatedEntities; _i < _a.length; _i++) {
            var item = _a[_i];
            this.ClaimRelatedEntitiesList.Insert(item);
        }
        this.CustomsDocumentsTicket.CustomsDocumentPointers.forEach(function (pointer) {
            var CRE = _this.ClaimRelatedEntitiesList.Collection.filter(function (d) { return d.EntityCounterKey + "" == pointer.Child1EntityId; })[0];
            if (CRE) {
                if (!_this.SelectedClaimRelatedEntities.Collection.includes(CRE)) {
                    _this.SelectedClaimRelatedEntities.Collection.push(CRE);
                }
            }
        });
    };
    PointersFromClaimRelatedEntitiesSelectionComponent.prototype.OnRowSelected = function (items) {
        var _this = this;
        this.SelectedRows = items;
        this.SelectedClaimRelatedEntities.Collection.forEach(function (CRELine) {
            var item = items.filter(function (d) { return d.EntityCounterKey == CRELine.EntityCounterKey; })[0];
            if (!item) {
                _this.SelectedClaimRelatedEntities.Remove(CRELine);
            }
        });
    };
    PointersFromClaimRelatedEntitiesSelectionComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    PointersFromClaimRelatedEntitiesSelectionComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.ConnectedClaimRelatedEntities = "";
        this.SelectedClaimRelatedEntities.Collection.forEach(function (item) {
            _this.ConnectedClaimRelatedEntities = _this.ConnectedClaimRelatedEntities + "," + item.EntityCounterKey;
        });
        this.ConnectedClaimRelatedEntities = this.ConnectedClaimRelatedEntities.substr(1, this.ConnectedClaimRelatedEntities.length - 1);
        this.CurrentSession.CloseCurrentWindowEmit("ok");
    };
    PointersFromClaimRelatedEntitiesSelectionComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './PointersFromClaimRelatedEntitiesSelectionComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], PointersFromClaimRelatedEntitiesSelectionComponent);
    return PointersFromClaimRelatedEntitiesSelectionComponent;
}());
exports.PointersFromClaimRelatedEntitiesSelectionComponent = PointersFromClaimRelatedEntitiesSelectionComponent;
//# sourceMappingURL=PointersFromClaimRelatedEntitiesSelectionComponent.js.map