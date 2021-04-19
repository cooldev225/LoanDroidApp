var data_table = null;
function datatableInit() {
    data_table = $('#kt_datatable').KTDatatable({
        data: {
            type: 'remote',
            source: {
                read: {
                    url: '/admin/getusersDataTable',
                    headers: {
                        'X-CSRF-Token': $('meta[name="csrf-token"]').attr('content')
                    },
                    params: {
                        role: $('#role_filter').val(),
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
                    field: 'id',
                    title: '',
                    template: function (row,index) {
                        return index+1;
                    },
                    width:40,
                },
                {
                    field: 'userName',
                    title: lang.users_tbl_username,
                },
                {
                    field: 'friendlyName',
                    title: lang.roles_tbl_name,
                },
                {
                    field: 'email',
                    title: lang.users_tbl_email,
                },
                {
                    field: 'phoneNumber',
                    title: lang.users_tbl_phonenumber,
                },
                {
                    field: 'otherPhone',
                    title: lang.users_tbl_otherphone,
                },
                {
                    field: 'officeNumber',
                    title: lang.users_tbl_officenumber,
                },
                {
                    field: 'jobTitle',
                    title: lang.users_tbl_department,
                },
                {
                    field: 'createdDate',
                    title: lang.global_tbl_createddate,
                    template: function (row) {
                        return getJustDateWIthYear(row.createdDate);
                    },
                },
                {
                    field: 'updatedDate',
                    title: lang.global_tbl_updateddate,
                    template: function (row) {
                        return getJustDateWIthYear(row.updatedDate);
                    },
                }, {
                    field: 'actions',
                    title: lang.global_tbl_action,
                    sortable: false,
                    overflow: 'visible',
                    autoHide: false,
                    width: 140,
                    template: function (row) {
                        return '\
                            <div class="dropdown dropdown-inline">\
                            <a data-toggle="modal" data-target="#editUserModal" href="javascript:;" onclick="\
                            $(\'#edit_user_id\').val(\''+ row.id + '\');\
                            $(\'#edit_user_username\').val(\''+ row.userName + '\');\
                            $(\'#edit_user_firstname\').val(\''+ row.firstName + '\');\
                            $(\'#edit_user_lastname\').val(\''+ row.lastName + '\');\
                            $(\'#edit_user_email\').val(\''+ row.email + '\');\
                            $(\'#edit_user_phonenumber\').val(\'' + row.phoneNumber + '\');\
                            $(\'#edit_user_otherphone\').val(\''+ (row.otherPhone == null ? '' : row.otherPhone) + '\');\
                            $(\'#edit_user_officenumber\').val(\''+ (row.officeNumber == null ? '' : row.officeNumber) + '\');\
                            $(\'#edit_user_department\').val(\''+ (row.jobTitle == null ? '' : row.jobTitle) + '\');\
                            setAvatar(\''+row.avatarImage+'\');\
                            " class="btn btn-sm btn-clean btn-icon" title="'+ lang.users_alert_editauser +'">\
                                <span class="svg-icon svg-icon-md"><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1"><g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd"><rect x="0" y="0" width="24" height="24"></rect><path d="M8,17.9148182 L8,5.96685884 C8,5.56391781 8.16211443,5.17792052 8.44982609,4.89581508 L10.965708,2.42895648 C11.5426798,1.86322723 12.4640974,1.85620921 13.0496196,2.41308426 L15.5337377,4.77566479 C15.8314604,5.0588212 16,5.45170806 16,5.86258077 L16,17.9148182 C16,18.7432453 15.3284271,19.4148182 14.5,19.4148182 L9.5,19.4148182 C8.67157288,19.4148182 8,18.7432453 8,17.9148182 Z" fill="#000000" fill-rule="nonzero" transform="translate(12.000000, 10.707409) rotate(-135.000000) translate(-12.000000, -10.707409) "></path><rect fill="#000000" opacity="0.3" x="5" y="20" width="15" height="2" rx="1"></rect></g></svg></span>\
                            </a>'+
                            '<a onclick="javascript:confirm(\'' + lang.users_alert_confirmresetpassword + '\')?resetPassword(\'' + row.id + '\'):\'\';" class="btn btn-sm btn-clean btn-icon mr-2" title="Reset password">\
                                <span class="svg-icon svg-icon-md"><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1"><g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd"><rect x="0" y="0" width="24" height="24"/><path d="M10.9,2 C11.4522847,2 11.9,2.44771525 11.9,3 C11.9,3.55228475 11.4522847,4 10.9,4 L6,4 C4.8954305,4 4,4.8954305 4,6 L4,18 C4,19.1045695 4.8954305,20 6,20 L18,20 C19.1045695,20 20,19.1045695 20,18 L20,16 C20,15.4477153 20.4477153,15 21,15 C21.5522847,15 22,15.4477153 22,16 L22,18 C22,20.209139 20.209139,22 18,22 L6,22 C3.790861,22 2,20.209139 2,18 L2,6 C2,3.790861 3.790861,2 6,2 L10.9,2 Z" fill="#000000" fill-rule="nonzero" opacity="0.3"/><path d="M24.0690576,13.8973499 C24.0690576,13.1346331 24.2324969,10.1246259 21.8580869,7.73659596 C20.2600137,6.12944276 17.8683518,5.85068794 15.0081639,5.72356847 L15.0081639,1.83791555 C15.0081639,1.42370199 14.6723775,1.08791555 14.2581639,1.08791555 C14.0718537,1.08791555 13.892213,1.15726043 13.7542266,1.28244533 L7.24606818,7.18681951 C6.93929045,7.46513642 6.9162184,7.93944934 7.1945353,8.24622707 C7.20914339,8.26232899 7.22444472,8.27778811 7.24039592,8.29256062 L13.7485543,14.3198102 C14.0524605,14.6012598 14.5269852,14.5830551 14.8084348,14.2791489 C14.9368329,14.140506 15.0081639,13.9585047 15.0081639,13.7695393 L15.0081639,9.90761477 C16.8241562,9.95755456 18.1177196,10.0730665 19.2929978,10.4469645 C20.9778605,10.9829796 22.2816185,12.4994368 23.2042718,14.996336 L23.2043032,14.9963244 C23.313119,15.2908036 23.5938372,15.4863432 23.9077781,15.4863432 L24.0735976,15.4863432 C24.0735976,15.0278051 24.0690576,14.3014082 24.0690576,13.8973499 Z" fill="#000000" fill-rule="nonzero" transform="translate(15.536799, 8.287129) scale(-1, 1) translate(-15.536799, -8.287129) "/></g></svg><!--end::Svg Icon--></span>\
                            </a > '+
                            '<a onclick="javascript:confirm(\'' + lang.users_alert_confirmdeleteauser + ' ' + row.userName + '?\')?deleteUser(\'' + row.id + '\'):\'\';" class="btn btn-sm btn-clean btn-icon mr-2" title="Reset password">\
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
function setAvatar(img) {
    $('#avatar_img').prop('src','data:image/png;base64,'+img);
}
function resetPassword(id) {
    var form_data = new FormData();
    form_data.append('id', id);
    $.ajax({
        url: '/admin/resetUserPassword',
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
            //data_table.reload();
        },
        error: function (response) {

        }
    });
}
jQuery(document).ready(function () {
    $('#role_filter').select2();
    $("#edit_user_avatar").on('change', function () {
        if (this.files && this.files[0]) {
            var FR = new FileReader();
            FR.addEventListener("load", function (e) {
                $("#avatar_img").attr('src',e.target.result);
            });
            FR.readAsDataURL(this.files[0]);
        }
    });
    $('.fileinput-delete').on('click', function () {
        $('#avatar_img').prop('src', $('#default_img').prop('src'));
    });
    
    datatableInit();
    $('#role_filter').on('change', function () {
        searchAction();
    });
    searchAction();
});
function searchAction() {
    data_table.setDataSourceParam('role', $('#role_filter').val());
    data_table.reload();
}
function UserAddAction() {
    $('#avatar_img').prop('src', $('#default_img').prop('src'));
    $('#edit_user_id').val('');
    $('#edit_user_username').val('');
    $('#edit_user_firstname').val('');
    $('#edit_user_lastname').val('');
    $('#edit_user_email').val('');
    $('#edit_user_phonenumber').val('');
    $('#edit_user_otherphone').val('');
    $('#edit_user_officenumber').val('');
    $('#edit_user_department').val('');
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
function submitUserEditForm() {
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
    form_data.append('department', $('#edit_user_department').val());
    form_data.append('avatarimage', $('#avatar_img').prop('src'));
    form_data.append('role', $('#role_filter').val());
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
            if (response < 0) {
                alert('This username is existen already.');
                return;
            }
            searchAction();
            $('#user_model_close_btn').trigger('click');
        },
        error: function (response) {

        }
    });
}
function deleteUser(id) {
    var form_data = new FormData();
    form_data.append('id', id);
    $.ajax({
        url: '/admin/deleteUser',
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
