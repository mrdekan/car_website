import svgCodes from "./svgCodesConst.js";
const similarCarsBlock = document.getElementById('similar-cars');
const similarCarsBlockMobile = document.getElementById('similar-cars-mobile');
const buyButton = document.getElementById('buy-car');
const buyButtonNotLogged = document.getElementById('buy-car-notLogged');
const buyResult = document.getElementById('buy-request-result');
const sliderNext = document.getElementById('next');
const sliderPrev = document.getElementById('prev');
const slides = document.querySelectorAll('.slider_element');
const popup = document.getElementById('popup');
const closeButton = document.getElementById('closePopup');
const sendNotLoggedInRequest = document.getElementById('notLoggedIngRequestSend');
const buyRequestNameInp = document.getElementById('request-name');
const buyRequestPhoneInp = document.getElementById('request-phone');
const imagesFullscreen = document.getElementById('img_full');
const photos = document.querySelectorAll('.slider_element-image');
const vin = document.getElementById('vin');
const photosRadios = document.getElementsByName('radio-btn');
let currentSlide = 0;
window.addEventListener("keydown", escKeyPress);
function escKeyPress(event) {
    //27 == ESC
    if (event.keyCode === 27) {
        imagesFullscreen.style.display = "none";
        enableScrolling();
    }
}
if (vin != null) {
    vin.addEventListener('click', () => {
        var newTab = window.open(`https://bidfax.info/?do=search&subaction=search&story=${vin.getAttribute('vin')}`, '_blank');
        newTab.focus();
    });
}
Array.from(photos).forEach(photo => {
    photo.addEventListener('click', () => {
        disableScrolling();
        imagesFullscreen.style.display = "block";
    });
});
if (buyButtonNotLogged) {
    buyButtonNotLogged.addEventListener('click', () => {
        popup.style.display = "block";
        disableScrolling();
        buyRequestNameInp.value = localStorage.getItem('name');
        buyRequestPhoneInp.value = localStorage.getItem('phone');
    });
}
imagesFullscreen.addEventListener("click", function (event) {
    if (event.target.tagName != 'IMG') {
        imagesFullscreen.style.display = "none";
        enableScrolling();
    }
    else {
        var newTab = window.open(event.target.getAttribute('src'), '_blank');
        newTab.focus();
    }
});
if (closeButton)
closeButton.addEventListener("click", function () {
    popup.style.display = "none";
    enableScrolling();
});
if (popup)
popup.addEventListener("mousedown", function (event) {
    if (event.target == popup) {
        popup.style.display = "none";
        enableScrolling();
    }
});
if (sendNotLoggedInRequest)
sendNotLoggedInRequest.addEventListener('click', () => {
    let name = buyRequestNameInp.value;
    localStorage.setItem('name', name);
    let phone = buyRequestPhoneInp.value;
    localStorage.setItem('phone', phone);
    var url = new URL(window.location.href);
    var pathSegments = url.pathname.split('/');
    var carId = pathSegments[pathSegments.length - 1];
    if (name && phone && name != '' && phone != '') {
        fetch(`/api/v1/cars/buyRequestNotLoggedIn?carId=${carId}&name=${name}&phone=${phone}`, {
            method: "POST"
        }).then(response => response.json())
            .then(data => {
                if (data && data.status == true) {
                    popup.style.display = "none";
                    enableScrolling();
                    let resBlock = document.createElement("div");
                    resBlock.innerHTML = `
                    <p class="buy-request-result-info-title">Дякуємо за ваш запит!</p>
                    <p>'Наш менеджер зв\'яжеться з вами найближчим часом для надання додаткової інформації та обговорення деталей.'</p>`;
                    resBlock.classList.add("buy-request-result-info")

                    buyResult.innerHTML = '';
                    buyResult.appendChild(resBlock);
                    setTimeout(function () {
                        resBlock.style.transform = "scaleY(1)";
                    }, 10);
                    setTimeout(function () {
                        resBlock.style.transform = "scaleY(0)";
                        resBlock.style.margin = '0 0 0 0';
                        resBlock.style.height = '0px';
                        resBlock.style.padding = '0 2%';
                        resBlock.style.border = '0px solid black';
                    }, 10000);
                    setTimeout(function () {
                        if (buyResult.contains(resBlock))
                            buyResult.removeChild(resBlock);
                    }, 10100);
                }
            })
            .catch(error => console.error("An error occurred while retrieving data:", error));
    }
});
function showSlide() {
    for (let i = 0; i < photosRadios.length; i++) {
        if (i == currentSlide)
            photosRadios[i].checked = true;
        else
            photosRadios[i].checked = false;
    }
}
function getSlide() {
    for (let i = 0; i < photosRadios.length; i++) {
        if (photosRadios[i].checked)
            return i;
    }
}
sliderNext.addEventListener('click', () => {
    currentSlide = getSlide();
    currentSlide++;
    if (currentSlide >= photosRadios.length) currentSlide = 0;
    showSlide();
});
sliderPrev.addEventListener('click', () => {
    currentSlide = getSlide();
    currentSlide--;
    if (currentSlide < 0) currentSlide = photosRadios.length - 1;
    showSlide();
});

function disableScrolling() {
    var x = window.scrollX;
    var y = window.scrollY;
    window.onscroll = function () { window.scrollTo(x, y); };
}

function enableScrolling() {
    window.onscroll = function () { };
}

getSimilarCars();



//#region Ajax requests
function updateLikeButtons() {
    let likeButtons = document.getElementsByClassName("car_container-right-like-cars");
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
function getSimilarCars() {
    if (!similarCarsBlock && !similarCarsBlockMobile) return;
    fetch(`/Car/FindSimilarCars/${similarCarsBlock.getAttribute('carId')}`)
        .then(response => response.json())
        .then(data => {
            if (data != null && data.success == true) {
                similarCarsBlock.innerHTML = '';
                similarCarsBlockMobile.innerHTML = '';
                data.cars.forEach(car => {
                    similarCarsBlock.innerHTML += formPcCar(car);
                    similarCarsBlockMobile.innerHTML += formCar(car);
                });
                updateLikeButtons();
            }

        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
function formPcCar(car) {
    return `<div class="similarCar">
                        <a href="/Car/Detail/${car.id}">${car.brand} ${car.model} ${car.year}</a>
                        <img alt="${car.brand} ${car.model} ${car.year}" src="${car.previewURL}">
                        <div class="similarCar_container-after">
                            <div class="car_container-right-like similar">
                                  <input type="checkbox" class="car_container-right-like-cars" carId="${car.id}" ${car.liked ? "checked" : ""}/>
                                  <span class="car_container-right-heart">
                                    <svg width="40" height="40" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg"><path fill-rule="evenodd" clip-rule="evenodd" d="M8.272 4.95258C7.174 4.95258 6.077 5.34958 5.241 6.14458C4.441 6.90658 4 7.91758 4 8.99158C4 10.0646 4.441 11.0756 5.241 11.8376L12 18.2696L18.759 11.8376C19.559 11.0756 20 10.0646 20 8.99158C20 7.91858 19.559 6.90658 18.759 6.14458C17.088 4.55458 14.368 4.55458 12.697 6.14458L12 6.80858L11.303 6.14458C10.467 5.34958 9.37 4.95258 8.272 4.95258ZM12 20.9996L3.847 13.2406C2.656 12.1076 2 10.5986 2 8.99158C2 7.38458 2.656 5.87558 3.847 4.74158C6.067 2.62858 9.552 2.43858 12 4.16858C14.448 2.43858 17.933 2.62858 20.153 4.74158C21.344 5.87558 22 7.38458 22 8.99158C22 10.5986 21.344 12.1076 20.153 13.2406L12 20.9996Z" fill="currentColor"/></svg>
                                  </span>
                                  <span class="car_container-right-span"><svg width="40" height="40" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                                    <path fill-rule="evenodd" clip-rule="evenodd" d="M12 20.9996L3.847 13.2406C2.656 12.1076 2 10.5986 2 8.99158C2 7.38458 2.656 5.87558 3.847 4.74158C6.067 2.62858 9.552 2.43858 12 4.16858C14.448 2.43858 17.933 2.62858 20.153 4.74158C21.344 5.87558 22 7.38458 22 8.99158C22 10.5986 21.344 12.1076 20.153 13.2406L12 20.9996Z" fill="currentColor"/></svg>
                                  </span>
                            </div>
                            <div class="car_container-right-price similar">
                                <p class="car_container-right-price-USD similar">${car.price} $</p>
                                <p class="car_container-right-price-UAH similar">≈ ${car.priceUAH} грн</p>
                            </div>
                        </div>
                    </div>`;
}
function formCar(car, waiting) {
    return `<a class="car mainPageCar" href="/Car/Detail/${car.id}">
                                  <p class="car_name">${car.brand} ${car.model} ${car.year}</p>
                                  <div class="car_container">
                                       <div class="car_container-img"> <div class="car_container-img-landscape"><img  alt="photo" src="${car.previewURL}" /></div></div>
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
                                 ${(waiting ? '' : `<input type="checkbox" class="car_container-right-like-cars" carId="${car.id}" carId="${car.id}" ${car.liked ? "checked" : ""}/>
                   <span class="car_container-right-heart">
                                    <svg width="40" height="40" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg"><path fill-rule="evenodd" clip-rule="evenodd" d="M8.272 4.95258C7.174 4.95258 6.077 5.34958 5.241 6.14458C4.441 6.90658 4 7.91758 4 8.99158C4 10.0646 4.441 11.0756 5.241 11.8376L12 18.2696L18.759 11.8376C19.559 11.0756 20 10.0646 20 8.99158C20 7.91858 19.559 6.90658 18.759 6.14458C17.088 4.55458 14.368 4.55458 12.697 6.14458L12 6.80858L11.303 6.14458C10.467 5.34958 9.37 4.95258 8.272 4.95258ZM12 20.9996L3.847 13.2406C2.656 12.1076 2 10.5986 2 8.99158C2 7.38458 2.656 5.87558 3.847 4.74158C6.067 2.62858 9.552 2.43858 12 4.16858C14.448 2.43858 17.933 2.62858 20.153 4.74158C21.344 5.87558 22 7.38458 22 8.99158C22 10.5986 21.344 12.1076 20.153 13.2406L12 20.9996Z" fill="currentColor"/></svg>
                                  </span>
                                  <span class="car_container-right-span"><svg width="40" height="40" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                                    <path fill-rule="evenodd" clip-rule="evenodd" d="M12 20.9996L3.847 13.2406C2.656 12.1076 2 10.5986 2 8.99158C2 7.38458 2.656 5.87558 3.847 4.74158C6.067 2.62858 9.552 2.43858 12 4.16858C14.448 2.43858 17.933 2.62858 20.153 4.74158C21.344 5.87558 22 7.38458 22 8.99158C22 10.5986 21.344 12.1076 20.153 13.2406L12 20.9996Z" fill="currentColor"/></svg>
                                  </span>

                   `)}
                                  </div>
                                  </div>
                                  </a>`;
}
if (buyButton) {
    buyButton.addEventListener('click', () => {
        buyRequest();
    });
}
function buyRequest() {
    fetch(`/Car/BuyRequest?id=${buyButton.getAttribute('carId')}&cancel=${buyButton.hasAttribute('cancel')}`)
        .then(response => response.json())
        .then(data => {
            //successCode == 0 --> some error
            //successCode == 1 --> success
            //successCode == 2 --> user not logged in
            let resBlock = document.createElement("div");
            if (data != null && data.successCode == 0) {
                resBlock.innerHTML = `
                    <p class="buy-request-result-info-title">${buyButton.hasAttribute('cancel') ? 'Запит скасовано.' : 'Дякуємо за ваш запит!'} </p>
                    <p>${buyButton.hasAttribute('cancel') ? 'Якщо запит було скасовано випадково, то Ви можете розмістити його ще раз.' : 'Наш менеджер зв\'яжеться з вами найближчим часом для надання додаткової інформації та обговорення деталей.'}</p>`;
                resBlock.classList.add("buy-request-result-info")
                if (buyButton.hasAttribute('cancel')) {
                    buyButton.removeAttribute('cancel');
                    buyButton.innerHTML = 'Купити';
                    buyButton.classList.remove('cancelBuy');
                }
                else {
                    buyButton.setAttribute('cancel', '');
                    buyButton.innerHTML = 'Скасувати запит';
                    buyButton.classList.add('cancelBuy');
                }
            }
            else if (data == null || data.successCode == 1) {
                resBlock.innerHTML = `
                    <p class="buy-request-result-error-title">Вибачте за незручності. Сталася помилка.</p>
                    <p>Будь ласка, оновіть сторінку або спробуйте пізніше.</p>`;
                resBlock.classList.add("buy-request-result-error")
            }
            else if (data.successCode == 2) {
                window.location.href = '/User/Login';
            }
            buyResult.innerHTML = '';
            buyResult.appendChild(resBlock);
            setTimeout(function () {
                resBlock.style.transform = "scaleY(1)";
            }, 10);
            setTimeout(function () {
                resBlock.style.transform = "scaleY(0)";
                resBlock.style.margin = '0 0 0 0';
                resBlock.style.height = '0px';
                resBlock.style.padding = '0 2%';
                resBlock.style.border = '0px solid black';
            }, 10000);
            setTimeout(function () {
                if (buyResult.contains(resBlock))
                    buyResult.removeChild(resBlock);
            }, 10100);
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
//#endregion
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