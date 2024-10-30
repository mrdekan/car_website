import { getIncomingCarsCount } from './modules/getIncomingCarsCount.js';
import { fuelName, transmissionName, drivelineName, formatNumberWithThousandsSeparator } from './modules/formCar.js';
import svgCodes from './modules/svgCodesConst.js';
const pages_buttons_containers = document.getElementsByClassName("pages_buttons");
let carsPage = 1, cars, selectedRadioButton;
getCars();
getIncomingCarsCount();
function updatePagesButtons(number) {
    pages = number;
    Array.from(pages_buttons_containers).forEach(buttons_container => {
        buttons_container.innerHTML = "";
        if (number > 1) {
            let currentPage = carsPage;
            if (number > 7) {
                if (currentPage >= 5 && number - currentPage > 4) {
                    buttons_container.innerHTML += `<button ${1 === currentPage ? 'class="selected"' : ''} page="${1}"">${1}</button>`;
                    buttons_container.innerHTML += `<p class="pages_buttons-space">...</p>`;
                    for (let i = currentPage - 1; i <= (number > currentPage + 4 ? currentPage + 2 : number); i++) {
                        buttons_container.innerHTML += `<button ${i === currentPage ? 'class="selected"' : ''} page="${i}"">${i}</button>`;
                    }
                    if (number > currentPage + 4) {
                        buttons_container.innerHTML += `<p class="pages_buttons-space">...</p>`;
                        buttons_container.innerHTML += `<button ${number === currentPage ? 'class="selected"' : ''} page="${number}"">${number}</button>`;
                    }
                }
                else if (currentPage <= 4) {
                    for (let i = 1; i <= 6; i++) {
                        buttons_container.innerHTML += `<button ${i === currentPage ? 'class="selected"' : ''} page="${i}"">${i}</button>`;
                    }
                    buttons_container.innerHTML += `<p class="pages_buttons-space">...</p>`;
                    buttons_container.innerHTML += `<button ${number === currentPage ? 'class="selected"' : ''} page="${number}"">${number}</button>`;
                }
                else if (number - currentPage <= 4) {
                    buttons_container.innerHTML += `<button ${1 === currentPage ? 'class="selected"' : ''} page="${1}"">${1}</button>`;
                    buttons_container.innerHTML += `<p class="pages_buttons-space">...</p>`;
                    for (let i = number - 5; i <= number; i++) {
                        buttons_container.innerHTML += `<button ${i === currentPage ? 'class="selected"' : ''} page="${i}"">${i}</button>`;
                    }
                }
            }
            else {
                for (let i = 1; i <= number; i++) {
                    buttons_container.innerHTML += `<button ${i === currentPage ? 'class="selected"' : ''} page="${i}"">${i}</button>`;
                }
            }
            Array.from(buttons_container.children).forEach(button => {
                if (button.tagName === 'BUTTON') {
                    button.addEventListener('click', () => {
                        updateCarsList(selectedRadioButton, + button.getAttribute("page"));
                    });
                }
            });
        }
    });
}
function updateCarsList(target, page = 1) {
    const carsList = document.getElementById("cars-list");
    carsList.innerHTML = `<div class="cars-not-found" style="height: ${carsList.clientHeight}px;"><div class="custom-loader"></div><h3 class="warning-text">Завантаження...</h3></div>`;
    updatePagesButtons(pages);
    carsPage = page;
    if (cars == null || cars.page != carsPage)
        getCars();
    else {
        setCarsFromData();
        updatePagesButtons(cars.pages);
    }
}
function getCars() {
    fetch(`/api/v1/cars/getIncomingCars?page=${carsPage}`)
        .then(response => response.json())
        .then(data => {
            if (data != null && data.status == true) {
                cars = data;
                setCarsFromData();
                updatePagesButtons(data.pages);
            }
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
function setCarsFromData() {
    const favList = document.getElementById("cars-list");
    if (cars != null && cars.cars.length != 0 && cars.status == true) {
        favList.innerHTML = "";
        cars.cars.forEach(car => favList.innerHTML += formCar(car));
    }
    else
        favList.innerHTML = (cars == null || cars.status == false) ? `<h3 class="warning-text">Щось пішло не так</h3>` : `<h3 class="warning-text">Тут ще нічого немає</h3>`;
}
function formCar(car) {
    return `<a class="car mainPageCar" href="/Car/IncomingCarDetail/${car.id}">
                                  <p class="car_name">${car.brand} ${car.model} ${car.year}</p>
                                  <div class="car_container">
                                       <div class="car_container-img"> <div class="car_container-img-landscape"><img  alt="photo" src="${car.previewURL}" /></div></div>
                                    <div class="car_container-info">
                                    <p class="car_container-info-name">${car.brand} ${car.model} ${car.year}</p>
                                            <div class="car_container-info-parameters">
                                                <div class="car_container-info-parameters-column">
                                                    <p class="car_container-info-parameters-column-text"><span>${svgCodes.race}</span>${car.mileage} тис. км</p>
                                                    <p class="car_container-info-parameters-column-text"><span>${svgCodes.fuel}</span>${fuelName(car.fuel)}, ${car.engineCapacity} ${car.fuel == 6 ? "кВт·год." : "л."}</p>
                                                    </div>
                                                <div class="car_container-info-parameters-column">
                                                    <p class="car_container-info-parameters-column-text"><span>${svgCodes.transmission}</span>${transmissionName(car.carTransmission)}</p>
                                                    <p class="car_container-info-parameters-column-text"><span>${svgCodes.driveline}</span>${drivelineName(car.driveline)}</p>
                                                    </div>
                                            </div>
                                            <p class="car_container-info-arrive"><span>${svgCodes.time}</span>${car.arriveMessage}</p>
                                        </div>
                                    </div>
                                    <div class="car_container-right">
                                    <div class="car_container-right-price">
                                                <p class="car_container-right-price-USD">${formatNumberWithThousandsSeparator(car.price)} $</p>
                                                <p class="car_container-right-price-UAH">≈ ${formatNumberWithThousandsSeparator(car.priceUAH)} грн</p>
                                            </div>
                                            ${car.arriveMessage ? `<p class="car_container-info-arrive-mobile"><span>${svgCodes.time}</span>${car.arriveMessage}</p>`:''}
                                  </div>
                                  </a>`;
}
