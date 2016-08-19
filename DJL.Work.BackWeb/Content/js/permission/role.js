!function ($, window, document, undefined) {
    if (window.Role === undefined || window.Role === null) {
        window.Role = {};
    }

    window.Role.urls = {
        saveRoleUrl: '/Permission/SaveRole',
        editRoleUrl: '/Permission/EditRole',
        setActionsUrl: '/Permission/SetRoleActions',
        getRoleItemsUrl: '/Permission/GetRoleItems',
        getAllActionsUrl: '/Permission/GetAllActions',
        getRoleActionsUrl: '/Permission/GetRoleActions',
        getRoleUsersUrl: '/Permission/GetRoleUsers'
    };

    window.Role.initTable = function () {
        $('#tableRoleManager').datagrid({
            title: '角色管理',
            iconCls: 'icon-edit',//图标 
            fit: true,
            idField: 'Id',
            toolbar: '#tbMenu_role',
            url: window.Role.urls.getRoleItemsUrl,
            columns: [[
                { field: 'Id', title: '编号', width: '10%' },
                { field: 'RoleName', title: '角色名', width: '30%' },
                { field: 'IsDeleted', title: '禁用', width: '10%', align: 'left' },
                { field: 'Remark', title: '备注', width: '25%', align: 'right' },
                { field: 'CreateTime', title: '创建时间', width: '23%', align: 'right' }
            ]]
        });
    }

    window.Role.selectedAllActions = function () {
        $('#div_role_actions_list').datalist('checkAll');
    }

    window.Role.unselectedActions = function () {
        var allRows = $('#div_role_actions_list').datalist('getRows');
        var rows = $('#div_role_actions_list').datalist('getChecked');
        var array = [];
        for (var i = 0; i < allRows.length; i++) {
            if (!rows.contains(allRows[i])) {
                array.push(allRows[i]);
            }
        }
        $('#div_role_actions_list').datalist('uncheckAll');
        for (var i = 0; i < array.length; i++) {
            var index = $('#div_role_actions_list').datalist('getRowIndex', array[i]);
            $('#div_role_actions_list').datalist('checkRow', index);
        }
    }

    window.Role.initBtnMenu = function () {
        $('#btn_role_refresh').on('click', function () {
            $('#tableRoleManager').datagrid('reload');
        });
        $('#btn_role_add').click(function () {
            $('#txt_roleAvailable').switchbutton({
                checked: true,
                onChange: function (checkd) {
                    if (checkd) {
                        $('#hid_txt_roleAvailable').val('on');
                    } else {
                        $('#hid_txt_roleAvailable').val('off');
                    }
                }
            });
            $('#hid_txt_roleAvailable').val('on');
            $('#div_role_add').dialog('open');
        });
        $('#btn_role_edit').click(function () {
            var selectedRow = $('#tableRoleManager').datagrid('getSelected');
            if (selectedRow === null) {
                $.messager.alert("警告", "没选中行，瞎点什么?");
                return false;
            }
            //init
            $('#txt_roleAvailable_edit').switchbutton({
                checked: true,
                onChange: function (checkd) {
                    if (checkd) {
                        $('#hid_txt_roleAvailable_edit').val('on');
                    } else {
                        $('#hid_txt_roleAvailable_edit').val('off');
                    }
                }
            });
            $('#txt_roleActions_edit').combobox({
                method: 'post',
                url: window.Role.urls.getAllActionsUrl
            });
            //bind data
            $('#txt_roleName_edit').textbox('setText', selectedRow.RoleName);
            $('#txt_roleName_edit').textbox('setValue', selectedRow.RoleName);

            if (selectedRow.IsDeleted === '是') {
                $('#txt_roleAvailable_edit').switchbutton('uncheck');
                $('#hid_txt_roleAvailable_edit').val('off');
            } else {
                $('#txt_roleAvailable_edit').switchbutton('check');
                $('#hid_txt_roleAvailable_edit').val('on');
            }

            $('#txt_roleRemark_edit').textbox('setText', selectedRow.Remark);
            $('#txt_roleRemark_edit').textbox('setValue', selectedRow.Remark);

            $('#hid_txt_edit_roleId').val(selectedRow.Id);

            $.post(window.Role.urls.getRoleActionsUrl, { roleId: selectedRow.Id }, function (data) {
                if (!data.success) {
                    $.messager.alert('error', data.message);
                    return false;
                }
                if (data.message !== '') {
                    var roleActions = data.message.split(',');
                    $('#txt_roleActions_edit').combobox('setValues', roleActions);
                }
                //open dialog
                $('#div_role_edit').dialog('open');
            });
        });
        $('#btn_role_setActions').click(function () {
            var selectedRow = $('#tableRoleManager').datagrid('getSelected');
            if (selectedRow === null) {
                $.messager.alert("警告", "没选中行，瞎点什么?");
                return false;
            }
            //init role-actions
            $('#div_role_actions_list').datalist({
                valueField: 'id',
                textField: 'text',
                checkbox: true,
                lines: true,
                url: window.Role.urls.getAllActionsUrl,
                ctrlSelect: true,
                singleSelect: false,
                onLoadSuccess: function (data) {
                    //load roles
                    $.post(window.Role.urls.getRoleActionsUrl, { roleId: selectedRow.Id }, function (data) {
                        if (!data.success) {
                            $.messager.alert('error', data.message, 'error');
                            return false;
                        }
                        if (data.message === '' || data.message === null) {
                            $('#div_role_setActions').dialog('open');
                            return false;
                        }
                        var actionIds = data.message.split(',');
                        var list = $('#div_role_actions_list');
                        var allRows = list.datalist('getRows');
                        //bind roles
                        for (var i = 0; i < allRows.length; i++) {
                            var r = allRows[i].id + '';
                            if (actionIds.contains(r)) {
                                list.datalist('checkRow', i);
                            }
                        }
                        $('#div_role_setActions').dialog('open');
                    });
                }
            });
        });
        $('#txt_roleActions').combobox({
            method: 'post',
            url: window.Role.urls.getAllActionsUrl
        });
        //look users
        $('#btn_role_lookUsers').click(function () {
            var selectedRow = $('#tableRoleManager').datagrid('getSelected');
            if (selectedRow === null) {
                $.messager.alert("警告", "没选中行，瞎点什么?");
                return false;
            }
            var url = window.Role.urls.getRoleUsersUrl + '?roleId=' + selectedRow.Id;
            $('#div_role_users_list').datalist({
                valueField: 'id',
                textField: 'text',
                checkbox: false,
                lines: false,
                url: url,
                ctrlSelect: false,
                singleSelect: true,
                onLoadSuccess: function (data) {
                    $('#div_role_lookUsers').dialog('open');
                }
            });
        });
        //search
        $('#btn_shared_search').on('click', function () {
            var parm = $('#txt_shared_search').textbox('getValue');
            $('#tableRoleManager').datagrid('load', {
                searchValue: parm
            });
        });
    }

    window.Role.addSaveRole = function () {
        if (!$('#form_save_role').form('validate')) {
            return false;
        }
        $('#form_save_role').form('submit', {
            url: window.Role.urls.saveRoleUrl,
            success: function (data) {
                data = JSON.parse(data);
                if (!data.success) {
                    $.messager.alert("警告", data.message);
                    return false;
                }
                $('#div_role_add').dialog('close');
                $('#tableRoleManager').datagrid('reload');
                $.messager.show({
                    title: '成功消息',
                    msg: data.message,
                    timeout: 3000,
                    showType: 'slide'
                });
            }
        });
    }

    window.Role.editSaveRole = function () {
        var b = $('#form_edit_role').form('validate');
        if (!b) return b;
        $('#form_edit_role').form('submit', {
            url: window.Role.urls.editRoleUrl,
            success: function (data) {
                data = JSON.parse(data);
                if (!data.success) {
                    $.messager.alert("警告", data.message);
                    return false;
                }
                $('#div_role_edit').dialog('close');
                $('#tableRoleManager').datagrid('reload');
                $.messager.show({
                    title: '成功消息',
                    msg: data.message,
                    timeout: 3000,
                    showType: 'slide'
                });
            }
        });
    }

    window.Role.editActionsSave = function () {
        var roleId = $('#tableRoleManager').datagrid('getSelected').Id;
        var checkRows = $('#div_role_actions_list').datalist('getChecked');
        var arr = [];
        for (var i = 0; i < checkRows.length; i++) {
            arr.push(parseInt(checkRows[i].id));
        }
        var actions = arr.join(',');
        $.post(window.Role.urls.setActionsUrl, { roleId: roleId, actionsId: actions }, function (data) {
            if (!data.success) {
                $.messager.alert('warning', data.message);
                return false;
            }
            $('#div_role_setActions').dialog('close');
            $.messager.show({
                title: '成功消息',
                msg: data.message,
                timeout: 3000,
                showType: 'slide'
            });
        });
    }

    window.Role.baseInit = function () {
        window.Role.initTable();
        window.Role.initBtnMenu();
    }

    window.Role.baseInit();

}(jQuery, window, document);