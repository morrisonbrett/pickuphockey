$(document).ready(function () {
    $('#SessionDate').datepicker({ dateFormat: "DD, mm/dd/yy" });
});

$(document).ready(function () {
    $("[id^=PaymentSent_]").change(function () {
        var checked = $(this).is(':checked');
    });

    $("[id^=PaymentReceived_]").change(function () {
        var checked = $(this).is(':checked');
    });
});
