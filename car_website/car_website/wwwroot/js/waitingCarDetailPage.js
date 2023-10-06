const rejectTextarea = document.querySelector('textarea');
const rejectSymbolsLeft = document.getElementById('reject-symbols');
rejectTextarea.addEventListener('input', function (event) {
    const maxLength = parseInt(event.target.getAttribute('maxlength'));
    let currentValue = event.target.value;
    if (currentValue.length > maxLength) {
        currentValue = currentValue.slice(0, maxLength);
    }
    event.target.value = currentValue;
    rejectSymbolsLeft.innerHTML = `${currentValue.length}/${maxLength}`;
});