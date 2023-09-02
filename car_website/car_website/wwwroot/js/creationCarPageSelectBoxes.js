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

getMarks();
if (selectBrandsBtn.firstElementChild.innerText != "Усі")
    getModelsOfMark();
addBrand();
addModel();
addBody();
addTransmission();
addFuel();
addDriveline();

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