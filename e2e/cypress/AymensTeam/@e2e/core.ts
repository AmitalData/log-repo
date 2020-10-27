export class Random {
    public static GetRandomNumber() {
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
    public static GetRandom() {
        var result = '';
        result += Math.floor(Math.random() * 1000000).toString();
        return result;
    }
}