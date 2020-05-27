"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var LuhnAlgorithm = /** @class */ (function () {
    function LuhnAlgorithm() {
    }
    LuhnAlgorithm.CalculateLuhnAlgorithm = function (value) {
        var sum = 0;
        var d;
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
    };
    LuhnAlgorithm.ConvertReshimonToDeclartion = function (reshimonNumber, declarationConvertionDigits) {
        var checkDigit = 0;
        //check reshimon validity
        //9 digits
        if (reshimonNumber.length != 9) {
            return null;
        }
        var tmp = 
        //DateTime.Now.ToString("yy").Substring(0, 1)
        //2017 return 1 
        //1984 return 8 
        (new Date()).getUTCFullYear().toString().substr(2, 1)
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
    };
    LuhnAlgorithm.ConvertDeclartionToReshimon = function (declarationNumber) {
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
        var checkDigit = 0;
        var tmp = declarationNumber.substr(1, 1) + declarationNumber.substr(6, 7);
        checkDigit = LuhnAlgorithm.CalculateLuhnAlgorithm(tmp);
        tmp = tmp + checkDigit;
        return tmp;
    };
    return LuhnAlgorithm;
}());
exports.LuhnAlgorithm = LuhnAlgorithm;
//# sourceMappingURL=LuhnAlgorithm.js.map