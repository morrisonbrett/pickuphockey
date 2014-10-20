$(document).ready(function () {
    $('#SessionDate').datepicker({ dateFormat: "DD, mm/dd/yy" });
});

$(document).ready(function () {
    $("[id^=PaymentSent_]").change(function () {
        var checked = $(this).is(':checked');
        var id = $(this).attr('id').replace(/PaymentSent_/, '');
        var data = { id: id, paymentSent: checked };

        $.ajax({
            type: "POST",
            url: "/BuySells/TogglePaymentSent",
            data: data,
            dataType: "json",
            success: function (result) {
                location.reload();
            }
        });
    });

    $("[id^=PaymentReceived_]").change(function () {
        var checked = $(this).is(':checked');
        var id = $(this).attr('id').replace(/PaymentReceived_/, '');
        var data = { id: id, paymentReceived: checked };

        $.ajax({
            type: "POST",
            url: "/BuySells/TogglePaymentReceived",
            data: data,
            dataType: "json",
            success: function (result) {
                location.reload();
            }
        });
    });

    $("[id^=UserActive_]").change(function () {
        var checked = $(this).is(':checked');
        var id = $(this).attr('id').replace(/UserActive_/, '');
        var data = { id: id, active: checked };

        $.ajax({
            type: "POST",
            url: "/Users/ToggleActive",
            data: data,
            dataType: "json",
            success: function (result) {
                $("[id^=ActiveMessage_" + id + "]").html(result.Message);
            }
        });

    });
});
