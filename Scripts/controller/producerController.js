
var producerconfig = {
    pageSize: 3,
    pageIndex: 1,
}

var producerController = {
    init: function () {
        producerController.loadData();
        producerController.registerEvent();
    },

    registerEvent: function () {

        $('#btnAddNew').off('click').on('click', function () {
            $('#modalAddUpdate').modal('show');
            producerController.resetForm();
        });

        $('.btnEdit').off('click').on('click', function () {
            $('#modalAddUpdate').modal('show');
            var id = $(this).data('id');
            producerController.loadDetail(id);
        });

        $('.btnDelete').off('click').on('click', function () {
            var id = $(this).data('id');
            bootbox.confirm("Are you sure to delete this producer?", function (result) {
                if (result == true) {
                    producerController.deleteProducer(id);
                }
            });
        });

        $('#btnReset').off('click').on('click', function () {
            $('#txtSearch').val('');
            producerController.loadData(true);
        });

        $('#btnSearch').off('click').on('click', function () {
            producerController.loadData(true);
        });

        $('#txtSearch').off('keypress').on('keypress', function (e) {
            if (e.which == 13) {
                producerController.loadData(true);
            }
        });

        $('#btnUpload').off('click').on('click', function (e) {
            e.preventDefault();
            var finder = new CKFinder();
            finder.selectActionFunction= function (url) {
                $('#urlLogo').val(url);
            };
            finder.popup();
        });        

        $('#frmSave').validate({
            rules: {
                txtName: "required",
                txtInformation: "required"
            },
            messages: {
                txtName: "Không được bỏ trống tên nhà sản xuất!!",
                txtInformation: "Không được bỏ trống thông tin "
            }
        });

        $('#btnSave').off('click').on('click', function () {
            if ($('#frmSave').valid()) {
                producerController.saveData();
            }
        });


    },

    loadData: function (changePageSize) {
        var name = $('#txtSearch').val();
        $.ajax({
            url: '/Admin/Producer/LoadData',
            type: 'GET',
            data: {
                name: name,
                page: producerconfig.pageIndex,
                pageSize: producerconfig.pageSize
            },
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            IDProducer: item.IDProducer,
                            NameProducer: item.NameProducer,
                            Information: item.Information,
                            Logo: item.Logo,
                        });
                    });
                    $('#tblData').html(html);
                    producerController.paging(response.total, changePageSize);
                    producerController.registerEvent();
                }
            }

        });
    },

    saveData: function () {
        var nameproducer = $('#txtName').val();
        var information = $('#txtInformation').val();
        var logo = $('#urlLogo').val();
        var id = parseInt($('#hidID').val());
        $.ajax({
            url: '/Admin/Producer/SaveData',
            data: {
                producer: {
                    IDProducer: id,
                    NameProducer: nameproducer,
                    Information: information,
                    Logo: logo,
                }
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    bootbox.alert("Save Success!!", function () {
                        $('#modalAddUpdate').modal('hide');
                        producerController.loadData(true);
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
            url: '/Admin/Producer/GetDetail',
            data: {
                id: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                var data = response.data;
                $('#hidID').val(data.IDProducer);
                $('#txtName').val(data.NameProducer);
                $('#txtInformation').val(data.Information);
                $('#urlLogo').val(data.Logo);
               
            },
            error: function (err) {
                console.log(err);
            }
        });
    },

    resetForm: function () {
        $('#hidID').val('0');
        $('#txtName').val('');
        $('#txtInformation').val('');
        $('#urlLogo').val('');
    },

    deleteProducer: function (id) {
        $.ajax({
            url: '/Admin/Producer/DeleteProducer',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    bootbox.alert("Delete Success!!", function () {
                        producerController.loadData(true);
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
        var totalPage = Math.ceil(totalRow / producerconfig.pageSize);

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
                producerconfig.pageIndex = page;
                setTimeout(producerController.loadData(), 200);
            }
        });
    }
}
producerController.init();
