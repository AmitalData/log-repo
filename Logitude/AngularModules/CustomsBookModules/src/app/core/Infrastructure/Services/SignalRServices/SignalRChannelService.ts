import { Injectable, Inject, EventEmitter } from '@angular/core';
import { Observable, Subject, interval } from 'rxjs';
import { timeInterval } from 'rxjs/operators';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { SessionLocator } from '../../Utilities/SessionLocator';
import { SessionInfo } from '../../Utilities/SessionInfo';
// import { CachedDataManager } from '../../Utilities/CachedDataManager';
import { ObjectsLocator } from '../../Locators/ObjectsLocator';

/**
 * When SignalR runs it will add functions to the global $ variable
 * that you use to create connections to the hub. However, in this
 * class we won't want to depend on any global variables, so this
 * class provides an abstraction away from using $ directly in here.
 */
//export class SignalrWindow extends Window {
//    $: any;
//}

declare var $: any;
declare var window: any;
export enum ConnectionState {
	Connecting = 1,
	Connected = 2,
	Reconnecting = 3,
	Disconnected = 4,
}

export class ChannelConfig {
	url: string;
	hubName: string;
	channel: string;
}

export class LogitudeHubChannelEvent {
	EventName: string;
	ChannelName: string;
	Timestamp: Date;
	EventParameter: string;
	//Json: string;

	constructor() {
		this.Timestamp = new Date();
	}
}

class ChannelSubject {
	channel: string;
	subject: Subject<LogitudeHubChannelEvent>;
}

/**
 * ChannelService is a wrapper around the functionality that SignalR
 * provides to expose the ideas of channels and events. With this service
 * you can subscribe to specific channels (or groups in signalr speak) and
 * use observables to react to specific events sent out on those channels.
 */
@Injectable()
export class SignalRChannelService {
	/**
	 * starting$ is an observable available to know if the signalr
	 * connection is ready or not. On a successful connection this
	 * stream will emit a value.
	 */
	starting$: Observable<any>;

	/**
	 * connectionState$ provides the current state of the underlying
	 * connection as an observable stream.
	 */
	connectionState$: Observable<ConnectionState>;

	/**
	 * error$ provides a stream of any error messages that occur on the
	 * SignalR connection
	 */
	error$: Observable<string>;

	// These are used to feed the public observables
	//
	private connectionStateSubject = new Subject<ConnectionState>();
	private startingSubject = new Subject<any>();
	private errorSubject = new Subject<any>();

	// These are used to track the internal SignalR state
	//
	private hubConnection: any;
	private hubProxy: any;

	// An internal array to track what channel subscriptions exist
	//
	private subjects = new Array<ChannelSubject>();
	public messageReceived: EventEmitter<LogitudeHubChannelEvent>;
	private connectionStarted: boolean = false;
	constructor() {
		//@Inject("channel.config") private channelConfig: ChannelConfig //private window: SignalrWindow,
		if ($ === undefined || $.hubConnection === undefined) {
			throw new Error(
				"The variable '$' or the .hubConnection() function are not defined...please check the SignalR scripts have been loaded properly",
			);
		}

		// Set up our observables
		//

		this.messageReceived = new EventEmitter<LogitudeHubChannelEvent>();

		let channelConfig = new ChannelConfig();
		channelConfig.url = ServiceHelper.GetLogitudeURL() + '/signalr';
		channelConfig.hubName = 'LogitudeHub';

		this.connectionState$ = this.connectionStateSubject.asObservable();
		this.error$ = this.errorSubject.asObservable();
		this.starting$ = this.startingSubject.asObservable();

		this.hubConnection = $.hubConnection();
		this.hubConnection.url = channelConfig.url;
		//this.hubConnection.qs = { "token": ServiceHelper.GetLoggedUserToken() };
		this.hubProxy = this.hubConnection.createHubProxy(channelConfig.hubName);

		// Define handlers for the connection state events
		//
		this.hubConnection.stateChanged((state: any) => {
			let newState = ConnectionState.Connecting;

			switch (state.newState) {
				case $.signalR.connectionState.connecting:
					newState = ConnectionState.Connecting;
					break;
				case $.signalR.connectionState.connected: {
					newState = ConnectionState.Connected;
					if (state.oldState === $.signalR.connectionState.reconnecting) {
						// CachedDataManager.CheckCachedTableLastUpdateDate().subscribe((reponse) => {
						// 	console.log('cached tables refresh called successfully!');
						// });
					}

					break;
				}
				case $.signalR.connectionState.reconnecting:
					newState = ConnectionState.Reconnecting;
					break;
				case $.signalR.connectionState.disconnected:
					{
						newState = ConnectionState.Disconnected;
						this.startReconnecting();
					}
					break;
			}

			// Push the new state on our subject
			//
			console.log('logitude hub is ' + ConnectionState[newState]);
			this.connectionStateSubject.next(newState);
		});

		// Define handlers for any errors
		//
		this.hubConnection.error((error: any) => {
			// Push the error on our subject
			//
			console.warn('logitude hub connection error: ' + error);
			this.errorSubject.next(error);
		});

		this.registerOnServerEvents();

		this.startConnection();
	}

	startReconnecting() {
		if (this.hubConnection.lastError) {
			console.log('Disconnected. Reason: ' + this.hubConnection.lastError.message);
		}
		this.connectionStarted = false;
		window.logitudeHubConnected = false;
		console.log('logitude hub was disconnected, will restart connection after 30 seconds...');

		var upgradingSystemsub = this.startReconnectTimer().subscribe((res: any) => {
			upgradingSystemsub.unsubscribe();
			if (this.connectionStarted != true) {
				//console.log("reconnecting to hub.");
				this.startConnection();
			} else {
				console.log('logitude hub reconnected successfully!');
			}
		});
	}

	startReconnectTimer() {
		return interval(30000).pipe(timeInterval());
	}

	/**
	 * Start the SignalR connection. The starting$ stream will emit an
	 * event if the connection is established, otherwise it will emit an
	 * error.
	 */

	startConnection(): void {
		// Now we only want the connection started once, so we have a special
		//  starting$ observable that clients can subscribe to know know if
		//  if the startup sequence is done.
		//
		// If we just mapped the start() promise to an observable, then any time
		//  a client subscried to it the start sequence would be triggered
		//  again since it's a cold observable.
		//
		if (ObjectsLocator.GlobalSetting.DeploymentStage == 'Dev') {
			this.hubConnection
				.start()
				.done((result: any) => {
					window.logitudeHubConnected = true;
					this.connectionStarted = true;
					console.log(result.data + ' Now connected ' + result.transport.name + ', connection ID= ' + result.id);
					this.startingSubject.next(null);

					// CachedDataManager.CheckCachedTableLastUpdateDate().subscribe((reponse) => {
					// 	console.log('cached tables refresh called successfully!');
					// });
				})
				.fail((error: any) => {
					console.warn('Could not connect ' + error);
					this.startingSubject.error(error);
				});
		} else {
			this.hubConnection
				.start({ transport: 'webSockets' })
				.done((result: any) => {
					window.logitudeHubConnected = true;
					this.connectionStarted = true;
					console.log(result.data + ' Now connected ' + result.transport.name + ', connection ID= ' + result.id);
					this.startingSubject.next(null); // Pass null as an argument

					// CachedDataManager.CheckCachedTableLastUpdateDate().subscribe((reponse) => {
					// 	console.log('cached tables refresh called successfully!');
					// });
				})
				.fail((error: any) => {
					console.warn('Could not connect ' + error);
					this.startingSubject.error(error);
				});
		}
	}

	/**
	 * Get an observable that will contain the data associated with a specific
	 * channel
	 * */

	subscribeCurrentUserChannel(): Observable<LogitudeHubChannelEvent> {
		var channelName: string = 'UserChannel' + SessionInfo.LoggedUserId + SessionInfo.LoggedUserTenant;

		return this.subscribeChannel(channelName);
	}

	subscribeCurrentTenantChannel(): Observable<LogitudeHubChannelEvent> {
		var channelName: string = 'Tenant' + SessionInfo.LoggedUserTenant + 'Channel';

		return this.subscribeChannel(channelName);
	}

	subscribeChannel(channel: string): Observable<LogitudeHubChannelEvent> {
		// Try to find an observable that we already created for the requested
		//  channel
		//
		//let channelSub = this.subjects.find((x: ChannelSubject) => {
		//    return x.channel === channel;
		//}) as ChannelSubject;

		let channelSub = this.subjects.filter((x) => x.channel === channel)[0] as ChannelSubject;

		// If we already have one for this event, then just return it
		//
		if (channelSub !== undefined) {
			console.log(`Found existing observable for ${channel} channel`);
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
		channelSub.subject = new Subject<LogitudeHubChannelEvent>();
		this.subjects.push(channelSub);

		// Now SignalR is asynchronous, so we need to ensure the connection is
		//  established before we call any server methods. So we'll subscribe to
		//  the starting$ stream since that won't emit a value until the connection
		//  is ready
		//
		if (this.connectionStarted) {
			console.log('calling channel subscribtion for ' + channel);
			this.hubProxy
				.invoke('Subscribe', channel, ServiceHelper.GetLoggedUserToken())
				.done(() => {
					console.log(`Successfully subscribed to ${channel} channel`);
				})
				.fail((error: any) => {
					console.log(`subscribtion faild! for ${channel} channel`);
					channelSub.subject.error(error);
				});
		} else {
			console.log('starting the hub connection before channel subscribtion');
			this.starting$.subscribe(
				() => {
					console.log('calling channel subscribtion for ' + channel);
					this.hubProxy
						.invoke('Subscribe', channel, ServiceHelper.GetLoggedUserToken())
						.done(() => {
							console.log(`Successfully subscribed to ${channel} channel`);
						})
						.fail((error: any) => {
							console.log(`subscribtion faild! for ${channel} channel`);
							channelSub.subject.error(error);
						});
				},
				(error: any) => {
					console.log('starting the hub connection failed!');
					channelSub.subject.error(error);
				},
			);
		}

		return channelSub.subject.asObservable();
	}

	// Not quite sure how to handle this (if at all) since there could be
	//  more than 1 caller subscribed to an observable we created
	//
	//unsubscribe(channel: string): Observable<any> {
	//    this.observables = this.observables.filter((x: ChannelObservable) => {
	//        return x.channel === channel;
	//    });
	//}

	public unSubscribeChannel(channel: string) {
		let channelSub = this.subjects.filter((x) => x.channel === channel)[0] as ChannelSubject;

		// If we already have one for this event, then just return it
		//
		if (channelSub !== undefined) {
			console.log(`Found existing observable for ${channel} channel`);
			channelSub.subject.unsubscribe();

			// server side hub method using proxy.invoke with method name pass as param
			this.hubProxy
				.invoke('Unsubscribe', channel)
				.done(() => {
					console.log(`Successfully unsubscribe from ${channel} channel`);
					let index: number = this.subjects.indexOf(channelSub);
					if (index !== -1) {
						this.subjects.splice(index, 1);
					}
				})
				.fail((error: any) => {
					//channelSub.subject.error(error);
				});

			//this.proxy.invoke('Unsubscribe', channelName);
		}
	}

	/** publish provides a way for calles to emit events on any channel. In a
	 * production app the server would ensure that only authorized clients can
	 * actually emit the message, but here we're not concerned about that.
	 */
	//publish(ev: LogitudeHubChannelEvent): void {
	//    this.hubProxy.invoke("Publish", ev);
	//}

	private registerOnServerEvents(): void {
		this.hubProxy.on('onChannelEvent', (ev: LogitudeHubChannelEvent) => {
			//console.log(`SignalR onChannelEvent received ${ev}`);
			console.log(`SignalR onChannelEvent - ${ev.EventName} event for ${ev.ChannelName} channel`, ev);

			// This method acts like a broker for incoming messages. We
			//  check the interal array of subjects to see if one exists
			//  for the channel this came in on, and then emit the event
			//  on it. Otherwise we ignore the message.

			let channelSub = this.subjects.filter((x) => x.channel === ev.ChannelName)[0] as ChannelSubject;
			// If we found a subject then emit the event on it
			//
			if (channelSub !== undefined) {
				return channelSub.subject.next(ev);
			}
		});

		this.hubProxy.on('onEvent', (ev: LogitudeHubChannelEvent) => {
			console.log(`SignalR onEvent received ${ev.EventName}`);
			this.messageReceived.emit(ev);
		});
	}
}
