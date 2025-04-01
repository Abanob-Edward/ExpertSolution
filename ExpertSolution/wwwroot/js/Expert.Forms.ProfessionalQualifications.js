$(document).ready(function () {
    FillProfessionalQualifications();
});

function SaveProfessionalqualifications() {
    var valid = false;
    var AttPdfError = false;
    valid = $('form#EvaluationThird')[0].checkValidity();
    var fileInput = document.getElementById("AttachmentFile_PQ");
    var fileInputName = document.getElementById("AttachmentName_PQ");
    var hrefValue = $("#AttachmentName_PQ").attr("href");

    if (fileInput && fileInput.files.length > 0) {
        // If a file is uploaded, check if it's a PDF.
        var file = fileInput.files[0];
        if (file.type !== "application/pdf") {
            valid = false;
            fileInput.classList.add("is-invalid"); // Optionally add a CSS class for styling errors.
            swal('خطأ', 'يُسمح فقط بملفات PDF للمرفقات', 'error');
        }
    } else if (!hrefValue || !fileInputName || hrefValue === "#") {
        // If no file is uploaded and no valid href is set, show an error.
        valid = false;
        swal('خطأ', 'يجب إرفاق ملف PDF أو إدخال اسم الملف', 'error');
    }

    if (valid) {
        var object = new Object();
        object.Id = $("#hiddenPQId").val();
        object.UserGUID = $("#hiddenUserGUID").val();
        object.Name = $("#txtName").val();
        object.Type = Expert.Helper.GetControlSelectedId("selecttype");
        object.TrainingType = Expert.Helper.GetControlSelectedId("selectTrainingType");
        object.Place = $("#txtplace").val();
        object.Organization = $("#txtOrg").val();
        object.Notes = $("#txtNotes").val();

        Expert.Ajax.Post('ApplicationForm/SaveProfessionalqualifications', object, function (data) {
            if (data != null && data.Valid) {
                Expert.Attachment.Save('PQ', data.id); // Save attachment              
                ClearProfessionalqualifications(); // Clear the form
                FillProfessionalQualifications();
                swal("حفظ", "تم الحفظ بنجاح ", "success");
                valid = true;
            } else {
                valid = false;
                swal('خطأ', 'حدث خطأ ما، يرجى المحاولة مرة أخرى', 'error');
            }
        });
    } else if (!valid && !AttPdfError) {
        valid = false;
        swal('خطأ', 'يجب ادخال كل الحقول المطلوبه', 'error');
    }
    return valid;
}
function FillProfessionalQualifications() {
    var object = new Object();
    object.UserGUID = $("#hiddenUserGUID").val();
    $("#tblPQ").empty();
    Expert.Ajax.Post('ApplicationForm/GetALLProfessionalQualifications', object, function (data) {
        if (data != null && data.Valid) {
            $.each(data.result, function (e, entity) {
                var row = "<tr>";
                var trainttype = "";
                if (entity.type == "1")
                    trainttype = "عن بعد";
                else if (entity.type == "2")
                    trainttype = "حضوريا";
                else if (entity.type == "3")
                    trainttype = "عن بعد / حضوريا";
                row += "<td width='15 % ' data-label=' : النوع'> " + entity.trainingTypeName + "</td > ";
                row += "<td width='25 % ' data-label=' : الاسم'> " + entity.name + "</td > ";
                row += "<td width='10 % ' data-label=' : مكان'>" + entity.place + "</td>";
                row += "<td width='15 % ' data-label=' : نوع التقديم'>" + trainttype + "</td>";
                row += "<td width='20 % ' data-label=' : المؤسسة المقدم لها الخدمة'>" + entity.organization + "</td>";
                row += "<td width='10 % ' data-label=' : الاعدادات' align='center'><div class='d-flex align-items-center gap-10 justify-content-center'>" +
                    "<button type='button' onclick='EditProfessionalqualifications(" + entity.id + ")' class='bg-success-focus text-success-600 bg-hover-success-200 fw-medium w-40-px h-40-px d-flex justify-content-center align-items-center rounded-circle'> " +
                    "<iconify-icon icon='lucide:edit' class='menu-icon'></iconify-icon></button>" +
                    "<button type='button' onclick='DeleteProfessionalqualifications(" + entity.id + ")' class='remove-item-btn bg-danger-focus bg-hover-danger-200 text-danger-600 fw-medium w-40-px h-40-px d-flex justify-content-center align-items-center rounded-circle'>" +
                    "<iconify-icon icon='fluent:delete-24-regular' class='menu-icon'></iconify-icon></button>" +
                    "</div></td>";
                row += "</tr>";
                $("#tblPQ").append(row);
            });
        }
    });
}
function EditProfessionalqualifications(id) {
    ClearProfessionalqualifications()
    var object = new Object();
    object.Id = id;
    Expert.Ajax.Post('ApplicationForm/GetProfessionalqualificationsById', object, function (data) {
        if (data != null && data.Valid) {
            console.log(data.result);
            $("#hiddenPQId").val(data.result.id);
            $("#txtName").val(data.result.name);
            $("#txtplace").val(data.result.place);
            $("#txtOrg").val(data.result.organization);
            $("#txtNotes").val(data.result.notes);
            $('#selectTrainingType').val(data.result.trainingType);
            $('#selecttype').val(data.result.type);
            $('#AttachmentName_PQ').attr('href', data.result.filename)
            $('#AttachmentName_PQ').text(data.result.fileDisplayName);
        }
    });

}
function DeleteProfessionalqualifications(id) {

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
            Expert.Ajax.Post('ApplicationForm/DeleteProfessionalqualifications', object, function (data) {
                console.log(data);
                if (data != null && data.Valid) {
                    FillProfessionalQualifications();
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
function ClearProfessionalqualifications() {
    $("#hiddenPQId").val(""); // Clear hidden input
    $("#txtName").val(""); // Clear text input
    $("#txtplace").val(""); // Clear text input
    $("#txtOrg").val(""); // Clear text input
    $("#txtNotes").val(""); // Clear text input
    $("#selecttype").val(""); // Reset dropdown
    $("#selectTrainingType").val(""); // Reset dropdown

    // Clear the <a> element
    $('#AttachmentName_PQ').attr('href', '#'); // Reset href to #
    $('#AttachmentName_PQ').text(''); // Clear text content
    $('#AttachmentFile_PQ').val(''); // Clear file input
}

