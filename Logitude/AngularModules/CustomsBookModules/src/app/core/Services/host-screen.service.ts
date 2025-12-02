import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root',
})
export class HostScreenService {
    public redirectUrl: string;
    public DefaultPageCustomsBook: string = "";
    constructor() { }

    public isAuthenticated(): boolean {
        const token = sessionStorage.getItem('Token');
        if (!token) return false;
        return true;
    }

    public closeSession() {
        sessionStorage.removeItem('Token');
        sessionStorage.removeItem('AmitalSSOAngular');
        sessionStorage.removeItem('Tenant');
        sessionStorage.removeItem('searchValue');
    }
}
