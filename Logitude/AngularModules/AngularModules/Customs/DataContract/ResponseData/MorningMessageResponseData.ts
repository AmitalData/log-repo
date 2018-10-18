import {ResponseDataBase} from './ResponseDataBase';

export class MorningMessageResponseData extends ResponseDataBase
{
    public MorningMessageList: Array<MorningMessageResult>;
}

export class MorningMessageResult {
    public MessageID: string;
    public Category : string;
    public CategoryName : string;
    public Subject : string;
    public Content : string;
    public MessageDate: string;



    public NeedExpandaple: boolean = false;

    public Toggle: boolean = false;
    public Indicator: string = "-";

    public ToggleIt() {
        this.Toggle = !this.Toggle;
        if (this.Toggle) {
            this.Indicator = "+++";
        } else {
            this.Indicator = "---";
        }
    }
}