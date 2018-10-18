
export enum SheetStatusEnum 
{
    notSetNotInUse = 0,
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
        Created = 1,
        //CustomRequest=3,
        //CustomRequestSigned = 4,
        InProcess = 2,
        WaitingForSigning = 5,
        SendFailed = 15,
        Sent = 20,
        Received = 21,
        ReceivedFailed = 22,
        SentResponseOnDCA = 23,
        AnalyzeFailed = 25,
        Analyzed = 30,
        Cancelled = 99
}
export enum SheetStatusEndEnum //: int
{
    notSetNotInUse = 0,
    SendFailed = 15,
    ReceivedFailed = 22,
    AnalyzeFailed = 25,
    Analyzed = 30,
    Cancelled = 99
}


export enum CustomsStepEnum 
{
    StartRequestParams = 0,
        CustomRequest = 1,
        CustomRequestSign = 2,//Have Sign Ver (If Needed)
        CustomRequestSignPersonal = 2,//Have Sign Ver (If Needed)

        DCAInProgressUploading = 11,// Sent via DCA Get ServerJobId
        DCAInProgressUploaded = 12,

        ReceivedCustomResponseCorrelation = 20,// SendWS or ReviveFromDownloadDcaMessagesWR
        AnalyzeResponseData = 30// DCASent Or  AnalyzeResponse
}




export enum SheetStatusCanCancleEnum //: int
{
    notSetNotInUse = 0,

    Created = 1,
    SendFailed = 15,///
    InProcess = 2,
    WaitingForSigning = 5,
    SentResponseOnDCA = 23,

}

export enum SheetStatusInProcessEnum //: int
{
    notSetNotInUse = 0,

    Created = 1,

    InProcess = 2,
    WaitingForSigning = 5,

    Sent = 20,
    Received = 21,
    SentResponseOnDCA = 23,
}

export class ResponseDataBase {



    public HasException: boolean;
    public UserMessage: string;
    public Succeeded: boolean;


    //itzik 
    public ContinueProcessInBackground: boolean;
    //public int Tenant { get; set; }
    public CustomsRequestsSheetId: string;

    public CorrelationId: string;
    //public bool ContinuePasiveSignInBackground { get; set; }

    ///public SheetStatusEnum RequestSheetStatus { get; set; }



    public static GetCustomsRequestStepText(customsRequestStep: CustomsStepEnum): string {
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
    }

    public static RequestSheetCanCancelled(requestStatusCode: string, isDCA: boolean): boolean
    {
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
        for (let e in SheetStatusCanCancleEnum) {
            if (e.toString() == requestStatusCode) {
                return true;
            }
        }
        if (!isDCA)
        {
            return false;
        }
        //if (isDCA && listStatusInProcess.Contains(requestStatusCode)) {
        //    return true;

        //}
        for (let e in SheetStatusInProcessEnum) {
            if (e.toString() == requestStatusCode){
                return true;
            }
        }
        return false;
    }


}

