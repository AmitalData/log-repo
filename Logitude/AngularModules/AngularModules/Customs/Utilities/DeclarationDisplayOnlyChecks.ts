
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {DeclarationPM} from '../EntityPMs/DeclarationPM';
import {DeclarationValidator} from '../Validators/DeclarationValidator'
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {Observable}     from 'rxjs/Rx';
import { TextCodeTranslator } from '../../Infrastructure/Utilities/TextCodeTranslator';
import {Http, Headers} from '@angular/http';
import {SessionInfo} from '../../Infrastructure/Utilities/SessionInfo';
import {MenuButtonsEvents, MenuButtonsStateChangedEventArgs} from '../../Infrastructure/Utilities/events/MenuButtonsEvents';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {DeclarationWebService} from '../Services/WebServices/DeclarationWebService';
import { AppTool, ArrayTool, DateTool} from '../../Infrastructure/Tools';
import { CourierMasterValidator } from '../../Customs/Validators/CourierMasterValidator';
import { CustomsRequestsSheetPM } from '../../Customs/EntityPMs/CustomsRequestsSheetPM';

export class DeclarationDisplayOnlyChecks {

    private entityPM: DeclarationPM;
    private publishEventOnFinish: boolean;
    private viewModel: string;
    private http: Http;
    private apiUrl: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustomsRequestSheetExtended';
        this.http = ServiceHelper.Http;
    }
    private timerToken: any;
    private _CourierMasterValidator: CourierMasterValidator = new CourierMasterValidator();

    public DeclarationViewDisplayOnlyChecks(entityPM: DeclarationPM) {
        var editComponentNeedsRefresh: boolean = null;
        if (this.CurrentSession.CurrentEditComponent) {
            editComponentNeedsRefresh = this.CurrentSession.CurrentEditComponent.EditComponentController.MustRefresh;
        }
        //if (!editComponentNeedsRefresh) {
        this.entityPM = entityPM;
        var declarationValidator: DeclarationValidator = new DeclarationValidator();
        declarationValidator.SetEntityPM(entityPM);
        var serviceResponse: ServiceResponse;
        serviceResponse = new ServiceResponse();
        if (this.entityPM.IsCancelled) {
            return Observable.defer(() => {
                // the declaration is cancelled 

                var message = TextCodeTranslator.Translate("Customs.Declaration.O.Cancelled");
                //for menu buttons
                clearTimeout(this.timerToken);
                //this.timerToken = setTimeout(() => {
                //    var args: MenuButtonsStateChangedEventArgs = new MenuButtonsStateChangedEventArgs();
                //    args.MenuButtonsStates = {};
                //    args.MenuButtonsStates["SendDeclaration"] = true;
                //    MenuButtonsEvents.MenuButtonsStateChanged.emit(args);
                //}, 100);

                //save button
                if (this.CurrentSession.CurrentEditComponent) {
                    this.CurrentSession.CurrentEditComponent.IsSaveBtnDisable = true;
                }
                serviceResponse.Result = new DisplayOnlyCheckResult(true, message);
                return Observable.of(serviceResponse);
            });
        }


        //other declaration checks
        declarationValidator.DeclarationViewDisplayOnlyChecks();
        if (declarationValidator.ValidationErrorMessageCodes.length > 0) {
            return Observable.defer(() => {
                var message = TextCodeTranslator.Translate(declarationValidator.ValidationErrorMessageCodes[0]);
                serviceResponse.Result = new DisplayOnlyCheckResult(true, message);
                //for menu buttons
                //this.timerToken = setTimeout(() => {
                //    var args: MenuButtonsStateChangedEventArgs = new MenuButtonsStateChangedEventArgs();
                //    args.MenuButtonsStates = {};
                //    args.MenuButtonsStates["SendDeclaration"] = true;
                //    MenuButtonsEvents.MenuButtonsStateChanged.emit(args);
                //}, 100);
                //save button
                if (this.CurrentSession.CurrentEditComponent) {
                    this.CurrentSession.CurrentEditComponent.IsSaveBtnDisable = true;
                }
                return Observable.of(serviceResponse);
            });
        }
        if (editComponentNeedsRefresh && this.CurrentSession.CurrentEditComponent.EditComponentController.MustRefreshMessage != null) {
            return Observable.defer(() => {
                var text = this.CurrentSession.CurrentEditComponent.EditComponentController.MustRefreshMessage;
                serviceResponse.Result = new DisplayOnlyCheckResult(true, text);
                return Observable.of(serviceResponse);
            });
            
        }


        //Check if changing StorageSiteCode
        if (this.entityPM.IsCourierDeclaration) {
            this._CourierMasterValidator.CheckRequestInProgressForCourierMaster(this.entityPM.Tenant, "UCBCMSS", this.entityPM.CourierMasterId).subscribe((response: any) => {
                var displayOnlyCheckResult = response.Result;
                if (displayOnlyCheckResult != null && displayOnlyCheckResult.length > 0) {
                    let customsRequestsSheetPM: CustomsRequestsSheetPM = displayOnlyCheckResult.filter(r => r.InterfaceTypeCode == "UCBCMSS")[0];
                    if (customsRequestsSheetPM != null) {
                        var errorMessage: string = "קיימת בקשה לשינוי אתר איחסון ברקע ";
                        SessionLocator.SelectedSession.CurrentEditComponent.IsSaveBtnDisable = true;
                        SessionLocator.SelectedSession.CurrentEditComponent.EditComponentController.MustRefresh = true;
                        editComponentNeedsRefresh = SessionLocator.SelectedSession.CurrentEditComponent.EditComponentController.MustRefresh;
                        SessionLocator.SelectedSession.CurrentEditComponent.EditComponentController.MustRefreshMessage = errorMessage;
                        serviceResponse.Result = new DisplayOnlyCheckResult(true, errorMessage);
                        return serviceResponse;
                    }
                }
            });
        }

        // Request sheets in progress check
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        return Observable.defer(() => {
            return this.http.get(this.apiUrl + '/GetRequestInProgress/?' + 'tenant=' + entityPM.Tenant + '&interfaceTypeCode= 2750' + '&objectTableId1=' + "" + '&entityId1=' + "" + '&objectTableId2=' + "" + '&entityId2=' + "" + '&customFileNo=' + entityPM.CustomFileNo + '&displayOnlyMode= true', { headers: authHeader })
                .map(response => {
                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    var requestSheets = response.json();
                    if ((requestSheets == null || requestSheets.length == 0) && !editComponentNeedsRefresh) {
                        if (this.CurrentSession.CurrentEditComponent) {
                            this.CurrentSession.CurrentEditComponent.IsSaveBtnDisable = false;
                            this.CurrentSession.CurrentEditComponent.EditComponentController.IsInBatchRequest = false;

                        }
                        serviceResponse.Result = new DisplayOnlyCheckResult(false, "");
                        return serviceResponse;
                    }
                    else if ((requestSheets[0].InterfaceTypeCode == null || requestSheets[0].InterfaceTypeCode == undefined) && !editComponentNeedsRefresh) {//DUMMY From ITZIK

                        if (this.CurrentSession.CurrentEditComponent) {
                            this.CurrentSession.CurrentEditComponent.IsSaveBtnDisable = false;
                            this.CurrentSession.CurrentEditComponent.EditComponentController.IsInBatchRequest = false;
                        }
                        serviceResponse.Result = new DisplayOnlyCheckResult(false, "");
                        return serviceResponse;

                        //if (this.entityPM.ConcurrencyGUID != requestSheets[0].MainEntityConcurrencyGUID) {
                        //    //this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe
                        //    //this.CurrentSession.CurrentEditComponent.ReloadEntityPM();


                        //}
                    }
                  
                    else {

                        if (this.CurrentSession.CurrentEditComponent) {
                            this.CurrentSession.CurrentEditComponent.IsSaveBtnDisable = true;
                            if (this.CurrentSession.CurrentEditComponent.EditComponentController.IsInBatchRequest) {
                                this.CurrentSession.CurrentEditComponent.EditComponentController.MustRefresh = true;
                                editComponentNeedsRefresh = this.CurrentSession.CurrentEditComponent.EditComponentController.MustRefresh;
                            }
                        }
                        let returnDefualt = () => {
                            var RequestInProgressInterfaceTypeName = requestSheets[0].InterfaceTypeName;
                            var text = TextCodeTranslator.Translate("Customs.General.RequestInProgress");
                            text = text.replace('{0}', RequestInProgressInterfaceTypeName);
                            if (editComponentNeedsRefresh == true) {
                                this.CurrentSession.CurrentEditComponent.EditComponentController.MustRefreshMessage = text;
                            }
                            serviceResponse.Result = new DisplayOnlyCheckResult(true, text);
                            return serviceResponse;
                        };
                        if (requestSheets[0].InterfaceTypeCode == "2755" && requestSheets[0].FutureSendDateTime) {
                            var myFutureSendDateTime: Date;
                            myFutureSendDateTime = new Date(requestSheets[0].FutureSendDateTime);

                            if (myFutureSendDateTime.valueOf() > Date.now().valueOf()) {

                                var datetimeParts = DateTool.GetDateParts(myFutureSendDateTime);
                                var stringOfYear = AppTool.PadLeft("" + datetimeParts.Year, 4, '0');
                                var stringOfMonth = AppTool.PadLeft("" + datetimeParts.Month, 2, '0');
                                var stringOfDay = AppTool.PadLeft("" + datetimeParts.Day, 2, '0');
                                var stringOfHours = AppTool.PadLeft("" + datetimeParts.Hours, 2, '0');
                                var stringOfHours12 = AppTool.PadLeft("" + datetimeParts.Hours12, 2, '0');
                                var stringOfMinutes = AppTool.PadLeft("" + datetimeParts.Minutes, 2, '0');
                                var stringOfSeconds = AppTool.PadLeft("" + datetimeParts.Seconds, 2, '0');
                                var stringOfMilliseconds = AppTool.PadLeft("" + datetimeParts.Milliseconds, 3, '0');
                                var stringDatetime = stringOfYear + "-" + stringOfMonth + "-" + stringOfDay + " " + stringOfHours + ":" + stringOfMinutes + "";

                                 //`לתצוגה בלבד - הוגדרה בקשה מתוזמנת לתאריך ${paymentPM.FuturePaymentDateTime}`;
                                    //`לתצוגה בלבד - הוגדרה בקשה מתוזמנת לתאריך ${requestSheets[0].FutureSendDateTime} - לא ניתן להמשיך עד לסיום טיפול או ביטול הבקשה`;

                                let text = ` הוגדרה בקשה מתוזמנת לתאריך ${stringDatetime} - לא ניתן להמשיך עד לסיום טיפול או ביטול הבקשה`;

                                if (editComponentNeedsRefresh == true) {
                                    this.CurrentSession.CurrentEditComponent.EditComponentController.MustRefreshMessage = text;
                                }
                                serviceResponse.Result = new DisplayOnlyCheckResult(true, text);
                                return serviceResponse;
                            }

                            return returnDefualt();


                        } else {
                            return returnDefualt();
                        }

                    }
                    //return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);
        }

        );
        //}

    }


    GetRequestByInterfaceTypeCode(entityPM: DeclarationPM) {


        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return Observable.defer(() => {
           
            return this.http.get(this.apiUrl + '/GetRequestByInterfaceTypeCode/?' + 'tenant=' + entityPM.Tenant + '&interfaceTypeCode=2755' + '&objectTableId1=' + "" + '&entityId1=' + "" + '&customFileNo=' + entityPM.CustomFileNo , { headers: authHeader })
                    .map(response => {
                        var serviceResponse: ServiceResponse = new ServiceResponse();
                        var requestSheets = response.json();
                        serviceResponse.Result = requestSheets;
                        return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetAnyRequest(interfaceTypeCode: string, customFileNo: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        return Observable.defer(() => {
            return this.http.get(this.apiUrl + '/GetAnyRequest/?' + 'tenant=' + tenant + '&interfaceTypeCode=' + interfaceTypeCode  +'&customFileNo=' + customFileNo , { headers: authHeader })
                .map(response => {
                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    var requestSheets = response.json();
                    serviceResponse.Result = requestSheets;
                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);
        });
    }
    CheckIfRequestInProgress(interfaceTypeCode: string, customFileNo: string, tenant: number, displayOnlyMode: boolean = true) {

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        return Observable.defer(() => {
            return this.http.get(this.apiUrl + '/GetRequestInProgress/?' + 'tenant=' + tenant + '&interfaceTypeCode=' + interfaceTypeCode + '&objectTableId1=' + "" + '&entityId1=' + "" + '&objectTableId2=' + "" + '&entityId2=' + "" + '&customFileNo=' + customFileNo + '&displayOnlyMode=' + displayOnlyMode, { headers: authHeader })
                .map(response => {
                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    var requestSheets = response.json();
                    serviceResponse.Result = requestSheets;
                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);
        });
    }

    CheckIfGeneralRequestInProgress(interfaceTypeCode: string, customFileNo: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        return Observable.defer(() => {
            return this.http.get(this.apiUrl + '/GetGeneralRequestInProgress/?' + 'tenant=' + tenant + '&interfaceTypeCode=' + interfaceTypeCode + '&objectTableId1=' + "" + '&entityId1=' + "" + '&objectTableId2=' + "" + '&entityId2=' + "" + '&customFileNo=' + customFileNo , { headers: authHeader })
                .map(response => {
                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    var requestSheets = response.json();
                    serviceResponse.Result = requestSheets;
                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);
        });
    }
}

export class DisplayOnlyCheckResult {
    constructor(public IsDisplayOnly: boolean,
        public DisplayOnlyMessage: string) {
    }
}
