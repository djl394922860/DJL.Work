!(function ($, window, document, undefined) {
    if (window.Carousel === undefined || window.Carousel === null) {
        window.Carousel = {};
    }

    window.Carousel.urls = {
        saveCarouseUrl: '/WebSiteSetting/SaveCarousel',
        getCarouseImgUrl: '/WebSiteSetting/GetCarouseImg',
        editCarouseUrl: '/WebSiteSetting/EditCarousel'
    };
    var selectedId;

    var imgIds = [];

    function ImgInfoItem(id, path, physical) {
        this.id = id;
        this.path = path;
        this.physical = physical;
    }

    function InitImgIds() {
        var rows = $('#tableCarouselCanfig').datagrid('getRows');
        for (var i = 0; i < rows.length; i++) {
            var item = new ImgInfoItem(rows[i].Id, rows[i].NetworkRelativePath, rows[i].PhysicalPath);
            imgIds.push(item);
        }
    }

    window.Carousel.preImage = function () {
        var index;
        for (var i = 0; i < imgIds.length; i++) {
            if (imgIds[i].id === selectedId.id) {
                index = i;
            }
        }
        index--;
        if (index < 0) index = imgIds.length - 1;
        var currentId = imgIds[index];
        selectedId = currentId;
        var url = selectedId.path;
        $('#div_carouse_show img').attr('src', url);
        $('#div_carouse_show p:first').text(selectedId.id);
        $('#p_carouse_path').text(selectedId.path);
        $('#div_carouse_show p:last').text(selectedId.physical);
    }

    window.Carousel.nextImage = function () {
        var index;
        for (var i = 0; i < imgIds.length; i++) {
            if (imgIds[i].id === selectedId.id) {
                index = i;
            }
        }
        index++;
        if (index > imgIds.length - 1) index = 0;
        var currentId = imgIds[index];
        selectedId = currentId;
        var url = selectedId.path;
        $('#div_carouse_show img').attr('src', url);
        $('#div_carouse_show p:first').text(selectedId.id);
        $('#p_carouse_path').text(selectedId.path);
        $('#div_carouse_show p:last').text(selectedId.physical);
    }

    window.Carousel.initTable = function () {
        $('#tableCarouselCanfig').datagrid({
            title: '主页轮播图片管理',
            iconCls: 'icon-edit',//图标 
            fit: true,
            idField: 'Id',
            toolbar: '#tbMenu',
            url: '/WebSiteSetting/GetCarouselItems',
            columns: [[
                { field: 'Id', title: '编号', width: '5%' },
                { field: 'Order', title: '排序', width: '5%' },
                { field: 'MusicName', title: '歌曲名', width: '10%' },
                { field: 'MusicDescription', title: '歌曲描述', align: 'left', width: '10%' },
                { field: 'ImageFormat', title: '图片格式', width: '8%' },
                { field: 'IsDeleted', title: '禁用', width: '5%', align: 'left' },
                { field: 'ImgSize', title: '图片大小(KB)', width: '5%', align: 'left' },
                { field: 'PhysicalPath', title: '物理路径', width: '13%' },
                { field: 'NetworkRelativePath', title: '相对路径', width: '13%' },
                { field: 'UpLoadTime', title: '上传时间', width: '11%', align: 'right' },
                { field: 'UpdateTime', title: '更新时间', width: '11%', align: 'right' }
            ]],
            onLoadSuccess: function (data) {
                InitImgIds();
            }
        });
    }

    window.Carousel.initBtnMenu = function () {
        $('#btn_carouse_refresh').on('click', function () {
            $('#tableCarouselCanfig').datagrid('reload');
        });
        $('#btn_carouse_add').click(function () {
            $('#txtAvailable').switchbutton({
                checked: true,
                onChange: function (checkd) {
                    if (checkd) {
                        $('#hid_txt_available').val('on');
                    } else {
                        $('#hid_txt_available').val('off');
                    }
                }
            });
            $('#hid_txt_available').val('on');
            //clear
            $('#txtMusicName').textbox('clear');
            $('#txtMusicDescription').textbox('clear');
            $('#txtUpload').textbox('clear');
            $('#div_carouse_add').dialog('open');
        });
        $('#btn_carouse_edit').click(function () {
            var selectedRow = $('#tableCarouselCanfig').datagrid('getSelected');
            if (selectedRow === null) {
                $.messager.alert("警告", "没选中行，瞎点什么?");
                return false;
            }
            //load value
            $('#txtMusicNameEdit').textbox('setText', selectedRow.MusicName);
            $('#txtMusicNameEdit').textbox('setValue', selectedRow.MusicName);

            $('#txtMusicDescriptionEdit').textbox('setText', selectedRow.MusicDescription);
            $('#txtMusicDescriptionEdit').textbox('setValue', selectedRow.MusicDescription);

            $('#txtOrderEdit').slider('setValue', selectedRow.Order);
            $('#hid_txt_carouseId').val(selectedRow.Id);
            //clear filebox
            $('#txtUploadEdit').filebox('clear');
            $('#txtAvailableEdit').switchbutton({
                onChange: function (checked) {
                    if (checked) {
                        $('#hid_txt_availableEdit').val('on');
                    } else {
                        $('#hid_txt_availableEdit').val('off');
                    }
                }
            });
            //load isdeleted         
            if (selectedRow.IsDeleted === '是') {
                $('#txtAvailableEdit').switchbutton('uncheck');
                $('#hid_txt_availableEdit').val('off');
            } else {
                $('#txtAvailableEdit').switchbutton('check');
                $('#hid_txt_availableEdit').val('on');
            }
            //load img
            $('#imgImageData').attr('src', selectedRow.NetworkRelativePath);
            $('#div_carouse_edit').dialog('open');
        });
        $('#btn_carouse_look').click(function () {
            var selectedRow = $('#tableCarouselCanfig').datagrid('getSelected');
            if (selectedRow === null) {
                $.messager.alert("警告", "没选中行，瞎点什么?");
                return false;
            }
            selectedId = new ImgInfoItem(selectedRow.Id, selectedRow.NetworkRelativePath, selectedRow.PhysicalPath);
            $('#div_carouse_show img').attr('src', selectedRow.NetworkRelativePath);
            //path
            $('#div_carouse_show p:first').text(selectedRow.Id);
            $('#p_carouse_path').text(selectedRow.NetworkRelativePath);
            $('#div_carouse_show p:last').text(selectedRow.PhysicalPath);
            $('#div_carouse_show').dialog('open');
        });
    }

    window.Carousel.addSaveCarouse = function () {
        if (!$('#form_save_carouse').form('validate')) {
            return false;
        }
        $('#form_save_carouse').form('submit', {
            url: window.Carousel.urls.saveCarouseUrl,
            success: function (data) {
                data = JSON.parse(data);
                if (!data.success) {
                    $('#txtUpload').filebox('clear');
                    $.messager.alert("警告", data.message);
                    return false;
                }
                $('#div_carouse_add').dialog('close');
                $('#tableCarouselCanfig').datagrid('reload');
                $.messager.show({
                    title: '成功消息',
                    msg: data.message,
                    timeout: 3000,
                    showType: 'slide'
                });
            }
        });
    }

    window.Carousel.editSaveCarouse = function () {
        if (!$('#form_edit_carouse').form('validate')) {
            return false;
        }
        $('#form_edit_carouse').form('submit', {
            url: window.Carousel.urls.editCarouseUrl,
            success: function (data) {
                data = JSON.parse(data);
                if (!data.success) {
                    $('#txtUploadEdit').filebox('clear');
                    $.messager.alert("警告", data.message);
                    return false;
                }
                $('#div_carouse_edit').dialog('close');
                $('#tableCarouselCanfig').datagrid('reload');
                $.messager.show({
                    title: '成功消息',
                    msg: data.message,
                    timeout: 3000,
                    showType: 'slide'
                });
            }
        });
    }

    window.Carousel.baseInit = function () {
        window.Carousel.initTable();
        window.Carousel.initBtnMenu();
    }

    window.Carousel.baseInit();
})(jQuery, window, document);