/// <reference path="../../tools.ts" />
declare var Stimulsoft: any;
declare var jQuery: any;
import {Component, OnInit, Output, EventEmitter}  from '@angular/core';

import {DocumentTypeTemplatePMExtendedService} from '../../../Common/Services/ExtendedPMs/DocumentTypeTemplatePMExtendedService';


import {StimulsoftArg} from '../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/StimulsoftArg';
import {DocumentTypeTemplateFilter} from '../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/Filters/DocumentTypeTemplateFilter';


import {DocumentTypeTemplatePM} from '../../../Common/EntityPMs/DocumentTypeTemplatePM';
import {DocumentTypeTemplateViewModel} from '../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/ViewModel/DocumentTypeTemplateViewModel';

import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';


@Component({
    moduleId: module.id,

    selector: 'StimulsoftDesigner',
    templateUrl: './StimulsoftDesignerComponent.html',
    inputs: ['stimulsoftArgData'],

    providers: [DocumentTypeTemplatePMExtendedService, DocumentTypeTemplatePM],




})
//
export class StimulsoftDesignerComponent implements OnInit {
    @Output() OnCloseWindow = new EventEmitter();
    stimulsoftArgData: StimulsoftArg;
    designer: any;

    //DocumenttypetemplateId: string;
     
    //CurrentDocumentOutId: string;
    //EntitiyId: string;
    //DocumenttypeCode: string;
    //DocumenttypecopyId: string;

    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _documentTypeTemplatePMExtendedService:DocumentTypeTemplatePMExtendedService) {
        Stimulsoft.Base.StiLicense.key = "6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHmx0GX2YaQY2fO4QUxViJm3MQEqlPzuUWXG/MVALbDozqE5ju" +
            "b1Lxxc9dG4qgTwOniU2gMMveQV+dJH1XkfRD1MNXb7qftfSxhKy/sz48Bbjuk1L3hTtOWwLJkGU/7cdsKzMCby7tGL" +
            "OGYgh8SwTOub9I9sRPEX2lQYcPP+Il4Xfhoo6Wuy8pZsfQ9T1qeKjawy2fkZdSnLcD6kfKqKtHQsdICN5BiXWAyXzw" +
            "act0mGsT790xrC2o/tO9hMolOEEYeJFTlsNJSorhgH6cn+TeBr/VhyDswq4OXv+op9bZc1z7dqYAYxkcWvdiQ04/L+" +
            "hJY8P23m4dqBZfxbUrOs17ZhtItSd2QWuUCyBywn6UZ9fZSOLDKl0Lk2ItxbjsHGv1Hp51puoRA/LxYOc5va7DT1Ws" +
            "R8S6if6a53D0VkMeB8gkBGwWVh8WhCH/uaOnq16tH2sicM8DpNRHUYymWcrF4QHpwZGeRiuMIkruiH7HZD+pTyPI8M" +
            "ObqwwI+EWgTu2QZmYXdH6VmzdL8T3d+pgYEz";

        Stimulsoft.Base.StiFontCollection.AddFontFile("./IDAutomationCMC7n10.ttf");

    }
    ngOnInit() {


    }

    private windowArgs: any;
    public TemplateId: string;
    private documentTypeTemplateViewModel:DocumentTypeTemplateViewModel;
    SetWindowArgs(args: any) {
        this.windowArgs = args;
        this.TemplateId = this.windowArgs.TemplateId;
        this.documentTypeTemplateViewModel = this.windowArgs.ViewModel;
        this.LoadStimulDesigner();
    }

    LoadStimulDesigner() {

        //var options = new Stimulsoft.Designer.StiDesignerOptions();
        //options.appearance.fullScreenMode = true;
        //options.appearance.interfaceType = "Mouse";
        //options.toolbar.showFileMenuExit = false;
        //options.appearance.showLocalization = true;
        //Stimulsoft.System.NodeJs.useWebKit = true;
        //Stimulsoft.System.NodeJs.localizationPath = "locales";

       var options = new Stimulsoft.Designer.StiDesignerOptions();
        options.appearance.fullScreenMode = true;
        options.toolbar.showFileMenuExit = false;
        options.allowChangeWindowTitle = false;
        //options.zoomout = "100%";
        options.dictionary.businessObjectsPermissions = Stimulsoft.Designer.StiDesignerPermissions.All;
            //options.toolbar.showPreviewButton = true;
            //options.toolbar.showFileMenu = true;
            //options.components.showImage = false;
            //options.components.showShape = false;
            //options.components.showPanel = false;
            //options.components.showCheckBox = false;
            //options.components.showSubReport = false;
            
            //widthwindow = window.innerWidth;
            var heighthwindow = window.innerHeight;

            options.width = "100%";
            options.height = (heighthwindow - 200).toString() + "px";
           
            this.designer = new Stimulsoft.Designer.StiDesigner(options, "StiDesigner", false);

            var isload = false;
            var report = new Stimulsoft.Report.StiReport();
            //report.zoomout = "100%";
          

            //if (this.stimulsoftArgData.EditDocumentComponent.IsManageStimul && item != null) {

            //    if (item.Jsonstring != null && item.Jsonstring != undefined) {
            //        isload = true;
            //        report.load(item.Jsonstring);
            //        this.designer.report = report;
            //        this.designer.renderHtml("designerContent");
            //    }
              
            //}


           // if (!isload) {

                //if (item == null || item === undefined) {
                //    if (this.stimulsoftArgData.EditDocumentComponent.SelectedDocumentTypeTemplateViewModel != null && this.stimulsoftArgData.EditDocumentComponent.SelectedDocumentTypeTemplateViewModel != undefined) {
                //        item = this.stimulsoftArgData.EditDocumentComponent.SelectedDocumentTypeTemplateViewModel;
                //    }
            
                //}
             
          
            if (this.documentTypeTemplateViewModel != null && this.documentTypeTemplateViewModel != undefined && this.documentTypeTemplateViewModel.IsHaveJsonString) {
              
                this._documentTypeTemplatePMExtendedService.GetTemplateBodyhtmlOrJsonByDocumentTemplateId(this.TemplateId, SessionLocator.Tenant,false,"stmual").subscribe(res=> {
                            report.load(res.Result);
                            this.designer.report = report;
                            this.designer.renderHtml("designerContent");
                        });
                }

                else {
                    //console.log("Start");
                    //console.log(this.DocumenttypetemplateId);
                    //console.log(this.Tenant);
          
                    this._documentTypeTemplatePMExtendedService.GetTemplateBodyByDocumentTemplateId(this.TemplateId, SessionLocator.Tenant).subscribe(res=> {
                        report.load(res.Result);
                        this.designer.report = report;
                        this.designer.renderHtml("designerContent");
                    });

                }
         
        
    }

    CloseButtonClicked() {
        this.CurrentSession.CurrentWindow.Close("");
    }

    SaveButtonClicked() {

        //var isdisplay = false;
        //if (this.stimulsoftArgData.EditDocumentComponent.IsManageStimul) {
        //    isdisplay = true;
        //}

        var jsonStr = this.designer.report.saveToJsonString();
        //StimulsoftArg.ReportData = jsonStr;
        var filter = new DocumentTypeTemplateFilter();
        filter.Id = this.TemplateId;
        filter.Tenant = SessionLocator.Tenant;
        filter.Body = jsonStr;
        filter.TemplateType = "Stimul";

        this._documentTypeTemplatePMExtendedService.SaveDocumentTemplate(filter).subscribe(res=> {
            this.CloseButtonClicked();
            //if (this.stimulsoftArgData.EditDocumentComponent.IsShowTemplateList) {

            //    if (this.stimulsoftArgData.EditDocumentComponent.SelectedItemFromMenu != null) {

                  
            //        this.stimulsoftArgData.EditDocumentComponent.SelectedItemFromMenu.Jsonstring = jsonStr;
            //        if (this.stimulsoftArgData.EditDocumentComponent.SelectedItemFromMenu == this.stimulsoftArgData.EditDocumentComponent.SelectedDocumentTypeTemplateViewModel) {


                     
            //            this.stimulsoftArgData.EditDocumentComponent.LoadstimulData(this.DocumenttypetemplateId, isdisplay,true);
                       
            //        }
            //    }

            //}
            //else {

            //    this.stimulsoftArgData.EditDocumentComponent.LoadstimulData(this.DocumenttypetemplateId, isdisplay,true);
            //}

            //this.CloseButtonClicked();
        });
   
    }


}
