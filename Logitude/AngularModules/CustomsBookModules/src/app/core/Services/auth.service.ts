import { Injectable } from '@angular/core';

@Injectable({
	providedIn: 'root',
})
export class AuthService {
    public redirectUrl: string;
    // public DefaultPageCustomsBook: string = "customs-book";
    public DefaultPageCustomsBook: string = "";
    constructor() {}

    public isAuthenticated(): boolean {
        const token = sessionStorage.getItem('Token');

        if(!token) return false;
        return true;
    }

    public closeSession() {
        sessionStorage.removeItem('Token');
    }
}
