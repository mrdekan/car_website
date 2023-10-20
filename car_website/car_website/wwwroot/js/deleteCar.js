const deleteButton = document.getElementById('delete');



deleteButton.addEventListener('click', () => {
    if (confirm("Ви точно впевнені, що хочете видалити це авто?")) {
        fetch(`/api/v1/cars/deleteCar/${deleteButton.getAttribute('carId')}`, {
            method: "DELETE"
        }).then(response => response.json())
            .then(data => {
                if (data && data.status == true) {
                    if (window.history.length > 1)
                        window.history.back();
                    else
                        window.location.href = '/';
                }
            })
            .catch(error => console.error("An error occurred while retrieving data:", error));
    }
});