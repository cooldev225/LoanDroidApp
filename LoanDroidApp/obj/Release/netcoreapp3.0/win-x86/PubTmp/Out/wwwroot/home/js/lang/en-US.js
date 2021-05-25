var lang = {
    //login page
    req_username: "Username is required",
    req_password: "Password is required",
    err_login: "Sorry, looks like there are some errors detected, please try again.",
    conf_gotit: "Ok, got it!",
    req_firstname: "First name is required",
    req_lasttname: "Last name is required",
    req_confirm: "The password confirmation is required",
    req_email: "Email is required",
    err_email: "The value is not a valid email address",
    err_match: "The password and its confirm are not the same",
    req_accept: "You must accept the terms and conditions",
    err_dbluser: "Username you typed is already exist.",
    conf_try: "Please, try again!",
    err_dblemail: "Email you typed is already exist.",
    err_dblaccess: "You are Logged in on other devices.",
    //
    success_saved: "Successfully saved!",

    //loanrequest
    amount: "Amount",
    frequently: "Frequently",
    capital: "Capital",
    interest: "Interest",
    dues: "Dues",
    balance: "Balance",
    date: "Date",
    times: "Times",
    status: "Status",
    investor: "Investor",
    savingRate: "Saving Rate",
    saving: "Saving",
    paynow: "Pay now",
    paid: "Paid",
    notnow: "Not now",
    questions: "Questions",
    milestones: "Milestones",
    reply:"Reply",

    global_tbl_action: "Action",
    global_tbl_createddate: "Created Date",
    global_tbl_updateddate: "Updated Date",

    New: "New",
    Representante_Processing: "Processing of Representante",
    Representante_Rejected: "Rejected by Representante",
    Contactor_Checking: "Checking of Contactor",
    Contactor_Rejected: "Rejected by Contactor",
    Service_Processing: "Processing of Service manager",
    Service_Rejected: "Rejected by Service manager",
    Debug_Processing: "Processing of Debugger",
    Debug_Rejected: "Rejected by Debugger",
    Collection_Processing: "Processing of Collection",
    Investor_Piad: "Paid from investor",
    Interesting_Process: "Processing of interst",
    Interesting_completed: "Completed request",
    Interesting_Incompleted: "Incompleted request",

    SEMANAL:"WEEKLY",
    QUINCENAL:"BIWEEKLY",
    MENSUAL:"MONTHLY",
    DIARIO:"DAILY",
};

const trans_pagination = {
    records: {
        processing: 'Loading...',
        noRecords: 'there is no record',
    },
    toolbar: {
        pagination: {
            items: {
                default: {
                    first: 'First',
                    prev: 'Previous',
                    next: 'Next',
                    last: 'Last',
                    more: 'More pages',
                    input: 'Page number',
                    select: 'Page size',
                },
                info: 'showing {{start}} - {{end}} of {{total}} records',
            },
        },
    },
};
