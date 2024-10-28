export function getIncomingCarsCount() {
    fetch(`/api/v1/cars/getIncomingCarsCount`)
        .then(response => response.json())
        .then(data => {
            if (data.code === 200 && data.count != undefined && data.count != null) {
                localStorage.setItem('incomingCarsCount', JSON.stringify({ time: Date.now(), count: data.count }));
                const elem = document.getElementById('incomingCarsCount');
                const elemMob = document.getElementById('incomingCarsCountMob');
                elem.innerHTML = data.count;
                elemMob.innerHTML = data.count;
                if (data.count != 0) {
                    elem.style.display = 'block';
                    elemMob.style.display = 'block';
                }
                else {
                    elem.style.display = 'none';
                    elemMob.style.display = 'none';
                }
            }
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}