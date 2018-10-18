

        import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
        import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
        export class TermsofUsePM {

            public UIProperties: UIProperties;
            constructor() {
                this.UIProperties = new UIProperties;
                this.IsDirty = false;
            }


            private version: number;
            public get Version() { return this.version; }
            public set Version(newValue: number) { this.version = newValue; this.MarkAsDirty(); }


            private date: Date;
            public get Date() { return this.date; }
            public set Date(newValue: Date) { this.date = newValue; this.MarkAsDirty(); }


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