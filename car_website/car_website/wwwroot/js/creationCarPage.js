const descriptionLength = document.getElementById("descriptionLength");
const description = document.getElementById("description");
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
