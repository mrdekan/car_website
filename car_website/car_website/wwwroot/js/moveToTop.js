const topBtn = document.getElementById('top');
topBtn.addEventListener('click', () => {
    fetch(`/api/v1/cars/setPriority?carId=${topBtn.getAttribute('carId')}&cancel=${topBtn.hasAttribute('cancel')}`, {
        method: "PUT"
    })
        .then(response => response.json())
        .then(data => {
            if (data.status == true) {
                if (topBtn.hasAttribute('cancel')) {
                    topBtn.removeAttribute('cancel');
                    topBtn.innerHTML = 'В топ';
                }
                else {
                    topBtn.setAttribute('cancel', '');
                    topBtn.innerHTML = 'Прибрати з топу';
                }
                topBtn.classList.toggle('cancelBuy');

            }
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
});