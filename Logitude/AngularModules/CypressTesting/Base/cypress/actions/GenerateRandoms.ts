export function GenerateRandomString(length: number, upperCase: boolean) {
    let randomString = ""
    let possible = "abcdefghijklmnopqrstuvwxyz"

    for (let i = 0; i < length; i++)
        randomString += possible.charAt(Math.floor(Math.random() * possible.length))

    if (upperCase) {
        randomString = randomString.toUpperCase()
    }

    return randomString
}

export function GenerateRandomNumber(minimum: number, maximum: number) {
    minimum = Math.ceil(minimum)
    maximum = Math.floor(maximum)

    return (Math.floor(Math.random() * (maximum - minimum + 1) + minimum))
}

export function GenerateRandomNumberAndString(length: number) {
    let randomString = ""
    let possible = "abcdefghijklmnopqrstuvwxyz0123456789"

    for (let i = 0; i < length; i++)
        randomString += possible.charAt(Math.floor(Math.random() * possible.length))

    return randomString
}

export function GetValidContainerNumber(input: any) {
    var myResult: string = "";

    var sum = 0;

    for (var i = 0; i < input.length - 1; i++) {
        if (IsAlpha(input[i])) {
            sum = sum + (GetCharCode(input[i]) * Math.pow(2, i));
        }

        else {
            sum = sum + (GetIntegerDigit(input[i]) * Math.pow(2, i));
        }
    }

    var integrSum = GetIntegerValue(sum);
    var checkDigit = GetIntegerDigit(input[input.length - 1]);

    var divisionby11: number = integrSum / 11;
    var erasedecimaldigits: number = GetIntegerValue(divisionby11);
    var multiplyby11 = erasedecimaldigits * 11;
    var validCheckDigit = (integrSum - multiplyby11);

    if (validCheckDigit == 10) {
        validCheckDigit = 0;
    }

    myResult = input.replace(/.$/, validCheckDigit.toString());

    return myResult;
}

export function GenerateCurrentDatetimeString(split: string): string {
    let currentDate = new Date();
    let currentDatetimeString = currentDate.getDate() + split + (currentDate.getMonth() + 1) + split + currentDate.getFullYear() + split + currentDate.getTime();
    return currentDatetimeString;
}

function IsAlpha(input: string): boolean {
    var myResult = true;

    if (input != null) {
        input = input.trim().toUpperCase();

        if (!input.match(/^[A-Z]*$/)) {
            myResult = false;
        }
    }

    return myResult;
}

function GetCharCode(c: any) {
    switch (c) {
        case 'A': return 10;
        case 'B': return 12;
        case 'C': return 13;
        case 'D': return 14;
        case 'E': return 15;
        case 'F': return 16;
        case 'G': return 17;
        case 'H': return 18;
        case 'I': return 19;
        case 'J': return 20;
        case 'K': return 21;
        case 'L': return 23;
        case 'M': return 24;
        case 'N': return 25;
        case 'O': return 26;
        case 'P': return 27;
        case 'Q': return 28;
        case 'R': return 29;
        case 'S': return 30;
        case 'T': return 31;
        case 'U': return 32;
        case 'V': return 34;
        case 'W': return 35;
        case 'X': return 36;
        case 'Y': return 37;
        case 'Z': return 38;
        default: return 0;
    }
}
function GetIntegerDigit(c: any) {
    switch (c) {
        case '1': return 1;
        case '2': return 2;
        case '3': return 3;
        case '4': return 4;
        case '5': return 5;
        case '6': return 6;
        case '7': return 7;
        case '8': return 8;
        case '9': return 9;
        default: return 0;
    }
}

function IsDecimal(input: string): boolean {
    var isDecimal = false;

    if (input != null) {
        input = input.trim();

        if (input.length > 0) {

            isDecimal = true;
            var isFirstDot = true;
            var isFirstminus = true;

            for (var i = 0; i < input.length; i++) {
                var letter = input[i];

                if (!letter.match(/^[0-9]/ig)) {
                    if (letter == '.') {
                        if (i == 0) {
                            isDecimal = false;
                            break;
                        }

                        else
                            if (isFirstDot) {
                                isFirstDot = false;
                                continue;
                            }

                            else {
                                isDecimal = false;
                                break;
                            }
                    }

                    else if (letter == '-') {
                        if (i != 0) {
                            isDecimal = false;
                            break;
                        }

                        else {
                            if (isFirstminus) {
                                isFirstminus = false;
                                continue;
                            }

                            else {
                                isDecimal = false;
                                break;
                            }
                        }
                    }

                    else {
                        isDecimal = false;
                        break;
                    }
                }
            }
        }
    }

    return isDecimal;
}

function GetIntegerValue(input: any): number {
    var myResult: number = 0;

    if (input != null) {
        var inputString = input + "";

        if (IsDecimal(inputString)) {
            myResult = +inputString.split(".")[0];
        }
    }

    return myResult;
}