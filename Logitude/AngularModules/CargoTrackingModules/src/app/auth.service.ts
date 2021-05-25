import { Injectable } from '@angular/core';

@Injectable()
export class AuthService {
    public redirectUrl: string;
    public DefaultPageCargoTracking: string = "cargo-tracking";
    constructor() {}

    public isAuthenticated(): boolean {
        const token = sessionStorage.getItem('Token');

        if(!token) return false;
        return true;
    }
}
