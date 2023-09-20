const svgCodes = {
    edit: `<svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg"><g clip-path="url(#clip0_217_311)"><path d="M21 13V20C21 20.5523 20.5523 21 20 21H4C3.44771 21 3 20.5523 3 20V4C3 3.44771 3.44771 3 4 3H11" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/><path d="M7 13.36V17H10.6586L21 6.65405L17.3475 3L7 13.36Z" stroke="currentColor" stroke-width="2" stroke-linejoin="round"/></g><defs><clipPath id="clip0_217_311"><rect width="24" height="24" fill="white"/></clipPath></defs></svg>`,
    submit: `<svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg"><g clip-path="url(#clip0_235_308)"><path d="M5 12L10 17L20 7" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/></g><defs><clipPath id="clip0_235_308"><rect width="24" height="24" fill="white"/></clipPath></defs></svg>`,
    cancel: `<svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg"><path fill-rule="evenodd" clip-rule="evenodd" d="M13.4143 12.0002L18.7072 6.70725C19.0982 6.31625 19.0982 5.68425 18.7072 5.29325C18.3162 4.90225 17.6843 4.90225 17.2933 5.29325L12.0002 10.5862L6.70725 5.29325C6.31625 4.90225 5.68425 4.90225 5.29325 5.29325C4.90225 5.68425 4.90225 6.31625 5.29325 6.70725L10.5862 12.0002L5.29325 17.2933C4.90225 17.6843 4.90225 18.3162 5.29325 18.7072C5.48825 18.9022 5.74425 19.0002 6.00025 19.0002C6.25625 19.0002 6.51225 18.9022 6.70725 18.7072L12.0002 13.4143L17.2933 18.7072C17.4883 18.9022 17.7442 19.0002 18.0002 19.0002C18.2562 19.0002 18.5122 18.9022 18.7072 18.7072C19.0982 18.3162 19.0982 17.6843 18.7072 17.2933L13.4143 12.0002Z" fill="currentColor"/></svg>`,
};
const radioButtons = document.querySelectorAll('input[name="page"]');
const pageUnderline = document.getElementById('line');
const pages_buttons_containers = document.getElementsByClassName("pages_buttons");
const pages_profile = document.getElementById("pages-profile");
let cars;
let carsPage = 1;
let waitingCars;
let waitingCarsPage = 1;
let buyRequests;
let buyRequestsPage = 1;
let favCars;
let favCarsPage = 1;
let offset;
let selectedRadioButton;
let selectedRadioIndex;
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
    selectedRadioIndex = selectedIndex;
    var computedStyle = getComputedStyle(pages_profile);
    //pageUnderline.style.width = `${radioButtons.length == 4 ? 16.5 : 22}%`;
    let num;
    if (    )
        num = 100;
    else
        num = 66;
    pageUnderline.style.width = `${num / radioButtons.length}%`;
    offset = radioButtons.length == 4 ? 300 : 200;
    let percent = selectedIndex * (offset / (radioButtons.length - 1));
    pageUnderline.style.transform = `translateX(${percent}%)`;
});
var mediaQuery = window.matchMedia('(max-width: 768px)');
function handleMediaChange(mediaQuery) {
    let num;
    if (mediaQuery.matches)
        num = 100;
    else
        num = 66;
    pageUnderline.style.width = `${num / radioButtons.length}%`;
    offset = radioButtons.length == 4 ? 300 : 200;
    let percent = selectedRadioIndex * (offset / (radioButtons.length - 1));
    pageUnderline.style.transform = `translateX(${percent}%)`;
}
mediaQuery.addListener(handleMediaChange);
handleMediaChange(mediaQuery);
radioButtons.forEach((radio, index) => {
    radio.addEventListener('change', () => {
        offset = radioButtons.length == 4 ? 300 : 200;
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
        selectedRadioIndex = 1;
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
        selectedRadioIndex = 3;
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
        selectedRadioIndex = 0;
        carsPage = page;
        if (cars == null || cars.page != carsPage)
            getCars();
        else {
            SetCarsFromData(cars);
            updateLikeButtons();
            updatePagesButtons(cars.pages);
        }
    }
    else if (target.id == "requests") {
        selectedRadioIndex = 2;
        buyRequestsPage = page;
        if (buyRequests == null || buyRequests.page != buyRequestsPage)
            getBuyRequests();
        else {
            SetCarsFromData(buyRequests);
            updateLikeButtons();
            updatePagesButtons(buyRequests.pages);
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
    fetch(`/api/v1/users/getFavoriteCars?page=${buyRequestsPage}`)
        .then(response => response.json())
        .then(data => {
            if (data != null && data.status == true) {
                favCars = data;
                SetCarsFromData(favCars);
                updateLikeButtons();
                updatePagesButtons(data.pages);
            }
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
function getBuyRequests() {
    var url = new URL(window.location.href);
    var pathSegments = url.pathname.split('/');
    var userId = pathSegments[pathSegments.length - 1];
    fetch(`/api/v1/users/getBuyRequests?id=${userId}&page=${favCarsPage}`)
        .then(response => response.json())
        .then(data => {
            if (data != null && data.status == true) {
                buyRequests = data;
                SetCarsFromData(buyRequests);
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
    fetch(`/api/v1/users/getWaitingCars?id=${userId}&page=${waitingCarsPage}`)
        .then(response => response.json())
        .then(data => {
            if (data != null && data.status == true) {
                waitingCars = data;
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
    fetch(`/api/v1/users/getSellingCars?id=${userId}&page=${carsPage}`)
        .then(response => response.json())
        .then(data => {
            if (data != null && data.status == true) {
                cars = data;
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
    if (data != null && data.cars.length != 0 && data.status == true) {
        favList.innerHTML = "";
        data.cars.forEach(car => {
            if (!waitingCarsList) {
                const block = formCar(car,false);
                favList.innerHTML += block;
            }
            else {
                const block = formCar(car.car, true);
                favList.innerHTML += block;
            }
        });
    }
    else if (data == null || data.status == false)
        favList.innerHTML = `<h3 class="warning-text">Щось пішло не так</h3>`;
    else
        favList.innerHTML = `<h3 class="warning-text">Тут ще нічого немає</h3>`;
}
function formCar(car,waiting) {
    return `<div class="car">
                   <div class="car_container">
                   <img  alt="photo" src="${car.photosURL[0]}" />
                   <div class="car_container-info">
                         <a href="/Car/Detail/${car.id}">${car.brand} ${car.model} ${car.year}</a>
                             <div class="car_container-info-parameters">
                                 <div class="car_container-info-parameters-column">
                                     <p class="car_container-info-parameters-column-race">${car.mileage} тис. км</p>
                                     <p class="car_container-info-parameters-column-fuel">${fuelName(car.fuel)}, ${car.engineCapacity} л.</p>
                                     <p class="car_container-info-parameters-column-vin">${car.vin}</p>
                                     </div>
                                 <div class="car_container-info-parameters-column">
                                     <p class="car_container-info-parameters-column-transmission">${transmissionName(car.carTransmission)}</p>
                                     <p class="car_container-info-parameters-column-driveline">${drivelineName(car.driveline)}</p>
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
                   ${(waiting ? '' : `<input type="checkbox" class="car_container-right-like-cars" carId="${car.id}" carId="${car.id}" ${car.liked ? "checked" : ""}/>
                   <span class="car_container-right-span"></span>`)}
                   </div>
                   </div>
                 </div>`;
}
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
function editPhone(button) {
    let phoneCont = document.getElementById('phone_container');
    phoneCont.innerHTML = `<input type="number" placeholder="380xxxxxxxxx" value="${button.getAttribute('phone').replace('+', '')}" id="new-phone"/><button onclick="sendNewPhone(this)" id="submit-phone" phone="${button.getAttribute('phone')}"><span>
    ${svgCodes.submit}</span></button><button phone="${button.getAttribute('phone')}" onclick="cancelEditPhone(this)" id="cancel-phone"><span>${svgCodes.cancel}</span></button>`;
    let inp = document.getElementById('new-phone');
    inp.addEventListener('keydown', (event) => {
        if (event.key === "." || event.key === ",")
            event.preventDefault();
    });
    inp.addEventListener('input', () => {
        if (inp.value.length > 12)
            inp.value = inp.value.slice(0, 12);
    });
}
function editName(button) {
    let nameCont = document.getElementById('name_container');
    nameCont.innerHTML = `<input type="text" placeholder="Прізвище" value="${button.getAttribute('surname')}" id="new-surname"/><input type="text" placeholder="Ім'я" value="${button.getAttribute('name')}" id="new-name"/><button onclick="sendNewName(this)" id="submit-phone" surname="${button.getAttribute('surname')}" name="${button.getAttribute('name')}"><span>
    ${svgCodes.submit}</span></button><button surname="${button.getAttribute('surname')}" name="${button.getAttribute('name')}" onclick="cancelEditName(this)" id="cancel-phone"><span>${svgCodes.cancel}</span></button>`
}
function sendNewName(button) {
    var currentUrl = window.location.href;
    var parts = currentUrl.split("/");
    var userId = parts[parts.length - 1];
    let nameInp = document.getElementById('new-name');
    let surnameInp = document.getElementById('new-surname');
    if (nameInp != null && surnameInp != null) {
        fetch(`/api/v1/users/changeName?newName=${nameInp.value}&newSurname=${surnameInp.value}&userId=${userId}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(response => response.json())
            .then(data => {
                clearNameError();
                let nameCont = document.getElementById('name_container');
                if (data != null && data.status == true && nameCont != null) {
                    nameCont.innerHTML = `<h2>${surnameInp.value} ${nameInp.value}</h2>
                        <button id="change-phone-number" onclick="editName(this)" surname="${surnameInp.value}" name="${nameInp.value}"><span>${svgCodes.edit}</span></button>`;
                } else if (nameCont != null) {
                    let error = document.createElement('span');
                    error.className = "text-danger";
                    error.id = 'name-error';
                    if (data != null && data.code == 400)
                        error.innerHTML = 'Некоректні дані';
                    else
                        error.innerHTML = 'Виникла помилка';
                    nameCont.insertAdjacentElement("afterend", error);
                }
            })
            .catch(error => console.error("An error occurred while retrieving data:", error));
    }
}
function sendNewPhone(button) {
    var currentUrl = window.location.href;
    var parts = currentUrl.split("/");
    var userId = parts[parts.length - 1];
    let phoneInp = document.getElementById('new-phone');
    if (phoneInp != null && phoneInp.value != button.getAttribute('phone')) {
        fetch(`/api/v1/users/changePhone?newPhone=${phoneInp.value}&userId=${userId}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(response => response.json())
            .then(data => {
                clearPhoneError();
                let phoneCont = document.getElementById('phone_container');
                if (data != null && data.status == true && phoneCont != null) {
                    phoneCont.innerHTML = `<h3>+${phoneInp.value}</h3>
                        <button id="change-phone-number" onclick="editPhone(this)" phone="+${phoneInp.value}"><span>${svgCodes.edit}</span></button>`;
                } else if (phoneCont != null) {
                    let error = document.createElement('span');
                    error.className = "text-danger";
                    error.id = 'phone-error';
                    if (data != null && data.code == 400)
                        error.innerHTML = 'Некоректний номер';
                    else
                        error.innerHTML = 'Виникла помилка';
                    phoneCont.insertAdjacentElement("afterend", error);
                }
            })
            .catch(error => console.error("An error occurred while retrieving data:", error));
    }
}
function clearPhoneError() {
    let error = document.getElementById('phone-error');
    if (error) error.remove();
}
function clearNameError() {
    let error = document.getElementById('name-error');
    if (error) error.remove();
}
function cancelEditName(button) {
    clearNameError();
    let nameCont = document.getElementById('name_container');
    nameCont.innerHTML = `<h2>${button.getAttribute('surname')} ${button.getAttribute('name')}</h2>
        <button id="change-phone-number" onclick="editName(this)" surname="${button.getAttribute('surname')}" name="${button.getAttribute('name')}"><span>
            ${svgCodes.edit}
        </span></button>`;
}
function cancelEditPhone(button) {
    clearPhoneError();
    let phoneCont = document.getElementById('phone_container');
    phoneCont.innerHTML = `<h3>${button.getAttribute('phone')}</h3>
        <button id="change-phone-number" onclick="editPhone(this)" phone=${button.getAttribute('phone')}><span>
            ${svgCodes.edit}
        </span></button>`;
}
function copyPhone(button) {
    var textToCopy = button.getAttribute('phone');
    var tempTextArea = document.createElement("textarea");
    tempTextArea.value = textToCopy;
    document.body.appendChild(tempTextArea);
    tempTextArea.select();
    tempTextArea.setSelectionRange(0, 99999);
    document.execCommand("copy");
    document.body.removeChild(tempTextArea);
}