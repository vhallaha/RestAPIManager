/// <reference path="../lib/jquery/jquery-3.2.1.min.js" />
/// <reference path="../lib/toastr/toastr.min.js" /> 
/// <reference path="../lib/bootstrap/js/bootstrap.min.js" />

/// <reference path="ui.form.js" />
/// <reference path="ui.utilities.js" />

var rp = rp || {};

rp.account = (function () {

    var login = function (t) {
        t = $(t);
        var data = null
        if ((data = rp.forms.dataCheckForm(t)) != null) {

            $.skylo('start');
            $.skylo('set', 50);
            $.post('/account/home/login', data, function (r) {
                $.skylo('end');

                if (r == rp.forms.returnVals.Failed) {
                    toastr.error('Username or password did not match any record from our database.');
                    t.removeClass('click-proc');
                }

                if (r == rp.forms.returnVals.Success) {
                    toastr.success('Login success, please wait while we transfer you to your page.');

                    setTimeout(function () {
                        var href = window.location.href.toLowerCase();
                        var qs = rp.utilities.getQS('returnurl', href)

                        if (!qs)
                            window.location.href = '/dashboard/';
                        else
                            window.location.href = qs;
                    }, 3500);
                }

            });
        }

    };

    var signup = function (t) {
        t = $(t);
        var data = null
        if ((data = rp.forms.dataCheckForm(t)) != null) {

            $.skylo('start');
            $.skylo('set', 50);
            $.post('/account/home/signup', data, function (r) {
                $.skylo('end');
                t.removeClass('click-proc');

                if (r == rp.forms.returnVals.Failed) { 
                    toastr.error('Failed to create a new account, please try again later.');
                    return;
                }

                if (r != rp.forms.returnVals.Success) {
                    toastr.warning(r);
                    return;
                }

                if (r == rp.forms.returnVals.Success) {
                    var href = window.location.href.toLowerCase();
                    var qs = rp.utilities.getQS('returnurl', href)

                    if (!qs)
                        window.location.href = '/dashboard/';
                    else
                        window.location.href = qs;
                }

            });

        }

    };

    var updateProfile = function (t) {
        t = $(t);
        var data = null
        if ((data = rp.forms.dataCheckForm(t)) != null) {

            $.skylo('start');
            $.skylo('set', 50);
            $.post('/account/profile/updateprofile/', data, function (r) {
                $.skylo('end');
                t.removeClass('click-proc');

                if (r == rp.forms.returnVals.Failed)
                    toastr.error('Failed to create a new account, please try again later.');
                else if (r == rp.forms.returnVals.Success)
                    toastr.success('Profile has been successfully updated.');
                else
                    toastr.warning(r);

            });

        }
    };

    var changePassword = function (t) {
        t = $(t);
        var data = null
        if ((data = rp.forms.dataCheckForm(t)) != null) {

            $.skylo('start');
            $.skylo('set', 50);
            $.post('/account/profile/changePassword/', data, function (r) {
                $.skylo('end');
                t.removeClass('click-proc');

                if (r == rp.forms.returnVals.Failed)
                    toastr.error('Failed to change the password, please try again later.');
                else if (r == rp.forms.returnVals.Success) {
                    toastr.success('Change Password complete, logging you out...');
                    setTimeout(function () { window.location.reload(); }, 3300);
                }
                else
                    toastr.warning(r);

            });

        }
    };

    return {
        'login': login,
        'signup': signup,
        'updateProfile': updateProfile,
        'changePassword': changePassword
    }

})();

$(function () {

    $('body').on('click', '[data-action=login]', function () {
        rp.account.login(this);
    });

    $('body').on('click', '[data-action=signup]', function () {
        rp.account.signup(this);
    });

    $('body').on('click', '[data-action=updateProfile]', function () {
        rp.account.updateProfile(this);
    });

    $('body').on('click', '[data-action=changePassword]', function () {
        rp.account.changePassword(this);
    });

});