import { Injectable } from "@angular/core";
import { CourierPendingReasonList } from "../../EntityLists/CourierPendingReasonList";
import { DateTool } from "../../../Infrastructure/Tools";
import { CourierPendingReasonListService } from "../StandardLists/CourierPendingReasonListService";
import { ServiceResponse } from "../../../Infrastructure/DataContracts/ServiceResponse";
import { Observable, of } from "rxjs";

@Injectable()
export class CacheCourierPendingReasonService {
    private static _instance: CacheCourierPendingReasonService;
    private CacheCourierPendingAt?: Date = null;
    private CacheCourierPendingReasonList: Array<CourierPendingReasonList> = [];

    public GetCache(): Array<CourierPendingReasonList> {
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
    public EnsureCacheLoaded(): Observable<boolean> {
        // if cache exists and still fresh -> immediate
        if (this.CacheCourierPendingReasonList && this.CacheCourierPendingReasonList.length > 0
            && this.CacheCourierPendingAt != null
            && DateTool.AddMinute(this.CacheCourierPendingAt, 15) >= DateTool.GetCurrentDateTimeAsUtc()) {
            return of(true);
        }

        return new Observable<boolean>(subscriber => {
            this.CacheCourierPendingAt = DateTool.GetCurrentDateTimeAsUtc();
            var svc = new CourierPendingReasonListService();
            svc.getAll().subscribe((rs: ServiceResponse) => {
                this.CacheCourierPendingReasonList = rs && rs.Result ? rs.Result : [];
                subscriber.next(true);
                subscriber.complete();
            }, err => subscriber.error(err));
        });
    }

    private ParseCodes(listString: string): string[] {
        if (!listString) return [];
        return ('' + listString)
            .split(/[,\s;|]+/g)
            .map(x => x.trim())
            .filter(x => !!x);
    }

    public HasPaymentHold(listString: string): boolean {
        if (!this.CacheCourierPendingReasonList || this.CacheCourierPendingReasonList.length === 0) return false;

        const codes = this.ParseCodes(listString);
        for (let i = 0; i < codes.length; i++) {
            const code = codes[i];
            const rec = this.CacheCourierPendingReasonList.filter(r => r && r.Code === code)[0];
            if (rec && rec.ErrorPlace === "1") return true;
        }
        return false;
    }

    public GetNamesToolTip(listString: string): string {
        if (!this.CacheCourierPendingReasonList || this.CacheCourierPendingReasonList.length === 0) return "";

        const codes = this.ParseCodes(listString);
        let text = "";

        for (let i = 0; i < codes.length; i++) {
            const code = codes[i];
            const rec = this.CacheCourierPendingReasonList.filter(r => r && r.Code === code)[0];
            if (rec) {
                if (text) text += "\n";
                if (rec.LocalName) text += rec.LocalName;
                else if (rec.EnglishName) text += rec.EnglishName;
                else text += rec.Code;
            }
        }
        return text;
    }

}
