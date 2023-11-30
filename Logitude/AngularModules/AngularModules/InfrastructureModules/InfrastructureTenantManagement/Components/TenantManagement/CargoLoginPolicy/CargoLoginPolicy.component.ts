import { Component, OnInit } from '@angular/core';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { TenantManagementPM } from 'Infrastructure/EntityPMs/TenantManagementPM';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';

@Component({
  selector: 'app-CargoLoginPolicy',
  templateUrl: './CargoLoginPolicy.component.html',
  styleUrls: ['./CargoLoginPolicy.component.scss']
})
export class CargoLoginPolicyComponent extends BaseComponent implements OnInit {
  entityPM: TenantManagementPM;
  private CurrentSession = SessionLocator.SelectedSession;
  public cargoTokenTimeout:number;

  constructor() {
    super();
  }

  ngOnInit() {
    this.cargoTokenTimeout = this.entityPM.CargoTokenTimeout;
  }

  SetWindowArgs(myArg: TenantManagementPM) {
    this.entityPM = myArg;

  }
  CancelButtonClicked() {
    this.CurrentSession.CloseCurrentWindow();
  }
  OkButtonClicked(){
    this.entityPM.CargoTokenTimeout = this.cargoTokenTimeout ;
    this.CancelButtonClicked();
  }
}
