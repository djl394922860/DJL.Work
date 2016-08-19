; (function ($, window, document, undefined) {
    if (window.SiteSroll === null || window.SiteSroll == undefined) {
        window.SiteSroll = {};
    }

    window.SiteSroll.init = function () {
        $(window).scroll(function () {
            if ($(window).scrollTop() > 115) {
                $("#back-to-top").fadeIn('fast');
                $('#nav_menu').addClass('navbar-fixed-top');
            }
            else {
                $("#back-to-top").fadeOut('fast');
                $('#nav_menu').removeClass('navbar-fixed-top');
            }
        });

        //当点击跳转链接后，回到页面顶部位置

        $("#back-to-top").click(function () {
            $('body,html').animate({ scrollTop: 0 }, 200);
            return false;
        });
    }

    window.SiteSroll.init();
})(jQuery, window, document);