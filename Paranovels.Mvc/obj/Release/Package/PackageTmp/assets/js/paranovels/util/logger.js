define(['jquery'], function (jquery) {
    var module = function ($) {
        // variables
        var logAjaxErrorUrl = "/api/logservice/a/logajaxerror",
            // functions
            isLogAjaxError = function (settings) {
                return settings.url == logAjaxErrorUrl;
            },

            logAjaxError = function (event, jqxhr, settings) {
                var error = {};
                error.DocumentTitle = event.target.title;
                error.DocumentUrl = event.target.URL;
                error.DocumentReferrerUrl = event.target.referrer;
                error.DocumentDomain = event.target.domain;

                error.AjaxUrl = settings.url;
                error.AjaxType = settings.type;
                error.AjaxContentType = settings.contentType;
                error.AjaxData = settings.data;

                console.log(jqxhr);

                error.ErrorStatus = jqxhr.status;
                error.ErrorStatusText = jqxhr.statusText;
                error.ErrorExceptionMessage = jqxhr.responseJSON.ExceptionMessage;
                error.ErrorExceptionType = jqxhr.responseJSON.ExceptionType;
                error.ErrorMessage = jqxhr.responseJSON.Message;
                error.ErrorStackTrace = jqxhr.responseJSON.StackTrace;

                error.ClientUserAgent = navigator.userAgent;
                error.ClientHostname = $('#clientinfo').data('hostname');
                error.ClientScreenResolution = $(window).width() + 'x' + $(window).height();

                //$.ajax({
                //    url: logAjaxErrorUrl,
                //    type: "POST",
                //    data: JSON.stringify(error),
                //    contentType: "application/json;charset=utf-8"
                //});
            };
        return {
            isLogAjaxError: isLogAjaxError,
            logAjaxError: logAjaxError
        }
    }(jquery);
    // nothing to return
    return module;
});

