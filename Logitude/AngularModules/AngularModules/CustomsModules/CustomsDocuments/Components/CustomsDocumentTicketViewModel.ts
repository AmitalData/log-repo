import { CustomsDocumentPM } from "../../../Customs/EntityPMs/CustomsDocumentPM";
import { CustomsDocumentsTicketPM } from "../../../Customs/EntityPMs/CustomsDocumentsTicketPM";
import { CustomsDocumentPointerPM } from "../../../Customs/EntityPMs/CustomsDocumentPointerPM";
import { DocumentsFilingPM } from "../../../Common/EntityPMs/DocumentsFilingPM";
import { CustomsDocumentMetaDataValuePM } from '../../../Customs/EntityPMs/CustomsDocumentMetaDataValuePM';
import { CustomDocumentTypeList } from '../../../Customs/EntityLists/CustomDocumentTypeList';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { CustomDocumentTypeMetaDataListService } from '../../../Customs/Services/StandardLists/CustomDocumentTypeMetaDataListService';
import { CustomDocumentTypeListService } from '../../../Customs/Services/StandardLists/CustomDocumentTypeListService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { CustomDocumentTypeMetaDataList } from '../../../Customs/EntityLists/CustomDocumentTypeMetaDataList';
import { AppTool, ArrayTool } from '../../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { RelatedDocumentViewModel } from './RelatedDocumentViewModel';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { CustomsDocumentPMService } from '../../../Customs/Services/StandardPMs/CustomsDocumentPMService';
import { CustomsDocumentsTicketPMService } from '../../../Customs/Services/StandardPMs/CustomsDocumentsTicketPMService';
import { CustomsDocumentsComponent } from './CustomsDocumentsComponent';
import { ICustomsDocumentsController } from './ICustomsDocumentsController';
import { CustomsDocumentsTicketsExtendedService } from '../../../Customs/Services/ExtendedPMs/CustomsDocumentsTicketsExtendedService'
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { CustDocRelatedDocsWebService } from '../../../Customs/Services/WebServices/CustDocRelatedDocsWebService';
import { CustDocMetaDataValuesWebService } from '../../../Customs/Services/WebServices/CustDocMetaDataValuesWebService';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { DeclarationPM } from "../../../Customs/EntityPMs/DeclarationPM";
import { DocumentTypeCustomsDataPMService } from '../../../Customs/Services/StandardPMs/DocumentTypeCustomsDataPMService';
import { CustomDocumentNewVersionService } from "../services/CustomDocumentNewVersion.service";
import { SessionComponent } from "Infrastructure/Components/Session/SessionComponent";
import { OcrDocumentExtendedListService } from "Customs/Services/ExtendedLists/OcrDocumentExtendedListService";
import { OcrDocumentPM } from "Customs/EntityPMs/OcrDocumentPM";
import { OcrDocumentPMService } from "Customs/Services/StandardPMs/OcrDocumentPMService";

export class CustomsDocumentTicketViewModel {

    //****************Properties****************//
    get Id() { return this.customsDocumentsTicketPM.Id; }
    set Id(value: string) {

        if (this.customsDocumentsTicketPM.Id != value) {
            this.customsDocumentsTicketPM.Id = value;
        }
    }

    get DocumentTypeCode() { return this.customsDocumentsTicketPM.DocumentTypeCode; }
    set DocumentTypeCode(value: string) {

        if (this.customsDocumentsTicketPM.DocumentTypeCode != value) {
            this.customsDocumentsTicketPM.DocumentTypeCode = value;
        }
    }

    get CustomsDocId() { return this.customsDocumentsTicketPM.CustomsDocId; }
    set CustomsDocId(value: string) {

        if (this.customsDocumentsTicketPM.CustomsDocId != value) {
            this.customsDocumentsTicketPM.CustomsDocId = value;
        }
    }

    get RequestedDocumentId() { return this.customsDocumentsTicketPM.RequestedCustomsDocId; }
    set RequestedDocumentId(value: string) {

        if (this.customsDocumentsTicketPM.RequestedCustomsDocId != value) {
            this.customsDocumentsTicketPM.RequestedCustomsDocId = value;
        }
    }

    get DocumentsFilingId() { return this.customsDocumentsTicketPM.DocumentsFilingId; }
    set DocumentsFilingId(value: string) {

        if (this.customsDocumentsTicketPM.DocumentsFilingId != value) {
            this.customsDocumentsTicketPM.DocumentsFilingId = value;
        }
    }

    get Extension() { return this.customsDocumentsTicketPM.Extension; }
    set Extension(value: string) {

        if (this.customsDocumentsTicketPM.Extension != value) {
            this.customsDocumentsTicketPM.Extension = value;
        }
    }

    get ExternalAttachmentId() { return this.customsDocumentsTicketPM.ExternalAttachmentId; }
    set ExternalAttachmentId(value: string) {

        if (this.customsDocumentsTicketPM.ExternalAttachmentId != value) {
            this.customsDocumentsTicketPM.ExternalAttachmentId = value;
        }
    }

    private isOcrRelatedDocument: boolean = false
    get IsOcrRelatedDocument() { return this.isOcrRelatedDocument; }
    set IsOcrRelatedDocument(value: boolean) {

        if (this.isOcrRelatedDocument != value) {
            this.isOcrRelatedDocument= value;
        }
    }

   
    public static IsOcrDocument : boolean = false;
    public FromCompanyDocumentType2Add: boolean = false;

    public get CustomDocumentTypeMetaDataLists() { return this.customDocumentTypeMetaDataLists };
    private customDocumentTypeMetaDataLists: CustomDocumentTypeMetaDataList[];
    public MetaDataCount: number;
    public IsMetaDataVisible: boolean;
    public LeadingMetaDataName: string;
    public LeadingMetaDataValue: string;
    public metaDataList: MetaDataValueViewModel[] = [];
    private customsDocumentTypeLists: CustomDocumentTypeList[];
    public DocumentTypeName: string;
    public DocumentStatusName: string;
    public CustomsDocIdLabelText: string;
    public Status1ImageGreen: boolean;
    public Status1ImageGray: boolean;
    public Status2ImageGreen: boolean;
    public Status2ImageGray: boolean;
    public Status2ErrorImage: boolean;
    get ExternalAttachmentIdVisibility() {
        if (this.ExternalAttachmentId) {
            return true;
        }
        else {
            return false;
        }
    }
    DeniedImageVisibility: boolean;
    ApprovedImageVisibility: boolean;
    public get CustomsDocumentMetaDataValuePMs() { return this.customsDocumentMetaDataValuePMs };
    private customsDocumentMetaDataValuePMs: CustomsDocumentMetaDataValuePM[];
    DataContext: CustomsDocumentsComponent;
    PreventEdit: boolean = false;
    _SInvoiceNumber: string = null;
    _IsClassified: boolean = false;
    private EntityResourceService: EntityResourceService;

    private readonly customDocumentNewVersionService: CustomDocumentNewVersionService = new CustomDocumentNewVersionService();
    //*****************************************//
    constructor(
        public customsDocumentsTicketPM: CustomsDocumentsTicketPM, 
        customsDocumentMetaDataValuePMs: CustomsDocumentMetaDataValuePM[], 
        private isNew: boolean, 
        public isDisplayOnly: boolean, 
        public EntityPM: any, 
        private objectTableName: string, 
        private iCustomsDocumentsController: ICustomsDocumentsController, 
        public documentsFilingId: string = null,
        ) {
        this.EntityResourceService = new EntityResourceService();
        if (customsDocumentMetaDataValuePMs != null) {
            this.customsDocumentMetaDataValuePMs = customsDocumentMetaDataValuePMs.filter(d => d.CustomsDocumentId == customsDocumentsTicketPM.DocumentsFilingId);
        }
        var customDocumentTypeListService: CustomDocumentTypeListService = new CustomDocumentTypeListService();
        customDocumentTypeListService.getAllFromCache().subscribe((resp: ServiceResponse) => {
            this.customsDocumentTypeLists = resp.Result;
            var customDocumentType = this.customsDocumentTypeLists.filter(d => d.Code == this.customsDocumentsTicketPM.DocumentTypeCode)[0];
            this.DocumentTypeName = customDocumentType.LocalName;
        });
        this.DocumentStatusName = this.customsDocumentsTicketPM.DocumentStatusName;
        this.CustomsDocIdLabelText = this.GetCustomsDocIdLabelText();
        this.SetStatusImages();
        this.SetApprovedDeniedImages();

        if (!this.isNew) {
            this.SetCustomDocumentMetaData();
        }
        debugger
        if (customsDocumentsTicketPM.DocumentTypeCode == "380" || (customsDocumentsTicketPM.DocumentTypeCode == "325" && EntityPM?.Direction =="E")) {

            var dec: DeclarationPM = EntityPM as DeclarationPM;
            if (dec) {
                if(!AppTool.IsNullOrEmpty(this.customsDocumentsTicketPM.ConnectedInvoicesSequences)){
                    var ary = this.customsDocumentsTicketPM.ConnectedInvoicesSequences.split(",");
                    var firstSeq = ary[0];
                    var sp = dec.SupplierInvoices.filter(r => !AppTool.IsNullOrEmpty(r.SequenceNumeric) && r.SequenceNumeric.toString() == firstSeq)[0];
                    if (sp) {
                        this._SInvoiceNumber = sp.InvoiceNumber;
                        if (ary.length > 1) {
                            this._SInvoiceNumber = this._SInvoiceNumber + "...";
                        }
                    }
                }
                
                    if ((!AppTool.IsNullOrEmpty(customsDocumentsTicketPM.DocumentsFilingId) || !AppTool.IsNullOrEmpty(this.documentsFilingId)) && customsDocumentsTicketPM.DocumentTypeCode == "380") {
                        var custDocRelatedDocsWebService: CustDocRelatedDocsWebService = new CustDocRelatedDocsWebService();
                        var checkOcr = dec.Direction == 'E' ? true : false
                        var docFilingId = !AppTool.IsNullOrEmpty(customsDocumentsTicketPM.DocumentsFilingId) ? customsDocumentsTicketPM.DocumentsFilingId : this.documentsFilingId
                        custDocRelatedDocsWebService.GetSingleDocumentsFilingPM(docFilingId, checkOcr).
                            subscribe((resp: ServiceResponse) => {
                                var documentFiling: DocumentsFilingPM = resp.Result;
                                if (documentFiling.DocumentTypeCode == "CLSI") {
                                    this._IsClassified = true;
                                }
                                if(!AppTool.IsNullOrEmpty(documentFiling.OcrReference)){
                                    this._SInvoiceNumber = documentFiling.OcrReference;
                                }

                            });
                    


                }

            }

        }
        this.EntityResourceService.getEntityResourceByTableName("Customs.Claim").subscribe((response: any) => {
            CustomsDocumentTicketViewModel.Customs_Claim_TH_CustomAnswer = TextCodeTranslator.Translate("Customs.Claim.TH.CustomAnswer");
        });
        this.EntityResourceService.getEntityResourceByTableName("Customs.OcrDocument").subscribe((response: any) => {
        });
    }
    public ListOfStatusCode2Show: string[] = ["1", "2"];
    public get HaveCustomAnswer(): boolean {
        if (AppTool.IsNullOrEmpty(this.customsDocumentsTicketPM.DocumentStatusCode) ||
            AppTool.IsNullOrEmpty(this.customsDocumentsTicketPM.DocumentsFilingId) ||
            //entityPm.DocumentStatusCode!= "1" 
            this.ListOfStatusCode2Show.indexOf(this.customsDocumentsTicketPM.DocumentStatusCode) == -1
        ) {

            return false;
        }
        return true;
    }

    static Customs_Claim_TH_CustomAnswer: string;
    public get CustomAnswerTitle() {
        if (this.HaveCustomAnswer) {
            return CustomsDocumentTicketViewModel.Customs_Claim_TH_CustomAnswer;//
        } else {
            return TextCodeTranslator.Translate("General.B.Edit");;

        }
    }

    public SetCustomDocumentMetaData(metaData: { [Code: string]: any; } = {} = null) {
        var customDocumentTypeMetaDataListService: CustomDocumentTypeMetaDataListService = new CustomDocumentTypeMetaDataListService();
        this.EntityResourceService.getEntityResourceByTableName("Customs.CustomDocumentTypeMetaData").subscribe((response: any) => {

            customDocumentTypeMetaDataListService.getAllFromCache().subscribe((res: ServiceResponse) => {
                this.customDocumentTypeMetaDataLists = res.Result;
                if (metaData == null) {
                    this.SetCustomDocumentMetaDataFromAll();
                }
                else {
                    this.SetCustomDocumentMetaDataFromDictionary(metaData);
                }
            });
        });
    }

    private SetCustomDocumentMetaDataFromDictionary(metaData: { [Code: string]: any; } = {}) {
        
        this.metaDataList = [];
        var keys: string[] = Object.keys(metaData);
        keys.forEach((key) => {
            var my = this.customDocumentTypeMetaDataLists.filter(d => d.MetaDataTypeCode == key)[0];
            var viewmodel: MetaDataValueViewModel = new MetaDataValueViewModel();
            viewmodel.MetaDataValue = metaData[key] != null ? metaData[key] + "" : null;
            viewmodel.Tenant = SessionLocator.Tenant;
            viewmodel.MetaDataTypeCode = key;
            if (my != null) {
                viewmodel.IsLeading = my.IsLeading;
                viewmodel.MetaDataTypeName = my.MetaDataTypeName;
            }
            this.metaDataList.push(viewmodel);
        });
        var leading: MetaDataValueViewModel = this.metaDataList.filter(d => d.IsLeading)[0];
        if (leading) {
            this.LeadingMetaDataValue = leading.MetaDataValue == "True" ? "כן" : (leading.MetaDataValue == "False" ? "לא" : leading.MetaDataValue);
            this.LeadingMetaDataName = leading.MetaDataTypeName;
        }
        else if (this.customDocumentTypeMetaDataLists != null) {
            {
                var types: CustomDocumentTypeMetaDataList[] = this.customDocumentTypeMetaDataLists.filter(d => d.DocumentTypeCode == this.customsDocumentsTicketPM.DocumentTypeCode).sort((a, b) => { return (a.MetaDataTypeCode === b.MetaDataTypeCode) ? 0 : (a.MetaDataTypeCode < b.MetaDataTypeCode) ? -1 : 1 });;
                if (this.EntityPM.Direction == "E" && this.customsDocumentsTicketPM.DocumentTypeCode == "707" && this.EntityPM.transportModeId=='A') {
                    var leadingType: CustomDocumentTypeMetaDataList = types.filter(d => d.MetaDataTypeCode == "101" && d.DocumentTypeCode == this.customsDocumentsTicketPM.DocumentTypeCode)[0];
                } else {
                    var leadingType: CustomDocumentTypeMetaDataList = types.filter(d => d.Mandatory && d.DocumentTypeCode == this.customsDocumentsTicketPM.DocumentTypeCode)[0];
                }
                if (leadingType) {
                    var leadingValue: MetaDataValueViewModel = this.metaDataList.filter(d => d.MetaDataTypeCode == leadingType.MetaDataTypeCode)[0];
                    this.LeadingMetaDataValue = leadingValue != null ? leadingValue.MetaDataValue : null;
                    this.LeadingMetaDataName = leadingType.MetaDataTypeName;
                }

            }
        }
    }

    private SetCustomDocumentMetaDataFromAll() {

        this.customDocumentTypeMetaDataLists = this.customDocumentTypeMetaDataLists.filter(d => d.DocumentTypeCode === this.customsDocumentsTicketPM.DocumentTypeCode);
        if (this.customDocumentTypeMetaDataLists) {
            var requiredMetaDatas: CustomDocumentTypeMetaDataList[] = this.customDocumentTypeMetaDataLists.filter(d => d.Mandatory && d.DocumentTypeCode === this.customsDocumentsTicketPM.DocumentTypeCode);
            var counter: number = 0;
            if (this.customsDocumentMetaDataValuePMs == null) {
                counter = requiredMetaDatas.length;
            }
            else {
                requiredMetaDatas.forEach((item) => {
                    var value: CustomsDocumentMetaDataValuePM = this.customsDocumentMetaDataValuePMs.filter(d => d.MetaDataTypeCode == item.MetaDataTypeCode)[0];
                    if (value && AppTool.IsNullOrEmpty(value.MetaDataValue)) {
                        counter++;
                    }
                    else if (value == null || value == undefined) {
                        counter++;
                    }
                });

            }
            this.MetaDataCount = counter;
            if (this.MetaDataCount > 0) {
                this.IsMetaDataVisible = true;
            }
            else {
                this.IsMetaDataVisible = false;
            }
        }

        if (this.customsDocumentMetaDataValuePMs && this.customDocumentTypeMetaDataLists) {
            var leading: CustomDocumentTypeMetaDataList = this.customDocumentTypeMetaDataLists.filter(d => d.IsLeading && d.DocumentTypeCode == this.customsDocumentsTicketPM.DocumentTypeCode)[0];
            if (leading) {
                var leadingValue: CustomsDocumentMetaDataValuePM = this.customsDocumentMetaDataValuePMs.filter(d => d.MetaDataTypeCode == leading.MetaDataTypeCode)[0];
                this.LeadingMetaDataValue = leadingValue != null ? (leadingValue.MetaDataValue == "True" ? "כן" : (leadingValue.MetaDataValue == "False" ? "לא" : leadingValue.MetaDataValue)) : null;
                this.LeadingMetaDataName = leading.MetaDataTypeName;
            }
            else if (this.customDocumentTypeMetaDataLists != null) {
                var types: CustomDocumentTypeMetaDataList[] = this.customDocumentTypeMetaDataLists.filter(d => d.DocumentTypeCode == this.customsDocumentsTicketPM.DocumentTypeCode).sort((a, b) => { return (a.MetaDataTypeCode === b.MetaDataTypeCode) ? 0 : (a.MetaDataTypeCode < b.MetaDataTypeCode) ? -1 : 1 });;
                // var leadingType: CustomDocumentTypeMetaDataList = types.filter(d => d.Mandatory && d.DocumentTypeCode == this.customsDocumentsTicketPM.DocumentTypeCode)[0];
                leading = types.filter(d => d.Mandatory && d.DocumentTypeCode == this.customsDocumentsTicketPM.DocumentTypeCode)[0];
                if (leading != null) {
                    var leadingValue: CustomsDocumentMetaDataValuePM = this.customsDocumentMetaDataValuePMs.filter(d => d.MetaDataTypeCode == leading.MetaDataTypeCode)[0];
                    this.LeadingMetaDataValue = leadingValue != null ? (leadingValue.MetaDataValue == "True" ? "כן" : (leadingValue.MetaDataValue == "False" ? "לא" : leadingValue.MetaDataValue)) : null;
                    this.LeadingMetaDataName = leading.MetaDataTypeName;
                }
            }
        }



    }

    GetStatusFontColor() {
        var customsDocIdForeground = null;

        if (this.customsDocumentsTicketPM.VerificationStatusTypeCode == "8") {
            return "#F78232";
        }

        if (this.customsDocumentsTicketPM.DocumentStatusCode == "1") {
            customsDocIdForeground = "#018057";
        }
        else if (this.customsDocumentsTicketPM.DocumentStatusCode == "2") {
            customsDocIdForeground = "red";
        }
        else if (AppTool.IsNullOrEmpty(this.customsDocumentsTicketPM.DocumentStatusCode)) {
            customsDocIdForeground = "#881C1D";
        }
        else if (this.customsDocumentsTicketPM.DocumentStatusCode == "7" || this.customsDocumentsTicketPM.DocumentStatusCode == "8") {
            customsDocIdForeground = "#F78232";
        }
        else {
            customsDocIdForeground = "#312C31";
        }
        return customsDocIdForeground;

    }

    GetCustomsDocIdLabelText() {

        var customsDocIdLabelText = null;
        if (AppTool.IsNullOrEmpty(this.customsDocumentsTicketPM.DocumentStatusCode)) {// this will cause a problem in statuses. || (AppTool.IsNullOrEmpty(this.customsDocumentsTicketPM.VerificationStatusTypeCode) && this.customsDocumentsTicketPM.RequestedCustomsDocId)) { //WI 35024
            this.DocumentStatusName = TextCodeTranslator.Translate("Customs.CustomsDocuments.CustomsDocNotSentYet");
            customsDocIdLabelText = null;//TextCodeTranslator.Translate("Customs.CustomsDocuments.CustomsDocNotSentYet");
        }
        else if (this.customsDocumentsTicketPM.VerificationStatusTypeCode == "8") {//WI 35024
            this.DocumentStatusName = TextCodeTranslator.Translate("Customs.CustomsDocuments.CustomsDocInVerificationProgress");
            customsDocIdLabelText = null;
        }
        else if (!AppTool.IsNullOrEmpty(this.customsDocumentsTicketPM.VerificationStatusTypeCode)) {
            // already filled in the init
            customsDocIdLabelText = null;
        }
        else if (this.customsDocumentsTicketPM.DocumentStatusCode == "7") {
            this.DocumentStatusName = TextCodeTranslator.Translate("Customs.CustomsDocuments.CustomsDocSendInProgress");
            customsDocIdLabelText = null;//TextCodeTranslator.Translate("Customs.CustomsDocuments.CustomsDocSendInProgress");
        }


        else {
            customsDocIdLabelText = TextCodeTranslator.Translate("Customs.CustomsDocuments.CustomsDocIdLabel");
        }
        return customsDocIdLabelText;
    }

    SetStatusImages() {
        if (this.customsDocumentsTicketPM.DocumentStatusCode == "1" || this.customsDocumentsTicketPM.DocumentStatusCode == "2" || this.customsDocumentsTicketPM.DocumentStatusCode == "7") {
            this.Status1ImageGreen = true;
            this.Status1ImageGray = false;
        }
        else {
            this.Status1ImageGreen = false;
            this.Status1ImageGray = true;
        }

        if (this.customsDocumentsTicketPM.DocumentStatusCode == "2") {
            this.Status2ImageGreen = false;
            this.Status2ImageGray = false;
            this.Status2ErrorImage = true;
        }
        else if (this.customsDocumentsTicketPM.DocumentStatusCode == "1") {
            this.Status2ImageGreen = true;
            this.Status2ImageGray = false;
            this.Status2ErrorImage = false;
        }
        else {
            this.Status2ImageGreen = false;
            this.Status2ImageGray = true;
            this.Status2ErrorImage = false;
        }
    }

    SetApprovedDeniedImages() {

        if (this.customsDocumentsTicketPM.VerificationStatusTypeCode == "4" || this.customsDocumentsTicketPM.VerificationStatusTypeCode == "5") {
            //approvedDeniedImageSource = "/Images/icons/ApprovedDocument.png";
            this.DeniedImageVisibility = false;
            this.ApprovedImageVisibility = true;
        }
        else if (this.customsDocumentsTicketPM.VerificationStatusTypeCode == "6") {
            // approvedDeniedImageSource = "/Images/icons/DeniedStamp.png";
            this.DeniedImageVisibility = true;
            this.ApprovedImageVisibility = false;
        }
        else {
            this.DeniedImageVisibility = false;
            this.ApprovedImageVisibility = false;
        }
    }

    allowDrop(event: DragEvent) {
        event.preventDefault();

        var documentFilingPMId = event.dataTransfer.getData("Id");
        //if (!documentFilingPMId) {
        //    event.dataTransfer.effectAllowed = "none";
        //    event.dataTransfer.dropEffect = "none";
        //}

    }

    ConnectDocumentToTicket(event: DragEvent, RelatedDocuments: RelatedDocumentViewModel[], dataContext: CustomsDocumentsComponent, isExport: boolean) {
       
        if (SessionLocator.SelectedSession.CurrentEditComponent) {
            if (SessionLocator.SelectedSession.CurrentEditComponent.EntityPM.IsDirty) {
                SessionLocator.SelectedSession.CurrentEditComponent.SaveChanges();
            }
        }
        if (this.isDisplayOnly && !this.customsDocumentsTicketPM.RequestedCustomsDocId) {
            var messageWindow = new MessageWindow();
            messageWindow.Width = 400;
            messageWindow.Height = 200;
            messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
            if(dataContext?.DisplayOnlyMessage?.includes(TextCodeTranslator.Translate("Customs.OcrDocument.O.OpenOcrInvoice")))
            {
                messageWindow.RTL = true;
                messageWindow.Show(TextCodeTranslator.Translate("Customs.OcrDocument.O.DCAOCRInPrograss"));
            }
            else{
                messageWindow.Show("ההצהרה כבר הוגשה - לא ניתן לקשר מסמכים חדשים");
            }
            messageWindow.WindowClosed.subscribe((event: any) => {

                messageWindow.Close();

            });
            return;
        }
        this.DataContext = dataContext;
        var documentFilingPMId = event.dataTransfer.getData("Id");
        var _DocumentTypeCustomsDataPMService: DocumentTypeCustomsDataPMService = new DocumentTypeCustomsDataPMService();

        if (!AppTool.IsNullOrEmpty(documentFilingPMId)) {
            var relatedDocumentViewModel: RelatedDocumentViewModel = RelatedDocuments.filter(d => d.Id == documentFilingPMId)[0];
            if (!this.isDisplayOnly || !AppTool.IsNullOrEmpty(this.customsDocumentsTicketPM.RequestedCustomsDocId)) {
                if (AppTool.IsNullOrEmpty(this.customsDocumentsTicketPM.DocumentsFilingId)) {
                    SessionLocator.SelectedSession.StartBusyIndicator(TextCodeTranslator.Translate("Customs.General.O.Saving"));
                    if (relatedDocumentViewModel.CustomDocument == null) {
                        var customsDocumentPMService: CustomsDocumentPMService = new CustomsDocumentPMService();
                        var docId = encodeURIComponent(relatedDocumentViewModel.Id);
                        customsDocumentPMService.get(docId).subscribe((response: ServiceResponse) => {
                            _DocumentTypeCustomsDataPMService.get(relatedDocumentViewModel.DocumentTypeCode).subscribe(res => {

                                relatedDocumentViewModel.CustomDocument = response.Result;
                                if (relatedDocumentViewModel.CustomDocument) {
                                    this.StartCustomsDocumentMetaDataCheck(relatedDocumentViewModel);
                                }
                                else {
                                    var customsDocumentPM: CustomsDocumentPM = new CustomsDocumentPM();
                                    customsDocumentPM.DocumentsFilingId = relatedDocumentViewModel.Id;
                                    customsDocumentPM.Tenant = SessionLocator.Tenant;
                                    isExport = relatedDocumentViewModel.DocumentCategoryCode == 'E' ? true : false;
                                    //if (isExport && AppTool.IsNullOrEmpty(this.customsDocumentsTicketPM.RequestedCustomsDocId)) {
                                    //    if (!AppTool.IsNullOrEmpty(res.Result)) { this.EntityPM
                                    //        customsDocumentPM.DocumentTypeCode = res.Result.CustomsDoucumentTypeCode;// relatedDocumentViewModel.DocumentTypeCode;
                                    //        this.customsDocumentsTicketPM.DocumentTypeCode = res.Result.CustomsDoucumentTypeCode;// relatedDocumentViewModel.DocumentTypeCode;

                                    //    }
                                    //    else {
                                    //        customsDocumentPM.DocumentTypeCode = this.customsDocumentsTicketPM.DocumentTypeCode;

                                    //    }
                                    //}
                                    //else {
                                    customsDocumentPM.DocumentTypeCode = this.customsDocumentsTicketPM.DocumentTypeCode;

                                    //}
                                    customsDocumentPM.DeclarationId = this.EntityPM.Id;
                                    customsDocumentPM.IsPartOfDeclaration = true;
                                    relatedDocumentViewModel.CustomDocument = customsDocumentPM;
                                    customsDocumentPMService.insert(customsDocumentPM).subscribe((resp: ServiceResponse) => {
                                        if (!resp.HasError) {
                                            this.StartCustomsDocumentMetaDataCheck(relatedDocumentViewModel);
                                            //if (isExport) {
                                            //    var customsDocumentsTicketPMService: CustomsDocumentsTicketPMService = new CustomsDocumentsTicketPMService();

                                            //    customsDocumentsTicketPMService.update(this.customsDocumentsTicketPM).subscribe();
                                            //}
                                        }
                                        else {
                                            SessionLocator.SelectedSession.StopBusyIndicator();
                                            if (SessionLocator.SelectedSession.CurrentEditComponent) {
                                                SessionLocator.SelectedSession.CurrentEditComponent.ValidationErrorsList = resp.ErrorsArray;
                                            }
                                            else {
                                                var messageWindow = new MessageWindow();
                                                messageWindow.Width = 400;
                                                messageWindow.Height = 200;
                                                messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                                                if (resp.ErrorsArray && resp.ErrorsArray.length > 0) {
                                                    messageWindow.Show(resp.ErrorsArray[0]);
                                                }
                                                else {
                                                    messageWindow.Show("Server Error");
                                                }
                                                messageWindow.WindowClosed.subscribe((event: any) => {

                                                    messageWindow.Close();

                                                });
                                            }
                                        }
                                        //SessionLocator.SelectedSession.StartBusyIndicatorSaving();
                                        //                                SessionLocator.SelectedSession.StopBusyIndicator();

                                    });
                                }
                            }
                            );


                        });
                    }
                    else if (relatedDocumentViewModel.CustomDocument != null) {
                        this.StartCustomsDocumentMetaDataCheck(relatedDocumentViewModel);

                    }

                }
            }
        }

    }
   async StartCustomsDocumentMetaDataCheck(relatedDocumentViewModel: RelatedDocumentViewModel) {
        CustomsDocumentTicketViewModel.IsOcrDocument = false;
        const isConnectTicket: boolean = await this.GetDocConnectTicket(relatedDocumentViewModel.CustomDocument.DocumentsFilingId)

        if (relatedDocumentViewModel.CustomDocument.DocumentTypeCode != this.customsDocumentsTicketPM.DocumentTypeCode&&isConnectTicket){
            SessionLocator.SelectedSession.StopBusyIndicator();
        var messageWindow = new MessageWindow();
        messageWindow.RTL=true;
        messageWindow.ShowWarningIcon=true;
        messageWindow.Width = 400;
        messageWindow.Height = 200;
        messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
        messageWindow.Show(".מסמך זה מקושר לסוג מסמך אחר בהצהרה אחרת. לא ניתן לקשר");
        messageWindow.WindowClosed.subscribe((event: any) => messageWindow.Close());
           return;
        }

        var isDifferentData = false;
        relatedDocumentViewModel.CustomDocument.CustomsDocumentMetaDataValues.forEach((metaDataValue) => {
            var metaDataViewModel = this.metaDataList.filter(d => d.MetaDataTypeCode == metaDataValue.MetaDataTypeCode)[0];
            if (metaDataViewModel != null) {
                if (metaDataViewModel.MetaDataValue != null && metaDataValue.MetaDataValue != null) {
                    if (metaDataViewModel.MetaDataValue != metaDataValue.MetaDataValue) {
                        isDifferentData = true;
                    }
                }
            }
        });

        if(!AppTool.IsNullOrEmpty(relatedDocumentViewModel.CustomDocument.CustomsDocId) && !AppTool.IsNullOrEmpty(this.customsDocumentsTicketPM.RequestedCustomsDocId) && relatedDocumentViewModel.CustomDocument.CustomsDocId != this.customsDocumentsTicketPM.RequestedCustomsDocId){
            SessionLocator.SelectedSession.StopBusyIndicator();
            var messageWindow = new MessageWindow();
            messageWindow.Width = 400;
            messageWindow.Height = 200;
            messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
            messageWindow.Show(TextCodeTranslator.Translate("Customs.Declaration.O.DocumentAndDocumentWithDifferentCustomsReference"));
            messageWindow.WindowClosed.subscribe((event: any) => {

                messageWindow.Close();

            });
        }
        else {
            SessionLocator.SelectedSession.StopBusyIndicator();

            if (isDifferentData && this.customsDocumentsTicketPM.CustomsDocumentPointers.length == 1) {
               // SessionLocator.SelectedSession.StopBusyIndicator();
                var confirmWindow = new ConfirmWindow();
                confirmWindow.Width = 400;
                confirmWindow.Height = 200;
                confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                confirmWindow.ShowNoButton = true;
                confirmWindow.NoButtonText = TextCodeTranslator.Translate("Customs.Declaration.O.No");
                confirmWindow.Show(TextCodeTranslator.Translate("Customs.CustomsDocuments.MetaDataDifference"));
                confirmWindow.WindowClosed.subscribe((event: any) => {
                    if (confirmWindow.Yes) {
                        this.ProcessConnectDocument(relatedDocumentViewModel);
                        confirmWindow.Close();
                    }
                    if (confirmWindow.No) {
                        confirmWindow.Close();
                    }
    
                });
            }
            else {

                var ocrStatuses = ['1','3','7','8','9'];

                if(ocrStatuses.includes(relatedDocumentViewModel.documentsFilingPM?.OcrStatusCode) || relatedDocumentViewModel.documentsFilingPM?.OcrNotConnect)
                {
                    SessionLocator.SelectedSession.StopBusyIndicator();
                    var status: string = null;
                    switch (relatedDocumentViewModel.documentsFilingPM?.OcrStatusCode) 
                    {
                        case "1":
                        case "3":
                          status = TextCodeTranslator.Translate("Customs.OcrDocument.O.InPrograss");
                          break;
                        case "7":
                          status = TextCodeTranslator.Translate("Customs.OcrDocument.O.Cancelled");
                          break;
                        case "8":
                          status = TextCodeTranslator.Translate("Customs.OcrDocument.O.Rejected");
                          break;
                        case "9":
                          status = TextCodeTranslator.Translate("Customs.General.O.Fail");
                          break;
                    }
                    
                    var confirmWindow = new ConfirmWindow();
                    confirmWindow.ShowNoButton = true;
                    var ShowMessage = "";
                    if(relatedDocumentViewModel.documentsFilingPM?.OcrNotConnect)
                        ShowMessage = TextCodeTranslator.Translate("Customs.OcrDocument.O.ConnectToDec") + " ,\n";
                    if(!AppTool.IsNullOrEmpty(status))
                        ShowMessage += TextCodeTranslator.Translate("Customs.OcrDocument.O.OcrStatus") + " " + status + " ,\n"
                    ShowMessage += TextCodeTranslator.Translate("Customs.OcrDocument.O.ContinueAnyway")
                    confirmWindow.Show(ShowMessage);
                    confirmWindow.WindowClosed.subscribe((event: any) => {
                    if (confirmWindow.Yes) {
                        CustomsDocumentTicketViewModel.IsOcrDocument = true;
                        this.ProcessConnectDocument(relatedDocumentViewModel);
                        confirmWindow.Close();
                    }
                    if (confirmWindow.No) {
                        confirmWindow.Close();
                        return;
                    }
    
                });

                }


                else if(relatedDocumentViewModel.documentsFilingPM?.OcrStatusCode == "2" || relatedDocumentViewModel.documentsFilingPM?.OcrStatusCode == "4" )
                {
                    CustomsDocumentTicketViewModel.IsOcrDocument = true;
                    this.ProcessConnectDocument(relatedDocumentViewModel);
                    
                }
                else{
                    this.ProcessConnectDocument(relatedDocumentViewModel);
                }
                
                

        
               
            }
        }

       
    }

    private SaveGeneratedPointer(relatedDocumentViewModel: RelatedDocumentViewModel = null) {
        var customsDocumentsTicketPMService: CustomsDocumentsTicketPMService = new CustomsDocumentsTicketPMService();
        customsDocumentsTicketPMService.insert(this.customsDocumentsTicketPM).subscribe((myResp: ServiceResponse) => {
            if (!myResp.HasError) {
                this.SetSavedMetaData(relatedDocumentViewModel);
                this.isNew = false;
            }
            else {
                SessionLocator.SelectedSession.StopBusyIndicator();
                if (SessionLocator.SelectedSession.CurrentEditComponent) {
                    SessionLocator.SelectedSession.CurrentEditComponent.ValidationErrorsList = myResp.ErrorsArray;
                }
                else {
                    var messageWindow = new MessageWindow();
                    messageWindow.Width = 400;
                    messageWindow.Height = 200;
                    messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                    if (myResp.ErrorsArray && myResp.ErrorsArray.length > 0) {
                        messageWindow.Show(myResp.ErrorsArray[0]);
                    }
                    else {
                        messageWindow.Show("Server Error");
                    }
                    messageWindow.WindowClosed.subscribe((event: any) => {

                        messageWindow.Close();

                    });
                }
            }
        });
    }

    private SetSavedMetaData(relatedDocumentViewModel: RelatedDocumentViewModel) {
        var documentsFilingId = encodeURIComponent(relatedDocumentViewModel.Id);
        var customsDocumentPMService: CustomsDocumentPMService = new CustomsDocumentPMService();
        if (relatedDocumentViewModel.CustomDocument == null) {
            customsDocumentPMService.get(documentsFilingId).subscribe((response: ServiceResponse) => {
                relatedDocumentViewModel.CustomDocument = response.Result;
                this.ApplySaveMetaData(relatedDocumentViewModel);
            });
        }
        else {
            this.ApplySaveMetaData(relatedDocumentViewModel);
        }
    }

    ApplySaveMetaData(relatedDocumentViewModel: RelatedDocumentViewModel) {
        this.metaDataList.forEach((value) => {
            var metadata: CustomsDocumentMetaDataValuePM = null;
            if (this.customsDocumentMetaDataValuePMs != null) {
                metadata = this.customsDocumentMetaDataValuePMs.filter(d => d.MetaDataTypeCode == value.MetaDataTypeCode)[0];
            }
            var metadataValue: string = null;
            if (metadata != null) {
                metadataValue = metadata.MetaDataValue;
                var editedValue: CustomsDocumentMetaDataValuePM = relatedDocumentViewModel.CustomDocument.CustomsDocumentMetaDataValues.filter(d => d.MetaDataTypeCode == metadata.MetaDataTypeCode)[0];
                if (editedValue != null) {
                    editedValue.MetaDataValue = metadata.MetaDataValue != null ? metadata.MetaDataValue : value.MetaDataValue;
                }
            }
            else {
                var newValue: CustomsDocumentMetaDataValuePM = new CustomsDocumentMetaDataValuePM(relatedDocumentViewModel.CustomDocument);
                newValue.MetaDataTypeCode = value.MetaDataTypeCode;
                newValue.CustomsDocumentId = relatedDocumentViewModel.Id;
                newValue.MetaDataValue = metadataValue != null ? metadataValue : value.MetaDataValue;
                newValue.Tenant = SessionLocator.Tenant;
                relatedDocumentViewModel.CustomDocument.AddCustomsDocumentMetaDataValue(newValue);

            }
        });
        var customsDocumentPMService: CustomsDocumentPMService = new CustomsDocumentPMService();
        customsDocumentPMService.update(relatedDocumentViewModel.CustomDocument).subscribe((response: ServiceResponse) => {
            SessionLocator.SelectedSession.StopBusyIndicator();

            if (response.HasError) {

                if (SessionLocator.SelectedSession.CurrentEditComponent) {
                    SessionLocator.SelectedSession.CurrentEditComponent.ValidationErrorsList = response.ErrorsArray;
                }
                else {
                    var messageWindow = new MessageWindow();
                    messageWindow.Width = 400;
                    messageWindow.Height = 200;
                    messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                    if (response.ErrorsArray && response.ErrorsArray.length > 0) {
                        messageWindow.Show(response.ErrorsArray[0]);
                    }
                    else {
                        messageWindow.Show("Server Error");
                    }
                    messageWindow.WindowClosed.subscribe((event: any) => {

                        messageWindow.Close();

                    });
                }
            }
            else {
                this.DataContext.SelectedDocumentId = this.Id;
                this.DataContext.RefreshEntity();
                //if (SessionLocator.SelectedSession.CurrentEditComponent) {
                //    SessionLocator.SelectedSession.CurrentEditComponent.ReloadEntityPM();
                //}
                //this.DataContext.RefreshButtonClicked(this.Id);
                //this.DataContext.EditCustomsDocumentsTicket(this);
            }
        });
    }

    GeneratecustomsDocumentMetaDataValues() {
        debugger
        if (this.customsDocumentsTicketPM.DocumentTypeCode == "IL_140") {
            var GenerateMetaData18 = true;
            for (var document of this.customsDocumentMetaDataValuePMs) {
                if (document.MetaDataTypeCode == "18") {
                    GenerateMetaData18 = false;
                }
            }
            if (GenerateMetaData18 && !AppTool.IsNullOrEmpty(this.EntityPM.ImporterCode)) {
                var item = new CustomsDocumentMetaDataValuePM(this.customsDocumentsTicketPM);
                item.MetaDataTypeCode = "18";
                item.Tenant = this.EntityPM.Tenant;
                item.MetaDataValue = this.EntityPM.ImporterCode;
                this.customsDocumentMetaDataValuePMs.push(item);
            }
        }
        var SupplierInvoicesTypeCodes = ["380", "325", "326"];
        if (SupplierInvoicesTypeCodes.includes(this.customsDocumentsTicketPM.DocumentTypeCode)) {
            var GenerateMetaData39 = true;
            for (var document of this.customsDocumentMetaDataValuePMs) {
                if (document.MetaDataTypeCode == "39") {
                    GenerateMetaData39 = false;
                }
            }
            if (this.EntityPM.SupplierInvoices[0] != null) {
                //if (GenerateMetaData39 && !AppTool.IsNullOrEmpty(this.EntityPM.SupplierInvoices[0].InvoiceNumber)) {
                if (GenerateMetaData39 && !AppTool.IsNullOrEmpty(this._SInvoiceNumber)) {
                
                    var item = new CustomsDocumentMetaDataValuePM(this.customsDocumentsTicketPM);
                    item.MetaDataTypeCode = "39";
                    item.Tenant = this.EntityPM.Tenant;
                    //item.MetaDataValue = this.EntityPM.SupplierInvoices[0].InvoiceNumber;
                    item.MetaDataValue = this._SInvoiceNumber;
                    this.customsDocumentMetaDataValuePMs.push(item);
                }
            }
        }
    }


    async ProcessConnectDocument(relatedDocumentViewModel: RelatedDocumentViewModel) {   
        if (relatedDocumentViewModel == null || await this.checkFileBiggerFrom200MB(relatedDocumentViewModel)) return;

        if (relatedDocumentViewModel.CustomDocument.DocumentTypeCode == null)
            relatedDocumentViewModel.CustomDocument.DocumentTypeCode = this.customsDocumentsTicketPM.DocumentTypeCode;

        if (relatedDocumentViewModel.CustomDocument.DocumentTypeCode == this.customsDocumentsTicketPM.DocumentTypeCode)
            this.connectDocument(relatedDocumentViewModel);

        else {
            if (!relatedDocumentViewModel.CustomDocument.CustomsDocId) {//םין סימוכין
                relatedDocumentViewModel.CustomDocument.DocumentTypeCode = this.customsDocumentsTicketPM.DocumentTypeCode;
                this.connectDocument(relatedDocumentViewModel);

            } 
            else { 
                const isNotConnect: boolean = await this.GetIsConnectDec(relatedDocumentViewModel.CustomDocument.DocumentsFilingId)
                if (isNotConnect) {// יש סימוכין ולם מקושר כבר להצהרה םו טיקט םחרת
                SessionLocator.SelectedSession.StopBusyIndicator();
                const accept: boolean = await this.confirmConnectionDiffrentDocTypeMsg();
                
                if (accept) {
                    relatedDocumentViewModel.CustomDocument.DocumentTypeCode = this.customsDocumentsTicketPM.DocumentTypeCode;
                    await this.customDocumentNewVersionService.NewVersion(relatedDocumentViewModel.CustomDocument, true);
                    SessionLocator.SelectedSession.StartBusyIndicator(TextCodeTranslator.Translate("Customs.General.O.Saving"));
                    this.connectDocument(relatedDocumentViewModel);
                }
                
              } else // יש סימוכין ומקושר  להצהרה םו טיקט
                this.cnotConnectDiffrentTypeDocumentMessage();  
            }      
        }
    }
    private async GetIsConnectDec(DocumentsFilingId): Promise<boolean>{
        var customsDocumentsTicketPMService: CustomsDocumentsTicketsExtendedService = new CustomsDocumentsTicketsExtendedService();
        const res = await new Promise<boolean>((resolve, reject) => {        
              customsDocumentsTicketPMService.GetIsConnectDec(DocumentsFilingId,this.EntityPM.id).subscribe((response: ServiceResponse) => {
                if (!response.HasError) {
                    if(response.Result.decConnect.length>0){

                        resolve(false);
                    }
                    else {
                      resolve(true);
                    }
                }
                
            });
        })

        return res;
    }

    private async GetDocConnectTicket(DocumentsFilingId): Promise<boolean>{
        var customsDocumentsTicketPMService: CustomsDocumentsTicketsExtendedService = new CustomsDocumentsTicketsExtendedService();
        const res = await new Promise<boolean>((resolve, reject) => {        
              customsDocumentsTicketPMService.GetDocConnectTicket(DocumentsFilingId, this.EntityPM.id, SessionLocator.Tenant).subscribe((response: ServiceResponse) => {
                if (!response.HasError) {
                    if(response.Result.decConnect.length>0){

                        resolve(true);
                    }
                    else {
                      resolve(false);
                    }
                }
                
            });
        })

        return res;
    }
    private async checkFileBiggerFrom200MB(relatedDocumentViewModel: RelatedDocumentViewModel) {
        var fileSizeInMB = relatedDocumentViewModel.FileSize / (1024 * 1024);
        if (fileSizeInMB > 200) { //if (fileSizeInMB > 30) {
            const confirmWindow = new ConfirmWindow();
            confirmWindow.Width = 400;
            confirmWindow.Height = 200;
            confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
            confirmWindow.ShowNoButton = false;
            confirmWindow.NoButtonText = TextCodeTranslator.Translate("Customs.General.B.No");
            confirmWindow.Show(TextCodeTranslator.Translate("Customs.General.O.DocumentSizeLimit"));
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) 
                    confirmWindow.Close();                
            });

            return true
        }

        return false;
    }

    private async confirmConnectionDiffrentDocTypeMsg(): Promise<boolean> {
        const confirmWindow = new ConfirmWindow();
        confirmWindow.Show(TextCodeTranslator.Translate("Customs.General.O.ConnectDiffrentTypeMessage"));
        await confirmWindow.WindowClosedPromise()
        
        return confirmWindow.Yes;
    }

    private connectDocument(relatedDocumentViewModel: RelatedDocumentViewModel) {       
            SessionLocator.SelectedSession.StartBusyIndicator(TextCodeTranslator.Translate("Customs.General.O.Saving"));
            this.customsDocumentMetaDataValuePMs = relatedDocumentViewModel.CustomDocument.CustomsDocumentMetaDataValues;
    
            //CustomsDocumentsTicketId adjustment mohammad 18.10.14
            this.customsDocumentsTicketPM.DocumentsFilingId = relatedDocumentViewModel.documentsFilingPM.Id;
            this.customsDocumentsTicketPM.Name = relatedDocumentViewModel.documentsFilingPM.DocumentTypeName;
            this.customsDocumentsTicketPM.Extension = relatedDocumentViewModel.documentsFilingPM.FileExtension;
            this.customsDocumentsTicketPM.FileSize = relatedDocumentViewModel.documentsFilingPM.FileSize;
            this.customsDocumentsTicketPM.IsMetaDataReady = relatedDocumentViewModel.CustomDocument.IsMetaDataReady;
    
    
            /// <---field to refresh Screen
            this.customsDocumentsTicketPM.DocumentStatusName = relatedDocumentViewModel.CustomDocument.DocumentStatusName;
            this.customsDocumentsTicketPM.DocumentStatusCode = relatedDocumentViewModel.CustomDocument.DocumentStatusCode;
            this.customsDocumentsTicketPM.CustomsDocId = relatedDocumentViewModel.CustomDocument.CustomsDocId;
            this.customsDocumentsTicketPM.ExternalAttachmentId = relatedDocumentViewModel.CustomDocument.ExternalAttachmentId;
            if (this.EntityPM.Direction == "E") {
                this.GeneratecustomsDocumentMetaDataValues();
            }
    
            if (this.isNew) {
    
                this.SaveGeneratedPointer(relatedDocumentViewModel);
            }
            else {
                var customsDocumentsTicketPMService: CustomsDocumentsTicketPMService = new CustomsDocumentsTicketPMService();
                customsDocumentsTicketPMService.update(this.customsDocumentsTicketPM).subscribe((response: ServiceResponse) => {
                    if (!response.HasError) {
                        relatedDocumentViewModel.IsConnected = true;
                        relatedDocumentViewModel.CustomDocument.IsPartOfDeclaration = true;
                        this.SetSavedMetaData(relatedDocumentViewModel);
                    }
                    else {
                        SessionLocator.SelectedSession.StopBusyIndicator();
                        if (SessionLocator.SelectedSession.CurrentEditComponent) {
                            SessionLocator.SelectedSession.CurrentEditComponent.ValidationErrorsList = response.ErrorsArray;
                        }
                        else {
                            var messageWindow = new MessageWindow();
                            messageWindow.Width = 400;
                            messageWindow.Height = 200;
                            messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                            if (response.ErrorsArray && response.ErrorsArray.length > 0) {
                                messageWindow.Show(response.ErrorsArray[0]);
                            }
                            else {
                                messageWindow.Show("Server Error");
                            }
                            messageWindow.WindowClosed.subscribe((event: any) => {
    
                                messageWindow.Close();
    
                            });
                        }
                    }     
            });

            }      
    }

    private cnotConnectDiffrentTypeDocumentMessage() {
        SessionLocator.SelectedSession.StopBusyIndicator();
        var messageWindow = new MessageWindow();
        messageWindow.Width = 400;
        messageWindow.Height = 200;
        messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
        messageWindow.Show(TextCodeTranslator.Translate("Customs.Declaration.O.DocumentAndCustomDocumentType"));
        messageWindow.WindowClosed.subscribe((event: any) => messageWindow.Close());
    }

    DisconnectButtonClicked_old(dataContext: CustomsDocumentsComponent) {


        this.DataContext = dataContext;
        if (this.customsDocumentsTicketPM.DocumentStatusCode == "8") {
            //var message: string = TextCodeTranslator.Translate("Customs.Declaration.O.DisconnectNotAllowed");
            //if (AppTool.IsNullOrEmpty(message) || message == "Customs.Declaration.O.DisconnectNotAllowed") {
            var message = ".לא ניתן לנתק מסמך בתהליך אימות";
            //}
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Width = 400;
            confirmWindow.Height = 200;
            confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
            confirmWindow.ShowNoButton = false;
            confirmWindow.NoButtonText = TextCodeTranslator.Translate("Customs.General.B.No");
            confirmWindow.Show(message);
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    confirmWindow.Close();
                }
            });
        }
        else if (this.customsDocumentsTicketPM.VerificationStatusTypeCode == "4" || this.customsDocumentsTicketPM.VerificationStatusTypeCode == "5" || this.customsDocumentsTicketPM.VerificationStatusTypeCode == "6") {
            //var message: string = TextCodeTranslator.Translate("Customs.Declaration.O.DisconnectNotAllowed");
            //if (AppTool.IsNullOrEmpty(message) || message == "Customs.Declaration.O.DisconnectNotAllowed") {
            var message = ".לא ניתן לנתק מסמך אומת/נדחה";
            //}
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Width = 400;
            confirmWindow.Height = 200;
            confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
            confirmWindow.ShowNoButton = false;
            confirmWindow.NoButtonText = TextCodeTranslator.Translate("Customs.General.B.No");
            confirmWindow.Show(message);
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    confirmWindow.Close();
                }
            });
        }
        else {
            this.iCustomsDocumentsController.CheckRequestsInProgress(this.customsDocumentsTicketPM.DocumentsFilingId).subscribe((response: ServiceResponse) => {
                if (!AppTool.IsNullOrEmpty(this.customsDocumentsTicketPM.CustomsDocId)) {
                    if (response.Result.IsDisplayOnly) {
                        if (this.isDisplayOnly) {
                            var message: string = TextCodeTranslator.Translate("Customs.Declaration.O.DisconnectNotAllowed");
                            if (AppTool.IsNullOrEmpty(message) || message == "Customs.Declaration.O.DisconnectNotAllowed") {
                                message =TextCodeTranslator.Translate("Customs.General.O.CannotDetachDocumentWithCustomsDocId")
                              
                            }
                            var confirmWindow = new ConfirmWindow();
                            confirmWindow.Width = 400;
                            confirmWindow.Height = 200;
                            confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                            confirmWindow.ShowNoButton = false;
                            confirmWindow.NoButtonText = TextCodeTranslator.Translate("Customs.General.B.No");
                            confirmWindow.Show(message);
                            confirmWindow.WindowClosed.subscribe((event: any) => {
                                if (confirmWindow.Yes) {
                                    confirmWindow.Close();
                                }
                            });
                        }
                        else {
                            this.ApplyDisconnectFromDocument(true);
                        }
                    }
                    else {
                        this.ApplyDisconnectFromDocument(true);
                    }
                }
                else {
                    if (this.isDisplayOnly && response.Result.IsDisplayOnly) {
                        if (!AppTool.IsNullOrEmpty(this.customsDocumentsTicketPM.RequestedCustomsDocId) && !(response.Result.IsDisplayOnly)) {
                            this.ApplyDisconnectFromDocument(true);
                        }
                        else {
                            var message: string = TextCodeTranslator.Translate("Customs.Declaration.O.DisconnectNotAllowed");
                            if (AppTool.IsNullOrEmpty(message) || message == "Customs.Declaration.O.DisconnectNotAllowed") {
                                message = "לא ניתן לנתק מסמך נדרש – קיימת בקשה בתהליך";
                            }
                            var confirmWindow = new ConfirmWindow();
                            confirmWindow.Width = 400;
                            confirmWindow.Height = 200;
                            confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                            confirmWindow.ShowNoButton = false;
                            confirmWindow.NoButtonText = TextCodeTranslator.Translate("Customs.General.B.No");
                            confirmWindow.Show(message);
                            confirmWindow.WindowClosed.subscribe((event: any) => {
                                if (confirmWindow.Yes) {
                                    confirmWindow.Close();
                                }
                            });
                        }
                    }
                    else {
                        if (response.Result.IsDisplayOnly) {
                            var message: string = TextCodeTranslator.Translate("Customs.Declaration.O.DisconnectNotAllowed");
                            if (AppTool.IsNullOrEmpty(message) || message == "Customs.Declaration.O.DisconnectNotAllowed") {
                                message = ".לא ניתן לנתק מסמך עם בקשה בתהליך";
                            }
                            var confirmWindow = new ConfirmWindow();
                            confirmWindow.Width = 400;
                            confirmWindow.Height = 200;
                            confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                            confirmWindow.ShowNoButton = false;
                            confirmWindow.NoButtonText = TextCodeTranslator.Translate("Customs.General.B.No");
                            confirmWindow.Show(message);
                            confirmWindow.WindowClosed.subscribe((event: any) => {
                                if (confirmWindow.Yes) {
                                    confirmWindow.Close();
                                }
                            });
                        }
                        else {
                            this.ApplyDisconnectFromDocument(true);
                        }
                    }
                }
            });
        }
    }

    DisconnectButtonClicked(dataContext: CustomsDocumentsComponent) {
        //Display only Logic - (Task 35024)
        var applyDisconnect: boolean = true;
        var message: string = "";
        if (this.customsDocumentsTicketPM) {
            var entitySpecialCondition = this.iCustomsDocumentsController.GetAddEditDocumentsEntitySpecialCondition();
            if ((this.isDisplayOnly || !entitySpecialCondition) && AppTool.IsNullOrEmpty(this.customsDocumentsTicketPM.RequestedCustomsDocId)) {//(Regular doc in a paid declaration) or just disabled declaration
                applyDisconnect = false;
                message = "ההצהרה לתצוגה בלבד - לא ניתן לנתק מסמכים";
            }
            if (this.isDisplayOnly && AppTool.IsNullOrEmpty(this.customsDocumentsTicketPM.RequestedCustomsDocId) && !AppTool.IsNullOrEmpty(this.customsDocumentsTicketPM.CustomsDocId)) {//(Regular doc that was already sent to customs)
                applyDisconnect = false;
                message = TextCodeTranslator.Translate("Customs.General.O.CannotDetachDocumentWithCustomsDocId")
               
            }

            if (!AppTool.IsNullOrEmpty(this.customsDocumentsTicketPM.RequestedCustomsDocId) && !AppTool.IsNullOrEmpty(this.customsDocumentsTicketPM.VerificationStatusTypeCode)) {//(Requested doc that was verified/denied/in verification process)
                applyDisconnect = false;
                message = ".לא ניתן לנתק מסמך אומת/נדחה/בתהליך אימות";
            }

            if (!AppTool.IsNullOrEmpty(this.customsDocumentsTicketPM.RequestedCustomsDocId) && AppTool.IsNullOrEmpty(this.customsDocumentsTicketPM.VerificationStatusTypeCode)) {//Requested Doc that wasn't send to customs for verification allow disconnect according to design.
                applyDisconnect = true;
                //message = "לם ניתן לנתק מסמך נדרש – קיימת בקשה בתהליך";
            }
            if (this.customsDocumentsTicketPM.DocumentStatusCode == '7') {
                applyDisconnect = false;
                message = "לא ניתן לנתק את המסמך - קיימת בקשה בתהליך";
            }
            if(this.EntityPM.Direction == 'E' && applyDisconnect && !AppTool.IsNullOrEmpty(this.customsDocumentsTicketPM?.DocumentsFilingId)){
                var ocrDocumentExtendedListService: OcrDocumentExtendedListService = new OcrDocumentExtendedListService();                
                ocrDocumentExtendedListService.GetOcrDocumentByDocumentFilingId(SessionLocator.Tenant, this.customsDocumentsTicketPM.DocumentsFilingId).subscribe((response)=>{
                    if(response?.Result?.NotConnect){
                        var ocrDocumentPM : OcrDocumentPM = response.Result;
                        ocrDocumentPM.NotConnect = false;
                        var ocrDocumentPMService: OcrDocumentPMService = new OcrDocumentPMService();   
                        SessionLocator.SelectedSession.StartBusyIndicator(TextCodeTranslator.Translate("Customs.General.O.Saving"));   
                        ocrDocumentPMService.update(ocrDocumentPM).subscribe(()=>{
                            SessionLocator.SelectedSession.StopBusyIndicator();
                        });             
                    }
                });
            }
            
        }
       
        if (applyDisconnect) {
            this.ApplyDisconnectFromDocument(true);
        }
        else {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Width = 400;
            confirmWindow.Height = 200;
            confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
            confirmWindow.ShowNoButton = false;
            confirmWindow.NoButtonText = TextCodeTranslator.Translate("Customs.General.B.No");
            confirmWindow.Show(message);
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    confirmWindow.Close();
                }
            });
        }
    }

    ApplyDisconnectFromDocument(submit: boolean) {
        if (SessionLocator.SelectedSession.CurrentEditComponent) {
            if (SessionLocator.SelectedSession.CurrentEditComponent.EntityPM.IsDirty) {
                SessionLocator.SelectedSession.CurrentEditComponent.SaveChanges();
            }
        }
        SessionLocator.SelectedSession.StartBusyIndicator(TextCodeTranslator.Translate("Customs.General.O.Saving"));

        this.customsDocumentsTicketPM.DocumentsFilingId = null;
        this.customsDocumentsTicketPM.Extension = null;
        this.customsDocumentsTicketPM.Name = null;
        this.customsDocumentsTicketPM.FileSize = null;
        this.LeadingMetaDataName = null;
        this.LeadingMetaDataValue = null;
        if (this.customsDocumentMetaDataValuePMs != null) {
            this.customsDocumentMetaDataValuePMs = [];
            this.customsDocumentMetaDataValuePMs = null;
        }
        this.customsDocumentsTicketPM.IsMetaDataReady = false;
        if (submit) {
            var customsDocumentsTicketPMService: CustomsDocumentsTicketPMService = new CustomsDocumentsTicketPMService();
            customsDocumentsTicketPMService.update(this.customsDocumentsTicketPM).subscribe((response: ServiceResponse) => {
                SessionLocator.SelectedSession.StopBusyIndicator();
                if (response.HasError) {
                    if (SessionLocator.SelectedSession.CurrentEditComponent) {
                        SessionLocator.SelectedSession.CurrentEditComponent.ValidationErrorsList = response.ErrorsArray;
                    }
                    else {
                        var messageWindow = new MessageWindow();
                        messageWindow.Width = 400;
                        messageWindow.Height = 200;
                        messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                        if (response.ErrorsArray && response.ErrorsArray.length > 0) {
                            messageWindow.Show(response.ErrorsArray[0]);
                        }
                        else {
                            messageWindow.Show("Server Error");
                        }
                        messageWindow.WindowClosed.subscribe((event: any) => {

                            messageWindow.Close();

                        });
                    }
                }
                else {
                    //this.DataContext.RefreshButtonClicked();


                    this.DataContext.RefreshEntity();
                    //if (SessionLocator.SelectedSession.CurrentEditComponent) {
                    //    SessionLocator.SelectedSession.CurrentEditComponent.ReloadEntityPM();
                    //}
                }
            });
        }
    }

    DeleteButtonClicked(dataContext: CustomsDocumentsComponent) {
        this.DataContext = dataContext;
        SessionLocator.SelectedSession.StartBusyIndicator(TextCodeTranslator.Translate("Customs.General.O.Saving"));
        if (!this.isNew) {
            if (!AppTool.IsNullOrEmpty(this.customsDocumentsTicketPM.RequestedCustomsDocId)) {
                SessionLocator.SelectedSession.StopBusyIndicator();
                var confirmWindow = new ConfirmWindow();
                confirmWindow.Width = 400;
                confirmWindow.Height = 200;
                confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                confirmWindow.ShowNoButton = false;
                confirmWindow.NoButtonText = TextCodeTranslator.Translate("Customs.General.B.No");
                confirmWindow.Show("Can't delete a ticket with a requested document id");//(TextCodeTranslator.Translate("Customs.General.O.DocumentSizeLimit"));
                confirmWindow.WindowClosed.subscribe((event: any) => {
                    if (confirmWindow.Yes) {
                        confirmWindow.Close();
                    }
                });
            }
            else {
                this.iCustomsDocumentsController.CheckRequestsInProgress(this.customsDocumentsTicketPM.DocumentsFilingId).subscribe((response: ServiceResponse) => {
                    if (response.Result.IsDisplayOnly) {
                        SessionLocator.SelectedSession.StopBusyIndicator();
                        var confirmWindow = new ConfirmWindow();
                        confirmWindow.Width = 400;
                        confirmWindow.Height = 200;
                        confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                        confirmWindow.ShowNoButton = false;
                        confirmWindow.NoButtonText = TextCodeTranslator.Translate("Customs.General.B.No");
                        confirmWindow.Show("Can't delete a ticket with a request in progress");//(TextCodeTranslator.Translate("Customs.General.O.DocumentSizeLimit"));
                        confirmWindow.WindowClosed.subscribe((event: any) => {
                            if (confirmWindow.Yes) {
                                confirmWindow.Close();
                            }
                        });
                    }
                    else {

                        var customsDocumentsTicketPMService: CustomsDocumentsTicketsExtendedService = new CustomsDocumentsTicketsExtendedService();
                        customsDocumentsTicketPMService.delete(this.customsDocumentsTicketPM.Id).subscribe((deleteResp: ServiceResponse) => {
                            SessionLocator.SelectedSession.StopBusyIndicator();
                            if (!deleteResp.HasError) {
                                this.DataContext.RefreshButtonClicked();
                            }
                            else {
                                if (SessionLocator.SelectedSession.CurrentEditComponent) {
                                    SessionLocator.SelectedSession.CurrentEditComponent.ValidationErrorsList = deleteResp.ErrorsArray;
                                }
                                else {
                                    var messageWindow = new MessageWindow();
                                    messageWindow.Width = 400;
                                    messageWindow.Height = 200;
                                    messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                                    if (deleteResp.ErrorsArray && deleteResp.ErrorsArray.length > 0) {
                                        messageWindow.Show(deleteResp.ErrorsArray[0]);
                                    }
                                    else {
                                        messageWindow.Show("Server Error");
                                    }
                                    messageWindow.WindowClosed.subscribe((event: any) => {

                                        messageWindow.Close();

                                    });
                                }
                            }
                        });

                    }
                });
            }
        }
        else {
            SessionLocator.SelectedSession.StopBusyIndicator();
            this.DataContext.RefreshButtonClicked();
        }
    }

    ViewDocumentsQuery(IsClose:boolean=false) {
        
        var windowArgs: any = this.EntityPM;

        var entityInfo = this.iCustomsDocumentsController.GetParentAndChildrenEntityCodesAndIds();

        var windowTitle = TextCodeTranslator.Translate("Customs.Declaration.O.RelatedDocuments");

        var logWindow = new LogitudeWindow();
        logWindow.Width = 1000;
        logWindow.Height = 800;
        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowArgs.IsClose = IsClose;
        logWindow.WindowClosed.subscribe(($event: any) => this.OnAddEditWindowClosed($event));
        logWindow.StartBusyIndicator(TextCodeTranslator.Translate("Customs.General.O.Loading"));
        logWindow.Show('./CustomsModules/CustomsDocuments/Components/DocumentsFilingsQueryComponent');
    }

    OnAddEditWindowClosed(event) {
        
        if (event != 'cancel' && this.isDisplayOnly && !this.customsDocumentsTicketPM.RequestedCustomsDocId) {
            var messageWindow = new MessageWindow();
            messageWindow.Width = 400;
            messageWindow.Height = 200;
            messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
            if(this.DataContext.DisplayOnlyMessage=="לתצוגה בלבד - אילוץ אושר")
               messageWindow.Show(TextCodeTranslator.Translate("Customs.General.O.HappinessConstraintDocumentsCannotBeLinked"));
            if(this.DataContext.EntityPM.declarationStatusTypeCode=="36"){
                messageWindow.Show(TextCodeTranslator.Translate("Customs.General.O.ClosedDeclarationDocumentsCannotBeLinked"));
            }
            else
               messageWindow.Show(TextCodeTranslator.Translate("Customs.General.O.DeclarationSubmittedDocumentsCannotBeLinked"));
            messageWindow.WindowClosed.subscribe((event: any) => {

                messageWindow.Close();

            });
            return;
        }
        if (event != 'cancel' && !AppTool.IsNullOrEmpty(event)) {
            var documentsFilingId = event;//encodeURIComponent(event);
            var custDocRelatedDocsWebService: CustDocRelatedDocsWebService = new CustDocRelatedDocsWebService();
            custDocRelatedDocsWebService.GetSingleDocumentsFilingPM(documentsFilingId).subscribe((resp: ServiceResponse) => {
                var documentFiling: DocumentsFilingPM = resp.Result;
                var custDocMetaDataValuesWebService: CustDocMetaDataValuesWebService = new CustDocMetaDataValuesWebService();
                custDocMetaDataValuesWebService.GetCustomsDocumentMetaDataValuesByConnectedEntity(documentFiling.EntityId).subscribe((metadataResp: ServiceResponse) => {
                    var values: CustomsDocumentMetaDataValuePM[] = metadataResp.Result;
                    var customsDocumentPMService: CustomsDocumentPMService = new CustomsDocumentPMService();

                    var documentsFilingIdEnc = encodeURIComponent(documentsFilingId);
                    customsDocumentPMService.get(documentsFilingIdEnc).subscribe((response: ServiceResponse) => {
                        var relatedDocumentViewModel: RelatedDocumentViewModel = new RelatedDocumentViewModel(documentFiling, values, this.isDisplayOnly);
                        relatedDocumentViewModel.CustomDocument = response.Result;
                        if (relatedDocumentViewModel.CustomDocument) {
                            this.StartCustomsDocumentMetaDataCheck(relatedDocumentViewModel);
                        }
                        else {
                            var customsDocumentPM: CustomsDocumentPM = new CustomsDocumentPM();
                            customsDocumentPM.DocumentsFilingId = relatedDocumentViewModel.Id;
                            customsDocumentPM.Tenant = SessionLocator.Tenant;
                            customsDocumentPM.DocumentTypeCode = this.customsDocumentsTicketPM.DocumentTypeCode;
                            customsDocumentPM.DeclarationId = this.EntityPM.Id;
                            relatedDocumentViewModel.CustomDocument = customsDocumentPM;
                            customsDocumentPMService.insert(customsDocumentPM).subscribe((resp: ServiceResponse) => {
                                if (!resp.HasError) {
                                    this.StartCustomsDocumentMetaDataCheck(relatedDocumentViewModel);
                                }
                                else {
                                    SessionLocator.SelectedSession.StopBusyIndicator();

                                    if (SessionLocator.SelectedSession.CurrentEditComponent) {
                                        SessionLocator.SelectedSession.CurrentEditComponent.ValidationErrorsList = resp.ErrorsArray;
                                    }
                                    else {
                                        var messageWindow = new MessageWindow();
                                        messageWindow.Width = 400;
                                        messageWindow.Height = 200;
                                        messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                                        if (resp.ErrorsArray && resp.ErrorsArray.length > 0) {
                                            messageWindow.Show(resp.ErrorsArray[0]);
                                        }
                                        else {
                                            messageWindow.Show("Server Error");
                                        }
                                        messageWindow.WindowClosed.subscribe((event: any) => {

                                            messageWindow.Close();

                                        });
                                    }
                                }
                                //SessionLocator.SelectedSession.StartBusyIndicatorSaving();
                                //                                SessionLocator.SelectedSession.StopBusyIndicator();

                            });
                        }

                    });
                });
            });
        }
    }

    PreventEditTicket() {
        this.PreventEdit = true;
    }

    AllowEditTicket() {
        this.PreventEdit = false;
    }
    ShowMustSend(): boolean {
        if (!this.customsDocumentsTicketPM.IsSendMandatory) {
            return false;
        }
        if (AppTool.IsNullOrEmpty(this.customsDocumentsTicketPM.CustomsDocId)) {
            return true;
        } else {
            return false;
        }
    }

}

export class MetaDataValueViewModel {
    public CustomsDocumentId: string;
    public Tenant: number;
    public MetaDataTypeCode: string;
    public MetaDataValue: string
    public MetaDataTypeName: string
    public IsLeading: boolean;
    public IsMandatory: boolean;
}
