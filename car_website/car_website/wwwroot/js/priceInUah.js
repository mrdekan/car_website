const inputForPriceInUsd = document.getElementById('price');
const spanForPriceInUah = document.getElementById('price-in-uah');
const text = spanForPriceInUah.innerHTML;
spanForPriceInUah.innerHTML = `${text} ≈ ${formatNumberWithThousandsSeparator(Math.round((+inputForPriceInUsd.value) * (+spanForPriceInUah.getAttribute('currency'))))} грн`;

inputForPriceInUsd.addEventListener('input', () => {
    spanForPriceInUah.innerHTML = `${text} ≈ ${formatNumberWithThousandsSeparator(Math.round((+inputForPriceInUsd.value) * (+spanForPriceInUah.getAttribute('currency'))))} грн`;
});

function formatNumberWithThousandsSeparator(number) {
    return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ' ');
}