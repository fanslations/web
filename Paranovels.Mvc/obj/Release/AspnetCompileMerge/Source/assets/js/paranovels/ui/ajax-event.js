define(['jquery', 'toastr'], function (jquery, toastr) {
    var module = function ($) {
        // variables
        var sharedKey = "ajaxevent",
            // functions
            init = function () {
                $(document).ajaxError(function (event, jqxhr, settings, thrownError) {
                    //console.log('error' + new Date().getTime());
                    //console.log(event);
                    //console.log(jqxhr);
                    //console.log(settings);
                    require(['util/logger'], function (logger) {
                        if (!logger.isLogAjaxError(settings)) {
                            logger.logAjaxError(event, jqxhr, settings);
                        }
                    });
                });

                $(document).ajaxSend(function (event, jqxhr, settings) {
                    //console.log('send' + new Date().getTime());
                    //console.log(event);
                    //console.log(jqxhr);
                    //console.log(settings);

                    var $invoker = $(event.target.activeElement);
                    if ($invoker.is("button, a") || $invoker.hasClass("loading")) {
                        if (!$invoker.hasClass(sharedKey)) {
                            $invoker.addClass(sharedKey);
                            $invoker.prop('disabled', true);
                            $invoker.data(sharedKey, $invoker.html());
                            $invoker.html('<i class="fa fa-spinner fa-spin"></i>Loading...');
                        }
                    }
                });

                $(document).ajaxComplete(function (event, jqxhr, settings) {
                    setTimeout(function () {
                        //console.log('complete' + new Date().getTime());
                        //console.log(event);
                        //console.log(jqxhr);
                        //console.log(settings);

                        var $invoker = $('.' + sharedKey);
                        if ($invoker.length == 1) {
                            $invoker.removeClass(sharedKey);
                            $invoker.prop('disabled', false);
                            $invoker.html($invoker.data(sharedKey));
                            $invoker.removeData(sharedKey);

                        }
                    }, 100);
                });

                $(document).ajaxStart(function () {
                    $('#ajaxstatus').slideDown();
                });

                $(document).ajaxStop(function () {
                    $('#ajaxstatus').slideUp();
                });
            };
        return {
            init: init
        }
    }(jquery);
    //module.init();
    // nothing to return
    return module;
});