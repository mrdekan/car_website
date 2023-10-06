const sellerBlock = document.getElementById('seller');

fetch(`/api/v1/users/getById/${sellerBlock.getAttribute('sellerId')}`)
    .then(response => response.json())
    .then(data => {
        if (data != null && data.status == true) {
        sellerBlock.innerHTML = `<div><h4>Продавець:</h4>
        <a href="/user/detail/${data.user.id}">${data.user.surname} ${data.user.name}</a>
        <span>+${data.user.phoneNumber}</span></div><div>
        <p>Авто у продажі: <span>${data.user.carsInSaleCount}</span></p>
        <p>Авто очікують модерації: <span>${data.user.waitingCarsCount}</span></p>
        <p>Роль: <span>${roleName(data.user.role)}</span></p>
        </div>`;
        }
    })
    .catch(error => console.error("An error occurred while retrieving data:", error));

function roleName(index) {
    switch (index) {
        case 0:
            return 'Користувач';
        case 1:
            return 'Адмін';
        case 2:
            return 'Розробник';
        case 3:
            return 'Модератор';
    }
}