define(['jquery'], function (jquery) {
    var module = function ($) {
        var init = function () {
            // add visual effect
            autoResizeTextarea();
        };

        var autoResizeTextarea = function () {

            var resize = function (element) {
                element.css("min-height", '50px');
                element.css("height", 'auto');
                element.css("height", element[0].scrollHeight + 'px');
                element.css('overflow', 'hidden');
                element.css('transition', 'height 0.8s linear');
            }
            $('textarea').each(function () {
                var $this = $(this);
                if ($this.hasClass('auto-resize') == false) {
                    resize($this);
                    $this.on('input', function () {
                        resize($this);
                        $this.addClass('auto-resize');
                    });
                }
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