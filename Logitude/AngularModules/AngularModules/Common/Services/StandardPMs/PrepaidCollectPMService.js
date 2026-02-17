System.register(['@angular/core', '@angular/http', 'rxjs/add/operator/map', 'rxjs/Rx', '../../EntityPMs/PrepaidCollectPM'], function(exports_1) {
    var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
        var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
        if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
        else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
        return c > 3 && r && Object.defineProperty(target, key, r), r;
    };
    var __metadata = (this && this.__metadata) || function (k, v) {
        if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
    };
    var core_1, http_1, Rx_1, PrepaidCollectPM_1;
    var PrepaidCollectPMService;
    return {
        setters:[
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (http_1_1) {
                http_1 = http_1_1;
            },
            function (_1) {},
            function (Rx_1_1) {
                Rx_1 = Rx_1_1;
            },
            function (PrepaidCollectPM_1_1) {
                PrepaidCollectPM_1 = PrepaidCollectPM_1_1;
            }],
        execute: function() {
            PrepaidCollectPMService = (function () {
                function PrepaidCollectPMService() {
                    this._apiUrl = 'http://localhost:9996/api/prepaidcollects';
                }
                PrepaidCollectPMService.prototype.setServiceArgs = function (serviceArgs) {
                    this._serviceArgs = serviceArgs;
                    this._http = serviceArgs.http;
                };
                PrepaidCollectPMService.prototype.get = function (id) {
                    var _this = this;
                    console.log('--------------------------------------> calling getSingleEntityPM:');
                    var authHeader = new http_1.Headers();
                    authHeader.append('Token', '2acc0a3d-a948-41fe-8a19-51a6d787e4fc');
                    return Rx_1.default.Observable.defer(function () {
                        return _this._http.get(_this._apiUrl + '?' + 'id=' + id, {
                            headers: authHeader
                        }).map(function (response) {
                            var pm = response.json();
                            var entity;
                            entity = _this.MapJsonToEntityPM(pm);
                            return entity;
                        });
                    });
                };
                PrepaidCollectPMService.prototype.insert = function (entityPM) {
                    var _this = this;
                    console.log('--------------------------------------> calling updateEntityPM:');
                    var authHeader = new http_1.Headers();
                    authHeader.append('Token', '2acc0a3d-a948-41fe-8a19-51a6d787e4fc');
                    authHeader.append('Content-Type', 'application/json');
                    console.log('server call ---------');
                    return Rx_1.default.Observable.defer(function () {
                        return _this._http.post(_this._apiUrl, JSON.stringify(entityPM), {
                            headers: authHeader,
                        }).map(function (response) {
                            var result = response.json();
                            return result;
                        });
                    });
                };
                PrepaidCollectPMService.prototype.update = function (entityPM) {
                    var _this = this;
                    console.log('--------------------------------------> calling updateEntityPM:');
                    var authHeader = new http_1.Headers();
                    authHeader.append('Token', '2acc0a3d-a948-41fe-8a19-51a6d787e4fc');
                    authHeader.append('Content-Type', 'application/json');
                    console.log('server call ---------');
                    return Rx_1.default.Observable.defer(function () {
                        return _this._http.put(_this._apiUrl, JSON.stringify(entityPM), {
                            headers: authHeader,
                        }).map(function (response) {
                            var result = response.json();
                            return result;
                        });
                    });
                };
                PrepaidCollectPMService.prototype.MapJsonToEntityPM = function (jsonPM) {
                    var entityPM;
                    entityPM = new PrepaidCollectPM_1.PrepaidCollectPM();
                    var jsonPMKeys = Object.keys(jsonPM);
                    for (var key in jsonPMKeys) {
                        var property = jsonPMKeys[key];
                        entityPM[property] = jsonPM[property];
                    }
                    entityPM.IsDirty = false;
                    return entityPM;
                };
                PrepaidCollectPMService = __decorate([
                    core_1.Injectable(), 
                    __metadata('design:paramtypes', [])
                ], PrepaidCollectPMService);
                return PrepaidCollectPMService;
            })();
            exports_1("PrepaidCollectPMService", PrepaidCollectPMService);
        }
    }
});
//# sourceMappingURL=PrepaidCollectPMService.js.map