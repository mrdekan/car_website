const root = document.documentElement;

if (localStorage.getItem('theme') == 'dark') {
    setDark();
}
else {
    setLight();
}
function setDark() {
    root.style.setProperty('--green-light', '#537049');
    root.style.setProperty('--green-medium', '#4b6742');
    root.style.setProperty('--green-dark', '#415939');
    root.style.setProperty('--background-color', '#222222');
    root.style.setProperty('--backgroundSecondary-color', '#303030');
    root.style.setProperty('--text-default', '#ffffff');
    root.style.setProperty('--icons-invert', '90%');
}
function setLight() {
    root.style.setProperty('--green-light', '#537049');
    root.style.setProperty('--green-medium', '#4b6742');
    root.style.setProperty('--green-dark', '#415939');
    root.style.setProperty('--background-color', '#ffffff');
    root.style.setProperty('--backgroundSecondary-color', '#f6f6f6');
    root.style.setProperty('--text-default', '#000000');
    root.style.setProperty('--icons-invert', '0%');
}