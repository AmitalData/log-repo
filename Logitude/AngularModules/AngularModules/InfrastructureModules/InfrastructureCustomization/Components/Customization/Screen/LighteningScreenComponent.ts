declare var System: any;
declare var window: any;
import { Component, OnInit } from '@angular/core';
import { ConfirmWindow } from 'Controls/Windows/ConfirmWindow';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { ScreenSectionListService } from 'Infrastructure/Services/StandardLists/ScreenSectionListService';
import { ScreenSectionPMService } from 'Infrastructure/Services/StandardPMs/ScreenSectionPMService';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ScreenLayoutComponent, SectionScreenItem } from '../ScreenLayoutComponent';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { ScreenSectionPM } from '../../../../../Infrastructure/EntityPMs/ScreenSectionPM';
import { CustomizationEditComponent } from '../CustomizationEditComponent';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';
import { forEach } from 'cypress/types/lodash';
const deleteSectionMessage = "Are you sure you want delete this section?";
@Component({
    selector: 'LighteningScreenComponent',
    templateUrl: './LighteningScreenComponent.html',
    styleUrls: ['./LighteningScreenComponent.css'],

})

export class LighteningScreenComponent extends BaseComponent implements OnInit
{
    public ScreenLayoutComponent: ScreenLayoutComponent;
    public lighteningScreenWidth: string;
    private screenLayoutwidth = 650;
    public SectionWidth = "750";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor()
    {
        super();
        this.lighteningScreenWidth = (window.innerWidth - this.screenLayoutwidth) + "px";
    }

    ngOnInit(): void
    {

    }

    private orderedSections = new ObservableCollection([]);
    get Sections() {

        return this.ScreenLayoutComponent.SectionScreens.filter(s => !s.Section.Inactive).sort((a, b) => {
            return (a.Section.Number === b.Section.Number) ? 0 : (a.Section.Number < b.Section.Number) ? -1 : 1
        })
    //    return this.ScreenLayoutComponent.SectionScreens.filter(s => !s.Section.Inactive)
    }


    Run(screenLayoutComponent: ScreenLayoutComponent)
    {
        this.ScreenLayoutComponent = screenLayoutComponent;
        this.SectionWidth = this.ScreenLayoutComponent.SelectedItem.ScreenPM.NumberOfColumns * 250 +"px";
    }

    public OnSectionScreenNameChange() {
        this.ScreenLayoutComponent.Modified = true;
    }

    RemoveSection(section)
    {
        const confirmWindow = new ConfirmWindow();
        confirmWindow.Show(deleteSectionMessage);
        confirmWindow.WindowClosed.subscribe((event: any) =>
        {
            if (confirmWindow.Yes)
                this.RemoveSectionFromScreenLayout(section);
        });
    }


    RemoveSectionFromScreenLayout(editedSection: SectionScreenItem)
    {
        SessionLocator.SelectedSession.StartBusyIndicatorSaving();
        const sectionIndex = this.ScreenLayoutComponent.SectionScreens.findIndex(s => s == editedSection);
        const section = this.ScreenLayoutComponent.SectionScreens.find(s => s == editedSection);
        if (sectionIndex < 0)
            return;

        section.Section.Inactive = true;
        section.Section.IsDirty = true;

        this.ScreenLayoutComponent.DeleteSectionFields(editedSection);
        SessionLocator.SelectedSession.StopBusyIndicator();

    }

    EditSection(screenSection) {
        if (!screenSection) return;
        if (!screenSection.Section) return;
        this.ShowEditNewGridScreenSectionComponent(screenSection);
    }

    ShowEditNewGridScreenSectionComponent(screenSection: SectionScreenItem) {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 420;
        logitudeWindow.Height = 250;
        logitudeWindow.Title = "Edit Component"
        let windowArgs: any = {};
        windowArgs.ObjecttableId = this.ScreenLayoutComponent.ObjecttableId;
        windowArgs.IsNew = false;
        windowArgs.Name = screenSection.Section.Name;
        windowArgs.RelatedScreenCode = screenSection.Section.RelatedScreenCode;
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/Screen/Section/AddEditGridScreenSectionComponent');
        logitudeWindow.WindowClosed.subscribe(($event: any) => {
            if (!$event) return;
            if (!$event.GridName) return;
            screenSection.Section.Name = $event.GridName;
            let isRelatedScreenCodeModified = screenSection.Section.RelatedScreenCode != $event.RelatedScreenCode;
            screenSection.Section.RelatedScreenCode = $event.RelatedScreenCode;
            this.ScreenLayoutComponent.Modified = true;
            if (isRelatedScreenCodeModified) {
                this.CurrentSession.SessionEvent.emit({ Name: "ReloadGridSection" });
            }
        });
    }

    
    
    DecrementOrder(currentSection: SectionScreenItem) {
        let allScreenSections = this.ScreenLayoutComponent.SectionScreens;

        if (currentSection == this.Sections[0])
            return;

        const prevSection: SectionScreenItem = allScreenSections.find(s => s.Section.Number == currentSection.Section.Number - 1);
        if (!prevSection) return;
        this.ScreenLayoutComponent.SetSectionIndexOrder(prevSection, (prevSection.Section.Number + 1));
        this.ScreenLayoutComponent.SetSectionIndexOrder(currentSection, (currentSection.Section.Number - 1));

        if (prevSection.Inactive) {
            this.DecrementOrder(currentSection);
        }

        currentSection.Section.IsDirty = true;
        this.ScreenLayoutComponent.Modified = true;

    }


    IncrementOrder(currentSection: SectionScreenItem) {
        let allScreenSections = this.ScreenLayoutComponent.SectionScreens;

        if (currentSection == this.Sections[this.Sections.length - 1])
            return;

        const nextSection: SectionScreenItem = allScreenSections.find(s => s.Section.Number == currentSection.Section.Number + 1);
        if (!nextSection) return;
        this.ScreenLayoutComponent.SetSectionIndexOrder(nextSection, (nextSection.Section.Number - 1));
        this.ScreenLayoutComponent.SetSectionIndexOrder(currentSection, (currentSection.Section.Number + 1));

        if (nextSection.Inactive) {
            this.IncrementOrder(currentSection);
        }

        currentSection.Section.IsDirty = true;
        this.ScreenLayoutComponent.Modified = true;

    }



    
}
