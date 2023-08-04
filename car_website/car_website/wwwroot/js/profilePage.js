

if (document.getElementById("favList")!=null)
    getFavorites();

function updateLikeButtons() {
    likeButtons = document.getElementsByClassName("car_container-right-like-cars");
    Array.from(likeButtons).forEach(like => {
        like.addEventListener('change', function (event) {
            fetch(`/car/like?carId=${event.target.getAttribute('carId')}&isLiked=${like.checked}`)
                .then(response => response.json())
                .then(data => {
                    //console.log(data);
                    if (data.success == false) like.checked = !like.checked;
                })
                .catch(error => console.error("An error occurred while retrieving data:", error));
        });
    });
}
//#region Ajax requests
function getFavorites() {
    fetch(`/User/GetFavoriteCars`)
        .then(response => response.json())
        .then(data => {
            console.log(data);
            const favList = document.getElementById("favList");
            favList.innerHTML = ""; //Clean up the old list
            //Form a block for each machine
            data.cars.forEach(car => {
                const block = `<div class="car">
                                  <div class="car_container">
                                        <img  alt="photo" src="${car.photosURL[0]}" />
                                    <div class="car_container-info">
                                        <a asp-controller="Car" asp-action="Detail" asp-route-id="${car.id}" href="/Car/Detail/${car.id}">${car.brand} ${car.model} ${car.year}</a>
                                            <div class="car_container-info-parameters">
                                                <div class="car_container-info-parameters-row">
                                                    <p class="car_container-info-parameters-row-race">${car.mileage} тис. км</p>
                                                    <p class="car_container-info-parameters-row-fuel">${fuelName(car.fuel)}, ${car.engineCapacity} л.</p>
                                                </div>
                                                <div class="car_container-info-parameters-row">
                                                    <p class="car_container-info-parameters-row-transmission">${transmissionName(car.carTransmission)}</p>
                                                    <p class="car_container-info-parameters-row-driveline">${drivelineName(car.driveline)}</p>
                                                </div>
                                                <div class="car_container-info-parameters-row">
                                                    <p class="car_container-info-parameters-row-vin">${car.vin}</p>
                                                </div>
                                            </div>
                                            
                                        </div>
                                    </div>
                                    <div class="car_container-right">
                                    <div class="car_container-right-price">
                                                <p class="car_container-right-price-USD">${formatNumberWithThousandsSeparator(car.price)} $</p>
                                                <p class="car_container-right-price-UAH">≈ ${formatNumberWithThousandsSeparator(car.priceUAH)} грн</p>
                                            </div>
                                  <div class="car_container-right-like">
                                  <input type="checkbox" class="car_container-right-like-cars" carId="${car.id}" checked/>
                                  <span class="car_container-right-span"></span>
                                  </div>
                                  </div>
                                </div>`;
                favList.innerHTML += block;
            });
            updateLikeButtons();
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
//#endregion


//#region info displaying

function fuelName(id) {
    switch (id) {
        case 1: {
            return "Газ";
        }
        case 2: {
            return "Газ/Бензин";
        }
        case 3: {
            return "Бензин";
        }
        case 4: {
            return "Дизель";
        }
        case 5: {
            return "Гібрид";
        }
        case 16: {
            return "Електро";
        }
    }
}
function transmissionName(id) {
    switch (id) {
        case 1: {
            return "Механічна";
        }
        case 2: {
            return "Автомат";
        }
    }
}
function drivelineName(id) {
    switch (id) {
        case 1: {
            return "Передній";
        }
        case 2: {
            return "Задній";
        }
        case 3: {
            return "Повний";
        }
    }
}
function formatNumberWithThousandsSeparator(number) {
    return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ' ');
}
//#endregion