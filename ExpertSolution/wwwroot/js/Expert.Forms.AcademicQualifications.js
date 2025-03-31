$(document).ready(function () {
    FillAcademicQualifications();
});

function SaveAcademicQualifications() {
    var valid = false;
    var AttPdfError = false;
    valid = $('form#EvaluationSecond')[0].checkValidity();
    var fileInput = document.getElementById("AttachmentFile_AQ");
    var fileInputName = document.getElementById("AttachmentName_AQ");
    var hrefValue = $("#AttachmentName_AQ").attr("href");
    if (fileInput && fileInput.files.length > 0) {
        // If a file is uploaded, check if it's a PDF.
        var file = fileInput.files[0];
        if (file.type !== "application/pdf") {
            valid = false;
            fileInput.classList.add("is-invalid"); // Optionally add a CSS class for styling errors.
            swal('خطأ', 'يُسمح فقط بملفات PDF للمرفقات', 'error');
        }
    }
    else if (!hrefValue ||!fileInputName || hrefValue === "#") {
        // If no file is uploaded, check if AttachmentName_Main has a value.
        valid = false;
        swal('خطأ', 'يجب إرفاق ملف PDF أو إدخال اسم الملف', 'error');
    }

    if (valid) {
       var object = new Object();
        object.Id = $("#hiddenAQId").val();
        object.UserGUID = $("#hiddenUserGUID").val();
        object.Name = $("#txtNameAQ").val();
        console.log("Selected Country ID:", $("#selectCountryIdAQ").val());
        object.DegreeScience = Expert.Helper.GetControlSelectedId("selectDegreeScience");
        object.CountryId = Expert.Helper.GetControlSelectedId("selectCountryIdAQ");
        object.Place = $("#txtplaceAQ").val();
        object.Year = $("#txtYear").val();
        object.Degree = $("#txtDegree").val();
        object.Notes = $("#txtNotesAQ").val();
        object.Specialist = $("#txtSpecialist").val();
        
        console.log(object);
        Expert.Ajax.Post('ApplicationForm/SaveAcademicQualifications', object, function (data) {
            if (data != null && data.Valid) {
                Expert.Attachment.Save('AQ', data.id)
               
                FillAcademicQualifications();
                swal("حفظ", "تم الحفظ بنجاح ", "success");
                valid = true;
                ClearAcademicQualifications();
            }
            else {
                valid = false;
                swal('خطأ', 'حدث خطأ ما، يرجى المحاولة مرة أخرى', 'error');
            }
        });
    } else if (!valid && !AttPdfError) {
        // If no file is uploaded and no valid href is set, show an error.
        valid = false;
        swal('خطأ', 'يجب ادخال كل الحقول المطلوبة', 'error');
    }
    return valid;
}
function FillAcademicQualifications() {
    var object = new Object();
    object.UserGUID = $("#hiddenUserGUID").val();
    $("#tblAQ").empty();
    Expert.Ajax.Post('ApplicationForm/GetALLAcademicQualifications', object, function (data) {
        if (data != null && data.Valid) {
            $.each(data.result, function (e, entity) {
                var row = "<tr>";
                row += "<td  data-label=' : المؤهل'> <div class='d-flex align-items-center'><span class='text-lg text-secondary-light fw-semibold flex-grow-1'>" + entity.name + "</span></div></td > ";
                row += "<td  data-label=' : جامعه / معهد'>" + entity.place + "</td>";
                row += "<td  data-label=' : الدرجة العلمية'>" + entity.degreeName + "</td>";
                row += "<td  data-label=' : التخصص'>" + entity.specialist + "</td>";
                row += "<td  data-label='  : سنة'>" + entity.year + "</td>";
                row += "<td  data-label=' : الدرجة / التقدير'>" + entity.degree + "</td>";
                row += "<td data-label='  : الاعدادات'  align='center'><div class='d-flex align-items-center gap-10 justify-content-center'>" +
                    "<button type='button' onclick='EditAcademicQualifications(" + entity.id + ")' class='bg-success-focus text-success-600 bg-hover-success-200 fw-medium w-40-px h-40-px d-flex justify-content-center align-items-center rounded-circle'> " +
                    "<iconify-icon icon='lucide:edit' class='menu-icon'></iconify-icon></button>" +
                    "<button type='button' onclick='DeleteAcademicQualifications(" + entity.id + ")' class='remove-item-btn bg-danger-focus bg-hover-danger-200 text-danger-600 fw-medium w-40-px h-40-px d-flex justify-content-center align-items-center rounded-circle'>" +
                    "<iconify-icon icon='fluent:delete-24-regular' class='menu-icon'></iconify-icon></button>" +
                    "</div></td>";
                row += "</tr>";
                $("#tblAQ").append(row);
            });
        }
    });
}
function EditAcademicQualifications(id) {
    ClearAcademicQualifications()
    var object = new Object();
    object.Id = id;
    Expert.Ajax.Post('ApplicationForm/GetAcademicQualificationsById', object, function (data) {
        console.log(data);
        if (data != null && data.Valid) {
            $("#hiddenAQId").val(data.result.id);
            $("#txtNameAQ").val(data.result.name);
            $("#txtplaceAQ").val(data.result.place);
            $("#txtYear").val(data.result.year);
            $("#txtDegree").val(data.result.degree);
            $("#txtNotesAQ").val(data.result.notes);
            $('#selectDegreeScience').val(data.result.degreeScience);
            $('#selectCountryIdAQ').val(data.result.countryId);
            $('#AttachmentName_AQ').attr('href', data.result.filename)
            $('#AttachmentName_AQ').text(data.result.fileDisplayName);
            $('#txtSpecialist').val(data.result.specialist);

        }
    });

}
function DeleteAcademicQualifications(id) {

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
            Expert.Ajax.Post('ApplicationForm/DeleteAcademicQualifications', object, function (data) {
                console.log(data);
                if (data != null && data.Valid) {
                    FillAcademicQualifications();
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
/*function ClearAcademicQualifications() {
    $("#hiddenAQId").val(""); // Clear hidden ID field
    $("#txtNameAQ").val(""); // Clear name field
    $("#txtplaceAQ").val(""); // Clear place field
    $("#txtYear").val(""); // Reset year dropdown to the first option
    $("#txtSpecialist").val(""); // Clear specialist field
    $("#selectDegreeScience").val(""); // Reset degree science dropdown
    $("#selectCountryIdAQ").val(""); // Reset country dropdown
    $("#txtDegree").val(""); // Clear degree field
    $("#txtNotesAQ").val(""); // Clear notes field
    $('#AttachmentName_AQ').attr('href', '#'); // Clear attachment link
    $('#AttachmentName_AQ').text(''); // Clear attachment display name
    $('#AttachmentFile_AQ').val(''); // Clear file input

   
}*/

function ClearAcademicQualifications() {
    // Clear all input fields
    $("#hiddenAQId").val(""); // Clear hidden ID field
    $("#txtNameAQ").val(""); // Clear name field
    $("#txtplaceAQ").val(""); // Clear place field
    $("#txtYear").val(""); // Reset year dropdown to the first option
    $("#txtSpecialist").val(""); // Clear specialist field
    $("#selectDegreeScience").val(""); // Reset degree science dropdown
    $("#selectCountryIdAQ").val(""); // Reset country dropdown
    $("#txtDegree").val(""); // Clear degree field
    $("#txtNotesAQ").val(""); // Clear notes field
    $('#AttachmentName_AQ').attr('href', '#'); // Clear attachment link
    $('#AttachmentName_AQ').text(''); // Clear attachment display name
    $('#AttachmentFile_AQ').val(''); // Clear file input

    // Remove Bootstrap validation classes and reset form state
    $("#EvaluationSecond").removeClass("was-validated");
    $("#EvaluationSecond").find(".form-control").removeClass("is-invalid");
    $("#EvaluationSecond").find(".invalid-feedback").remove();
}