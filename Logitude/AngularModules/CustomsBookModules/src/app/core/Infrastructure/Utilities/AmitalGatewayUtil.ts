// declare var window: any;
// import { UserPM } from '../EntityPMs/UserPM';
// import { SessionLocator } from './SessionLocator';
// import { AppTool } from '../Tools';
// import { EventEmitter } from '@angular/core';
// import { EditComponent } from '../Components/EditComponent/EditComponent';
// import { DeclarationEditComponentController } from '../../Customs/Controller/DeclarationEditComponentController';
// import { LogitudeWindow } from '../../Controls/Windows/LogitudeWindow';
// import { MessageWindow } from '../../Controls/Windows/MessageWindow';

// import { IIGGeneralMessagesService} from '../../Customs/Services/WebServices/IIGGeneralMessagesService';
// //import { CustomsRequestMenuService} from '../../Customs/Services/Others/CustomsRequestMenuService';
// import { ObservableCollection } from './ObservableCollection';
// import { SupplierInvoiceItemList } from "../../Customs/EntityLists/Extended/SupplierInvoiceItemList";
// import { TextCodeTranslator } from './TextCodeTranslator';
// import { ClientPM } from '../../Customs/EntityPMs/ClientPM';

// //import { ClientMessagesService } from '../../Customs/Services/WebServices/ClientMessagesService';
// //import { CustDocMetaDataValuesWebService } from '../../Customs/Services/WebServices/CustDocMetaDataValuesWebService';
// //import { DeclarationMenuButtonsHandler } from '../../Customs/Components/MenuButtons/DeclarationMenuButtonsHandler';
// //import { DeclarationPMService } from '../../Customs/Services/StandardPMs/DeclarationPMService';
// import { DeclarationPM } from '../../Customs/EntityPMs/DeclarationPM';
// // import { ConsignmentPM } from '../../Customs/EntityPMs/ConsignmentPM';
// import { EntityResourceService } from '../Services/EntityResourceService';
// import { EntityPMService } from '../Services/EntityPMService';
// import { CourierMasterPMService } from '../../Customs/Services/StandardPMs/CourierMasterPMService';
// import { ServiceResponse } from '../DataContracts/ServiceResponse';
// import { DeclarationWebService } from '../../Customs/Services/WebServices/DeclarationWebService';
// import { MaintenanceComponent } from 'Infrastructure/Components/Maintenance/MaintenanceComponent';

// export class AmitalGatewayUtil {
//     //private CurrentSession = SessionLocator.SelectedSession;
//     //private constructor() { }
//     private static _Instance: AmitalGatewayUtil;
//     public static get Instance(): AmitalGatewayUtil {
//         this._Instance = this._Instance || new AmitalGatewayUtil();

//         return this._Instance;
//     }
//     public UnifaceRequestArrived: EventEmitter<any> = new EventEmitter();

//     private _AmitalBrowserInUse: boolean = false;
//     public get isUnifreightHost(): boolean {
//         try { return  !!window?.JSBridge || !!window?.parent?.JSBridge; }
//         catch  { return false; }
//     }
//     public get AmitalBrowserInUse(): boolean { return (this._AmitalBrowserInUse === true); }
//     public set AmitalBrowserInUse(newValue: boolean) { this._AmitalBrowserInUse = newValue; }
//     public IsDeclarationInUse(CustomFileNo: string, IsConvertedDeclaration: boolean, IsConnectedToUnifreight: boolean): boolean {
//         if (!AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
//             return false;
//         }
//         if (AppTool.IsNullOrEmpty(CustomFileNo)) {
//             return false;
//         }
//         return true;
//         //if (IsConvertedDeclaration || IsConnectedToUnifreight) {
//        /* if (IsConnectedToUnifreight) {
//             return true;
//         } else {

//             return false;
//         }*/
//     }
//     NoteUnifreightIamReady() {
//         this.sendPostMessage("site ready");

//         let myRequestWrapper = new RequestWrapperM();
//         myRequestWrapper.MessageID = "NoteUnifreightIamReady";
//         myRequestWrapper.UnifreightMessage = new UnifreightMessageM();
//         this.SendRequestJSONToUnifreightAsync(myRequestWrapper);
//     }
//     GetDefaultUnifreightMessageM(): UnifreightMessageM {
//         let unifreightMessageM = new UnifreightMessageM();
//         unifreightMessageM.Requset = [];
//         unifreightMessageM.Response = [];
//         return unifreightMessageM;
//     }

//     public ShowCFIFILEMFUStatusScreen(
//         UnifreightEntityNumber: string,
//         LogitudeEntityNumber: string,
//         ViewModelName: string
//         ) {
//         //var unifreightMessageM = GetMessage(UnifreightEntityNumber, LogitudeEntityNumber, ViewModelName);
//         var unifreightMessageM =
//             AmitalGatewayUtil.Instance.
//                 DeclarationMessaging.GetMessage(UnifreightEntityNumber, LogitudeEntityNumber, ViewModelName, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightEntity());

//         AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
//             "AmitalGatewayUtil.ShowCFIFILEMFUStatusScreen",
//             "CFIHMAIN.LogitudeTask",
//             "ShowCFIFILEMFUStatusScreen",
//             unifreightMessageM,
//             " Feature 62838: מסך עבודה כניסה ל-F/U ביונפרייט");
//     }

//     public ShowDeclarationCertificatesByGroups(
//         UnifreightEntityNumber: string,
//         LogitudeEntityNumber: string,
//         ViewModelName: string,
//         CustomerId: string) {
//         //var unifreightMessageM = GetMessage(UnifreightEntityNumber, LogitudeEntityNumber, ViewModelName);
//         var unifreightMessageM =
//             AmitalGatewayUtil.Instance.
//                 DeclarationMessaging.GetMessage(UnifreightEntityNumber, LogitudeEntityNumber, ViewModelName, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightEntity());
//         unifreightMessageM.Requset.push(["CustomerId", CustomerId]);

//         AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
//             "ScriptableGatewayUtil.ShowDeclarationCertificatesByGroupsUnifreightCallBack",
//             "CFIHMAIN.LogitudeTask",
//             "ShowDeclarationCertificatesByGroupsUnifreightCallBack",
//             unifreightMessageM,
//             " אישורים נדרשים");
//     }

//     public ShowDocumentsSharing(
//         UnifreightEntityNumber: string,
//         LogitudeEntityNumber: string,
//         ViewModelName: string,
//         CustomerId: string) {
//         var unifreightMessageM =
//             AmitalGatewayUtil.Instance.
//                 DeclarationMessaging.GetMessage(UnifreightEntityNumber, LogitudeEntityNumber, ViewModelName, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightEntity());
//         unifreightMessageM.Requset.push(["CustomerId", CustomerId]);

//         AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
//             "ScriptableGatewayUtil.ShowDocumentsSharingUnifreightCallBack",
//             "CFIHMAIN.LogitudeTask",
//             "ShowDocumentsSharingUnifreightCallBack",
//             unifreightMessageM,
//             " שיתוף מסמכים");
//     }

//     public GetRihbitFromTransmissions(
//         UnifreightEntityNumber: string,
//         LogitudeEntityNumber: string,
//         ViewModelName: string,
//         CustomerId: string) {
//         var unifreightMessageM =
//             AmitalGatewayUtil.Instance.
//                 DeclarationMessaging.GetMessage(UnifreightEntityNumber, LogitudeEntityNumber, ViewModelName, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightEntity());
//         unifreightMessageM.Requset.push(["CustomerId", CustomerId]);

//         AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
//             "ScriptableGatewayUtil.GetRihbitFromTransmissions",
//             "CFIHMAIN.LogitudeTask",
//             "GetRihbitFromTransmissions",
//             unifreightMessageM,
//             "קבצי רכבים");
//     }

//     public ShowCFIFILEMMoveToQueueScreen(
//         UnifreightEntityNumber: string,
//         LogitudeEntityNumber: string,
//         ViewModelName: string
//     ) {
//         var unifreightMessageM =
//             AmitalGatewayUtil.Instance.
//                 DeclarationMessaging.GetMessage(UnifreightEntityNumber, LogitudeEntityNumber, ViewModelName, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightEntity());

//         AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
//             "AmitalGatewayUtil.ShowCFIFILEMMoveToQueueScreen",
//             "CFIHMAIN.LogitudeTask",
//             "ShowCFIFILEMMoveToQueueScreen",
//             unifreightMessageM,
//             " העברה לתור");
//     }

//     public ShowCFIFILEMMoveSIToOCRScreen(
//         UnifreightEntityNumber: string,
//         LogitudeEntityNumber: string,
//         ViewModelName: string
//     ) {
//         var unifreightMessageM =
//             AmitalGatewayUtil.Instance.
//                 DeclarationMessaging.GetMessage(UnifreightEntityNumber, LogitudeEntityNumber, ViewModelName, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightEntity());

//         AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
//             "AmitalGatewayUtil.ShowCFIFILEMMoveSIToOCRScreen",
//             "CFIHMAIN.LogitudeTask",
//             "ShowCFIFILEMMoveSIToOCRScreen",
//             unifreightMessageM,
//             " העברת חשבונות ספק ל-OCR");
//     }

//     public ShowCFIFILEMMoveToCollector(
//         UnifreightEntityNumber: string,
//         LogitudeEntityNumber: string,
//         ViewModelName: string
//     ) {
//         var unifreightMessageM =
//             AmitalGatewayUtil.Instance.
//                 DeclarationMessaging.GetMessage(UnifreightEntityNumber, LogitudeEntityNumber, ViewModelName, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightEntity());

//         AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
//             "AmitalGatewayUtil.ShowCFIFILEMMoveToCollector",
//             "CFIHMAIN.LogitudeTask",
//             "ShowCFIFILEMMoveToCollector",
//             unifreightMessageM,
//             " העברה לגובה");
//     }

//     public ShowCFIFILEMEnterRemarks(
//         UnifreightEntityNumber: string,
//         LogitudeEntityNumber: string,
//         ViewModelName: string
//     ) {
//         var unifreightMessageM =
//             AmitalGatewayUtil.Instance.
//                 DeclarationMessaging.GetMessage(UnifreightEntityNumber, LogitudeEntityNumber, ViewModelName, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightEntity());

//         AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
//             "AmitalGatewayUtil.ShowCFIFILEMEnterRemarks",
//             "CFIHMAIN.LogitudeTask",
//             "ShowCFIFILEMEnterRemarks",
//             unifreightMessageM,
//             " הזנת הערות לתור");
//     }
//     public ShowSharedDocuments(
//         UnifreightEntityNumber: string,
//         LogitudeEntityNumber: string,
//         ViewModelName: string
//     ) {
//         var unifreightMessageM =
//             AmitalGatewayUtil.Instance.
//                 DeclarationMessaging.GetMessage(UnifreightEntityNumber, LogitudeEntityNumber, ViewModelName, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightEntity());

//         AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
//             "AmitalGatewayUtil.ShowSharedDocuments",
//             "CFIHMAIN.LogitudeTask",
//             "ShowSharedDocuments",
//             unifreightMessageM,
//             "שיתוף מסמכים");
//     }

//     public NewCustomsFileScreen(
//         ViewModelName: string
//     ) {
//         var unifreightMessageM =
//             AmitalGatewayUtil.Instance.
//                 DeclarationMessaging.GetMessage("", "", ViewModelName, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightEntity());

//         AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
//             "AmitalGatewayUtil.NewCustomsFileScreen",
//             "CFIHMAIN.LogitudeTask",
//             "NewCustomsFileScreen",
//             unifreightMessageM,
//             "פתיחת תיק עמילות חדש");
//     }

//     SendTotangoUserActivity(module: string, activity: string) {
//         var req = new UnifreightMessageM();
//         req.Requset.push(["module", module]);
//         req.Requset.push(["activity", activity]);
//         this.SendRequestToUnifreightAsync("", "",
//             "SendUserActivity", req, "");
//     }
//     public SendRequestToUnifreightAsync(
//         SenderID: string, ReceiverID: string, MessageID: string,
//         unifreightMessageM: UnifreightMessageM, MoreParams: string, showAlert: boolean = true) {

//         let requestWrapper = new RequestWrapperM()
//         requestWrapper.SenderID = SenderID;
//         requestWrapper.ReceiverID = ReceiverID;
//         requestWrapper.MessageID = MessageID;
//         requestWrapper.UnifreightMessage = unifreightMessageM;
//         requestWrapper.MoreParams = MoreParams;

//         this.sendPostMessage(MessageID, MoreParams);
//         this.SendRequestJSONToUnifreightAsync(requestWrapper, showAlert);
//     }

//     SendRequestJSONToUnifreightAsync(myRequestWrapper: RequestWrapperM, showAlert: boolean = true) {
//         try {
//             if (AppTool.IsNullOrEmpty(window.parent._JavascriptGateway) && showAlert) {
//                 alert("_JavascriptGateway not exist !!!");
//                 SessionLocator.SelectedSession.StopBusyIndicator();
//                 return;

//             } else {
//                 var myRequestWrapperJSON = JSON.stringify(myRequestWrapper);
//                 window.parent._JavascriptGateway.SendRequestJSONToUnifreightAsync(myRequestWrapperJSON);
//             }
//         } catch (error) {}
//     }
//     public IsAmitalBackButtonDisable: boolean = false;
//     AmitalBackButtonClicked() {
//         try {
//             close(); // its close only if its open by window.open() method
//             window.parent.close();
//         } catch{}

//         this.sendPostMessage("amitalBackButtonClicked");

//         let RequestWrapper = new RequestWrapperM()
//         let myUnifreightMessageM = new UnifreightMessageM();
//         //myUnifreightMessageM.LogitudeCommandId = "LogitudeCommandId";
//         //myUnifreightMessageM.LogitudeEntity = "LogitudeEntity";
//         //myUnifreightMessageM.Requset.push(["key1", "Value2"]);
//         //myUnifreightMessageM.Requset.push(["key66", "Vafdgdflue2"]);
//         //var j = JSON.stringify(myUnifreightMessageM);
//         RequestWrapper.MessageID = "AmitalBackButtonCommandAction";
//         RequestWrapper.DeclarePurpose = "Uniface have to close GGGQWBLOGITUDE Component"

//         this.SendRequestJSONToUnifreightAsync(RequestWrapper);

//     }
//     public IsTabCA23: boolean = true;//the 1st tab ==> the default tab !!
//     public _LastUnifreightMessageM: UnifreightMessageM;

//     private sendPostMessage(messageID: string, moreParams?: string): void {
//         if (!window.parent) return;

//         const msg = { messageID: messageID, moreParams: moreParams };
//         window.parent.postMessage(msg, '*');

//         if (window.parent.parent)
//             window.parent.parent.postMessage(msg, '*');
//     }

//     UnifaceRequest(myParam, myEditTab, change2EditTab: () => void, change2CA23Tab: () => void) {
//         const MaintenanceMenu: string = "General.MH.Maintenance";
//         let unifreightMessage: UnifreightMessageM = myParam;
//         //if (AppTool.IsNullOrEmpty(unifreightMessage.LogitudeCommandId)) {
//         //    throw new Error("UnifaceRequest get bad  unifreightMessage (LogitudeCommandId is null !?!?!?)");
//         //}
//         this._LastUnifreightMessageM = unifreightMessage
//         this._LastUnifreightMessageM.Requset = this._LastUnifreightMessageM.Requset || [];
//         this._LastUnifreightMessageM.Response = this._LastUnifreightMessageM.Response || [];
//         switch (unifreightMessage.LogitudeCommandId) {

//             case "SessionLocator.SelectedSession.CurrentEditComponent.ReloadEntityPM()":
//             case "this.CurrentSession.CurrentEditComponent.ReloadEntityPM()": {
//                 if (SessionLocator.SelectedSession.CurrentEditComponent) {
//                     SessionLocator.SelectedSession.CurrentEditComponent.ReloadEntityPM();
//                 }
//                 break;
//             }
//             case "CreateInvoiceCommand": {
//                     //$$GGG_IN = "UnifreightEntity=CFIFILEM;UnifreightEntityNumber=%%FILE_NO.CFIFILEM;LogitudeEntity=Customs.Declaration;LogitudeEntityNumber=%%LOGITUDE_FILE.CFIFILEM;LogitudeViewModel=UnifreightMassageHandler;LogitudeCommandId=CreateInvoiceCommand;formtitle=%%$text(IMP_DECLERATION)"

//                 SessionLocator.SelectedSession.StartBusyIndicatorLoading();
//                 this.SelectCustomsRequestMenu(MaintenanceMenu);

//                 let showInvoiceFromUrouter = new ShowInvoiceFromUrouterReturnCreateInvoiceCommand();
//                 showInvoiceFromUrouter.Run(myParam);

//             }
//                 break;
//             case "ShowDeclarationByIdReturnCloseSave": {
//                 //change2EditTab();
//                 //if (AppTool.IsNullOrEmpty(myEditTab.SessionComponent)) {
//                 //    setTimeout(() => { AmitalGatewayUtil.Instance.ShowDeclarationByIdReturnCloseSave.StartDoIt(unifreightMessage, myEditTab, change2CA23Tab); }, 500);
//                 //} else {

//                 //    AmitalGatewayUtil.Instance.ShowDeclarationByIdReturnCloseSave.StartDoIt(unifreightMessage, myEditTab, change2CA23Tab);
//                 //}
//                 this.ShowDeclarationByIdReturnCloseSaveMethod(
//                     myParam, myEditTab, change2EditTab, change2CA23Tab);
//                 break;
//             }
//             case "ShowSupplierInvoiceSelectorByDecIdReturnChosenChildren":
//                 {
//                     this.ShowDeclarationByIdReturnCloseSaveMethod(
//                         myParam, myEditTab, change2EditTab, change2CA23Tab);
//                 }
//                 break;
//             case "ShowClientReturnIfExist":
//                 {
//                     SessionLocator.SelectedSession.StartBusyIndicatorLoading();
//                     let clientAction = new ClientAction();
//                     clientAction.Run(myParam);
//                 }
//                 break;
//             case "MapDocumentTypeCustomsData":
//                 {
//                     SessionLocator.SelectedSession.StartBusyIndicatorLoading();
//                     this.SelectCustomsRequestMenu(MaintenanceMenu);

//                     let mapDocumentTypeCustomsData = new MapDocumentTypeCustomsData();
//                     mapDocumentTypeCustomsData .Run(myParam);
//                 }
//                 break;
//             case "ShowGeneralLOVReturnSelected": {

//                 this.SelectCustomsRequestMenu(MaintenanceMenu);

//                 let mapGeneralLOV = new ShowGeneralLOVReturnSelected();
//                 mapGeneralLOV.Run(myParam);
//             }
//                 break;
//             case "MapExceptionReasonCodeData": {
//                 {
//                     this.SelectCustomsRequestMenu(MaintenanceMenu);

//                     let mapExceptionReasonCodeData = new MapExceptionReasonCodeData();
//                     mapExceptionReasonCodeData.Run(myParam);
//                 }
//             }
//                 break;
//             case "MapPendingReasonCodeData":
//                 {
//                     this.SelectCustomsRequestMenu(MaintenanceMenu);

//                     let mapPendingReasonCodeData = new MapPendingReasonCodeData();
//                     mapPendingReasonCodeData.Run(myParam);
//                 }
//                 break;
//             case "ShowDeclarationStatusQuery":
//                 {
//                     this.SelectCustomsRequestMenu();

//                     SessionLocator.SelectedSession.StartBusyIndicator("");
//                     var servicelink = '../../Customs/Services/StandardPMs/DeclarationPMService';         //mohammad
//                     servicelink = './Customs/Services/StandardPMs/DeclarationPMService';   //itzik !!
//                     SessionLocator.DynamicLoader.GetInstance(servicelink).then((declarationPMService: any) => {
//                         declarationPMService.get(this._LastUnifreightMessageM.LogitudeEntityNumber)
//                             .subscribe((myDeclarationResponse) => {
//                                 SessionLocator.SelectedSession.StopBusyIndicator();
//                                 let decList: DeclarationPM = myDeclarationResponse.Result;
//                                 var servicelink = '../../Customs/Services/Others/CustomsRequestMenuService';
//                                 servicelink = './Customs/Services/Others/CustomsRequestMenuService';
//                                 SessionLocator.DynamicLoader.GetInstance(servicelink).then((service: any) => {
//                                     let my = {
//                                         "DeclarationNumber": decList.DeclarationNumber,
//                                         "CustomsFile": this._LastUnifreightMessageM.UnifreightEntityNumber,
//                                         "DeclarationId": this._LastUnifreightMessageM.LogitudeEntityNumber,
//                                     };
//                                     service.WindowClosed.subscribe(
//                                         (myarg) => {
//                                             //SessionLocator.SelectedSession.CurrentEditComponent.ReloadEntityPM();
//                                             AmitalGatewayUtil.Instance.
//                                                 //ShowClientReturnIfExistUnifreightCallBack(false);
//                                                 AmitalBackButtonClicked();
//                                         }
//                                     );

//                                     service.ShowModalAsEditMenuAction("8250", my);
//                                 });
//                     });
//                              // this will cause the customs to build in aot every time...mohammad.

//                             //let declarationPMService: DeclarationPMService = new DeclarationPMService();

//                             // this will cause the customs to build in aot every time...mohammad.

//                             //let customsRequestMenuService = new CustomsRequestMenuService();
//                             //let my = {
//                             //    "DeclarationNumber": decList.DeclarationNumber,
//                             //    "CustomsFile": this._LastUnifreightMessageM.UnifreightEntityNumber,
//                             //    "DeclarationId": this._LastUnifreightMessageM.LogitudeEntityNumber,
//                             //};
//                             //customsRequestMenuService.WindowClosed.subscribe(
//                             //    (myarg) => {
//                             //        //SessionLocator.SelectedSession.CurrentEditComponent.ReloadEntityPM();
//                             //        AmitalGatewayUtil.Instance.
//                             //            //ShowClientReturnIfExistUnifreightCallBack(false);
//                             //            AmitalBackButtonClicked();
//                             //    }
//                             //);

//                             //customsRequestMenuService.ShowModalAsEditMenuAction("8250", my);

//                         });

//                 }
//                 break;

//             case "ShowManifestQuery":   //..V_TEMP = "ShowManifestQuery"
//                 {
//                     this.SelectCustomsRequestMenu();
//                     SessionLocator.SelectedSession.StartBusyIndicator("");

//                     var servicelink = '../../Customs/Services/StandardPMs/DeclarationPMService';
//                     servicelink = './Customs/Services/StandardPMs/DeclarationPMService';
//                     SessionLocator.DynamicLoader.GetInstance(servicelink).then((declarationPMService: any) => {
//                         declarationPMService.get(this._LastUnifreightMessageM.LogitudeEntityNumber)
//                             .subscribe((myDeclarationResponse) => {
//                                 SessionLocator.SelectedSession.StopBusyIndicator();
//                                 let decList: DeclarationPM = myDeclarationResponse.Result;
//                                 decList.Consignments[0];

//                                 var servicelink = '../../Customs/Services/Others/CustomsRequestMenuService';
//                                 servicelink = './Customs/Services/Others/CustomsRequestMenuService';
//                                 SessionLocator.DynamicLoader.GetInstance(servicelink).then((service: any) => {
//                                     let my = {
//                                         "Mode": "SendCargoQueryRequestFromDeclaration",
//                                         "CargoTypeCode": decList.Consignments[0].CargoTypeCode,
//                                         "ManifestNumber": decList.Consignments[0].ManifestNumber,
//                                         "SecondCargoID": decList.Consignments[0].SecondCargoID,
//                                         "DeclarationId": this._LastUnifreightMessageM.LogitudeEntityNumber,
//                                     };
//                                     service.WindowClosed.subscribe(
//                                         (myarg) => {
//                                             //SessionLocator.SelectedSession.CurrentEditComponent.ReloadEntityPM();
//                                             AmitalGatewayUtil.Instance.
//                                                 //ShowClientReturnIfExistUnifreightCallBack(false);
//                                                 AmitalBackButtonClicked();
//                                         }
//                                     );

//                                     service.ShowModalAsEditMenuAction("8240", my);
//                                 });
//                     });

//                    // let declarationPMService: DeclarationPMService = new DeclarationPMService();

//                             // this will cause the customs to build.
//                             //let customsRequestMenuService = new CustomsRequestMenuService();

//                             //let my = {
//                             //    "Mode": "SendCargoQueryRequestFromDeclaration",
//                             //    "CargoTypeCode": decList.Consignments[0].CargoTypeCode,
//                             //    "ManifestNumber": decList.Consignments[0].ManifestNumber,
//                             //    "SecondCargoID": decList.Consignments[0].SecondCargoID,
//                             //    "DeclarationId": this._LastUnifreightMessageM.LogitudeEntityNumber,
//                             //};
//                             //customsRequestMenuService.WindowClosed.subscribe(
//                             //    (myarg) => {
//                             //        //SessionLocator.SelectedSession.CurrentEditComponent.ReloadEntityPM();
//                             //        AmitalGatewayUtil.Instance.
//                             //            //ShowClientReturnIfExistUnifreightCallBack(false);
//                             //            AmitalBackButtonClicked();
//                             //    }
//                             //);

//                             //customsRequestMenuService.ShowModalAsEditMenuAction("8240", my);

//                         });

//                 }
//                 break;

//             case "ShowCourierMasterByIdReturnCloseSave": {
//                 this.ShowCourierMasterByIdReturnCloseSaveMethod(
//                     myParam, myEditTab, change2EditTab, change2CA23Tab);
//                 break;
//             }

//             case "CreateNewShaamToken": {
//                 this.openCreateNewShaamToken(MaintenanceMenu);
//                 break;
//             }

//             case "ConfirmationNumberTokenLog": {
//                 this.openConfirmationNumberTokenLog(change2EditTab, MaintenanceMenu);
//                 break;
//             }

//             case "showShaamTokenManagment": {
//                 this.showShaamTokenManagment(MaintenanceMenu);
//             }

//             default: {
//                 //throw new Error("UnifaceRequest get bad  unifreightMessage (LogitudeCommandId is unknown ) " + unifreightMessage.LogitudeCommandId);
//                 this.UnifaceRequestArrived.emit(myParam)
//                 //break;
//                 //SessionLocator.SelectedSession
//                 //SessionLocator.AllSessions[0].
//             }
//         }
//     }

//     async showShaamTokenManagment(maintenanceMenu: string) {
//         this.SelectCustomsRequestMenuSafty(maintenanceMenu);
//         let maintenanceComponent: MaintenanceComponent | null = SessionLocator.SelectedSession.menuReference.find(com => com.instance instanceof MaintenanceComponent)?.instance;

//          while (!maintenanceComponent?.PagesMenu) {
//             await new Promise(res => setTimeout(() => res(null), 100));
//             maintenanceComponent = SessionLocator.SelectedSession.menuReference.find(com => com.instance instanceof MaintenanceComponent)?.instance;
//         }

//         const mainItem = maintenanceComponent.PagesMenu.find(x => x.Code === 'SHA');
//         maintenanceComponent.PageChanged(mainItem);
//     }

//     async openCreateNewShaamToken(MaintenanceMenu: string) {
//         this.SelectCustomsRequestMenuSafty(MaintenanceMenu);
//         new EntityResourceService().getEntityResourceByTableName("Customs.ConfirmationNumberTokenLog", 0).subscribe((response: any) => {
//             const windowTitle = TextCodeTranslator.Translate('General.MC.TokenManagement');
//             const logWindow = new LogitudeWindow();
//             logWindow.Width = window.outerWidth;
//             logWindow.Height = window.outerHeight;
//             logWindow.Title = windowTitle;
//             logWindow.IsShowCloseButton = true;
//             logWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/ShaamSettings/ShaamTokensComponent');
//             logWindow.WindowClosed.subscribe(() => AmitalGatewayUtil.Instance.AmitalBackButtonClicked());
//         });
//     }

//     async openConfirmationNumberTokenLog(change2EditTab: () => void, MaintenanceMenu: string) {
//         this.SelectCustomsRequestMenuSafty(MaintenanceMenu);
//         change2EditTab();

//         const objectTableId = window.ObjectTables.filter(d => d.Name == "Customs.ConfirmationNumberTokenLog")[0].Id
//         const allQueries: any[] = window.Queries.filter(x => x.ObjectTableId === objectTableId).sort((a, b) => { return a.IndexOrder - b.IndexOrder });
//         const selectedQuery = allQueries.filter(f => ((f.UserId == SessionLocator.LoggedUserId && f.Tenant == SessionLocator.Tenant) || f.Tenant == 0))[0];
//         const objectTablePM = window.ObjectTables.filter(d => d.Id == objectTableId)[0];

//         const listArgs: any = {};
//         listArgs.QueryCode = selectedQuery.Code;
//         listArgs.ObjectTableName = objectTablePM.Name;
//         listArgs.BackButtonTitle = "Back";

//         new EntityResourceService().getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe((response: any) => {
//             listArgs.DisplayTitle = TextCodeTranslator.Translate(selectedQuery.NameTextCodeCode);
//             SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', SessionLocator.SelectedSession.SessionLocation.viewContainerRef)
//                 .then(cmpRef => {
//                     cmpRef.instance.ComponentRef = cmpRef;
//                     cmpRef.instance.Run(listArgs);
//                     cmpRef.instance.BackCompleted.subscribe(bk => AmitalGatewayUtil.Instance.AmitalBackButtonClicked());
//                 });
//         });
//     }

//     async SelectCustomsRequestMenuSafty(menuCode: string) {
//         while (!SessionLocator.SelectedSession?.MainMenuComponent)
//             await new Promise(res => setTimeout(() => res(null), 100));

//         return this.SelectCustomsRequestMenu(menuCode);
//     }

//     SelectCustomsRequestMenu(menuCode: string = "General.MH.Customs") {

//         var mySelectedItem = SessionLocator.SelectedSession.MainMenuComponent.MainMenuItems
//             .filter(m => m.TextCode ==
//                 //"General.MH.Customs"
//                 menuCode
//             )[0];
//         if (mySelectedItem != null) {
//             SessionLocator.SelectedSession.MainMenuComponent.SelectionChanged(mySelectedItem);
//         }
//     }

//     ShowClientReturnIfExistUnifreightCallBack(exist: boolean) {

//         this._LastUnifreightMessageM.Requset.push(["LogitudeReturnClientExist", exist.toString()]);
//         this._LastUnifreightMessageM.Response.push(["LogitudeReturnClientExist", exist.toString()]);

//                 this.SendRequestToUnifreightAsync(
//             "UnifreightMassageHandler.ShowClientReturnIfExistUnifreightCallBack",
//             "CFIHMAIN.LogitudeTask",
//             "ShowClientReturnIfExistUnifreightCallBack",
//             this._LastUnifreightMessageM,
//                 "Task ???");

//         //CloseEditWindow(false, false);
//     }

//     CreateQInvoiceUnifreightCallBack(ptoCreateQInvoice: string, remark: string, excludeLines : string) {

//         //Response.InvoiceAction

//         let toCreateQInvoice: string = ptoCreateQInvoice ? "1" : "0";
//         this._LastUnifreightMessageM.Requset.push(["InvoiceAction", toCreateQInvoice]);
//         this._LastUnifreightMessageM.Response.push(["InvoiceAction", toCreateQInvoice]);

//         this._LastUnifreightMessageM.Requset.push(["Remark", remark]);
//         this._LastUnifreightMessageM.Response.push(["Remark", remark]);

//         this._LastUnifreightMessageM.Requset.push(["ExcludeLines", excludeLines]);
//         this._LastUnifreightMessageM.Response.push(["ExcludeLines", excludeLines]);

//         this.SendRequestToUnifreightAsync(
//             "UnifreightMassageHandler.CreateInvoiceCommandUnifreightCallBack",
//             "CFIHMAIN.LogitudeTask",
//             "CreateInvoiceCommand",
//             this._LastUnifreightMessageM,
//             "Task Feature 67646: תור חשבוניות - מסך הצגת נתוני חשבונית");

//         //CloseEditWindow(false, false);
//     }
//     ShowGeneralLOVReturnSelectedCallBack(event: string) {

//         this._LastUnifreightMessageM.Requset.push(["ShowGeneralLOVReturnSelectedCancel", (event == "ShowGeneralLOVReturnSelectedCancel").toString()]);
//         this._LastUnifreightMessageM.Requset.push(["ShowGeneralLOVReturnSelectedValue", event]);

//         this.SendRequestToUnifreightAsync(
//             "UnifreightMassageHandler.ShowGeneralLOVReturnSelectedCallBack",
//             "CFIHMAIN.LogitudeTask",
//             "ShowGeneralLOVReturnSelectedCallBack",
//             this._LastUnifreightMessageM,
//             "Task ???");

//         //CloseEditWindow(false, false);
//     }

//     ShowDeclarationByIdReturnCloseSaveMethod(
//         myParam,
//         myEditTab,
//         change2EditTab: () => void,
//         change2CA23Tab: () => void) {
//         change2EditTab();
//         if (AppTool.IsNullOrEmpty(myEditTab.SessionComponent)) {
//             setTimeout(() => { AmitalGatewayUtil.Instance.ShowDeclarationByIdReturnCloseSave.StartDoIt(this._LastUnifreightMessageM, myEditTab, change2CA23Tab); }, 500);
//         } else {

//             AmitalGatewayUtil.Instance.ShowDeclarationByIdReturnCloseSave.StartDoIt(this._LastUnifreightMessageM, myEditTab, change2CA23Tab);
//         }
//     }

//     ShowDeclarationByIdReturnCloseSave = class {
//         static StartDoIt(unifreightMessage: UnifreightMessageM, myEditTab, callback2TabZero: () => void) {
//             //BackButtonLabel: "הצהרות ללם התרה"EntityId :"1-103991" ,ObjectTableName:"Customs.Declaration"
//             let isSaved: boolean = false;
//             let BackButtonLabel = "תיק עמילות"

//             SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent',
//                 //myEditTab.SessionComponent.viewContainerRef
//                 SessionLocator.SelectedSession.SessionLocation.viewContainerRef
//                 //SessionLocator.AllSessions[1].SessionLocation.viewContainerRef
//             )
//                 .then((cmpRef:any) => {
//                      //this.SelectionChanged(myDeclarationEditTab);
//                     cmpRef.instance.ComponentRef = cmpRef;
//                     let myEditComponent: EditComponent = cmpRef.instance;

//                     if (unifreightMessage.UnifreightEntity == "BFIFILE" /*|| myEditComponent.EntityPM?.Direction == "E"*/)
//                     {
//                         BackButtonLabel = "תיק יצוא"
//                     }
//                     if(!AmitalGatewayUtil.Instance.AmitalBrowserInUse){

//                         const openDec= TextCodeTranslator.Translate("Customs.Declaration.Q.DeclarationWithoutReleaseQuery");
//                         if(!AppTool.IsNullOrEmpty(openDec) ) {
//                             BackButtonLabel = openDec;
//                         } else {
//                             BackButtonLabel = "הצהרות פתוחות";
//                         }

//                     }
//                     let myDeclarationEditComponentController: DeclarationEditComponentController = myEditComponent.EditComponentController as DeclarationEditComponentController;
//                     this.getEntity(unifreightMessage.LogitudeEntityNumber).subscribe((data:any) => {
//                         cmpRef.instance.Run({
//                             EntityId: (data && data.Result) ? data.Result.Id : unifreightMessage.LogitudeEntityNumber, //unifreightMessage.LogitudeEntityNumber,//"1-103991"

//                             ObjectTableName: unifreightMessage.LogitudeEntity,//'Customs.Declaration'
//                             BackButtonLabel: BackButtonLabel

//                         });
//                      })

//                     let lockMess = UnifreightMessageM.GetStringValue(unifreightMessage, "Requset.LockedMessage");
//                     let unifreightJumpTo = UnifreightMessageM.GetStringValue(unifreightMessage, "Requset.JumpTo");
//                     console.log(lockMess);
//                      if (!AppTool.IsNullOrEmpty(lockMess)) {

//                         let sub = myEditComponent.OnFirstTimeAfterSingleDataLoaded.subscribe(
//                             (token1) => {
//                                 sub.unsubscribe();
//                                   myDeclarationEditComponentController = myEditComponent.EditComponentController as DeclarationEditComponentController;
//                                 if (AppTool.IsNullOrEmpty(myDeclarationEditComponentController)) {
//                                     console.log("myDeclarationEditComponentController is null");
//                                 } else {
//                                     myDeclarationEditComponentController.UnifaceStartAsLock(lockMess);
//                                 }
//                             }
//                         );

//                     }

//                     if (unifreightMessage.LogitudeCommandId == "ShowSupplierInvoiceSelectorByDecIdReturnChosenChildren") {
//                         //"UnifreightEntity=CFIFILEM·;UnifreightEntityNumber=172900228·;LogitudeEntity=Customs.Declaration·;LogitudeEntityNumber=1-103927·;LogitudeViewModel=UnifreightMassageHandler·;LogitudeCommandId=ShowSupplierInvoiceSelectorByDecIdReturnChosenChildren·;formtitle=חשבונית ספק"
//                         //0042-17 //3822
//                         //cmpRef.instance.LoadCompleted.subscribe(loadSuccess => {
//                         let sub = myEditComponent.OnFirstTimeAfterSingleDataLoaded.subscribe(
//                             (token1) =>
//                             //if (loadSuccess)
//                             {
//                                 sub.unsubscribe();
//                                 let windowArgs: any = {};
//                                 let certificates: any[] = [];
//                                 windowArgs.DeclarationPM = myEditComponent.EntityPM;
//                                 windowArgs.Parent = {};
//                                 windowArgs.Parent.SelectedInvoiceItems = [];
//                                 windowArgs.Parent.SelectedInvoices = [];
//                                 //windowArgs.Protest = this.protestPM;
//                                 var logWindow = new LogitudeWindow();
//                                 logWindow.Height = 700;
//                                 logWindow.Width = 1000;
//                                 logWindow.ShowCloseButton = true;
//                                 logWindow.WindowArgs = windowArgs;
//                                 logWindow.WindowClosed.subscribe(($event: any) => {
//                                     //alert("WindowClosed : ");
//                                     //if ("ok" == $event)
//                                     console.log(windowArgs);

//                                     let mySelectedInvoiceItems: ObservableCollection;
//                                     mySelectedInvoiceItems = windowArgs.Parent.SelectedInvoiceItems;
//                                     if (!AppTool.IsNullOrEmpty(mySelectedInvoiceItems) &&
//                                         !AppTool.IsNullOrEmpty(mySelectedInvoiceItems.Collection) &&
//                                         mySelectedInvoiceItems.Collection.length > 0) {
//                                         let mySelectedInvoiceItem: SupplierInvoiceItemList = mySelectedInvoiceItems.Collection[0];
//                                         let myIIGGeneralMessagesService = new IIGGeneralMessagesService();
//                                         myIIGGeneralMessagesService.GetLOGISUPPACC(
//                                             mySelectedInvoiceItem.DeclarationId,
//                                             mySelectedInvoiceItem.CounterKey,
//                                             mySelectedInvoiceItem.LineNumber,
//                                             mySelectedInvoiceItem.Tenant).subscribe(
//                                             (serviceResponse:any) => {
//                                                 let xmlSupplierInvoiceSelector: string
//                                                     = serviceResponse.Result;
//                                                 //xmlSupplierInvoiceSelector = serviceResponse.
//                                                 this.ShowSupplierInvoiceSelectorByDecIdUnifreightCallBack(xmlSupplierInvoiceSelector, false);
//                                                 callback2TabZero();
//                                             });

//                                     } else {
//                                         this.ShowSupplierInvoiceSelectorByDecIdUnifreightCallBack("", true);
//                                         callback2TabZero();

//                                     }
//                                     return;

//                                 });

//                                 //logWindow.Show('./Customs/Components/Declaration/DeclarationPayment/SupplierInvoiceSelectionComponent');
//                                 logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationOthers/Components/DeclarationPayment/SupplierInvoiceSelectionComponent');
//                             });
//                         return;
//                     }

//                     if (!AppTool.IsNullOrEmpty(unifreightJumpTo)) {
//                         let sub = myEditComponent.OnFirstTimeAfterSingleDataLoaded.subscribe(
//                             (token1) => {
//                                 switch (unifreightJumpTo) {
//                                     case "Payment":
//                                         {
//                                             var servicelink = '../../Customs/Components/MenuButtons/DeclarationMenuButtonsHandler';
//                                             servicelink = './Customs/Components/MenuButtons/DeclarationMenuButtonsHandler';
//                                             SessionLocator.DynamicLoader.GetInstance(servicelink).then((handler: any) => {
//                                                 handler.EntityPM = myEditComponent.EntityPM;
//                                                 handler.OpenPaymentOrderWindow();
//                                             });
//                                             // this will cause the customs to build ..... mohammad.
//                                             //var handler = new DeclarationMenuButtonsHandler();
//                                             //handler.EntityPM=myEditComponent.EntityPM;
//                                             //handler.OpenPaymentOrderWindow();
//                                         }
//                                         break;
//                                     case "RequestSheet":
//                                         {
//                                             //Code  :"DCCA""Customs.Declaration.TH.CustomsAnswers"

//                                             //Code:                        "DCRS"                                                "Customs.Declaration.TH.RequestSheet"

//                                             myEditComponent.PreSelectedTabCode = "DCRS";

//                                         }
//                                         break;
//                                     case "Answer":
//                                         {
//                                             //Code  :"DCCA""Customs.Declaration.TH.CustomsAnswers"

//                                             myEditComponent.PreSelectedTabCode = "DCCA";

//                                         }
//                                         break;

//                                 }
//                             });

//                     }

//                     cmpRef.instance.SaveCompleted.subscribe(saveIt => {
//                         isSaved = true;
//                     });
//                     cmpRef.instance.BackCompleted.subscribe(bk => {
//                         this.ShowDeclarationByIdUnifreightCallBack(isSaved);
//                         callback2TabZero();

//                     });
//                 });

//         }

//         static getEntity(LogitudeEntityNumber: string) {
//             var declarationWebService: DeclarationWebService = new DeclarationWebService();
//             return declarationWebService.GetAcceptDeclarationAmendment(LogitudeEntityNumber)

//         }

//         private static ShowSupplierInvoiceSelectorByDecIdUnifreightCallBack(
//             SupplierInvoiceSelector: string, CancelButtonClick: boolean) {

//             //args = args ?? new CustomizedEventArgs.ChildrenCoosingEventArgs();

//             if (AmitalGatewayUtil.Instance._LastUnifreightMessageM.Requset.filter((item) => item[0] == "ShowSupplierInvoiceSelectorByDecIdUnifreightCallBack").length == 0) {
//                 AmitalGatewayUtil.Instance._LastUnifreightMessageM.Requset.push(["ShowSupplierInvoiceSelectorByDecIdUnifreightCallBack", ""]);
//             }

//             //if (_BusyIndicatorStartEvent == null) {
//             //    _BusyIndicatorStartEvent = _CurrentAssemlyLocator.EventAggregator.GetEvent<BusyIndicatorStartEvent>();
//             //}
//             //_BusyIndicatorStartEvent.Publish(new BusyIndicatorStartEventArgs() { Start = false });
//             if (!AppTool.IsNullOrEmpty(SupplierInvoiceSelector) && !CancelButtonClick) {

//                 AmitalGatewayUtil.Instance._LastUnifreightMessageM.Requset.push(["SupplierInvoiceSelector", SupplierInvoiceSelector]);
//             }
//             else {
//                 AmitalGatewayUtil.Instance._LastUnifreightMessageM.Requset["CancelButtonClick"] = "CancelButtonClick";
//             }

//             var myRequestWrapperM = new RequestWrapperM();

//             myRequestWrapperM.SenderID = "UnifreightMassageHandler.ShowSupplierInvoiceSelectorByDecIdUnifreightCallBack";
//             myRequestWrapperM.ReceiverID = "CFIHMAIN.LogitudeTask";
//             myRequestWrapperM.MessageID = "ShowSupplierInvoiceSelectorByDecIdUnifreightCallBack";
//             myRequestWrapperM.UnifreightMessage = AmitalGatewayUtil.Instance._LastUnifreightMessageM;

//             AmitalGatewayUtil.Instance.SendRequestJSONToUnifreightAsync(myRequestWrapperM);

//             //CloseEditWindow(false, false);

//         }

//         private static ShowDeclarationByIdUnifreightCallBack(save: boolean) {
//             if (AmitalGatewayUtil.Instance._LastUnifreightMessageM.Requset.filter((item) => item[0] == "ShowDeclarationByIdUnifreightCallBack").length == 0) {
//                 AmitalGatewayUtil.Instance._LastUnifreightMessageM.Requset.push(["ShowDeclarationByIdUnifreightCallBack", save.toString()]);
//             }
//             let tuple = AmitalGatewayUtil.Instance._LastUnifreightMessageM.Requset.filter((item) => item[0] == "ShowDeclarationByIdUnifreightCallBack")[0];
//             tuple[1] = save.toString();

//             var myRequestWrapperM = new RequestWrapperM();

//             myRequestWrapperM.SenderID = "UnifreightMassageHandler.ShowDeclarationByIdUnifreightCallBack";
//             myRequestWrapperM.ReceiverID = "CFIHMAIN.LogitudeTask";
//             myRequestWrapperM.MessageID = "ShowDeclarationByIdUnifreightCallBack";
//             myRequestWrapperM.UnifreightMessage = AmitalGatewayUtil.Instance._LastUnifreightMessageM;

//             AmitalGatewayUtil.Instance.SendRequestJSONToUnifreightAsync(myRequestWrapperM);
//         }
//     }

//     ShowCourierMasterByIdReturnCloseSaveMethod(
//         myParam,
//         myEditTab,
//         change2EditTab: () => void,
//         change2CA23Tab: () => void) {
//         change2EditTab();
//         if (AppTool.IsNullOrEmpty(myEditTab.SessionComponent)) {
//             setTimeout(() => { AmitalGatewayUtil.Instance.ShowCourierMasterById.StartDoItForCourier(this._LastUnifreightMessageM, myEditTab, change2CA23Tab); }, 500);
//         } else {

//             AmitalGatewayUtil.Instance.ShowCourierMasterById.StartDoItForCourier(this._LastUnifreightMessageM, myEditTab, change2CA23Tab);
//         }
//     }

//     ShowCourierMasterById = class {
//         static _entityResourceService: EntityResourceService = new EntityResourceService();
//         static StartDoItForCourier(unifreightMessage: UnifreightMessageM, myEditTab, callback2TabZero: () => void) {
//             let isSaved: boolean = false;
//             let BackButtonLabel = "תיק עמילות"
//             var windowArgs: any = {};
//             let courierMasterService: CourierMasterPMService = new CourierMasterPMService();

//             this._entityResourceService.getEntityResourceByTableName("Customs.CourierMaster").subscribe(response => {
//                 this._entityResourceService.getEntityResourceByTableName("Customs.DeclarationCourierStatus").subscribe(response => {
//                     this._entityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(response => {
//                         courierMasterService.get(unifreightMessage.LogitudeEntityNumber).subscribe((myResponse: ServiceResponse) => {
//                                 if (myResponse.HasError) {
//                                     console.log("Error while getting EntityPM", myResponse);
//                                 }
//                                 else {
//                                     windowArgs.CurrentEntity = myResponse.Result;
//                                     var logWindow = new LogitudeWindow();
//                                     logWindow.Width = 1500;
//                                     logWindow.Height = 1000;
//                                     logWindow.WindowArgs = windowArgs;
//                                     logWindow.ShowCloseButton = true;
//                                     //logWindow.IsHideHeader = true;
//                                     logWindow.IsFillScreen = true;
//                                     AmitalGatewayUtil.Instance.IsAmitalBackButtonDisable = true;
//                                     logWindow.Show('./CustomsModules/CustomsCourier/Components/CourierWorkSheet/CourierWorksheetComponent');
//                                     logWindow.WindowClosed.subscribe(($event1: any) => {
//                                         //AmitalGatewayUtil.Instance.IsAmitalBackButtonDisable = false;
//                                         this.ShowCourierMasterByIdReturnCloseSaveCallBack(isSaved);
//                                         callback2TabZero();

//                                     });
//                                 }
//                             });

//                         });
//                     });
//                 });
//             }

//         private static ShowCourierMasterByIdReturnCloseSaveCallBack(save: boolean) {
//             if (AmitalGatewayUtil.Instance._LastUnifreightMessageM.Requset.filter((item) => item[0] == "ShowCourierMasterByIdReturnCloseSaveCallBack").length == 0) {
//                 AmitalGatewayUtil.Instance._LastUnifreightMessageM.Requset.push(["ShowCourierMasterByIdReturnCloseSaveCallBack", save.toString()]);
//             }
//             let tuple = AmitalGatewayUtil.Instance._LastUnifreightMessageM.Requset.filter((item) => item[0] == "ShowCourierMasterByIdReturnCloseSaveCallBack")[0];
//             tuple[1] = save.toString();

//             var myRequestWrapperM = new RequestWrapperM();

//             myRequestWrapperM.SenderID = "UnifreightMassageHandler.ShowCourierMasterByIdReturnCloseSaveCallBack";
//             myRequestWrapperM.ReceiverID = "CFIHMAIN.LogitudeTask";
//             myRequestWrapperM.MessageID = "ShowCourierMasterByIdReturnCloseSaveCallBack";
//             myRequestWrapperM.UnifreightMessage = AmitalGatewayUtil.Instance._LastUnifreightMessageM;

//             AmitalGatewayUtil.Instance.SendRequestJSONToUnifreightAsync(myRequestWrapperM);
//         }
//     }
//     GeneralMessaging = class {
//         public static get ResponseEntityAlreadyLockKey() { return "Response.EntityAlreadyLock"; }
//         public static get ResponseEntityAlreadyLockMessage() { return "Response.EntityAlreadyLockMessage"; }
//         public static get RaiseUnlockIIGEntityMessageId() { return "RaiseUnlockIIGEntityMessage"; }

//         public static get RaiseLockIIGEntReturnEntityAlreadyLockMessage() { return "RaiseLockIIGEntReturnEntityAlreadyLockMessage"; }
//         public static RaiseLockIIGEntityReturnEntityAlreadyLock
//             (
//             LogitudeEntityName: string,
//             LogitudeEntityNumber: string,
//             ViewModelName: string) {

//             var unifreightMessageM = AmitalGatewayUtil.Instance.DeclarationMessaging.GetMessage("", LogitudeEntityNumber, ViewModelName, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightEntity());
//             unifreightMessageM.LogitudeEntity = LogitudeEntityName;

//             unifreightMessageM.Requset.push(["ExpectedCallBack", AmitalGatewayUtil.Instance.GeneralMessaging.ResponseEntityAlreadyLockKey]);

//             AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
//                 "AmitalGatewayUtil.RaiseLockIIGEntReturnEntityAlreadyLock",
//                 "CFIHMAIN.LogitudeTask",
//                 AmitalGatewayUtil.Instance.GeneralMessaging.RaiseLockIIGEntReturnEntityAlreadyLockMessage,
//                 unifreightMessageM,
//                 "XXXEditControlViewModelController.OnFirstTimeSingleDataLoaded.RaiseLockIIGEntReturnEntityAlreadyLock");
//         }

//         public static RaiseUnlockIIGEntity(LogitudeEntity, LogitudeEntityNumber,
//             HaveSaved: boolean) {

//             var unifreightMessageM = AmitalGatewayUtil.Instance.DeclarationMessaging.GetMessage("", LogitudeEntityNumber, "", AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightEntity());
//             unifreightMessageM.LogitudeEntity = LogitudeEntity;
//             unifreightMessageM.Requset.push(["HaveSaved", HaveSaved.toString()]);

//             AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
//                 "AmitalGatewayUtil.RaiseUnlockIIGEntity",
//                 "CFIHMAIN.LogitudeTask",
//                 AmitalGatewayUtil.Instance.GeneralMessaging.RaiseUnlockIIGEntityMessageId,
//                 unifreightMessageM,
//                 "XXXEditControlViewModelController.OnFirstTimeSingleDataLoaded.RaiseUnlockIIGEntity");
//         }
//     }

//     DeclarationMessaging = class {

//         //public static string InstructionReturnCanIContinue { get { return "Response.InstructionReturnCanIContinue"; } }
//         public static get UnifreightResponseStatus() { return "Response.UnifreightResponseStatus"; }
//         public static get LogitudeEntityDeclaration() { return "Declaration"; }
//         public static get ResponseCFIFILMAlreadyLockKey() { return "Response.CFIFILMAlreadyLock"; }
//         public static get ResponseCFIFILMAlreadyLockMessgae() { return "Response.CFIFILMAlreadyLockMessgae"; }
//         public static get ResponseBFIFILEAlreadyLockKey() { return "Response.BFIFILEAlreadyLock"; }
//         public static get ResponseBFIFILEAlreadyLockMessgae() { return "Response.BFIFILEAlreadyLockMessgae"; }
//         public static get RaiseInstructionReturnCanIContinueMessage() { return "RaiseInstructionReturnCanIContinueMessage"; }
//         public static get RaiseCFIFILMLockReturnCFIFILMAlreadyLockMessage() { return "RaiseCFIFILMLockReturnCFIFILMAlreadyLockMessage"; }
//         public static get RaiseBFIFILELockReturnBFIFILEAlreadyLockMessage() { return "RaiseBFIFILELockReturnBFIFILEAlreadyLockMessage"; }
//         public static get RaiseUnlockCFIFILEMMessage() { return "RaiseUnlockCFIFILEMMessage"; }
//         public static get RaiseUnlockBFIFILEMessage() { return "RaiseUnlockBFIFILEMessage"; }
//         public static get PrintStimulReturnCanIContinue() { return "Response.PrintStimulReturnCanIContinue"; }
//         public static get RaisePrintStimulReturnCanIContinueMessage() { return "RaisePrintStimulReturnCanIContinueMessage"; }  //Yuval Chalup 26.07.2015 TASK-14849
//         public static get ResponseInstructionCancel() { return "Response.InstructionCancel"; } //Yuval Chalup 11.10.2015 AMI-54798
//         public static get OpenNewBrowser() { return "OpenNewBrowser"; }
//         public static UnifreightEntity(Direction:string="I") {
//             let unifreightEntity: string = Direction == "E" ? "BFIFILE" : "CFIFILEM";
//             return unifreightEntity;//"CFIFILEM";
//         }

//         public static get RaiseLookUpMessage() { return "RaiseLookUpMessage"; }

//         public static RaiseLookUp(
//             UnifreightEntityNumber: string,
//             LogitudeEntityNumber: string, ViewModelName: string,
//             LOV_FORM: string,
//             PRIMARY_KEY:string) {

//             var unifreightMessageM = AmitalGatewayUtil.Instance.DeclarationMessaging.GetMessage(UnifreightEntityNumber, LogitudeEntityNumber, ViewModelName,"CFIFILEM");
//             unifreightMessageM.Requset.push(["LOV_FORM", LOV_FORM]);
//             unifreightMessageM.Requset.push(["PRIMARY_KEY", PRIMARY_KEY]);

//             unifreightMessageM.Requset.push(["ExpectedCallBack", "Response.LOV_RETURN_VALUE"]);

//             AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
//                 "ScriptableGatewayUtil.RaiseLookUpMessage",
//                 "CFIHMAIN.LogitudeTask",
//                 AmitalGatewayUtil.Instance.DeclarationMessaging.RaiseLookUpMessage,
//                 unifreightMessageM,
//                 "Task 161681: מסר שחרור רכבים מהנמל עבור מטרו");
//         }

//         public static RaiseInstructionReturnCanIContinue(
//             UnifreightEntityNumber: string,
//             LogitudeEntityNumber: string, ViewModelName: string,
//             ViewPlace: string, Direction: string) {

//             var unifreightMessageM = AmitalGatewayUtil.Instance.DeclarationMessaging.GetMessage(UnifreightEntityNumber, LogitudeEntityNumber, ViewModelName, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightEntity(Direction));
//             unifreightMessageM.Requset.push(["ViewPlace", ViewPlace]);
//             unifreightMessageM.Requset.push(["ExpectedCallBack", "Response.InstructionReturnCanIContinue"]);

//             AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
//                 "ScriptableGatewayUtil.RaiseInstructionReturnCanIContinue",
//                 "CFIHMAIN.LogitudeTask",
//                 AmitalGatewayUtil.Instance.DeclarationMessaging.RaiseInstructionReturnCanIContinueMessage,
//                 unifreightMessageM,
//                 "AMI-49615 - הפעלת Instructions מתוך הצהרת יבוא");
//         }

//         public static RaiseCheckInsuranseReturnIsNeededAmount(
//             UnifreightEntityNumber: string,
//             LogitudeEntityNumber: string, ViewModelName: string, action: string) {

//             var unifreightMessageM = AmitalGatewayUtil.Instance.DeclarationMessaging.GetMessage(UnifreightEntityNumber, LogitudeEntityNumber, ViewModelName, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightEntity());

//             unifreightMessageM.Requset.push(["ExpectedCallBack", "Response.Action,Response.InsuranseIsNeeded,Response.InsuranseIsSucceeded,Response.InsuranceHasOpen,Response.InsuranceMessage,Response.InsuranseAmount,Response.InsuranseCurrency,Response.ExpensesAmount,Response.ExpensesAmountCurr,Response.FreightAmount,Response.FreightAmountCurr,Response.FreightAmount2,Response.FreightAmountCurr2,Response.TotalFreightInFreightCurr"]);
//             if (!AppTool.IsNullOrEmpty(action)) {
//                 unifreightMessageM.Requset.push(["Action", action]);
//             }
//             AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
//                 "ScriptableGatewayUtil.RaiseCheckInsuranseReturnIsNeededAmount",
//                 "CFIHMAIN.LogitudeTask",
//                 "RaiseCheckInsuranseReturnIsNeededAmount",
//                 unifreightMessageM,
//                 "AMI-49619 - ביטוח שער עולמי - פיתוח ממשק לבדיקה האם נדרש לתיק ביטוח");
//         }

//         public static RaiseOpenNewBrowser(url: string) {
//             let letsTry = true;
//             if (letsTry) {
//                 var win = window.open(url);
//             } else {
//                 AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
//                     "It's time to open new Browser", "GGGQWBLOGITUDE",
//                     AmitalGatewayUtil.Instance.DeclarationMessaging.OpenNewBrowser,
//                     AmitalGatewayUtil.Instance.GetDefaultUnifreightMessageM(),
//                     url); //Due System shutdown (pop up blocker ) we will open new Browser from Uniface process
//             }
//         }

//         public static RaiseCFIFILMLockReturnCFIFILMAlreadyLock
//             (UnifreightEntityNumber: string,
//                 LogitudeEntityNumber: string, ViewModelName: string, UnifreightEntity: string) {

//             var unifreightMessageM = AmitalGatewayUtil.Instance.DeclarationMessaging.GetMessage(UnifreightEntityNumber, LogitudeEntityNumber, ViewModelName
//                 , UnifreightEntity);
//             unifreightMessageM.LogitudeCommandId
//             unifreightMessageM.Requset.push(["ExpectedCallBack", AmitalGatewayUtil.Instance.DeclarationMessaging.ResponseCFIFILMAlreadyLockKey]);

//             AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
//                 "ScriptableGatewayUtil.RaiseCFIFILMLockReturnCFIFILMAlreadyLock",
//                 "CFIHMAIN.LogitudeTask",
//                 AmitalGatewayUtil.Instance.DeclarationMessaging.RaiseCFIFILMLockReturnCFIFILMAlreadyLockMessage,
//                 unifreightMessageM,
//                 "DeclarationEditControlViewModelController.OnFirstTimeSingleDataLoaded.RaiseCheckCFIFILMLockReturnIsCFIFILMLock");
//         }
//         public static RaiseBFIFILELockReturnBFIFILEAlreadyLock
//         (UnifreightEntityNumber: string,
//             LogitudeEntityNumber: string, ViewModelName: string, UnifreightEntity: string) {

//         var unifreightMessageM = AmitalGatewayUtil.Instance.DeclarationMessaging.GetMessage(UnifreightEntityNumber, LogitudeEntityNumber, ViewModelName
//             , UnifreightEntity);
//         unifreightMessageM.LogitudeCommandId
//         unifreightMessageM.Requset.push(["ExpectedCallBack", AmitalGatewayUtil.Instance.DeclarationMessaging.ResponseBFIFILEAlreadyLockKey]);

//         AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
//             "ScriptableGatewayUtil.RaiseBFIFILELockReturnBFIFILEAlreadyLock",
//             "BFIHMAIN.LogitudeTask",
//             AmitalGatewayUtil.Instance.DeclarationMessaging.RaiseBFIFILELockReturnBFIFILEAlreadyLockMessage,
//             unifreightMessageM,
//             "DeclarationEditControlViewModelController.OnFirstTimeSingleDataLoaded.RaiseCheckBFIFILELockReturnIsBFIFILELock");
//     }

//         public static RaiseUnlockCFIFILEM(UnifreightEntityNumber, LogitudeEntityNumber,
//             HaveSaved: boolean) {

//             var unifreightMessageM = AmitalGatewayUtil.Instance.DeclarationMessaging.GetMessage(UnifreightEntityNumber, LogitudeEntityNumber, "", AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightEntity());
//             unifreightMessageM.Requset.push(["HaveSaved", HaveSaved.toString()]);

//             AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
//                 "ScriptableGatewayUtil.RaiseUnlockCFIFILEM",
//                 "CFIHMAIN.LogitudeTask",
//                 AmitalGatewayUtil.Instance.DeclarationMessaging.RaiseUnlockCFIFILEMMessage,
//                 unifreightMessageM,
//                 "DeclarationEditControlViewModelController.OnFirstTimeSingleDataLoaded.RaiseCheckCFIFILMLockReturnIsCFIFILMLock");
//         }
//         public static RaiseUnlockBFIFILE(UnifreightEntityNumber, LogitudeEntityNumber,
//             HaveSaved: boolean) {

//             var unifreightMessageM = AmitalGatewayUtil.Instance.DeclarationMessaging.GetMessage(UnifreightEntityNumber, LogitudeEntityNumber, "", AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightEntity());
//             unifreightMessageM.Requset.push(["HaveSaved", HaveSaved.toString()]);

//             AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
//                 "ScriptableGatewayUtil.RaiseUnlockBFIFILE",
//                 "BFIHMAIN.LogitudeTask",
//                 AmitalGatewayUtil.Instance.DeclarationMessaging.RaiseUnlockBFIFILEMessage,
//                 unifreightMessageM,
//                 "DeclarationEditControlViewModelController.OnFirstTimeSingleDataLoaded.RaiseCheckBFIFILELockReturnIsBFIFILELock");
//         }
//         //<--- Yuval Chalup 26.07.2015 TASK-14849
//         public static RaisePrintStimulReturnCanIContinue(
//             UnifreightEntityNumber: string,
//             LogitudeEntityNumber: string, ViewModelName: string,
//             ViewPlace: string) {
//             var unifreightMessageM = AmitalGatewayUtil.Instance.DeclarationMessaging.GetMessage(UnifreightEntityNumber, LogitudeEntityNumber, ViewModelName, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightEntity());
//             unifreightMessageM.Requset.push(["ExpectedCallBack", "Response.PrintStimulReturnCanIContinue"]);
//             if (ViewPlace == "RELEASE") // moran 29.2.16 - Task 19807 -->
//             {
//                 unifreightMessageM.Requset.push(["FormToPrint", "RELEASE"]);

//                 AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
//                     "ScriptableGatewayUtil.RaisePrintStimulReturnCanIContinue",
//                     "CFIHMAIN.LogitudeTask",
//                     AmitalGatewayUtil.Instance.DeclarationMessaging.RaisePrintStimulReturnCanIContinueMessage,
//                     unifreightMessageM,
//                     " - הדפסת שחרור חלקי מהצהרה  - הפעלת טופס A/200 ביוניפירייט ");
//             }
//             else // moran 29.2.16 - Task 19807 <--
//             {
//                 unifreightMessageM.Requset.push(["FormToPrint", ViewPlace]);
//                 AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
//                     "ScriptableGatewayUtil.RaisePrintStimulReturnCanIContinue",
//                     "CFIHMAIN.LogitudeTask",
//                     AmitalGatewayUtil.Instance.DeclarationMessaging.RaisePrintStimulReturnCanIContinueMessage,
//                     unifreightMessageM,
//                     "ASK-14849 - הדפסת צרופה מהצהרה  - הפעלת טופס A/33 ביוניפירייט ");
//             }
//         }
//         //Yuval Chalup 26.07.2015 TASK-14849 --->

//         static GetMessage(UnifreightEntityNumber: string,
//             LogitudeEntityNumber: string, LogitudeViewModel: string,
//             UnifreightEntity: string

//         ): UnifreightMessageM {
//             var unifreightMessageM = new UnifreightMessageM();
//             unifreightMessageM.UnifreightEntity = UnifreightEntity;//

//             unifreightMessageM.UnifreightEntityNumber = UnifreightEntityNumber;
//             unifreightMessageM.LogitudeEntity = AmitalGatewayUtil.Instance.DeclarationMessaging.LogitudeEntityDeclaration;
//             unifreightMessageM.LogitudeEntityNumber = LogitudeEntityNumber;
//             unifreightMessageM.LogitudeViewModel = LogitudeViewModel;
//             unifreightMessageM.Requset = [];
//             return unifreightMessageM;
//         }

//         public ShowDeclarationCertificatesByGroupsUnifreightCallBack(
//             UnifreightEntityNumber, LogitudeEntityNumber, ViewModelName, CustomerId) {
//             var unifreightMessageM = AmitalGatewayUtil.Instance.DeclarationMessaging.GetMessage(UnifreightEntityNumber, LogitudeEntityNumber, ViewModelName, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightEntity());
//             unifreightMessageM.Requset.push(["CustomerId", CustomerId]);

//             AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
//                 "ScriptableGatewayUtil.ShowDeclarationCertificatesByGroupsUnifreightCallBack",
//                 "CFIHMAIN.LogitudeTask",
//                 "ShowDeclarationCertificatesByGroupsUnifreightCallBack",
//                 unifreightMessageM,
//                 " אישורים נדרשים");
//         }

//         public ShowDocumentsSharingUnifreightCallBack(
//             UnifreightEntityNumber, LogitudeEntityNumber, ViewModelName, CustomerId) {
//             var unifreightMessageM = AmitalGatewayUtil.Instance.DeclarationMessaging.GetMessage(UnifreightEntityNumber, LogitudeEntityNumber, ViewModelName, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightEntity());
//             unifreightMessageM.Requset.push(["CustomerId", CustomerId]);

//             AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
//                 "ScriptableGatewayUtil.ShowDocumentsSharingUnifreightCallBack",
//                 "CFIHMAIN.LogitudeTask",
//                 "ShowDocumentsSharingUnifreightCallBack",
//                 unifreightMessageM,
//                 " שיתוף מסמכים");
//         }

//     }
// }
// export class RequestWrapperM {
//     ////////itzik : why Ayman1 add this line ?!?!?  private CurrentSession = SessionLocator.SelectedSession;
//     //String SENDERID : In = "SessionViewModel"
//     public SenderID: string;
//     //String RECEIVERID : In = "GGGQWBLOGITUDE"
//     public ReceiverID: string;
//     //String MESSAGEID : In = "AmitalBackButtonCommandAction"
//     public MessageID: string;
//     //Boolean MESSAGECOMPRESS : In = 0
//     public MESSAGECOMPRESS: boolean;

//     public DeclarePurpose: string;
//     //String MESSAGEXML : In = "<!--xml generated  FROM C:\LogitudeWorld\Amital\Simplog.Infrastructure\Utilities\UnifreightListsUtil.cs-->
//     public UnifreightMessage: UnifreightMessageM;
//     //    < ArrayOfEntry >
//     //    <Entry>
//     //    <Key><![CDATA[UnifreightEntity]] > </Key>
//     //    < Value > <![CDATA[]] > </Value>
//     //    < /Entry>
//     //    < Entry >
//     //    <Key><![CDATA[UnifreightEntityNumber]] > </Key>
//     //    < Value > <![CDATA[]] > </Value>
//     //    < /Entry>
//     //    < Entry >
//     //    <Key><![CDATA[LogitudeEntity]] > </Key>
//     //    < Value > <![CDATA[]] > </Value>
//     //    < /Entry>
//     //    < Entry >
//     //    <Key><![CDATA[LogitudeEntityNumber]] > </Key>
//     //    < Value > <![CDATA[]] > </Value>
//     //    < /Entry>
//     //    < Entry >
//     //    <Key><![CDATA[LogitudeViewModel]] > </Key>
//     //    < Value > <![CDATA[]] > </Value>
//     //    < /Entry>
//     //    < Entry >
//     //    <Key><![CDATA[LogitudeCommandId]] > </Key>
//     //    < Value > <![CDATA[]] > </Value>
//     //    < /Entry>
//     //    < /ArrayOfEntry>"
//     public MoreParams : string = ""
// }
// export class UnifreightMessageM {

//     public UnifreightEntity: string;
//     public UnifreightEntityNumber: string;
//     public LogitudeEntity: string;
//     public LogitudeEntityNumber: string;
//     public LogitudeViewModel: string;
//     public LogitudeCommandId: string;
//     public Requset: Array<[string, string]> = [];
//     public Response: Array<[string, string]> = [];

//     public static GetStringValue(myUnifreightMessageM: UnifreightMessageM ,theKey: string): string {
//         let StringValue = "";
//         let listRes = myUnifreightMessageM.Requset
//             .filter(itm => itm[0] == theKey);
//         if (listRes.length > -1 && !AppTool.IsNullOrEmpty(listRes[0])) {
//             if (!AppTool.IsNullOrEmpty(listRes[0][1])) {
//                 StringValue = listRes[0][1];
//             }
//         } else {

//             let listRes = myUnifreightMessageM.Response
//                 .filter(itm => itm[0] == theKey);
//             if (listRes.length > -1 && !AppTool.IsNullOrEmpty(listRes[0])) {
//                 if (!AppTool.IsNullOrEmpty(listRes[0][1])) {
//                     StringValue = listRes[0][1];
//                 }
//             }
//         }
//         return StringValue;
//     }
// }

// export class ClientAction {
//     private CurrentSession = SessionLocator.SelectedSession;
//     public Run(unifreightMessage: UnifreightMessageM) {
//         //"UnifreightEntity=GNDCARD·;UnifreightEntityNumber=10009065·;LogitudeEntity=Customs.Client·;LogitudeEntityNumber=049028392·;LogitudeViewModel=UnifreightMassageHandler·;LogitudeCommandId=ShowClientReturnIfExist·;formtitle=Client"
//         let UnifreightEntityNumber = unifreightMessage.UnifreightEntityNumber;
//         let ImporterVat = unifreightMessage.LogitudeEntityNumber;

//         var servicelink = '../../Customs/Services/WebServices/ClientMessagesService';
//         servicelink = './Customs/Services/WebServices/ClientMessagesService';
//         SessionLocator.DynamicLoader.GetInstance(servicelink).then((service: any) => {
//             service.GetSingleClientPMByCode(ImporterVat, false)
//                 .subscribe((rsp) => {
//                     var myClientPM: ClientPM = rsp.Result;
//                     SessionLocator.SelectedSession.StopBusyIndicator();
//                     if (AppTool.IsNullOrEmpty(myClientPM)) {
//                         AmitalGatewayUtil.Instance.ShowClientReturnIfExistUnifreightCallBack(false);

//                     } else {
//                         this.ShowClientEditControl(myClientPM);
//                     }
//                 });
//         });
//         //this will cause the customs to build ....mohammad.
//         //var clientExtendedPMService = new ClientMessagesService();
//         //clientExtendedPMService.GetSingleClientPMByCode(ImporterVat, false)
//         //    .subscribe((rsp) => {
//         //        var myClientPM: ClientPM = rsp.Result;
//         //        SessionLocator.SelectedSession.StopBusyIndicator();
//         //        if (AppTool.IsNullOrEmpty(myClientPM)) {
//         //            AmitalGatewayUtil.Instance.ShowClientReturnIfExistUnifreightCallBack(false);

//         //        } else {
//         //            this.ShowClientEditControl(myClientPM);
//         //        }
//         //    });
//     }
//     ShowClientEditControl(myClientPM: ClientPM) {
//         //windowArgs.CurrentEntity = myResponse.Result;
//         var logWindow = new LogitudeWindow();
//         logWindow.Width = 960;
//         logWindow.Height = 570;
//         logWindow.Title = TextCodeTranslator.Translate("Customs.Client.O.EditClient");// "Edit Client";
//         logWindow.WindowArgs = { "CurrentEntity": myClientPM, "isNewClient": false };
//         logWindow.ShowCloseButton = true;
//         logWindow.Show('./CustomsModules/CustomsClient/Components/EditTabs/ClientEditComponent');

//         logWindow.WindowClosed.subscribe(($event1: any) => {
//             AmitalGatewayUtil.Instance.ShowClientReturnIfExistUnifreightCallBack(true);
//         });
//     }

// }
// export class ShowGeneralLOVReturnSelected {
//     private CurrentSession = SessionLocator.SelectedSession;
//     public Run(unifreightMessage: UnifreightMessageM) {
//         //"UnifreightEntity=GNDCARD·;UnifreightEntityNumber=10009065·;LogitudeEntity=Customs.Client·;LogitudeEntityNumber=049028392·;LogitudeViewModel=UnifreightMassageHandler·;LogitudeCommandId=ShowClientReturnIfExist·;formtitle=Client"
//         let UnifreightEntityNumber = unifreightMessage.UnifreightEntityNumber;
//         //let ImporterVat = unifreightMessage.LogitudeEntityNumber;
//         //let formtitle: string=            = UnifreightMessageM.GetStringValue(unifreightMessage, "Requset.formtitle");
//         SessionLocator.SelectedSession.StartBusyIndicatorLoading();
//         let LOVText: string
//             = UnifreightMessageM.GetStringValue(unifreightMessage, "Requset.LOVText");
//         var logWindow = new LogitudeWindow();
//         logWindow.Width = 450;
//         logWindow.Height = 250;
//         logWindow.Title = 'הזן ' + LOVText ;
//         logWindow.WindowArgs = {
//             "LogitudeEntityNumber": unifreightMessage.LogitudeEntityNumber,
//             "LogitudeEntity": unifreightMessage.LogitudeEntity,
//             "LOVText": LOVText,

//         };
//         logWindow.ShowCloseButton = true;
//         logWindow.Show(
//             //'./CustomsModules/CustomsClient/Components/EditTabs/ClientEditComponent'
//             //'./Customs/Components/Maintenance/DocumentTypeCustomsDataComponent'
//             './CustomsModules/CustomsMaintenance/Components/GeneralLOVComponent'
//         );
//         AmitalGatewayUtil.Instance.IsAmitalBackButtonDisable = true;
//         logWindow.WindowClosed.subscribe((event1: any) => {
//             ///AmitalGatewayUtil.Instance.AmitalBackButtonClicked();
//             if (event1 == "Cancel") {

//             }
//             SessionLocator.SelectedSession.StopBusyIndicator();
//             AmitalGatewayUtil.Instance.IsAmitalBackButtonDisable = false;
//             AmitalGatewayUtil.Instance.ShowGeneralLOVReturnSelectedCallBack(event1);

//             //SessionLocator.SelectedSession.StopBusyIndicator();
//         });

//     }
// }

// export class MapDocumentTypeCustomsData {
//     private CurrentSession = SessionLocator.SelectedSession;
//     public Run(unifreightMessage: UnifreightMessageM) {
//         //"UnifreightEntity=GNDCARD·;UnifreightEntityNumber=10009065·;LogitudeEntity=Customs.Client·;LogitudeEntityNumber=049028392·;LogitudeViewModel=UnifreightMassageHandler·;LogitudeCommandId=ShowClientReturnIfExist·;formtitle=Client"
//         let UnifreightEntityNumber = unifreightMessage.UnifreightEntityNumber;
//         //let ImporterVat = unifreightMessage.LogitudeEntityNumber;
//         let UnifaceNAME_HEB: string
//             = UnifreightMessageM.GetStringValue(unifreightMessage, "Requset.UnifaceNAME_HEB");
//         var logWindow = new LogitudeWindow();
//         logWindow.Width = 450;
//         logWindow.Height = 250;
//         logWindow.Title = 'קשר סוג מסמך לשער עולמי';//TextCodeTranslator.Translate("Customs.Client.O.EditClient");// "Edit Client";
//         logWindow.WindowArgs = {
//             "UnifaceDOC_ID": UnifreightEntityNumber,
//             "UnifaceNAME_HEB": UnifaceNAME_HEB

//         };
//         logWindow.ShowCloseButton = true;
//         logWindow.Show(
//             //'./CustomsModules/CustomsClient/Components/EditTabs/ClientEditComponent'
//             //'./Customs/Components/Maintenance/DocumentTypeCustomsDataComponent'
//             './CustomsModules/CustomsMaintenance/Components/DocumentTypeCustomsDataComponent'
//         );

//         logWindow.WindowClosed.subscribe(($event1: any) => {
//             AmitalGatewayUtil.Instance.AmitalBackButtonClicked();
//             SessionLocator.SelectedSession.StopBusyIndicator();
//         });

//     }
// }

// export class MapPendingReasonCodeData {
//     public Run(unifreightMessage: UnifreightMessageM) {
//         let UnifreightEntityNumber = unifreightMessage.UnifreightEntityNumber;

//         var logWindow = new LogitudeWindow();
//         logWindow.Width = 500;
//         logWindow.Height = 400;
//         logWindow.Title = 'קשר סטטוס לסיבת Pending';
//         logWindow.WindowArgs = {
//             "UnifreightStatusCode": UnifreightEntityNumber,
//             "FromUnifreight": true,
//         };
//         logWindow.ShowCloseButton = true;
//         logWindow.Show('./CustomsModules/CustomsCourier/Components/CourierPendingReason/AddCourierPendingToUnifreightStatusComponent');
//         logWindow.WindowClosed.subscribe(($event1: any) => {
//             AmitalGatewayUtil.Instance.AmitalBackButtonClicked();
//         });

//     }

// }

// export class MapExceptionReasonCodeData {
//     public Run(unifreightMessage: UnifreightMessageM) {
//         let UnifreightEntityNumber = unifreightMessage.UnifreightEntityNumber;

//         var logWindow = new LogitudeWindow();
//         logWindow.Width = 500;
//         logWindow.Height = 400;
//         logWindow.Title = 'קשר סטטוס לסיבת חריג';
//         logWindow.WindowArgs = {
//             "UnifreightStatusCode": UnifreightEntityNumber,
//             "FromUnifreight": true,
//         };
//         logWindow.ShowCloseButton = true;
//         logWindow.Show('./CustomsModules/CustomsReferant/Components/ReferantExceptionReason/AddExceptionReasonToUnifreightStatusComponent');
//         logWindow.WindowClosed.subscribe(($event1: any) => {
//             AmitalGatewayUtil.Instance.AmitalBackButtonClicked();
//         });

//     }

// }

// export class ShowInvoiceFromUrouterReturnCreateInvoiceCommand {
//     private CurrentSession = SessionLocator.SelectedSession;
//     public Run(unifreightMessage: UnifreightMessageM) {
//         //"UnifreightEntity=GNDCARD·;UnifreightEntityNumber=10009065·;LogitudeEntity=Customs.Client·;LogitudeEntityNumber=049028392·;LogitudeViewModel=UnifreightMassageHandler·;LogitudeCommandId=ShowClientReturnIfExist·;formtitle=Client"
//         let UnifreightEntityNumber = unifreightMessage.UnifreightEntityNumber;
//         //let ImporterVat = unifreightMessage.LogitudeEntityNumber;
//         let UnifaceNAME_HEB: string
//             = UnifreightMessageM.GetStringValue(unifreightMessage, "Requset.UnifaceNAME_HEB");
//         var logWindow = new LogitudeWindow();
//         logWindow.Width = 1600;
//         logWindow.Height = 800;
//         logWindow.Title = 'תור חשבונית';//TextCodeTranslator.Translate("Customs.Client.O.EditClient");// "Edit Client";
//         logWindow.WindowArgs = {
//             "unifreightMessage": unifreightMessage,

//         };
//         AmitalGatewayUtil.Instance.IsAmitalBackButtonDisable = true;
//         logWindow.ComponentLoaded.subscribe(comp => {
//             logWindow.WindowClosed.subscribe((toCreateQInvoice: string) => {
//                 SessionLocator.SelectedSession.StopBusyIndicator();
//                 AmitalGatewayUtil.Instance.IsAmitalBackButtonDisable = false;
//                 let CreateInvoice;
//                 let ExcludeLines = "";
//                 if (toCreateQInvoice.includes("1")) {
//                     CreateInvoice = 1;
//                     ExcludeLines = toCreateQInvoice.replace("1;", "");
//                 } else {
//                     CreateInvoice = 0;
//                 }
//                 AmitalGatewayUtil.Instance.CreateQInvoiceUnifreightCallBack(CreateInvoice, comp.Remarks, ExcludeLines);
//             });
//         });
//         logWindow.Show('./CustomsModules/InvoiceQueue/Components/InvoiceQueueComponent');
//         /*
//         logWindow.WindowClosed.subscribe((toCreateQInvoice: any) => {
//             //AmitalGatewayUtil.Instance.AmitalBackButtonClicked();
//             SessionLocator.SelectedSession.StopBusyIndicator();
//             AmitalGatewayUtil.Instance.IsAmitalBackButtonDisable = false;

//             AmitalGatewayUtil.Instance.CreateQInvoiceUnifreightCallBack(toCreateQInvoice);

//         });
//         */
//     }
// }
