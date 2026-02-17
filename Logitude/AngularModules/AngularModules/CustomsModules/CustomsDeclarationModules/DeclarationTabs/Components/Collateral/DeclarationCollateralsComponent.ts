declare var System: any;
declare var window: any;
import { Component, OnInit, OnDestroy } from '@angular/core';
import { AppTool, ArrayTool } from '../../../../../Infrastructure/Tools';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { FeatureLocator } from '../../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { LogTab } from '../../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { MessageWindow } from '../../../../../Controls/Windows/MessageWindow';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { DeclarationWebService } from '../../../../../Customs/Services/WebServices/DeclarationWebService';
import { TapagPMService } from '../../../../../Customs/Services/StandardPMs/TapagPMService';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { DeclarationPM } from '../../../../../Customs/EntityPMs/DeclarationPM';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';;
import { CustomsCollateralPM } from '../../../../../Customs/EntityPMs/CustomsCollateralPM';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';
import { CustomsCollateralPMService } from '../../../../../Customs/Services/StandardPMs/CustomsCollateralPMService';

@Component({
  moduleId: module.id,
  templateUrl: './DeclarationCollateralsComponent.html',
})

export class DeclarationCollateralsComponent extends BaseComponent implements OnInit, OnDestroy {
  public EntityPM: DeclarationPM = null;
  public ObjectTableName = "Customs.Declaration";
  public DataContext: DeclarationCollateralsComponent = this;
  public CurrentEditComponentId: string;
  public collateralObslist: ObservableCollection;

  private _DeclarationWebService: DeclarationWebService = new DeclarationWebService;
  private _CustomsCollateralPMService: CustomsCollateralPMService = new CustomsCollateralPMService;

  IsLoaded: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
  constructor(private entityArgs: EntityArgs, private EntityResourceService: EntityResourceService) {
    super();
    this.collateralObslist = new ObservableCollection([]);

    this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsCollateral").subscribe((response: any) => {
      this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsCollateralsAnswer").subscribe((response: any) => {
        this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsCollateralsCondition").subscribe((response: any) => {
          this.EntityPM = this.entityArgs.EntityPM;
          this.LoadDeclarationCollateralsList();
          this.Listen();
          this.IsLoaded = true;
        });
      });
    });
  }

  ngOnInit() {
    this.EntityPM = this.entityArgs.EntityPM;
  }

  ngOnDestroy() {
    console.log("DeclarationCollateralsComponent:ngOnDestroy");
    this.entityArgs = null;
  }
  private Listen() {
    if (this.CurrentSession.CurrentEditComponent != null) {

      this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;

      this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
        this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
          if (isSaveSuccess) {
            this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
          }
        })
      );

      this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
        this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
          if (isLoadSuccess) {
            this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
            this.LoadDeclarationCollateralsList();
          }
        })
      );

      this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
        this.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
          if (this.CurrentEditComponentId == this.CurrentSession.CurrentEditComponent.ComponentId) {
            if (tabCode == "DCCL") {
              this.LoadDeclarationCollateralsList();
            }
          }
        })
      );
    }
  }

  private LoadDeclarationCollateralsList() {
    this.collateralObslist = new ObservableCollection([]);

    this._DeclarationWebService.GetDeclarationCollateralsList(this.EntityPM.Id, this.EntityPM.Tenant)
      .subscribe((myResponse: ServiceResponse) => {
        this.CurrentSession.StopBusyIndicator();
        this.GetDeclarationCollateralsListsOp_Completed(myResponse, false);
      });
  }

  private GetDeclarationCollateralsListsOp_Completed(myResponse: ServiceResponse, sourceIsCostomFile: boolean) {
    if (myResponse.Result != null) {
      myResponse.Result.forEach((item: CustomsCollateralPM) => {
        this.collateralObslist.Insert(item);
      });
    }
  }

  RefreshEntity() {
    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
  }

  EditButtonClicked(item: CustomsCollateralPM) {
    if (!AppTool.IsNullOrEmpty(item)) {
      this._CustomsCollateralPMService.get(item.Id).subscribe(response => {
        var windowArgs: any = {};
        windowArgs.CurrentEntity = response.Result;
        windowArgs.declarationPM = this.EntityPM;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 600;
        logWindow.Height = 700;
        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./CustomsModules/CustomsCollateral/Components/CustomsCollateralComponent');
        this.CurrentSession.StopBusyIndicator();

      });
    }

  }
}
