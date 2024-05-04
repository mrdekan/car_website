const container = document.getElementById('orders-container');

getOrders();

function getOrders() {
    fetch(`/api/v1/orders/getAll`)
        .then(response => response.json())
        .then(data => {
            if (data == null || data.status == false || data.success == false)
                container.innerHTML = `<h3 style="text-align: center;" class="warning-text">Помилка при отриманні даних</h3>`;
            else {
                container.innerHTML = '';
                if (data.orders.length > 0)
                    for (let i = 0; i < data.orders.length;) {
                        let temp = '<div class="orders-row">';
                        for (let j = 0; j < 3; j++, i++) {
                            order = data.orders[i];
                            if (!order)
                                break;
                            temp += `<div class='order'>
                                <div class='order-row'><p>Марка: </p><span>${order.brand || 'Не обрано'}</span></div>
                                <div class='order-row'><p>Модель: </p><span>${order.model || 'Не обрано'}</span></div>
                                <div class='order-row'><p>Рік: </p><span>${order.year || 'Не обрано'}</span></div>
                                <div class='order-row'><p>Максимальна ціна: </p><span>${order.maxPrice || 'Не обрано'}</span></div>
                                ${order.description ? `<div class='order-row'><p>Опис:</p><span>${order.description}</span></div>` : ''}
                                ${order.name ? `<div class='order-row'><p>Ім'я: </p><span>${order.name}</span></div>` : ''}
                                ${order.phone ? `<div class='order-row'><p>Телефон: </p><span>${order.phone}</span></div>` : ''}
                                </div>`;
                        }
                        temp += '</div>';
                        container.innerHTML += temp;
                    }
                else
                    container.innerHTML = `<h3 style="text-align: center;" class="warning-text">Запити на покупку відсутні</h3>`;
            }
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}