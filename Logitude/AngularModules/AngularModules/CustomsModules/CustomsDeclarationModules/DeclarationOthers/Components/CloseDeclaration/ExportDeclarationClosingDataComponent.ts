declare var window: any;
import { OnInit, Component, ChangeDetectorRef } from '@angular/core';
import { ExportDeclarationClosingDataPM } from '../../../../../Customs/EntityPMs/ExportDeclarationClosingDataPM';
import { DeclarationPM } from '../../../../../Customs/EntityPMs/DeclarationPM';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';
import { ExportDeclarationClosingDataPMService } from '../../../../../Customs/Services/StandardPMs/ExportDeclarationClosingDataPMService';
import { Time } from '@angular/common';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { ConsignmentPM } from '../../../../../Customs/EntityPMs/ConsignmentPM';
import { GenericRequestParams } from '../../../../../Customs/DataContract/RequestParams/GenericRequestParams';
import { CustomSendOptionsArgs } from '../../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { CustomMessageProgressComponent, ShowProgressBarParams } from '../../../../CustomsControls/Components/CustomMessageProgressComponent';
import { DeclarationEditComponentController } from '../../../../../Customs/Controller/DeclarationEditComponentController';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { DeclarationWebService } from '../../../../../Customs/Services/WebServices/DeclarationWebService';
import { AmendmentRequestParams } from '../../../../../Customs/DataContract/RequestParams/AmendmentRequestParams';
import { AppTool, DateTool } from '../../../../../Infrastructure/Tools';
import { ExportDeclarationClosingDatasExtendPMService } from 'Customs/Services/ExtendedPMs/ExportDeclarationClosingDatasExtendPMService';
import { CustomsSettingListService } from 'Customs/Services/StandardLists/CustomsSettingListService';
import { ExportDeclarationClosingWebService } from 'Customs/Services/WebServices/ExportDeclarationClosingWebService';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';
import { CargoIdentifireTypeListService } from 'Customs/Services/StandardLists/CargoIdentifireTypeListService';
import { UnifreightController, UnifreightResponseEventArgs } from 'Customs/Controller/UnifreightController';
import { AmitalGatewayUtil, UnifreightMessageM } from 'Infrastructure/Utilities/AmitalGatewayUtil';
import { SupplierInvoiceExtendedListService } from 'Customs/Services/ExtendedLists/SupplierInvoiceExtendedListService';
import { ObservableCollection } from 'Infrastructure/Utilities/ObservableCollection';
import { CustDocsTicketWebService } from 'Customs/Services/WebServices/CustDocsTicketWebService';
import { CustomsDocumentsTicketPM } from 'Customs/EntityPMs/CustomsDocumentsTicketPM';
import { CustDocMetaDataValuesWebService } from 'Customs/Services/WebServices/CustDocMetaDataValuesWebService';
import { CustomsDocumentMetaDataValuePM } from 'Customs/EntityPMs/CustomsDocumentMetaDataValuePM';
import { SupplierInvoiceItemLine } from '../DeclarationPayment/SupplierInvoiceSelectionComponent';
import { SupplierInvoiceItemList } from 'Customs/EntityLists/Extended/SupplierInvoiceItemList';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { SupplierInvoiceModificationPM } from '../../../../../Customs/EntityPMs/SupplierInvoiceModificationPM';
import { CustomsExchangeRateExtendedPMService } from '../../../../../Customs/Services/ExtendedPMs/CustomsExchangeRateExtendedPMService';
import { forEach } from 'cypress/types/lodash';
import { DeclarationPMService } from '../../../../../Customs/Services/StandardPMs/DeclarationPMService';
import { DeclarationExtendedPMService } from '../../../../../Customs/Services/ExtendedPMs/DeclarationExtendedPMService';
import { SupplierInvoicePM } from '../../../../../Customs/EntityPMs/SupplierInvoicePM';
import { SupplierInvoicePMService } from '../../../../../Customs/Services/StandardPMs/SupplierInvoicePMService';
import { MessageWindow } from '../../../../../Controls/Windows/MessageWindow';
import { SupplierInvoiceExtendedPMService } from '../../../../../Customs/Services/ExtendedPMs/SupplierInvoiceExtendedPMService';
import { ApiQueryFilters } from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { IncotemrsFileValidationList } from '../../../../../Customs/EntityLists/IncotemrsFileValidationList';
import { IncotemrsFileValidationListService } from '../../../../../Customs/Services/StandardLists/IncotemrsFileValidationListService';
import { ConfirmWindow } from '../../../../../Controls/Windows/ConfirmWindow';
import { LogtuideTableDataService } from 'Infrastructure/Services/logtuide-table-data.service';
import { CertificateOfOriginWebService } from 'Customs/Services/WebServices/CertificateOfOriginWebService';
import { CertificateOfOriginPM } from 'Customs/EntityPMs/CertificateOfOriginPM';

@Component({
    selector: 'ExportDeclarationClosingDataComponent',
    templateUrl: './ExportDeclarationClosingDataComponent.html',
})

export class ExportDeclarationClosingDataComponent extends BaseComponent {
    public DataContext: any = this;
    public DecPM: DeclarationPM;
    public IsReadOnly: boolean = false;
    public ObjectTableName: string = "Customs.ExportDeclarationClosingData";
    public IsReady: boolean = false;
    ValidationErrors: string[] = [];
    DeclarationService: DeclarationWebService = new DeclarationWebService();;
    exportDeclarationClosingDataPMService: ExportDeclarationClosingDataPMService = new ExportDeclarationClosingDataPMService();
    declarationPMService: DeclarationPMService = new DeclarationPMService();
    declarationExtendedPMService: DeclarationExtendedPMService = new DeclarationExtendedPMService();
    supplierInvoiceExtendedPMService: SupplierInvoiceExtendedPMService = new SupplierInvoiceExtendedPMService();
    incotemrsFileValidationListService: IncotemrsFileValidationListService = new IncotemrsFileValidationListService();
    certificateOfOriginWebService: CertificateOfOriginWebService = new CertificateOfOriginWebService();
    exportDeclarationClosingDatasExtendPMService: ExportDeclarationClosingDatasExtendPMService = new ExportDeclarationClosingDatasExtendPMService();
    _CargoIdentifireTypeListService: CargoIdentifireTypeListService = new CargoIdentifireTypeListService();
    private CurrentSession = SessionLocator.SelectedSession;
    supplierInvoiceExtendedListService: SupplierInvoiceExtendedListService = new SupplierInvoiceExtendedListService();
    public SupplierInvoiceItemList: SupplierInvoiceItemList[] = [];
    public IsNew: boolean = false;
    private exportDeclarationClosingWebService: ExportDeclarationClosingWebService = new ExportDeclarationClosingWebService();

    public ActualSailingDate: string = "תאריך הפלגה בפועל";
    public ActualTakeOffDate: string = "תאריך המראה בפועל";
    public TypeCodeFilterItems: ApiQueryFilters;

    ManifestNumberPlaceholder: string = '';
    SecondCargoIdPlaceholder: string = '';
    ThirdCargoIdPlaceholder: string = '';

    constructor(
        private EntityResourceService: EntityResourceService,
        private readonly cdr: ChangeDetectorRef, public entityArgs: EntityArgs,
        private logtuideTableDataService: LogtuideTableDataService,
    ) {
        super();
        this.ModificationsList = new ObservableCollection([]);
    }

    SetUIProperty() {

        if (this.DecPM.TransportModeId != 'O')
            this.UIProperties.SetEnabled("FinalShipCode", this.ObjectTableName, false);
        this.UIProperties.SetWarning("FinalCargoTypeCode", this.ObjectTableName, true);
        this.UIProperties.SetWarning("LoadingDateTime", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("Smp", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("FlightDate", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("MainAWB", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ChargingSite", this.ObjectTableName, false);


    }


    SetWindowArgs(args: any) {
        this.EntityResourceService.getEntityResourceByTableName("Customs.ExportDeclarationClosingData").subscribe((response: any) => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceModification").subscribe((response: any) => {
                this.DecPM = args.EntityPM;
                this.declarationExtendedPMService.GetSingleFullData(this.DecPM.Id).subscribe((response1: ServiceResponse) => {
                    this.DecPM = response1.Result;
                    this.FillInvoiceNumbersList();
                    this.fillModifications();
                    this.GetExportDeclarationClosingData(this.DecPM.Id);
                });




                this.SetUIProperty();

                if (this.DecPM.IsExportClosed && (!this.DecPM.AmendmentDontDisplayInList || (this.DecPM.AmendmentDontDisplayInList && !AppTool.IsNullOrEmpty(this.DecPM.AmendmentStatus)))) {
                    this.IsReadOnly = true

                    this.setInputsReadOnly();
                }
                else {
                    if (['6', '7', '8', '10', '11'].includes(this.DecPM.ExportCloseAmendmentStatus)) {

                        this.IsReadOnly = true
                        this.setInputsReadOnly();
                    }
                }

                if (this.DecPM.Direction === 'E') {
                    this.setIdentifiersPlaceHolders();
                }


            });
        });

    }

    fillModifications() {
        for (let invoice of this.DecPM.SupplierInvoices) {
            for (let item of invoice.SupplierInvoiceModifications) {
                //if (item.TypeCode != "I02" && item.TypeCode != "67" && item.TypeCode != "104") {
                //if (this.DecPM.Direction == "E" && item.TypeCode != "160") {
                //var entityParentPM = this.DecPM.SupplierInvoices.find(x => x.InvoiceCounterKey == item.InvoiceCounterKey);
                var a = new ModificationItemModel(item, this, invoice);
                this.ModificationsList.Insert(a);
                //}
                //}
            }
        }
        this.TypeCodeFilterItems = new ApiQueryFilters();
        this.TypeCodeFilterItems.addAdditionalFilter("IsRelevantInvoiceExport", true, null, null, "Equals", false, false, false, "boolean", false, true);
        /*this.exportDeclarationClosingDatasExtendPMService.GetSupplierInvoiceModificationsForDeclaration(this.DecPM.Id).subscribe((response: any) => {

            for (let item of response.Result) {
                if (item.TypeCode != "I02" && item.TypeCode != "67" && item.TypeCode != "104") {
                    if (this.DecPM.Direction == "E" && item.TypeCode != "160") {
                        item.EntityParentPM = this.DecPM.SupplierInvoices.find(x => x.InvoiceCounterKey == item.InvoiceCounterKey);
                        var a = new ModificationItemModel(item, this);
                        this.ModificationsList.Insert(a);
                    }

                }
            }

        });*/
    }
    setInputsReadOnly() {

        this.UIProperties.SetEnabled("FinalCargoTypeCode", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("FinalSecondCargoId", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("FinalThirdCargoId", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("LoadingDateTime", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("LoadingSite", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("FinalManifestNumber", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("FinalShipCode", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("FinalLoadingSite", this.ObjectTableName, false);

    }

    GetExportDeclarationClosingData(id: string) {
        if (id != null) {

            this.exportDeclarationClosingDatasExtendPMService.GetSingleWithEFIFILEMData(id).subscribe((response: any) => {
                if (!this.DecPM.IsConnectedToUnifreight && AmitalGatewayUtil.Instance.AmitalBrowserInUse)
                    this.operationalDataFromUnifreight()
                this.EntityPM = response.Result;
                if (this.EntityPM)
                    if (response.Result.ChangeSetOp == "1") {
                        this.EntityPM.IsDirty = true;
                        this.IsNew = true;
                    } else {
                        this.EntityPM.IsDirty = false;
                    }

                if (this.DecPM.Direction === 'E' && this.DecPM.TransportModeId === 'O') {

                    if (!this.FinalShipCode)
                        this.FinalShipCode = this.DecPM.Consignments.find(x => x.ConsignmentType == "E")?.ShipCode;

                    if (this.FinalCargoTypeCode == null) {

                        this.FinalManifestNumber = this.FinalManifestNumber == null ? '' : this.FinalManifestNumber;
                        this.FinalSecondCargoId = this.FinalSecondCargoId == null ? '' : this.FinalSecondCargoId;
                        this.FinalThirdCargoId = this.FinalThirdCargoId == null ? '' : this.FinalThirdCargoId;
                        this.FinalCargoTypeCode = '37';

                    }

                    else {

                        this.setIdentifiersPlaceHolders();
                    }

                }
                else {
                    this.ManifestNumberPlaceholder = 'XXX-XXXXXXXX';
                    this.setWarningValues();
                }


                if (AppTool.IsNullOrEmpty(this.EntityPM.FinalCargoTypeCode) && this.DecPM.Direction == 'E' && this.DecPM.TransportModeId == 'A') {
                    this.EntityPM.IsDirty = true;
                    this.EntityPM.FinalCargoTypeCode = "36";
                }







                /*this.EntityPM = new ExportDeclarationClosingDataPM();
                this.EntityPM.DeclarationId = id;
                this.EntityPM.Tenant = this.DecPM.Tenant;
                var consignments = this.DecPM.Consignments.filter(x => x.ConsignmentType == 'E');
                if (consignments.length == 1) {
                    this.EntityPM.FinalCargoTypeCode = consignments[0].CargoTypeCode;
                    this.EntityPM.FinalSecondCargoId = consignments[0].SecondCargoID;
                    this.EntityPM.FinalThirdCargoId = consignments[0].ThirdCargoID;
                    this.EntityPM.FinalManifestNumber = consignments[0].ManifestNumber;
                    this.EntityPM.FinalShipCode = consignments[0].ShipCode;
                    this.EntityPM.FinalLoadingSite = consignments[0].ExportLoadingPortCode;
                }
                this.IsNew = true;*/

                this.IsReady = true;

                this.initOceanExportData();

                if (this.DecPM.TransportModeId == 'L') //TransportMod- land
                {


                    this.exportDeclarationClosingDataPMService.get(id).subscribe((response: any) => {
                        if (AppTool.IsNullOrEmpty(response.Result)) {


                            if (!AppTool.IsNullOrEmpty(this.DecPM.Consignments[0].CargoTypeCodeForExport)) this.FinalCargoTypeCode = this.DecPM.Consignments[0].CargoTypeCodeForExport;



                            if (!AppTool.IsNullOrEmpty(this.DecPM.Consignments[0].ManifestNumber)) this.FinalManifestNumber = this.DecPM.Consignments[0].ManifestNumber;



                            if (!AppTool.IsNullOrEmpty(this.DecPM.Consignments[0].ExportLoadingPortCode)) this.FinalLoadingSite = this.DecPM.Consignments[0].ExportLoadingPortCode;




                        }
                    });



                }

            });



        }
    }
    ModificationsList: ObservableCollection;
    AddModificationButton() {

        if (this.ModificationsList.Length > 0) {
            var exist = this.ModificationsList.Collection.find(d => !d.isValid);
            if (exist) {
                return;
            }
        }

        var modificationCounter = 0;
        if (this.ModificationsList.Length > 0) {
            modificationCounter = this.getMax(this.ModificationsList.Collection, "ModificationCounterKey");
        }
        modificationCounter += 1;

        var item = new SupplierInvoiceModificationPM(null);
        item.DeclarationId = this.DecPM.Id;
        item.InvoiceCounterKey = this.DecPM.SupplierInvoices[0].InvoiceCounterKey;
        item.Tenant = SessionLocator.Tenant;
        item.ChangeSetOp = "Insert";
        item.ModificationCounterKey = modificationCounter;

        this.DecPM.SupplierInvoices[0].SupplierInvoiceModifications.push(item);
        this.DecPM.SupplierInvoices[0].IsDirty = true;
        this.ModificationsList.Insert(new ModificationItemModel(item, this, this.DecPM.SupplierInvoices[0]));

    }
    RemoveModification(item: ModificationItemModel) {

        console.log("... Removing ", item);
        var invoice = this.DecPM.SupplierInvoices.find(x => x.InvoiceCounterKey == item.ModificationPM.InvoiceCounterKey);
        item.ModificationPM.ChangeSetOp = "Delete";
        item.ModificationPM.IsDirty = true;
        this.ModificationsList.Remove(item);
        invoice.IsDirty = true;

    }
    getMax(list: any[], propertyName: string) {
        var max = -99999;
        var maxObj = list && list.length > 0 ? list.reduce(function (prev, current) { return (prev[propertyName] > current[propertyName]) ? prev : current }) : null;
        if (maxObj != null)
            if (max <= maxObj[propertyName])
                max = maxObj[propertyName];
        return max;
    }
    setWarningValues() {
        if (this.DecPM.Direction === 'E') {
            this.UIProperties.SetWarning("FinalManifestNumber", this.ObjectTableName, AppTool.IsNullOrEmpty(this.FinalManifestNumber));
            this.UIProperties.SetWarning("FinalSecondCargoId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.FinalSecondCargoId) && !AppTool.IsNullOrEmpty(this.SecondCargoIdPlaceholder));
            this.UIProperties.SetWarning("FinalThirdCargoId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.FinalThirdCargoId) && !AppTool.IsNullOrEmpty(this.ThirdCargoIdPlaceholder));
        }
    }

    get LoadingDateTime() { return this.EntityPM ? this.EntityPM.LoadingDateTime : null; }
    set LoadingDateTime(value: Date) {

        if (this.EntityPM.LoadingDateTime != value) {
            this.EntityPM.LoadingDateTime = value;
            this.EntityPM.IsDirty = true;
        }

    }

    get FinalShipCode() { return this.EntityPM ? this.EntityPM.FinalShipCode : null; }
    set FinalShipCode(value: string) {
        if (this.EntityPM.FinalShipCode != value) {
            this.EntityPM.FinalShipCode = value;
            this.EntityPM.IsDirty = true;
        }
    }

    get LoadingSite() { return this.EntityPM ? this.EntityPM.LoadingDateTime : null; }
    set LoadingSite(value: Date) {
        if (this.EntityPM.LoadingDateTime != value) {
            this.EntityPM.LoadingDateTime = value;
            this.EntityPM.IsDirty = true;
        }
    }

    get FinalLoadingSite() { return this.EntityPM ? this.EntityPM.FinalLoadingSite : null; }
    set FinalLoadingSite(value: string) {
        if (this.EntityPM.FinalLoadingSite != value) {
            this.EntityPM.FinalLoadingSite = value;
            this.EntityPM.IsDirty = true;
        }
    }
    get MainAWB() { return this.EntityPM ? this.EntityPM.MAIN_AWB : null; }
    set MainAWB(value: string) {
        if (this.EntityPM.MAIN_AWB != value) {
            this.EntityPM.MAIN_AWB = value;
        }
    }
    public get FlightDate() {
        if (this.EntityPM != null && this.EntityPM.FLIGHT_DATE != null) {
            var myFormats = DateTool.GetDateFormats(this.EntityPM.FLIGHT_DATE);
            return myFormats.DateString as any;
            // + " " + myFormats.ShortTimeString;
        }
        return null;
    }
    public set FlightDate(value: Date) {
        if (this.EntityPM.FLIGHT_DATE != value)
            this.EntityPM.FLIGHT_DATE = value;
    }

    get Smp() { return this.EntityPM ? this.EntityPM.SMP : null; }
    set Smp(value: string) {
        if (this.EntityPM.SMP != value) {
            this.EntityPM.SMP = value;
        }
    }

    get ChargingSite() { return this.EntityPM ? this.EntityPM.ChargingSite : null; }
    set ChargingSite(value: string) {
        if (this.EntityPM.ChargingSite != value) {
            this.EntityPM.ChargingSite = value;
        }
    }

    get FinalCargoTypeCode() {

        return this.EntityPM ? this.EntityPM.FinalCargoTypeCode : null;
    }
    set FinalCargoTypeCode(value: string) {

        if (this.EntityPM.FinalCargoTypeCode != value) {
            this.EntityPM.FinalCargoTypeCode = value;
            this.EntityPM.IsDirty = true;

            if (this.DecPM.Direction === 'E') {
                this.setIdentifiersPlaceHolders();
            }
        }

    }

    get FinalManifestNumber() {

        return this.EntityPM ? this.EntityPM.FinalManifestNumber : null;
    }

    set FinalManifestNumber(value: string) {

        if (this.EntityPM.FinalManifestNumber != value) {
            this.EntityPM.FinalManifestNumber = value;
            this.EntityPM.IsDirty = true;


        }
    }
    get FinalSecondCargoId() { return this.EntityPM ? this.EntityPM.FinalSecondCargoId : null; }
    set FinalSecondCargoId(value: string) {
        if (this.EntityPM.FinalSecondCargoId != value) {
            this.EntityPM.FinalSecondCargoId = value;
            this.EntityPM.IsDirty = true;
        }
    }
    get FinalThirdCargoId() { return this.EntityPM ? this.EntityPM.FinalThirdCargoId : null; }
    set FinalThirdCargoId(value: string) {
        if (this.EntityPM.FinalThirdCargoId != value) {
            this.EntityPM.FinalThirdCargoId = value;
            this.EntityPM.IsDirty = true;
        }
    }

    ViewDocumentsComponent() {

        var windowArgs: any = {};
        windowArgs.EntityPM = this.DecPM;
        windowArgs.ClosingData = this.EntityPM;
        //windowArgs.ObjectTableName = "Customs.DeclarationCancellation";
        windowArgs.ObjectTableName = "Customs.Declaration";// this.ObjectTableName;
        windowArgs.EntityParentPM = "ExportDeclarationClosingData";
        //    windowArgs.SkipCtor = this.SkipCtor;
        windowArgs.IsFromStandAloneScreen = true;
        windowArgs.IsClose = true;
        var windowTitle = "Customs.Declaration.TH.Documents";

        var logWindow = new LogitudeWindow();
        logWindow.IsHideHeader = true;
        logWindow.Width = 1000;
        logWindow.Height = 700;
        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => this.OnDocumentsWindowClosed($event));
        this.entityArgs.SkipCtor = true;
        logWindow.Show('./CustomsModules/CustomsDocuments/Components/CustomsDocumentsComponent');
    }
    OnDocumentsWindowClosed(event) {
        this.entityArgs.SkipCtor = false;
    }

    async SendButtonClicked(event: CustomSendOptionsArgs) {
        if (this.ModificationsList.Length > 0) {
            let counter = 0;
            this.ModificationsList.Collection.forEach((mod) => {
                counter += 1;
                if (!AppTool.IsNullOrEmpty(mod.InvoiceNumber)) {
                    if (AppTool.IsNullOrEmpty(mod.TypeName)) {
                        var msg = `Line ${counter}- `;
                        msg += TextCodeTranslator.Translate("Customs.ExportDeclarationClosingData.O.TypeName");
                        this.ValidationErrors.push(msg);
                    }
                    if (AppTool.IsNullOrEmpty(mod.CurrencyTypeCode)) {
                        var msg = `Line ${counter}- `;
                        msg += TextCodeTranslator.Translate("Customs.ExportDeclarationClosingData.O.CurrencyTypeCode");
                        this.ValidationErrors.push(msg);
                    }
                    if (AppTool.IsNullOrEmpty(mod.Amount)) {
                        var msg = `Line ${counter}- `;
                        msg += TextCodeTranslator.Translate("Customs.ExportDeclarationClosingData.O.Amount");
                        this.ValidationErrors.push(msg);
                    }
                }
            });
        }
        if (AppTool.IsNullOrEmpty(this.EntityPM.FinalCargoTypeCode)) {

            var msg = TextCodeTranslator.Translate("Customs.SpecialActivityRequest.F.CargoIdentifierTypMandatory");

            this.ValidationErrors.push(msg);
            this.FillValidationErrors("Errors");
        }
        else {
            if (AppTool.IsNullOrEmpty(this.EntityPM.FinalManifestNumber)) {
                var msg = TextCodeTranslator.Translate("Customs.SpecialActivityRequest.F.CargoIdentifierKey1Mandatory");//" שדה מזהה מטען ראשון שדה חובה";

                this.ValidationErrors.push(msg);
                this.FillValidationErrors("Errors");
            }
            else {
                if (AppTool.IsNullOrEmpty(this.FinalSecondCargoId) && !AppTool.IsNullOrEmpty(this.SecondCargoIdPlaceholder)) {

                    var msg = TextCodeTranslator.Translate("Customs.SpecialActivityRequest.F.CargoIdentifierKey2Mandatory"); //" ×©×“×” ×ž×–×”×” ×ž×˜×¢×Ÿ ×©× ×™ ×©×“×” ×—×•×‘×”";


                    this.ValidationErrors.push(msg);
                    this.FillValidationErrors("Errors");
                }
                else {




                    if (AppTool.IsNullOrEmpty(this.EntityPM.LoadingDateTime)) {

                        var msg = TextCodeTranslator.Translate("Customs.SpecialActivityRequest.F.LoadingDateTimeMandatory");


                        this.ValidationErrors.push(msg);
                        this.FillValidationErrors("Errors");
                    }

                    else {

                        const errors = await this.ValidateModifications();
                        if (errors.length > 0) {
                            this.FillValidationErrors("Errors");
                        }
                        else {
                            if (this.ValidationErrors.length > 0) {
                                if (this.ModificationsList.Length > 0) {
                                    this.FillValidationErrors("Errors");
                                }
                                else {
                                    this.FillValidationWarnings("Warnings");
                                }
                            }
                            else
                                this.CheckDocuments();
                        }

                    }
                }
            }
        }
    }
    CheckDocuments() {

        var custDocsTicketWebService: CustDocsTicketWebService = new CustDocsTicketWebService();
        var custDocsMetadataWebService: CustDocMetaDataValuesWebService = new CustDocMetaDataValuesWebService();
        var customsDocTickets: string = "";
        var MetadataValues: CustomsDocumentMetaDataValuePM[];
        var CustomsDocumentsTickets: CustomsDocumentsTicketPM[];
        var CustomsDocumentsTickets954: CustomsDocumentsTicketPM[];

        var SupplierInvoiceNumberList = ""
        this.SupplierInvoiceItemList = [];
        this.supplierInvoiceExtendedListService.GetPreferenceDocumentNumberSupplierInvoiceItemByDeclarationId(this.DecPM.Id).subscribe((response: any) => {
            if (response) {

                response.Result.forEach(element => {
                    if (!AppTool.IsNullOrEmpty(element.PreferenceDocumentNumber))
                        this.SupplierInvoiceItemList.push(element)
                });

                if (this.SupplierInvoiceItemList.length > 0) {
                    custDocsTicketWebService.GetCustomsDocumentsTicketsByEntityIdAndChilds(this.DecPM.Id, null, null, null, "ExportDeclarationClosingData", false).subscribe((response: ServiceResponse) => {
                        CustomsDocumentsTickets = response.Result;
                        CustomsDocumentsTickets = CustomsDocumentsTickets.filter(c => !AppTool.IsNullOrEmpty(c.CustomsDocId))
                        CustomsDocumentsTickets954 = CustomsDocumentsTickets.filter(c => c.DocumentTypeCode == '954');
                        CustomsDocumentsTickets = CustomsDocumentsTickets.filter(c => c.DocumentTypeCode == 'IL_1844' || c.DocumentTypeCode == 'IL_329');
                        if (CustomsDocumentsTickets954.length > 0)
                            this.Send("ok")
                        else {
                            if (CustomsDocumentsTickets.length == 0) {
                                this.SupplierInvoiceItemList.forEach(supplierInvoiceItem => {
                                    SupplierInvoiceNumberList = SupplierInvoiceNumberList + ',' + this.DecPM.SupplierInvoices.find(s => s.InvoiceCounterKey == supplierInvoiceItem.CounterKey).InvoiceNumber;
                                });
                                this.checkCertificateOfOrigin(SupplierInvoiceNumberList);
                            }
                            else {
                                for (var i = 0; i < CustomsDocumentsTickets.length; i++) {
                                    customsDocTickets = customsDocTickets + "," + CustomsDocumentsTickets[i].DocumentsFilingId;
                                }

                                customsDocTickets = customsDocTickets.substr(1, customsDocTickets.length - 1);
                                custDocsMetadataWebService.GetCustomsDocumentMetaDataValuesByCustomsDocumentFilingIds(customsDocTickets).subscribe((response2: ServiceResponse) => {
                                    MetadataValues = response2.Result;
                                    var metaDataTypesCode = MetadataValues.filter(c => c.MetaDataTypeCode == '35' || c.MetaDataTypeCode == '46')
                                    var index = 0
                                    this.SupplierInvoiceItemList.forEach(element => {
                                        index = metaDataTypesCode.findIndex(t => t.MetaDataValue == element.PreferenceDocumentNumber);
                                        if (index < 0) {
                                            SupplierInvoiceNumberList = SupplierInvoiceNumberList + ',' + this.DecPM.SupplierInvoices.find(s => s.InvoiceCounterKey == element.CounterKey).InvoiceNumber;
                                        }
                                        if (SupplierInvoiceNumberList.length > 0) {
                                            this.ShowWarnningMessage(SupplierInvoiceNumberList);
                                            SupplierInvoiceNumberList = "";
                                        }
                                        else {
                                            this.Send("ok");
                                        }
                                    })
                                });


                            }
                        }
                    })
                }
                else {
                    this.Send("ok")
                }

            }
            else {
                this.Send("ok")
            }



        });
    }

    checkCertificateOfOrigin(SupplierInvoiceNumberList: string) {
        var amendmentOriginalDeclartation = AppTool.IsNullOrEmpty(this.DecPM?.AmendmentOriginalDeclartation) ? "" : this.DecPM?.AmendmentOriginalDeclartation;
        this.certificateOfOriginWebService.GetCertificateOfOriginByID(this.DecPM?.Id, amendmentOriginalDeclartation, this.DecPM?.Tenant).subscribe(myResult => {
            const data: CertificateOfOriginPM[] = myResult.Result;;
            if (data && data.length > 0) {
                const existMatchCertificate = data.filter(i =>
                    this.SupplierInvoiceItemList.some(item => item.PreferenceDocumentNumber === i.COONumber)
                );
                if (existMatchCertificate?.length > 0) {
                    this.Send("ok");
                    return;
                }
            }
            this.ShowWarnningMessage(SupplierInvoiceNumberList)
        });
    }

    SendAmendmentCloseDeclaration(event: CustomSendOptionsArgs) {
        this.CurrentSession.StartBusyIndicator("שליחת מסר סגירת הצהרה");
        let objecttableId = window.ObjectTables.filter(d => d.Name === 'Customs.Declaration')[0].Id;
        var searchParams: AmendmentRequestParams = new AmendmentRequestParams();
        searchParams.Tenant = SessionLocator.Tenant;
        searchParams.AppicationId = this.EntityPM.DeclarationId;
        searchParams.LoggingEnabled = true;
        searchParams.LoggingEntityId = this.EntityPM.DeclarationId;
        searchParams.LoggingEntityReference = this.DecPM.DeclarationNumber;
        searchParams.LoggingObjectTableId = objecttableId;
        searchParams.LoggingUserId = SessionLocator.LoggedUserId;
        searchParams.RequestName = "Export Amendment Declaration Request";
        searchParams.ResponseName = "Amendment Declaration Response";
        searchParams.RequestVIA = event.RequestVIA;
        searchParams.ForcePersonalSign = event.ForcePersonalSign;
        searchParams.IsExportClose = true;
        searchParams.IsTransShipment = this.DecPM.DeclarationTypeCode === '3';
        //searchParams.TestCase = event.TestCase;
        let myShowProgressBarParams: ShowProgressBarParams = null;

        CustomMessageProgressComponent.ShowProgressBar(this.CurrentSession, searchParams.PBId, "שליחת מסר סגירה", false, myShowProgressBarParams)
            .then((res) => {




            }
            ).catch((err) => {

                this.CurrentSession.StopBusyIndicator();
                this.ValidationErrors.push(err);
                this.FillValidationErrors("Errors");
            });

        this.DeclarationService.PostSendDeclarationClosingAmendment(searchParams).subscribe((response: ServiceResponse) => {

            if (!AppTool.IsNullOrEmpty(response) && !AppTool.IsNullOrEmpty(response.Result) && !AppTool.IsNullOrEmpty(response.Result.UserMessage) && response.Result.HasException) {
                //this.ValidationErrors.push(response.Result.UserMessage);
                //this.FillValidationErrors("Errors");
            }

            if (!AppTool.IsNullOrEmpty(response) && !AppTool.IsNullOrEmpty(response.Result) && (response.Result.IsExportCloseApprove || response.Result.HasException)) {


                this.CurrentSession.CurrentEditComponent.PreSelectedTabCode = "DEGC";
                this.CurrentSession.CurrentEditComponent.SetSelectedTab();
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                if (!response.Result.HasException) {
                    var tab = this.CurrentSession.CurrentEditComponent.TabsItemsSource.filter(d => d.Code == "CloD")[0];
                    tab.IsDisabled = false;
                }

            }
            //if reject
            else {
                this.CurrentSession.CurrentEditComponent.PreSelectedTabCode = "CloD";
                this.CurrentSession.CurrentEditComponent.SetSelectedTab();
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                var tab = this.CurrentSession.CurrentEditComponent.TabsItemsSource.filter(d => d.Code == "CloD")[0];
                tab.IsDisabled = false;
            }


            SessionLocator.SelectedSession.CloseCurrentWindow();
        });

    }
    ShowWarnningMessage(error: string) {
        this.CurrentSession.StopBusyIndicator();
        this.ValidationErrors.push(TextCodeTranslator.Translate("Customs.ExportClosindData.O.NoFindTrufaToPreferenceDocument") + " " + `${error}`)
        var windowArgs: any = {};
        windowArgs.Errors = this.ValidationErrors;
        windowArgs.ComponentHeight = '328px';
        windowArgs.CancelButtonVisibility = true

        var windowTitle = TextCodeTranslator.Translate("Customs.ExportClosindData.O.CheckingAttachmentCertificates");

        var logWindow = new LogitudeWindow();
        logWindow.Width = 600;
        logWindow.Height = 400;
        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = true;
        windowArgs.SaveButtonText = "שלח";

        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) =>
            this.Send($event)


        );
        logWindow.Show('./CustomsModules/CustomsControls/Components/CustomsErrorsComponent');
    }

    FillValidationErrors(title: string) {


        this.CurrentSession.StopBusyIndicator();
        var windowArgs: any = {};
        windowArgs.Errors = this.ValidationErrors;
        windowArgs.ComponentHeight = '328px';
        var windowTitle = title;
        var logWindow = new LogitudeWindow();
        logWindow.Width = 600;
        logWindow.Height = 400;
        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => this.OnAddEditWindowClosed($event));
        logWindow.Show('./CustomsModules/CustomsControls/Components/CustomsErrorsComponent');
    }
    FillValidationWarnings(title: string) {
        this.CurrentSession.StopBusyIndicator();
        var windowArgs: any = {};
        windowArgs.Warning = this.ValidationErrors;
        windowArgs.ComponentHeight = '328px';
        var windowTitle = title;
        windowArgs.SaveButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
        windowArgs.CancelButtonVisibility = true;
        var logWindow = new LogitudeWindow();
        logWindow.Width = 600;
        logWindow.Height = 400;
        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = true;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => this.OnWarningsWindowClosed($event));
        logWindow.Show('./CustomsModules/CustomsControls/Components/CustomsErrorsComponent');
    }

    OnAddEditWindowClosed(event) {

        this.ValidationErrors = [];

    }
    OnWarningsWindowClosed(event) {
        this.ValidationErrors = [];
        if (!AppTool.IsNullOrEmpty(event) && event == "ok") {
            this.CheckDocuments();
        }

    }
    Send(event) {
        this.ValidationErrors = [];
        if (event == "ok") {

            if (this.EntityPM.IsDirty) {
                if (this.IsNew) {
                    this.exportDeclarationClosingDataPMService.insert(this.EntityPM).subscribe((response: ServiceResponse) => {

                        if (!response.HasError) {
                            this.saveInvoices(true, event);
                        }
                    });
                } else {
                    this.exportDeclarationClosingDataPMService.update(this.EntityPM).subscribe((response: ServiceResponse) => {

                        if (!response.HasError) {
                            this.saveInvoices(true, event);
                        }
                    });
                }
            }
            else {

                this.saveInvoices(true, event);
            }
        }


    }


    CancelButtonClicked() {
        SessionLocator.SelectedSession.CloseCurrentWindowEmit("Cancel");
    }

    private async setFreightChargeWarning(incotermCode: string) {
        if (incotermCode) {
            const filters: ApiQueryFilters = new ApiQueryFilters();
            filters.PageIndex = 0;
            filters.PageSize = 50;
            filters.addAdditionalFilter("ENGLISHNAME", incotermCode, null, null, "Contains", false, false, false, "Text", false, false);
            filters.addAdditionalFilter("LeadDocumentTypeID", '2', null, null, "Contains", false, false, false, "Text", false, false);
            const incotemrsFileValidationList: IncotemrsFileValidationList[] = await this.logtuideTableDataService.getDataFromService(this.incotemrsFileValidationListService.getByFilters(filters))

            var isFreightCharge = incotemrsFileValidationList.some(x => x.IsFreightCharge);
            return isFreightCharge;
        }
        return false;
    }


    async OkButtonClicked() {

        if (this.ValidationErrors.length > 0)
            this.FillValidationErrors("Errors");


        /*const errors = await this.ValidateModifications();
        if (errors.length > 0) {
            this.FillValidationErrors("Errors");
            return;
        }
        if (this.ValidationErrors.length > 0)
            this.FillValidationWarnings("Warnings");
        else*/
        this.CurrentSession.CurrentEditComponent.StartBusyIndicator("×©×ž×™×¨×”");


        if (this.IsNew) {
            this.exportDeclarationClosingDataPMService.insert(this.EntityPM).subscribe((response: ServiceResponse) => {
                this.saveInvoices();
            });
        } else {
            this.exportDeclarationClosingDataPMService.update(this.EntityPM).subscribe((response: ServiceResponse) => {
                this.saveInvoices();
            });
        }

    }
    async ValidateModifications() {
        var validationErrors = [];

        var invoice = this.DecPM.SupplierInvoices[0];
        var existFreightCharge = false;
        const isFreightCharge = await this.setFreightChargeWarning(invoice.IncotermCode);
        if (this.ModificationsList.Length > 0) {


            this.ModificationsList.Collection.forEach((mod) => {

                var typeCode = mod.TypeCode;
                if (typeCode == "104") existFreightCharge = true;

                if (typeCode != "I02" && !(this.DecPM.Direction == "E" && typeCode == "160")) {
                    var exists = [];
                    exists = this.ModificationsList.Collection.filter(d => d.TypeCode == typeCode && d.InvoiceCounterKey == mod.InvoiceCounterKey);
                    if (exists.length > 1) {
                        var txt = TextCodeTranslator.Translate("Customs.Declaration.O.ExistingType");
                        if (!validationErrors.includes(txt)) {
                            validationErrors.push(txt);
                            if (!this.ValidationErrors.includes(txt)) {
                                this.ValidationErrors.push(txt);
                            }
                        }
                    }
                }
            });

        }
        if (!existFreightCharge && isFreightCharge) {
            this.ValidationErrors.push(TextCodeTranslator.Translate("Customs.General.O.NoDetailsForActualFreightAmount"));
        }



        return validationErrors;
    }

    saveInvoices(fromSend = false, event: CustomSendOptionsArgs = null) {

        var pms: SupplierInvoicePM[] = [];

        for (let invoice of this.DecPM.SupplierInvoices) {
            for (let i of invoice.SupplierInvoiceModifications) {
                if (i.IsDirty) {
                    if (i.ChangeSetOp == "None") i.ChangeSetOp = "Update";
                    if (!pms.includes(invoice))
                        pms.push(invoice);
                }
            }
        }
        if (pms.length == 0) {
            if (fromSend)
                this.SendAmendmentCloseDeclaration(event);
            else {
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                SessionLocator.SelectedSession.CloseCurrentWindowEmit("Cancel");
            }
            return;
        }

        this.supplierInvoiceExtendedPMService.updateSupplierInvoiceModifications(pms).subscribe((myResult: any) => {

            var res: ServiceResponse = myResult;
            if (res.HasError) {
                var msg = new MessageWindow();
                msg.Show(res.ErrorsArray.join());
                if (!fromSend)
                    this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
            }
            else {
                if (fromSend) {
                    this.SendAmendmentCloseDeclaration(event);
                }
                else {
                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                    SessionLocator.SelectedSession.CloseCurrentWindowEmit("Cancel");
                }
            }
        });

    }
    setIdentifiersPlaceHolders() {

        this._CargoIdentifireTypeListService.getSingleFromCache(this.FinalCargoTypeCode)
            .subscribe((Response: ServiceResponse) => {
                if (Response.Result != null) {

                    this.ManifestNumberPlaceholder = Response.Result.CargoIdentifierKey1Name;
                    this.SecondCargoIdPlaceholder = Response.Result.CargoIdentifierKey2Name ?? '';
                    this.ThirdCargoIdPlaceholder = Response.Result.CargoIdentifierKey3Name ?? '';

                    this.setWarningValues();
                }
            });
    }

    async initOceanExportData() {
        if (this.DecPM.Direction !== 'E' || this.DecPM.TransportModeId !== 'O' || !(await this.isConnectedToUniFreight())) return;

        const exportData = await this.exportDeclarationClosingWebService.getUnifreightData(this.DecPM.ExportFile);
        if (!exportData) return;

        this.FlightDate = exportData.flightDate;
        this.ChargingSite = exportData.loadingSite;
        this.Smp = exportData.HAWB;
        this.MainAWB = exportData.MAWB;

        this.cdr.detectChanges();
    }

    isConnectedToUniFreight(): Promise<boolean> {
        return new Promise<boolean>((resolve, reject) =>
            new CustomsSettingListService().getSingleFromCache(this.DecPM.Tenant.toString()).subscribe((response: ServiceResponse) =>
                resolve(response.Result.IsConnectedToUniFreight)))
    }

    operationalDataFromUnifreight() {

        SessionLocator.SelectedSession.StartBusyIndicatorLoading();
        let sub = AmitalGatewayUtil.Instance.UnifaceRequestArrived
            .subscribe(
                (mess: UnifreightMessageM) => {
                    var IsMatchUnifreightCallbackCommand = (
                        mess.LogitudeEntityNumber == this.DecPM.Id &&
                        mess.LogitudeViewModel == "ExportDeclarationClosingDataComponent.ts");
                    if (IsMatchUnifreightCallbackCommand) {
                        sub.unsubscribe();
                        SessionLocator.SelectedSession.StopBusyIndicator();
                        let LoadPort = UnifreightMessageM.GetStringValue(mess, "LoadPort");
                        let Mawb = UnifreightMessageM.GetStringValue(mess, "Mawb");
                        let Hawb = UnifreightMessageM.GetStringValue(mess, "Hawb");
                        let FlightDate = UnifreightMessageM.GetStringValue(mess, "FlightDate");
                        if (Mawb != null && this.EntityPM != null) {
                            this.MainAWB = Mawb;

                            if (AppTool.IsNullOrEmpty(this.EntityPM.FinalManifestNumber) && this.IsNew && this.DecPM.Direction == 'E' && (this.DecPM.TransportModeId == 'A' || this.DecPM.TransportModeId == 'O')) {
                                this.EntityPM.IsDirty = true;
                                this.EntityPM ? this.EntityPM.FinalManifestNumber = this.EntityPM.MAIN_AWB : null;
                            }
                        }
                        Hawb ? this.Smp = Hawb : '';
                        if (LoadPort != null) {
                            this.ChargingSite = LoadPort;
                            if (this.IsNew) {
                                this.FinalLoadingSite = LoadPort;
                            }
                        }
                        if (!AppTool.IsNullOrEmpty(FlightDate)) {
                            this.FlightDate = new Date(Date.UTC(Number(FlightDate.substring(0, 4)), Number(FlightDate.substring(4, 6)) - 1, Number(FlightDate.substring(6, 8))));
                            if (this.IsNew) {
                                this.LoadingDateTime = new Date(Date.UTC(Number(FlightDate.substring(0, 4)), Number(FlightDate.substring(4, 6)) - 1, Number(FlightDate.substring(6, 8))));
                            }
                        }
                        SessionLocator.SelectedSession.CurrentListComponent.DoRefresh();
                    }
                }
            );

        SessionLocator.SelectedSession.StartBusyIndicator("");
        var unifreightMessageM =
            AmitalGatewayUtil.Instance.
                DeclarationMessaging.GetMessage(this.DecPM.CustomFileNo, this.DecPM.Id, "ExportDeclarationClosingDataComponent.ts", "BFIFILE");

        AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
            "AmitalGatewayUtil.CustomExportCloseFile",
            "BFIHMAIN.LogitudeTask",
            "CustomExportCloseFile",
            unifreightMessageM,
            "× ×ª×•× ×™×� ×ª×¤×¢×•×œ×™×� ×‘×¡×’×™×¨×ª ×”×¦×”×¨×”");

    }

    InvoicesNumbersList: any[];
    FillInvoiceNumbersList() {
        var items = this.DecPM.SupplierInvoices.map(x => ({ InvoiceCounterKey: x.InvoiceCounterKey, InvoiceNumber: x.InvoiceNumber }));
        this.InvoicesNumbersList = [];
        items.forEach(x => this.InvoicesNumbersList.push(x));


    }


}

export class ModificationItemModel extends BaseComponent {
    public ModificationPM: SupplierInvoiceModificationPM = null;
    public ObjectTableName = "Customs.SupplierInvoiceModification";
    public DataContext = this;
    isValid: boolean;
    public customsExchangeRateExtendedPMService: CustomsExchangeRateExtendedPMService = new CustomsExchangeRateExtendedPMService();

    constructor(private modificationPM: SupplierInvoiceModificationPM, private parent: ExportDeclarationClosingDataComponent, private entityParentPM: SupplierInvoicePM) {
        super();
        this.ModificationPM = modificationPM;
        this.isValid = true;
        this.selectedInvoice = parent.InvoicesNumbersList.find(x => x.InvoiceCounterKey == this.entityParentPM.InvoiceCounterKey);
        this.InvoiceNumber = this.entityParentPM.InvoiceNumber;
        this.customsExchangeRateExtendedPMService.GetCustomsExchangeRateForCurrencyAndDate(this.entityParentPM.InvoiceCurrencyTypeCode, this.parent.DecPM.TaxationDateTime).subscribe((response: any) => {
            if (response) {
                if (response.Result) {
                    var rate = response.Result[0];
                    if (rate) {
                        this.InvoiceCurrencyExchangeRtae = rate.ExchangeRate;
                    }
                }
            }
        });
    }

    //#region Properties
    get TypeDesc() { return this.ModificationPM.TypeDesc; }
    set TypeDesc(value: string) {
        if (this.ModificationPM.TypeDesc != value) {
            this.ModificationPM.TypeDesc = value;
        }
    }
    get TypeCode() { return this.ModificationPM.TypeCode; }
    set TypeCode(value: string) {
        if (this.ModificationPM.TypeCode != value) {

            if (value == "I02") {
                this.ModificationPM.TypeCode = value;
                this.isValid = false;
                this.parent.ValidationErrors.push(TextCodeTranslator.Translate("Customs.Declaration.O.CalculatedFee"));
            } else {
                var exists_prev = [];
                if (this.entityParentPM.SupplierInvoiceModifications.length != 0) {
                    exists_prev = this.parent.ModificationsList.Collection.filter(d => d.TypeCode == this.ModificationPM.TypeCode && d != this);
                }
                if (exists_prev.length == 1) {
                    exists_prev[0].isValid = true;
                }
                var exists;
                if (this.entityParentPM.SupplierInvoiceModifications.length != 0) {
                    exists = this.entityParentPM.SupplierInvoiceModifications.find(d => d.TypeCode == value);
                }
                if (exists) {
                    this.ModificationPM.TypeCode = value;
                    this.isValid = false;
                    this.parent.ValidationErrors.push(TextCodeTranslator.Translate("Customs.Declaration.O.ExistingType"));
                } else {
                    this.ModificationPM.TypeCode = value;
                    this.isValid = true;
                }

            }

        }
    }

    get TypeName() { return this.ModificationPM.TypeName; }
    set TypeName(value: string) {
        if (this.ModificationPM.TypeName != value) {
            this.ModificationPM.TypeName = value;

        }
    }

    get CurrencyTypeCode() { return this.ModificationPM.CurrencyTypeCode; }
    set CurrencyTypeCode(value: string) {
        if (this.ModificationPM.CurrencyTypeCode != value) {
            this.ModificationPM.CurrencyTypeCode = value;

        }
    }

    get CurrencyTypeName() { return this.ModificationPM.CurrencyTypeName; }
    set CurrencyTypeName(value: string) {
        if (this.ModificationPM.CurrencyTypeName != value) {
            this.ModificationPM.CurrencyTypeName = value;

        }
    }

    get Amount() { return this.ModificationPM.Amount; }
    set Amount(value: number) {
        if (this.ModificationPM.Amount != value) {
            this.ModificationPM.Amount = value;
        }

    }

    get InvoiceCounterKey() { return this.ModificationPM.InvoiceCounterKey; }
    set InvoiceCounterKey(value: number) {
        if (this.ModificationPM.InvoiceCounterKey != value) {
            this.ModificationPM.InvoiceCounterKey = value;

        }
    }

    InvoiceNumber: string;


    doCalculate: boolean = false;

    OriginalText: string;
    AmountOriginalText(originalText: string) {
        this.OriginalText = originalText;
        if (this.OriginalText) {
            if ((this.OriginalText + "").indexOf('%') > -1) {
                this.doCalculate = true;
            }
        }

        this.OriginalText = null;
    }
    selectedInvoice: any;
    InvoicesSelectionChanged(selectedItem) {
        if (selectedItem != null) {
            if (!AppTool.IsNullOrEmpty(this.InvoiceCounterKey)) {
                if (this.ModificationPM.ChangeSetOp == "Insert") {
                    var index = this.parent.DecPM.SupplierInvoices.find(x => x.InvoiceCounterKey == this.InvoiceCounterKey).SupplierInvoiceModifications.indexOf(this.ModificationPM);
                    if (index > -1) {
                        this.parent.DecPM.SupplierInvoices.find(x => x.InvoiceCounterKey == this.InvoiceCounterKey).SupplierInvoiceModifications.splice(index, 1);
                    }
                    this.parent.DecPM.SupplierInvoices.find(x => x.InvoiceCounterKey == selectedItem.InvoiceCounterKey).SupplierInvoiceModifications.push(this.ModificationPM);
                }
                else {
                    //×œ×� × ×™×ª×Ÿ ×œ×©× ×•×ª ×—×©×‘×•×Ÿ ×œ×©×•×¨×” ×©×ž×•×¨×”

                }
            }
            this.ModificationPM.IsDirty = true;
            this.InvoiceCounterKey = selectedItem.InvoiceCounterKey;
            this.InvoiceNumber = selectedItem.InvoiceNumber;
            var invoice = this.parent.DecPM.SupplierInvoices.find(x => x.InvoiceCounterKey == selectedItem.InvoiceCounterKey);
            this.entityParentPM = invoice;
        }
    }
    //#endregion

    SetLocalName(entity, fieldName) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        } else {
            this[fieldName] = null;
        }

    }
    DiscountInNIS: number = 0;
    InvoiceCurrencyExchangeRtae: number = 0;
    DiscountInDsicCurrency: number = 0;


    OnAmountLostFocus() {

        if (this.doCalculate) {
            var value = this.Amount;


            this.customsExchangeRateExtendedPMService.GetCustomsExchangeRateForCurrencyAndDate(this.CurrencyTypeCode, this.parent.DecPM.TaxationDateTime).subscribe((response: any) => {
                if (this.entityParentPM.InvoiceAmount) {
                    this.DiscountInNIS = this.entityParentPM.InvoiceAmount * this.InvoiceCurrencyExchangeRtae * value;
                    if (response) {
                        if (response.Result) {
                            var result = response.Result[0];
                            if (result) {

                                var amount = this.DiscountInNIS / result.ExchangeRate;
                                if (amount) {
                                    this.Amount = amount;
                                }


                            }
                        }
                    }
                }
                this.doCalculate = false;


            });
        }

    }
}





