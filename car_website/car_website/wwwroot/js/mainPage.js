//#region constants
const apply_button = document.getElementById("refresh_cars");
const brand_select = document.getElementById("brand-select");
const model_select = document.getElementById("model-select");
const body_Type_select = document.getElementById("bodyType_select");
const year_min_select = document.getElementById("year_min-select");
const year_max_select = document.getElementById("year_max-select");
const price_max_input = document.getElementById("price_max-input");
const price_min_input = document.getElementById("price_min-input");
const race_max_input = document.getElementById("race_max-input");
const race_min_input = document.getElementById("race_min-input");
const transmission_select = document.getElementById("transmission-select");
const fuel_select = document.getElementById("fuel-select");
const driveline_select = document.getElementById("driveline-select");
const engineVolume_min_input = document.getElementById("engineVolume_min-input");
const engineVolume_max_input = document.getElementById("engineVolume_max-input");
const clear_filters = document.getElementById("clear_filters");
let likeButtons = document.getElementsByClassName("like_cars"); 
//#endregion
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
function clearFilters() {
    engineVolume_max_input.value = "";
    engineVolume_min_input.value = "";
    price_max_input.value = "";
    price_min_input.value = "";
    race_max_input.value = "";
    race_min_input.value = "";
    body_Type_select.value = 0;
    brand_select.value = "Any";
    driveline_select.value = 0;
    fuel_select.value = 0;
    model_select.value = "Any";
    transmission_select.value = 0;
    year_max_select.value = 0;
    year_min_select.value = 0;
}
clear_filters.addEventListener("click", () => {
    clearFilters();
    applyFilter();
});
//#region input fields settings
engineVolume_min_input.addEventListener('input', function (event) {
    const maxLength = parseInt(event.target.getAttribute('maxlength'));
    let currentValue = event.target.value;
    currentValue = currentValue.replace(/[^\d.,]/g, ''); // Updated regular expression to include dots and commas
    if (currentValue.length > maxLength) {
        currentValue = currentValue.slice(0, maxLength);
    }
    event.target.value = currentValue;
});
engineVolume_max_input.addEventListener('input', function (event) {
    const maxLength = parseInt(event.target.getAttribute('maxlength'));
    let currentValue = event.target.value;
    currentValue = currentValue.replace(/[^\d.,]/g, ''); // Updated regular expression to include dots and commas
    if (currentValue.length > maxLength) {
        currentValue = currentValue.slice(0, maxLength);
    }
    event.target.value = currentValue;
});
price_max_input.addEventListener('input', function (event) {
    const maxLength = parseInt(event.target.getAttribute('maxlength'));
    let currentValue = event.target.value;
    currentValue = currentValue.replace(/[^\d]/g, '');
    if (currentValue.length > maxLength) {
        currentValue = currentValue.slice(0, maxLength);
    }
    event.target.value = currentValue;
});
price_min_input.addEventListener('input', function (event) {
    const maxLength = parseInt(event.target.getAttribute('maxlength'));
    let currentValue = event.target.value;
    currentValue = currentValue.replace(/[^\d]/g, '');
    if (currentValue.length > maxLength) {
        currentValue = currentValue.slice(0, maxLength);
    }
    event.target.value = currentValue;
});
race_max_input.addEventListener('input', function (event) {
    const maxLength = parseInt(event.target.getAttribute('maxlength'));
    let currentValue = event.target.value;
    currentValue = currentValue.replace(/[^\d]/g, '');
    if (currentValue.length > maxLength) {
        currentValue = currentValue.slice(0, maxLength);
    }
    event.target.value = currentValue;
});
race_min_input.addEventListener('input', function (event) {
    const maxLength = parseInt(event.target.getAttribute('maxlength'));
    let currentValue = event.target.value;
    currentValue = currentValue.replace(/[^\d]/g, '');
    if (currentValue.length > maxLength) {
        currentValue = currentValue.slice(0, maxLength);
    }
    event.target.value = currentValue;
});
apply_button.onclick = () => applyFilter();
brand_select.addEventListener('change', function () {
    if (brand_select.value == "Any")
        model_select.innerHTML = '<option value="Any">Усі</option>';
    else {
        getModelsOfMark();
    }
});
//#endregion

applyFilter();
if (brand_select.value != "Any")
    getModelsOfMark()

//#region Ajax requests
function getModelsOfMark() {
    fetch(`/home/GetModels?brand=${brand_select.value}`)
        .then(response => response.json())
        .then(data => {
            model_select.innerHTML = '<option value="Any">Усі</option>';
            console.log(data);
            data.models.forEach(model => {
                if(model!='Інше')
                model_select.innerHTML += `<option value=${model.replace(' ', '_')}>${model}</option>`;
            });
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
function applyFilter() {
    const filters = {
        body: Number(body_Type_select.value),
        brand: brand_select.value,
        model: model_select.value,
        minYear: Number(year_min_select.value),
        maxYear: Number(year_max_select.value),
        minPrice: Number(price_min_input.value),
        maxPrice: Number(price_max_input.value),
        carTransmission: Number(transmission_select.value),
        fuel: Number(fuel_select.value),
        driveLine: Number(driveline_select.value),
        minEngineCapacity: Number(engineVolume_min_input.value),
        maxEngineCapacity: Number(engineVolume_max_input.value),
        minMileage: Number(race_min_input.value),
        maxMileage: Number(race_max_input.value)
    };
    fetch(`/home/GetCars`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(filters)
    })
        .then(response => response.json())
        .then(data => {
            console.log(data);
            const carList = document.getElementById("carList");
            carList.innerHTML = ""; //Clean up the old list
            //Form a block for each machine
            data.cars.forEach(car => {
                const block = `<div class="car">
                                  <div class="car_container">
                                        <img  alt="photo" src="${car.photosURL[0]}" />
                                    <div class="car_container-info">
                                        <a href="/Car/Detail/${car.id}">${car.brand} ${car.model} ${car.year}</a>
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
                                  <input type="checkbox" class="car_container-right-like-cars" carId="${car.id}" ${car.liked ? "checked" : ""}/>
                                  <span class="car_container-right-span"></span>
                                  </div>
                                  </div>
                                </div>`;
                carList.innerHTML += block;
            });
            
            updateLikeButtons();
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
//<img alt="#" src="../img/heart.svg" /> </button>



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