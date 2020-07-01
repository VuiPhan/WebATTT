
var employeeconfig = {
    pageSize: 3,
    pageIndex: 1,
}

var employeeController = {
    init: function () {
        employeeController.loadData(true,0);
        employeeController.registerEvent();
    },
   
    registerEvent: function (idtab) {
        $('.tabEmployee').off('click').on('click', function () {
            idtab = $(this).data('id');
            employeeController.loadData(true, idtab);
        });


        $('#btnSave').off('click').on('click', function () {
            if ($('#frmSave').valid()) {
                employeeController.saveData(idtab);
            }
        });

        $('#btnAddNew').off('click').on('click', function () {
            $('#modalAddUpdate').modal('show');
            employeeController.resetForm();
        });

        $('.btnEdit').off('click').on('click', function () {
            $('#modalAddUpdate').modal('show');
            var id = $(this).data('id');
            employeeController.loadDetail(id);
           
        });

        $('.btnDelete').off('click').on('click', function () {
            var id = $(this).data('id');
            bootbox.confirm("Are you sure to delete this employee?", function (result) {
                if (result == true) {
                    employeeController.deleteEmployee(id,idtab);
                }
            });
        });

        $('#btnReset').off('click').on('click', function () {
            $('#txtSearch').val('');
            employeeController.loadData(true, idtab);
        });

        $('#btnSearch').off('click').on('click', function () {
            employeeController.loadData(true, idtab);
        });

        $('#txtSearch').off('keypress').on('keypress', function (e) {
            if (e.which == 13) {
                employeeController.loadData(true, idtab);
            }
        });

        $('#btnUpload').off('click').on('click', function (e) {
            e.preventDefault();
            var finder = new CKFinder();
            finder.selectActionFunction = function (url) {
                $('#urlAvatar').val(url);
            };
            finder.popup();
        });

        $('#frmSave').validate({
            rules: {
                txtUserName: "required",
                txtPassword: "required",
                txtFullName: "required",
                txtIDCard: {
                    required: true,
                    rangelength: [9,9],
                    number: true
                },
                txtEmail: "email",
                txtSalary: {
                    required: true,
                    number: true
                }
            },

            messages: {
                txtUserName: "Không được bỏ trống UserName!!",
                txtPassword: "Không được bỏ trống PassWord!!",
                txtFullName: "Không được bỏ trống FullName!!",
                txtIDCard: {
                    required: "Không được bỏ trống IdCard!!",
                    rangelength: "IdCard cần đủ 9 kí tự!!",
                    number: "Phải nhập số!!"
                },
                txtEmail: "Phải nhập đúng định dạng Email!!",
                txtSalary: {
                    required: "Không được bỏ trống Salary!!",
                    number: "Salary phải là số!!"
                }
            }
        });
    },
    loadData: function (changePageSize,type) {
        var name = $('#txtSearch').val();
        $.ajax({
            url: '/Admin/Employee/LoadData',
            type: 'GET',
            data: {
                type: type,
                name: name,
                page: employeeconfig.pageIndex,
                pageSize: employeeconfig.pageSize
            },
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            IDMember: item.IDMember,
                            UserName: item.UserName,
                            PassWord: item.PassWord,
                            FullName: item.FullName,
                            IDCard: item.IDCard,
                            Email: item.Email,
                            Address: item.Address,
                            PhoneNumber: item.PhoneNumber,
                            Salary: item.Salary,
                            Avatar: item.Avatar
                        });
                    });
                    $('#tblData').html(html);
                    var idtab = type;
                    employeeController.paging(response.total, changePageSize, idtab);
                    employeeController.registerEvent(idtab);
                }
            }

        });
    },

    saveData: function (idtab) {
        var username = $('#txtUserName').val();
        var password = $('#txtPassword').val();
        var fullname = $('#txtFullName').val();
        var address = $('#txtAddress').val();
        var phonenumber = $('#txtPhoneNumber').val();
        var avatar = $('#urlAvatar').val();
        var salary = $('#txtSalary').val();
        var idcard = $('#txtIDCard').val();
        var email = $('#txtEmail').val();
        var id = parseInt($('#hidID').val());
        var idmemtype = parseInt($('#DropDwnMemtype').val());
        $.ajax({
            url: '/Admin/Employee/SaveData',
            data: {
                employee: {
                    UserName: username,
                    PassWord: password,
                    FullName: fullname,
                    Address: address,
                    IDCard: idcard,
                    Email: email,
                    PhoneNumber: phonenumber,
                    Salary: salary,
                    Avatar: avatar,
                    IDMember: id,
                    IDMemType: idmemtype
                },
                IdMemType: idmemtype
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    bootbox.alert("Save Success!!", function () {
                        $('#modalAddUpdate').modal('hide');
                        employeeController.loadData(true, idtab);
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
            url: '/Admin/Employee/GetDetail',
            data: {
                id: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                var data = response.data;
                $('#txtUserName').val(data.UserName);
                $('#txtPassword').val(data.PassWord);
                $('#txtFullName').val(data.FullName);
                $('#txtIDCard').val(data.IDCard);
                $('#txtEmail').val(data.Email);
                $('#txtAddress').val(data.Address);
                $('#txtPhoneNumber').val(data.PhoneNumber);
                $('#txtSalary').val(data.Salary);
                $('#urlAvatar').val(data.Avatar);
                $('#hidID').val(data.IDMember);
                $('#DropDwnMemtype').val(data.IDMemType);
            },
            error: function (err) {
                console.log(err);
            }
        });
    },

    resetForm: function () {
        $('#txtUserName').val('');
        $('#txtPassword').val('');
        $('#txtFullName').val('');
        $('#txtIDCard').val('');
        $('#txtEmail').val('');
        $('#txtAddress').val('');
        $('#txtPhoneNumber').val('');
        $('#txtSalary').val('');
        $('#urlAvatar').val('');
        $('#hidID').val('0');
    },

    deleteEmployee: function (id,idtab) {
        $.ajax({
            url: '/Admin/Employee/DeleteEmployee',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    bootbox.alert("Delete Success!!", function () {
                        employeeController.loadData(true,idtab);
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

    paging: function (totalRow, changePageSize,idtab) {
        var totalPage = Math.ceil(totalRow / employeeconfig.pageSize);

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
                employeeconfig.pageIndex = page;
                setTimeout(employeeController.loadData(false, idtab), 200);
            }
        });
    }
}
employeeController.init();


