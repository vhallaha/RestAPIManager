/// <reference path="_references.js" />

var rp = rp || {};

rp.utilities = (function () {

    var getQS = function (name, url) {
        if (!url) url = window.location.href;
        name = name.replace(/[\[\]]/g, "\\$&");
        var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
            results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, " "));
    }

    var init = function () {

        var leftnav = function () {

            $('[data-action=scrollbar]').perfectScrollbar();

        };

        return {
            'leftnav': leftnav
        }

    }();
    
    return {
        'getQS': getQS,
        'init': init
    };
})();

rp.ui = (function () {

    var nav = function () {

        var left = function () {

            var open = function(){
                var wrapper = $('.nav-wrapper');

                if (wrapper.hasClass('.nav-close'))
                    return;

                wrapper.removeClass('nav-close');
                wrapper.addClass('nav-open');
            };

            var close = function () {
                var wrapper = $('.nav-wrapper'); 
                wrapper.addClass('nav-close');
                wrapper.removeClass('nav-open');
            };

            var toggle = function () { 
                var wrapper = $('.nav-wrapper');
                wrapper.toggleClass('nav-close');
                wrapper.toggleClass('nav-open');
            };

            var onResize = function () {
                var wrapper = $('.nav-wrapper');
                var width = $(window).width();

                if (width <= 500 && !wrapper.hasClass('nav-open'))
                    close();
            };

            return {
                'open' : open,
                'close': close,
                'toggle': toggle,
                'onResize': onResize
            }

        }();

        return {
            'left': left
        }

    }();

    return {
        'nav' : nav
    }
})();

/* INITIALIZE CONTROLS
--------------------------------------------*/
rp.utilities.init.leftnav();


toastr.options = {
    "closeButton": false,
    "debug": false,
    "newestOnTop": false,
    "progressBar": true,
    "positionClass": "toast-top-right",
    "preventDuplicates": false,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
};

$.skylo({
    'state': 'info',
    'inchSpeed': 200,
    'initialBurst': 5,
    'flat': false
});
