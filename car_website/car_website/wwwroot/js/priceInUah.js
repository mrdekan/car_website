import { formatNumberWithThousandsSeparator } from './modules/formCar.js';
const inputForPriceInUsd = document.getElementById('price'),
spanForPriceInUah = document.getElementById('price-in-uah'),
text = spanForPriceInUah.innerHTML;
spanForPriceInUah.innerHTML = `${text} ≈ ${formatNumberWithThousandsSeparator(Math.round((+inputForPriceInUsd.value) * (+spanForPriceInUah.getAttribute('currency').replace(',','.'))))} грн`;
inputForPriceInUsd.addEventListener('input', () => {
    spanForPriceInUah.innerHTML = `${text} ≈ ${formatNumberWithThousandsSeparator(Math.round((+inputForPriceInUsd.value) * (+spanForPriceInUah.getAttribute('currency').replace(',', '.'))))} грн`;
});