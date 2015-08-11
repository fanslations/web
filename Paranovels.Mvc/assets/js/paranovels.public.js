// knockout must load befire jquery to fix bug on IE8
define([
    'jquery',
    'toastr',
    'slideout',
    'bootstrap',
    'timeago',
    'ui/aria-visual',
    'ui/form-ajax',
    'ui/inline-edit',
    'ui/auto-resize',
    'ui/ajax-event',
    'ui/class-action',
    'ui/transform',
    'ui/modal',
    'util/update-checker'

], function ($, toastr, Slideout) {
    $(document).ready(function () {
        for (var i = 0; i < window.pnHooks.length; i++) {
            window.pnHooks[i]();
        }
        $(".timeago").timeago();
        // settings for toastr
        toastr.options = {
            "closeButton": false,
            "debug": false,
            "newestOnTop": false,
            "progressBar": false,
            "positionClass": "toast-top-full-width",
            "preventDuplicates": false,
            "onclick": null,
            "showDuration": "300",
            "hideDuration": "1000",
            "timeOut": "5000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        };
        // slideout menu
        var slideout = new Slideout({
            'panel': document.getElementById('panel'),
            'menu': document.getElementById('menu'),
            'padding': 256,
            'tolerance': 70
        });
        $('.slideout').each(function () {
            $(this).on('click', function () {
                slideout.toggle();
            });
        });

    });
    return {};
});

