
$(document).ready(function () {
    $("#calculateTotal").on("click", function () {
        calculateTotal();
    });
});

function calculateTotal() {
    var total = 0;
    $(".don-gia").each(function () {
        if ($(this).closest('tr').css('display') !== 'none') {
            var donGia = parseFloat($(this).val().replace(/\./g, "").replace(/,/g, ".")) || 0;
            total += donGia;
        }
    });

    // Hiển thị tổng tiền đã tính được trong phần tử có id là "tong-tien"
    $("#tong-tien").text(total.toLocaleString("vi-VN"));
}

   
    // Sử dụng event delegation cho sự kiện "change" trên .form-control
    $(document).on("change", ".form-control", function () {
        var fileName = $(this).val().split("\\").pop();
        // $(this).siblings(".control-label").addClass("selected").html(fileName);
    });

function DeleteItem(btn) {
    var rows = $('#ExpTable tbody tr');
    var visibleRows = rows.filter(function () {
        return $(this).css('display') !== 'none';
    });

    if (visibleRows.length === 1) {
        alert("Bạn không thể xoá dòng đầu tiên.");
        return;
    }

    var btnIdx = btn.id.replace('btnremove-', '');
    var idofIsDeleted = btnIdx + "__IsDelete";
    var hidIsDelId = document.querySelector("[id$='" + idofIsDeleted + "']").id;
    document.getElementById(hidIsDelId).value = "true";
    $(btn).closest('tr').hide();
    calculateTotal();
}


function AddItem(btn) {

    var table = document.getElementById('ExpTable');
    var rows = table.getElementsByTagName('tr');

    var rowOuterHtml = rows[rows.length - 1].outerHTML;

    var lastrowIdx = document.getElementById('hdnLastIndex').value;

    var nextrowIdx = eval(lastrowIdx) + 1;

    document.getElementById('hdnLastIndex').value = nextrowIdx;

    rowOuterHtml = rowOuterHtml.replaceAll('_' + lastrowIdx + '_', '_' + nextrowIdx + '_');
    rowOuterHtml = rowOuterHtml.replaceAll('[' + lastrowIdx + ']', '[' + nextrowIdx + ']');
    rowOuterHtml = rowOuterHtml.replaceAll('-' + lastrowIdx, '-' + nextrowIdx);


    var newRow = table.insertRow();
    newRow.innerHTML = rowOuterHtml;

    var isDeleteInput = newRow.querySelector('[name*="IsDelete"]');
    if (isDeleteInput) {
        isDeleteInput.value = "False";
    }
    calculateTotal();
    var btnAddID = btn.id;
    var btnDeleteid = btnAddID.replaceAll('btnadd', 'btnremove');
    
    var delbtn = document.getElementById(btnDeleteid);
    delbtn.classList.add("visible");
    delbtn.classList.remove("invisible");
   
    var addbtn = document.getElementById(btnAddID);
    addbtn.classList.remove("visible");
    addbtn.classList.add("invisible");
   
    rebindvalidators();
}
function rebindvalidators() {
    var $form = $("#ApplicantForm");

    $form.unbind();

    $form.data("validator", null);

    $.validator.unobtrusive.parse($form);

    $form.validate($form.data("unobtrusiveValidation").options);
}
       

    var object = { stat: false, ele: null };

    function ConfirmSave(ev) {
        if (object.stat) {
            return true;
        }
        swal({
            title: "Bạn có muốn lưu dữ liệu?",
            text: "Kiểm tra kỹ trước khi lưu!",

            type: "warning",
            icon: "warning",
            showCancelButton: true,
            confirmButtonClass: "btn btn-danger",
            confirmButtonText: "Lưu!",
            cancelButtonText: "Xem lại",
            closeOnConfirm: true
        }, function () {
            swal("Xóa!", "Đã ẩn thành công.", "success");
            object.stat = true;
            object.ele = ev;
            object.ele.click();
        },1000);
        return false;
    }



    function formatCurrency(input) {
        // Lấy giá trị nhập vào
        let value = input.value;

        // Loại bỏ tất cả các dấu chấm và dấu phân cách khác
        value = value.replace(/[^0-9]/g, '');

        // Định dạng lại giá trị thành số tiền có dấu chấm phân cách hàng nghìn
        if (value.length > 3) {
            value = value.replace(/\B(?=(\d{3})+(?!\d))/g, '.');
        }

        // Gán giá trị đã định dạng lại vào trường input
        input.value = value;
    }


// Sử dụng event delegation cho cả document
$(document).on('change', '.TenNhom', function () {
    var id = $(this).val();
    var row = $(this).closest('tr'); // Lấy hàng chứa phần tử đang thay đổi

    $.ajax({
        url: '/BenhNhans/CvTheoNcv',
        data: { id: id },
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            // Tìm phần tử .TenCV trong hàng hiện tại
            var TenCV = row.find('.TenCV');

            // Xóa tất cả các tùy chọn hiện tại trong dropdown "TenCV"
            TenCV.empty();

            TenCV.append($('<option>', {
                value: '',
                text: 'Chọn'
            }));

            // Thêm các tùy chọn mới từ dữ liệu JSON
            $.each(data, function (index, cv) {
                TenCV.append($('<option>', {
                    value: cv.idcvdt,
                    text: cv.tenCv + ' - ' + cv.gia
                }));
            });
        }
    });
});
