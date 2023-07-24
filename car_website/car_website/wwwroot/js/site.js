const apply_button = document.getElementById("refresh_cars");
const brand_select = document.getElementById("brand-select");
const model_select = document.getElementById("model-select");
const body_Type_select = document.getElementById("bodyType_select");
const year_min_select = document.getElementById("year_min-select");
const year_max_select = document.getElementById("year_max-select");

apply_button.onclick = () => applyFilter();
brand_select.addEventListener('change', function () {
    if (brand_select.value == "Any")
        model_select.innerHTML = '<option value="Any">Усі</option>';
    else
        getModelsOfMark();
});


//getModelsOfMark();
updateCars();



//Ajax requests
function getModelsOfMark() {
    fetch(`/home/GetModels?brand=${brand_select.value}`)
        .then(response => response.json())
        .then(data => {
            model_select.innerHTML = '<option value="Any">Усі</option>';
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