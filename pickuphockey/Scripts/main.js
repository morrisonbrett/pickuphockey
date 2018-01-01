$(document).ready(function () {
    $.datetimepicker.setDateFormatter({
        parseDate: function (date, format) {
            var d = moment(date, format);
            return d.isValid() ? d.toDate() : false;
        },

        formatDate: function (date, format) {
            return moment(date).format(format);
        }
    });

    function onDateTimeSelect(e) {
        var m = moment(e);
        var f = m.format('dddd, MM/DD/YYYY, HH:mm');
        $('#SessionDate').val(f);
    }

    $('#SessionDate').datetimepicker({
        timePicker: true,
        closeOnDateSelect: true,
        format: 'dddd, MM/DD/YYYY, HH:mm',
        formatTime: 'HH:mm',
        formatDate: 'dddd, MM/DD/YYYY',
        defaultTime: '07:40',
        step: 5,
        onSelectDate: onDateTimeSelect,
        onSelectTime: onDateTimeSelect
    });
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
