import {DocumentTypePM} from 'Common/EntityPMs/DocumentTypePM';
import {DocumentOutPM} from 'Common/EntityPMs/DocumentOutPM';
import {DocumentTypeListService} from 'Common/Services/StandardLists/DocumentTypeListService';
import {DocumentTypePMExtendedService} from 'Common/Services/ExtendedPMs/DocumentTypePMExtendedService';
import {DocumentTypeList} from 'Common/EntityLists/DocumentTypeList';
import {DocumentOutPMService} from 'Common/Services/ExtendedPMs/DocumentOutPMService';
import {DocsOutDataViewModel} from 'InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/ViewModel/DocsOutDataViewModel';
import { DocumentsFilingExtendedPMService } from 'Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';
import { DownloadManager } from 'Infrastructure/Utilities/DownloadManager';
import { GeneralPrintHelper } from 'Infrastructure/Helpers/GeneralPrintHelper';
import { ServiceLocator } from 'Infrastructure/Locators/ServiceLocator';
import { ServiceHelper } from 'Infrastructure/Utilities/ServiceHelper';
import { AppTool } from 'Infrastructure/Tools';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { MessageWindow } from 'Controls/Windows/MessageWindow';
import { ConfirmWindow } from 'Controls/Windows/ConfirmWindow';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { SessionInfo } from 'Infrastructure/Utilities/SessionInfo';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';
import { HtmlEditorService } from 'Common/Services/DocumentServices/HtmlEditorService';
import { DocumentTypeCustomFieldService } from 'Common/Services/ExtendedPMs/DocumentTypeCustomFieldService';
import { ExportDocumentService } from 'Common/Services/DocumentServices/ExportDocumentService';
import { DocumentTypeTemplateListExtendedService } from 'Common/Services/ExtendedLists/DocumentTypeTemplateListExtendedService';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { DocumentTypeTemplateViewModel } from 'InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/ViewModel/DocumentTypeTemplateViewModel';
import { DocumentCopiesViewModel } from 'InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/ViewModel/DocumentCopiesViewModel';
import { FeatureLocator } from 'Infrastructure/Utilities/FeatureLocator';
import { FroalaEditorFilters } from 'InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/Filters/FroalaEditorFilters';
import { ExportDocumentArgs } from 'Infrastructure/DataContracts/ExportDocumentArgs';
import { DocumentsExecutionLogList } from 'Common/EntityLists/DocumentsExecutionLogList';
import { DocumentsExecutionLogListExtendedService } from 'Common/Services/ExtendedLists/DocumentsExecutionLogListExtendedService';
import { interval } from 'rxjs';
import { timeInterval } from 'rxjs/operators';
import { DocumentCustomFieldsArgs } from 'InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/Filters/DocumentCustomFieldsArgs';
import { DocumentTypeCustomFieldPM } from 'Common/EntityPMs/DocumentTypeCustomFieldPM';
import { SelectItem, InterestReportArguments } from 'Accounting/DataContracts/InterestReportArgs';
import { InterestReportExtendedListService } from 'Accounting/Services/ExtendedLists/InterestReportExtendedListService';
import { ObjectsLocator } from 'Infrastructure/Locators/ObjectsLocator';
declare var window: any;

export class BatchPrintServiceHelper{
    public IsLoadPrintControl: boolean = false;
    public IsStartPrint: boolean = false;
    public ChildReference: string;
    public EntityId: string;
    public DocumentTypeCode: string;
    public ChildEntityId: string;
    public CurrentObjectTableId: string;
    public CurrentDocumentOut: DocumentOutPM;
    private CurrentSession = SessionLocator.SelectedSession;
    public DataContext: DocsOutDataViewModel;
    ChildObjectTableId: string;
    documentTypeList: DocumentTypeList;
    documentOutPM: DocumentOutPM;
    documentTypePM: DocumentTypePM;
    documentTypePMService: DocumentTypePMExtendedService;
    documentOutPMService: DocumentOutPMService;
    public isRTL: boolean = false;
    public ObjectTableName: string ="ARInvoice";
    public Entity:any;
    public CurrentCount:number;
    public LastCount:number;

    IsBuildDocumentViaWorkerRole: boolean = false;
    public InterestReportArgs: InterestReportArguments;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private interestReportExtendedListService: InterestReportExtendedListService = new InterestReportExtendedListService();
    DocumentsFilingExtendedPMService: DocumentsFilingExtendedPMService = new DocumentsFilingExtendedPMService();
    public _documentTypeCustomFieldService: DocumentTypeCustomFieldService = new DocumentTypeCustomFieldService();
    public _documentOutPMService: DocumentOutPMService = new DocumentOutPMService();
    public _documentTypePMService: DocumentTypePMExtendedService = new DocumentTypePMExtendedService();
    public _exportDocumentService: ExportDocumentService = new ExportDocumentService();
    public _documentTypeTemplateListExtendedService: DocumentTypeTemplateListExtendedService = new DocumentTypeTemplateListExtendedService();
    public _htmlEditorService: HtmlEditorService = new HtmlEditorService();
constructor( ){
    if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
    this.documentTypePMService = new DocumentTypePMExtendedService();
    this.documentOutPMService = new DocumentOutPMService();
}
 
CreateInvoiceButtonClicked() {
  
    this.CurrentSession.StartBusyIndicatorLoading();
    this.interestReportExtendedListService.PutBatchPrint(this.InterestReportArgs).subscribe((response: ServiceResponse) => {
    this.StopBusyIndicator();
      var mm: ServiceResponse = response;
      if (!mm.HasError) {
        let file = new Blob([mm.Result], { type: 'application/pdf' });
        let url = window.URL.createObjectURL(file);
        window.open(url);
      }
      else {
        if(mm.ErrorsArray){
          var msg = new MessageWindow();
          msg.RTL = this.isRTL;
          msg.Width = 400;
          msg.Show(mm.ErrorsArray[0]);
      }
      }

    });
}
PrintAllInterestInvoices(InterestReportArgs: InterestReportArguments){
this.InterestReportArgs= InterestReportArgs;
this.InterestReportArgs.SelectedItems =[];
this.LastCount = this.InterestReportArgs.Entities.length -1;
this.CurrentCount =0

this.PrintInvoice(this.InterestReportArgs.Entities[this.CurrentCount])
this.BuildingDocumentText  = "Building document ( "+(this.CurrentCount+1)+" From "+(this.LastCount+1)+" )";

 
}
 PrintInvoice(Entity:any){
    this.Entity =Entity; 
    var myEntityId: string = null;
    var myChildEntityId: string = null;
    var myObjectTableName: string = null;
    var mychildObjectTableId: string = null;
    var myDocumentTypeCode: string = null;
    var myReference: string = null;
    myEntityId = this.Entity.Id;
    myChildEntityId = null;
    mychildObjectTableId = null;
    myObjectTableName = "ARInvoice";
    myDocumentTypeCode = "999G";
    myReference = !AppTool.IsNullOrEmpty(this.Entity.InvoiceNumber) ? this.Entity.InvoiceNumber : "Draft: " + this.Entity.DraftNumber;
   this.PreparedPrintVariable(myObjectTableName,myDocumentTypeCode,myEntityId,myChildEntityId,myReference,mychildObjectTableId);
   this.ShowPrintControl();
  }
  PrintAllDocs() {

    var token = ServiceHelper.GetLDocumentDownloadToken();
    var Item :SelectItem=new SelectItem();
    Item.Id = this.Entity.Id;
    Item.SecurityId = this.CurrentDocumentOut.SecurityId;
    Item.TempId = token;
    this.InterestReportArgs.SelectedItems.push(Item);

    if(this.CurrentCount == this.LastCount){
      this.CreateInvoiceButtonClicked();
    }
    else{
        this.CurrentCount +=1;
        this.PrintInvoice(this.InterestReportArgs.Entities[this.CurrentCount])
        this.BuildingDocumentText  = "Building document ( "+(this.CurrentCount+1)+" From "+(this.LastCount+1)+" )";
    }
    // window.open(ServiceHelper.GetLogitudeURL() + "WebPages/MergeAllPage.aspx?securityId=" + this.CurrentDocumentOut.SecurityId + "~" + SessionInfo.LoggedUserId + "&tempId=" + token);
  }
 
 
 
 
  PreparedPrintVariable(objecttablename: string, documentTypeCode: string, entityId: string, childEntityId: string, childReference:string ,childObjectTableId:string ) {
    this.ObjectTableName = objecttablename;
    if (!AppTool.IsNullOrEmpty(documentTypeCode)) {
        this.DocumentTypeCode = documentTypeCode.toUpperCase();
    }

    this.CurrentObjectTableId = window.ObjectTables.filter(d => d.Name == objecttablename)[0].Id;
    this.EntityId = entityId == "null" || !entityId ? "" : entityId;
    this.ChildEntityId = childEntityId == "null" || !childEntityId ? "" : childEntityId;
    this.ChildObjectTableId = childObjectTableId == "null" || !childObjectTableId ? "" : childObjectTableId;
    this.ChildReference = childReference == "null" || !childReference ? "" : childReference;

    this.documentTypePMService = new DocumentTypePMExtendedService();
    this.documentOutPMService = new DocumentOutPMService();
    var documentTypeListService = new DocumentTypeListService();


    var apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
    apiQueryFilters.GetAll = true;
    apiQueryFilters.Tenant = SessionInfo.LoggedUserTenant;

    documentTypeListService.getAllFromCache(apiQueryFilters).subscribe((res:any) => {
        var pmResponse: ServiceResponse = res;
        if (!pmResponse.HasError) {
            var myResult = pmResponse.Result;
              this.documentTypeList = myResult.filter(d => d.Code.toUpperCase() == this.DocumentTypeCode)[0];
            if (this.documentTypeList) {
                if (this.documentTypeList.DocumentTypeDefaultReportTemplateId) {
                    this.IsLoadPrintControl = true;

                }
                else this.ShowMessage("Document type of code " + this.DocumentTypeCode + " has no default template");

            }

            else {
                if (this.ObjectTableName == "APPayment") {
                    this.ShowMessage("There is no document type for A/P Payment please go to maintenance and add it!");
                }
                else if (this.ObjectTableName == "ARPayment") {
                    this.ShowMessage("There is no document type for A/R Payment please go to maintenance and add it!");
                }

                else this.ShowMessage("Document type of code " + this.DocumentTypeCode + " not exists");

            }
        

        }
    });


}
StartPrinting(myEntityId: string, myChildEntityId: string, myObjectTableName: string, mychildObjectTableId: string, myDocumentTypeCode: string, myReference: string) {
  var myPrintHelper = new GeneralPrintHelper(myObjectTableName, myDocumentTypeCode, myEntityId, myChildEntityId, myReference, mychildObjectTableId);
  if (myPrintHelper.IsLoadPrintControl) {
      ServiceLocator.SendTotangoUserActivity("ARInvoice", "PrintInvoice");
      myPrintHelper.ShowPrintControl();
  }
}
public ShowMessage(message: string, title: string = "") {

  var messageWindow: MessageWindow = new MessageWindow();
  messageWindow.Show(message);

  if (title) {
      messageWindow.Title = title;
  }
}

ShowPrintControl() {

  if (this.IsLoadPrintControl && !this.IsStartPrint) {
      this.IsStartPrint = true;
      this.CurrentSession.StartBusyIndicatorLoading();
      this.documentOutPMService.getDocumentOutByDocumentTypeEntityAndChild(this.EntityId, SessionInfo.LoggedUserTenant, this.ChildEntityId, this.documentTypeList.Id).subscribe((res:any) => {
          var pmResponse: ServiceResponse = res;
          if (!pmResponse.HasError) {
              var myResult = pmResponse.Result;

              this.documentOutPM = myResult;

              if (!this.documentOutPM) {
                  this.documentOutPMService.getCreateDocumentOut(this.documentTypeList.Id, this.EntityId, this.ChildEntityId, this.ChildReference, this.CurrentObjectTableId, SessionInfo.LoggedUserTenant).subscribe((res:any) => {
                      var pmResponse: ServiceResponse = res;
                      if (!pmResponse.HasError) {
                          var myResult = pmResponse.Result;
                          if (myResult) {
                              this.documentOutPM = myResult;
                              this.LoadDocumentTypePm();
                          }

                      }
                      else {
                        this.StopBusyIndicator();
                        this.IsStartPrint = false;
                      }

                  });
              }
              else {


                  this.LoadDocumentTypePm();
              }

          }
          else {
              this.IsStartPrint = false;
              this.StopBusyIndicator();
            }

      });

  }

}

LoadDocumentTypePm() {


  this.documentTypePMService.getSingleDocumentType(this.documentTypeList.Id, this.documentOutPM.Id, SessionInfo.LoggedUserTenant).subscribe((res:any) => {


      var pmResponse: ServiceResponse = res;
      if (!pmResponse.HasError) {
          var myResult = pmResponse.Result;
          if (myResult) {
              this.documentTypePM = myResult;
              this.LoadPrintControl();
          }

      }
      else {
        this.StopBusyIndicator();
        this.IsStartPrint = false;
      }


  });
                      
}


LoadPrintControl() {
  
     this.IsStartPrint = false;

     this.StopBusyIndicator();
     var documentOutPmLists = new Array<DocumentOutPM>();
      //this.documentOutPM.NeedsRebuild = true;
     documentOutPmLists.push(this.documentOutPM);
     var SelectedInternalDocument = new DocsOutDataViewModel(this.documentTypePM, this.EntityId, this.documentOutPM.ChildEntityId, this.CurrentObjectTableId, this.ChildObjectTableId, this.documentOutPM.ChildEntityReference,
         documentOutPmLists, null, null, null);
     this.CurrentDocumentOut = SelectedInternalDocument.CurrentDocument;
     SelectedInternalDocument.IsNotFromDocsOutListOpenPrintControl = true;
      SelectedInternalDocument.IsAWBWizard = false;

    //   var logitudeWindow = new LogitudeWindow();
    //         logitudeWindow.Width = 760;

    //         var heightwindwo: number = this.documentOutPM.IssuedDate ? 552 : 502; 
    //         logitudeWindow.Height = heightwindwo;
    //         logitudeWindow.DataContext = SelectedInternalDocument;
    //         logitudeWindow.Title = "Print " + this.documentTypePM.Name;
    //         logitudeWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/PrintDocumentComponent');
    //         logitudeWindow.WindowClosed.subscribe(($event: any) => {
    //             if (this.CurrentSession.CurrentEditComponent) {
    //                 this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    //             }
    //         });

            this.SetDataContext(SelectedInternalDocument);
            
    //    this.PrintAllDocs();
 
}

SetDataContext(dataContext: any) {

    this.DataContext = dataContext;
    this.setArguments(this.DataContext);
}


public setArguments(item: DocsOutDataViewModel) {

    this._exportDocumentService.GetIsRunStimulDocumentViaWorkerRole().subscribe((res: any) => {

        var serviceResponse: ServiceResponse = res;
        if (!serviceResponse.HasError) this.IsBuildDocumentViaWorkerRole = serviceResponse.Result;

        this._entityResourceService.getEntityResourceByTableName("DocsOut").subscribe((response: any) => {


            if (!item.DocumentTypePM) {
                this.CurrentSession.StartBusyIndicator("Loading...");

                this._documentTypePMService.GetSinglePMWithOutInclude(item.Id, SessionLocator.Tenant).subscribe((res: any) => {

                    var pmResponse: ServiceResponse = res;
                    if (!pmResponse.HasError) {
                        item.DocumentTypePM = pmResponse.Result;
                    }
                    this.StopBusyIndicator();
                    this.Start(item);
                });

            }
            else this.Start(item);

        });
    });

}

 
IsQuotationDocument: boolean = false;
IsSystemAdditionalPrintingFields: boolean;
PrintingFieldsScreenCode: string;
BuildingDocumentText: string = "Building document ( "+(this.CurrentCount+1)+" From "+(this.LastCount+1)+" )";
ObjectTableId: string;
public DocumentTypeTemplateLists: DocumentTypeTemplateViewModel[];
public Title: string;
public isAWBWizard: boolean;
public PrintAllCopiesBtnVisible: boolean;
public PrintAllCopiesBtnDisable: boolean;
public SelectedAsDefaultBtnVisible: boolean;
LastBuildDateVisible: boolean;

 Start(item: DocsOutDataViewModel) {

    this.DataContext = item;
    var buildingDocumentText: string = TextCodeTranslator.Translate("Accounting.General.O.BuildingDocument");

    if (!AppTool.IsNullOrEmpty(buildingDocumentText)) {
      //  this.BuildingDocumentText = buildingDocumentText;
    }


    this.ChildEntityId = item.ChildEntityId ? item.ChildEntityId : "";
    this.ChildObjectTableId = item.ChildObjectTableId ? item.ChildObjectTableId : "";
    this.ChildReference = item.ChildReference ? item.ChildReference : "";
    this.ObjectTableId = item.CurrentObjectTableId;
    this.CurrentDocumentOut = this.DataContext.CurrentDocument;
    this.DocumentTypeTemplateLists = new Array<DocumentTypeTemplateViewModel>();
    this.EntityId = item.EntityId;
    this.Title = "Print " + this.DataContext.DocumentTypePM.Name;
    this.isAWBWizard = this.DataContext.IsAWBWizard;
    this.IsSystemAdditionalPrintingFields = this.DataContext.DocumentTypePM.IsSystemAdditionalPrintingFields;
    this.PrintingFieldsScreenCode = this.DataContext.DocumentTypePM.PrintingFieldsScreenCode;

    var table = window.ObjectTables.filter(d => d.Id == this.ObjectTableId)[0];


    if (table) {
        this.ObjectTableId = table.Id;
        this.ObjectTableName = table.Name;

    }



    if (this.ObjectTableName == "Quote" && item.DocumentTypeCode == "QUOTE") {

        this.IsQuotationDocument = true;
    }



    if (this.isAWBWizard) {
        this.PrintAllCopiesBtnVisible = false;
        this.SelectedAsDefaultBtnVisible = false;
        this.LastBuildDateVisible = false;

    }

    if (this.DataContext.DocumentTypePM.IsReadOnly) {
        this.PrintAllCopiesBtnVisible = false;
        this.SelectedAsDefaultBtnVisible = false;
        this.LastBuildDateVisible = false;

    }


    if (this.DataContext.DocumentTypePM.IsDocumentOneTimePrintLimited) {
        this._documentOutPMService.getSingleDocumentOutPM(this.CurrentDocumentOut.Id, this.CurrentDocumentOut.Tenant).subscribe((res: any) => {

            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    myResult.HasFollowUp = this.CurrentDocumentOut.HasFollowUp;
                    this.CurrentDocumentOut = myResult;
                    this.InitializeCopeisControl();
                }

            }


        });
    }

    else {

        this.InitializeCopeisControl();
    }



}

IsNoTemplateDefult: boolean = false;
IsShowDocumentCustomFields: boolean;
BuildButtonIsEnabled: boolean = true;
public LastBuildDate: Date;

InitializeCopeisControl() {

    if (!this.CurrentDocumentOut.DocumentTemplateEditorTool && !this.IsQuotationDocument) {
        this.IsNoTemplateDefult = true;
    }

    this.GetTemplates();


    // this.IsLoading = true;

    if (this.CurrentDocumentOut.DocumentTemplateEditorTool == "R") {
        this.IsShowDocumentCustomFields = false;
        this.PrintAllCopiesBtnDisable = true;
    }
    else {
        this.BuildButtonIsEnabled = true;
        this.PrintAllCopiesBtnDisable = false;
    }




    if (this.DataContext.DocumentTypePM.IsDocumentOneTimePrintLimited) {
        this.SelectedAsDefaultBtnVisible = false;
        this.BuildButtonIsEnabled = false;
    }

    this.LoadCopiesControl();


    this.LoadDocumentCustomFields();

    if (this.CurrentDocumentOut.IssuedDate) {
        this.LastBuildDate = this.CurrentDocumentOut.IssuedDate;//.toString();
        if (!this.isAWBWizard) {
            this.LastBuildDateVisible = true;
        }
    }
    else {
        this.LastBuildDateVisible = false;
    }



}
public DocumentCustomFieldsArgs: DocumentCustomFieldsArgs;
public DocumentTypeCustomFieldLists: DocumentTypeCustomFieldPM[];

public LoadDocumentCustomFields() {

    this.DocumentCustomFieldsArgs = new DocumentCustomFieldsArgs();
    this.DocumentCustomFieldsArgs.EditCustomField = false
    this.DocumentCustomFieldsArgs.ObjectTableId = this.ObjectTableId;
    this.DocumentCustomFieldsArgs.DocumentTypeId = this.DataContext.DocumentTypePM.Id;
    this.DocumentCustomFieldsArgs.EntityId = this.EntityId;

    if (!this.IsSystemAdditionalPrintingFields) {

        this._documentTypeCustomFieldService.getDocumentTypeCustomFieldsByDocument(this.CurrentDocumentOut.Tenant, this.DocumentCustomFieldsArgs.DocumentTypeId).subscribe((res: any) => {


            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {

                    this.DocumentTypeCustomFieldLists = myResult;
                    if (this.DocumentTypeCustomFieldLists.length > 0) {

                        this.DocumentCustomFieldsArgs.DocumentTypeCustomFieldLists = this.DocumentTypeCustomFieldLists;
                        this.IsShowDocumentCustomFields = true;


                    }
                    else {
                        this.IsShowDocumentCustomFields = false;

                    }
                }
                else {
                    this.IsShowDocumentCustomFields = false;

                }

            }
            else this.StopBusyIndicator();








        });
    }
    else {
        if (!AppTool.IsNullOrEmpty(this.PrintingFieldsScreenCode)) {

            this.DocumentCustomFieldsArgs.ScreenCode = this.PrintingFieldsScreenCode;
            this.DocumentCustomFieldsArgs.ObjectTableName = this.ObjectTableName;
            this.DocumentCustomFieldsArgs.EntityPM = this.DataContext.EntityPM;
            this.IsShowDocumentCustomFields = true;

        }
    }

}

public ItemsSource: DocumentCopiesViewModel[];
public DocumentTypeload: DocumentTypePM;
public Items: DocumentCopiesViewModel[];

LoadCopiesControl() {


    this.ItemsSource = new Array<DocumentCopiesViewModel>();

    this._documentTypePMService.getSingleDocumentType(this.DataContext.DocumentTypePM.Id, this.CurrentDocumentOut.Id, this.CurrentDocumentOut.Tenant).subscribe((res: any) => {

        var pmResponse: ServiceResponse = res;
        if (!pmResponse.HasError) {
            var myResult = pmResponse.Result;
            if (myResult) {
                this.DocumentTypeload = myResult;
                if (this.DocumentTypeload != null) {
                    this.DataContext.DocumentTypePM = myResult;
                    if (this.DataContext.DocumentTypePM.DocumentTypeCopies != null) {
                        this.DocumentTypeload.DocumentTypeCopies.forEach((item) => {

                            this.ItemsSource.push(new DocumentCopiesViewModel(item, this.CurrentDocumentOut, this.EntityId, this.ChildEntityId, this.ObjectTableId, this.ChildObjectTableId, this.DocumentTypeload, this.ChildReference));
                        });

                        var item = this.ItemsSource.filter(d => d.IsSelected)[0];
                        var anySelected = false;
                        if (!item) {
                            this.ItemsSource.forEach((item) => {
                                item.IsHideSetSelectedAsDefaultBtn = true;
                                item.IsSelected = item.IsSelectedByDefault;
                                this.SelectedAsDefaultBtnVisible = false;
                                anySelected = true;
                            });

                        }

                        this.Items = this.ItemsSource.filter(s=>!s.CurrentDocumentType.IsDocumentOneTimePrintLimited  || s.CurrentDocumentType.LimitedPrintCopyId != s.CurrentDocumentOutCopy.DocumentTypeCopyId || AppTool.IsNullOrEmpty(s.CurrentDocumentOutCopy.LastPrintedByUserId));
                        this.SortItemSource();

                    }

                }


                if (this.ItemsSource.length == 1) {
                    this.PrintAllCopiesBtnVisible = false;
                }
                else {

                    if (this.ItemsSource.length > 1) {
                        this.PrintAllCopiesBtnVisible = true;
                    }

                }

                this.CopiesControlLoaded(this.ItemsSource);

            }
        }


    });


}
public lastCount: number = 0;

CopiesControlLoaded(copies: Array<DocumentCopiesViewModel>) {


    if (copies != null) {


        if (copies.length == 1) {
            this.PrintAllCopiesBtnVisible = false;
            this.SelectedAsDefaultBtnVisible = false;
        }
        else if (copies.length > 1) {
            this.PrintAllCopiesBtnVisible = true;

        }



        if (FeatureLocator.IsPackage_EAWB()) {
            copies.forEach(d => d.IsSelectedByDefault = true);
        }


        if (this.DocumentTypeload.IsDocumentOneTimePrintLimited) {
            copies.forEach((item) => { item.CurrentDocumentTypeCopy.IsSelectedByDefault = true; });
        }

        this.lastCount = copies.filter(d => d.CurrentDocumentTypeCopy.IsSelectedByDefault).length;

        // if ((this.CurrentDocumentOut.DocumentOutCopies.length == 0 || this.CurrentDocumentOut.NeedsRebuild)) {
 
            var editorToolCode = null;


            if (this.CurrentDocumentOut.DocumentTemplateEditorTool != null && this.CurrentDocumentOut.DocumentTemplateEditorTool != "") {
                editorToolCode = this.CurrentDocumentOut.DocumentTemplateEditorTool;

            }


            else if (this.DataContext.DocumentTypePM.DocumentTypeDefaultEditorTool != null && this.DataContext.DocumentTypePM.DocumentTypeDefaultEditorTool != "") {
                editorToolCode = this.DocumentTypeload.DocumentTypeDefaultEditorTool;
            }

            if (editorToolCode) {
                if (editorToolCode == "S") this.BuildCurrentCopies(copies, "New");
                if (editorToolCode == "R") {

                    this.Items = new Array<DocumentCopiesViewModel>();
                    this.Items = copies;
                    this.ReBluidHtmlDocument(this.CurrentDocumentOut.DocumentTypeId);
                }
            }
            else {

                this.StopBusyIndicator();
            }
        // }

        // else {
        //     this.Items = new Array<DocumentCopiesViewModel>();
        //     this.Items = copies;
        //     this.StopBusyIndicator();

        // }



        this.SortItemSource();


    }

}
IsDocumentBuildSucceeded: boolean = false;
IsDocumentBuildFailed: boolean = false;
HeaderHeight: number;
FooterHeight: number;
public HtmlEditorData: string;

ReBluidHtmlDocument(documentTypeCopyId: string) {
    this.IsDocumentBuildSucceeded = false;
    this.IsDocumentBuildFailed = false;

    var documentTypeId = this.CurrentDocumentOut.DocumentTypeId;
    var shipmentId = this.CurrentDocumentOut.EntityId;

    var item = null;
    if (this.DocumentTypeTemplateLists) item = this.DocumentTypeTemplateLists.filter(d => d.Id == this.CurrentDocumentOut.DocumentTemplateId)[0];

    if (item && !AppTool.IsNullOrEmpty(item.HtmlResolve)) {
        this.HeaderHeight = item.TemplateHeaderHeight;
        this.FooterHeight = item.TemplateFooterHeight;
        this.HtmlEditorData = item.HtmlResolve;
        this.CurrentSession.StartBusyIndicator(this.BuildingDocumentText);
        this.SaveReportData(documentTypeCopyId);
        item.HtmlResolve = null;
    }
    else {
        this.CurrentSession.StartBusyIndicatorLoading();
        this._htmlEditorService.getEditorHtmlData(this.CurrentDocumentOut.Id, shipmentId, this.ObjectTableId, this.ChildEntityId, this.ChildObjectTableId, SessionInfo.LoggedUserTenant, SessionInfo.LoggedUserId, false, this.CurrentDocumentOut.DocumentTemplateId, "", "Edit").subscribe((res: any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    this.HtmlEditorData = "<header>" + "<height>" + "<div style='display:none'>" + myResult.HeaderHeight + "</div></height>" + myResult.HeaderHtml + "</header>" + myResult.Htmlstring + "<footer>" + "<height>" + "<div style='display:none'>" + myResult.FooterHeight + "</div></height>" + myResult.FooterHtml + "</footer>";
                    this.HeaderHeight = myResult.HeaderHeight;
                    this.FooterHeight = myResult.FooterHeight;
                }
                this.StopBusyIndicator();
                this.CurrentSession.StartBusyIndicator("Building document ( "+(this.CurrentCount+1)+" From "+(this.LastCount+1)+" )");
                this.SaveReportData(documentTypeCopyId);

            } else this.StopBusyIndicator();



        });



    }
}


BuildCurrentCopies(copies: Array<DocumentCopiesViewModel>, mode: string) {

    this.IsDocumentBuildSucceeded = false;
    this.IsDocumentBuildFailed = false;

    this.CurrentSession.StartBusyIndicator(this.BuildingDocumentText);

    this.AddedDocumentTypeCopyViewModels = new Array<DocumentCopiesViewModel>();
    this.RemovedDocumentTypeCopyViewModels = new Array<DocumentCopiesViewModel>();

    var anySelected = false;


    copies.forEach((copy) => {
        if (!this.DocumentTypeload.IsDocumentOneTimePrintLimited) {
            if (copy.IsSelected || copies.length == 1) {
                anySelected = true;

                if (copies.length == 1) {
                    copy.IsSelected = true;
                    copy.CurrentDocumentTypeCopy.IsSelectedByDefault = true;
                    copy.IsDiableSelctedDocumentTypeCopy = true;
                }
                else {
                    copy.IsDiableSelctedDocumentTypeCopy = false;
                }
                this.AddedDocumentTypeCopyViewModels.push(copy);
            }
            if (copy.Exists && !copy.IsSelected) {

                var index = this.RemovedDocumentTypeCopyViewModels.indexOf(copy, 0);
                if (index) {
                    this.RemovedDocumentTypeCopyViewModels.splice(index, 1);
                }

            }
        }

        else {
            copy.IsSelected = true;
            anySelected = true;
            this.AddedDocumentTypeCopyViewModels.push(copy);
        }


    });
    if (anySelected) {

        this.lastCount = this.AddedDocumentTypeCopyViewModels.length;

        var numberOfCopy = this.AddedDocumentTypeCopyViewModels.filter(d => d.IsSelected).length;
        var count: number = 0;
        var copiesIds: string[] = [];

        this.AddedDocumentTypeCopyViewModels.filter(d => d.IsSelected).forEach((copy) => {
            if (!this.IsBuildDocumentViaWorkerRole) {

                this._exportDocumentService.getDocumentPdfFile(this.DataContext.DocumentTypePM.Id, this.EntityId, this.ObjectTableId, this.ChildEntityId, this.ChildObjectTableId, this.CurrentDocumentOut.Id, this.CurrentDocumentOut.Tenant, copy.CurrentDocumentTypeCopy.Id, SessionLocator.LoggedUserId).subscribe((res: any) => {
                    count += 1;
                    var pmResponse: ServiceResponse = res;
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;

                        if (myResult != null) {
                            copy.Status = "Success";
                            copy.Exists = true;

                            if (numberOfCopy == count) {
                                this.UpdateDocumentOutData();
                                this.SaveContext();
                            }
                        }
                        else  this.StopBusyIndicator();
                            
                         




                    } else {
                        var messageError: string;
                        if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                            messageError = pmResponse.ErrorsArray[0];
                        }

                        this.ShowMessage(messageError);
                      
                        this.StopBusyIndicator();
                       
                    }

                });
            }

        });

        if (this.IsBuildDocumentViaWorkerRole) {
            this.BliudDocumentViewWorkerRole(this.AddedDocumentTypeCopyViewModels.filter(d => d.IsSelected));
        }


        if (mode == "New" && this.AddedDocumentTypeCopyViewModels) {

            var copies = new Array<DocumentCopiesViewModel>();
            this.Items.forEach((copy) => {
                var item = this.AddedDocumentTypeCopyViewModels.filter(d => d.Id == copy.Id)[0];
                if (item) copies.push(item);
                else copies.push(copy);

            });

            this.Items = copies;

            this.SortItemSource();
        }




    }

    else {
        
        this.StopBusyIndicator();
        
        this.ShowMessage(TextCodeTranslator.Translate("DocsOut.M.SelectCopyThenRebuild"));
    }


}
BliudDocumentViewWorkerRole(documentTypeCopyLists: DocumentCopiesViewModel[]) {
    if (documentTypeCopyLists.length > 0) {
        var exportDocumentArgs = new ExportDocumentArgs();
        exportDocumentArgs.DocumentTypeId = this.DataContext.DocumentTypePM.Id;
        exportDocumentArgs.EntityId = this.EntityId;
        exportDocumentArgs.ObjectTableId = this.ObjectTableId;
        exportDocumentArgs.ChildEntityId = this.ChildEntityId;
        exportDocumentArgs.ChildObjectTableId = this.ChildObjectTableId;
        exportDocumentArgs.CurrentDocumentOutId = this.CurrentDocumentOut.Id;
        exportDocumentArgs.LoggedContactId = SessionLocator.LoggedUserId;
        exportDocumentArgs.Tenant = SessionLocator.Tenant;
        exportDocumentArgs.DocumentTypeName = this.DataContext.DocumentTypePM.Name;
        exportDocumentArgs.DocumentTypeTemplateId = this.CurrentDocumentOut.DocumentTemplateId;
        exportDocumentArgs.DocumentTypeCopyIdsList = documentTypeCopyLists.map(function (a) { return a.Id; });
        this._exportDocumentService.BuildDocumentViaWorkerRole(exportDocumentArgs).subscribe((myResponse: ServiceResponse) => {
            var result: any = myResponse.Result;
            if (!myResponse.HasError && result) {
                this.StartCheckDocumentBuildViaWorkerRoleTimer(result, documentTypeCopyLists);
            } else {

                this.StopBusyIndicator();


                var messageError: string;
                if (myResponse.ErrorsArray && myResponse.ErrorsArray.length > 0) {
                    messageError = myResponse.ErrorsArray[0];
                }
                this.ShowMessage(messageError);
            }

        });
    }
}
initializeStartCheckDocumentBuildViaWorkerRoleTimer() {
    return interval(250).pipe(timeInterval());
}

private StartCheckDocumentBuildViaWorkerRoleTimerTimersub: any = null;
IsStartCheckDocumentBuildViaWorkerRoleTimer: boolean = false;
private documentsExecutionLogListExtendedService: DocumentsExecutionLogListExtendedService;

StartCheckDocumentBuildViaWorkerRoleTimer(documentExecutionLogId, documentTypeCopyLists) {
    if (this.IsStartCheckDocumentBuildViaWorkerRoleTimer) {
        this.StartCheckDocumentBuildViaWorkerRoleTimerTimersub.unsubscribe();
    }

    this.IsStartCheckDocumentBuildViaWorkerRoleTimer = true;
    this.StartCheckDocumentBuildViaWorkerRoleTimerTimersub = this.initializeStartCheckDocumentBuildViaWorkerRoleTimer().subscribe(respose => {


        if ((this.CurrentSession && this.CurrentSession.isDestroingSession) || !this.IsStartCheckDocumentBuildViaWorkerRoleTimer) {
            this.StartCheckDocumentBuildViaWorkerRoleTimerTimersub.unsubscribe();
            this.IsStartCheckDocumentBuildViaWorkerRoleTimer = false;
            return;
        }


        if (this.IsStartCheckDocumentBuildViaWorkerRoleTimer) {

            if (this.documentsExecutionLogListExtendedService == null) {
                this.documentsExecutionLogListExtendedService = new DocumentsExecutionLogListExtendedService();
            }


            this.documentsExecutionLogListExtendedService.GetDocumentsExecutionLogList(documentExecutionLogId).subscribe((res: any) => {
                var pmResponse: ServiceResponse = res;
                var documentsExecutionLogList: DocumentsExecutionLogList = res.Result;

                if (this.IsStartCheckDocumentBuildViaWorkerRoleTimer) {
                    if (pmResponse.HasError || !documentsExecutionLogList || (documentsExecutionLogList && (documentsExecutionLogList.StatusCode == "D" || documentsExecutionLogList.StatusCode == "F" || documentsExecutionLogList.StatusCode == "T"))) {
                        this.StartCheckDocumentBuildViaWorkerRoleTimerTimersub.unsubscribe();
                        this.IsStartCheckDocumentBuildViaWorkerRoleTimer = false;
                        this.StopBusyIndicator();
                    }

                    if (!pmResponse.HasError) {

                        if (documentsExecutionLogList) {
                            if (documentsExecutionLogList.StatusCode == "F" || documentsExecutionLogList.StatusCode == "T") {
                                this.ShowMessage(documentsExecutionLogList.ExceptionMessage);
                            }
                            else if (documentsExecutionLogList.StatusCode == "D") {
                                documentTypeCopyLists.forEach((copy) => {
                                    copy.Status = "Success";
                                    copy.Exists = true;
                                });
                                this.CurrentDocumentOut.Issued = true;
                                this.CurrentDocumentOut.NeedsRebuild = false;
                                this.DataContext.Issued = true;
                                this.CurrentDocumentOut.IssuedByUserId = SessionInfo.LoggedUserId;
                                this.CurrentDocumentOut.IsChangeIssuedDate = true;
                                this.SaveContext();
                            }
                        }
                        else {
                            this.ShowMessage("Documents execution Log not found");
                        }

                    }
                    else {

                        var messageError: string;
                        if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                            messageError = pmResponse.ErrorsArray[0];
                        }
                        this.ShowMessage(messageError);





                    }

                }

            });

        }
    });

}




UpdateDocumentOutData() {
    if (this.CurrentDocumentOut) {
        this.CurrentDocumentOut.Issued = true;
        this.CurrentDocumentOut.NeedsRebuild = false;
        this.DataContext.Issued = true;
        this.CurrentDocumentOut.IssuedByUserId = SessionInfo.LoggedUserId;
        this.CurrentDocumentOut.IsChangeIssuedDate = true;
    }
}

docIds: string;
public idArray: string[];
public AddedDocumentTypeCopyViewModels: DocumentCopiesViewModel[];
public RemovedDocumentTypeCopyViewModels: DocumentCopiesViewModel[];
SaveReportData(documentTypeCopyId: string) {

    var filter = new FroalaEditorFilters();
    filter.DocumentOutId = this.DataContext.CurrentDocument.Id;
    filter.DocumentTypeCopyId = documentTypeCopyId;
    filter.Tenant = SessionInfo.LoggedUserTenant;
    filter.HtmlString = this.HtmlEditorData;
    filter.HeaderHeight = this.HeaderHeight;
    filter.FooterHeight = this.FooterHeight;
    filter.EntityId = this.EntityId;
    filter.ChildEntityId = this.ChildEntityId;
    filter.DocumentTypeId = this.DataContext.DocumentTypePM.Id;

    this._htmlEditorService.saveEditedReportToServer(filter).subscribe((res: any) => {

        this.CurrentSession.StartBusyIndicator(this.BuildingDocumentText);

        var pmResponse: ServiceResponse = res;
        if (!pmResponse.HasError) {
            var myResult = pmResponse.Result;
            if (myResult) {
                this.docIds = myResult;
                this.idArray = this.docIds.split(',');

                this.AddedDocumentTypeCopyViewModels = new Array<DocumentCopiesViewModel>();
                this.RemovedDocumentTypeCopyViewModels = new Array<DocumentCopiesViewModel>();
                var anySelected = false;

                this.Items.forEach((copy) => {
                    if (!this.DocumentTypeload.IsDocumentOneTimePrintLimited) {

                        if (copy.IsSelected) {
                            anySelected = true;

                            this.AddedDocumentTypeCopyViewModels.push(copy);
                        }

                        if (copy.Exists && !copy.IsSelected) {

                            this.RemovedDocumentTypeCopyViewModels.push(copy);
                        }
                    }

                    else {
                        copy.IsSelected = true;
                        anySelected = true;

                        this.AddedDocumentTypeCopyViewModels.push(copy);
                    }

                });


                if (anySelected) {
                    this.lastCount = this.AddedDocumentTypeCopyViewModels.length;
                    var numberOfCopy = this.AddedDocumentTypeCopyViewModels.filter(d => d.IsSelected).length;
                    if (numberOfCopy > 0) {
                        this.CurrentDocumentOut.XamlDocumentId = this.idArray[1];
                        this.CurrentDocumentOut.IssuedByUserId = SessionInfo.LoggedUserId;
                        this.CurrentDocumentOut.IsChangeIssuedDate = true;
                        this.SaveContext();
                    }
                    else this.StopBusyIndicator();
                }
                else {

                    this.StopBusyIndicator();
                }


            }
            else {

                this.StopBusyIndicator();


            }

        }
        else this.StopBusyIndicator();



    });

}
SaveContext() {


    this._documentOutPMService.putDocumentOut(this.CurrentDocumentOut).subscribe((res: any) => {

        var pmResponse: ServiceResponse = res;
        if (!pmResponse.HasError && pmResponse.Result) {
            var myResult = pmResponse.Result;
            this._documentOutPMService.getSingleDocumentOutPM(this.DataContext.CurrentDocument.Id, this.DataContext.CurrentDocument.Tenant).subscribe((res: any) => {
                this.StopBusyIndicator();
                var pmResponse: ServiceResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {
                        myResult.HasFollowUp = this.DataContext.CurrentDocument.HasFollowUp;
                        this.CurrentDocumentOut = myResult;
                        this.DataContext.CurrentDocument = myResult;
                        if (this.AddedDocumentTypeCopyViewModels != null) {
                            this.AddedDocumentTypeCopyViewModels.forEach((copy) => {
                                copy.RefereshDocumentOutCopies(this.CurrentDocumentOut);
                            });

                            this.DataContext.HasFile = true;
                            if (this.DataContext.Issued != true) {
                                this.DataContext.Issued = true;
                            }
                            this.LastBuildDate = this.CurrentDocumentOut.IssuedDate;
                            this.DataContext.IssuedDate = this.CurrentDocumentOut.IssuedDate;
                            this.DataContext.IssuedByUserName = this.CurrentDocumentOut.IssuedByUserName;
                            ServiceLocator.SendTotangoUserActivity(this.ObjectTableName, this.DataContext.DocumentTypePM.Name + " Built");

                            if (this.DataContext.IsNotFromDocsOutListOpenPrintControl) {
                                this.CurrentSession.FireEvent("RefreshDocumentOutPrint");
                            }

                            this.IsDocumentBuildSucceeded = true;


                            this.PrintAllDocs();
                        }
                    }

                }
                else this.StopBusyIndicator();


            });



        } else this.StopBusyIndicator();




    });

}
SortItemSource() {

    if (this.Items) {
        this.Items = this.Items.sort(d => d.IndexOrder);
    }

}


IsNoTemplateFound: boolean = false;
public CurrentDocumentTypeTemplateList: DocumentTypeTemplateViewModel;

GetTemplates() {

    this.CurrentSession.StartBusyIndicatorLoading();
    this._documentTypeTemplateListExtendedService.getDocumentTypeTemplateListsForDocumentType(this.DataContext.DocumentTypePM.Id, this.DataContext.DocumentTypePM.Tenant).subscribe((res: any) => {
        this.DocumentTypeTemplateLists = new Array<DocumentTypeTemplateViewModel>();

        var pmResponse: ServiceResponse = res;
        if (!pmResponse.HasError) {
            var myResult = pmResponse.Result;
            if (myResult) {
                myResult.filter(d => d.InActive == false).forEach((item) => {
                    if (item.TemplateType == "P") {
                        this.DocumentTypeTemplateLists.push(new DocumentTypeTemplateViewModel(item));
                    }
                });

                if (!this.IsNoTemplateFound && !this.IsQuotationDocument) {
                    if (this.DocumentTypeTemplateLists.length == 0) {
                        this.ShowMessage(TextCodeTranslator.Translate("DocsOut.M.NoTemplatesFound"));
                        this.IsNoTemplateFound = true;
                    }
                    else {
                        if (this.IsNoTemplateDefult) this.ShowMessage("Please select template as default");

                    }
                }

                if (this.DocumentTypeTemplateLists.length > 0) {

                    this.CurrentDocumentOut.DocumentTemplateId
                    var item = this.DocumentTypeTemplateLists.filter(r => r.Id == this.CurrentDocumentOut.DocumentTemplateId)[0];

                    if (item == null) item = this.DocumentTypeTemplateLists.filter(r => r.Id == this.DataContext.DocumentTypePM.DocumentTypeDefaultReportTemplateId)[0];
                    if (item == null) item = this.DocumentTypeTemplateLists[0];

                    this.CurrentDocumentTypeTemplateList = item;


                }
            }

        }

        else {
            this.StopBusyIndicator();
            var messageError: string;
            if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                messageError = pmResponse.ErrorsArray[0];
            }
            this.ShowMessage(messageError);
        }


    });

}
StopBusyIndicator() {
    if(this.LastCount == this.CurrentCount){
        this.CurrentSession.StopBusyIndicator();
    }

}



}

export class convertModelToFormData {      
    public static convert (model: any, form: FormData = null, namespace = ''): FormData {
        let formData = form || new FormData();
        let formKey;

        for (let propertyName in model) {
            if (!model.hasOwnProperty(propertyName) || !model[propertyName]) continue;
            let formKey = namespace ? `${namespace}[${propertyName}]` : propertyName;
            if (model[propertyName] instanceof Date)
                formData.append(formKey, model[propertyName].toISOString());
            else if (model[propertyName] instanceof Array) {
                model[propertyName].forEach((element, index) => {
                    const tempFormKey = `${formKey}[${index}]`;
                    this.convert(element, formData, tempFormKey);
                });
            }
            else if (typeof model[propertyName] === 'object' && !(model[propertyName] instanceof File))
                this.convert(model[propertyName], formData, formKey);
            else
                formData.append(formKey, model[propertyName].toString());
        }
        return formData;
    }
}