declare var System: any;
declare var window: any;
import {EntityArgs} from '../../../../../../Infrastructure/DataContracts/EntityArgs';
import {DocumentTypeList} from '../../../../../../Common/EntityLists/DocumentTypeList';
import {DocumentTypePM} from '../../../../../../Common/EntityPMs/DocumentTypePM';
import {DocumentOutPM} from '../../../../../../Common/EntityPMs/DocumentOutPM';
import {DocumentOutCopyPM} from '../../../../../../Common/EntityPMs/DocumentOutCopyPM';
import {DocumentOutPMService} from '../../../../../../Common/Services/ExtendedPMs/DocumentOutPMService';
import {CommunicationLogPMViewModel} from '../ViewModel/CommunicationLogPMViewModel';
import {AppTool} from '../../../../../../Infrastructure/Tools';
import {ServiceResponse} from '../../../../../../Infrastructure/DataContracts/ServiceResponse';
import {GeneralDocumentFollowUpHelper} from '../../../../../../Infrastructure/Helpers/GeneralDocumentFollowUpHelper';
import {AttachmentsList} from '../Filters/AttachmentsList';


export class DocsOutDataViewModel {

    public AttachmentsLists: AttachmentsList[];
    IsCrm: boolean = false;
    EventTypeCode: string;
    EntityPM: any;
    generalDocumentFollowUpHelper: GeneralDocumentFollowUpHelper;
    IsNotFromDocsOutListOpenPrintControl: boolean = false;
    DocumentTypeId: string;
    DocumentTypeName: string;
    Name: string;
    ChildReference: string
    IssuedByUserName: string
    IssuedDate: Date
    Id: string;
    Exists: boolean;
    TemplateType: string;
    IsSendButtonsVisible: boolean;
    IsBuildViewButtonsVisible: boolean;
    public DocumentOutCopyPM: DocumentOutCopyPM;
    HasFile: boolean;
    public CurrentDocument: DocumentOutPM;
    public DocumentTypeList: DocumentTypeList;
    public VisiblePrintButton: boolean;
    public VisibleSendButton: boolean;
    public documentOutCopyId: string;
    public IsSend: boolean; 
    public EventRefreshName: string;
    Subject: string;
    ToSpecificeEmail: string = "";

    public EntityId: string;
    public DocsOutItemsList: DocsOutDataViewModel[];
    public CommunicationLogPMs : CommunicationLogPMViewModel[];
    public CommunicationLogObsList: CommunicationLogPMViewModel[];
    public SelectedCommunicationLogViewMode: CommunicationLogPMViewModel;
    public  ExportQuotationsToIntegratedSystem: boolean;
    PageRequestSendComponent: string;
    public ModeEditDocument: string;
    public HasTree: boolean = false;
    IsViewTree: boolean = false;
    public ShowFollowUp: boolean;
    public HasFollowUp: boolean;
    public ChildEntityId: string;
    public ChildObjectTableId: string;
    public ChildObjectTableName: string;
    public CurrentObjectTableId: string;
    WindowWidth: number;
    WindowHeight: number;
    public IsAWBWizard: boolean;
    public ScreenEditHeight: number;
    public ScreenEditWidth: number;
    public ModeSendDocument: string;
    public DocsOutTabComponent: any;
    public   IsViewGeneralAttachment: boolean;
    ObjectTableName: string;
    DocumentTypePM: DocumentTypePM;
    DocumentType: any;
    DocumentTypeCode: string = "";
    public CommunicationLogObsListHeight: string = "65px";
    constructor(docType: any, entityId: string, childEntityId: string, currentObjectTableId: string, childObjectTableId: string, childReference: string, internalDocuments: DocumentOutPM[], communicationLogLists: CommunicationLogPMViewModel[], docsOutTabComponent: any, entityPM: any, objectTableName: string = null, documentTypeList: DocumentTypeList = null) {
        this.DocsOutTabComponent = docsOutTabComponent;
        this.DocumentTypePM = docType;
        this.DocumentTypeList = documentTypeList;
        this.EntityPM = entityPM;
        this.EntityId = !AppTool.IsNullOrEmpty(entityId) ? entityId : "";   
        this.ChildEntityId = !AppTool.IsNullOrEmpty(childEntityId) ? childEntityId : ""; 
        this.ChildObjectTableId = !AppTool.IsNullOrEmpty(childObjectTableId) ? childObjectTableId : ""; 
        this.ChildReference = !AppTool.IsNullOrEmpty(childReference) ? childReference : ""; 
        this.CurrentObjectTableId = !AppTool.IsNullOrEmpty(currentObjectTableId) ? currentObjectTableId : ""; 

       
  
        this.DocumentType = this.DocumentTypePM ? this.DocumentTypePM : this.DocumentTypeList;

        
        var table = window.ObjectTables.filter(d => d.Id == this.CurrentObjectTableId)[0];

        if (objectTableName) {
            table = window.ObjectTables.filter(d => d.Name == objectTableName)[0];
        }

        if (table) {
     
            this.ObjectTableName = table.Name;
            this.CurrentObjectTableId = table.Id; 
        }


        if (!AppTool.IsNullOrEmpty(this.ChildObjectTableId)) {
            var table = window.ObjectTables.filter(d => d.Id == this.ChildObjectTableId)[0];
            if (table) this.ChildObjectTableName = table.Name;
           
        }

        this.generalDocumentFollowUpHelper = new GeneralDocumentFollowUpHelper(this.ObjectTableName, this.EntityId, this.ChildEntityId, this.ChildReference, "DocOut", this, entityPM);

        this.CommunicationLogPMs = communicationLogLists;
        if (internalDocuments != null) {

            if (childEntityId) this.CurrentDocument = internalDocuments.filter(d => d.DocumentTypeId == this.DocumentType.Id && d.ChildEntityId == childEntityId)[0];
            else this.CurrentDocument = internalDocuments.filter(d => d.DocumentTypeId == this.DocumentType.Id)[0];
        }

        

        if (this.CurrentDocument != null) {

            if (this.CommunicationLogPMs) {
                this.CommunicationLogObsList = this.CommunicationLogPMs.filter(d => d.CurrentEntityPm.DocumentOutId == this.CurrentDocument.Id);
                if (this.CommunicationLogObsList && this.CommunicationLogObsList.length > 0)
                    this.HasTree = true;
            }
                


            if (this.CurrentDocument.DocumentOutCopies.length > 0 && this.DocumentType.TemplateFormatCode == "P") this.HasFile = true;

            else this.HasFile = false;
   
            this.IssuedByUserName = this.CurrentDocument.IssuedByUserName;
            this.IssuedDate = this.CurrentDocument.IssuedDate;
            this.Exists = true;
        }

        else this.Exists = false;

        if (this.DocumentType) {
            this.Name = this.DocumentType.Name;
            this.Id = this.DocumentType.Id;
            this.DocumentTypeId = this.DocumentType.Id;
            this.DocumentTypeName = this.DocumentType.Name;
            this.TemplateType = this.DocumentType.TemplateFormatCode;
            this.DocumentTypeCode = this.DocumentType.Code;
            
            if (this.DocumentType.TemplateFormatCode == "M") {
                this.IsSendButtonsVisible = true;
                this.IsBuildViewButtonsVisible = false;
            }
            else {
                this.IsSendButtonsVisible = false;
                this.IsBuildViewButtonsVisible = true;
            }
        }
       

      



    }

 
   

    ViewTree() {

        if (this.IsViewTree) this.IsViewTree = false;

        else {
            this.SetCommunicationLogListHeight();
          
            this.IsViewTree = true;
        }

    }

    SetCommunicationLogListHeight() {

        if (this.CommunicationLogObsList && this.CommunicationLogObsList.length == 1) {
            this.CommunicationLogObsListHeight = "65px";
        }
        else if (this.CommunicationLogObsList && this.CommunicationLogObsList.length == 2) {
            this.CommunicationLogObsListHeight = "90px";
        } 
        else if (this.CommunicationLogObsList && this.CommunicationLogObsList.length == 3) {
            this.CommunicationLogObsListHeight = "120px";
        }
        else if (this.CommunicationLogObsList && this.CommunicationLogObsList.length == 4) {
            this.CommunicationLogObsListHeight = "144px";
        }

        else if (this.CommunicationLogObsList && this.CommunicationLogObsList.length == 5 || this.CommunicationLogObsList.length > 5) {
            this.CommunicationLogObsListHeight = "170px";
        }
      
    }


    get InternalDocumentId() {
        if (this.CurrentDocument) {
            return this.CurrentDocument.Id;
        }
        else return null;
    }
    set InternalDocumentId(newValue: string) {
        if (this.CurrentDocument && this.CurrentDocument.Id != newValue) {
            this.CurrentDocument.Id =newValue;

        }
    }
    

    get Issued() {
        if (this.CurrentDocument) {
            return this.CurrentDocument.Issued;
        }
        else return false;
    }
    set Issued(newValue: boolean) {
        if (this.CurrentDocument) {
            this.CurrentDocument.Issued = newValue;
            if (newValue) {
                this.generalDocumentFollowUpHelper.MarkFollowUpAsDone();
            }
        }
    }




    OnSelectedChangeCommunicationLogObsList(communicationLog: CommunicationLogPMViewModel) {
        this.SelectedCommunicationLogViewMode = communicationLog;

    }





    //CreateDocument(propertyName: string, value: any) {

    //    if (this.CurrentDocument == null) {
    //        this.DocsOutTabComponent._documentOutPMService.getCreateDocumentOut(this.Id, this.EntityId, "", this.ChildReference, this.DocsOutTabComponent.ObjectTableId, this.DocsOutTabComponent.SessionInfo.LoggedUserTenant).subscribe(res => {

    //            var pmResponse: ServiceResponse = res;
    //            if (!pmResponse.HasError) {
    //                var myResult = pmResponse.Result;
    //                if (myResult) {
    //                    this.DocsOutTabComponent._documentOutPMService.getSingleDocumentOutPM(myResult.Id, myResult.Tenant).subscribe(res => {

    //                        var pmResponse: ServiceResponse = res;
    //                        if (!pmResponse.HasError) {
    //                            var myResult = pmResponse.Result;
    //                            if (myResult) {
    //                                switch (propertyName) {
    //                                    case "Note":
    //                                        this.CurrentDocument.Notes = value;
    //                                        break;
    //                                }
    //                            }
    //                        }
    //                    });
    //                }
    //            }

    //            });
    //    }
    //    else {
    //        switch (propertyName) {
    //            case "Note":
    //                this.CurrentDocument.Notes = value;
    //                break;
    //        }
         

    //    }

    //}



}