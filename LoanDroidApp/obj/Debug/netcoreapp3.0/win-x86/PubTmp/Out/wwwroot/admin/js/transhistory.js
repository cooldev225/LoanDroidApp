var data_table = null;
function datatableInit() {
    data_table = $('#kt_datatable').KTDatatable({
        data: {
            type: 'remote',
            source: {
                read: {
                    url: '/admin/getTransactionHistoryDataTable',
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

        // columns definition
        columns:
            [
                
                {
                    field: 'fromFirstName',
                    title: '#',
                    template: function (row, index) {
                        return index + 1;
                    },
                    width: 30,
                },
                {
                    field: 'fromUserName',
                    title: lang.from,
                    width: 200,
                    template: function (row, index) {
                        return '\
                            <div class="d-flex align-items-center">\
                                <div class="symbol symbol-40 symbol-sm flex-shrink-0">\
                                    <img src="../images/default-avatar.png" onload="setImgAvatar($(this),\''+ row.fromAvatarImage + '\');" alt="photo">\
                                </div>\
                                <div class="ml-4">\
                                    <div class="text-dark-75 font-weight-bolder font-size-lg mb-0">'+ row.fromFriendlyName + '</div>\
                                    <a href="#" class="text-muted font-weight-bold text-hover-primary">'+ row.fromEmail + '</a>\
                                </div>\
                            </div>\
                        ';
                    }
                },
                {
                    field: 'fromFriendName',
                    title: lang.fromOption,
                },
                {
                    field: 'amount',
                    title: lang.amount,
                },
                {
                    field: 'toUserName',
                    title: lang.to,
                    width: 200,
                    template: function (row, index) {
                        return '\
                            <div class="d-flex align-items-center">\
                                <div class="symbol symbol-40 symbol-sm flex-shrink-0">\
                                    <img src="../images/default-avatar.png" onload="setImgAvatar($(this),\''+ row.toAvatarImage + '\');" alt="photo">\
                                </div>\
                                <div class="ml-4">\
                                    <div class="text-dark-75 font-weight-bolder font-size-lg mb-0">'+ row.toFriendlyName + '</div>\
                                    <a href="#" class="text-muted font-weight-bold text-hover-primary">'+ row.toEmail + '</a>\
                                </div>\
                            </div>\
                        ';
                    }
                },
                {
                    field: 'toFriendName',
                    title: lang.toOption,
                },
                {
                    field: 'createdDate',
                    title: lang.global_tbl_createddate,
                    template: function (row, index) {
                        return getJustDateWIthYear(row.createdDate);
                    },
                },
            ],
        translate: trans_pagination,
    });
}
function setImgAvatar(obj,img) {
    $(obj).prop('src', 'data:image/png;base64,' + img);
}
jQuery(document).ready(function () {
    datatableInit();
});
function setAvatar(img) {
    $('#avatar_img').prop('src', 'data:image/png;base64,' + img);
}
