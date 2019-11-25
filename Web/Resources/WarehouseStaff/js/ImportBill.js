$("#LoadingStatus").html("Loading....");
$.get("/ImportBills/GetImportBillList", null, DataBind);

function DataBind(ImportBillList) {
    var SetData = $("#SetImportBillList");
    for (var i = 0; i < ImportBillList.length; i++) {
        var Data = "<tr class='row_" + ImportBillList[i].IDImport + "'>" +
            "<td>" + ImportBillList[i].NameProduct + "</td>" +
            "<td>" + ImportBillList[i].Price + "</td>" +
            "<td>" + ImportBillList[i].Amount + "</td>" +
            "<td>" + ImportBillList[i].NameSupplier + "</td>" +
            "<td>" + "<a href='#' class='btn btn-warning' data-toggle='modal' data-target='#MyModal' onclick='EditImportBillRecord(" + ImportBillList[i].IDImport + ")' ><span class='mdi mdi-table-edit'></span></a>" + "</td>" +
            "<td>" + "<a href='#' class='btn btn-danger' onclick='DeleteImportBillRecord(" + ImportBillList[i].IDImport + ")'><span class='mdi mdi-delete'></span></a>" + "</td>" +
            "</tr>";
        SetData.append(Data);
        $("#LoadingStatus").html(" ");

    }
}

//Show The Popup Modal For Add New ImportBill

function AddNewImportBill(IDImport) {
    $('#Price').css('border-color', 'lightgrey');
    $('#Amount').css('border-color', 'lightgrey');
    $("#form")[0].reset();
    $("#IDImport").val(0);
    $("#DropDwnPro option:selected").text("--Sản phẩm--");
    $("#DropDwnSup option:selected").text("--Nhà cung cấp--");
    $("#MyModal").modal();

}


//Show The Popup Modal For Edit ImportBill Record

function EditImportBillRecord(IDImport) {
    var url = "/ImportBills/GetImportBillById?IDImport=" + IDImport;
    $("#MyModal").modal();
    $.ajax({
        type: "GET",
        url: url,
        success: function (data) {
            var obj = JSON.parse(data);
            $("#IDImport").val(obj.IDImport);
            $("#DropDwnPro option:selected").text(obj.Product.NameProduct);
            $("#DropDwnPro option:selected").val(obj.IDProduct);
            $("#Price").val(obj.Price);
            $("#Amount").val(obj.Amount);
            $("#DropDwnSup option:selected").text(obj.Supplier.NameSupplier);
            $("#DropDwnSup option:selected").val(obj.IDSupplier);
        }
    })
}

var SaveImportBillRecord = function () {
    var data = $("#SubmitForm").serialize();
    if (!$('#Price').val() || !$('#Amount').val()) {
        if ($('#Price').val().trim() == "") {
            $('#Price').css('border-color', 'Red');
        }
        else {
            $('#Price').css('border-color', 'lightgrey');
        }
        if ($('#Amount').val().trim() == "") {
            $('#Amount').css('border-color', 'Red');
        }
        else {
            $('#Amount').css('border-color', 'lightgrey');
        }
        return alert('Không được bỏ trống mục nào');
    }
    $.ajax({
        type: "Post",
        url: "/ImportBills/SaveDataInDatabase",
        data: data,
        success: function (result) {
            window.location.href = "/WarehouseStaff/ImportBills/ImportBills";
            $("#MyModal").modal("hide");
        }
    })
}

//Show The Popup Modal For DeleteComfirmation

var DeleteImportBillRecord = function (IDImport) {
    $("#IDImport").val(IDImport);
    $("#DeleteConfirmation").modal("show");
}
var ConfirmDelete = function () {
    var IDImport = $("#IDImport").val();
    $.ajax({
        type: "POST",
        url: "/ImportBills/DeleteImportBillRecord?IDImport=" + IDImport,
        success: function (result) {
            $("#DeleteConfirmation").modal("hide");
            $(".row_" + IDImport).remove();
        }
    })
}
