import { Component } from '@angular/core';
import { BaseComponent } from '../../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { AppTool } from '../../../../../../Infrastructure/Tools';
import { SupplierInvoiceItemPM } from '../../../../../../Customs/EntityPMs/SupplierInvoiceItemPM';
import { SupplierInvoicePM } from '../../../../../../Customs/EntityPMs/SupplierInvoicePM';
import { SupplierInvioceItemCertificatPM } from '../../../../../../Customs/EntityPMs/SupplierInvioceItemCertificatPM';
import { SessionLocator } from '../../../../../../Infrastructure/Utilities/SessionLocator';
import { ConfirmWindow } from '../../../../../../Controls/Windows/ConfirmWindow';
import { TextCodeTranslator } from '../../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { SupplierInvoiceItemsProdIdentPM } from '../../../../../../Customs/EntityPMs/SupplierInvoiceItemsProdIdentPM';
import { ObservableCollection } from '../../../../../../Infrastructure/Utilities/ObservableCollection';
import { ProductIdentificationTypeListService } from '../../../../../../Customs/Services/StandardLists/ProductIdentificationTypeListService';
import { ProductIdentificationTypeList } from '../../../../../../Customs/EntityLists/ProductIdentificationTypeList';
import { ConfirmationTypePM } from '../../../../../../Customs/EntityPMs/ConfirmationTypePM';
import { AttachmentTypePM } from '../../../../../../Customs/EntityPMs/AttachmentTypePM';
import { CertificateExemptionTypePM } from '../../../../../../Customs/EntityPMs/CertificateExemptionTypePM';
import { CertificateExemptionTypeListService } from '../../../../../../Customs/Services/StandardLists/CertificateExemptionTypeListService';
import { EntityResourceService } from '../../../../../../Infrastructure/Services/EntityResourceService';
import { SupplierInvoicePMService } from '../../../../../../Customs/Services/StandardPMs/SupplierInvoicePMService';
import { ApiQueryFilters, FilterItem } from '../../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { FeatureLocator } from '../../../../../../Infrastructure/Utilities/FeatureLocator';
import { AttachmentTypeListService } from '../../../../../../Customs/Services/StandardLists/AttachmentTypeListService';
import { ConfirmationTypeListService } from 'Customs/Services/StandardLists/ConfirmationTypeListService';
declare var window: any;

@Component({

    templateUrl: './SupplierInvoiceItemCertificatesComponent.html',
})

export class SupplierInvoiceItemCertificatesComponent extends BaseComponent {
    public ObjectTableName: string = "Customs.SupplierInvioceItemCertificat";
    public DataContext = this;
    public invoiceItemPM: SupplierInvoiceItemPM;
    public invoice: SupplierInvoicePM;
    public ItemsSource: ObservableCollection;
    public productIdentificationTypeListService: ProductIdentificationTypeListService = new ProductIdentificationTypeListService();
    public supplierInvoicePMService: SupplierInvoicePMService = new SupplierInvoicePMService();

    public ValidationErrorsList: string[] = [];
    public OriginalItemPM: SupplierInvoiceItemPM;
    public ClonedItemPM: SupplierInvoiceItemPM;
    FIELD_IS_REQUIERD: string;
    IsDisplayOnly: boolean;
    IsHeaderVisible: boolean = false;
    IsFromCustomsAnswers: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    confirmationTypeFilter: ApiQueryFilters = null as any;


    constructor() {
        super();
        this.ItemsSource = new ObservableCollection([]);
        this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var table = window.ObjectTables.filter(d => d.Name === 'Customs.Declaration')[0];
        var ikeaFeature = FeatureLocator.Features.filter(f => (f.Code == "IKEA") && f.ObjectTableId == table.Id)[0];
        if (ikeaFeature) {
            this.IKEAFeature = 'visibile';
        }

        this.initConfirmationTypeFilter();
    }
    IKEAFeature: string = 'hidden';
    LineNumber: string;
    public TypeCodeFilterItems: ApiQueryFilters;
    allowExport: boolean;
    SetWindowArgs(args: any) {
        var decPM = SessionLocator.SelectedSession.CurrentEditComponent.EntityPM;
        this.TypeCodeFilterItems = new ApiQueryFilters();
        if (decPM.direction == "I") {
            this.TypeCodeFilterItems.addAdditionalFilter("IsImportDeclaration", true, null, null, "Equals", false, false, false, "boolean");
        }
        if (decPM.direction == "E") {
            this.TypeCodeFilterItems.addAdditionalFilter("IsExportDeclaration", true, null, null, "Equals", false, false, false, "boolean");
        }
        var _entityResourceService: EntityResourceService = new EntityResourceService();
        _entityResourceService.getEntityResourceByTableName("Customs.CertificateExemptionType", 0).subscribe((res: any) => {
            var entityListService: CertificateExemptionTypeListService = new CertificateExemptionTypeListService();
            entityListService.getAllFromCache().subscribe((res: any) => {
            });
        });

        _entityResourceService.getEntityResourceByTableName("Customs.AttachmentType", 0).subscribe((res: any) => {
            var listService: AttachmentTypeListService = new AttachmentTypeListService();
            listService.getAllFromCache().subscribe((res: any) => {
            });
        });

        if (!AppTool.IsNullOrEmpty(args)) {
            this.invoiceItemPM = args.SupplierInvoiceItemPM;
            this.BuildCertificatesList();
            this.IsDisplayOnly = args.IsDisplayOnly;
            this.OriginalItemPM = args.SupplierInvoiceItemPM;
            this.allowExport = args.allowExport;
            this.ClonedItemPM = this.CloneEntity(args.SupplierInvoiceItemPM);
            if (this.IsDisplayOnly) {
                this.UIProperties.SetEnabled("CatalogNumber", "Customs.SupplierInvioceItemCertificat", false);

            }
            var identification = this.invoiceItemPM.SupplierInvoiceItemsProdIdents.filter(d => d.TypeCode == "MN")[0];
            if (identification != null) {
                this.CatalogNumber = identification.Identification;
            }
            if (!AppTool.IsNullOrEmpty(args.DeclarationError)) {
                this.IsFromCustomsAnswers = true;
                this.IsHeaderVisible = true;

                this.invoice = args.SupplierInvoicePM;
                this.InvoiceNumber = args.InvoiceNumber;

                //Select a line
                this.LineNumber = args.LineNumber;
                this.LineNumber = this.LineNumber.split(",")[2];
                var selectedRow = this.ItemsSource.Collection.find(d => d.SequenceNumeric == this.LineNumber);
                this.SelectedRow = selectedRow;

                this.ShowXMLErrors(args.DeclarationError);
            }
            if (!AppTool.IsNullOrEmpty(args.AmendmentView)) {
                this.IsFromCustomsAnswers = true;
                this.IsHeaderVisible = true;

                this.invoice = args.SupplierInvoicePM;
                this.InvoiceNumber = args.InvoiceNumber;

                //Select a line
                //this.LineNumber = args.LineNumber;
                //this.LineNumber = this.LineNumber.split(",")[0];
                //var selectedRow = this.ItemsSource.Collection.find(d => d.SequenceNumeric == this.LineNumber);
                //this.SelectedRow = selectedRow;

                this.ShowXMLCorrections(args.AmendmentView);
            }

        }
    }


    initConfirmationTypeFilter() {
        this.confirmationTypeFilter = new ApiQueryFilters();
        var decPM = SessionLocator.SelectedSession.CurrentEditComponent.EntityPM;
        if (decPM.direction == "E") {
            this.confirmationTypeFilter.addAdditionalFilter("IsImport", false, null, null, "Equals", false, false, false, "Boolean");
        }
        else {
            this.confirmationTypeFilter.addAdditionalFilter("IsImport", true, null, null, "Equals", false, false, false, "Boolean");
        }
    }


    ShowXMLErrors(error) {
        if (!AppTool.IsNullOrEmpty(error.Field)) {
            this.UIProperties.SetValidity(error.Field, this.ObjectTableName, false, error.Description);
        }

        var errors = [];
        if (!AppTool.IsNullOrEmpty(error.Description)) {
            var xmlErrors: any[] = error.Description.split(/,|:/);
            for (var xmlError of xmlErrors) {
                errors.push(xmlError);
            }
            this.ValidationErrorsList = [];
            this.ValidationErrorsList = errors;
        }
        if (error.EntityName != null) {
            if (error.EntityName.toLowerCase() == "supplierinvoiceitem") {
                //if (OnShowXMLErrors != null) {
                //    OnShowXMLErrors(new OnShowXMLErrorEvenArgs() { SupplierInvoiceItem = InvoiceItemsObslist.Where(d => d.SequenceNumeric == error.Line).FirstOrDefault(), });
                //}
            }
        }
    }
    ShowXMLCorrections(error) {
        if (!AppTool.IsNullOrEmpty(error.Field)) {
            this.UIProperties.SetValidity(error.Field, "Customs.SupplierInvioceItemCertificat", false, error.ErrorType);
        }
        var errors = [];
        errors.push(error.ErrorType);
        this.ValidationErrorsList = errors;
    }

    get CatalogNumber() { return this.invoiceItemPM.CatalogNumber; }
    set CatalogNumber(value: string) {
        if (this.invoiceItemPM.CatalogNumber != value) {
            this.invoiceItemPM.CatalogNumber = value;

        }
    }

    get ClassificationCode() { return this.invoiceItemPM.ClassificationCode; }
    set ClassificationCode(value: string) {
        if (this.invoiceItemPM.ClassificationCode != value) {
            this.invoiceItemPM.ClassificationCode = value;

        }
    }

    invoiceNumber: string;
    get InvoiceNumber() { return this.invoiceNumber; }
    set InvoiceNumber(value: string) {
        if (this.invoiceNumber != value) {
            this.invoiceNumber = value;

        }
    }


    BuildCertificatesList() {
        this.ItemsSource.Clear();
        for (let item of this.invoiceItemPM.SupplierInvioceItemCertificats) {
            this.ItemsSource.Insert(new InvoiceItemCertificateLine(item, this));
        }


    }

    Add() {
        if (!this.IsDisplayOnly) {
            var counter: number = 0;


            if (this.invoiceItemPM.SupplierInvioceItemCertificats.length > 0) {

                var items = this.invoiceItemPM.SupplierInvioceItemCertificats.sort((a, b) => { return (a.LineNumber === b.LineNumber) ? 0 : (a.LineNumber < b.LineNumber) ? -1 : 1 });
                if (items.length == 0) counter = 0;
                else {
                    counter = items[this.invoiceItemPM.SupplierInvioceItemCertificats.length - 1].LineNumber;
                }


            }

            counter += 1;


            var sequence: number = 0;

            if (this.invoiceItemPM.SupplierInvioceItemCertificats.length > 0) {
                var items = this.invoiceItemPM.SupplierInvioceItemCertificats.sort((a, b) => { return (a.SequenceNumeric === b.SequenceNumeric) ? 0 : (a.SequenceNumeric < b.SequenceNumeric) ? -1 : 1 });
                if (items.length == 0) sequence = 0;
                else {
                    sequence = items[this.invoiceItemPM.SupplierInvioceItemCertificats.length - 1].SequenceNumeric;
                }

            }


            sequence += 1;

            var item: SupplierInvioceItemCertificatPM = new SupplierInvioceItemCertificatPM(this.invoiceItemPM);

            item.DeclarationId = this.invoiceItemPM.DeclarationId,
                item.Tenant = this.invoiceItemPM.Tenant;
            item.InvoiceCounterKey = this.invoiceItemPM.CounterKey,
                item.LineNumber = this.invoiceItemPM.LineNumber,
                item.ItemCertificateCounterKey = counter,
                item.SequenceNumeric = sequence


            if (!this.invoiceItemPM.SupplierInvioceItemCertificats.includes(item)) {
                this.invoiceItemPM.AddSupplierInvioceItemCertificat(item);
                this.ItemsSource.Insert(new InvoiceItemCertificateLine(item, this));

            }

        }
        //    this.BuildCertificatesList();



    }



    CloneEntity(entityToClone: SupplierInvoiceItemPM) {

        var clonedEntity: SupplierInvoiceItemPM;
        clonedEntity = new SupplierInvoiceItemPM(entityToClone.EntityParentPM);

        this.MapEntitytoEntity(entityToClone, clonedEntity);


        clonedEntity.SupplierInvioceItemCertificats = [];
        entityToClone.SupplierInvioceItemCertificats.forEach((itemMod) => {
            var clonedItemMod = new SupplierInvioceItemCertificatPM(itemMod.EntityParentPM);
            this.MapEntitytoEntity(itemMod, clonedItemMod);
            clonedEntity.SupplierInvioceItemCertificats.push(clonedItemMod);
        });




        return clonedEntity;
    }

    RejectChanges() {
        this.MapEntitytoEntity(this.ClonedItemPM, this.OriginalItemPM, true);
    }

    MapEntitytoEntity(srcEntity: any, targetEntity: any, takeKeysFromTarget: boolean = false) {
        var keys;
        keys = Object.keys(takeKeysFromTarget ? targetEntity : srcEntity);
        for (var key in keys) {
            var property = keys[key];
            targetEntity[property] = srcEntity[property];
        }
    }
    CancelButtonClicked() {


        if (this.invoiceItemPM.IsDirty && !this.IsDisplayOnly && !this.IsFromCustomsAnswers) {
            var confirm = new ConfirmWindow();

            confirm.YesButtonText = TextCodeTranslator.Translate("General.B.Yes");

            confirm.ShowNoButton = true;
            confirm.Show(TextCodeTranslator.Translate("Customs.Declaration.O.Cancel"));
            confirm.WindowClosed.subscribe((event: any) => {
                if (confirm.Yes) {
                    confirm.Close();
                    this.OkButtonClicked();



                }
                else {
                    this.RejectChanges();
                    this.CurrentSession.CloseCurrentWindow();
                }

            });

        }
        else {
            this.CurrentSession.CloseCurrentWindowEmit('cancel');
        }



    }

    isValid: boolean;
    inValid: boolean;
    hasRequest: boolean;
    notMandatoryIsNotEmpty: boolean = false;
    OkButtonClicked() {
        this.ValidationErrorsList = [];
        var errors: string[] = [];
        this.isValid = true;
        this.inValid = false;
        const decPM = SessionLocator.SelectedSession.CurrentEditComponent.EntityPM;
        const isExport: boolean = decPM.direction == 'E'

        for (let item of this.invoiceItemPM.SupplierInvioceItemCertificats) {
            if (!AppTool.IsNullOrEmpty(item.ApprovalRequestNumber)) {
                this.hasRequest = true;
            }
            if (item.AttachmentTypeCode == null) {
                //var translatedRequiredError: string = TextCodeTranslator.Translate("General.M.FieldIsRequired");
                //var fieldError: string = translatedRequiredError.replace("%FieldName", "AttachmentTypeCode");
                errors.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("Customs.SupplierInvioceItemCertificat.F.AttachmentTypeCode")));




                this.isValid = false;
                break;
            }


            else {
                if (item.AttachmentTypeCode == "1" || item.AttachmentTypeCode == "2") {
                    if (AppTool.IsNullOrEmpty(item.CertificateNumber) || AppTool.IsNullOrEmpty(item.ReqConfirmationTypeCode) || AppTool.IsNullOrEmpty(item.ResConfirmationTypeCode)) {
                        this.isValid = false;
                        this.inValid = true;
                    }
                    if (!AppTool.IsNullOrEmpty(item.CertificateExemptionTypeCode) || (!isExport && !AppTool.IsNullOrEmpty(item.CustomsAttachmentID))) {
                        this.inValid = true;
                        this.notMandatoryIsNotEmpty = true;
                    }

                }

                else {
                    if (item.AttachmentTypeCode == "4") {
                        if (AppTool.IsNullOrEmpty(item.CertificateExemptionTypeCode) || AppTool.IsNullOrEmpty(item.ReqConfirmationTypeCode)) {
                            this.isValid = false;
                            this.inValid = true;
                        }

                        if (!AppTool.IsNullOrEmpty(item.CertificateNumber) || !AppTool.IsNullOrEmpty(item.ResConfirmationTypeCode) || (!isExport && !AppTool.IsNullOrEmpty(item.CustomsAttachmentID))) {
                            this.inValid = true;
                            this.notMandatoryIsNotEmpty = true;

                        }
                    }

                }
            }


        }

        if (this.inValid) {
            this.isValid = false;

            var confirm = new ConfirmWindow();


            confirm.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
            confirm.ShowNoButton = true;
            if (this.notMandatoryIsNotEmpty) {
                confirm.Show(TextCodeTranslator.Translate("Customs.Declaration.O.CertificateNotMandatoryFields"));
            }
            else {
                confirm.Show(TextCodeTranslator.Translate("Customs.Declaration.O.CertificateMandatoryFields"));
            }


            confirm.WindowClosed.subscribe((event: any) => {
                if (confirm.Yes) {



                    if (errors.length == 0) {
                        if (this.hasRequest) {
                            this.invoiceItemPM.CertificatesStatusCode = "4";
                        }
                        else {
                            this.invoiceItemPM.CertificatesStatusCode = "2";
                        }
                        if (!AppTool.IsNullOrEmpty(this.CatalogNumber)) {
                            this.AddIdentification();
                        }

                        if (this.IsFromCustomsAnswers) {
                            //SaveValidCertificateEvent saveValidCertificateEvent = SessionLocator.CurrentAssemblyLocator.EventAggregator.GetEvent<SaveValidCertificateEvent>();
                            //saveValidCertificateEvent.Publish(new SaveValidCertificateEventArgs() { Valid = true });

                            this.supplierInvoicePMService.update(this.invoice).subscribe((response: any) => {
                                var result = response.Result;
                                console.log("[response/supplierInvoicePMService.update]", result);
                                this.CurrentSession.CloseCurrentWindow();
                                if (!AppTool.IsNullOrEmpty(result)) {

                                } else {
                                }
                            });

                        } else {
                            this.CurrentSession.CloseCurrentWindow();
                        }

                        //this.CurrentSession.CloseCurrentWindow();
                    }
                    else {
                        this.ValidationErrorsList = errors;
                    }

                    confirm.Close();

                }

            });


        }

        else {


            if (errors.length == 0) {


                if (this.invoiceItemPM.SupplierInvioceItemCertificats.length > 0) {
                    if (this.hasRequest) {
                        this.invoiceItemPM.CertificatesStatusCode = "3";
                    }
                    else {
                        this.invoiceItemPM.CertificatesStatusCode = "1";
                    }
                }
                else {
                    this.invoiceItemPM.CertificatesStatusCode = null;
                }

                if (!AppTool.IsNullOrEmpty(this.CatalogNumber)) {
                    this.AddIdentification();
                }


                if (this.IsFromCustomsAnswers) {

                    this.supplierInvoicePMService.update(this.invoice).subscribe((response: any) => {
                        var result = response.Result;
                        console.log("[response/supplierInvoicePMService.update]", result);
                        this.CurrentSession.CloseCurrentWindow();
                        if (!AppTool.IsNullOrEmpty(result)) {

                        } else {
                        }
                    });

                }
                else {
                    this.CurrentSession.CloseCurrentWindow();
                }

            }

            else {
                this.ValidationErrorsList = errors;
            }


        }



        return this.isValid;




    }

    public SelectedRow: any = null;
    OnRowSelected(itemComponent: any) {
        this.SelectedRow = itemComponent;
    }

    OnRowEnded($event) {
        console.log("this.ItemsSource.Length : " + this.ItemsSource.Length);
        if (($event) == this.ItemsSource.Length) {
            this.Add();

        }
    }

    OnFocus() {
        if (this.ItemsSource.Length == 0) {
            this.Add();
        }
    }

    AddIdentification() {

        var line: number = 0;
        var count: number = this.invoiceItemPM.SupplierInvoiceItemsProdIdents.length;
        if (this.invoiceItemPM.SupplierInvoiceItemsProdIdents.length > 0) {

            var identifications = this.invoiceItemPM.SupplierInvoiceItemsProdIdents.sort(d => d.LineNumber);
            line = identifications[count - 1].LineNumber;
        }

        line += 1;

        var item: SupplierInvoiceItemsProdIdentPM = new SupplierInvoiceItemsProdIdentPM(this.invoiceItemPM);

        item.DeclarationId = this.invoiceItemPM.DeclarationId;
        item.Tenant = this.invoiceItemPM.Tenant;
        item.InvoiceCounterKey = this.invoiceItemPM.CounterKey;
        item.InvoiceItemLineNumber = this.invoiceItemPM.LineNumber;
        item.LineNumber = line,
            item.Identification = this.CatalogNumber;
        item.TypeCode = "MN";
        this.productIdentificationTypeListService.getSingle(item.TypeCode).subscribe((response: any) => {

            var result: ProductIdentificationTypeList;
            result = response.Result;

            item.TypeName = result.LocalName;
        });




        var productIdent: SupplierInvoiceItemsProdIdentPM = this.invoiceItemPM.SupplierInvoiceItemsProdIdents.find(m => m.TypeCode == "MN");

        if (productIdent != null) {
            if (productIdent.Identification == null) {
                productIdent.Identification = this.CatalogNumber;
            }
        }

        else {
            this.invoiceItemPM.AddSupplierInvoiceItemsProdIdent(item);

        }



    }

}

export class InvoiceItemCertificateLine extends BaseComponent {
    public DataContext = this;
    public ObjectTableName: string = "Customs.SupplierInvioceItemCertificat";
    public entityPM: SupplierInvioceItemCertificatPM;
    public invoiceItem: SupplierInvoiceItemPM;
    public parent: SupplierInvoiceItemCertificatesComponent;
    constructor(EntityPM: SupplierInvioceItemCertificatPM, Parent: SupplierInvoiceItemCertificatesComponent) {
        super();
        this.entityPM = EntityPM;
        this.parent = Parent;
        var confirmationTypeService = new ConfirmationTypeListService();
        confirmationTypeService.getSingleFromCache(EntityPM.ReqConfirmationTypeCode).subscribe((req: any) => {
            if (req.Result != null) {
                this.ConfirmationType = req.Result;
            }
        });
        confirmationTypeService.getSingleFromCache(EntityPM.ResConfirmationTypeCode).subscribe((req: any) => {
            if (req.Result != null) {
                this.ResConfirmationType = req.Result;
            }
        });
        var certificateExemptionTypeService = new CertificateExemptionTypeListService();
        certificateExemptionTypeService.getSingleFromCache(EntityPM.CertificateExemptionTypeCode).subscribe((req: any) => {
            if (req.Result != null) {
                this.CertificateExemptionType = req.Result;
            }
        });
        var attachmentTypeService = new AttachmentTypeListService();
        attachmentTypeService.getSingleFromCache(EntityPM.AttachmentTypeCode).subscribe((req: any) => {
            if (req.Result != null) {
                this.AttachmentType = req.Result;
            }
        });
      
    }

    //#region properties


    get SequenceNumeric() { return this.entityPM.SequenceNumeric; }
    set SequenceNumeric(value: number) {
        this.entityPM.SequenceNumeric = value;
    }
    confirmationType: ConfirmationTypePM;
    get ConfirmationType() { return this.confirmationType; }
    set ConfirmationType(value: ConfirmationTypePM) {
        if (this.confirmationType != value) {
            this.confirmationType = value;
        }
        if (!AppTool.IsNullOrEmpty(value)) {
            this.ConfirmationTypeName = value.LocalName;


        } else {
            this.ConfirmationTypeCode = null;
            this.ConfirmationTypeName = null;
        }
    }


    attachmentType: AttachmentTypePM;
    get AttachmentType() { return this.attachmentType; }
    set AttachmentType(value: AttachmentTypePM) {

        if (this.attachmentType != value) {
            this.attachmentType = value;
        }
        if (!AppTool.IsNullOrEmpty(value)) {
            this.AttachmentTypeName = value.LocalName;


        } else {
            this.AttachmentTypeCode = null;
            this.AttachmentTypeName = null;
        }
    }



    certificateExemptionType: CertificateExemptionTypePM;
    get CertificateExemptionType() { return this.certificateExemptionType; }
    set CertificateExemptionType(value: CertificateExemptionTypePM) {

        if (this.certificateExemptionType != value) {
            this.certificateExemptionType = value;
        }
        if (!AppTool.IsNullOrEmpty(value)) {
            this.CertificateExemptionTypeName = value.LocalName;


        } else {
            this.CertificateExemptionTypeCode = null;
            this.CertificateExemptionTypeName = null;
        }
    }


    resConfirmationType: ConfirmationTypePM;
    get ResConfirmationType() { return this.resConfirmationType; }
    set ResConfirmationType(value: ConfirmationTypePM) {

        if (this.resConfirmationType != value) {
            this.resConfirmationType = value;
        }
        if (!AppTool.IsNullOrEmpty(value)) {
            this.ResConfirmationTypeName = value.LocalName;


        } else {
            this.ResConfirmationTypeCode = null;
            this.ResConfirmationTypeName = null;
        }
    }


    get ConfirmationTypeCode() { return this.entityPM.ReqConfirmationTypeCode; }
    set ConfirmationTypeCode(value: string) {
        if (this.entityPM.ReqConfirmationTypeCode != value) {
            this.entityPM.ReqConfirmationTypeCode = value;

        }
    }

    get ConfirmationTypeName() { return this.entityPM.ReqConfirmationTypeName; }
    set ConfirmationTypeName(value: string) {
        if (this.entityPM.ReqConfirmationTypeName != value) {
            this.entityPM.ReqConfirmationTypeName = value;

        }
    }

    get ResConfirmationTypeCode() { return this.entityPM.ResConfirmationTypeCode; }
    set ResConfirmationTypeCode(value: string) {
        if (this.entityPM.ResConfirmationTypeCode != value) {
            this.entityPM.ResConfirmationTypeCode = value;

        }
    }

    get ResConfirmationTypeName() { return this.entityPM.ResConfirmationTypeName; }
    set ResConfirmationTypeName(value: string) {
        if (this.entityPM.ResConfirmationTypeName != value) {
            this.entityPM.ResConfirmationTypeName = value;

        }
    }

    get CertificateNumber() { return this.entityPM.CertificateNumber; }
    set CertificateNumber(value: string) {
        if (this.entityPM.CertificateNumber != value) {
            this.entityPM.CertificateNumber = value;

        }
    }

    get CertificateExemptionTypeCode() { return this.entityPM.CertificateExemptionTypeCode; }
    set CertificateExemptionTypeCode(value: string) {
        if (this.entityPM.CertificateExemptionTypeCode != value) {
            this.entityPM.CertificateExemptionTypeCode = value;

        }
    }

    get CertificateExemptionTypeName() { return this.entityPM.CertificateExemptionTypeName; }
    set CertificateExemptionTypeName(value: string) {
        if (this.entityPM.CertificateExemptionTypeName != value) {
            this.entityPM.CertificateExemptionTypeName = value;

        }
    }


    get AttachmentTypeCode() { return this.entityPM.AttachmentTypeCode; }
    set AttachmentTypeCode(value: string) {
        if (this.entityPM.AttachmentTypeCode != value) {
            this.entityPM.AttachmentTypeCode = value;

        }
    }




    get AttachmentTypeName() { return this.entityPM.AttachmentTypeName; }
    set AttachmentTypeName(value: string) {
        if (this.entityPM.AttachmentTypeName != value) {
            this.entityPM.AttachmentTypeName = value;

        }
    }

    get CustomsAttachmentID() { return this.entityPM.CustomsAttachmentID; }
    set CustomsAttachmentID(value: string) {
        if (this.entityPM.CustomsAttachmentID != value) {
            this.entityPM.CustomsAttachmentID = value;

        }
    }

    get ApprovalRequestNumber() { return this.entityPM.ApprovalRequestNumber; }
    set ApprovalRequestNumber(value: string) {
        if (this.entityPM.ApprovalRequestNumber != value) {
            this.entityPM.ApprovalRequestNumber = value;

        }
    }

    get ExternalRequestTypeCode() { return this.entityPM.ExternalRequestTypeCode; }
    set ExternalRequestTypeCode(value: string) {
        if (this.entityPM.ExternalRequestTypeCode != value) {
            this.entityPM.ExternalRequestTypeCode = value;

        }
    }

    //#endregion

    DeleteButtonClicked() {

        this.parent.ItemsSource.Remove(this);
        if (this.parent.invoiceItemPM.SupplierInvioceItemCertificats.includes(this.entityPM)) {
            this.parent.invoiceItemPM.RemoveSupplierInvioceItemCertificat(this.entityPM);
        }

        //    this.parent.BuildCertificatesList();



    }


}
