import { getIncomingCarsCount } from './modules/getIncomingCarsCount.js';
const burgerButton = document.getElementById('nav-icon2');
const menu = document.getElementById('dropdown-menu');
burgerButton.addEventListener("click", function () {
    this.classList.toggle("open");
    menu.classList.toggle("open");
});
const oldData = localStorage.getItem('incomingCarsCount');
let needReload = false;
if (oldData) {
    const obj = JSON.parse(oldData);
    if (!obj.count || Date.now() - obj.time > 600000)
        getIncomingCarsCount();
    else {
        const elem = document.getElementById('incomingCarsCount');
        const elemMob = document.getElementById('incomingCarsCountMob');
        elem.innerHTML = obj.count;
        elemMob.innerHTML = obj.count;
        elem.style.display = 'block';
        elemMob.style.display = 'block';
    }
}
else
    getIncomingCarsCount();
