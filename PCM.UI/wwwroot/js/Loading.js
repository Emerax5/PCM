// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.


$(function () {
    window.addEventListener('submit', logSubmit);
    function logSubmit(event) {
        var loadingText = '<span class="spinner-grow spinner-grow-sm" role="status" aria-hidden="true"></span> Cargando...';
        if ($("#btnSubmit").html() !== loadingText) {
            $("#btnSubmit").data('original-text', $("#btnSubmit").html());
            $("#btnSubmit").html(loadingText);

        }
        setTimeout(function () { alert("Solicitud ha tomado mucho tiempo, puede esperar o intentar nuevamente.");}, 60000);
    }
});