!function ($, window, document, undefind) {
    if (window.Action === undefined || window.Action === null) {
        window.Action = {};
    }

    window.Action.urls = {
        saveActionUrl: '/Permission/SaveAction',
        editActionUrl: '/Permission/EditAction',
        getActionItemsUrl: '/Permission/GetActionItems',
        getActionRolesUrl: '/Permission/GetActionRoles'
    };

    window.Action.initTable = function () {
        $('#tableActionManager').datagrid({
            title: '后台权限管理',
            iconCls: 'icon-edit',//图标 
            fit: true,
            idField: 'Id',
            toolbar: '#tbMenu_action',
            url: window.Action.urls.getActionItemsUrl,
            columns: [[
                { field: 'Id', title: '编号', width: '5%' },
                { field: 'ActionName', title: '权限名', width: '15%' },
                { field: 'ActionMethod', title: '权限请求方式', width: '10%' },
                { field: 'ActionRequestUrl', title: '权限请求地址', width: '25%' },
                { field: 'IsDeleted', title: '禁用', width: '5%', align: 'center' },
                { field: 'Remark', title: '备注', width: '25%', align: 'left' },
                { field: 'CreateTime', title: '创建时间', width: '13%', align: 'center' }
            ]]
        });
    }

    window.Action.initBtnMenu = function () {
        $('#btn_action_refresh').on('click', function () {
            $('#tableActionManager').datagrid('reload');
        });
        $('#btn_action_add').click(function () {
            //clear
            $('#txt_actionName').textbox('clear');
            $('#txt_actionRequestUrl').textbox('clear');
            $('#txt_actionRemark').textbox('clear');
            //init
            $('#txt_actionAvailable').switchbutton({
                checked: true,
                onChange: function (checkd) {
                    if (checkd) {
                        $('#hid_txt_actionAvailable').val('on');
                    } else {
                        $('#hid_txt_actionAvailable').val('off');
                    }
                }
            });
            $('#hid_txt_actionAvailable').val('on');
            $('#txt_actionMethod').combobox({
                data: [{
                    'id': 0,
                    'text': 'Get',
                    'selected': true
                }, {
                    'id': 1,
                    'text': 'Post'
                }]
            });
            $('#div_action_add').dialog('open');
        });
        $('#btn_action_edit').click(function () {
            var selectedRow = $('#tableActionManager').datagrid('getSelected');
            if (selectedRow === null) {
                $.messager.alert("警告", "没选中行，瞎点什么?");
                return false;
            }
            //init
            $('#hid_txt_edit_actionId').val(selectedRow.Id);

            $('#txt_actionAvailable_edit').switchbutton({
                checked: true,
                onChange: function (checkd) {
                    if (checkd) {
                        $('#hid_txt_actionAvailable_edit').val('on');
                    } else {
                        $('#hid_txt_actionAvailable_edit').val('off');
                    }
                }
            });
            $('#txt_actionMethod_edit').combobox({
                data: [{
                    'id': 0,
                    'text': 'Get'
                }, {
                    'id': 1,
                    'text': 'Post'
                }]
            });
            //bind data
            if (selectedRow.ActionMethod === 'Get') {
                $('#txt_actionMethod_edit').combobox('setValue', 0);
            } else {
                $('#txt_actionMethod_edit').combobox('setValue', 1);
            }

            $('#txt_actionName_edit').textbox('setText', selectedRow.ActionName);
            $('#txt_actionName_edit').textbox('setValue', selectedRow.ActionName);

            if (selectedRow.IsDeleted === '是') {
                $('#txt_actionAvailable_edit').switchbutton('uncheck');
                $('#hid_txt_actionAvailable_edit').val('off');
            } else {
                $('#txt_actionAvailable_edit').switchbutton('check');
                $('#hid_txt_actionAvailable_edit').val('on');
            }

            $('#txt_actionRequestUrl_edit').textbox('setText', selectedRow.ActionRequestUrl);
            $('#txt_actionRequestUrl_edit').textbox('setValue', selectedRow.ActionRequestUrl);

            $('#txt_actionRemark_edit').textbox('setText', selectedRow.Remark);
            $('#txt_actionRemark_edit').textbox('setValue', selectedRow.Remark);

            //open dialog
            $('#div_action_edit').dialog('open');
        });
        $('#btn_action_lookRoles').on('click', function () {
            var selectedRow = $('#tableActionManager').datagrid('getSelected');
            if (selectedRow === null) {
                $.messager.alert("警告", "没选中行，瞎点什么?");
                return false;
            }
            var url = window.Action.urls.getActionRolesUrl + '?actionId=' + selectedRow.Id;
            $('#div_action_roles_list').datalist({
                valueField: 'id',
                textField: 'text',
                checkbox: false,
                lines: false,
                url: url,
                ctrlSelect: false,
                singleSelect: true,
                onLoadSuccess: function (data) {
                    $('#div_action_lookRoles').dialog('open');
                }
            });
        });
        //search
        $('#btn_shared_search').on('click', function () {
            var parm = $('#txt_shared_search').textbox('getValue');
            $('#tableActionManager').datagrid('load', {
                searchValue: parm
            });
        });
    }

    window.Action.addSaveAction = function () {
        if (!$('#form_save_action').form('validate')) {
            return false;
        }
        $('#form_save_action').form('submit', {
            url: window.Action.urls.saveActionUrl,
            success: function (data) {
                data = JSON.parse(data);
                if (!data.success) {
                    $.messager.alert("警告", data.message);
                    return false;
                }
                $('#div_action_add').dialog('close');
                $('#tableActionManager').datagrid('reload');
                $.messager.show({
                    title: '成功消息',
                    msg: data.message,
                    timeout: 3000,
                    showType: 'slide'
                });
            }
        });
    }

    window.Action.editSaveAction = function () {
        var b = $('#form_edit_action').form('validate');
        if (!b) return b;
        $('#form_edit_action').form('submit', {
            url: window.Action.urls.editActionUrl,
            success: function (data) {
                data = JSON.parse(data);
                if (!data.success) {
                    $.messager.alert("警告", data.message);
                    return false;
                }
                $('#div_action_edit').dialog('close');
                $('#tableActionManager').datagrid('reload');
                $.messager.show({
                    title: '成功消息',
                    msg: data.message,
                    timeout: 3000,
                    showType: 'slide'
                });
            }
        });
    }

    window.Action.baseInit = function () {
        window.Action.initTable();
        window.Action.initBtnMenu();
    }

    window.Action.baseInit();

}(jQuery, window, document);