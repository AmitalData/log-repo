import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AppHeaderComponent } from '../shared/components/app-header/app-header.component';
import { AppFooterComponent } from '../shared/components/app-footer/app-footer.component';
import { MainPageComponent } from '../features/main-page/main-page.component';
import { NgFor, NgForOf } from '@angular/common';
import { CommonModule } from '@angular/common';
import { SessionInfo } from './Infrastructure/Utilities/SessionInfo';
import { CookieconsentComponent } from "../shared/components/cookieconsent/cookieconsent.component";

@Component({
	selector: 'app-root',
	standalone: true,
	imports: [NgFor, NgForOf, RouterOutlet, AppHeaderComponent, AppFooterComponent, MainPageComponent, CommonModule, CookieconsentComponent],
	templateUrl: './app.component.html',
	styleUrl: './app.component.css',
})
export class AppComponent {

	constructor()
    {
        //RootContext.AppComponent = this;
        this.SetSeSessionInfo();
    }

    private SetSeSessionInfo() {
        SessionInfo.LoggedUserEmail = sessionStorage.getItem("LoggedUserEmail");
        SessionInfo.LoggedUserId = sessionStorage.getItem("LoggedUserId");
        SessionInfo.LoggedUserTenant = Number(sessionStorage.getItem("LoggedUserTenant"));
        SessionInfo.Token = sessionStorage.getItem("Token");
        SessionInfo.DocumentDownloadToken = sessionStorage.getItem("DocumentDownloadToken");
    }

}
