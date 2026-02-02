import { Routes } from '@angular/router';
import { LoginComponent } from '../features/login-page/login.component';
import { MainPageComponent } from '../features/main-page/main-page.component';
import { AuthGuardService as AuthGuard } from './Services/auth-guard.service';
import { ResetPasswordComponent } from '../features/reset-password-page/reset-password.component';
import { ChangePasswordComponent } from '../features/change-password-page/change-password.component';
import { HostScreenGuardService } from './Services/host-screen-guard.service';

export const routes: Routes = [
    // { path: 'Customs-Book', redirectTo: "customs-book/login", pathMatch: "full" },
    // { path: 'login', component: MainPageComponent, canActivate: [AuthGuard] },
    { path: 'resetpassword', component: ResetPasswordComponent },
    { path: 'changepassword', component: ChangePasswordComponent },
    { path: 'login', component: LoginComponent },
    { path: 'loginCustomsBookHost', component: LoginComponent, canActivate: [HostScreenGuardService] },
    { path: 'customs-book', component: MainPageComponent, canActivate: [AuthGuard] },
    { path: '', component: MainPageComponent, canActivate: [AuthGuard]  },
    { path: '**', component:  MainPageComponent, canActivate: [AuthGuard]  },
];
