import {Pipe} from '@angular/core';
import {AppTool} from '../Tools';

@Pipe({ name: 'AttatchmentIconPipe' })

export class AttatchmentIconPipe {

    transform(value: string,Parameter: string = null): string {
        var path = "something";
        var newValue = value;

        if (!AppTool.IsNullOrEmpty(newValue)) {

            if (!AppTool.IsNullOrEmpty(Parameter)) {
                path = this.ApplyParameterPipe(newValue, Parameter);
            }
            else {
                path = this.GetIcon(newValue);
            }
        }

        return path; 
    }
    ApplyParameterPipe(value: string, Parameter: string) {
        var path = "something";

        if (Parameter == "FilingInboxCode") {
            path = this.GetIcon(value.split('.')[1]);
        }

        return path;
    }

    GetIcon(value) {
        var path = "something";
        if (value != null) {
            switch (value.toUpperCase()) {
                case "PDF":
                    {
                        path = "./Images/FileIcons/File-pdf-48.png";
                        break;
                    }

                case "TXT":
                    {
                        path = "./Images/FileIcons/txt_48.png";
                        break;
                    }

                case "XLS":
                    {
                        path = "./Images/FileIcons/Microsoft-Office-Excel-48.png";
                        break;
                    }

                case "XLSX":
                    {
                        path = "./Images/FileIcons/Microsoft-Office-Excel-48.png";
                        break;
                    }

                case "DOC":
                    {
                        path = "./Images/FileIcons/Microsoft-Office-Word-48.png";
                        break;
                    }

                case "DOCX":
                    {
                        path = "./Images/FileIcons/Microsoft-Office-Word-48.png";
                        break;
                    }

                case "PPT":
                    {
                        path = "./Images/FileIcons/Microsoft-Office-PowerPoint-48.png";
                        break;
                    }

                case "PPTX":
                    {
                        path = "./Images/FileIcons/Microsoft-Office-PowerPoint-48.png";
                        break;
                    }

                case "ZIP":
                    {
                        path = "./Images/FileIcons/Zip-icon.png";
                        break;
                    }

                case "RAR":
                    {
                        path = "./Images/FileIcons/Zip-icon.png";
                        break;
                    }

                case "XML":
                    {
                        path = "./Images/FileIcons/Document-xml-48.png";
                        break;
                    }

                default:
                    {
                        path = "./Images/FileIcons/attachment-icon.png";
                        break;
                    }
            }
        }
        return path;
    }

}
