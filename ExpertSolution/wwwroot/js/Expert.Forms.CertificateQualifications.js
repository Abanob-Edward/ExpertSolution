$(document).ready(function () {
    FillCertificateQualifications();
});

function SaveCertificateQualifications() {
    var valid = false;
    var AttPdfError = false;
    valid = $('form#EvaluationFourth')[0].checkValidity();
    var fileInput = document.getElementById("AttachmentFile_CQ");
    var fileInputName = document.getElementById("AttachmentName_CQ");
    var hrefValue = $("#AttachmentName_CQ").attr("href");
    if (fileInput && fileInput.files.length > 0) {
        // If a file is uploaded, check if it's a PDF.
        var file = fileInput.files[0];
        if (file.type !== "application/pdf") {
            valid = false;
            AttPdfError = true;
            fileInput.classList.add("is-invalid"); // Optionally add a CSS class for styling errors.
            swal('خطأ', 'يُسمح فقط بملفات PDF للمرفقات', 'error');
        }
    } else if (!hrefValue || !fileInputName || hrefValue === "#") {
        // If no file is uploaded and no valid href is set, show an error.
        valid = false;
        AttPdfError = true;
        swal('خطأ', 'يجب إرفاق ملف PDF أو إدخال اسم الملف', 'error');
    }
    if (valid) {
        var object = new Object();
        object.Id = $("#hiddenCQId").val();
        object.UserGUID = $("#hiddenUserGUID").val();
        object.Name = $("#txtNameCQ").val();
        object.Place = $("#txtplaceCQ").val();
        object.Year = $("#txtYearCQ").val();
        object.Degree = $("#txtDegreeCQ").val();
        object.Notes = $("#txtNotesCQ").val();
        console.log(object);
        Expert.Ajax.Post('ApplicationForm/SaveCertificateQualifications', object, function (data) {
            if (data != null && data.Valid) {
                ClearCertificateQualifications();
                FillCertificateQualifications();
                Expert.Attachment.Save('CQ', data.id)
                swal("حفظ", "تم الحفظ بنجاح ", "success");
                valid = true;
            }
            else {
                valid = false;
                swal('خطأ', 'حدث خطأ ما، يرجى المحاولة مرة أخرى', 'error');
            }
        });
    }
    else if (!valid && !AttPdfError ) {
        // If no file is uploaded and no valid href is set, show an error.
        valid = false;
        swal('خطأ', 'يجب ادخال كل الحقول المطلوبة', 'error');
    }
    return valid;
}
function FillCertificateQualifications() {
    var object = new Object();
    object.UserGUID = $("#hiddenUserGUID").val();
    $("#tblCQ").empty();
    Expert.Ajax.Post('ApplicationForm/GetALLCertificateQualifications', object, function (data) {
        if (data != null && data.Valid) {
            $.each(data.result, function (e, entity) {
                var row = "<tr>";
                row += "<td> <div class='d-flex align-items-center'><span class='text-lg text-secondary-light fw-semibold flex-grow-1'>" + entity.name + "</span></div></td > ";
                row += "<td >" + entity.place + "</td>";
                row += "<td >" + entity.year + "</td>";
                row += "<td >" + entity.degree + "</td>";
                row += "<td  align='center'><div class='d-flex align-items-center gap-10 justify-content-center'>" +
                    "<button type='button' onclick='EditCertificateQualifications(" + entity.id + ")' class='bg-success-focus text-success-600 bg-hover-success-200 fw-medium w-40-px h-40-px d-flex justify-content-center align-items-center rounded-circle'> " +
                    "<iconify-icon icon='lucide:edit' class='menu-icon'></iconify-icon></button>" +
                    "<button type='button' onclick='DeleteCertificateQualifications(" + entity.id + ")' class='remove-item-btn bg-danger-focus bg-hover-danger-200 text-danger-600 fw-medium w-40-px h-40-px d-flex justify-content-center align-items-center rounded-circle'>" +
                    "<iconify-icon icon='fluent:delete-24-regular' class='menu-icon'></iconify-icon></button>" +
                    "</div></td>";
                row += "</tr>";
                $("#tblCQ").append(row);
            });
        }
    });
}
function EditCertificateQualifications(id) {
    ClearCertificateQualifications()
    var object = new Object();
    object.Id = id;
    Expert.Ajax.Post('ApplicationForm/GetCertificateQualificationsById', object, function (data) {
        if (data != null && data.Valid) {
            $("#hiddenCQId").val(data.result.id);
            $("#txtNameCQ").val(data.result.name);
            $("#txtplaceCQ").val(data.result.place);
            $("#txtYearCQ").val(data.result.year);
            $("#txtDegreeCQ").val(data.result.degree);
            $("#txtNotesCQ").val(data.result.notes);
            $('#AttachmentName_CQ').attr('href', data.result.filename)
            $('#AttachmentName_CQ').text(data.result.fileDisplayName);
        }
    });

}
function DeleteCertificateQualifications(id) {

    swal({
        title: "حذف",
        text: "سيتم حذف هذا العنصر",
        icon: "question",
        showCancelButton: true,
        cancelButtonText: 'إلغاء',
        confirmButtonText: 'متأكد',
        customClass: { confirmButton: "btn btn-primary fs-md-4", cancelButton: "btn btn-secondary fs-md-4" },
        closeOnConfirm: true,
        closeOnCancel: false
    }, function (isConfirm) {
        if (isConfirm) {
            var object = new Object();
            object.Id = id;
            Expert.Ajax.Post('ApplicationForm/DeleteCertificateQualifications', object, function (data) {
                console.log(data);
                if (data != null && data.Valid) {
                    FillCertificateQualifications();
                }
                else {
                    swal('خطأ', 'حدث خطأ ما، يرجى المحاولة مرة أخرى', 'error');
                }
            });

        } else {
            swal("إلغاء", "لم يتم الحذف، يمكنك المتابعة ", "success");
        }
    });

}
function ClearCertificateQualifications() {
    $("#hiddenCQId").val("");
    $("#txtNameCQ").val(" ");
    $("#txtplaceCQ").val(" ");
    $("#txtYearCQ").val(" ");
    $("#txtDegreeCQ").val(" ");
    $("#txtNotesCQ").val("");
}
