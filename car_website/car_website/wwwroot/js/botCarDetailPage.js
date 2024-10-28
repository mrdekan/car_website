import { notificationModule } from './modules/notificationModule.js/index.js';
const deleteBtn = document.getElementById('delete');
deleteBtn.addEventListener('click', () => {
    if(confirm(`Видалити це авто? Ви не матиме можливості відновити його потім.`))
        fetch(`/api/v1/bot/delete/${deleteBtn.getAttribute('carId')}`, {
        method: 'DELETE',
    })
        .then(response => response.json())
        .then(data => {
            if (data.status)
                notificationModule.showNotification('Авто успішно видалено');
            else if (data.status == false)
                notificationModule.showNotification('Сталася помилка: ' + data.code, true);
            else
                notificationModule.showNotification('Сталася невідома помилка', true);
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
});
