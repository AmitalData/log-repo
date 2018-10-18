import {BookingPM} from '../EntityPMs/BookingPM';

export class BookingUtilities {

    public static IsBookingEditEnabled(entityPM: BookingPM): boolean {
        var myResult: boolean = true;

        if (entityPM.IsCancelled) {
            myResult = false;
        }

        else if (entityPM.BookingStatusCode != "CRT") {
            myResult = false;
        }

        return myResult;
    }    
}