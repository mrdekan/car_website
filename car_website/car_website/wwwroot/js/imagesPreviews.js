const photoInputs = document.querySelectorAll('.photoInp');
photoInputs.forEach((inp) => { 
    inp.addEventListener('change', function () {
    const file = this.files[0];
    if (file) {
        const reader = new FileReader();
        reader.addEventListener("load", function () {
            const preview = document.getElementById(`${inp.id}-preview`);
            const old = document.getElementById(`${inp.id}-old`);
            if (old)
                old.style.display = 'none';
            preview.setAttribute("src", this.result);
            preview.style.display = 'block';
        });
        reader.readAsDataURL(file);
    }
});
});