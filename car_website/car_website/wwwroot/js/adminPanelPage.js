const radioButtons = document.querySelectorAll('input[name="action"]');
const container = document.getElementById('container');
let buyRequestsPage = 1;
let buyRequestsCache;
let waitingCarsPage = 1;
let waitingCarsCache;
let usersPage = 1;
let usersCache;
let brandsPage = 1;
let brandsCache;
let modelsCache = {};
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
            getBuyRequests();
        else {
            showData(buyRequestsCache);
        }
    }
    else if (target.id == "waitingCars") {
        if (waitingCarsCache == null)
            getWaitingCars();
        else {
            showData(waitingCarsCache);
        }
    }
    else if (target.id == "brands") {
        if (brandsCache == null)
            getBrands();
        else {
            showData(brandsCache);
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
                                                <div class="car_container-info-parameters-column">
                                                    <p class="car_container-info-parameters-column-race">${car.car.mileage} тис. км</p>
                                                    <p class="car_container-info-parameters-column-transmission">${transmissionName(car.car.carTransmission)}</p>
                                                    <p class="car_container-info-parameters-column-vin">${car.car.vin}</p>
                                                
                                                    </div>
                                                <div class="car_container-info-parameters-column">
                                                    <p class="car_container-info-parameters-column-fuel">${fuelName(car.car.fuel)}, ${car.car.engineCapacity} л.</p>
                                                    <p class="car_container-info-parameters-column-driveline">${drivelineName(car.car.driveline)}</p>
                                                    
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
        else if (data.type == "Brands") {
            container.innerHTML = "";
            if (data.brands.length == 0) {
                container.innerHTML = `<h3 class="warning-text">Тут ще нічого немає</h3>`;
            }
            else {
                container.innerHTML = '<div class="brands-and-models"></div>';
                container.firstElementChild.innerHTML += '<div id="modelsCont"></div><div id="brandsCont"></div>';
                let brandsContainer = document.getElementById('brandsCont');
                let modelsContainer = document.getElementById('modelsCont');
                data.brands.forEach(brand => {
                    if (brand != "Інше") {
                        let currBrand = document.createElement('div');
                        currBrand.classList.add('brand');
                        currBrand.innerHTML = `<input type="radio" id="${brand.replace(' ', '_')}"
                        name="brands" value="${brand.replace(' ', '_')}">
                        <label for="${brand.replace(' ', '_')}">${brand}</label>`;
                        brandsContainer.appendChild(currBrand);
                        currBrand.addEventListener('change', (event) => {
                            getModelsOfMark(event.target.getAttribute('value'), modelsContainer);
                        });
                    }
                });
            }
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
function getBrands() {
    fetch(`/Admin/GetBrands?page=${waitingCarsPage}`)
        .then(response => response.json())
        .then(data => {
            brandsCache = data;
            showData(brandsCache);
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
async function getModelsOfMark(brand, modelsContainer) {
    brand = brand.replace('_', ' ');
    modelsContainer.innerHTML = '';
    if (modelsCache[brand] == null) {
        fetch(`/home/GetModels?brand=${brand}`)
            .then(response => response.json())
            .then(data => {
                data.models = data.models.filter((n) => { return n != 'Інше' });
                modelsCache[brand] = data.models;
                modelsCache[brand].forEach(model => {
                    modelsContainer.innerHTML += `<div class="model"><p>${model}</p><button brand="${brand.replace(' ', '_')}" model="${model.replace(' ', '_')}">X</button></div>`;
                });
            })
            .catch(error => console.error("An error occurred while retrieving data:", error));
    }
    else {
        modelsCache[brand].forEach(model => {
            modelsContainer.innerHTML += `<div class="model"><p>${model}</p><button brand="${brand.replace(' ', '_')}" model="${model.replace(' ', '_')}">X</button></div>`;
        });
    }
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