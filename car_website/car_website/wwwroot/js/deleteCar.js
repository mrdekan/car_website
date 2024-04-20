const deleteButton = document.getElementById('delete');



deleteButton.addEventListener('click', () => {
    if (confirm(deleteButton.hasAttribute('delete') ? "Ви точно впевнені, що хочете видалити це авто?" : "Ви точно впевнені, що хочете відмітити це авто, як продане?")) {
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