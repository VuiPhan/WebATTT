
var supplierconfig = {
    pageSize: 3,
    pageIndex: 1,
}

var supplierController = {
    init: function () {
        supplierController.loadData();
        supplierController.registerEvent();
    },

    registerEvent: function () {

        $('#btnAddNew').off('click').on('click', function () {
            $('#modalAddUpdate').modal('show');
            supplierController.resetForm();
        });

        $('.btnEdit').off('click').on('click', function () {
            $('#modalAddUpdate').modal('show');
            var id = $(this).data('id');
            supplierController.loadDetail(id);
        });

        $('.btnDelete').off('click').on('click', function () {
            var id = $(this).data('id');
            bootbox.confirm("Are you sure to delete this supplier?", function (result) {
                if (result == true) {
                    supplierController.deleteSupplier(id);
                }
            });
        });

        $('#btnReset').off('click').on('click', function () {
            $('#txtSearch').val('');
            supplierController.loadData(true);
        });

        $('#btnSearch').off('click').on('click', function () {
            supplierController.loadData(true);
        });

        $('#txtSearch').off('keypress').on('keypress', function (e) {
            if (e.which == 13) {
                supplierController.loadData(true);
            }
        });

        $('#frmSave').validate({
            rules: {
                txtName: "required",
                txtAddress: "required",
                txtEmail: "email",
                txtSdt: {
                    number: true,
                    required: true,
                    rangelength: [10,10]
                },
                txtFax : "required"
            },
            messages: {
                txtName: "Không được bỏ trống tên!!",
                txtAddress: "Không được bỏ trống địa chỉ!!",
                txtEmail: "Hãy nhập đúng định dạng Email!!",
                txtSdt: {
                    number: "Vui lòng nhập số!!",
                    required: "Không được bỏ trống số điện thoại!!",
                    rangelength: "Số điện thoại bắt buộc phải 10 số!!"
                },
                txtFax: "Không được bỏ trống Fax!!"
            }
        });

        $('#btnSave').off('click').on('click', function () {
            if ($('#frmSave').valid()) {
                supplierController.saveData();
            }
        });

    },

    loadData: function (changePageSize) {
        var name = $('#txtSearch').val();
        $.ajax({
            url: '/Admin/Supplier/LoadData',
            type: 'GET',
            data: {
                name: name,
                page: supplierconfig.pageIndex,
                pageSize: supplierconfig.pageSize
            },
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            IDSupplier: item.IDSupplier,
                            NameSupplier: item.NameSupplier,
                            Address: item.Address,
                            Email: item.Email,
                            PhoneNumber: item.PhoneNumber,
                            Fax: item.Fax
                        });
                    });
                    $('#tblData').html(html);
                    supplierController.paging(response.total, changePageSize);
                    supplierController.registerEvent();
                }
            }

        });
    },

    saveData: function () {
        var namesupplier = $('#txtName').val();
        var address = $('#txtAddress').val();
        var email = $('#txtEmail').val();
        var sdt = $('#txtSdt').val();
        var fax = $('#txtFax').val();
        var id = parseInt($('#hidID').val());
        $.ajax({
            url: '/Admin/Supplier/SaveData',
            data: {
                supplier: {
                    IDSupplier: id,
                    NameSupplier: namesupplier,
                    Address: address,
                    Email: email,
                    PhoneNumber: sdt,
                    Fax: fax
                }
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    bootbox.alert("Save Success!!", function () {
                        $('#modalAddUpdate').modal('hide');
                        supplierController.loadData(true);
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
            url: '/Admin/Supplier/GetDetail',
            data: {
                id: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                var data = response.data;
                $('#hidID').val(data.IDSupplier);
                $('#txtName').val(data.NameSupplier);
                $('#txtAddress').val(data.Address);
                $('#txtEmail').val(data.Email);
                $('#txtSdt').val(data.PhoneNumber);
                $('#txtFax').val(data.Fax);
            },
            error: function (err) {
                console.log(err);
            }
        });
    },

    resetForm: function () {
        $('#hidID').val('0');
        $('#txtName').val('');
        $('#txtAddress').val('');
        $('#txtEmail').val('');
        $('#txtSdt').val('');
        $('#txtFax').val('');
    },

    deleteSupplier: function (id) {
        $.ajax({
            url: '/Admin/Supplier/DeleteSupplier',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    bootbox.alert("Delete Success!!", function () {
                        supplierController.loadData(true);
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
        var totalPage = Math.ceil(totalRow / supplierconfig.pageSize);

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
                supplierconfig.pageIndex = page;
                setTimeout(supplierController.loadData(), 200);
            }
        });
    }
}
supplierController.init();


