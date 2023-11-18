//#region constants
const svgCodes = {
    driveline: `<svg version="1.0" xmlns="http://www.w3.org/2000/svg"width="24" height="24" viewBox="0 0 512.000000 512.000000"preserveAspectRatio="xMidYMid meet"><g transform="translate(0.000000,512.000000) scale(0.100000,-0.100000)" fill="currentColor" stroke="none"><path d="M950 5007 c-50 -16 -136 -103 -149 -152 -7 -26 -11 -208 -11 -531 l0 -490 28 -55 c33 -66 93 -113 162 -128 80 -17 356 -14 419 5 64 19 127 73 155 134 18 39 21 70 24 243 l4 197 143 0 142 0 6 -58 c5 -66 32 -124 74 -164 36 -35 109 -68 151 -68 18 0 40 -8 52 -20 19 -19 20 -33 20 -252 0 -268 5 -285 83 -354 50 -44 102 -64 166 -64 l41 0 0 -1180 0 -1180 -439 0 -439 0 -4 198 c-3 172 -6 203 -24 242 -28 61 -91 115 -155 134 -72 22 -364 22 -435 1 -66 -19 -121 -67 -150 -129 l-24 -51 0 -490 c0 -323 4 -504 11 -530 14 -53 99 -136 155 -153 59 -17 406 -16 457 2 57 21 100 58 132 117 29 54 29 55 33 257 l4 202 978 0 978 0 4 -202 c4 -202 4 -203 33 -257 32 -59 75 -96 132 -117 58 -20 404 -19 466 2 56 19 131 97 147 152 6 23 10 225 10 527 l0 490 -24 51 c-29 62 -84 110 -150 129 -71 21 -363 21 -435 -1 -64 -19 -127 -73 -155 -134 -18 -39 -21 -70 -24 -242 l-4 -198 -439 0 -439 0 0 1180 0 1180 43 0 c97 0 188 58 228 145 16 35 19 68 19 273 0 219 1 233 20 252 12 12 34 20 53 20 41 0 114 33 150 68 42 40 69 98 74 164 l6 58 142 0 143 0 4 -197 c3 -173 6 -204 24 -243 28 -61 91 -115 155 -134 63 -19 339 -22 419 -5 69 15 129 62 163 128 l27 55 0 490 c0 303 -4 505 -10 528 -16 55 -91 133 -147 152 -62 21 -408 22 -466 2 -57 -21 -100 -58 -132 -117 -29 -54 -29 -55 -33 -256 l-4 -203 -144 0 -144 0 0 38 c0 96 -50 182 -132 227 l-53 30 -505 0 c-502 0 -505 0 -546 -22 -87 -47 -132 -115 -141 -214 l-6 -59 -142 0 -143 0 -4 203 c-4 201 -4 202 -33 256 -32 59 -75 96 -132 117 -48 17 -411 17 -463 1z m420 -206 c6 -13 10 -180 10 -474 0 -429 -1 -455 -19 -471 -16 -14 -40 -16 -187 -14 -168 3 -168 3 -181 28 -10 19 -12 126 -11 478 3 390 5 454 18 462 8 5 92 10 187 10 160 0 173 -1 183 -19z m2750 9 c13 -8 15 -72 18 -462 1 -352 -1 -459 -11 -478 -13 -25 -13 -25 -181 -28 -147 -2 -171 0 -187 14 -18 16 -19 42 -19 471 0 294 4 461 10 474 10 18 23 19 183 19 95 0 179 -5 187 -10z m-1089 -290 c18 -10 19 -23 19 -184 0 -196 1 -192 -77 -205 -95 -15 -155 -60 -194 -146 -16 -35 -19 -69 -19 -268 0 -138 -4 -236 -10 -248 -10 -18 -23 -19 -190 -19 -167 0 -180 1 -190 19 -6 12 -10 110 -10 248 0 199 -3 233 -19 268 -39 86 -99 131 -194 146 -78 13 -77 9 -77 205 0 154 2 174 18 183 24 14 916 15 943 1z m-1669 -3257 c17 -15 18 -43 18 -470 0 -294 -4 -461 -10 -474 -10 -18 -23 -19 -183 -19 -95 0 -179 5 -187 10 -13 8 -15 72 -15 473 0 379 3 467 14 480 20 25 335 25 363 0z m2765 -13 c10 -19 12 -126 11 -478 -3 -390 -5 -454 -18 -462 -8 -5 -92 -10 -187 -10 -160 0 -173 1 -183 19 -6 13 -10 179 -10 472 0 409 2 454 17 471 15 17 32 18 187 16 l170 -3 13 -25z"/></g></svg>`,
    car: `<svg width="24" height="24" viewBox="0 0 30 30" fill="none" xmlns="http://www.w3.org/2000/svg"><path d="M24.9997 14.6876H4.9997C4.7122 14.6876 4.4497 14.5626 4.2747 14.3376C4.0997 14.1251 4.0247 13.8251 4.0872 13.5501L5.4997 6.8001C5.9622 4.6126 6.8997 2.6001 10.6122 2.6001H19.3997C23.1122 2.6001 24.0497 4.6251 24.5122 6.8001L25.9247 13.5626C25.9872 13.8376 25.9122 14.1251 25.7372 14.3501C25.5497 14.5626 25.2872 14.6876 24.9997 14.6876ZM6.1497 12.8126H23.8372L22.6622 7.1876C22.3122 5.5501 21.8997 4.4751 19.3872 4.4751H10.6122C8.0997 4.4751 7.6872 5.5501 7.3372 7.1876L6.1497 12.8126Z" fill="currentColor"/><path d="M24.9495 28.4376H22.5995C20.5745 28.4376 20.187 27.2751 19.937 26.5126L19.687 25.7626C19.362 24.8126 19.3245 24.6876 18.1995 24.6876H11.7995C10.6745 24.6876 10.5995 24.9001 10.312 25.7626L10.062 26.5126C9.7995 27.2876 9.4245 28.4376 7.3995 28.4376H5.0495C4.062 28.4376 3.112 28.0251 2.4495 27.3001C1.7995 26.5876 1.487 25.6376 1.5745 24.6876L2.2745 17.0751C2.462 15.0126 3.012 12.8126 7.0245 12.8126H22.9745C26.987 12.8126 27.5245 15.0126 27.7245 17.0751L28.4245 24.6876C28.512 25.6376 28.1995 26.5876 27.5495 27.3001C26.887 28.0251 25.937 28.4376 24.9495 28.4376ZM11.7995 22.8126H18.1995C20.4745 22.8126 21.012 23.8251 21.462 25.1501L21.7245 25.9251C21.937 26.5626 21.937 26.5751 22.612 26.5751H24.962C25.4245 26.5751 25.862 26.3876 26.1745 26.0501C26.4745 25.7251 26.612 25.3126 26.5745 24.8751L25.8745 17.2626C25.712 15.5751 25.512 14.7001 22.9995 14.7001H7.0245C4.4995 14.7001 4.2995 15.5751 4.1495 17.2626L3.4495 24.8751C3.412 25.3126 3.5495 25.7251 3.8495 26.0501C4.1495 26.3876 4.5995 26.5751 5.062 26.5751H7.412C8.087 26.5751 8.087 26.5626 8.2995 25.9376L8.5495 25.1751C8.862 24.1751 9.3245 22.8126 11.7995 22.8126Z" fill="currentColor"/><path d="M4.99951 10.9376H3.74951C3.23701 10.9376 2.81201 10.5126 2.81201 10.0001C2.81201 9.48759 3.23701 9.06259 3.74951 9.06259H4.99951C5.51201 9.06259 5.93701 9.48759 5.93701 10.0001C5.93701 10.5126 5.51201 10.9376 4.99951 10.9376Z" fill="currentColor"/><path d="M26.2495 10.9376H24.9995C24.487 10.9376 24.062 10.5126 24.062 10.0001C24.062 9.48759 24.487 9.06259 24.9995 9.06259H26.2495C26.762 9.06259 27.187 9.48759 27.187 10.0001C27.187 10.5126 26.762 10.9376 26.2495 10.9376Z" fill="currentColor"/><path d="M14.9995 7.18759C14.487 7.18759 14.062 6.76259 14.062 6.25009V3.75009C14.062 3.23759 14.487 2.81259 14.9995 2.81259C15.512 2.81259 15.937 3.23759 15.937 3.75009V6.25009C15.937 6.76259 15.512 7.18759 14.9995 7.18759Z" fill="currentColor"/><path d="M16.8745 7.18759H13.1245C12.612 7.18759 12.187 6.76259 12.187 6.25009C12.187 5.73759 12.612 5.31259 13.1245 5.31259H16.8745C17.387 5.31259 17.812 5.73759 17.812 6.25009C17.812 6.76259 17.387 7.18759 16.8745 7.18759Z" fill="currentColor"/><path d="M11.2495 19.6876H7.49951C6.98701 19.6876 6.56201 19.2626 6.56201 18.7501C6.56201 18.2376 6.98701 17.8126 7.49951 17.8126H11.2495C11.762 17.8126 12.187 18.2376 12.187 18.7501C12.187 19.2626 11.762 19.6876 11.2495 19.6876Z" fill="currentColor"/><path d="M22.4995 19.6876H18.7495C18.237 19.6876 17.812 19.2626 17.812 18.7501C17.812 18.2376 18.237 17.8126 18.7495 17.8126H22.4995C23.012 17.8126 23.437 18.2376 23.437 18.7501C23.437 19.2626 23.012 19.6876 22.4995 19.6876Z" fill="currentColor"/></svg>`,
    fuel: `<svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg"><path d="M17.5 22.75H3.5C3.09 22.75 2.75 22.41 2.75 22V5.00001C2.75 2.76001 4.26 1.25001 6.5 1.25001H14.5C16.74 1.25001 18.25 2.76001 18.25 5.00001V22C18.25 22.41 17.91 22.75 17.5 22.75ZM4.25 21.25H16.75V5.00001C16.75 3.59001 15.91 2.75001 14.5 2.75001H6.5C5.09 2.75001 4.25 3.59001 4.25 5.00001V21.25Z" fill="currentColor"/><path d="M18.9999 22.75H1.99994C1.58994 22.75 1.24994 22.41 1.24994 22C1.24994 21.59 1.58994 21.25 1.99994 21.25H18.9999C19.4099 21.25 19.7499 21.59 19.7499 22C19.7499 22.41 19.4099 22.75 18.9999 22.75Z" fill="currentColor"/><path d="M12.6102 10.75H8.38023C6.75023 10.75 5.74023 9.74 5.74023 8.11V6.88C5.74023 5.25 6.75023 4.24001 8.38023 4.24001H12.6102C14.2402 4.24001 15.2502 5.25 15.2502 6.88V8.11C15.2502 9.74 14.2402 10.75 12.6102 10.75ZM8.39023 5.75C7.58023 5.75 7.25023 6.08 7.25023 6.89V8.12C7.25023 8.93 7.58023 9.25999 8.39023 9.25999H12.6202C13.4302 9.25999 13.7602 8.93 13.7602 8.12V6.89C13.7602 6.08 13.4302 5.75 12.6202 5.75H8.39023Z" fill="currentColor"/><path d="M9.49994 13.75H6.49994C6.08994 13.75 5.74994 13.41 5.74994 13C5.74994 12.59 6.08994 12.25 6.49994 12.25H9.49994C9.90994 12.25 10.2499 12.59 10.2499 13C10.2499 13.41 9.90994 13.75 9.49994 13.75Z" fill="currentColor"/><path d="M17.5 16.76C17.09 16.76 16.75 16.43 16.75 16.01C16.75 15.6 17.08 15.26 17.5 15.26L21.25 15.25V10.46L19.66 9.66998C19.29 9.47998 19.14 9.02998 19.32 8.65998C19.51 8.28998 19.96 8.13999 20.33 8.31999L22.33 9.31999C22.58 9.44999 22.74 9.70998 22.74 9.98998V15.99C22.74 16.4 22.41 16.74 21.99 16.74L17.5 16.76Z" fill="currentColor"/></svg>`,
    transmission: `<svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg"><path d="M12 15.75C9.93 15.75 8.25 14.07 8.25 12C8.25 9.93 9.93 8.25 12 8.25C14.07 8.25 15.75 9.93 15.75 12C15.75 14.07 14.07 15.75 12 15.75ZM12 9.75C10.76 9.75 9.75 10.76 9.75 12C9.75 13.24 10.76 14.25 12 14.25C13.24 14.25 14.25 13.24 14.25 12C14.25 10.76 13.24 9.75 12 9.75Z" fill="currentColor"/><path d="M15.21 22.19C15 22.19 14.79 22.16 14.58 22.11C13.96 21.94 13.44 21.55 13.11 21L12.99 20.8C12.4 19.78 11.59 19.78 11 20.8L10.89 20.99C10.56 21.55 10.04 21.95 9.42 22.11C8.79 22.28 8.14 22.19 7.59 21.86L5.87 20.87C5.26 20.52 4.82 19.95 4.63 19.26C4.45 18.57 4.54 17.86 4.89 17.25C5.18 16.74 5.26 16.28 5.09 15.99C4.92 15.7 4.49 15.53 3.9 15.53C2.44 15.53 1.25 14.34 1.25 12.88V11.12C1.25 9.66 2.44 8.47 3.9 8.47C4.49 8.47 4.92 8.3 5.09 8.01C5.26 7.72 5.19 7.26 4.89 6.75C4.54 6.14 4.45 5.42 4.63 4.74C4.81 4.05 5.25 3.48 5.87 3.13L7.6 2.14C8.73 1.47 10.22 1.86 10.9 3.01L11.02 3.21C11.61 4.23 12.42 4.23 13.01 3.21L13.12 3.02C13.8 1.86 15.29 1.47 16.43 2.15L18.15 3.14C18.76 3.49 19.2 4.06 19.39 4.75C19.57 5.44 19.48 6.15 19.13 6.76C18.84 7.27 18.76 7.73 18.93 8.02C19.1 8.31 19.53 8.48 20.12 8.48C21.58 8.48 22.77 9.67 22.77 11.13V12.89C22.77 14.35 21.58 15.54 20.12 15.54C19.53 15.54 19.1 15.71 18.93 16C18.76 16.29 18.83 16.75 19.13 17.26C19.48 17.87 19.58 18.59 19.39 19.27C19.21 19.96 18.77 20.53 18.15 20.88L16.42 21.87C16.04 22.08 15.63 22.19 15.21 22.19ZM12 18.49C12.89 18.49 13.72 19.05 14.29 20.04L14.4 20.23C14.52 20.44 14.72 20.59 14.96 20.65C15.2 20.71 15.44 20.68 15.64 20.56L17.37 19.56C17.63 19.41 17.83 19.16 17.91 18.86C17.99 18.56 17.95 18.25 17.8 17.99C17.23 17.01 17.16 16 17.6 15.23C18.04 14.46 18.95 14.02 20.09 14.02C20.73 14.02 21.24 13.51 21.24 12.87V11.11C21.24 10.48 20.73 9.96 20.09 9.96C18.95 9.96 18.04 9.52 17.6 8.75C17.16 7.98 17.23 6.97 17.8 5.99C17.95 5.73 17.99 5.42 17.91 5.12C17.83 4.82 17.64 4.58 17.38 4.42L15.65 3.43C15.22 3.17 14.65 3.32 14.39 3.76L14.28 3.95C13.71 4.94 12.88 5.5 11.99 5.5C11.1 5.5 10.27 4.94 9.7 3.95L9.59 3.75C9.34 3.33 8.78 3.18 8.35 3.43L6.62 4.43C6.36 4.58 6.16 4.83 6.08 5.13C6 5.43 6.04 5.74 6.19 6C6.76 6.98 6.83 7.99 6.39 8.76C5.95 9.53 5.04 9.97 3.9 9.97C3.26 9.97 2.75 10.48 2.75 11.12V12.88C2.75 13.51 3.26 14.03 3.9 14.03C5.04 14.03 5.95 14.47 6.39 15.24C6.83 16.01 6.76 17.02 6.19 18C6.04 18.26 6 18.57 6.08 18.87C6.16 19.17 6.35 19.41 6.61 19.57L8.34 20.56C8.55 20.69 8.8 20.72 9.03 20.66C9.27 20.6 9.47 20.44 9.6 20.23L9.71 20.04C10.28 19.06 11.11 18.49 12 18.49Z" fill="currentColor"/></svg>`,
    race: `<svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg"><g clip-path="url(#clip0_14_212)"><path d="M15.1486 9.3893C15.1486 9.3893 13.534 13.9404 12.7667 14.735C11.9994 15.5296 10.7333 15.5517 9.93872 14.7844C9.14412 14.0171 9.12202 12.7509 9.88932 11.9564C10.6566 11.1618 15.1486 9.3893 15.1486 9.3893Z" stroke="currentColor" stroke-width="2" stroke-linejoin="round"/><path d="M19.4246 19.4246C21.3248 17.5245 22.5 14.8995 22.5 12C22.5 6.201 17.799 1.5 12 1.5C6.201 1.5 1.5 6.201 1.5 12C1.5 14.8995 2.67525 17.5245 4.57538 19.4246" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/><path d="M12 2V4" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/><path d="M19.4227 5.57105L17.8684 6.82965" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/><path d="M21.2613 13.6164L19.3126 13.1665" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/><path d="M2.73877 13.6164L4.68751 13.1665" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/><path d="M4.57733 5.57105L6.13162 6.82965" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/></g><defs><clipPath id="clip0_14_212"><rect width="24" height="24" fill="white"/></clipPath></defs></svg>`
}
const selectBrandsBtn = document.getElementById("brandsButton"),
searchBrandInp = document.getElementById("searchCar"),
brandsOptions = document.getElementById("brands"),
selectModelsBtn = document.getElementById("modelsButton"),
searchModelInp = document.getElementById("searchModel"),
modelsOptions = document.getElementById("models"),
selectBodiesBtn = document.getElementById("bodiesButton"),
bodiesOptions = document.getElementById("bodies"),
selectTransmissionsBtn = document.getElementById("transmissionsButton"),
transmissionsOptions = document.getElementById("transmissions"),
selectFuelsBtn = document.getElementById("fuelsButton"),
fuelsOptions = document.getElementById("fuels"),
selectDrivelinesBtn = document.getElementById("drivelinesButton"),
drivelinesOptions = document.getElementById("drivelines"),
selectSortingBtn = document.getElementById("sortingButton"),
sortingOptions = document.getElementById("sortings"),
apply_button = document.getElementById("refresh_cars"),
year_min_select = document.getElementById("year_min-select"),
year_max_select = document.getElementById("year_max-select"),
price_max_input = document.getElementById("price_max-input"),
price_min_input = document.getElementById("price_min-input"),
race_max_input = document.getElementById("race_max-input"),
race_min_input = document.getElementById("race_min-input"),
engineVolume_min_input = document.getElementById("engineVolume_min-input"),
engineVolume_max_input = document.getElementById("engineVolume_max-input"),
clear_filters = document.getElementById("clear_filters"),
pages_buttons_containers = document.getElementsByClassName("pages_buttons"),
minYearValue = document.getElementById('min-year-value'),
minYearSlider = document.getElementById('min-year-slider'),
maxYearValue = document.getElementById('max-year-value'),
maxYearSlider = document.getElementById('max-year-slider'),
minMaxYear = document.getElementById('min-max-year'),
maxMinYear = document.getElementById('max-min-year'),
currentYear = minYearSlider.getAttribute('max'),
yearLabel = document.getElementById('year-label'),
openFilter = document.getElementById('open-filters'),
filter = document.querySelector('.filters_wrapper'),
engineVolumeLbl = document.getElementById('engine-volume-lbl');
//#endregion

//#region Selects content
let carsPage = 1;
let likeButtons = document.getElementsByClassName("like_cars");
let brands = ["Усі"], models = ["Усі"];
let bodies = ["Усі", "Седан", "Позашляховик", "Мінівен", "Хетчбек", "Універсал", "Купе", "Кабріолет", "Пікап", "Ліфтбек", "Автобус"];
let transmissions = ["Усі", "Механічна", "Автоматична"];
let sortings = ["За датою додавання", "За спаданням ціни", "За зростанням ціни"];
let fuels = {};
fuels["Усі"] = 0;
fuels["Бензин"] = 3;
fuels["Дизель"] = 4;
fuels["Газ"] = 1;
fuels["Газ/Бензин"] = 2;
fuels["Гібрид"] = 5;
fuels["Електро"] = 6;
let drivelines = ["Усі", "Передній", "Задній", "Повний"];
let modelsCache = {};
let carsCache = {};
let pages = 0;
let isElectro = false;
//#endregion

//#region Functions' calls
function createLi(text) {
    let li = document.createElement('LI');
    li.innerText = text;
    return li;
}
function getKeyByValue(object, value) {
    for (const key in object)
        if (object[key] === value)
            return key;
    return null;
}
document.addEventListener('DOMContentLoaded', function () {
    let lastCarsString = sessionStorage.getItem("last");
    let lastCars = JSON.parse(lastCarsString);
    if (lastCars && lastCars.data) {
        carsPage = lastCars.data.page;
        updatePagesButtons(lastCars.data.pages);
        pages = lastCars.data.pages;
        carList.innerHTML = "";
        lastCars.data.cars.forEach(car => {
            const block = formCar(car);
            carList.innerHTML += block;
        });
        if (lastCars.data.cars == null || lastCars.data.cars.length == 0) {
            updatePagesButtons(0);
            carList.innerHTML = `<div class="cars-not-found"><h3 class="warning-text">Нічого не знайдено</h3></div>`;
        }
        carsCache[lastCars.filters] = lastCars.data;
        updateLikeButtons();
        if (lastCars.filters.driveLine != 0)
            updateDriveline(createLi(drivelines[lastCars.filters.driveLine]));
        if (lastCars.filters.body != 0)
            updateBody(createLi(bodies[lastCars.filters.body]));
        if (lastCars.filters.carTransmission != 0)
            updateTransmission(createLi(transmissions[lastCars.filters.carTransmission]));
        if (lastCars.filters.fuel != 0)
            updateFuel(createLi(getKeyByValue(fuels, lastCars.filters.fuel)));
        if (lastCars.filters.maxEngineCapacity != 0)
            engineVolume_max_input.value = lastCars.filters.maxEngineCapacity;
        if (lastCars.filters.minEngineCapacity != 0)
            engineVolume_min_input.value = lastCars.filters.minEngineCapacity;
        if (lastCars.filters.minPrice != 0)
            price_min_input.value = lastCars.filters.minPrice;
        if (lastCars.filters.maxPrice != 0)
            price_max_input.value = lastCars.filters.maxPrice;
        if (lastCars.filters.minMileage != 0)
            race_min_input.value = lastCars.filters.minMileage;
        if (lastCars.filters.maxMileage != 0)
            race_max_input.value = lastCars.filters.maxMileage;
        if (lastCars.filters.minYear != 2000) {
            minYearSlider.value = lastCars.filters.minYear;
            maxYearSlider.setAttribute('min', lastCars.filters.minYear);
            updateYearLabel();
        }
        if (lastCars.filters.maxYear != maxYearSlider.getAttribute('max')) {
            maxYearSlider.value = lastCars.filters.maxYear;
            minYearSlider.setAttribute('max', lastCars.filters.maxYear);
            updateYearLabel();
        }
        if (lastCars.filters.sortingType != 0) {
            let selectedLi = createLi(sortings[lastCars.filters.sortingType]);
            addSorting(selectedLi.innerText);
            sortingOptions.parentElement.classList.remove("active");
            selectSortingBtn.classList.remove("active");
            selectSortingBtn.firstElementChild.innerText = selectedLi.innerText;
        }
        getMarks();
        if (lastCars.filters.brand != "Усі") {
            updateName(createLi(lastCars.filters.brand));
            if (lastCars.filters.model != "Усі")
                getModelsOfMark(lastCars.filters.model);
            else
                getModelsOfMark();
        }
        isFilterClear();
    }
    else
        applyFilter();
});
getMarks();
if (selectBrandsBtn.firstElementChild.innerText != "Усі")
    getModelsOfMark();
addBrand();
addModel();
addBody();
addTransmission();
addFuel();
addDriveline();
addSorting();
isFilterClear();
//#endregion
//#region Pages & Likes
openFilter.addEventListener('click', () => filter.classList.toggle("open"));
function updateLikeButtons() {
    likeButtons = document.getElementsByClassName("car_container-right-like-cars");
    Array.from(likeButtons).forEach(like => {
        like.addEventListener('change', function (event) {
            fetch(`/car/like?carId=${event.target.getAttribute('carId')}&isLiked=${like.checked}`)
                .then(response => response.json())
                .then(data => {
                    if (data.success == false) {
                        like.checked = !like.checked;
                        window.location.href = '/User/Login';
                    }
                })
                .catch(error => console.error("An error occurred while retrieving data:", error));
        });
    });
}
function updatePagesButtons(number) {
    Array.from(pages_buttons_containers).forEach(buttons_container => {
        buttons_container.innerHTML = "";
        if (number > 1) {
            if (number > 8) {
                if (carsPage >= 5 && number - carsPage > 4) {
                    buttons_container.innerHTML += `<button ${1 === carsPage ? 'class="selected"' : ''} page="${1}"">${1}</button>`;
                    buttons_container.innerHTML += `<p class="pages_buttons-space">...</p>`;
                    for (let i = carsPage - 1; i <= (number > carsPage + 4 ? carsPage + 2 : number); i++)
                        buttons_container.innerHTML += `<button ${i === carsPage ? 'class="selected"' : ''} page="${i}"">${i}</button>`;
                    if (number > carsPage + 4) {
                        buttons_container.innerHTML += `<p class="pages_buttons-space">...</p>`;
                        buttons_container.innerHTML += `<button ${number === carsPage ? 'class="selected"' : ''} page="${number}"">${number}</button>`;
                    }
                }
                else if (carsPage <= 4) {
                    for (let i = 1; i <= 6; i++)
                        buttons_container.innerHTML += `<button ${i === carsPage ? 'class="selected"' : ''} page="${i}"">${i}</button>`;
                    buttons_container.innerHTML += `<p class="pages_buttons-space">...</p>`;
                    buttons_container.innerHTML += `<button ${number === carsPage ? 'class="selected"' : ''} page="${number}"">${number}</button>`;
                }
                else if (number - carsPage <= 4) {
                    buttons_container.innerHTML += `<button ${1 === carsPage ? 'class="selected"' : ''} page="${1}"">${1}</button>`;
                    buttons_container.innerHTML += `<p class="pages_buttons-space">...</p>`;
                    for (let i = number - 5; i <= number; i++)
                        buttons_container.innerHTML += `<button ${i === carsPage ? 'class="selected"' : ''} page="${i}"">${i}</button>`;
                }
            }
            else {
                for (let i = 1; i <= number; i++)
                    buttons_container.innerHTML += `<button ${i === carsPage ? 'class="selected"' : ''} page="${i}"">${i}</button>`;
            }
            Array.from(buttons_container.children).forEach(button => {
                if (button.tagName === 'BUTTON')
                    button.addEventListener('click', () => applyFilter(+button.getAttribute("page")));
            });
        }
    });
}
function isFilterClear() {
    let res = true;
    if (engineVolume_max_input.value
        || engineVolume_min_input.value > 0
        || price_max_input.value > 0
        || price_min_input.value > 0
        || race_max_input.value > 0
        || race_min_input > 0
        || minYearSlider.value != 2000
        || maxYearSlider.value != 2023
        || selectBrandsBtn.firstElementChild.innerText != "Усі"
        || selectBodiesBtn.firstElementChild.innerText != "Усі"
        || selectTransmissionsBtn.firstElementChild.innerText != "Усі"
        || selectFuelsBtn.firstElementChild.innerText != "Усі"
        || selectDrivelinesBtn.firstElementChild.innerText != "Усі"
        || selectModelsBtn.firstElementChild.innerText != "Усі"
    )
        res = false;
    if (!res) openFilter.setAttribute('filtered', '');
    else openFilter.removeAttribute('filtered');
}
function clearFilters() {
    engineVolume_max_input.value = "";
    engineVolume_min_input.value = "";
    price_max_input.value = "";
    price_min_input.value = "";
    race_max_input.value = "";
    race_min_input.value = "";
    selectBrandsBtn.firstElementChild.innerText = "Усі";
    addBrand();
    selectSortingBtn.firstElementChild.innerText = "Сортування";
    addSorting();
    selectBodiesBtn.firstElementChild.innerText = "Усі";
    addBody();
    bodiesOptions.scrollTop = 0;
    selectTransmissionsBtn.firstElementChild.innerText = "Усі";
    addTransmission();
    transmissionsOptions.scrollTop = 0;
    selectFuelsBtn.firstElementChild.innerText = "Усі";
    addFuel();
    fuelsOptions.scrollTop = 0;
    selectDrivelinesBtn.firstElementChild.innerText = "Усі";
    addDriveline();
    drivelinesOptions.scrollTop = 0;
    selectModelsBtn.firstElementChild.innerText = "Усі";
    models = ['Усі'];
    addModel();
    minYearSlider.value = 2000;
    minYearSlider.setAttribute('max', maxYearSlider.getAttribute('max'));
    minMaxYear.textContent = maxYearSlider.getAttribute('max');
    maxMinYear.textContent = minYearSlider.getAttribute('min');
    maxYearSlider.value = 2023;
    maxYearSlider.setAttribute('min', minYearSlider.getAttribute('min'));
    yearLabel.textContent = `Рік`;
    applyFilter();
    isFilterClear()
}
clear_filters.onclick = () => {
    window.scrollTo({ top: 0, behavior: 'smooth' });
    clearFilters();
}
//#endregion

//#region input fields settings
minYearSlider.oninput = (() => {
    var value = minYearSlider.value;
    minYearValue.textContent = value;
    var coef = 46.9 / (minYearSlider.getAttribute('max') - 2000);
    minYearValue.style.left = (value - 2000) * coef + 3 + '%';
    minYearValue.classList.add("show");
    maxYearSlider.setAttribute('min', value);
    maxMinYear.textContent = value;
    updateYearLabel();
});
maxYearSlider.onblur = (() => maxYearValue.classList.remove("show"));
maxYearSlider.oninput = (() => {
    var valueMax = maxYearSlider.value;
    maxYearValue.textContent = valueMax;
    var coefMax = 46.9 / (currentYear - maxYearSlider.getAttribute('min'));
    maxYearValue.style.left = (valueMax - maxYearSlider.getAttribute('min')) * coefMax + 3 + '%';
    maxYearValue.classList.add("show");
    minYearSlider.setAttribute('max', valueMax);
    minMaxYear.textContent = valueMax;
    updateYearLabel();
});
function updateYearLabel() {
    if (minYearSlider.value == maxYearSlider.value)
        yearLabel.textContent = `Рік (${minYearSlider.value})`;
    else if (minYearSlider.value == 2000 && maxYearSlider.value != maxYearSlider.getAttribute('max'))
        yearLabel.textContent = `Рік (≤ ${maxYearSlider.value})`;
    else if (minYearSlider.value != 2000 && maxYearSlider.value == maxYearSlider.getAttribute('max'))
        yearLabel.textContent = `Рік (≥ ${minYearSlider.value})`;
    else if (minYearSlider.value == 2000 && maxYearSlider.value == maxYearSlider.getAttribute('max'))
        yearLabel.textContent = `Рік`;
    else
        yearLabel.textContent = `Рік (${minYearSlider.value} — ${maxYearSlider.value})`;
}
minYearSlider.onblur = (() => minYearValue.classList.remove("show"));
const inputsWithComma = [engineVolume_min_input, engineVolume_max_input];
inputsWithComma.forEach((inp) => {
    inp.addEventListener('input', function (event) {
        let value = event.target.value.replace(/[^\d.,]/g, '');;
        value = value.replace(',', '.');
        let length = 2;
        if (isElectro) length++;
        if (value.includes('.')) length++;
        value = value.slice(0, length);
        event.target.value = value;
    });
    inp.addEventListener('keydown', (e) => {
        if ((e.target.value.includes('.') || e.target.value == '' || e.target.value.length >= (isElectro ? 3 : 2)) && (e.key == '.' || e.key == ','))
            e.preventDefault();
    });
});
const inputs = [price_max_input, price_min_input, race_max_input, race_min_input];
inputs.forEach((inp) => {
    inp.addEventListener('input', function (event) {
        const maxLength = +event.target.getAttribute('maxlength');
        let currentValue = event.target.value;
        currentValue = currentValue.replace(/[^\d]/g, '');
        if (currentValue.length > maxLength)
            currentValue = currentValue.slice(0, maxLength);
        event.target.value = currentValue;
    });
})
apply_button.onclick = () => {
    window.scrollTo({ top: 0, behavior: 'smooth' });
    applyFilter();
}
//#endregion

//#region Ajax requests
function getModelsOfMark(applyModel = "") {
    var brand = selectBrandsBtn.firstElementChild.innerText;
    if (modelsCache[brand] == null) {
        fetch(`/home/GetModels?brand=${brand}`)
            .then(response => response.json())
            .then(data => {
                models = ["Усі"];
                models = models.concat(data.models);
                models = models.filter((n) => { return n != 'Інше' });
                modelsCache[brand] = models;
                addModel();
                if (applyModel != "")
                    updateModel(createLi(applyModel));
            })
            .catch(error => console.error("An error occurred while retrieving data:", error));
    }
    else {
        models = modelsCache[brand];
        addModel();
    }
}
function getMarks() {
    fetch(`/home/GetBrands`)
        .then(response => response.json())
        .then(data => {
            brands = ["Усі"];
            brands = brands.concat(data.brands);
            brands = brands.filter((n) => { return n != 'Інше' });
            addBrand();
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
function applyFilter(page = 1) {
    isFilterClear();
    filter.classList.remove("open");
    const filters = {
        body: bodies.indexOf(selectBodiesBtn.firstElementChild.innerHTML),
        brand: selectBrandsBtn.firstElementChild.innerText,
        model: selectModelsBtn.firstElementChild.innerText,
        minYear: Number(minYearSlider.value),
        maxYear: Number(maxYearSlider.value),
        minPrice: Number(price_min_input.value),
        maxPrice: Number(price_max_input.value),
        carTransmission: transmissions.indexOf(selectTransmissionsBtn.firstElementChild.innerHTML),
        fuel: fuels[selectFuelsBtn.firstElementChild.innerHTML],
        driveLine: drivelines.indexOf(selectDrivelinesBtn.firstElementChild.innerHTML),
        minEngineCapacity: Number(engineVolume_min_input.value),
        maxEngineCapacity: Number(engineVolume_max_input.value),
        minMileage: Number(race_min_input.value),
        maxMileage: Number(race_max_input.value),
        page: page,
        sortingType: sortings.includes(selectSortingBtn.innerText) ? sortings.indexOf(selectSortingBtn.innerText) : 0,
    };
    const carList = document.getElementById("carList");
    carsPage = page;
    let filtersString = JSON.stringify(filters);
    updatePagesButtons(pages);
    carList.innerHTML = `<div class="cars-not-found" style="height: ${carList.clientHeight}px;"><div class="custom-loader"></div><h3 class="warning-text">Завантаження...</h3></div>`;
    if (!carsCache[filtersString] || carsCache[filtersString].status == false) {
        fetch(`/api/v1/cars/getFiltered`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: filtersString
        })
            .then(response => response.json())
            .then(data => {
                setCarsData(data, filters, filtersString);
            })
            .catch(error => console.error("An error occurred while retrieving data:", error));
    }
    else
        setCarsData(carsCache[filtersString],filters,filtersString);
}
function setCarsData(data, filters, filtersString) {
    sessionStorage.setItem("last", JSON.stringify({ filters, data }));
    if (data != null && data.status == true && data.cars.length > 0) {
        carsPage = data.page;
        updatePagesButtons(data.pages);
        pages = data.pages;
        carList.innerHTML = "";
        data.cars.forEach(car => {
            const block = formCar(car);
            carList.innerHTML += block;
        });
        carsCache[filtersString] = data;
        updateLikeButtons();
    }
    else if (data != null && data.status == true) {
        updatePagesButtons(0);
        carList.innerHTML = `<div class="cars-not-found"><h3 class="warning-text">Нічого не знайдено</h3></div>`;
    }
    else {
        updatePagesButtons(0);
        carList.innerHTML = `<div class="cars-not-found"><h3 class="warning-text">Щось пішло не так</h3></div>`;
    }
}
//#endregion
function formCar(car) {
    return `
                                  <a class="car mainPageCar" href="/Car/Detail/${car.id}">
                                  <p class="car_name">${car.brand} ${car.model} ${car.year}</p>
                                  <div class="car_container">
                                       <div class="car_container-img"> <div class="car_container-img-landscape"><img  alt="${car.brand} ${car.model} ${car.year}" src="${car.previewURL}" /></div></div>
                                    <div class="car_container-info">
                                    <p class="car_container-info-name">${car.brand} ${car.model} ${car.year}</p>
                                            <div class="car_container-info-parameters">
                                                <div class="car_container-info-parameters-column">
                                                    <p class="car_container-info-parameters-column-text"><span>${svgCodes.race}</span>${car.mileage} тис. км</p>
                                                    <p class="car_container-info-parameters-column-text"><span>${svgCodes.fuel}</span>${fuelName(car.fuel)}, ${car.engineCapacity} ${car.fuel == 6 ? "кВт·год." : "л."}</p>
                                                    ${car.vin == null ? `` : `<p class="car_container-info-parameters-column-text vin"><span>${svgCodes.car}</span>${car.vin}</p>`}
                                                    </div>
                                                <div class="car_container-info-parameters-column">
                                                    <p class="car_container-info-parameters-column-text"><span>${svgCodes.transmission}</span>${transmissionName(car.carTransmission)}</p>
                                                    <p class="car_container-info-parameters-column-text"><span>${svgCodes.driveline}</span>${drivelineName(car.driveline)}</p>
                                                    </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="car_container-right">
                                    <div class="car_container-right-price">
                                                <p class="car_container-right-price-USD">${formatNumberWithThousandsSeparator(car.price)} $</p>
                                                <p class="car_container-right-price-UAH">≈ ${formatNumberWithThousandsSeparator(car.priceUAH)} грн</p>
                                            </div>
                                            ${car.priority > 0 ? '<span class="car_container-right-top">Топ</span>' : ''}
                                  <div class="car_container-right-like">
                                  <input type="checkbox" class="car_container-right-like-cars" carId="${car.id}" ${car.liked ? "checked" : ""}/>
                                  <span class="car_container-right-heart">
                                    <svg width="40" height="40" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg"><path fill-rule="evenodd" clip-rule="evenodd" d="M8.272 4.95258C7.174 4.95258 6.077 5.34958 5.241 6.14458C4.441 6.90658 4 7.91758 4 8.99158C4 10.0646 4.441 11.0756 5.241 11.8376L12 18.2696L18.759 11.8376C19.559 11.0756 20 10.0646 20 8.99158C20 7.91858 19.559 6.90658 18.759 6.14458C17.088 4.55458 14.368 4.55458 12.697 6.14458L12 6.80858L11.303 6.14458C10.467 5.34958 9.37 4.95258 8.272 4.95258ZM12 20.9996L3.847 13.2406C2.656 12.1076 2 10.5986 2 8.99158C2 7.38458 2.656 5.87558 3.847 4.74158C6.067 2.62858 9.552 2.43858 12 4.16858C14.448 2.43858 17.933 2.62858 20.153 4.74158C21.344 5.87558 22 7.38458 22 8.99158C22 10.5986 21.344 12.1076 20.153 13.2406L12 20.9996Z" fill="currentColor"/></svg>
                                  </span>
                                  <span class="car_container-right-span"><svg width="40" height="40" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                                    <path fill-rule="evenodd" clip-rule="evenodd" d="M12 20.9996L3.847 13.2406C2.656 12.1076 2 10.5986 2 8.99158C2 7.38458 2.656 5.87558 3.847 4.74158C6.067 2.62858 9.552 2.43858 12 4.16858C14.448 2.43858 17.933 2.62858 20.153 4.74158C21.344 5.87558 22 7.38458 22 8.99158C22 10.5986 21.344 12.1076 20.153 13.2406L12 20.9996Z" fill="currentColor"/></svg>
                                  </span>
                                  </div>
                                  </div>
                                  </a>`;
}
//#region info displaying
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
//#endregion

//#region Custom selects
function refreshBrands() {
    if (selectBrandsBtn.firstElementChild.innerText === 'Усі')
        brandsOptions.innerHTML = `<li onclick="updateModel(this)" class="selected">Усі</li>`;
    else
        brandsOptions.innerHTML = '';
    brands.forEach(brand => {
        if (brand !== 'Усі' || selectBrandsBtn.firstElementChild.innerText !== 'Усі') {
            let isSelected = brand == selectBrandsBtn.firstElementChild.innerText ? "selected" : "";
            let li = `<li onclick="updateName(this)" class="${isSelected}">${brand}</li>`;
            brandsOptions.insertAdjacentHTML("beforeend", li);
        }
    });
}
function refreshModels() {
    if (selectModelsBtn.firstElementChild.innerText === 'Усі')
        modelsOptions.innerHTML = `<li onclick="updateModel(this)" class="selected">Усі</li>`;
    else
        modelsOptions.innerHTML = '';
    models.forEach(model => {
        if (model !== 'Усі' || selectModelsBtn.firstElementChild.innerText !== 'Усі') {
            let isSelected = model == selectModelsBtn.firstElementChild.innerText ? "selected" : "";
            let li = `<li onclick="updateModel(this)" class="${isSelected}">${model}</li>`;
            modelsOptions.insertAdjacentHTML("beforeend", li);
        }
    });
}
searchBrandInp.addEventListener("keyup", () => {
    let arr = [];
    let searchWord = searchBrandInp.value.toLowerCase();
    arr = brands.filter(data => {
        return data.toLowerCase().startsWith(searchWord);
    }).map(data => {
        let isSelected = data == selectBrandsBtn.firstElementChild.innerText ? "selected" : "";
        return `<li onclick="updateName(this)" class="${isSelected}">${data}</li>`;
    }).join("");
    brandsOptions.innerHTML = arr ? arr : `<p style="margin-top: 10px;">Марку не знайдено.</p>`;
});
searchModelInp.addEventListener("keyup", () => {
    let arr = [];
    let searchWord = searchModelInp.value.toLowerCase();
    arr = models.filter(data => {
        return data.toLowerCase().startsWith(searchWord);
    }).map(data => {
        let isSelected = data == selectModelsBtn.firstElementChild.innerText ? "selected" : "";
        return `<li onclick="updateModel(this)" class="${isSelected}">${data}</li>`;
    }).join("");
    modelsOptions.innerHTML = arr ? arr : `<p style="margin-top: 10px;">Модель не знайдено.</p>`;
});
function addBrand(selectedBrand) {
    if (!selectedBrand) {
        selectModelsBtn.firstElementChild.innerText = 'Усі';
        brandsOptions.innerHTML = `<li onclick="updateModel(this)" class="selected">Усі</li>`;
    }
    else
        brandsOptions.innerHTML = '';
    brands.forEach(brand => {
        if (brand != 'Усі' || selectedBrand) {
            let isSelected = brand == selectedBrand ? "selected" : "";
            let li = `<li onclick="updateName(this)" class="${isSelected}">${brand}</li>`;
            brandsOptions.insertAdjacentHTML("beforeend", li);
        }
    });
}
function addModel(selectedModel) {
    if (!selectedModel) {
        selectModelsBtn.firstElementChild.innerText = 'Усі';
        modelsOptions.innerHTML = `<li onclick="updateModel(this)" class="selected">Усі</li>`;
    }
    else
        modelsOptions.innerHTML = '';
    models.forEach(model => {
        if (model != 'Усі' || selectedModel) {
            let isSelected = model == selectedModel ? "selected" : "";
            let li = `<li onclick="updateModel(this)" class="${isSelected}">${model}</li>`;
            modelsOptions.insertAdjacentHTML("beforeend", li);
        }
    });
}
function addBody(selectedBody) {
    bodiesOptions.innerHTML = '';
    bodies.forEach(body => {
        let isSelected = body == selectedBody || !selectedBody && body == 'Усі' ? "selected" : "";
        let li = `<li onclick="updateBody(this)" class="${isSelected}">${body}</li>`;
        bodiesOptions.insertAdjacentHTML("beforeend", li);
    });
}
function addTransmission(selectedTransmission) {
    transmissionsOptions.innerHTML = '';
    transmissions.forEach(transmission => {
        let isSelected = transmission == selectedTransmission || !selectedTransmission && transmission == 'Усі' ? "selected" : "";
        let li = `<li onclick="updateTransmission(this)" class="${isSelected}">${transmission}</li>`;
        transmissionsOptions.insertAdjacentHTML("beforeend", li);
    });
}
function addFuel(selectedFuel) {
    fuelsOptions.innerHTML = '';
    for (const [key, value] of Object.entries(fuels)) {
        let isSelected = key == selectedFuel || !selectedFuel && key == 'Усі' ? "selected" : "";
        let li = `<li onclick="updateFuel(this)" class="${isSelected}">${key}</li>`;
        fuelsOptions.insertAdjacentHTML("beforeend", li);
    }
}
function addDriveline(selectedDriveline) {
    drivelinesOptions.innerHTML = '';
    drivelines.forEach(driveline => {
        let isSelected = driveline == selectedDriveline || !selectedDriveline && driveline == 'Усі' ? "selected" : "";
        let li = `<li onclick="updateDriveline(this)" class="${isSelected}">${driveline}</li>`;
        drivelinesOptions.insertAdjacentHTML("beforeend", li);
    });
}
function addSorting(selectedSorting) {
    sortingOptions.innerHTML = '';
    sortings.forEach(sorting => {
        let isSelected = sorting == selectedSorting || !selectedSorting && sorting == 'За датою додавання' ? "selected" : "";
        let li = `<li onclick="updateSorting(this)" class="${isSelected}">${sorting}</li>`;
        sortingOptions.insertAdjacentHTML("beforeend", li);
    });
}
function updateName(selectedLi) {
    searchBrandInp.value = "";
    addBrand(selectedLi.innerText);
    brandsOptions.parentElement.classList.remove("active");
    selectBrandsBtn.classList.remove("active");
    selectBrandsBtn.firstElementChild.innerText = selectedLi.innerText;
    getModelsOfMark();
}
function updateModel(selectedLi) {
    searchModelInp.value = "";
    addModel(selectedLi.innerText);
    modelsOptions.parentElement.classList.remove("active");
    selectModelsBtn.classList.remove("active");
    selectModelsBtn.firstElementChild.innerText = selectedLi.innerText;
}
function updateBody(selectedLi) {
    addBody(selectedLi.innerText);
    bodiesOptions.parentElement.classList.remove("active");
    selectBodiesBtn.classList.remove("active");
    selectBodiesBtn.firstElementChild.innerText = selectedLi.innerText;
}
function updateTransmission(selectedLi) {
    addTransmission(selectedLi.innerText);
    transmissionsOptions.parentElement.classList.remove("active");
    selectTransmissionsBtn.classList.remove("active");
    selectTransmissionsBtn.firstElementChild.innerText = selectedLi.innerText;
}
function updateFuel(selectedLi) {
    addFuel(selectedLi.innerText);
    if (selectedLi.innerText == "Електро") {
        engineVolumeLbl.innerHTML = 'Ємність батареї (кВт·год.)';
        isElectro = true;
    }
    else {
        engineVolumeLbl.innerHTML = "Об'єм двигуна (л.)";
        isElectro = false;
        if (engineVolume_max_input.value.includes('.'))
            engineVolume_max_input.value = engineVolume_max_input.value.slice(0, 3);
        else
            engineVolume_max_input.value = engineVolume_max_input.value.slice(0, 2);
        if (engineVolume_min_input.value.includes('.'))
            engineVolume_min_input.value = engineVolume_min_input.value.slice(0, 3);
        else
            engineVolume_min_input.value = engineVolume_min_input.value.slice(0, 2);
    }
    fuelsOptions.parentElement.classList.remove("active");
    selectFuelsBtn.classList.remove("active");
    selectFuelsBtn.firstElementChild.innerText = selectedLi.innerText;
}
function updateDriveline(selectedLi) {
    addDriveline(selectedLi.innerText);
    drivelinesOptions.parentElement.classList.remove("active");
    selectDrivelinesBtn.classList.remove("active");
    selectDrivelinesBtn.firstElementChild.innerText = selectedLi.innerText;
}
function updateSorting(selectedLi) {
    addSorting(selectedLi.innerText);
    sortingOptions.parentElement.classList.remove("active");
    selectSortingBtn.classList.remove("active");
    selectSortingBtn.firstElementChild.innerText = selectedLi.innerText;
    applyFilter();
}
function hideBrand() {
    searchBrandInp.value = "";
    brandsOptions.parentElement.classList.remove("active");
    selectBrandsBtn.classList.remove("active");
}
function hideModel() {
    searchModelInp.value = "";
    modelsOptions.parentElement.classList.remove("active");
    selectModelsBtn.classList.remove("active");
}
function hideBody() {
    bodiesOptions.parentElement.classList.remove("active");
    selectBodiesBtn.classList.remove("active");
    bodiesOptions.scrollTop = 0;
}
function hideTransmission() {
    transmissionsOptions.parentElement.classList.remove("active");
    selectTransmissionsBtn.classList.remove("active");
    transmissionsOptions.scrollTop = 0;
}
function hideDriveline() {
    drivelinesOptions.parentElement.classList.remove("active");
    selectDrivelinesBtn.classList.remove("active");
    drivelinesOptions.scrollTop = 0;
}
function hideSorting() {
    sortingOptions.parentElement.classList.remove("active");
    selectSortingBtn.classList.remove("active");
    sortingOptions.scrollTop = 0;
}
function hideFuel() {
    fuelsOptions.parentElement.classList.remove("active");
    selectFuelsBtn.classList.remove("active");
    fuelsOptions.scrollTop = 0;
}
//#endregion

//#region Click events
selectBrandsBtn.addEventListener("click", () => {
    searchBrandInp.value = "";
    brandsOptions.parentElement.classList.toggle("active");
    selectBrandsBtn.classList.toggle("active");
});
selectModelsBtn.addEventListener("click", () => {
    searchModelInp.value = "";
    modelsOptions.parentElement.classList.toggle("active");
    selectModelsBtn.classList.toggle("active");
});
selectBodiesBtn.addEventListener("click", () => {
    bodiesOptions.parentElement.classList.toggle("active");
    selectBodiesBtn.classList.toggle("active");
    bodiesOptions.scrollTop = 0;
});
selectTransmissionsBtn.addEventListener("click", () => {
    transmissionsOptions.parentElement.classList.toggle("active");
    selectTransmissionsBtn.classList.toggle("active");
    transmissionsOptions.scrollTop = 0;
});
selectFuelsBtn.addEventListener("click", () => {
    fuelsOptions.parentElement.classList.toggle("active");
    selectFuelsBtn.classList.toggle("active");
    fuelsOptions.scrollTop = 0;
});
selectDrivelinesBtn.addEventListener("click", () => {
    drivelinesOptions.parentElement.classList.toggle("active");
    selectDrivelinesBtn.classList.toggle("active");
    drivelinesOptions.scrollTop = 0;
});
selectSortingBtn.addEventListener("click", () => {
    sortingOptions.parentElement.classList.toggle("active");
    selectSortingBtn.classList.toggle("active");
    sortingOptions.scrollTop = 0;
});
document.addEventListener('click', function (event) {
    if (!selectBrandsBtn.parentElement.contains(event.target)) {
        hideBrand();
        refreshBrands();
    }
    if (!selectModelsBtn.parentElement.contains(event.target)) {
        hideModel();
        refreshModels();
    }
    if (!selectBodiesBtn.parentElement.contains(event.target))
        hideBody();
    if (!selectTransmissionsBtn.parentElement.contains(event.target))
        hideTransmission();
    if (!selectFuelsBtn.parentElement.contains(event.target))
        hideFuel();
    if (!selectDrivelinesBtn.parentElement.contains(event.target))
        hideDriveline();
    if (!selectSortingBtn.parentElement.contains(event.target))
        hideSorting();
});
//#endregion