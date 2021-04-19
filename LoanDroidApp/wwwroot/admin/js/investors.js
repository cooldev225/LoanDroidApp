var data_table = null;
function expendDetailBody(e) {
    var detail = $('<div/>').attr('id', 'child_data_ajax_' + e.data.id).attr('style', 'background-color: #f3f6f9;');
    var html = '\
            <div class="container" style="padding-top:20px;">\
        ';
    for (var i = 0; i < e.data.accountPayment.length; i++) {
        if (e.data.accountPayment[i].type == 0) {
            html += '\
                <div class="row">\
                    <div class="col-xl-12">\
                        <div class="card card-custom gutter-b card-stretch" style="margin:0px 10px;">\
                            <div class="card-body">\
                                <div class="d-flex align-items-center" style="overflow: overlay;">\
                                    <div class="flex-shrink-0 mr-4 symbol symbol-65">\
                                        <img src="../images/Global_Gateway.svg" alt="image">\
                                    </div>\
                                    <div class="d-flex flex-column mr-auto">\
                                        <a href="'+ e.data.accountPayment[i].gatewayUrl + '" class="card-title text-hover-primary font-weight-bolder font-size-h5 text-dark mb-1">\
                                            '+ e.data.accountPayment[i].gatewayName + ' - ' + e.data.accountPayment[i].gatewayUrl + '\
                                        </a>\
                                        <span class="text-muted font-weight-bold">\
                                            '+ e.data.accountPayment[i].gatewayUserName + '\
                                        </span>\
                                    </div>\
                                    <div class="mr-12 d-flex flex-column">\
                                        <span class="font-weight-bolder mb-4">'+ lang.users_tbl_username + '</span>\
                                        <span class="font-weight-bold text-muted font-size-h5 pt-1">'+ e.data.accountPayment[i].gatewayUserName + '</span>\
                                    </div>\
                                    <div class="mr-12 d-flex flex-column">\
                                        <span class="font-weight-bolder mb-4">'+ lang.users_tbl_email + '</span>\
                                        <span class="font-weight-bold text-muted font-size-h5 pt-1">'+ e.data.accountPayment[i].gatewayEmail + '</span>\
                                    </div>\
                                    <div class="mr-12 d-flex flex-column">\
                                        <span class="font-weight-bolder mb-4">'+ lang.password + '</span>\
                                        <span class="font-weight-bold text-muted font-size-h5 pt-1">'+ e.data.accountPayment[i].gatewayPassword + '</span>\
                                    </div>\
                                </div>\
                            </div>\
                            <div class="card-footer d-flex align-items-center">\
                                <div class="d-flex">\
                                    <div class="d-flex align-items-center mr-7">\
                                        <span class="svg-icon svg-icon-gray-500"><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">\
                                            <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">\
                                                <rect x="0" y="0" width="24" height="24"></rect>\
                                                <path d="M10.5,5 L19.5,5 C20.3284271,5 21,5.67157288 21,6.5 C21,7.32842712 20.3284271,8 19.5,8 L10.5,8 C9.67157288,8 9,7.32842712 9,6.5 C9,5.67157288 9.67157288,5 10.5,5 Z M10.5,10 L19.5,10 C20.3284271,10 21,10.6715729 21,11.5 C21,12.3284271 20.3284271,13 19.5,13 L10.5,13 C9.67157288,13 9,12.3284271 9,11.5 C9,10.6715729 9.67157288,10 10.5,10 Z M10.5,15 L19.5,15 C20.3284271,15 21,15.6715729 21,16.5 C21,17.3284271 20.3284271,18 19.5,18 L10.5,18 C9.67157288,18 9,17.3284271 9,16.5 C9,15.6715729 9.67157288,15 10.5,15 Z" fill="#000000"></path>\
                                                <path d="M5.5,8 C4.67157288,8 4,7.32842712 4,6.5 C4,5.67157288 4.67157288,5 5.5,5 C6.32842712,5 7,5.67157288 7,6.5 C7,7.32842712 6.32842712,8 5.5,8 Z M5.5,13 C4.67157288,13 4,12.3284271 4,11.5 C4,10.6715729 4.67157288,10 5.5,10 C6.32842712,10 7,10.6715729 7,11.5 C7,12.3284271 6.32842712,13 5.5,13 Z M5.5,18 C4.67157288,18 4,17.3284271 4,16.5 C4,15.6715729 4.67157288,15 5.5,15 C6.32842712,15 7,15.6715729 7,16.5 C7,17.3284271 6.32842712,18 5.5,18 Z" fill="#000000" opacity="0.3"></path>\
                                            </g>\
                                            </svg>\
                                        </span>\
                                        <a href="#" class="font-weight-bolder text-primary ml-2">72 Ocupaciones</a>\
                                    </div>\
                                </div>\
                                <div style="margin-left:calc(100% - 480px);">\
                                    <button type="button" onclick="" class="btn btn-light-primary btn-sm text-uppercase font-weight-bolder mt-5 mt-sm-0 mr-auto mr-sm-0 ml-sm-auto"><span class="svg-icon svg-icon-md"><!--begin::Svg Icon | path:C:\wamp64\www\keenthemes\themes\metronic\theme\html\demo1\dist/../src/media/svg/icons\Code\Git3.svg--><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1"><g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd"><rect x="0" y="0" width="24" height="24"/><path d="M7,11 L15,11 C16.1045695,11 17,10.1045695 17,9 L17,8 L19,8 L19,9 C19,11.209139 17.209139,13 15,13 L7,13 L7,15 C7,15.5522847 6.55228475,16 6,16 C5.44771525,16 5,15.5522847 5,15 L5,9 C5,8.44771525 5.44771525,8 6,8 C6.55228475,8 7,8.44771525 7,9 L7,11 Z" fill="#000000" opacity="0.3"/>        <path d="M6,21 C7.1045695,21 8,20.1045695 8,19 C8,17.8954305 7.1045695,17 6,17 C4.8954305,17 4,17.8954305 4,19 C4,20.1045695 4.8954305,21 6,21 Z M6,23 C3.790861,23 2,21.209139 2,19 C2,16.790861 3.790861,15 6,15 C8.209139,15 10,16.790861 10,19 C10,21.209139 8.209139,23 6,23 Z" fill="#000000" fill-rule="nonzero"/>        <path d="M18,7 C19.1045695,7 20,6.1045695 20,5 C20,3.8954305 19.1045695,3 18,3 C16.8954305,3 16,3.8954305 16,5 C16,6.1045695 16.8954305,7 18,7 Z M18,9 C15.790861,9 14,7.209139 14,5 C14,2.790861 15.790861,1 18,1 C20.209139,1 22,2.790861 22,5 C22,7.209139 20.209139,9 18,9 Z" fill="#000000" fill-rule="nonzero"/>        <path d="M6,7 C7.1045695,7 8,6.1045695 8,5 C8,3.8954305 7.1045695,3 6,3 C4.8954305,3 4,3.8954305 4,5 C4,6.1045695 4.8954305,7 6,7 Z M6,9 C3.790861,9 2,7.209139 2,5 C2,2.790861 3.790861,1 6,1 C8.209139,1 10,2.790861 10,5 C10,7.209139 8.209139,9 6,9 Z" fill="#000000" fill-rule="nonzero"/>    </g></svg><!--end::Svg Icon--></span>'+ lang.tracking + '</button>\
                                    <button type="button" data-toggle="modal" data-target="#editPaymentModal" onclick="\
                                        addPaymentAction(\''+ e.data.id + '\');\
                                        setPaymentActiveTab(0);\
                                        $(\'#edit_payment_id\').val(\''+ e.data.accountPayment[i].id + '\');\
                                        $(\'#edit_payment_gatewayusername\').val(\'' + e.data.accountPayment[i].gatewayUserName + '\');\
                                        $(\'#edit_payment_gatewaypassword\').val(\''+ e.data.accountPayment[i].gatewayPassword + '\');\
                                        $(\'#edit_payment_gatewayemail\').val(\''+ e.data.accountPayment[i].gatewayEmail + '\');\
                                        $(\'#edit_payment_gatewayurl\').val(\''+ e.data.accountPayment[i].gatewayUrl + '\');\
                                        $(\'#edit_payment_gatewayname\').val(\''+ e.data.accountPayment[i].gatewayName + '\');\
                                        " class="btn btn-light-primary btn-sm text-uppercase font-weight-bolder mt-5 mt-sm-0 mr-auto mr-sm-0 ml-sm-auto"><span class="svg-icon svg-icon-md"><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1"><g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd"><rect x="0" y="0" width="24" height="24"></rect><path d="M8,17.9148182 L8,5.96685884 C8,5.56391781 8.16211443,5.17792052 8.44982609,4.89581508 L10.965708,2.42895648 C11.5426798,1.86322723 12.4640974,1.85620921 13.0496196,2.41308426 L15.5337377,4.77566479 C15.8314604,5.0588212 16,5.45170806 16,5.86258077 L16,17.9148182 C16,18.7432453 15.3284271,19.4148182 14.5,19.4148182 L9.5,19.4148182 C8.67157288,19.4148182 8,18.7432453 8,17.9148182 Z" fill="#000000" fill-rule="nonzero" transform="translate(12.000000, 10.707409) rotate(-135.000000) translate(-12.000000, -10.707409) "></path><rect fill="#000000" opacity="0.3" x="5" y="20" width="15" height="2" rx="1"></rect></g></svg></span>' + lang.edit + '</button>\
                                    <button onclick="deletePaymentAction('+ e.data.accountPayment[i].id + ');" type="button" class="btn btn-light-primary btn-sm text-uppercase font-weight-bolder mt-5 mt-sm-0 mr-auto mr-sm-0 ml-sm-auto"><span class="svg-icon svg-icon-md" style="margin-top: -5px;"><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1"><g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd"><rect x="0" y="0" width="24" height="24"></rect><path d="M6,8 L6,20.5 C6,21.3284271 6.67157288,22 7.5,22 L16.5,22 C17.3284271,22 18,21.3284271 18,20.5 L18,8 L6,8 Z" fill="#000000" fill-rule="nonzero"></path><path d="M14,4.5 L14,4 C14,3.44771525 13.5522847,3 13,3 L11,3 C10.4477153,3 10,3.44771525 10,4 L10,4.5 L5.5,4.5 C5.22385763,4.5 5,4.72385763 5,5 L5,5.5 C5,5.77614237 5.22385763,6 5.5,6 L18.5,6 C18.7761424,6 19,5.77614237 19,5.5 L19,5 C19,4.72385763 18.7761424,4.5 18.5,4.5 L14,4.5 Z" fill="#000000" opacity="0.3"></path></g></svg></span>' + lang.delete + '</button>\
                                </div>\
                            </div>\
                        </div>\
                    </div>\
                </div>\
            ';
        } else if (e.data.accountPayment[i].type == 1) {
            html += '\
                <div class="row">\
                    <div class="col-xl-12">\
                        <div class="card card-custom gutter-b card-stretch" style="margin:0px 10px;">\
                            <div class="card-body">\
                                <div class="d-flex align-items-center" style="overflow: overlay;">\
                                    <div class="flex-shrink-0 mr-4 symbol symbol-65">\
                                        <img src="../images/card.jpg" alt="image">\
                                    </div>\
                                    <div class="d-flex flex-column mr-auto">\
                                        <a href="javascript:;" class="card-title text-hover-primary font-weight-bolder font-size-h5 text-dark mb-1">\
                                            '+ lang.card + '\
                                        </a>\
                                        <span class="text-muted font-weight-bold">\
                                            '+ e.data.accountPayment[i].cardFirstName + ' ' + e.data.accountPayment[i].cardLastName + '\
                                        </span>\
                                    </div>\
                                    <div class="mr-12 d-flex flex-column">\
                                        <span class="font-weight-bolder mb-4">'+ lang.users_tbl_number + '</span>\
                                        <span class="font-weight-bold text-muted font-size-h5 pt-1">'+ e.data.accountPayment[i].cardNumber + '</span>\
                                    </div>\
                                    <div class="mr-12 d-flex flex-column">\
                                        <span class="font-weight-bolder mb-4">'+ lang.expiration + '</span>\
                                        <span class="font-weight-bold text-muted font-size-h5 pt-1">'+ e.data.accountPayment[i].cardExpirationDate + '</span>\
                                    </div>\
                                    <div class="mr-12 d-flex flex-column">\
                                        <span class="font-weight-bolder mb-4">'+ lang.clients_tbl_address + '-1</span>\
                                        <span class="font-weight-bold text-muted font-size-h5 pt-1">'+ e.data.accountPayment[i].cardAddress1 + '</span>\
                                    </div>\
                                    <div class="mr-12 d-flex flex-column">\
                                        <span class="font-weight-bolder mb-4">'+ lang.clients_tbl_address + '-2</span>\
                                        <span class="font-weight-bold text-muted font-size-h5 pt-1">'+ e.data.accountPayment[i].cardAddress2 + '</span>\
                                    </div>\
                                </div>\
                            </div>\
                            <div class="card-footer d-flex align-items-center">\
                                <div class="d-flex">\
                                    <div class="d-flex align-items-center mr-7">\
                                        <span class="svg-icon svg-icon-gray-500"><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">\
                                            <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">\
                                                <rect x="0" y="0" width="24" height="24"></rect>\
                                                <path d="M10.5,5 L19.5,5 C20.3284271,5 21,5.67157288 21,6.5 C21,7.32842712 20.3284271,8 19.5,8 L10.5,8 C9.67157288,8 9,7.32842712 9,6.5 C9,5.67157288 9.67157288,5 10.5,5 Z M10.5,10 L19.5,10 C20.3284271,10 21,10.6715729 21,11.5 C21,12.3284271 20.3284271,13 19.5,13 L10.5,13 C9.67157288,13 9,12.3284271 9,11.5 C9,10.6715729 9.67157288,10 10.5,10 Z M10.5,15 L19.5,15 C20.3284271,15 21,15.6715729 21,16.5 C21,17.3284271 20.3284271,18 19.5,18 L10.5,18 C9.67157288,18 9,17.3284271 9,16.5 C9,15.6715729 9.67157288,15 10.5,15 Z" fill="#000000"></path>\
                                                <path d="M5.5,8 C4.67157288,8 4,7.32842712 4,6.5 C4,5.67157288 4.67157288,5 5.5,5 C6.32842712,5 7,5.67157288 7,6.5 C7,7.32842712 6.32842712,8 5.5,8 Z M5.5,13 C4.67157288,13 4,12.3284271 4,11.5 C4,10.6715729 4.67157288,10 5.5,10 C6.32842712,10 7,10.6715729 7,11.5 C7,12.3284271 6.32842712,13 5.5,13 Z M5.5,18 C4.67157288,18 4,17.3284271 4,16.5 C4,15.6715729 4.67157288,15 5.5,15 C6.32842712,15 7,15.6715729 7,16.5 C7,17.3284271 6.32842712,18 5.5,18 Z" fill="#000000" opacity="0.3"></path>\
                                            </g>\
                                            </svg>\
                                        </span>\
                                        <a href="#" class="font-weight-bolder text-primary ml-2">72 Ocupaciones</a>\
                                    </div>\
                                </div>\
                                <div style="margin-left:calc(100% - 480px);">\
                                    <button type="button" class="btn btn-light-primary btn-sm text-uppercase font-weight-bolder mt-5 mt-sm-0 mr-auto mr-sm-0 ml-sm-auto"><span class="svg-icon svg-icon-md"><!--begin::Svg Icon | path:C:\wamp64\www\keenthemes\themes\metronic\theme\html\demo1\dist/../src/media/svg/icons\Code\Git3.svg--><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1"><g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd"><rect x="0" y="0" width="24" height="24"/><path d="M7,11 L15,11 C16.1045695,11 17,10.1045695 17,9 L17,8 L19,8 L19,9 C19,11.209139 17.209139,13 15,13 L7,13 L7,15 C7,15.5522847 6.55228475,16 6,16 C5.44771525,16 5,15.5522847 5,15 L5,9 C5,8.44771525 5.44771525,8 6,8 C6.55228475,8 7,8.44771525 7,9 L7,11 Z" fill="#000000" opacity="0.3"/>        <path d="M6,21 C7.1045695,21 8,20.1045695 8,19 C8,17.8954305 7.1045695,17 6,17 C4.8954305,17 4,17.8954305 4,19 C4,20.1045695 4.8954305,21 6,21 Z M6,23 C3.790861,23 2,21.209139 2,19 C2,16.790861 3.790861,15 6,15 C8.209139,15 10,16.790861 10,19 C10,21.209139 8.209139,23 6,23 Z" fill="#000000" fill-rule="nonzero"/>        <path d="M18,7 C19.1045695,7 20,6.1045695 20,5 C20,3.8954305 19.1045695,3 18,3 C16.8954305,3 16,3.8954305 16,5 C16,6.1045695 16.8954305,7 18,7 Z M18,9 C15.790861,9 14,7.209139 14,5 C14,2.790861 15.790861,1 18,1 C20.209139,1 22,2.790861 22,5 C22,7.209139 20.209139,9 18,9 Z" fill="#000000" fill-rule="nonzero"/>        <path d="M6,7 C7.1045695,7 8,6.1045695 8,5 C8,3.8954305 7.1045695,3 6,3 C4.8954305,3 4,3.8954305 4,5 C4,6.1045695 4.8954305,7 6,7 Z M6,9 C3.790861,9 2,7.209139 2,5 C2,2.790861 3.790861,1 6,1 C8.209139,1 10,2.790861 10,5 C10,7.209139 8.209139,9 6,9 Z" fill="#000000" fill-rule="nonzero"/>    </g></svg><!--end::Svg Icon--></span>'+ lang.tracking + '</button>\
                                    <button type="button" data-toggle="modal" data-target="#editPaymentModal" onclick="\
                                        addPaymentAction(\''+ e.data.id + '\');\
                                        setPaymentActiveTab(1);\
                                        $(\'#edit_payment_id\').val(\''+ e.data.accountPayment[i].id + '\');\
                                        $(\'#edit_payment_cardfirstname\').val(\'' + e.data.accountPayment[i].cardFirstName + '\');\
                                        $(\'#edit_payment_cardlastname\').val(\''+ e.data.accountPayment[i].cardLastName + '\');\
                                        $(\'#edit_payment_cardnumber\').val(\''+ e.data.accountPayment[i].cardNumber + '\');\
                                        $(\'#edit_payment_cardexpirationdate\').val(\''+ e.data.accountPayment[i].cardExpirationDate + '\');\
                                        $(\'#edit_payment_cardaddress1\').val(\''+ e.data.accountPayment[i].cardAddress1 + '\');\
                                        $(\'#edit_payment_cardaddress2\').val(\''+ e.data.accountPayment[i].cardAddress2 + '\');\
                                        " type="button" class="btn btn-light-primary btn-sm text-uppercase font-weight-bolder mt-5 mt-sm-0 mr-auto mr-sm-0 ml-sm-auto"><span class="svg-icon svg-icon-md"><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1"><g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd"><rect x="0" y="0" width="24" height="24"></rect><path d="M8,17.9148182 L8,5.96685884 C8,5.56391781 8.16211443,5.17792052 8.44982609,4.89581508 L10.965708,2.42895648 C11.5426798,1.86322723 12.4640974,1.85620921 13.0496196,2.41308426 L15.5337377,4.77566479 C15.8314604,5.0588212 16,5.45170806 16,5.86258077 L16,17.9148182 C16,18.7432453 15.3284271,19.4148182 14.5,19.4148182 L9.5,19.4148182 C8.67157288,19.4148182 8,18.7432453 8,17.9148182 Z" fill="#000000" fill-rule="nonzero" transform="translate(12.000000, 10.707409) rotate(-135.000000) translate(-12.000000, -10.707409) "></path><rect fill="#000000" opacity="0.3" x="5" y="20" width="15" height="2" rx="1"></rect></g></svg></span>'+ lang.edit + '</button>\
                                    <button onclick="deletePaymentAction('+ e.data.accountPayment[i].id + ');" type="button" class="btn btn-light-primary btn-sm text-uppercase font-weight-bolder mt-5 mt-sm-0 mr-auto mr-sm-0 ml-sm-auto"><span class="svg-icon svg-icon-md" style="margin-top: -5px;"><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1"><g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd"><rect x="0" y="0" width="24" height="24"></rect><path d="M6,8 L6,20.5 C6,21.3284271 6.67157288,22 7.5,22 L16.5,22 C17.3284271,22 18,21.3284271 18,20.5 L18,8 L6,8 Z" fill="#000000" fill-rule="nonzero"></path><path d="M14,4.5 L14,4 C14,3.44771525 13.5522847,3 13,3 L11,3 C10.4477153,3 10,3.44771525 10,4 L10,4.5 L5.5,4.5 C5.22385763,4.5 5,4.72385763 5,5 L5,5.5 C5,5.77614237 5.22385763,6 5.5,6 L18.5,6 C18.7761424,6 19,5.77614237 19,5.5 L19,5 C19,4.72385763 18.7761424,4.5 18.5,4.5 L14,4.5 Z" fill="#000000" opacity="0.3"></path></g></svg></span>' + lang.delete + '</button>\
                                </div>\
                            </div>\
                        </div>\
                    </div>\
                </div>\
            ';
        } else if (e.data.accountPayment[i].type == 2) {
            html += '\
                <div class="row">\
                    <div class="col-xl-12">\
                        <div class="card card-custom gutter-b card-stretch" style="margin:0px 10px;">\
                            <div class="card-body">\
                                <div class="d-flex align-items-center" style="overflow: overlay;">\
                                    <div class="flex-shrink-0 mr-4 symbol symbol-65">\
                                        <img src="../images/bank.png" alt="image">\
                                    </div>\
                                    <div class="d-flex flex-column mr-auto">\
                                        <a href="javascript:;" class="card-title text-hover-primary font-weight-bolder font-size-h5 text-dark mb-1">\
                                            '+ lang.bank + ' - ' + e.data.accountPayment[i].bankName + '\
                                        </a>\
                                        <span class="text-muted font-weight-bold">\
                                            '+ e.data.accountPayment[i].bankCountry + ' ' + e.data.accountPayment[i].bankCity + '\
                                        </span>\
                                    </div>\
                                    <div class="mr-12 d-flex flex-column">\
                                        <span class="font-weight-bolder mb-4">'+ lang.currency + '</span>\
                                        <span class="font-weight-bold text-muted font-size-h5 pt-1">'+ e.data.accountPayment[i].bankCurrency + '</span>\
                                    </div>\
                                    <div class="mr-12 d-flex flex-column">\
                                        <span class="font-weight-bolder mb-4">'+ lang.currency + '</span>\
                                        <span class="font-weight-bold text-muted font-size-h5 pt-1">'+ e.data.accountPayment[i].bankAccountHolder + '</span>\
                                    </div>\
                                    <div class="mr-12 d-flex flex-column">\
                                        <span class="font-weight-bolder mb-4">'+ lang.routingNumber + '</span>\
                                        <span class="font-weight-bold text-muted font-size-h5 pt-1">'+ e.data.accountPayment[i].bankRoutingNumber + '</span>\
                                    </div>\
                                    <div class="mr-12 d-flex flex-column">\
                                        <span class="font-weight-bolder mb-4">'+ lang.street + '</span>\
                                        <span class="font-weight-bold text-muted font-size-h5 pt-1">'+ e.data.accountPayment[i].bankStreet + '</span>\
                                    </div>\
                                    <div class="mr-12 d-flex flex-column">\
                                        <span class="font-weight-bolder mb-4">'+ lang.region + '</span>\
                                        <span class="font-weight-bold text-muted font-size-h5 pt-1">'+ e.data.accountPayment[i].bankRegion + '</span>\
                                    </div>\
                                    <div class="mr-12 d-flex flex-column">\
                                        <span class="font-weight-bolder mb-4">Bic '+ lang.users_tbl_number + '-2</span>\
                                        <span class="font-weight-bold text-muted font-size-h5 pt-1">'+ e.data.accountPayment[i].bankSwiftBicNumber + '</span>\
                                    </div>\
                                    <div class="mr-12 d-flex flex-column">\
                                        <span class="font-weight-bolder mb-4">IBAN '+ lang.users_tbl_number + '</span>\
                                        <span class="font-weight-bold text-muted font-size-h5 pt-1">'+ e.data.accountPayment[i].bankIBANNumber + '</span>\
                                    </div>\
                                </div>\
                            </div>\
                            <div class="card-footer d-flex align-items-center">\
                                <div class="d-flex">\
                                    <div class="d-flex align-items-center mr-7">\
                                        <span class="svg-icon svg-icon-gray-500"><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">\
                                            <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">\
                                                <rect x="0" y="0" width="24" height="24"></rect>\
                                                <path d="M10.5,5 L19.5,5 C20.3284271,5 21,5.67157288 21,6.5 C21,7.32842712 20.3284271,8 19.5,8 L10.5,8 C9.67157288,8 9,7.32842712 9,6.5 C9,5.67157288 9.67157288,5 10.5,5 Z M10.5,10 L19.5,10 C20.3284271,10 21,10.6715729 21,11.5 C21,12.3284271 20.3284271,13 19.5,13 L10.5,13 C9.67157288,13 9,12.3284271 9,11.5 C9,10.6715729 9.67157288,10 10.5,10 Z M10.5,15 L19.5,15 C20.3284271,15 21,15.6715729 21,16.5 C21,17.3284271 20.3284271,18 19.5,18 L10.5,18 C9.67157288,18 9,17.3284271 9,16.5 C9,15.6715729 9.67157288,15 10.5,15 Z" fill="#000000"></path>\
                                                <path d="M5.5,8 C4.67157288,8 4,7.32842712 4,6.5 C4,5.67157288 4.67157288,5 5.5,5 C6.32842712,5 7,5.67157288 7,6.5 C7,7.32842712 6.32842712,8 5.5,8 Z M5.5,13 C4.67157288,13 4,12.3284271 4,11.5 C4,10.6715729 4.67157288,10 5.5,10 C6.32842712,10 7,10.6715729 7,11.5 C7,12.3284271 6.32842712,13 5.5,13 Z M5.5,18 C4.67157288,18 4,17.3284271 4,16.5 C4,15.6715729 4.67157288,15 5.5,15 C6.32842712,15 7,15.6715729 7,16.5 C7,17.3284271 6.32842712,18 5.5,18 Z" fill="#000000" opacity="0.3"></path>\
                                            </g>\
                                            </svg>\
                                        </span>\
                                        <a href="#" class="font-weight-bolder text-primary ml-2">72 Ocupaciones</a>\
                                    </div>\
                                </div>\
                                <div style="margin-left:calc(100% - 480px);">\
                                    <button type="button" class="btn btn-light-primary btn-sm text-uppercase font-weight-bolder mt-5 mt-sm-0 mr-auto mr-sm-0 ml-sm-auto"><span class="svg-icon svg-icon-md"><!--begin::Svg Icon | path:C:\wamp64\www\keenthemes\themes\metronic\theme\html\demo1\dist/../src/media/svg/icons\Code\Git3.svg--><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1"><g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd"><rect x="0" y="0" width="24" height="24"/><path d="M7,11 L15,11 C16.1045695,11 17,10.1045695 17,9 L17,8 L19,8 L19,9 C19,11.209139 17.209139,13 15,13 L7,13 L7,15 C7,15.5522847 6.55228475,16 6,16 C5.44771525,16 5,15.5522847 5,15 L5,9 C5,8.44771525 5.44771525,8 6,8 C6.55228475,8 7,8.44771525 7,9 L7,11 Z" fill="#000000" opacity="0.3"/>        <path d="M6,21 C7.1045695,21 8,20.1045695 8,19 C8,17.8954305 7.1045695,17 6,17 C4.8954305,17 4,17.8954305 4,19 C4,20.1045695 4.8954305,21 6,21 Z M6,23 C3.790861,23 2,21.209139 2,19 C2,16.790861 3.790861,15 6,15 C8.209139,15 10,16.790861 10,19 C10,21.209139 8.209139,23 6,23 Z" fill="#000000" fill-rule="nonzero"/>        <path d="M18,7 C19.1045695,7 20,6.1045695 20,5 C20,3.8954305 19.1045695,3 18,3 C16.8954305,3 16,3.8954305 16,5 C16,6.1045695 16.8954305,7 18,7 Z M18,9 C15.790861,9 14,7.209139 14,5 C14,2.790861 15.790861,1 18,1 C20.209139,1 22,2.790861 22,5 C22,7.209139 20.209139,9 18,9 Z" fill="#000000" fill-rule="nonzero"/>        <path d="M6,7 C7.1045695,7 8,6.1045695 8,5 C8,3.8954305 7.1045695,3 6,3 C4.8954305,3 4,3.8954305 4,5 C4,6.1045695 4.8954305,7 6,7 Z M6,9 C3.790861,9 2,7.209139 2,5 C2,2.790861 3.790861,1 6,1 C8.209139,1 10,2.790861 10,5 C10,7.209139 8.209139,9 6,9 Z" fill="#000000" fill-rule="nonzero"/>    </g></svg><!--end::Svg Icon--></span>'+ lang.tracking + '</button>\
                                    <button type="button" data-toggle="modal" data-target="#editPaymentModal" onclick="\
                                        addPaymentAction(\''+ e.data.id + '\');\
                                        setPaymentActiveTab(2);\
                                        $(\'#edit_payment_id\').val(\''+ e.data.accountPayment[i].id + '\');\
                                        $(\'#edit_payment_bankcountry\').val(\'' + e.data.accountPayment[i].bankCountry + '\');\
                                        $(\'#edit_payment_bankcurrency\').val(\''+ e.data.accountPayment[i].bankCurrency + '\');\
                                        $(\'#edit_payment_bankroutingnumber\').val(\''+ e.data.accountPayment[i].bankRoutingNumber + '\');\
                                        $(\'#edit_payment_bankaccountholder\').val(\''+ e.data.accountPayment[i].bankAccountHolder + '\');\
                                        $(\'#edit_payment_bankname\').val(\''+ e.data.accountPayment[i].bankName + '\');\
                                        $(\'#edit_payment_bankstreet\').val(\''+ e.data.accountPayment[i].bankStreet + '\');\
                                        $(\'#edit_payment_bankcity\').val(\''+ e.data.accountPayment[i].bankCity + '\');\
                                        $(\'#edit_payment_bankregion\').val(\''+ e.data.accountPayment[i].bankRegion + '\');\
                                        $(\'#edit_payment_bankswiftbicnumber\').val(\''+ e.data.accountPayment[i].bankSwiftBicNumber + '\');\
                                        $(\'#edit_payment_bankibannumber\').val(\''+ e.data.accountPayment[i].bankIBANNumber + '\');\
                                        " type="button" class="btn btn-light-primary btn-sm text-uppercase font-weight-bolder mt-5 mt-sm-0 mr-auto mr-sm-0 ml-sm-auto"><span class="svg-icon svg-icon-md"><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1"><g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd"><rect x="0" y="0" width="24" height="24"></rect><path d="M8,17.9148182 L8,5.96685884 C8,5.56391781 8.16211443,5.17792052 8.44982609,4.89581508 L10.965708,2.42895648 C11.5426798,1.86322723 12.4640974,1.85620921 13.0496196,2.41308426 L15.5337377,4.77566479 C15.8314604,5.0588212 16,5.45170806 16,5.86258077 L16,17.9148182 C16,18.7432453 15.3284271,19.4148182 14.5,19.4148182 L9.5,19.4148182 C8.67157288,19.4148182 8,18.7432453 8,17.9148182 Z" fill="#000000" fill-rule="nonzero" transform="translate(12.000000, 10.707409) rotate(-135.000000) translate(-12.000000, -10.707409) "></path><rect fill="#000000" opacity="0.3" x="5" y="20" width="15" height="2" rx="1"></rect></g></svg></span>'+ lang.edit + '</button>\
                                    <button onclick="deletePaymentAction('+ e.data.accountPayment[i].id + ');" type="button" class="btn btn-light-primary btn-sm text-uppercase font-weight-bolder mt-5 mt-sm-0 mr-auto mr-sm-0 ml-sm-auto"><span class="svg-icon svg-icon-md" style="margin-top: -5px;"><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1"><g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd"><rect x="0" y="0" width="24" height="24"></rect><path d="M6,8 L6,20.5 C6,21.3284271 6.67157288,22 7.5,22 L16.5,22 C17.3284271,22 18,21.3284271 18,20.5 L18,8 L6,8 Z" fill="#000000" fill-rule="nonzero"></path><path d="M14,4.5 L14,4 C14,3.44771525 13.5522847,3 13,3 L11,3 C10.4477153,3 10,3.44771525 10,4 L10,4.5 L5.5,4.5 C5.22385763,4.5 5,4.72385763 5,5 L5,5.5 C5,5.77614237 5.22385763,6 5.5,6 L18.5,6 C18.7761424,6 19,5.77614237 19,5.5 L19,5 C19,4.72385763 18.7761424,4.5 18.5,4.5 L14,4.5 Z" fill="#000000" opacity="0.3"></path></g></svg></span>' + lang.delete + '</button>\
                                </div>\
                            </div>\
                        </div>\
                    </div>\
                </div>\
            ';
        }
    }
    html += '</div>';
    detail.append(html);
    detail.appendTo(e.detailCell);
}
function datatableInit() {
    data_table = $('#kt_datatable').KTDatatable({
        data: {
            type: 'remote',
            source: {
                read: {
                    url: '/admin/getInvestorsDataTable',
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
                    field: 'id',
                    title: '',
                    width: 20,
                },
                {
                    field: 'firstName',
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
                                    <img src="../images/default-avatar.png" onload="setImgAvatar($(this),\''+ row.avatarImage + '\');" alt="photo">\
                                </div>\
                                <div class="ml-4">\
                                    <div class="text-dark-75 font-weight-bolder font-size-lg mb-0">'+ row.friendlyName + '</div>\
                                    <a href="#" class="text-muted font-weight-bold text-hover-primary">'+ row.email + '</a>\
                                </div>\
                            </div>\
                        ';
                    }
                },
                {
                    field: 'phoneNumber',
                    title: lang.users_tbl_number,
                    template: function (row, index) {
                        return '\
                            <span style="width: 121px;">\
                                '+ (row.phoneNumber == '' || row.phoneNumber == null ? '' : '<div class="font-weight-bolder font-size-lg mb-0">' + row.phoneNumber + '</div>') + '\
                                '+ (row.otherPhone == '' || row.otherPhone == null ? '' : '<div class="font-weight-bold text-muted">' + row.otherPhone + '</div>') + '\
                                '+ (row.officeNumber == '' || row.officeNumber == null ? '' : '<div class="font-weight-bold text-muted">' + row.officeNumber + '</div>') + '\
                            </span>\
                        ';
                    },
                    width: 100,
                },
                {
                    field: 'address',
                    title: lang.clients_tbl_address,
                },
                {
                    field: 'createdDate',
                    title: lang.global_tbl_createddate,
                    template: function (row) {
                        return getJustDateWIthYear(row.createdDate);
                    },
                    template: function (row, index) {
                        return '\
                            <span style="width: 121px;">\
                                <div class="font-weight-bolder font-size-lg mb-0">' + getJustDateWIthYear(row.createdDate) + '</div>\
                                <div class="font-weight-bold text-muted">' + getJustDateWIthYear(row.updatedDate) + '</div>\
                            </span>\
                        ';
                    },
                }, {
                    field: 'actions',
                    title: lang.global_tbl_action,
                    sortable: false,
                    overflow: 'visible',
                    autoHide: false,
                    width: 160,
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
                            setAvatar(\''+ row.avatarImage +'\');\
                            " class="btn btn-sm btn-clean btn-icon" title="'+ lang.investors_alert_editauser + '">\
                                <span class="svg-icon svg-icon-md"><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1"><g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd"><rect x="0" y="0" width="24" height="24"></rect><path d="M8,17.9148182 L8,5.96685884 C8,5.56391781 8.16211443,5.17792052 8.44982609,4.89581508 L10.965708,2.42895648 C11.5426798,1.86322723 12.4640974,1.85620921 13.0496196,2.41308426 L15.5337377,4.77566479 C15.8314604,5.0588212 16,5.45170806 16,5.86258077 L16,17.9148182 C16,18.7432453 15.3284271,19.4148182 14.5,19.4148182 L9.5,19.4148182 C8.67157288,19.4148182 8,18.7432453 8,17.9148182 Z" fill="#000000" fill-rule="nonzero" transform="translate(12.000000, 10.707409) rotate(-135.000000) translate(-12.000000, -10.707409) "></path><rect fill="#000000" opacity="0.3" x="5" y="20" width="15" height="2" rx="1"></rect></g></svg></span>\
                            </a>'+
                            '<a onclick="javascript:confirm(\'' + lang.users_alert_confirmresetpassword + '\')?resetPassword(\'' + row.id + '\'):\'\';" class="btn btn-sm btn-clean btn-icon mr-2" title="Reset password">\
                                <span class="svg-icon svg-icon-md"><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1"><g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd"><rect x="0" y="0" width="24" height="24"/><path d="M10.9,2 C11.4522847,2 11.9,2.44771525 11.9,3 C11.9,3.55228475 11.4522847,4 10.9,4 L6,4 C4.8954305,4 4,4.8954305 4,6 L4,18 C4,19.1045695 4.8954305,20 6,20 L18,20 C19.1045695,20 20,19.1045695 20,18 L20,16 C20,15.4477153 20.4477153,15 21,15 C21.5522847,15 22,15.4477153 22,16 L22,18 C22,20.209139 20.209139,22 18,22 L6,22 C3.790861,22 2,20.209139 2,18 L2,6 C2,3.790861 3.790861,2 6,2 L10.9,2 Z" fill="#000000" fill-rule="nonzero" opacity="0.3"/><path d="M24.0690576,13.8973499 C24.0690576,13.1346331 24.2324969,10.1246259 21.8580869,7.73659596 C20.2600137,6.12944276 17.8683518,5.85068794 15.0081639,5.72356847 L15.0081639,1.83791555 C15.0081639,1.42370199 14.6723775,1.08791555 14.2581639,1.08791555 C14.0718537,1.08791555 13.892213,1.15726043 13.7542266,1.28244533 L7.24606818,7.18681951 C6.93929045,7.46513642 6.9162184,7.93944934 7.1945353,8.24622707 C7.20914339,8.26232899 7.22444472,8.27778811 7.24039592,8.29256062 L13.7485543,14.3198102 C14.0524605,14.6012598 14.5269852,14.5830551 14.8084348,14.2791489 C14.9368329,14.140506 15.0081639,13.9585047 15.0081639,13.7695393 L15.0081639,9.90761477 C16.8241562,9.95755456 18.1177196,10.0730665 19.2929978,10.4469645 C20.9778605,10.9829796 22.2816185,12.4994368 23.2042718,14.996336 L23.2043032,14.9963244 C23.313119,15.2908036 23.5938372,15.4863432 23.9077781,15.4863432 L24.0735976,15.4863432 C24.0735976,15.0278051 24.0690576,14.3014082 24.0690576,13.8973499 Z" fill="#000000" fill-rule="nonzero" transform="translate(15.536799, 8.287129) scale(-1, 1) translate(-15.536799, -8.287129) "/></g></svg><!--end::Svg Icon--></span>\
                            </a > '+
                            '<a onclick="addPaymentAction(\'' + row.id + '\');" class="btn btn-sm btn-clean btn-icon" title="Payment Option setup" data-toggle="modal" data-target="#editPaymentModal" href="javascript:;">\
                                <span class="svg-icon svg-icon-md"><!--begin::Svg Icon | path:C:\wamp64\www\keenthemes\themes\metronic\theme\html\demo1\dist/../src/media/svg/icons\Shopping\Credit-card.svg--><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">    <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">        <rect x="0" y="0" width="24" height="24"/>        <rect fill="#000000" opacity="0.3" x="2" y="5" width="20" height="14" rx="2"/>        <rect fill="#000000" x="2" y="8" width="20" height="3"/>        <rect fill="#000000" opacity="0.3" x="16" y="14" width="4" height="2" rx="1"/>    </g></svg><!--end::Svg Icon--></span>\
                            </a> '+
                            '<a onclick="javascript:confirm(\'' + lang.investors_alert_confirmdeleteauser + ' ' + row.userName + '?\')?deleteUser(\'' + row.id + '\'):\'\';" class="btn btn-sm btn-clean btn-icon mr-2" title="Reset password">\
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
    datatableInit();
});
function setAvatar(img) {
    $('#avatar_img').prop('src', 'data:image/png;base64,' + img);
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
            if (response < 0) {
                alert('This username is existen already.');
                return;
            }
            data_table.reload();
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



function setImgAvatar(obj, img) {
    $(obj).prop('src', 'data:image/png;base64,' + img);
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
    $('#editPaymentModal .modal-content .card-toolbar ul li:nth-child(' + (type + 1) + ')').css('display', 'block');
}