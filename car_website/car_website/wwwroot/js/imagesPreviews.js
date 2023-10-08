const photoInputs = document.getElementsByName('photoInp');
//const photoPreview = document.getElementsById('Photo1-preview');
photoInputs.forEach((inp) => { 
    inp.addEventListener('change', function () {
    const file = this.files[0];
    if (file) {
        const reader = new FileReader();
        reader.addEventListener("load", function () {
            const preview = document.getElementById(`${inp.id}-preview`);
            preview.setAttribute("src", this.result);
            preview.style.display = 'block';
        });
        reader.readAsDataURL(file);
    }
});
});