; (function ($, window, document, undefined) {
    if (window.Register === null || window.Register == undefined) {
        window.Register = {};
    }

    window.Register.changeImage = function (obj) {
        $(obj).attr('src', '/Home/ValidataCode?time' + (new Date()).getTime());
    }

    window.Register.init = function () {
        $('#formRegister').bootstrapValidator({
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
                            message: '邮箱地址格式有误,例如:123456@qq.com'
                        },
                        stringLength: {
                            min: 6,
                            max: 32,
                            message: '邮箱长度必须在6到32位之间'
                        },
                        threshold: 6,
                        remote: {
                            url: '/Home/CheckUserEmail',
                            message: '邮箱已被注册',
                            delay: 2000,
                            type: 'POST'
                        }
                    }
                },
                Name: {
                    message: '用户名验证失败',
                    validators: {
                        notEmpty: {
                            message: '用户名不能为空'
                        },
                        stringLength: {
                            min: 6,
                            max: 18,
                            message: '用户名长度必须在6到18位之间'
                        },
                        regexp: {
                            regexp: /^[a-zA-Z0-9_]+$/,
                            message: '用户名只能包含大写、小写、数字和下划线'
                        },
                        threshold: 2, //有6字符以上才发送ajax请求，（input中输入一个字符，插件会向服务器发送一次，设置限制，6字符以上才开始）
                        remote: {
                            //ajax验证。server result:{"valid",true or false} 向服务发送当前input name值，获得一个json数据。例表示正确：{"valid",true}
                            url: '/Home/CheckUserName',//验证地址
                            message: '用户已存在',//提示消息
                            delay: 2000,//每输入一个字符，就发ajax请求，服务器压力还是太大，设置2秒发送一次ajax（默认输入一个字符，提交一次，服务器压力太大）
                            type: 'POST'//请求方式
                            /**自定义提交数据，默认值提交当前input value
                             *  data: function(validator) {
                                  return {
                                      password: $('[name="passwordNameAttributeInYourForm"]').val(),
                                      whatever: $('[name="whateverNameAttributeInYourForm"]').val()
                                  };
                               }
                             */
                        }
                    }
                },
                Password: {
                    message: '密码无效',
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
                            field: 'Name',//需要进行比较的input name值
                            message: '不能和用户名相同'
                        },
                        regexp: {
                            regexp: /^[a-zA-Z0-9_\.]+$/,
                            message: '密码只能包含大写、小写、数字、斜线、点和下划线'
                        },
                        identical: {//相同
                            field: 'Password2',
                            message: '两次密码不一致'
                        }
                    }
                },
                Password2: {
                    message: '密码无效',
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
                            field: 'Name',//需要进行比较的input name值
                            message: '不能和用户名相同'
                        },
                        regexp: {
                            regexp: /^[a-zA-Z0-9_\.]+$/,
                            message: '密码只能包含大写、小写、数字、斜线、点和下划线'
                        },
                        identical: {//相同
                            field: 'Password',
                            message: '两次密码不一致'
                        }
                    }
                },
                Vcode: {
                    validators: {
                        notEmpty: {
                            message: '验证码不能为空'
                        },
                        stringLength: {
                            min: 4,
                            max: 6,
                            message: '验证码长度注意'
                        },
                    }
                }
            },
            submitHandler: function (validator, form, submitButton) {
                // Use Ajax to submit form data
                $.post(form.attr('action'), form.serialize(), function (result) {
                    // ... process the result ...
                    if (!result.success) {
                        window.Register.changeImage($('#imgCode'));
                        $('#vcode').val('');
                        $(form).data('bootstrapValidator').resetForm();
                        alert(result.message);//todo 美化
                        return false;
                    }
                    alert('注册成功,请到您的注册邮箱激活!');
                    window.location.href = result.message;
                }, 'json');
            }
        });
    }

    window.Register.init();
})(jQuery, window, document);