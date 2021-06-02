

        import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
        import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
        export class TermsofUsePM {
             
            public UIProperties: UIProperties;
            constructor() {
                this.UIProperties = new UIProperties;
                this.IsDirty = false;
            }

            private tenant: number;
            public get Tenant() { return this.tenant; }
            public set Tenant(newValue: number) { this.tenant = newValue; this.MarkAsDirty(); }

            private id: number;
            public get Id() { return this.id; }
            public set Id(newValue: number) { this.id = newValue; this.MarkAsDirty(); }
             

            private versionNumber: number;
            public get VersionNumber() { return this.versionNumber; }
            public set VersionNumber(newValue: number) { this.versionNumber = newValue; this.MarkAsDirty(); }

            private versionDocumentId: string;
            public get VersionDocumentId() { return this.versionDocumentId; }
            public set VersionDocumentId(newValue: string) { this.versionDocumentId = newValue; this.MarkAsDirty(); }

            private versionDocumentName: string;
            public get VersionDocumentName() { return this.versionDocumentName; }
            public set VersionDocumentName(newValue: string) { this.versionDocumentName = newValue; this.MarkAsDirty(); }
             
            private date: Date;
            public get Date() { return this.date; }
            public set Date(newValue: Date) { this.date = newValue; this.MarkAsDirty(); }

            private fileData: any;
            public get FileData() { return this.fileData; }
            public set FileData(newValue: Date) { this.fileData = newValue; this.MarkAsDirty(); }


            private divSelectBackgroud: string;
            public get DivSelectBackgroud() { return this.divSelectBackgroud; }
            public set DivSelectBackgroud(newValue: string) { this.divSelectBackgroud = newValue; this.MarkAsDirty(); }
            


            public OldEntityPM: TermsofUsePM;

            public IsDirty: boolean;
            MarkAsDirty() {
                this.IsDirty = true;

            }
            private MyClone: TermsofUsePM;

            public CloneMe() {
                ServiceHelper.CloneEntityPM(this);
            }

            public RejectChanges() {
                ServiceHelper.RejectEntityPMChanges(this);
            }

        }