
import {Component}  from '@angular/core';
import {BaseComponent} from '../../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ObservableCollection} from '../../../../../../Infrastructure/Utilities/ObservableCollection';
import {AppTool} from '../../../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../../../Infrastructure/Utilities/SessionLocator';
import { SupplierInvoicePM } from '../../../../../../Customs/EntityPMs/SupplierInvoicePM';
import { SupplierInvioceItemCertificatPM } from '../../../../../../Customs/EntityPMs/SupplierInvioceItemCertificatPM';
import {ServiceResponse} from '../../../../../../Infrastructure/DataContracts/ServiceResponse';

import {TextCodeTranslator} from '../../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {MultiCertificatesService} from '../../../../../../Customs/Services/Others/MultiCertificatesService';

import {MessageWindow} from '../../../../../../Controls/Windows/MessageWindow';
import { ApiQueryFilters } from '../../../../../../Infrastructure/DataContracts/ApiQueryFilters';

@Component({
    
    templateUrl: './MultiCertificateUpdateComponent.html',
})

export class MultiCertificateUpdateComponent extends BaseComponent {
    DataContext: any = this;
    public ValidationErrorsList: string[] = [];
    CertTableName: string = "Customs.SupplierInvioceItemCertificat";
    public TypeCodeFilterItems: ApiQueryFilters;
    IsDisplayOnly: boolean = false;
    _MultiCertificatesService: MultiCertificatesService = new MultiCertificatesService();
    CertPM: SupplierInvioceItemCertificatPM;
    InvoicPM: SupplierInvoicePM;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.SetUIProperties();
    }

    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.InvoicPM = args.EntityPM;
            var decPM = SessionLocator.SelectedSession.CurrentEditComponent.EntityPM;
            this.TypeCodeFilterItems = new ApiQueryFilters();
            if (decPM.direction == "I") {
                this.TypeCodeFilterItems.addAdditionalFilter("IsImportDeclaration", true, null, null, "Equals", false, false, false, "boolean");
            }
            if (decPM.direction == "E") {
                this.TypeCodeFilterItems.addAdditionalFilter("IsExportDeclaration", true, null, null, "Equals", false, false, false, "boolean");
            }

        }


    }

    SetUIProperties() {

        //all cases required
        this.UIProperties.SetRequired("ExternalRequestTypeCode", this.CertTableName, true);
        this.UIProperties.SetValidity("ExternalRequestTypeCode", this.CertTableName, true, "");
        this.UIProperties.SetRequired("ApprovalRequestNumber", this.CertTableName, true);
        this.UIProperties.SetValidity("ApprovalRequestNumber", this.CertTableName, true, "");
        this.UIProperties.SetRequired("ReqConfirmationTypeCode", this.CertTableName, true);
        this.UIProperties.SetValidity("ReqConfirmationTypeCode", this.CertTableName, true, "");
        this.UIProperties.SetRequired("AttachmentTypeCode", this.CertTableName, true);
        this.UIProperties.SetValidity("AttachmentTypeCode", this.CertTableName, true, "");

        if (this.AttachmentTypeCode == "1" || this.AttachmentTypeCode == "2") {
            this.UIProperties.SetRequired("CertificateNumber", this.CertTableName, true); //required
            this.UIProperties.SetValidity("CertificateNumber", this.CertTableName, true, "");
            this.UIProperties.SetRequired("ResConfirmationTypeCode", this.CertTableName, true); //required
            this.UIProperties.SetValidity("ResConfirmationTypeCode", this.CertTableName, true, "");
            this.UIProperties.SetRequired("CertificateExemptionTypeCode", this.CertTableName, false); //optional
            this.UIProperties.SetValidity("CertificateExemptionTypeCode", this.CertTableName, true, "");
        }
        else if (this.AttachmentTypeCode == "3") {
            this.UIProperties.SetRequired("CertificateNumber", this.CertTableName, false); //optional
            this.UIProperties.SetValidity("CertificateNumber", this.CertTableName, true, "");
            this.UIProperties.SetRequired("ResConfirmationTypeCode", this.CertTableName, false); //optional
            this.UIProperties.SetValidity("ResConfirmationTypeCode", this.CertTableName, true, "");
            this.UIProperties.SetRequired("CertificateExemptionTypeCode", this.CertTableName, false); //optional
            this.UIProperties.SetValidity("CertificateExemptionTypeCode", this.CertTableName, true, "");
        }
        else if (this.AttachmentTypeCode == "4") {
            this.UIProperties.SetRequired("CertificateNumber", this.CertTableName, false); //optional
            this.UIProperties.SetValidity("CertificateNumber", this.CertTableName, true, "");
            this.UIProperties.SetRequired("ResConfirmationTypeCode", this.CertTableName, false); //optional
            this.UIProperties.SetValidity("ResConfirmationTypeCode", this.CertTableName, true, "");
            this.UIProperties.SetRequired("CertificateExemptionTypeCode", this.CertTableName, true); //required
            this.UIProperties.SetValidity("CertificateExemptionTypeCode", this.CertTableName, true, "");
        }
    }

    //#region Properties
    _ExternalRequestTypeCode: string = null;
    get ExternalRequestTypeCode() { return this._ExternalRequestTypeCode }
    set ExternalRequestTypeCode(value: string) { this._ExternalRequestTypeCode = value; }

    _ApprovalRequestNumber: string = null;
    get ApprovalRequestNumber() { return this._ApprovalRequestNumber }
    set ApprovalRequestNumber(value: string) { this._ApprovalRequestNumber = value; }



    _ReqConfirmationTypeCode: string = null;
    get ReqConfirmationTypeCode() { return this._ReqConfirmationTypeCode }
    set ReqConfirmationTypeCode(value: string) {
        this._ReqConfirmationTypeCode = value;
    }

    _AttachmentTypeCode: string = null;
    get AttachmentTypeCode() { return this._AttachmentTypeCode }
    set AttachmentTypeCode(value: string) {
        this._AttachmentTypeCode = value;
        this.SetUIProperties();
    }

    _CertificateNumber: string = null;
    get CertificateNumber() { return this._CertificateNumber }
    set CertificateNumber(value: string) { this._CertificateNumber = value; }

    _CertificateExemptionTypeCode: string = null;
    get CertificateExemptionTypeCode() { return this._CertificateExemptionTypeCode }
    set CertificateExemptionTypeCode(value: string) { this._CertificateExemptionTypeCode = value; }

    _ResConfirmationTypeCode: string = null;
    get ResConfirmationTypeCode() { return this._ResConfirmationTypeCode }
    set ResConfirmationTypeCode(value: string) {
        this._ResConfirmationTypeCode = value;
    }
    //#endregion

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("cancel");
    }

    OkButtonClicked() {
        var errors = [];
        this.ValidationErrorsList = errors;

        //#region Manditory fields validation
        if (!this.ExternalRequestTypeCode ||
            !this.ApprovalRequestNumber ||
            !this.ReqConfirmationTypeCode ||
            !this.AttachmentTypeCode) {
            errors.push(TextCodeTranslator.Translate("Customs.Declaration.O.AllFieldsAreRequired"));
        }
        else if (this.AttachmentTypeCode == "1" || this.AttachmentTypeCode == "2") {
            if (!this.CertificateNumber || !this.ResConfirmationTypeCode)
                errors.push(TextCodeTranslator.Translate("Customs.Declaration.O.AllFieldsAreRequired"));
        }
        else if (this.AttachmentTypeCode == "4") {
            if (!this.CertificateExemptionTypeCode)
                errors.push(TextCodeTranslator.Translate("Customs.Declaration.O.AllFieldsAreRequired"));
        }
        //#endregion 

        if (errors.length == 0) {
            //this.UpdateChanges();
            this.UpdateChangesOnClientSide();
        } else {
            this.ValidationErrorsList = [];
            this.ValidationErrorsList = errors;
        }
    }

    UpdateChanges() { // not used

        // this is method is not used
        // to use it , call this function, and change code when closing
        // multi certificate window


        this._MultiCertificatesService.UpdateCertificatesBySearchFields(
            this.InvoicPM.DeclarationId,
            this.InvoicPM.InvoiceCounterKey,
            this.ExternalRequestTypeCode,
            this.ApprovalRequestNumber,

            this.ReqConfirmationTypeCode,
            this.AttachmentTypeCode,
            this.CertificateNumber,
            this.CertificateExemptionTypeCode,
            this.ResConfirmationTypeCode

        ).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                var updatedRowCount: number = myResponse.Result;
                if (updatedRowCount != null && updatedRowCount != undefined) {
                    if (updatedRowCount == 0) {
                        var msg = new MessageWindow();
                        msg.RTL = true;
                        msg.Show(TextCodeTranslator.Translate("Customs.Declaration.O.Nomatchinglineswerefound"));

                    } else {
                        var msg = new MessageWindow();
                        var txt = TextCodeTranslator.Translate("Customs.Declaration.O.itemswereupdated");
                        msg.RTL = true;
                        msg.ShowSuccessIcon = true;
                        msg.WindowClosed.subscribe(() => {
                            this.CurrentSession.CloseCurrentWindowEmit("ok");
                        });
                        msg.Show(txt.replace("#Number", updatedRowCount + ""));

                    }
                } else {
                    console.error("ERROR IN SERVICE!!!!!", myResponse.ErrorsArray);
                }
            }

        });


    }

    UpdateChangesOnClientSide() {

        var updatedRowCount = 0;

        this.CurrentSession.CurrentWindow.StartBusyIndicator("Updating...");

        //Update certificates by search fields
        if (this.InvoicPM.SupplierInvoiceItems) {
            this.InvoicPM.SupplierInvoiceItems.forEach((item) => {
                if (item.SupplierInvioceItemCertificats) {
                    item.SupplierInvioceItemCertificats.forEach((cert) => {

                        //check by search fields
                        if (cert.ExternalRequestTypeCode == this.ExternalRequestTypeCode && cert.ApprovalRequestNumber == this.ApprovalRequestNumber) {
                            cert.ReqConfirmationTypeCode = this.ReqConfirmationTypeCode;
                            cert.AttachmentTypeCode = this.AttachmentTypeCode;
                            cert.CertificateNumber = this.CertificateNumber;
                            cert.CertificateExemptionTypeCode = this.CertificateExemptionTypeCode;
                            cert.ResConfirmationTypeCode = this.ResConfirmationTypeCode;

                            updatedRowCount++;
                        }

                        //update invoice item status
                        this.UpdateCertStatusAlaaMethod(cert, item);
                        //this.UpdateCertStatus(cert, item);

                    });
                }
            });
        }

        this.CurrentSession.CurrentWindow.StopBusyIndicator();

        // Show response message
        if (updatedRowCount == 0) {
            var msg = new MessageWindow();
            msg.RTL = true;
            msg.Show(TextCodeTranslator.Translate("Customs.Declaration.O.Nomatchinglineswerefound"));

        } else {
            var msg = new MessageWindow();
            var txt = TextCodeTranslator.Translate("Customs.Declaration.O.itemswereupdated");
            msg.RTL = true;
            msg.ShowSuccessIcon = true;
            msg.WindowClosed.subscribe(() => {
                this.CurrentSession.CloseCurrentWindowEmit("ok");
            });
            msg.Show(txt.replace("#Number", updatedRowCount + ""));

        }



    }

    //itzik method
    UpdateCertStatus(cert, item) {
        if (cert && item) {

            var statusCode: string;
            if (cert.AttachmentTypeCode == null) {
                statusCode = "2";
            }
            else if (cert.AttachmentTypeCode == "1" || cert.AttachmentTypeCode == "2") {
                if (!(cert.CertificateNumber) || !(cert.ReqConfirmationTypeCode) || !(cert.ResConfirmationTypeCode) || (cert.CertificateExemptionTypeCode)) {
                    statusCode = "2";
                }
                else {
                    statusCode = "1";
                }
            }
            else if (cert.AttachmentTypeCode == "4") {
                if (!(cert.CertificateExemptionTypeCode) || !(cert.ReqConfirmationTypeCode) || (cert.CertificateNumber) || (cert.ResConfirmationTypeCode)) {
                    statusCode = "2";
                }
                else {
                    statusCode = "1";
                }
            }
            else {
                statusCode = "1";
            }

            item.CertificatesStatusCode = statusCode;
        }
    }
    
    UpdateCertStatusAlaaMethod(cert, item) {
        if (cert && item) {

            var statusCode: string;
            var valid: boolean = true;
            var hasRequest = !AppTool.IsNullOrEmpty(cert.ApprovalRequestNumber);


            if (cert.AttachmentTypeCode == null) {
                valid = false;
            }
            else {
                if (cert.AttachmentTypeCode == "1" || cert.AttachmentTypeCode == "2") {
                    if (AppTool.IsNullOrEmpty(cert.CertificateNumber) || AppTool.IsNullOrEmpty(cert.ReqConfirmationTypeCode) || AppTool.IsNullOrEmpty(cert.ResConfirmationTypeCode)) {
                        valid = false;
                    }
                    if (!AppTool.IsNullOrEmpty(cert.CertificateExemptionTypeCode) || !AppTool.IsNullOrEmpty(cert.CustomsAttachmentID)) {
                        valid = false;
                    }
                }
                else {
                    if (cert.AttachmentTypeCode == "4") {
                        if (AppTool.IsNullOrEmpty(cert.CertificateExemptionTypeCode) || AppTool.IsNullOrEmpty(cert.ReqConfirmationTypeCode)) {
                            valid = false;
                        }
                        if (!AppTool.IsNullOrEmpty(cert.CertificateNumber) || !AppTool.IsNullOrEmpty(cert.ResConfirmationTypeCode) || !AppTool.IsNullOrEmpty(cert.CustomsAttachmentID)) {
                            valid = false;
                        }
                    }
                }
            }

            if (valid) {
                if (hasRequest) {
                    item.CertificatesStatusCode = "3";
                }
                else {
                    item.CertificatesStatusCode = "1";
                }
            }
            else
            {
                if (hasRequest) {
                    item.CertificatesStatusCode = "4";
                }
                else {
                    item.CertificatesStatusCode = "2";
                }

            }

        }
        
    }

}
