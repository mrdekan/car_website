const apply_button = document.getElementById("refresh_cars");



apply_button.onclick = () => applyFilter();



updateCars();



//Ajax requests
function applyFilter() {
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
        .catch(error => console.error("Произошла ошибка при получении данных:", error));
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
        .catch(error => console.error("Произошла ошибка при получении данных:", error));
}