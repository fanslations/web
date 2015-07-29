define(['jquery', 'ui/auto-resize', 'toastr'], function (jquery, autoResize, toastr) {
    var module = function ($) {
        var init = function () {
            // add visual effect

            bindEditor();
        };

        var bindEditor = function () {

            $('[data-form-ajax]').each(function () {

                var $form = $(this);
                var json = $form.data('form-ajax');
                json.reload = json.reload == undefined;

                $('input.submit', $form).on('change', function () {
                    $form.submit();
                });

                //$form.find('input, textarea, select').first().focus();
                $form.on('submit', function (event) {

                    event.preventDefault();
                    var formData = new FormData($form[0]);
                    toastr["info"]("Saving data to server...");
                    $.ajax({
                        type: $form.attr('method'),
                        url: $form.attr('action'),
                        data: formData, //$form.serialize(),
                        contentType: false,
                        processData: false,
                        async: true,
                        cache: false,
                        success: function (data) {
                            if (json.returnUrl) {
                                window.location = json.returnUrl;
                            } else {
                                if (data.IsSuccessful) {
                                    if (json.reload) {
                                        if (data.RedirectUrl) {
                                            window.location = data.RedirectUrl;
                                        } else {
                                            window.location.reload();
                                        }
                                    }
                                } else {
                                    alert(data.ErrorMessage);
                                }
                            }
                        }
                    });
                });

            });
        };

        return {
            init: init
        };
    }(jquery);
    module.init();
    // return module
    return module;
});