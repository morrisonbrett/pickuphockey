function LoadRegularSet(id) {
    var data = { id: id };

    if (id == undefined)
        return;

    var regularSetDescription = document.getElementById("RegularSetDescription");
    regularSetDescription.innerHTML = 'Loading...';

    var lr = document.getElementById("LightRegulars");
    var dr = document.getElementById("DarkRegulars");
    var em = document.getElementById("RegularSetEmails");
    lr.innerHTML = '';
    dr.innerHTML = '';
    em.innerHTML = '';

    $.ajax({
        type: "POST",
        url: "/Sessions/RegularSet",
        data: data,
        dataType: "json",
        success: function (result) {
            if (result == undefined)
                return;

            // Set the header
            regularSetDescription.innerHTML = 'Roster - ' + result[0].RegularSet.Description;

            var lrt = '';
            var drt = '';
            var ems = '';

            var lightTotal = 0.0;
            var darkTotal = 0.0;

            var lightCount = 0;
            var darkCount = 0;

            for (let i = 0; i < result.length; i++) {
                var s = '';
                s += result[i].User.FirstName;
                s += '<span> </span>';
                s += result[i].User.LastName;
                s += '<span id="ratingDisplay" display="none"><span> (</span>';
                s += result[i].User.Rating.toFixed(1);
                s += '</span><span id="ratingDisplay" display="none">)</span><span>, </span>';
                s += result[i].PositionPreference == 1 ? 'Forward' : 'Defense';
                s += '<br />'

                if (result[i].TeamAssignment == 1) {
                    lightTotal += result[i].User.Rating;
                    lightCount++;
                    lrt += s;
                } else if (result[i].TeamAssignment == 2) {
                    darkTotal += result[i].User.Rating;
                    darkCount++;
                    drt += s;
                }
                ems += result[i].User.Email + '<br />';
            }

            lrt += "<span id='ratingDisplay' display='none'><br/> <b>Total</b>: " + lightTotal.toFixed(1) + ", <b>Average</b>: " + (lightTotal / lightCount).toFixed(2) + "</span>";
            drt += "<span id='ratingDisplay' display='none'><br/> <b>Total</b>: " + darkTotal.toFixed(1) + ", <b>Average</b>: " + (darkTotal / darkCount).toFixed(2) + "</span>";

            // Replace the TD with the data
            lr.innerHTML = lrt;
            dr.innerHTML = drt;

            em.innerHTML = ems;
        }
    });
}

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

    $("[id^=UserPreferred_]").change(function () {
        var checked = $(this).is(':checked');
        var id = $(this).attr('id').replace(/UserPreferred_/, '');
        var data = { id: id, preferred: checked };

        $.ajax({
            type: "POST",
            url: "/Users/TogglePreferred",
            data: data,
            dataType: "json",
            success: function (result) {
                $("[id^=PreferredMessage_" + id + "]").html(result.Message);
            }
        });
    });

    $("[id^=TeamAssignment_]").change(function () {
        var teamAssignment = $(this).val();
        var id = $(this).attr('id').replace(/TeamAssignment_/, '');
        var data = { id: id, teamAssignment: teamAssignment };
        console.info(data);

        $.ajax({
            type: "POST",
            url: "/BuySells/UpdateTeamAssignment",
            data: data,
            dataType: "json",
            success: function (result) {
                location.reload();
            }
        });
    });

    $("[id^=SellerNoteFlagged_]").change(function () {
        var checked = $(this).is(':checked');
        var id = $(this).attr('id').replace(/SellerNoteFlagged_/, '');
        var data = { id: id, sellerNoteFlagged: checked };

        $.ajax({
            type: "POST",
            url: "/BuySells/ToggleSellerNoteFlagged",
            data: data,
            dataType: "json",
            success: function (result) {
                location.reload();
            }
        });
    });

    $("[id^=BuyerNoteFlagged_]").change(function () {
        var checked = $(this).is(':checked');
        var id = $(this).attr('id').replace(/BuyerNoteFlagged_/, '');
        var data = { id: id, buyerNoteFlagged: checked };

        $.ajax({
            type: "POST",
            url: "/BuySells/ToggleBuyerNoteFlagged",
            data: data,
            dataType: "json",
            success: function (result) {
                location.reload();
            }
        });
    });

    $("[id^=RegularSetId]").change(function () {
        var sel = document.getElementById("RegularSetId");
        var regularSetId = sel.options[sel.selectedIndex].value;

        LoadRegularSet(regularSetId);
    });
});

function setRatingsCookie() {
    const expirationTime = new Date(new Date().getTime() + 30 * 60 * 1000); // 30 minutes from now
    document.cookie = `ratings=true; expires=${expirationTime.toUTCString()}; path=/`;
}

// Function to check if the "ratings" cookie is set
function isRatingsCookieSet() {
    return document.cookie.split(';').some(item => item.trim().startsWith('ratings='));
}

// Function to delete the "ratings" cookie
function deleteRatingsCookie() {
    document.cookie = 'ratings=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;';
}
