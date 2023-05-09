

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
                error: function () {
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
        else if (element.hasClass("select2")) {
            error.insertAfter(element.next("span")); // select2
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
        $(element).addClass('is-valid').removeClass('is-invalid');
    }
});


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

var oldHtml = $.fn.html;
$.fn.html = function () {
    try {
        let object = JSON.parse(arguments[0]);
        if (object) {
            arguments[0] = object.message;
        }
    }
    catch (e) {
        console.log("Text Not Json");
    }
    oldHtml.apply(this, arguments);
    let result = oldHtml.apply(this, arguments);
    return result;
};

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




let color = '#e0311e';
let maxParticles = 150;

// ParticlesJS Config.
particlesJS('particles-js', {
    'particles': {
        'number': {
            'value': maxParticles,
            'density': {
                'enable': true,
                'value_area': 800
            }
        },
        'color': {
            'value': color
        },
        'shape': {
            'type': 'circle',
            'stroke': {
                'width': 0,
                'color': '#000000'
            },
            'polygon': {
                'nb_sides': 5
            },
        },
        'opacity': {
            'value': 0.5,
            'random': false,
            'anim': {
                'enable': false,
                'speed': 1,
                'opacity_min': 0.1,
                'sync': false
            }
        },
        'size': {
            'value': 3,
            'random': true,
            'anim': {
                'enable': false,
                'speed': 40,
                'size_min': 0.1,
                'sync': false
            }
        },
        'line_linked': {
            'enable': true,
            'distance': 150,
            'color': color,
            'opacity': 1,
            'width': 1
        },
        'move': {
            'enable': true,
            'speed': 2,
            'direction': 'none',
            'random': false,
            'straight': false,
            'out_mode': 'out',
            'bounce': false,
            'attract': {
                'enable': false,
                'rotateX': 600,
                'rotateY': 1200
            }
        }
    },
    'interactivity': {
        'detect_on': 'canvas',
        'events': {
            'onhover': {
                'enable': true,
                'mode': 'grab'
            },
            'onclick': {
                'enable': true,
                'mode': 'push'
            },
            'resize': true
        },
        'modes': {
            'grab': {
                'distance': 140,
                'line_linked': {
                    'opacity': 1
                }
            },
            'bubble': {
                'distance': 400,
                'size': 40,
                'duration': 2,
                'opacity': 8,
                'speed': 3
            },
            'repulse': {
                'distance': 200,
                'duration': 0.4
            },
            'push': {
                'particles_nb': 4
            },
            'remove': {
                'particles_nb': 2
            }
        }
    },
    'retina_detect': true
});