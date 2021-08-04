import {Component, OnInit} from '@angular/core';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {PartnerItem} from './PartnersTabComponent';
import {QuotePM} from '../../../../Quote/EntityPMs/QuotePM';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {QuoteUtilities} from '../../../../Quote/Utilities/QuoteUtilities';
import {AppTool} from '../../../../Infrastructure/Tools';

@Component({
    
    selector: 'AddEditPartnerComponent',
    templateUrl: './AddEditPartnerComponent.html',
})

export class AddEditPartnerComponent implements OnInit {
    public EntityPM: QuotePM;
    public DataContext: PartnerItem;
    public ObjectTableName: string = "QuoteOP";
    private isMyCustomer: boolean = false;
    private oldCustomerPartnerId: string = null;
    public ValidationErrorsList: string[];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
       
    }

    ngOnInit() {
        this.DataContext.SetUIProperties();
    }

    public IsEditingEnabled: boolean = true;
    public IsEditPartnerEnabled: boolean = false;
    public IsAgentPartner: boolean = false;
    SetDataContext(dataContext: PartnerItem) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.isMyCustomer = dataContext.IsCustomer;
        this.oldCustomerPartnerId = this.EntityPM.CustomerId;

        this.IsEditingEnabled = QuoteUtilities.IsQuoteEditEnabled(this.EntityPM);
        this.IsAgentPartner = this.DataContext.Code == "AGENT" ? true : false;

        if (this.IsEditingEnabled) {
            if (!AppTool.IsNullOrEmpty(this.DataContext.PartnerId)) {
                this.IsEditPartnerEnabled = true;
            }
        }
    }
    
    CancelButtonClicked() {
        this.DataContext.ResetOriginData();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {

        this.ValidationErrorsList = [];

        var msg: string = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        if (this.DataContext.PartnerId == null) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", TextCodeTranslator.Translate("QuoteOP.S.Partners.Name")));
        }

        if (this.DataContext.Reference1 != null) {
            if (this.DataContext.Reference1.length > 50) {
                this.ValidationErrorsList.push(TextCodeTranslator.Translate("QuoteOP.S.Partners.Reference1") + " max length is 50");
            }
        }

        if (this.DataContext.Reference2 != null) {
            if (this.DataContext.Reference2.length > 50) {
                this.ValidationErrorsList.push(TextCodeTranslator.Translate("QuoteOP.S.Partners.Reference2") + " max length is 50");
            }
        }

        if (this.ValidationErrorsList.length == 0) {

            if (this.DataContext.IsNewAdded) {
                if (this.DataContext.fatherComponent.ItemsCollection.indexOf(this.DataContext) == -1) {

                    var BreakException = {};

                    try {
                        this.DataContext.fatherComponent.ItemsCollection.sort((a, b) => { return a.PartnerIndex - b.PartnerIndex }).forEach((item) => {

                            if (this.DataContext.PartnerIndex < item.PartnerIndex) {
                                this.DataContext.fatherComponent.ItemsCollection.splice(this.DataContext.fatherComponent.ItemsCollection.indexOf(item), 0, this.DataContext);

                                throw BreakException;
                            }
                        });
                    }

                    catch (e) {
                        if (e !== BreakException) throw e;
                    }

                    finally {
                        if (this.DataContext.fatherComponent.ItemsCollection.indexOf(this.DataContext) == -1) {
                            this.DataContext.fatherComponent.ItemsCollection.push(this.DataContext);
                        }
                    }
                }

                //this.DataContext.fatherComponent.SetAddButtonsIsDisabled();
            }

            else {
                if (this.isMyCustomer) {
                    if (this.EntityPM.CustomerId != this.DataContext.PartnerId) {
                        this.EntityPM.CustomerId = this.DataContext.PartnerId;
                    }

                    if (this.EntityPM.CustomerName != this.DataContext.PartnerName) {
                        this.EntityPM.CustomerName = this.DataContext.PartnerName;
                    }

                    //if (this.EntityPM.CustomerAddressId != this.DataContext.AddressId) {
                    //    this.EntityPM.CustomerAddressId = this.DataContext.AddressId;
                    //}

                    if (this.EntityPM.CustomerContactId != this.DataContext.ContactId) {
                        this.EntityPM.CustomerContactId = this.DataContext.ContactId;
                    }

                    if (this.EntityPM.CustomerReference1 != this.DataContext.Reference1) {
                        this.EntityPM.CustomerReference1 = this.DataContext.Reference1;
                    }

                    if (this.EntityPM.CustomerReference2 != this.DataContext.Reference2) {
                        this.EntityPM.CustomerReference2 = this.DataContext.Reference2;
                    }

                    if (this.oldCustomerPartnerId != this.EntityPM.CustomerId) {
                        this.DataContext.fatherComponent.OnCustomerChanged();
                    }
                }
            }

            this.CurrentSession.CloseCurrentWindowEmit("OK");
            this.CurrentSession.FireEvent("QuotePartnersChanged");
        }
    }
}
