import {Component,ChangeDetectorRef} from '@angular/core'; 
import {WebFreightDomainService} from '../../../Infrastructure/Services/WebFreightDomainService';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';

@Component({

    template: `<table>
                <tr>
                <td>
                       <div *ngIf="ShowImg" style="width: 100px;height:35px;text-indent: 10px; overflow: hidden; text-overflow: ellipsis;float:left;margin-right: 5px;margin-top: -8px;">
                       <img *ngIf="Source" style="width: 70px;max-height:35px;" src="{{Source}}"  title="{{rowData ? rowData['PartnerName'] : ''}}" />
                       </div>
                </td>
                <td *ngIf="ToggleIsExportShipments">
                       <div style="text-indent: 10px; overflow: hidden; text-overflow: ellipsis;float:left;">
                        <img width="18" height="15" style="vertical-align: middle;margin-left: -7px;" [src]="DirectionSRC" title="{{rowData ? rowData['DirectionName']:''}}" />
                       </div>
                </td>
                <td>
                       <div style="text-indent: 10px; overflow: hidden; text-overflow: ellipsis;float:left;">
                        <img width="18" height="15" style="vertical-align: middle;margin-left: -7px;" [src]="TransportModSRC" title="{{rowData ? rowData['TransportModeName']:''}}" />
                       </div>
                </td> 
                <td style="width:90px;">
                        <div style="text-indent: 10px; overflow: hidden; text-overflow: ellipsis;float:left; position: absolute;top: 0;bottom: 0;left: 0;right: 0;">
                        <span style="text-overflow: ellipsis;overflow: hidden;white-space: nowrap;" *ngIf="fieldName == 'My Shipments'">{{rowData ? rowData['CustomerReference1']:''}}</span>
                        <span style="text-overflow: ellipsis;overflow: hidden;white-space: nowrap;" *ngIf="fieldName != 'My Shipments'">{{rowData ? rowData['ForwarderShipmentNumber']:''}}</span>
                        </div>
                </td>
                </tr>
              </table>
            `
})

export class ReferenceNumberCellDisplayListTemplate {

    public rowData: any;
    public fieldName: any;
    public Source: any = "";
    public ShowImg: boolean = true;
    public TransportModSRC: string = '';
    public DirectionSRC: string = '';

    //public Imgs: Logosdictionary[];
    private CurrentSession = SessionLocator.SelectedSession;
    public ToggleIsExportShipments: boolean = false;
    constructor(private CD: ChangeDetectorRef) {
        if (!this.CurrentSession.Imgs) {
            this.CurrentSession.Imgs = [];
        }
        var FeatureToggle = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "LEX" && d.TenantNumber == SessionLocator.Tenant)[0];
        if (FeatureToggle) {
            this.ToggleIsExportShipments = true;
        }
    }

    setVariables(rowData: any, fieldName: string) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        if (this.rowData['TransportModeId']) {
            this.TransportModSRC = './Images/TransportModes/' + rowData['TransportModeId'] + '.png';
        }
        if (this.rowData['DirectionId']) {
            this.DirectionSRC = './Images/Directions/' + rowData['DirectionId'] + '.png';
        }  
        var myService: WebFreightDomainService = new WebFreightDomainService();
        if (!SessionLocator.PrivateLableSettings) {
            this.ShowImg = true;
            if (this.CurrentSession.Imgs.filter(a => a.LogoId == rowData['PartnerLogoId']).length > 0) {
                this.Source = this.CurrentSession.Imgs.filter(a => a.LogoId == rowData['PartnerLogoId'])[0].Src;
                var isDestroyed: boolean = this.CD['destroyed'];
                if (!isDestroyed) {
                    this.CD.detectChanges();
                }
            }
            else {
                if (rowData['PartnerLogoId']) {
                    myService.getHypridPartnerLogo(rowData['PartnerLogoId']).subscribe(myResult => {
                        if (myResult) {
                            this.Source = "data:image/JPEG;base64," + myResult;
                            if (this.CurrentSession.Imgs.filter(a => a.LogoId == rowData['PartnerLogoId']).length == 0) {
                                this.CurrentSession.Imgs.push(new Logosdictionary(rowData['PartnerLogoId'], this.Source));
                            }
                            var isDestroyed: boolean = this.CD['destroyed'];
                            if (!isDestroyed) {
                                this.CD.detectChanges();
                            }
                        }
                    });
                }
            }  
        }
        else {
            this.ShowImg = false;
        } 
        var isDestroyed: boolean = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    }

}

export class Logosdictionary {
    constructor(logoId: string, src: string) {
        this.LogoId = logoId;
        this.Src = src;
    }
    public LogoId: string;
    public Src: string;
}
