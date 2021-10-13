
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
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';

@Injectable()
export class GITITEMCacheService {
    private static _instance: GITITEMCacheService;
    private CurrentSession = SessionLocator.SelectedSession;
  private constructor() {
    this._ItemCode_LocalCache = [];
      this.GetCountryPURForItems();
      this.GetUnitPURForItems();
  }

  GITITEMExtendedPMService: GITITEMExtendedPMService = new GITITEMExtendedPMService();


    public IsCountryPURForItems: boolean = false;
    public IsUnitPURForItems: boolean = false;

  private _ItemCode_LocalCache: ItemCodeComponent[];
  /*public*/private get ItemCode_LocalCache(): ItemCodeComponent[] {
    return this._ItemCode_LocalCache;
  }
  //public set ItemCode_LocalCache(value: ItemCodeComponent[]) {
  //  this._ItemCode_LocalCache = value;
  //}

    FirstItemCodeComponent(itemCode) {
        var itemCodeDetails = GITITEMCacheService.Instance.ItemCode_LocalCache.filter(vm => vm.ItemCode == itemCode)[0];
        return itemCodeDetails;
    }
    AddItemCodeComponent(ItemCodeComponent) {
        GITITEMCacheService.Instance.ItemCode_LocalCache.push(ItemCodeComponent);
    }
                                        
    //public const  ToChangeRow ="ToChangeRow"
    OnItemCodeAdd(mySupplierInvoiceItemPM, itemCodeDetails): Promise<OnItemCodeAddResult> {
        //ToChangeRowTrue_ToChangeDBFalse
        return new Promise<OnItemCodeAddResult>(resolve => {
            let isChanged: boolean = false;
            if (!AppTool.IsNullOrEmpty(mySupplierInvoiceItemPM.ClassificationCode) && mySupplierInvoiceItemPM.ClassificationCode != itemCodeDetails.ClassificationCode) {
                isChanged = true;
            }
            if (!AppTool.IsNullOrEmpty(mySupplierInvoiceItemPM.ItemDescription) && mySupplierInvoiceItemPM.ItemDescription != itemCodeDetails.ItemDescription) {
                isChanged = true;
            }
            if (GITITEMCacheService.Instance.IsUnitPURForItems && !AppTool.IsNullOrEmpty(mySupplierInvoiceItemPM.InvoiceQuantityType) && mySupplierInvoiceItemPM.InvoiceQuantityType != itemCodeDetails.InvoiceQuantityType) {
                isChanged = true;
            }
            if (GITITEMCacheService.Instance.IsCountryPURForItems && !AppTool.IsNullOrEmpty(mySupplierInvoiceItemPM.OriginCountryCode) && mySupplierInvoiceItemPM.OriginCountryCode != itemCodeDetails.OriginCountryCode) {
                isChanged = true;
            }



            if (!isChanged) {
                resolve(OnItemCodeAddResult.voidDoNothing);
                return;
            }
            let confirmWindow = new ConfirmWindow();
            confirmWindow.Show("הערכים בטבלת פריטים שונים , האם לדרוס ערכי השורה ?")
            console.log("אחרת הנתונים הנ'ל יועדכנו ב DB !!!!!!!!!!!");
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    console.log("לדרוס ערכי השורה ")

                    resolve(OnItemCodeAddResult.OverwriteRowFromDB);
                    return;
                } else if (confirmWindow.No) {
                    console.log("אחרת הנתונים הנ'ל יועדכנו ב DB !!!!!!!!!!!");
                    resolve(OnItemCodeAddResult.AddTaskToUpdateDB);
                    return;
                }
            })
        });


    }

  private GetCountryPURForItems() {
    //this.CurrentSession.StartBusyIndicator("Customs.General.O.Loading");
    var myCustomsSettingExtendedListService = new CustomsSettingExtendedListService();
    myCustomsSettingExtendedListService.GetDefault("ISRAEL", "CGG_I_PUR_CTRY", "NON", "NON", SessionLocator.Tenant)
      .subscribe((response:any) => {
        //this.CurrentSession.StopBusyIndicator();
        if (!response.HasError && response.Result != null && response.Result.DefaultValue == "Y") {
          this.IsCountryPURForItems = true;
        }
      });
  }


    private GetUnitPURForItems() {
        var myCustomsSettingExtendedListService = new CustomsSettingExtendedListService();
        myCustomsSettingExtendedListService.GetDefault("ISRAEL", "CGG_I_PUR_UNIT", "NON", "NON", SessionLocator.Tenant)
            .subscribe((response:any) => {
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
            myGITITEMPM.TARIFFID = item.TariffID;
            myGITITEMPM.GITITEMCRPMs = item.GITITEMCRPMs;

          //this.GITITEMExtendedPMService.insert(myGITITEMPM).subscribe((myResult:any) => {
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
            this.GITITEMExtendedPMService.insert(chunkDtos).subscribe((myResult:any) => {
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
export enum OnItemCodeAddResult {
    voidDoNothing=0,OverwriteRowFromDB=1,AddTaskToUpdateDB=2
}
