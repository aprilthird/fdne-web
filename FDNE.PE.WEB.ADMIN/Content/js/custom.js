

var _app = {
    parseUrl: function (urlString) {
        var url = window.location.protocol;
        url += "//";
        url += window.location.host;
        url += this.constants.url.root;
        url += urlString;
        return url;
    },
    parseToBase64: function (file, onload) {
        if (!file) {
            onload();
        } else {
            var reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = function () {
                onload(reader.result);
            };
            reader.onerror = function (error) {
                //console.log('Error: ', error);
            }; 
        }      
    },
    constants: {
        url: {
            root: ""
        },
        formats: {
            datepicker: "dd/mm/yyyy",
            datetimepicker: "dd/mm/yyyy H:ii P",
            datepickerJsMoment: "DD/MM/YYYY",
            datetimepickerJsMoment: "DD/MM/YYYY h:mm A",
            timepickerJsMoment: "h:mm A",
            momentJsDate: "DD-MM-YYYY",
            momentJsDateTime: "DD-MM-YYYY h:mm A"
        },
        calendar: {
            dayNames: ["Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado"],
            monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Setiembre", "Octubre", "Noviembre", "Diciembre"],
            dayNamesShort: ["Dom", "Lun", "Mar", "Mié", "Jue", "Vie", "Sáb"],
            monthNamesShort: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"],
            dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"]
        }
    },
    loader: {
        showOnElement: function (selector, message) {
            $(selector).block({
                message:
                    `<div id="loader_spinner" class="d-flex align-items-center">
                        <div class="sk-circle m-0 mr-3">
                            <div class="sk-circle1 sk-child"></div>
                            <div class="sk-circle2 sk-child"></div>
                            <div class="sk-circle3 sk-child"></div>
                            <div class="sk-circle4 sk-child"></div>
                            <div class="sk-circle5 sk-child"></div>
                            <div class="sk-circle6 sk-child"></div>
                            <div class="sk-circle7 sk-child"></div>
                            <div class="sk-circle8 sk-child"></div>
                            <div class="sk-circle9 sk-child"></div>
                            <div class="sk-circle10 sk-child"></div>
                            <div class="sk-circle11 sk-child"></div>
                            <div class="sk-circle12 sk-child"></div>
                        </div>
                        ${message}
                    </div>`,
                blockMsgClass: "blockMsg d-flex justify-content-center align-items-center",
                css: {
                    backgroundColor: "rgba(0, 0, 0, .3)",
                    border: "none",
                    borderRadius: "5px",
                    color: "white",
                    paddingTop: "2rem",
                    paddingBottom: "2rem",
                    paddingRight: "2rem",
                    paddingLeft: "2rem",
                    top: "50%",
                    left: "50%",
                    transform: "translate(-50%, -50%)",
                    width: "auto",
                    zIndex: "100001"
                }
            });
        },
        hideOnElement: function (element) {
            $(selector).unblock();
        },
        show: function (message) {
            message = message || "Cargando...";
            $.blockUI({
                message: 
                    `<div id="loader_spinner" class="d-flex align-items-center">
                        <div class="sk-circle m-0 mr-3">
                            <div class="sk-circle1 sk-child"></div>
                            <div class="sk-circle2 sk-child"></div>
                            <div class="sk-circle3 sk-child"></div>
                            <div class="sk-circle4 sk-child"></div>
                            <div class="sk-circle5 sk-child"></div>
                            <div class="sk-circle6 sk-child"></div>
                            <div class="sk-circle7 sk-child"></div>
                            <div class="sk-circle8 sk-child"></div>
                            <div class="sk-circle9 sk-child"></div>
                            <div class="sk-circle10 sk-child"></div>
                            <div class="sk-circle11 sk-child"></div>
                            <div class="sk-circle12 sk-child"></div>
                        </div>
                        ${message}
                    </div>`,
                blockMsgClass: "blockMsg d-flex justify-content-center align-items-center",
                css: {
                    backgroundColor: "rgba(0, 0, 0, .3)",
                    border: "none",
                    borderRadius: "5px",
                    color: "white",
                    paddingTop: "2rem",
                    paddingBottom: "2rem",
                    paddingRight: "2rem",
                    paddingLeft: "2rem",
                    top: "50%",
                    left: "50%",
                    transform: "translate(-50%, -50%)",
                    width: "auto",
                    zIndex: "100001"
                }
            });
        },
        hide: function (message) {
            $.unblockUI();
        }
    },
    show: {
        notification: {
            success: function (message, title) {
                title = title || "Exito";
                toastr.success(message, title);
            },
            warning: function (message, title) {
                title = title || "Alerta";
                toastr.warning(message, title);
            },
            info: function (message, title) {
                title = title || "Info:";
                toastr.info(message, title);
            },
            error: function (title, message) {
                title = title || "Error";
                toastr.error(message, title);
            },
            add: {
                success: function () {
                    toastr.success("Registro ingresado correctamente.", "Éxito");
                },
                error: function() {
                    toastr.error("No se pudo completar el registro.", "Error");
                }
            },
            edit: {
                success: function () {
                    toastr.success("Cambios guardados correctamente.", "Éxito");
                },
                error: function () {
                    toastr.error("No se pudo guardar los cambios.", "Error");
                }
            },
            delete: {
                success: function () {
                    toastr.success("Registro eliminado correctamente.", "Éxito");
                },
                error: function () {
                    toastr.error("No se pudo eliminar el registro.", "Error");
                }
            }
        }
    }
};

// ----------
// jQuery Validation
// ----------
$.extend($.validator.messages, {
    accept: "Por favor, ingrese un archivo con un formato válido.",
    cifES: "Por favor, escriba un CIF válido.",
    creditcard: "Por favor, escriba un número de tarjeta válido.",
    date: "Por favor, escriba una fecha válida.",
    dateISO: "Por favor, escriba una fecha (ISO) válida.",
    digits: "Por favor, escriba sólo dígitos.",
    email: "Por favor, escriba un correo electrónico válido.",
    equalTo: "Por favor, escriba el mismo valor de nuevo.",
    extension: "Por favor, escriba un valor con una extensión permitida.",
    max: $.validator.format("Por favor, escriba un valor menor o igual a {0}."),
    maxlength: $.validator.format("Por favor, no escriba más de {0} caracteres."),
    min: $.validator.format("Por favor, escriba un valor mayor o igual a {0}."),
    minlength: $.validator.format("Por favor, no escriba menos de {0} caracteres."),
    nieES: "Por favor, escriba un NIE válido.",
    nifES: "Por favor, escriba un NIF válido.",
    number: "Por favor, escriba un número válido.",
    pattern: "Por favor, escriba un formato válido.",
    range: $.validator.format("Por favor, escriba un valor entre {0} y {1}."),
    rangelength: $.validator.format("Por favor, escriba un valor entre {0} y {1} caracteres."),
    remote: "Por favor, llene este campo.",
    required: "Este campo es obligatorio.",
    url: "Por favor, escriba una URL válida.",
    step: "Por favor, ingrese un número entero."
});

jQuery.validator.setDefaults({
    errorElement: "em",
    errorPlacement: function (error, element) {
        if (element.parent(".input-group").length) {
            error.insertAfter(element.parent()); // radio/checkbox?      
        }
        else if (element.parent(".m-input-icon").length) {
            error.insertAfter(element.parent());
        }
        else if (element.parent().parent(".m-radio-inline").length) {
            error.insertAfter(element.parent().parent());
        }
        else if (element.hasClass("select2-single")) {
            error.addClass("text-danger").insertAfter(element.next("span")); // select2
        } else {
            error.addClass('invalid-feedback');
            if (element.prop('type') === 'checkbox') {
                error.insertAfter(element.parent('label'));
            } else {
                error.insertAfter(element);
            }
            error.insertAfter(element); // default
        }
    },
    highlight: function highlight(element) {
        $(element).addClass('is-invalid').removeClass('is-valid');
    },
    unhighlight: function unhighlight(element) {
        $(element).removeClass('is-invalid');//.addClass('is-valid')
    },
    success: function (label, element) {
        $(element).removeClass('is-invalid');//.addClass('is-valid')
    }
});

// ----------
// jQuery Validation Images
// ----------
$.validator.addMethod("fileSizeBs", function (value, element, param) {
    return this.optional(element) || element.files[0].size <= param;
}, "El archivo debe pesar menos de {0} B.");

$.validator.addMethod("fileSizeKBs", function (value, element, param) {
    return this.optional(element) || element.files[0].size <= param * 1024;
}, "El archivo debe pesar menos de {0} KB.");

$.validator.addMethod("fileSizeMBs", function (value, element, param) {
    return this.optional(element) || element.files[0].size <= param * 1024 * 1024;
}, "El archivo debe pesar menos de {0} MB.");

// ----------
// Datepicker
// ----------
var dayNames = ["Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado"];
var monthNames = [
    "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Setiembre", "Octubre", "Noviembre",
    "Diciembre"
];
var dayNamesShort = ["Dom", "Lun", "Mar", "Mié", "Jue", "Vie", "Sáb"];
var dayNamesMin = ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"];
var monthNamesShort = ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"];
var todayString = "Hoy";

$.fn.datepicker.dates.es = {
    clear: "Borrar",
    days: dayNames,
    daysMin: dayNamesMin,
    daysShort: dayNamesShort,
    format: _app.constants.formats.datepicker,
    months: monthNames,
    monthsShort: monthNamesShort,
    monthsTitle: "Meses",
    today: todayString,
    weekStart: 1
};

$.fn.datepicker.defaults.autoclose = true;
$.fn.datepicker.defaults.clearBtn = true;
$.fn.datepicker.defaults.language = "es";
//$.fn.datepicker.defaults.templates = {
//    leftArrow: "<i class=\"la la-angle-left\"></i>",
//    rightArrow: "<i class=\"la la-angle-right\"></i>"
//};
$.fn.datepicker.defaults.todayHighlight = true;

// ----------
// Select2
// ----------
(function () {
    if (jQuery && jQuery.fn && jQuery.fn.select2 && jQuery.fn.select2.amd) {
        var e = jQuery.fn.select2.amd;

        return e.define("select2/i18n/es", [], function () {
            return {
                errorLoading: function () {
                    return "No se pudieron cargar los resultados";
                },
                inputTooLong: function (e) {
                    var t = e.input.length - e.maximum,
                        n = "Por favor, elimine " + t + " car";

                    return t == 1 ? n += "ácter" : n += "acteres", n;
                },
                inputTooShort: function (e) {
                    var t = e.minimum - e.input.length,
                        n = "Por favor, introduzca " + t + " car";

                    return t == 1 ? n += "ácter" : n += "acteres", n;
                },
                loadingMore: function () {
                    return "Cargando más resultados…";
                },
                maximumSelected: function (e) {
                    var t = "Sólo puede seleccionar " + e.maximum + " elemento";

                    return e.maximum != 1 && (t += "s"), t;
                },
                noResults: function () {
                    return "No se encontraron resultados";
                },
                searching: function () {
                    return "Buscando…";
                }
            };
        }), { define: e.define, require: e.require };
    }
})();

//$.fn.select2.defaults.set("ajax--delay", 1000);
$.fn.select2.defaults.set("language", "es");
$.fn.select2.defaults.set("placeholder", "---");
$.fn.select2.defaults.set("width", "100%");
$.fn.select2.defaults.set("theme", "bootstrap");

//--------------
// Toastr
//--------------
toastr.options = {
    closeButton: true,
    newestOnTop: false,
    positionClass: "toast-top-right",
    preventDuplicates: false,
    progressBar: true,
    onclick: null,
    showDuration: "300",
    hideDuration: "1000",
    timeOut: "5000",
    extendedTimeOut: "1000",
    showEasing: "easeOutBounce",
    hideEasing: "easeInBack",
    closeEasing: "easeInBack",
    showMethod: "slideDown",
    hideMethod: "slideUp",
    closeMethod: "slideUp"
};

// ----------
// DataTables
// ----------
$.extend($.fn.dataTable.defaults, {
    dom: '<"top row"<"col-md-6"B><"col-md-6"f>>rt<"bottom row"<"col-md-4"i><"col-md-4 text-center pt-2"l><"col-md-4"p>>',
    buttons: [
        { extend: "copyHtml5", text: "<i class='fa fa-copy'></i> Copiar", className: " btn-dark" },
        { extend: "excelHtml5", text: "<i class='fa fa-file-excel-o'></i> Excel", className: "btn-success" },
        { extend: "csvHtml5", text: "<i class='fa fa-file-excel-o'></i> CSV", className: "btn-success" },
        { extend: "pdfHtml5", text: "<i class='fa fa-file-pdf-o'></i> PDF", className: "btn-danger" },
        { extend: "print", text: "<i class='fa fa-print'></i> Imprimir", className: "btn-dark" }
    ],
    language: {
        "sProcessing": "<div class='m-blockui' style='display: inline; background: none; box-shadow: none;'><span>Cargando...</span><span><div class='m-loader  m-loader--brand m-loader--lg'></div></span></div>",
        "sLengthMenu": "Mostrar _MENU_ registros",
        "sZeroRecords": "No se encontraron resultados",
        "sEmptyTable": "Ningún dato disponible en esta tabla",
        "sInfo": "Mostrando _START_ - _END_ de _TOTAL_ registros",
        "sInfoEmpty": "Mostrando 0 - 0 de 0 registros",
        "sInfoFiltered": "(filtrado de _MAX_ registros)",
        "sInfoPostFix": "",
        "sSearch": "Buscar:",
        "sUrl": "",
        "sInfoThousands": ",",
        "sLoadingRecords": "Cargando...",
        //"oPaginate": {
        //    "sFirst": "<i class='la la-angle-double-left'></i>",
        //    "sLast": "<i class='la la-angle-double-right'></i>",
        //    "sNext": "<i class='la la-angle-right'></i>",
        //    "sPrevious": "<i class='la la-angle-left'></i>"
        //},
        "oAria": {
            "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
            "sSortDescending": ": Activar para ordenar la columna de manera descendente"
        }
    },
    lengthMenu: [10, 25, 50],
    orderMulti: false,
    pagingType: "full_numbers",
    responsive: true,
    info: true,
    order: []
});

// ----------------
// Bootstrap Alerts
// ----------------
$(".alert").on("close.bs.alert", function () {
    $(this).addClass("d-none");
    return false;
});