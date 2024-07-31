import { Routes } from '@angular/router';
import { LoginComponent } from '../features/login-page/login.component';
import { MainPageComponent } from '../features/main-page/main-page.component';
import { AuthGuardService as AuthGuard } from './Services/auth-guard.service';
import { ResetPasswordComponent } from '../features/reset-password-page/reset-password.component';
import { ChangePasswordComponent } from '../features/change-password-page/change-password.component';

export const routes: Routes = [
    { path: 'Customs-Book', redirectTo: "customs-book/login", pathMatch: "full" },
    { path: 'Customs-Book/login', redirectTo: "customs-book/login", pathMatch: "full" },
    { path: 'customs-book/resetpassword', component: ResetPasswordComponent },
    { path: 'customs-book/changepassword', component: ChangePasswordComponent },
    { path: 'customs-book/login', component: LoginComponent },
    {
        path: 'customs-book',
        component: MainPageComponent,
        canActivate: [AuthGuard],
        children: [
            { path: "", component: MainPageComponent },
            { path: '**', redirectTo: 'customs-book', pathMatch: 'full' },
        ]
    },
    // { path: 'Error401', component: Error401Component },
    { path: '', redirectTo: 'customs-book', pathMatch: 'full' },
    { path: '**', redirectTo: 'customs-book', pathMatch: 'full' },
];
