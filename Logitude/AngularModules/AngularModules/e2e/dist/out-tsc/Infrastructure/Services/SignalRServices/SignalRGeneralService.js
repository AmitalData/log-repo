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
// import the packages  
var core_1 = require("@angular/core");
var Subject_1 = require("rxjs/Subject");
var ServiceHelper_1 = require("../../Utilities/ServiceHelper");
var SignalRGeneralService = /** @class */ (function () {
    function SignalRGeneralService() {
        this.proxyName = 'LogitudeGeneralHub';
        this.startingSubject = new Subject_1.Subject();
        debugger;
        // Constructor initialization  
        this.connectionEstablished = new core_1.EventEmitter();
        this.messageReceived = new core_1.EventEmitter();
        this.connectionExists = false;
        this.starting$ = this.startingSubject.asObservable();
        // create hub connection  
        this.connection = $.hubConnection(ServiceHelper_1.ServiceHelper.GetLogitudeURL());
        // create new proxy as name already given in top  
        this.proxy = this.connection.createHubProxy(this.proxyName);
        // register on server events  
        this.registerOnServerEvents();
        // call the connecion start method to start the connection to send and receive events.  
        this.startConnection();
    }
    //// method to hit from client  
    //public sendTime() {
    //    // server side hub method using proxy.invoke with method name pass as param  
    //    this.proxy.invoke('GetRealTime');
    //}
    //public sendUTCTime() {
    //    // server side hub method using proxy.invoke with method name pass as param  
    //    this.proxy.invoke('GetRealTimeForMyChannel', 'UTC');
    //}
    SignalRGeneralService.prototype.subscribeChannel = function (channelName) {
        // server side hub method using proxy.invoke with method name pass as param  
        this.proxy.invoke('Subscribe', channelName);
    };
    SignalRGeneralService.prototype.unSubscribeChannel = function (channelName) {
        // server side hub method using proxy.invoke with method name pass as param  
        this.proxy.invoke('Unsubscribe', channelName);
    };
    // check in the browser console for either signalr connected or not  
    SignalRGeneralService.prototype.startConnection = function () {
        var _this = this;
        this.connection.start().done(function (data) {
            console.log('Now connected ' + data.transport.name + ', connection ID= ' + data.id);
            _this.connectionEstablished.emit(true);
            _this.connectionExists = true;
            _this.startingSubject.next();
        }).fail(function (error) {
            console.log('Could not connect ' + error);
            _this.connectionEstablished.emit(false);
            _this.startingSubject.error(error);
        });
    };
    SignalRGeneralService.prototype.registerOnServerEvents = function () {
        var _this = this;
        debugger;
        this.proxy.on('onEvent', function (ev) {
            console.log('received in SignalRService: ' + JSON.stringify(ev.Data));
            _this.messageReceived.emit(ev);
        });
        //this.proxy.on('onCachedTableUpdatedEvent', (channel: string, ev: ChannelEvent) => {
        //    console.log('received in SignalRService: ' + JSON.stringify(ev.Data));
        //    this.messageReceived.emit(ev);
        //});
        //this.proxy.on('setUTCRealTime', (data: string) => {
        //    console.log('received in SignalRService: ' + JSON.stringify(data));
        //    this.messageReceived.emit(new MessageDetails(data, true));
        //});
    };
    SignalRGeneralService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], SignalRGeneralService);
    return SignalRGeneralService;
}());
exports.SignalRGeneralService = SignalRGeneralService;
var ChannelEvent = /** @class */ (function () {
    function ChannelEvent() {
        this.Timestamp = new Date();
    }
    return ChannelEvent;
}());
exports.ChannelEvent = ChannelEvent;
//# sourceMappingURL=SignalRGeneralService.js.map