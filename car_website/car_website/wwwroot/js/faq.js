const faqs = document.querySelectorAll('.faq_question-btn');
faqs.forEach((el) => {
    el.addEventListener('click', () => {
        el.parentElement.classList.toggle('open');
    });
});