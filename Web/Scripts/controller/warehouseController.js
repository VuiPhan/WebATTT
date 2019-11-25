
var warehouseconfig = {
    pageSize: 3,
    pageIndex: 1,
}

var warehouseController = {
    init: function () {
        warehouseController.loadData();
        warehouseController.registerEvent();
    },

    registerEvent: function () {

        $('#btnAddNew').off('click').on('click', function () {
            $('#modalAddUpdate').modal('show');
            warehouseController.resetForm();
        });

        $('#btnSave').off('click').on('click', function () {
            warehouseController.saveData();
        });

        $('.btnEdit').off('click').on('click', function () {
            $('#modalAddUpdate').modal('show');
            var id = $(this).data('id');
            warehouseController.loadDetail(id);
        });

        $('.btnDelete').off('click').on('click', function () {
            var id = $(this).data('id');
            bootbox.confirm("Are you sure to delete this warehouse?", function (result) {
                if (result == true) {
                    warehouseController.deletewarehouse(id);
                }
            });
        });

        $('#btnReset').off('click').on('click', function () {
            $('#txtSearch').val('');
            warehouseController.loadData(true);
        });

        $('#btnSearch').off('click').on('click', function () {
            warehouseController.loadData(true);
        });

        $('#txtSearch').off('keypress').on('keypress', function (e) {
            if (e.which == 13) {
                warehouseController.loadData(true);
            }
        });

    },

    loadData: function (changePageSize) {
        var name = $('#txtSearch').val();
        $.ajax({
            url: '/Admin/WareHouse/LoadData',
            type: 'GET',
            data: {
                name: name,
                page: warehouseconfig.pageIndex,
                pageSize: warehouseconfig.pageSize
            },
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            IDWareHouse: item.IDWareHouse,
                            Address: item.Address,
                            Amount: item.Amount,
                        });
                    });
                    $('#tblData').html(html);
                    warehouseController.paging(response.total, changePageSize);
                    warehouseController.registerEvent();
                }
            }

        });
    },

    saveData: function () {
        var address = $('#txtAddress').val();
        var amount = parseInt($('#txtAmount').val());
        var id = parseInt($('#hidID').val());
        $.ajax({
            url: '/Admin/WareHouse/SaveData',
            data: {
                warehouse: {
                    Address: address,
                    Amount: amount,
                    IDWareHouse: id
                }
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    bootbox.alert("Save Success!!", function () {
                        $('#modalAddUpdate').modal('hide');
                        warehouseController.loadData(true);
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
            url: '/Admin/WareHouse/GetDetail',
            data: {
                id: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                var data = response.data;
                $('#hidID').val(data.IDWareHouse);
                $('#txtAddress').val(data.Address);
                $('#txtAmount').val(data.Amount);
            },
            error: function (err) {
                console.log(err);
            }
        });
    },

    resetForm: function () {
        $('#hidID').val('0');
        $('#txtAddress').val('');
        $('#txtAmount').val('0');
    },

    deletewarehouse: function (id) {
        $.ajax({
            url: '/Admin/WareHouse/DeleteWareHouse',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    bootbox.alert("Delete Success!!", function () {
                        warehouseController.loadData(true);
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

    paging: function (totalRow, changePageSize) {
        var totalPage = Math.ceil(totalRow / warehouseconfig.pageSize);

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
                warehouseconfig.pageIndex = page;
                setTimeout(warehouseController.loadData(), 200);
            }
        });
    }
}
warehouseController.init();


