
var importbillconfig = {
    pageSize: 3,
    pageIndex: 1,
}

var importbillController = {
    init: function () {
        importbillController.loadData();
        importbillController.registerEvent();
    },

    registerEvent: function () {


        $('.btnDetail').off('click').on('click', function () {
            $('#modalAddUpdate').modal('show');
            var id = $(this).data('id');
            importbillController.loadDetail(id);
        });

        $('#btnReset').off('click').on('click', function () {
            $('#txtSearch').val('');
            importbillController.loadData(true);
        });

        $('#btnSearch').off('click').on('click', function () {
            importbillController.loadData(true);
        });

        $('#txtSearch').off('keypress').on('keypress', function (e) {
            if (e.which == 13) {
                importbillController.loadData(true);
            }
        });

    },

    loadData: function (changePageSize) {
        var idimport = $('#txtSearch').val();
        $.ajax({
            url: '/Admin/ImportBill/LoadData',
            type: 'GET',
            data: {
                IDImport: idimport,
                page: importbillconfig.pageIndex,
                pageSize: importbillconfig.pageSize
            },
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template-import').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            IDImport: item.IDImport,
                            IDSupplier: item.IDSupplier,
                            Price: item.Price,
                            Amount: item.Amount,
                        });
                    });
                    $('#tblDataImport').html(html);
                    importbillController.paging(response.total, changePageSize);
                    importbillController.registerEvent();
                }
            }

        });
    },

  
    loadDetail: function (id) {
        $.ajax({
            url: '/Admin/ImportBill/GetDetail',
            data: {
                id: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                var data = response.data;
                var html = '';
                var template = $('#data-template-detailimport').html();
                $.each(data, function (i, item) {
                    html += Mustache.render(template, {
                        IDDetailImport: item.IDDetailImport,
                        IDProduct: item.IDProduct,
                        NameProduct: item.NameProduct,
                        Price: item.Price,
                        Amount: item.Amount,
                    });
                });
                $('#tblDataDetailImport').html(html);
            },
            error: function (err) {
                console.log(err);
            }
        });
    },

    paging: function (totalRow, changePageSize) {
        var totalPage = Math.ceil(totalRow / importbillconfig.pageSize);

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
                importbillconfig.pageIndex = page;
                setTimeout(importbillController.loadData(), 200);
            }
        });
    }
}
importbillController.init();


