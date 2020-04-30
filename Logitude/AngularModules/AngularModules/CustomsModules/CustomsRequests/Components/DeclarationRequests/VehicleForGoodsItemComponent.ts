declare var window: any;

import { Directive, ChangeDetectorRef, Input, Output, Component, OnInit, OnChanges, EventEmitter, AfterViewInit } from '@angular/core';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { LogTab } from '../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, ArrayTool } from '../../../../Infrastructure/Tools';
import { CommunicationLogStepDataViewModel } from '../../../../InfrastructureModules/InfrastructureCommunications/Components/CommunicationLog/ViewModel/CommunicationLogStepDataViewModel';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    selector: 'VehicleForGoodsItemComponent',
    
    templateUrl: './VehicleForGoodsItemComponent.html',
})

export class VehicleForGoodsItemComponent
    extends BaseComponent
    implements OnInit {

    //public ObjectTableName: string = "Customs.CommunicationLog";
    public DataContext: VehicleForGoodsItemComponent = this;
    public Tab: LogTab;
    public IsDisplayOnly: boolean = false;

    VehiclesDetailsList: ObservableCollection;

    constructor(public entityArgs: EntityArgs, private cd: ChangeDetectorRef) {
        super();
        this.VehiclesDetailsList = new ObservableCollection([]);
    }

    ngOnInit() {

    }

    SetWindowArgs(vehiclesList: ObservableCollection) {
        vehiclesList.Collection.forEach((item) => {
            this.VehiclesDetailsList.Insert(item);
        });
    }
}
