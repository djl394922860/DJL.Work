; (function ($, window, document, undefined) {
    if (window.ActiveAgain === null || window.ActiveAgain == undefined) {
        window.ActiveAgain = {};
    }

    window.ActiveAgain.checkEmail = function isEmail(str) {
        var reg = /^(\w)+(\.\w+)*@(\w)+((\.\w+)+)$/;
        return reg.test(str);
    }

    window.ActiveAgain.todo = function () {
        $('#warper div').each(function (index, ele) {
            $(ele).hide();
        });
        var email = $.trim($('#email').val());
        if (email === "") {
            $('#warning').show();
            $('#email').val('');
            return false;
        }
        if (!window.ActiveAgain.checkEmail(email)) {
            $('#danger').show();
            return false;
        }
        $.post('/Home/ActiveAgain', { email: email }, function (data) {
            if (!data.success) {
                alert(data.message);
                return false;
            }
            $('#success').show();
            $('#btnActiveAgain').attr('disabled', 'disabled');
            $('#email').val('');
        });
    }

    window.ActiveAgain.init = function () {
        document.onkeydown = function (event) {
            var e = event || window.event || arguments.callee.caller.arguments[0];
            if (e && e.keyCode == 13) { // enter 键
                window.ActiveAgain.todo();
            }
        };

        $('#btnActiveAgain').on('click', function () {
            window.ActiveAgain.todo();
        });

    }

    window.ActiveAgain.init();

})(jQuery, window, document);