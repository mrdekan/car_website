const svgCodes = {
    edit: `<svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg"><g clip-path="url(#clip0_217_311)"><path d="M24 0H0V24H24V0Z" fill="white" fill-opacity="0.01"/><path d="M21 13V20C21 20.5523 20.5523 21 20 21H4C3.44771 21 3 20.5523 3 20V4C3 3.44771 3.44771 3 4 3H11" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/><path d="M7 13.36V17H10.6586L21 6.65405L17.3475 3L7 13.36Z" stroke="currentColor" stroke-width="2" stroke-linejoin="round"/></g><defs><clipPath id="clip0_217_311"><rect width="24" height="24" fill="white"/></clipPath></defs></svg>`,
    delete: `<svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg"><g clip-path="url(#clip0_217_304)"><path d="M4.5 5V22H19.5V5H4.5Z" stroke="currentColor" stroke-width="2" stroke-linejoin="round"/><path d="M10 10V16.5" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/><path d="M14 10V16.5" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/><path d="M2 5H22" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/><path d="M8 5L9.6445 2H14.3885L16 5H8Z" stroke="currentColor" stroke-width="2" stroke-linejoin="round"/></g><defs><clipPath id="clip0_217_304"><rect width="24" height="24" fill="white"/></clipPath></defs></svg>`,
    submit: `<svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg"><g clip-path="url(#clip0_235_308)"><path d="M5 12L10 17L20 7" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/></g><defs><clipPath id="clip0_235_308"><rect width="24" height="24" fill="white"/></clipPath></defs></svg>`,
    cancel: `<svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg"><path fill-rule="evenodd" clip-rule="evenodd" d="M13.4143 12.0002L18.7072 6.70725C19.0982 6.31625 19.0982 5.68425 18.7072 5.29325C18.3162 4.90225 17.6843 4.90225 17.2933 5.29325L12.0002 10.5862L6.70725 5.29325C6.31625 4.90225 5.68425 4.90225 5.29325 5.29325C4.90225 5.68425 4.90225 6.31625 5.29325 6.70725L10.5862 12.0002L5.29325 17.2933C4.90225 17.6843 4.90225 18.3162 5.29325 18.7072C5.48825 18.9022 5.74425 19.0002 6.00025 19.0002C6.25625 19.0002 6.51225 18.9022 6.70725 18.7072L12.0002 13.4143L17.2933 18.7072C17.4883 18.9022 17.7442 19.0002 18.0002 19.0002C18.2562 19.0002 18.5122 18.9022 18.7072 18.7072C19.0982 18.3162 19.0982 17.6843 18.7072 17.2933L13.4143 12.0002Z" fill="currentColor"/></svg>`,
    add: `<svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg"><g clip-path="url(#clip0_223_315)"><path d="M19.5 3H4.5C3.67157 3 3 3.67157 3 4.5V19.5C3 20.3284 3.67157 21 4.5 21H19.5C20.3284 21 21 20.3284 21 19.5V4.5C21 3.67157 20.3284 3 19.5 3Z" stroke="currentColor" stroke-width="2" stroke-linejoin="round"/><path d="M12 8V16" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/><path d="M8 12H16" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/></g><defs><clipPath id="clip0_223_315"><rect width="24" height="24" fill="white"/></clipPath></defs></svg>`,
};
const radioButtons = document.querySelectorAll('input[name="action"]');
const container = document.getElementById('container');
let buyRequestsPage = 1, buyRequestsCache;
let waitingCarsPage = 1, waitingCarsCache;
let usersPage = 1, usersCache;
let brandsPage = 1, brandsCache;
let modelsCache = {};
let selectedBrand;
window.addEventListener('load', function () {
    radioButtons.forEach(function (radio) {
        if (radio.checked) {
            updateInfo(radio);
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
        else
            showData(usersCache);
    }
    else if (target.id == "buyRequests") {
        if (buyRequestsCache == null)
            getBuyRequests();
        else
            showData(buyRequestsCache);
    }
    else if (target.id == "waitingCars") {
        if (waitingCarsCache == null)
            getWaitingCars();
        else
            showData(waitingCarsCache);
    }
    else if (target.id == "brands") {
        if (brandsCache == null)
            getBrands();
        else
            showData(brandsCache);
    }
}
function showData(data) {
    if (data == null || data.status == false || data.success == false)
        container.innerHTML = `<h3 class="warning-text">Помилка при отриманні даних</h3>`;
    else {
        if (data.type == "Users") {
            container.innerHTML = "";
            if (data.users.length == 0)
                container.innerHTML = `<h3 class="warning-text">Тут ще нікого немає</h3>`;
            data.users.forEach(user => {
                container.innerHTML += `<div class="user_container-element">
                <a href="/User/Detail/${user.id}">${user.name} ${user.surname}</a>
                <p>${user.phoneNumber}</p>
            </div>`;
            });
        }
        else if (data.type == "WaitingCars") {
            container.innerHTML = "";
            if (data.cars.length == 0)
                container.innerHTML = `<h3 class="warning-text">Тут ще нічого немає</h3>`;
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
            if (data.requests.length == 0)
                container.innerHTML = `<h3 class="warning-text">Тут ще нічого немає</h3>`;
            data.requests.forEach(request => {
                container.innerHTML += `<div class="buy-request">
            <div class="buy-request_buyer">
                <h3>Клієнт:</h3>
                ${request.buyerId?`<a href="/User/Detail/${request.buyerId}">${request.buyerName}</a>`: `<p class="buy-request-name">${request.buyerName}</p>`}
                <p>${request.buyerPhone}</p>
                ${request.buyerId ? '' : '<p>Не зареєстрований</p>'}
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
            if (data.brands.length == 0)
                container.innerHTML = `<h3 class="warning-text">Тут ще нічого немає</h3>`;
            else {
                container.innerHTML = '<div class="brands-and-models"></div>';
                container.firstElementChild.innerHTML += '<div id="modelsCont"></div><div id="brandsCont"></div>';
                let brandsContainer = document.getElementById('brandsCont');
                brandsContainer.innerHTML = `<div class="new-model"><input type="text" placeholder="Назва марки" id="new-brand-name"/>
                <button onclick="addBrand(this)"><span>${svgCodes.add}
                </span></button></div>`;
                if (selectedBrand == null || !data.brands.includes(selectedBrand)) selectedBrand = data.brands[0];
                data.brands.forEach(brand => {
                    if (brand != "Інше") {
                        let currBrand = document.createElement('div');
                        currBrand.classList.add('brand');
                        currBrand.innerHTML = `<input type="radio" id="${brand.replace(' ', '_')}"
                        name="brands" value="${brand.replace(' ', '_')}" ${brand == selectedBrand ? 'checked' : ''}>
                        <label for="${brand.replace(' ', '_')}">${brand}</label><div model_buttons>
                        <button brand="${brand.replace(' ', '_')}" class="model_buttons-edit"><span>
                        ${svgCodes.edit}
                        </span></button><button onclick="deleteBrand(this)" brand="${brand.replace(' ', '_')}" class="model_buttons-delete"><span>${svgCodes.delete}
                        </span></button></div>`;
                        if (brand == selectedBrand)
                            getModelsOfMark(brand);
                        brandsContainer.appendChild(currBrand);
                        currBrand.addEventListener('change', (event) => {
                            selectedBrand = event.target.getAttribute('value').replace('_', ' ');
                            getModelsOfMark(event.target.getAttribute('value'));
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
    fetch(`/api/v1/brands/getAll`)
        .then(response => response.json())
        .then(data => {
            data.type = "Brands";
            brandsCache = data;
            showData(brandsCache, "Brands");
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
function addModel(button) {
    const input = document.getElementById('new-model-name');
    if (input != null && input.value !== '') {
        fetch(`/api/v1/brands/addModel?brand=${button.getAttribute('brand')}&model=${input.value}`, {
            method: 'POST',
        })
            .then(response => response.json())
            .then(data => {
                if (data != null && data.status == true) {
                    getModelsOfMark(button.getAttribute('brand'), true);
                }
            })
            .catch(error => console.error("An error occurred while retrieving data:", error));
    }
}
function addBrand(button) {
    const input = document.getElementById('new-brand-name');
    if (input != null && input.value !== '') {
        fetch(`/api/v1/brands/add?brand=${input.value}`, {
            method: 'POST',
        })
            .then(response => response.json())
            .then(data => {
                if (data != null && data.status == true)
                    getBrands();
            })
            .catch(error => console.error("An error occurred while retrieving data:", error));
    }
}
function deleteBrand(button) {
    if (confirm(`Видалити марку "${button.getAttribute('brand')}"?`)) {
        fetch(`/api/v1/brands/delete?brand=${button.getAttribute('brand')}`, {
            method: 'PUT',
        })
            .then(response => response.json())
            .then(data => {
                if (data != null && data.status == true)
                    getBrands();
            })
            .catch(error => console.error("An error occurred while retrieving data:", error));
    }
}
function deleteModel(button) {
    if (confirm(`Видалити модель "${button.getAttribute('model')}" у марки "${button.getAttribute('brand')}"?`)) {
        fetch(`/api/v1/brands/deleteModel?brand=${button.getAttribute('brand')}&model=${button.getAttribute('model')}`, {
            method: 'PUT',
        })
            .then(response => response.json())
            .then(data => {
                if (data != null && data.status == true)
                    getModelsOfMark(button.getAttribute('brand'), true);
            })
            .catch(error => console.error("An error occurred while retrieving data:", error));
    }
}
function editModel(button) {
    const input = document.getElementById('edit-model-name');
    if (input != null && input.value !== '') {
        let newName = input.value;
        let oldName = button.getAttribute('model');
        if (confirm(`Змінити назву моделі з "${oldName}" на "${newName}?"`)) {
            fetch(`/api/v1/brands/editModel?brand=${button.getAttribute('brand')}&newName=${newName}&oldName=${oldName}`, {
                method: 'PUT',
            })
                .then(response => response.json())
                .then(data => {
                    if (data != null && data.status == true)
                        getModelsOfMark(button.getAttribute('brand'), true);
                })
                .catch(error => console.error("An error occurred while retrieving data:", error));
        }
    }
}
function changeModelToEditMode(button) {
    showModels(button.getAttribute('brand'));
    let modelsCont = document.getElementById('modelsCont');
    if (modelsCont == null) return;
    let brand = button.getAttribute('brand');
    let model = button.getAttribute('model');
    for (let i = 0; i < modelsCont.children.length; i++) {
        if (modelsCont.children[i].textContent.replace(/[\n\r]+|[\s]{2,}/g, '') === model) {
            modelsCont.children[i].innerHTML = `<input type="text" placeholder="Нова назва" value="${button.getAttribute('model')}" id="edit-model-name"/><div model_buttons>
                    <button onclick="editModel(this)" brand="${brand.replace(' ', '_')}" model="${model.replace(' ', '_')}" class="model_buttons-submit"><span>${svgCodes.submit}
                    </span></button><button onclick="getModelsOfMark('${brand}')" brand="${brand.replace(' ', '_')}" model="${model.replace(' ', '_')}" class="model_buttons-delete"><span>${svgCodes.cancel}
                    </span></button></div>`;
            break;
        }
    }
}
async function getModelsOfMark(brand, forced = false) {
    brand = brand.replace('_', ' ');
    if (modelsCache[brand] == null || forced) {
        fetch(`/home/GetModels?brand=${brand}`)
            .then(response => response.json())
            .then(data => {
                data.models = data.models.filter((n) => { return n != 'Інше' });
                modelsCache[brand] = data.models;
                showModels(brand);
            })
            .catch(error => console.error("An error occurred while retrieving data:", error));
    }
    else
        showModels(brand);
}
function showModels(brand) {
    let modelsContainer = document.getElementById('modelsCont');
    if (modelsContainer == null) return;
    modelsContainer.innerHTML = `<div class="new-model"><input type="text" placeholder="Назва моделі" id="new-model-name"/><button onclick="addModel(this)" brand="${brand}"><span>${svgCodes.add}</span></button></div>`;
    modelsCache[brand].forEach(model => {
        modelsContainer.innerHTML += `<div class="model"><p>${model}</p><div model_buttons>
                    <button onclick="changeModelToEditMode(this)" brand="${brand.replace(' ', '_')}" model="${model.replace(' ', '_')}" class="model_buttons-edit"><span>${svgCodes.edit}
                    </span></button><button onclick="deleteModel(this)" brand="${brand.replace(' ', '_')}" model="${model.replace(' ', '_')}" class="model_buttons-delete"><span>${svgCodes.delete}
                    </span></button></div></div>`;
    });
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
        case 6: {
            return "Електро";
        }
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