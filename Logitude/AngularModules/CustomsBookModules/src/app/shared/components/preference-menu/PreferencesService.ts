
import { Injectable } from '@angular/core';
import { API_MainService } from '../../../core/API_MainService';
import { BehaviorSubject } from 'rxjs';
import { SessionInfo } from '../../../core/Infrastructure/Utilities/SessionInfo';

@Injectable({
    providedIn: 'root'
})

export class PreferencesService {
    constructor(private apiService: API_MainService) {
    }
    allPreferences: BehaviorSubject<CB_Preference[]> = new BehaviorSubject<CB_Preference[]>([]);
    public _showSetingsPopup: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

    getPreference(level: number, type: PreferenceType): string {
        if (this.allPreferences.getValue()?.length > 0) {
            if (type == PreferenceType.Background) return this.allPreferences.getValue().filter(item => item.Level == level)[0]?.BackgroundColor;
            else return this.allPreferences.getValue().filter(item => item.Level == level)[0]?.TextColor;
        }
    }

    AddAllCB_Preferences(preferences: CB_Preference[]) {
        this.apiService.AddNewAllCB_Preferences(preferences).subscribe((preferences) => {
            console.log(preferences);
            this._showSetingsPopup.next(false);
        });
    }

    updateAllPreferences(preferences: CB_Preference[]) {
        this.apiService.EditAllCB_Preferences(preferences).subscribe((preferences) => {
            console.log(preferences);
            this._showSetingsPopup.next(false);
        });
    }

    DeleteAllCB_Preferences(preferences: CB_Preference[]) {
        this.apiService.DeleteAllCB_Preferences(preferences).subscribe((preferences) => {
            console.log(preferences);
            this._showSetingsPopup.next(false);
        });
    }

    addNewPreference(preference: CB_Preference) {
        this.apiService.AddNewCB_Preference(preference).subscribe((preferences) => {
            console.log(preferences);
        });
    }

    updatePreference(preference: CB_Preference) {
        this.apiService.EditCB_Preference(preference).subscribe((preferences) => {
            console.log(preferences);
        });
    }
    deletePreference(preference: CB_Preference) {
        this.apiService.DeleteCB_Preference(preference).subscribe((preferences) => {
            console.log(preferences);
        });
    }

    getPreferencesByUserId(userId: string, tenant: number) {
        this.apiService.GetCB_PreferenceByUserIdAndTenant(userId, tenant).subscribe((data: any) => {
            let PreferencesList: CB_Preference[] = data?.body;
            this.allPreferences.next(PreferencesList);
            // console.log(PreferencesList);
        });
    }

    showSettingsClick(showSetings: boolean) {
        this._showSetingsPopup.next(showSetings);
    }

    defualtDataPreferences: CB_Preference[] = [
        { Id: '', Tenant: SessionInfo.LoggedUserTenant, BackgroundColor: '#F3F5F7', TextColor: '#1C1C1C', Level: 1, UserId: SessionInfo.LoggedUserId },
        { Id: '', Tenant: SessionInfo.LoggedUserTenant, BackgroundColor: '#F3F5F7', TextColor: '#1C1C1C', Level: 2, UserId: SessionInfo.LoggedUserId },
        { Id: '', Tenant: SessionInfo.LoggedUserTenant, BackgroundColor: '#F3F5F7', TextColor: '#1C1C1C', Level: 3, UserId: SessionInfo.LoggedUserId },
        { Id: '', Tenant: SessionInfo.LoggedUserTenant, BackgroundColor: '#F3F5F7', TextColor: '#1C1C1C', Level: 4, UserId: SessionInfo.LoggedUserId },
        { Id: '', Tenant: SessionInfo.LoggedUserTenant, BackgroundColor: '#F3F5F7', TextColor: '#1C1C1C', Level: 5, UserId: SessionInfo.LoggedUserId }
    ];

    public hierarchyLevels: HierarchyLevel[] = [
        { level: 1, label: 'חלק' },
        { level: 2, label: 'פרק' },
        { level: 3, label: 'פרט' },
        { level: 4, label: 'סעיף' },
        { level: 5, label: 'פרט מכס' }
    ];
    public headerColumns: string[] = ['רמה', 'רקע', 'צבע טקסט'];
}

export class CB_Preference {
    Id: string;
    Tenant: number;
    BackgroundColor: string;
    TextColor: string;
    Level: number;
    UserId: string;
}
export interface HierarchyLevel {
    level: number;
    label: string
};
export interface SettinsTableData {
    hierarchyLevels: HierarchyLevel[];
    headerColumns: string[];
};

export enum PreferenceType {
    Background = 'background',
    Text = 'text'
}