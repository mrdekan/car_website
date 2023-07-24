const brand_select = document.getElementById("brand-select");
const model_select = document.getElementById("model-select");

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
            model_select.innerHTML = '<option value="Any">Не обрано</option>';
            data.models.forEach(model => {
                model_select.innerHTML += `<option value=${model}>${model}</option>`;
            });
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}