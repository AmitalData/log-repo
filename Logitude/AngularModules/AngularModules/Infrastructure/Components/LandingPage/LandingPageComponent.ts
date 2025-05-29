import { Component } from "@angular/core";

@Component({
    selector: 'app-landing-page',
    template: `
        <div class='CenterCenter' [ngClass]='{"error-msg": error}'>
            <p>{{message}}</p>
        </div>
    `,
    styleUrls: ['./LandingPageComponent.scss']
})
export class LandingPageComponent {
    public message: string = "";
    public error: boolean = false;
}