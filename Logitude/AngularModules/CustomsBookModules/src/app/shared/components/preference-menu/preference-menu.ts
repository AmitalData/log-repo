import { Component, OnInit, ViewContainerRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgFor, NgIf, NgStyle } from '@angular/common';
import { CB_Preference, PreferencesService, SettinsTableData } from './PreferencesService';
import { SessionInfo } from '../../../core/Infrastructure/Utilities/SessionInfo';
import { MatIconModule } from '@angular/material/icon';
import { ColorPickerService, Cmyk, ColorPickerModule } from 'ngx-color-picker';

@Component({
  selector: 'app-preference-menu',
  standalone: true,
  imports: [FormsModule, NgFor, NgIf, MatIconModule, NgStyle, ColorPickerModule],
  templateUrl: './preference-menu.html',
  styleUrl: './preference-menu.css',
})
export class PreferenceMenuComponent implements OnInit {
  public toggle: boolean = false;
  public rgbaText: string = 'rgba(165, 26, 214, 0.2)';
  public selectedColor: string = 'color1';
  public cmykColor: Cmyk = new Cmyk(0, 0, 0, 0);

  tableData: SettinsTableData = {
    hierarchyLevels: this.preferencesService.hierarchyLevels,
    headerColumns: this.preferencesService.headerColumns
  }
  showSetings: boolean = false;
  preferences: CB_Preference[] = [];
  settingsColors: string = "הגדרת צבעים";

  constructor(public vcRef: ViewContainerRef, private cpService: ColorPickerService, private preferencesService: PreferencesService) { }
  ngOnInit(): void {
    this.loadPreferences();
  }

  public presetValues: string[] = [
    "#FFFFFF", "#000000", "#808080", "#000080", "#0000FF", "#ADD8E6", "#00FFFF",
    "#008080", "#00FF00", "#32CD32", "#228B22", "#FFFF00", "#FFD700", "#FFA500",
    "#FF0000", "#FF7F50", "#FA8072", "#FFC0CB", "#FF00FF", "#800080", "#4B0082",
    "#E6E6FA", "#DDA0DD", "#A52A2A"
  ];


  public onEventLog(event: string, data: any): void {
    console.log(event, data);
  }

  public convertColor(color: string, format: 'cmyk' | 'rgba'): Cmyk | string {
    const hsva = this.cpService.stringToHsva(color, true);
    if (!hsva) return format === 'cmyk' ? new Cmyk(0, 0, 0, 0) : '';
    if (format === 'cmyk') {
      return this.cpService.rgbaToCmyk(this.cpService.hsvaToRgba(hsva));
    }
    return this.cpService.outputFormat(hsva, 'rgba', null);
  }

  public onChangeColorCmyk(color: string): Cmyk {
    return this.convertColor(color, 'cmyk') as Cmyk;
  }

  public onChangeColorHex8(color: string): string {
    return this.convertColor(color, 'rgba') as string;
  }

  loadPreferences(): void {
    this.preferencesService.allPreferences.subscribe((data) => {
      this.preferences = this.tableData.hierarchyLevels.map(({ level }) =>
        data.find((p) => p?.Level === level) ?? this.createDefaultPreference(level)
      );
    });

    this.preferencesService._showSetingsPopup.subscribe((data) => {
      this.showSetings = data;
    });
  }

  updateBackgroundColor(event: Event, index: number): void {
    this.preferences[index].BackgroundColor = (event.target as HTMLInputElement)?.value ? (event.target as HTMLInputElement).value : '#F3F5F7';
  }

  updateTextColor(event: Event, index: number): void {
    this.preferences[index].TextColor = (event.target as HTMLInputElement)?.value ? (event.target as HTMLInputElement).value : '#1C1C1C';
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

