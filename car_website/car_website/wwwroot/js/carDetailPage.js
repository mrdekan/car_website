const similarCarsBlock = document.getElementById('similar-cars');
const buyButton = document.getElementById('buy-car');
const buyResult = document.getElementById('buy-request-result');
getSimilarCars();


buyButton.addEventListener('click', () => {
    buyRequest();
    if (buyButton.hasAttribute('cancel')) {
        buyButton.removeAttribute('cancel');
        buyButton.innerHTML = 'Запит на покупку';
    }
    else {
        buyButton.setAttribute('cancel','');
        buyButton.innerHTML = 'Скасувати запит';
    }
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
            if (data != null && data.successCode == 1) {
                buyResult.innerHTML += `<div class="buy-request-result-info">
                    <p class="buy-request-result-info-title">Дякуємо за ваш запит!</p>
                    <p>Наш менеджер зв'яжеться з вами найближчим часом для надання додаткової інформації та обговорення деталей.</p>
                </div>`;
            }
            else if (data == null || data.successCode == 0) {
                buyResult.innerHTML += `<div class="buy-request-result-error">
                    <p class="buy-request-result-error-title">Вибачте за незручності. Сталася помилка.</p>
                    <p>Будь ласка, оновіть сторінку або спробуйте пізніше.</p>
                </div>`;
            }
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
//#endregion