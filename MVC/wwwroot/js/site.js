// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

$(function () {
    // copy short url to clip board
    $('.btn-copy').on("click", function () {
        let textToCopy = $(this).closest(".row").find('.short-url').text();
        navigator.clipboard.writeText(textToCopy);
    });
    // prevent submit when no input
    $('.create-url').on("submit", function () {
        if ($('#Url').val() == '') {
            return false;
        }
    });
});
