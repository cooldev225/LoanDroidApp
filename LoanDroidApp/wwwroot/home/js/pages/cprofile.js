function setImgAvatar(obj,img) {
    $(obj).prop('src', 'data:image/png;base64,' + img);
}
jQuery(document).ready(function () {
    $("#edit_user_avatar").on('change', function () {
        if (this.files && this.files[0]) {
            var FR = new FileReader();
            FR.addEventListener("load", function (e) {
                $("#avatar_img").attr('src', e.target.result);
            });
            FR.readAsDataURL(this.files[0]);
        }
    });
    $('.fileinput-delete').on('click', function () {
        $('#avatar_img').prop('src', $('#default_img').prop('src'));
    });

    $("#la").bind(
        "slider:changed",
        function (event, data) {
            $("#la_value").html(data.value.toFixed(0));
            calculateEMI();
        }
    );

    $("#nm").bind(
        "slider:changed",
        function (event, data) {
            $("#nm_value").html(data.value.toFixed(0));
            calculateEMI();
        }
    );

    $("#roi").bind(
        "slider:changed",
        function (event, data) {
            $("#roi_value").html(data.value.toFixed(2));
            calculateEMI();
        }
    );
    calculateEMI();
});
function setAvatar(img) {
    $('#avatar_img').prop('src', 'data:image/png;base64,' + img);
}
function isValidateEditUserModal() {
    if ($('#edit_user_username').val() == '') {
        $('#edit_user_username').focus();
        return false;
    }
    if ($('#edit_user_firstname').val() == '') {
        $('#edit_user_firstname').focus();
        return false;
    }
    if ($('#edit_user_email').val() == '') {
        $('#edit_user_email').focus();
        return false;
    }
    if ($('#edit_user_phonenumber').val() == '') {
        $('#edit_user_phonenumber').focus();
        return false;
    }
    return true;
}
function submitProfile() {
    if (!isValidateEditUserModal()) return;
    var form_data = new FormData();
    form_data.append('id', $('#edit_user_id').val());
    form_data.append('username', $('#edit_user_username').val());
    form_data.append('firstname', $('#edit_user_firstname').val());
    form_data.append('lastname', $('#edit_user_lastname').val());
    form_data.append('email', $('#edit_user_email').val());
    form_data.append('phonenumber', $('#edit_user_phonenumber').val());
    form_data.append('otherphone', $('#edit_user_otherphone').val());
    form_data.append('officenumber', $('#edit_user_officenumber').val());
    form_data.append('address', $('#edit_user_address').val());
    form_data.append('avatarimage', $('#avatar_img').prop('src'));
    form_data.append('role', 'cliente');
    $.ajax({
        url: '/admin/saveUser',
        headers: {
            'X-CSRF-Token': $('meta[name="csrf-token"]').attr('content')
        },
        data: form_data,
        cache: false,
        contentType: false,
        processData: false,
        type: 'POST',
        dataType: "json",
        success: function (response) {
            location.refresh = true;
        },
        error: function (response) {

        }
    });
}


function submitPaymentEditForm() {
    if (!isValidateEditPaymentModal()) return;
    var form_data = new FormData();
    form_data.append('id', $('#edit_payment_id').val());
    form_data.append('userid', $('#edit_payment_userid').val());
    form_data.append('gatewayname', $('#edit_payment_gatewayname').val());
    form_data.append('gatewayurl', $('#edit_payment_gatewayurl').val());
    form_data.append('gatewayemail', $('#edit_payment_gatewayemail').val());
    form_data.append('gatewayusername', $('#edit_payment_gatewayusername').val());
    form_data.append('gatewaypassword', $('#edit_payment_gatewaypassword').val());
    form_data.append('cardfirstname', $('#edit_payment_cardfirstname').val());
    form_data.append('cardlastname', $('#edit_payment_cardlastname').val());
    form_data.append('cardnumber', $('#edit_payment_cardnumber').val());
    form_data.append('cardexpirationdate', $('#edit_payment_cardexpirationdate').val());
    form_data.append('cardaddress1', $('#edit_payment_cardaddress1').val());
    form_data.append('cardaddress2', $('#edit_payment_cardaddress2').val());
    form_data.append('bankcountry', $('#edit_payment_bankcountry').val());
    form_data.append('bankcurrency', $('#edit_payment_bankcurrency').val());
    form_data.append('bankroutingnumber', $('#edit_payment_bankroutingnumber').val());
    form_data.append('bankaccontholder', $('#edit_payment_bankaccontholder').val());
    form_data.append('bankname', $('#edit_payment_bankname').val());
    form_data.append('bankstreet', $('#edit_payment_bankstreet').val());
    form_data.append('bankcity', $('#edit_payment_bankcity').val());
    form_data.append('bankregion', $('#edit_payment_bankregion').val());
    form_data.append('bankswiftbicnumber', $('#edit_payment_bankswiftbicnumber').val());
    form_data.append('bankibannumber', $('#edit_payment_bankibannumber').val());
    form_data.append('type', getPaymentActiveTab());
    $.ajax({
        url: '/admin/saveAccountPayment',
        headers: {
            'X-CSRF-Token': $('meta[name="csrf-token"]').attr('content')
        },
        data: form_data,
        cache: false,
        contentType: false,
        processData: false,
        type: 'POST',
        dataType: "json",
        success: function (response) {
            if (response < 0) {
                alert('This username is existen already.');
                return;
            }
            data_table.reload();
            $('#payment_model_close_btn').trigger('click');
        },
        error: function (response) {

        }
    });
}
function isValidateEditPaymentModal() {
    type = getPaymentActiveTab();
    if (type == 0) {
        if ($('#edit_payment_gatewayname').val() == '') {
            $('#edit_payment_gatewayname').focus();
            return false;
        }
        if ($('#edit_payment_gatewayurl').val() == '') {
            $('#edit_payment_gatewayurl').focus();
            return false;
        }
        if ($('#edit_payment_gatewayemail').val() == '') {
            $('#edit_payment_gatewayemail').focus();
            return false;
        }
        if ($('#edit_payment_gatewayusername').val() == '') {
            $('#edit_payment_gatewayusername').focus();
            return false;
        }
        if ($('#edit_payment_gatewaypassword').val() == '') {
            $('#edit_payment_gatewaypassword').focus();
            return false;
        }        
    } else if (type == 1) {
        if ($('#edit_payment_cardfirstname').val() == '') {
            $('#edit_payment_cardfirstname').focus();
            return false;
        }
        if ($('#edit_payment_cardlastname').val() == '') {
            $('#edit_payment_cardlastname').focus();
            return false;
        }
        if ($('#edit_payment_cardnumber').val() == '') {
            $('#edit_payment_cardnumber').focus();
            return false;
        }
        if ($('#edit_payment_cardexpirationdate').val() == '') {
            $('#edit_payment_cardexpirationdate').focus();
            return false;
        }
        if ($('#edit_payment_cardaddress1').val() == '') {
            $('#edit_payment_cardaddress1').focus();
            return false;
        }
    } else if (type == 2) {
        if ($('#edit_payment_bankcountry').val() == '') {
            $('#edit_payment_bankcountry').focus();
            return false;
        }
        if ($('#edit_payment_bankcurrency').val() == '') {
            $('#edit_payment_bankcurrency').focus();
            return false;
        }
        if ($('#edit_payment_bankroutingnumber').val() == '') {
            $('#edit_payment_bankroutingnumber').focus();
            return false;
        }
        if ($('#edit_payment_bankaccontholder').val() == '') {
            $('#edit_payment_bankaccontholder').focus();
            return false;
        }
        if ($('#edit_payment_bankname').val() == '') {
            $('#edit_payment_bankname').focus();
            return false;
        }
        if ($('#edit_payment_bankstreet').val() == '') {
            $('#edit_payment_bankstreet').focus();
            return false;
        }
        if ($('#edit_payment_bankcity').val() == '') {
            $('#edit_payment_bankcity').focus();
            return false;
        }
        if ($('#edit_payment_bankregion').val() == '') {
            $('#edit_payment_bankregion').focus();
            return false;
        }
        if ($('#edit_payment_bankswiftbicnumber').val() == '') {
            $('#edit_payment_bankswiftbicnumber').focus();
            return false;
        }
        if ($('#edit_payment_bankibannumber').val() == '') {
            $('#edit_payment_bankibannumber').focus();
            return false;
        }
    }
    return true;
}
function addPaymentAction(userid) {
    setPaymentActiveTab(0);
    $('#editPaymentModal .modal-content .card-toolbar ul li').css('display', 'block');
    $('#edit_payment_id').val('0');
    $('#edit_payment_userid').val(userid);
    $('#edit_payment_gatewayname').val('');
    $('#edit_payment_gatewayurl').val('');
    $('#edit_payment_gatewayemail').val('');
    $('#edit_payment_gatewayusername').val('');
    $('#edit_payment_gatewaypassword').val('');
    $('#edit_payment_cardfirstname').val('');
    $('#edit_payment_cardlastname').val('');
    $('#edit_payment_cardnumber').val('');
    $('#edit_payment_cardexpirationdate').val('');
    $('#edit_payment_cardaddress1').val('');
    $('#edit_payment_cardaddress2').val('');
    $('#edit_payment_bankcountry').val('');
    $('#edit_payment_bankcurrency').val('');
    $('#edit_payment_bankroutingnumber').val('');
    $('#edit_payment_bankaccontholder').val('');
    $('#edit_payment_bankname').val('');
    $('#edit_payment_bankstreet').val('');
    $('#edit_payment_bankcity').val('');
    $('#edit_payment_bankregion').val('');
    $('#edit_payment_bankswiftbicnumber').val('');
    $('#edit_payment_bankibannumber').val('');
}
function deletePaymentAction(id) {
    var form_data = new FormData();
    form_data.append('id', id);
    $.ajax({
        url: '/admin/deleteAccountPayment',
        headers: {
            'X-CSRF-Token': $('meta[name="csrf-token"]').attr('content')
        },
        data: form_data,
        cache: false,
        contentType: false,
        processData: false,
        type: 'POST',
        dataType: "json",
        success: function (response) {
            data_table.reload();
        },
        error: function (response) {

        }
    });
}
function getPaymentActiveTab() {
    if ($('#kt_tab_pane_1_2').hasClass('active')) return 0;
    if ($('#kt_tab_pane_2_2').hasClass('active')) return 1;
    if ($('#kt_tab_pane_3_2').hasClass('active')) return 2;
    return 0;
}
function setPaymentActiveTab(type) {
    $('#kt_tab_pane_1_2').removeClass('active');
    $('#kt_tab_pane_1_2').removeClass('show');
    $('#kt_tab_pane_2_2').removeClass('active');
    $('#kt_tab_pane_2_2').removeClass('show');
    $('#kt_tab_pane_3_2').removeClass('active');
    $('#kt_tab_pane_3_2').removeClass('show');
    $('#kt_tab_pane_' + (type + 1) + '_2').addClass('active');
    $('#kt_tab_pane_' + (type + 1) + '_2').addClass('show');
    $('#editPaymentModal .modal-content .card-toolbar ul li').css('display', 'none');
    $('#editPaymentModal .modal-content .card-toolbar ul li:nth-child('+(type+1)+')').css('display', 'block');
}



function calculateEMI() {
    var loanAmount = $("#la_value").html();
    var numberOfMonths = $("#nm_value").html();
    var rateOfInterest = $("#roi_value").html();
    var monthlyInterestRatio = (rateOfInterest / 100) / 12;

    var top = Math.pow((1 + monthlyInterestRatio), numberOfMonths);
    var bottom = top - 1;
    var sp = top / bottom;
    var emi = ((loanAmount * monthlyInterestRatio) * sp);
    var full = numberOfMonths * emi;
    var interest = full - loanAmount;
    var int_pge = (interest / full) * 100;
    $("#tbl_int_pge").html(int_pge.toFixed(2) + " %");
    //$("#tbl_loan_pge").html((100-int_pge.toFixed(2))+" %");

    var emi_str = emi.toFixed(2).toString().replace(/,/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    var loanAmount_str = loanAmount.toString().replace(/,/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    var full_str = full.toFixed(2).toString().replace(/,/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    var int_str = interest.toFixed(2).toString().replace(/,/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");

    $("#emi").html(emi_str);
    $("#tbl_emi").html(emi_str);
    $("#tbl_la").html(loanAmount_str);
    $("#tbl_nm").html(numberOfMonths);
    $("#tbl_roi").html(rateOfInterest);
    $("#tbl_full").html(full_str);
    $("#tbl_int").html(int_str);
    var detailDesc = "<thead><tr class='table-head'><th>Payment No.</th><th>Begining Balance</th><th>EMI</th><th>Principal</th><th>Interest</th><th>Ending Balance</th></thead><tbody>";
    var bb = parseInt(loanAmount);
    var int_dd = 0;
    var pre_dd = 0;
    var end_dd = 0;
    for (var j = 1; j <= numberOfMonths; j++) {
        int_dd = bb * ((rateOfInterest / 100) / 12);
        pre_dd = emi.toFixed(2) - int_dd.toFixed(2);
        end_dd = bb - pre_dd.toFixed(2);
        detailDesc += "<tr><td>" + j + "</td><td>" + bb.toFixed(2) + "</td><td>" + emi.toFixed(2) + "</td><td>" + pre_dd.toFixed(2) + "</td><td>" + int_dd.toFixed(2) + "</td><td>" + end_dd.toFixed(2) + "</td></tr>";
        bb = bb - pre_dd.toFixed(2);
    }
    detailDesc += "</tbody>";
    $("#loantable").html(detailDesc);

}