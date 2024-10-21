const descriptionLength = document.getElementById("descriptionLength");
const description = document.getElementById("description");
const climatControl = document.getElementById("climat-control");
const airConditioner = document.getElementById("air-conditioner");
const datePicker = document.getElementById("pick-arrival");
const dateInput = document.getElementById("arrivalDate");
if (climatControl)
climatControl.addEventListener('input', () => {
    if (climatControl.checked)
        airConditioner.checked = false;
});
if (airConditioner)
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
if (datePicker) {
    const todayDate = new Date();
    const mm = String(todayDate.getMonth() + 1).padStart(2, '0');
    const dd = String(todayDate.getDate()).padStart(2, '0');
    const yyyy = todayDate.getFullYear();
    const today= yyyy+'-'+mm+'-'+dd;
    datePicker.setAttribute('min', today);
    datePicker.addEventListener('input', function (event) {
        let date = new Date(datePicker.value);
        const dd = String(date.getDate()).padStart(2, '0');
        const mm = String(date.getMonth() + 1).padStart(2, '0');
        const yyyy = date.getFullYear();
        const resDate = dd + '.' + mm + '.' + yyyy;
        console.log(resDate);
        dateInput.value = resDate;
    })
}