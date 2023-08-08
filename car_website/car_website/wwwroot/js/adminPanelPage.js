const radioButtons = document.querySelectorAll('input[name="action"]');
const container = document.getElementById('container');
let buyRequestsPage = 1;
let buyRequestsCache;
let waitingCarsPage = 1;
let waitingCarsCache;
let usersPage = 1;
let usersCache;
window.addEventListener('load', function () {
    radioButtons.forEach(function (radio) {
        if (radio.checked) {
            selectedRadioButton = radio;
            updateInfo(selectedRadioButton);
        }
    });
});

radioButtons.forEach(function (radio) {
    radio.addEventListener('change', handleRadioChange);
});
function handleRadioChange(event) {
    updateInfo(event.target);
}
function updateInfo(target) {
    container.innerHTML = "";
    if (target.id == "users") {
        if (usersCache == null)
            getUsers();
        else {
            showData(usersCache);
        }
    }
    else if (target.id == "buyRequests") {
        if (buyRequestsCache == null)
            console.log("Not implemented yet");
        else {
            showData(buyRequestsCache);
        }
    }
    else if (target.id == "waitingCars") {
        if (waitingCarsCache == null)
            console.log("Not implemented yet");
        else {
            showData(waitingCarsCache);
        }
    }
}
function showData(data) {
    if (data == null || data.success == false) {
        container.innerHTML = "Помилка при отриманні даних";
    }
    else {
        container.innerHTML = "";
        data.users.forEach(user => {
            container.innerHTML += `<div class="user_container-element">
                <a href="/User/Detail/${user.id}">${user.name} ${user.surname}</a>
                <p>${user.phoneNumber}</p>
            </div>`;
        });
    }
}

//#region Ajax requests
function getUsers() {
    fetch(`/Admin/GetUsers?page=${usersPage}`)
        .then(response => response.json())
        .then(data => {
            usersCache = data;
            showData(usersCache);
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
//#endregion