


function initializeFlatpickrs() {
    for (let i = 0; i < 100; i++) {
        flatpickr(`#NgayTao-${i}`, {
            altInput: true,
            enableTime: true,      
            time_24hr: true,
            dateFormat: "DD/MM/YYYY HH:MM",
            altFormat: "DD/MM/YYYY HH:MM",
           /* allowInput: true,*/
            "locale": "vn",
            parseDate: (datestr, format) => {
                return moment(datestr, format, true).toDate();
            },
            formatDate: (date, format, locale) => {
                return moment(date).format(format);
            },
        });
    }
}

// Gọi hàm khi DOM đã được load
document.addEventListener('DOMContentLoaded', initializeFlatpickrs);

   
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
    var rowToHide = $(btn).closest('tr');
    var selectElement = rowToHide.find('select.selectIdcvdt');
    selectElement.find('option:contains("-Chọn-")').prop('selected', true);
    selectElement.change(); // Trigger the change event

    rowToHide.hide();

    // Reset the value of SoTienThanhToan to 0 for this row
    var inputSoTien = rowToHide.find('input[name$=".SoTienThanhToan"]');
    inputSoTien.val(0);
    
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
    // Ẩn input có class là "form-control form-control input"
    var newInputs = newRow.getElementsByClassName('form-control form-control input');
    for (var i = 0; i < newInputs.length; i++) {
        var input = newInputs[i];
        input.style.display = "none";
    }

    initializeFlatpickrs();

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
    }, 1000);
    return false;
} function Save(ev) {
    if (object.stat) {
        return true;
    }
    swal({
        title: "Gía tiền có sự chênh lệch",
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
    }, 1000);
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


function calculateTotal() {
    var total = 0;
    $(".don-gia").each(function () {
        var donGia = parseFloat($(this).val().replace(/\./g, "").replace(/,/g, ".")) || 0;
        total += donGia;
    });

    // Hiển thị tổng tiền đã tính được trong phần tử có id là "tong-tien"
    $("#tong-tien").text(total.toLocaleString("vi-VN"));
    checkAndShowSaveButton();
}


function checkAndShowSaveButton() {
    var totalCongViec = parseFloat($("#TienTT").text().replace(/\./g, "").replace(/,/g, ".")) || 0;
    var totalThanhToan = parseFloat($("#tong-tien").text().replace(/\./g, "").replace(/,/g, ".")) || 0;

    if (totalCongViec === totalThanhToan) {
        // Nếu tổng tiền công việc và tổng tiền thanh toán bằng nhau, hiển thị nút "Lưu"
        $(".save-button").show();
    } else {
        // Ngược lại, ẩn nút "Lưu"
        $(".save-button").hide();
    }
}

$(document).ready(function () {
    function checkAndShowSaveButton() {
        var totalCongViec = parseFloat($("#TienTT").text().replace(/\./g, "").replace(/,/g, ".")) || 0;
        var totalThanhToan = parseFloat($("#tong-tien").text().replace(/\./g, "").replace(/,/g, ".")) || 0;

        if (totalCongViec === totalThanhToan) {
            // Nếu tổng tiền công việc và tổng tiền thanh toán bằng nhau, hiển thị nút "Lưu"
            $(".save-button").show();
        } else {
            // Ngược lại, ẩn nút "Lưu"
            $(".save-button").hide();
        }
    }
});