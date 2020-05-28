"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SheetStatusEnum;
(function (SheetStatusEnum) {
    SheetStatusEnum[SheetStatusEnum["notSetNotInUse"] = 0] = "notSetNotInUse";
    /*
SELECT TOP 1000 [Code]
  ,[EnglishName]
  ,[LocalName]
  ,[SearchFields]
FROM [Amital1_Main].[Customs].[CustomsRequestsSheetStatuses]
order by cast(Code as decimal)
     *
     Code	EnglishName
Code	EnglishName
1	Created
2	In Process
15	Send Failed
20	Sent
25	Analyze Failed
30	Analyzed
99	Cancelled
     *
     */
    SheetStatusEnum[SheetStatusEnum["Created"] = 1] = "Created";
    //CustomRequest=3,
    //CustomRequestSigned = 4,
    SheetStatusEnum[SheetStatusEnum["InProcess"] = 2] = "InProcess";
    SheetStatusEnum[SheetStatusEnum["WaitingForSigning"] = 5] = "WaitingForSigning";
    SheetStatusEnum[SheetStatusEnum["SendFailed"] = 15] = "SendFailed";
    SheetStatusEnum[SheetStatusEnum["Sent"] = 20] = "Sent";
    SheetStatusEnum[SheetStatusEnum["Received"] = 21] = "Received";
    SheetStatusEnum[SheetStatusEnum["ReceivedFailed"] = 22] = "ReceivedFailed";
    SheetStatusEnum[SheetStatusEnum["SentResponseOnDCA"] = 23] = "SentResponseOnDCA";
    SheetStatusEnum[SheetStatusEnum["AnalyzeFailed"] = 25] = "AnalyzeFailed";
    SheetStatusEnum[SheetStatusEnum["Analyzed"] = 30] = "Analyzed";
    SheetStatusEnum[SheetStatusEnum["Cancelled"] = 99] = "Cancelled";
})(SheetStatusEnum = exports.SheetStatusEnum || (exports.SheetStatusEnum = {}));
var SheetStatusEndEnum;
(function (SheetStatusEndEnum) {
    SheetStatusEndEnum[SheetStatusEndEnum["notSetNotInUse"] = 0] = "notSetNotInUse";
    SheetStatusEndEnum[SheetStatusEndEnum["SendFailed"] = 15] = "SendFailed";
    SheetStatusEndEnum[SheetStatusEndEnum["ReceivedFailed"] = 22] = "ReceivedFailed";
    SheetStatusEndEnum[SheetStatusEndEnum["AnalyzeFailed"] = 25] = "AnalyzeFailed";
    SheetStatusEndEnum[SheetStatusEndEnum["Analyzed"] = 30] = "Analyzed";
    SheetStatusEndEnum[SheetStatusEndEnum["Cancelled"] = 99] = "Cancelled";
})(SheetStatusEndEnum = exports.SheetStatusEndEnum //: int
 || (exports.SheetStatusEndEnum //: int
 = {}));
var CustomsStepEnum;
(function (CustomsStepEnum) {
    CustomsStepEnum[CustomsStepEnum["StartRequestParams"] = 0] = "StartRequestParams";
    CustomsStepEnum[CustomsStepEnum["CustomRequest"] = 1] = "CustomRequest";
    CustomsStepEnum[CustomsStepEnum["CustomRequestSign"] = 2] = "CustomRequestSign";
    CustomsStepEnum[CustomsStepEnum["CustomRequestSignPersonal"] = 2] = "CustomRequestSignPersonal";
    CustomsStepEnum[CustomsStepEnum["DCAInProgressUploading"] = 11] = "DCAInProgressUploading";
    CustomsStepEnum[CustomsStepEnum["DCAInProgressUploaded"] = 12] = "DCAInProgressUploaded";
    CustomsStepEnum[CustomsStepEnum["ReceivedCustomResponseCorrelation"] = 20] = "ReceivedCustomResponseCorrelation";
    CustomsStepEnum[CustomsStepEnum["AnalyzeResponseData"] = 30] = "AnalyzeResponseData"; // DCASent Or  AnalyzeResponse
})(CustomsStepEnum = exports.CustomsStepEnum || (exports.CustomsStepEnum = {}));
var SheetStatusCanCancleEnum;
(function (SheetStatusCanCancleEnum) {
    SheetStatusCanCancleEnum[SheetStatusCanCancleEnum["notSetNotInUse"] = 0] = "notSetNotInUse";
    SheetStatusCanCancleEnum[SheetStatusCanCancleEnum["Created"] = 1] = "Created";
    SheetStatusCanCancleEnum[SheetStatusCanCancleEnum["SendFailed"] = 15] = "SendFailed";
    SheetStatusCanCancleEnum[SheetStatusCanCancleEnum["InProcess"] = 2] = "InProcess";
    SheetStatusCanCancleEnum[SheetStatusCanCancleEnum["WaitingForSigning"] = 5] = "WaitingForSigning";
    SheetStatusCanCancleEnum[SheetStatusCanCancleEnum["SentResponseOnDCA"] = 23] = "SentResponseOnDCA";
})(SheetStatusCanCancleEnum = exports.SheetStatusCanCancleEnum //: int
 || (exports.SheetStatusCanCancleEnum //: int
 = {}));
var SheetStatusInProcessEnum;
(function (SheetStatusInProcessEnum) {
    SheetStatusInProcessEnum[SheetStatusInProcessEnum["notSetNotInUse"] = 0] = "notSetNotInUse";
    SheetStatusInProcessEnum[SheetStatusInProcessEnum["Created"] = 1] = "Created";
    SheetStatusInProcessEnum[SheetStatusInProcessEnum["InProcess"] = 2] = "InProcess";
    SheetStatusInProcessEnum[SheetStatusInProcessEnum["WaitingForSigning"] = 5] = "WaitingForSigning";
    SheetStatusInProcessEnum[SheetStatusInProcessEnum["Sent"] = 20] = "Sent";
    SheetStatusInProcessEnum[SheetStatusInProcessEnum["Received"] = 21] = "Received";
    SheetStatusInProcessEnum[SheetStatusInProcessEnum["SentResponseOnDCA"] = 23] = "SentResponseOnDCA";
})(SheetStatusInProcessEnum = exports.SheetStatusInProcessEnum //: int
 || (exports.SheetStatusInProcessEnum //: int
 = {}));
var ResponseDataBase = /** @class */ (function () {
    function ResponseDataBase() {
    }
    //public bool ContinuePasiveSignInBackground { get; set; }
    ///public SheetStatusEnum RequestSheetStatus { get; set; }
    ResponseDataBase.GetCustomsRequestStepText = function (customsRequestStep) {
        var mess = "";
        switch (customsRequestStep) {
            case CustomsStepEnum.CustomRequest:
                mess = "מכין בקשה";
                break;
            case CustomsStepEnum.CustomRequestSign:
                mess = "חותם על הבקשה";
                mess = "ממתין לחתימה יזומה";
                break;
            case CustomsStepEnum.DCAInProgressUploading:
                break;
            case CustomsStepEnum.DCAInProgressUploaded:
                break;
            case CustomsStepEnum.ReceivedCustomResponseCorrelation:
                mess =
                    "ממתין לתשובת המכס";
                //"שולח בקשה";
                break;
            //case CustomsStepEnum.SentResponseOnDCA:
            //    mess = "נשלח המשוב יתקבל בכספת";
            //    break;
            case CustomsStepEnum.AnalyzeResponseData:
                mess = "מנתח תשובה";
                break;
            case CustomsStepEnum.StartRequestParams:
            default:
                mess = "בונה בקשה";
                break;
        }
        return mess;
    };
    ResponseDataBase.RequestSheetCanCancelled = function (requestStatusCode, isDCA) {
        //throw new Error('RequestSheetCanCancelled TODO !!!')
        //var listStatusInProcess = Enum.GetValues(typeof (SheetStatusInProcessEnum))
        //.AsQueryable().OfType<SheetStatusInProcessEnum>()
        //.Select(rec => ((int)rec).ToString())
        //.ToList();
        //var listSheetStatusCanCancleEnum = Enum.GetValues(typeof (SheetStatusCanCancleEnum))
        //    .AsQueryable().OfType<SheetStatusCanCancleEnum>()
        //    .Select(rec => ((int)rec).ToString())
        //            .ToList();
        //if (listSheetStatusCanCancleEnum.Contains(requestStatusCode)) {
        //    return true;
        //}
        for (var e in SheetStatusCanCancleEnum) {
            if (e.toString() == requestStatusCode) {
                return true;
            }
        }
        if (!isDCA) {
            return false;
        }
        //if (isDCA && listStatusInProcess.Contains(requestStatusCode)) {
        //    return true;
        //}
        for (var e in SheetStatusInProcessEnum) {
            if (e.toString() == requestStatusCode) {
                return true;
            }
        }
        return false;
    };
    return ResponseDataBase;
}());
exports.ResponseDataBase = ResponseDataBase;
//# sourceMappingURL=ResponseDataBase.js.map