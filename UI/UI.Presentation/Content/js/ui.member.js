/// <reference path="../lib/jquery/jquery-3.2.1.min.js" />
/// <reference path="../lib/toastr/toastr.min.js" /> 
/// <reference path="../lib/bootstrap/js/bootstrap.min.js" />

/// <reference path="ui.form.js" />
/// <reference path="ui.utilities.js" />

var rp = rp || {};

rp.member = (function () {

    var manager = function () {

        var create = function (t) {
            t = $(t);
            var data = null;
            if ((data = rp.forms.dataCheckForm(t)) != null) {
                $.skylo('start');
                $.skylo('set', 50);

                $.ajax({
                    url: '/member/manager/create',
                    data: data,
                    method: rp.forms.methods.POST,
                    success: function (r) {
                        $.skylo('end');
                        t.removeClass('click-proc');

                        if (r == rp.forms.returnVals.Failed) {
                            toastr.error('Failed to create a new manager, please try again later.');
                            return;
                        }

                        if (r != rp.forms.returnVals.Success) {
                            toastr.warning(r);
                            return;
                        }

                        if (r == rp.forms.returnVals.Success) {
                            toastr.success('Manager has successfully been created, please wait while we redirect you back to the dashboard.');

                            var id = data.identity || data.Identity;
                            setTimeout(function () {
                                window.location.href = '/Member/Home/ManagerDashboard/' + id;
                            }, 3500);
                        }
                    },
                    error: function () {
                        $.skylo('end');
                        t.removeClass('click-proc');

                        toastr.error('Failed to create a new manager, please try again later.');
                    }
                });

            }

        };

        var update = function (t) {
            t = $(t);
            var data = null;
            if ((data = rp.forms.dataCheckForm(t)) != null) {

                $.ajax({
                    url: '/member/manager/update',
                    data: data,
                    method: rp.forms.methods.PUT,
                    success: function (r) {
                        $.skylo('end');
                        t.removeClass('click-proc');

                        if (r == rp.forms.returnVals.Failed) {
                            toastr.error('Failed to update the manager, please try again later.');
                            return;
                        }

                        if (r != rp.forms.returnVals.Success) {
                            toastr.warning(r);
                            return;
                        }

                        if (r == rp.forms.returnVals.Success) {
                            toastr.success('Manager has successfully been updated, please wait while we redirect you back to the dashboard.');
                             
                            var id = data.identity || data.Identity;
                            setTimeout(function () {
                                window.location.href = '/Member/Home/ManagerDashboard/' + id;
                            }, 3500);
                        }
                    },
                    error: function () {
                        $.skylo('end');
                        t.removeClass('click-proc');

                        toastr.error('Failed to update the manager, please try again later.');
                    }
                });

            }
        };

        var del = function (t) { };


        return {
            'create': create,
            'update': update,
            'del': del
        }

    }();


    return {
        'manager': manager
    }

})();

$(function () {

    $('body').on('click', '[data-action=createMemberManager]', function () {
        rp.member.manager.create(this);
    });

    $('body').on('click', '[data-action=updateMemberManager]', function () {
        rp.member.manager.update(this);
    });

});