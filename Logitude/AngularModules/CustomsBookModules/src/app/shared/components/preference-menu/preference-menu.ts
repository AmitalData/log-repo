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
  public presetValues: string[] = [];
  public selectedColor: string = 'color1';
  public cmykColor: Cmyk = new Cmyk(0, 0, 0, 0);

  tableData: SettinsTableData = {
    hierarchyLevels: this.preferencesService.hierarchyLevels,
    headerColumns: this.preferencesService.headerColumns
  }
  showSetings: boolean = false;
  preferences: CB_Preference[] = [];
  settingsColors: string = "הגדרת צבעים";

  constructor(public vcRef: ViewContainerRef, private cpService: ColorPickerService, private preferencesService: PreferencesService) {
    this.presetValues = this.getColorValues();
  }
  ngOnInit(): void {
    this.loadPreferences();
  }

  public colorList = [
    { key: "white", value: "#FFFFFF", friendlyName: "White" },
    { key: "black", value: "#000000", friendlyName: "Black" },
    { key: "gray", value: "#808080", friendlyName: "Gray" },
    { key: "navy", value: "#000080", friendlyName: "Navy" },
    { key: "blue", value: "#0000FF", friendlyName: "Blue" },
    { key: "lightblue", value: "#ADD8E6", friendlyName: "Light Blue" },
    { key: "cyan", value: "#00FFFF", friendlyName: "Cyan" },
    { key: "teal", value: "#008080", friendlyName: "Teal" },
    { key: "green", value: "#00FF00", friendlyName: "Green" },
    { key: "lime", value: "#32CD32", friendlyName: "Lime" },
    { key: "forestgreen", value: "#228B22", friendlyName: "Forest Green" },
    { key: "yellow", value: "#FFFF00", friendlyName: "Yellow" },
    { key: "gold", value: "#FFD700", friendlyName: "Gold" },
    { key: "orange", value: "#FFA500", friendlyName: "Orange" },
    { key: "red", value: "#FF0000", friendlyName: "Red" },
    { key: "coral", value: "#FF7F50", friendlyName: "Coral" },
    { key: "salmon", value: "#FA8072", friendlyName: "Salmon" },
    { key: "pink", value: "#FFC0CB", friendlyName: "Pink" },
    { key: "magenta", value: "#FF00FF", friendlyName: "Magenta" },
    { key: "purple", value: "#800080", friendlyName: "Purple" },
    { key: "indigo", value: "#4B0082", friendlyName: "Indigo" },
    { key: "lavender", value: "#E6E6FA", friendlyName: "Lavender" },
    { key: "plum", value: "#DDA0DD", friendlyName: "Plum" },
    { key: "brown", value: "#A52A2A", friendlyName: "Brown" }
  ];
  getColorValues() {
    return this.colorList.map(c => c.value);
  }


  public onEventLog(event: string, data: any): void {
    console.log(event, data);
  }

  public onChangeColorCmyk(color: string): Cmyk {
    const hsva = this.cpService.stringToHsva(color);

    if (hsva) {
      const rgba = this.cpService.hsvaToRgba(hsva);

      return this.cpService.rgbaToCmyk(rgba);
    }

    return new Cmyk(0, 0, 0, 0);
  }

  public onChangeColorHex8(color: string): string {
    const hsva = this.cpService.stringToHsva(color, true);

    if (hsva) {
      return this.cpService.outputFormat(hsva, 'rgba', null);
    }

    return '';
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

  updateBackgroundColor(event: Event, index: number): void {
    this.preferences[index].BackgroundColor = (event.target as HTMLInputElement).value;
  }

  updateTextColor(event: Event, index: number): void {
    this.preferences[index].TextColor = (event.target as HTMLInputElement).value;
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

