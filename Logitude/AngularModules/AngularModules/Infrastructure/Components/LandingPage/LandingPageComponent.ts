import { Component } from "@angular/core";

@Component({
    selector: 'app-landing-page',
    template: `
        <div class='CenterCenter' [ngClass]='{"error-msg": error}'>
            <p class='msg'>{{message}}</p>
        </div>
    `,
    styles: [`
        .CenterCenter { 
            height: max-content;
            color: green; 
            width: 90vw;
            text-align: center;
            direction: rtl;            
        },

        .error-msg { 
            color: red 
        },
    `,
        `p { 
            font-size: 3rem;
            white-space: normal;
        }`   
    ]
})
export class LandingPageComponent {
    public message: string = "";
    public error: boolean = false;
}