
declare var window: any;
import { TaxDeductionReportPM } from '../../EntityPMs/TaxDeductionReportPM';
import { MenuButtonPM } from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { TenantPM } from '../../../Common/EntityPMs/TenantPM';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { TaxReportPMService } from '../../Services/StandardPMs/TaxReportPMService';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { AppTool } from '../../../Infrastructure/Tools';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { EntityPMService } from '../../../Infrastructure/Services/EntityPMService';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
//import { TaxDeductionReportExtendedPMService } from '../../Services/ExtendedPMs/TaxReportExtendedPMService';
import { DownloadManager } from '../../../Infrastructure/Utilities/DownloadManager';
import { GeneralPrintHelper } from '../../../Infrastructure/Helpers/GeneralPrintHelper';
import { ServiceLocator } from '../../../Infrastructure/Locators/ServiceLocator';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { DocumentsFilingExtendedPMService } from '../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';
import { DocumentsFilingViewsExtService } from '../../../Common/Services/ExtendedLists/DocumentsFilingViewsExtService';
import { DocumentTypeListExtendedService } from '../../../Common/Services/ExtendedLists/DocumentTypeListExtendedService';

export class TaxDeductionReportMenuButtonsHandler {

    public EntityPM: TaxDeductionReportPM;
    public entityArgs: EntityArgs
    public TenantPM: TenantPM;
    public ObjectTableName: string = "TaxDeductionReport"
    EntityResourceService: EntityResourceService = new EntityResourceService();
   // _TaxReportExtendedPMService: TaxReportExtendedPMService = new TaxReportExtendedPMService();
    _DocumentsFilingViewsExtService: DocumentsFilingViewsExtService = new DocumentsFilingViewsExtService();
    documentsFilingExtendedPMService: DocumentsFilingExtendedPMService = new DocumentsFilingExtendedPMService();
    documentTypeListExtendedService: DocumentTypeListExtendedService = new DocumentTypeListExtendedService();
    objectTable: any;
     docFilingPM: any;
    public SetEntityPM(entityArgs: EntityArgs) {
        this.TenantPM = SessionLocator.TenantPM;
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.objectTable = window.ObjectTables.filter(d => d.Name === this.ObjectTableName)[0];
    }

    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {

                var table = window.ObjectTables.filter(d => d.Name === 'TaxDeductionReport')[0];


                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];

                    switch (button.EventCode) {
                        case "TDMR":
                            {
                               
                                    button.IsDisabled = false;
                                
                                break;
                            }

                        case "DNPD":
                        case "TXFL":
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
    documentType: any;

    public MenuButtonClick(menuButton: MenuButtonPM) {

        switch (menuButton.EventCode) {
            case "DNPD": 
                {
                   
                                       
                    

                    var myPrintHelper = new GeneralPrintHelper("TaxDeductionReport", "TDDP", this.EntityPM.Id, null, this.EntityPM.Email, null);
                    if (myPrintHelper.IsLoadPrintControl) {
                        ServiceLocator.SendTotangoUserActivity("TaxDeductionReport", "Print");
                        myPrintHelper.ShowPrintControl();
                    }

                    //this.documentTypeListExtendedService.getDocumentTypeListByCode("TDDP", this.EntityPM.Tenant).subscribe(myResult => {
                    
                    //    var mm: ServiceResponse = myResult;
                    //    if (!mm.HasError) {
                    //        this.documentType = mm.Result;

                    //        if (this.documentType) {

                    //            this.documentsFilingExtendedPMService.GetDocumentsFilingByDocumentType(this.documentType.Id, this.objectTable.Id, this.EntityPM.Id, this.EntityPM.Tenant).subscribe(myResult => {
                               
                    //                var mm: ServiceResponse = myResult;
                    //                if (!mm.HasError) {
                    //                    this.docFilingPM = mm.Result;

                    //                    if (this.docFilingPM) {
                    //                        DownloadManager.DownloadPage(null, this.docFilingPM.SecurityId);


                    //                    }
                    //                }

                    //            });

                    //        }
                    //    }

                    //});


                    //this._DocumentsFilingViewsExtService.get(this.EntityPM.Id, this.objectTable.Id).subscribe(myResult => {
                    //    console.log("[GetLastDocumentsFilingPM]", myResult);
                    //    var mm: ServiceResponse = myResult;
                    //    if (!mm.HasError) {
                    //        this.docFilingPM = mm.Result;

                    //        if (this.docFilingPM) {

                    //            DownloadManager.DownloadPage(null, this.docFilingPM.SecurityId);

                    //        }

                    //    }

                    //});



                    break;
                }
            case "TXFL":
                {

                   
              


                        this._DocumentsFilingViewsExtService.GetLastDocumentsFilingPM(this.EntityPM.Id, this.objectTable.Id).subscribe(myResult => {
                            console.log("[GetLastDocumentsFilingPM]", myResult);
                            var mm: ServiceResponse = myResult;
                            if (!mm.HasError) {
                                this.docFilingPM = mm.Result;

                                if (this.docFilingPM) {
                                 
                                        DownloadManager.DownloadPage(null, this.docFilingPM.SecurityId);
                                    
                                }

                            }
                           
                        });


                   
                    break;
                }

        }



    }

    private StartBusyIndicator(message: string) {
        SessionLocator.CurrentSession.StartBusyIndicator(message);
    }

    private StopBusyIndicator() {
        SessionLocator.CurrentSession.StopBusyIndicator();
    }

}
