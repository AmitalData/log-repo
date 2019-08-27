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
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var AWBStackDomainService_1 = require("../../../Services/AWBStackDomainService");
var CardListService_1 = require("../../../Services/StandardLists/CardListService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var StackSelectionComponent = /** @class */ (function () {
    function StackSelectionComponent(entityResourceService) {
        this.entityResourceService = entityResourceService;
        this.ShipperId = null;
        this.AirlineId = null;
        this.AirlineName = null;
        this.ItemsCount = 0;
        this.SelectedItem = null;
        this.ObjectTableName = "MAWBStack";
        this.IsResourcesReady = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.ItemsSource = [];
        this.StackDomainService = new AWBStackDomainService_1.AWBStackDomainService();
        this.CurrentSession.StartBusyIndicator("Loading Air Waybill Numbers");
    }
    StackSelectionComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.args = args;
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(function (res) {
            _this.IsResourcesReady = true;
            if (!Tools_1.AppTool.IsNullOrEmpty(args.ShipperId)) {
                _this.ShipperId = args.ShipperId;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(args.CardId)) {
                _this.AirlineId = args.CardId;
                var myCardService = new CardListService_1.CardListService;
                myCardService.getSingle(args.CardId).subscribe(function (myResponse) {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list != null) {
                                _this.AirlineName = list.EnglishName;
                            }
                        }
                    }
                });
            }
            _this.Load();
        });
    };
    StackSelectionComponent.prototype.Load = function () {
        var _this = this;
        this.ItemsCount = 0;
        this.ItemsSource = [];
        if (this.AirlineId != null) {
            this.StackDomainService.GetMAWBStackPMsByAirlineIdAndShipperId(this.AirlineId, this.ShipperId, 100, 0).subscribe(function (myResult) {
                var data = myResult;
                _this.ItemsSource = data.sort(function (a, b) { return a.Number - b.Number; });
                _this.CurrentSession.StopBusyIndicator();
            });
            this.StackDomainService.GetMAWBStackPMsCountByAirlineIdAndShipperId(this.AirlineId, this.ShipperId).subscribe(function (myResult) {
                _this.ItemsCount = myResult;
            });
            this.StackDomainService.GetCardHasAssignedMawbStacks(this.AirlineId, this.ShipperId).subscribe(function (myResult) {
                _this.args.HasAssignedStocks = myResult;
            });
        }
    };
    StackSelectionComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    StackSelectionComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        if (this.SelectedItem != null) {
            this.args.SelectedStack = this.SelectedItem;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ShipperId)) {
                if (this.args.SelectedStack.AssignedToId != this.ShipperId) {
                    var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                    confirmWindow.Title = "Stock Number";
                    confirmWindow.Width = 450;
                    confirmWindow.Height = 190;
                    confirmWindow.Show("The MAWB number that you selected is not assigned to this shipper. Continue anyway?");
                    confirmWindow.WindowClosed.subscribe(function (c) {
                        if (confirmWindow.Yes) {
                            _this.CurrentSession.CloseCurrentWindowEmit("OK");
                        }
                    });
                }
                else {
                    this.CurrentSession.CloseCurrentWindowEmit("OK");
                }
            }
            else {
                this.CurrentSession.CloseCurrentWindowEmit("OK");
            }
        }
        //if (this.SelectedItem != null) {
        //    this.args.SelectedStack = this.SelectedItem;
        //    if (this.args.SelectedStack.AssignedToId == null && this.args.HasAssignedStocks) {
        //        var confirmWindow = new ConfirmWindow();
        //        confirmWindow.Title = "Stock Number";
        //        confirmWindow.Width = 450;
        //        confirmWindow.Height = 190;
        //        confirmWindow.Show("The MAWB number that you selected is not assigned to this shipper. Continue anyway?");
        //        confirmWindow.WindowClosed.subscribe(c => {
        //            if (confirmWindow.Yes) {
        //                this.CurrentSession.CloseCurrentWindowEmit("OK");
        //            }
        //        });
        //    }
        //    else {
        //        this.CurrentSession.CloseCurrentWindowEmit("OK");
        //    }
        //}
    };
    StackSelectionComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './StackSelectionComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], StackSelectionComponent);
    return StackSelectionComponent;
}());
exports.StackSelectionComponent = StackSelectionComponent;
//# sourceMappingURL=StackSelectionComponent.js.map