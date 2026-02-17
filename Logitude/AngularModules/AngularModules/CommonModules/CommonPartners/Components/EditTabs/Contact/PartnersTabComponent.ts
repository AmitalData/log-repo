import {Component} from '@angular/core';
import {CardPM} from '../../../../../Common/EntityPMs/CardPM';
import {ContactPM} from '../../../../../Common/EntityPMs/ContactPM';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';
import {CardPMService} from '../../../../../Common/Services/StandardPMs/CardPMService';
import {PartnersDomainService} from '../../../../../Common/Services/PartnersDomainService';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {ConfirmWindow} from '../../../../../Controls/Windows/ConfirmWindow';

@Component({
    moduleId: module.id,
    templateUrl: './PartnersTabComponent.html',
})

export class PartnersTabComponent {
    public EntityPM: ContactPM;
    public ObjectTableName: string = "Contact";
    public ItemsSource: CardPM[];
    private CardService: CardPMService;
    private DomainService: PartnersDomainService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = entityArgs.EntityPM;
        this.DomainService = new PartnersDomainService();
        this.ItemsSource = [];
        this.LoadData();
    }

    public IsNoDataVisible: boolean = false;
    private LoadData() {
        this.IsNoDataVisible = false;
        this.DomainService.GetCardsForContact(this.EntityPM.Id).subscribe((myResult:any) => {
            this.ItemsSource = myResult;
            this.IsNoDataVisible = this.ItemsSource.length == 0 ? true : false;
        });
    }

    EditEntityClicked(item: CardPM) {
        if (item != null) {
            var itemTableName = item.PartnerTypeId == "PO" ? "Customer" : item.PartnerTypeName;

            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: item.Id, ObjectTableName: itemTableName });
                    cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                        this.LoadData();
                    });
                });
        }
    }

    DisconnectClicked(item: CardPM) {
        if (item != null) {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Title = "Disconnect";
            confirmWindow.Show("Are you sure you want to disconnect?");
            confirmWindow.WindowClosed.subscribe(s => {
                if (confirmWindow.Yes) {
                    item.DisconectFromContact = true;
                    item.ContactId = this.EntityPM.Id;

                    var indexOfItem = this.ItemsSource.indexOf(item);
                    if (indexOfItem > -1) {
                        this.ItemsSource.splice(indexOfItem, 1);
                    }

                    if (this.CardService == null) {
                        this.CardService = new CardPMService();
                    }

                    this.CardService.update(item).subscribe(myResult => {

                    });
                }
            });
        }
    }
}
