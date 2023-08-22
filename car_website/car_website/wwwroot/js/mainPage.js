//#region constants
const selectBrandsBtn = document.getElementById("brandsButton"),
    searchBrandInp = document.getElementById("searchCar"),
    brandsOptions = document.getElementById("brands");
const selectModelsBtn = document.getElementById("modelsButton"),
    searchModelInp = document.getElementById("searchModel"),
    modelsOptions = document.getElementById("models");
const selectBodiesBtn = document.getElementById("bodiesButton"),
    bodiesOptions = document.getElementById("bodies");
const selectTransmissionsBtn = document.getElementById("transmissionsButton"),
    transmissionsOptions = document.getElementById("transmissions");
const selectFuelsBtn = document.getElementById("fuelsButton"),
    fuelsOptions = document.getElementById("fuels");
const selectDrivelinesBtn = document.getElementById("drivelinesButton"),
    drivelinesOptions = document.getElementById("drivelines");
const apply_button = document.getElementById("refresh_cars");
const year_min_select = document.getElementById("year_min-select");
const year_max_select = document.getElementById("year_max-select");
const price_max_input = document.getElementById("price_max-input");
const price_min_input = document.getElementById("price_min-input");
const race_max_input = document.getElementById("race_max-input");
const race_min_input = document.getElementById("race_min-input");
const engineVolume_min_input = document.getElementById("engineVolume_min-input");
const engineVolume_max_input = document.getElementById("engineVolume_max-input");
const clear_filters = document.getElementById("clear_filters");
const pages_buttons_containers = document.getElementsByClassName("pages_buttons");
let likeButtons = document.getElementsByClassName("like_cars");
const minYearValue = document.getElementById('min-year-value');
const minYearSlider = document.getElementById('min-year-slider');
const maxYearValue = document.getElementById('max-year-value');
const maxYearSlider = document.getElementById('max-year-slider');
const minMaxYear = document.getElementById('min-max-year');
const maxMinYear = document.getElementById('max-min-year');
const currentYear = minYearSlider.getAttribute('max');
const yearLabel = document.getElementById('year-label');
let carsPage = 1;
//#endregion

//#region Selects content
let brands = ["Усі"];
let models = ["Усі"];
let bodies = ["Усі", "Седан", "Позашляховик", "Мінівен", "Хетчбек", "Універсал", "Купе", "Кабріолет", "Пікап", "Ліфтбек", "Фастбек"];
let transmissions = ["Усі", "Механічна", "Автоматична"];
let fuels = {};
fuels["Усі"] = 0;
fuels["Бензин"] = 3;
fuels["Дизель"] = 4;
fuels["Газ"] = 1;
fuels["Газ/Бензин"] = 2;
fuels["Гібрид"] = 5;
fuels["Електро"] = 6;
let drivelines = ["Усі", "Передній", "Задній", "Повний"];
let modelsCache = {};
//#endregion

//#region Functions' calls
getMarks();
applyFilter();
if (selectBrandsBtn.firstElementChild.innerText != "Усі")
    getModelsOfMark();
addBrand();
addModel();
addBody();
addTransmission();
addFuel();
addDriveline();
//#endregion

//#region Pages & Likes
function updateLikeButtons() {
    likeButtons = document.getElementsByClassName("car_container-right-like-cars");
    Array.from(likeButtons).forEach(like => {
        like.addEventListener('change', function (event) {
            fetch(`/car/like?carId=${event.target.getAttribute('carId')}&isLiked=${like.checked}`)
                .then(response => response.json())
                .then(data => {
                    if (data.success == false) {
                        window.location.href = '/User/Login';
                        like.checked = !like.checked;
                    }
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
    selectBrandsBtn.firstElementChild.innerText = "Усі";
    addBrand();
    selectBodiesBtn.firstElementChild.innerText = "Усі";
    addBody();
    bodiesOptions.scrollTop = 0;
    selectTransmissionsBtn.firstElementChild.innerText = "Усі";
    addTransmission();
    transmissionsOptions.scrollTop = 0;
    selectFuelsBtn.firstElementChild.innerText = "Усі";
    addFuel();
    fuelsOptions.scrollTop = 0;
    selectDrivelinesBtn.firstElementChild.innerText = "Усі";
    addDriveline();
    drivelinesOptions.scrollTop = 0;
    selectModelsBtn.firstElementChild.innerText = "Усі";
    models = ['Усі'];
    addModel();
    minYearSlider.value = 1980;
    minYearSlider.setAttribute('max', maxYearSlider.getAttribute('max'));
    minMaxYear.textContent = maxYearSlider.getAttribute('max');
    maxMinYear.textContent = minYearSlider.getAttribute('min');
    maxYearSlider.value = 2023;
    maxYearSlider.setAttribute('min', minYearSlider.getAttribute('min'));
    yearLabel.textContent = `Рік`;
}
clear_filters.addEventListener("click", () => {
    clearFilters();
    applyFilter();
});
//#endregion

//#region input fields settings
minYearSlider.oninput = (() => {
    var value = minYearSlider.value;
    minYearValue.textContent = value;
    var coef = 46.9 / (minYearSlider.getAttribute('max') - 1980);
    minYearValue.style.left = (value - 1980) * coef + 3 + '%';
    minYearValue.classList.add("show");
    maxYearSlider.setAttribute('min', value);
    maxMinYear.textContent = value;
    if (minYearSlider.value == maxYearSlider.value)
        yearLabel.textContent = `Рік (${minYearSlider.value})`;
    else if (minYearSlider.value == 1980 && maxYearSlider.value != maxYearSlider.getAttribute('max'))
        yearLabel.textContent = `Рік (≤ ${maxYearSlider.value})`;
    else if (minYearSlider.value != 1980 && maxYearSlider.value == maxYearSlider.getAttribute('max'))
        yearLabel.textContent = `Рік (≥ ${minYearSlider.value})`;
    else if (minYearSlider.value == 1980 && maxYearSlider.value == maxYearSlider.getAttribute('max'))
        yearLabel.textContent = `Рік`;
    else
        yearLabel.textContent = `Рік (${minYearSlider.value} — ${maxYearSlider.value})`;
});
maxYearSlider.onblur = (() => {
    maxYearValue.classList.remove("show");
});
maxYearSlider.oninput = (() => {
    var valueMax = maxYearSlider.value;
    maxYearValue.textContent = valueMax;
    var coefMax = 46.9 / (currentYear - maxYearSlider.getAttribute('min'));
    maxYearValue.style.left = (valueMax - maxYearSlider.getAttribute('min')) * coefMax + 3 + '%';
    maxYearValue.classList.add("show");
    minYearSlider.setAttribute('max', valueMax);
    minMaxYear.textContent = valueMax;
    if (minYearSlider.value == maxYearSlider.value)
        yearLabel.textContent = `Рік (${minYearSlider.value})`;
    else if (minYearSlider.value == 1980 && maxYearSlider.value != maxYearSlider.getAttribute('max'))
        yearLabel.textContent = `Рік (≤ ${maxYearSlider.value})`;
    else if (minYearSlider.value != 1980 && maxYearSlider.value == maxYearSlider.getAttribute('max'))
        yearLabel.textContent = `Рік (≥ ${minYearSlider.value})`;
    else if (minYearSlider.value == 1980 && maxYearSlider.value == maxYearSlider.getAttribute('max'))
        yearLabel.textContent = `Рік`;
    else
        yearLabel.textContent = `Рік (${minYearSlider.value} — ${maxYearSlider.value})`;
});
minYearSlider.onblur = (() => {
    minYearValue.classList.remove("show");
});
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
//#endregion

//#region Ajax requests
function getModelsOfMark() {
    var brand = selectBrandsBtn.firstElementChild.innerText;
    if (modelsCache[brand] == null) {
        fetch(`/home/GetModels?brand=${brand}`)
            .then(response => response.json())
            .then(data => {
                models = ["Усі"];
                models = models.concat(data.models);
                models = models.filter((n) => { return n != 'Інше' });
                modelsCache[brand] = models;
                addModel();
            })
            .catch(error => console.error("An error occurred while retrieving data:", error));
    }
    else {
        models = modelsCache[brand];
        addModel();
    }
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
        body: bodies.indexOf(selectBodiesBtn.firstElementChild.innerHTML),
        brand: selectBrandsBtn.firstElementChild.innerText,
        model: selectModelsBtn.firstElementChild.innerText,
        minYear: Number(minYearSlider.value),
        maxYear: Number(maxYearSlider.value),
        minPrice: Number(price_min_input.value),
        maxPrice: Number(price_max_input.value),
        carTransmission: transmissions.indexOf(selectTransmissionsBtn.firstElementChild.innerHTML),
        fuel: fuels[selectFuelsBtn.firstElementChild.innerHTML],
        driveLine: drivelines.indexOf(selectDrivelinesBtn.firstElementChild.innerHTML),
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
            if (data != null && data.success == true && data.cars.length > 0) {
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
            else if (data != null && data.success == true) {
                updatePagesButtons(0);
                carList.innerHTML = `<div class="cars-not-found"><h3 class="warning-text">Нічого не знайдено</h3></div>`;
            }
            else {
                carList.innerHTML = `<div class="cars-not-found"><h3 class="warning-text">Щось пішло не так</h3></div>`;
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

//#region Custom selects
function refreshBrands() {
    if (selectBrandsBtn.firstElementChild.innerText === 'Усі') {
        brandsOptions.innerHTML = `<li onclick="updateModel(this)" class="selected">Усі</li>`;
    }
    else {
        brandsOptions.innerHTML = '';
    }
    brands.forEach(brand => {
        if (brand !== 'Усі' || selectBrandsBtn.firstElementChild.innerText !== 'Усі') {
            let isSelected = brand == selectBrandsBtn.firstElementChild.innerText ? "selected" : "";
            let li = `<li onclick="updateName(this)" class="${isSelected}">${brand}</li>`;
            brandsOptions.insertAdjacentHTML("beforeend", li);
        }
    });
}
function refreshModels() {
    if (selectModelsBtn.firstElementChild.innerText === 'Усі') {
        modelsOptions.innerHTML = `<li onclick="updateModel(this)" class="selected">Усі</li>`;
    }
    else {
        modelsOptions.innerHTML = '';
    }
    models.forEach(model => {
        if (model !== 'Усі' || selectModelsBtn.firstElementChild.innerText !== 'Усі') {
            let isSelected = model == selectModelsBtn.firstElementChild.innerText ? "selected" : "";
            let li = `<li onclick="updateModel(this)" class="${isSelected}">${model}</li>`;
            modelsOptions.insertAdjacentHTML("beforeend", li);
        }
    });
}
searchBrandInp.addEventListener("keyup", () => {
    let arr = [];
    let searchWord = searchBrandInp.value.toLowerCase();
    arr = brands.filter(data => {
        return data.toLowerCase().startsWith(searchWord);
    }).map(data => {
        let isSelected = data == selectBrandsBtn.firstElementChild.innerText ? "selected" : "";
        return `<li onclick="updateName(this)" class="${isSelected}">${data}</li>`;
    }).join("");
    brandsOptions.innerHTML = arr ? arr : `<p style="margin-top: 10px;">Марку не знайдено.</p>`;
});
searchModelInp.addEventListener("keyup", () => {
    let arr = [];
    let searchWord = searchModelInp.value.toLowerCase();
    arr = models.filter(data => {
        return data.toLowerCase().startsWith(searchWord);
    }).map(data => {
        let isSelected = data == selectModelsBtn.firstElementChild.innerText ? "selected" : "";
        return `<li onclick="updateModel(this)" class="${isSelected}">${data}</li>`;
    }).join("");
    modelsOptions.innerHTML = arr ? arr : `<p style="margin-top: 10px;">Модель не знайдено.</p>`;
});
function addBrand(selectedBrand) {
    if (!selectedBrand) {
        selectModelsBtn.firstElementChild.innerText = 'Усі';
        brandsOptions.innerHTML = `<li onclick="updateModel(this)" class="selected">Усі</li>`;
    }
    else {
        brandsOptions.innerHTML = '';
    }
    brands.forEach(brand => {
        if (brand != 'Усі' || selectedBrand) {
            let isSelected = brand == selectedBrand ? "selected" : "";
            let li = `<li onclick="updateName(this)" class="${isSelected}">${brand}</li>`;
            brandsOptions.insertAdjacentHTML("beforeend", li);
        }
    });
}
function addModel(selectedModel) {
    if (!selectedModel) {
        selectModelsBtn.firstElementChild.innerText = 'Усі';
        modelsOptions.innerHTML = `<li onclick="updateModel(this)" class="selected">Усі</li>`;
    }
    else {
        modelsOptions.innerHTML = '';
    }
    models.forEach(model => {
        if (model != 'Усі' || selectedModel) {
            let isSelected = model == selectedModel ? "selected" : "";
            let li = `<li onclick="updateModel(this)" class="${isSelected}">${model}</li>`;
            modelsOptions.insertAdjacentHTML("beforeend", li);
        }
    });
}
function addBody(selectedBody) {
    bodiesOptions.innerHTML = '';
    bodies.forEach(body => {
        let isSelected = body == selectedBody || !selectedBody && body=='Усі' ? "selected" : "";
        let li = `<li onclick="updateBody(this)" class="${isSelected}">${body}</li>`;
        bodiesOptions.insertAdjacentHTML("beforeend", li);
    });
}
function addTransmission(selectedTransmission) {
    transmissionsOptions.innerHTML = '';
    transmissions.forEach(transmission => {
        let isSelected = transmission == selectedTransmission || !selectedTransmission && transmission == 'Усі' ? "selected" : "";
        let li = `<li onclick="updateTransmission(this)" class="${isSelected}">${transmission}</li>`;
        transmissionsOptions.insertAdjacentHTML("beforeend", li);
    });
}
function addFuel(selectedFuel) {
    fuelsOptions.innerHTML = '';
    for (const [key, value] of Object.entries(fuels)) {
        let isSelected = key == selectedFuel || !selectedFuel && key == 'Усі' ? "selected" : "";
        let li = `<li onclick="updateFuel(this)" class="${isSelected}">${key}</li>`;
        fuelsOptions.insertAdjacentHTML("beforeend", li);
    }
}
function addDriveline(selectedDriveline) {
    drivelinesOptions.innerHTML = '';
    drivelines.forEach(driveline => {
        let isSelected = driveline == selectedDriveline || !selectedDriveline && driveline == 'Усі' ? "selected" : "";
        let li = `<li onclick="updateDriveline(this)" class="${isSelected}">${driveline}</li>`;
        drivelinesOptions.insertAdjacentHTML("beforeend", li);
    });
}
function updateName(selectedLi) {
    searchBrandInp.value = "";
    addBrand(selectedLi.innerText);
    brandsOptions.parentElement.classList.remove("active");
    selectBrandsBtn.classList.remove("active");
    selectBrandsBtn.firstElementChild.innerText = selectedLi.innerText;
    if (selectBrandsBtn.firstElementChild.innerText == "Усі")
        modelsOptions.innerHTML = `<li onclick="updateModel(this)" class="selected">Усі</li>`;
    else {
        getModelsOfMark();
    }
}
function updateModel(selectedLi) {
    searchModelInp.value = "";
    addModel(selectedLi.innerText);
    modelsOptions.parentElement.classList.remove("active");
    selectModelsBtn.classList.remove("active");
    selectModelsBtn.firstElementChild.innerText = selectedLi.innerText;
}
function updateBody(selectedLi) {
    addBody(selectedLi.innerText);
    bodiesOptions.parentElement.classList.remove("active");
    selectBodiesBtn.classList.remove("active");
    selectBodiesBtn.firstElementChild.innerText = selectedLi.innerText;
}
function updateTransmission(selectedLi) {
    addTransmission(selectedLi.innerText);
    transmissionsOptions.parentElement.classList.remove("active");
    selectTransmissionsBtn.classList.remove("active");
    selectTransmissionsBtn.firstElementChild.innerText = selectedLi.innerText;
}
function updateFuel(selectedLi) {
    addFuel(selectedLi.innerText);
    fuelsOptions.parentElement.classList.remove("active");
    selectFuelsBtn.classList.remove("active");
    selectFuelsBtn.firstElementChild.innerText = selectedLi.innerText;
}
function updateDriveline(selectedLi) {
    addDriveline(selectedLi.innerText);
    drivelinesOptions.parentElement.classList.remove("active");
    selectDrivelinesBtn.classList.remove("active");
    selectDrivelinesBtn.firstElementChild.innerText = selectedLi.innerText;
}
function hideBrand() {
    searchBrandInp.value = "";
    brandsOptions.parentElement.classList.remove("active");
    selectBrandsBtn.classList.remove("active");
}
function hideModel() {
    searchModelInp.value = "";
    modelsOptions.parentElement.classList.remove("active");
    selectModelsBtn.classList.remove("active");
}
function hideBody() {
    bodiesOptions.parentElement.classList.remove("active");
    selectBodiesBtn.classList.remove("active");
    bodiesOptions.scrollTop = 0;
}
function hideTransmission() {
    transmissionsOptions.parentElement.classList.remove("active");
    selectTransmissionsBtn.classList.remove("active");
    transmissionsOptions.scrollTop = 0;
}
function hideDriveline() {
    drivelinesOptions.parentElement.classList.remove("active");
    selectDrivelinesBtn.classList.remove("active");
    drivelinesOptions.scrollTop = 0;
}
function hideFuel() {
    fuelsOptions.parentElement.classList.remove("active");
    selectFuelsBtn.classList.remove("active");
    fuelsOptions.scrollTop = 0;
}
//#endregion

//#region Click events
selectBrandsBtn.addEventListener("click", () => {
    searchBrandInp.value = "";
    brandsOptions.parentElement.classList.toggle("active");
    selectBrandsBtn.classList.toggle("active");
    hideModel();
    hideBody();
    hideTransmission();
    hideFuel();
    hideDriveline();
});
selectModelsBtn.addEventListener("click", () => {
    searchModelInp.value = "";
    modelsOptions.parentElement.classList.toggle("active");
    selectModelsBtn.classList.toggle("active");
    hideBrand();
    hideBody();
    hideTransmission();
    hideFuel();
    hideDriveline();
});
selectBodiesBtn.addEventListener("click", () => {
    bodiesOptions.parentElement.classList.toggle("active");
    selectBodiesBtn.classList.toggle("active");
    bodiesOptions.scrollTop = 0;
    hideBrand();
    hideModel();
    hideTransmission();
    hideFuel();
    hideDriveline();
});
selectTransmissionsBtn.addEventListener("click", () => {
    transmissionsOptions.parentElement.classList.toggle("active");
    selectTransmissionsBtn.classList.toggle("active");
    transmissionsOptions.scrollTop = 0;
    hideBody();
    hideBrand();
    hideModel();
    hideFuel();
    hideDriveline();
});
selectFuelsBtn.addEventListener("click", () => {
    fuelsOptions.parentElement.classList.toggle("active");
    selectFuelsBtn.classList.toggle("active");
    fuelsOptions.scrollTop = 0;
    hideBody();
    hideBrand();
    hideModel();
    hideTransmission();
    hideDriveline();
});
selectDrivelinesBtn.addEventListener("click", () => {
    drivelinesOptions.parentElement.classList.toggle("active");
    selectDrivelinesBtn.classList.toggle("active");
    drivelinesOptions.scrollTop = 0;
    hideBody();
    hideBrand();
    hideModel();
    hideTransmission();
    hideFuel();
});
document.addEventListener('click', function (event) {
    if (!selectBrandsBtn.parentElement.contains(event.target)) {
        hideBrand();
        refreshBrands();
    }
    if (!selectModelsBtn.parentElement.contains(event.target)) {
        hideModel();
        refreshModels();
    }
    if (!selectBodiesBtn.parentElement.contains(event.target)) {
        hideBody();
    }
    if (!selectTransmissionsBtn.parentElement.contains(event.target)) {
        hideTransmission();
    }
    if (!selectFuelsBtn.parentElement.contains(event.target)) {
        hideFuel();
    }
    if (!selectDrivelinesBtn.parentElement.contains(event.target)) {
        hideDriveline();
    }
});
//#endregion