

export class LuhnAlgorithm {


    public static CalculateLuhnAlgorithm(value: string) {
        var sum: number = 0;
        var d: number;
        for (var i = 0; i < value.toString().length; i++) {
            d = 0;
            d = +(value.toString().substring(i, i + 1));
            if (i % 2 != 0)
                d = d * 2;
            if (d > 9)
                d -= 9;
            sum += d;
        }

        if (sum % 10 == 0) {
            return 0;
        }
        else {
            var x = sum % 10;
            return 10 - x;
        }
    }



    public static ConvertReshimonToDeclartion(reshimonNumber: string, declarationConvertionDigits: string): string {
        let checkDigit: number = 0;

        //check reshimon validity
        //9 digits
        if (reshimonNumber.length != 9) {
            return null;
        }

        debugger;
        var year = Number( (new Date()).getUTCFullYear().toString().substr(2, 1));

        if ( reshimonNumber.substr(0, 1) > (new Date()).getUTCFullYear().toString().substr(3, 1)) {
            year = year - 1;
        }


        let tmp: string =
            //DateTime.Now.ToString("yy").Substring(0, 1)
            //2017 return 1 
            //1984 return 8 
            year
            +
            reshimonNumber.substr(0, 1)
            +
            declarationConvertionDigits
            + "00" +
            reshimonNumber.substr(1, 7);
        checkDigit = LuhnAlgorithm
            .CalculateLuhnAlgorithm(tmp.substr(5, 8));
        tmp = tmp + checkDigit;
        return tmp;
    }

    public static ConvertDeclartionToReshimon(declarationNumber: string): string {
        //check Declaration validity
        //14 digits
        if (declarationNumber.length != 14) {
            return null;
        }
        //check digits 3-4 is 98 or 99 according to DclarationType
        if (declarationNumber.substr(2, 2) != "98" &&
            declarationNumber.substr(2, 2) != "99") {
            return null;
        }

        let checkDigit: number = 0;
        let tmp: string = declarationNumber.substr(1, 1) + declarationNumber.substr(6, 7);
        checkDigit = LuhnAlgorithm.CalculateLuhnAlgorithm(tmp);
        tmp = tmp + checkDigit;

        return tmp;
    }


}
