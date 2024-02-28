
    var defaultLength = 10;
    var defaultLength2 = 10;

    // Nếu đã lưu trữ số hàng hiển thị trên localstorage thì lấy giá trị đó
    if (localStorage.getItem("exampleLength")) {
        defaultLength = parseInt(localStorage.getItem("exampleLength"));
     }  if (localStorage.getItem("exampleLength2")) {
        defaultLength2 = parseInt(localStorage.getItem("exampleLength2"));
     }
    $(document).ready(function () {
        $('#example').DataTable({
            searching: false,
            info: false,
            ordering: false,  // Tắt sắp xếp
            pageLength: defaultLength,
            language: {
                url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/vi.json',
                emptyTable: 'Không có dữ liệu.',
                paginate: {
                    previous: 'Trước',
                    next: 'Sau'
                }
            }
        });
     });

    $(document).ready(function () {
        $('.example2').DataTable({
            // searching: false,
            destroy: true,
            /*info: false,*/
           /* ordering: false,  // Tắt sắp xếp*/
            pageLength: defaultLength2,
            language: {
                search: '',
                url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/vi.json',
                emptyTable: 'Không có dữ liệu.',
                paginate: {
                    previous: 'Trước',
                    next: 'Sau'
                }
            }
        });
        $('#example').on('length.dt', function (e, settings, len) {
            localStorage.setItem("exampleLength", len);
        }); $('.example2').on('length.dt', function (e, settings, len) {
            localStorage.setItem("exampleLength2", len);
        });
     });
    