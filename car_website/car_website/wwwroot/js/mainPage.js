﻿const apply_button = document.getElementById("refresh_cars");
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

console.log(brand_select);
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


//getModelsOfMark();
updateCars();



//Ajax requests
function getModelsOfMark() {
    fetch(`/home/GetModels?brand=${brand_select.value}`)
        .then(response => response.json())
        .then(data => {
            model_select.innerHTML = '<option value="Any">Усі</option>';
            console.log(data);
            data.models.forEach(model => {
                model_select.innerHTML += `<option value=${model}>${model}</option>`;
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
        maxEngineCapacity: Number(engineVolume_max_input.value)
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
                const block = `<div class="car-container">
                                    <div class="car-container-image-wrap">
                                        <img alt="photo" src="${car.photosURL[0]}" />
                                    </div>
                                    <div class="car-container-info">
                                        <a asp-controller="Car" asp-action="Detail" asp-route-id="${car.id}" href="/Car/Detail/${car.id}">${car.brand} ${car.model} ${car.year}</a>
                                    </div>
                                </div>`;
                carList.innerHTML += block;
            });
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
function updateCars() {
    const filters = {
        body: 0,
        brand: "",
        model: "",
        minYear: 0,
        maxYear: 0,
        minPrice: 0,
        maxPrice: 0,
        carTransmission: 0,
        fuel: 0,
        driveLine: 0,
        minEngineCapacity: 0,
        maxEngineCapacity: 0
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
                const block = `<div class="car-container">
                                    <div class="car-container-image-wrap">
                                        <img alt="photo" src="${car.photosURL[0]}" />
                                    </div>
                                    <div class="car-container-info">
                                        <a asp-controller="Car" asp-action="Detail" asp-route-id="${car.id}" href="/Car/Detail/${car.id}">${car.brand} ${car.model} ${car.year}</a>
                                    </div>
                                </div>`;
                carList.innerHTML += block;
            });
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}