const photoInputs = document.querySelectorAll('.photoInp');
photoInputs.forEach((inp) => {
    if (!inp.hasAttribute('multiple'))
    inp.addEventListener('change', function () {
    const file = this.files[0];
    if (file) {
        const reader = new FileReader();
        reader.addEventListener("load", function () {
            const preview = document.getElementById(`${inp.id}-preview`);
            const old = document.getElementById(`${inp.id}-old`);
            if (old) old.style.display = 'none';
            preview.setAttribute("src", this.result);
            preview.style.display = 'block';
        });
        reader.readAsDataURL(file);
    }
});
});

const photoInput = document.getElementById('AdditionalPhotos');
const galleryContainer = document.getElementById('additional-images');
if (photoInput)
photoInput.addEventListener('change', function () {
    galleryContainer.innerHTML = '';
    const files = Array.from(this.files);
    const width = (galleryContainer.clientWidth-1) / Math.ceil(Math.sqrt(files.length));
    files.forEach((file) => {
        if (file) {
            const reader = new FileReader();
            reader.addEventListener("load", function () {
                const img = document.createElement('img');
                img.setAttribute("src", this.result);
                img.classList.add('multiple-photos-photo');
                img.style.width = width + 'px';
                img.style.height = width + 'px';
                galleryContainer.appendChild(img);
            });
            reader.readAsDataURL(file);
        }
    });
});