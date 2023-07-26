const pass_field = document.getElementById("pass");
const confirmpassfield = document.getElementById("confirmpass");
const pass_checkbox = document.getElementById("pass_show");
const confirmpass_checbox = document.getElementById("confirmpass_show");

pass_checkbox.addEventListener('change', function () {
    pass_field.setAttribute("type", pass_checkbox.checked ? "text" : "password");
});

confirmpass_checbox.addEventListener('change', function () {
    confirmpassfield.setAttribute("type", confirmpass_checbox.checked ? "text" : "password");
});