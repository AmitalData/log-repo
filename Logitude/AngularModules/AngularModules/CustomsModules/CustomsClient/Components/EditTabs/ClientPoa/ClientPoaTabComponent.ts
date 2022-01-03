import {Component}  from '@angular/core';
import {LogitudeWindow} from '../../../../../Controls/Windows/LogitudeWindow';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import {ConfirmWindow} from '../../../../../Controls/Windows/ConfirmWindow';
import { CustomMessageProgressComponent } from '../../../../CustomsControls/Components/CustomMessageProgressComponent';
import {ClientMessagesService} from '../../../../../Customs/Services/WebServices/ClientMessagesService';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {INF_MSG_GenericResponseData} from '../../../../../Customs/DataContract/ResponseData/INF_MSG_GenericResponseData';
import { ClaimGeneralTabComponent } from '../../../../CustomsClaim/Components/EditTabs/General/ClaimGeneralTabComponent';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';
import { ClientPM } from 'Customs/Entitypms/ClientPM';

@Component({
    
    templateUrl: './ClientPoaTabComponent.html',
})

export class ClientPoaTabComponent extends BaseComponent{

    public entityResourceService: EntityResourceService = new EntityResourceService();
    objectTableName: string = "Customs.ClientsPoa";
    public PoaList: ObservableCollection;
    entityPM: ClientPM;
    clientMessageService: ClientMessagesService = new ClientMessagesService();
    responseData: INF_MSG_GenericResponseData;

    Mode: string = "";
    Parent: ClaimGeneralTabComponent;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _EntityArgs: EntityArgs) {
        super();
        this.PoaList = new ObservableCollection([]);

    }

    InitTab(EntityPM: ClientPM) {

        this.entityPM = EntityPM;
        if (this.entityPM.ClientPoas) {
            this.entityPM.ClientPoas.forEach((item) => {
                this.PoaList.Insert(item);
            });
        }

    }

    SetWindowArgs(args: any) {
        if (args != null) {
            this.Mode = "Claims";
            this.entityPM = args.EntityPM;
            this.Parent = args.Parent;
        }
    }

   
    public operationType: any;
    
    ReloadEntityPM() {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    }

   

    
   
}


