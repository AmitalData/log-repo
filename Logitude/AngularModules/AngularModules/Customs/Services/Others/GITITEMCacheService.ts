
import { Injectable, EventEmitter } from '@angular/core';
import { AppTool } from '../../../Infrastructure/Tools';
//ConfirmWindow
import { CommunicationLogStepListService } from '../../../Common/Services/ExtendedLists/CommunicationLogStepListService';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
//import { BaseRequestsSheetMassaging } from '../../Customs/Components/CustomsRequests/BaseRequestsSheetMassaging';
import { BaseRequestsSheetMassaging } from '../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { DownloadManager } from '../../../Infrastructure/Utilities/DownloadManager';
import { GITITEMExtendedPMService } from '../../../Customs/Services/ExtendedPMs/GITITEMExtendedPMService';
//import { ItemCodeComponent } from './Components/SupplierInvoices/SupplierInvoiceGeneralTabComponent';

import { ItemCodeComponent } from '../../../CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/SupplierInvoiceGeneralTabComponent';
import { GITITEMDto } from '../../EntityPMs/Extended/GITITEMDto';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { CustomsSettingExtendedListService } from '../../../Customs/Services/ExtendedLists/CustomsSettingExtendedListService';

@Injectable()
export class GITITEMCacheService {
  private static _instance: GITITEMCacheService;
  private constructor() {
    this._ItemCode_LocalCache = [];
      this.GetCountryPURForItems();
      this.GetUnitPURForItems();
  }

  GITITEMExtendedPMService: GITITEMExtendedPMService = new GITITEMExtendedPMService();


    public IsCountryPURForItems: boolean = false;
    public IsUnitPURForItems: boolean = false;

  private _ItemCode_LocalCache: ItemCodeComponent[];
  public get ItemCode_LocalCache(): ItemCodeComponent[] {
    return this._ItemCode_LocalCache;
  }
  //public set ItemCode_LocalCache(value: ItemCodeComponent[]) {
  //  this._ItemCode_LocalCache = value;
  //}


  private GetCountryPURForItems() {
    //SessionLocator.CurrentSession.StartBusyIndicator("Customs.General.O.Loading");
    var myCustomsSettingExtendedListService = new CustomsSettingExtendedListService();
    myCustomsSettingExtendedListService.GetDefault("ISRAEL", "CGG_I_PUR_CTRY", "NON", "NON", SessionLocator.Tenant)
      .subscribe(response => {
        //SessionLocator.CurrentSession.StopBusyIndicator();
        if (!response.HasError && response.Result != null && response.Result.DefaultValue == "Y") {
          this.IsCountryPURForItems = true;
        }
      });
  }


    private GetUnitPURForItems() {
        var myCustomsSettingExtendedListService = new CustomsSettingExtendedListService();
        myCustomsSettingExtendedListService.GetDefault("ISRAEL", "CGG_I_PUR_UNIT", "NON", "NON", SessionLocator.Tenant)
            .subscribe(response => {
                if (!response.HasError && response.Result != null && response.Result.DefaultValue == "Y") {
                    this.IsUnitPURForItems = true;
                }
            });
    }

    public SaveItemCodeLocalCache() {
        let listGITITEMDto: GITITEMDto[] = [];
    if (this.ItemCode_LocalCache != null && this.ItemCode_LocalCache.length > 0) {
      for (let item of this.ItemCode_LocalCache) {
        if (item.IsNew) {
          item.IsNew = false;     //
          var myGITITEMPM = new GITITEMDto();
          //myGITITEMPM.PARTNERID = this.declarationPM.CustomerCode;
          if (AppTool.IsNullOrEmpty(item.VendorNumber)) {
            item.VendorNumber = "NULL";
          }
          myGITITEMPM.PARTNERID = item.CustomerCode;
          myGITITEMPM.SAPAKID = item.VendorNumber;
          myGITITEMPM.ITEMNO = item.ItemCode;
          myGITITEMPM.PRATID = item.ClassificationCode;
          myGITITEMPM.NAMEENG = item.ItemDescription;
          myGITITEMPM.ORIGINCOUNTRY = item.OriginCountryCode;
          myGITITEMPM.UNITID = item.InvoiceQuantityType;

          //this.GITITEMExtendedPMService.insert(myGITITEMPM).subscribe(myResult => {
          //  var mm: ServiceResponse = myResult;
          //  if (!mm.HasError) {
          //    //this.entity = mm.Result;
          //  }
          //});
            listGITITEMDto.push(myGITITEMPM);
        }
        }

        let i = 0;
        let j = 0;
        let chunk = 50;
        for (i = 0, j = listGITITEMDto.length; i < j; i += chunk) {
            let chunkDtos = listGITITEMDto.slice(i, i + chunk);
            this.GITITEMExtendedPMService.insert(chunkDtos).subscribe(myResult => {
                var mm: ServiceResponse = myResult;
                if (!mm.HasError) {
                    var entity = mm.Result;
                }
            });
        }

    }
  }

  public static get Instance() {
    return this._instance || (this._instance = new this());
  }
}
