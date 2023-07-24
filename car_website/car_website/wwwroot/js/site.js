const apply_button = document.getElementById("refresh_cars");
apply_button.onclick = () => applyFilter();
function applyFilter() {
    const filters = {
        //name: document.getElementById("nameFilter").value,
        brand: "Chrysler",
        // Добавьте другие фильтры здесь
    };

    fetch(`/home/GetCars`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(filters)
    })
        .then(response => response.json())
        .then(data => {
            // Обновляем список машин на странице с полученными данными
            console.log(data);
            const carList = document.getElementById("carList");
            carList.innerHTML = ""; // Очищаем список перед добавлением новых элементов

            data.cars.forEach(car => {
                const listItem = document.createElement("li");
                listItem.innerText = car.Model;
                carList.appendChild(listItem);
            });
        })
        .catch(error => console.error("Произошла ошибка при получении данных:", error));
}