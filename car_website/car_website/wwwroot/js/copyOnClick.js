import { notificationModule } from './notificationModule.js';

const copyButtons = document.querySelectorAll('.copy-on-click');
copyButtons.forEach(function (button) {
    button.addEventListener('click', function () {
        const textToCopy = button.getAttribute('copy-text');
        const tempInput = document.createElement('textarea');
        tempInput.value = textToCopy;
        document.body.appendChild(tempInput);
        tempInput.select();
        tempInput.setSelectionRange(0, 99999);
        document.execCommand('copy');
        document.body.removeChild(tempInput);
        notificationModule.showNotification('Скопійовано!');
    });
});