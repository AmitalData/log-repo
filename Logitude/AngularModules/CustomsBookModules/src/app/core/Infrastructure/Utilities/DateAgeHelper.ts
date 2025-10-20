export class DateAgeHelper {
    public Years: number = 0;
    public Months: number = 0;
    public Days: number = 0;
    public Hours: number = 0;
    public Minutes: number = 0;
    public Seconds: number = 0;
    public Week: number = 0;

    constructor(date: Date) {
        if (!date) return;

        const now = new Date();
        const diff = now.getTime() - date.getTime();

        // Calculate time differences
        this.Seconds = Math.floor(diff / 1000);
        this.Minutes = Math.floor(this.Seconds / 60);
        this.Hours = Math.floor(this.Minutes / 60);
        this.Days = Math.floor(this.Hours / 24);
        this.Week = Math.floor(this.Days / 7);
        this.Months = Math.floor(this.Days / 30);
        this.Years = Math.floor(this.Days / 365);

        // Adjust for remaining time
        this.Seconds = this.Seconds % 60;
        this.Minutes = this.Minutes % 60;
        this.Hours = this.Hours % 24;
        this.Days = this.Days % 30;
        this.Week = this.Week % 4;
        this.Months = this.Months % 12;
    }
}

