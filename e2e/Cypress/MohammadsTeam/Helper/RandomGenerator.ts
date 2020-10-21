export class RandomGenerator {

    constructor() { }


GenerateRandomNumber(){

    var result = '';
    var characters = '0A1BC2DE3F4G5HI6J7KL8M9NOP1Q0R3S9T6U4V7W5X8Y2Z';
    var charactersLength = characters.length;
    for (var i = 0; i < 5; i++) {
        result += characters.charAt(Math.floor(Math.random() * charactersLength));
    }
    return result;
}

GenerateRandomNumberACC(){
var result = '';
var characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
var charactersLength = characters.length;
for (var i = 0; i < 2; i++) {
    result += characters.charAt(Math.floor(Math.random() * charactersLength));
}
result += Math.floor(Math.random() * 1000000).toString();
for (var i = 0; i < 2; i++) {
    result += characters.charAt(Math.floor(Math.random() * charactersLength));
}

return result;
}
 RandomNum() {
    var result = '';
    var characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
    var charactersLength = characters.length;
    for (var i = 0; i < 2; i++) {
        result += characters.charAt(Math.floor(Math.random() * charactersLength));
    }
    result += Math.floor(Math.random() * 1000000).toString();
    for (var i = 0; i < 2; i++) {
        result += characters.charAt(Math.floor(Math.random() * charactersLength));
    }

    return result;
}


GenerateRandomNumberByDate(){
var date = new Date();
var components = [
    date.getYear(),
    date.getMonth(),
    date.getDate(),
    date.getHours(),
    date.getMinutes(),
    date.getSeconds(),
    date.getMilliseconds()
];

var id = components.join("");
	
}


}