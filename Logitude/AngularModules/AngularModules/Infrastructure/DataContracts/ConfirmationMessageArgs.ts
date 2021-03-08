import { Injectable } from '@angular/core';

@Injectable()

export class ConfirmationMessageArgs{
   
    public YesText: string;
    public NoText: string;
    public MessageText: string;
    public ShowCancelButton: boolean;
    public YesAction: any;
    public NoAction: any;

    public Builder = new class {

        constructor(public superThis: ConfirmationMessageArgs) {
            
        }

        public YesText(YesText: string) {
            this.superThis.YesText = YesText;
            return this;
        }

        public NoText(NoText: string) {
            this.superThis.NoText = NoText;
            return this;
        }

        public MessageText(MessageText: string) {
            this.superThis.MessageText = MessageText;
            return this;
        }

        public ShowCancelButton(ShowCancelButton: boolean) {
            this.superThis.ShowCancelButton = ShowCancelButton;
            return this;
        }

        public YesAction(YesAction: any){
            this.superThis.YesAction = YesAction;
            return this;
        }

        public NoAction(NoAction: any) {
            this.superThis.NoAction = NoAction;
            return this;
        }

        public build(): ConfirmationMessageArgs {
            return this.superThis;
        }

    }(this);

}

