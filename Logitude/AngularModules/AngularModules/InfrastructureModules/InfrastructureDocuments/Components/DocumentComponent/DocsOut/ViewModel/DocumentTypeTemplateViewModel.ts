import {EntityArgs} from '../../../../../../Infrastructure/DataContracts/EntityArgs';
import {DocumentTypeTemplateList} from '../../../../../../Common/EntityLists/DocumentTypeTemplateList';


export class DocumentTypeTemplateViewModel {




    public IsEnabledAddDocumentTemplate: boolean;
   

    public OriginalTemplate: string;

    public ActiveId: string;
    CC:string = "";
    From: string = "";
    ReplyTo: string = "";
    TemplateSubject: string = "";
    TemplateCc: string = "";
    TemplateBcc: string = "";
    To: string = "";
    
    public Id: string;
    public  Tenant: number;
    public  TemplateBody: string;
    public TemplateType: string;
    public LastUpdatedByUserId: string;
    public DocumentTypeId: string;
    public Subject: string;
    public LastUpdateDate: Date;
    public  EditorTool: string;
    public  VerticalShift: number;
    public HorizontalShift: number;
    public OriginalTemplateId: string;
    public  Language: string;
    public  InternalRemarks: string;
    public  CountryCode: string;
    public IsEnabledForCustomers: boolean;
    public IsCopiedAtSignup: boolean;
    public LastUpdateByUserName: string;
    public OriginalTemplateName: string;
    public DocumentTypeCode: string;
    public  DocumentTypeName: string;
    public  IsHideDocumentName: boolean;
    public CountryName: string;
    public ObjectTableId: string;
    public DivSelectBackgroud: string ;
    public IsSelected: boolean;
    public Jsonstring: string;
    public HtmlData: string;
    public StimulData: any;
    public IsHaveJsonString: string;
    public HtmlResolve: string;
    public LableSetactive: string;
    public Entity: any;
    public IsLoad: boolean;
    public TemplateBodyHtml: any;
    public TemplateFooterHtml: any;
    public TemplateHeaderHtml: any;


    public TemplateHeaderHeight: number;
    public TemplateFooterHeight: number;
    public TemplateTechnologyCode: string;
    public IsSystem: boolean;




    private description: string = "";
    get Description() {


        return this.description;
    }
    set Description(newValue: string) {
        if (this.description != newValue) {
            this.description = newValue;
            this.Entity.Description = newValue;
        }
    }


    private inActive: boolean = false;
    get InActive() { return this.inActive; }
    set InActive(newValue: boolean) {
        if (this.inActive != newValue) {
            this.inActive = newValue;
            this.Entity.InActive = newValue;
        }
    }



    private isDefault: boolean = false;
    get IsDefault() { return this.isDefault; }
    set IsDefault(newValue: boolean) {
        if (this.isDefault != newValue) {
            this.isDefault = newValue;
            this.Entity.IsDefault = newValue;
        }
    }


    constructor(documentTypeTemplate: any) {
            
                this.Entity = documentTypeTemplate; 
                this.ActiveId = documentTypeTemplate.Id + "Active";
                this.OriginalTemplate = documentTypeTemplate.OriginalTemplateName;
                this.IsEnabledAddDocumentTemplate = true;
                this.Id = documentTypeTemplate.Id;
                this.Tenant = documentTypeTemplate.Tenant;
                //this.TemplateBody = documentTypeTemplate.TemplateBody;
                this.TemplateType = documentTypeTemplate.TemplateType;
                this.LastUpdatedByUserId = documentTypeTemplate.LastUpdatedByUserId;
                this.DocumentTypeId = documentTypeTemplate.DocumentTypeId;
                this.Subject = documentTypeTemplate.Subject;
                this.LastUpdateDate = documentTypeTemplate.LastUpdateDate;
                this.InActive = documentTypeTemplate.InActive;
                this.EditorTool = documentTypeTemplate.EditorTool;
                this.VerticalShift = documentTypeTemplate.VerticalShift;
                this.HorizontalShift = documentTypeTemplate.HorizontalShift;
                this.OriginalTemplateId = documentTypeTemplate.OriginalTemplateId;
                this.Description = documentTypeTemplate.Description;
                this.Language = documentTypeTemplate.Language;
                this.InternalRemarks = documentTypeTemplate.InternalRemarks;
                this.CountryCode = documentTypeTemplate.CountryCode;
                this.IsEnabledForCustomers = documentTypeTemplate.IsEnabledForCustomers;
                this.IsCopiedAtSignup = documentTypeTemplate.IsCopiedAtSignup;
                this.LastUpdateByUserName = documentTypeTemplate.LastUpdateByUserName;
                this.IsDefault = documentTypeTemplate.IsDefault;
                this.OriginalTemplateName = documentTypeTemplate.OriginalTemplateName;
                this.DocumentTypeCode = documentTypeTemplate.DocumentTypeCode;
                this.IsHideDocumentName = documentTypeTemplate.IsHideDocumentName;

                this.TemplateBodyHtml = documentTypeTemplate.TemplateBodyHtml;
                this.TemplateHeaderHtml = documentTypeTemplate.TemplateHeaderHtml;
                this.TemplateFooterHtml = documentTypeTemplate.TemplateFooterHtml;
                this.CountryName = documentTypeTemplate.CountryName;
                this.ObjectTableId = documentTypeTemplate.ObjectTableId;

                this.TemplateHeaderHeight = documentTypeTemplate.TemplateHeaderHeight;
                this.TemplateFooterHeight = documentTypeTemplate.TemplateFooterHeight;
                this.TemplateTechnologyCode = documentTypeTemplate.TemplateTechnologyCode;


				this.From = documentTypeTemplate.From;
			    this.ReplyTo = documentTypeTemplate.ReplyTo;
                this.CC = documentTypeTemplate.CC;
                this.To = documentTypeTemplate.To;
                this.IsSystem = documentTypeTemplate.IsSystem;

                if (documentTypeTemplate.InActive) {
                    this.LableSetactive = "Mark as active";
                }
                else {
                    this.LableSetactive = "Mark as inactive";
                }
              
                if (!documentTypeTemplate.IsHideDocumentName) {
                    this.DocumentTypeName = documentTypeTemplate.DocumentTypeName;
                }
                else {
                    this.DocumentTypeName = "";
                }

               // this.IsHaveJsonString = documentTypeTemplateList.IsHaveJsonString;

    }




}
