var homeconfig = {
    pageSize: 2,
    pageIndex: 1,
}
var x = 10;
var BillAccount = {
    init: function () {
        BillAccount.loadData(10, false);
        BillAccount.registerEvent();
    },
    registerEvent: function () {
        $('.btn-Edit').off('click').on('click', function () {
            //$('#MyModal').modal('show');
            var IDOrder = $(this).data('id');
            BillAccount.loadDetail(IDOrder);
        });

        $('.btnstatus').off('click').on('click', function () {
            const status = $(this).data('status');
            x = status;
            BillAccount.loadData(status, true);
        });

        $('.btn-Update').off('click').on('click', function () {
            //bootbox.alert("This is the default alert!");

            var id = $(this).data('id');
            const status = $(this).data('status');
            bootbox.confirm("Bạn sẽ chuyển đơn hàng này đến trạng thái tiếp theo?", function (result) {
                BillAccount.UpdateStatusOrder(id, status);
            });

        });
    },
    UpdateStatusOrder: function (id, status) {
        $.ajax({
            url: '/SalesMan/Order/BrowseStatusOrder',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    bootbox.alert("Đã chuyển trạng thái thành công", function () {
                        BillAccount.loadData(x, true);
                    });
                }
                else {
                    bootbox.alert(response.mess);
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    },

    loadDetail: function (IDOrder) {
        $.ajax({

            success: function (res) {
                window.open("/ChiTietDonHangKhachHang/" + IDOrder, null, null, null);
            }
        });
    },


    //
    loadData: function (status, changePageSize) {
        $.ajax({
            url: '/Checkout/GetOrderList',
            type: 'get',
            data: {
                status: status,
                page: homeconfig.pageIndex,
                pageSize: homeconfig.pageSize
            },
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    $('#idSLDon').html(response.total + " Đơn hàng");
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        var date = item.OrderedDate;
                        var nowDate1 = new Date(parseInt(date.substr(6)));
                        var nowDate = nowDate1.toDateString();
                        html += Mustache.render(template, {
                            IDOrder: item.IDOrder,
                            OrderedDate: nowDate,
                            StatusOrder: item.Name,
                            // DeliveryStatus: item.DeliveryStatus == true ? "<span class=\"badge badge-success\">Đã giao </span>" : "<span class=\"badge badge-danger\">Chưa giao hàng</span>",
                            //Payed: item.Payed == true ? "<span class=\"badge badge-success\">Đã thanh toán</span>" : "<span class=\"badge badge-danger\">Chưa thanh toán</span>",
                            NameCus: item.FullName
                        });
                    });
                    $('#tblData').html(html);

                    BillAccount.paging(response.total, function () {
                        BillAccount.loadData(x, false)
                    }, changePageSize);
                    BillAccount.registerEvent();

                }

            }
        })

    },



    paging: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / homeconfig.pageSize);

        // Unbind pagination if it existed or click change pagesize
        if ($('#pagination a').length === 0 || changePageSize === true) {
            $('#pagination').empty();
            $('#pagination').removeData("twbs-pagination");
            $('#pagination').unbind("page");
        }

        $('#pagination').twbsPagination({
            totalPages: totalPage,
            first: "Đầu",
            next: "Tiếp",
            last: "Cuối",
            prev: "Trước",
            visiblePages: 10,
            onPageClick: function (event, page) {
                homeconfig.pageIndex = page;
                setTimeout(callback, 200);
            }
        });
    }
}
BillAccount.init()