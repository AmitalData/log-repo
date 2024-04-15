import { TextCodeTranslator } from './Utilities/TextCodeTranslator';
import { SessionLocator } from './Utilities/SessionLocator';

export class AppTool {
	public static GetCounterPrefixLength(prefix: string) {
		//[B]_[YYYY]_SH
		let prefixLength: number = 0;
		let currentVar = '';
		let allVariables = [];
		if (!AppTool.IsNullOrEmpty(prefix)) {
			for (var i = 0; i < prefix.length; i++) {
				let c = prefix.charAt(i);
				if (c == '[') {
					currentVar += c;
				} else if (c == ']') {
					currentVar += c;
					allVariables.push(currentVar);
					currentVar = '';
				} else if (!AppTool.IsNullOrEmpty(currentVar)) currentVar += c;
			}

			let prefixWithoutVars = prefix;
			allVariables.forEach((prefVar) => {
				if (prefVar == '[B]') prefixLength += 4;
				if (prefVar == '[YY]' || prefVar == '[MM]') prefixLength += 2;
				if (prefVar == '[YYYY]') prefixLength += 4;
			});

			allVariables.forEach((prefVar) => {
				if (prefVar == '[B]' || prefVar == '[YY]' || prefVar == '[MM]' || '[YYYY]') {
					prefixWithoutVars = prefixWithoutVars.replace(prefVar, '');
				}
			});

			prefixLength += prefixWithoutVars.length;
		}

		return prefixLength;
	}
	public static GetCounterResolvedNumber(prefix: string, startNumber: number, suffix: string, counterSize: number) {
		//[B]_[YYYY]_SH
		let calculatedNumber: string = startNumber ? startNumber.toString() : '';
		let calculatedPrefix: string = !AppTool.IsNullOrEmpty(prefix) ? prefix : '';
		let calculatedSuffix: string = !AppTool.IsNullOrEmpty(suffix) ? suffix : '';

		let currentDate: Date = new Date();
		var curr_date = currentDate.getDate();
		let curr_month: string = (currentDate.getMonth() + 1).toString(); //Months are zero based
		let mm = AppTool.PadLeft(curr_month, 2, '0');
		let yyyy = currentDate.getFullYear().toString();
		let yy = yyyy.substring(2, yyyy.length);

		if (!AppTool.IsNullOrEmpty(prefix)) {
			calculatedPrefix = calculatedPrefix.replace('[YYYY]', yyyy);
			calculatedPrefix = calculatedPrefix.replace('[YY]', yy);
			calculatedPrefix = calculatedPrefix.replace('[B]', 'BBBBB');
			calculatedPrefix = calculatedPrefix.replace('[MM]', mm);
		}

		if (!AppTool.IsNullOrEmpty(suffix)) {
			calculatedSuffix = calculatedSuffix.replace('[YYYY]', yyyy);
			calculatedSuffix = calculatedSuffix.replace('[YY]', yy);
			calculatedSuffix = calculatedSuffix.replace('[B]', 'BBBBB');
			calculatedSuffix = calculatedSuffix.replace('[MM]', mm);
		}

		let totalNumberLength = calculatedNumber.length;
		if (calculatedPrefix) totalNumberLength += calculatedPrefix.length;
		if (calculatedPrefix) totalNumberLength += calculatedSuffix.length;

		if (counterSize > totalNumberLength)
			calculatedNumber = AppTool.PadLeft(
				calculatedNumber,
				counterSize - totalNumberLength + calculatedNumber.length,
				'0',
			);

		calculatedNumber = calculatedPrefix + calculatedNumber + calculatedSuffix;

		return calculatedNumber;
	}

	public static TenantPM: any;
	public static IsNullOrEmpty(myFieldValue: any) {
		var myResult: boolean = false;

		if (myFieldValue == null || myFieldValue === undefined) {
			myResult = true;
		} else if (typeof myFieldValue == 'string') {
			myFieldValue = myFieldValue.split(' ').join('');

			if (myFieldValue.trim().length == 0) {
				myResult = true;
			}
		} else if (typeof myFieldValue == 'number') {
			if (isNaN(myFieldValue)) {
				myResult = true;
			} else if (myFieldValue.toString().trim().length == 0) {
				myResult = true;
			}
		}

		return myResult;
	}
	public static IsNullOrZero(myFieldValue: number) {
		var myResult: boolean = false;

		if (myFieldValue == null || myFieldValue === undefined) {
			myResult = true;
		} else if (myFieldValue.toString().trim().length == 0) {
			myResult = true;
		} else if (typeof myFieldValue == 'number') {
			if (isNaN(myFieldValue)) {
				myResult = true;
			} else if (myFieldValue == 0) {
				myResult = true;
			}
		}

		return myResult;
	}
	public static IsLCLEntity(myTransportModeId: string, myShipmentTypeId: string) {
		var myResult = false;

		if (myTransportModeId != null) {
			myTransportModeId = myTransportModeId.toUpperCase();
		}

		if (myShipmentTypeId != null) {
			myShipmentTypeId = myShipmentTypeId.toUpperCase();
		}

		if (myTransportModeId == 'A') {
			myResult = true;
		} else if (myTransportModeId == 'O' && myShipmentTypeId == 'LCLD') {
			myResult = true;
		} else if (myTransportModeId == 'I' && myShipmentTypeId == 'LTL') {
			myResult = true;
		}

		return myResult;
	}
	public static IsFCLEntity(myTransportModeId: string, myShipmentTypeId: string) {
		var myResult = false;

		if (myTransportModeId != null) {
			myTransportModeId = myTransportModeId.toUpperCase();
		}

		if (myShipmentTypeId != null) {
			myShipmentTypeId = myShipmentTypeId.toUpperCase();
		}

		if (myTransportModeId == 'O' && (myShipmentTypeId == 'FCLD' || myShipmentTypeId == 'MYGO')) {
			myResult = true;
		}

		if (myTransportModeId == 'I' && (myShipmentTypeId == 'FTL' || myShipmentTypeId == 'MYGI')) {
			myResult = true;
		}

		return myResult;
	}
	public static PadLeft(myString: string, myCount: number, myChar: string) {
		if (this.IsNullOrZero(myCount)) {
			myCount = 0;
		}

		if (this.IsNullOrEmpty(myChar)) {
			myChar = '';
		}

		if (this.IsNullOrEmpty(myString)) {
			myString = '';
		}

		while (myString.length < myCount) {
			myString = myChar + myString;
		}

		return myString;
	}
	public static PadRight(myString: string, myCount: number, myChar: string) {
		if (this.IsNullOrZero(myCount)) {
			myCount = 0;
		}

		if (this.IsNullOrEmpty(myChar)) {
			myChar = '';
		}

		if (this.IsNullOrEmpty(myString)) {
			myString = '';
		}

		while (myString.length < myCount) {
			myString = myString + myChar;
		}

		return myString;
	}
	public static Round(num, decimals) {
		var myResult: number = null;

		if (!this.IsNullOrEmpty(num)) {
			var sign = num >= 0 ? 1 : -1;
			var myResultString = (Math.round(num * Math.pow(10, decimals) + sign * 0.001) / Math.pow(10, decimals)).toFixed(
				decimals,
			);
			myResult = +myResultString;
		}

		return myResult;
	}
	public static ToNumber(currencyformat: string) {
		var myResult: number = null;

		if (!this.IsNullOrEmpty(currencyformat)) {
			let numberformat = currencyformat.toString().replace(/[^0-9.-]+/g, '');
			myResult = +numberformat;
		}

		return myResult;
	}

	public static GetNewGuid() {
		//http://stackoverflow.com/questions/26501688/a-typescript-guid-class

		return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
			var r = (Math.random() * 16) | 0,
				v = c == 'x' ? r : (r & 0x3) | 0x8;
			return v.toString(16);
		});
	}
	public static GetLogitudeURL() {
		var logitude_url = location.href.replace('index.html', '');

		if (location.href.indexOf('localhost') > -1) {
			logitude_url = 'http://localhost:9996/'; //test.logitudeworld.com/test/';//
		} else {
			var urlArr = location.href.split('/index.html');
			var url = urlArr[0];
			url = url.replace(url.substring(url.lastIndexOf('/'), url.length), '');
			logitude_url = url + '/';
		}

		return logitude_url;
	}

	public static GetQuickbooksPageBasedOnAuth() {
		SessionLocator.TenantManagementJS;
		var logitude_url = location.href.replace('index.html', '');

		if (location.href.indexOf('localhost') > -1) {
			logitude_url = 'http://localhost:9996/'; //test.logitudeworld.com/test/';//
		} else {
			var urlArr = location.href.split('/index.html');
			var url = urlArr[0];
			url = url.replace(url.substring(url.lastIndexOf('/'), url.length), '');
			logitude_url = url + '/';
		}

		return logitude_url;
	}

	public static GetComponentName(myComponentPath: string) {
		var myComponentName = null;

		if (!this.IsNullOrEmpty(myComponentPath)) {
			var urlParts: string[] = myComponentPath.split('/');
			myComponentName = urlParts[urlParts.length - 1];
		}

		return myComponentName;
	}

	public static GetLongMasterField(myTransportModeId: string, myAirlinePrefix: string, myMaster: string) {
		var myField: string = null;

		if (myTransportModeId == 'A') {
			if (!this.IsNullOrEmpty(myAirlinePrefix) && !this.IsNullOrEmpty(myMaster)) {
				myField = myAirlinePrefix + '-' + myMaster;
			}
		} else {
			myField = myMaster;
		}

		return myField;
	}
	public static IsAirlineRuleFieldValid(myRule: any, myFieldValue: any): boolean {
		var myResult = true;

		if (myRule != null) {
			if (myFieldValue == null || isNaN(myFieldValue)) {
				if (myRule.IsMandatoryForSending) {
					myResult = false;
				}
			} else if (typeof myFieldValue == 'string') {
				if (this.IsNullOrEmpty(myFieldValue)) {
					if (myRule.IsMandatoryForSending) {
						myResult = false;
					}
				} else if (myRule.MaxSize > 0) {
					if (myFieldValue.length > myRule.MaxSize) {
						myResult = false;
					}
				}
			} else if (typeof myFieldValue == 'number') {
				if (this.IsNullOrZero(myFieldValue)) {
					if (myRule.IsMandatoryForSending) {
						myResult = false;
					}
				}
			}
		}

		return myResult;
	}
	public static ValidateMasterField(
		myMasterField: string,
		myTransportModeId: string,
		isCheckDigit: boolean,
		isLimitedLength: boolean,
	) {
		var myResult = null;

		if (!this.IsNullOrEmpty(myMasterField) && myTransportModeId == 'A') {
			if (!FormatTool.IsNumeric(myMasterField)) {
				myResult = 'Master Field must be all digits';
			} else {
				if (isLimitedLength && myMasterField.length != 8) {
					myResult = 'Master Field length must be 8 digits';
				} else {
					if (isCheckDigit) {
						var myPrefix: string = myMasterField.substr(0, 7);
						var myCheckDegit: string = myMasterField.substr(7, 1);

						var myPrefixInteger: number = +myPrefix;

						var myMod: number = myPrefixInteger % 7;

						if (myMod >= 7) {
							myMod = myMod % 7;
						}

						if (myMod.toString() != myCheckDegit) {
							myResult = 'Master Field invalid check digit';
						}
					}
				}
			}
		}

		return myResult;
	}

	// Measurments
	public static GetWeightFromWeight(fromWeightCode: string, toWeightCode: string, myWeight: number) {
		var myResult: number = null;
		var factorOfConvert: number = 1;

		if (myWeight != null) {
			if (this.IsNullOrEmpty(fromWeightCode)) {
				factorOfConvert = 1;
			} else {
				switch (fromWeightCode.toUpperCase()) {
					case 'KG': {
						factorOfConvert = 1;
						break;
					}
					case 'LB': {
						factorOfConvert = 0.45359237;
						break;
					} // 1 LB = 0.45359237 KG
					case 'MT': {
						factorOfConvert = 1000;
						break;
					} // 1 mt = 1000 KG
				}
			}

			var weightInKilograms: number = myWeight * factorOfConvert;

			if (this.IsNullOrEmpty(toWeightCode)) {
				factorOfConvert = 1;
			} else {
				switch (toWeightCode.toUpperCase()) {
					case 'KG': {
						factorOfConvert = 1;
						break;
					}
					case 'LB': {
						factorOfConvert = 0.45359237;
						break;
					} // 1 LB = 0.45359237 KG
					case 'MT': {
						factorOfConvert = 1000;
						break;
					} // 1 mt = 1000 KG
				}
			}

			myResult = weightInKilograms / factorOfConvert;
		}

		if (myResult != null) {
			myResult = this.Round(myResult, 3);
		}

		return myResult;
	}
	public static GetWeightFromVolume(volumeCode: string, weightCode: string, myVolume: number, myRatio: number) {
		var myResult: number = null;
		var factorOfConvert: number = 1;

		if (myVolume != null) {
			var volumeInCBM: number = null;
			var weightInKilograms: number = null;

			if (this.IsNullOrEmpty(volumeCode)) {
				factorOfConvert = 1;
			} else {
				switch (volumeCode.toUpperCase()) {
					case 'CBM': {
						factorOfConvert = 1;
						break;
					}
					case 'CBI': {
						factorOfConvert = 61024;
						break;
					} // 1m³ = 61024in³
					case 'CBF': {
						factorOfConvert = 35.315;
						break;
					} // 1m³ = 35.315ft³
				}
			}

			volumeInCBM = myVolume / factorOfConvert;
			weightInKilograms = (volumeInCBM * 1000) / myRatio;

			if (this.IsNullOrEmpty(weightCode)) {
				factorOfConvert = 1;
			} else {
				switch (weightCode.toUpperCase()) {
					case 'KG': {
						factorOfConvert = 1;
						break;
					}
					case 'LB': {
						factorOfConvert = 0.45359237;
						break;
					} // 1 LB = 0.45359237 KG
					case 'MT': {
						factorOfConvert = 1000;
						break;
					} // 1 mt = 1000 KG
				}
			}

			myResult = weightInKilograms / factorOfConvert;
		}

		if (myResult != null) {
			myResult = this.Round(myResult, 3);
		}

		return myResult;
	}
	public static GetWeightFromDimentions(
		myWidth: number,
		myHeight: number,
		myLength: number,
		myQuantity: number,
		dimentionCode: string,
		weightCode: string,
		myRatio: number,
	) {
		var myResult: number = null;
		var factorOfConvert: number = 1;

		if (this.IsNullOrEmpty(dimentionCode)) {
			factorOfConvert = 1;
		} else {
			switch (dimentionCode.toUpperCase()) {
				case 'CM': {
					factorOfConvert = 1;
					break;
				}
				case 'INC': {
					factorOfConvert = 2.54;
					break;
				} // 1 inch = 2.54 centimeters
				case 'FT': {
					factorOfConvert = 30.48;
					break;
				} // 1ft = 30.48 centimeters
			}
		}

		var volumeInCentimeters: number = myQuantity * (myWidth * myHeight * myLength) * Math.pow(factorOfConvert, 3);
		var volumeInCBM: number = volumeInCentimeters / Math.pow(100, 3);

		var weightInKilograms: number = (volumeInCBM * 1000) / myRatio;

		if (this.IsNullOrEmpty(weightCode)) {
			factorOfConvert = 1;
		} else {
			switch (weightCode.toUpperCase()) {
				case 'KG': {
					factorOfConvert = 1;
					break;
				}
				case 'LB': {
					factorOfConvert = 0.45359237;
					break;
				} // 1 LB = 0.45359237 KG
				case 'MT': {
					factorOfConvert = 1000;
					break;
				} // 1 mt = 1000 KG
			}
		}

		myResult = weightInKilograms / factorOfConvert;

		if (myResult != null) {
			myResult = this.Round(myResult, 3);
		}

		return myResult;
	}
	public static GetRatio(directionId: string, transportModeId: string, shipmentTypeId: string, countryCode: string) {
		var myResult: number = null;

		if (!this.IsNullOrEmpty(countryCode)) {
			if (countryCode.toUpperCase() == 'US') {
				if (directionId == 'D') {
					if (transportModeId == 'A') {
						myResult = 7;
					} else if (transportModeId == 'I') {
						if (shipmentTypeId == 'LTL') {
							myResult = 9;
						}
					}
				}
			}
		}

		if (myResult == null) {
			{
				switch (transportModeId) {
					case 'A': {
						myResult = SessionLocator.TenantPM.AirRatio;
						break;
					}

					case 'O': {
						if (shipmentTypeId == 'FCL' || shipmentTypeId == 'FCLD') {
							myResult = SessionLocator.TenantPM.FCLRatio;
						} else {
							myResult = SessionLocator.TenantPM.LCLRatio;
						}

						break;
					}

					case 'I': {
						if (shipmentTypeId == 'FTL') {
							myResult = SessionLocator.TenantPM.FTLRatio;
						} else {
							myResult = SessionLocator.TenantPM.LTLRatio;
						}

						break;
					}
				}
			}
		}

		return myResult;
	}
	public static GetPickupDeliveryRatio(shipmentTypeId: string) {
		var myResult: number = null;
		switch (shipmentTypeId != null && shipmentTypeId.toLowerCase()) {
			case 'air':
			case 'lcl':
			case 'lcld':
			case 'ltl': {
				myResult = SessionLocator.TenantPM.LTLRatio;
				break;
			}
			case 'ftl':
			case 'fcl':
			case 'fcld': {
				myResult = SessionLocator.TenantPM.FTLRatio;
				break;
			}
			default:
				break;
		}
		return myResult;
	}

	public static GetRatioFromDimFactor(myDimFactor: number, dimentionCode: string, weightCode: string) {
		var myResult: number = null;

		if (myDimFactor != null) {
			var WeightFactorOfConvert: number = 1;
			var DimensiosFactorOfConvert: number = 1;

			if (!this.IsNullOrEmpty(weightCode)) {
				switch (weightCode.toUpperCase()) {
					case 'KG': {
						WeightFactorOfConvert = 1;
						break;
					}
					case 'LB': {
						WeightFactorOfConvert = 0.45359237;
						break;
					}
					case 'MT': {
						WeightFactorOfConvert = 1000;
						break;
					}
				}
			}

			if (!this.IsNullOrEmpty(dimentionCode)) {
				switch (dimentionCode.toUpperCase()) {
					case 'CM': {
						DimensiosFactorOfConvert = 1;
						break;
					}
					case 'INC': {
						DimensiosFactorOfConvert = 2.54;
						break;
					}
					case 'FT': {
						DimensiosFactorOfConvert = 30.48;
						break;
					}
				}
			}

			myResult = (myDimFactor * Math.pow(DimensiosFactorOfConvert, 3)) / (WeightFactorOfConvert * 1000);
		}

		return myResult;
	}
	public static GetDimFactorFromRatio(myRatio: number, dimentionCode: string, weightCode: string) {
		var myResult: number = null;

		if (myRatio != null) {
			var WeightFactorOfConvert: number = 1;
			var DimensiosFactorOfConvert: number = 1;

			if (!this.IsNullOrEmpty(weightCode)) {
				switch (weightCode.toUpperCase()) {
					case 'KG': {
						WeightFactorOfConvert = 1;
						break;
					}
					case 'LB': {
						WeightFactorOfConvert = 0.45359237;
						break;
					}
					case 'MT': {
						WeightFactorOfConvert = 1000;
						break;
					}
				}
			}

			if (!this.IsNullOrEmpty(dimentionCode)) {
				switch (dimentionCode.toUpperCase()) {
					case 'CM': {
						DimensiosFactorOfConvert = 1;
						break;
					}
					case 'INC': {
						DimensiosFactorOfConvert = 2.54;
						break;
					}
					case 'FT': {
						DimensiosFactorOfConvert = 30.48;
						break;
					}
				}
			}

			myResult = (WeightFactorOfConvert * 1000 * myRatio) / Math.pow(DimensiosFactorOfConvert, 3);
		}

		if (myResult != null) {
			var toString: string = myResult.toString();
			var myArray: string[] = toString.split('.');

			if (myArray.length > 1) {
				var strDigits: string = '0.' + myArray[1];
				var digits: number = +strDigits;

				if (digits < 0.5) {
					myResult = Math.floor(myResult);
				} else {
					myResult = Math.ceil(myResult);
				}
			}
		}

		return myResult;
	}
	public static GetVolumeFromWeight(weightCode: string, volumeCode: string, myWeight: number, myRatio: number) {
		var myResult: number = null;
		var factorOfConvert: number = 1;

		if (myWeight != null) {
			if (this.IsNullOrEmpty(weightCode)) {
				factorOfConvert = 1;
			} else {
				switch (weightCode.toUpperCase()) {
					case 'KG': {
						factorOfConvert = 1;
						break;
					}
					case 'LB': {
						factorOfConvert = 0.45359237;
						break;
					} // 1 LB = 0.45359237 KG
					case 'MT': {
						factorOfConvert = 1000;
						break;
					} // 1 mt = 1000 KG
				}
			}

			var weightInKilograms: number = (weightInKilograms = myWeight * factorOfConvert);
			var volumeInCBM: number = (weightInKilograms * myRatio) / 1000;

			if (this.IsNullOrEmpty(volumeCode)) {
				factorOfConvert = 1;
			} else {
				switch (volumeCode.toUpperCase()) {
					case 'CBM': {
						factorOfConvert = 1;
						break;
					}
					case 'CBI': {
						factorOfConvert = 61024;
						break;
					} // 1m³ = 61024in³
					case 'CBF': {
						factorOfConvert = 35.315;
						break;
					} // 1m³ = 35.315ft³
				}
			}

			myResult = volumeInCBM * factorOfConvert;
		}

		if (myResult != null) {
			myResult = this.Round(myResult, 3);
		}

		return myResult;
	}
	public static GetVolumeFromDimentions(
		dimentionCode: string,
		volumeCode: string,
		myWidth: number,
		myHeight: number,
		myLength: number,
		myQuantity: number,
	) {
		var myResult: number = null;
		var factorOfConvert: number = 1;

		if (this.IsNullOrEmpty(dimentionCode)) {
			dimentionCode = 'CM';
		}

		if (this.IsNullOrEmpty(volumeCode)) {
			volumeCode = 'CBM';
		}

		if (dimentionCode.toUpperCase() == 'INC' && volumeCode.toUpperCase() == 'CBI') {
			myResult = myQuantity * (myWidth * myHeight * myLength);
		} else if (dimentionCode.toUpperCase() == 'FT' && volumeCode.toUpperCase() == 'CBF') {
			myResult = myQuantity * (myWidth * myHeight * myLength);
		} else {
			switch (dimentionCode.toUpperCase()) {
				case 'CM': {
					factorOfConvert = 1;
					break;
				}
				case 'INC': {
					factorOfConvert = 2.54;
					break;
				} // 1 inch = 2.54 centimeters
				case 'FT': {
					factorOfConvert = 30.48;
					break;
				} // 1ft = 30.48 centimeters
			}

			var volumeInCentimeters: number = myQuantity * (myWidth * myHeight * myLength) * Math.pow(factorOfConvert, 3);
			var volumeInCBM: number = volumeInCentimeters / Math.pow(100, 3);

			switch (volumeCode.toUpperCase()) {
				case 'CBM': {
					factorOfConvert = 1;
					break;
				}
				case 'CBI': {
					factorOfConvert = 61024;
					break;
				} // 1m³ = 61024in³
				case 'CBF': {
					factorOfConvert = 35.315;
					break;
				} // 1m³ = 35.315ft³
			}

			myResult = volumeInCBM * factorOfConvert;
		}

		if (myResult != null) {
			myResult = this.Round(myResult, 3);
		}

		return myResult;
	}
	public static GetDimentionsCodeFromVolumeCode(volumeCode: string) {
		var myResult: string = null;

		if (volumeCode) {
			switch (volumeCode.toUpperCase()) {
				case 'CBM': {
					myResult = 'Cm';
					break;
				}
				case 'CBF': {
					myResult = 'Ft';
					break;
				}
				case 'CBI': {
					myResult = 'Inc';
					break;
				}
			}
		}

		return myResult;
	}
	public static ComputePackageVolume(
		myQuantity: number,
		myWidth: number,
		myHeight: number,
		myLength: number,
		myWeight: number,
		myRatio: number,
		dimentionCode: string,
		volumeCode: string,
		fromWeightCode: string,
	) {
		var myResult: number = null;

		if (myWidth == null || myHeight == null || myLength == null || myQuantity == null) {
			if (myWeight != null) {
				myResult = this.GetVolumeFromWeight(fromWeightCode, volumeCode, myWeight, myRatio);
			}
		} else {
			myResult = this.GetVolumeFromDimentions(dimentionCode, volumeCode, myWidth, myHeight, myLength, myQuantity);
		}

		if (myResult != null) {
			myResult = this.Round(myResult, 3);
		}

		return myResult;
	}
	public static ComputePackageVolumetricWeight(
		myQuantity: number,
		myWidth: number,
		myHeight: number,
		myLength: number,
		myVolume: number,
		myWeight: number,
		myRatio: number,
		dimentionCode: string,
		volumeCode: string,
		grossWeightCode: string,
		chargeableWeightCode: string,
	) {
		var myResult: number = null;

		if (myWidth == null || myHeight == null || myLength == null || myQuantity == null) {
			if (myVolume != null) {
				myResult = this.GetWeightFromVolume(volumeCode, chargeableWeightCode, myVolume, myRatio);
			} else if (myWeight != null) {
				myResult = this.GetWeightFromWeight(grossWeightCode, chargeableWeightCode, myWeight);
			}
		} else {
			myResult = this.GetWeightFromDimentions(
				myWidth,
				myHeight,
				myLength,
				myQuantity,
				dimentionCode,
				chargeableWeightCode,
				myRatio,
			);
		}

		if (myResult != null) {
			myResult = this.Round(myResult, 3);
		}

		return myResult;
	}
	public static CalculateChargeableWeight(
		myGrossWeight: number,
		myVolumetricWeight: number,
		grossWeightUnitCode: string,
		chargeableWeightUnitCode: string,
		directionId: string,
		transportModeId: string,
	) {
		var myResult: number = null;

		if (myGrossWeight != null || myVolumetricWeight != null) {
			var grossWeightInVolumetricUnit: number = this.GetWeightFromWeight(
				grossWeightUnitCode,
				chargeableWeightUnitCode,
				myGrossWeight,
			);

			if (grossWeightInVolumetricUnit != null && myVolumetricWeight == null) {
				myResult = grossWeightInVolumetricUnit;
			}

			if (grossWeightInVolumetricUnit == null && myVolumetricWeight != null) {
				myResult = myVolumetricWeight;
			}

			if (grossWeightInVolumetricUnit != null && myVolumetricWeight != null) {
				myResult = grossWeightInVolumetricUnit > myVolumetricWeight ? grossWeightInVolumetricUnit : myVolumetricWeight;
			}
		}

		if (myResult != null) {
			myResult = this.RoundChargeableWeight(myResult, chargeableWeightUnitCode, directionId, transportModeId);
		}

		return myResult;
	}
	public static RoundChargeableWeight(
		myArgs: number,
		chargeableWeightUnitCode: string,
		directionId: string,
		transportModeId: string,
	) {
		var result: number = myArgs;

		if (chargeableWeightUnitCode != 'MT') {
			if (directionId == 'E' && transportModeId == 'A') {
				if (result != null) {
					var toString: string = result.toString();
					var r: string[] = toString.split('.');

					if (r.length > 1) {
						var strDigits: string = '0.' + r[1];
						var digits: number = +strDigits;
						var integer: number = +r[0];

						if (digits <= 0.5) {
							result = integer + 0.5;
						} else {
							result = integer + 1;
						}
					}
				}
			}
		}

		return result;
	}
	public static GetChargeableWeightUnitCode(myTransportModeId: string) {
		var myResult: string;

		if (myTransportModeId == 'A') {
			myResult = this.TenantPM.ChargeableWeightUnitCode;
		} else if (myTransportModeId == 'O' || myTransportModeId == 'I') {
			myResult = this.TenantPM.WeightMeasurementUnitCode;
		}

		return myResult;
	}

	public static DeepCopy(oldObj: any) {
		var newObj = oldObj;
		if (oldObj && typeof oldObj === 'object') {
			if (Object.prototype.toString.call(oldObj) === '[object Array]') {
				newObj = [];
			} else {
				newObj = Object.create(oldObj);
			}

			for (var i in oldObj) {
				newObj[i] = this.DeepCopy(oldObj[i]);
			}
		}

		return newObj;
	}
	public static CreateCopy(dataContext: any, fields: string[]) {
		//var copy = Object.create(dataContext);
		var copy: any = {};
		if (dataContext != null && fields != null) {
			for (var i in fields) {
				var fieldName = fields[i];
				var fieldValue = dataContext[fieldName];
				copy[fieldName] = fieldValue;
			}
		}

		return copy;
	}
	public static RejectChanges(dataContext: any, fields: any) {
		if (dataContext != null && fields != null) {
			for (var i in fields) {
				dataContext[i] = fields[i];
			}
		}
	}
	public static GetEnvironmentLogo(myLogoCode: string) {
		var myResult = null;
		var LogoURL = window.sessionStorage.getItem('LogoURL');
		if (myLogoCode == 'C.R.M') {
			myResult = './Images/ApplicationLogo/CRMLogo.png';
		} else if (myLogoCode == 'A.N.G') {
			myResult = './Images/ApplicationLogo/Angular/AngularLogo.png';
		} else if (myLogoCode == 'U.N.I') {
			myResult = './Images/ApplicationLogo/UnifreightLogo.jpg';
		} else if (myLogoCode == 'L.O.B') {
			if (LogoURL) {
				myResult = LogoURL;
			} else {
				myResult = './Images/ApplicationLogo/LogBox.png';
			}
		} else {
			myResult = './Images/ApplicationLogo/LogitudeLogo.jpg';
		}

		return myResult;
	}
	public static GetTextWidth(myString: string, fontSize: number = 12) {
		var myResult: number = 0;

		if (!this.IsNullOrEmpty(myString)) {
			var canvas = document.createElement('canvas');
			var ctx = canvas.getContext('2d');
			ctx.font = fontSize + 'px Lucida Sans Unicode';
			myResult = ctx.measureText(myString).width;
		}

		return myResult;
	}
	public static AddAmounts(amount1: number, amount2) {
		var myResult: number = 0;

		if (!this.IsNullOrEmpty(amount1)) {
			myResult += amount1;
		}

		if (!this.IsNullOrEmpty(amount2)) {
			myResult += amount2;
		}

		return myResult;
	}
	public static GetIdsArrayText(array: string[]) {
		var myResult: string = null;

		if (array) {
			if (array.length > 0) {
				array.forEach((item) => {
					if (this.IsNullOrEmpty(myResult)) {
						myResult = item;
					} else {
						myResult += '.' + item;
					}
				});
			}
		}

		return myResult;
	}
	public static GetMainMenuIconCode(Code) {
		var myResult: string = null;

		switch (Code) {
			case 'General.MH.Occasions': {
				myResult = 'Occasion';
				break;
			}

			case 'General.MH.Operations': {
				myResult = 'Box';
				break;
			}
			case 'General.MH.ContainersFU': {
				myResult = 'FollowUps';
				break;
			}

			case 'General.MH.Quotes': {
				myResult = 'Quote';
				break;
			}

			case 'General.MH.SharedLogistics': {
				myResult = 'LogBoxIcon';
				break;
			}

			case 'General.MH.CRM':
			case 'General.MH.Dashboard':
			case 'General.MH.Importers': {
				myResult = 'Bars';
				break;
			}

			case 'General.MH.FullAccounting':
			case 'General.MH.Accounting': {
				myResult = 'Dollar';
				break;
			}

			case 'General.MH.Contacts': {
				myResult = 'Contacts';
				break;
			}
			case 'General.MH.Social': {
				myResult = 'Social';
				break;
			}

			case 'General.MH.Maintenance': {
				myResult = 'Maintenance';
				break;
			}

			case 'General.MH.Reports': {
				myResult = 'Table';
				break;
			}

			case 'General.MH.GettingStarted': {
				myResult = 'GettingStarted';
				break;
			}

			case 'General.MH.ActivationWizard': {
				myResult = 'Box';
				break;
			}
			case 'General.MH.Ticket': {
				myResult = 'Tickets';
				break;
			}

			case 'General.MH.TimeManagement': {
				myResult = 'Time';
				break;
			}

			case 'General.MH.FilingInbox': {
				myResult = 'Inbox';
				break;
			}

			case 'General.MH.CrossDocks': {
				myResult = 'CrossDocks';
				break;
			}

			case 'General.MH.Documents':
			case 'General.MH.DocumentsFiling': {
				myResult = 'Documents';
				break;
			}

			case 'General.MH.Depositions': {
				myResult = 'Deposition';
				break;
			}

			case 'General.MH.TariffModule': {
				myResult = 'Tariff';
				break;
			}

			default: {
				myResult = 'Person';
				break;
			}
		}

		return myResult;
	}
	public static Replace(myString: string, replacedText: string, replaceWith: string) {
		if (!this.IsNullOrEmpty(myString)) {
			myString = myString.split(replacedText).join(replaceWith);
		}

		return myString;
	}

	public static KillEventEmitter(event: any) {
		if (event) {
			var isStopped: boolean = event['isStopped'];

			if (isStopped == false) {
				event.unsubscribe();
			}

			event = null;
		}
	}
	public static GetActivityImageSrc(code: string) {
		var ImageSrc = '';

		switch (code) {
			case 'TS': {
				ImageSrc = './Images/Buttons/TS.png';
				break;
			}
			case 'VM': {
				ImageSrc = './Images/Buttons/VM.png';
				break;
			}
			case 'AP': {
				ImageSrc = './Images/Buttons/AP.png';
				break;
			}
			case 'CL': {
				ImageSrc = './Images/Buttons/CL.png';
				break;
			}
			case 'EI': {
				ImageSrc = './Images/Buttons/EI.png';
				break;
			}
			case 'EO': {
				ImageSrc = './Images/Buttons/EO.png';
				break;
			}
		}

		return ImageSrc;
	}
	public static IsMobileDetected() {
		if (
			navigator.userAgent.match(/Android/i) ||
			navigator.userAgent.match(/webOS/i) ||
			navigator.userAgent.match(/iPhone/i) ||
			navigator.userAgent.match(/iPad/i) ||
			navigator.userAgent.match(/iPod/i) ||
			navigator.userAgent.match(/BlackBerry/i) ||
			navigator.userAgent.match(/Windows Phone/i)
		) {
			return true;
		} else {
			return false;
		}
	}

	public static ReplaceDecimalSeparatorWithADot(value: string, decimalSeparator: string) {
		if (decimalSeparator != '.') {
			value = value.split(decimalSeparator).join('.');
		}
		return value;
	}

	public static RemoveThousandsSeparator(value: string, thousandsSeparator: string) {
		if ((value + '').indexOf(thousandsSeparator) > -1) {
			value = value.split(thousandsSeparator).join('');
		}
		return value;
	}

	public static GetNumberFromText(numberText: string) {
		var decimalSeparator: string;
		var thousandsSeparator: string;
		switch (SessionLocator.TenantPM.NumberFormatCode) {
			case 'CD': {
				thousandsSeparator = ',';
				decimalSeparator = '.';
				break;
			}

			case 'DC': {
				thousandsSeparator = '.';
				decimalSeparator = ',';
				break;
			}

			case 'AD': {
				thousandsSeparator = "'";
				decimalSeparator = '.';
				break;
			}

			default: {
				thousandsSeparator = ',';
				decimalSeparator = '.';
				break;
			}
		}
		var numberValue = NaN;
		if (!AppTool.IsNullOrEmpty(numberText)) {
			numberText = this.RemoveThousandsSeparator(numberText, thousandsSeparator);
			numberText = this.ReplaceDecimalSeparatorWithADot(numberText, decimalSeparator);
			numberValue = Number(numberText);
		}
		return numberValue;
	}
}
export class DateTool {
	public static ActualDateMessage = "Can't set Field to future date";

	// Get: Month + 1
	// Set: Month - 1
	// new Date(we need to pass exact month)
	// http://stackoverflow.com/questions/34546447/bind-an-input-with-type-datetime-local-to-a-date-property-in-angular-2
	// http://praveenlobo.com/blog/how-to-convert-javascript-local-date-to-utc-and-utc-to-local-date/
	// http://stackoverflow.com/questions/5619202/converting-string-to-date-in-js

	public static TenantPM: any;
	public static GetCurrentDateAsUtc() {
		var myResult: Date = new Date();
		myResult.setUTCFullYear(myResult.getFullYear());
		myResult.setUTCMonth(myResult.getMonth());
		myResult.setUTCDate(myResult.getDate());
		myResult.setUTCHours(0);
		myResult.setUTCMinutes(0);
		myResult.setUTCSeconds(0);
		myResult.setUTCMilliseconds(0);
		return myResult;
	}
	public static GetCurrentDateAsUtcForAccountingValidation() {
		var myResult: Date = new Date();
		myResult.setUTCFullYear(myResult.getUTCFullYear());
		myResult.setUTCMonth(myResult.getUTCMonth());
		myResult.setUTCDate(myResult.getUTCDate());
		myResult.setUTCHours(0);
		myResult.setUTCMinutes(0);
		myResult.setUTCSeconds(0);
		myResult.setUTCMilliseconds(0);
		return myResult;
	}
	public static GetCurrentDateTimeAsUtc() {
		var myResult: Date = new Date();
		myResult.setUTCFullYear(myResult.getFullYear());
		myResult.setUTCMonth(myResult.getMonth());
		myResult.setUTCDate(myResult.getDate());
		myResult.setUTCHours(myResult.getHours());
		myResult.setUTCMinutes(myResult.getMinutes());
		myResult.setUTCSeconds(myResult.getSeconds());
		myResult.setUTCMilliseconds(myResult.getMilliseconds());
		return myResult;
	}
	public static GetDateByDay(day: number) {
		var myResult: Date = new Date();
		myResult.setUTCMilliseconds(0);
		myResult.setUTCSeconds(0);
		myResult.setUTCMinutes(0);
		myResult.setUTCHours(0);
		myResult.setUTCDate(myResult.getDate() + day);
		myResult.setUTCMonth(myResult.getMonth());
		myResult.setUTCFullYear(myResult.getFullYear());
		return myResult;
	}
	public static GetDateByMonth(month: number) {
		var myResult: Date = new Date();

		var myResultDays = myResult.getDate();
		var myResultMonth = myResult.getMonth() + month;
		var myResultFullYear = myResult.getFullYear();
		if (myResultDays > 28) {
			var requestedMonthMaxDays = new Date(myResultFullYear, myResultMonth + 1, 0).getDate();
			if (requestedMonthMaxDays < myResultDays) {
				myResultDays = requestedMonthMaxDays;
			}
		}

		myResult.setUTCMilliseconds(0);
		myResult.setUTCSeconds(0);
		myResult.setUTCMinutes(0);
		myResult.setUTCHours(0);
		myResult.setUTCDate(myResultDays);
		myResult.setUTCMonth(myResultMonth);
		myResult.setUTCFullYear(myResultFullYear);
		return myResult;
	}
	public static GetStartOfTheWeek(date: Date) {
		var myResult: Date = null;

		if (date != null) {
			date = new Date(date.toString());
			var day = date.getDay() + 1;
			var diff = date.getDate() - day + (day == 0 ? -6 : 1);

			myResult = new Date(date.setDate(diff));
		}

		return myResult;
	}
	public static GetDateFromDate(givenDate: Date, isDateOnly: boolean = false) {
		var myResult: Date = null;

		if (!AppTool.IsNullOrEmpty(givenDate)) {
			if (typeof givenDate == 'string') {
				var str: string = givenDate + '';

				//console.log("toDateString:" + new Date(str).toDateString());
				//console.log("toISOString:" + new Date(str).toISOString());
				//console.log("toLocaleDateString:" + new Date(str).toLocaleDateString());
				//console.log("toUTCString:" + new Date(str).toUTCString());
				//givenDate = new Date(Date.parse(str));
				givenDate = new Date(str);
				//givenDate.setHours(givenDate.getHours() + 2);
			}

			myResult = new Date();
			myResult.setUTCFullYear(givenDate.getUTCFullYear());
			myResult.setUTCMonth(givenDate.getUTCMonth());
			myResult.setUTCDate(givenDate.getUTCDate());

			if (isDateOnly) {
				myResult.setUTCHours(0);
				myResult.setUTCMinutes(0);
				myResult.setUTCSeconds(0);
				myResult.setUTCMilliseconds(0);
			} else {
				myResult.setUTCHours(givenDate.getUTCHours());
				myResult.setUTCMinutes(givenDate.getUTCMinutes());
				myResult.setUTCSeconds(givenDate.getUTCSeconds());
				myResult.setUTCMilliseconds(givenDate.getUTCMilliseconds());
			}
		}

		return myResult;
	}
	public static TruncateTime(date: Date, setActual: boolean = false) {
		var myResult: Date = null;

		if (date != null) {
			var myResult: Date = this.GetDateParts(date).DateObject;
			if (setActual) {
				myResult.setHours(0);
				myResult.setMinutes(0);
				myResult.setSeconds(0);
				myResult.setMilliseconds(0);
			} else {
				myResult.setUTCHours(0);
				myResult.setUTCMinutes(0);
				myResult.setUTCSeconds(0);
				myResult.setUTCMilliseconds(0);
			}
		}

		return myResult;
	}
	public static AddHours(myDate: Date, hours: number) {
		var myResult = this.GetDateParts(myDate).DateObject;

		if (myResult != null) {
			myResult.setUTCHours(myResult.getHours() + hours);
		}

		return myResult;
	}
	public static AddDays(input: Date, days: number) {
		if (days) {
			if (input) {
				var output: Date = null;
				var dateParts = this.GetDateParts(input);

				if (typeof input == 'string') {
					output = dateParts.DateObject;
					output.setUTCDate(output.getDate() + days);
				} else {
					output = new Date();
					output.setUTCMonth(0);
					output.setUTCDate(1);
					output.setUTCFullYear(dateParts.Year);
					output.setUTCMonth(dateParts.Month - 1);
					output.setUTCDate(dateParts.Day + days);
					output.setUTCHours(dateParts.Hours);
					output.setUTCMinutes(dateParts.Minutes);
					output.setUTCSeconds(dateParts.Seconds);
					output.setUTCMilliseconds(dateParts.Milliseconds);
				}

				return output;
			}
		}

		return input;
	}
	public static NextDay(myDate: Date, days: number) {
		var myResult = this.GetDateParts(myDate).DateObject;

		if (myDate != null) {
			myResult.setUTCDate(myDate.getDate() + days);
		}

		return myResult;
	}
	public static GetDaysBetweenDates(date1: Date, date2: Date, useFloor: boolean = false) {
		var myResult: number = 0;

		if (date1 != null && date2 != null) {
			if (date1 != undefined && date2 != undefined) {
				date1 = this.TruncateTime(date1, useFloor);
				date2 = this.TruncateTime(date2, useFloor);

				var d1 = new Date(date1.toString());
				var d2 = new Date(date2.toString());
				var timeDiff = Math.abs(d2.getTime() - d1.getTime());
				var daysDiff = timeDiff / (1000 * 3600 * 24);

				if (useFloor) {
					myResult = Math.floor(daysDiff);
				} else {
					myResult = Math.ceil(daysDiff);
				}
			}
		}

		return myResult;
	}
	public static GetMinutesBetweenDates(date1: Date, date2: Date) {
		var myResult: number = 0;

		if (date1 != null && date2 != null) {
			if (date1 != undefined && date2 != undefined) {
				date1 = this.TruncateTime(date1);
				date2 = this.TruncateTime(date2);

				var d1 = new Date(date1.toString());
				var d2 = new Date(date2.toString());
				var timeDiff = Math.abs(d2.getTime() - d1.getTime());
				var Daysdiff = Math.ceil(timeDiff / (1000 * 60));
				myResult = Daysdiff;
			}
		}

		return myResult;
	}
	public static GetHoursBetweenDates(date1: Date, date2: Date) {
		var myResult: number = 0;

		if (date1 != null && date2 != null) {
			if (date1 != undefined && date2 != undefined) {
				date1 = this.TruncateTime(date1);
				date2 = this.TruncateTime(date2);

				var d1 = new Date(date1.toString());
				var d2 = new Date(date2.toString());
				var timeDiff = Math.abs(d2.getTime() - d1.getTime());
				var Daysdiff = Math.ceil(timeDiff / (1000 * 3600));
				myResult = Daysdiff;
			}
		}

		return myResult;
	}
	public static GetSecondsBetweenDates(date1: Date, date2: Date) {
		var myResult: number = 0;

		if (date1 != null && date2 != null) {
			if (date1 != undefined && date2 != undefined) {
				date1 = this.TruncateTime(date1);
				date2 = this.TruncateTime(date2);

				var d1 = new Date(date1.toString());
				var d2 = new Date(date2.toString());
				var timeDiff = Math.abs(d2.getTime() - d1.getTime());
				var Daysdiff = Math.ceil(timeDiff / 1000);
				myResult = Daysdiff;
			}
		}

		return myResult;
	}

	public static GetDateParts(input: Date) {
		var myDateParts = new DateParts();

		if (!AppTool.IsNullOrEmpty(input)) {
			// 2009-12-18 10:54:50.546
			// 2016-12-27T00:00:00
			// 2016-12-27T16:58:15.527
			// 2012-02-07CET00:00:00
			// 2009-04-19T16:11:05+02:00
			// 2009-04-19T16:11:05Z

			if (typeof input == 'string') {
				var dateTimeString: string = (input + '').replace('Z', '');

				var splitBy = ' ';

				if (dateTimeString.indexOf('CET') !== -1) {
					splitBy = 'CET';
				} else if (dateTimeString.indexOf('T') !== -1) {
					splitBy = 'T';
				}

				if (!AppTool.IsNullOrEmpty(dateTimeString)) {
					var dateString: string = dateTimeString.split(splitBy)[0];
					var timeString: string = dateTimeString.split(splitBy)[1];

					if (!AppTool.IsNullOrEmpty(dateString)) {
						var dateParts: string[] = dateString.split('-');

						myDateParts.Year = +dateParts[0];
						myDateParts.Month = +dateParts[1];
						myDateParts.Day = +dateParts[2];

						myDateParts.LocalYear = myDateParts.Year;
						myDateParts.LocalMonth = myDateParts.Month;
						myDateParts.LocalDay = myDateParts.Day;
					}

					if (!AppTool.IsNullOrEmpty(timeString)) {
						var timeParts: string[] = timeString.split(':');

						myDateParts.Hours = +timeParts[0];
						myDateParts.Minutes = +timeParts[1];
						myDateParts.Seconds = +timeParts[2].split('.')[0];

						if (!AppTool.IsNullOrEmpty(timeParts[2].split('.')[1])) {
							myDateParts.Milliseconds = +timeParts[2].split('.')[1];
						}
					}
				}
			} else {
				myDateParts.Year = input.getUTCFullYear();
				myDateParts.Month = input.getUTCMonth() + 1;
				myDateParts.Day = input.getUTCDate();
				myDateParts.Hours = input.getUTCHours();
				myDateParts.Minutes = input.getUTCMinutes();
				myDateParts.Seconds = input.getUTCSeconds();
				myDateParts.Milliseconds = input.getUTCMilliseconds();

				// "Thu Apr 05 2018 03:00:00 GMT+0300 (Eastern Europe Daylight Time)"
				var inputConverted: string = input + '';
				var inputConvertedParts: string[] = inputConverted.split(' ');
				var MonthName: string = inputConvertedParts[1];

				myDateParts.LocalYear = +inputConvertedParts[3];
				myDateParts.LocalMonth = this.GetMonthFromName(MonthName);
				myDateParts.LocalDay = +inputConvertedParts[2];
			}

			// we need it by this sort (Year > Month > Day)
			// if current month is 6 (max day is 30/6)
			// and we used this method to get date of (31/5)
			// then it will be a problem
			// the new Date(); will init date as today date (month 6)
			// but then when assign the day (31) it will 31/6 which will be converted to 1/7
			// then when assign the month (5) it will become (1/5) instead of (31/5)

			// example
			// input                                                26/06/2017
			// var dateObject = new Date();                         31/08/2017  Thu Aug 31 2017 08:16:39 GMT+0300
			// dateObject.setUTCFullYear(myDateParts.Year);         31/08/2017
			// dateObject.setUTCMonth((myDateParts.Month - 1));     01/07/2017  Sat Jul 01 2017 08:21:59 GMT+0300   (No Date 31/06 so it turned it to 01/07)
			// dateObject.setUTCDate(myDateParts.Day);              26/07/2017  Wed Jul 26 2017 08:21:59 GMT+0300

			var dateObject = new Date();
			dateObject.setUTCMonth(0);
			dateObject.setUTCDate(1);
			dateObject.setUTCFullYear(myDateParts.Year);
			dateObject.setUTCMonth(myDateParts.Month - 1);
			dateObject.setUTCDate(myDateParts.Day);
			dateObject.setUTCHours(myDateParts.Hours);
			dateObject.setUTCMinutes(myDateParts.Minutes);
			dateObject.setUTCSeconds(myDateParts.Seconds);
			dateObject.setUTCMilliseconds(myDateParts.Milliseconds);

			myDateParts.DateObject = dateObject;
			myDateParts.Hours12 = myDateParts.Hours > 12 ? myDateParts.Hours - 12 : myDateParts.Hours;
			myDateParts.DateTicks = myDateParts.DateObject.valueOf();
			myDateParts.TotalMinutes = myDateParts.Hours * 60 + myDateParts.Minutes;
		}

		return myDateParts;
	}
	public static GetDateFormats(input: Date) {
		var myDateFormats = new DateFormats();

		if (!AppTool.IsNullOrEmpty(input)) {
			var myDateParts = this.GetDateParts(input);

			var stringOfYear = AppTool.PadLeft('' + myDateParts.Year, 4, '0');
			var stringOfMonth = AppTool.PadLeft('' + myDateParts.Month, 2, '0');
			var stringOfDay = AppTool.PadLeft('' + myDateParts.Day, 2, '0');
			var stringOfHours = AppTool.PadLeft('' + myDateParts.Hours, 2, '0');
			var stringOfHours12 = AppTool.PadLeft('' + myDateParts.Hours12, 2, '0');
			var stringOfMinutes = AppTool.PadLeft('' + myDateParts.Minutes, 2, '0');
			var stringOfSeconds = AppTool.PadLeft('' + myDateParts.Seconds, 2, '0');
			var stringOfMilliseconds = AppTool.PadLeft('' + myDateParts.Milliseconds, 3, '0');

			// Build Formats
			myDateFormats.AMPM = myDateParts.Hours >= 12 ? 'PM' : 'AM';
			myDateFormats.LocalDateString = myDateParts.DateObject.toLocaleDateString();
			myDateFormats.LocalTimeString = myDateParts.DateObject.toLocaleTimeString();
			myDateFormats.LocalDateTimeString = myDateParts.DateObject.toLocaleString();

			myDateFormats.ShortTimeString = stringOfHours + ':' + stringOfMinutes;
			myDateFormats.LongTimeString = stringOfHours + ':' + stringOfMinutes + ':' + stringOfSeconds;
			myDateFormats.ShortTimeString12 = stringOfHours12 + ':' + stringOfMinutes + ' ' + myDateFormats.AMPM;
			myDateFormats.LongTimeString12 =
				stringOfHours12 + ':' + stringOfMinutes + ':' + stringOfSeconds + ' ' + myDateFormats.AMPM;

			// ShortDateString
			if (!AppTool.IsNullOrEmpty(this.TenantPM.DateTimeFormat)) {
				var myDateTimeFormatPrefix = this.TenantPM.DateTimeFormat.toLowerCase().substring(0, 2);

				if (myDateTimeFormatPrefix == 'mm') {
					myDateFormats.ShortDateString = stringOfMonth + '/' + stringOfDay + '/' + stringOfYear;
				} else {
					myDateFormats.ShortDateString = stringOfDay + '/' + stringOfMonth + '/' + stringOfYear;
				}
			} else {
				myDateFormats.ShortDateString = myDateFormats.LocalDateString;
			}

			// DateString
			//var date_Today = this.GetCurrentDateAsUtc();
			//var date_Tommorow = this.AddDays(this.GetCurrentDateAsUtc(), 1);
			//var date_Yesterday = this.AddDays(this.GetCurrentDateAsUtc(), -1);

			var date_Today = new Date();
			var date_Tommorow = new Date();
			var date_Yesterday = new Date();
			date_Tommorow.setDate(date_Tommorow.getDate() + 1);
			date_Yesterday.setDate(date_Yesterday.getDate() + -1);

			var dateParts_Today = this.GetDateParts(date_Today);
			var dateParts_Tommorow = this.GetDateParts(date_Tommorow);
			var dateParts_Yesterday = this.GetDateParts(date_Yesterday);

			if (
				dateParts_Today.LocalYear == myDateParts.Year &&
				dateParts_Today.LocalMonth == myDateParts.Month &&
				dateParts_Today.LocalDay == myDateParts.Day
			) {
				myDateFormats.DateString = TextCodeTranslator.Translate('General.O.Today');
			} else if (
				dateParts_Tommorow.LocalYear == myDateParts.Year &&
				dateParts_Tommorow.LocalMonth == myDateParts.Month &&
				dateParts_Tommorow.LocalDay == myDateParts.Day
			) {
				myDateFormats.DateString = TextCodeTranslator.Translate('General.O.Tomorrow');
			} else if (
				dateParts_Yesterday.LocalYear == myDateParts.Year &&
				dateParts_Yesterday.LocalMonth == myDateParts.Month &&
				dateParts_Yesterday.LocalDay == myDateParts.Day
			) {
				myDateFormats.DateString = TextCodeTranslator.Translate('General.O.Yesterday');
			} else {
				myDateFormats.DateString = myDateFormats.ShortDateString;
			}

			myDateFormats.DateParts = myDateParts;
			this.GetPartsNames(myDateFormats);
		}

		return myDateFormats;
	}
	private static GetTodayDateParts() {
		var myResult: Date = new Date();

		var Hours_NOR = myResult.getHours();
		var Hours_UTC = myResult.getUTCHours();
		var Hours = Hours_UTC - Hours_NOR;

		myResult.setUTCFullYear(myResult.getUTCFullYear());
		myResult.setUTCMonth(myResult.getUTCMonth());
		myResult.setUTCDate(myResult.getUTCDate());
		myResult.setUTCHours(Hours);
		myResult.setUTCMinutes(0);
		myResult.setUTCSeconds(0);
		myResult.setUTCMilliseconds(0);

		var myDateParts = this.GetDateParts(myResult);
		return myDateParts;
	}
	private static GetTommorowDateParts() {
		var myResult: Date = new Date();

		var Hours_NOR = myResult.getHours();
		var Hours_UTC = myResult.getUTCHours();
		var Hours = Hours_UTC - Hours_NOR;

		myResult.setUTCFullYear(myResult.getUTCFullYear());
		myResult.setUTCMonth(myResult.getUTCMonth());
		myResult.setUTCDate(myResult.getUTCDate() + 1);
		myResult.setUTCHours(Hours);
		myResult.setUTCMinutes(0);
		myResult.setUTCSeconds(0);
		myResult.setUTCMilliseconds(0);

		var myDateParts = this.GetDateParts(myResult);
		return myDateParts;
	}
	private static GetYesterdayDateParts() {
		var myResult: Date = new Date();

		var Hours_NOR = myResult.getHours();
		var Hours_UTC = myResult.getUTCHours();
		var Hours = Hours_UTC - Hours_NOR;

		myResult.setUTCFullYear(myResult.getUTCFullYear());
		myResult.setUTCMonth(myResult.getUTCMonth());
		myResult.setUTCDate(myResult.getUTCDate() - 1);
		myResult.setUTCHours(Hours);
		myResult.setUTCMinutes(0);
		myResult.setUTCSeconds(0);
		myResult.setUTCMilliseconds(0);

		var myDateParts = this.GetDateParts(myResult);
		return myDateParts;
	}

	public static GetPartsNames(dateFormats: DateFormats) {
		if (dateFormats) {
			var dayIndex = dateFormats.DateParts.DateObject.getDay();
			var monthIndex = dateFormats.DateParts.DateObject.getMonth();

			switch (dayIndex) {
				case 0: {
					(dateFormats.DayName = 'Sunday'), (dateFormats.DayNameShort = 'Sun');
					break;
				}
				case 1: {
					(dateFormats.DayName = 'Monday'), (dateFormats.DayNameShort = 'Mon');
					break;
				}
				case 2: {
					(dateFormats.DayName = 'Tuesday'), (dateFormats.DayNameShort = 'Tue');
					break;
				}
				case 3: {
					(dateFormats.DayName = 'Wednesday'), (dateFormats.DayNameShort = 'Wed');
					break;
				}
				case 4: {
					(dateFormats.DayName = 'Thursday'), (dateFormats.DayNameShort = 'Thu');
					break;
				}
				case 5: {
					(dateFormats.DayName = 'Friday'), (dateFormats.DayNameShort = 'Fri');
					break;
				}
				case 6: {
					(dateFormats.DayName = 'Saturday'), (dateFormats.DayNameShort = 'Sat');
					break;
				}
			}

			switch (monthIndex) {
				case 0: {
					(dateFormats.MonthName = 'January'), (dateFormats.MonthNameShort = 'Jan');
					break;
				}
				case 1: {
					(dateFormats.MonthName = 'February'), (dateFormats.MonthNameShort = 'Feb');
					break;
				}
				case 2: {
					(dateFormats.MonthName = 'March'), (dateFormats.MonthNameShort = 'Mar');
					break;
				}
				case 3: {
					(dateFormats.MonthName = 'April'), (dateFormats.MonthNameShort = 'Apr');
					break;
				}
				case 4: {
					(dateFormats.MonthName = 'May'), (dateFormats.MonthNameShort = 'May');
					break;
				}
				case 5: {
					(dateFormats.MonthName = 'June'), (dateFormats.MonthNameShort = 'Jun');
					break;
				}
				case 6: {
					(dateFormats.MonthName = 'July'), (dateFormats.MonthNameShort = 'Jul');
					break;
				}
				case 7: {
					(dateFormats.MonthName = 'August'), (dateFormats.MonthNameShort = 'Aug');
					break;
				}
				case 8: {
					(dateFormats.MonthName = 'September'), (dateFormats.MonthNameShort = 'Sep');
					break;
				}
				case 9: {
					(dateFormats.MonthName = 'October'), (dateFormats.MonthNameShort = 'Oct');
					break;
				}
				case 10: {
					(dateFormats.MonthName = 'November'), (dateFormats.MonthNameShort = 'Nov');
					break;
				}
				case 11: {
					(dateFormats.MonthName = 'December'), (dateFormats.MonthNameShort = 'Dec');
					break;
				}
			}
		}
	}
	public static GetMonthFromName(name: string) {
		var myResult: number = 0;

		if (name) {
			switch (name.toLowerCase()) {
				case 'jan':
				case 'january': {
					myResult = 1;
					break;
				}

				case 'feb':
				case 'february': {
					myResult = 2;
					break;
				}

				case 'mar':
				case 'march': {
					myResult = 3;
					break;
				}

				case 'apr':
				case 'april': {
					myResult = 4;
					break;
				}

				case 'may':
				case 'may': {
					myResult = 5;
					break;
				}

				case 'jun':
				case 'june': {
					myResult = 6;
					break;
				}

				case 'jul':
				case 'july': {
					myResult = 7;
					break;
				}

				case 'aug':
				case 'august': {
					myResult = 8;
					break;
				}

				case 'sep':
				case 'september': {
					myResult = 9;
					break;
				}

				case 'oct':
				case 'october': {
					myResult = 10;
					break;
				}

				case 'nov':
				case 'november': {
					myResult = 11;
					break;
				}

				case 'dec':
				case 'december': {
					myResult = 12;
					break;
				}
			}
		}

		return myResult;
	}
	public static GetCurrentFullDayOfWeek(dayIndex: number) {
		var dayName = '';
		switch (dayIndex) {
			case 0: {
				dayName = 'Sunday';
				break;
			}
			case 1: {
				dayName = 'Monday';
				break;
			}
			case 2: {
				dayName = 'Tuesday';
				break;
			}
			case 3: {
				dayName = 'Wednesday';
				break;
			}
			case 4: {
				dayName = 'Thursday';
				break;
			}
			case 5: {
				dayName = 'Friday';
				break;
			}
			case 6: {
				dayName = 'Saturday';
				break;
			}
		}
		return dayName;
	}
	public static GetRelativeRateDate(entityDate: Date, rateDate: Date, endText: string) {
		var dateString = '';

		if (entityDate != null && rateDate != null) {
			var count = 0;

			var dateParts1: DateParts = this.GetDateParts(entityDate);
			var dateParts2: DateParts = this.GetDateParts(rateDate);

			if (dateParts2.DateTicks > dateParts1.DateTicks) {
				var todayDate: Date = this.GetCurrentDateAsUtc();

				var dateTime: Date = dateParts2.DateObject;

				if (dateTime.valueOf() === todayDate.valueOf()) {
					//dateString = TextCodeTranslator.Translate("General.O.Today");
				} else {
					dateString = this.GetDateFormats(rateDate).ShortDateString;
				}
			} else {
				if (AppTool.IsNullOrEmpty(endText)) {
					endText = 'old';
				}

				var days = DateTool.GetDaysBetweenDates(dateParts1.DateObject, dateParts2.DateObject);

				if (days == 0) {
					//dateString = TextCodeTranslator.Translate("General.O.Today");
				} else {
					if (days <= 30) {
						dateString = days == 1 ? days + ' day' : days + ' days';
					} else {
						if (Math.abs(dateParts1.Year - dateParts2.Year) > 0) {
							count = Math.abs(dateParts1.Year - dateParts2.Year);
							dateString = count == 1 ? count + ' year' : count + ' years';
						} else if (Math.abs(dateParts1.Month - dateParts2.Month) > 0) {
							count = Math.abs(dateParts1.Month - dateParts2.Month);
							dateString = count == 1 ? count + ' month' : count + ' months';
						}
					}

					dateString = dateString + ' ' + endText;
				}
			}
		}

		return dateString;
	}

	public static AddMillisecond(myDate: Date, millisecond: number): Date {
		if (myDate) {
			var myResult = new Date(myDate + '');

			if (myResult != null) {
				myResult.setTime(myResult.getTime() + millisecond);
			}

			return myResult;
		}
		return myDate;
	}
	public static AddSecond(myDate: Date, second: number): Date {
		return DateTool.AddMillisecond(myDate, second * 1000);
	}
	public static AddMinute(myDate: Date, minute: number): Date {
		return DateTool.AddSecond(myDate, minute * 60);
	}
	public static AddHour(myDate: Date, hour: number): Date {
		return DateTool.AddMinute(myDate, hour * 60);
	}

	public static GetDate(year: number, month: number, day: number, hour: number, minute: number, second: number) {
		var date: Date = new Date();
		date.setUTCHours(hour);
		date.setUTCMinutes(minute);
		date.setUTCSeconds(second);
		date.setUTCMilliseconds(0);
		var endDayNumber: number = new Date(date.getFullYear(), date.getMonth() + 1, 0).getDate();
		if (endDayNumber == 31) {
			date.setUTCDate(day);
			date.setUTCMonth(month);
		} else {
			date.setUTCMonth(month);
			date.setUTCDate(day);
		}
		date.setUTCFullYear(year);
		return date;
	}
	public static GetMinDateTime() {
		var myResult: Date;
		//myResult= new Date();
		//myResult.setUTCFullYear(0);
		//myResult.setUTCMonth(0);
		//myResult.setUTCDate(0);
		//myResult.setUTCHours(0);
		//myResult.setUTCMinutes(0);
		//myResult.setUTCSeconds(0);
		//myResult.setUTCMilliseconds(0);

		//https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Date
		//The JavaScript date is based on a time value that is milliseconds since midnight 01 January, 1970 UTC.A day holds 86, 400, 000 milliseconds.The JavaScript Date object range is - 100, 000, 000 days to 100, 000, 000 days relative to 01 January, 1970 UTC.
		// itzik my birthday is 1971 so i was born 1 year After Century :) !!!
		//myResult = new Date(1970);
		myResult = new Date(null);
		return myResult;
	}
	public static IsNullOrMinDateTime(myDate: Date): boolean {
		if (myDate == null || myDate === undefined) {
			return true;
		}
		if (myDate.valueOf() == DateTool.GetMinDateTime().valueOf()) {
			return true;
		}
		if (myDate.toString() == '0001-01-01T00:00:00') {
			return true;
		}
		return false;
	}

	public static IsDateBigger(date1: any, date2: any) {
		var myResult: boolean = false;

		if (!AppTool.IsNullOrEmpty(date1) && !AppTool.IsNullOrEmpty(date2)) {
			var Date1Ticks = this.GetDateParts(date1).DateTicks;
			var Date2Ticks = this.GetDateParts(date2).DateTicks;

			if (Date1Ticks > Date2Ticks) {
				myResult = true;
			}
		}

		return myResult;
	}
	public static IsActualDateValid(date: any) {
		var myResult: boolean = true;

		if (!AppTool.IsNullOrEmpty(date)) {
			var Date1Ticks = this.GetDateParts(date).DateTicks;
			var Date2Ticks = this.GetCurrentDateTimeAsUtc().valueOf();

			if (Date1Ticks > Date2Ticks) {
				var ticks = Date1Ticks - Date2Ticks;
				var seconds = ticks / 1000;
				var minutes = seconds / 60;

				if (minutes > 24 * 60) {
					myResult = false;
				}
			}
		}

		return myResult;
	}
}
export class FontTool {
	public static Red: string = '#E53030';
	public static Black: string = '#282E30';
	public static Green: string = '#009161';
	public static Gray: string = '#6E7172';
	public static Orange: string = '#F37021';
	public static Magenta: string = '#FF6BFF';
	public static CellDisabledColor: string = '#A8AAAD';
	public static CellDisabledBackground: string = 'rgba(230, 231, 232, 0.5)';
	public static CellIsCheckedBackground: string = 'rgba(208, 224, 234, 0.4)';
}
export class FormatTool {
	public static IsEmail(input: string): boolean {
		var myResult = true;

		if (!AppTool.IsNullOrEmpty(input)) {
			var exp =
				/^(([^<>()[\]\\.,;:\s@\""]+(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
			if (!input.match(exp)) {
				myResult = false;
			}
		}

		return myResult;
	}

	public static IsTextFormatted(input: string): boolean {
		var myResult = false;

		if (AppTool.IsNullOrEmpty(input)) {
			myResult = true;
		} else {
			input = input.trim().toUpperCase();
			input = input.replace(/[^A-Z0-9\-\. ]/g, '');

			// trdtrdt dd' sd < lo
			// input.replace(/[A-Z0-9\-\. ]/g, "");     Result='<
			// input.replace(/[^A-Z0-9\-\. ]/g, "");    Result= trdtrdt dd sd  lo

			if (!AppTool.IsNullOrEmpty(input)) {
				myResult = true;
			}
		}

		return myResult;
	}
	public static IsDecimal(input: string): boolean {
		var isDecimal = false;

		if (input != null) {
			input = input.trim();

			if (input.length > 0) {
				isDecimal = true;
				var isFirstDot = true;
				var isFirstminus = true;

				for (var i = 0; i < input.length; i++) {
					var letter = input[i];

					if (!letter.match(/^[0-9]/gi)) {
						if (letter == '.') {
							if (i == 0) {
								isDecimal = false;
								break;
							} else if (isFirstDot) {
								isFirstDot = false;
								continue;
							} else {
								isDecimal = false;
								break;
							}
						} else if (letter == '-') {
							if (i != 0) {
								isDecimal = false;
								break;
							} else {
								if (isFirstminus) {
									isFirstminus = false;
									continue;
								} else {
									isDecimal = false;
									break;
								}
							}
						} else {
							isDecimal = false;
							break;
						}
					}
				}
			}
		}

		return isDecimal;
	}
	public static IsText(input: string): boolean {
		var myResult = true;

		if (!AppTool.IsNullOrEmpty(input)) {
			input = input.trim().toUpperCase();

			if (!input.match(/^[A-Z0-9\-\. ]*$/)) {
				myResult = false;
			}
		}

		return myResult;
	}
	public static IsAlpha(input: string): boolean {
		var myResult = true;

		if (!AppTool.IsNullOrEmpty(input)) {
			input = input.trim().toUpperCase();

			if (!input.match(/^[A-Z]*$/)) {
				myResult = false;
			}
		}

		return myResult;
	}
	public static IsNumeric(input: string): boolean {
		var myResult = true;

		if (!AppTool.IsNullOrEmpty(input)) {
			input = input.trim().toUpperCase();

			if (!input.match(/^[0-9]*$/)) {
				myResult = false;
			}
		}

		return myResult;
	}
	public static IsAlphaNumeric(input: string): boolean {
		var myResult = true;

		if (!AppTool.IsNullOrEmpty(input)) {
			input = input.trim().toUpperCase();

			if (!input.match(/^[A-Z0-9]*$/)) {
				myResult = false;
			}
		}

		return myResult;
	}
	public static IsEnglishText(input: string): boolean {
		var myResult = true;

		if (!AppTool.IsNullOrEmpty(input)) {
			input = input.trim().toUpperCase();

			// English Only
			//if (!input.match(/^[\x20-\x7E]*$/)) {
			//    myResult = false;
			//}

			// English + Latin
			if (!input.match(/^[\x20-\xFF]*$/)) {
				myResult = false;
			}
		}

		return myResult;
	}
	public static FormatNumber(myNumber: number, myFormat: string = 'N2') {
		var myResult: string = '';

		var isValidNumber = FormatTool.ValidateNumber(myNumber);

		if (isValidNumber) {
			var fractionDigits = FormatTool.GetFractionDigits(myFormat);

			myNumber = AppTool.Round(myNumber, fractionDigits);

			var myStringNumber = myNumber + '';
			var myStringNumber1 = myStringNumber.split('.')[0];
			var myStringNumber2 = myStringNumber.split('.')[1];

			var firstFormatChar = FormatTool.GetFirstFormatChar();
			var secondFormatChar = FormatTool.GetSecondFormatChar();

			myResult = myStringNumber1.replace(/\B(?=(\d{3})+(?!\d))/g, firstFormatChar);

			if (myFormat.toString().toLowerCase() != 'n0') {
				if (AppTool.IsNullOrEmpty(myStringNumber2)) {
					myStringNumber2 = '0';
				}

				myResult += secondFormatChar + AppTool.PadRight(myStringNumber2, fractionDigits, '0');
			}
		}

		return myResult;
	}

	private static ValidateNumber(myNumber: number) {
		var isValid = false;

		if (!AppTool.IsNullOrEmpty(myNumber)) {
			isValid = true;

			if (typeof myNumber == 'string') {
				isValid = false;

				if (FormatTool.IsDecimal(myNumber + '')) {
					myNumber = +myNumber;
					isValid = true;
				}
			}
		}

		return isValid;
	}
	private static GetFractionDigits(myFormat: string) {
		var fractionDigits = 2;

		switch (myFormat.toString().toLowerCase()) {
			case 'n0': {
				fractionDigits = 0;
				break;
			}
			case 'n1': {
				fractionDigits = 1;
				break;
			}
			case 'n2': {
				fractionDigits = 2;
				break;
			}
			case 'n3': {
				fractionDigits = 3;
				break;
			}
			case 'n4': {
				fractionDigits = 4;
				break;
			}
			case 'n5': {
				fractionDigits = 5;
				break;
			}
			default: {
				fractionDigits = 2;
				break;
			}
		}

		return fractionDigits;
	}
	private static GetFirstFormatChar() {
		switch (SessionLocator.TenantPM.NumberFormatCode) {
			case 'DC': {
				return '.';
			}
			case 'AD': {
				return "'";
			}
			default: {
				return ',';
			}
		}
	}
	private static GetSecondFormatChar() {
		switch (SessionLocator.TenantPM.NumberFormatCode) {
			case 'DC': {
				return ',';
			}
			case 'AD': {
				return '.';
			}
			default: {
				return '.';
			}
		}
	}

	public static CustomFormatNumber(myNumber: number, myFormat: string = 'N2') {
		var first = ',';
		var second = '.';
		switch (SessionLocator.TenantPM.NumberFormatCode) {
			case 'CD': {
				first = ',';
				second = '.';
				break;
			}

			case 'DC': {
				first = '.';
				second = ',';
				break;
			}

			case 'AD': {
				first = "'";
				second = '.';
				break;
			}

			default: {
				first = ',';
				second = '.';
				break;
			}
		}

		var myResult: string = '';

		if (!AppTool.IsNullOrEmpty(myNumber)) {
			var isValid = true;

			if (typeof myNumber == 'string') {
				isValid = false;

				if (FormatTool.IsDecimal(myNumber + '')) {
					myNumber = +myNumber;
					isValid = true;
				}
			}

			if (isValid) {
				var myFractionDigits = 2;

				switch (myFormat.toString().toLowerCase()) {
					case 'n0': {
						myFractionDigits = 0;
						break;
					}
					case 'n1': {
						myFractionDigits = 1;
						break;
					}
					case 'n2': {
						myFractionDigits = 2;
						break;
					}
					case 'n3': {
						myFractionDigits = 3;
						break;
					}
					case 'n4': {
						myFractionDigits = 4;
						break;
					}
					case 'n5': {
						myFractionDigits = 5;
						break;
					}
					default: {
						myFractionDigits = 2;
						break;
					}
				}

				myNumber = AppTool.Round(myNumber, myFractionDigits);

				var myStringNumber = myNumber + '';
				var myStringNumber1 = myStringNumber.split('.')[0];
				var myStringNumber2 = myStringNumber.split('.')[1];

				myResult = myStringNumber1.replace(/\B(?=(\d{3})+(?!\d))/g, first);
				if (!AppTool.IsNullOrEmpty(myStringNumber2)) {
					myResult += second + myStringNumber2;
				}

				if (myFormat.toString().toLowerCase() != 'n0') {
					var side1 = myResult.split('.')[0];
					var side2 = myResult.split('.')[1];
					myResult = side1 + second + AppTool.PadRight(side2, myFractionDigits, '0');
				}
			}
		}

		return myResult;
	}

	public static Validate_IATACode(input: string): boolean {
		var myResult: boolean = false;

		if (!AppTool.IsNullOrEmpty(input)) {
			if (input.length <= 7) {
				var pattern = /^\d+$/;
				if (pattern.test(input)) {
					myResult = true;
				}
			}
		}

		return myResult;
	}
	public static Validate_CASSCode(input: string): boolean {
		var myResult: boolean = false;

		if (!AppTool.IsNullOrEmpty(input)) {
			if (!AppTool.IsNullOrEmpty(input)) {
				if (input.length <= 4) {
					var pattern = /^\d+$/;
					if (pattern.test(input)) {
						myResult = true;
					}
				}
			}
		}

		return myResult;
	}
	public static Validate_SLAC(input: string): boolean {
		var myResult: boolean = false;

		if (!AppTool.IsNullOrEmpty(input)) {
			if (!AppTool.IsNullOrEmpty(input)) {
				if (input.length <= 5) {
					var pattern = /^\d+$/;
					if (pattern.test(input)) {
						myResult = true;
					}
				}
			}
		}

		return myResult;
	}
	public static Validate_FlightNumber(input: string): boolean {
		var myResult = false;

		if (!AppTool.IsNullOrEmpty(input)) {
			input = input.trim().toUpperCase();

			if (input.match(/^[0-9]{3,4}$/)) {
				myResult = true;
			} else if (input.match(/^[0-9]{4}[A-Z]{1}$/)) {
				myResult = true;
			}
		}

		return myResult;
	}
	public static Validate_CommodityNo(input: string): boolean {
		var myResult = false;

		if (AppTool.IsNullOrEmpty(input)) {
			myResult = true;
		} else {
			if (input.match(/^[0-9]{4,7}$/)) {
				myResult = true;
			}
		}

		return myResult;
	}
	public static Validate_DeclaredCarriage(input: string): boolean {
		var myResult = false;

		if (AppTool.IsNullOrEmpty(input)) {
			myResult = true;
		} else {
			input = input.replace(' ', '').toUpperCase();

			if (input == 'NVD') {
				myResult = true;
			} else {
				input = input.length <= 12 ? input : input.substring(0, 12);

				if (this.IsDecimal(input)) {
					myResult = true;
				}
			}
		}

		return myResult;
	}
	public static Validate_DeclaredCustoms(input: string): boolean {
		var myResult = false;

		if (AppTool.IsNullOrEmpty(input)) {
			myResult = true;
		} else {
			input = input.replace(' ', '').toUpperCase();

			if (input == 'NCV') {
				myResult = true;
			} else {
				input = input.length <= 12 ? input : input.substring(0, 12);

				if (this.IsDecimal(input)) {
					myResult = true;
				}
			}
		}

		return myResult;
	}
	public static Validate_DeclaredInsurrence(input: string): boolean {
		var myResult = false;

		if (AppTool.IsNullOrEmpty(input)) {
			myResult = true;
		} else {
			input = input.replace(' ', '').toUpperCase();

			if (input == 'XXX') {
				myResult = true;
			} else {
				input = input.length <= 12 ? input : input.substring(0, 12);

				if (this.IsDecimal(input)) {
					myResult = true;
				}
			}
		}

		return myResult;
	}
	public static GetWrongTextFormatMessage(fieldName: string): string {
		var myResult: string = 'Invalid format. Can contain [a-z/0-9/./-]';

		if (!AppTool.IsNullOrEmpty(fieldName)) {
			myResult = fieldName + ' ' + myResult;
		}

		return myResult;
	}
	public static GetIntegerValue(input: any): number {
		var myResult: number = 0;

		if (!AppTool.IsNullOrEmpty(input)) {
			var inputString = input + '';

			if (this.IsDecimal(inputString)) {
				myResult = +inputString.split('.')[0];
			}
		}

		return myResult;
	}

	public static ValidateContainerNumber(input: string) {
		var myResult: string = null;

		if (!AppTool.IsNullOrEmpty(input)) {
			input = input.trim().toUpperCase();

			if (!input.match(/^([a-zA-Z]{4})([0-9]{7})$/)) {
				myResult = TextCodeTranslator.Translate('General.M.ContainerNumberFormatisInvalid');
			} else {
				var sum = 0;

				for (var i = 0; i < input.length - 1; i++) {
					if (this.IsAlpha(input[i])) {
						sum = sum + this.GetCharCode(input[i]) * Math.pow(2, i);
					} else {
						sum = sum + this.GetIntegerDigit(input[i]) * Math.pow(2, i);
					}
				}

				var integrSum = this.GetIntegerValue(sum);
				var checkDigit = this.GetIntegerDigit(input[input.length - 1]);

				var divisionby11: number = integrSum / 11;
				var erasedecimaldigits: number = this.GetIntegerValue(divisionby11);
				var multiplyby11 = erasedecimaldigits * 11;
				var validCheckDigit = integrSum - multiplyby11;

				if (validCheckDigit == 10) {
					validCheckDigit = 0;
				}

				if (validCheckDigit != checkDigit) {
					var wStr: string = TextCodeTranslator.Translate('General.M.ContainerNumberCheckDigitiswrong');
					wStr = wStr.replace('%CheckDigit', validCheckDigit.toString());
					myResult = wStr;
				}
			}
		}

		return myResult;
	}
	private static GetCharCode(c: any) {
		switch (c) {
			case 'A':
				return 10;
			case 'B':
				return 12;
			case 'C':
				return 13;
			case 'D':
				return 14;
			case 'E':
				return 15;
			case 'F':
				return 16;
			case 'G':
				return 17;
			case 'H':
				return 18;
			case 'I':
				return 19;
			case 'J':
				return 20;
			case 'K':
				return 21;
			case 'L':
				return 23;
			case 'M':
				return 24;
			case 'N':
				return 25;
			case 'O':
				return 26;
			case 'P':
				return 27;
			case 'Q':
				return 28;
			case 'R':
				return 29;
			case 'S':
				return 30;
			case 'T':
				return 31;
			case 'U':
				return 32;
			case 'V':
				return 34;
			case 'W':
				return 35;
			case 'X':
				return 36;
			case 'Y':
				return 37;
			case 'Z':
				return 38;
			default:
				return 0;
		}
	}
	private static GetIntegerDigit(c: any) {
		switch (c) {
			case '1':
				return 1;
			case '2':
				return 2;
			case '3':
				return 3;
			case '4':
				return 4;
			case '5':
				return 5;
			case '6':
				return 6;
			case '7':
				return 7;
			case '8':
				return 8;
			case '9':
				return 9;
			default:
				return 0;
		}
	}
	public static FormatBigNumbersToExtension(value) {
		if (value >= 1000000000) return Math.round((value / 1000000000) * 100) / 100 + 'Bil';
		else if (value >= 1000000) return Math.round((value / 1000000) * 100) / 100 + 'Mil';
		else if (value >= 1000) {
			console.log(Math.round(value / 1000) / 1000);
			return Math.round(value / 1000) / 1000 + 'K';
		} else return value + '';
	}

	public static PatternText: string = 'lol';
}
export class ArrayTool {
	public static Sum(array: any[], fieldname: string): number {
		var myResult: number = 0;

		if (array && fieldname) {
			array.forEach((item) => {
				var itemValue = item[fieldname];

				if (!AppTool.IsNullOrEmpty(itemValue)) {
					if (typeof itemValue == 'string') {
						if (FormatTool.IsDecimal(itemValue)) {
							itemValue = +itemValue;
						}
					}

					if (typeof itemValue == 'number') {
						myResult += itemValue;
					}
				}
			});
		}

		if (AppTool.IsNullOrEmpty(myResult)) {
			myResult = 0;
		}

		return myResult;
	}
	public static Contains(array: any[], item: any): boolean {
		var myResult = false;

		if (array && item) {
			if (array.indexOf(item) > -1) {
				myResult = true;
			}
		}

		return myResult;
	}
	public static Max(array: any[], fieldname: string): number {
		var myResult: number = 0;

		if (array && fieldname) {
			array.forEach((item) => {
				var itemValue = item[fieldname];

				if (!AppTool.IsNullOrEmpty(itemValue)) {
					if (typeof itemValue == 'string') {
						if (FormatTool.IsDecimal(itemValue)) {
							itemValue = +itemValue;
						}
					}

					if (typeof itemValue == 'number') {
						if (myResult < itemValue) {
							myResult = itemValue;
						}
					}
				}
			});
		}

		if (AppTool.IsNullOrEmpty(myResult)) {
			myResult = 0;
		}

		return myResult;
	}
	public static Sort(array: any[], fieldname: string) {
		var myResult = array;

		if (array) {
			if (array.length > 1) {
				var filledItem: any = array.filter((f) => !AppTool.IsNullOrEmpty(f[fieldname]))[0];
				if (filledItem) {
					var fieldValue: any = filledItem[fieldname];
					var fieldType: string = typeof fieldValue;

					switch (fieldType) {
						case 'string': {
							break;
						}

						case 'number': {
							break;
						}

						case 'date': {
							break;
						}
					}
				}
			}
		}

		return myResult;
	}
	public static SortByDate(array: any[], fieldname: string) {
		var myResult = array;

		if (array) {
			if (array.length > 1) {
				var array_Sorted: any[] = [];
				var array_Fixed: SortingClass[] = [];

				array.forEach((item) => {
					array_Fixed.push(new SortingClass(item, fieldname));
				});

				array_Fixed
					.sort((a, b) => {
						return a.SortingValue === b.SortingValue ? 0 : a.SortingValue < b.SortingValue ? -1 : 1;
					})
					.forEach((item) => {
						array_Sorted.push(item.Item);
					});

				myResult = array_Sorted;
			}
		}

		return myResult;
	}
	public static GroupIt(
		array: any[],
		funcReturnKeyFromItemOrLiteral: any,
		//(item: any) => string  //User-Defined Type Guards

		///fieldname: string //return object that his properties R the Keys {USD: Array(2)} or {USD: Array(2) , EUR : Array(1)}
	) {
		//let grouped: {};//return object that his properties R the Keys {USD: Array(2)} or {USD: Array(2) , EUR : Array(1)}
		var toString = Object.prototype.toString;
		var isFunction = function (o) {
			return toString.call(o) == '[object Function]';
		};
		let propName = funcReturnKeyFromItemOrLiteral.toString();
		return array.reduce((grouped, item: any) => {
			//var key = funcReturnKeyFromItem(item) ? item.apply(this, [item]) : item[item];
			//var key = isFunction(fieldname) ? fieldname.apply(this, [item]) : item[fieldname];
			//: funcReturnKeyFromItem;
			let key: string = '';

			if (isFunction(funcReturnKeyFromItemOrLiteral)) {
				key = funcReturnKeyFromItemOrLiteral.apply(this, [item]);
			} else {
				key = item[propName];
			}

			//if (AppTool.IsNullOrEmpty(grouped[key])) {
			//    grouped.push(new Array<any>());
			//}
			//grouped[key].push(item);

			grouped[key] = grouped[key] || [];
			grouped[key].push(item);

			return grouped; //{USD: Array(2)} or {USD: Array(2) , EUR : Array(1)}
		}, {});
	}
}
export class FileLoader {
	public static AllFroalaResources: ResourceFile[] = [];
	public static IsFroalaFilesLoaded: boolean = false;
	// @Output() static ResourcesLoaded: EventEmitter<boolean> = new EventEmitter<boolean>();

	public static LoadFroalaResources(): Promise<boolean> {
		return new Promise((resolve, reject) => {
			if (this.IsFroalaFilesLoaded) {
				resolve(true);
			} else {
				this.IsFroalaFilesLoaded = true;

				var i: number = 0;
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/js/froala_editor.min.js'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/js/plugins/align.min.js'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/js/plugins/char_counter.min.js'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/js/plugins/code_beautifier.min.js'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/js/plugins/code_view.min.js'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/js/plugins/colors.min.js'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/js/plugins/emoticons.min.js'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/js/plugins/entities.min.js'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/js/plugins/file.min.js'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/js/plugins/font_family.min.js'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/js/plugins/font_size.min.js'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/js/plugins/fullscreen.min.js'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/js/plugins/image.min.js'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/js/plugins/image_manager.min.js'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/js/plugins/inline_style.min.js'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/js/plugins/line_breaker.min.js'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/js/plugins/link.min.js'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/js/plugins/lists.min.js'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/js/plugins/paragraph_format.min.js'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/js/plugins/paragraph_style.min.js'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/js/plugins/quick_insert.min.js'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/js/plugins/quote.min.js'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/js/plugins/table.min.js'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/js/plugins/save.min.js'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/js/plugins/url.min.js'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/js/plugins/video.min.js'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/js/plugins/line_height.min.js'));

				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/css/font-awesome.min.css'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/css/froala_editor.css'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/css/froala_style.css'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/css/plugins/code_view.css'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/css/plugins/colors.css'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/css/plugins/emoticons.css'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/css/plugins/image_manager.css'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/css/plugins/image.css'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/css/plugins/line_breaker.css'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/css/plugins/table.css'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/css/plugins/char_counter.css'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/css/plugins/video.css'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/css/plugins/fullscreen.css'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/css/plugins/quick_insert.css'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/css/plugins/file.css'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Froala/css/codemirror.min.css'));

				// this.ResourcesLoaded.subscribe(s => {
				//     resolve(true);
				// });

				this.RunLoadingFroalaResources();

				//FileLoader.LoadJS("_Resources/Froala/js/froala_editor.min.js", () => {
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/align.min.js");
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/char_counter.min.js");
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/code_beautifier.min.js");
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/code_view.min.js");
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/colors.min.js");
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/emoticons.min.js");
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/entities.min.js");
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/file.min.js");
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/font_family.min.js");
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/font_size.min.js");
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/fullscreen.min.js");

				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/image.min.js", () => {
				//        FileLoader.LoadJS("_Resources/Froala/js/plugins/image_manager.min.js");
				//    });

				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/inline_style.min.js");
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/line_breaker.min.js");
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/link.min.js");
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/lists.min.js");
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/paragraph_format.min.js");
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/paragraph_style.min.js");
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/quick_insert.min.js");
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/quote.min.js");
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/table.min.js");
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/save.min.js");
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/url.min.js");
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/video.min.js");

				//    FileLoader.LoadCSS("_Resources/Froala/css/font-awesome.min.css", () => {
				//        FileLoader.LoadCSS("_Resources/Froala/css/froala_editor.css");
				//        FileLoader.LoadCSS("_Resources/Froala/css/froala_style.css");
				//        FileLoader.LoadCSS("_Resources/Froala/css/plugins/code_view.css");
				//        FileLoader.LoadCSS("_Resources/Froala/css/plugins/colors.css");
				//        FileLoader.LoadCSS("_Resources/Froala/css/plugins/emoticons.css");
				//        FileLoader.LoadCSS("_Resources/Froala/css/plugins/image_manager.css");
				//        FileLoader.LoadCSS("_Resources/Froala/css/plugins/image.css");
				//        FileLoader.LoadCSS("_Resources/Froala/css/plugins/line_breaker.css");
				//        FileLoader.LoadCSS("_Resources/Froala/css/plugins/table.css");
				//        FileLoader.LoadCSS("_Resources/Froala/css/plugins/char_counter.css");
				//        FileLoader.LoadCSS("_Resources/Froala/css/plugins/video.css");
				//        FileLoader.LoadCSS("_Resources/Froala/css/plugins/fullscreen.css");
				//        FileLoader.LoadCSS("_Resources/Froala/css/plugins/quick_insert.css");
				//        FileLoader.LoadCSS("_Resources/Froala/css/plugins/file.css");
				//        FileLoader.LoadCSS("_Resources/Froala/css/codemirror.min.css");

				//        resolve(true);
				//    });
				//});

				//this.LoadJS("Froala/js/plugins/image.min.js", this.OnImageMinLoaded);
				//this.LoadCSS("https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.4.0/css/font-awesome.min.css");
			}
		});
	}

	public static LoadStimulSoftResources(): Promise<boolean> {
		return new Promise((resolve, reject) => {
			if (this.IsFroalaFilesLoaded) {
				resolve(true);
			} else {
				this.IsFroalaFilesLoaded = true;

				var i: number = 0;

				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Stimulsoft/js/jquery.1.8.3.js'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Stimulsoft/js/stimulsoft.reports.js'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Stimulsoft/js/stimulsoft.reports.maps.js'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Stimulsoft/js/stimulsoft.viewer.js'));
				this.AllFroalaResources.push(new ResourceFile(++i, '_Resources/Stimulsoft/js/stimulsoft.designer.js'));
				//this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Stimulsoft/js/stimulsoft.designer.runtime.js"));

				//this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Stimulsoft/css/stimulsoft.designer.office2013.css"));
				this.AllFroalaResources.push(
					new ResourceFile(++i, '_Resources/Stimulsoft/css/stimulsoft.designer.office2013.darkgrayblue.css'),
				);

				this.AllFroalaResources.push(
					new ResourceFile(++i, '_Resources/Stimulsoft/css/stimulsoft.designer.runtime.office2013.darkgrayblue.css'),
				);

				this.AllFroalaResources.push(
					new ResourceFile(++i, '_Resources/Stimulsoft/css/stimulsoft.viewer.office2013.darkgrayblue.css'),
				);

				//this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Stimulsoft/css/stimulsoft.designer.office2013.lightgrayblue.css"));

				//this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Stimulsoft/css/stimulsoft.designer.office2013.lightgrayblue.css"));

				//this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Stimulsoft/css/stimulsoft.viewer.office2013.darkgrayblue.css"));

				// this.ResourcesLoaded.subscribe(s => {
				//     resolve(true);
				// });

				this.RunLoadingFroalaResources();

				//FileLoader.LoadJS("_Resources/Froala/js/froala_editor.min.js", () => {
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/align.min.js");
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/char_counter.min.js");
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/code_beautifier.min.js");
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/code_view.min.js");
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/colors.min.js");
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/emoticons.min.js");
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/entities.min.js");
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/file.min.js");
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/font_family.min.js");
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/font_size.min.js");
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/fullscreen.min.js");

				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/image.min.js", () => {
				//        FileLoader.LoadJS("_Resources/Froala/js/plugins/image_manager.min.js");
				//    });

				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/inline_style.min.js");
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/line_breaker.min.js");
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/link.min.js");
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/lists.min.js");
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/paragraph_format.min.js");
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/paragraph_style.min.js");
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/quick_insert.min.js");
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/quote.min.js");
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/table.min.js");
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/save.min.js");
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/url.min.js");
				//    FileLoader.LoadJS("_Resources/Froala/js/plugins/video.min.js");

				//    FileLoader.LoadCSS("_Resources/Froala/css/font-awesome.min.css", () => {
				//        FileLoader.LoadCSS("_Resources/Froala/css/froala_editor.css");
				//        FileLoader.LoadCSS("_Resources/Froala/css/froala_style.css");
				//        FileLoader.LoadCSS("_Resources/Froala/css/plugins/code_view.css");
				//        FileLoader.LoadCSS("_Resources/Froala/css/plugins/colors.css");
				//        FileLoader.LoadCSS("_Resources/Froala/css/plugins/emoticons.css");
				//        FileLoader.LoadCSS("_Resources/Froala/css/plugins/image_manager.css");
				//        FileLoader.LoadCSS("_Resources/Froala/css/plugins/image.css");
				//        FileLoader.LoadCSS("_Resources/Froala/css/plugins/line_breaker.css");
				//        FileLoader.LoadCSS("_Resources/Froala/css/plugins/table.css");
				//        FileLoader.LoadCSS("_Resources/Froala/css/plugins/char_counter.css");
				//        FileLoader.LoadCSS("_Resources/Froala/css/plugins/video.css");
				//        FileLoader.LoadCSS("_Resources/Froala/css/plugins/fullscreen.css");
				//        FileLoader.LoadCSS("_Resources/Froala/css/plugins/quick_insert.css");
				//        FileLoader.LoadCSS("_Resources/Froala/css/plugins/file.css");
				//        FileLoader.LoadCSS("_Resources/Froala/css/codemirror.min.css");

				//        resolve(true);
				//    });
				//});

				//this.LoadJS("Froala/js/plugins/image.min.js", this.OnImageMinLoaded);
				//this.LoadCSS("https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.4.0/css/font-awesome.min.css");
			}
		});
	}

	//public static OnImageMinLoaded() {
	//    FileLoader.LoadJS("Froala/js/plugins/image_manager.min.js");
	//}

	public static LoadJS(fileURL: string, callback: any = null) {
		if (fileURL) {
			var script = document.createElement('script');
			script.setAttribute('type', 'text/javascript');
			script.setAttribute('src', fileURL);

			if (callback) {
				script.onload = callback;
			}

			document.getElementsByTagName('head')[0].appendChild(script);
		}
	}
	public static LoadCSS(fileURL: string, callback: any = null) {
		if (fileURL) {
			var link = document.createElement('link');
			link.setAttribute('rel', 'stylesheet');
			link.setAttribute('type', 'text/css');
			link.setAttribute('href', fileURL);

			if (callback) {
				link.onload = callback;
			}

			document.getElementsByTagName('head')[0].appendChild(link);
		}
	}
	private static RunLoadingFroalaResources() {
		var file: ResourceFile = this.AllFroalaResources.sort((a, b) => {
			return a.Index - b.Index;
		}).filter((f) => f.IsLoaded == false)[0];

		if (file) {
			if (file.URL.indexOf('.css') > -1) {
				FileLoader.LoadCSS(file.URL, () => {
					file.IsLoaded = true;

					FileLoader.RunLoadingFroalaResources();
				});
			} else {
				FileLoader.LoadJS(file.URL, () => {
					file.IsLoaded = true;

					FileLoader.RunLoadingFroalaResources();
				});
			}
		} else {
			// this.ResourcesLoaded.emit(true);
		}
	}
}
class ResourceFile {
	public URL: string;
	public IsLoaded: boolean;
	public Index: number;
	constructor(index: number, url: string) {
		this.URL = url;
		this.Index = index;
		this.IsLoaded = false;
	}
}

////--- itzik : Why Array Tool ? - better extend !!!!
class AmitalList<Item> extends Array<Item> {
	Max<Selected>(select: (item: Item) => Selected): Item {
		return this.reduce((a: Item, b: Item): Item => (select(a) > select(b) ? a : b));
	}
}
////--- itzik : Why Array Tool ? - better extend !!!!

export class Pattern {
	public static Text: string = 'lol';
}
export class DateParts {
	public Year: number = 0;
	public Month: number = 0;
	public Day: number = 0;
	public Hours: number = 0;
	public Hours12: number = 0;
	public Minutes: number = 0;
	public Seconds: number = 0;
	public Milliseconds: number = 0;
	public DateObject: Date = null;
	public DateTicks: number = 0;
	public LocalYear: number = 0;
	public LocalMonth: number = 0;
	public LocalDay: number = 0;
	public TotalMinutes: number = 0;
}
export class DateFormats {
	public AMPM: string;
	public DateString: string;
	public ShortDateString: string;
	public ShortTimeString: string;
	public ShortTimeString12: string;
	public LongTimeString: string;
	public LongTimeString12: string;
	public LocalDateString: string;
	public LocalTimeString: string;
	public LocalDateTimeString: string;
	public DayName: string;
	public MonthName: string;
	public DayNameShort: string;
	public MonthNameShort: string;
	public DateParts: DateParts;
}
export class ImageTool {
	public static AllImagesInfo: ImageInfo[] = [];
	public static IsImageExists(image_url: string) {
		var isDisplay = false;

		if (!AppTool.IsNullOrEmpty(image_url)) {
			var imageInfo = this.AllImagesInfo.filter((f) => f.Url.toLowerCase() == image_url.toLowerCase())[0];
			if (imageInfo) {
				isDisplay = imageInfo.Exists;
			} else {
				var http = new XMLHttpRequest();
				http.open('HEAD', image_url, false);
				http.send();
				isDisplay = http.status == 404 ? false : true;
				http.abort();

				imageInfo = new ImageInfo();
				imageInfo.Url = image_url.toLowerCase();
				imageInfo.Exists = isDisplay;
				this.AllImagesInfo.push(imageInfo);
			}

			if (this.AllImagesInfo.length >= 250) {
				this.AllImagesInfo = [];
			}
		}

		return isDisplay;
	}
}
class ImageInfo {
	public Url: string = null;
	public Exists: boolean = null;
}
class SortingClass {
	public Item: any;
	public SortingValue: number = 0;
	constructor(item: any, fieldname: string) {
		this.Item = item;
		this.SortingValue = DateTool.GetDateParts(item[fieldname]).DateTicks;
	}
}
