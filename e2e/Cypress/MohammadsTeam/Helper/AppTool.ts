export class AppTool {
 
    public static IsNullOrEmpty(myFieldValue: any) {
        var myResult: boolean = false;

        if (myFieldValue == null || myFieldValue === undefined) {
            myResult = true;
        }

        else if (typeof (myFieldValue) == "string") {
            myFieldValue = myFieldValue.split(" ").join("");
            
            if (myFieldValue.trim().length == 0) {
                myResult = true;
            }
        }

        else if (typeof (myFieldValue) == "number") {
            if (isNaN(myFieldValue)) {
                myResult = true;
            }

            else if (myFieldValue.toString().trim().length == 0) {
                myResult = true;
            }
        }

        return myResult;
    }
    public static IsNullOrZero(myFieldValue: number) {
        var myResult: boolean = false;

        if (myFieldValue == null || myFieldValue === undefined) {
            myResult = true;
        }


        else if (myFieldValue.toString().trim().length == 0) {
            myResult = true;
        }

        else if (typeof (myFieldValue) == "number") {
            if (isNaN(myFieldValue)) {
                myResult = true;
            }

            else if (myFieldValue == 0) {
                myResult = true;
            }
        }

        return myResult;
    }
}