// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.


$(document).ready(function () {

    $('#phone').mask('(000) 000-0000');

    $('#money1').mask("000,000.00", { reverse: true });

    $('#money2').mask("000,000.00", { reverse: true });

    $('#money3').mask("000,000.00", { reverse: true });


});

function formatCurrency(selector) {
    document.querySelectorAll(selector).forEach(function (elem) {
        var amt = elem.textContent.replace("$", "");
        if (amt && amt.split(".").length <= 2) {
            var formatter = new Intl.NumberFormat('en-US', {
                style: 'currency',
                currency: 'USD',
                minimumFractionDigits: 2
            });
            amt = formatter.format(amt);
        }
        elem.textContent = amt;
    })
}

formatCurrency("#money")
