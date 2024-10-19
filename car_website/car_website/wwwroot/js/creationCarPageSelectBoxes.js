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
    selectColorsBtn = document.getElementById("colorsButton"),
    colorsOptions = document.getElementById("colors"),
    engineVolumeInp = document.getElementById('engine-volume'),
    mileage = document.getElementById('mileage'),
    vin = document.getElementById('vin'),
    year = document.getElementById('year'),
    price = document.getElementById('price'),
    engineVolumeLbl = document.getElementById('engine-volume-label'),
    colorRealInp = document.getElementById('real-color-inp'),
    drivelineRealInp = document.getElementById('real-driveline-inp'),
    fuelRealInp = document.getElementById('real-fuel-inp'),
    bodyRealInp = document.getElementById('real-body-inp'),
    transmissionRealInp = document.getElementById('real-transmission-inp'),
    brandRealInp = document.getElementById('real-brand-inp'),
    modelRealInp = document.getElementById('real-model-inp'),
    otherModelInp = document.getElementById('other-model-inp'),
    otherBrandInp = document.getElementById('other-brand-inp');
let brands = ["Не обрано"];
let models = ["Не обрано"];
let bodies = ["Не обрано", "Седан", "Позашляховик", "Мінівен", "Хетчбек", "Універсал", "Купе", "Кабріолет", "Пікап", "Ліфтбек", "Автобус"];
let colors = ["Не обрано", "Бежевий", "Чорний", "Синій", "Коричневий", "Зелений", "Сірий", "Помаранчевий", "Фіолетовий", "Червоний", "Білий", "Жовтий"];
let transmissions = ["Не обрано", "Механічна", "Автоматична"];
let fuels = {};
fuels["Не обрано"] = 0;
fuels["Бензин"] = 3;
fuels["Дизель"] = 4;
fuels["Газ"] = 1;
fuels["Газ/Бензин"] = 2;
fuels["Гібрид"] = 5;
fuels["Електро"] = 6;
let drivelines = ["Не обрано", "Передній", "Задній", "Повний"];
let modelsCache = {};
let isElectro = false;
if (selectBrandsBtn) {
    getMarks();
    if (selectBrandsBtn.firstElementChild.innerText != "Не обрано")
        getModelsOfMark();
    addBrand();
    addModel();
}
addBody();
addTransmission();
addFuel();
addDriveline();
addColor();
if (drivelineRealInp.value != 'Any')
    updateDriveline(createLi(drivelines[drivelineRealInp.value]));
if (bodyRealInp.value != 'Any')
    updateBody(createLi(bodies[bodyRealInp.value]));
if (transmissionRealInp.value != 'Any')
    updateTransmission(createLi(transmissions[transmissionRealInp.value]));
if (colorRealInp.value != 'Any')
    updateColor(createLi(colors[+colorRealInp.value + 1]));
if (fuelRealInp.value != 'Any')
    updateFuel(createLi(getKeyByValue(fuels, + fuelRealInp.value)));
let inps = [mileage, vin, year, price];
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
inps.forEach(inp => {
    inp.addEventListener('input', function (event) {
        const maxLength = +event.target.getAttribute('maxlength');
        let currentValue = event.target.value;
        if (currentValue.length > maxLength)
            currentValue = currentValue.slice(0, maxLength);
        event.target.value = currentValue;
    });
});

engineVolumeInp.addEventListener('input', function (event) {
    let value = event.target.value.replace(/[^\d.,]/g, '');;
    value = value.replace(',', '.');
    let length = 2;
    if (isElectro) length++;
    if (value.includes('.'))
        length++;
    value = value.slice(0, length);
    event.target.value = value;
});
engineVolumeInp.addEventListener('keydown', (e) => {
    if ((e.target.value.includes('.') || e.target.value == '' || e.target.value.length >= (isElectro ? 3 : 2)) && (e.key == '.' || e.key == ','))
        e.preventDefault();
});
if (otherModelInp)
otherModelInp.addEventListener('input', function () {
    if (otherModelInp.value.length > 30)
        otherModelInp.value = otherModelInp.value.slice(0, 30);
    modelRealInp.value = otherModelInp.value;
});
if (otherBrandInp)
otherBrandInp.addEventListener('input', function () {
    if (otherBrandInp.value.length > 30)
        otherBrandInp.value = otherBrandInp.value.slice(0, 30);
    brandRealInp.value = otherBrandInp.value;
});
function getModelsOfMark() {
    var brand = selectBrandsBtn.firstElementChild.innerText;
    if (modelsCache[brand] == null) {
        fetch(`/api/v1/brands/getModels?brand=${brand}`)
            .then(response => response.json())
            .then(data => {
                models = ["Не обрано"];
                models = models.concat(data.models);
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
    fetch(`/api/v1/brands/getAll`)
        .then(response => response.json())
        .then(data => {
            let prevBrand = brandRealInp.value;
            let prevModel = modelRealInp.value;
            brands = ["Не обрано"];
            brands = brands.concat(data.brands);
            addBrand();
            if (prevBrand.length > 0) {
                if (brands.includes(prevBrand)) {
                    updateName(createLi(prevBrand));
                    fetch(`/api/v1/brands/getModels?brand=${prevBrand}`)
                        .then(response => response.json())
                        .then(data => {
                            models = ["Не обрано"];
                            models = models.concat(data.models);
                            modelsCache[prevBrand] = models;
                            addModel();
                            if (prevModel.length > 0) {
                                if (models.includes(prevModel))
                                    updateModel(createLi(prevModel));
                                else {
                                    updateModel(createLi("Інше"));
                                    otherModelInp.value = prevModel;
                                    modelRealInp.value = prevModel;
                                }
                            }
                        })
                        .catch(error => console.error("An error occurred while retrieving data:", error));
                }
                else {
                    updateName(createLi("Інше"));
                    otherBrandInp.value = prevBrand;
                    brandRealInp.value = prevBrand;
                    if (prevModel.length > 0) {
                        updateModel(createLi("Інше"));
                        otherModelInp.value = prevModel;
                        modelRealInp.value = prevModel;
                    }
                }
            }
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
//#region Custom selects
function refreshBrands() {
    if (selectBrandsBtn.firstElementChild.innerText === 'Не обрано')
        brandsOptions.innerHTML = `<li onclick="updateModel(this)" class="selected">Не обрано</li>`;
    else
        brandsOptions.innerHTML = '';
    brands.forEach(brand => {
        if (brand !== 'Не обрано' || selectBrandsBtn.firstElementChild.innerText !== 'Не обрано') {
            let isSelected = brand == selectBrandsBtn.firstElementChild.innerText ? "selected" : "";
            let li = `<li onclick="updateName(this)" class="${isSelected}">${brand}</li>`;
            brandsOptions.insertAdjacentHTML("beforeend", li);
        }
    });
}
function refreshModels() {
    if (selectModelsBtn.firstElementChild.innerText === 'Інше') return;
    if (selectModelsBtn.firstElementChild.innerText === 'Не обрано')
        modelsOptions.innerHTML = `<li onclick="updateModel(this)" class="selected">Не обрано</li>`;
    else
        modelsOptions.innerHTML = '';
    models.forEach(model => {
        if (model !== 'Не обрано' || selectModelsBtn.firstElementChild.innerText !== 'Не обрано') {
            let isSelected = model == selectModelsBtn.firstElementChild.innerText ? "selected" : "";
            let li = `<li onclick="updateModel(this)" class="${isSelected}">${model}</li>`;
            modelsOptions.insertAdjacentHTML("beforeend", li);
        }
    });
}
if (searchBrandInp)
searchBrandInp.addEventListener("keyup", () => {
    let arr = [];
    let searchWord = searchBrandInp.value.toLowerCase();
    arr = brands.filter(data => {
        return data.toLowerCase().startsWith(searchWord);
    }).map(data => {
        let isSelected = data == selectBrandsBtn.firstElementChild.innerText ? "selected" : "";
        return `<li onclick="updateName(this)" class="${isSelected}">${data}</li>`;
    }).join("");
    brandsOptions.innerHTML = arr ? arr : `<li onclick="updateName(this)">Інше</li>`;
});
if (searchModelInp)
searchModelInp.addEventListener("keyup", () => {
    let arr = [];
    let searchWord = searchModelInp.value.toLowerCase();
    arr = models.filter(data => {
        return data.toLowerCase().startsWith(searchWord);
    }).map(data => {
        let isSelected = data == selectModelsBtn.firstElementChild.innerText ? "selected" : "";
        return `<li onclick="updateModel(this)" class="${isSelected}">${data}</li>`;
    }).join("");
    modelsOptions.innerHTML = arr ? arr : `<li onclick="updateModel(this)">Інше</li>`;
});
function addBrand(selectedBrand) {
    if (!selectedBrand) {
        selectModelsBtn.firstElementChild.innerText = 'Не обрано';
        brandsOptions.innerHTML = `<li onclick="updateModel(this)" class="selected">Не обрано</li>`;
    }
    else
        brandsOptions.innerHTML = '';
    brands.forEach(brand => {
        if (brand != 'Не обрано' || selectedBrand) {
            let isSelected = brand == selectedBrand ? "selected" : "";
            let li = `<li onclick="updateName(this)" class="${isSelected}">${brand}</li>`;
            brandsOptions.insertAdjacentHTML("beforeend", li);
        }
    });
}
function addModel(selectedModel, onlyOther) {
    if (onlyOther) {
        modelsOptions.innerHTML = `<li onclick="updateModel(this)" class="selected">Інше</li>`;
        console.log(modelsOptions.innerHTML)
        console.log(modelsOptions)
        return;
    }
    if (!selectedModel) {
        otherModelInp.style.display = 'none';
        selectModelsBtn.querySelector('span').style.display = 'block';
        selectModelsBtn.firstElementChild.innerText = 'Не обрано';
        modelsOptions.innerHTML = `<li onclick="updateModel(this)" class="selected">Не обрано</li>`;
    }
    else {
        modelsOptions.innerHTML = '';
    }
    models.forEach(model => {
        if (model != 'Не обрано' || selectedModel) {
            let isSelected = model == selectedModel ? "selected" : "";
            let li = `<li onclick="updateModel(this)" class="${isSelected}">${model}</li>`;
            modelsOptions.insertAdjacentHTML("beforeend", li);
        }
    });
}
function addBody(selectedBody) {
    bodiesOptions.innerHTML = '';
    bodies.forEach(body => {
        let isSelected = body == selectedBody || !selectedBody && body == 'Не обрано' ? "selected" : "";
        let li = `<li onclick="updateBody(this)" class="${isSelected}">${body}</li>`;
        bodiesOptions.insertAdjacentHTML("beforeend", li);
    });
}
function addTransmission(selectedTransmission) {
    transmissionsOptions.innerHTML = '';
    transmissions.forEach(transmission => {
        let isSelected = transmission == selectedTransmission || !selectedTransmission && transmission == 'Не обрано' ? "selected" : "";
        let li = `<li onclick="updateTransmission(this)" class="${isSelected}">${transmission}</li>`;
        transmissionsOptions.insertAdjacentHTML("beforeend", li);
    });
}
function addFuel(selectedFuel) {
    fuelsOptions.innerHTML = '';
    for (const [key, value] of Object.entries(fuels)) {
        let isSelected = key == selectedFuel || !selectedFuel && key == 'Не обрано' ? "selected" : "";
        let li = `<li onclick="updateFuel(this)" class="${isSelected}">${key}</li>`;
        fuelsOptions.insertAdjacentHTML("beforeend", li);
    }
}
function addDriveline(selectedDriveline) {
    drivelinesOptions.innerHTML = '';
    drivelines.forEach(driveline => {
        let isSelected = driveline == selectedDriveline || !selectedDriveline && driveline == 'Не обрано' ? "selected" : "";
        let li = `<li onclick="updateDriveline(this)" class="${isSelected}">${driveline}</li>`;
        drivelinesOptions.insertAdjacentHTML("beforeend", li);
    });
}
function addColor(selectedColor) {
    colorsOptions.innerHTML = '';
    colors.forEach(color => {
        let isSelected = color == selectedColor || !selectedColor && color == 'Не обрано' ? "selected" : "";
        let li = `<li onclick="updateColor(this)" class="${isSelected}">${color}</li>`;
        colorsOptions.insertAdjacentHTML("beforeend", li);
    });
}
function updateName(selectedLi) {
    if (selectedLi.innerText != "Не обрано") {
        brands = brands.filter(c => c !== 'Не обрано');
        brandRealInp.value = selectedLi.innerText;
        otherBrandInp.style.display = 'none';
        selectBrandsBtn.querySelector('span').style.display = 'block';
    }
    if (selectedLi.innerText == 'Інше') {
        otherBrandInp.value = "";
        otherBrandInp.style.display = 'block';
        selectBrandsBtn.querySelector('span').style.display = 'none';
        addBrand();
        selectBrandsBtn.firstElementChild.innerText = `Інше`
        otherModelInp.value = "";
        otherModelInp.style.display = 'block';
        selectModelsBtn.querySelector('span').style.display = 'none';
        modelRealInp.value = "Any";
        brandRealInp.value = "Any";
        selectModelsBtn.firstElementChild.innerText = 'Інше'
        models = models.filter(c => c !== 'Не обрано');
        addModel("Інше", true);
    }
    else
        addModel();
    searchBrandInp.value = "";
    addBrand(selectedLi.innerText);
    brandsOptions.parentElement.classList.remove("active");
    selectBrandsBtn.classList.remove("active");
    selectBrandsBtn.firstElementChild.innerText = selectedLi.innerText;
    if (selectBrandsBtn.firstElementChild.innerText == "Не обрано")
        modelsOptions.innerHTML = `<li onclick="updateModel(this)" class="selected">Не обрано</li>`;
    else if (selectedLi.innerText != 'Інше')
        getModelsOfMark();
}
function updateModel(selectedLi) {
    if (selectedLi.innerText != "Не обрано" && selectedLi.innerText != "Інше") {
        models = models.filter(c => c !== 'Не обрано');
        modelRealInp.value = selectedLi.innerText;
        otherModelInp.style.display = 'none';
        selectModelsBtn.querySelector('span').style.display = 'block';
    }
    else {
        if (selectedLi.innerText == 'Інше') {
            otherModelInp.value = "";
            otherModelInp.style.display = 'block';
            selectModelsBtn.querySelector('span').style.display = 'none';
        }
        modelRealInp.value = "Any";
    }
    searchModelInp.value = "";
    addModel(selectedLi.innerText);
    modelsOptions.parentElement.classList.remove("active");
    selectModelsBtn.classList.remove("active");
    selectModelsBtn.firstElementChild.innerText = selectedLi.innerText;
}
function updateBody(selectedLi) {
    if (selectedLi.innerText != "Не обрано") {
        bodies = bodies.filter(c => c !== 'Не обрано');
        bodyRealInp.value = bodies.indexOf(selectedLi.innerText) + 1;
    }
    addBody(selectedLi.innerText);
    bodiesOptions.parentElement.classList.remove("active");
    selectBodiesBtn.classList.remove("active");
    selectBodiesBtn.firstElementChild.innerText = selectedLi.innerText;
}
function updateTransmission(selectedLi) {
    if (selectedLi.innerText != "Не обрано") {
        transmissions = transmissions.filter(c => c !== 'Не обрано');
        transmissionRealInp.value = transmissions.indexOf(selectedLi.innerText) + 1;
    }
    addTransmission(selectedLi.innerText);
    transmissionsOptions.parentElement.classList.remove("active");
    selectTransmissionsBtn.classList.remove("active");
    selectTransmissionsBtn.firstElementChild.innerText = selectedLi.innerText;
}
function updateFuel(selectedLi) {
    if (selectedLi.innerText != "Не обрано") {
        delete fuels["Не обрано"];
        fuelRealInp.value = fuels[selectedLi.innerText];
    }
    if (selectedLi.innerText == "Електро") {
        engineVolumeInp.setAttribute('placeholder', 'Ємність акумулятора');
        engineVolumeLbl.innerHTML = 'Ємність акумулятора (кВт·год.)';
        isElectro = true;
    }
    else {
        engineVolumeInp.setAttribute('placeholder', "Об'єм двигуна");
        engineVolumeLbl.innerHTML = "Об'єм двигуна(л.)";
        isElectro = false;
        if (engineVolumeInp.value.includes('.')) engineVolumeInp.value = engineVolumeInp.value.slice(0, 3);
        else engineVolumeInp.value = engineVolumeInp.value.slice(0, 2);
    }
    addFuel(selectedLi.innerText);
    fuelsOptions.parentElement.classList.remove("active");
    selectFuelsBtn.classList.remove("active");
    selectFuelsBtn.firstElementChild.innerText = selectedLi.innerText;
}
function updateDriveline(selectedLi) {
    if (selectedLi.innerText != "Не обрано") {
        drivelines = drivelines.filter(c => c !== 'Не обрано');
        drivelineRealInp.value = drivelines.indexOf(selectedLi.innerText) + 1;
    }
    addDriveline(selectedLi.innerText);
    drivelinesOptions.parentElement.classList.remove("active");
    selectDrivelinesBtn.classList.remove("active");
    selectDrivelinesBtn.firstElementChild.innerText = selectedLi.innerText;
}
function updateColor(selectedLi) {
    if (selectedLi.innerText != "Не обрано") {
        colors = colors.filter(c => c !== 'Не обрано');
        colorRealInp.value = colors.indexOf(selectedLi.innerText);
    }
    addColor(selectedLi.innerText);
    colorsOptions.parentElement.classList.remove("active");
    selectColorsBtn.classList.remove("active");
    selectColorsBtn.firstElementChild.innerText = selectedLi.innerText;
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
function hideColor() {
    colorsOptions.parentElement.classList.remove("active");
    selectColorsBtn.classList.remove("active");
    colorsOptions.scrollTop = 0;
}
//#endregion

//#region Click events
if (selectBrandsBtn)
selectBrandsBtn.addEventListener("click", (e) => {
    if (e.target.tagName != 'INPUT') {
        searchBrandInp.value = "";
        brandsOptions.parentElement.classList.toggle("active");
        selectBrandsBtn.classList.toggle("active");
    }
});
if (selectModelsBtn)
selectModelsBtn.addEventListener("click", (e) => {
    if (e.target.tagName != 'INPUT') {
        searchModelInp.value = "";
        modelsOptions.parentElement.classList.toggle("active");
        selectModelsBtn.classList.toggle("active");
    }
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
selectColorsBtn.addEventListener("click", () => {
    colorsOptions.parentElement.classList.toggle("active");
    selectColorsBtn.classList.toggle("active");
    colorsOptions.scrollTop = 0;
});
document.addEventListener('click', function (event) {
    if (selectBrandsBtn&&!selectBrandsBtn.parentElement.contains(event.target)) {
        hideBrand();
        refreshBrands();
    }
    if (selectModelsBtn&&!selectModelsBtn.parentElement.contains(event.target)) {
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
    if (!selectColorsBtn.parentElement.contains(event.target))
        hideColor();
});
//#endregion