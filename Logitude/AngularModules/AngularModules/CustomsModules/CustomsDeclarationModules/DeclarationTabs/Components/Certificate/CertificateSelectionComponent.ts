import {Component}  from '@angular/core';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {CertificateTicket} from '../../../../../Customs/DataContract/CertificateTicket';
import {ConfirmWindow} from '../../../../../Controls/Windows/ConfirmWindow';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {CertificateConnectedItem} from '../../../../../Customs/DataContract/CertificateConnectedItem';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {MultiCertificatesService} from '../../../../../Customs/Services/Others/MultiCertificatesService';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {AppTool} from '../../../../../Infrastructure/Tools';
import {ObservableCollection} from '../../../../../Infrastructure/Utilities/ObservableCollection';



@Component({
    moduleId: module.id,
    templateUrl: './CertificateSelectionComponent.html',
})



export class CertificateSelectionComponent extends BaseComponent {

    public ItemsSource = new ObservableCollection([]);
    connectedItems: CertificateConnectedItem[] = [];
    ExcludedItems: CertificateConnectedItem[] = [];
   ticket: CertificateTicket;
   multiCertificatesService: MultiCertificatesService = new MultiCertificatesService();
    private CurrentSession = SessionLocator.SelectedSession;
   constructor() {
        super();

   }
   ThereIsNoCertificates: boolean = false;
   GridHeight: number;
    SetWindowArgs(args: any) {

        this.ItemsSource.InsertCollection(args.certificateList);
        if (this.ItemsSource.Length == 0) {
            this.ThereIsNoCertificates = true;
            this.GridHeight = 35;
        }
        this.connectedItems = args.ConnectedItems;
        this.ExcludedItems = args.ExcludedItems;
        this.ticket = args.Ticket;
   }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("no");

        this.CurrentSession.CloseCurrentWindow();
    }

    SelectedRow: any;
    OnRowSelected(selectedRow: any) {
        this.SelectedRow = selectedRow;
    }
    OkButtonClicked() {
        if (!AppTool.IsNullOrEmpty(this.SelectedRow)) {
            this.CurrentSession.StartBusyIndicator("");
            var certificateTicket: CertificateTicket = new CertificateTicket();
            certificateTicket.DeclarationId = this.ticket.DeclarationId;
            certificateTicket.InvoiceNumber = null;
            certificateTicket.AttachmentTypeCode = this.SelectedRow.AttachmentTypeCode;
            certificateTicket.CertificateNumber = this.SelectedRow.CertificateNumber;
            certificateTicket.ResConfirmationTypeCode = this.SelectedRow.ResConfirmationTypeCode;
            certificateTicket.CertificateExemptionTypeCode = this.SelectedRow.CertificateExemptionTypeCode;
            certificateTicket.ReqConfirmationTypeCode = this.SelectedRow.ReqConfirmationTypeCode;
            certificateTicket.CustomsAttachmentId = this.SelectedRow.CustomsAttachmentId;
            certificateTicket.oldAttachment = this.ticket.AttachmentTypeCode;
            certificateTicket.oldCertificateExempt = this.ticket.CertificateExemptionTypeCode;
            certificateTicket.oldCertificateNumber = this.ticket.CertificateNumber;
            certificateTicket.oldResConfirmation = this.ticket.ResConfirmationTypeCode;
            certificateTicket.IsAllSelected = this.ticket.IsAllSelected;
            certificateTicket.SelectedItems = [];
            //if (!certificateTicket.IsAllSelected) {

                certificateTicket.SelectedItems = this.connectedItems;
            //}
                certificateTicket.ConnectedItemsKeys = "";
                certificateTicket.SelectedItems.forEach((item) => {
                    certificateTicket.ConnectedItemsKeys = certificateTicket.ConnectedItemsKeys + "," + item.DeclarationId + ";" + item.InvoiceCounterKey + ";" + item.LineNumber + ";" + item.ItemCertificateCounterKey;
                });
                certificateTicket.ConnectedItemsKeys = certificateTicket.ConnectedItemsKeys.substr(1, certificateTicket.ConnectedItemsKeys.length - 1);

                certificateTicket.ExcludedItemsKeys = "";
                this.ExcludedItems.forEach((item) => {
                    certificateTicket.ExcludedItemsKeys = certificateTicket.ExcludedItemsKeys + "," + item.DeclarationId + ";" + item.InvoiceCounterKey + ";" + item.LineNumber + ";" + item.ItemCertificateCounterKey;
                });
                certificateTicket.ExcludedItemsKeys = certificateTicket.ExcludedItemsKeys.substr(1, certificateTicket.ExcludedItemsKeys.length - 1);

            this.multiCertificatesService.PutCertificateTickets(certificateTicket)
                .subscribe((response: ServiceResponse) => {
                    if (!response.HasError) {
                        this.CurrentSession.StopBusyIndicator();
                        this.CurrentSession.CloseCurrentWindowEmit("ok");
                        this.CurrentSession.CloseCurrentWindow();
                    }
                });
        }
    }

}


