import { Component, OnInit } from '@angular/core';
import { ManifestSL } from '../../../Common/DataContracts/ManifestSL';
import {AppTool, DateTool} from '../../../Infrastructure/Tools';
import { AgentSharedManifestList } from '../../../Common/EntityLists/AgentSharedManifestList';
import { AgentSharedManifestPM } from '../../../Common/EntityPMs/AgentSharedManifestPM';

@Component({
    moduleId: module.id,
    selector: 'SharedManifestHeaderComponent',
    templateUrl: './SharedManifestHeaderComponent.html',
})
export class SharedManifestHeaderComponent {
    SharedManifestHeaderData: SharedManifestHeader;
    constructor() {

    }

    Run(currentEntity: AgentSharedManifestPM, entityList: AgentSharedManifestList ) {
        this.SharedManifestHeaderData = new SharedManifestHeader(currentEntity, entityList);
    }
}


export class SharedManifestHeader {
    DirectionId: string = "";
    Route: string = "";
    LongMaster: string = "";
    ShipperName: string;
    CreateDate: Date;
    TransportModeId: string = "";
    AgentName: string = "";
    ShipmentLevelName: string = "";
    AgentReference: string = "";
    CarrierName: string = "";
    FreightPrepaidCollectId: string = "";
    OtherPrepaidCollectId: string = "";
    MainCarriageETA: Date;
    MainCarriageATD: Date;
    ManifestSL: ManifestSL;
    ShipmentLevelCode: string = "";
    MAWBOBLDate: Date;

    constructor(entityPM: AgentSharedManifestPM, entityList: AgentSharedManifestList) {

        if (entityPM) {
            this.CreateDate = entityPM.CreateDate;
            this.AgentReference = entityPM.AgentReference;
            this.ManifestSL = entityPM.ManifestSL;
        }

        if (entityList) {
            this.Route = entityList.Routing;
            this.ShipmentLevelName = entityList.ShipmentLevelName;
        }

        if (this.ManifestSL) {
            this.LongMaster = this.ManifestSL.LongMaster;
            this.AgentName = this.ManifestSL.AgentName;
            this.ShipperName = this.ManifestSL.ShipperName;
            this.CarrierName = this.ManifestSL.CarrierName;
            this.FreightPrepaidCollectId = this.ManifestSL.FreightPrepaidCollectId;
            this.OtherPrepaidCollectId = this.ManifestSL.OtherPrepaidCollectId;
            this.TransportModeId = this.ManifestSL.TransportModeId;
            this.DirectionId = this.ManifestSL.DirectionId;
            this.MainCarriageATD = this.ManifestSL.MainCarriageATD;
            this.MainCarriageETA = this.ManifestSL.MainCarriageETA;
            this.MAWBOBLDate = this.ManifestSL.MAWBOBLDate;
            this.ShipmentLevelCode = this.ManifestSL.ShipmentLevelCode;
            
        }
    }
}



