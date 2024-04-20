const btn = document.getElementById('downloadButton');
btn.addEventListener('click', function () {
    const fileUrls = btn.getAttribute('files').split('__');
    fileUrls.forEach(function (url) {
        var xhr = new XMLHttpRequest();
        xhr.open('GET', 'https://1auto.cn.ua/' + url.replace(/\\/g, '/'), true);
        xhr.responseType = 'blob';
        xhr.onload = function () {
            if (xhr.status === 200) {
                var blob = new Blob([xhr.response], { type: 'application/octet-stream' });
                var link = document.createElement('a');
                link.href = window.URL.createObjectURL(blob);
                link.setAttribute('download', url.split('/').pop());
                link.style.display = 'none';
                document.body.appendChild(link);
                link.click();
                document.body.removeChild(link);
            }
        };
        xhr.send();
    });
});
