const changeRoleBtn = document.getElementById('change-role');
changeRoleBtn.addEventListener('click', () => {
    let currentUrl = window.location.href;
    let parts = currentUrl.split("/");
    let userId = parts[parts.length - 1];
    if (changeRoleBtn.hasAttribute('cancel')) {
        fetch(`/api/v1/users/removeAdmin/${userId}`, {
            method: "PUT"
        })
            .then(response => response.json())
            .then(data => {
                if (data && data.status == true) {
                    changeRoleBtn.innerHTML = 'Зробити адміном';
                    changeRoleBtn.removeAttribute('cancel');
                }
            })
            .catch(error => console.error("An error occurred while retrieving data:", error));
    }
    else {
        fetch(`/api/v1/users/setAdmin/${userId}`, {
            method: "PUT"
        })
            .then(response => response.json())
            .then(data => {
                if (data && data.status == true) {
                    changeRoleBtn.innerHTML = 'Забрати адмінку';
                    changeRoleBtn.setAttribute('cancel','');
                }
            })
            .catch(error => console.error("An error occurred while retrieving data:", error));
    }
});