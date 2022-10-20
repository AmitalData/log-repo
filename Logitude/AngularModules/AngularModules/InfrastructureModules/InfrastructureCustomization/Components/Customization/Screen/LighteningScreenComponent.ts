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
    constructor()
    {
        super();
        this.lighteningScreenWidth = (window.innerWidth - this.screenLayoutwidth) + "px";

    }

    ngOnInit(): void
    {

    }


    get Sections(){
        return this.ScreenLayoutComponent.SectionScreens.filter(s=>!s.Section.Inactive)
    }


    Run(screenLayoutComponent: ScreenLayoutComponent)
    {
        this.ScreenLayoutComponent = screenLayoutComponent;

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
            screenSection.Section.RelatedScreenCode = $event.RelatedScreenCode;
            this.ScreenLayoutComponent.Modified = true;
        });
    }

}
