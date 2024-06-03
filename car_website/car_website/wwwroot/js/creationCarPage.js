const descriptionLength = document.getElementById("descriptionLength");
const description = document.getElementById("description");
const climatControl = document.getElementById("climat-control");
const airConditioner = document.getElementById("air-conditioner");
climatControl.addEventListener('input', () => {
    if (climatControl.checked)
        airConditioner.checked = false;
});
airConditioner.addEventListener('input', () => {
    if (airConditioner.checked)
        climatControl.checked = false;
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
if (description.hasAttribute('restore')) {
    description.innerHTML = description.getAttribute('value');
    descriptionLength.innerHTML = `${description.value.length}/${parseInt(description.getAttribute('maxlength'))}`;
}
