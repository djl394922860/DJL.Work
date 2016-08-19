; (function ($, window, document, undefined) {
    if (window.Forget === null || window.Forget == undefined) {
        window.Forget = {};
    }

    window.Forget.checkEmail = function isEmail(str) {
        var reg = /^(\w)+(\.\w+)*@(\w)+((\.\w+)+)$/;
        return reg.test(str);
    }

    window.Forget.todo = function () {
        $('#warper div').each(function (index, ele) {
            $(ele).hide();
        });
        var email = $.trim($('#email').val());
        if (email === "") {
            $('#warning').show();
            $('#email').val('');
            return false;
        }
        if (!window.Forget.checkEmail(email)) {
            $('#danger').show();
            return false;
        }
        $.post('/Home/Forget', { email: email }, function (data) {
            if (!data.success) {
                alert(data.message);
                return false;
            }
            $('#success').show();
            $('#btnForget').attr('disabled', 'disabled');
            $('#email').val('');
        });
    }

    window.Forget.init = function () {

        document.onkeydown = function (event) {
            var e = event || window.event || arguments.callee.caller.arguments[0];
            if (e && e.keyCode == 13) { // enter 键
                window.Forget.todo();
            }
        };

        $("#btnForget").click(function () {
            window.Forget.todo();
        });

    }

    window.Forget.init();
})(jQuery, window, document);