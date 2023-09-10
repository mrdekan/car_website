const similarCarsBlock = document.getElementById('similar-cars');
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

let currentSlide = 0;

if (buyButtonNotLogged) {
    buyButtonNotLogged.addEventListener('click', () => {
        popup.style.display = "block";
        disableScrolling();
        buyRequestNameInp.value = localStorage.getItem('name');
        buyRequestPhoneInp.value = localStorage.getItem('phone');
    });
}
closeButton.addEventListener("click", function () {
    popup.style.display = "none";
    enableScrolling();
});
popup.addEventListener("mousedown", function (event) {
    if (event.target == popup) {
        popup.style.display = "none";
        enableScrolling();
    }
});
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
    if (currentSlide >= slides.length) currentSlide = 0;
    else if (currentSlide < 0) currentSlide = slides.length - 1;
    slides.forEach((slide) => {
        slide.style.display = 'none';
    });
    slides[currentSlide].style.display = 'flex';
}
showSlide();
sliderNext.addEventListener('click', () => {
    currentSlide++;
    showSlide();
});
sliderPrev.addEventListener('click', () => {
    currentSlide--;
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
function getSimilarCars() {
    fetch(`/Car/FindSimilarCars/${similarCarsBlock.getAttribute('carId')}`)
        .then(response => response.json())
        .then(data => {
            if (data != null && data.success == true) {
                similarCarsBlock.innerHTML = '';
                data.cars.forEach(car => {
                    similarCarsBlock.innerHTML += `<div class="similarCar">
                        <a href="/Car/Detail/${car.id}">${car.info}</a>
                        <img alt="photo" src="${car.photoURL}">
                        <div class="similarCar_container-after">
                            <div class="car_container-right-like similar">
                                  <input type="checkbox" class="car_container-right-like-cars" carId="${car.id}" carId="${car.id}" ${car.liked ? "checked" : ""}/>
                                  <span class="car_container-right-span similar"></span>
                            </div>
                            <div class="car_container-right-price similar">
                                <p class="car_container-right-price-USD similar">${car.price} $</p>
                                <p class="car_container-right-price-UAH similar">≈ ${car.priceUAH} грн</p>
                            </div>
                        </div>
                    </div>`;
                });
            }

        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
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
                    buyButton.innerHTML = 'Запит на покупку';
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