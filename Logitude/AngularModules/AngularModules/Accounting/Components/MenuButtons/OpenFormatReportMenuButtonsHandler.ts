
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
import { DocumentTypePMExtendedService } from '../../../Common/Services/ExtendedPMs/DocumentTypePMExtendedService';
import { DocumentsFilingExtendedPMService } from '../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';


export class OpenFormatReportMenuButtonsHandler {
    public EntityPM: OpenFormatReportPM;
    public entityArgs: EntityArgs
    public TenantPM: TenantPM;
    public ObjectTableName: string = "OpenFormatReport"
    _DocumentsFilingViewsExtService: DocumentsFilingViewsExtService = new DocumentsFilingViewsExtService();
    docFilingPM: any;
    DocumentTypePMExtendedService: DocumentTypePMExtendedService = new DocumentTypePMExtendedService();
    documentType: any;
    DocumentsFilingExtendedPMService: DocumentsFilingExtendedPMService = new DocumentsFilingExtendedPMService();
    private CurrentSession = SessionLocator.SelectedSession;

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
                        case "INIDL":
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
                    

                    this.DocumentTypePMExtendedService.GetDocumentTypeByCode("BKMV", this.TenantPM.Id).subscribe(myResult => {
                        console.log("[GetLastDocumentsFilingPM]", myResult);
                        var mm: ServiceResponse = myResult;
                        if (!mm.HasError) {
                            this.documentType = mm.Result;
                            if (this.documentType) {
                                this.GetDocument();
                            }

                        }
                    });
                    break;
                }
            case "INIDL": // Download
                {

                    this.DocumentTypePMExtendedService.GetDocumentTypeByCode("INI", this.TenantPM.Id).subscribe(myResult => {
                        console.log("[GetLastDocumentsFilingPM]", myResult);
                        var mm: ServiceResponse = myResult;
                        if (!mm.HasError) {
                            this.documentType = mm.Result;
                            if (this.documentType) {
                                this.GetDocument();
                            }

                        }
                    });
                    
                    break;
                }
        }



    }


    GetDocument() {

        var objectTable = window.ObjectTables.filter(d => d.Name === this.ObjectTableName)[0];
       

        this.DocumentsFilingExtendedPMService.GetDocumentsFilingByDocumentType(this.documentType.Id, objectTable.Id, this.EntityPM.Id, this.TenantPM.Id).subscribe(myResult => {
            console.log("[GetLastDocumentsFilingPM]", myResult);
            var mm: ServiceResponse = myResult;
            if (!mm.HasError) {
                this.docFilingPM = mm.Result;

                DownloadManager.DownloadPage(null, this.docFilingPM.SecurityId);
            }
        });
        
    
    }
    

    private StartBusyIndicator(message: string) {
        this.CurrentSession.StartBusyIndicator(message);
    }

    private StopBusyIndicator() {
        this.CurrentSession.StopBusyIndicator();
    }
}

