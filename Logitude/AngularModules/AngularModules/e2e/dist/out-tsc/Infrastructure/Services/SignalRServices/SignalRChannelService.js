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
var Subject_1 = require("rxjs/Subject");
var Observable_1 = require("rxjs/Observable");
var ServiceHelper_1 = require("../../Utilities/ServiceHelper");
var SessionInfo_1 = require("../../Utilities/SessionInfo");
var CachedDataManager_1 = require("../../Utilities/CachedDataManager");
var ObjectsLocator_1 = require("../../Locators/ObjectsLocator");
var ConnectionState;
(function (ConnectionState) {
    ConnectionState[ConnectionState["Connecting"] = 1] = "Connecting";
    ConnectionState[ConnectionState["Connected"] = 2] = "Connected";
    ConnectionState[ConnectionState["Reconnecting"] = 3] = "Reconnecting";
    ConnectionState[ConnectionState["Disconnected"] = 4] = "Disconnected";
})(ConnectionState = exports.ConnectionState || (exports.ConnectionState = {}));
var ChannelConfig = /** @class */ (function () {
    function ChannelConfig() {
    }
    return ChannelConfig;
}());
exports.ChannelConfig = ChannelConfig;
var LogitudeHubChannelEvent = /** @class */ (function () {
    //Json: string;
    function LogitudeHubChannelEvent() {
        this.Timestamp = new Date();
    }
    return LogitudeHubChannelEvent;
}());
exports.LogitudeHubChannelEvent = LogitudeHubChannelEvent;
var ChannelSubject = /** @class */ (function () {
    function ChannelSubject() {
    }
    return ChannelSubject;
}());
/**
 * ChannelService is a wrapper around the functionality that SignalR
 * provides to expose the ideas of channels and events. With this service
 * you can subscribe to specific channels (or groups in signalr speak) and
 * use observables to react to specific events sent out on those channels.
 */
var SignalRChannelService = /** @class */ (function () {
    function SignalRChannelService(
    //private window: SignalrWindow,
    //@Inject("channel.config") private channelConfig: ChannelConfig
    ) {
        var _this = this;
        // These are used to feed the public observables 
        //
        this.connectionStateSubject = new Subject_1.Subject();
        this.startingSubject = new Subject_1.Subject();
        this.errorSubject = new Subject_1.Subject();
        // An internal array to track what channel subscriptions exist 
        //
        this.subjects = new Array();
        this.connectionStarted = false;
        if ($ === undefined || $.hubConnection === undefined) {
            throw new Error("The variable '$' or the .hubConnection() function are not defined...please check the SignalR scripts have been loaded properly");
        }
        // Set up our observables
        //
        this.messageReceived = new core_1.EventEmitter();
        var channelConfig = new ChannelConfig();
        channelConfig.url = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + "/signalr";
        channelConfig.hubName = "LogitudeHub";
        this.connectionState$ = this.connectionStateSubject.asObservable();
        this.error$ = this.errorSubject.asObservable();
        this.starting$ = this.startingSubject.asObservable();
        this.hubConnection = $.hubConnection();
        this.hubConnection.url = channelConfig.url;
        //this.hubConnection.qs = { "token": ServiceHelper.GetLoggedUserToken() };
        this.hubProxy = this.hubConnection.createHubProxy(channelConfig.hubName);
        // Define handlers for the connection state events
        //
        this.hubConnection.stateChanged(function (state) {
            var newState = ConnectionState.Connecting;
            switch (state.newState) {
                case $.signalR.connectionState.connecting:
                    newState = ConnectionState.Connecting;
                    break;
                case $.signalR.connectionState.connected:
                    {
                        newState = ConnectionState.Connected;
                        if (state.oldState === $.signalR.connectionState.reconnecting) {
                            CachedDataManager_1.CachedDataManager.CheckCachedTableLastUpdateDate().subscribe(function (reponse) {
                                console.log("cached tables refresh called successfully!");
                            });
                        }
                        break;
                    }
                case $.signalR.connectionState.reconnecting:
                    newState = ConnectionState.Reconnecting;
                    break;
                case $.signalR.connectionState.disconnected:
                    {
                        newState = ConnectionState.Disconnected;
                        _this.startReconnecting();
                    }
                    break;
            }
            // Push the new state on our subject
            //
            console.log("logitude hub is " + ConnectionState[newState]);
            _this.connectionStateSubject.next(newState);
        });
        // Define handlers for any errors
        //
        this.hubConnection.error(function (error) {
            // Push the error on our subject
            //
            console.warn("logitude hub connection error: " + error);
            _this.errorSubject.next(error);
        });
        this.registerOnServerEvents();
        this.startConnection();
    }
    SignalRChannelService.prototype.startReconnecting = function () {
        var _this = this;
        if (this.hubConnection.lastError) {
            console.log("Disconnected. Reason: " + this.hubConnection.lastError.message);
        }
        this.connectionStarted = false;
        window.logitudeHubConnected = false;
        console.log("logitude hub was disconnected, will restart connection after 30 seconds...");
        var upgradingSystemsub = this.startReconnectTimer().subscribe(function (res) {
            upgradingSystemsub.unsubscribe();
            if (_this.connectionStarted != true) {
                //console.log("reconnecting to hub.");
                _this.startConnection();
            }
            else {
                console.log("logitude hub reconnected successfully!");
            }
        });
    };
    SignalRChannelService.prototype.startReconnectTimer = function () {
        return Observable_1.Observable.interval(30000).timeInterval();
    };
    /**
     * Start the SignalR connection. The starting$ stream will emit an
     * event if the connection is established, otherwise it will emit an
     * error.
     */
    SignalRChannelService.prototype.startConnection = function () {
        var _this = this;
        // Now we only want the connection started once, so we have a special
        //  starting$ observable that clients can subscribe to know know if
        //  if the startup sequence is done.
        //
        // If we just mapped the start() promise to an observable, then any time
        //  a client subscried to it the start sequence would be triggered
        //  again since it's a cold observable.
        //
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting.DeploymentStage == "Dev") {
            this.hubConnection.start().done(function (result) {
                window.logitudeHubConnected = true;
                _this.connectionStarted = true;
                console.log(result.data + ' Now connected ' + result.transport.name + ', connection ID= ' + result.id);
                _this.startingSubject.next();
                CachedDataManager_1.CachedDataManager.CheckCachedTableLastUpdateDate().subscribe(function (reponse) {
                    console.log("cached tables refresh called successfully!");
                });
            }).fail(function (error) {
                console.warn('Could not connect ' + error);
                _this.startingSubject.error(error);
            });
        }
        else {
            this.hubConnection.start({ transport: 'webSockets' }).done(function (result) {
                window.logitudeHubConnected = true;
                _this.connectionStarted = true;
                console.log(result.data + ' Now connected ' + result.transport.name + ', connection ID= ' + result.id);
                _this.startingSubject.next();
                CachedDataManager_1.CachedDataManager.CheckCachedTableLastUpdateDate().subscribe(function (reponse) {
                    console.log("cached tables refresh called successfully!");
                });
            }).fail(function (error) {
                console.warn('Could not connect ' + error);
                _this.startingSubject.error(error);
            });
        }
    };
    /**
     * Get an observable that will contain the data associated with a specific
     * channel
     * */
    SignalRChannelService.prototype.subscribeCurrentUserChannel = function () {
        var channelName = "UserChannel" + SessionInfo_1.SessionInfo.LoggedUserId + SessionInfo_1.SessionInfo.LoggedUserTenant;
        return this.subscribeChannel(channelName);
    };
    SignalRChannelService.prototype.subscribeCurrentTenantChannel = function () {
        var channelName = "Tenant" + SessionInfo_1.SessionInfo.LoggedUserTenant + "Channel";
        return this.subscribeChannel(channelName);
    };
    SignalRChannelService.prototype.subscribeChannel = function (channel) {
        // Try to find an observable that we already created for the requested 
        //  channel
        //
        //let channelSub = this.subjects.find((x: ChannelSubject) => {
        //    return x.channel === channel;
        //}) as ChannelSubject;
        var _this = this;
        var channelSub = this.subjects.filter(function (x) { return x.channel === channel; })[0];
        // If we already have one for this event, then just return it
        //
        if (channelSub !== undefined) {
            console.log("Found existing observable for " + channel + " channel");
            return channelSub.subject.asObservable();
        }
        //
        // If we're here then we don't already have the observable to provide the
        //  caller, so we need to call the server method to join the channel 
        //  and then create an observable that the caller can use to received
        //  messages.
        //
        // Now we just create our internal object so we can track this subject
        //  in case someone else wants it too
        //
        channelSub = new ChannelSubject();
        channelSub.channel = channel;
        channelSub.subject = new Subject_1.Subject();
        this.subjects.push(channelSub);
        // Now SignalR is asynchronous, so we need to ensure the connection is
        //  established before we call any server methods. So we'll subscribe to 
        //  the starting$ stream since that won't emit a value until the connection
        //  is ready
        //
        if (this.connectionStarted) {
            console.log("calling channel subscribtion for " + channel);
            this.hubProxy.invoke("Subscribe", channel, ServiceHelper_1.ServiceHelper.GetLoggedUserToken())
                .done(function () {
                console.log("Successfully subscribed to " + channel + " channel");
            })
                .fail(function (error) {
                console.log("subscribtion faild! for " + channel + " channel");
                channelSub.subject.error(error);
            });
        }
        else {
            console.log("starting the hub connection before channel subscribtion");
            this.starting$.subscribe(function () {
                console.log("calling channel subscribtion for " + channel);
                _this.hubProxy.invoke("Subscribe", channel, ServiceHelper_1.ServiceHelper.GetLoggedUserToken())
                    .done(function () {
                    console.log("Successfully subscribed to " + channel + " channel");
                })
                    .fail(function (error) {
                    console.log("subscribtion faild! for " + channel + " channel");
                    channelSub.subject.error(error);
                });
            }, function (error) {
                console.log("starting the hub connection failed!");
                channelSub.subject.error(error);
            });
        }
        return channelSub.subject.asObservable();
    };
    // Not quite sure how to handle this (if at all) since there could be
    //  more than 1 caller subscribed to an observable we created
    //
    //unsubscribe(channel: string): Observable<any> {
    //    this.observables = this.observables.filter((x: ChannelObservable) => {
    //        return x.channel === channel;
    //    });
    //}
    SignalRChannelService.prototype.unSubscribeChannel = function (channel) {
        var _this = this;
        var channelSub = this.subjects.filter(function (x) { return x.channel === channel; })[0];
        // If we already have one for this event, then just return it
        //
        if (channelSub !== undefined) {
            console.log("Found existing observable for " + channel + " channel");
            channelSub.subject.unsubscribe();
            // server side hub method using proxy.invoke with method name pass as param  
            this.hubProxy.invoke("Unsubscribe", channel)
                .done(function () {
                console.log("Successfully unsubscribe from " + channel + " channel");
                var index = _this.subjects.indexOf(channelSub);
                if (index !== -1) {
                    _this.subjects.splice(index, 1);
                }
            })
                .fail(function (error) {
                //channelSub.subject.error(error);
            });
            //this.proxy.invoke('Unsubscribe', channelName);
        }
    };
    /** publish provides a way for calles to emit events on any channel. In a
     * production app the server would ensure that only authorized clients can
     * actually emit the message, but here we're not concerned about that.
     */
    //publish(ev: LogitudeHubChannelEvent): void {
    //    this.hubProxy.invoke("Publish", ev);
    //}
    SignalRChannelService.prototype.registerOnServerEvents = function () {
        var _this = this;
        this.hubProxy.on('onChannelEvent', function (ev) {
            //console.log(`SignalR onChannelEvent received ${ev}`);
            console.log("SignalR onChannelEvent - " + ev.EventName + " event for " + ev.ChannelName + " channel", ev);
            // This method acts like a broker for incoming messages. We 
            //  check the interal array of subjects to see if one exists
            //  for the channel this came in on, and then emit the event
            //  on it. Otherwise we ignore the message.
            var channelSub = _this.subjects.filter(function (x) { return x.channel === ev.ChannelName; })[0];
            // If we found a subject then emit the event on it
            //
            if (channelSub !== undefined) {
                return channelSub.subject.next(ev);
            }
        });
        this.hubProxy.on('onEvent', function (ev) {
            console.log("SignalR onEvent received " + ev.EventName);
            _this.messageReceived.emit(ev);
        });
    };
    SignalRChannelService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], SignalRChannelService);
    return SignalRChannelService;
}());
exports.SignalRChannelService = SignalRChannelService;
//# sourceMappingURL=SignalRChannelService.js.map