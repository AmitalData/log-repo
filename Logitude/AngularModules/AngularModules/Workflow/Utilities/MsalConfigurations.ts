export class MsalConfigurations {
    public static ClientId: string = "95b6ca59-8e30-45ab-b9cb-e4765e82971a";
    public static Authority: string = "https://login.microsoftonline.com/common";
    public static RedirectUri: string = "/";
    public static LoginScopes: string[] = ["openid", "offline_access", "profile", "User.Read", "Mail.Read"];
}