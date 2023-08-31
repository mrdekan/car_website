const pass_field = document.getElementById("pass");
const pass_checkbox = document.getElementById("pass_show");

pass_checkbox.addEventListener('change', function () {
    pass_field.setAttribute("type", pass_checkbox.checked ? "text" : "password");
});
/*document.querySelector('form').addEventListener('submit', function (e) {
    e.preventDefault();

    var formData = new FormData(e.target);

    fetch('/api/v1/login', {
        method: 'POST',
        body: formData
    })
        .then(response => response.json())
        .then(data => {
            console.log(data);
            getData(`/api/v1/getCarsCount`)
            


        })
        .catch(error => {
            console.error('Error:', error);
        });
});*/

async function getData(url) {
    const token = sessionStorage.getItem("accessToken");

    const response = await fetch(url, {
        method: "GET",
        headers: {
            credentials: 'include'
        }
    });
    console.log(response)
    if (response.ok === true) {

        const data = await response.json();
        console.log(response)
    }
    else
        console.log("Status: ", response.status);
};