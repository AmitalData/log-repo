import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgFor, NgIf } from '@angular/common';
import { CB_Preference, PreferencesService, SettinsTableData } from './PreferencesService';
import { SessionInfo } from '../../../core/Infrastructure/Utilities/SessionInfo';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-preference-menu',
  standalone: true,
  imports: [FormsModule, NgFor, NgIf, MatIconModule],
  templateUrl: './preference-menu.html',
  styleUrl: './preference-menu.css',
})
export class PreferenceMenuComponent implements OnInit {

  tableData: SettinsTableData = {
    hierarchyLevels: this.preferencesService.hierarchyLevels,
    headerColumns: this.preferencesService.headerColumns
  }
  showSetings: boolean = false;
  preferences: CB_Preference[] = [];
  settingsColors: string = "הגדרת צבעים";
  constructor(private preferencesService: PreferencesService) { }

  ngOnInit(): void {
    this.loadPreferences();
  }

  loadPreferences(): void {
    this.preferencesService.allPreferences.subscribe((data) => {
      this.preferences = this.tableData.hierarchyLevels.map(({ level }) =>
        data.find((p) => p.Level === level) ?? this.createDefaultPreference(level)
      );
    });

    this.preferencesService._showSetingsPopup.subscribe((data) => {
      this.showSetings = data;
    });
  }


  createDefaultPreference(level: number): CB_Preference {
    return {
      Id: '',
      Tenant: SessionInfo.LoggedUserTenant,
      BackgroundColor: '#F3F5F7',
      TextColor: '#1C1C1C',
      Level: level,
      UserId: SessionInfo.LoggedUserId
    };
  }

  savePreferences(): void {
    this.preferencesService.updateAllPreferences(this.preferencesService.allPreferences?.getValue());
  }

  clearPreferences(): void {
    if (this.preferencesService.allPreferences?.getValue()?.length > 0) {
      this.preferences.forEach((pref) => {
        pref.BackgroundColor = '#F3F5F7';
        pref.TextColor = '#1C1C1C';
      });
      this.preferencesService.updateAllPreferences(this.preferences);
    }
  }

  showSettingsClick() {
    this.preferencesService.showSettingsClick(!this.showSetings);
  }
}

