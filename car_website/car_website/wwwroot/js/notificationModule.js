export const notificationModule = (function () {
    let notificationTimeout;
    const activeNotifications = [];

    function showNotification(message, duration = 3000) {
        // Скрываем все активные уведомления
        activeNotifications.forEach(notification => hideNotification(notification));

        const notificationElement = document.createElement('div');
        notificationElement.classList.add('notification');
        notificationElement.textContent = message;
        document.body.appendChild(notificationElement);
        activeNotifications.push(notificationElement);

        setTimeout(() => {
            notificationElement.style.transform = 'translateY(-125%)';
            clearTimeout(notificationTimeout);
            notificationTimeout = setTimeout(() => {
                hideNotification(notificationElement);
            }, duration);
        }, 50);

        notificationElement.addEventListener('click', () => {
            hideNotification(notificationElement);
        });
    }

    function hideNotification(notificationElement) {
        notificationElement.style.transform = 'translateY(10%)';
        notificationElement.style.opacity = '0.3';
        setTimeout(() => {
            notificationElement.parentNode.removeChild(notificationElement);
            const index = activeNotifications.indexOf(notificationElement);
            if (index !== -1) {
                activeNotifications.splice(index, 1);
            }
        }, 500);
    }

    return {
        showNotification
    };
})();
