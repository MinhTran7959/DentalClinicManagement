
    $(document).ready(function () {
        // Sử dụng sự kiện change cho select element với ID là "Phong"
        $("#Phong2").on("change", function () {
            var searchText = $(this).val().toLowerCase();
            $(".Phong2").filter(function () {
                $(this).toggle($(this).text().toLowerCase().indexOf(searchText) > -1);
            });
        });
        // Lấy giá trị đã chọn từ localStorage và đặt giá trị cho select element
        var selectedPhong2 = localStorage.getItem('Phong2');
        if (selectedPhong2) {
            $("#Phong2").val(selectedPhong2);
            // Kích hoạt sự kiện change sau khi đặt giá trị
            $("#Phong2").trigger("change");
        }
        // Lưu giá trị khi có sự thay đổi
        $("#Phong2").on("change", function () {
            var selectedValue = $(this).val();
            localStorage.setItem('Phong2', selectedValue);
        });
    });

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

    function KetThucKham(checkbox) {
        var tenBenhNhan = checkbox.getAttribute("data-name");

        if (object.stat) {
            return true;
        }

        swal({
            title: "Kết thúc đợt khám khám bệnh ?",
            text: "Bệnh nhân: " + tenBenhNhan,
            showCancelButton: true,
            confirmButtonClass: "btn btn-danger",
            confirmButtonText: "Hoàn tất!",
            cancelButtonText: "Huỷ",
            closeOnConfirm: true,
           
        }, function () {
            swal({
               
                title: "Hoàn tất",
                icon: "success",
                button: "Tiếp tục",

            });
            object.stat = true;
            object.ele = checkbox;
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

    function speakText(benhnhanId) {
        // Thay đổi ID của phần tử cần đọc
        var elementId = "myText_" + benhnhanId;

        // Lấy nội dung từ phần tử cụ thể
        var mainText = document.getElementById(elementId).textContent;

        // Tách nội dung thành tên bệnh nhân và ngày sinh
        var texts = mainText.split(" - ");

        // Lấy tên bệnh nhân và ngày sinh
        var tenBn = texts[0];
        var ngaySinh = texts[1];

        // Thêm câu chào "Mời bệnh nhân" và "sinh ngày"
        var introText = "Mời bệnh nhân " + tenBn + ", sinh ngày " + ngaySinh + ". ";

        // Tạo đoạn văn bản để đọc
        var textToRead = introText;

        // Tạo đối tượng SpeechSynthesisUtterance và đọc văn bản
        var utterance = new SpeechSynthesisUtterance(textToRead);
        utterance.lang = "vi-VN"; // Thiết lập ngôn ngữ là tiếng Việt
        window.speechSynthesis.speak(utterance);
    }

    function speakText2(benhnhanId2) {
        // Thay đổi ID của phần tử cần đọc
        var elementId2 = "myText2_" + benhnhanId2;

        // Lấy nội dung từ phần tử cụ thể
        var mainText2 = document.getElementById(elementId2).textContent;

        // Tách nội dung thành tên bệnh nhân và ngày sinh
        var texts2 = mainText2.split(" - ");

        // Lấy tên bệnh nhân và ngày sinh
        var tenBn2 = texts2[0];
        var ngaySinh2 = texts2[1];

        // Thêm câu chào "Mời bệnh nhân" và "sinh ngày"
        var introText2 = "Mời bệnh nhân " + tenBn2 + ", sinh ngày " + ngaySinh2 + ". ";

        // Tạo đoạn văn bản để đọc
        var textToRead2 = introText2;

        // Tạo đối tượng SpeechSynthesisUtterance và đọc văn bản
        var utterance2 = new SpeechSynthesisUtterance(textToRead2);
        utterance2.lang = "vi-VN"; // Thiết lập ngôn ngữ là tiếng Việt
        window.speechSynthesis.speak(utterance2);
    }

   




