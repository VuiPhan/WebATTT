
var categoryconfig = {
    pageSize: 3,
    pageIndex : 1,
}

var categoryController = {
    init: function () {
        categoryController.loadData();
        categoryController.registerEvent();
    },

    registerEvent: function () {

        $('#btnAddNew').off('click').on('click', function () {
            $('#modalAddUpdate').modal('show');
            categoryController.resetForm();
        });
        
        $('.btnEdit').off('click').on('click', function () {
            $('#modalAddUpdate').modal('show');
            var id = $(this).data('id');
            categoryController.loadDetail(id);
        });

        $('.btnDelete').off('click').on('click', function () {
            var id = $(this).data('id');
            bootbox.confirm("Are you sure to delete this category?", function (result) {
                if (result == true) {
                    categoryController.deleteCategory(id);
                }
            });
        });

        $('#btnReset').off('click').on('click', function () {
            $('#txtSearch').val('');
            categoryController.loadData(true);
        });

        $('#btnSearch').off('click').on('click', function () {
            categoryController.loadData(true);
        });

        $('#txtSearch').off('keypress').on('keypress', function (e) {
            if (e.which == 13) {
                categoryController.loadData(true);
            }
        });

        $('#frmSave').validate({
            rules: {
                txtName: "required",
                txtAlias: "required"
            },
            messages: {
                txtName: "Bạn phải nhập tên danh mục!!",
                txtAlias: "Bạn phải nhập bí danh!!"
            }
        });

        $('#btnSave').off('click').on('click', function () {
            if ($('#frmSave').valid()){
                categoryController.saveData();
            }
        });

    },

    loadData: function (changePageSize) {
        var name = $('#txtSearch').val();
        $.ajax({
            url: '/Admin/Category/LoadData',
            type: 'GET',
            data: {
                name: name,
                page: categoryconfig.pageIndex,
                pageSize: categoryconfig.pageSize
            },
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            IDProductType: item.IDProductType,
                            NameProductType: item.NameProductType,
                            Icon: item.Icon,
                            Alias: item.Alias,
                        });
                    });
                    $('#tblData').html(html);
                    categoryController.paging(response.total, changePageSize);
                    categoryController.registerEvent();
                }
            }

        });
    },

    saveData: function () {
        var nameproducttype = $('#txtName').val();
        var icon = $('#txtIcon').val();
        var alias = $('#txtAlias').val();
        var id = parseInt($('#hidID').val());
        $.ajax({
            url: '/Admin/Category/SaveData',
            data: {
                category: {
                    NameProductType: nameproducttype,
                    Icon: icon,
                    Alias: alias,
                    IDProductType: id
                }
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    bootbox.alert("Save Success!!", function () {
                        $('#modalAddUpdate').modal('hide');
                        categoryController.loadData(true);
                    });
                }
                else {
                    bootbox.alert(response.message);
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    },

    loadDetail: function (id) {
        $.ajax({
            url: '/Admin/Category/GetDetail',
            data: {
                id: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                    var data = response.data;
                    $('#hidID').val(data.IDProductType);
                    $('#txtName').val(data.NameProductType);
                    $('#txtIcon').val(data.Icon);
                    $('#txtAlias').val(data.Alias);
            },
            error: function (err) {
                console.log(err);
            }
        });
    },

    resetForm: function () {
        $('#hidID').val('0');
        $('#txtName').val('');
        $('#txtIcon').val('');
        $('#txtAlias').val('');
    },

    deleteCategory: function (id) {
        $.ajax({
            url: '/Admin/Category/DeleteCategory',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    bootbox.alert("Delete Success!!", function () {
                        categoryController.loadData(true);
                    });
                }
                else {
                    bootbox.alert(response.message);
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    },

    paging: function (totalRow,changePageSize) {
        var totalPage = Math.ceil(totalRow / categoryconfig.pageSize);

        if ($('#pagination a').length === 0 || changePageSize == true) {
            $('#pagination').empty();
            $('#pagination').removeData("twbs-pagination");
            $('#pagination').unbind("page");
        }

        $('#pagination').twbsPagination({
            totalPages: totalPage,
            visiblePages: 10,
            first: "Đầu",
            next: "Tiếp",
            last: "Cuối",
            prev: "Trước",
            onPageClick: function (event, page) {
                categoryconfig.pageIndex = page;
                setTimeout(categoryController.loadData(), 200);
            }
        });
    }
}
categoryController.init();


