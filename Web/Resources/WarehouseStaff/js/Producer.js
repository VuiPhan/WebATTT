$("#LoadingStatus").html("Loading....");
$.get("/Producers/GetProducerList", null, DataBind);

function DataBind(ProducerList) {
    var SetData = $("#SetProducerList");
    for (var i = 0; i < ProducerList.length; i++) {
        var Data = "<tr class='row_" + ProducerList[i].IDProducer + "'>" +
            "<td>" + ProducerList[i].NameProducer + "</td>" +
            "<td>" + ProducerList[i].Information + "</td>" +
            "<td>" + ProducerList[i].Logo + "</td>" +
            "<td>" + "<a href='#' class='btn btn-warning' data-toggle='modal' data-target='#MyModal' onclick='EditProducerRecord(" + ProducerList[i].IDProducer + ")' ><span class='mdi mdi-table-edit'></span></a>" + "</td>" +
            "<td>" + "<a href='#' class='btn btn-danger' onclick='DeleteProducerRecord(" + ProducerList[i].IDProducer + ")'><span class='mdi mdi-delete'></span></a>" + "</td>" +
            "</tr>";
        SetData.append(Data);
        $("#LoadingStatus").html(" ");

    }
}

//Show The Popup Modal For Add New Producer

function AddNewProducer(IDProducer) {
    $("#form")[0].reset();
    $("#IDProducer").val(0);
    $("#MyModal").modal();

}


//Show The Popup Modal For Edit Producer Record

function EditProducerRecord(IDProducer) {
    var url = "/Producers/GetProducerById?IDProducer=" + IDProducer;
    $("#MyModal").modal();
    $.ajax({
        type: "GET",
        url: url,
        success: function (data) {
            var obj = JSON.parse(data);
            $("#IDProducer").val(obj.IDProducer);
            $("#NameProducer").val(obj.NameProducer);
            $("#Information").val(obj.Information);
            $("#Logo").val(obj.Logo);

        }
    })
}

var SaveProducerRecord = function () {
    var data = $("#SubmitForm").serialize();
    if (!$('#NameProducer').val() || !$('#Information').val() || !$('#Logo').val()) {
        return alert('Không được bỏ trống mục nào');
    }
    $.ajax({
        type: "Post",
        url: "/Producers/SaveDataInDatabase",
        data: data,
        success: function (result) {
            window.location.href = "/WarehouseStaff/Producers/Producers";
            $("#MyModal").modal("hide");
        }
    })
}

//Show The Popup Modal For DeleteComfirmation

var DeleteProducerRecord = function (IDProducer) {
    $("#IDProducer").val(IDProducer);
    $("#DeleteConfirmation").modal("show");
}
var ConfirmDelete = function () {
    var IDProducer = $("#IDProducer").val();
    $.ajax({
        type: "POST",
        url: "/Producers/DeleteProducerRecord?IDProducer=" + IDProducer,
        success: function (result) {
            $("#DeleteConfirmation").modal("hide");
            $(".row_" + IDProducer).remove();
        }
    })
}



