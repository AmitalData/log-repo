var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var core_1 = require('angular2/core');
var http_1 = require('angular2/http');
require('rxjs/add/operator/map');
var EmployeeGroupListService = (function () {
    function EmployeeGroupListService(_http) {
        this._http = _http;
        this._sharedEmployeeGroupListArray = [];
        this._apiUrl = 'http://localhost:9996/api/EmployeeGroupPMs';
        console.log("EmployeeGroupPMService instantiated");
        console.log(this._sharedEmployeeGroupListArray);
    }
    EmployeeGroupListService.prototype.getSingleEntityListFromServer = function (id, tenant) {
        return this._http.get(this._apiUrl + '?id=')
            .map(function (res) { return res.json(); });
    };
    //getSingleEntityFromArray(id: string) {
    //    return this._sharedEmployeeGroupListArray.filter(c => c.Id === id)[0];
    //}
    EmployeeGroupListService.prototype.getSingleEntityList = function (id, tenant) {
        var _this = this;
        var exists = this._sharedEmployeeGroupListArray.filter(function (c) { return c.Id === id; });
        if (!exists) {
            return this._http.get(this._apiUrl + '?id=' + id)
                .map(function (res) {
                var pm = res.json();
                _this._sharedEmployeeGroupListArray.push(pm);
                return Promise.Resolve(pm);
            });
        }
        else {
            return Promise.Resolve(this._sharedEmployeeGroupListArray.filter(function (c) { return c.Id === id; })[0]);
        }
    };
    EmployeeGroupListService.prototype.getAllEntityListsFromServer = function (tenant) {
        return this._http.get(this._apiUrl)
            .map(function (res) {
            var dataList = res.json();
            //this._sharedEmployeeGroupListArray.push(dataList);
            return Promise.Resolve(dataList);
        });
    };
    EmployeeGroupListService.prototype.getFilteredEntityListsFromServer = function (filters) {
        var request = JSON.stringify(filters);
        return this._http.get(this._apiUrl + '?queryOperations=');
        //.map(res => res.json());
    };
    EmployeeGroupListService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [http_1.Http])
    ], EmployeeGroupListService);
    return EmployeeGroupListService;
})();
exports.EmployeeGroupListService = EmployeeGroupListService;
//# sourceMappingURL=employee-group-list.service.js.map