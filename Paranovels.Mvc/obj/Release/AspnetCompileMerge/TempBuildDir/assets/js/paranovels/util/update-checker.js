define(['jquery'], function (jquery) {
    var module = function ($) {
        var groupIntervalId;

        var init = function () {
            // add visual effect
            //groupIntervalId = setInterval(function () {
            //    checkGroup();
            //}, 1000 * 60 * 1); // 1 minute
        };

        var checkGroup = function () {
            $.post("/group/checkfeedupdate/5", function(data) {
                if (!data) {
                    clearInterval(groupIntervalId);
                }
            }).fail(function () {
                clearInterval(groupIntervalId);
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