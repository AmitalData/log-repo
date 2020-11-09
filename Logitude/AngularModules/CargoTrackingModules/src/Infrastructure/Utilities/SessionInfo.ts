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

    //private static loggedUserPM: UserPM;
    //public static get LoggedUserPM(): UserPM { return this.loggedUserPM; }
    //public static set LoggedUserPM(newValue: UserPM)
    //{
    //    if (this.loggedUserPM != newValue) this.loggedUserPM = newValue;
    //}
}
