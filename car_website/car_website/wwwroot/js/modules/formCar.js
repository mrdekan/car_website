function fuelName(id) {
    switch (id) {
        case 1:
            return "Газ";
        case 2:
            return "Газ/Бензин";
        case 3:
            return "Бензин";
        case 4:
            return "Дизель";
        case 5:
            return "Гібрид";
        case 6:
            return "Електро";
    }
}
function transmissionName(id) {
    switch (id) {
        case 1:
            return "Механічна";
        case 2:
            return "Автомат";
    }
}
function drivelineName(id) {
    switch (id) {
        case 1:
            return "Передній";
        case 2:
            return "Задній";
        case 3:
            return "Повний";
    }
}
function formatNumberWithThousandsSeparator(number) {
    return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ' ');
}
export { fuelName, transmissionName, drivelineName, formatNumberWithThousandsSeparator };