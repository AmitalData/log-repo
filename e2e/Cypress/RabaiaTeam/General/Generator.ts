export function GenerateRandomString(length: number, upperCase: boolean){
    let randomString = ""
    let possible = "abcdefghijklmnopqrstuvwxyz"

    for (let i = 0; i < length; i++)
        randomString += possible.charAt(Math.floor(Math.random() * possible.length))

    if (upperCase) {
        randomString = randomString.toUpperCase()
    }

    return randomString
}

export function GenerateRandomNumber(minimum:number, maximum: number) {
    minimum = Math.ceil(minimum)
    maximum = Math.floor(maximum)

    return (Math.floor(Math.random() * (maximum - minimum + 1) + minimum))
}