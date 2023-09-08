const root = document.documentElement;
const themeSeparator = document.querySelector('.header_right-separator');
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
    themeSeparator.style.setProperty('--custom-border-radius', '0 7px 7px 0');
    root.style.setProperty('--green-light', '#18bacd');
    root.style.setProperty('--green-medium', '#0885a4');
    root.style.setProperty('--green-dark', '#0d769a');
    root.style.setProperty('--background-main', '#222222');
    root.style.setProperty('--background-secondary', '#303030');
    root.style.setProperty('--text-default', '#ffffff');
    root.style.setProperty('--icons-invert', '90%');
    root.style.setProperty('--success-main', '#46a75f');
    root.style.setProperty('--success-secondary', '#2b3129');
    root.style.setProperty('--error-main', '#ff3636'); //#c21111
    root.style.setProperty('--error-secondary', '#442c31');
    root.style.setProperty('--gray-default', '#999999');
    root.style.setProperty('--beige-default', '#ed7a13'); //A39E87 7D7A53 6F7255 5E6044
    root.style.setProperty('--theme-separator', '2px');
    root.style.setProperty('--filters-background', '#303030');
}
function setLight() {
    themeSeparator.style.setProperty('--custom-border-radius', '7px 0 0 7px');
    root.style.setProperty('--green-light', '#537049');
    root.style.setProperty('--green-medium', '#4b6742');
    root.style.setProperty('--green-dark', '#415939');
    root.style.setProperty('--background-main', '#ffffff');
    root.style.setProperty('--background-secondary', '#f6f6f6');
    root.style.setProperty('--text-default', '#000000');
    root.style.setProperty('--icons-invert', '0%');
    root.style.setProperty('--success-main', '#3a784a');
    root.style.setProperty('--success-secondary', '#D0E0CA');
    root.style.setProperty('--error-main', '#ff3636 ');
    root.style.setProperty('--error-secondary', '#f7dfe5');
    root.style.setProperty('--gray-default', '#505050');
    root.style.setProperty('--beige-default', '#F3EFCA');
    root.style.setProperty('--theme-separator', '-31px');
    root.style.setProperty('--filters-background', '#F3EFCA');
}