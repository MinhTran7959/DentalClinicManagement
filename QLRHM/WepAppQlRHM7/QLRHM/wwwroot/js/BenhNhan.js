

    function showButton(selectElement) {
        var button = selectElement.parentNode.querySelector('button');
        if (selectElement.value === "1") {
            button.style.display = "none";
        } else {
            button.style.display = "block";
        }
    }

    // Call the function when the page loads
    document.addEventListener("DOMContentLoaded", function () {
        var selectElements = document.querySelectorAll('.demo-consoles.selectize');
        selectElements.forEach(function (selectElement) {
            showButton(selectElement);
        });
    });



    var object = { stat: false, ele: null };

function AnBenhNhan(link) {
    var tenBenhNhan = link.getAttribute("data-name");
        if (object.stat) {
            return true;
        }
    swal({
        title: "Bạn có muốn ẩn dữ liệu của \n bệnh nhân: " + tenBenhNhan +"?",
         

            type: "warning",
            icon: "warning",
            showCancelButton: true,
            confirmButtonClass: "btn btn-danger",
            confirmButtonText: "Ẩn!",
            cancelButtonText: "Huỷ",
            closeOnConfirm: true
    },
        function () {
            swal("Xóa!", "Đã ẩn thành công.", "success");
            object.stat = true;
        object.ele = link;
            object.ele.click();
        });
        return false;
    }

    var object = { stat: false, ele: null };

    function PhongKham(checkbox) {
        var tenBenhNhan = checkbox.getAttribute("data-name");

        if (object.stat) {
            return true;
        }

        swal({
            title: "Chuyển bệnh nhân vào phòng khám ?",
            text: "Bệnh nhân: " + tenBenhNhan,
            showCancelButton: true,
            confirmButtonClass: "btn btn-danger",
            confirmButtonText: "Hoàn tất!",
            cancelButtonText: "Huỷ",
            closeOnConfirm: true
        }, function () {
            swal("Chuyển!", "Đã ẩn thành công.", "success");
            object.stat = true;
            object.ele = checkbox;
            object.ele.click();
        });

        return false;
}
var object = { stat: false, ele: null };


function KhoiPhuc(link) {
    var tenBenhNhan = link.getAttribute("data-name");

        if (object.stat) {
            return true;
        }

        swal({
            title: "Khôi phục bệnh nhân:\n "  + tenBenhNhan +"?",
         /*   text: "Bệnh nhân: " + tenBenhNhan,*/
            showCancelButton: true,
            confirmButtonClass: "btn btn-danger",
            confirmButtonText: "Khôi phục!",
            cancelButtonText: "Huỷ",
            closeOnConfirm: true
        }, function () {
            swal("Chuyển!", "Đã ẩn thành công.", "success");
            object.stat = true;
            object.ele = link;
            object.ele.click();
        });

        return false;
    }

    function capitalizeFirstLetter(input) {
        var words = input.value.split(' ');
        var capitalizedWords = words.map(function (word) {
            return word.charAt(0).toUpperCase() + word.slice(1);
        });
        input.value = capitalizedWords.join(' ');
    }
