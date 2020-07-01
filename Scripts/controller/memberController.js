
var memberconfig = {
    pageSize: 3,
    pageIndex: 1,
}

var memberController = {
    init: function () {
        memberController.loadData();
        memberController.registerEvent();
    },

    registerEvent: function () {

        $('#btnAddNew').off('click').on('click', function () {
            $('#modalAddUpdate').modal('show');
            memberController.resetForm();
        });

        $('.btnEdit').off('click').on('click', function () {
            $('#modalAddUpdate').modal('show');
            var id = $(this).data('id');
            memberController.loadDetail(id);
        });

        $('.btnDelete').off('click').on('click', function () {
            var id = $(this).data('id');
            bootbox.confirm("Are you sure to delete this member?", function (result) {
                if (result == true) {
                    memberController.deleteMember(id);
                }
            });
        });

        $('#btnReset').off('click').on('click', function () {
            $('#txtSearch').val('');
            memberController.loadData(true);
        });

        $('#btnSearch').off('click').on('click', function () {
            memberController.loadData(true);
        });

        $('#txtSearch').off('keypress').on('keypress', function (e) {
            if (e.which == 13) {
                memberController.loadData(true);
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
                txtEmail: "email",
                txtAddress: "required"


            },
            messages: {
                txtUserName: "Không được bỏ trống UserName!!",
                txtPassword: "Không được bỏ trống PassWord!!",
                txtFullName: "Không được bỏ trống FullName!!",
                txtEmail: "Phải nhập đúng định dạng Email!!",
                txtAddress: "Không được bỏ trống Address!!"
            }
        });

        $('#btnSave').off('click').on('click', function () {
            if ($('#frmSave').valid()) {
                memberController.saveData();
            }
        });


    },

    loadData: function (changePageSize) {
        var name = $('#txtSearch').val();
        $.ajax({
            url: '/Admin/Member/LoadData',
            type: 'GET',
            data: {
                name: name,
                page: memberconfig.pageIndex,
                pageSize: memberconfig.pageSize
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
                            Address: item.Address,
                            Email: item.Email,
                            PhoneNumber: item.PhoneNumber,
                            Avatar: item.Avatar
                        });
                    });
                    $('#tblData').html(html);
                    memberController.paging(response.total, changePageSize);
                    memberController.registerEvent();
                }
            }

        });
    },

    saveData: function () {
        var username = $('#txtUserName').val();
        var password = $('#txtPassword').val();
        var fullname = $('#txtFullName').val();
        var address = $('#txtAddress').val();
        var email = $('#txtEmail').val();
        var phonenumber = $('#txtPhoneNumber').val();
        var avatar = $('#urlAvatar').val();
        var id = parseInt($('#hidID').val());
        $.ajax({
            url: '/Admin/Member/SaveData',
            data: {
                member: {
                    UserName: username,
                    PassWord: password,
                    FullName: fullname,
                    Address: address,
                    Email: email,
                    PhoneNumber: phonenumber,
                    Avatar: avatar,
                    IDMember: id
                }
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    bootbox.alert("Save Success!!", function () {
                        $('#modalAddUpdate').modal('hide');
                        memberController.loadData(true);
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
            url: '/Admin/Member/GetDetail',
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
                $('#txtAddress').val(data.Address);
                $('#txtEmail').val(data.Email);
                $('#txtPhoneNumber').val(data.PhoneNumber);
                $('#urlAvatar').val(data.Avatar);
                $('#hidID').val(data.IDMember);
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
        $('#txtAddress').val('');
        $('#txtEmail').val('');
        $('#txtPhoneNumber').val('');
        $('#urlAvatar').val('');
        $('#hidID').val('0');
    },

    deleteMember: function (id) {
        $.ajax({
            url: '/Admin/Member/DeleteMember',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    bootbox.alert("Delete Success!!", function () {
                        memberController.loadData(true);
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

    paging: function (totalRow, changePageSize) {
        var totalPage = Math.ceil(totalRow / memberconfig.pageSize);

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
                memberconfig.pageIndex = page;
                setTimeout(memberController.loadData(), 200);
            }
        });
    }
}
memberController.init();


