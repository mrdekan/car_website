﻿const sellerBlock = document.getElementById('seller');
fetch(`/api/v1/users/getById/${sellerBlock.getAttribute('sellerId')}`)
    .then(response => response.json())
    .then(data => {
        if (data != null && data.status == true) {
        sellerBlock.innerHTML = `<div><h4>Продавець:</h4>
        <a href="/user/detail/${data.user.id}">${data.user.surname} ${data.user.name}</a>
        <span>+<span class="copy-on-click" style="display: inline;" copy-text="@Model.Phone" title="Натисніть щоб скопіювати">${data.user.phoneNumber}</span></span></div><div>
        <p>У продажі: <span>${data.user.carsInSaleCount}</span></p>
        <p>Очікують: <span>${data.user.waitingCarsCount}</span></p>
        <p>Роль: <span>${roleName(data.user.role)}</span></p>
        </div>`;
        }
    })
    .catch(error => console.error("An error occurred while retrieving data:", error));
const roleName = (index)=>
    ['Користувач', 'Адмін', 'Розробник', 'Модератор'][index];