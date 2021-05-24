var data_table_calc = null;
function setImgAvatar(obj, img) {
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
    $('#edit_user_birth').datepicker({
        rtl: KTUtil.isRTL(),
        todayHighlight: true,
        orientation: 'bottom left',
        format: "dd/mm/yyyy",
    });
    $("#edit_user_passport").inputmask({
        mask: "999-9999999-9",
        definitions: { '5': { validator: "[0-5]" } }
    });
    //$("#edit_user_company").select2();
    //$("#edit_user_nationality").select2();
    //$("#edit_user_province").select2();
    datatableLoansInit();
});
function setAvatar(img) {
    $('#avatar_img').prop('src', 'data:image/png;base64,' + img);
}
function isValidateEditUserModal() {
    if ($('#edit_user_username').val() == '') {
        $('#edit_user_username').focus();
        return false;
    }
    if ($('#edit_user_email').val() == '') {
        $('#edit_user_email').focus();
        return false;
    }
    if ($('#edit_user_passport').val() == '') {
        $('#edit_user_passport').focus();
        return false;
    }
    if ($('#edit_user_sex').val() == 0) {
        $('#edit_user_sex').focus();
        return false;
    }
    if ($('#edit_user_firstname').val() == '') {
        $('#edit_user_firstname').focus();
        return false;
    }
    if ($('#edit_user_lastname').val() == '') {
        $('#edit_user_lastname').focus();
        return false;
    }    
    if ($('#edit_user_phonenumber').val() == '') {
        $('#edit_user_phonenumber').focus();
        return false;
    } 
    if ($('#edit_user_birth').val() == '') {
        $('#edit_user_birth').focus();
        return false;
    }
    if ($('#edit_user_numdependant').val() == '') {
        $('#edit_user_numdependant').focus();
        return false;
    }
    if ($('#edit_user_mothername').val() == '') {
        $('#edit_user_mothername').focus();
        return false;
    }
    if ($('#edit_user_fathername').val() == '') {
        $('#edit_user_fathername').focus();
        return false;
    }
    return true;
}
function submitProfile() {
    if (!isValidateEditUserModal()) return;
    var form_data = new FormData();
    form_data.append('id', $('#edit_user_id').val());
    form_data.append('username', $('#edit_user_username').val());
    form_data.append('email', $('#edit_user_email').val());
    form_data.append('passport', $('#edit_user_passport').val());
    form_data.append('firstname', $('#edit_user_firstname').val());
    form_data.append('lastname', $('#edit_user_lastname').val());
    form_data.append('sex', $('#edit_user_sex').val()); 
    form_data.append('marital', $('#edit_user_marital').val());
    form_data.append('phonenumber', $('#edit_user_phonenumber').val());
    form_data.append('otherphone', $('#edit_user_otherphone').val()); 
    form_data.append('birth', $('#edit_user_birth').val());
    form_data.append('numdependant', $('#edit_user_numdependant').val()); 
    form_data.append('residence', $('#edit_user_residence').val());
    form_data.append('residenceperiod', $('#edit_user_residenceperiod').val()); 
    form_data.append('company', $('#edit_user_company').val());
    form_data.append('officenumber', $('#edit_user_officenumber').val());
    form_data.append('address', $('#edit_user_address').val()); 
    form_data.append('nationality', $('#edit_user_nationality').val());
    form_data.append('province', $('#edit_user_province').val());
    form_data.append('mothername', $('#edit_user_mothername').val());
    form_data.append('motherphone', $('#edit_user_motherphone').val());
    form_data.append('fathername', $('#edit_user_fathername').val());
    form_data.append('fatherphone', $('#edit_user_fatherphone').val());
    form_data.append('avatarimage', $('#avatar_img').prop('src'));
    form_data.append('role', 'inversora');
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
            var $toast = toastr["success"](lang.success_saved, "ok");
            if (eval($("#step_div").prop('class')) == 0) location.reload(true);
        },
        error: function (response) {

        }
    });
}


function submitPaymentEditForm() {
    if (!isValidateEditPaymentModal()) return;
    var type = getPaymentActiveTab();
    var form_data = new FormData();
    form_data.append('id', $('#edit_payment_id' + type).val());
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
    form_data.append('type', type);
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
            var $toast = toastr["success"](lang.success_saved, "ok");
            if (eval($("#step_div").prop('class')) == 1) location.reload(true);
        },
        error: function (response) {

        }
    });
}
function isValidateEditPaymentModal() {
    type = getPaymentActiveTab();
    if (type == 0){
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


function datatableLoansInit() {
    var data_table_loans = $('#kt_datatable_loans').KTDatatable({
        data: {
            type: 'remote',
            source: {
                read: {
                    url: '/admin/getInvestmentDataTable',
                    headers: {
                        'X-CSRF-Token': $('meta[name="csrf-token"]').attr('content')
                    },
                },
            },
            pageSize: 10,
            serverPaging: true,
            serverFiltering: false,
            serverSorting: true
        },
        layout: {
            scroll: true,
            footer: false,
        },
        sortable: false,
        pagination: false,

        search: {
            input: $('#kt_datatable_search_query'),
            key: 'generalSearch'
        },
        columns:
            [
                {
                    field: 'investorId',
                    title: '#',
                    template: function (row, index) {
                        return index + 1;
                    },
                    width: 30,
                },
                {
                    field: 'amount',
                    title: lang.amount,
                    width: 100,
                    template: function (row, index) {
                        return '\
                            <span>\
                                <div class="font-weight-bolder font-size-lg mb-0">' + row.amount + '</div>\
                                <div class="font-weight-bold text-muted">' + row.savingRate + '%</div>\
                            </span>\
                        ';
                    },
                },
                {
                    field: 'loanCycle',
                    title: lang.frequently,
                    template: function (row) {
                        var c = "";
                        if (row.cycle == 0) c = lang.SEMANAL;
                        else if (row.cycle == 1) c = lang.QUINCENAL;
                        else if (row.cycle == 2) c = lang.MENSUAL;
                        else if (row.cycle == 3) c = lang.DIARIO;
                        return '<span>\
                                <div class="font-weight-bolder font-size-lg mb-0">' + c + '</div>\
                                <div class="font-weight-bold text-muted">' + row.times + ' ' + lang.times + '</div>\
                            </span>';
                    },
                },
                {
                    field: 'requestedDate',
                    title: lang.global_tbl_createddate,
                    template: function (row, index) {
                        return '\
                            <span>\
                                <div class="font-weight-bolder font-size-lg mb-0">' + getJustDateWIthYear(row.requestedDate) + '</div>\
                                <div class="font-weight-bold text-muted">' + getJustDateWIthYear(row.updatedDate) + '</div>\
                            </span>\
                        ';
                    },
                }, {
                    field: 'status',
                    title: lang.status,
                    template: function (row, index) {
                        var c = "";
                        if (row.status == 0) c = lang.New;
                        else if (row.status == 1) c = lang.Representante_Processing;
                        else if (row.status == 2) c = lang.Representante_Rejected;
                        else if (row.status == 3) c = lang.Contactor_Checking;
                        else if (row.status == 4) c = lang.Contactor_Rejected;
                        else if (row.status == 5) c = lang.Service_Processing;
                        else if (row.status == 6) c = lang.Service_Rejected;
                        else if (row.status == 7) c = lang.Debug_Processing;
                        else if (row.status == 8) c = lang.Debug_Rejected;
                        else if (row.status == 9) c = lang.Collection_Processing;
                        else if (row.status == 10) c = lang.Investor_Piad;
                        else if (row.status == 11) c = lang.Interesting_Process;
                        else if (row.status == 12) c = lang.Interesting_completed;
                        else if (row.status == 13) c = lang.Interesting_Incompleted;
                        return '\
                            <span>\
                                <div class="font-weight-bolder font-size-lg mb-0">' + c + '</div>\
                                <div class="font-weight-bold text-muted">' + (row.statusReason == null ? "" : row.statusReason) + '</div>\
                            </span>\
                        ';
                    },
                }
            ],
        translate: trans_pagination,
    });
}
