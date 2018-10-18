import {CustomsDocumentPM} from "../../../Customs/EntityPMs/CustomsDocumentPM";
import {DocumentsFilingPM} from "../../../Common/EntityPMs/DocumentsFilingPM";
import {CustomsDocumentMetaDataValuePM} from '../../../Customs/EntityPMs/CustomsDocumentMetaDataValuePM';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {CustomDocumentTypeMetaDataListService} from '../../../Customs/Services/StandardLists/CustomDocumentTypeMetaDataListService';
import {CustomDocumentTypeMetaDataList} from '../../../Customs/EntityLists/CustomDocumentTypeMetaDataList';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {AppTool, ArrayTool} from '../../../Infrastructure/Tools';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';

export class RelatedDocumentViewModel {

    //**************Properties******************//
    public IsConnected: boolean = false;
    get Id() { return this.documentsFilingPM.Id; }
    set Id(value: string) {

        if (this.documentsFilingPM.Id != value) {
            this.documentsFilingPM.Id = value;
        }
    }
    get Name() { return this.documentsFilingPM.Name; }
    set Name(value: string) {

        if (this.documentsFilingPM.Name != value) {
            this.documentsFilingPM.Name = value;
        }
    }
    public DocumentTypeName: string;
    //get DocumentTypeName() { return this.documentsFilingPM.CustomsDocumentTypeName; }
    //set DocumentTypeName(value: string) {

    //    if (this.documentsFilingPM.CustomsDocumentTypeName != value) {
    //        this.documentsFilingPM.CustomsDocumentTypeName = value;
    //    }
    //}
    public LeadingMetaDataName: string;
    public LeadingMetaDataValue: string;
    public Status1ImageGreen: boolean;
    public Status1ImageGray: boolean;
    public Status2ImageGreen: boolean;
    public Status2ImageGray: boolean;
    public Status2ErrorImage: boolean;
    get ExternalAttachmentId() { return this.documentsFilingPM.ExternalAttachmentId; }
    set ExternalAttachmentId(value: string) {

        if (this.documentsFilingPM.ExternalAttachmentId != value) {
            this.documentsFilingPM.ExternalAttachmentId = value;
        }
    }
    get ExternalAttachmentIdVisibility() {
        if (this.ExternalAttachmentId) {
            return true;
        }
        else {
            return false;
        }
    }
    get Extension() { return this.documentsFilingPM.FileExtension; }
    set Extension(value: string) {

        if (this.documentsFilingPM.FileExtension != value) {
            this.documentsFilingPM.FileExtension = value;
        }
    }
    get Code() { return this.documentsFilingPM.Code; }
    set Code(value: string) {

        if (this.documentsFilingPM.Code != value) {
            this.documentsFilingPM.Code = value;
        }
    }
    get CodeVisibility() {
        if (this.ExternalAttachmentId) {
            return false;
        }
        else {
            return true;
        }
    }
    private customDocumentTypeMetaDataLists: CustomDocumentTypeMetaDataList[];
    get FileSize() { return this.documentsFilingPM.FileSize; }
    set FileSize(value: number) {

        if (this.documentsFilingPM.FileSize != value) {
            this.documentsFilingPM.FileSize = value;
        }
    }
    public CustomDocument: CustomsDocumentPM;
    private EntityResourceService: EntityResourceService;
   //******************************************//

    constructor(public documentsFilingPM: DocumentsFilingPM, private customsDocumentMetaDataValuePMs: CustomsDocumentMetaDataValuePM[], private isDisplayOnly: boolean) {
        this.EntityResourceService = new EntityResourceService();
        this.SetStatusImages();
        this.SetCustomDocumentMetaData();
        //this.DocumentTypeName = documentsFilingPM.CustomsDocumentTypeName;
    }

    SetStatusImages() {
        if (this.documentsFilingPM.CustomsDocumentStatusCode == "1" || this.documentsFilingPM.CustomsDocumentStatusCode == "2" || this.documentsFilingPM.CustomsDocumentStatusCode == "7") {
            this.Status1ImageGreen = true;
            this.Status1ImageGray = false;
        }
        else {
            this.Status1ImageGreen = false;
            this.Status1ImageGray = true;
        }

        if (this.documentsFilingPM.CustomsDocumentStatusCode == "2") {
            this.Status2ImageGreen = false;
            this.Status2ImageGray = false;
            this.Status2ErrorImage = true;
        }
        else if (this.documentsFilingPM.CustomsDocumentStatusCode == "1") {
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

    private SetCustomDocumentMetaData() {
        var customDocumentTypeMetaDataListService: CustomDocumentTypeMetaDataListService = new CustomDocumentTypeMetaDataListService();
        customDocumentTypeMetaDataListService.getAllFromCache().subscribe((res: ServiceResponse) => {
            this.customDocumentTypeMetaDataLists = res.Result;
            this.EntityResourceService.getEntityResourceByTableName("Customs.CustomDocumentTypeMetaData").subscribe(response => {

                this.customDocumentTypeMetaDataLists = this.customDocumentTypeMetaDataLists.filter(d => d.DocumentTypeCode === this.documentsFilingPM.CustomsDocumentTypeCode);
                this.DocumentTypeName = this.documentsFilingPM.Description;
                if (this.customsDocumentMetaDataValuePMs && this.customDocumentTypeMetaDataLists) {
                    var leading: CustomDocumentTypeMetaDataList = this.customDocumentTypeMetaDataLists.filter(d => d.IsLeading && d.DocumentTypeCode == this.documentsFilingPM.CustomsDocumentTypeCode)[0];
                    if (leading) {
                        var leadingValue: CustomsDocumentMetaDataValuePM = this.customsDocumentMetaDataValuePMs.filter(d => d.MetaDataTypeCode == leading.MetaDataTypeCode)[0];
                        this.LeadingMetaDataValue = leadingValue != null ? (leadingValue.MetaDataValue == "True" ? "כן" : (leadingValue.MetaDataValue == "False" ? "לא" : leadingValue.MetaDataValue)) : null;
                        this.LeadingMetaDataName = leading.MetaDataTypeName;
                        if (this.LeadingMetaDataValue) {
                            this.DocumentTypeName = this.documentsFilingPM.CustomsDocumentTypeName;
                        }
                        else {
                            this.DocumentTypeName = this.documentsFilingPM.Description;
                        }
                    }
                }

            });
        });
    }

    ConnectDocumentToTicket(params: any) {
        console.log(params);
    }
    OnDragStart(event: DragEvent) {
        //if (this.isDisplayOnly) {
        //    event.preventDefault();
        //}
        //else {
            event.dataTransfer.setData("Id", this.documentsFilingPM.Id);//.setData("text", event.target.id);
        //}
    }
}
