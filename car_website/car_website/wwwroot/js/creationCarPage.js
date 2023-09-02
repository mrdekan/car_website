const brand_select = document.getElementById("brand-select");
const model_select = document.getElementById("model-select");
const descriptionLength = document.getElementById("descriptionLength");
const description = document.getElementById("description");
const allSelects = document.querySelectorAll('select');
const photosBlock = document.getElementById("photos-select");



allSelects.forEach(select => {
    select.addEventListener('change', function () {
        const options = Array.from(this.options);
        const optionToRemove = options.find(option => option.text === 'Не обрано');
        if (optionToRemove && !optionToRemove.selected) {
            optionToRemove.remove();
        }
    });
});

description.addEventListener('input', function (event) {
    const maxLength = parseInt(event.target.getAttribute('maxlength'));
    let currentValue = event.target.value;
    if (currentValue.length > maxLength) {
        currentValue = currentValue.slice(0, maxLength);
    }
    event.target.value = currentValue;
    descriptionLength.innerHTML = `${currentValue.length}/${maxLength}`;
});

brand_select.addEventListener('change', function () {
    if (brand_select.value == "Any")
        model_select.innerHTML = '<option value="Any">Не обрано</option>';
    else
        getModelsOfMark();
});

function getModelsOfMark() {
    fetch(`/home/GetModels?brand=${brand_select.value}`)
        .then(response => response.json())
        .then(data => {
            if (brand_select.value!="Інше")
                model_select.innerHTML = '<option value="Any">Не обрано</option>';
            else
                model_select.innerHTML = '';
            data.models.forEach(model => {
                model_select.innerHTML += `<option value=${model}>${model}</option>`;
            });
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}