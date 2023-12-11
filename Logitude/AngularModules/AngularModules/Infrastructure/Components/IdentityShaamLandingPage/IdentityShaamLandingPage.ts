import { Component } from "@angular/core";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { TextCodeTranslator } from "Infrastructure/Utilities/TextCodeTranslator";
import { ShaamWebService } from "Shipment/Services/ShaamWebService";

@Component({
    template: `
            <img *ngIf="loadingNotfinish else loadingFinish" class="CenterCenter" src="./_Resources/Images/Gif/Bluespin.gif" />
            <ng-template #loadingFinish>
                <div class='CenterCenter msg' [ngClass]='{"error-msg": error}'>{{msg}}</div>
            </ng-template>
        `,
    styles: [
        '.center { position: fixed; top: 50%; left: 50%;   transform: translate(-50%, -50%); }',
        '.msg { font-size: 30px; width: max-content; height: max-content; color: green }',
        '.error-msg { color: red }',
    ]

})
export class IdentityShaamLandingPageComponent {
    loadingNotfinish: boolean = false;
    error: boolean = false;
    msg: string = '';

    ngOnInit() {
        this.createNewRefreshToken();
    }

    private async createNewRefreshToken() {
        const urlParameter: string = SessionLocator.ExternalParams.Menu;
        const params: string[] = urlParameter.split(';');
        const tenant = +params[1];
        const user: string = params[2];
        const code: string = SessionLocator.ExternalParams['code'];

        try {
            await new ShaamWebService().postNewRefreshToken(tenant, user, code).toPromise();
            this.msg = 'Activate shaam token success, please close this session and connect again';
        } catch (error) {
            this.error = true;
            this.msg = 'Activate shaam token failed, please connact to amital';
        } finally {
            this.loadingNotfinish = false;
        }
    }
}