const pass_field = document.getElementById("pass");
const pass_checkbox = document.getElementById("pass_show");

pass_checkbox.addEventListener('change', function () {
    pass_field.setAttribute("type", pass_checkbox.checked ? "text" : "password");
});