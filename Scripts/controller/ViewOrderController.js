var homeconfig = {
    pageSize: 5,
    pageIndex: 1,
}
var x = 10;
var dateSearch1;
var dateSearch2;
var dateOld;
var dateNew;

var ViewOrder = {
    init: function () {
       ViewOrder.loadData(10,false);
        ViewOrder.registerEvent();
    },
    registerEvent: function () {
        $('.btn-Edit').off('click').on('click', function () {
            //$('#MyModal').modal('show');
            var IDOrder = $(this).data('id');
            ViewOrder.loadDetail(IDOrder);
        });

        $('.btnstatus').off('click').on('click', function () {
            const status = $(this).data('status');
            x = status;
            ViewOrder.loadData(status, true, null);
        });

        $('.btn-Update').off('click').on('click', function () {
            //bootbox.alert("This is the default alert!");

            var id = $(this).data('id');
            const status = $(this).data('status');
            bootbox.confirm("Bạn sẽ chuyển đơn hàng này đến trạng thái tiếp theo?", function (result) {
                if (result)
                ViewOrder.UpdateStatusOrder(id, status);
            });
           
        });

        $('#btn-Search').off('click').on('click', function () {
            //bootbox.alert("This is the default alert!");
            var strSeach = $("#strSeach").val();
         
            const status = x;
            ViewOrder.SearchData(status, true, strSeach);

        });
      
        $('#datepicker1').off('change').on('change', function () {
            //bootbox.alert("This is the default alert!");
            dateSearch1 = $("#datepicker1").val();
            console.log(dateSearch1)
            //alert(strSeach);
            //ViewOrder.SearchData(status, true, strSeach);

        });
        var ss = 0;
        $('#datepicker2').off('change').on('change', function () {
            dateSearch2 = $("#datepicker2").val();
            console.log(dateSearch2)
           
        });
        $('#btn-SearchDate').off('click').on('click', function () {
            //bootbox.alert("This is the default alert!");
            if (dateSearch2 != null && dateSearch1 != null) {
                ViewOrder.SearchDateData(x, true, dateSearch1, dateSearch2);
            }
        });
        $('.btn-Delete').off('click').on('click', function () {
            //bootbox.alert("This is the default alert!");

            var id = $(this).data('id');
            const status = $(this).data('status');
            bootbox.confirm("Bạn muốn hủy đơn hàng này?", function (result) {
                if (result)
                ViewOrder.DeleteOrder(id);
            });

        });

    },
    SearchDateData: function (status, changePageSize, dateSearch1 ,dateSearch2) {
        $.ajax({
            url: '/SalesMan/Order/SearchDayOrderList',
            type: 'get',
            data: {
                status: status,
                page: homeconfig.pageIndex,
                pageSize: homeconfig.pageSize,
                dateSearch1: dateSearch1,
                dateSearch2: dateSearch2
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

                    ViewOrder.paging(response.total, function () {
                        ViewOrder.SearchDateData(status, false, dateSearch1, dateSearch2);
                    }, changePageSize);
                    ViewOrder.registerEvent();

                }
                else {
                    alert(response.mess);
                    return false;
                }
            }
        })

    },
    UpdateStatusOrder: function (id,status) {
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
                        ViewOrder.loadData(x, true);
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
                window.open("/chitietDonHang/" + IDOrder, null, null, null);
            }
        });
    },
    DeleteOrder: function (id) {
        $.ajax({
            url: '/SalesMan/Order/DeleteOrder',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    bootbox.alert("Đã chuyển trạng thái thành công", function () {
                        ViewOrder.loadData(x, true);
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

    //
    loadData: function (status, changePageSize) {
        $.ajax({
            url: '/SalesMan/Order/GetOrderList',
            type: 'get',
            data: {
                status: status,
                page: homeconfig.pageIndex,
                pageSize: homeconfig.pageSize,
     
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

                    ViewOrder.paging(response.total, function () {
                        ViewOrder.loadData(x, false)
                    }, changePageSize);
                    ViewOrder.registerEvent();
                }

            }
        })

    },

    SearchData: function (status, changePageSize, strSearch) {
        $.ajax({
            url: '/SalesMan/Order/SearchOrderList',
            type: 'get',
            data: {
                status: status,
                page: homeconfig.pageIndex,
                pageSize: homeconfig.pageSize,
                strSearch: strSearch
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

                    ViewOrder.paging(response.total, function () {
                        ViewOrder.SearchData(status, false, strSeach);
                    }, changePageSize);
                    ViewOrder.registerEvent();

                }
                else {
                    alert(response.mess);
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
ViewOrder.init()