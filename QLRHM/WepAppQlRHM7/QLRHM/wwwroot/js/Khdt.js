

$("#Search").on("keyup", function () {
    var searchText = $(this).val().toLowerCase();
    $(".Search").filter(function () {
        $(this).toggle($(this).text().toLowerCase().indexOf(searchText) > -1);
    });
});



$(document).ready(function () {
    $("#Search1").on("keyup", function () {
        var searchText = $(this).val().toLowerCase();
        $(".Search").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(searchText) > -1);
        });
    });
});

$(document).ready(function () {
    $('.open-modal').click(function () {
        var url = $(this).data('url');
        $.get(url, function (data) {
            $('#partial-view-container').html(data);
            $('#myModal').modal('show');
        });
    });
});
$(document).ready(function () {
    $('.open-modal2').click(function () {
        var url = $(this).data('url');
        $.get(url, function (data) {
            $('#partial-view-container2').html(data);
            $('#myModal2').modal('show');
        });
    });
});
$(document).ready(function () {
    $('.open-modal3').click(function () {
        var url = $(this).data('url');
        $.get(url, function (data) {
            $('#partial-view-container3').html(data);
            $('#myModal3').modal('show');
        });
    });
});
$(document).ready(function () {
    $('.open-modal4').click(function () {
        var url = $(this).data('url');
        $.get(url, function (data) {
            $('#partial-view-container4').html(data);
            $('#myModal4').modal('show');
        });
    });
});
$(document).ready(function () {
    $('.open-modal5').click(function () {
        var url = $(this).data('url');
        $.get(url, function (data) {
            $('#partial-view-container5').html(data);
            $('#myModal5').modal('show');
        });
    });
});

$(document).ready(function () {
    $('.open-modal6').click(function () {
        var url = $(this).data('url');
        $.get(url, function (data) {
            $('#partial-view-container6').html(data);
            $('#myModal6').modal('show');
        });
    });
});

$(document).ready(function () {
    $('.open-modal7').click(function () {
        var url = $(this).data('url');
        $.get(url, function (data) {
            $('#partial-view-container7').html(data);
            $('#myModal7').modal('show');
        });
    });
});
$(document).ready(function () {
    $('.open-modal8').click(function () {
        var url = $(this).data('url');
        $.get(url, function (data) {
            $('#partial-view-container8').html(data);
            $('#myModal8').modal('show');
        });
    });
});

var object = { stat: false, ele: null };

function ConfirmDelete(ev) {
    if (object.stat) {
        return true;
    }
    swal({
        title: "Bạn muốn ẩn dữ liệu này?",
        text: "Dữ liệu này sẽ mất khỏi bảng!",

        type: "warning",
        icon: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn btn-danger",
        confirmButtonText: "Xóa!",
        closeOnConfirm: true
    }, function () {
        swal("Xóa!", "Đã ẩn thành công.", "success");
        object.stat = true;
        object.ele = ev;
        object.ele.click();
    }, 1000);
    return false;
} var object = { stat: false, ele: null };

function ConfirmActive(ev) {
    if (object.stat) {
        return true;
    }
    swal({
        title: "Bạn muốn ẩn dữ liệu này?",
        text: "Dữ liệu này sẽ mất khỏi bảng!",

        type: "warning",
        icon: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn btn-danger",
        confirmButtonText: "Xóa!",
        closeOnConfirm: true
    },
        function () {
            swal("Xóa!", "Đã Hiện thành công.", "success");
            object.stat = true;
            object.ele = ev;
            object.ele.click();

        }, 1000);
    return false;
}



var object = { stat: false, ele: null };

function ThanhToan(link) {
    var tenPhong = link.getAttribute("data-name");
    if (object.stat) {
        return true;
    }

    swal({
        title: "Không có công việc nào cần thanh toán",
        text: "Kiểm tra lại kế hoạch của bênh nhân: " + tenPhong,
        type: "warning",
        icon: "warning",
        showCancelButton: true,
        confirmButtonClass: false,
        confirmButtonText: false,
        cancelButtonText: "Đóng",
        closeOnConfirm: true
    }, function () {
        swal("Xoá!", "Đã Hiện thành công.", "success");
        object.stat = true;
        object.ele = link;
        object.ele.click();
    });

    return false;
}
function XoaLichHen(link) {
    var tenPhong = link.getAttribute("data-name");
    if (object.stat) {
        return true;
    }

    swal({
        title: "Bạn có muốn xoá: " + tenPhong,
        
        type: "warning",
       
        showCancelButton: true,
        confirmButtonClass: false,
       
        cancelButtonText: "Đóng",
        closeOnConfirm: true
    }, function () {
        swal("Xoá!", "Đã Hiện thành công.", "success");
        object.stat = true;
        object.ele = link;
        object.ele.click();
    });

    return false;
}

function XoaToaThuoc(link) {
    var tenPhong = link.getAttribute("data-name");
    if (object.stat) {
        return true;
    }

    swal({
        title: "Bạn có muốn xoá toa thuốc: " + tenPhong,

        type: "warning",

        showCancelButton: true,
        confirmButtonClass: false,

        cancelButtonText: "Đóng",
        closeOnConfirm: true
    }, function () {
        swal("Xoá!", "Đã Hiện thành công.", "success");
        object.stat = true;
        object.ele = link;
        object.ele.click();
    });

    return false;
}

function XacNhanThanhToan(link) {
    var TenBenhNhan = link.getAttribute("data-name");
    var tenCV = link.getAttribute("data-name2");
    var SoTien = link.getAttribute("data-name3");
    if (object.stat) {
        return true;
    }

    swal({
        title: "Thanh toán bệnh nhân\n" + TenBenhNhan,
        text: "Công việc: " + tenCV + ", tổng tiền: " + SoTien,
        //type: "warning",
        //icon: "warning",
        showCancelButton: true,
        confirmButtonClass: false,
        confirmButtonText: false,
        cancelButtonText: "Đóng",
        closeOnConfirm: true
    }, function () {
        swal("Xoá!", "Đã Hiện thành công.", "success");
        object.stat = true;
        object.ele = link;
        object.ele.click();
    });

    return false;
}
function XacNhanDieuTri(link) {
    var MaKHDT = link.getAttribute("data-name");
    
    if (object.stat) {
        return true;
    }

    swal({
        title: "Xác nhận điều trị kế hoạch\n" + MaKHDT,
        text: "Tiếp tục chuyển sang thanh toán nếu đồng ý",
        //type: "warning",
        //icon: "warning",
        showCancelButton: true,
        confirmButtonClass: false,
        confirmButtonText: false,
        cancelButtonText: "Đóng",
        closeOnConfirm: true
    }, function () {
        swal("Xoá!", "Đã Hiện thành công.", "success");
        object.stat = true;
        object.ele = link;
        object.ele.click();
    });

    return false;
}
function XacNhanDenHen(link) {
    var MaKHDT = link.getAttribute("data-name");
    
    if (object.stat) {
        return true;
    }

    swal({
        title: "Xác nhận lịch hẹn\n" + MaKHDT,
        text: "Bệnh nhând dã đến hẹn",
       
        showCancelButton: true,
        confirmButtonClass: false,
        confirmButtonText: false,
        cancelButtonText: "Đóng",
        closeOnConfirm: true
    }, function () {
        swal("Xoá!", "Đã Hiện thành công.", "success");
        object.stat = true;
        object.ele = link;
        object.ele.click();
    });

    return false;
}


function XacNhanHoanTatDieuTri(link) {
    var MaKHDT = link.getAttribute("data-name");
    
    if (object.stat) {
        return true;
    }

    swal({
        title: "Xác nhận hoàn tất điều trị kế hoạch\n" + MaKHDT,
       
        type: "success",
        icon: "success",
        showCancelButton: true,
        confirmButtonClass: false,
        confirmButtonText: false,
        cancelButtonText: "Đóng",
        closeOnConfirm: true
    }, function () {
        swal("Xoá!", "Đã Hiện thành công.", "success");
        object.stat = true;
        object.ele = link;
        object.ele.click();
    });

    return false;
}
    document.addEventListener('DOMContentLoaded', function () {

        flatpickr(".LichHen", {
            altInput: true,
            minDate: "today",
            dateFormat: "DD/MM/YYYY",
            altFormat: "DD/MM/YYYY",
            /*allowInput: true,*/
            "locale": "vn",
            parseDate: (datestr, format) => {
                return moment(datestr, format, true).toDate();
            },
            formatDate: (date, format, locale) => {
                return moment(date).format(format);
            },
        });

     });
      document.addEventListener('DOMContentLoaded', function () {

        flatpickr(".GioHen", {
            enableTime: true,
            noCalendar: true,
            dateFormat: "H:i",
            time_24hr: true
        });

     });


