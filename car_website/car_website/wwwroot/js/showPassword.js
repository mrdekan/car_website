import svgCodes from "./svgCodesConst.js";
const show_pass_btns = document.getElementsByName('show-password');
show_pass_btns.forEach(btn => {
    btn.addEventListener('click', (e) => {
        let pass_field = document.getElementById(btn.getAttribute('target-id'));
        e.preventDefault();
        if (btn.hasAttribute('shown')) {
            pass_field.setAttribute('type', 'password');
            btn.removeAttribute('shown');
            btn.firstElementChild.innerHTML = svgCodes.shown;
            return;
        }
        pass_field.setAttribute('type', 'text');
        btn.setAttribute('shown', '');
        btn.firstElementChild.innerHTML = svgCodes.hidden;
    });
});
