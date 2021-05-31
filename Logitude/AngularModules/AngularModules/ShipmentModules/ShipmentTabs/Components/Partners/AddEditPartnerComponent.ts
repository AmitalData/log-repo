import {Component, OnInit} from '@angular/core';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {PartnerItem} from './PartnersTabComponent';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';

@Component({
    
    selector: 'AddEditPartnerComponent',
    templateUrl: './AddEditPartnerComponent.html',
})

export class AddEditPartnerComponent implements OnInit {
    public EntityPM: ShipmentPM;
    public DataContext: PartnerItem;
    public ObjectTableName: string = "Shipment";
    private isMyCustomer: boolean = false;
    private oldCustomerPartnerId: string = null;
    public ValidationErrorsList: string[];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

    }

    ngOnInit() {
        this.DataContext.SetUIProperties();
    }

    SetDataContext(dataContext: PartnerItem) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.isMyCustomer = dataContext.IsCustomer;
        this.oldCustomerPartnerId = this.EntityPM.CustomerId;
        this.Clone();
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    private showDeleteProductItemsConfirmWindow: boolean = false;
    OkButtonClicked() {
        this.ValidationErrorsList = [];

        var msg: string = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        if (this.DataContext.PartnerId == null) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Shipment.S.Partners.Name")));
        }

        if (this.DataContext.Reference1 != null) {
            if (this.DataContext.Reference1.length > 50) {
                this.ValidationErrorsList.push(TextCodeTranslator.Translate("Shipment.S.Partners.Reference1") + " max length is 50");
            }
        }

        if (this.DataContext.Reference2 != null) {
            if (this.DataContext.Reference2.length > 50) {
                this.ValidationErrorsList.push(TextCodeTranslator.Translate("Shipment.S.Partners.Reference2") + " max length is 50");
            }
        }

        if (this.ValidationErrorsList.length == 0) {
            if (this.isMyCustomer) {
                if (this.EntityPM.ShipmentLevelCode == "C") {
                    if (this.EntityPM.CustomerId != null) {
                        this.EntityPM.CustomerId = null;
                    }

                    if (this.EntityPM.CustomerName != null) {
                        this.EntityPM.CustomerName = null;
                    }

                    if (this.EntityPM.CustomerReference1 != null) {
                        this.EntityPM.CustomerReference1 = null;
                    }

                    if (this.EntityPM.CustomerReference2 != null) {
                        this.EntityPM.CustomerReference2 = null;
                    }

                    if (this.EntityPM.CustomerAddressId != null) {
                        this.EntityPM.CustomerAddressId = null;
                    }

                    if (this.EntityPM.CustomerContactId != null) {
                        this.EntityPM.CustomerContactId = null;
                    }

                    if (this.EntityPM.ShipmentCustomerTypeCode != null) {
                        this.EntityPM.ShipmentCustomerTypeCode = null;
                    }
                }

                else {
                    if (this.EntityPM.CustomerId != this.DataContext.PartnerId) {
                        this.EntityPM.CustomerId = this.DataContext.PartnerId;
                    }

                    if (this.EntityPM.CustomerName != this.DataContext.Name) {
                        this.EntityPM.CustomerName = this.DataContext.Name;
                    }

                    if (this.EntityPM.CustomerAddressId != this.DataContext.AddressId) {
                        this.EntityPM.CustomerAddressId = this.DataContext.AddressId;
                    }

                    if (this.EntityPM.CustomerContactId != this.DataContext.ContactId) {
                        this.EntityPM.CustomerContactId = this.DataContext.ContactId;
                    }

                    if (this.EntityPM.CustomerReference1 != this.DataContext.Reference1) {
                        this.EntityPM.CustomerReference1 = this.DataContext.Reference1;
                    }

                    if (this.EntityPM.CustomerReference2 != this.DataContext.Reference2) {
                        this.EntityPM.CustomerReference2 = this.DataContext.Reference2;
                    }
                }

                if (this.oldCustomerPartnerId != this.EntityPM.CustomerId) {
                    this.DataContext.fatherComponent.OnCustomerChanged();

                    if (this.EntityPM.ShipmentProductItems.length > 0) {
                        this.showDeleteProductItemsConfirmWindow = true;
                    }                    
                }
            }

            if (this.showDeleteProductItemsConfirmWindow) {
                this.ShowDeleteProductItemsConfirmation();
            }

            else {
                this.CurrentSession.CloseCurrentWindowEmit("OK");
                this.CurrentSession.FireEvent("ShipmentPartnersChanged");
            }
        }
    }
    ShowDeleteProductItemsConfirmation() {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("All product items in this shipment will be deleted");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.EntityPM.ShipmentProductItems = [];
                this.CurrentSession.CloseCurrentWindowEmit("OK");
                this.CurrentSession.FireEvent("ShipmentPartnersChanged");
            }
        });
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('PartnerId');
        this.myCloner.AddField('AddressId');
        this.myCloner.AddField('ContactId');
        this.myCloner.AddField('Reference1');
        this.myCloner.AddField('Reference2');
        this.myCloner.AddField('Name');
        this.myCloner.AddField('Note');
        this.myCloner.AddEntity(this.EntityPM);
    }
    private RejectChanges() {
        this.DataContext.IsReseting = true;
        this.myCloner.RejectChanges();
        this.DataContext.IsReseting = false;
    }
}
