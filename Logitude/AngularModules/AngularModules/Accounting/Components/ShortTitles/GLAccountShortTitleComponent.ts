import { PartnerTypeListService } from './../../../Common/Services/StandardLists/PartnerTypeListService';
import { GLAccountExtendedPMService } from './../../Services/ExtendedPMs/GLAccountExtendedPMService';
import {Component} from '@angular/core';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {GLAccountPM} from '../../EntityPMs/GLAccountPM';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../Infrastructure/Tools';
import {CustomerList} from '../../../Common/EntityLists/CustomerList';
import {CustomerListService} from '../../../Common/Services/StandardLists/CustomerListService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';
import { GLAccountPMService } from '../../Services/StandardPMs/GLAccountPMService';
import { CardList } from '../../../Common/EntityLists/CardList';
import { PartnerTypeList } from '../../../Common/EntityLists/PartnerTypeList';
@Component({
    
    templateUrl: "./GLAccountShortTitleComponent.html",
})






export class GLAccountShortTitleComponent {
    public EntityPM: GLAccountPM;
    public isRTL: boolean = false;
    public IsConnectedToOneCard: boolean = false;
    public IsConnectedToMoreThanOneCard:boolean= false;
    private CurrentSession = SessionLocator.SelectedSession;
    GLAccountPMService: GLAccountPMService = new GLAccountPMService();
    _GLAccountExtendedPMService: GLAccountExtendedPMService = new GLAccountExtendedPMService();

    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = this.entityArgs.EntityPM;

        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");

        if (this.EntityPM != null) {
        }
        this.Listen();

        this.SetObjectTableNameAndTabCode();
        this.CheckIsConnectedCard(this.EntityPM.CustomerGLAccountId || this.EntityPM.Id);
        this.GetConnectedCards(this.EntityPM.CustomerGLAccountId || this.EntityPM.Id);

        console.log("[GLAccountShortTitleComponent]");

    }

    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;

                }
            });

            this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;

                }
            });
        }
    }
    ObjectTableName: string;
    SelectedTabCode: string;
    public ConnectedCards:CardList[]=[];
    _PartnerTypeListService: PartnerTypeListService = new PartnerTypeListService();
    OpenCardScreen(cardId:string)
    {
        var selectedCard;
        if(this.ConnectedCards.length==1){
             selectedCard = this.ConnectedCards[0];
        }
        else{
            selectedCard = this.ConnectedCards.filter(d=> d.Id== cardId)[0];
         }
      //  var accountId = this.EntityPM.CustomerGLAccountId || this.EntityPM.Id; // if GLAccount is splitted, (EntityPM.CustomerGLAccountId) is filled
       // this.GetConnectedCards(accountId).then((connectedCards: CardList[]) => {
          //  var firstConnectedCard = connectedCards[0];
          
           // var partnerTypeObjectTableName = this.GetPartnerTypeObjectTableName(firstConnectedCard.PartnerTypeId);
         //   this.OpenCard(firstConnectedCard.Id, partnerTypeObjectTableName);

      //  });
      
      var partnerTypeObjectTableName = this.GetPartnerTypeObjectTableName(selectedCard.PartnerTypeId);
      this.OpenCard(selectedCard.Id, partnerTypeObjectTableName);

    }

    private OpenCard(connectedCardId: string, partnerTypeName: string)
    {
        if (!AppTool.IsNullOrEmpty(connectedCardId)) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef =>
                {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: connectedCardId, ObjectTableName: partnerTypeName == "Others" ? "Vendor" : partnerTypeName, SelectedTabCode: this.SelectedTabCode });
                    cmpRef.instance.BackCompleted.subscribe(bk =>
                    {
                    });
                });
        }
    }

    private SetObjectTableNameAndTabCode() {

        if (this.EntityPM.AccountTypeCode == "3") {
            this.ObjectTableName = "Vendor";
            this.SelectedTabCode = null;
        }
        else if (this.EntityPM.AccountTypeCode == "2") {
            this.ObjectTableName = "Customer";
            this.SelectedTabCode = "CLOV";
        }

    }

    GetConnectedCards(accountId: string)
    {

        return new Promise(resolve =>
        {
            this.CurrentSession.StartBusyIndicatorLoading();
            this._GLAccountExtendedPMService.GetConnectedCardsForGLAccount(accountId)
                .subscribe((myResponse: ServiceResponse) =>
                {
                    this.CurrentSession.StopBusyIndicator();
                   this.ConnectedCards = myResponse.Result;
                   
                    if ( this.ConnectedCards)
                        resolve( this.ConnectedCards);
                });
        });
    }

    GetPartnerType(partnerTypeId: string)
    {

        return new Promise(resolve =>
        {
            this.CurrentSession.StartBusyIndicatorLoading();
            this._PartnerTypeListService.getSingle(partnerTypeId)
                .subscribe((myResponse: ServiceResponse) =>
                {
                    this.CurrentSession.StopBusyIndicator();
                    var partnerType = myResponse.Result;
                    resolve(partnerType);
                });
        });
    }

    CheckIsConnectedCard(accountId: string) {
        this._GLAccountExtendedPMService.GetConnectedCardsForGLAccount(accountId).subscribe((myResponse: ServiceResponse) => {
            var connectedCards = myResponse.Result;
            if (connectedCards.length == 1) {
                this.IsConnectedToOneCard = true;
            }
            else if(connectedCards.length >1){
                this.IsConnectedToMoreThanOneCard = true;
            }
            });

    }

    GetPartnerTypeObjectTableName(partnerTypeId: string){
        var objectTableName;
        switch (partnerTypeId) {
            case 'AG': { objectTableName = 'Agent'; break; }
            case 'AL': { objectTableName = 'Airline'; break; }
            case 'CG': { objectTableName = 'CustomAgent'; break; }
            case 'CH': { objectTableName = 'CustomsShipper'; break; }
            case 'CS': { objectTableName = 'Customer'; break; }
            case 'PO': { objectTableName = 'Customer'; break; }
            case 'PT': { objectTableName = 'Participant'; break; }
            case 'SG': { objectTableName = 'ShippingAgent'; break; }
            case 'SL': { objectTableName = 'ShippingLine'; break; }
            case 'TR': { objectTableName = 'Trucker'; break; }
            case 'VD': { objectTableName = 'Vendor'; break; }
            case 'WH': { objectTableName = 'Warehouse'; break; }
            case 'AC': { objectTableName = 'AccountingPartner'; break; }

            case 'CC': { objectTableName = 'Custom Clearance'; break; } // not found
            case 'CO': { objectTableName = 'Coloader'; break; } // not found
            case 'FL': { objectTableName = 'Freelancer'; break; } // not found
            case 'OT': { objectTableName = 'Others'; break; } // not found
        }
        return objectTableName;
    }
}
