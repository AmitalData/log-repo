import { CargoTrackingShipmentSearchInput } from 'src/CargoTracking/DataContracts/CargoTrackingShipmentFilters';

export class SessionInfo{

    private static loggedUserId: string;
    public static get LoggedUserId(): string { return this.loggedUserId; }
    public static set LoggedUserId(newValue: string) { this.loggedUserId = newValue; }

    private static loggedUserTenant: number;
    public static get LoggedUserTenant(): number { return this.loggedUserTenant; }
    public static set LoggedUserTenant(newValue: number) { this.loggedUserTenant = newValue; }

    private static token: string;
    public static get Token(): string { return this.token; }
    public static set Token(newValue: string) { this.token = newValue; }

    private static documentDownloadToken: string;
    public static get DocumentDownloadToken(): string { return this.documentDownloadToken; }
    public static set DocumentDownloadToken(newValue: string) { this.documentDownloadToken = newValue; }

    private static loggedUserEmail: string;
    public static get LoggedUserEmail(): string { return this.loggedUserEmail; }
    public static set LoggedUserEmail(newValue: string) { this.loggedUserEmail = newValue; }

    public static LoggedUser: any;
    public static LoggedUserCompanyLogins: any[] = [];

    public static ShipmentsFilters: CargoTrackingShipmentSearchInput;

    public static LoggedUserPM: any;
    public static LoggedContact: any;
    
    private static isAdmin: string;
    public static get IsAdmin(): string { return this.isAdmin; }
    public static set IsAdmin(newValue: string) { this.isAdmin = newValue; }

}
