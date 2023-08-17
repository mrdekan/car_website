const wrapper = document.querySelector(".wrapper"),
    selectBtn = wrapper.querySelector(".select-btn"),
    searchInp = document.getElementById("searchCar"),
    options = wrapper.querySelector(".options");
//#region constants
//const root = document.documentElement;
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
const pages_buttons_containers = document.getElementsByClassName("pages_buttons");
let likeButtons = document.getElementsByClassName("like_cars");
let carsPage = 1;


let brands = ["Усі"];
//root.style.setProperty('--primary-color', '#222222');
getMarks();
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
function updatePagesButtons(number) {
    Array.from(pages_buttons_containers).forEach(buttons_container => {
        buttons_container.innerHTML = "";
        if (number > 1) {
            if (number > 8) {
                if (carsPage >= 5 && number - carsPage > 4) {
                    buttons_container.innerHTML += `<button ${1 === carsPage ? 'class="selected"' : ''} page="${1}"">${1}</button>`;
                    buttons_container.innerHTML += `<p class="pages_buttons-space">...</p>`;
                    for (let i = carsPage - 1; i <= (number > carsPage + 4 ? carsPage + 2 : number); i++) {
                        buttons_container.innerHTML += `<button ${i === carsPage ? 'class="selected"' : ''} page="${i}"">${i}</button>`;
                    }
                    if (number > carsPage + 4) {
                        buttons_container.innerHTML += `<p class="pages_buttons-space">...</p>`;
                        buttons_container.innerHTML += `<button ${number === carsPage ? 'class="selected"' : ''} page="${number}"">${number}</button>`;
                    }
                }
                else if (carsPage <= 4) {
                    for (let i = 1; i <= 6; i++) {
                        buttons_container.innerHTML += `<button ${i === carsPage ? 'class="selected"' : ''} page="${i}"">${i}</button>`;
                    }
                    buttons_container.innerHTML += `<p class="pages_buttons-space">...</p>`;
                    buttons_container.innerHTML += `<button ${number === carsPage ? 'class="selected"' : ''} page="${number}"">${number}</button>`;
                }
                else if (number - carsPage <= 4) {
                    buttons_container.innerHTML += `<button ${1 === carsPage ? 'class="selected"' : ''} page="${1}"">${1}</button>`;
                    buttons_container.innerHTML += `<p class="pages_buttons-space">...</p>`;
                    for (let i = number - 5; i <= number; i++) {
                        buttons_container.innerHTML += `<button ${i === carsPage ? 'class="selected"' : ''} page="${i}"">${i}</button>`;
                    }
                }
            }
            else {
                for (let i = 1; i <= number; i++) {
                    buttons_container.innerHTML += `<button ${i === carsPage ? 'class="selected"' : ''} page="${i}"">${i}</button>`;
                }
            }
            Array.from(buttons_container.children).forEach(button => {
                if (button.tagName === 'BUTTON') {
                    button.addEventListener('click', () => {
                        applyFilter(+button.getAttribute("page"));
                    });
                }
            });
        }
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
    selectBtn.firstElementChild.innerText = "Усі";
    driveline_select.value = 0;
    fuel_select.value = 0;
    model_select.value = "Any";
    transmission_select.value = 0;
    year_max_select.value = 0;
    year_min_select.value = 0;
    model_select.innerHTML = '<option value="Any">Усі</option>';
}
clear_filters.addEventListener("click", () => {
    clearFilters();
    applyFilter();
});
//#region input fields settings
engineVolume_min_input.addEventListener('input', function (event) {
    const maxLength = parseInt(event.target.getAttribute('maxlength'));
    let currentValue = event.target.value;
    currentValue = currentValue.replace(/[^\d.,]/g, '');
    if (currentValue.length > maxLength) {
        currentValue = currentValue.slice(0, maxLength);
    }
    event.target.value = currentValue;
});
engineVolume_max_input.addEventListener('input', function (event) {
    const maxLength = parseInt(event.target.getAttribute('maxlength'));
    let currentValue = event.target.value;
    currentValue = currentValue.replace(/[^\d.,]/g, '');
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
/*brand_select.addEventListener('change', function () {
    if (brand_select.value == "Any")
        model_select.innerHTML = '<option value="Any">Усі</option>';
    else {
        getModelsOfMark();
    }
});*/
//#endregion

applyFilter();
if (selectBtn.firstElementChild.innerText != "Усі")
    getModelsOfMark()

//#region Ajax requests
function getModelsOfMark() {
    fetch(`/home/GetModels?brand=${selectBtn.firstElementChild.innerText}`)
        .then(response => response.json())
        .then(data => {
            model_select.innerHTML = '<option value="Any">Усі</option>';
            data.models.forEach(model => {
                if (model != 'Інше')
                    model_select.innerHTML += `<option value=${model.replace(' ', '_')}>${model}</option>`;
            });
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
function getMarks() {
    fetch(`/home/GetBrands`)
        .then(response => response.json())
        .then(data => {
            brands = ["Усі"];
            brands = brands.concat(data.brands);
            brands = brands.filter((n) => { return n != 'Інше' });
            addBrand();
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
function applyFilter(page = 1) {
    const filters = {
        body: Number(body_Type_select.value),
        brand: selectBtn.firstElementChild.innerText,
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
        maxMileage: Number(race_max_input.value),
        page: page,
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
            //console.log(data);
            if (data != null && data.success == true) {
                carsPage = data.page;
                updatePagesButtons(data.pages);
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
                                                <div class="car_container-info-parameters-column">
                                                    <p class="car_container-info-parameters-column-race">${car.mileage} тис. км</p>
                                                    <p class="car_container-info-parameters-column-fuel">${fuelName(car.fuel)}, ${car.engineCapacity} л.</p>

                                                    <p class="car_container-info-parameters-column-vin">${car.vin}</p>
                                                
                                                    </div>
                                                <div class="car_container-info-parameters-column">
                                                    <p class="car_container-info-parameters-column-transmission">${transmissionName(car.carTransmission)}</p>

                                                    <p class="car_container-info-parameters-column-driveline">${drivelineName(car.driveline)}</p>
                                                    
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
            }
            else {
                carList.innerHTML = `<h3 class="warning-text">Щось пішло не так</h3>`;
            }
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

//#region custom checkboxes
//let countries = getMarks().brands;
function addBrand(selectedCountry) {
    options.innerHTML = "";
    brands.forEach(country => {
        let isSelected = country == selectedCountry ? "selected" : "";
        let li = `<li onclick="updateName(this)" class="${isSelected}">${country}</li>`;
        options.insertAdjacentHTML("beforeend", li);
    });
}
addBrand();
function updateName(selectedLi) {
    searchInp.value = "";
    addBrand(selectedLi.innerText);
    wrapper.classList.remove("active");
    selectBtn.firstElementChild.innerText = selectedLi.innerText;
    if (selectBtn.firstElementChild.innerText == "Усі")
        model_select.innerHTML = '<option value="Any">Усі</option>';
    else {
        getModelsOfMark();
    }
}
searchInp.addEventListener("keyup", () => {
    let arr = [];
    let searchWord = searchInp.value.toLowerCase();
    arr = brands.filter(data => {
        return data.toLowerCase().startsWith(searchWord);
    }).map(data => {
        let isSelected = data == selectBtn.firstElementChild.innerText ? "selected" : "";
        return `<li onclick="updateName(this)" class="${isSelected}">${data}</li>`;
    }).join("");
    options.innerHTML = arr ? arr : `<p style="margin-top: 10px;">Oops! Country not found</p>`;
});
selectBtn.addEventListener("click", () => wrapper.classList.toggle("active"));
//#endregion