$(document).ready(function () {
    $(".InfoDelete").click(function (event) {
        if (!confirm('Do you want to delete the record?')) {
            event.preventDefault();
        }
    })
})