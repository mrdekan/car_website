const radioButtons = document.querySelectorAll('input[name="page"]');
const pageUnderline = document.getElementById('line');
let cars;
let waitingCars;
let favCars;
let offset;
var selectedRadioButton;
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
    console.log(selectedIndex);
    pageUnderline.style.transform = `translateX(${percent}%)`;
    //pageUnderline.style.transition = "transform 0.22s ease";
});
radioButtons.forEach((radio, index) => {
    radio.addEventListener('change', () => {
        const percent = index * (offset / (radioButtons.length - 1));
        pageUnderline.style.transform = `translateX(${percent}%)`;
    });
});
function handleRadioChange(event) {
    updateCarsList(event.target);
}
function updateCarsList(target) {
    if (target.id == "waiting") {
        if (waitingCars == null)
            getWaiting();
        else {
            SetCarsFromData(waitingCars, true);
            updateLikeButtons();
        }
    }
    else if (target.id == "favorite") {
        if (favCars == null)
            getFavorites();
        else {
            SetCarsFromData(favCars);
            updateLikeButtons();
        }
    }
    else if (target.id == "for-sell") {
        if (cars == null)
            getCars();
        else {
            SetCarsFromData(cars);
            updateLikeButtons();
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
    fetch(`/User/GetFavoriteCars`)
        .then(response => response.json())
        .then(data => {
            favCars = data;
            SetCarsFromData(favCars);
            updateLikeButtons();
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
function getWaiting() {
    var url = new URL(window.location.href);
    var pathSegments = url.pathname.split('/');
    var userId = pathSegments[pathSegments.length - 1];
    fetch(`/User/GetWaiting?id=${userId}`)
        .then(response => response.json())
        .then(data => {
            waitingCars = data;
            SetCarsFromData(waitingCars, true);
            updateLikeButtons();
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
function getCars() {
    var url = new URL(window.location.href);
    var pathSegments = url.pathname.split('/');
    var userId = pathSegments[pathSegments.length - 1];
    fetch(`/User/GetCars?id=${userId}`)
        .then(response => response.json())
        .then(data => {
            cars = data;
            SetCarsFromData(cars);
            updateLikeButtons();
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