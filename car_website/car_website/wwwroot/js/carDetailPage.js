const similarCarsBlock = document.getElementById('similar-cars');
const buyButton = document.getElementById('buy-car');
const buyResult = document.getElementById('buy-request-result');
const sliderNext = document.getElementById('next');
const sliderPrev = document.getElementById('prev');
const slides = document.querySelectorAll('.slider_element');
let currentSlide = 0;

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

getSimilarCars();


buyButton.addEventListener('click', () => {
    buyRequest();

});
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
                        <p>${car.price} $</p>
                        <p>≈ ${car.priceUAH} грн</p>
                    </div>`;
                });
            }

        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
function buyRequest() {
    fetch(`/Car/BuyRequest?id=${buyButton.getAttribute('carId')}&cancel=${buyButton.hasAttribute('cancel')}`)
        .then(response => response.json())
        .then(data => {
            //successCode == 0 --> some error
            //successCode == 1 --> success
            //successCode == 2 --> user not logged in
            let resBlock = document.createElement("div");
            if (data != null && data.successCode == 1) {
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
            else if (data == null || data.successCode == 0) {
                resBlock.innerHTML = `
                    <p class="buy-request-result-error-title">Вибачте за незручності. Сталася помилка.</p>
                    <p>Будь ласка, оновіть сторінку або спробуйте пізніше.</p>`;
                resBlock.classList.add("buy-request-result-error")
            }
            buyResult.innerHTML = '';
            buyResult.appendChild(resBlock);
            setTimeout(function () {
                resBlock.style.transform = "scaleY(1)"
            }, 10);
            setTimeout(function () {
                resBlock.style.transform = "scaleY(0)"
            }, 10000);
            setTimeout(function () {
                buyResult.removeChild(resBlock);
            }, 10200);
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
//#endregion