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
    else
        document.getElementById('incomingCarsCount').innerHTML = obj.count;
}
else
    getIncomingCarsCount();


function getIncomingCarsCount() {
    fetch(`/api/v1/cars/getIncomingCarsCount`)
        .then(response => response.json())
        .then(data => {
            if (data.code === 200 && data.count != undefined && data.count != null) {
                localStorage.setItem('incomingCarsCount', JSON.stringify({ time: Date.now(), count: data.count }));
                const elem = document.getElementById('incomingCarsCount');
                elem.innerHTML = data.count;
                if (data.count != 0)
                    elem.style.display = 'block';
                else 
                    elem.style.display = 'none';
            }
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}