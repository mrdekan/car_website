const burgerButton = document.getElementById('nav-icon2');
const menu = document.getElementById('dropdown-menu');
burgerButton.addEventListener("click", function () {
    this.classList.toggle("open");
    menu.classList.toggle("open");
});