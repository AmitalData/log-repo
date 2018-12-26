import {Component, ChangeDetectorRef} from '@angular/core';
import {AppTool, DateTool, FontTool} from '../../../Infrastructure/Tools';

@Component({
    moduleId: module.id,
    templateUrl: './FieldTemplateComponent.html',
})

export class FieldTemplateComponent {
    public Entity: any = null;
    public FieldName: string = null;
    public FieldValue: any = null;
    public ObjectTableName: string = null;
    public SpotlightDataTemplate: string = null;
    public IsSpotLightTemplate: boolean = false;
    public isPotentialCustomer: boolean = false;
    public IsHeaderScreenTemplate: boolean = false;
    public  CustomsShipperValidityEndDateBackgroudColor: string;


    constructor(private cd: ChangeDetectorRef) {

    }

    public Run(args: any) {
        this.Entity = args['Entity'];
        this.FieldName = args['FieldName'];
        this.ObjectTableName = args['ObjectTableName'];
        this.IsSpotLightTemplate = args['IsSpotLightTemplate'];
        this.SpotlightDataTemplate = args['SpotlightDataTemplate'];
        this.IsHeaderScreenTemplate = args['IsHeaderScreenTemplate'];

        if (this.Entity != null && this.FieldName != null) {
            this.FieldValue = this.Entity[this.FieldName];

            if (this.ObjectTableName == "Customer") {
                if (this.FieldName == "RankCode") {
                    this.SetRanksSource();
                }

                else if (this.FieldName == "LastShipmentDate") {
                    this.SetLastShipmentDateTemplate();
                }

                else if (this.FieldName == "StartWorkingDate") {
                    this.SetStartWorkingDateTemplate();
                }
                else if (this.FieldName == "SharedLogisticsInvitationStatusName") {
                    this.SetSharedLogisticsInvitationStatusTemplate();
                }

                if (!AppTool.IsNullOrEmpty(this.Entity.CustomerStatusTemplateCode)) {
                    this.isPotentialCustomer = true;
                }
            }

            if (this.ObjectTableName == "AgentSharedManifest") {
                if (this.FieldName == "StatusName") {
                    this.SetAgentSharedManifestStatusColorTemplate();
                }
            }


            if (this.ObjectTableName == "CustomsShipper") {
                if (this.FieldName == "ValidityEndDate") {

                    this.CustomsShipperValidityEndDateBackgroudColor = this.transform(this.FieldValue);
                }
            }
                

        }
    }

    transform(fieldValue: any): string {

        var myFieldDate: Date = new Date(fieldValue);

        var myResult = FontTool.Green;
        if (myFieldDate) {
            var todayDate = DateTool.GetCurrentDateTimeAsUtc();
            if (myFieldDate.valueOf() <= todayDate.valueOf()) {
                myResult = FontTool.Red;
            }
            else if (DateTool.AddDays(todayDate, 30).valueOf() > myFieldDate.valueOf()) {
                myResult = FontTool.Orange;
            }
        }

        return myResult;
    }

    // Rank
    public RankSource1: string = null;
    public RankSource2: string = null;
    public RankSource3: string = null;
    private SetRanksSource() {
        var RankCode = this.Entity['RankCode'];

        switch (RankCode) {
            case "1": {
                this.RankSource1 = "./Images/Icons/StarOrange.png";
                this.RankSource2 = "./Images/Icons/StarGray.png";
                this.RankSource3 = "./Images/Icons/StarGray.png";
                break;
            }

            case "2": {
                this.RankSource1 = "./Images/Icons/StarOrange.png";
                this.RankSource2 = "./Images/Icons/StarOrange.png";
                this.RankSource3 = "./Images/Icons/StarGray.png";
                break;
            }

            case "3": {
                this.RankSource1 = "./Images/Icons/StarOrange.png";
                this.RankSource2 = "./Images/Icons/StarOrange.png";
                this.RankSource3 = "./Images/Icons/StarOrange.png";
                break;
            }

            default: {
                this.RankSource1 = "./Images/Icons/StarGray.png";
                this.RankSource2 = "./Images/Icons/StarGray.png";
                this.RankSource3 = "./Images/Icons/StarGray.png";
            }
        }
    }


    SharedLogisticsInvitationStatusColor: string = null;
    
    SetSharedLogisticsInvitationStatusTemplate() {
        if (!AppTool.IsNullOrEmpty(this.FieldValue)) {
            if (this.FieldValue == "Activated") {
                this.SharedLogisticsInvitationStatusColor = "Green";
            }

            else if (this.FieldValue == "Invited") {
                this.SharedLogisticsInvitationStatusColor = "Orange";
    }

            else if (this.FieldValue == "Not Invited") {
                this.SharedLogisticsInvitationStatusColor = "Red";
            }
        
        }

    }
    
    public LastShipmentDateColor: string = null;
    public LastShipmentDateValue: string = null;
    private SetLastShipmentDateTemplate() {
        if (!AppTool.IsNullOrEmpty(this.FieldValue)) {

            var myFieldDate: Date = new Date(this.FieldValue);

            if (myFieldDate.valueOf() >= DateTool.GetDateByDay(-7).valueOf()) {
                this.LastShipmentDateColor = "Green";
            }

            else if (myFieldDate.valueOf() >= DateTool.GetDateByDay(-30).valueOf()) {
                this.LastShipmentDateColor = "Orange";
            }

            else {
                this.LastShipmentDateColor = "Red";
            }

            var days = DateTool.GetDaysBetweenDates(myFieldDate, DateTool.GetCurrentDateAsUtc(), true);

            if (days == 0) {
                this.LastShipmentDateValue = "Today";
            }

            if (days == 1) {
                this.LastShipmentDateValue = "Yesterday";
            }

            if (days > 1 && days < 31) {
                this.LastShipmentDateValue = days + " Days";
            }

            if (days >= 31 && days < 1095) {
                var months: number = +(days / 31).toString().split(".")[0];
                if (months == 1) {
                    this.LastShipmentDateValue = months + " Month";
                }

                else {
                    this.LastShipmentDateValue = months + " Months";
                }
            }

            if (days > 1095) {
                var years: number = +(days / 365).toString().split(".")[0];

                if (years == 1) {
                    this.LastShipmentDateValue = years + " Year";
                }

                else {
                    this.LastShipmentDateValue = years + " Years";
                }
            }

        }
    }



    public AgentSharedManifestStatusColor: string = null;
    private SetAgentSharedManifestStatusColorTemplate() {
        if (!AppTool.IsNullOrEmpty(this.FieldValue)) {

            if (this.FieldValue == "Cancelled") {
                this.AgentSharedManifestStatusColor = "Black";
            }

            else
                if (this.FieldValue == "Waiting") {
                    this.AgentSharedManifestStatusColor = "Orange";
                }


                else {
                    this.AgentSharedManifestStatusColor = "Green";
                }

        }
    }


    public StartWorkingDateValue: string = null;
    private SetStartWorkingDateTemplate() {
        if (!AppTool.IsNullOrEmpty(this.FieldValue)) {
            var myFieldDate: Date = new Date(this.FieldValue);
            var LastDate: Date = DateTool.GetDateParts(myFieldDate).DateObject;
            LastDate.setHours(0);
            var TodayDate: Date = DateTool.GetCurrentDateAsUtc();
            TodayDate.setHours(0);
            if (LastDate != null ? LastDate.valueOf() > TodayDate.valueOf() : true)
                return null;
            var days = DateTool.GetDaysBetweenDates(LastDate, TodayDate );

            if (days == 0) {
                this.StartWorkingDateValue = "Today";
            }

            if (days == 1) {
                this.StartWorkingDateValue = "Yesterday";
            }

            if (days > 1 && days < 31) {
                this.StartWorkingDateValue = days + " Days";
            }

            if (days > 31 && days < 1095) {
                var months: number = +(days / 31).toString().split(".")[0];
                if (months == 1) {
                    this.StartWorkingDateValue = months + " Month";
                }

                else {
                    this.StartWorkingDateValue = months + " Months";
                }
            }

            if (days > 1095) {
                var years: number = +(days / 365).toString().split(".")[0];
                if (years == 1) {
                    this.StartWorkingDateValue = years + " Year";
                }
                else {
                    this.StartWorkingDateValue = years + " Years";
                }
            }
        }
    }
}
