$("#LoadingStatus").html("Loading....");
$.get("/ProductTypes/GetProductTypeList", null, DataBind);

function DataBind(ProductTypeList) {
    var SetData = $("#SetProductTypeList");
    for (var i = 0; i < ProductTypeList.length; i++) {
        var Data = "<tr class='row_" + ProductTypeList[i].IDProductType + "'>" +
            "<td>" + ProductTypeList[i].NameProductType + "</td>" +
            "<td>" + ProductTypeList[i].Icon + "</td>" +
            "<td>" + ProductTypeList[i].Alias + "</td>" +
            "<td>" + "<a href='#' class='btn btn-warning' data-toggle='modal' data-target='#MyModal' onclick='EditProductTypeRecord(" + ProductTypeList[i].IDProductType + ")' ><span class='mdi mdi-table-edit'></span></a>" + "</td>" +
            "<td>" + "<a href='#' class='btn btn-danger' onclick='DeleteProductTypeRecord(" + ProductTypeList[i].IDProductType + ")'><span class='mdi mdi-delete'></span></a>" + "</td>" +
            "</tr>";
        SetData.append(Data);
        $("#LoadingStatus").html(" ");

    }
}

//Show The Popup Modal For Add New ProductType

function AddNewProductType(IDProductType) {
    $("#form")[0].reset();
    $("#IDProductType").val(0);
    $("#MyModal").modal();

}


//Show The Popup Modal For Edit ProductType Record

function EditProductTypeRecord(IDProductType) {
    var url = "/ProductTypes/GetProductTypeById?IDProductType=" + IDProductType;
    $("#MyModal").modal();
    $.ajax({
        type: "GET",
        url: url,
        success: function (data) {
            var obj = JSON.parse(data);
            $("#IDProductType").val(obj.IDProductType);
            $("#NameProductType").val(obj.NameProductType);
            $("#Icon").val(obj.Icon);
            $("#Alias").val(obj.Alias);
        }
    })
}

var SaveProductTypeRecord = function () {
    var data = $("#SubmitForm").serialize();
    if (!$('#NameProductType').val() || !$('#Icon').val() || !$('#Alias').val()) {
        return alert('Không được bỏ trống mục nào');
    }
    $.ajax({
        type: "Post",
        url: "/ProductTypes/SaveDataInDatabase",
        data: data,
        success: function (result) {
            window.location.href = "/WarehouseStaff/ProductTypes/ProductTypes";
            $("#MyModal").modal("hide");
        }
    })
}

//Show The Popup Modal For DeleteComfirmation

var DeleteProductTypeRecord = function (IDProductType) {
    $("#IDProductType").val(IDProductType);
    $("#DeleteConfirmation").modal("show");
}
var ConfirmDelete = function () {
    var IDProductType = $("#IDProductType").val();
    $.ajax({
        type: "POST",
        url: "/ProductTypes/DeleteProductTypeRecord?IDProductType=" + IDProductType,
        success: function (result) {
            $("#DeleteConfirmation").modal("hide");
            $(".row_" + IDProductType).remove();
        }
    })
}



