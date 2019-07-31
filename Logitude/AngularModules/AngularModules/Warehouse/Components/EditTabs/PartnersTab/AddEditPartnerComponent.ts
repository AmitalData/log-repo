import {Component, OnInit} from '@angular/core';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {PartnerItem} from './WarehouseEntryPartnersTabComponent';
import {WarehouseEntryPM} from '../../../EntityPMs/WarehouseEntryPM';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../../Infrastructure/Tools';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {NewEntityArgs} from '../../../../Infrastructure/Args';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';

@Component({
    moduleId: module.id,
    selector: 'AddEditPartnerComponent',
    templateUrl: './AddEditPartnerComponent.html',
})

export class AddEditPartnerComponent extends BaseComponent implements OnInit {
    public EntityPM: WarehouseEntryPM;
    public DataContext: PartnerItem;
    public ObjectTableName: string = "WarehouseEntry";
    private isMyCustomer: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }

    public IsEditingEnabled: boolean = true;
    public IsEditPartnerEnabled: boolean = false;


    ngOnInit() {
        this.DataContext.SetUIProperties();
    }

    SetDataContext(dataContext: PartnerItem) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.isMyCustomer = dataContext.IsCustomer;

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

    public ValidationErrorsList: string[];
    OkButtonClicked() {

        this.ValidationErrorsList = [];

        var msg: string = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        if (this.DataContext.PartnerId == null) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", TextCodeTranslator.Translate("WarehouseEntry.S.Partners.Name")));
        }

        if (this.DataContext.Reference1 != null) {
            if (this.DataContext.Reference1.length > 50) {
                this.ValidationErrorsList.push(TextCodeTranslator.Translate("WarehouseEntry.S.Partners.Reference1") + " max length is 50");
            }
        }

        if (this.DataContext.Reference2 != null) {
            if (this.DataContext.Reference2.length > 50) {
                this.ValidationErrorsList.push(TextCodeTranslator.Translate("WarehouseEntry.S.Partners.Reference2") + " max length is 50");
            }
        }

        if (this.ValidationErrorsList.length == 0) {

            if (this.isMyCustomer) {
                if (this.EntityPM.CustomerId != this.DataContext.PartnerId) {
                    this.EntityPM.CustomerId = this.DataContext.PartnerId;
                }
            }
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
            }

            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    }
}
