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
var InfrastructureDomainService_1 = require("../../Services/InfrastructureDomainService");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var BatchTaskExecutionListService_1 = require("../../Services/StandardLists/BatchTaskExecutionListService");
var EraseTenantManagementDataComponent = /** @class */ (function () {
    function EraseTenantManagementDataComponent() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsResponseProgressVisible = false;
        // Timer
        this.timerSeconds = 1;
        this.Retries = 0;
        this.myService = new InfrastructureDomainService_1.InfrastructureDomainService();
    }
    EraseTenantManagementDataComponent.prototype.SetWindowArgs = function (args) {
        this.entityId = args;
        this.Message = null;
        this.GetCounts();
    };
    EraseTenantManagementDataComponent.prototype.GetCounts = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Check Data Counts...");
        this.myService.GetDataCountForTenant(this.entityId).subscribe(function (response) {
            if (!response.HasError) {
                _this.summaryRecord = response.Result;
            }
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    EraseTenantManagementDataComponent.prototype.DeleteClicked = function (type) {
        this.Message = null;
        this.type = type;
        switch (type) {
            case "B": {
                this.DoDelete(type);
                break;
            }
            case "T": {
                if (this.summaryRecord != null) {
                    if (this.summaryRecord.ActivitiesCount == 0) {
                        this.DoDelete(type);
                    }
                    else {
                        this.Message = "You can't Tickets, since you have CRM Records";
                    }
                }
                break;
            }
            case "C": {
                this.DoDelete(type);
                break;
            }
            case "P": {
                if (this.summaryRecord != null) {
                    if (this.summaryRecord.ShipmentsCount == 0 && this.summaryRecord.QuotesCount == 0 && this.summaryRecord.ARInvoicesCount == 0
                        && this.summaryRecord.APInvoicesCount == 0 && this.summaryRecord.ARPaymentsCount == 0 && this.summaryRecord.APPaymentsCount == 0
                        && this.summaryRecord.TicketsCount == 0 && this.summaryRecord.ActivitiesCount == 0 && this.summaryRecord.OpportunitiesCount == 0) {
                        this.DoDelete(type);
                    }
                    else {
                        this.Message = "You can't Erase Shippers & Consignees, since you have Business Records, CRM Records or Tickets";
                    }
                }
                break;
            }
        }
    };
    EraseTenantManagementDataComponent.prototype.CloseResponseProgressClicked = function () {
        this.StopTimer();
    };
    EraseTenantManagementDataComponent.prototype.DoDelete = function (type) {
        var _this = this;
        this.Message = null;
        this.Retries = 0;
        this.myService.DeleteDataForTenant(this.entityId, type).subscribe(function (response) {
            if (!response.HasError) {
                var mm = response;
                _this.batchEntity = mm.Result;
                if (_this.batchEntity != null) {
                    _this.IsResponseProgressVisible = true;
                    _this.timer = setInterval(function () { return _this.RunTimerFunction(); }, _this.timerSeconds * 1000);
                }
            }
        });
    };
    EraseTenantManagementDataComponent.prototype.IncreaseTimer = function () {
        var _this = this;
        clearTimeout(this.timer);
        this.timer = setInterval(function () { return _this.RunTimerFunction(); }, this.timerSeconds * 1000);
    };
    EraseTenantManagementDataComponent.prototype.AdjustTimerSpeed = function () {
        if (this.Retries <= 60) {
            if (this.timerSeconds != 1) {
                this.timerSeconds = 1;
                this.IncreaseTimer();
            }
        }
        else if (this.Retries <= 120) {
            if (this.timerSeconds != 5) {
                this.timerSeconds = 5;
                this.IncreaseTimer();
            }
        }
        else if (this.Retries <= 180) {
            if (this.timerSeconds != 60) {
                this.timerSeconds = 60;
                this.IncreaseTimer();
            }
        }
        else {
            this.StopTimer();
        }
    };
    EraseTenantManagementDataComponent.prototype.RunTimerFunction = function () {
        this.Retries++;
        this.GetBTE();
        this.AdjustTimerSpeed();
    };
    EraseTenantManagementDataComponent.prototype.StopTimer = function () {
        if (this.timer) {
            clearTimeout(this.timer);
        }
        this.IsResponseProgressVisible = false;
    };
    EraseTenantManagementDataComponent.prototype.ngOnDestroy = function () {
        this.StopTimer();
    };
    EraseTenantManagementDataComponent.prototype.GetBTE = function () {
        var _this = this;
        var batchTaskExecutionListService = new BatchTaskExecutionListService_1.BatchTaskExecutionListService();
        batchTaskExecutionListService.getSingle(this.batchEntity.Id).subscribe(function (myResult) {
            var mm = myResult;
            if (!mm.HasError) {
                _this.bteList = mm.Result;
                var window = new MessageWindow_1.MessageWindow();
                if (_this.bteList.StatusCode == "D") // D- Done
                 {
                    _this.GetCounts();
                    switch (_this.type) {
                        case "B": {
                            window.Show("Erasing Business Records Completed Succesfully");
                            break;
                        }
                        case "P": {
                            window.Show("Erasing Shippers & Consignees Completed Succesfully");
                            break;
                        }
                        case "T": {
                            window.Show("Erasing Tickets Completed Succesfully");
                            break;
                        }
                        case "C": {
                            window.Show("Erasing CRM Data Completed Succesfully");
                            break;
                        }
                    }
                    _this.StopTimer();
                }
                else if (_this.bteList.StatusCode == "F") // F- Failed
                 {
                    _this.StopTimer();
                    window.Show("Faild: " + _this.bteList.ErrorLog);
                }
            }
        });
    };
    EraseTenantManagementDataComponent.prototype.ResetCountersClicked = function (code) {
        this.Message = null;
        if (this.summaryRecord != null) {
            if (code == "B") {
                if (this.summaryRecord.ShipmentsCount == 0 && this.summaryRecord.QuotesCount == 0 && this.summaryRecord.ARInvoicesCount == 0
                    && this.summaryRecord.APInvoicesCount == 0 && this.summaryRecord.ARPaymentsCount == 0 && this.summaryRecord.APPaymentsCount == 0) {
                    this.DoReset(code);
                }
                else {
                    this.Message = "You can't Reset Counters, since you have Business Records, CRM data";
                }
            }
            else if (code == "P") {
                if (this.summaryRecord.CustomersCount == 0) {
                    this.DoReset(code);
                }
                else {
                    this.Message = "You can't Reset Counters, since you have Shippers & Consignees";
                }
            }
            else if (code == "T") {
                if (this.summaryRecord.TicketsCount == 0) {
                    this.DoReset(code);
                }
                else {
                    this.Message = "You can't Reset Counters, since you have Tickets";
                }
            }
        }
    };
    EraseTenantManagementDataComponent.prototype.DoReset = function (code) {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Reset Counters...");
        this.myService.ResetCountersForTenant(this.entityId, code).subscribe(function (response) {
            if (!response.HasError) {
                var window = new MessageWindow_1.MessageWindow();
                window.Show("Reset Counters Completed Succesfully");
            }
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    EraseTenantManagementDataComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    EraseTenantManagementDataComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './EraseTenantManagementDataComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], EraseTenantManagementDataComponent);
    return EraseTenantManagementDataComponent;
}());
exports.EraseTenantManagementDataComponent = EraseTenantManagementDataComponent;
//# sourceMappingURL=EraseTenantManagementDataComponent.js.map