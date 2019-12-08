import {Component, ViewChild, ViewContainerRef, EventEmitter, ChangeDetectorRef} from '@angular/core';
import {AppTool} from '../../../Infrastructure/Tools';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';

@Component({
    moduleId: module.id,
    templateUrl: './FieldTemplateComponent.html',
})

export class FieldTemplateComponent {
    public Entity: any = null;
    public FieldName: string = null;
    public FieldValue: any = null;
    public ObjectTableName: string = null;
    public ShipmentLevelCode: string = null;
    public IsHouseIconVisible: boolean = true;
    public IsHouseNotConnected: boolean = false;
    public SpotlightDataTemplate: string = null;
    public IsSpotLightTemplate: boolean = false;
    public IsHeaderScreenTemplate: boolean = false;
    public localCurrency: string = "(" + SessionLocator.LocalCurrencyCode + ")";
    public ProfitCurrency: string = "(" + SessionLocator.TenantPM.ProfitCurrencyCode + ")";
    @ViewChild('SpotLight', { read: ViewContainerRef }) SpotLightViewContainerRef: ViewContainerRef;
    constructor() {

    }
    
    public Run(args: any) {
        this.Entity = args['Entity'];
        this.FieldName = args['FieldName'];
        this.ObjectTableName = args['ObjectTableName'];
        this.IsSpotLightTemplate = args['IsSpotLightTemplate'];
        this.SpotlightDataTemplate = args['SpotlightDataTemplate'];
        this.IsHeaderScreenTemplate = args['IsHeaderScreenTemplate'];

        if (this.Entity != null) {
            if (this.FieldName != null) {
                this.FieldValue = this.Entity[this.FieldName];
                this.ShipmentLevelCode = this.Entity.ShipmentLevelCode;

                if (this.Entity.ShipmentLevelCode == 'H' && AppTool.IsNullOrEmpty(this.Entity.MasterShipmentDataId)) {
                    this.IsHouseNotConnected = true;
                }

                if (this.IsHeaderScreenTemplate) {
                    if (!this.IsHouseNotConnected) {
                        this.IsHouseIconVisible = false;
                    }
                }

                switch (this.FieldName) {
                    case "FWBStatusName": {
                        this.SetFWBStatusSource();
                        break;
                    }

                    case "FHLStatusName": {
                        this.SetFHLStatusSource();
                        break;
                    }

                    case "CargonautFWBStatusName": {
                        this.SetCargonautFWBStatusSource();
                        break;
                    }

                    case "CargonautFHLStatusName": {
                        this.SetCargonautFHLStatusSource();
                        break;
                    }
                }
            }

            if (this.IsSpotLightTemplate) {
                this.RunComponent();
            }
        }
    }

    private Retries: number = 0;
    private timerToken: any;
    RunComponent() {
        if (this.SpotLightViewContainerRef) {
            this.SpotLightViewContainerRef.clear();

            var myComponentPath = "./Shipment/Components/Spotlight/ShipmentSpotlightComponent";
            SessionLocator.DynamicLoader.Load(myComponentPath, this.SpotLightViewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.Run(this.Entity.Id);
                });
        }

        else {
            this.RunComponentTimer();
        }
    }
    RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }

    public FWBStatusSource: string = null;
    public FWBStatusTooltip: string = null;
    private SetFWBStatusSource() {
        this.FWBStatusSource = "./Images/Icons/AWB/" + this.Entity['FWBStatusCode'] + ".png";
        this.FWBStatusTooltip = this.ConvertTooltip(this.Entity['FWBStatusName']);
    }

    public FHLStatusSource: string = null;
    public FHLStatusTooltip: string = null;
    private SetFHLStatusSource() {
        this.FHLStatusSource = "./Images/Icons/AWB/" + this.Entity['FHLStatusCode'] + ".png";
        this.FHLStatusTooltip = this.ConvertTooltip(this.Entity['FHLStatusName']);
    }

    public CargonautFWBStatusSource: string = null;
    public CargonautFWBStatusTooltip: string = null;
    private SetCargonautFWBStatusSource() {
        this.CargonautFWBStatusSource = "./Images/Icons/AWB/" + this.Entity['CargonautFWBStatusCode'] + ".png";
        this.CargonautFWBStatusTooltip = this.ConvertTooltip(this.Entity['CargonautFWBStatusName']);
    }

    public CargonautFHLStatusSource: string = null;
    public CargonautFHLStatusTooltip: string = null;
    private SetCargonautFHLStatusSource() {
        this.CargonautFHLStatusSource = "./Images/Icons/AWB/" + this.Entity['CargonautFHLStatusCode'] + ".png";
        this.CargonautFHLStatusTooltip = this.ConvertTooltip(this.Entity['CargonautFHLStatusName']);
    }

    private ConvertTooltip(tooltip: string) {
        var myResult = tooltip;
        if (myResult) {
            myResult.replace("Error", "FNA error by airline");
            myResult.replace("Accepted", "Accepted by airline");
        }
        return myResult;
    }
}
