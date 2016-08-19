!(function ($, window, document, undefined) {
    if (window.Admin === undefined || window.Admin === null) {
        window.Admin = {};
    }

    window.Admin.enableValidation = function () {
        $('#txtOldPwd').textbox({ required: true });
        $('#txtNewPwd').textbox({ required: true });

        $('#txtConfirmPwd').textbox({ required: true });

    }

    window.Admin.disableValidation = function () {
        $('#txtOldPwd').textbox({ required: false });
        $('#txtNewPwd').textbox({ required: false });

        $('#txtConfirmPwd').textbox({ required: false });

    }

    window.Admin.initChangeInfo = function () {
        $('#hidIsUpdatedPwd').val('false');
        window.Admin.disableValidation();
        $('#wrap').hide();
        $('#divHeaderMenu a:first').click(function () {
            $.post('/Home/GetAdminInfo', {}, function (data) {
                if (!data.success) {
                    $.messager.alert("消息", data.message, 'info');
                    return false;
                }
                var admin = JSON.parse(data.message);
                $('#txtName').textbox('setText', admin.UserName);
                $('#txtName').textbox('setValue', admin.UserName);

                $('#txtEmail').textbox('setText', admin.Email);
                $('#txtEmail').textbox('setValue', admin.Email);

                $('#txtRemark').textbox('setText', admin.Remark);
                $('#txtRemark').textbox('setValue', admin.Remark);
                $('#hidId').val(admin.Id);
                $('#divInfoDialog').dialog('open');
            }, 'json');
        });
        $('#sb').switchbutton({
            onChange: function (checked) {
                if (checked === true) {
                    window.Admin.enableValidation();
                    $('#wrap').show('normal');
                    $('#hidIsUpdatedPwd').val('true');
                } else {
                    window.Admin.disableValidation();
                    $('#wrap').hide('normal');
                    $('#hidIsUpdatedPwd').val('false');
                }
            }
        });
    }

    window.Admin.init = function () {
        window.Admin.initMenu();
        window.Admin.initChangeInfo();
        window.Admin.initTabMenu();
    }

    window.Admin.save = function () {
        if (!$('#formEditInfo').form('validate')) {
            return false;
        }
        $.messager.confirm('确认对话框', '您想要修改吗？', function (r) {
            if (r) {
                $.post('/Home/UpdateAdminInfo', $('#formEditInfo').serialize(), function (data) {
                    $('#divInfoDialog').dialog('close');
                    if (!data.success) {
                        $.messager.show({
                            title: '警告',
                            msg: data.message,
                            timeout: 3000,
                            showType: 'slide'
                        });
                        return false;
                    }
                    $('#span_adminName').text('欢迎：' + data.message);
                    $.messager.show({
                        title: '消息',
                        msg: '修改成功',
                        timeout: 3000,
                        showType: 'slide'
                    });
                }, 'json');
            }
        });
    }

    window.Admin.initMenu = function () {
        $('#divMenu li a').click(function () {
            $('#divMenu li a').each(function (index, element) {
                $(element).removeClass('selected');
            });
            $(this).addClass('selected');
            var title = $(this).text();
            var url = $(this).attr('href');
            var tab = $('#divTabs');
            if (tab.tabs('exists', title)) {
                tab.tabs('select', title);
                return false;
            }
            tab.tabs('add', {
                title: title,
                href: url,
                closable: true,
                justified: true
            });
            return false;
        });
    }

    window.Admin.initTabMenu = function () {
        var tab = $('#divTabs');
        tab.tabs({
            onContextMenu: function (e, title, index) {
                //该方法通知浏览器不要执行与此事件关联的默认动作
                //即屏蔽了浏览器在tab页上的鼠标右键事件
                e.preventDefault();
                //显示右键菜单
                if (index > 0) {
                    var divTabMenus = $("#divTabMenus");
                    divTabMenus.menu('show', {
                        left: e.pageX,
                        top: e.pageY
                    }).data("tabTitle", title);
                    //为右键菜单选项绑定事件
                    divTabMenus.menu({
                        onClick: function (item) {
                            window.Admin.closeTab(this, item.name);
                        }
                    });
                }
            }
        });
    }

    window.Admin.closeTab = function (menu, type) {
        var divTab = $('#divTabs');
        var allTabs = divTab.tabs('tabs');
        var allTabtitle = [];
        $.each(allTabs, function (i, n) {
            var opt = $(n).panel('options');
            if (opt.closable)
                allTabtitle.push(opt.title);
        });
        var curTabTitle = $(menu).data("tabTitle");
        var curTabIndex = divTab.tabs("getTabIndex", divTab.tabs("getTab", curTabTitle));
        type = parseInt(type);
        switch (type) {
            case 1://关闭当前
                divTab.tabs("close", curTabIndex);
                return false;
                break;
            case 2://全部关闭
                for (var i = 0; i < allTabtitle.length; i++) {
                    divTab.tabs('close', allTabtitle[i]);
                }
                break;
            case 3://除此之外全部关闭
                for (var i = 0; i < allTabtitle.length; i++) {
                    if (curTabTitle != allTabtitle[i])
                        divTab.tabs('close', allTabtitle[i]);
                }
                divTab.tabs('select', curTabTitle);
                break;
            case 4://当前侧面右边
                for (var i = curTabIndex; i < allTabtitle.length; i++) {
                    divTab.tabs('close', allTabtitle[i]);
                }
                divTab.tabs('select', curTabTitle);
                break;
            case 5: //当前侧面左边
                for (var i = 0; i < curTabIndex - 1; i++) {
                    divTab.tabs('close', allTabtitle[i]);
                }
                divTab.tabs('select', curTabTitle);
                break;
            case 6: //刷新
                var panel = divTab.tabs("getTab", curTabTitle).panel("refresh");
                break;
        }
    }

    window.Admin.init();
})(jQuery, window, document);