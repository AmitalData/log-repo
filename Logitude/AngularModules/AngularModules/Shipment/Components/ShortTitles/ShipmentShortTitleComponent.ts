import {Component} from '@angular/core';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {ShipmentPM} from '../../EntityPMs/ShipmentPM';
import { AppTool } from '../../../Infrastructure/Tools';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { ShipmentUnassignedFieldPM } from '../../EntityPMs/ShipmentUnassignedFieldPM';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';

@Component({
    
    templateUrl: "ShipmentShortTitleComponent.html",
})

export class ShipmentShortTitleComponent {
    public CustomerRankName: string = null;
    public EntityPM: ShipmentPM;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = this.entityArgs.EntityPM;

        if (this.EntityPM != null) {
            this.BuildComponent();
        }

        this.Listen();
    }
    
    private LoadCompletedEvent: any = null;
    private SessionEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent) {
            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.BuildComponent();
                }
            });

            this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(s => {
                if (s == "ShipmentUnassignedDataChanged") {
                    this.ComputeUnassigedValidationVisibility();
                }
            });
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
        AppTool.KillEventEmitter(this.SessionEvent);
    }

    public Background: string;
    public DirectionImageSRC: string;
    public TransportModeImageSRC: string;
    public PartnerName: string;
    public RankCode: string;
    public RankName: string;
    public RankSource1: string;
    public RankSource2: string;
    public RankSource3: string;
    public IsRankVisible: boolean = false;   
    public IsUnassigedValidationVisible: boolean = false;
    public UnassigedValidationMessage: string;
    private BuildComponent() {
        this.ComputeUnassigedValidationVisibility();

        if (this.EntityPM.ShipmentLevelCode == "C") {
            this.Background = "rgba(35, 172, 214, 0.15)";
            this.PartnerName = this.EntityPM.AgentName;
        }

        else {
            this.Background = "rgba(235, 235, 235, 1)";
            this.PartnerName = this.EntityPM.CustomerName;
            this.RankName = this.EntityPM.CustomerRankName;

            if (this.RankName != null) {

                switch (this.RankName.toLowerCase()) {
                    case "silver": {
                        this.RankCode = "1";
                        this.RankSource1 = "./Images/Icons/StarOrange.png";
                        this.RankSource2 = "./Images/Icons/StarGray.png";
                        this.RankSource3 = "./Images/Icons/StarGray.png";
                        break;
                    }

                    case "gold": {
                        this.RankCode = "2";
                        this.RankSource1 = "./Images/Icons/StarOrange.png";
                        this.RankSource2 = "./Images/Icons/StarOrange.png";
                        this.RankSource3 = "./Images/Icons/StarGray.png";
                        break;
                    }

                    case "platinum": {
                        this.RankCode = "3";
                        this.RankSource1 = "./Images/Icons/StarOrange.png";
                        this.RankSource2 = "./Images/Icons/StarOrange.png";
                        this.RankSource3 = "./Images/Icons/StarOrange.png";
                        break;
                    }

                    default: {
                        this.RankCode = "0";
                        this.RankSource1 = "./Images/Icons/StarGray.png";
                        this.RankSource2 = "./Images/Icons/StarGray.png";
                        this.RankSource3 = "./Images/Icons/StarGray.png";
                    }
                }

                this.IsRankVisible = true;
            }            
        }

        this.DirectionImageSRC = "./Images/Directions/" + this.EntityPM.DirectionId + ".png";
        this.TransportModeImageSRC = "./Images/Icons/" + this.EntityPM.TransportModeId + ".png";
    }

    get IsCancelled() { return this.EntityPM.IsCancelled; }

    ComputeUnassigedValidationVisibility() {
        this.IsUnassigedValidationVisible = false;
        this.UnassigedValidationMessage = null;

        if (this.EntityPM.HasUnassignedData && FeatureLocator.HasFeaturePermession("Shipment", "UpdateUnassignedData")) {
            var myList: ShipmentUnassignedFieldPM[] = this.EntityPM.ShipmentUnassignedFields.filter(s => AppTool.IsNullOrEmpty(s.ReplacedDataId));

            if (myList.length > 0) {
                this.IsUnassigedValidationVisible = true;
                this.UnassigedValidationMessage = "Some fields in this shipment contain unassigned data, would you like to update them?";
            }            
        }
    }

    UpdateUnassigedDataClicked() {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.WindowArgs = this.EntityPM;
        logitudeWindow.Title = "Unassiged Data Management - " + this.EntityPM.ShipmentNumber;
        logitudeWindow.Show("./Shipment/Components/UnassigedData/UpdateUnassigedDataComponent");
        logitudeWindow.WindowClosed.subscribe(s => {
            if (s == "ok") {
                this.ComputeUnassigedValidationVisibility();
            }
        });
    }
}
