declare var window: any;
import { Component, AfterViewInit, ViewChild } from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {ShipmentPMService} from '../../../../Shipment/Services/StandardPMs/ShipmentPMService';
import {EntityLastActivityService} from '../../../../Infrastructure/Services/EntityLastActivityService';
import {ShipmentTool} from '../../../../Shipment/Tools';
import {AWBWizardArgs, FSRWizardArgs} from '../../../../Shipment/Args';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { ChildDirective } from '../../../../Infrastructure/Directives/ChildDirective';

@Component({
  templateUrl: './AWBWizardLoadComponent.html',
})

export class AWBWizardLoadComponent implements AfterViewInit {
  public EntityId: string = null;
  public EntityPM: ShipmentPM;
  public childComponentInstance: any = null;
  private CurrentSession = SessionLocator.SelectedSession;
  @ViewChild(ChildDirective) Child: ChildDirective;

  SetWindowArgs(entityId: string) {
    this.CurrentSession.StartBusyIndicatorLoading();
    this.EntityId = entityId;
    this.Load();
  }

  private isViewInited = false;
  ngAfterViewInit() {
    this.isViewInited = true;
    this.Load();
  }

  private Load() {
    if (this.EntityId != null && this.isViewInited) {

      var myService: ShipmentPMService = new ShipmentPMService();

      myService.get(this.EntityId).subscribe((myResponse: ServiceResponse) => {
        if (myResponse != null) {
          if (!myResponse.HasError) {
            this.EntityPM = myResponse.Result;

            if (this.EntityPM != null) {
              this.ImportWizard();
              this.SendActivityLog();
            }
          }
        }

        this.CurrentSession.StopBusyIndicator();
      });
    }
  }

  private ImportWizard() {

    var isFullWizard: boolean = ShipmentTool.IsFullAWBWizard(this.EntityPM.DirectionId);

    if (isFullWizard) {

      var myAWBWizardArgs: AWBWizardArgs = new AWBWizardArgs();
      myAWBWizardArgs.EntityPM = this.EntityPM;
      myAWBWizardArgs.ShipmentLevelCode = this.EntityPM.ShipmentLevelCode;

      SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentAWB/Components/AWBWizard/AWBWizardComponent', this.Child.Location)
        .then(cmpRef => {
          cmpRef.instance.SetWindowArgs(myAWBWizardArgs);
          this.CurrentSession.StopBusyIndicator();
          this.childComponentInstance = cmpRef.instance;
        });
      }
      
      else {
      var myFSRWizardArgs: FSRWizardArgs = new FSRWizardArgs();
      myFSRWizardArgs.EntityPM = this.EntityPM;
      myFSRWizardArgs.ShipmentLevelCode = this.EntityPM.ShipmentLevelCode;

      SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentAWB/Components/FSRWizard/FSRWizardComponent', this.Child.Location)
      .then(cmpRef => {
        cmpRef.instance.SetWindowArgs(myFSRWizardArgs);
        this.CurrentSession.StopBusyIndicator();
        this.childComponentInstance = cmpRef.instance;
        });
    }
  }

  private SendActivityLog() {
    var ObjectTableName = "Shipment";
    var ObjectTable = window.ObjectTables.filter(x => x.Name === ObjectTableName)[0];
    var ObjectTableId = ObjectTable.Id;

    var myService: EntityLastActivityService = new EntityLastActivityService();
    myService.AddActivityLog(this.EntityId, ObjectTableId, SessionLocator.LoggedUserId, 'V').subscribe();
  }
}
