const selectBrandsBtn = document.getElementById("brandsButton"),
    searchBrandInp = document.getElementById("searchCar"),
    brandsOptions = document.getElementById("brands"),
    brandInput = document.getElementById('brand-input'),
    selectModelsBtn = document.getElementById("modelsButton"),
    searchModelInp = document.getElementById("searchModel"),
    modelsOptions = document.getElementById("models"),
    modelInput = document.getElementById('model-input'),
    brandRealInp = document.getElementById('real-brand-inp'),
    modelRealInp = document.getElementById('real-model-inp'),
    otherModelInp = document.getElementById('other-model-inp'),
    otherBrandInp = document.getElementById('other-brand-inp');
const yearInput = document.getElementById("year-input");
const priceUAH = document.getElementById('price-uah');
const priceInput = document.getElementById('price-input');
const description = document.getElementById('description');
const descriptionLimit = document.getElementById('description-limit');
let currencyRate = 0;
let brands = ["Не обрано"];
let models = ["Не обрано"];
let modelsCache = {};
getCurrencyRate();
getMarks();
addModel("Не обрано");
priceInput.addEventListener("keydown", (event) => {
    if (event.key === "." || event.key === ",")
        event.preventDefault();
});
yearInput.addEventListener("keydown", (event) => {
    if (event.key === "." || event.key === ",")
        event.preventDefault();
});
yearInput.addEventListener("input", () => {
    if (yearInput.value.length > 4)
        yearInput.value = yearInput.value.slice(0, 4);
});
priceInput.addEventListener("input", () => {
    if (priceInput.value.length > 7)
        priceInput.value = priceInput.value.slice(0, 7);
    if (priceInput.value != '0' && priceInput.value != '' && currencyRate != 0)
        priceUAH.innerHTML = `≈ ${formatNumberWithThousandsSeparator(Math.round(Number(priceInput.value) * currencyRate))} грн`;
    else
        priceUAH.innerHTML = '';
});
description.addEventListener("input", () => {
    if (description.value.length > Number(description.getAttribute('maxlength')))
        description.value = description.value.slice(0, Number(description.getAttribute('maxlength')));
    descriptionLimit.innerHTML = `${description.value.length}/${description.getAttribute('maxlength')}`;
});
//#region Ajax requests
function getCurrencyRate() {
    fetch(`/home/GetCurrency`)
        .then(response => response.json())
        .then(data => {
            if (data != null && data.success == true)
                currencyRate = data.currencyRate;
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
function getMarks() {
    fetch(`/home/GetBrands`)
        .then(response => response.json())
        .then(data => {
            brands = data.brands;
            addBrand(selectBrandsBtn.firstElementChild.innerText);
            if (selectBrandsBtn.firstElementChild.innerText != 'Не обрано')
                getModelsOfMark(selectBrandsBtn.firstElementChild.innerText);
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
function getModelsOfMark() {
    var brand = selectBrandsBtn.firstElementChild.innerText;
    if (modelsCache[brand] == null) {
        fetch(`/home/GetModels?brand=${brand}`)
            .then(response => response.json())
            .then(data => {
                models = data.models;
                modelsCache[brand] = models;
                addModel();
                refreshModels();
            })
            .catch(error => console.error("An error occurred while retrieving data:", error));
    }
    else {
        models = modelsCache[brand];
        addModel();
    }
}
//#endregion
function formatNumberWithThousandsSeparator(number) {
    return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ' ');
}
//#region Custom select boxes
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
    console.log(onlyOther)
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
searchModelInp.addEventListener("keyup", () => {
    let arr = [];
    let searchWord = searchModelInp.value.toLowerCase();
    arr = models.filter(data => {
        return data.toLowerCase().startsWith(searchWord);
    }).map(data => {
        let isSelected = data == selectModelsBtn.firstElementChild.innerText ? "selected" : "";
        return `<li onclick="updateModel(this)" class="${isSelected}">${data}</li>`;
    }).join("");
    modelsOptions.innerHTML = arr ? arr : `<li onclick="updateModel(this)" ${selectBrandsBtn.firstElementChild.innerText == 'Не обрано' ? 'class="selected"' : ''}>${selectBrandsBtn.firstElementChild.innerText == 'Не обрано' ? 'Не обрано' : 'Інше'}</li>`;
});
selectBrandsBtn.addEventListener("click", (e) => {
    if (e.target.tagName != 'INPUT') {
        searchBrandInp.value = "";
        brandsOptions.parentElement.classList.toggle("active");
        selectBrandsBtn.classList.toggle("active");
    }
});
selectModelsBtn.addEventListener("click", (e) => {
    if (e.target.tagName != 'INPUT') {
        searchModelInp.value = "";
        modelsOptions.parentElement.classList.toggle("active");
        selectModelsBtn.classList.toggle("active");
    }
});
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
document.addEventListener('click', function (event) {
    if (!selectBrandsBtn.parentElement.contains(event.target)) {
        hideBrand();
        refreshBrands();
    }
    if (!selectModelsBtn.parentElement.contains(event.target)) {
        hideModel();
        refreshModels();
    }
});
//#ednregion