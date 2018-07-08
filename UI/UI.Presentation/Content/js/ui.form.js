/// <reference path="_references.js" />

var rp = rp || {};

rp.forms = (function () {

    var _validator = {
        /* START : PRIVATE METHODS */
        _getName: function (t) {

            // get the display name.
            var name = t.attr('data-validate-name');
            if (!name)
                name = t.attr('name');

            return name;
        },
        _getValue: function (t) {
            var val = t.val();
            val = val.trim();
            val = val.toLowerCase();

            return val;
        },
        _errorMsg: function (t, type, def) {
            var m = t.attr('data-validate-error-msg'); 

            if (!m)
                m = def;
            else {
                m = m.split(':');
                if (m[0].toLowerCase() === type)
                    m = m[1];
                else
                    m = def;
            }

            return m;
        },
        _isOptional: function (t) {
            var isOptional = t.attr('data-validate-required');
            if (isOptional)
                isOptional = isOptional === 'false';
            else
                isOptional = true;

            return isOptional;
        },
        _validData: function (t) {
            t = $(t);
            return {
                t: t,
                name: _validator._getName(t),
                value: _validator._getValue(t)
            }
        },

        /* START : PUBLIC METHODS */
        required: function (t) {
            var data = _validator._validData(t);

            var isValid = false;

            var msg = '';
            if (data.t.attr('type') !== 'checkbox')
                isValid = data.value.length > 0;
            else
                isValid = data.t.prop('checked') === true;

            if (!isValid && !_validator._isOptional(t))
                return {
                    name: data.name,
                    msg: _validator._errorMsg(data.t, 'required', data.name + ' is required, please enter a proper value.')
                };
        },
        email: function (t) {
            var data = _validator._validData(t);

            var rex = new RegExp(/^[A-Z0-9._%+-]+@(?:[A-Z0-9-]+\.)+[A-Z]{2,}$/i);
            if (!rex.test(data.value) && !_validator._isOptional(data.t))
                return {
                    name: data.name,
                    msg: _validator._errorMsg(data.t, 'email', 'Please enter a valid email address.')
                };
        },
        url: function (t) {
            var data = _validator._validData(t);

            var rex = new RegExp(/^([a-z][a-z0-9\*\-\.]*):\/\/(?:(?:(?:[\w\.\-\+!$&'\(\)*\+,;=]|%[0-9a-f]{2})+:)*(?:[\w\.\-\+%!$&'\(\)*\+,;=]|%[0-9a-f]{2})+)?(?:(?:[a-z0-9\-\.]|%[0-9a-f]{2})+)(?::[0-9]+)?(?:[\/|\?](?:[\w#!:\.\?\+=&@!$'~*,;\/\(\)\[\]\-]|%[0-9a-f]{2})*)?$/);
            if (!rex.test(data.value) && !_validator._isOptional(data.t))
                return {
                    name: name,
                    msg: _validator._errorMsg(t, 'url', 'Please enter a valid url address.')
                };
        },
        compare: function (t) {
            var data = _validator._validData(t);

            var target = data.t.attr('data-validate-compare');
            var compare = $('[name=' + target + ']').val();

            if (data.value !== compare)
                return {
                    name: data.name,
                    msg: _validator._errorMsg(data.t, 'compare', 'The value for ' + data.name + ' and ' + target + ' did not match.')
                }

        },
        length: function (t) {
            var data = _validator._validData(t);

            var length = data.t.attr('data-validate-length');
            length = parseInt(length);

            if (!(data.value.length >= length))
                return {
                    name: data.name,
                    msg: _validator._errorMsg(data.t, 'length', 'You need to have atleast ' + length + ' character(s) long.')
                }
        },
        lengthmax: function (t) {
            var data = _validator._validData(t);

            var lengthRange = data.t.attr('data-validate-length-range');
            if (!lengthRange)
                return {
                    name: data.name,
                    msg: 'Invalid form string length range.'
                };

            lengthRange = lengthRange.split(':');
            var length = data.value.length;
            var from = parseInt(lengthRange[0]);
            var to = parseInt(lengthRange[1]);

            if (length < from || length > to)
                return {
                    name: data.name,
                    msg: _validator._errorMsg(data.t, 'range', 'You need a range of ' + from + ' to ' + to + ' characters(s) long.')
                }

        },
        rex: function (t) {
            var data = _validator._validData(t);

            var rex = data.t.attr('data-validate-rex');
            rex = new RegExp(rex);

            if (!rex.test(data.value) && !_validator._isOptional(data.t))
                return {
                    name: data.name,
                    msg: _validator._errorMsg(data.t, 'rex', data.name + ' is not valid.')
                };
        },
        delimit: function (t) {
            var data = _validator._validData(t);

            var delimitMin = parseInt(data.t.attr('data-validate-delimit'));
            var checkDelimit = data.value.split(',');

            if (checkDelimit.length < delimitMin)
                return {
                    name: data.name,
                    msg: _validator._errorMsg(data.t, 'delimit', data.name + ' needs to have atleast ' + delimitMin + '.')
                }

        }
    };
    var _setParam = function (arrName, val, param) {
        var cur = param;

        for (var i = 0; i < (arrName.length - 1); i++) {
            if (!(arrName[i] in cur)) {
                cur[arrName[i]] = {};
            }
            cur = cur[arrName[i]]; //Move to the property for the next iteration
        }

        //set the value for the last property name
        cur[arrName[arrName.length - 1]] = val;

    }

    var doValidate = function (t) {
        t = $(t);

        var errorList = [];
        var attr = t.data();

        for (var prop in attr) {
            prop = prop.replace('validate', '').toLowerCase();
            var func = _validator[prop];

            if (func) {
                var errorMsg = func(t);
                if (errorMsg)
                    errorList.push(errorMsg);
            }

        }

        // Clean the UI from error message.
        var container = t.closest('.form-group .col-sm-9');

        if (!container[0])
            container = t.closest('.form-group .col-sm-8');

        if (!container[0])
            container = t.closest('.form-group');

        t.removeClass('input-validation-error');
        container.find('.error-container').remove();

        if (errorList.length > 0) {
            t.addClass('input-validation-error');

            var errorContainer = '<div class="error-container">';
            for (var i = 0; i < errorList.length; i++)
                errorContainer += '<div>' + errorList[i].msg + '</div>';

            errorContainer += '</div>';
            container.append(errorContainer);
        }

        return errorList;
    }

    var getPostDataFromForm = function (t, formName, visibleOnly) {
        var doProc = true;

        formName = $(formName);

        if (visibleOnly)
            formName = formName.find('[name]:visible, [name][data-post-force-add=true], [name][data-post-force-add=True]');
        else
            formName = formName.find('[name]');

        var param = {};

        formName.each(function (a, b) {
            b = $(b);

            if (b.attr('type') === 'radio' && !b.prop('checked'))
                return;

            var msg = doValidate(b);
            if (msg.length === 0) {
                var name = b.attr('name');
                name = name.split('.');

                if (name === 'undefined' || name === undefined)
                    return;
                
                var val = null;
                if (b.attr('type') === 'checkbox'){ 
                    val = b.is(':checked');
                }
                else
                    val = b.val();

                _setParam(name, val, param);
            }
            else
                doProc = false;
        });

        if (doProc === false)
            return null;

        return param;
    }

    var getPostData = function (t, visibleOnly) {
        var formName = $(t).closest('.form-box');
        return getPostDataFromForm(t, formName, visibleOnly);
    }

    var dataCheckForm = function (t) {
        t = $(t);
        if (t.hasClass('click-proc'))
            return null;

        t.addClass('click-proc');

        var visibleOnly = $(t).closest('.form-box').attr('data-visible-only');
        if (!visibleOnly)
            visibleOnly = false;

        var data = rp.forms.getPostData(t, visibleOnly);

        if (data == null) {
            toastr.error('Please make sure that the form is valid.');
            t.removeClass('click-proc');
            return null;
        } 
        return data;
    }

    var returnVals = {
        'Success': '1',
        'Failed': '0'
    };

    var methods = {
        GET: 'GET',
        POST: 'POST',
        DELETE: 'DELETE',
        PUT: 'PUT'
    }

    return {
        'returnVals': returnVals,
        'methods' : methods,
        'dataCheckForm': dataCheckForm,
        'getPostData': getPostData,
        'getPostDataFromForm': getPostDataFromForm
    }

})();