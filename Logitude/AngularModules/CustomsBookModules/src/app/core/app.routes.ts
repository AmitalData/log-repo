import { Routes } from '@angular/router';
import { AuthGuardService as AuthGuard } from './Services/auth-guard.service';

export const routes: Routes = [
    {
        path: 'login',
        loadComponent: () => import('../features/login-page/login.component').then(c => c.LoginComponent)
    },
    {
        path: 'resetpassword',
        loadComponent: () => import('../features/reset-password-page/reset-password.component').then(c => c.ResetPasswordComponent)
    },
    {
        path: 'changepassword',
        loadComponent: () => import('../features/change-password-page/change-password.component').then(c => c.ChangePasswordComponent)
    },
    {
        path: 'customs-book',
        loadComponent: () => import('../features/main-page/main-page.component').then(c => c.MainPageComponent),
        canActivate: [AuthGuard]
    },
    {
        path: '',
        redirectTo: 'customs-book',
        pathMatch: 'full'
    },
    {
        path: '**',
        redirectTo: 'customs-book'
    },
];
