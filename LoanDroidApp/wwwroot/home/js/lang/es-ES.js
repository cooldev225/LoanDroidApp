var lang = {
    //login page
    req_username: "El nombre de usuario es obligatorio",
    req_password: "Se requiere contraseña",
    err_login: "Lo siento, parece que se han detectado algunos errores. Vuelve a intentarlo",
    conf_gotit: "Ok, ¡entendido!",
    req_firstname: "El nombre es obligatorio",
    req_lasttname: "El apellido es obligatorio",
    req_confirm: "Se requiere la confirmación de la contraseña",
    req_email: "Se requiere correo electrónico",
    err_email: "El valor no es una dirección de correo electrónico válida",
    err_match: "La contraseña y su confirmación no son lo mismo",
    req_accept: "Debe aceptar los términos y condiciones",
    err_dbluser: "El nombre de usuario que escribiste ya existe",
    conf_try: "¡Por favor, inténtalo de nuevo!",
    err_dblemail: "El correo electrónico que escribió ya existe",
    err_dblaccess: "Has iniciado sesión en otros dispositivos",
    //
    success_saved: "Guardado con éxito!",

    //loanrequest
    amount: "Cantidad",
    frequently: "Frecuencia",
    capital: "Capital",
    interest: "Interes",
    dues: "Cuotas",
    balance: "Balance",
    date: "Fecha",
    times: "Tiempo",
    status: "Status",
    investor: "Inversora",
    savingRate: "Tasa de ahorro",
    paynow: "Paga ahora",
    paid: "Pagado",
    notnow: "Ahora no",

    global_tbl_action: "Acción",
    global_tbl_createddate: "Fecha de creación",
    global_tbl_updateddate: "Fecha actualizada",

    New: "Nuevo",
    Contactor_Checking: "Comprobación de contactor",
    Contactor_Rejected: "Rechazado por contactor",
    Service_Mapping: "Mapeo por administrador de servicio",
    Service_rejected: "Rechazado por el gerente de servicio",
    Debug_Processing: "Procesamiento de depurador",
    Debug_rejected: "Rechazado por el depurador",
    Collection_Processing: "Procesamiento de la colección",
    Investor_Piad: "Pagado por el inversor",
    Interesting_Process: "Procesamiento de interés",
    Interesting_completed: "Solicitud completada",
    Interesting_Incompleted: "Solicitud incompleta",

    SEMANAL: "SEMANAL",
    QUINCENAL: "QUINCENAL",
    MENSUAL: "MENSUAL",
    DIARIO: "DIARIO",
};

const trans_pagination = {
    records: {
        processing: 'Cargando...',
        noRecords: 'no hay registros.',//'No se encontrarón archivos',
    },
    toolbar: {
        pagination: {
            items: {
                default: {
                    first: 'Primero',
                    prev: 'Anterior',
                    next: 'Siguiente',
                    last: 'Último',
                    more: 'Páginas malas',
                    input: 'Número de página',
                    select: 'Seleccionar tamaño de página',
                },
                info: 'Demostración {{start}} - {{end}} de {{total}} registros',
            },
        },
    },
};
