const usersContainer = document.getElementById("users_container");
let page = 1;
getUsers();
//#region Ajax requests
function getUsers() {
    fetch(`/Admin/GetUsers?page=${page}`)
        .then(response => response.json())
        .then(data => {
            console.log(data);
            showData(data);
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
//#endregion
function showData(data) {
    if (data == null || data.success == false) {
        usersContainer.innerHTML = "Помилка при отриманні даних";
    }
    else {
        usersContainer.innerHTML = "";
        data.users.forEach(user => {
            usersContainer.innerHTML += `<div class="user_container-element">
                <a href="/User/Detail/${user.id}">${user.name} ${user.surname}</a>
                <p>${user.phoneNumber}</p>
            </div>`;
        });
    }
}