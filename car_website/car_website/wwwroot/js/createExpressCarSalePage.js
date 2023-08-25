const priceUAH = document.getElementById('price-uah');
const priceInput = document.getElementById('price-input');
let currencyRate = 0;
getMarks();
priceInput.addEventListener("keydown", (event) => {
    if (event.key === "." || event.key === ",")
        event.preventDefault();
});
priceInput.addEventListener("input", () => {
    if (priceInput.value.length > 6)
        priceInput.value = priceInput.value.slice(0, 6);
    if (priceInput.value != '0' && priceInput.value != '' && currencyRate != 0)
        priceUAH.innerHTML = `≈ ${Math.round(Number(priceInput.value) * currencyRate)} грн`;
    else
        priceUAH.innerHTML = '';
});
function getMarks() {
    fetch(`/home/GetCurrency`)
        .then(response => response.json())
        .then(data => {
            if (data != null && data.success == true)
                currencyRate = data.currencyRate;
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}