requirejs.config({
    waitSeconds: 1000,
    //urlArgs: "bust=" + (new Date()).getTime(),
    //appDir: "/Content/Main",
    enforceDefine: true,
    baseUrl: "/assets/js",
    paths: {
        /* Path */
        'ui': "paranovels/ui",
        'util': "paranovels/util",
        /* Load cdn. On fail, load local file. */
        'jquery': '../bower_components/jquery/dist/jquery',
        'angular': '../bower_components/angularjs/angular',
        'bootstrap': '../bower_components/bootstrap/dist/js/bootstrap',
        'marked': '../bower_components/marked/lib/marked',
        'tagsinput': '../bower_components/tagsinput/dist/bootstrap-tagsinput',
        'typeahead': '../bower_components/typeahead.js/dist/typeahead.jquery',
        'bloodhound': '../bower_components/typeahead.js/dist/bloodhound',
        'timeago': '../bower_components/jquery-timeago/jquery.timeago',
        'selectize': '../bower_components/selectize/dist/js/standalone/selectize',
        'toastr': '../bower_components/toastr/toastr',
        'slideout': '../bower_components/slideout.js/dist/slideout'
    },
    shim: {
        'jquery': {
            exports: '$',
            init: function () {
                //Using a function allows you to call noConflict for
                //libraries that support it, and do other cleanup.
                //However, plugins for those libraries may still want
                //a global. "this" for the function will be the global
                //object. The dependencies will be passed in as
                //function arguments. If this function returns a value,
                //then that value is used as the module export value
                //instead of the object found via the 'exports' string.
                return this.$.noConflict();
            }
        },
        'angular': {
            exports: 'angular',
            init: function() {}
        },
        'bootstrap': {
            exports: '$',
            deps: ['jquery']
        },
        'tagsinput': {
            exports: '$',
            deps: ['jquery']
        },
        'timeago': {
            exports: '$',
            deps: ['jquery']
        },
        'selectize': {
            exports: '$',
            deps: ['jquery']
        },
    }
});

requirejs.onError = function (err) {
    console.log(err);
    if (err.requireType === 'timeout') {
        console.log('modules: ' + err.requireModules);
    }
};
