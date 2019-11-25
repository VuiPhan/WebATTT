$("#LoadingStatus").html("Loading....");
$.get("/Suppliers/GetSupplierList", null, DataBind);

function DataBind(SupplierList) {
    var SetData = $("#SetSupplierList");
    for (var i = 0; i < SupplierList.length; i++) {
        var Data = "<tr class='row_" + SupplierList[i].IDSupplier + "'>" +
            "<td>" + SupplierList[i].NameSupplier + "</td>" +
            "<td>" + SupplierList[i].Address + "</td>" +
            "<td>" + SupplierList[i].Email + "</td>" +
            "<td>" + SupplierList[i].SDT + "</td>" +
            "<td>" + SupplierList[i].Fax + "</td>" +
            "<td>" + "<a href='#' class='btn btn-warning' data-toggle='modal' data-target='#MyModal' onclick='EditSupplierRecord(" + SupplierList[i].IDSupplier + ")' ><span class='mdi mdi-table-edit'></span></a>" + "</td>" +
            "<td>" + "<a href='#' class='btn btn-danger' onclick='DeleteSupplierRecord(" + SupplierList[i].IDSupplier + ")'><span class='mdi mdi-delete'></span></a>" + "</td>" +
            "</tr>";
        SetData.append(Data);
        $("#LoadingStatus").html(" ");

    }
}

//Show The Popup Modal For Add New Supplier

function AddNewSupplier(IDSupplier) {
    $("#form")[0].reset();
    $("#IDSupplier").val(0);
    $("#MyModal").modal();

}


//Show The Popup Modal For Edit Supplier Record

function EditSupplierRecord(IDSupplier) {
    var url = "/Suppliers/GetSupplierById?IDSupplier=" + IDSupplier;
    $("#MyModal").modal();
    $.ajax({
        type: "GET",
        url: url,
        success: function (data) {
            var obj = JSON.parse(data);
            $("#IDSupplier").val(obj.IDSupplier);
            $("#NameSupplier").val(obj.NameSupplier);
            $("#Address").val(obj.Address);
            $("#Email").val(obj.Email);
            $("#SDT").val(obj.SDT);
            $("#Fax").val(obj.Fax);
        }
    })
}

var SaveSupplierRecord = function () {
    var data = $("#SubmitForm").serialize();
    if (!$('#NameSupplier').val() || !$('#Address').val() || !$('#Email').val() || !$('#SDT').val() || !$('#Fax').val()) {
        $('#NameSupplier').css('border-color', 'Red');
        $('#Address').css('border-color', 'Red');
        $('#Email').css('border-color', 'Red');
        $('#SDT').css('border-color', 'Red');
        $('#Fax').css('border-color', 'Red');
        return alert('Không được bỏ trống mục nào');
    }
    $.ajax({
        type: "Post",
        url: "/Suppliers/SaveDataInDatabase",
        data: data,
        success: function (result) {
            window.location.href = "/WarehouseStaff/Suppliers/Suppliers";
            $("#MyModal").modal("hide");
        }
    })
}

//Show The Popup Modal For DeleteComfirmation

var DeleteSupplierRecord = function (IDSupplier) {
    $("#IDSupplier").val(IDSupplier);
    $("#DeleteConfirmation").modal("show");
}
var ConfirmDelete = function () {
    var IDSupplier = $("#IDSupplier").val();
    $.ajax({
        type: "POST",
        url: "/Suppliers/DeleteSupplierRecord?IDSupplier=" + IDSupplier,
        success: function (result) {
            $("#DeleteConfirmation").modal("hide");
            $(".row_" + IDSupplier).remove();
        }
    })
}
