document.addEventListener('DOMContentLoaded', function () {
    var openButtons = document.querySelectorAll('.modal-open-btn');
    var closeButtons = document.querySelectorAll('.modal-close-btn');
    var overlay = document.querySelector('.modal-overlay');

    function openModal() {
        if (overlay) overlay.classList.add('open');
    }
    function closeModal() {
        if (overlay) overlay.classList.remove('open');
    }

    openButtons.forEach(function (btn) {
        btn.addEventListener('click', function (e) { e.preventDefault(); openModal(); });
    });
    closeButtons.forEach(function (btn) {
        btn.addEventListener('click', function (e) { e.preventDefault(); closeModal(); });
    });
    if (overlay) {
        overlay.addEventListener('click', function (e) {
            if (e.target === overlay) closeModal();
        });
    }
    document.addEventListener('keydown', function (e) {
        if (e.key === 'Escape') closeModal();
    });
});