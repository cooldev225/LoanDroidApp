var data_table = null;
function datatableInit() {
    data_table = $('#kt_datatable').KTDatatable({
        data: {
            type: 'remote',
            source: {
                read: {
                    url: '/admin/getCompanyDataTable',
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
                /*{
                    field: 'id',
                    title: '',
                    width: 20,
                },*/
                {
                    field: 'id',
                    title: '#',
                    template: function (row, index) {
                        return index + 1;
                    },
                    width: 30,
                },
                {
                    field: 'name',
                    title: lang.roles_tbl_name,
                },
                {
                    field: 'purpose',
                    title: lang.purpose,
                },
                {
                    field: 'direction',
                    title: lang.direction,
                },
                {
                    field: 'phone',
                    title: lang.phone,
                },
                {
                    field: 'url',
                    title: 'URL',
                    template: function (row, index) {
                        return '\
                            <a href="#" onclick="window.open(\''+row.url+'\');">\
                                '+row.url+'\
                            </a>\
                        ';
                    },
                },{
                    field: 'actions',
                    title: $("#managepermission").val() == "true" ?lang.global_tbl_action:'',
                    sortable: false,
                    overflow: $("#managepermission").val() == "true" ?'visible':'',
                    autoHide: false,
                    width: 140,
                    template: function (row) {
                        return $("#managepermission").val() == "false" ?'':'\
                            <div class="dropdown dropdown-inline">\
                            <a data-toggle="modal" data-target="#editCompanyModal" href="javascript:;" onclick="\
                            $(\'#edit_company_id\').val(\''+ row.id + '\');\
                            $(\'#edit_company_name\').val(\''+ row.name + '\');\
                            $(\'#edit_company_purpose\').val(\''+ row.purpose + '\');\
                            $(\'#edit_company_direction\').val(\''+ row.direction + '\');\
                            $(\'#edit_company_phone\').val(\''+ row.phone + '\');\
                            $(\'#edit_company_url\').val(\'' + row.url + '\');\
                            " class="btn btn-sm btn-clean btn-icon" title="'+ lang.company_alert_edit + '">\
                                <span class="svg-icon svg-icon-md"><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1"><g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd"><rect x="0" y="0" width="24" height="24"></rect><path d="M8,17.9148182 L8,5.96685884 C8,5.56391781 8.16211443,5.17792052 8.44982609,4.89581508 L10.965708,2.42895648 C11.5426798,1.86322723 12.4640974,1.85620921 13.0496196,2.41308426 L15.5337377,4.77566479 C15.8314604,5.0588212 16,5.45170806 16,5.86258077 L16,17.9148182 C16,18.7432453 15.3284271,19.4148182 14.5,19.4148182 L9.5,19.4148182 C8.67157288,19.4148182 8,18.7432453 8,17.9148182 Z" fill="#000000" fill-rule="nonzero" transform="translate(12.000000, 10.707409) rotate(-135.000000) translate(-12.000000, -10.707409) "></path><rect fill="#000000" opacity="0.3" x="5" y="20" width="15" height="2" rx="1"></rect></g></svg></span>\
                            </a>'+
                            '<a onclick="javascript:confirm(\'' + lang.company_alert_confirmdelete + ' ' + row.name + '?\')?deleteCompany(\'' + row.id + '\'):\'\';" class="btn btn-sm btn-clean btn-icon" title="Reset password">\
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
jQuery(document).ready(function () {
    datatableInit();
});
function CompanyAddAction() {
    $('#edit_company_id').val('');
    $('#edit_company_name').val('');
    $('#edit_company_purpose').val('');
    $('#edit_company_direction').val('');
    $('#edit_company_phone').val('');
    $('#edit_company_url').val('');
}
function isValidateEditCompanyModal() {
    if ($('#edit_company_name').val() == '') {
        $('#edit_company_name').focus();
        return false;
    }
    if ($('#edit_company_purpose').val() == '') {
        $('#edit_company_purpose').focus();
        return false;
    }
    if ($('#edit_company_direction').val() == '') {
        $('#edit_company_direction').focus();
        return false;
    }
    if ($('#edit_company_phone').val() == '') {
        $('#edit_company_phone').focus();
        return false;
    }
    if ($('#edit_company_url').val() == '') {
        $('#edit_company_url').focus();
        return false;
    }
    return true;
}
function submitCompanyEditForm() {
    if (!isValidateEditCompanyModal()) return;
    var form_data = new FormData();
    form_data.append('id', $('#edit_company_id').val());
    form_data.append('name', $('#edit_company_name').val());
    form_data.append('purpose', $('#edit_company_purpose').val());
    form_data.append('direction', $('#edit_company_direction').val());
    form_data.append('phone', $('#edit_company_phone').val());
    form_data.append('url', $('#edit_company_url').val());
    $.ajax({
        url: '/admin/saveCompany',
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
            $('#company_model_close_btn').trigger('click');
        },
        error: function (response) {

        }
    });
}
function deleteCompany(id) {
    var form_data = new FormData();
    form_data.append('id', id);
    $.ajax({
        url: '/admin/deleteCompany',
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
