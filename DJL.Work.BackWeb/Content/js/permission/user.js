!(function ($, window, document, undefined) {
    if (window.User === undefined || window.User === null) {
        window.User = {};
    }

    window.User.urls = {
        saveUserUrl: '/Permission/SaveUser',
        editUserUrl: '/Permission/EditUser',
        setRolesUrl: '/Permission/SetUserRoles',
        getUserItemsUrl: '/Permission/GetUserItems',
        getAllRolesUrl: '/Permission/GetAllRoles',
        getEnableUserUrl: '/Permission/EnableUser',
        getUserRolesUrl: '/Permission/GetUserRoles'
    };

    window.User.initTable = function () {
        $('#tableUserManager').datagrid({
            title: '后台管理员管理',
            iconCls: 'icon-edit',//图标 
            fit: true,
            idField: 'Id',
            toolbar: '#tbMenu_user',
            url: window.User.urls.getUserItemsUrl,
            columns: [[
                { field: 'Id', title: '编号', width: '10%' },
                { field: 'UserName', title: '管理员姓名', width: '15%' },
                { field: 'UserEmail', title: '管理员邮箱', width: '20%' },
                { field: 'IsDeleted', title: '禁用', width: '15%', align: 'left' },
                { field: 'Remark', title: '备注', width: '25%', align: 'right' },
                { field: 'CreateTime', title: '创建时间', width: '13%', align: 'right' }
            ]]
        });
    }

    window.User.selectedAllRoles = function () {
        $('#div_user_roles_list').datalist('checkAll');
    }

    window.User.unselectedRoles = function () {
        var allRows = $('#div_user_roles_list').datalist('getRows');
        var rows = $('#div_user_roles_list').datalist('getChecked');
        var array = [];
        for (var i = 0; i < allRows.length; i++) {
            if (!rows.contains(allRows[i])) {
                array.push(allRows[i]);
            }
        }
        $('#div_user_roles_list').datalist('uncheckAll');
        for (var i = 0; i < array.length; i++) {
            var index = $('#div_user_roles_list').datalist('getRowIndex', array[i]);
            $('#div_user_roles_list').datalist('checkRow', index);
        }
    }

    window.User.initBtnMenu = function () {
        $('#btn_user_refresh').on('click', function () {
            $('#tableUserManager').datagrid('reload');
        });
        $('#btn_user_add').click(function () {
            $('#txt_userAvailable').switchbutton({
                checked: true,
                onChange: function (checkd) {
                    if (checkd) {
                        $('#hid_txt_userAvailable').val('on');
                    } else {
                        $('#hid_txt_userAvailable').val('off');
                    }
                }
            });
            $('#hid_txt_userAvailable').val('on');
            //clear
            $('#txt_userName').textbox('clear');
            $('#txt_userEmail').textbox('clear');
            $('#txt_userNewPwd').textbox('clear');
            $('#txt_userConfirmPwd').textbox('clear');
            $('#txt_userRoles').combobox('clear');
            $('#txt_userRemark').textbox('clear');
            $('#div_user_add').dialog('open');
        });
        $('#btn_user_edit').click(function () {
            var selectedRow = $('#tableUserManager').datagrid('getSelected');
            if (selectedRow === null) {
                $.messager.alert("警告", "没选中行，瞎点什么?");
                return false;
            }
            var str = selectedRow.IsDeleted;
            if (str === "是") {
                $.messager.alert('消息', '你在逗我么？已经是禁用状态了');
                return false;
            }
            $.messager.confirm('确认', '您确认想要禁用这个管理员吗？', function (r) {
                if (r) {
                    var id = selectedRow.Id;
                    $.post(window.User.urls.editUserUrl, { userId: id }, function (data) {
                        if (!data.success) {
                            $.messager.alert('警告', data.message, 'error');
                            return false;
                        }
                        $('#tableUserManager').datagrid('reload');
                        $.messager.show({
                            title: '成功',
                            msg: data.message,
                            timeout: 3000,
                            showType: 'slide'
                        });
                    });
                }
            });
        });
        $('#btn_user_editToWork').click(function () {
            var selectedRow = $('#tableUserManager').datagrid('getSelected');
            if (selectedRow === null) {
                $.messager.alert("警告", "没选中行，瞎点什么?");
                return false;
            }
            var str = selectedRow.IsDeleted;
            if (str === "否") {
                $.messager.alert('消息', '你在逗我么？已经是启用状态了');
                return false;
            }
            $.messager.confirm('确认', '您确认想要启用这个管理员吗？', function (r) {
                if (r) {
                    var id = selectedRow.Id;
                    $.post(window.User.urls.getEnableUserUrl, { userId: id }, function (data) {
                        if (!data.success) {
                            $.messager.alert('警告', data.message, 'error');
                            return false;
                        }
                        $('#tableUserManager').datagrid('reload');
                        $.messager.show({
                            title: '成功',
                            msg: data.message,
                            timeout: 3000,
                            showType: 'slide'
                        });
                    });
                }
            });
        });
        $('#btn_user_setRoles').click(function () {
            var selectedRow = $('#tableUserManager').datagrid('getSelected');
            if (selectedRow === null) {
                $.messager.alert("警告", "没选中行，瞎点什么?");
                return false;
            }
            //init user-roles
            $('#div_user_roles_list').datalist({
                valueField: 'id',
                textField: 'text',
                checkbox: true,
                lines: true,
                url: window.User.urls.getAllRolesUrl,
                ctrlSelect: true,
                singleSelect: false,
                onLoadSuccess: function (data) {
                    //load roles
                    $.post(window.User.urls.getUserRolesUrl, { userId: selectedRow.Id }, function (data) {
                        if (!data.success) {
                            $.messager.alert('error', data.message, 'error');
                            return false;
                        }
                        if (data.message === '' || data.message === null) {
                            $('#div_user_setRoles').dialog('open');
                            return false;
                        }
                        var roleIds = data.message.split(',');
                        var list = $('#div_user_roles_list');
                        var allRows = list.datalist('getRows');
                        //bind roles
                        for (var i = 0; i < allRows.length; i++) {
                            var r = allRows[i].id + '';
                            if (roleIds.contains(r)) {
                                list.datalist('checkRow', i);
                            }
                        }
                        $('#div_user_setRoles').dialog('open');
                    });
                }
            });
        });
        $('#txt_userRoles').combobox({
            method: 'post',
            url: window.User.urls.getAllRolesUrl
        });
        //search
        $('#btn_shared_search').on('click', function () {
            var parm = $('#txt_shared_search').textbox('getValue');
            $('#tableUserManager').datagrid('load', {
                searchValue: parm
            });
        });
    }

    window.User.addSaveUser = function () {
        if (!$('#form_save_user').form('validate')) {
            return false;
        }
        $('#form_save_user').form('submit', {
            url: window.User.urls.saveUserUrl,
            success: function (data) {
                data = JSON.parse(data);
                if (!data.success) {
                    $.messager.alert("警告", data.message);
                    return false;
                }
                $('#div_user_add').dialog('close');
                $('#tableUserManager').datagrid('reload');
                $.messager.show({
                    title: '成功消息',
                    msg: data.message,
                    timeout: 3000,
                    showType: 'slide'
                });
            }
        });
    }

    window.User.editRolesSave = function () {
        var userId = $('#tableUserManager').datagrid('getSelected').Id;
        var checkRows = $('#div_user_roles_list').datalist('getChecked');
        var arr = [];
        for (var i = 0; i < checkRows.length; i++) {
            arr.push(parseInt(checkRows[i].id));
        }
        var roles = arr.join(',');
        $.post(window.User.urls.setRolesUrl, { userId: userId, rolesId: roles }, function (data) {
            if (!data.success) {
                $.messager.alert('warning', data.message);
                return false;
            }
            $('#div_user_setRoles').dialog('close');
            $.messager.show({
                title: '成功消息',
                msg: data.message,
                timeout: 3000,
                showType: 'slide'
            });
        });
    }

    window.User.baseInit = function () {
        window.User.initTable();
        window.User.initBtnMenu();
    }

    window.User.baseInit();
})(jQuery, window, document);