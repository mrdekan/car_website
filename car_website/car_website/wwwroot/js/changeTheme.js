const themeRadios = document.querySelectorAll('input[name="theme"]');
if (localStorage.getItem('theme') == 'dark') {
    themeRadios[1].checked = true;
}
else {
    themeRadios[0].checked = true;
}
function handleRadioChange(radio) {
    if (radio.target.getAttribute('theme') == 'dark') {
        localStorage.setItem('theme', 'dark');
        setDark();
    }
    else {
        localStorage.setItem('theme', 'light');
        setLight();
    }
}
themeRadios.forEach(function (radio) {
    radio.addEventListener('change', handleRadioChange);
});