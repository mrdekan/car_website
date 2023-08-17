const root = document.documentElement;

if (localStorage.getItem('theme') == 'dark') {
    setDark();
}
else {
    setLight();
}
/*
    --success-main: #3a784a;
    --success-secondary: #D0E0CA;
    --error-main: #db3d5f;
    --error-secondary: #f7dfe5;
    --gray-default: #404040;
*/
function setDark() {
    root.style.setProperty('--green-light', '#537049');
    root.style.setProperty('--green-medium', '#4b6742');
    root.style.setProperty('--green-dark', '#415939');
    root.style.setProperty('--background-main', '#222222');
    root.style.setProperty('--background-secondary', '#303030');
    root.style.setProperty('--text-default', '#ffffff');
    root.style.setProperty('--icons-invert', '90%');
    root.style.setProperty('--success-main', '#46a75f');
    root.style.setProperty('--success-secondary', '#2b3129');
    root.style.setProperty('--error-main', '#ff436b');
    root.style.setProperty('--error-secondary', '#442c31');
    root.style.setProperty('--gray-default', '#999999');
}
function setLight() {
    root.style.setProperty('--green-light', '#537049');
    root.style.setProperty('--green-medium', '#4b6742');
    root.style.setProperty('--green-dark', '#415939');
    root.style.setProperty('--background-main', '#ffffff');
    root.style.setProperty('--background-secondary', '#f6f6f6');
    root.style.setProperty('--text-default', '#000000');
    root.style.setProperty('--icons-invert', '0%');
    root.style.setProperty('--success-main', '#3a784a');
    root.style.setProperty('--success-secondary', '#D0E0CA');
    root.style.setProperty('--error-main', '#db3d5f');
    root.style.setProperty('--error-secondary', '#f7dfe5');
    root.style.setProperty('--gray-default', '#505050');
}