import {Component}  from '@angular/core';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {CertificateTicket} from '../../../../../Customs/DataContract/CertificateTicket';
import { ConfirmWindow } from '../../../../../Controls/Windows/ConfirmWindow';
import { MessageWindow } from '../../../../../Controls/Windows/MessageWindow';

import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {CertificateConnectedItem} from '../../../../../Customs/DataContract/CertificateConnectedItem';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {MultiCertificatesService} from '../../../../../Customs/Services/Others/MultiCertificatesService';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {AppTool} from '../../../../../Infrastructure/Tools';
import {CertificateTabComponent, CertificateTicketListItem} from './CertificateTabComponent';
import {CustomsSettingListService} from '../../../../../Customs/Services/StandardLists/CustomsSettingListService';
import { CustomsSettingList } from '../../../../../Customs/EntityLists/CustomsSettingList';
import { AmitalGatewayUtil, UnifreightMessageM } from '../../../../../Infrastructure/Utilities/AmitalGatewayUtil';
import { CardListService } from '../../../../../Common/Services/StandardLists/CardListService'
import { CardList } from '../../../../../Common/EntityLists/CardList';
import { ConfirmationTypeList } from '../../../../../Customs/EntityLists/ConfirmationTypeList';

import { ConfirmationTypeListService } from '../../../../../Customs/Services/StandardLists/ConfirmationTypeListService';
import { CustomsDocumentsTicketPM } from '../../../../../Customs/EntityPMs/CustomsDocumentsTicketPM';
import { RelatedEntityParams } from '../../../../CustomsDocuments/Components/CustomsDocumentsComponent';
import { CustomsDocumentPointerPM } from '../../../../../Customs/EntityPMs/CustomsDocumentPointerPM';
import { CustomsDocumentsTicketsExtendedService } from '../../../../../Customs/Services/ExtendedPMs/CustomsDocumentsTicketsExtendedService';
import { CustomsDocumentsTicketPMService } from '../../../../../Customs/Services/StandardPMs/CustomsDocumentsTicketPMService';
import { DocumentsFilingPMService } from '../../../../../Common/Services/StandardPMs/DocumentsFilingPMService';
import { ApiQueryFilters } from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
@Component({
    
    templateUrl: './CreateEditTicketComponent.html',
})

export class CreateEditTicketComponent extends BaseComponent {
    public ObjectTableName: string = "Customs.SupplierInvioceItemCertificat";
    public DataContext: any = this;
    ticket: CertificateTicket;
    connectedItems: CertificateConnectedItem[] = [];
    Items: CertificateConnectedItem[] = [];
    multiCertificatesService: MultiCertificatesService = new MultiCertificatesService();
    customsSettingListService: CustomsSettingListService = new CustomsSettingListService;
    ExcludedItems: CertificateConnectedItem[] = [];
    declarationId: string;
    oldAttachment: string;
    oldCertificateNumber: string;
    oldResConfirmation: string;
    oldCertificateExempt: string;
    Parent: CertificateTabComponent;
    isNew: boolean;
    ResConfirmationFilterVisibility: boolean;
    _CardListService: CardListService = new CardListService();
    _ConfirmationTypeListService: ConfirmationTypeListService = new ConfirmationTypeListService();
    public ValidationErrorsList: string[] = [];
    public CustomsDocumentsTicket: CustomsDocumentsTicketPM;
    public documentsFilingPMService = new DocumentsFilingPMService();
    public customsDocumentsTicketPMService = new CustomsDocumentsTicketPMService();
    documentFilingId: string;
    documentTypeId: string;
    public TypeCodeFilterItems: ApiQueryFilters;
    public ExportTypeCodeFilterItems: ApiQueryFilters;
    constructor() {
        super();
        this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        this.TypeCodeFilterItems = new ApiQueryFilters();
        this.TypeCodeFilterItems.addAdditionalFilter("IsImportDeclaration", true, null, null, "Equals", false, false, false, "boolean");
        this.ExportTypeCodeFilterItems = new ApiQueryFilters();
        this.ExportTypeCodeFilterItems.addAdditionalFilter("IsExportDeclaration", true, null, null, "Equals", false, false, false, "boolean");
    }
    public ExemptRowHeight: number;
    public FilterSelectedValue: string;
    ExemptionFilterSelectedValue: string = 'other';
    IsSearchIconVisibile: boolean;
    private currentSession=SessionLocator.SelectedSession;
    SetWindowArgs(args: any) {

        this.IsSearchIconVisibile = false;
        this.connectedItems = [];
        this.Items = [];
        this.customsSettingListService.getAll().subscribe((response: ServiceResponse) => {
            var list: CustomsSettingList[] = response.Result;
           
            if (!AppTool.IsNullOrEmpty(list)) {
                var customsSetting =
                    //list[0];
                    list.filter(d => d.Tenant == SessionLocator.Tenant)[0];

                if (!AppTool.IsNullOrEmpty(customsSetting)) {
                    if (customsSetting.UnifreightCertificateActivated) {
                        //if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
                        let myDec = SessionLocator.SelectedSession.CurrentEditComponent.EntityPM;
                        //if (AmitalGatewayUtil.Instance.IsDeclarationInUse(myDec.CustomFileNo, myDec.IsConvertedDeclaration, myDec.IsConnectedToUnifreight)) {
                        if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {     //'Search Certificate Document' Icon Is Not Appearing - Certificate Multi Entry - Edit Declaration 
                            this.IsSearchIconVisibile = true;
                        }
                    }
                    //else {
                    //    this.IsSearchIconVisibile = false;
                    //}
                }
            }
        });
        this.ticket = args.Ticket;
        this.isNew = args.IsNew;
        this.connectedItems = args.ConnectedItems;
        this.ExcludedItems = args.ExcludedItems;
        this.IsAllSelected = args.IsAllSelected;
        this.Items = args.Items;
        this.declarationId = args.DeclarationId;
        this.oldAttachment = this.ticket.AttachmentTypeCode;
        this.oldCertificateNumber = this.ticket.CertificateNumber;
        this.oldCertificateExempt = this.ticket.CertificateExemptionTypeCode;
        this.oldResConfirmation = this.ticket.ResConfirmationTypeCode;
        this.Parent = args.Parent;
        this.ReqConfirmationTypeCode = this.ticket.ReqConfirmationTypeCode;


        if (!this.isNew) {
            this.FilterSelectedValue = this.ticket.AttachmentTypeCode;
     

            this.AttachmentTypeCode = this.ticket.AttachmentTypeCode;
            this.CertificateNumber = this.ticket.CertificateNumber;
            this.CertificateExemptionTypeCode = this.ticket.CertificateExemptionTypeCode;
            this.ResConfirmationTypeCode = this.ticket.ResConfirmationTypeCode;
            this.ReqConfirmationTypeCode = this.ticket.ReqConfirmationTypeCode;
        }

   

        if (this.ticket.CertificateExemptionTypeCode == "92") {
            this.ExemptionFilterSelectedValue = "92";
        }
        else if (this.ticket.CertificateExemptionTypeCode == "96") {
            this.ExemptionFilterSelectedValue = "96";
        }
        else {
            this.ExemptionFilterSelectedValue = "other";
        }
        this.SetFieldsVisibility(this.FilterSelectedValue);
        this.SetFieldsDisabled(this.isNew);

    }
    SetFieldsDisabled(isNew: boolean) {
        if (isNew || this.AttachmentTypeCode == "3" || this.AttachmentTypeCode == null) {
            this.UIProperties.SetEnabled("CertificateNumber", "Customs.SupplierInvioceItemCertificat", false);
            this.UIProperties.SetEnabled("ResConfirmationTypeCode", "Customs.SupplierInvioceItemCertificat", false);
            this.UIProperties.SetEnabled("CertificateExemptionTypeCode", "Customs.SupplierInvioceItemCertificat", false);

        }
        else if (this.CertificateExemptionTypeCode == "96" || this.CertificateExemptionTypeCode == "92") {
            this.UIProperties.SetEnabled("CertificateExemptionTypeCode", "Customs.SupplierInvioceItemCertificat", false);
        }
        else if (this.ResConfirmationTypeCode == this.ReqConfirmationTypeCode) {
            this.UIProperties.SetEnabled("ResConfirmationTypeCode", "Customs.SupplierInvioceItemCertificat", false);
        }
      //  this.ResConfirmationFilterSelectedValue = "request";
     
    }

    private certificateNumber: string;
    public get CertificateNumber() { return this.certificateNumber; }
    public set CertificateNumber(newValue: string) {
        this.certificateNumber = newValue;
    }

    private resConfirmationTypeCode: string;
    public get ResConfirmationTypeCode() { return this.resConfirmationTypeCode; }
    public set ResConfirmationTypeCode(newValue: string) {
        this.resConfirmationTypeCode = newValue;
    }

    private reqConfirmationTypeCode: string;
    public get ReqConfirmationTypeCode() { return this.reqConfirmationTypeCode; }
    public set ReqConfirmationTypeCode(newValue: string) {
        this.reqConfirmationTypeCode = newValue;
    }
    private attachmentTypeCode: string;
    public get AttachmentTypeCode() { return this.attachmentTypeCode; }
    public set AttachmentTypeCode(newValue: string) {
        this.attachmentTypeCode = newValue;
        this.UIProperties.SetEnabled("CertificateNumber", "Customs.SupplierInvioceItemCertificat", true);
        this.UIProperties.SetEnabled("ResConfirmationTypeCode", "Customs.SupplierInvioceItemCertificat", true);
        this.UIProperties.SetEnabled("CertificateExemptionTypeCode", "Customs.SupplierInvioceItemCertificat", true);

    }

    private certificateExemptionTypeCode: string;
    public get CertificateExemptionTypeCode() { return this.certificateExemptionTypeCode; }
    public set CertificateExemptionTypeCode(newValue: string) {
        this.certificateExemptionTypeCode = newValue;
    }
    IsExemptVisibile: boolean;
    IsConfirmationVisibile: boolean;

    SetFieldsVisibility(value:string) {
        switch (value) {
          
            case "1":
                {
                
                    this.IsConfirmationVisibile = true;
                    this.IsExemptVisibile = false;
                    this.ExemptRowHeight = 0;
                    this.ResConfirmationFilterVisibility = false;
                
                break;
                }
            case "2": {
                this.IsConfirmationVisibile = true;
                this.IsExemptVisibile = false;
                this.ExemptRowHeight = 0;
                this.ResConfirmationFilterVisibility = true;
                this.UIProperties.SetEnabled("ResConfirmationTypeCode", "Customs.SupplierInvioceItemCertificat", false);
                this.ResConfirmationTypeCode = this.ReqConfirmationTypeCode;
               
                break;
            }
            case "4": {
                
                this.IsConfirmationVisibile = false;
                this.IsExemptVisibile = true;
                this.ExemptRowHeight = 30;
                this.ResConfirmationFilterVisibility = false;
                break;
            }
            default: {
               
                this.IsConfirmationVisibile = true;
                this.IsExemptVisibile = false;
                this.ExemptRowHeight = 0;
                this.ResConfirmationFilterVisibility = false;
                break;
            }

        }

        //if (this.ticket.ResConfirmationTypeCode == "102" || this.ticket.ResConfirmationTypeCode == "219") {
        if (this.ResConfirmationTypeCode == this.ReqConfirmationTypeCode) {
            this.ResConfirmationFilterSelectedValue = "request";

        }
        else {
            this.ResConfirmationFilterSelectedValue = "other";

        }
    }

    FilterItemClicked(itemValue: string) {
        if (this.FilterSelectedValue != itemValue) {
            this.FilterSelectedValue = null;
            this.ExemptionFilterSelectedValue = "other";
            this.ResConfirmationFilterSelectedValue = "request";

            if (this.CertificateNumber != null || this.ResConfirmationTypeCode != null || this.CertificateExemptionTypeCode != null) {

            var msg = TextCodeTranslator.Translate("Customs.Declaration.O.DeletingDetails");
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Width = 300;
            confirmWindow.Height = 150;
            confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.Declaration.O.Yes");
            confirmWindow.NoButtonText = TextCodeTranslator.Translate("Customs.Declaration.O.No");
            confirmWindow.Show(msg);
           
                confirmWindow.WindowClosed.subscribe((event: any) => {

                    if (confirmWindow.Yes) {
                        this.CertificateNumber = null;
                        this.ResConfirmationTypeCode = null;
                        this.CertificateExemptionTypeCode = null;
                  
                        this.FilterSelectedValue = itemValue;
                        this.AttachmentTypeCode = itemValue;
                        this.SetFieldsVisibility(itemValue);
                    }
                    else {
                        this.FilterSelectedValue = this.AttachmentTypeCode;
                    }
                });

            }
            else {
                this.FilterSelectedValue = itemValue;
                this.AttachmentTypeCode = itemValue;
                this.SetFieldsVisibility(itemValue);
            }
           
           
        }
    }


    ExemptionFilterClicked(value: string) {
        if (this.ExemptionFilterSelectedValue != value) {
            this.ExemptionFilterSelectedValue = value;
            if (value == "92") {
                this.CertificateExemptionTypeCode = "92";
                this.UIProperties.SetEnabled("CertificateExemptionTypeCode", "Customs.SupplierInvioceItemCertificat", false);
            }
            else if (value == "96") {
                this.CertificateExemptionTypeCode = "96";
                this.UIProperties.SetEnabled("CertificateExemptionTypeCode", "Customs.SupplierInvioceItemCertificat", false);
            }


            else {
                this.CertificateExemptionTypeCode = null;
                this.UIProperties.SetEnabled("CertificateExemptionTypeCode", "Customs.SupplierInvioceItemCertificat", true);
            }
          
           


        }
    }
    ResConfirmationFilterSelectedValue: string = "request";
    ResConfirmationFilterClicked(value: string) {

        if (this.ResConfirmationFilterSelectedValue != value) {
            this.ResConfirmationFilterSelectedValue = value;
            if (value == "request") {
                this.ResConfirmationTypeCode = this.ReqConfirmationTypeCode;
                this.UIProperties.SetEnabled("ResConfirmationTypeCode", "Customs.SupplierInvioceItemCertificat", false);
            }
          


            else {
                this.ResConfirmationTypeCode = null;
                this.UIProperties.SetEnabled("ResConfirmationTypeCode", "Customs.SupplierInvioceItemCertificat", true);
            }




        }
    }


 

    public GetGeneratedCustomTicketAndPointer(relatedEntityParams: RelatedEntityParams, documentTypeCode: string, documentsFilingId: string) {

        if (documentsFilingId == "" || documentsFilingId == null)
            return;

        var newTicket: CustomsDocumentsTicketPM = new CustomsDocumentsTicketPM();
        this.documentsFilingPMService.get(documentsFilingId).subscribe((data:any) => {
            if (data.Result == null) {

                let msg = new MessageWindow();

                msg.Width = 350;
                msg.Show(`לאישור קושר מסמך שעדיין לא הוזרם להיבריד`);
                return;
            }
             else {
                newTicket.DocumentsFilingId = documentsFilingId;

          
        newTicket.DocumentTypeCode = documentTypeCode;
        newTicket.Tenant = SessionLocator.Tenant;
        newTicket.ConnectedInvoiceItemsSequences = relatedEntityParams.ChildEntity2Id;
        newTicket.ConnectedInvoicesSequences = relatedEntityParams.ChildEntity1Id;
         //newTicket.Id = "Generated: " + documentTypeCode;
        var newCustomsDocumentPM: CustomsDocumentPointerPM = new CustomsDocumentPointerPM(newTicket)

        newCustomsDocumentPM.Tenant = SessionLocator.Tenant;
        newCustomsDocumentPM.ParentEntityId = relatedEntityParams.ParentEntityId;
        newCustomsDocumentPM.ParentEntityCode = relatedEntityParams.ParentEntityCode;
        newCustomsDocumentPM.Child1EntityCode = relatedEntityParams.ChildEntity1Code;
        newCustomsDocumentPM.Child2EntityCode = relatedEntityParams.ChildEntity2Code;
        newCustomsDocumentPM.Child3EntityCode = relatedEntityParams.ChildEntity3Code;
        newCustomsDocumentPM.Child1EntityId = relatedEntityParams.ChildEntity1Id;
        newCustomsDocumentPM.Child2EntityId = relatedEntityParams.ChildEntity2Id;
        newCustomsDocumentPM.Child3EntityId = relatedEntityParams.ChildEntity3Id;
        newCustomsDocumentPM.DocumentTypeCode = documentTypeCode;

        newTicket.AddCustomsDocumentPointer(newCustomsDocumentPM);
                this.customsDocumentsTicketPMService.insert(newTicket).subscribe((myResp: ServiceResponse) => {
                    if ( !myResp.HasError) {
 
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
          });
        //return newTicket;


    }


    CancelButtonClicked() {
        SessionLocator.SelectedSession.CloseCurrentWindowEmit("no");

       // SessionLocator.SelectedSession.CloseCurrentWindow();
    }
    FIELD_IS_REQUIERD: string;
    GetRequierdFieldErrorText(fieldName) {
        return this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate(fieldName));
    }
    isValid: boolean;
    inValid: boolean;
    notMandatoryIsNotEmpty:boolean = false;
    OkButtonClicked() {

        var errors: string[] = [];
        this.ValidationErrorsList = [];

      //  this.connectedItems = [];

        this.isValid = true;
        this.inValid = false;


        if (AppTool.IsNullOrEmpty(this.AttachmentTypeCode)) {
            errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvioceItemCertificat.F.AttachmentTypeCode"));
             this.isValid = false;

        }
        else {
            if (this.AttachmentTypeCode == "1" || this.AttachmentTypeCode == "2") {
                if (AppTool.IsNullOrEmpty(this.CertificateNumber) || AppTool.IsNullOrEmpty(this.ticket.ReqConfirmationTypeCode) || AppTool.IsNullOrEmpty(this.ResConfirmationTypeCode)) {
                    this.isValid = false;
                    this.inValid = true;
                }
                if (!AppTool.IsNullOrEmpty(this.CertificateExemptionTypeCode)) {
                    this.inValid = true;
                    this.notMandatoryIsNotEmpty = true;
                }

            }

            else {
                if (this.AttachmentTypeCode == "4") {
                    if (AppTool.IsNullOrEmpty(this.CertificateExemptionTypeCode) || AppTool.IsNullOrEmpty(this.ticket.ReqConfirmationTypeCode)) {
                        this.isValid = false;
                        this.inValid = true;
                    }

                    if (!AppTool.IsNullOrEmpty(this.CertificateNumber) || !AppTool.IsNullOrEmpty(this.ResConfirmationTypeCode)) {
                        this.inValid = true;
                        this.notMandatoryIsNotEmpty = true;

                    }
                }

            }
        }




        if (this.inValid) {
            this.isValid = false;

           
         
            var confirm = new ConfirmWindow();
            confirm.Width = 300;
            confirm.Height = 150;
            confirm.YesButtonText = TextCodeTranslator.Translate("Customs.Declaration.O.Yes");
            confirm.NoButtonText = TextCodeTranslator.Translate("Customs.Declaration.O.No");
         
            if (this.notMandatoryIsNotEmpty) {
                confirm.Show(TextCodeTranslator.Translate("Customs.Declaration.O.CertificateNotMandatoryFields"));
            }
            else {
                confirm.Show(TextCodeTranslator.Translate("Customs.Declaration.O.CertificateMandatoryFields"));
            }
            confirm.WindowClosed.subscribe((event: any) => {

                if (confirm.Yes) {

                    if (!AppTool.IsNullOrEmpty(this.connectedItems) && this.connectedItems.length > 0) {
                        var certificate: CertificateTicketListItem = this.Parent.CertificateTicketsList.filter(d => d.AttachmentTypeCode == this.AttachmentTypeCode && d.CertificateNumber == this.CertificateNumber && d.CertificateExemptionTypeCode == this.CertificateExemptionTypeCode && d.ResConfirmationTypeCode == this.ResConfirmationTypeCode)[0];


                        if (certificate != null) {
                            if (certificate.ticket != null) {

                                var confirmWindow = new ConfirmWindow();
                                confirmWindow.Width = 300;
                                confirmWindow.Height = 150;
                                confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.Declaration.O.Yes");
                                confirmWindow.NoButtonText = TextCodeTranslator.Translate("Customs.Declaration.O.No");


                                confirmWindow.Show(TextCodeTranslator.Translate("Customs.Declaration.O.TicketAlreadyExist"));

                                confirmWindow.WindowClosed.subscribe((event: any) => {

                                    if (confirmWindow.Yes) {
                                        this.UpdateTicket();
                                    }
                                });


                            }

                            else if (certificate.ticket == this.ticket) {
                                this.CancelButtonClicked();
                            }
                        }




                        else if (certificate == null) {
                            this.UpdateTicket();


                        }
                    }
                    else {
                      
                        this.UpdateTicket();
                    }
                   

                }
                   
            });
            
        }



        else {

            if (errors.length == 0) {
            
            
                if (!AppTool.IsNullOrEmpty(this.connectedItems) && this.connectedItems.length > 0) {

                    var certificate: CertificateTicketListItem = this.Parent.CertificateTicketsList.filter(d => d.AttachmentTypeCode == this.AttachmentTypeCode && d.CertificateNumber == this.CertificateNumber && d.CertificateExemptionTypeCode == this.CertificateExemptionTypeCode && d.ResConfirmationTypeCode == this.ResConfirmationTypeCode)[0];


                    if (certificate != null) {
                        if (certificate.ticket == this.ticket) {
                            this.CancelButtonClicked();
                        }

                        else if (certificate.ticket != null) {
                            var confirmWindow = new ConfirmWindow();
                            confirmWindow.Width = 300;
                            confirmWindow.Height = 150;
                            confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.Declaration.O.Yes");
                            confirmWindow.NoButtonText = TextCodeTranslator.Translate("Customs.Declaration.O.No");


                            confirmWindow.Show(TextCodeTranslator.Translate("Customs.Declaration.O.TicketAlreadyExist"));

                            confirmWindow.WindowClosed.subscribe((event: any) => {

                                if (confirmWindow.Yes) {
                                    this.UpdateTicket();
                                }
                            });

                        }

                    }

                    else if (certificate == null) {
                        this.UpdateTicket();

                    }
                }
                else {
                   
                    this.UpdateTicket();
                }

             

            }


            else {

                this.ValidationErrorsList = errors;

            }

        }


      
    }

    IsAllSelected: boolean;
    
    public certificateTicke: CertificateTicket
    UpdateTicket() {

        SessionLocator.SelectedSession.StartBusyIndicator("");
        this.certificateTicke  = new CertificateTicket();
        this.certificateTicke.DeclarationId = this.declarationId;
        this.certificateTicke.InvoiceNumber = null;
        this.certificateTicke.AttachmentTypeCode = this.AttachmentTypeCode;
        this.certificateTicke.CertificateNumber = this.CertificateNumber;
        this.certificateTicke.ResConfirmationTypeCode = this.ResConfirmationTypeCode;
        this.certificateTicke.CertificateExemptionTypeCode = this.CertificateExemptionTypeCode;
        this.certificateTicke.ReqConfirmationTypeCode = this.ticket.ReqConfirmationTypeCode;
        this.certificateTicke.oldAttachment = this.oldAttachment;
        this.certificateTicke.oldCertificateExempt = this.oldCertificateExempt;
        this.certificateTicke.oldCertificateNumber = this.oldCertificateNumber;
        this.certificateTicke.oldResConfirmation = this.oldResConfirmation;
        this.certificateTicke.IsAllSelected = this.IsAllSelected;

        this.certificateTicke.ExternalCertificatCode = this.ticket.ExternalCertificatCode;// Itzik :  Response.ExternalCertificatCode  from  UnifreightCertificateCallbackAction

        this.certificateTicke.SelectedItems = [];
        //if (!this.certificateTicke.IsAllSelected) {
        if (!AppTool.IsNullOrEmpty(this.connectedItems) && this.connectedItems.length > 0) {

            this.certificateTicke.SelectedItems = this.connectedItems;

            //   this.certificateTicke.SelectedItems = this.Items;

            this.certificateTicke.ConnectedItemsKeys = "";
            if (this.certificateTicke.SelectedItems) {
                this.certificateTicke.SelectedItems.forEach((item) => {
                    this.certificateTicke.ConnectedItemsKeys = this.certificateTicke.ConnectedItemsKeys + "," + item.DeclarationId + ";" + item.InvoiceCounterKey + ";" + item.LineNumber + ";" + item.ItemCertificateCounterKey;
                });
                this.certificateTicke.ConnectedItemsKeys = this.certificateTicke.ConnectedItemsKeys.substr(1, this.certificateTicke.ConnectedItemsKeys.length - 1);
            }
        }
    
        this.certificateTicke.ExcludedItemsKeys = "";
        if (!AppTool.IsNullOrEmpty(this.ExcludedItems)) {
            this.ExcludedItems.forEach((item) => {
                this.certificateTicke.ExcludedItemsKeys = this.certificateTicke.ExcludedItemsKeys + "," + item.DeclarationId + ";" + item.InvoiceCounterKey + ";" + item.LineNumber + ";" + item.ItemCertificateCounterKey;
            });
            this.certificateTicke.ExcludedItemsKeys = this.certificateTicke.ExcludedItemsKeys.substr(1, this.certificateTicke.ExcludedItemsKeys.length - 1);
        }
        this.multiCertificatesService.PutCertificateTickets(this.certificateTicke)
            .subscribe((response: ServiceResponse) => {
                if (!response.HasError) {

                    var entityParams: RelatedEntityParams = new RelatedEntityParams()
                    entityParams.ParentEntityCode = 'Declaration';
                    entityParams.ParentEntityId = this.declarationId;

                    this.GetGeneratedCustomTicketAndPointer(entityParams, this.documentTypeId, this.documentFilingId)



                    SessionLocator.SelectedSession.StopBusyIndicator();
                    SessionLocator.SelectedSession.CloseCurrentWindowEmit("ok");

                    //SessionLocator.SelectedSession.CloseCurrentWindow();
                }
            });
    }
    SearchMethod() {
        
        let sub = AmitalGatewayUtil.Instance.UnifaceRequestArrived
            .subscribe(

            (myUnifreightMessageM: UnifreightMessageM) => {
                if (
                    myUnifreightMessageM.LogitudeViewModel == "CreateEditTicketComponent" &&
                    (myUnifreightMessageM.LogitudeEntity == "Customs.Declaration" || myUnifreightMessageM.LogitudeEntity == "Declaration") &&
                    myUnifreightMessageM.LogitudeEntityNumber == this.Parent.DeclarationPM.Id) {
                    sub.unsubscribe();
                    SessionLocator.SelectedSession.StopBusyIndicator();
                    this.UnifreightCertificateCallbackAction(myUnifreightMessageM);


                }

            });
        SessionLocator.SelectedSession.StartBusyIndicator("Loading ...");
        
        this._CardListService.getSingle(this.Parent.DeclarationPM.CustomerId)
            .subscribe((res:any) => {
                let cardList: CardList = res.Result;
                let unifaceCustId: string = ""
                if (!AppTool.IsNullOrEmpty(cardList)) {
                    unifaceCustId = cardList.Code;
                    //alert(unifaceCustId);
                }
                AmitalGatewayUtil.Instance
                    .ShowDeclarationCertificatesByGroups(
                    this.Parent.DeclarationPM.CustomFileNo,
                    this.Parent.DeclarationPM.Id,
                    "CreateEditTicketComponent",//this.GetType().Name,
                    unifaceCustId);
            });
    }
    private UnifreightCertificateCallbackAction(unifreightMessageM: UnifreightMessageM) {

        //if ($instanceparent($instanceparent($instanceparent)) == "GGGQWBLOGITUDE")
        //    ;;; $$GGG_OUT = ""
        //PutItem / id v_Cert_Details , "Response.ExternalCertificatCode", SERIAL_NO.GCRCRTF
        //PutItem / id v_Cert_Details, "Response.CertificateNumber", CERTIFICATE_NO.GCRCRTF
        //PutItem / id v_Cert_Details, "Response.ResponseConfirmationTypeCode", CERTIFICATE_TYPE.GCRCRTF
        //PutItem / id v_Cert_Details, "Response.AttachmentTypeCode", cert_source.GCRCRTF
        //PutItem / Id $$GGG_OUT, "Certificate_Details", v_Cert_Details

        //endif

        let sAttachmentTypeCode: string
            = UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.AttachmentTypeCode");
        let sCertificateNumber: string
            = UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.CertificateNumber");
        //                   CERTIFICATE_NO
        let sResponseConfirmationTypeCode: string
            = UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.ResponseConfirmationTypeCode");
            //CERTIFICATE_TYPE
        let sExternalCertificatCode: string = 
            UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.ExternalCertificatCode");
        //  SERIAL_NO

          this.documentFilingId 
            = UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.DocumentFilingId");

          this.documentTypeId 
            = UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.DocumentTypeId");



        if (AppTool.IsNullOrEmpty(sExternalCertificatCode)) {
            console.log("sExternalCertificatCode is null - u did not choose any Certificate")
            return;
        }
        sResponseConfirmationTypeCode = sResponseConfirmationTypeCode.replace("0", "");
        //if (AppTool.IsNullOrEmpty(sResponseConfirmationTypeCode)) {
        //    return;
        //}

        //this.FilterItemClicked(sAttachmentTypeCode);
        this.AttachmentTypeCode =this.ticket.AttachmentTypeCode = sAttachmentTypeCode;


        var funcSetTicketAndOkClick = () => {
            this.CertificateNumber = this.ticket.CertificateNumber = sCertificateNumber;
            //this.CertificateExemptionTypeCode =
            this.ResConfirmationTypeCode = this.ticket.ResConfirmationTypeCode = sResponseConfirmationTypeCode;
            //ticket.CertificateExemptionTypeCode = "";
            this.ticket.ExternalCertificatCode = sExternalCertificatCode;

            this.OkButtonClicked();
        };
        if (AppTool.IsNullOrEmpty(sResponseConfirmationTypeCode)) {
            funcSetTicketAndOkClick();
            return;
        }
        SessionLocator.SelectedSession.StartBusyIndicator("Loading ...");
        
        this._ConfirmationTypeListService.getSingle(sResponseConfirmationTypeCode)
            .subscribe((res:any) => {
                this.currentSession.StopBusyIndicator();
                let myConfirmationTypeList :ConfirmationTypeList=res.Result;
                if (AppTool.IsNullOrEmpty(myConfirmationTypeList)) {
                    let msg = new MessageWindow();
                    
                    msg.Width = 350;
                    msg.Show(`סוג אישור לא קיים במערכת (${sResponseConfirmationTypeCode})`);

                    return;
                }
                funcSetTicketAndOkClick();

                //this.CertificateNumber = this.ticket.CertificateNumber = sCertificateNumber;
                ////this.CertificateExemptionTypeCode =
                //this.ResConfirmationTypeCode =this.ticket.ResConfirmationTypeCode = sResponseConfirmationTypeCode;
                ////ticket.CertificateExemptionTypeCode = "";
                //this.ticket.ExternalCertificatCode = sExternalCertificatCode;

                //this.OkButtonClicked();
        });

        
        
    }


}
