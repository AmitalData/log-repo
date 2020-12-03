import { Injectable } from "@angular/core";
import { CourierPendingReasonList } from "../../EntityLists/CourierPendingReasonList";
import { DateTool } from "../../../Infrastructure/Tools";
import { CourierPendingReasonListService } from "../StandardLists/CourierPendingReasonListService";
import { ServiceResponse } from "../../../Infrastructure/DataContracts/ServiceResponse";

@Injectable()
export class CacheCourierPendingReasonService {
    private static _instance: CacheCourierPendingReasonService;
    private  CacheCourierPendingAt?: Date = null;
    private  CacheCourierPendingReasonList: Array<CourierPendingReasonList> = [];

    public GetCache(): Array<CourierPendingReasonList>{
        this.CreateCacheCourierPendingReasonList();
        return this.CacheCourierPendingReasonList; 
    }
    public static get Instance() {
        return this._instance || (this._instance = new this());
    }
    private constructor() {

    }

    public CreateCacheCourierPendingReasonList() {

        if (this.CacheCourierPendingAt == null ||
            (this.CacheCourierPendingAt != null &&
            DateTool.AddMinute(this.CacheCourierPendingAt, 15) < DateTool.GetCurrentDateTimeAsUtc())
        ) {
            this.CacheCourierPendingAt = DateTool.GetCurrentDateTimeAsUtc();
            var myCourierPendingReasonListService = new CourierPendingReasonListService();
            myCourierPendingReasonListService.getAll()
                .subscribe((rs: ServiceResponse) => {
                    this.CacheCourierPendingReasonList = rs.Result;
                });
        }

    }
}
