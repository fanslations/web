define(['jquery'], function (jquery) {
    var module = function ($) {
        var init = function () {
            // modal
            $(document).on('click', '[data-modal]', function (evt) {
                evt.preventDefault();

                var json = $(this).data('modal');

                $('#globalmodal').remove();
                var container = $('<div id="globalmodal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="global modal" aria-hidden="true">');
                $('body').append(container);

                if (json.css) {
                    container.css(json.css);
                }
                container.html('<div class="modal-dialog"><div class="modal-content"></div></div>');
                //container.load($(this).attr('href'));
                container.modal({
                    remote: $(this).attr('href'),
                    show: true
                });
                // force browser to redraw container to get correct size of chart
                container.hide().show();
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