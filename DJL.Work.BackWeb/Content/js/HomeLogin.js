!(function ($, window, document, undefined) {
    if (window.AdminLogin === undefined || window.AdminLogin === null) {
        window.AdminLogin = {};
    }

    window.AdminLogin.formInit = function () {
        var validator = $("#formLogin").validate({
            rules: {
                AdminName: {
                    required: true,
                    minlength: 1,
                    maxlength: 32
                },
                AdminPwd: {
                    required: true,
                    minlength: 6,
                    maxlength: 18
                },
                ValidataCode: {
                    required: true,
                    minlength: 4,
                    maxlength: 6,
                    number: true
                }
            },
            messages: {
                AdminName: {
                    required: '*帐号是必须的',
                    minlength: '*字符不能少一个',
                    maxlength: '*字符不能大于32个'
                },
                AdminPwd: {
                    required: '*密码是必须的',
                    minlength: '*字符不能少6个',
                    maxlength: '*字符不能大于18个'
                },
                ValidataCode: {
                    required: '*验证码是必须的',
                    minlength: '*字符不能少4个',
                    maxlength: '*字符不能大于6个',
                    number: '*请输入正确的验证码'
                }
            },
            errorPlacement: function (error, element) {
                if (element.attr("name") === "ValidataCode")
                    error.insertAfter("#imgCode");
                else
                    error.insertAfter(element);
            },
            submitHandler: function (form) {
                $.post('/Home/Login', $(form).serialize(), function (data) {
                    if (data.success) {
                        window.location.href = data.message;
                    } else {
                        $('#imgCode').attr('src', '/Home/GetCodeImg?v=' + Math.random());
                        $('#txtCode').val('');
                        alert(data.message);
                    }
                })
            }
        });
        $('#formLogin input[type="reset"]').click(function () {
            validator.resetForm();
        });
    }

    window.AdminLogin.imgCodeInit = function () {
        $('#imgCode').click(function () {
            var src = '/Home/GetCodeImg?v=' + Math.random();
            $(this).attr('src', src);
        });
    }

    window.AdminLogin.init = function () {
        window.AdminLogin.formInit();
        window.AdminLogin.imgCodeInit();
    }

    window.AdminLogin.init();
})(jQuery, window, document);