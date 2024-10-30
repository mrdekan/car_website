const fuelName = (id) => ["Газ", "Газ/Бензин", "Бензин", "Дизель", "Гібрид", "Електро"][id - 1];
const transmissionName = (id) => id == 1 ? "Механічна" : "Автомат";
const drivelineName = (id) => ["Передній", "Задній", "Повний"][id - 1];
const formatNumberWithThousandsSeparator = (number) => number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ' ');
export { fuelName, transmissionName, drivelineName, formatNumberWithThousandsSeparator };