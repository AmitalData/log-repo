import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
export class DocumentPM {

    public UIProperties: UIProperties;
    constructor() {
        this.UIProperties = new UIProperties;
        this.IsDirty = false;
    }


    private id: string;
    public get Id() { return this.id; }
    public set Id(newValue: string) { this.id = newValue; this.MarkAsDirty(); }

    private fileName: string;
    public get FileName() { return this.fileName; }
    public set FileName(newValue: string) { this.fileName = newValue; this.MarkAsDirty(); }


    private fileSize: number;
    public get FileSize() { return this.fileSize; }
    public set FileSize(newValue: number) { this.fileSize = newValue; this.MarkAsDirty(); }

    private extension: string;
    public get Extension() { return this.extension; }
    public set Extension(newValue: string) { this.extension = newValue; this.MarkAsDirty(); }

    private calculatedFileName: string;
    public get CalculatedFileName() { return this.calculatedFileName; }
    public set CalculatedFileName(newValue: string) { this.calculatedFileName = newValue; this.MarkAsDirty(); }
    
    public IsDirty: boolean;
    MarkAsDirty() {
        this.IsDirty = true;

    }

}
