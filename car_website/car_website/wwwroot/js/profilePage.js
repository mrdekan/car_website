const radioButtons = document.querySelectorAll('input[name="page"]');
const pageUnderline = document.getElementById('line');
const pages_buttons_containers = document.getElementsByClassName("pages_buttons");

let cars;
let carsPage = 1;
let waitingCars;
let waitingCarsPage = 1;
let favCars;
let favCarsPage = 1;
let offset;
var selectedRadioButton;

function updatePagesButtons(number) {
    Array.from(pages_buttons_containers).forEach(buttons_container => {
        buttons_container.innerHTML = "";
        if (number > 1) {
            let currentPage = 1;
            if (selectedRadioButton.id == "waiting")
                currentPage = waitingCarsPage;
            else if (selectedRadioButton.id == "favorite")
                currentPage = favCarsPage;
            else if (selectedRadioButton.id == "for-sell")
                currentPage = carsPage;
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
window.addEventListener('load', function () {
    let i = 0;
    let selectedIndex;
    radioButtons.forEach(function (radio) {
        if (radio.checked) {
            selectedRadioButton = radio;
            updateCarsList(selectedRadioButton);
            selectedIndex = i;
        }
        i++;
    });
    pageUnderline.style.width = `${90 / radioButtons.length}%`;
    offset = radioButtons.length == 3 ? 200 : 100;
    //pageUnderline.style.transition = "0s";
    let percent = selectedIndex * (offset / (radioButtons.length - 1));
    //console.log(selectedIndex);
    pageUnderline.style.transform = `translateX(${percent}%)`;
    //pageUnderline.style.transition = "transform 0.22s ease";
});
radioButtons.forEach((radio, index) => {
    radio.addEventListener('change', () => {
        const percent = index * (offset / (radioButtons.length - 1));
        selectedRadioButton = radio;
        pageUnderline.style.transform = `translateX(${percent}%)`;
    });
});
function handleRadioChange(event) {
    updateCarsList(event.target);
}
function updateCarsList(target, page = 1) {
    if (target.id == "waiting") {
        waitingCarsPage = page;
        if (waitingCars == null || waitingCars.page != waitingCarsPage)
            getWaiting();
        else {
            SetCarsFromData(waitingCars, true);
            updateLikeButtons();
            updatePagesButtons(waitingCars.pages);
        }
    }
    else if (target.id == "favorite") {
        favCarsPage = page;
        if (favCars == null || favCars.page != favCarsPage)
            getFavorites();
        else {
            SetCarsFromData(favCars);
            updateLikeButtons();
            updatePagesButtons(favCars.pages);
        }
    }
    else if (target.id == "for-sell") {
        carsPage = page;
        if (cars == null || cars.page != carsPage)
            getCars();
        else {
            SetCarsFromData(cars);
            updateLikeButtons();
            updatePagesButtons(cars.pages);
        }
    }
}
radioButtons.forEach(function (radio) {
    radio.addEventListener('change', handleRadioChange);
});
function updateLikeButtons() {
    likeButtons = document.getElementsByClassName("car_container-right-like-cars");
    Array.from(likeButtons).forEach(like => {
        like.addEventListener('change', function (event) {
            fetch(`/car/like?carId=${event.target.getAttribute('carId')}&isLiked=${like.checked}`)
                .then(response => response.json())
                .then(data => {
                    if (data.success == false) like.checked = !like.checked;
                })
                .catch(error => console.error("An error occurred while retrieving data:", error));
        });
    });
}
//#region Ajax requests
function getFavorites() {
    //console.log(page);
    fetch(`/User/GetFavoriteCars?page=${favCarsPage}`)
        .then(response => response.json())
        .then(data => {
            if (data != null && data.success == true) {
                favCars = data;
                //favCarsPage = data.page;
                SetCarsFromData(favCars);
                updateLikeButtons();
                updatePagesButtons(data.pages);
            }
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
function getWaiting() {
    var url = new URL(window.location.href);
    var pathSegments = url.pathname.split('/');
    var userId = pathSegments[pathSegments.length - 1];
    fetch(`/User/GetWaiting?id=${userId}&page=${waitingCarsPage}`)
        .then(response => response.json())
        .then(data => {
            if (data != null && data.success == true) {
                waitingCars = data;
                //waitingCarsPage = data.page;
                SetCarsFromData(waitingCars, true);
                updateLikeButtons();
                updatePagesButtons(data.pages);
            }
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
function getCars() {
    var url = new URL(window.location.href);
    var pathSegments = url.pathname.split('/');
    var userId = pathSegments[pathSegments.length - 1];
    fetch(`/User/GetCars?id=${userId}&page=${carsPage}`)
        .then(response => response.json())
        .then(data => {
            if (data != null && data.success == true) {
                cars = data;
                console.log(data);
                //carsPage = data.page;
                SetCarsFromData(cars);
                updateLikeButtons();
                updatePagesButtons(data.pages);
            }
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
//#endregion


//#region info displaying
function SetCarsFromData(data, waitingCarsList = false) {
    const favList = document.getElementById("cars-list");
    if (data != null && data.cars.length != 0 && data.success == true) {
        favList.innerHTML = "";
        data.cars.forEach(car => {
            if (!waitingCarsList) {
                const block = `<div class="car">
                                  <div class="car_container">
                                        <img  alt="photo" src="${car.photosURL[0]}" />
                                    <div class="car_container-info">
                                        <a href="/Car/Detail/${car.id}">${car.brand} ${car.model} ${car.year}</a>
                                            <div class="car_container-info-parameters">
                                                <div class="car_container-info-parameters-row">
                                                    <p class="car_container-info-parameters-row-race">${car.mileage} тис. км</p>
                                                    <p class="car_container-info-parameters-row-fuel">${fuelName(car.fuel)}, ${car.engineCapacity} л.</p>
                                                </div>
                                                <div class="car_container-info-parameters-row">
                                                    <p class="car_container-info-parameters-row-transmission">${transmissionName(car.carTransmission)}</p>
                                                    <p class="car_container-info-parameters-row-driveline">${drivelineName(car.driveline)}</p>
                                                </div>
                                                <div class="car_container-info-parameters-row">
                                                    <p class="car_container-info-parameters-row-vin">${car.vin}</p>
                                                </div>
                                            </div>
                                            
                                        </div>
                                    </div>
                                    <div class="car_container-right">
                                    <div class="car_container-right-price">
                                                <p class="car_container-right-price-USD">${formatNumberWithThousandsSeparator(car.price)} $</p>
                                                <p class="car_container-right-price-UAH">≈ ${formatNumberWithThousandsSeparator(car.priceUAH)} грн</p>
                                            </div>
                                  <div class="car_container-right-like">
                                  <input type="checkbox" class="car_container-right-like-cars" carId="${car.id}" carId="${car.id}" ${car.liked ? "checked" : ""}/>
                                  <span class="car_container-right-span"></span>
                                  </div>
                                  </div>
                                </div>`;
                favList.innerHTML += block;
            }
            else {
                const block = `<div class="car">
                                  <div class="car_container">
                                        <img  alt="photo" src="${car.car.photosURL[0]}" />
                                    <div class="car_container-info">
                                        <a href="/Car/WaitingCarDetail/${car.id}">${car.car.brand} ${car.car.model} ${car.car.year}</a>
                                            <div class="car_container-info-parameters">
                                                <div class="car_container-info-parameters-row">
                                                    <p class="car_container-info-parameters-row-race">${car.car.mileage} тис. км</p>
                                                    <p class="car_container-info-parameters-row-fuel">${fuelName(car.car.fuel)}, ${car.car.engineCapacity} л.</p>
                                                </div>
                                                <div class="car_container-info-parameters-row">
                                                    <p class="car_container-info-parameters-row-transmission">${transmissionName(car.car.carTransmission)}</p>
                                                    <p class="car_container-info-parameters-row-driveline">${drivelineName(car.car.driveline)}</p>
                                                </div>
                                                <div class="car_container-info-parameters-row">
                                                    <p class="car_container-info-parameters-row-vin">${car.car.vin}</p>
                                                </div>
                                            </div>
                                            
                                        </div>
                                    </div>
                                    <div class="car_container-right">
                                    <div class="car_container-right-price">
                                                <p class="car_container-right-price-USD">${formatNumberWithThousandsSeparator(car.car.price)} $</p>
                                                <p class="car_container-right-price-UAH">≈ ${formatNumberWithThousandsSeparator(car.car.priceUAH)} грн</p>
                                            </div>
                                  </div>
                                  </div>
                                </div>`;
                favList.innerHTML += block;
            }
        });
    }
    else if (data.success == false) {
        favList.innerHTML = `<h3 class="warning-text">Щось пішло не так</h3>`;
    }
    else {
        favList.innerHTML = `<h3 class="warning-text">Тут ще нічого немає</h3>`;
    }
}
function fuelName(id) {
    switch (id) {
        case 1: {
            return "Газ";
        }
        case 2: {
            return "Газ/Бензин";
        }
        case 3: {
            return "Бензин";
        }
        case 4: {
            return "Дизель";
        }
        case 5: {
            return "Гібрид";
        }
        case 16: {
            return "Електро";
        }
    }
}
function transmissionName(id) {
    switch (id) {
        case 1: {
            return "Механічна";
        }
        case 2: {
            return "Автомат";
        }
    }
}
function drivelineName(id) {
    switch (id) {
        case 1: {
            return "Передній";
        }
        case 2: {
            return "Задній";
        }
        case 3: {
            return "Повний";
        }
    }
}
function formatNumberWithThousandsSeparator(number) {
    return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ' ');
}
//#endregion