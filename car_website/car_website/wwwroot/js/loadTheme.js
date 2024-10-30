const root = document.documentElement;
if (localStorage.getItem('theme') == 'light') setLight();
else setDark()
function setDark() {
    localStorage.setItem('theme', 'dark');
    const themeSeparator = document.querySelector('.header_right-separator');
    if (themeSeparator) themeSeparator.style.setProperty('--custom-border-radius', '0 7px 7px 0');
    root.style.setProperty('--green-light', '#5b8894'); //#006383
    root.style.setProperty('--green-medium', '#3c7583'); //#184872
    root.style.setProperty('--green-dark', '#186472'); //#133d61
    root.style.setProperty('--background-main', '#222222');
    root.style.setProperty('--background-secondary', '#303030');
    root.style.setProperty('--text-default', '#ffffff');
    root.style.setProperty('--icons-invert', '90%');
    root.style.setProperty('--success-main', '#46a75f');
    root.style.setProperty('--success-secondary', '#2b3129');
    root.style.setProperty('--error-main', '#ff3636');
    root.style.setProperty('--error-secondary', '#442c31');
    root.style.setProperty('--gray-default', '#999999');
    root.style.setProperty('--beige-default', '#f86820');
    root.style.setProperty('--theme-separator', '2px');
    root.style.setProperty('--filters-background', '#303030');
}
function setLight() {
    localStorage.setItem('theme', 'light');
    const themeSeparator = document.querySelector('.header_right-separator');
    if (themeSeparator) themeSeparator.style.setProperty('--custom-border-radius', '7px 0 0 7px');
    root.style.setProperty('--green-light', '#5b8894'); //#537049
    root.style.setProperty('--green-medium', '#3c7583'); //#4b6742
    root.style.setProperty('--green-dark', '#186472'); //#415939 old green colors
    root.style.setProperty('--background-main', '#f6f6f6');
    root.style.setProperty('--background-secondary', '#ffffff');
    root.style.setProperty('--text-default', '#000000');
    root.style.setProperty('--icons-invert', '0%');
    root.style.setProperty('--success-main', '#3a784a');
    root.style.setProperty('--success-secondary', '#D0E0CA');
    root.style.setProperty('--error-main', '#ff3636 ');
    root.style.setProperty('--error-secondary', '#f7dfe5');
    root.style.setProperty('--gray-default', '#505050');
    root.style.setProperty('--beige-default', '#f86820'); //#ed7a13
    root.style.setProperty('--theme-separator', '-31px');
    root.style.setProperty('--filters-background', '#F3EFCA');
}