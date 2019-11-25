$("#LoadingStatus").html("Loading....");
$.get("/WareHouses/GetWareHouseList", null, DataBind);

function DataBind(WareHouseList) {
    var SetData = $("#SetWareHouseList");
    for (var i = 0; i < WareHouseList.length; i++) {
        var Data = "<tr class='row_" + WareHouseList[i].IDWareHouse + "'>" +
            "<td>" + WareHouseList[i].Address + "</td>" +
            "<td>" + WareHouseList[i].Amount + "</td>" +
            "<td>" + "<a href='#' class='btn btn-warning' data-toggle='modal' data-target='#MyModal' onclick='EditWareHouseRecord(" + WareHouseList[i].IDWareHouse + ")' ><span class='mdi mdi-table-edit'></span></a>" + "</td>" +
            "<td>" + "<a href='#' class='btn btn-danger' onclick='DeleteWareHouseRecord(" + WareHouseList[i].IDWareHouse + ")'><span class='mdi mdi-delete'></span></a>" + "</td>" +
            "</tr>";
        SetData.append(Data);
        $("#LoadingStatus").html(" ");

    }
}

//Show The Popup Modal For Add New WareHouse

function AddNewWareHouse(IDWareHouse) {
    $("#form")[0].reset();
    $("#IDWareHouse").val(0);
    $("#MyModal").modal();
}

//Show The Popup Modal For Edit WareHouse Record

function EditWareHouseRecord(IDWareHouse) {
    var url = "/WareHouses/GetWareHouseById?IDWareHouse=" + IDWareHouse;
    $("#MyModal").modal();
    $.ajax({
        type: "GET",
        url: url,
        success: function (data) {
            var obj = JSON.parse(data);
            $("#IDWareHouse").val(obj.IDWareHouse);
            $("#Address").val(obj.Address);
            $("#Amount").val(obj.Amount);
        }
    })
}

var SaveWareHouseRecord = function () {
    var data = $("#SubmitForm").serialize();
    if (!$('#Address').val() || !$('#Amount').val()) {
        return alert('Không được bỏ trống mục nào');
    }
    $.ajax({
        type: "Post",
        url: "/WareHouses/SaveDataInDatabase",
        data: data,
        success: function (result) {
            window.location.href = "/WarehouseStaff/WareHouses/WareHouses";
            $("#MyModal").modal("hide");
        }
    })
}

//Show The Popup Modal For DeleteComfirmation

var DeleteWareHouseRecord = function (IDWareHouse) {
    $("#IDWareHouse").val(IDWareHouse);
    $("#DeleteConfirmation").modal("show");
}
var ConfirmDelete = function () {
    var IDWareHouse = $("#IDWareHouse").val();
    $.ajax({
        type: "POST",
        url: "/WareHouses/DeleteWareHouseRecord?IDWareHouse=" + IDWareHouse,
        success: function (result) {
            $("#DeleteConfirmation").modal("hide");
            $(".row_" + IDWareHouse).remove();
        }
    })
}