const themeRadiosLight = document.getElementById('light');
const themeRadiosDark = document.getElementById('dark');
if (localStorage.getItem('theme') == 'dark') {
    themeRadiosDark.checked = true;
}
else {
    themeRadiosLight.checked = true;
}
function handleRadioChange(radio) {
    console.log(radio.target.getAttribute('theme'))
    if (radio.target.getAttribute('theme') == 'dark') {
        localStorage.setItem('theme', 'dark');
        setDark();
    }
    else {
        localStorage.setItem('theme', 'light');
        setLight();
    }
}
themeRadiosDark.addEventListener('click', handleRadioChange);
themeRadiosLight.addEventListener('click', handleRadioChange);
