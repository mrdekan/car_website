const selectBrandsBtn = document.getElementById("brandsButton"),
    searchBrandInp = document.getElementById("searchCar"),
    brandsOptions = document.getElementById("brands");

const priceUAH = document.getElementById('price-uah');
const priceInput = document.getElementById('price-input');
const description = document.getElementById('description');
const descriptionLimit = document.getElementById('description-limit');
let currencyRate = 0;
let brands = ["Не обрано"];
let models = ["Не обрано"];
getCurrencyRate();
getMarks();
priceInput.addEventListener("keydown", (event) => {
    if (event.key === "." || event.key === ",")
        event.preventDefault();
});
priceInput.addEventListener("input", () => {
    if (priceInput.value.length > 9)
        priceInput.value = priceInput.value.slice(0, 9);
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
            brands = ["Не обрано"];
            brands = brands.concat(data.brands);
            brands = brands.filter((n) => { return n != 'Інше' });
            addBrand();
            console.log(brands)
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
function formatNumberWithThousandsSeparator(number) {
    return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ' ');
}
function addBrand(selectedBrand) {
    if (!selectedBrand) {
        //selectModelsBtn.firstElementChild.innerText = 'Усі';
        brandsOptions.innerHTML = `<li onclick="updateModel(this)" class="selected">Не обрано</li>`;
    }
    else {
        brandsOptions.innerHTML = '';
    }
    brands.forEach(brand => {
        if (brand != 'Не обрано' || selectedBrand) {
            let isSelected = brand == selectedBrand ? "selected" : "";
            let li = `<li onclick="updateName(this)" class="${isSelected}">${brand}</li>`;
            brandsOptions.insertAdjacentHTML("beforeend", li);
        }
    });
}
function updateName(selectedLi) {
    searchBrandInp.value = "";
    addBrand(selectedLi.innerText);
    brandsOptions.parentElement.classList.remove("active");
    selectBrandsBtn.classList.remove("active");
    selectBrandsBtn.firstElementChild.innerText = selectedLi.innerText;
    if (selectBrandsBtn.firstElementChild.innerText == "Усі")
        modelsOptions.innerHTML = `<li onclick="" class="selected">Усі</li>`; //updateModel(this)
    else {
        //getModelsOfMark();
    }
}
selectBrandsBtn.addEventListener("click", () => {
    searchBrandInp.value = "";
    brandsOptions.parentElement.classList.toggle("active");
    selectBrandsBtn.classList.toggle("active");
});