define(['jquery'], function (jquery) {
    var module = function ($) {
        var init = function () {
            // add visual effect
           // ariaLabelledBy();
        };

        var ariaLabelledBy = function() {
            $('[aria-labelledby]').on("focus", function() {
                var id = $(this).attr('aria-labelledby');
                $('#' + id).show();
            });
            $('[aria-labelledby]').on("blur", function () {
                var id = $(this).attr('aria-labelledby');
                $('#' + id).hide();
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