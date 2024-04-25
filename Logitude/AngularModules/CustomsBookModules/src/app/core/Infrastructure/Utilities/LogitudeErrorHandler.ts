import { ErrorHandler } from '@angular/core';
// import {MessageWindow} from '../../Controls/Windows/MessageWindow';
import { Guid } from '../Utilities/Guid';
import { SessionInfo } from '../Utilities/SessionInfo';
import { SessionLocator } from '../Utilities/SessionLocator';
declare var window, OrginalError, Contexting: any;

export class LogitudeErrorHandler implements ErrorHandler {
	// private CurrentSession = SessionLocator.SelectedSession;
	private _console: Console = console;
	//private rethrowError = true;
	//constructor(rethrowError?: boolean) {
	//    this._console = console;
	//}
	handleError(error) {
		this.LogError(error);
	}

	public LogError(error) {
		// this.DisplayErrorMessage(error);
		this.StoreErrorLog(error);

		this._console.error(error);
		this.consoleError(error);
		this.consolePromiseError(error);
	}

	// DisplayErrorMessage(error: any) {
	//     try {
	//         if (this.CurrentSession) {
	//             this.CurrentSession.StopBusyIndicator();

	//             if (error.message) {

	//                     var mywindow = new MessageWindow();
	//                     mywindow.Show(error.message);

	//             }
	//         }
	//     } catch (e) { }
	// }

	StoreErrorLog(error: any) {
		try {
			if (error.message && error.stack) {
				var errorLog: ErrorLogPM = new ErrorLogPM();
				errorLog.Id = Guid.newGuid();
				errorLog.ClientDate = new Date();
				errorLog.Tenant = SessionInfo.LoggedUserTenant;
				errorLog.Tier = 'Client';
				errorLog.UserName = SessionInfo.LoggedUserEmail;
				errorLog.Exception = error.message;
				errorLog.StackTrace = error.stack;

				var rejection = error && error.rejection;
				if (rejection) {
					if (rejection instanceof Error) {
						errorLog.StackTrace = rejection.stack;
					}
				}

				window.sessionStorage.setItem(['ErrorLogs', errorLog.Id], JSON.stringify(errorLog));
			}
		} catch (e) {
			console.error(e);
		}
	}

	consolePromiseError(e) {
		try {
			var rejection = e && e.rejection;
			if (rejection) {
				console.error(
					'Unhandled Promise rejection:',
					rejection instanceof Error ? rejection.message : rejection,
					'; Zone:',
					e.zone.name,
					'; Task:',
					e.task && e.task.source,
					'; Value:',
					rejection,
					rejection instanceof Error ? rejection.stack : undefined,
				);
			}
		} catch (e) {}
	}

	consoleError(error) {
		try {
			var originalError = this._findOriginalError(error);
			var originalStack = this._findOriginalStack(error);
			var context = this._findContext(error);
			this._console.error('EXCEPTION: ' + this._extractMessage(error));
			if (originalError) {
				this._console.error('ORIGINAL EXCEPTION: ' + this._extractMessage(originalError));
			}
			if (originalStack) {
				this._console.error('ORIGINAL STACKTRACE:');
				this._console.error(originalStack);
			}
			if (context) {
				this._console.error('ERROR CONTEXT:');
				this._console.error(context);
			}
			// We rethrow exceptions, so operations like 'bootstrap' will result in an error
			// when an error happens. If we do not rethrow, bootstrap will always succeed.
			//if (this.rethrowError)
			//   throw error;
		} catch (e) {}
	}

	/** @internal */
	_extractMessage(error) {
		return error instanceof Error ? error.message : error.toString();
	}
	/** @internal */
	_findContext(error) {
		if (error) {
			return error.context ? Contexting(error) : this._findContext(OrginalError(error));
		} else {
			return null;
		}
	}
	/** @internal */
	_findOriginalError(error) {
		//while (error && error.originalError) {
		//    error = error.originalError;
		//}

		//return error;

		var e = OrginalError(error);
		while (e && OrginalError(e)) {
			e = OrginalError(e);
		}
		return e;
	}
	/** @internal */
	_findOriginalStack(error) {
		if (!(error instanceof Error)) return null;
		var e = error;
		var stack = e.stack;
		while (e instanceof Error && OrginalError(e)) {
			e = OrginalError(e);
			if (e instanceof Error && e.stack) {
				stack = e.stack;
			}
		}
		return stack;
	}
}
class ErrorLogPM {
	public Id: string;
	public ClientDate: Date;
	public Tenant: number;
	public Tier: string;
	public UserName: string;
	public Exception: string;
	public StackTrace: string;
}

// https://www.bennadel.com/blog/3138-creating-a-custom-errorhandler-in-angular-2-rc-6.htm
