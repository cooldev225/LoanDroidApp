var data_table_calc = null;
var SavingItem = null;
var item_id = null, item_rate = null, item_times = null, item_cycle = null, item_amount = null;
jQuery(document).ready(function () {
    $('.wantlend-SavingItems').each(function () {
        SavingItem = $(this); 
        item_id = SavingItem.parent().find('.id').html();
        item_amount = SavingItem.parent().find('.amount').html();
        item_times = SavingItem.parent().find('.times').html();
        item_rate = SavingItem.parent().find('.rate').html();
        item_cycle = SavingItem.parent().find('.ocycle').html();
        SavingItem.find('li:nth-child(1)').html(
            SavingItem.find('li:nth-child(1)').html()
                .replace("RATE", item_rate)
                .replace("CYCLE", SavingItem.parent().find('.cycle').html())
                .replace("TIMES", item_times)
        );
        SavingItem.find('li:nth-child(2)').html(
            SavingItem.find('li:nth-child(2)').html()
                .replace("RATE", item_rate)
                .replace("CYCLE", SavingItem.parent().find('.cycle').html())
                .replace("TIMES", item_times)
        ); 
        var form_data = new FormData();
        form_data.append('id', item_id);
        form_data.append('amount', item_amount);
        form_data.append('interestingrate', item_rate);
        form_data.append('times', item_times);
        form_data.append('cycle', item_cycle);
        form_data.append('pagination[page]', 1);
        form_data.append('pagination[perpage]', item_times);
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
                //console.log($(this)[0].data.get("id"));
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
                var cycle_amount=bala;
                var cycle_saving=parseFloat(inte.toFixed(2));
                var total_amount=parseFloat((bala * response.data.length).toFixed(2));
                var total_rate = capi == 0 ? 0 : parseFloat((bala * response.data.length * 100 / capi - 100).toFixed(2));
                var obj=$('#kt_datatable_calc_' + $(this)[0].data.get("id")).parent().parent().parent().find('.wantlend-SavingItems');
                obj.find('li:nth-child(2)').html(
                    obj.find('li:nth-child(2)').html()
                        .replace("TOTAL_AMOUNT", total_amount+"$")
                        .replace("CYCLE_AMOUNT", cycle_amount)
                );
                obj.find('li:nth-child(3)').html(
                    obj.find('li:nth-child(3)').html()
                        .replace("TOTAL_RATE", total_rate)
                );
            },
            error: function (response) {

            }
        });   

        data_table_calc = $('#kt_datatable_calc_' + item_id).KTDatatable({
            data: {
                type: 'remote',
                source: {
                    read: {
                        url: '/admin/getLoanCalculatorDataTable',
                        headers: {
                            'X-CSRF-Token': $('meta[name="csrf-token"]').attr('content')
                        },
                        params: {
                            amount: item_amount,
                            interestingrate: item_rate,
                            cycle: item_cycle,
                            times: item_times,
                        }
                    },
                },
                pageSize: 5,
                serverPaging: true,
                serverFiltering: true,
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
                        title: lang.saving,
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
    });
    $('.wantlend-SavingItems-invest').each(function () {
        SavingItem = $(this);
        item_id = SavingItem.parent().find('.id').html();
        var amount = $('#paid_amount_' + item_id).html().replace('<small>$</small>', '');
        item_amount = SavingItem.parent().find('.amount').html();
        item_times = SavingItem.parent().find('.times').html();
        item_rate = SavingItem.parent().find('.rate').html();
        item_cycle = SavingItem.parent().find('.ocycle').html();
        SavingItem.find('li:nth-child(1)').html(
            SavingItem.find('li:nth-child(1)').html()
                .replace("RATE", item_rate)
                .replace("CYCLE", SavingItem.parent().find('.cycle').html())
                .replace("TIMES", item_times)
        );
        SavingItem.find('li:nth-child(2)').html(
            SavingItem.find('li:nth-child(2)').html()
                .replace("RATE", item_rate)
                .replace("CYCLE", SavingItem.parent().find('.cycle').html())
                .replace("TIMES", item_times)
        );
        var form_data = new FormData();
        form_data.append('investorId', item_id);
        form_data.append('amount', amount>0?amount:item_amount);
        form_data.append('interestingrate', item_rate);
        form_data.append('times', item_times);
        form_data.append('cycle', item_cycle);
        form_data.append('pagination[page]', 1);
        form_data.append('pagination[perpage]', item_times);
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
                //console.log($(this)[0].data.get("id"));
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
                var cycle_amount = bala;
                var cycle_saving = parseFloat(inte.toFixed(2));
                var total_amount = parseFloat((bala * response.data.length).toFixed(2));
                var total_rate = capi == 0 ? 0 : parseFloat((bala * response.data.length * 100 / capi - 100).toFixed(2));
                var obj = $('#kt_datatable_invest_' + $(this)[0].data.get("investorId")).parent().parent().parent().find('.wantlend-SavingItems-invest');
                obj.find('li:nth-child(2)').html(
                    obj.find('li:nth-child(2)').html()
                        .replace("TOTAL_AMOUNT", total_amount + "$")
                        .replace("CYCLE_AMOUNT", cycle_amount)
                );
                obj.find('li:nth-child(3)').html(
                    obj.find('li:nth-child(3)').html()
                        .replace("TOTAL_RATE", total_rate)
                );
            },
            error: function (response) {

            }
        });

        data_table_calc = $('#kt_datatable_invest_' + item_id).KTDatatable({
            data: {
                type: 'remote',
                source: {
                    read: {
                        url: '/admin/getLoanCalculatorDataTable',
                        headers: {
                            'X-CSRF-Token': $('meta[name="csrf-token"]').attr('content')
                        },
                        params: {
                            amount: item_amount,
                            interestingrate: item_rate,
                            cycle: item_cycle,
                            times: item_times,
                        }
                    },
                },
                pageSize: 5,
                serverPaging: true,
                serverFiltering: true,
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
                        title: lang.saving,
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
    });
    $('.question-form').submit(function (e) {
        e.preventDefault();
    });
});
function questionAction(id,userid) {
    if ($('#message_'+id).css('display') == 'flex') {
        var form_data = new FormData();
        form_data.append('loanId', id);
        form_data.append('toUserId', userid);
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
                var html = '';
                if (response.data.length > 0) {
                    html += '<h2 class="comments-title">' + response.data.length+' '+lang.questions+'</h2>';
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
                        html+='</li>\
                            </ul>\
                        ';
                    }
                }
                $('#message_' + id + ' .comments-area').html(html);
            },
            error: function (response) {

            }
        }); 
    }
}
function submitQuestion(id) {
    if ($("#question_txt_" + id).val() == '') {
        $("#question_txt_" + id).focus();
        return;
    }
    var form_data = new FormData();
    form_data.append('_id', id);
    form_data.append('toUserId', $("#clientId_txt_" + id).val());
    form_data.append('question', $("#question_txt_" + id).val());
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
            questionAction($(this)[0].data.get("_id"), $(this)[0].data.get("toUserId"));
        },
        error: function (response) {

        }
    }); 
}
function applyNowAction(id) {
    var form_data = new FormData(); 
    form_data.append('loanId', id);
    form_data.append('amount', $('.params-' + id).find('.amount').html());
    form_data.append('savingrate', $('.params-' + id).find('.rate').html());
    form_data.append('times', $('.params-' + id).find('.times').html());
    form_data.append('cycle', $('.params-' + id).find('.ocycle').html());
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
            location.href = "/home/wantlend?tab=2";
        },
        error: function (response) {

        }
    }); 
}
function cancelInvestment(id) {
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
            location.href = "/home/wantlend?tab=1";
        },
        error: function (response) {

        }
    });
}
function milestoneAction(id) {
    if ($('#milestones_' + id).css('display') == 'flex') {
        var form_data = new FormData();
        form_data.append('investmentId', id);
        form_data.append('pagination[page]', 1);
        form_data.append('pagination[perpage]', 100000);
        $.ajax({
            url: '/admin/getMilestoneDataTable',
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
                var id = $(this)[0].data.get("investmentId");
                var html = '<h2 class="comments-title">' + response.data.length + ' ' + lang.milestones +'</h2>';
                if (response.data.length > 0) {
                    html += '';
                    var amount = 0;
                    for (var i = 0; i < response.data.length; i++) {
                        amount += eval(response.data[i].amount);
                        html += '\
                            <div class="row" style="border-bottom: 1px solid #e8eaec;margin-bottom: 10px;padding - bottom: 10px;">\
                                <div class="col-2">\
                                    '+ getJustDateWIthYear(response.data[i].createdDate) + '\
                                </div>\
                                <div class="col-2">\
                                    '+ response.data[i].amount + '$\
                                </div>\
                                <div class="col-8">\
                                    '+ response.data[i].description + '\
                                </div>\
                            </div>\
                        ';
                    }
                }
                $('#milestones_' + id + ' .comments-area').html(html);
                $("#amount_txt_" + id).val(eval($('.iparams-' + id).find('.amount').html()) - amount);
                $("#paid_amount_" + id).html(amount + '<small>$</small>');
            },
            error: function (response) {

            }
        });
    }
}
function submitMilestone(id) {
    if ($("#amount_txt_" + id).val() == '') {
        $("#amount_txt_" + id).focus();
        return;
    }
    if (eval($("#amount_txt_" + id).val()) <=0) {
        $("#amount_txt_" + id).val('');
        $("#amount_txt_" + id).focus();
        return;
    }
    if ($("#description_txt_" + id).val() == '') {
        $("#description_txt_" + id).focus();
        return;
    }
    var form_data = new FormData();
    form_data.append('investmentId', id);
    form_data.append('amount', $("#amount_txt_" + id).val());
    form_data.append('description', $("#description_txt_" + id).val());
    $.ajax({
        url: '/admin/saveMilestone',
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
            milestoneAction($(this)[0].data.get("investmentId"));
        },
        error: function (response) {

        }
    });
}