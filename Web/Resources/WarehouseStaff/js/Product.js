$.get("/Products/GetProductList", null, DataBind);

function DataBind(ProductList) {
    var SetData1 = $("#SetProductList1");
    for (var i = 0; i < ProductList.length; i++) {
        var Data = "<tr class='row_" + ProductList[i].IDProduct + "'>" +
            "<td>" + ProductList[i].NameProduct + "</td>" +
            "<td>" + ProductList[i].Price + "</td>" +
            "<td>" + ProductList[i].YearManufacture + "</td>" +
            "<td>" + ProductList[i].Introduce + "</td>" +
            "<td>" + ProductList[i].Description + "</td>" +
            //"<td>" + ProductList[i].Image0 + "</td>" +
            //"<td>" + ProductList[i].Image1 + "</td>" +
            //"<td>" + ProductList[i].Image2 + "</td>" +
            //"<td>" + ProductList[i].Image3 + "</td>" +
            "<td>" + ProductList[i].NumberOfPurchases + "</td>" +
            
            "<td>" + ProductList[i].New + "</td>" +
            "<td>" + "<a href='#' class='btn btn-warning' data-toggle='modal' data-target='#MyModal' onclick='EditProductRecord(" + ProductList[i].IDProduct + ")' ><span class='mdi mdi-table-edit'></span></a>" + "</td>" +
            "</tr>";
        SetData1.append(Data);
    }
    var SetData2 = $("#SetProductList2");
    for (var i = 0; i < ProductList.length; i++) {
        var Data = "<tr class='row_" + ProductList[i].IDProduct + "'>" +
            "<td>" + ProductList[i].DateUpdate + "</td>" +
            "<td>" + ProductList[i].NameProducer + "</td>" +
            "<td>" + ProductList[i].NameProductType + "</td>" +
            "<td>" + ProductList[i].Address + "</td>" +
            "<td>" + ProductList[i].NameSupplier + "</td>" +
            "<td>" + ProductList[i].RemainingAmount + "</td>" +
            "<td>" + "<a href='#' class='btn btn-danger' onclick='DeleteProductRecord(" + ProductList[i].IDProduct + ")'><span class='mdi mdi-delete'></span></a>" + "</td>" +
            "</tr>";
        SetData2.append(Data);
    }
}

//Show The Popup Modal For Add New Product

function AddNewProduct(IDProduct) {
    $("#form")[0].reset();
    $("#IDProduct").val(0);
    $("#DropDwnPro option:selected").text("--Sản xuất bởi--");
    $("#DropDwnProType option:selected").text("--Loại sản phẩm--");
    $("#DropDwnWar option:selected").text("--Kho hàng tại--");
    $("#DropDwnSup option:selected").text("--Cung cấp bởi--");
    $("#MyModal").modal();

}


//Show The Popup Modal For Edit Product Record

function EditProductRecord(IDProduct) {
    var url = "/Products/GetProductById?IDProduct=" + IDProduct;
    $("#MyModal").modal();
    $.ajax({
        type: "GET",
        url: url,
        success: function(data) {
            var obj = JSON.parse(data);
            $("#IDProduct").val(obj.IDProduct);
            $("#NameProduct").val(obj.NameProduct);
            $("#Price").val(obj.Price);
            $("#YearManufacture").val(obj.YearManufacture);
            $("#Introduce").val(obj.Introduce);
            $("#Description").val(obj.Description);
            //$("#Image0").val(obj.Image0);
            //$("#Image1").val(obj.Image1);
            //$("#Image2").val(obj.Image2);
            //$("#Image3").val(obj.Image3);
            $("#NumberOfPurchases").val(obj.NumberOfPurchases);
            $("#New").val(obj.New);
            $("#DateUpdate").val(obj.DateUpdate);
            //$("#DropDwnPro option:selected").text(obj.Producer.NameProducer);
            $("#DropDwnPro option:selected").val(obj.IDProducer);
           // $("#DropDwnProType option:selected").text(obj.ProductType.NameProductType);
            $("#DropDwnProType option:selected").val(obj.IDProductType);
           // $("#DropDwnWar option:selected").text(obj.WareHouse.Address);
            $("#DropDwnWar option:selected").val(obj.IDWareHouse);
            //$("#DropDwnSup option:selected").text(obj.Supplier.NameSupplier);
            $("#DropDwnSup option:selected").val(obj.IDSupplier);
            $("#RemainingAmount").val(obj.RemainingAmount);
        }
    })
}

var SaveProductRecord = function() {
    var data = $("#SubmitForm").serialize();
    if (!$('#NameProduct').val() || !$('#Price').val()  || !$('#Introduce').val() || !$('#Description').val() || !$('#NumberOfPurchases').val()) {
        if ($('#NameProduct').val().trim() == "") {
            $('#NameProduct').css('border-color', 'Red');
        }
        else {
            $('#NameProduct').css('border-color', 'lightgrey');
        }
        if ($('#Price').val().trim() == "") {
            $('#Price').css('border-color', 'Red');
        }
        else {
            $('#Price').css('border-color', 'lightgrey');
        }        
        if ($('#Introduce').val().trim() == "") {
            $('#Introduce').css('border-color', 'Red');
        }
        else {
            $('#Introduce').css('border-color', 'lightgrey');
        }
        if ($('#Description').val().trim() == "") {
            $('#Description').css('border-color', 'Red');
        }
        else {
            $('#Description').css('border-color', 'lightgrey');
        }       
        if ($('#NumberOfPurchases').val().trim() == "") {
            $('#NumberOfPurchases').css('border-color', 'Red');
        }
        else {
            $('#NumberOfPurchases').css('border-color', 'lightgrey');
        }
              
        return alert('Không được bỏ trống mục nào');
    }
    $.ajax({
        type: "Post",
        url: "/Products/SaveDataInDatabase",
        data: data,
        success: function(result) {
            window.location.href = "/WarehouseStaff/Products/Products";
            $("#MyModal").modal("hide");
        }
    })
}

//Show The Popup Modal For DeleteComfirmation

var DeleteProductRecord = function(IDProduct) {
    $("#IDProduct").val(IDProduct);
    $("#DeleteConfirmation").modal("show");
}
var ConfirmDelete = function() {
    var IDProduct = $("#IDProduct").val();
    $.ajax({
        type: "POST",
        url: "/Products/DeleteProductRecord?IDProduct=" + IDProduct,
        success: function(result) {
            $("#DeleteConfirmation").modal("hide");
            $(".row_" + IDProduct).remove();
        }
    })
}
