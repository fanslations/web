define(['jquery', 'toastr'], function (jquery, toastr) {
    var module = function ($) {
        var init = function () {
            // add visual effect
            actionSubmit();
            actionToggle();
            actionVote();
            actionRead();
            actionRate();
            actionConnect();
        };

        var actionSubmit = function () {
            $('select.action.submit').each(function () {
                $(this).on("change", function () {
                    $(this).closest('form').submit();
                });
            });
        }
        var actionToggle = function () {
            $('.action.toggle').each(function () {
                var $control = $(this);
                var json = $control.data('toggle');
                // turn on the switch
                $(json.target).each(function () {
                    var $target = $(this);
                    if ($target.is(':visible')) {
                        $control.addClass('on');
                    } else {
                        $control.removeClass('on');
                    }

                    $control.on('click', function () {
                        if ($target.is(':visible')) {
                            $target.removeClass('fadeIn').addClass('fadeOut').hide();
                            $target.removeClass('on');
                            $control.removeClass('on');
                            // sync the controls
                            $('.action.toggle.on').each(function () {
                                var json2 = $(this).data('toggle');
                                if (json2.target == '#' + $target.attr('id')) {
                                    $(this).removeClass('on');
                                }
                            });
                        } else {
                            $target.removeClass('fadeOut').addClass('fadeIn').show();
                            $target.addClass('on');
                            $control.addClass('on');
                            // sync the controls
                            $('.action.toggle').each(function () {
                                var json2 = $(this).data('toggle');
                                if (json2.target == '#' + $target.attr('id')) {
                                    $(this).addClass('on');
                                }
                            });
                        }
                    });
                });
            });
        };

        var actionVote = function () {
            $('.action.vote-up, .action.vote-down').each(function () {
                var $this = $(this);
                var json = $this.data('vote');
                $this.on('click', function () {
                    $.ajax({
                        type: 'POST',
                        url: '/vote/form',
                        data: JSON.stringify(json), //$form.serialize(),
                        dataType: 'json',
                        contentType: 'application/json;charset=UTF-8',
                        processData: false,
                        //async: false,
                        cache: false,
                    }).done(function (responseData) {
                        // for up vote
                        if ($this.hasClass('vote-up')) {
                            if (json.Vote == 1) {
                                $this.closest('.vote').removeClass('dislikes').addClass('likes');
                                json.Vote = 0;
                            } else {
                                $this.closest('.vote').removeClass('dislikes').removeClass('likes');
                                json.Vote = 1;
                            }
                        }
                        // for down vote
                        if ($this.hasClass('vote-down')) {
                            if (json.Vote == -1) {
                                $this.closest('.vote').removeClass('likes').addClass('dislikes');
                                json.Vote = 0;

                            } else {
                                $this.closest('.vote').removeClass('likes').removeClass('dislikes');
                                json.Vote = -1;
                            }
                        }
                        $this.data('vote', json);
                        $this.siblings('.score').text(responseData);
                    });
                });
            });
        };

        var actionRate = function () {
            $('.action.rate').each(function () {
                var $this = $(this);
                var json = $this.data('rate');
                $this.on('click', function () {
                    $.ajax({
                        type: 'POST',
                        url: '/rate/form',
                        data: JSON.stringify(json), //$form.serialize(),
                        dataType: 'json',
                        contentType: 'application/json;charset=UTF-8',
                        processData: false,
                        //async: false,
                        cache: false,
                    }).done(function (responseData) {
                        toastr["success"]("Successful!");

                    });
                });
            });
        };

        var actionRead = function () {
            $('.action.read').each(function () {
                var $this = $(this);
                var json = $this.data('read');
                $this.on('click', function () {
                    $.ajax({
                        type: 'POST',
                        url: '/read/form',
                        data: JSON.stringify(json), //$form.serialize(),
                        dataType: 'json',
                        contentType: 'application/json;charset=UTF-8',
                        processData: false,
                        //async: false,
                        cache: false,
                    }).done(function (responseData) {
                        toastr["success"]("Successful!");
                        $this.parent('.read').removeClass('no').addClass('yes');
                        $this.find('.fa').removeClass('fa-eye').addClass('fa-check');
                    });
                });
            });
        };

        var actionConnect = function () {
            $('.action.connect').each(function () {
                var $this = $(this);
                var json = $this.data('connect');
                $this.on('click', function () {
                    $.ajax({
                        type: 'POST',
                        url: '/connector/form',
                        data: JSON.stringify(json), //$form.serialize(),
                        dataType: 'json',
                        contentType: 'application/json;charset=UTF-8',
                        processData: false,
                        //async: false,
                        cache: false,
                    }).done(function (responseData) {
                        toastr["success"]("Successful!");
                        window.location.reload(true);
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