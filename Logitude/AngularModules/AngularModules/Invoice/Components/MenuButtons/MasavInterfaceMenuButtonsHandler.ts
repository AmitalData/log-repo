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
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { DocumentTypePMExtendedService } from 'Common/Services/ExtendedPMs/DocumentTypePMExtendedService';
import { DocumentsFilingExtendedPMService } from 'Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';
import { DocumentTypePM } from 'Common/EntityPMs/DocumentTypePM';
import { DownloadManager } from 'Infrastructure/Utilities/DownloadManager';
declare var window: any;

export class MasavInterfaceMenuButtonsHandler {
    public entityPM: MasavInterfacePM;
    public entityArgs: EntityArgs
    fullAccountingSettingPMService: FullAccountingSettingPMService = new FullAccountingSettingPMService();
    documentTypePMExtendedService: DocumentTypePMExtendedService = new DocumentTypePMExtendedService();
    documentsFilingExtendedPMService: DocumentsFilingExtendedPMService = new DocumentsFilingExtendedPMService();

    constructor(){
    }

    public SetEntityPM(entityArgs: EntityArgs) {
        this.entityArgs = entityArgs;
        this.entityPM = entityArgs.EntityPM;
    }
    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.entityPM != null) {
            if (this.entityArgs.EditComponent != null) {
               
                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    switch (button.EventCode) {

                        case "TRAN": {
                            if (this.entityPM.StatusCode === MasavInterfaceStatus.Draft || this.entityPM.StatusCode === MasavInterfaceStatus.Failed || this.entityPM.StatusCode === MasavInterfaceStatus.Cancelled) {
                                button.IsDisabled = false;
                            }
                            else {
                                button.IsDisabled = true;
                            }
                            break;
                        }

                        case "MIDL": {
                            if (this.entityPM.StatusCode === MasavInterfaceStatus.Transmitted) {
                                button.IsDisabled = false;
                            }

                            else {
                                button.IsDisabled = true;
                            }
                            break;
                        }

                        case "MICN": {
                            
                            if (this.entityPM.StatusCode == MasavInterfaceStatus.Transmitted) {
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
                this.download("Masav")
                break;
            }

            case "MICN": {
                this.cancel(); 
                break;
            }

            
        }
    }
    transmitter(){
        this.entityPM.StatusCode = MasavInterfaceStatus.InProgress;
        this.save();     
        SessionLocator.SelectedSession?.FireEvent("TransmitterMasavInterface");        
    }  
    cancel() {    
       
       var confirmWindow = new ConfirmWindow();
       confirmWindow.Show(TextCodeTranslator.Translate("MasavInterface.O.CancelTransmission"));
       confirmWindow.WindowClosed.subscribe((event: any) => {
           if (confirmWindow.Yes) {
              this.entityPM.StatusCode = MasavInterfaceStatus.CancellationInProgress;
              this.save(); 
              SessionLocator.SelectedSession?.FireEvent("CancelMasavInterface");   
           }
       });       
            
    }
    
    save(){
        this.entityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
        this.entityPM.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
        this.entityArgs.EditComponent.SaveChanges();        
        this.entityArgs.EditComponent.ReloadEntityPM();
          
    }
    
    download(code: string) {
        this.documentTypePMExtendedService.GetDocumentTypeByCode(code, this.entityPM.Tenant).subscribe((myResult:any) => {
            var mm: ServiceResponse = myResult;
            if (!mm.HasError)  {
                const documentType = mm.Result;
                this.getDocument(documentType); 
            } 
        });
    }
    getDocument(documentType : DocumentTypePM) {
        var objectTable = window.ObjectTables.filter(d => d.Name === "MasavInterface")[0];        
        this.documentsFilingExtendedPMService.GetDocumentsFilingByDocumentType(documentType.Id, objectTable.Id, this.entityPM.Id, this.entityPM.Tenant).subscribe((myResult:any) => {
            var mm: ServiceResponse = myResult;
            if (!mm.HasError) {
                var documentFiling = mm.Result;
                var securityId = documentFiling.SecurityId;                
                DownloadManager.DownloadPage(null, securityId);              
               
            }
        });
    
    }
   

  
   

   
}
