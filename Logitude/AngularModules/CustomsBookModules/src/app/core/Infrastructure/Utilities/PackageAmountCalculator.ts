import { AppTool } from '../Tools';

export class PackageAmountCalculator {

    public static ComputeVolume(volume: number, quantity: number, width: number, height: number, length: number, weight: number, myRatio: number, dimentionCode: string, volumeCode: string, weightCode: string) {

        var output: number = volume;

        if (width && height && length && quantity) {
            output = this.GetVolumeFromDimentions(dimentionCode, volumeCode, width, height, length, quantity);
        }

        else if (weight) {
            if (AppTool.IsNullOrEmpty(output)) {
                output = this.GetVolumeFromWeight(weightCode, volumeCode, weight, myRatio);
            }
        }

        else {
            output = null;
        }

        if (output != null) {
            output = AppTool.Round(output, 3);
        }

        return output;
    }

    public static ComputeVolumetricWeight(volumetricWeight: number, volume: number, weight: number, ratio: number, volumeCode: string, weightCode: string, chargeableWeightCode: string) {

        var output: number = volumetricWeight;

        if (AppTool.IsNullOrEmpty(volume) && AppTool.IsNullOrEmpty(weight)) {
            output = null;
        }

        else {
            if (volume) {
                output = this.GetWeightFromVolume(volumeCode, chargeableWeightCode, volume, ratio);
            }

            else if (weight) {
                output = this.GetWeightFromWeight(weightCode, chargeableWeightCode, weight);
            }
        }

        if (output != null) {
            output = AppTool.Round(output, 3);
        }

        return output;
    }

    private static GetVolumeFromDimentions(dimentionCode: string, volumeCode: string, myWidth: number, myHeight: number, myLength: number, myQuantity: number) {

        var myResult: number = null;
        var factorOfConvert: number = 1;

        if (AppTool.IsNullOrEmpty(dimentionCode)) {
            dimentionCode = "CM";
        }

        if (AppTool.IsNullOrEmpty(volumeCode)) {
            volumeCode = "CBM";
        }

        if (dimentionCode.toUpperCase() == "INC" && volumeCode.toUpperCase() == "CBI") {
            myResult = myQuantity * (myWidth * myHeight * myLength);
        }

        else if (dimentionCode.toUpperCase() == "FT" && volumeCode.toUpperCase() == "CBF") {
            myResult = myQuantity * (myWidth * myHeight * myLength);
        }

        else {
            switch (dimentionCode.toUpperCase()) {
                case "CM": { factorOfConvert = 1; break; }
                case "INC": { factorOfConvert = 2.54; break; }      // 1 inch = 2.54 centimeters
                case "FT": { factorOfConvert = 30.48; break; }      // 1ft = 30.48 centimeters
            }

            var volumeInCentimeters: number = myQuantity * (myWidth * myHeight * myLength) * Math.pow(factorOfConvert, 3);
            var volumeInCBM: number = volumeInCentimeters / Math.pow(100, 3);

            switch (volumeCode.toUpperCase()) {
                case "CBM": { factorOfConvert = 1; break; }
                case "CBI": { factorOfConvert = 61024; break; }      // 1m³ = 61024in³
                case "CBF": { factorOfConvert = 35.315; break; }     // 1m³ = 35.315ft³
            }

            myResult = volumeInCBM * factorOfConvert;
        }

        if (myResult != null) {
            myResult = AppTool.Round(myResult, 3);
        }

        return myResult;
    }
    private static GetVolumeFromWeight(weightCode: string, volumeCode: string, myWeight: number, myRatio: number) {

        var myResult: number = null;
        var factorOfConvert: number = 1;

        if (myWeight != null) {
            if (AppTool.IsNullOrEmpty(weightCode)) {
                factorOfConvert = 1;
            }

            else {
                switch (weightCode.toUpperCase()) {
                    case "KG": { factorOfConvert = 1; break; }
                    case "LB": { factorOfConvert = 0.45359237; break; }     // 1 LB = 0.45359237 KG
                    case "MT": { factorOfConvert = 1000; break; }           // 1 mt = 1000 KG
                }
            }

            var weightInKilograms: number = weightInKilograms = myWeight * factorOfConvert;
            var volumeInCBM: number = (weightInKilograms * myRatio) / 1000;

            if (AppTool.IsNullOrEmpty(volumeCode)) {
                factorOfConvert = 1;
            }

            else {
                switch (volumeCode.toUpperCase()) {
                    case "CBM": { factorOfConvert = 1; break; }
                    case "CBI": { factorOfConvert = 61024; break; }      // 1m³ = 61024in³
                    case "CBF": { factorOfConvert = 35.315; break; }     // 1m³ = 35.315ft³
                }
            }

            myResult = volumeInCBM * factorOfConvert;
        }

        if (myResult != null) {
            myResult = AppTool.Round(myResult, 3);
        }

        return myResult;
    }
    private static GetWeightFromVolume(volumeCode: string, weightCode: string, volume: number, ratio: number) {

        var myResult: number = null;
        var factorOfConvert: number = 1;

        if (volume != null) {
            var volumeInCBM: number = null;
            var weightInKilograms: number = null;

            if (AppTool.IsNullOrEmpty(volumeCode)) {
                factorOfConvert = 1;
            }

            else {
                switch (volumeCode.toUpperCase()) {
                    case "CBM": { factorOfConvert = 1; break; }
                    case "CBI": { factorOfConvert = 61024; break; }      // 1m³ = 61024in³
                    case "CBF": { factorOfConvert = 35.315; break; }     // 1m³ = 35.315ft³
                }
            }

            volumeInCBM = volume / factorOfConvert;
            weightInKilograms = (volumeInCBM * 1000) / ratio;

            if (AppTool.IsNullOrEmpty(weightCode)) {
                factorOfConvert = 1;
            }

            else {
                switch (weightCode.toUpperCase()) {
                    case "KG": { factorOfConvert = 1; break; }
                    case "LB": { factorOfConvert = 0.45359237; break; }     // 1 LB = 0.45359237 KG
                    case "MT": { factorOfConvert = 1000; break; }           // 1 mt = 1000 KG
                }
            }

            myResult = weightInKilograms / factorOfConvert;
        }

        if (myResult != null) {
            myResult = AppTool.Round(myResult, 3);
        }

        return myResult;
    }

    private static GetWeightFromWeight(fromWeightCode: string, toWeightCode: string, myWeight: number) {

        var myResult: number = null;
        var factorOfConvert: number = 1;

        if (myWeight != null) {
            if (AppTool.IsNullOrEmpty(fromWeightCode)) {
                factorOfConvert = 1;
            }

            else {
                switch (fromWeightCode.toUpperCase()) {
                    case "KG": { factorOfConvert = 1; break; }
                    case "LB": { factorOfConvert = 0.45359237; break; }     // 1 LB = 0.45359237 KG
                    case "MT": { factorOfConvert = 1000; break; }           // 1 mt = 1000 KG
                }
            }

            var weightInKilograms: number = myWeight * factorOfConvert;

            if (AppTool.IsNullOrEmpty(toWeightCode)) {
                factorOfConvert = 1;
            }

            else {
                switch (toWeightCode.toUpperCase()) {
                    case "KG": { factorOfConvert = 1; break; }
                    case "LB": { factorOfConvert = 0.45359237; break; }     // 1 LB = 0.45359237 KG
                    case "MT": { factorOfConvert = 1000; break; }           // 1 mt = 1000 KG
                }
            }

            myResult = weightInKilograms / factorOfConvert;
        }

        if (myResult != null) {
            myResult = AppTool.Round(myResult, 3);
        }

        return myResult;
    }


}
