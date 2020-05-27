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
var DWQueryBuilderService_1 = require("../../../../Infrastructure/Services/ExtendedPMs/DWQueryBuilderService");
var DateSampleComponent = /** @class */ (function () {
    function DateSampleComponent() {
        this.DateSample = "";
        this._DWQueryBuilderService = new DWQueryBuilderService_1.DWQueryBuilderService();
    }
    DateSampleComponent.prototype.ngOnInit = function () {
        var _this = this;
        if (this.ShowSampleDateCommand) {
            this.ShowSampleDateCommand.subscribe(function (DateFilter) {
                _this._DWQueryBuilderService.GetDateFilterSample(DateFilter).subscribe(function (myResult) {
                    if (!myResult.HasError) {
                        _this.DateSample = myResult.Result;
                        //SessionLocator.CurrentSession.StopBusyIndicator();
                        //this.RunReportComplete.emit({ rowData: this.rowData, Msg: "MT5000", Count: this.count });// more than 50000
                    }
                    else {
                        //SessionLocator.CurrentSession.StopBusyIndicator();
                    }
                });
            });
        }
    };
    DateSampleComponent = __decorate([
        core_1.Component({
            selector: 'DateSampleComponent',
            moduleId: module.id,
            templateUrl: './DateSampleComponent.html',
            inputs: ['ShowSampleDateCommand']
        }),
        __metadata("design:paramtypes", [])
    ], DateSampleComponent);
    return DateSampleComponent;
}());
exports.DateSampleComponent = DateSampleComponent;
//# sourceMappingURL=DateSampleComponent.js.map