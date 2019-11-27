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
    moduleId: module.id,
    templateUrl: "./GLAccountShortTitleComponent.html",
})






export class GLAccountShortTitleComponent {
    public EntityPM: GLAccountPM;
    public isRTL: boolean = false;
    public IsConnectedCard: boolean = false;
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

    _PartnerTypeListService: PartnerTypeListService = new PartnerTypeListService();
    OpenCardScreen()
    {
        var accountId = this.EntityPM.CustomerGLAccountId || this.EntityPM.Id; // if GLAccount is splitted, (EntityPM.CustomerGLAccountId) is filled
        this.GetConnectedCards(accountId).then((connectedCards: CardList[]) => {
            var firstConnectedCard = connectedCards[0];

            this.GetPartnerType(firstConnectedCard.PartnerTypeId).then((partnerType: PartnerTypeList) =>
            {
                this.OpenCard(firstConnectedCard.Id, partnerType);
            });

        });
    }

    private OpenCard(connectedCardId: string, partnerType: PartnerTypeList)
    {
        if (!AppTool.IsNullOrEmpty(connectedCardId)) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef =>
                {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: connectedCardId, ObjectTableName: partnerType.Name, SelectedTabCode: this.SelectedTabCode });
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
                    var connectedCards = myResponse.Result;
                    if (connectedCards)
                        resolve(connectedCards);
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
            if (connectedCards.length > 0) {
                this.IsConnectedCard = true;
            }
            });

    }
}
