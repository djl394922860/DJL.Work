; (function ($, window, document, undefined) {
    if (window.Login === null || window.Login == undefined) {
        window.Login = {};
    }

    window.Login.init = function () {
        $('#formLogin').bootstrapValidator({
            message: 'This value is not valid',
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            fields: {
                Email: {
                    validators: {
                        notEmpty: {
                            message: '邮箱不能为空'
                        },
                        emailAddress: {
                            message: '邮箱地址格式有误'
                        }
                    }
                },
                Password: {
                    validators: {
                        notEmpty: {
                            message: '密码不能为空'
                        },
                        stringLength: {
                            min: 6,
                            max: 18,
                            message: '用户名长度必须在6到18位之间'
                        },
                        different: {//不能和用户名相同
                            field: 'Email',//需要进行比较的input name值
                            message: '不能和邮箱相同'
                        }
                    }
                }
            },
            submitHandler: function (validator, form, submitButton) {
                // Use Ajax to submit form data
                $.post(form.attr('action'), form.serialize(), function (result) {
                    // ... process the result ...
                    if (!result.success) {
                        $(form).data('bootstrapValidator').resetForm();
                        //alert(result.message);
                        $('#alert').find('p').text(result.message);
                        $('#alert').css('display', 'block');
                        return false;
                    }
                    window.location.href = result.message;
                }, 'json');
            }
        });
    }

    window.Login.baseInit = function () {
        window.Login.init();
        $('#alert').find('span').click(function () {
            $('#alert').css('display', 'none');
        });
    }

    window.Login.baseInit();

})(jQuery, window, document);