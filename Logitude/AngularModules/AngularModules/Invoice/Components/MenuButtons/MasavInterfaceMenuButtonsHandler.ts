import {MenuButtonPM} from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import { FullAccountingSettingPMService } from '../../../Accounting/Services/StandardPMs/FullAccountingSettingPMService';
import { LedgerTransactionPM } from 'Accounting/EntityPMs/LedgerTransactionPM';
import { MasavInterfaceStatus } from '../EditTabs/MasavInterface/MasavInterfaceDetailsTabComponent';
import { MasavInterfacePM } from 'Invoices/EntityPMs/MasavInterfacePM';
import { DateTool } from 'Infrastructure/Tools';
import { ConfirmWindow } from 'Controls/Windows/ConfirmWindow';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';

export class MasavInterfaceMenuButtonsHandler {
    public EntityPM: MasavInterfacePM;
    public entityArgs: EntityArgs
    ReconcileInternalTrans:LedgerTransactionPM[];
    private CurrentSession = SessionLocator.SelectedSession;
    fullAccountingSettingPMService: FullAccountingSettingPMService = new FullAccountingSettingPMService();

    constructor(){
    }

    public SetEntityPM(entityArgs: EntityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
    }

    

    

    

    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
               
                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    switch (button.EventCode) {

                        case "TRAN": {
                            if (this.EntityPM.StatusCode === MasavInterfaceStatus.Draft || this.EntityPM.StatusCode === MasavInterfaceStatus.Failed) {
                                button.IsDisabled = false;
                            }
                            else {
                                button.IsDisabled = true;
                            }
                            break;
                        }

                        case "MIDL": {
                            if (this.EntityPM.StatusCode === MasavInterfaceStatus.Transmitted) {
                                button.IsDisabled = false;
                            }

                            else {
                                button.IsDisabled = true;
                            }
                            break;
                        }

                        case "MICN": {
                            
                            if (this.EntityPM.StatusCode == MasavInterfaceStatus.Transmitted) {
                                button.IsDisabled = false;
                            }
                            else {
                                button.IsDisabled = true;
                            }
                            break;
                        }

                       
                    }
                }
            }
        }
    }

    public MenuButtonClick(menuButton: MenuButtonPM) {
        switch (menuButton.EventCode) {
            case "TRAN": {
                this.transmitter();
                break;
            }

            case "MIDL": {               
                
                break;
            }

            case "MICN": {
                this.cancel(); 
                break;
            }

            
        }
    }


    transmitter(){
        this.EntityPM.StatusCode = MasavInterfaceStatus.Transmitted;
        this.save();     
        SessionLocator.SelectedSession?.FireEvent("TransmitterMasavInterface");        
    }  
    
    cancel() {    
       
       var confirmWindow = new ConfirmWindow();
       confirmWindow.Show(TextCodeTranslator.Translate("MasavInterface.O.CancelTransmission"));
       confirmWindow.WindowClosed.subscribe((event: any) => {
           if (confirmWindow.Yes) {
              this.EntityPM.StatusCode = MasavInterfaceStatus.Draft;
              this.save(); 
              SessionLocator.SelectedSession?.FireEvent("CancelMasavInterface");   
           }
       });       
            
    }
    
    save(){
        this.EntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
        this.EntityPM.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
        this.entityArgs.EditComponent.SaveChanges();        
        this.entityArgs.EditComponent.ReloadEntityPM();
          
    }
    

   

  
   

   
}
