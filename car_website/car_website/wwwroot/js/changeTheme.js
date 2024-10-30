const themeRadiosLight = document.getElementById('light'),
themeRadiosDark = document.getElementById('dark');
if (localStorage.getItem('theme') == 'dark') themeRadiosDark.checked = true;
else themeRadiosLight.checked = true;
function handleRadioChange(radio) {
    if (radio.target.getAttribute('theme') == 'dark') setDark();
    else setLight();
}
themeRadiosDark.addEventListener('click', handleRadioChange);
themeRadiosLight.addEventListener('click', handleRadioChange);
