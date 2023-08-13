const similarCarsBlock = document.getElementById('similar-cars');
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
                        <p>${car.price} $</p>
                        <p>${car.priceUAH} грн</p>
                    </div>`;
                });
            }
            
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
//#endregion