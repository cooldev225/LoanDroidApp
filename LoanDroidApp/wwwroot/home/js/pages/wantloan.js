var data_table_calc = null;
jQuery(document).ready(function () {
    $("#edit_loan_amount").on(
        "change",
        function (event, data) {
            calculateEMI();
        }
    );

    $("#edit_loan_times").on(
        "change",
        function (event, data) {
            calculateEMI();
        }
    );

    $("#edit_loan_cycle").on(
        "change",
        function (event, data) {
            $("#edit_loan_interestingrate").val($("#edit_loan_cycle ").children("option:selected").prop("class"));
            calculateEMI();
        }
    );

    $("#edit_loan_interestingrate").on(
        "change",
        function (event, data) {
            calculateEMI();
        }
    );
    datatableCalcInit();
    $("#edit_loan_interestingrate").val($("#edit_loan_cycle ").children("option:selected").prop("class"));
    calculateEMI();
    messageAction();
});
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
    if ($('#edit_loan_description').val() == '') {
        $('#edit_loan_description').focus();
        return false;
    }
    return true;
}
function submitLoanRequestForm() {
    if (!isValidateEditLoanModal()) return;
    var form_data = new FormData();
    form_data.append('id', $('#edit_loan_id').val());
    form_data.append('clientid', $('#edit_loan_clientid').val());
    form_data.append('requesteddate', $('#edit_loan_requesteddate').val());
    form_data.append('amount', $('#edit_loan_amount').val());
    form_data.append('interestingrate', $('#edit_loan_interestingrate').val());
    form_data.append('times', $('#edit_loan_times').val());
    form_data.append('cycle', $('#edit_loan_cycle').val());
    form_data.append('description', $('#edit_loan_description').val());
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
            var $toast = toastr["success"](lang.success_saved, "ok");
            calculateEMI();
        },
        error: function (response) {

        }
    });
}
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
                        requesteddate: $('#edit_loan_requesteddate').val(),
                        cycle: $('#edit_loan_cycle').val(),
                        times: $('#edit_loan_times').val(),
                        loan_id: $('#edit_loan_id').val(),
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
                }, {
                    field: 'actions',
                    title: $("#isAcceptedLoan").val() == "true" ? lang.global_tbl_action : '',
                    sortable: false,
                    overflow: $("#isAcceptedLoan").val() == "true" ? 'visible' : '',
                    autoHide: false,
                    width: 180,
                    template: function (row,index) {
                        return $("#isAcceptedLoan").val() == "false" ? '' :'\
                            <div class="dropdown dropdown-inline">\
                            '+ (row.status == 0 ? '<a onclick="javascript:paynow(' + row.capital + ',' + row.interest + ',' + row.dues + ',' + index + ');" class="datatable-cell btn btn-primary" title="Reset password">\
                                <span class="">'+ lang.paynow + '</span>\
                            </a > ': row.status==1?'\
                            <span class="datatable-cell notnow">'+ lang.notnow +'</span>\
                            ': '<span class="datatable-cell paid">' + getJustDateWIthYear(row.paidDate)+' '+lang.paid +'</span>')+
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
    data_table_calc.setDataSourceParam('requesteddate', $('#edit_loan_requesteddate').val());
    data_table_calc.setDataSourceParam('cycle', $('#edit_loan_cycle').val());
    data_table_calc.setDataSourceParam('times', $('#edit_loan_times').val());
    data_table_calc.setDataSourceParam('loan_id', $('#edit_loan_id').val());
    data_table_calc.reload();
    $(".bootstrap-select.datatable-pager-size .dropdown-menu").css('top','1200px');
}
function paynow(capital,interest,balance,index) {
    var form_data = new FormData();
    form_data.append('LoanRequestId', $('#edit_loan_id').val()); 
    form_data.append('Capital', capital);
    form_data.append('Interest', interest);
    form_data.append('Balabnce', balance);
    form_data.append('TimesNum', index);
    $.ajax({
        url: '/admin/saveLoanInterestPayment',
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
            calculateEMI();
        },
        error: function (response) {

        }
    });
}
function calculateEMI() {
    $("#cycle_div").html('\
        '+ $("#edit_loan_cycle option:selected").html()+'\
        <h2 id="cycle_lab" class="mb-0"></h2>\
    ');
    if (!isValidateEditLoanModal()) return;
    var form_data = new FormData();
    form_data.append('requesteddate', $('#edit_loan_requesteddate').val());
    form_data.append('amount', $('#edit_loan_amount').val());
    form_data.append('interestingrate', $('#edit_loan_interestingrate').val());
    form_data.append('times', $('#edit_loan_times').val());
    form_data.append('cycle', $('#edit_loan_cycle').val());
    form_data.append('pagination[page]', 1);
    form_data.append('pagination[perpage]', $('#edit_loan_times').val());
    $.ajax({
        url: '/admin/getLoanCalculatorDataTable',
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
            var inte = 0;
            var bala = 0;
            var capi = 0;
            for (var i = 0; i < response.data.length; i++) {
                if (i == 0) {
                    bala = eval(response.data[i].dues);
                    capi = eval(response.data[i].balance) + eval(response.data[i].capital);
                }
                inte += eval(response.data[i].interest);
            }
            $("#cycle_lab").html(bala);
            $("#interest_lab").html(parseFloat(inte.toFixed(2)));
            $("#balance_lab").html(parseFloat((bala * response.data.length).toFixed(2)));
            $("#percentage_lab").html(capi==0?0:parseFloat((bala * response.data.length * 100 / capi - 100).toFixed(2)));
        },
        error: function (response) {

        }
    });
    calculateLoan();
}


function messageAction() {
    var form_data = new FormData();
    form_data.append('loanId', $('#edit_loan_id').val());
    form_data.append('toUserId', $('#edit_loan_clientid').val());
    form_data.append('pagination[page]', 1);
    form_data.append('pagination[perpage]', 100000);
    $.ajax({
        url: '/admin/getMessageDataTable',
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
            var id = $(this)[0].data.get("loanId");
            var html = '<h2 class="comments-title">' + response.data.length + ' ' + lang.questions +'</h2>';
            if (response.data.length > 0) {
                html += '';
                for (var i = 0; i < response.data.length; i++) {
                    html += '\
                            <ul class="comment-list listnone">\
                                <li class="comment">\
                                    <div class="comment-body mb30">\
                                        <div class="">\
                                            <div class="comment-author">\
                                                <img style="width:100px;height:100px;" src="data:image/png;base64,'+ response.data[i].fromAvatarImage + '" class="img-fluid" />\
                                            </div>\
                                            <div class="comment-info">\
                                                <div class="comment-header">\
                                                    <h4 class="user-title">'+ response.data[i].fromFriendlyName + '</h4>\
                                                    <div class="comment-meta">\
                                                        <span class="comment-meta-date">'+ getJustDateWIthYear(response.data[i].createdDate) + '</span>\
                                                    </div>\
                                                </div>\
                                                <div class="comment-content">\
                                                    <p>'+ response.data[i].question + '</p>\
                                                </div>\
                                            </div>\
                                        </div>\
                                    </div>';
                    if (response.data[i].answer != '' && response.data[i].answer != null)
                        html += '\
                                    <ul class="childern listnone">\
                                        <li class="comment">\
                                            <div class="comment-body mb30">\
                                                <div class="">\
                                                    <div class="comment-author">\
                                                        <img style="width:100px;height:100px;" src="data:image/png;base64,'+ response.data[i].toAvatarImage + '" class="img-fluid" />\
                                                    </div>\
                                                    <div class="comment-info">\
                                                        <div class="comment-header">\
                                                            <h4 class="user-title">'+ response.data[i].toFriendlyName + '</h4>\
                                                            <div class="comment-meta">\
                                                                <span class="comment-meta-date">'+ getJustDateWIthYear(response.data[i].updatedDate) + '</span>\
                                                            </div>\
                                                        </div>\
                                                        <div class="comment-content">\
                                                            <p>'+ response.data[i].answer + '</p>\
                                                        </div>\
                                                    </div>\
                                                </div>\
                                            </div>\
                                        </li>\
                                    </ul>\
                        ';
                    else
                        html += '\
                                    <ul class="childern listnone">\
                                        <li class="comment">\
                                            <div class="comment-body mb30">\
                                                <div class="">\
                                                    <div class="comment-author">\
                                                        <img style="width:100px;height:100px;" src="data:image/png;base64,'+ response.data[i].toAvatarImage + '" class="img-fluid" />\
                                                    </div>\
                                                    <div class="comment-info">\
                                                        <div class="comment-header">\
                                                            <h4 class="user-title">'+ response.data[i].toFriendlyName + '</h4>\
                                                            <div class="comment-meta">\
                                                                <span class="comment-meta-date">'+ (new Date()).getDay() + '/' + (new Date()).getMonth() + '/' + (new Date()).getFullYear() + '</span>\
                                                                <textarea id="answer_txt_'+ response.data[i].id +'" class="form-control" rows="6" style="margin-top: 20px;"></textarea>\
                                                                <button type="button" onclick="submitAnswer('+ response.data[i].id+');" class="btn btn-primary">'+lang.reply+'</button>\
                                                            </div>\
                                                        </div>\
                                                    </div>\
                                                </div>\
                                            </div>\
                                        </li>\
                                    </ul>\
                        ';
                    html += '</li>\
                            </ul>\
                        ';
                }
            } 
            $('#message_area .comments-area').html(html);
        },
        error: function (response) {

        }
    }); 
}
function submitAnswer(id) {
    if ($("#answer_txt_" + id).val() == '') {
        $("#answer_txt_" + id).focus();
        return;
    }
    var form_data = new FormData();
    form_data.append('id', id);
    form_data.append('answer', $("#answer_txt_" + id).val());
    $.ajax({
        url: '/admin/saveMessage',
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
            messageAction();
        },
        error: function (response) {

        }
    }); 
}