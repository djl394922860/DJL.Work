!(function ($, window, document, undefined) {
    // extend the 'equals' rule    
    $.extend($.fn.validatebox.defaults.rules, {
        equals: {
            validator: function (value, param) {
                if (value.length < 6) return false;
                return value == $(param[0]).val();
            },
            message: '重复密码必须与新密码一致!并且长度不能小于6'
        }
    });

    $.extend($.fn.validatebox.defaults.rules, {
        noEquals: {
            validator: function (value, param) {
                if (value.length < 6) return false;
                return value !== $(param[0]).val();
            },
            message: '新密码不能与原密码相同!并且长度不能小于6'
        }
    });
})(jQuery,window,document);