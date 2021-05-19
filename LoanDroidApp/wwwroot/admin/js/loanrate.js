function isValidateEditRateModal() {
    $('.input-text').each(function () {
        if ($(this).val() == '' ||!$.isNumeric($(this).val())|| eval($(this).val()) <= 0 || eval($(this).val()) >=100) {
            $(this).val('');
            $(this).focus();
            return false;
        }    
    });    
    return true;
}
var form_data = null;
function submitRateEditForm() {
    if (!isValidateEditRateModal()) return;
    var form_data = new FormData();
    $('.input-text').each(function () {
        form_data.append($(this).prop('id').replace('edit_rate_',''), $(this).val());
    });    
    $.ajax({
        url: '/admin/saveloanRate',
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
            var $toast = toastr["success"](lang.success_saved, "ok");
        },
        error: function (response) {

        }
    });
}
