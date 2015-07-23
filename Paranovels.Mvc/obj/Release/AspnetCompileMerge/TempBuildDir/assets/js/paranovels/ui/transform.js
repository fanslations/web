define(['jquery'], function (jquery) {
    var module = function ($) {
        var cache = [];
        var userIds = [];
        var init = function () {
            setDefaults();

            $("[data-transform]").each(function () {
                var json = $(this).data('transform');

                if (json.type == 'user') {
                    if ($.inArray(json.id, userIds) == -1) {
                        userIds.push(json.id);
                    }
                }
            });

            console.log('userIds', userIds);
            $.ajax({
                type: 'POST',
                url: '/query/users',
                data: JSON.stringify({ UserIDs: userIds }), //$form.serialize(),
                dataType: 'json',
                contentType: 'application/json;charset=UTF-8',
                processData: false,
                async: false,
                cache: false,
            }).done(function (responseData) {
                for (var i = 0; i < responseData.length; i++) {
                    if (cache['user' + responseData[i].UserID] == null) {
                        cache['user' + responseData[i].UserID] = responseData[i] || { FirstName: "", LastName: "" };
                    }
                }
                transform();
            });
        };

        var transform = function() {

            $("[data-transform]").each(function () {
                var json = $(this).data('transform');
                $(this).html(manual(json));
                $(this).removeAttr('data-transform');
            });
        };

        var manual = function(json) {
            if (json.type == 'user') {
                return cache[json.type + json.id].FirstName + ' ' + cache[json.type + json.id].LastName;
            }
        };

        var setDefaults = function() {
            cache['user0'] = { FirstName: "Fanslations", LastName: "" };
        };

        return {
            init: init,
            manual: manual
        };
    }(jquery);
    module.init();
    // return module
    return module;
});