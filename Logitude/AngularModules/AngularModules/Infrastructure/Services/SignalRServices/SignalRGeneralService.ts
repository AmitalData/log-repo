
// import the packages  
import {  Injectable,EventEmitter} from '@angular/core';
import { Observable, Subject } from "rxjs";
import { ServiceHelper } from '../../Utilities/ServiceHelper';

// declare the global variables  
declare var $: any;
@Injectable()
export class SignalRGeneralService {
    // Declare the variables  
    private proxy: any;
    private proxyName: string = 'LogitudeGeneralHub';
    private connection: any;
    // create the Event Emitter  
    public messageReceived: EventEmitter<ChannelEvent>;
    public connectionEstablished: EventEmitter<Boolean>;
    public connectionExists: Boolean;

    starting$: Observable<any>;
    private startingSubject = new Subject<any>();

    constructor() {
        // Constructor initialization  
        this.connectionEstablished = new EventEmitter<Boolean>();
        this.messageReceived = new EventEmitter<ChannelEvent>();
        this.connectionExists = false;

        this.starting$ = this.startingSubject.asObservable();

        // create hub connection  
        this.connection = $.hubConnection(ServiceHelper.GetLogitudeURL());
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

    public subscribeChannel(channelName: string) {
        // server side hub method using proxy.invoke with method name pass as param  
        this.proxy.invoke('Subscribe', channelName);
    }
    public unSubscribeChannel(channelName: string) {
        // server side hub method using proxy.invoke with method name pass as param  
        this.proxy.invoke('Unsubscribe', channelName);
    }
    // check in the browser console for either signalr connected or not  
    private startConnection(): void {
        this.connection.start().done((data: any) => {
            console.log('Now connected ' + data.transport.name + ', connection ID= ' + data.id);
            this.connectionEstablished.emit(true);
            this.connectionExists = true;

            this.startingSubject.next();

        }).fail((error: any) => {
            console.log('Could not connect ' + error);
            this.connectionEstablished.emit(false);

            this.startingSubject.error(error);
        });
    }
    private registerOnServerEvents(): void {
        this.proxy.on('onEvent', (ev: ChannelEvent) => {
            console.log('received in SignalRService: ' + JSON.stringify(ev.Data));
            this.messageReceived.emit(ev);
        });

        //this.proxy.on('onCachedTableUpdatedEvent', (channel: string, ev: ChannelEvent) => {
        //    console.log('received in SignalRService: ' + JSON.stringify(ev.Data));
        //    this.messageReceived.emit(ev);
        //});

        //this.proxy.on('setUTCRealTime', (data: string) => {

        //    console.log('received in SignalRService: ' + JSON.stringify(data));
        //    this.messageReceived.emit(new MessageDetails(data, true));
        //});
    }


}
export class ChannelEvent {
    EventName: string;
    ChannelName: string;
    Timestamp: Date;
    Data: any;
    Json: string;

    constructor() {
        this.Timestamp = new Date();
    }
}

