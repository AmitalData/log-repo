
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
import { ServiceLocator } from '../../../Infrastructure/Locators/ServiceLocator';
import { GeneralPrintHelper } from '../../../Infrastructure/Helpers/GeneralPrintHelper';

export class OpenFormatReportMenuButtonsHandler {
    public EntityPM: OpenFormatReportPM;
    public entityArgs: EntityArgs
    public TenantPM: TenantPM;
    public ObjectTableName: string = "OpenFormatReport"
    _DocumentsFilingViewsExtService: DocumentsFilingViewsExtService = new DocumentsFilingViewsExtService();
    BMKDocFilingPM: any;
    INIDocFilingPM: any;
    DocumentTypePMExtendedService: DocumentTypePMExtendedService = new DocumentTypePMExtendedService();
    BMKDocumentType: any;
    INIDocumentType: any;
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
                        //case "INIDL":
                        //    {
                        //        if (this.EntityPM.StatusTypeCode != "3") {
                        //            button.IsDisabled = true;
                        //        }
                        //        else {
                        //            button.IsDisabled = false;
                        //        }

                        //        break;
                        //    }

                        case "PDFD":
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
                    this.try = true;
                    this.DocumentTypePMExtendedService.GetDocumentTypeByCode("BKMV", this.TenantPM.Id).subscribe((myResult:any) => {
                        console.log("[GetLastDocumentsFilingPM]", myResult);
                        var mm: ServiceResponse = myResult;
                        if (!mm.HasError) {
                            this.BMKDocumentType = mm.Result;
                            if (this.BMKDocumentType) {

                                this.GetDocumentType("INI");
                              
                            }

                        }
                    });
                    break;
                }
            //case "INIDL": // Download
            //    {

            //        this.DocumentTypePMExtendedService.GetDocumentTypeByCode("INI", this.TenantPM.Id).subscribe((myResult:any) => {
            //            console.log("[GetLastDocumentsFilingPM]", myResult);
            //            var mm: ServiceResponse = myResult;
            //            if (!mm.HasError) {
            //                this.documentType = mm.Result;
            //                if (this.documentType) {
            //                    this.GetDocument();
            //                }

            //            }
            //        });
                    
            //        break;
            //    }
            case "PDFD":

                {
                    var myPrintHelper = new GeneralPrintHelper("OpenFormatReport", "OFDP", this.EntityPM.Id, null, this.EntityPM.ReportNumber, null);
                    if (myPrintHelper.IsLoadPrintControl) {
                        ServiceLocator.SendTotangoUserActivity("OpenFormatReport", "Print");
                        myPrintHelper.ShowPrintControl();
                    }
                    break;


            }
        }



    }
    try: boolean = true;

    GetDocument() {

        var objectTable = window.ObjectTables.filter(d => d.Name === this.ObjectTableName)[0];
       

        this.DocumentsFilingExtendedPMService.GetDocumentsFilingByDocumentType(this.BMKDocumentType.Id, objectTable.Id, this.EntityPM.Id, this.TenantPM.Id).subscribe((myResult:any) => {
            console.log("[GetLastDocumentsFilingPM]", myResult);
            var mm: ServiceResponse = myResult;
            if (!mm.HasError) {
                this.BMKDocFilingPM = mm.Result;
                this.DocumentsFilingExtendedPMService.GetDocumentsFilingByDocumentType(this.INIDocumentType.Id, objectTable.Id, this.EntityPM.Id, this.TenantPM.Id).subscribe((myResult:any) => {
                    console.log("[GetLastDocumentsFilingPM]", myResult);
                    var mm: ServiceResponse = myResult;
                    if (!mm.HasError) {
                        this.INIDocFilingPM = mm.Result;
                        this.GetDocumentType("INI");
                        var securityIds = this.BMKDocFilingPM.SecurityId + "," + this.INIDocFilingPM.SecurityId;
                        if (this.try) {
                            DownloadManager.DownloadPage(null, securityIds);
                            this.try = false;
                        }
                    }
                });
             
            }
        });
        
    
    }

    public GetDocumentType(code: string) {

        this.DocumentTypePMExtendedService.GetDocumentTypeByCode(code, this.TenantPM.Id).subscribe((myResult:any) => {
            console.log("[GetLastDocumentsFilingPM]", myResult);
            var mm: ServiceResponse = myResult;
            if (!mm.HasError) {
                 if (code == "INI") {
                    this.INIDocumentType = mm.Result;
                    if (this.INIDocumentType) {

                           this.GetDocument();
                    }
                }

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

