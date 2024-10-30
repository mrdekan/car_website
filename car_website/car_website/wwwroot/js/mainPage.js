import { fuelName, transmissionName, drivelineName, formatNumberWithThousandsSeparator } from './modules/formCar.js';
import svgCodes from './modules/svgCodesConst.js';
//#region constants
const selectBrandsBtn = document.getElementById("brandsButton"),
    searchBrandInp = document.getElementById("searchCar"),
    brandsOptions = document.getElementById("brands"),
    selectModelsBtn = document.getElementById("modelsButton"),
    searchModelInp = document.getElementById("searchModel"),
    modelsOptions = document.getElementById("models"),
    selectBodiesBtn = document.getElementById("bodiesButton"),
    bodiesOptions = document.getElementById("bodies"),
    selectTransmissionsBtn = document.getElementById("transmissionsButton"),
    transmissionsOptions = document.getElementById("transmissions"),
    selectFuelsBtn = document.getElementById("fuelsButton"),
    fuelsOptions = document.getElementById("fuels"),
    selectDrivelinesBtn = document.getElementById("drivelinesButton"),
    drivelinesOptions = document.getElementById("drivelines"),
    selectSortingBtn = document.getElementById("sortingButton"),
    sortingOptions = document.getElementById("sortings"),
    apply_button = document.getElementById("refresh_cars"),
    year_min_select = document.getElementById("year_min-select"),
    year_max_select = document.getElementById("year_max-select"),
    price_max_input = document.getElementById("price_max-input"),
    price_min_input = document.getElementById("price_min-input"),
    race_max_input = document.getElementById("race_max-input"),
    race_min_input = document.getElementById("race_min-input"),
    engineVolume_min_input = document.getElementById("engineVolume_min-input"),
    engineVolume_max_input = document.getElementById("engineVolume_max-input"),
    clear_filters = document.getElementById("clear_filters"),
    pages_buttons_containers = document.getElementsByClassName("pages_buttons"),
    minYearValue = document.getElementById('min-year-value'),
    minYearSlider = document.getElementById('min-year-slider'),
    maxYearValue = document.getElementById('max-year-value'),
    maxYearSlider = document.getElementById('max-year-slider'),
    minMaxYear = document.getElementById('min-max-year'),
    maxMinYear = document.getElementById('max-min-year'),
    currentYear = minYearSlider.getAttribute('max'),
    yearLabel = document.getElementById('year-label'),
    openFilter = document.getElementById('open-filters'),
    filter = document.querySelector('.filters_wrapper'),
    engineVolumeLbl = document.getElementById('engine-volume-lbl'),
    mainCont = document.getElementById('main-cont');
//#endregion
const isAuthStr = mainCont.getAttribute('auth');
const isAuth = isAuthStr.toLowerCase() === 'true';
//#region Selects content
let carsPage = 1,
likeButtons = document.getElementsByClassName("like_cars"),
brands = ["Усі"], models = ["Усі"],
bodies = ["Усі", "Седан", "Позашляховик", "Мінівен", "Хетчбек", "Універсал", "Купе", "Кабріолет", "Пікап", "Ліфтбек", "Автобус"],
transmissions = ["Усі", "Механічна", "Автоматична"],
sortings = ["За датою додавання", "За спаданням ціни", "За зростанням ціни"],
fuels = {};
fuels["Усі"] = 0;
fuels["Бензин"] = 3;
fuels["Дизель"] = 4;
fuels["Газ"] = 1;
fuels["Газ/Бензин"] = 2;
fuels["Гібрид"] = 5;
fuels["Електро"] = 6;
let drivelines = ["Усі", "Передній", "Задній", "Повний"];
let modelsCache = {};
let carsCache = {};
let pages = 0;
let isElectro = false;
const filtersCont = document.querySelector('.filters_wrapper');
const updateFiltersSize = () => {
    let height = document.body.scrollHeight - window.innerHeight;
    if (height > 0 && filtersCont)
        filtersCont.style.maxHeight = height - window.scrollY > 90 ? 'calc(100vh - 85px)' : `calc(100vh - ${195 - (height - window.scrollY)}px)`;
}
window.addEventListener('scroll', updateFiltersSize);
window.addEventListener('resize', updateFiltersSize);


//#region Functions' calls
function createLi(text) {
    let li = document.createElement('LI');
    li.innerText = text;
    return li;
}
function getKeyByValue(object, value) {
    for (const key in object)
        if (object[key] === value)
            return key;
    return null;
}
document.addEventListener('DOMContentLoaded', function () {
    let lastCarsString = sessionStorage.getItem("last");
    let lastCars = JSON.parse(lastCarsString);
    if (lastCars && lastCars.data && false) {
        carsPage = lastCars.data.page;
        updatePagesButtons(lastCars.data.pages);
        pages = lastCars.data.pages;
        carList.innerHTML = "";
        lastCars.data.cars.forEach(car =>carList.innerHTML += formCar(car));
        if (lastCars.data.cars == null || lastCars.data.cars.length == 0) {
            updatePagesButtons(0);
            carList.innerHTML = `<div class="cars-not-found"><h3 class="warning-text">Нічого не знайдено</h3></div>`;
        }
        carsCache[lastCars.filters] = lastCars.data;
        updateLikeButtons();
        if (lastCars.filters.driveLine != 0)
            updateDriveline(createLi(drivelines[lastCars.filters.driveLine]));
        if (lastCars.filters.body != 0)
            updateBody(createLi(bodies[lastCars.filters.body]));
        if (lastCars.filters.carTransmission != 0)
            updateTransmission(createLi(transmissions[lastCars.filters.carTransmission]));
        if (lastCars.filters.fuel != 0)
            updateFuel(createLi(getKeyByValue(fuels, lastCars.filters.fuel)));
        if (lastCars.filters.maxEngineCapacity != 0)
            engineVolume_max_input.value = lastCars.filters.maxEngineCapacity;
        if (lastCars.filters.minEngineCapacity != 0)
            engineVolume_min_input.value = lastCars.filters.minEngineCapacity;
        if (lastCars.filters.minPrice != 0)
            price_min_input.value = lastCars.filters.minPrice;
        if (lastCars.filters.maxPrice != 0)
            price_max_input.value = lastCars.filters.maxPrice;
        if (lastCars.filters.minMileage != 0)
            race_min_input.value = lastCars.filters.minMileage;
        if (lastCars.filters.maxMileage != 0)
            race_max_input.value = lastCars.filters.maxMileage;
        if (lastCars.filters.minYear != 2000) {
            minYearSlider.value = lastCars.filters.minYear;
            maxYearSlider.setAttribute('min', lastCars.filters.minYear);
            updateYearLabel();
        }
        if (lastCars.filters.maxYear != maxYearSlider.getAttribute('max')) {
            maxYearSlider.value = lastCars.filters.maxYear;
            minYearSlider.setAttribute('max', lastCars.filters.maxYear);
            updateYearLabel();
        }
        if (lastCars.filters.sortingType != 0) {
            let selectedLi = createLi(sortings[lastCars.filters.sortingType]);
            addSorting(selectedLi.innerText);
            sortingOptions.parentElement.classList.remove("active");
            selectSortingBtn.classList.remove("active");
            selectSortingBtn.firstElementChild.innerText = selectedLi.innerText;
        }
        //getMarks();
        if (lastCars.filters.brand != "Усі") {
            updateName(createLi(lastCars.filters.brand));
            if (lastCars.filters.model != "Усі")
                getModelsOfMark(lastCars.filters.model);
            else
                getModelsOfMark();
        }
        isFilterClear();
    }
    else
        applyFilter();
});
getMarks();
if (selectBrandsBtn.firstElementChild.innerText != "Усі")
    getModelsOfMark();
addBrand();
addModel();
addBody();
addTransmission();
addFuel();
addDriveline();
addSorting();
isFilterClear();
//#endregion
//#region Pages & Likes
openFilter.addEventListener('click', () => filter.classList.toggle("open"));
function updateLikeButtons() {
    likeButtons = document.getElementsByClassName("car_container-right-like-cars");
    Array.from(likeButtons).forEach(like => {
        like.addEventListener('change', function (event) {
            fetch(`/car/like?carId=${event.target.getAttribute('carId')}&isLiked=${like.checked}`)
                .then(response => response.json())
                .then(data => {
                    if (data.success == false) {
                        like.checked = !like.checked;
                        window.location.href = '/User/Login';
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
                    for (let i = carsPage - 1; i <= (number > carsPage + 4 ? carsPage + 2 : number); i++)
                        buttons_container.innerHTML += `<button ${i === carsPage ? 'class="selected"' : ''} page="${i}"">${i}</button>`;
                    if (number > carsPage + 4) {
                        buttons_container.innerHTML += `<p class="pages_buttons-space">...</p>`;
                        buttons_container.innerHTML += `<button ${number === carsPage ? 'class="selected"' : ''} page="${number}"">${number}</button>`;
                    }
                }
                else if (carsPage <= 4) {
                    for (let i = 1; i <= 6; i++)
                        buttons_container.innerHTML += `<button ${i === carsPage ? 'class="selected"' : ''} page="${i}"">${i}</button>`;
                    buttons_container.innerHTML += `<p class="pages_buttons-space">...</p>`;
                    buttons_container.innerHTML += `<button ${number === carsPage ? 'class="selected"' : ''} page="${number}"">${number}</button>`;
                }
                else if (number - carsPage <= 4) {
                    buttons_container.innerHTML += `<button ${1 === carsPage ? 'class="selected"' : ''} page="${1}"">${1}</button>`;
                    buttons_container.innerHTML += `<p class="pages_buttons-space">...</p>`;
                    for (let i = number - 5; i <= number; i++)
                        buttons_container.innerHTML += `<button ${i === carsPage ? 'class="selected"' : ''} page="${i}"">${i}</button>`;
                }
            }
            else {
                for (let i = 1; i <= number; i++)
                    buttons_container.innerHTML += `<button ${i === carsPage ? 'class="selected"' : ''} page="${i}"">${i}</button>`;
            }
            Array.from(buttons_container.children).forEach(button => {
                if (button.tagName === 'BUTTON')
                    button.addEventListener('click', () => applyFilter(+button.getAttribute("page")));
            });
        }
    });
}
function isFilterClear() {
    let res = true;
    if (engineVolume_max_input.value
        || engineVolume_min_input.value > 0
        || price_max_input.value > 0
        || price_min_input.value > 0
        || minYearSlider.value != 2000
        || maxYearSlider.value != 2023
        || selectBrandsBtn.firstElementChild.innerText != "Усі"
        || selectBodiesBtn.firstElementChild.innerText != "Усі"
        || selectTransmissionsBtn.firstElementChild.innerText != "Усі"
        || selectFuelsBtn.firstElementChild.innerText != "Усі"
        || selectDrivelinesBtn.firstElementChild.innerText != "Усі"
        || selectModelsBtn.firstElementChild.innerText != "Усі"
    )
        res = false;
    if (!res) openFilter.setAttribute('filtered', '');
    else openFilter.removeAttribute('filtered');
}
function clearFilters() {
    engineVolume_max_input.value = "";
    engineVolume_min_input.value = "";
    price_max_input.value = "";
    price_min_input.value = "";
    selectBrandsBtn.firstElementChild.innerText = "Усі";
    addBrand();
    selectSortingBtn.firstElementChild.innerText = "Сортування";
    addSorting();
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
    minYearSlider.value = 2000;
    minYearSlider.setAttribute('max', maxYearSlider.getAttribute('max'));
    minMaxYear.textContent = maxYearSlider.getAttribute('max');
    maxMinYear.textContent = minYearSlider.getAttribute('min');
    maxYearSlider.value = 2023;
    maxYearSlider.setAttribute('min', minYearSlider.getAttribute('min'));
    yearLabel.textContent = `Рік`;
    applyFilter();
    isFilterClear()
}
clear_filters.onclick = () => {
    window.scrollTo({ top: 0, behavior: 'smooth' });
    clearFilters();
}
//#endregion

//#region input fields settings
minYearSlider.oninput = (() => {
    var value = minYearSlider.value;
    minYearValue.textContent = value;
    var coef = 46.9 / (minYearSlider.getAttribute('max') - 2000);
    minYearValue.style.left = (value - 2000) * coef + 3 + '%';
    minYearValue.classList.add("show");
    maxYearSlider.setAttribute('min', value);
    maxMinYear.textContent = value;
    updateYearLabel();
});
maxYearSlider.onblur = (() => maxYearValue.classList.remove("show"));
maxYearSlider.oninput = (() => {
    var valueMax = maxYearSlider.value;
    maxYearValue.textContent = valueMax;
    var coefMax = 46.9 / (currentYear - maxYearSlider.getAttribute('min'));
    maxYearValue.style.left = (valueMax - maxYearSlider.getAttribute('min')) * coefMax + 3 + '%';
    maxYearValue.classList.add("show");
    minYearSlider.setAttribute('max', valueMax);
    minMaxYear.textContent = valueMax;
    updateYearLabel();
});
function updateYearLabel() {
    if (minYearSlider.value == maxYearSlider.value)
        yearLabel.textContent = `Рік (${minYearSlider.value})`;
    else if (minYearSlider.value == 2000 && maxYearSlider.value != maxYearSlider.getAttribute('max'))
        yearLabel.textContent = `Рік (≤ ${maxYearSlider.value})`;
    else if (minYearSlider.value != 2000 && maxYearSlider.value == maxYearSlider.getAttribute('max'))
        yearLabel.textContent = `Рік (≥ ${minYearSlider.value})`;
    else if (minYearSlider.value == 2000 && maxYearSlider.value == maxYearSlider.getAttribute('max'))
        yearLabel.textContent = `Рік`;
    else
        yearLabel.textContent = `Рік (${minYearSlider.value} — ${maxYearSlider.value})`;
}
minYearSlider.onblur = (() => minYearValue.classList.remove("show"));
const inputsWithComma = [engineVolume_min_input, engineVolume_max_input];
inputsWithComma.forEach((inp) => {
    inp.addEventListener('input', function (event) {
        let value = event.target.value.replace(/[^\d.,]/g, '');;
        value = value.replace(',', '.');
        let length = 2;
        if (isElectro) length++;
        if (value.includes('.')) length++;
        value = value.slice(0, length);
        event.target.value = value;
    });
    inp.addEventListener('keydown', (e) => {
        if ((e.target.value.includes('.') || e.target.value == '' || e.target.value.length >= (isElectro ? 3 : 2)) && (e.key == '.' || e.key == ','))
            e.preventDefault();
    });
});
const inputs = [price_max_input, price_min_input];
inputs.forEach((inp) => {
    inp.addEventListener('input', function (event) {
        const maxLength = +event.target.getAttribute('maxlength');
        let currentValue = event.target.value;
        currentValue = currentValue.replace(/[^\d]/g, '');
        if (currentValue.length > maxLength)
            currentValue = currentValue.slice(0, maxLength);
        event.target.value = currentValue;
    });
})
apply_button.onclick = () => {
    window.scrollTo({ top: 0, behavior: 'smooth' });
    applyFilter();
}
//#endregion

//#region Ajax requests
function getModelsOfMark(applyModel = "") {
    var brand = selectBrandsBtn.firstElementChild.innerText;
    if (modelsCache[brand] == null) {
        fetch(`/api/v1/brands/getModels?brand=${brand}`)
            .then(response => response.json())
            .then(data => {
                models = ["Усі"];
                models = models.concat(data.models);
                models = models.filter((n) => { return n != 'Інше' });
                modelsCache[brand] = models;
                addModel();
                if (applyModel != "")
                    updateModel(createLi(applyModel));
            })
            .catch(error => console.error("An error occurred while retrieving data:", error));
    }
    else {
        models = modelsCache[brand];
        addModel();
    }
}
function getMarks() {
    fetch(`/api/v1/brands/getAll`)
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
    isFilterClear();
    filter.classList.remove("open");
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
        minMileage: 0,
        maxMileage: 0,
        page: page,
        sortingType: sortings.includes(selectSortingBtn.innerText) ? sortings.indexOf(selectSortingBtn.innerText) : 0,
    };
    const carList = document.getElementById("carList");
    carsPage = page;
    let filtersString = JSON.stringify(filters);
    updatePagesButtons(pages);
    carList.innerHTML = `<div class="cars-not-found" style="height: ${carList.clientHeight}px;"><div class="custom-loader"></div><h3 class="warning-text">Завантаження...</h3></div>`;
    if (!carsCache[filtersString] || carsCache[filtersString].status == false) {
        fetch(`/api/v1/cars/getFiltered`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: filtersString
        })
            .then(response => response.json())
            .then(data => {
                setCarsData(data, filters, filtersString);
            })
            .catch(error => console.error("An error occurred while retrieving data:", error));
    }
    else
        setCarsData(carsCache[filtersString], filters, filtersString);
}
function setCarsData(data, filters, filtersString) {
    sessionStorage.setItem("last", JSON.stringify({ filters, data }));
    if (data != null && data.status == true && data.cars.length > 0) {
        carsPage = data.page;
        updatePagesButtons(data.pages);
        pages = data.pages;
        carList.innerHTML = "";
        data.cars.forEach(car => {
            const block = formCar(car);
            carList.innerHTML += block;
        });
        carsCache[filtersString] = data;
        updateLikeButtons();
    }
    else if (data != null && data.status == true) {
        updatePagesButtons(0);
        carList.innerHTML = `<div class="cars-not-found"><h3 class="warning-text">Нічого не знайдено</h3></div>`;
    }
    else {
        updatePagesButtons(0);
        carList.innerHTML = `<div class="cars-not-found"><h3 class="warning-text">Щось пішло не так</h3></div>`;
    }
    window.scrollTo({ top: 0, behavior: 'smooth' });
}
//#endregion
function formCar(car) {
    return `
                                  <a class="car mainPageCar" href="/Car/Detail/${car.id}">
                                  <p class="car_name">${car.brand} ${car.model} ${car.year}</p>
                                  <div class="car_container">
                                       <div class="car_container-img">${car.isSold ? '<div class="car_container-img-sold"></div>' : ''} <div class="car_container-img-landscape"><img alt="${car.brand} ${car.model} ${car.year}" src="https:\\\\1auto.cn.ua${car.previewURL}" /></div></div>
                                    <div class="car_container-info">
                                    <p class="car_container-info-name">${car.brand} ${car.model} ${car.year}</p>
                                            <div class="car_container-info-parameters">
                                                <div class="car_container-info-parameters-column">
                                                    <p class="car_container-info-parameters-column-text"><span>${svgCodes.race}</span>${car.mileage} тис. км</p>
                                                    <p class="car_container-info-parameters-column-text"><span>${svgCodes.fuel}</span>${fuelName(car.fuel)}, ${car.engineCapacity} ${car.fuel == 6 ? "кВт·год." : "л."}</p>
                                                    ${car.vin == null ? `` : `<p class="car_container-info-parameters-column-text vin"><span>${svgCodes.car}</span>${car.vin}</p>`}
                                                    </div>
                                                <div class="car_container-info-parameters-column">
                                                    <p class="car_container-info-parameters-column-text"><span>${svgCodes.transmission}</span>${transmissionName(car.carTransmission)}</p>
                                                    <p class="car_container-info-parameters-column-text"><span>${svgCodes.driveline}</span>${drivelineName(car.driveline)}</p>
                                                    </div>
                                            </div>
                                            ${car.arriveMessage?`<p class="car_container-info-arrive"> <span>${svgCodes.time}</span>${ car.arriveMessage }</p>`:''}
                                        </div>
                                    </div>
                                    <div class="car_container-right">
                                    <div class="car_container-right-price">
                                                <p class="car_container-right-price-USD">${formatNumberWithThousandsSeparator(car.price)} $</p>
                                                <p class="car_container-right-price-UAH">≈ ${formatNumberWithThousandsSeparator(car.priceUAH)} грн</p>
                                            </div>
                                            ${car.priority > 0 && !car.isSold ? '<span class="car_container-right-top">Топ</span>' : ''}
                                  ${!car.arriveMessage ? `<div class="car_container-right-like">
                                  ${isAuth?`<input type = "checkbox" class="car_container-right-like-cars" carId = "${car.id}" ${ car.liked ? "checked" : "" } />
                                  <span class="car_container-right-heart">
                                    <svg width="40" height="40" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg"><path fill-rule="evenodd" clip-rule="evenodd" d="M8.272 4.95258C7.174 4.95258 6.077 5.34958 5.241 6.14458C4.441 6.90658 4 7.91758 4 8.99158C4 10.0646 4.441 11.0756 5.241 11.8376L12 18.2696L18.759 11.8376C19.559 11.0756 20 10.0646 20 8.99158C20 7.91858 19.559 6.90658 18.759 6.14458C17.088 4.55458 14.368 4.55458 12.697 6.14458L12 6.80858L11.303 6.14458C10.467 5.34958 9.37 4.95258 8.272 4.95258ZM12 20.9996L3.847 13.2406C2.656 12.1076 2 10.5986 2 8.99158C2 7.38458 2.656 5.87558 3.847 4.74158C6.067 2.62858 9.552 2.43858 12 4.16858C14.448 2.43858 17.933 2.62858 20.153 4.74158C21.344 5.87558 22 7.38458 22 8.99158C22 10.5986 21.344 12.1076 20.153 13.2406L12 20.9996Z" fill="currentColor"/></svg>
                                  </span>
                                  <span class="car_container-right-span"><svg width="40" height="40" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                                    <path fill-rule="evenodd" clip-rule="evenodd" d="M12 20.9996L3.847 13.2406C2.656 12.1076 2 10.5986 2 8.99158C2 7.38458 2.656 5.87558 3.847 4.74158C6.067 2.62858 9.552 2.43858 12 4.16858C14.448 2.43858 17.933 2.62858 20.153 4.74158C21.344 5.87558 22 7.38458 22 8.99158C22 10.5986 21.344 12.1076 20.153 13.2406L12 20.9996Z" fill="currentColor"/></svg>
                                  </span>`: ''}
                                  </div>`: `<p class="car_container-info-arrive-mobile"><span>${svgCodes.time}</span>${car.arriveMessage}</p>`}
                                  </div>
                                  </a>`;
}

//#region Custom selects
function refreshBrands() {
    brandsOptions.innerHTML = selectBrandsBtn.firstElementChild.innerText === 'Усі'?`<li onclick="updateModel(this)" class="selected">Усі</li>`:'';
    brands.forEach(brand => {
        if (brand !== 'Усі' || selectBrandsBtn.firstElementChild.innerText !== 'Усі') {
            let isSelected = brand == selectBrandsBtn.firstElementChild.innerText ? "selected" : "";
            let li = `<li onclick="updateName(this)" class="${isSelected}">${brand}</li>`;
            brandsOptions.insertAdjacentHTML("beforeend", li);
        }
    });
}
function refreshModels() {
    modelsOptions.innerHTML = selectModelsBtn.firstElementChild.innerText === 'Усі'?`<li onclick="updateModel(this)" class="selected">Усі</li>`:'';
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
    else
        brandsOptions.innerHTML = '';
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
    else
        modelsOptions.innerHTML = '';
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
        let isSelected = body == selectedBody || !selectedBody && body == 'Усі' ? "selected" : "";
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
function addSorting(selectedSorting) {
    sortingOptions.innerHTML = '';
    sortings.forEach(sorting => {
        let isSelected = sorting == selectedSorting || !selectedSorting && sorting == 'За датою додавання' ? "selected" : "";
        let li = `<li onclick="updateSorting(this)" class="${isSelected}">${sorting}</li>`;
        sortingOptions.insertAdjacentHTML("beforeend", li);
    });
}
function updateName(selectedLi) {
    searchBrandInp.value = "";
    addBrand(selectedLi.innerText);
    brandsOptions.parentElement.classList.remove("active");
    selectBrandsBtn.classList.remove("active");
    selectBrandsBtn.firstElementChild.innerText = selectedLi.innerText;
    getModelsOfMark();
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
    if (selectedLi.innerText == "Електро") {
        engineVolumeLbl.innerHTML = 'Ємність батареї (кВт·год.)';
        isElectro = true;
    }
    else {
        engineVolumeLbl.innerHTML = "Об'єм двигуна (л.)";
        isElectro = false;
        engineVolume_max_input.value = engineVolume_max_input.value.includes('.') ? engineVolume_max_input.value.slice(0, 3) : engineVolume_max_input.value.slice(0, 2);
        engineVolume_min_input.value = engineVolume_min_input.value.includes('.') ? engineVolume_min_input.value.slice(0, 3) : engineVolume_min_input.value.slice(0, 2);
    }
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
function updateSorting(selectedLi) {
    addSorting(selectedLi.innerText);
    sortingOptions.parentElement.classList.remove("active");
    selectSortingBtn.classList.remove("active");
    selectSortingBtn.firstElementChild.innerText = selectedLi.innerText;
    applyFilter();
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
function hideSorting() {
    sortingOptions.parentElement.classList.remove("active");
    selectSortingBtn.classList.remove("active");
    sortingOptions.scrollTop = 0;
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
});
selectModelsBtn.addEventListener("click", () => {
    searchModelInp.value = "";
    modelsOptions.parentElement.classList.toggle("active");
    selectModelsBtn.classList.toggle("active");
});
selectBodiesBtn.addEventListener("click", () => {
    bodiesOptions.parentElement.classList.toggle("active");
    selectBodiesBtn.classList.toggle("active");
    bodiesOptions.scrollTop = 0;
});
selectTransmissionsBtn.addEventListener("click", () => {
    transmissionsOptions.parentElement.classList.toggle("active");
    selectTransmissionsBtn.classList.toggle("active");
    transmissionsOptions.scrollTop = 0;
});
selectFuelsBtn.addEventListener("click", () => {
    fuelsOptions.parentElement.classList.toggle("active");
    selectFuelsBtn.classList.toggle("active");
    fuelsOptions.scrollTop = 0;
});
selectDrivelinesBtn.addEventListener("click", () => {
    drivelinesOptions.parentElement.classList.toggle("active");
    selectDrivelinesBtn.classList.toggle("active");
    drivelinesOptions.scrollTop = 0;
});
selectSortingBtn.addEventListener("click", () => {
    sortingOptions.parentElement.classList.toggle("active");
    selectSortingBtn.classList.toggle("active");
    sortingOptions.scrollTop = 0;
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
    if (!selectBodiesBtn.parentElement.contains(event.target))
        hideBody();
    if (!selectTransmissionsBtn.parentElement.contains(event.target))
        hideTransmission();
    if (!selectFuelsBtn.parentElement.contains(event.target))
        hideFuel();
    if (!selectDrivelinesBtn.parentElement.contains(event.target))
        hideDriveline();
    if (!selectSortingBtn.parentElement.contains(event.target))
        hideSorting();
});
//#endregion