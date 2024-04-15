import { UserPM } from '../../Infrastructure/EntityPMs/UserPM';
import { SessionLocator } from './SessionLocator';

export class SessionInfo {
	private static loggedUserId: string;
	public static get LoggedUserId(): string {
		return this.loggedUserId;
	}
	public static set LoggedUserId(newValue: string) {
		this.loggedUserId = newValue;
	}

	private static loggedUserTenant: number;
	public static get LoggedUserTenant(): number {
		return this.loggedUserTenant;
	}
	public static set LoggedUserTenant(newValue: number) {
		this.loggedUserTenant = newValue;
	}

	private static token: string;
	public static get Token(): string {
		return this.token;
	}
	public static set Token(newValue: string) {
		this.token = newValue;
	}

	private static documentDownloadToken: string;
	public static get DocumentDownloadToken(): string {
		return this.documentDownloadToken;
	}
	public static set DocumentDownloadToken(newValue: string) {
		this.documentDownloadToken = newValue;
	}

	private static loggedSessionToken: string;
	public static get LoggedSessionToken(): string {
		return this.loggedSessionToken;
	}
	public static set LoggedSessionToken(newValue: string) {
		this.loggedSessionToken = newValue;
	}

	private static loggedUserEmail: string;
	public static get LoggedUserEmail(): string {
		return this.loggedUserEmail;
	}
	public static set LoggedUserEmail(newValue: string) {
		this.loggedUserEmail = newValue;
	}

	private static sessionTimeout: number;
	public static get SessionTimeout(): number {
		return this.sessionTimeout;
	}
	public static set SessionTimeout(newValue: number) {
		this.sessionTimeout = newValue;
	}

	private static webTokenLifeTimeInMinutes: number;
	public static get WebTokenLifeTimeInMinutes(): number {
		return this.webTokenLifeTimeInMinutes;
	}
	public static set WebTokenLifeTimeInMinutes(newValue: number) {
		this.webTokenLifeTimeInMinutes = newValue;
	}

	private static webTokenExpirationWarningInMinutes: number;
	public static get WebTokenExpirationWarningInMinutes(): number {
		return this.webTokenExpirationWarningInMinutes;
	}
	public static set WebTokenExpirationWarningInMinutes(newValue: number) {
		this.webTokenExpirationWarningInMinutes = newValue;
	}

	private static keepUserLoggedIn: boolean;
	public static get KeepUserLoggedIn(): boolean {
		return this.keepUserLoggedIn;
	}
	public static set KeepUserLoggedIn(newValue: boolean) {
		this.keepUserLoggedIn = newValue;
	}

	private static lastLoginDateTime: Date;
	public static get LastLoginDateTime(): Date {
		return this.lastLoginDateTime;
	}
	public static set LastLoginDateTime(newValue: Date) {
		this.lastLoginDateTime = newValue;
	}

	private static loggedUserPM: UserPM;
	public static get LoggedUserPM(): UserPM {
		return this.loggedUserPM;
	}
	public static set LoggedUserPM(newValue: UserPM) {
		if (this.loggedUserPM != newValue) {
			this.loggedUserPM = newValue;
			SessionLocator.LoggedUserPM = newValue;

			if (newValue) {
				SessionLocator.LoggedUserId = newValue.Id;
			}
		}
	}

	private static loggedUserCardId: string;
	public static get LoggedUserCardId(): string {
		return this.loggedUserCardId;
	}
	public static set LoggedUserCardId(newValue: string) {
		this.loggedUserCardId = newValue;
	}

	private static loggedUserCardType: string;
	public static get LoggedUserCardType(): string {
		return this.loggedUserCardType;
	}
	public static set LoggedUserCardType(newValue: string) {
		this.loggedUserCardType = newValue;
	}
}
