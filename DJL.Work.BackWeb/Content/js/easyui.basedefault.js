!(function ($, window, document, undefined) {
    $.extend($.fn.datagrid.defaults, {
        fitColumns: true,
        pagination: true,
        rownumbers: true,
        singleSelect: true,
        loadMsg: '正在为您加载数据 loading......',
        pagePosition: 'bottom'
    });
})(jQuery, window, document);