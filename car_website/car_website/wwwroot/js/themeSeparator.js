﻿const themeSeparator = document.querySelector('.header_right-separator');
if (localStorage.getItem('theme') == 'light' && themeSeparator)
    themeSeparator.style.setProperty('--custom-border-radius', '7px 0 0 7px');
else if (themeSeparator)
    themeSeparator.style.setProperty('--custom-border-radius', '0 7px 7px 0');