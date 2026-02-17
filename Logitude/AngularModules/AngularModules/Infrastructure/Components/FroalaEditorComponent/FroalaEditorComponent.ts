declare var jQuery, SetHtmlToFrame, GetHtmlFromFrame, getHTMLID: any, RegisterCustomFroalaEditorButtom: any;
import {Component, ElementRef, OnInit, AfterViewInit, EventEmitter, Output, ChangeDetectorRef} from '@angular/core';
import {FroalaEditorSetting} from '../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/FroalaEditorSetting';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';
import {AppTool, FileLoader} from '../../Tools';

@Component({
    moduleId: module.id,

    selector: 'FroalaEditor',
    templateUrl: './FroalaEditorComponent.html',
    inputs: ['EditorfroalaSetting']
})

export class FroalaEditorComponent implements OnInit, AfterViewInit {
    elementRef: ElementRef;
    @Output() ComponentFroalaReady = new EventEmitter();
    PreviewDivHeight: string;
    PreviewDivHeight2: string;
    public EditorfroalaSetting: FroalaEditorSetting;
    public HtmlString: string;
    Id: string;
    PreviewDivId: string;
    IsDisableMode: boolean = false;
    UseNormalPreview: boolean = false;
    @Output() FroalaReady: EventEmitter<boolean> = new EventEmitter<boolean>();
    constructor(elementRef: ElementRef, private cd: ChangeDetectorRef) {
        this.elementRef = elementRef;
    }

    ngOnInit() {
        this.EditorfroalaSetting.froalaEditorComponent = this;
        this.Id = this.EditorfroalaSetting.Id;
        this.PreviewDivId = this.EditorfroalaSetting.Id + "Div";

        this.PreviewDivHeight = this.EditorfroalaSetting.Height + "px";
        this.PreviewDivHeight2 = (this.EditorfroalaSetting.Height - 10) + "px";

        if (this.EditorfroalaSetting.IsDisableEdit) {
            this.IsDisableMode = true;
            this.UseNormalPreview = this.EditorfroalaSetting.UseNormalPreview;
        }
    }

    ngAfterViewInit() {
        //FileLoader.LoadFroalaResources().then((isLoaded: boolean) => {
        //    this.ResourcesLoaded.emit(true);
        //    this.ShowEditor(); 
        //});     

        this.ShowEditor();
    }

    public ShowEditor(height: number = this.EditorfroalaSetting.Height) {
        if (this.EditorfroalaSetting.HtmlString) this.HtmlString = this.EditorfroalaSetting.HtmlString;
        if (!this.HtmlString) this.HtmlString = "";

        //  this.HtmlString = this.CheckHtmlStyle(this.HtmlString);


        if (!this.UseNormalPreview) {

            //Froala Editor
            var HtmlID = getHTMLID(this.Id);

            RegisterCustomFroalaEditorButtom(this);


            var froalakey: string = ObjectsLocator.GlobalSetting != null && ObjectsLocator.GlobalSetting.WorkEnvironment == "cloud" ? "8A-9pwkamE5f1kG4ok==" : "ubd1wxffppaxjuE-11A2C-9rs==";


            if (HtmlID.data('froala.editor')) HtmlID.froalaEditor('destroy');

            HtmlID.froalaEditor({
                allowedImageTypes: ["jpeg", "jpg", "png"],
                toolbarButtons: this.EditorfroalaSetting.PageType == "Send" ? ['bold', 'italic', 'underline', 'fontFamily', 'fontSize', 'color', 'inlineStyle', 'paragraphStyle', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'insertTable', 'undo', 'redo', 'selectAll', 'rightToLeft', 'leftToRight', 'lineHeight'] : ['bold', 'italic', 'underline', 'fontFamily', 'fontSize', 'color', 'inlineStyle', 'paragraphStyle', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'insertTable', 'undo', 'redo', 'selectAll', 'insertLink', 'rightToLeft', 'leftToRight', 'PageBreak', 'lineHeight'],

                toolbarButtonsMD: this.EditorfroalaSetting.PageType == "Send" ? ['bold', 'italic', 'underline', 'fontFamily', 'fontSize', 'color', 'inlineStyle', 'paragraphStyle', 'paragraphFormat', 'align', 'formatOL', 'insertTable', 'rightToLeft', 'leftToRight', 'lineHeight'] : ['bold', 'italic', 'underline', 'fontFamily', 'fontSize', 'color', 'inlineStyle', 'paragraphStyle', 'paragraphFormat', 'align', 'formatOL', 'insertTable', 'insertLink', 'rightToLeft', 'leftToRight', 'PageBreak', 'lineHeight'],
                toolbarButtonsSM: this.EditorfroalaSetting.PageType == "Send" ? ['bold', 'italic', 'underline', 'fontFamily', 'fontSize', 'color', 'insertTable', 'align', 'rightToLeft', 'leftToRight', 'lineHeight'] : ['bold', 'italic', 'underline', 'fontFamily', 'fontSize', 'color', 'inlineStyle', 'align', 'insertTable', 'rightToLeft', 'leftToRight', 'PageBreak', 'lineHeight'],
                toolbarButtonsXS: this.EditorfroalaSetting.PageType == "Send" ? ['bold', 'italic', 'underline', 'fontFamily', 'fontSize', 'color', 'insertTable', 'align', 'rightToLeft', 'leftToRight', 'lineHeight'] : ['bold', 'italic', 'underline', 'fontFamily', 'fontSize', 'color', 'inlineStyle', 'align', 'insertTable', 'rightToLeft', 'leftToRight', 'PageBreak', 'lineHeight'],
                lineBreakerTags: ['table', 'hr', 'form'],
                pluginsEnabled: null,
                height: height,
                heightMax: height,
                iframe: true,
                charCounterCount: false,
                inlineMode: false,
                zIndex: -1,
                direction: '',
                key: froalakey,
                useClasses: false,

                tableStyles: {
                    All: 'All',
                    Box: 'Box',
                    None: 'None',
                    Red: 'Border red',
                    Blue: 'Border blue',
                    DarkBlue: 'Border dark blue',
                    //Green: 'Border green',
                    //Yellow: 'Border yellow',
                    Brown: 'Border brown',
                    Maroon: 'Border maroon',

                    Black: 'Border Black',
                    Gray: 'Border gray',
                    LightGray: 'Border light gray',

                    White: 'Border white',

                },
                //scrollableContainer: '#' + this.Id,

                tableMultipleStyles: true,

                tableCellStyles: {
                    BorderLeft: 'Remove border left',
                    BorderRight: 'Remove border right',
                    BorderBottom: 'Remove border bottom',
                    BorderTop: 'Remove border top',
                },


                lineHeights: {
                    Default: '',
                    '0.2': '0.2',
                    '0.5': '0.5',
                    Single: '1',
                    '1.15': '1.15',
                    '1.5': '1.5',
                    Double: '2'
                },

            });

            HtmlID.froalaEditor('html.set', this.HtmlString);

            if (this.IsDisableMode) {

                HtmlID.froalaEditor('edit.off');
                HtmlID.froalaEditor('toolbar.hide');
            }
        }

        else {
            SetHtmlToFrame(this.PreviewDivId, this.HtmlString);
        }

        this.EditorfroalaSetting.FroalaEditorIsReady = true;

    }

    getHtml() {

        var html: string = "";

        var HtmlID = getHTMLID(this.Id);
        if (this.EditorfroalaSetting.FroalaEditorIsReady) {

            if (!this.UseNormalPreview) {

                if (HtmlID) {
                    html = HtmlID.froalaEditor('html.get');
                }
                else html = this.HtmlString;
                this.EditorfroalaSetting.HtmlString = html;
            } else {
                html = GetHtmlFromFrame(this.PreviewDivId);
                this.EditorfroalaSetting.HtmlString = html;
            }

        }

        return html;
    }



    SetHtml(html: string) {
        this.HtmlString = this.EditorfroalaSetting.HtmlString = html;
        if (this.EditorfroalaSetting.FroalaEditorIsReady) {
            if (!this.UseNormalPreview) {
                var HtmlID = getHTMLID(this.Id);
                if (HtmlID) {
                    HtmlID.froalaEditor('html.set', html);
                }

            } else {
                var element = document.getElementById(this.PreviewDivId);
                if (element) SetHtmlToFrame(this.PreviewDivId, html);
            }

        }
    }


    public SetHeight(height: number) {

        this.ShowEditor(height);


    }

    public InSertHtml(html: string) {
        if (!this.IsDisableMode) {
            if (this.EditorfroalaSetting.FroalaEditorIsReady) {
                var HtmlID = getHTMLID(this.Id);
                if (HtmlID) {
                    HtmlID.froalaEditor('html.insert', html, true);
                }
            }

        }
    }


    public DestroyfroalaEditor() {

        if (!this.UseNormalPreview) {
            if (this.EditorfroalaSetting.FroalaEditorIsReady) {
                var HtmlID = getHTMLID(this.Id);

                if (HtmlID && HtmlID.data('froala.editor')) {
                    HtmlID.froalaEditor('destroy');
                }
            }
        }


    }

    CheckHtmlStyle(html: string) {
        if (html.indexOf(".Class1") == -1) {
            var styles = "<style>.class1{border-collapse: collapse;}.class1 td, th{border: 1px solid red;line-height:21px;}.class1 td{font-size: 14px; padding-left: 4px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;}.class2{border-collapse: collapse;}.class2 td, th{border: 1px solid blue;line-height:21px;}.class2 td{font-size: 14px; padding-left: 4px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;}table thead tr th, table tbody tr td  {font - size: 14px; padding - left: 4px; overflow: hidden; white - space: nowrap; text - overflow: ellipsis; border: 1px solid lightgray; line - height:18px; } table{border-collapse: collapse; }</style>";

            html = styles + html;
        }
        return html;

    }



}

