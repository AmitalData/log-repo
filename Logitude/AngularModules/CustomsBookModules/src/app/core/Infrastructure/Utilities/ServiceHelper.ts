import { ServiceResponse } from '../DataContracts/ServiceResponse';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { AppTool, DateTool } from '../Tools';
// import { MessageWindow } from '../../Controls/Windows/MessageWindow';
import { Guid } from '../Utilities/Guid';
import { SessionInfo } from '../Utilities/SessionInfo';
import { SessionLocator } from '../Utilities/SessionLocator';
import { LogitudeErrorHandler } from '../Utilities/LogitudeErrorHandler';
import { LoginService } from '../Services/LoginService';
import { of } from 'rxjs';

// import { isNullOrUndefined } from 'util';
declare var window: any;

export class ServiceHelper {
	public static HttpClient: HttpClient;

	// private static _CurrentSession = SessionLocator.SelectedSession;
	// private static get CurrentSession() {
	// 	if (this._CurrentSession == null) {
	// 		// this._CurrentSession = SessionLocator.SelectedSession;
	// 	}
	// 	return this._CurrentSession;
	// }
	public static _LogitudeErrorHandler: LogitudeErrorHandler = new LogitudeErrorHandler();

	public static HandleServiceError(error: any) {
		var response: ServiceResponse;
		console.log(response);
		response = new ServiceResponse();
		response.HasError = true;
		if (error instanceof HttpErrorResponse) {
			if (error.status == 400) {
				var apiException = error.error;
				if (
					apiException.ErrorType == 'Exception' ||
					apiException.ErrorType == 'ModelStateError' ||
					apiException.ErrorType == 'DbEntityValidationException' ||
					apiException.ErrorType == 'ApplicationException' ||
					apiException.ErrorType == 'EntityCommandExecutionException' ||
					apiException.ErrorType == 'NullReferenceException' ||
					apiException.ErrorType == 'SecurityException'
				) {
					var errorMessage: string = apiException.ShortErrorMessage;
					if (apiException.ShortErrorMessage) {
						errorMessage = apiException.ShortErrorMessage;
						// response.ErrorsArray.push(apiException.ShortErrorMessage);
					} else {
						errorMessage = apiException.ExceptionMessage;
						//response.ErrorsArray.push(apiException.ExceptionMessage);
					}

					if (errorMessage) {
						if (errorMessage.indexOf('session expiration') == -1) {
							if (errorMessage.indexOf(';') != -1) {
								var errArray = errorMessage.split(';');
								for (var k in errArray) {
									response.ErrorsArray.push(errArray[k]);
								}
							} else {
								response.ErrorsArray.push(errorMessage);
							}
						}
					}
				} else if (apiException.ErrorType == 'AutenticationException') {
					if (!SessionLocator.IsSiguOut) {
						// SessionLocator.HomeComponent.SignoutClicked();
					}
				}

				// Ayman: please leave this commented
				else if (apiException.ErrorType == 'OptimisticConcurrencyException') {
					//var message: string = TextCodeTranslator.Translate("General.M.CantUpdateRecord");
					response.ErrorsArray.push(apiException.ShortErrorMessage);
				} else {
					// ServiceHelper.LogServiceError(apiException.ShortErrorMessage, apiException.ErrorMessage);
				}
			} else {
				if (error.status == 0) {
					console.log(error.message);
					const apiException = error.error;
					let additionalDetails = '';
					if (
						SessionLocator.LoggedUserPM.Email == 'bdd@tests.com' ||
						SessionLocator.LoggedUserPM.Email == 'specflowtest@logitudeworld.com' ||
						SessionLocator.LoggedUserPM.Email == 'BDDSpecialCases@mail.com' ||
						SessionLocator.LoggedUserPM.Email == 'ahmada@logitudeworld.com'
					) {
						additionalDetails =
							': ' + apiException?.ExceptionMessage
								? apiException?.ExceptionMessage
								: error.message
								? error.message
								: error.statusText;
					}
					// ServiceHelper.LogServiceError(
					// 	'There seems to be an Internet Connection Problem' + additionalDetails,
					// 	'net::ERR_CONNECTION_REFUSED',
					// 	false,
					// ); //("net::ERR_CONNECTION_REFUSED", "net::ERR_CONNECTION_REFUSED");
				} else if (error.status == 500) {
					try {
						var errorObject = JSON.parse(error['_body']);
						// ServiceHelper.LogServiceError(
						// 	errorObject.Message + ' ' + errorObject.ExceptionMessage,
						// 	errorObject.StackTrace,
						// );
					} catch (e) {}
					//ServiceHelper._LogitudeErrorHandler.handleError(error);
				}
			}
		} else if (error instanceof HttpErrorResponse) {
			// response = ServiceHelper.HttpClientHandleServiceError(error);
		} else {
			var exceptionmessage = error.message + '\n' + error.stack;
			response.ErrorsArray.push(exceptionmessage);

			ServiceHelper._LogitudeErrorHandler.handleError(error);
		}

		return of(response);
	}

	// private static HttpClientHandleServiceError(error: any) {
	// 	var response: ServiceResponse;
	// 	response = new ServiceResponse();
	// 	response.HasError = true;

	// 	if (error instanceof HttpErrorResponse) {
	// 		if (error.status == 400) {
	// 			var apiException = error.error;
	// 			if (
	// 				apiException.ErrorType == 'Exception' ||
	// 				apiException.ErrorType == 'ModelStateError' ||
	// 				apiException.ErrorType == 'DbEntityValidationException' ||
	// 				apiException.ErrorType == 'ApplicationException' ||
	// 				apiException.ErrorType == 'EntityCommandExecutionException' ||
	// 				apiException.ErrorType == 'NullReferenceException'
	// 			) {
	// 				var errorMessage: string = apiException.ShortErrorMessage;
	// 				if (apiException.ShortErrorMessage) {
	// 					errorMessage = apiException.ShortErrorMessage;
	// 					// response.ErrorsArray.push(apiException.ShortErrorMessage);
	// 				} else {
	// 					errorMessage = apiException.ExceptionMessage;
	// 					//response.ErrorsArray.push(apiException.ExceptionMessage);
	// 				}

	// 				if (errorMessage) {
	// 					if (errorMessage.indexOf('session expiration') == -1) {
	// 						if (errorMessage.indexOf(';') != -1) {
	// 							var errArray = errorMessage.split(';');
	// 							for (var k in errArray) {
	// 								response.ErrorsArray.push(errArray[k]);
	// 							}
	// 						} else {
	// 							response.ErrorsArray.push(errorMessage);
	// 						}
	// 					}
	// 				}
	// 			} else if (apiException.ErrorType == 'AutenticationException') {
	// 				if (!SessionLocator.IsSiguOut) {
	// 					// SessionLocator.HomeComponent.SignoutClicked();
	// 				}
	// 			}

	// 			// Ayman: please leave this commented
	// 			else if (apiException.ErrorType == 'OptimisticConcurrencyException') {
	// 				//var message: string = TextCodeTranslator.Translate("General.M.CantUpdateRecord");
	// 				response.ErrorsArray.push(apiException.ShortErrorMessage);
	// 			} else {
	// 				ServiceHelper.LogServiceError(apiException.ShortErrorMessage, apiException.ErrorMessage);
	// 			}
	// 		} else {
	// 			if (error.status == 0) {
	// 				console.log(error.message);
	// 				const apiException = error.error;
	// 				let additionalDetails = '';
	// 				if (
	// 					SessionLocator.LoggedUserPM.Email == 'angular@fnarsoft.com' ||
	// 					SessionLocator.LoggedUserPM.Email == 'specflowtest@logitudeworld.com' ||
	// 					SessionLocator.LoggedUserPM.Email == 'BDDSpecialCases@mail.com' ||
	// 					SessionLocator.LoggedUserPM.Email == 'ahmada@logitudeworld.com'
	// 				) {
	// 					additionalDetails =
	// 						': ' +
	// 						(!isNullOrUndefined(apiException?.ExceptionMessage)
	// 							? apiException?.ExceptionMessage
	// 							: !isNullOrUndefined(error.message)
	// 							? error.message
	// 							: error.statusText);
	// 				}
	// 				ServiceHelper.LogServiceError(
	// 					'There seems to be an Internet Connection Problem' + additionalDetails,
	// 					'net::ERR_CONNECTION_REFUSED',
	// 					false,
	// 				); //("net::ERR_CONNECTION_REFUSED", "net::ERR_CONNECTION_REFUSED");
	// 			} else if (error.status == 500) {
	// 				try {
	// 					var errorObject = JSON.parse(error['_body']);
	// 					ServiceHelper.LogServiceError(
	// 						errorObject.Message + ' ' + errorObject.ExceptionMessage,
	// 						errorObject.StackTrace,
	// 					);
	// 				} catch (e) {}
	// 				//ServiceHelper._LogitudeErrorHandler.handleError(error);
	// 			}
	// 		}
	// 	} else {
	// 		var exceptionmessage = error.message + '\n' + error.stack;
	// 		response.ErrorsArray.push(exceptionmessage);

	// 		ServiceHelper._LogitudeErrorHandler.handleError(error);
	// 	}

	// 	return response;
	// }

	public static HandleTimerServiceError(error: any) {
		var response: ServiceResponse;
		response = new ServiceResponse();
		response.HasError = true;
		if (error instanceof HttpErrorResponse) {
			//var mm = error.json();
			if (error.status == 400) {
				var apiException = error.error;
				if (
					apiException.ErrorType == 'Exception' ||
					apiException.ErrorType == 'ModelStateError' ||
					apiException.ErrorType == 'DbEntityValidationException' ||
					apiException.ErrorType == 'ApplicationException'
				) {
					var errorMessage: string = apiException.ShortErrorMessage;
					if (apiException.ShortErrorMessage) {
						errorMessage = apiException.ShortErrorMessage;
					} else {
						errorMessage = apiException.ExceptionMessage;
					}

					if (errorMessage) {
						if (errorMessage.indexOf('session expiration') == -1) {
							if (errorMessage.indexOf(';') != -1) {
								var errArray = errorMessage.split(';');
								for (var k in errArray) {
									response.ErrorsArray.push(errArray[k]);
								}
							} else {
								response.ErrorsArray.push(errorMessage);
							}
						}
					}
				} else if (apiException.ErrorType == 'AutenticationException') {
					if (!SessionLocator.IsSiguOut) {
						if (errorMessage != '') {
							// SessionLocator.HomeComponent.SignoutClicked();
						}
					}
				} else {
					console.error(apiException.ShortErrorMessage);
					//ServiceHelper.LogServiceError(apiException.ShortErrorMessage, apiException.ErrorMessage);
				}
			} else {
				if (error.status == 0) {
					console.error('net::ERR_CONNECTION_REFUSED');
					//ServiceHelper.LogServiceError("net::ERR_CONNECTION_REFUSED", "net::ERR_CONNECTION_REFUSED");
				} else {
				}
			}
		} else {
			var exceptionmessage = error.message + '\n' + error.stack;
			response.ErrorsArray.push(exceptionmessage);
		}

		return of(response);
	}

	// private static LogServiceError(exception: string, stackTrace: string, logException = true) {
	// 	try {
	// 		if (exception) {
	// 			if (exception.startsWith('Sorry! you have no permission to do this operation')) return;

	// 			if (this.CurrentSession) {
	// 				this.CurrentSession.StopBusyIndicator();

	// 				if (!this.CurrentSession.IsShowErrorWindow) {
	// 					this.CurrentSession.IsShowErrorWindow = true;
	// 					const mywindow = new MessageWindow();
	// 					mywindow.Show(exception);

	// 					mywindow.WindowClosed.subscribe(() => {
	// 						this.CurrentSession.IsShowErrorWindow = false;
	// 						if (exception) {
	// 							if (exception.indexOf('Internet Connection Problem') > -1) {
	// 								const loginService: LoginService = new LoginService();
	// 								loginService.GetDocumentDownloadToken().subscribe((myResult: any) => {
	// 									if (myResult) {
	// 										SessionInfo.DocumentDownloadToken = myResult;
	// 									}
	// 								});
	// 							}
	// 						}
	// 					});
	// 				}
	// 			}

	// 			if (stackTrace && logException === true) {
	// 				var errorLog: ErrorLogPM = new ErrorLogPM();
	// 				errorLog.Id = Guid.newGuid();
	// 				errorLog.ClientDate = new Date();
	// 				errorLog.Tenant = SessionInfo.LoggedUserTenant;
	// 				errorLog.Tier = 'Client';
	// 				errorLog.UserName = SessionInfo.LoggedUserEmail;
	// 				errorLog.Exception = exception;
	// 				errorLog.StackTrace = stackTrace;
	// 				window.sessionStorage.setItem(['ErrorLogs', errorLog.Id], JSON.stringify(errorLog));

	// 				console.error(exception);
	// 			}
	// 		}
	// 	} catch (e) {
	// 		console.error(e);
	// 	}
	// }

	public static CloneObject(sourceObject: any, targetObject: any = null) {
		if (!targetObject) {
			targetObject = {};
		}

		var jsonPMKeys = Object.keys(sourceObject);
		for (var key in jsonPMKeys) {
			//if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM") {
			//    continue;
			//}

			var property = jsonPMKeys[key];
			targetObject[property] = sourceObject[property];
		}
		return targetObject;
	}
	public static DeepClone(currentObject: any) {
		var newObj = currentObject;
		if (currentObject && typeof currentObject === 'object') {
			if (Object.prototype.toString.call(currentObject) === '[object Array]') {
				newObj = [];
			} else {
				newObj = Object.create(currentObject);
			}

			for (var i in currentObject) {
				newObj[i] = this.DeepClone(currentObject[i]);
			}
		}

		return newObj;
	}
	public static CloneEntityPM(currentObject: any) {
		var newObj = currentObject;

		if (currentObject && typeof currentObject === 'object') {
			if (Object.prototype.toString.call(currentObject) === '[object Array]') {
				newObj = [];
			} else {
				newObj = Object.create(currentObject);
			}

			for (var i in currentObject) {
				if (typeof currentObject[i] === 'object') {
					continue;
				} else {
					newObj[i] = this.DeepClone(currentObject[i]);
				}
			}
		}

		newObj.IsDirty = currentObject.IsDirty;
		currentObject.MyClone = newObj;
		return newObj;
	}
	public static RejectEntityPMChanges(currentObject: any) {
		var oldObj = currentObject.MyClone;
		if (oldObj) {
			var isDirty = currentObject.MyClone.IsDirty;

			if (oldObj && typeof oldObj === 'object') {
				oldObj.MyClone = null;

				for (var i in oldObj) {
					if (typeof oldObj[i] === 'object') {
						continue;
					} else {
						currentObject[i] = this.DeepClone(oldObj[i]);
					}
				}
			}

			currentObject.IsDirty = isDirty;
		}

		return currentObject;
	}
	public static base64ToBufferConvertor(str: string) {
		str = window.atob(str); // creates a ASCII string
		var buffer = new ArrayBuffer(str.length),
			view = new Uint8Array(buffer);
		for (var i = 0; i < str.length; i++) {
			view[i] = str.charCodeAt(i);
		}

		return buffer;
	}
	public static GetLogitudeURL() {
		return AppTool.GetLogitudeURL();
	}
	public static GetDateString(givenDate: Date) {
		var myResult: string = null;

		if (!AppTool.IsNullOrEmpty(givenDate)) {
			var DateParts = DateTool.GetDateParts(givenDate);

			myResult =
				DateParts.Year +
				':' +
				DateParts.Month +
				':' +
				DateParts.Day +
				':' +
				DateParts.Hours +
				':' +
				DateParts.Minutes +
				':' +
				DateParts.Seconds;
		}

		return myResult;
	}

	static MapJsonToEntity<T>(jsonList: any, classType: { new (): T }) {
		var entityList: T;
		entityList = new classType();
		var jsonListKeys = Object.keys(jsonList);

		for (var key in jsonListKeys) {
			var property = jsonListKeys[key];
			entityList[property] = jsonList[property];
		}

		return entityList;
	}
	static MapJsonToArrayofEntities<T>(jsonArray: any, classType: { new (): T }) {
		var mappedArray: Array<T> = [];
		for (var key in jsonArray) {
			var entity = jsonArray[key];
			mappedArray.push(ServiceHelper.MapJsonToEntity<T>(entity, classType));
		}

		return mappedArray;
	}

	public static GetLoggedUserToken() {
		return SessionInfo.Token;
	}

	public static GetLDocumentDownloadToken() {
		return SessionInfo.DocumentDownloadToken;
	}

	public static GetHttpHeaders() {
		const httpOptions = {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
				Token: ServiceHelper.GetLoggedUserToken(),
			}),
		};

		if (!AppTool.IsNullOrEmpty(SessionLocator.WorkerRoleName))
			httpOptions.headers = httpOptions.headers.append('workerrolename', SessionLocator.WorkerRoleName);

		return httpOptions;
	}

	public static GetHttpHeadersForblob() {
		const httpOptions = {
			responseType: 'blob' as 'json',
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
				Token: ServiceHelper.GetLoggedUserToken(),
			}),
		};

		if (!AppTool.IsNullOrEmpty(SessionLocator.WorkerRoleName))
			httpOptions.headers = httpOptions.headers.append('workerrolename', SessionLocator.WorkerRoleName);

		return httpOptions;
	}
	public static GetHttpFullHeaders() {
		const httpOptions: { headers; observe } = {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
				Token: ServiceHelper.GetLoggedUserToken(),
			}),

			observe: 'response',
		};

		if (!AppTool.IsNullOrEmpty(SessionLocator.WorkerRoleName))
			httpOptions.headers = httpOptions.headers.append('workerrolename', SessionLocator.WorkerRoleName);

		return httpOptions;
	}

	public static GetHttpHeadersWithoutToken() {
		const httpOptions = {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
			}),
		};

		return httpOptions;
	}

	public static GetHttpFullHeadersWithoutToken() {
		const httpOptions: { headers; observe } = {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
			}),

			observe: 'response',
		};

		return httpOptions;
	}
	public static OpenWindowWithParams(url: string, params: any[]) {
		var mapForm = document.createElement('form');
		mapForm.target = '_blank';
		mapForm.method = 'POST'; // or "post" if appropriate
		mapForm.action = url;
		for (var i = 0; i < params.length; i++) {
			var mapInput = document.createElement('input');
			mapInput.type = 'hidden';
			mapInput.name = params[i].name;
			mapInput.setAttribute('value', params[i].value);
			mapForm.appendChild(mapInput);
		}
		document.body.appendChild(mapForm);
		mapForm.submit();
		document.body.removeChild(mapForm);
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
