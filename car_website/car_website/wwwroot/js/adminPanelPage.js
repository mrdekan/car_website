const radioButtons = document.querySelectorAll('input[name="action"]');
const container = document.getElementById('container');
let buyRequestsPage = 1;
let buyRequestsCache;
let waitingCarsPage = 1;
let waitingCarsCache;
let usersPage = 1;
let usersCache;
window.addEventListener('load', function () {
    radioButtons.forEach(function (radio) {
        if (radio.checked) {
            selectedRadioButton = radio;
            updateInfo(selectedRadioButton);
        }
    });
});

radioButtons.forEach(function (radio) {
    radio.addEventListener('change', handleRadioChange);
});
function handleRadioChange(event) {
    updateInfo(event.target);
}
function updateInfo(target) {
    container.innerHTML = "";
    if (target.id == "users") {
        if (usersCache == null)
            getUsers();
        else {
            showData(usersCache);
        }
    }
    else if (target.id == "buyRequests") {
        if (buyRequestsCache == null)
            getBuyRequests()
        else {
            showData(buyRequestsCache);
        }
    }
    else if (target.id == "waitingCars") {
        if (waitingCarsCache == null)
            getWaitingCars()
        else {
            showData(waitingCarsCache);
        }
    }
}
function showData(data) {
    if (data == null || data.success == false) {
        container.innerHTML = `<h3 class="warning-text">Помилка при отриманні даних</h3>`;
    }
    else {
        if (data.type == "Users") {
            container.innerHTML = "";
            if (data.users.length == 0) {
                container.innerHTML = `<h3 class="warning-text">Пусто</h3>`;
            }
            data.users.forEach(user => {
                container.innerHTML += `<div class="user_container-element">
                <a href="/User/Detail/${user.id}">${user.name} ${user.surname}</a>
                <p>${user.phoneNumber}</p>
            </div>`;
            });
        }
        else if (data.type == "WaitingCars") {
            container.innerHTML = "";
            if (data.cars.length == 0) {
                container.innerHTML = `<h3 class="warning-text">Тут ще нічого немає</h3>`;
            }
            data.cars.forEach(car => {
                container.innerHTML += `<div class="car">
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
            });
        }
        else if (data.type == "BuyRequests") {
            container.innerHTML = "";
            if (data.requests.length == 0) {
                container.innerHTML = `<h3 class="warning-text">Тут ще нічого немає</h3>`;
            }
            data.requests.forEach(request => {
                container.innerHTML += `<div class="buy-request">
            <div class="buy-request_buyer">
                <h3>Клієнт:</h3>
                <a href="/User/Detail/${request.buyerId}">${request.buyerName}</a>
                <p>${request.buyerPhone}</p>
            </div>
            <div class="buy-request_car">
                <a href="/Car/Detail/${request.carId}">${request.carInfo}</a>
                <img alt="photo" src=${request.carPhotoURL}>
            </div>
            <div class="buy-request_seller">
                <h3>Власник:</h3>
                <a href="/User/Detail/${request.sellerId}">${request.sellerName}</a>
                <p>${request.sellerPhone}</p>
            </div>
        </div>`;
            });
        }
    }
}

//#region Ajax requests
function getUsers() {
    fetch(`/Admin/GetUsers?page=${usersPage}`)
        .then(response => response.json())
        .then(data => {
            usersCache = data;
            showData(usersCache);
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
function getWaitingCars() {
    fetch(`/Admin/GetWaitingCars?page=${waitingCarsPage}`)
        .then(response => response.json())
        .then(data => {
            waitingCarsCache = data;
            showData(waitingCarsCache);
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
function getBuyRequests() {
    fetch(`/Admin/GetBuyRequests?page=${buyRequestsPage}`)
        .then(response => response.json())
        .then(data => {
            buyRequestsCache = data;
            showData(buyRequestsCache);
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
//#endregion
//#region info displaying
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