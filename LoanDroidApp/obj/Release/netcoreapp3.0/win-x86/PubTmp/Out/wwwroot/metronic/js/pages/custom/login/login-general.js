"use strict";

// Class Definition
var KTLogin = function() {
    var _login;

    var _showForm = function(form) {
        var cls = 'login-' + form + '-on';
        var form = 'kt_login_' + form + '_form';

        _login.removeClass('login-forgot-on');
        _login.removeClass('login-signin-on');
        _login.removeClass('login-signup-on');

        _login.addClass(cls);

        KTUtil.animateClass(KTUtil.getById(form), 'animate__animated animate__backInUp');
    }

    var _handleSignInForm = function() {
        var validation;

        // Init form validation rules. For more info check the FormValidation plugin's official documentation:https://formvalidation.io/
        validation = FormValidation.formValidation(
			KTUtil.getById('kt_login_signin_form'),
			{
				fields: {
					username: {
						validators: {
							notEmpty: {
								message: lang.req_email
							}
						}
					},
					password: {
						validators: {
							notEmpty: {
								message: lang.req_password
							}
						}
					}
				},
				plugins: {
                    trigger: new FormValidation.plugins.Trigger(),
                    submitButton: new FormValidation.plugins.SubmitButton(),
                    //defaultSubmit: new FormValidation.plugins.DefaultSubmit(), // Uncomment this line to enable normal button submit after form validation
					bootstrap: new FormValidation.plugins.Bootstrap()
				}
			}
		);

        $('#kt_login_signin_submit').on('click', function (e) {
            //e.preventDefault();

            validation.validate().then(function(status) {
		        if (status == 'Valid') {
					$('#kt_login_signin_form').submit();
					return;
				} else {
					swal.fire({
		                text: lang.err_login,
		                icon: "error",
		                buttonsStyling: false,
		                confirmButtonText: lang.conf_gotit,
                        customClass: {
    						confirmButton: "btn font-weight-bold btn-light-primary"
    					}
		            }).then(function() {
						KTUtil.scrollTop();
					});
				}
		    });
        });

        // Handle forgot button
        $('#kt_login_forgot').on('click', function (e) {
            e.preventDefault();
            _showForm('forgot');
        });

        // Handle signup
        $('#kt_login_signup').on('click', function (e) {
			e.preventDefault();
            _showForm('signup');
        });
    }

    var _handleSignUpForm = function(e) {
        var validation;
        var form = KTUtil.getById('kt_login_signup_form');

        // Init form validation rules. For more info check the FormValidation plugin's official documentation:https://formvalidation.io/
        validation = FormValidation.formValidation(
			form,
			{
				fields: {
					email: {
                        validators: {
							notEmpty: {
								message: lang.req_email
							},
                            emailAddress: {
								message: lang.err_email
							}
						}
					},
                    password: {
                        validators: {
                            notEmpty: {
                                message: lang.req_password
                            }
                        }
                    },
                    cpassword: {
                        validators: {
                            notEmpty: {
                                message: lang.req_confirm
                            },
                            identical: {
                                compare: function() {
                                    return form.querySelector('[name="password"]').value;
                                },
                                message: lang.err_match                            }
                        }
                    },
                    agree: {
                        validators: {
                            notEmpty: {
                                message: lang.req_accept
                            }
                        }
                    },
				},
				plugins: {
					trigger: new FormValidation.plugins.Trigger(),
					bootstrap: new FormValidation.plugins.Bootstrap()
				}
			}
		);

        $('#kt_login_signup_submit').on('click', function (e) {
			e.preventDefault();
            validation.validate().then(function(status) {
		        if (status == 'Valid') {
					$('#kt_login_signup_form').submit();
					return;
				} else {
					swal.fire({
		                text: lang.err_login,
		                icon: "error",
		                buttonsStyling: false,
		                confirmButtonText: lang.conf_gotit,
                        customClass: {
    						confirmButton: "btn font-weight-bold btn-light-primary"
    					}
		            }).then(function() {
						KTUtil.scrollTop();
					});
				}
		    });
        });

        // Handle cancel button
        $('#kt_login_signup_cancel').on('click', function (e) {
            e.preventDefault();

            _showForm('signin');
        });
    }

    var _handleForgotForm = function(e) {
        var validation;

        // Init form validation rules. For more info check the FormValidation plugin's official documentation:https://formvalidation.io/
        validation = FormValidation.formValidation(
			KTUtil.getById('kt_login_forgot_form'),
			{
				fields: {
					email: {
						validators: {
							notEmpty: {
								message: lang.req_email
							},
                            emailAddress: {
								message: lang.err_email
							}
						}
					}
				},
				plugins: {
					trigger: new FormValidation.plugins.Trigger(),
					bootstrap: new FormValidation.plugins.Bootstrap()
				}
			}
		);

        // Handle submit button
        $('#kt_login_forgot_submit').on('click', function (e) {
            e.preventDefault();

            validation.validate().then(function(status) {
		        if (status == 'Valid') {
					// Submit form
					$('#kt_login_forgot_form').submit();
                    KTUtil.scrollTop();
				} else {
					swal.fire({
		                text: lang.err_login,
		                icon: "error",
		                buttonsStyling: false,
		                confirmButtonText: lang.err_login,
                        customClass: {
    						confirmButton: "btn font-weight-bold btn-light-primary"
    					}
		            }).then(function() {
						KTUtil.scrollTop();
					});
				}
		    });
        });

        // Handle cancel button
        $('#kt_login_forgot_cancel').on('click', function (e) {
            e.preventDefault();

            _showForm('signin');
        });
    }

    // Public Functions
    return {
        // public functions
        init: function() {
            _login = $('#kt_login');

            _handleSignInForm();
            _handleSignUpForm();
            _handleForgotForm();
        }
    };
}();

// Class Initialization
jQuery(document).ready(function() {
	KTLogin.init();
	if($('#error').val()=='double_username'){
		swal.fire({
			text: lang.err_dbluser,
			icon: "error",
			buttonsStyling: false,
			confirmButtonText: conf_try,
			customClass: {
				confirmButton: "btn font-weight-bold btn-light-danger"
			}
		}).then(function() {
			$('input[name ="username"]').val('');
			$('input[name ="username"]').removeClass('is-valid');
			KTUtil.scrollTop();
		});
	}else if($('#error').val()=='double_email'){
		swal.fire({
			text: lang.dblemail,
			icon: "error",
			buttonsStyling: false,
			confirmButtonText: lang.conf_try,
			customClass: {
				confirmButton: "btn font-weight-bold btn-light-danger"
			}
		}).then(function() {
			$('input[name ="email"]').val('');
			$('input[name ="email"]').removeClass('is-valid');
			KTUtil.scrollTop();
		});
	} else if ($('#error').val() == 'double_attempt') {
		swal.fire({
			text: lang.err_dblaccess,
			icon: "error",
			buttonsStyling: false,
			confirmButtonText: lang.conf_try,
			customClass: {
				confirmButton: "btn font-weight-bold btn-light-danger"
			}
		}).then(function () {
			$('input[name ="email"]').val('');
			$('input[name ="email"]').removeClass('is-valid');
			KTUtil.scrollTop();
		});
	} else if ($('#error').val() != '')  {
		swal.fire({
			text: $('#error').val(),
			icon: "error",
			buttonsStyling: false,
			confirmButtonText: lang.conf_try,
			customClass: {
				confirmButton: "btn font-weight-bold btn-light-danger"
			}
		}).then(function () {
			$('input[name ="email"]').val('');
			$('input[name ="email"]').removeClass('is-valid');
			KTUtil.scrollTop();
		});
	}
});