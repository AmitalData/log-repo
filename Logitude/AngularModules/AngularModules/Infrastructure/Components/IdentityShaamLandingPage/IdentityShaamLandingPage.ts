import { Component } from "@angular/core";
import { AmitalGatewayUtil } from "Infrastructure/Utilities/AmitalGatewayUtil";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { ShaamWebService, newRefreshTokenResponse } from "Shipment/Services/ShaamWebService";

@Component({
    template: `
            <img *ngIf="loadingNotfinish else loadingFinish" class="CenterCenter" src="./_Resources/Images/Gif/Bluespin.gif" />
            <ng-template #loadingFinish>
                <div class='CenterCenter msg' [ngClass]='{"error-msg": error}'>
                    <h1>{{msg}}</h1>
                    <h3>{{errorMsg}}</h3>
                    <h3 *ngIf='shaamTokenRedirect'>You rediract with a few seconds...</h3>
                </div>
            </ng-template>
        `,
    styles: [
        '.center { position: fixed; top: 50%; left: 50%;   transform: translate(-50%, -50%); }',
        '.msg { font-size: 30px; width: max-content; height: max-content; color: green }',
        '.error-msg { color: red }',
        'h1 { font-size: 50px; }',
        'h3 { font-size: 35px; }',
        'h1, h3 { width: 90vw; white-space: normal; text-align: center; direction: rtl; }',
    ]

})
export class IdentityShaamLandingPageComponent {
    loadingNotfinish: boolean = true;
    error: boolean = false;
    readonly userKey: string = 'shaamRediract_user';
    readonly tenantKey: string = 'shaamRediract_tenant';
    msg: string = '';
    errorMsg: string = '';
    shaamTokenRedirect: string = '';

    async ngOnInit() {
        this.rediractOrCreateNewToken();
    }

    rediractOrCreateNewToken() {
        const redirectToShaam: string | null = this.getRediractToShaamQS();
        if (redirectToShaam) {
            this.saveUserDataInStorage();
            location.href = redirectToShaam;
        } else
            this.createNewRefreshToken();
    }

    getRediractToShaamQS(): string | null {
        const redirectToShaamKey: string = 'redirectToShaam';
        const currentUrl: string = window.location.href;
        const indexKey = currentUrl.indexOf(redirectToShaamKey);
        return indexKey > -1 ? currentUrl.substring(indexKey + 'redirectToShaam'.length + 1) : null;
    }

    saveUserDataInStorage() {
        const urlParams: URLSearchParams = new URLSearchParams(window.location.search);
        localStorage[this.userKey] = urlParams.get('user');
        localStorage[this.tenantKey] = urlParams.get('tenant');
    }

    GetUserDataFromStorage(): { user: string; tenant: number; } {
        return {
            user: localStorage[this.userKey],
            tenant: localStorage[this.tenantKey],
        }
    }

    private async createNewRefreshToken() {
        const { user, tenant }: { user: string; tenant: number; } = this.GetUserDataFromStorage();
        const code: string = SessionLocator.ExternalParams['code'];

        try {
            if (!user || !code || tenant == null || tenant == undefined)
                throw `parameter not found - user: ${user}, code: ${code}, tenant: ${tenant}`;

            const res: newRefreshTokenResponse = await new ShaamWebService().postNewRefreshToken(tenant, user, code);
            if (!res.approved)
                throw res.message;

            this.msg = 'טוקן שע"מ התקבל בהצלחה! ניתן לסגור את חלון זה ולהמשיך עבודה. על מנת לראות את פרטי הטוקן שהתקבל, יש לרענן את מסך התצוגה.';
        } catch (error) {
            console.log(error)
            this.error = true;
            this.msg = 'שגיאה בקבלת טוקן משע"מ! אנא בדקו את השגיאה שהתקבלה וטפלו בהתאם:';
            this.errorMsg = error.toString();

        } finally {
            this.loadingNotfinish = false;
        }

        // this.redirect();

        AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
            "", "",
            'createNewRefreshTokenFinish',
            AmitalGatewayUtil.Instance.GetDefaultUnifreightMessageM(),
            JSON.stringify({ success: !this.error }),
            false);
    }

    private redirect() {
        const shaamTokenRedirect: string = localStorage.getItem('shaamTokenRedirect');

        if (shaamTokenRedirect)
            setTimeout(() => location.href = shaamTokenRedirect + "&logitudeCommandId=CreateNewShaamToken", 5000);
    }
}
