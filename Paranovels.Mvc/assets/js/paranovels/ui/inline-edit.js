define(['jquery', 'ui/auto-resize'], function (jquery, autoResize) {
    var module = function ($) {
        var init = function () {
            // add visual effect
            bindEditor();
        };

        var bindEditor = function () {

            $('[data-inline-edit]').each(function () {

                var $this = $(this);
                var json = $this.data('inline-edit');
                var inlineEditPostUrl = json.PostUrl || $this.data('posturl') || '/' + location.pathname.split("/")[1] + '/inlineedit/';

                $this.on('click', function () {

                    $.post(inlineEditPostUrl, json, function(html) {
                        $this.after(html);

                        $('#' + json.InlineEditProperty + 'Cancel').on('click', function () {
                            $('#' + json.InlineEditProperty + 'Container').remove();
                            $this.removeClass("on");
                        });

                        var $form = $('#' + json.InlineEditProperty + 'Form');
                        $form.find('input, textarea, select').first().focus();
                        $form.on('submit', function(event) {
             
                                event.preventDefault();
                                var formData = new FormData($form[0]);
                              
                                $.ajax({
                                    type: $form.attr('method'),
                                    url: $form.attr('action'),
                                    data: formData, //$form.serialize(),
                                    contentType: false,
                                    processData: false,
                                    async: false,
                                    cache: false,
                                    success: function(data) {
                                        window.location.reload();
                                    }
                                });
                        });
                    }).done(function (data) {
                        //console.log('done', data);
                        $this.addClass("on"); // hide "improve this" link
                        autoResize.init(); // auto resize textarea
                    }).fail(function (xhr) {
                        //console.log('fail', xhr);
                        alert('A error occur!');
                    });;
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