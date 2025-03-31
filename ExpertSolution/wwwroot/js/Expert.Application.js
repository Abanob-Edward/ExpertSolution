$(document).ready(function () {
    //document.getElementById("txtEstablishmentDate").max = "2021-01-01";
    //document.getElementById("txtEstablishmentDate").min = "1900-01-01";
    $('form').keypress(function (e) {
        if (e.which == 13) { // Checks for the enter key
            e.preventDefault(); // Stops IE from triggering the button to be clicked
        }
    });
    Expert.Application.progressBar = document.getElementById("progress-bar");
    Expert.Application.steps = document.querySelectorAll(".step");
    Expert.Application.active = 1;
    Expert.Application.ApplicationFormDisabled = $("#hiddenUserGUID").attr("data-status");
    if (Expert.Application.ApplicationFormDisabled.toLowerCase() == "true") {
        $('.form-control').attr('disabled', 'disabled');
        $('.form-check-input').attr('disabled', 'disabled');
        $('.form-select').attr('disabled', 'disabled');

    }
});
document.addEventListener("DOMContentLoaded", function () {
    var yearDropdown = document.getElementById("txtYear");

    // Set the range of years (e.g., from 1900 to the current year)
    var currentYear = new Date().getFullYear();
    var startYear = 1900;

    // Populate the dropdown with years
    for (var year = currentYear; year >= startYear; year--) {
        var option = document.createElement("option");
        option.value = year;
        option.text = year;
        yearDropdown.appendChild(option);
    }
});
Expert.Application =
{
    progressBar: "",
    steps: "",
    active: 0,
    maxlimit: 500,
    ApplicationFormDisabled: "",
    NavigationClicked: function (number) {
        //$("#" + currentDiv + "").hide();
        //$("#" + otherDiv + "").show();
        $('.divapplicationform').hide(); // Shows
        $('[name=div' + number + ']').show();
        //$('.float_form').hide(); // hides
        $(window).scrollTop(0);
        Expert.Application.active = number;
        Expert.Application.updateProgress();
    },
    Next: function (currentDiv, otherDiv)
    {
        var valid = true;
        if (currentDiv == 'divMainInformation')
            valid = Expert.Application.SaveMainInformation();
        else if (currentDiv == 'divEvaluationFirst')
            valid = Expert.Application.checkDiv2Validation();
        else if (currentDiv == 'divEvaluationSecond')
            valid = Expert.Application.checkDiv3Validation();
        else if (currentDiv == 'divEvaluationThird')
            valid = Expert.Application.checkDiv4Validation();
        else if (currentDiv == 'divEvaluationFourth')
            valid = Expert.Application.checkDiv5Validation();
        //else
        //    valid = Expert.Application.SaveEvaluation(currentDiv.replace('divEvaluation', ''));
        //valid = Expert.Application.Save(currentDiv)
        if (valid) {
            $("#" + currentDiv + "").hide();
            $("#" + otherDiv + "").show();
            $(window).scrollTop(0);
            Expert.Application.active++;
            Expert.Application.updateProgress();
        }
    },
    Previous: function (currentDiv, otherDiv) {
        $("#" + currentDiv + "").hide();
        $("#" + otherDiv + "").show();
        $(window).scrollTop(0);
        Expert.Application.active--;
        Expert.Application.updateProgress();
    },
    SaveClose: function (currentDiv) {
        var valid = true;
        if (currentDiv == 'divMainInformation')
            valid = Expert.Application.SaveMainInformation();
        if (valid) {
            swal({
                title: "حفظ وإغلاق التسجيل",
                text: "سيتم غلق التسجيل، والخروج من النظام، يمكنك المتابعة بعد ذلك",
                icon: "question",
                showCancelButton: true,
                cancelButtonText: 'إلغاء',
                confirmButtonText: 'متأكد',
                customClass: { confirmButton: "btn btn-primary fs-md-4", cancelButton: "btn btn-secondary fs-md-4" },
                closeOnConfirm: false,
                closeOnCancel: true
            }, function (isConfirm) {
                if (isConfirm) {
                    window.location = "/Account/SignOut";

                } else {

                }
            });
        };

    },
    updateProgress: function () {
        Expert.Application.steps.forEach((step, i) => {
            if (i < Expert.Application.active) {
                step.classList.add("active");
            } else {
                step.classList.remove("active");
            }
        });
        Expert.Application.progressBar.style.width = ((Expert.Application.active - 1) / (Expert.Application.steps.length - 1)) * 100 + "%";
    },
    textCounter: function (field, remainingC) {
        if (field.value.length > Expert.Application.maxlimit) {
            field.value = field.value.substring(0, Expert.Application.maxlimit);
            return false;
        } else {
            $('#' + remainingC).html("متبقى : " + (Expert.Application.maxlimit - field.value.length) + " حرف");
        }
        if (field.value == '') {
            var answervalue = $("input:radio[name ='" + field.id.replace('Eval_', 'radio_') + "']:checked").val();
            if (answervalue == 1) {
                $("#" + field.id).prop('required', true);
            }
            else {
                $("#" + field.id).prop('required', false);
            }
        }

    },
    Save: function (currentDiv) {
        var valid = false;
        if (Expert.Application.ApplicationFormDisabled.toLowerCase() != "true") {
            if (currentDiv == 'divMainInformation')
                valid = Expert.Application.SaveMainInformation();
            //else if (currentDiv == 'divEvaluationSecond')
            //    valid = Expert.Application.SaveMainInformation();
            //else
            //    valid = Expert.Application.SaveEvaluation(currentDiv.replace('divEvaluation', ''));
            //if (!valid) {
            //    swal('خطأ', 'يجب ملئ البيانات غير المدخلة', 'error');
            //}
        }
        else {
            valid = true;
        }
        console.log(valid);
        return valid;
    },
    SaveMainInformation: function () {
        // Check the overall form validity first.
        var valid = $('form#mainform')[0].checkValidity();

        // Additional check: Validate the attachment file type and ensure at least one of the fields has a value.
        var fileInput = document.getElementById("AttachmentFile_Main");
        var fileInputName = document.getElementById("AttachmentName_Main");

        if (fileInput && fileInput.files.length > 0) {
            // If a file is uploaded, check if it's a PDF.
            var file = fileInput.files[0];
            if (file.type !== "application/pdf") {
                valid = false;
                fileInput.classList.add("is-invalid"); // Optionally add a CSS class for styling errors.
                swal('خطأ', 'يُسمح فقط بملفات PDF للمرفقات', 'error');
            }
        } else if (!fileInputName || !fileInputName.href) {
            // If no file is uploaded, check if AttachmentName_Main has a value.
            valid = false;
            swal('خطأ', 'يجب إرفاق ملف PDF أو إدخال اسم الملف', 'error');
        }

        // Log any invalid fields for debugging.
        var invalidFields = $("#mainform :invalid");
        invalidFields.each(function () {
            var fieldName = $(this).attr("Placeholder");
            var errorMessage = `يجب ادخال ${fieldName}`;
         

            if (fieldName!= null) {
                // Display the error message using SweetAlert
                swal({
                    title: 'خطأ',
                    text: errorMessage,
                    icon: 'error',
                    button: 'حسنا',
                    className: 'swal2-error-style' // Apply the custom CSS class
                });
            }

            console.log("Field name: " + fieldName);
            console.log("Validation message: " + errorMessage);
        });
        // Function to validate the Social-table
       
            const rowCount = $('#Social-table tbody tr').length;
            if (rowCount === 0) {
                swal({
                    title: 'خطأ',
                    text: 'يجب إضافة وسيلة تواصل واحدة على الأقل',
                    icon: 'error',
                    button: 'حسنا'
                });
                valid = false;
            }
        $('#Social-table tbody tr').each(function () {
            const select = $(this).find('select.form-select');
            const input = $(this).find('input.invoive-form-control');

            // Check if the select has a valid value
            if (select.val() === "0") {
               
                swal({
                    title: 'خطأ',
                    text: 'يجب اختيار وسيلة تواصل',
                    icon: 'error',
                    button: 'حسنا'
                });
                valid = false;
            }else if (input.val() === "") {
                valid = false;
                swal({
                    title: 'خطأ',
                    text: 'يجب إدخال الرابط',
                    icon: 'error',
                    button: 'حسنا'
                });
                return false; // Exit the loop early
            }
        });
         
        if (valid) {
            var object = {};
            object.GUID = $("#hiddenUserGUID").val();
            object.ContactFullName = $("#txtContactFullName").val();
            object.Phone = $("#phone").val();
            object.BirthDate = $("#txtBirthDate").val();
            object.NationalId = $("#txtNationalId").val();
            object.JobTitle = $("#txtJobTitle").val();
            object.NationalityId = Expert.Helper.GetControlSelectedId("selectNationalityId");
            object.CountryId = Expert.Helper.GetControlSelectedId("selectCountryId");
            object.WorkOn = $("#txtWorkOn").val();
            Expert.Ajax.Post('ApplicationForm/SaveMainInformation', object, function (data) {
                if (data != null && data.Valid) {
                    Expert.Attachment.Save('Main', 0);
                    valid = true;
                } else {
                    valid = false;
                    swal('خطأ', 'حدث خطأ ما، يرجى المحاولة مرة أخرى', 'error');
                }
            });
        }
        return valid;
    },
    SaveEvaluation: function (code) {
        var questionlength = 0;
        if (code == "First" || code == "Fifth" || code == "Third") {
            questionlength = 5;
        }
        else if (code == "Second" || code == "Fourth") {
            questionlength = 4;
        }
        else if (code == "Seven" || code == "Six") {
            questionlength = 7;
        }
        var valid = false;
        valid = $('form#Evaluation' + code)[0].checkValidity();
        if (valid) {
            var Evalarray = [];
            for (var i = 1; i <= questionlength; i++) {
                var cntrlId = code + i.toString() + "1";
                var answervalue = $("input:radio[name ='radio_" + cntrlId + "']:checked").val();
                var object = new Object();
                object.UserGUID = $("#hiddenUserGUID").val();
                object.Code = code;
                object.Number = i;
                object.Answer = answervalue != undefined ? answervalue : 2;
                object.AnswerFirst = $("#Eval_" + code + i.toString() + "1").val();
                object.AnswerSecond = $("#Eval_" + code + i.toString() + "2").val();
                object.AnswerThird = $("#Eval_" + code + i.toString() + "3").val();
                if (object.Description != '') {
                    Evalarray.push(object);
                }

            }
            var postData = { Evalarray: Evalarray };
            Expert.Ajax.Post('ApplicationForm/SaveApplicationEvaluation', postData, function (data) {
                if (data != null && data.Valid) {
                    valid = true;
                }
                else {
                    valid = false;
                    swal('خطأ', 'حدث خطأ ما، يرجى المحاولة مرة أخرى', 'error');
                }
            });
        }
        return valid;
    },
    checkAnswer: function (radio, code, no) {
        var value = radio.value;
        var cntrl = $("#Eval_" + code + no);
        if (value == 1) {
            cntrl.prop('required', true);
        }
        else {
            cntrl.prop('required', false);
        }
        if (code + no == "First11") {
            var cntrl2 = $("#Eval_" + code + '12');
            var cntrl3 = $("#Eval_" + code + '13');
            if (value == 1) {
                cntrl2.prop('required', true);
                cntrl3.prop('required', true);
            }
            else {
                cntrl2.prop('required', false);
                cntrl3.prop('required', false);
            }
        }
    },
    Finish: function (currentDiv) {

        if (!Expert.Application.SaveMainInformation()) {
            swal('خطأ', 'يجب ملئ البيانات الرئيسية الغير مدخلة', 'error');
            return;
        }
        if (!Expert.Application.checkDiv2Validation()) {
            swal('خطأ', 'يجب إضافة اللغات', 'error');
            return;
        }
        //else if ($("#tblComputerSkills tr")) {
        //    swal('خطأ', 'يجب إضافة مهارات استخدام الحاسب الالى', 'error');
        //    return;
        //}
        else if (!Expert.Application.checkDiv3Validation()) {
            swal('خطأ', 'يجب إضافة المؤهلات الأكاديمية', 'error');
            return;
        }
        else if (!Expert.Application.checkDiv4Validation()) {
              swal('خطأ', 'يجب علي الاقل ادخال خبراه عملية واحده', 'error');
            return;
        }
        else if (!Expert.Application.checkDiv5Validation()) {
            swal('خطأ', ' يجب علي الاقل ادخال شهادة مهنية واحدة', 'error');
            return;
        } else if (!Expert.Application.checkDiv6Validation()) {
            swal('خطأ', 'يجب الاجابه علي كل اساله هل يمكنك :', 'error');
            return;
        }
        
       /* else if ($("#tblAQ tr").length == 0) {
            swal('خطأ', 'يجب إضافة المؤهلات الأكاديمية', 'error');
            return;
        }
        else if ($("#tblCQ tr").length == 0) {
            swal('خطأ', 'يجب إضافة الشهادات المهنية', 'error');
            return;
        }*/
        swal({
            title: "إنهاء التسجيل",
            text: "سيتم إنهاء التسجيل، ولا يمكن التعديل بعد ذلك",
            icon: "question",
            showCancelButton: true,
            cancelButtonText: 'إلغاء',
            confirmButtonText: 'متأكد',
            customClass: { confirmButton: "btn btn-primary fs-md-4", cancelButton: "btn btn-secondary fs-md-4" },
            closeOnConfirm: false,
            closeOnCancel: false
        }, function (isConfirm) {
            if (isConfirm) {
                var object = new Object();
                object.UserGUID = $("#hiddenUserGUID").val();
                object.Status = "Finished";
                Expert.Ajax.Post('ApplicationForm/ApplicationFormChangeStatus', object, function (data) {
                    if (data != null && data.Valid) {
                        valid = true;
                        swal({
                            title: "إنهاء التسجيل",
                            text: "تم إرسال الاستمارة للتقيم",
                            icon: "success",
                            showCancelButton: false,
                            cancelButtonText: 'إلغاء',
                            confirmButtonText: 'OK',
                            customClass: { confirmButton: "btn btn-primary fs-md-4", cancelButton: "btn btn-secondary fs-md-4" },
                            closeOnConfirm: true,
                            closeOnCancel: true
                        }, function (isConfirm) {
                            if (isConfirm) {
                                //window.location = "/Account/SignOut";
                                window.location = "https://www.arado.org/paymentgateway/Payment/PostPayment/" + data.paymentId;
                            }
                        });;
                    }
                    else {
                        valid = false;
                        swal('خطأ', 'حدث خطأ ما، يرجى المحاولة مرة أخرى', 'error');
                    }
                });

            } else {
                swal("إلغاء", "لم يتم إنهاء التسجيل، يمكنك المتابعة ", "success");
            }
        });



    },
    checkDiv2Validation: function () {
        // Get the tbody element by its ID
        const tbody = document.getElementById('tbllanguages');
        ;
        // Check if the tbody has any rows
        if (tbody && tbody.getElementsByTagName('tr').length > 0) {
            return true;
        } else {
            swal({
                title: 'خطأ',
                text: ' يجب علي الاقل ادخال مهارة شخصيه او لغه',
                icon: 'error',
                button: 'حسنا',
                className: 'swal2-error-style' // Apply the custom CSS class
            });
            return false;
        }
    },
    checkDiv3Validation: function () {
        // Get the tbody element by its ID
        const tbody = document.getElementById('tblAQ');
            ;
        // Check if the tbody has any rows
        if (tbody && tbody.getElementsByTagName('tr').length > 0) {
            return true;
        } else {
            swal({
                title: 'خطأ',
                text: ' يجب علي الاقل ادخال مؤهل واحد',
                icon: 'error',
                button: 'حسنا',
                className: 'swal2-error-style' // Apply the custom CSS class
            });
            return false;
        }
    },
    checkDiv4Validation: function () {
        // Get the tbody element by its ID
        const tbody = document.getElementById('tblPQ');
        ;
        // Check if the tbody has any rows
        if (tbody && tbody.getElementsByTagName('tr').length > 0) {
            return true;
        } else {
            swal({
                title: 'خطأ',
                text: ' يجب علي الاقل ادخال خبراه عملية واحده',
                icon: 'error',
                button: 'حسنا',
                className: 'swal2-error-style' // Apply the custom CSS class
            });
            return false;
        }
    } ,
    checkDiv5Validation: function () {
        // Get the tbody element by its ID
        const tbody = document.getElementById('tblCQ');
        ;
        // Check if the tbody has any rows
        if (tbody && tbody.getElementsByTagName('tr').length > 0) {
            return true;
        } else {
            swal({
                title: 'خطأ',
                text: ' يجب علي الاقل ادخال شهادة مهنية واحدة',
                icon: 'error',
                button: 'حسنا',
                className: 'swal2-error-style' // Apply the custom CSS class
            });
            return false;
        }
    },
    checkDiv6Validation : function () {
    const questions = document.querySelectorAll('[id^="question_"]');
    console.log("Total questions found:", questions.length);

    // Create a set to track answered questions
    const answeredQuestions = new Set();

    // Loop through all answer inputs
    questions.forEach(input => {
        if (input.checked) {
            const questionId = input.name.split('_')[1]; // Extract question ID
            answeredQuestions.add(questionId);
        }
    });

    // Check if all questions have been answered
    const totalQuestions = new Set([...questions].map(input => input.name.split('_')[1])); // Get unique question IDs
    if (answeredQuestions.size !== totalQuestions.size) {
        swal({
            title: "⚠️ تنبيه!",
            text: "يجب الإجابة على جميع الأسئلة قبل الإرسال.",
            icon: "error",
            button: "موافق"
        });
        return false; // Validation failed
    }

    return true; // Validation passed
}
    
}

