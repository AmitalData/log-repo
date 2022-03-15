import { Component, ViewChildren, QueryList, OnInit } from '@angular/core';
import { LocationDirective } from '../../../Infrastructure/Utilities/LocationDirective';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';

@Component({
  selector: 'TariffComponent',
  templateUrl: './TariffWorkspaceComponent.html',
  providers: [EntityResourceService],
})

export class TariffWorkspaceComponent {
  public HasSettingMenu: boolean = false;
  @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
  private CurrentSession = SessionLocator.SelectedSession;
  constructor(private _entityResourceService: EntityResourceService) {

    if (FeatureLocator.HasFeaturePermession("TariffSetting", "Module")) {
      this.HasSettingMenu = true;
    }

    this.RunComponent();
  }

  private isLoaderReady: boolean = false;
  RunComponent() {
    if (this.AllLocations) {
      if (this.AllLocations.length == 0) {
        this.RunComponentTimer();
      }
      else {
        this.isLoaderReady = true;
        this.SelectedItem = "COST";
      }
    }
    else {
      this.RunComponentTimer();
    }
  }

  private Retries: number = 0;
  private timerToken: any;
  private RunComponentTimer() {
    this.Retries++;
    if (this.timerToken) {
      clearTimeout(this.timerToken);
    }
    if (this.Retries < 20) {
      this.timerToken = setTimeout(() => this.RunComponent(), 1);
    }
  }

  private selectedItem: string;
  get SelectedItem() { return this.selectedItem; }
  set SelectedItem(newValue: string) {
    if (this.selectedItem != newValue) {
      this.selectedItem = newValue;
      this.SelectionChanged();
    }
  }

  private Page_TW: any = null;
  private Page_DW: any = null;
  private Page_SA: any = null;
  SelectionChanged() {
    if (this.isLoaderReady) {
      if (this.SelectedItem != null) {

        let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == this.SelectedItem)[0];
        if (myLocation != null) {

          switch (this.SelectedItem) {

            case "COST": {
              if (this.Page_TW == null) {
                  this._entityResourceService.getEntityResourceByTableName("Tariff", 0).subscribe((response: any) => {
                      this._entityResourceService.getEntityResourceByTableName("TariffLinesContainersPrice", 0).subscribe((response: any) => {
                          SessionLocator.DynamicLoader.Load('./TariffModule/Components/Workspaces/CostWorkspaceComponent', myLocation.viewContainerRef)
                              .then(cmpRef => {
                                  this.Page_TW = cmpRef.instance;
                              });
                      });
                  });
              }
              else {
                this.Page_TW.LoadAllScreenData();
              }

              break;
            }
              case "SALE": {
                  if (this.Page_SA == null) {
                      this._entityResourceService.getEntityResourceByTableName("Tariff", 0).subscribe((response: any) => {
                          this._entityResourceService.getEntityResourceByTableName("TariffLinesContainersPrice", 0).subscribe((response: any) => {
                              SessionLocator.DynamicLoader.Load('./TariffModule/Components/Workspaces/SaleWorkspaceComponent', myLocation.viewContainerRef)
                                  .then(cmpRef => {
                                      this.Page_SA = cmpRef.instance;
                                  });
                          });
                      });
                  }
                  else {
                      this.Page_SA.LoadAllScreenData();
                  }

                  break;
              }
            case "SETG": {
              if (this.Page_DW == null) {
                this._entityResourceService.getEntityResourceByTableName("Tariff", 0).subscribe((response: any) => {
                  SessionLocator.DynamicLoader.Load('./TariffModule/Components/Workspaces/SettingsWorkspaceComponent', myLocation.viewContainerRef)
                    .then(cmpRef => {
                      this.Page_DW = cmpRef.instance;
                    });
                });
              }

              break;
            }
          }
          this.CurrentSession.ChangeSessionHeader({ Text: TextCodeTranslator.Translate("General.MH.TariffModule") });
        }
      }
    }
  }

  private mySelectedActivityFilter: string = "All";
  get SelectedActivityFilter() { return this.mySelectedActivityFilter; }
  set SelectedActivityFilter(value: string) {
    if (this.mySelectedActivityFilter != value) {
      this.mySelectedActivityFilter = value;
      this.Page_TW.SelectedActivityFilter = value;
    }
  }
}
