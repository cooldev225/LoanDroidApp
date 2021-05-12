var data_table = null;
var status_table = null;
function expendDetailBody(e) {
    status_table = $('<div/>').attr('id', 'kt_datatable_status_' + e.data.id).appendTo(e.detailCell).KTDatatable({
        data: {
            type: 'remote',
            source: {
                read: {
                    url: '/admin/getLoanRequestStatusDataTable',
                    headers: {
                        'X-CSRF-Token': $('meta[name="csrf-token"]').attr('content')
                    },
                    params: {
                        requestId:e.data.id
                    }
                },
            },
            pageSize: 10, 
            serverPaging: true,
            serverFiltering: false,
            serverSorting: true
        },
        layout: {
            scroll: false,
            footer: false,
            header: false
        },
        sortable: false,
        pagination: true,
        columns:
            [
                {
                    field: 'userName',
                    title: lang.client,
                    width: 180,
                    template: function (row, index) {
                        return '\
                            <div class="d-flex align-items-center">\
                                <div class="symbol symbol-40 symbol-sm flex-shrink-0">\
                                    <img src="../images/default-avatar.png" onload="setImgAvatar($(this),\''+ row.byAvatarImage + '\');" alt="photo">\
                                </div>\
                                <div class="ml-4">\
                                    <div class="text-dark-75 font-weight-bolder font-size-lg mb-0">'+ row.byFriendlyName + '</div>\
                                    <a href="#" class="text-muted font-weight-bold text-hover-primary">'+ row.byUserName + '</a>\
                                </div>\
                            </div>\
                        ';
                    }
                },{
                    field: 'status',
                    title: lang.status,
                    width: 180,
                    template: function (row, index) {
                        var c = "";
                        if (row.status == 0) c = lang.Contactor_Checking;
                        else if (row.status == 1) c = lang.Contactor_Rejected;
                        else if (row.status == 2) c = lang.Debug_Processing;
                        else if (row.status == 3) c = lang.Debug_rejected;
                        else if (row.status == 4) c = lang.Collection_Processing;
                        else if (row.status == 5) c = lang.Investor_Piad;
                        else if (row.status == 6) c = lang.Interesting_Process;
                        else if (row.status == 7) c = lang.Interesting_completed;
                        else if (row.status == 8) c = lang.Interesting_Incompleted;
                        return '\
                            <span>\
                                <div class="font-weight-bolder font-size-lg mb-0">' + c + '</div>\
                            </span>\
                        ';
                    },
                },
                {
                    field: 'statusReason',
                    title: lang.statusReason,
                },
                {
                    field: 'createdDate',
                    title: lang.global_tbl_createddate,
                    width:80,
                    template: function (row, index) {
                        return '\
                                <span>\
                                    <div class="font-weight-bold text-muted">' + getJustDateWIthYear(row.createdDate) + '</div>\
                                </span>\
                            ';
                    },
                }, {
                    field: 'actions',
                    title: lang.global_tbl_action,
                    sortable: false,
                    overflow: 'visible',
                    autoHide: false,
                    width: 60,
                    template: function (row) {
                        return '\
                            <div class="dropdown dropdown-inline">\
                            <a onclick="deleteLoanRequestStatus(\'' + row.id + '\');" class="btn btn-sm btn-clean btn-icon" title="delete loan request status">\
                                <span class="svg-icon svg-icon-md" style="margin-top: -5px;"><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1"><g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd"><rect x="0" y="0" width="24" height="24"></rect><path d="M6,8 L6,20.5 C6,21.3284271 6.67157288,22 7.5,22 L16.5,22 C17.3284271,22 18,21.3284271 18,20.5 L18,8 L6,8 Z" fill="#000000" fill-rule="nonzero"></path><path d="M14,4.5 L14,4 C14,3.44771525 13.5522847,3 13,3 L11,3 C10.4477153,3 10,3.44771525 10,4 L10,4.5 L5.5,4.5 C5.22385763,4.5 5,4.72385763 5,5 L5,5.5 C5,5.77614237 5.22385763,6 5.5,6 L18.5,6 C18.7761424,6 19,5.77614237 19,5.5 L19,5 C19,4.72385763 18.7761424,4.5 18.5,4.5 L14,4.5 Z" fill="#000000" opacity="0.3"></path></g></svg></span>\
                            </a>'+
                            '</div>\
                        ';
                    },
                }
            ],
        translate: trans_pagination,
    });
}
function datatableInit() {
    data_table = $('#kt_datatable').KTDatatable({
        data: {
            type: 'remote',
            source: {
                read: {
                    url: '/admin/getLoanRequestsDataTable',
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
            footer: true,
        },
        sortable: true,
        pagination: true,

        search: {
            input: $('#kt_datatable_search_query'),
            key: 'generalSearch'
        },
        columns:
            [
                {
                    field: 'id',
                    title: '',
                    width: 20,
                },
                {
                    field: 'clientid',
                    title: '#',
                    template: function (row, index) {
                        return index + 1;
                    },
                    width: 30,
                },
                {
                    field: 'userName',
                    title: lang.client,
                    width: 200,
                    template: function (row, index) {
                        return '\
                            <div class="d-flex align-items-center">\
                                <div class="symbol symbol-40 symbol-sm flex-shrink-0">\
                                    <img data-toggle="modal" data-target="#viewUserModal" onclick="viewUser(\''+ row.clientId +'\');" style="cursor: pointer;" src="../images/default-avatar.png" onload="setImgAvatar($(this),\''+ row.avatarImage + '\');" alt="photo"></a>\
                                </div>\
                                <div class="ml-4">\
                                    <div data-toggle="modal" data-target="#viewUserModal" class="text-dark-75 font-weight-bolder font-size-lg mb-0" style="cursor: pointer;" onclick="viewUser(\''+ row.clientId +'\');">'+ row.friendlyName + '</div>\
                                    <a data-toggle="modal" data-target="#viewUserModal" href="#" onclick="viewUser(\''+ row.clientId +'\');" class="text-muted font-weight-bold text-hover-primary">'+ row.userName + '</a>\
                                </div>\
                            </div>\
                        ';
                    }
                },
                {
                    field: 'amount',
                    title: lang.amount,
                    width: 100,
                    template: function (row, index) {
                        return '\
                            <span>\
                                <div class="font-weight-bolder font-size-lg mb-0">' + row.amount + '</div>\
                                <div class="font-weight-bold text-muted">' + row.interestingRate + '%</div>\
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
                        if (row.status == 0) c = lang.Contactor_Checking;
                        else if (row.status == 1) c = lang.Contactor_Rejected;
                        else if(row.status == 2) c = lang.Debug_Processing;
                        else if (row.status == 3) c = lang.Debug_rejected;
                        else if (row.status == 4) c = lang.Collection_Processing;
                        else if (row.status == 5) c = lang.Investor_Piad;
                        else if (row.status == 6) c = lang.Interesting_Process;
                        else if (row.status == 7) c = lang.Interesting_completed;
                        else if (row.status == 8) c = lang.Interesting_Incompleted;
                        return '\
                            <span>\
                                <div class="font-weight-bolder font-size-lg mb-0">' + c + '</div>\
                                <div class="font-weight-bold text-muted">' + (row.statusReason==null?"":row.statusReason) + '</div>\
                            </span>\
                        ';
                    },
                },{
                    field: 'actions',
                    title: lang.global_tbl_action,
                    sortable: false,
                    overflow: 'visible',
                    autoHide: false,
                    width: 140,
                    template: function (row) {
                        return '\
                            <div class="dropdown dropdown-inline">\
                            <a data-toggle="modal" data-target="#editLoanModal" href="javascript:;" onclick="\
                            $(\'#edit_loan_id\').val(\''+ row.id + '\');\
                            $(\'#edit_loan_clientid\').val(\''+ row.clientId + '\');\
                            $(\'#edit_loan_requesteddate\').val(\''+ getJustDateWIthYear(row.requestedDate) + '\');\
                            $(\'#edit_loan_amount\').val(\''+ row.amount + '\');\
                            $(\'#edit_loan_interestingrate\').val(\''+ row.interestingRate + '\');\
                            $(\'#edit_loan_times\').val(\''+ row.times + '\');\
                            $(\'#edit_user_cycle\').val(\'' + row.cycle + '\');\
                            calculateLoan();\
                            " class="btn btn-sm btn-clean btn-icon" title="'+ lang.clients_alert_editauser + '">\
                                <span class="svg-icon svg-icon-md"><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1"><g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd"><rect x="0" y="0" width="24" height="24"></rect><path d="M8,17.9148182 L8,5.96685884 C8,5.56391781 8.16211443,5.17792052 8.44982609,4.89581508 L10.965708,2.42895648 C11.5426798,1.86322723 12.4640974,1.85620921 13.0496196,2.41308426 L15.5337377,4.77566479 C15.8314604,5.0588212 16,5.45170806 16,5.86258077 L16,17.9148182 C16,18.7432453 15.3284271,19.4148182 14.5,19.4148182 L9.5,19.4148182 C8.67157288,19.4148182 8,18.7432453 8,17.9148182 Z" fill="#000000" fill-rule="nonzero" transform="translate(12.000000, 10.707409) rotate(-135.000000) translate(-12.000000, -10.707409) "></path><rect fill="#000000" opacity="0.3" x="5" y="20" width="15" height="2" rx="1"></rect></g></svg></span>\
                            </a>'+
                            //'<a data-toggle="modal" data-target="#calculatorLoanModal" href="javascript:;" onclick="calculateLoan();" class="btn btn-sm btn-clean btn-icon" title="">\
                            //    <span class="svg-icon svg-icon-md"><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">    <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">        <rect x="0" y="0" width="24" height="24"/>        <rect fill="#000000" opacity="0.3" x="7" y="4" width="10" height="4"/>        <path d="M7,2 L17,2 C18.1045695,2 19,2.8954305 19,4 L19,20 C19,21.1045695 18.1045695,22 17,22 L7,22 C5.8954305,22 5,21.1045695 5,20 L5,4 C5,2.8954305 5.8954305,2 7,2 Z M8,12 C8.55228475,12 9,11.5522847 9,11 C9,10.4477153 8.55228475,10 8,10 C7.44771525,10 7,10.4477153 7,11 C7,11.5522847 7.44771525,12 8,12 Z M8,16 C8.55228475,16 9,15.5522847 9,15 C9,14.4477153 8.55228475,14 8,14 C7.44771525,14 7,14.4477153 7,15 C7,15.5522847 7.44771525,16 8,16 Z M12,12 C12.5522847,12 13,11.5522847 13,11 C13,10.4477153 12.5522847,10 12,10 C11.4477153,10 11,10.4477153 11,11 C11,11.5522847 11.4477153,12 12,12 Z M12,16 C12.5522847,16 13,15.5522847 13,15 C13,14.4477153 12.5522847,14 12,14 C11.4477153,14 11,14.4477153 11,15 C11,15.5522847 11.4477153,16 12,16 Z M16,12 C16.5522847,12 17,11.5522847 17,11 C17,10.4477153 16.5522847,10 16,10 C15.4477153,10 15,10.4477153 15,11 C15,11.5522847 15.4477153,12 16,12 Z M16,16 C16.5522847,16 17,15.5522847 17,15 C17,14.4477153 16.5522847,14 16,14 C15.4477153,14 15,14.4477153 15,15 C15,15.5522847 15.4477153,16 16,16 Z M16,20 C16.5522847,20 17,19.5522847 17,19 C17,18.4477153 16.5522847,18 16,18 C15.4477153,18 15,18.4477153 15,19 C15,19.5522847 15.4477153,20 16,20 Z M8,18 C7.44771525,18 7,18.4477153 7,19 C7,19.5522847 7.44771525,20 8,20 L12,20 C12.5522847,20 13,19.5522847 13,19 C13,18.4477153 12.5522847,18 12,18 L8,18 Z M7,4 L7,8 L17,8 L17,4 L7,4 Z" fill="#000000"/>    </g></svg></span>\
                            //</a > '+
                            '<a onclick="addStatusAction(\'' + row.id +'\');" class="btn btn-sm btn-clean btn-icon" title="Request Status" data-toggle="modal" data-target="#editLoanStatusModal" href="javascript:;">\
                                <span class="svg-icon svg-icon-md"><!--begin::Svg Icon | path:C:\wamp64\www\keenthemes\themes\metronic\theme\html\demo1\dist/../src/media/svg/icons\Code\Time-schedule.svg--><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">    <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">        <rect x="0" y="0" width="24" height="24"/>        <path d="M10.9630156,7.5 L11.0475062,7.5 C11.3043819,7.5 11.5194647,7.69464724 11.5450248,7.95024814 L12,12.5 L15.2480695,14.3560397 C15.403857,14.4450611 15.5,14.6107328 15.5,14.7901613 L15.5,15 C15.5,15.2109164 15.3290185,15.3818979 15.1181021,15.3818979 C15.0841582,15.3818979 15.0503659,15.3773725 15.0176181,15.3684413 L10.3986612,14.1087258 C10.1672824,14.0456225 10.0132986,13.8271186 10.0316926,13.5879956 L10.4644883,7.96165175 C10.4845267,7.70115317 10.7017474,7.5 10.9630156,7.5 Z" fill="#000000"/>        <path d="M7.38979581,2.8349582 C8.65216735,2.29743306 10.0413491,2 11.5,2 C17.2989899,2 22,6.70101013 22,12.5 C22,18.2989899 17.2989899,23 11.5,23 C5.70101013,23 1,18.2989899 1,12.5 C1,11.5151324 1.13559454,10.5619345 1.38913364,9.65805651 L3.31481075,10.1982117 C3.10672013,10.940064 3,11.7119264 3,12.5 C3,17.1944204 6.80557963,21 11.5,21 C16.1944204,21 20,17.1944204 20,12.5 C20,7.80557963 16.1944204,4 11.5,4 C10.54876,4 9.62236069,4.15592757 8.74872191,4.45446326 L9.93948308,5.87355717 C10.0088058,5.95617272 10.0495583,6.05898805 10.05566,6.16666224 C10.0712834,6.4423623 9.86044965,6.67852665 9.5847496,6.69415008 L4.71777931,6.96995273 C4.66931162,6.97269931 4.62070229,6.96837279 4.57348157,6.95710938 C4.30487471,6.89303938 4.13906482,6.62335149 4.20313482,6.35474463 L5.33163823,1.62361064 C5.35654118,1.51920756 5.41437908,1.4255891 5.49660017,1.35659741 C5.7081375,1.17909652 6.0235153,1.2066885 6.2010162,1.41822583 L7.38979581,2.8349582 Z" fill="#000000" opacity="0.3"/>    </g></svg><!--end::Svg Icon--></span>\
                            </a> '+
                            '<a onclick="deleteLoan(\'' + row.id + '\');" class="btn btn-sm btn-clean btn-icon" title="delete loan request">\
                                <span class="svg-icon svg-icon-md" style="margin-top: -5px;"><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1"><g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd"><rect x="0" y="0" width="24" height="24"></rect><path d="M6,8 L6,20.5 C6,21.3284271 6.67157288,22 7.5,22 L16.5,22 C17.3284271,22 18,21.3284271 18,20.5 L18,8 L6,8 Z" fill="#000000" fill-rule="nonzero"></path><path d="M14,4.5 L14,4 C14,3.44771525 13.5522847,3 13,3 L11,3 C10.4477153,3 10,3.44771525 10,4 L10,4.5 L5.5,4.5 C5.22385763,4.5 5,4.72385763 5,5 L5,5.5 C5,5.77614237 5.22385763,6 5.5,6 L18.5,6 C18.7761424,6 19,5.77614237 19,5.5 L19,5 C19,4.72385763 18.7761424,4.5 18.5,4.5 L14,4.5 Z" fill="#000000" opacity="0.3"></path></g></svg></span>\
                            </a>'+
                            '</div>\
                        ';
                    },
                }
            ],
        detail: {
            title: 'Load children Accounts of Payment',
            content: expendDetailBody,
        },
        translate: trans_pagination,
    });
}
function setImgAvatar(obj,img) {
    $(obj).prop('src', 'data:image/png;base64,' + img);
}
jQuery(document).ready(function () {
    $('#edit_loan_clientid').select2();
    $('#edit_loan_requesteddate').datepicker({
        rtl: KTUtil.isRTL(),
        todayHighlight: true,
        orientation: 'bottom left',
        format: "dd/mm/yyyy",
    });
    datatableInit();
    datatableCalcInit();
});
function viewUser(uid) {
    var form_data = new FormData();
    form_data.append('id', uid);
    $.ajax({
        url: '/admin/getUserRow',
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
            $('#edit_user_id').val(response.user.id);
            $('#edit_user_username').val(response.user.userName);
            $('#edit_user_email').val(response.user.email);
            $('#edit_user_passport').val(response.user.passport);
            $('#edit_user_firstname').val(response.user.firstName);
            $('#edit_user_lastname').val(response.user.lastName);
            $('#edit_user_sex').val(response.user.sex);
            $('#edit_user_marital').val(response.user.marital);
            $('#edit_user_phonenumber').val(response.user.phoneNumber);
            $('#edit_user_otherphone').val(response.user.otherPhone);
            $('#edit_user_birth').val(response.user.birth);
            $('#edit_user_numdependant').val(response.user.numDependant);
            $('#edit_user_residence').val(response.user.residence);
            $('#edit_user_residenceperiod').val(response.user.residencePeriod);
            $('#edit_user_company').val(response.user.companyId);
            $('#edit_user_officenumber').val(response.user.officeNumber);
            $('#edit_user_address').val(response.user.address);
            $('#edit_user_nationality').val(response.user.nationalityId);
            $('#edit_user_province').val(response.user.provinceId);
            $('#edit_user_mothername').val(response.user.motherName);
            $('#edit_user_motherphone').val(response.user.motherPhone);
            $('#edit_user_fathername').val(response.user.fatherName);
            $('#edit_user_fatherphone').val(response.user.fatherPhone);

            $('#edit_payment_gatewayusername').val(response.payment01.gatewayUserName);
            $('#edit_payment_gatewaypassword').val(response.payment01.gatewayPassword);
            $('#edit_payment_gatewayemail').val(response.payment01.gatewayEmail);
            $('#edit_payment_gatewayurl').val(response.payment01.gatewayUrl);
            $('#edit_payment_gatewayname').val(response.payment01.gatewayName);
            $('#edit_payment_cardfirstname').val(response.payment02.cardFirstName);
            $('#edit_payment_cardlastname').val(response.payment02.cardLastName);
            $('#edit_payment_cardnumber').val(response.payment02.cardNumber);
            $('#edit_payment_cardexpirationdate').val(response.payment02.cardExpirationDate);
            $('#edit_payment_cardaddress1').val(response.payment02.cardAddress1);
            $('#edit_payment_cardaddress2').val(response.payment02.cardAddress2);
            $('#edit_payment_bankcountry').val(response.payment03.bankCountry);
            $('#edit_payment_bankcurrency').val(response.payment03.bankCurrency);
            $('#edit_payment_bankroutingnumber').val(response.payment03.bankRoutingNumber);
            $('#edit_payment_bankaccountholder').val(response.payment03.bankAccountHolder);
            $('#edit_payment_bankname').val(response.payment03.bankName);
            $('#edit_payment_bankstreet').val(response.payment03.bankStreet);
            $('#edit_payment_bankcity').val(response.payment03.bankCity);
            $('#edit_payment_bankregion').val(response.payment03.bankRegion);
            $('#edit_payment_bankswiftbicnumber').val(response.payment03.bankSwiftBicNumber);
            $('#edit_payment_bankibannumber').val(response.payment03.bankIBANNumber);
            //$('#avatar_img').prop('src');
        },
        error: function (response) {

        }
    });
}
function exportExcel() {
    window.open('/document/exportDocumentDataTable' +
        '?status=' + $('#kt_datatable_search_status').val() +
        '&q_start=' + encodeIvanFormat($('#kt_datatable_search_start').val()) + ' 00:00:00' +
        '&q_end=' + encodeIvanFormat($('#kt_datatable_search_end').val()) + ' 23:59:59' +
        '&q_user=' + $('#kt_datatable_search_user').val() +
        '&finished_status=' + $('#kt_datatable_search_finished').val() +
        '&q_business=' + $('#kt_datatable_search_business').val() +
        '&q_project=' + $('#kt_datatable_search_project').val() +
        '&q_group=' + $('#kt_datatable_search_group').val() +
        '&q_subgroup=' + $('#kt_datatable_search_subgroup').val());
}
function LoanAddAction() {
    $('#edit_loan_id').val('');
    $('#edit_loan_requesteddate').val('');
    $('#edit_loan_amount').val('');
    $('#edit_loan_interestingrate').val('');
    $('#edit_loan_cycle').val('0');
    $('#edit_loan_times').val('');
    calculateLoan();
}
function isValidateEditLoanModal() {
    if ($('#edit_loan_requesteddate').val() == '') {
        $('#edit_loan_requesteddate').focus();
        return false;
    }
    if ($('#edit_loan_amount').val() == '') {
        $('#edit_loan_amount').focus();
        return false;
    }
    if ($('#edit_loan_interestingrate').val() == '') {
        $('#edit_loan_interestingrate').focus();
        return false;
    }
    if ($('#edit_loan_times').val() == '') {
        $('#edit_loan_times').focus();
        return false;
    }
    return true;
}
function submitLoanEditForm() {
    if (!isValidateEditLoanModal()) return;
    var form_data = new FormData();
    form_data.append('id', $('#edit_loan_id').val());
    form_data.append('clientid', $('#edit_loan_clientid').val());
    form_data.append('requesteddate', encodeIvanFormat($('#edit_loan_requesteddate').val()));
    form_data.append('amount', $('#edit_loan_amount').val());
    form_data.append('interestingrate', $('#edit_loan_interestingrate').val());
    form_data.append('times', $('#edit_loan_times').val());
    form_data.append('cycle', $('#edit_loan_cycle').val());
    $.ajax({
        url: '/admin/saveLoanRequest',
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
            $('#loan_model_close_btn').trigger('click');
        },
        error: function (response) {

        }
    });
}
function deleteLoan(id) {
    var form_data = new FormData();
    form_data.append('id', id);
    $.ajax({
        url: '/admin/deleteLoanRequest',
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

var calculatorJson = null;
var data_table_calc = null;
function datatableCalcInit() {
    data_table_calc = $('#kt_datatable_calc').KTDatatable({
        data: {
            type: 'remote',
            source: {
                read: {
                    url: '/admin/getLoanCalculatorDataTable',
                    headers: {
                        'X-CSRF-Token': $('meta[name="csrf-token"]').attr('content')
                    },
                    params: {
                        amount: $('#edit_loan_amount').val(),
                        interestingrate: $('#edit_loan_interestingrate').val(),
                        requesteddate: encodeIvanFormat($('#edit_loan_requesteddate').val()),
                        cycle: $('#edit_loan_cycle').val(),
                        times: $('#edit_loan_times').val(),
                    }
                },
            },
            pageSize: 10,
            serverPaging: true,
            serverFiltering: false,
            serverSorting: true
        },
        layout: {
            scroll: true,
            footer: true,
        },
        sortable: false,
        pagination: true,
        columns:
        [
            {
                field: 'date',
                title: lang.date,
                template: function (row, index) {
                    return getJustDateWIthYear(row.date);
                },
                //width:200,
            },
            {
                field: 'capital',
                title: lang.capital,
                textAlign: 'left',
                //width: 100,
            },
            {
                field: 'interest',
                title: lang.interest,
                textAlign: 'left',
                //width: 100,
            },
            {
                field: 'dues',
                title: lang.dues,
                //width: 200,
            },
            {
                field: 'balance',
                title: lang.balance,
                //width: 200,
            },
            {
                field: 'actions',
                title: lang.global_tbl_action,
                sortable: false,
                overflow: 'visible',
                autoHide: false,
                width: 160,
                template: function (row, index) {
                    return '\
                        <div class="dropdown dropdown-inline">\
                        '+ (row.status == 2 ? '<span class="datatable-cell paid">' + getJustDateWIthYear(row.paidDate) + ' ' + lang.paid + '</span>' :
                            '') +
                        '</div>\
                    ';
                },
            }
        ],
        translate: trans_pagination,
    });
}
function calculateLoan() {
    data_table_calc.setDataSourceParam('amount', $('#edit_loan_amount').val());
    data_table_calc.setDataSourceParam('interestingrate', $('#edit_loan_interestingrate').val());
    data_table_calc.setDataSourceParam('requesteddate', encodeIvanFormat($('#edit_loan_requesteddate').val()));
    data_table_calc.setDataSourceParam('cycle', $('#edit_loan_cycle').val());
    data_table_calc.setDataSourceParam('times', $('#edit_loan_times').val());
    data_table_calc.reload();
}
function addStatusAction(id) {
    $('#edit_loan_id').val(id);
    $('#edit_loan_statusreason').val('');
}
function submitLoanStatusEditForm() {
    var form_data = new FormData();
    form_data.append('requestid', $('#edit_loan_id').val());
    form_data.append('status', $('#edit_loan_status').val());
    form_data.append('statusreason', $('#edit_loan_statusreason').val());
    $.ajax({
        url: '/admin/saveLoanRequestStatus',
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
            $('#loanstatus_model_close_btn').trigger('click');
        },
        error: function (response) {

        }
    });
}

function deleteLoanRequestStatus(id) {
    var form_data = new FormData();
    form_data.append('id', id);
    $.ajax({
        url: '/admin/deleteLoanRequestStatus',
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