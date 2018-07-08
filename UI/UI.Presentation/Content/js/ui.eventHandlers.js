/// <reference path="ui.utilities.js" />

$(function () {

    /* FAKE LOADER
    -----------------------------------------------------------------*/
    $.skylo('start');
    $.skylo('set', 50);

    window.onbeforeunload = function () {
        $.skylo('start');
        $.skylo('set', 80);
    };
     
    $(document).ready(function () {
        $.skylo('end');
    });

     
    /* WINDOW RESIZE
    -----------------------------------------------------------------*/
    $(window).resize(function () {
        rp.ui.nav.left.onResize();
    });

    /*  LEFT NAV TOGGLE
    -----------------------------------------------------------------*/
    $('body').on('click', '[data-action=leftNavToggle]:not(.click-proc)', function () {
        var t = $(this);
        t.addClass('click-proc');

        rp.ui.nav.left.toggle();
        t.removeClass('click-proc'); 
    });

    /*  BOOTSTRAP POP-OVERS
    -----------------------------------------------------------------*/
    $('[data-toggle="popover"]').popover().click(function () { 
        var t = this;
        setTimeout(function () {
            $(t).popover('hide');
        }, 5000);
    });

});