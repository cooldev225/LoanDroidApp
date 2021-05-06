var data_table = null;
var status_table = null;
function expendDetailBody(e) {
    status_table = $('<div/>').attr('id', 'kt_datatable_status_' + e.data.id).appendTo(e.detailCell).KTDatatable({
        data: {
            type: 'remote',
            source: {
                read: {
                    url: '/admin/getInvestmentStatusDataTable',
                    headers: {
                        'X-CSRF-Token': $('meta[name="csrf-token"]').attr('content')
                    },
                    params: {
                        investmentId:e.data.id
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
                        if (row.status == 0) c = "New";
                        else if (row.status == 1) c = "Service_Processing";
                        else if (row.status == 2) c = "Service_rejected";
                        else if (row.status == 3) c = "Created_Milestone";
                        else if (row.status == 4) c = "Completed_Payment";
                        else if (row.status == 5) c = "Debug_Processing";
                        else if (row.status == 6) c = "Debug_rejected";
                        else if (row.status == 7) c = "Collection_Processing";
                        else if (row.status == 8) c = "Collection_Error";
                        else if (row.status == 9) c = "Saving_Process";
                        else if (row.status == 10) c = "Incompleted_Investment";
                        else if (row.status == 11) c = "Completed_Investment";
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
                            <a onclick="deleteInvestmentStatus(\'' + row.id + '\');" class="btn btn-sm btn-clean btn-icon" title="delete investment status">\
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
                    field: 'investorid',
                    title: '#',
                    template: function (row, index) {
                        return index + 1;
                    },
                    width: 30,
                },
                {
                    field: 'userName',
                    title: lang.investor,
                    width: 200,
                    template: function (row, index) {
                        return '\
                            <div class="d-flex align-items-center">\
                                <div class="symbol symbol-40 symbol-sm flex-shrink-0">\
                                    <img src="../images/default-avatar.png" onload="setImgAvatar($(this),\''+ row.avatarImage + '\');" alt="photo">\
                                </div>\
                                <div class="ml-4">\
                                    <div class="text-dark-75 font-weight-bolder font-size-lg mb-0">'+ row.friendlyName + '</div>\
                                    <a href="#" class="text-muted font-weight-bold text-hover-primary">'+ row.userName + '</a>\
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
                        if (row.cycle == 0) c= "Weekly";
                        else if (row.cycle == 1) c = "Monthly";
                        else if (row.cycle == 2) c = "Quarter";
                        else if (row.cycle == 3) c = "HalfOfYear";
                        else if (row.cycle == 4) c = "Annual";
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
                        if (row.status == 0) c = "New";
                        else if (row.status == 1) c = "Service_Processing";
                        else if (row.status == 2) c = "Service_rejected";
                        else if (row.status == 3) c = "Created_Milestone";
                        else if (row.status == 4) c = "Completed_Payment";
                        else if (row.status == 5) c = "Debug_Processing";
                        else if (row.status == 6) c = "Debug_rejected";
                        else if (row.status == 7) c = "Collection_Processing";
                        else if (row.status == 8) c = "Collection_Error";
                        else if (row.status == 9) c = "Saving_Process";
                        else if (row.status == 10) c = "Incompleted_Investment";
                        else if (row.status == 11) c = "Completed_Investment";
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
                            <a data-toggle="modal" data-target="#editInvestModal" href="javascript:;" onclick="\
                            $(\'#edit_invest_id\').val(\''+ row.id + '\');\
                            $(\'#edit_invest_investorid\').val(\''+ row.investorId + '\');\
                            $(\'#edit_invest_requesteddate\').val(\''+ getJustDateWIthYear(row.requestedDate) + '\');\
                            $(\'#edit_invest_amount\').val(\''+ row.amount + '\');\
                            $(\'#edit_invest_savingrate\').val(\''+ row.savingRate + '\');\
                            $(\'#edit_invest_times\').val(\''+ row.times + '\');\
                            $(\'#edit_user_cycle\').val(\'' + row.cycle + '\');\
                            calculateLoan();\
                            " class="btn btn-sm btn-clean btn-icon" title="'+ lang.clients_alert_editauser + '">\
                                <span class="svg-icon svg-icon-md"><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1"><g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd"><rect x="0" y="0" width="24" height="24"></rect><path d="M8,17.9148182 L8,5.96685884 C8,5.56391781 8.16211443,5.17792052 8.44982609,4.89581508 L10.965708,2.42895648 C11.5426798,1.86322723 12.4640974,1.85620921 13.0496196,2.41308426 L15.5337377,4.77566479 C15.8314604,5.0588212 16,5.45170806 16,5.86258077 L16,17.9148182 C16,18.7432453 15.3284271,19.4148182 14.5,19.4148182 L9.5,19.4148182 C8.67157288,19.4148182 8,18.7432453 8,17.9148182 Z" fill="#000000" fill-rule="nonzero" transform="translate(12.000000, 10.707409) rotate(-135.000000) translate(-12.000000, -10.707409) "></path><rect fill="#000000" opacity="0.3" x="5" y="20" width="15" height="2" rx="1"></rect></g></svg></span>\
                            </a>'+
                            '<a onclick="addStatusAction(\'' + row.id +'\');" class="btn btn-sm btn-clean btn-icon" title="Request Status" data-toggle="modal" data-target="#editInvestStatusModal" href="javascript:;">\
                                <span class="svg-icon svg-icon-md"><!--begin::Svg Icon | path:C:\wamp64\www\keenthemes\themes\metronic\theme\html\demo1\dist/../src/media/svg/icons\Code\Time-schedule.svg--><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">    <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">        <rect x="0" y="0" width="24" height="24"/>        <path d="M10.9630156,7.5 L11.0475062,7.5 C11.3043819,7.5 11.5194647,7.69464724 11.5450248,7.95024814 L12,12.5 L15.2480695,14.3560397 C15.403857,14.4450611 15.5,14.6107328 15.5,14.7901613 L15.5,15 C15.5,15.2109164 15.3290185,15.3818979 15.1181021,15.3818979 C15.0841582,15.3818979 15.0503659,15.3773725 15.0176181,15.3684413 L10.3986612,14.1087258 C10.1672824,14.0456225 10.0132986,13.8271186 10.0316926,13.5879956 L10.4644883,7.96165175 C10.4845267,7.70115317 10.7017474,7.5 10.9630156,7.5 Z" fill="#000000"/>        <path d="M7.38979581,2.8349582 C8.65216735,2.29743306 10.0413491,2 11.5,2 C17.2989899,2 22,6.70101013 22,12.5 C22,18.2989899 17.2989899,23 11.5,23 C5.70101013,23 1,18.2989899 1,12.5 C1,11.5151324 1.13559454,10.5619345 1.38913364,9.65805651 L3.31481075,10.1982117 C3.10672013,10.940064 3,11.7119264 3,12.5 C3,17.1944204 6.80557963,21 11.5,21 C16.1944204,21 20,17.1944204 20,12.5 C20,7.80557963 16.1944204,4 11.5,4 C10.54876,4 9.62236069,4.15592757 8.74872191,4.45446326 L9.93948308,5.87355717 C10.0088058,5.95617272 10.0495583,6.05898805 10.05566,6.16666224 C10.0712834,6.4423623 9.86044965,6.67852665 9.5847496,6.69415008 L4.71777931,6.96995273 C4.66931162,6.97269931 4.62070229,6.96837279 4.57348157,6.95710938 C4.30487471,6.89303938 4.13906482,6.62335149 4.20313482,6.35474463 L5.33163823,1.62361064 C5.35654118,1.51920756 5.41437908,1.4255891 5.49660017,1.35659741 C5.7081375,1.17909652 6.0235153,1.2066885 6.2010162,1.41822583 L7.38979581,2.8349582 Z" fill="#000000" opacity="0.3"/>    </g></svg><!--end::Svg Icon--></span>\
                            </a> '+
                            '<a onclick="deleteInvest(\'' + row.id + '\');" class="btn btn-sm btn-clean btn-icon" title="delete investment">\
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
    $('#edit_invest_investorid').select2();
    $('#edit_invest_requesteddate').datepicker({
        rtl: KTUtil.isRTL(),
        todayHighlight: true,
        orientation: 'bottom left',
        format: "dd/mm/yyyy",
    });
    datatableInit();
    datatableCalcInit();
});
function InvestAddAction() {
    $('#edit_invest_id').val('');
    $('#edit_invest_requesteddate').val('');
    $('#edit_invest_amount').val('');
    $('#edit_invest_savingrate').val('');
    $('#edit_invest_cycle').val('0');
    $('#edit_invest_times').val('');
    $('#kt_datatable_calc').html();
    calculateLoan();
}
function isValidateEditInvestModal() {
    if ($('#edit_invest_requesteddate').val() == '') {
        $('#edit_invest_requesteddate').focus();
        return false;
    }
    if ($('#edit_invest_amount').val() == '') {
        $('#edit_invest_amount').focus();
        return false;
    }
    if ($('#edit_invest_savingrate').val() == '') {
        $('#edit_invest_savingrate').focus();
        return false;
    }
    if ($('#edit_invest_times').val() == '') {
        $('#edit_invest_times').focus();
        return false;
    }
    return true;
}
function submitInvestEditForm() {
    if (!isValidateEditInvestModal()) return;
    var form_data = new FormData();
    form_data.append('id', $('#edit_invest_id').val());
    form_data.append('investorid', $('#edit_invest_investorid').val());
    form_data.append('requesteddate', encodeIvanFormat($('#edit_invest_requesteddate').val()));
    form_data.append('amount', $('#edit_invest_amount').val());
    form_data.append('savingrate', $('#edit_invest_savingrate').val());
    form_data.append('times', $('#edit_invest_times').val());
    form_data.append('cycle', $('#edit_invest_cycle').val());
    $.ajax({
        url: '/admin/saveInvestment',
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
            $('#invest_model_close_btn').trigger('click');
        },
        error: function (response) {

        }
    });
}
function deleteInvest(id) {
    var form_data = new FormData();
    form_data.append('id', id);
    $.ajax({
        url: '/admin/deleteInvestment',
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
                        amount: $('#edit_invest_amount').val(),
                        interestingrate: $('#edit_invest_savingrate').val(),
                        requesteddate: encodeIvanFormat($('#edit_invest_requesteddate').val()),
                        cycle: $('#edit_invest_cycle').val(),
                        times: $('#edit_invest_times').val(),
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
                title: lang.savingRate,
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
        ],
        translate: trans_pagination,
    });
}
function calculateLoan() {
    data_table_calc.setDataSourceParam('amount', $('#edit_invest_amount').val());
    data_table_calc.setDataSourceParam('interestingrate', $('#edit_invest_savingrate').val());
    data_table_calc.setDataSourceParam('requesteddate', encodeIvanFormat($('#edit_invest_requesteddate').val()));
    data_table_calc.setDataSourceParam('cycle', $('#edit_invest_cycle').val());
    data_table_calc.setDataSourceParam('times', $('#edit_invest_times').val());
    data_table_calc.reload();
}
function addStatusAction(id) {
    $('#edit_invest_id').val(id);
    $('#edit_invest_statusreason').val('');
}
function submitInvestStatusEditForm() {
    var form_data = new FormData();
    form_data.append('investmentid', $('#edit_invest_id').val());
    form_data.append('status', $('#edit_invest_status').val());
    form_data.append('statusreason', $('#edit_invest_statusreason').val());
    $.ajax({
        url: '/admin/saveInvestmentStatus',
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
            $('#investstatus_model_close_btn').trigger('click');
        },
        error: function (response) {

        }
    });
}

function deleteInvestmentStatus(id) {
    var form_data = new FormData();
    form_data.append('id', id);
    $.ajax({
        url: '/admin/deleteInvestmentStatus',
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