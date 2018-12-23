
import { OpenFormatReportPM } from '../../EntityPMs/OpenFormatReportPM';
import { MenuButtonPM } from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { TenantPM } from '../../../Common/EntityPMs/TenantPM';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
declare var window: any;
import { DocumentsFilingViewsExtService } from '../../../Common/Services/ExtendedLists/DocumentsFilingViewsExtService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { DownloadManager } from '../../../Infrastructure/Utilities/DownloadManager';



export class OpenFormatReportMenuButtonsHandler {
    public EntityPM: OpenFormatReportPM;
    public entityArgs: EntityArgs
    public TenantPM: TenantPM;
    public ObjectTableName: string = "OpenFormatReport"
    _DocumentsFilingViewsExtService: DocumentsFilingViewsExtService = new DocumentsFilingViewsExtService();
    docFilingPM: any;
 

    public SetEntityPM(entityArgs: EntityArgs) {
        this.TenantPM = SessionLocator.TenantPM;
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
    }

    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {

                var table = window.ObjectTables.filter(d => d.Name === 'OpenFormatReport')[0];



                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];

                    switch (button.EventCode) {
                        case "OFMR":
                            {

                                button.IsDisabled = false;

                                break;
                            }

                        case "OPDL":
                            {
                                if (this.EntityPM.StatusTypeCode != "3") {
                                    button.IsDisabled = true;
                                }
                                else {
                                    button.IsDisabled = false;
                                }

                                break;
                            }
                    }
                }
            }
        }

        return menuButtons;
    }

    public MenuButtonClick(menuButton: MenuButtonPM) {

        switch (menuButton.EventCode) {
          
            case "OPDL": // Download
                {
                    

                    this.GetDocument();
                    break;
                }

        }



    }


    GetDocument() {

        var objectTable = window.ObjectTables.filter(d => d.Name === this.ObjectTableName)[0];
       


        this._DocumentsFilingViewsExtService.GetLastDocumentsFilingPM(this.EntityPM.Id, objectTable.Id).subscribe(myResult => {
                console.log("[GetLastDocumentsFilingPM]", myResult);
                var mm: ServiceResponse = myResult;
            if (!mm.HasError) {
                this.docFilingPM = mm.Result;

                DownloadManager.DownloadPage(null, this.docFilingPM.SecurityId);
            }
            });
        
    
    }


    private StartBusyIndicator(message: string) {
        SessionLocator.CurrentSession.StartBusyIndicator(message);
    }

    private StopBusyIndicator() {
        SessionLocator.CurrentSession.StopBusyIndicator();
    }
}

