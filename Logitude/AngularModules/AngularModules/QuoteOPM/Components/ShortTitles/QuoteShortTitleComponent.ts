import {Component} from '@angular/core';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {QuoteOPPM} from '../../EntityPMs/QuoteOPPM';
import {AppTool} from '../../../Infrastructure/Tools';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {CardListService} from '../../../Common/Services/StandardLists/CardListService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {CardList} from '../../../Common/EntityLists/CardList';

@Component({
    
    templateUrl: "./QuoteShortTitleComponent.html",
})

export class QuoteShortTitleComponent {
  public CustomerRankName: any;

    public EntityPM: QuoteOPPM;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = this.entityArgs.EntityPM;
        this.Listen();
        if (this.EntityPM != null) {
            this.BuildComponent();
        }
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {

            if (!this.SaveCompletedEvent) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.BuildComponent();
                    }
                });
            }

            if (!this.LoadCompletedEvent) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.BuildComponent();
                    }
                });
            }
        }
    }


    public DirectionImageSRC: string;
    public TransportModeImageSRC: string;
    public RankCode: string;
    public RankName: string;
    public RankSource1: string;
    public RankSource2: string;
    public RankSource3: string;
    public IsRankVisible: boolean = false;

    get IsCancelled() { return this.EntityPM.IsCancelled; }  

    private BuildComponent() {
        this.RankName = this.EntityPM.CustomerRankName;

        if (this.RankName != null) {

            switch (this.RankName.toLowerCase()) {
                case "silver": {
                    this.RankCode = "1";
                    this.RankSource1 = "./Images/Icons/StarOrange.png";
                    this.RankSource2 = "./Images/Icons/StarGray.png";
                    this.RankSource3 = "./Images/Icons/StarGray.png";
                    break;
                }

                case "gold": {
                    this.RankCode = "2";
                    this.RankSource1 = "./Images/Icons/StarOrange.png";
                    this.RankSource2 = "./Images/Icons/StarOrange.png";
                    this.RankSource3 = "./Images/Icons/StarGray.png";
                    break;
                }

                case "platinum": {
                    this.RankCode = "3";
                    this.RankSource1 = "./Images/Icons/StarOrange.png";
                    this.RankSource2 = "./Images/Icons/StarOrange.png";
                    this.RankSource3 = "./Images/Icons/StarOrange.png";
                    break;
                }

                default: {
                    this.RankCode = "0";
                    this.RankSource1 = "./Images/Icons/StarGray.png";
                    this.RankSource2 = "./Images/Icons/StarGray.png";
                    this.RankSource3 = "./Images/Icons/StarGray.png";
                }
            }

            this.IsRankVisible = true;
        }

        this.DirectionImageSRC = "./Images/Directions/" + this.EntityPM.DirectionId + ".png";
        this.TransportModeImageSRC = "./Images/Icons/" + this.EntityPM.TransportModeId + ".png";
    }

    ViewCustomer() {
        if (this.EntityPM != null) {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.CustomerId)) {

                var myService: CardListService = new CardListService();
                myService.getSingle(this.EntityPM.CustomerId).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CardList = myResponse.Result;

                        if (list != null) {
                            var objectTableName = null;

                            switch (list.PartnerTypeId) {
                                case "CS":
                                case "PO":
                                    {
                                        objectTableName = "Customer";
                                        break;
                                    }

                                case "AG": { objectTableName = "Agent"; break; }
                                case "AL": { objectTableName = "Airline"; break; }
                                case "TR": { objectTableName = "Trucker"; break; }
                                case "CG": { objectTableName = "CustomAgent"; break; }
                                case "SG": { objectTableName = "ShippingAgent"; break; }
                                case "SL": { objectTableName = "ShippingLine"; break; }
                                case "VD": { objectTableName = "Vendor"; break; }
                                case "WH": { objectTableName = "Warehouse"; break;}
                            }

                            if (!AppTool.IsNullOrEmpty(objectTableName)) {
                                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        cmpRef.instance.ComponentRef = cmpRef;
                                        cmpRef.instance.Run({ EntityId: this.EntityPM.CustomerId, ObjectTableName: objectTableName, BackButtonLabel: 'Quote' });

                                        let isEditComponentSaved = false;
                                        cmpRef.instance.BackCompleted.subscribe(bk => {
                                            if (isEditComponentSaved) {
                                                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                                            }
                                        });

                                        cmpRef.instance.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                                            if (isSaveSuccess) {
                                                isEditComponentSaved = true;
                                            }
                                        });
                                    });
                            }
                        }
                    }
                }); 
            }
        }
    }     
}
