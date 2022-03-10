import { AmitalGatewayUtil, UnifreightMessageM } from '../../Infrastructure/Utilities/AmitalGatewayUtil';
import { AppTool } from '../../Infrastructure/Tools';
import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';
import { IEditComponentController } from '../../Infrastructure/Components/EditComponent/EditComponent';
import { DeclarationPM } from '../../Customs/EntityPMs/DeclarationPM';

export class UnifreightController {
    constructor(
        private _DeclarationPM: DeclarationPM,
        private _ViewModelName: string)
    { }

    public UnifreightCallbackCompleted

    public SendRequestPrintStimulToUnifreightAsync(PrintParamsXml: string) {
        var declarationId = this._DeclarationPM.Id;
        var declarationNumber = this._DeclarationPM.DeclarationNumber;
        var customFileNo = this._DeclarationPM.CustomFileNo;
        
        AmitalGatewayUtil.Instance.DeclarationMessaging
            .RaisePrintStimulReturnCanIContinue(customFileNo, declarationId,
            this._ViewModelName, PrintParamsXml);
        
    }
    public SendRequestInstructionToUnifreightAsync(ViewPlace: string) {
        var declarationId = this._DeclarationPM.Id;
        var declarationNumber = this._DeclarationPM.DeclarationNumber;
        var customFileNo = this._DeclarationPM.CustomFileNo;

        AmitalGatewayUtil.Instance.DeclarationMessaging.RaiseInstructionReturnCanIContinue
            (customFileNo, declarationId, this._ViewModelName, ViewPlace, this._DeclarationPM.Direction);

    }

    public GetPromise(): Promise<UnifreightResponseEventArgs> {
        return new Promise((resolve, reject) => {

            let sub = AmitalGatewayUtil.Instance.UnifaceRequestArrived
                .subscribe(
                (mess: UnifreightMessageM) => {
                    var IsMatchUnifreightCallbackCommand = (
                        mess.LogitudeEntity == AmitalGatewayUtil.Instance.DeclarationMessaging.LogitudeEntityDeclaration &&
                        mess.LogitudeEntityNumber == this._DeclarationPM.Id &&
                        mess.LogitudeViewModel == this._ViewModelName);
                    if (IsMatchUnifreightCallbackCommand) {
                        sub.unsubscribe();
                        let sBool = UnifreightMessageM.GetStringValue(mess, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightResponseStatus);
                        if (!AppTool.IsNullOrEmpty(sBool)) {
                            let bcanContinue = (sBool.toLowerCase() == 'true');
                            //if (bcanContinue) {
                            let unifreightResponseEventArgs = new UnifreightResponseEventArgs(mess, bcanContinue);
                            resolve(unifreightResponseEventArgs);
                            //}
                        } else {
                            reject("Unifreight didn't send param " + AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightResponseStatus);
                        }
                        
                        
                    }
                }
                );
        });
    }


}

export class UnifreightResponseEventArgs {
    constructor(
        public UnifreightMessage: UnifreightMessageM,
        public UnifreightResponseStatus: boolean) { }
}


export class UnifreightInstructionController {
    constructor(private EntityPM: DeclarationPM, private ViewPlace: string) { }

    public ShowInstruction(OnResponceOKMethod: () => void, OnResponceFailedMethod: () => void) {
        if (AmitalGatewayUtil.Instance.IsDeclarationInUse(this.EntityPM.CustomFileNo, this.EntityPM.IsConvertedDeclaration, this.EntityPM.IsConnectedToUnifreight)) {
            SessionLocator.SelectedSession.StartBusyIndicator("Check Instruction...");

            var myUnifreightController = new UnifreightController(this.EntityPM,
                "UnifreightInstructionController");

            myUnifreightController.SendRequestInstructionToUnifreightAsync(this.ViewPlace/*"SENDTOMEHES"*/);
            myUnifreightController.GetPromise().
                then((e) => {
                    SessionLocator.SelectedSession.StopBusyIndicator();
                    var UnifreightResponseStatus = e.UnifreightResponseStatus;
                    var UnifreightMessage = e.UnifreightMessage;
                    if (UnifreightResponseStatus) {
                        OnResponceOKMethod();
                    }
                    else {
                        OnResponceFailedMethod();
                    }
                });
        } else {
            OnResponceOKMethod();
        }
    }
}
