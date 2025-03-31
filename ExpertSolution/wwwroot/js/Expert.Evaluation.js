
Expert.Evaluation =
{
    evalCode: "",
    evalNumber: 0,
    Save: function () {
        $('#submitBtn').prop('disabled', true);
        var valid = $('form')[0].checkValidity();
        console.log(valid);
        if (valid) {
            var object = new Object();
            object.ContactFullName = $("#txtFirstName").val();// + " " + $("#txtLastName").val();
            //object.LastName = $("#txtLastName").val();
            //object.Position = $("#txtPosition").val();
            // object.OrganizationName = $("#txtOrganizationName").val();
            object.GUID = $("#hiddenUserGUID").val();
            var msg = "تم إنشاء حساب المٌقيم بنجاح";
            if (object.GUID != '') {
                msg = "تم تعديل حساب المٌقيم بنجاح";
            }
            object.CountryId = Expert.Helper.GetControlSelectedId("selectCountryId");
            object.EvaluatorGroupId = Expert.Helper.GetControlSelectedId("selectEvaluatorGroupId");
            object.OrganizationPhone = $("#txtPhone").val();
            object.Email = $("#txtEmail").val();
            object.Password = $("#txtPassword").val();
            console.log(object);
            Expert.Ajax.Post('/Evaluation/AccountSave', object, function (data) {
                if (data != null && data.Valid && data.Id > 0) {
                    console.log(data);
                    swal({
                        title: 'إنشاء حساب',
                        text: msg,
                        showCancelButton: false,
                        showConfirmButton: true,
                        confirmButtonText: 'Ok',
                        dangerMode: false,
                    }, function () {
                        window.location = '/Evaluation/List';
                        //location.reload();
                    });
                }
                else {
                    $('#submitBtn').removeAttr("disabled");
                    swal('خطأ', 'البريد الإلكترونى مستخدم من قبل', 'error');
                }
            });
        }
        else {
            $('#submitBtn').removeAttr("disabled");
        }
    },
    ShowActionModal: function (code, number, pointDegree, evaluator) {
        //$('form#formEvaluation').resetValidation();
        $('#txtDegree').prop("disabled", false);
        $('#selectDegreeReason').prop("disabled", false);
        $('#txtDegreeNotes').prop("disabled", false);
        $('.invalid-feedback').hide();
        $('#lblDegreeMessage').text('يجب إضافة درجة التقييم بين 0 و ' + pointDegree);
        $('#lblDegreeMessageinfo').text('الدرجة - الوزن المخصص للمعيار من 0 -  ' + pointDegree);
        $("#txtDegree").attr({
            "max": pointDegree,
            "min": 0
        });

        Expert.Evaluation.evalCode = code;
        Expert.Evaluation.evalNumber = number;
        $('#EvalActionModal').modal('show');

        $("#txtDegree").val("");
        $("#txtDegreeNotes").val("");
        $('#selectDegreeReason').val(0);

        var object = new Object();
        object.ApplicationFormGUID = $("#hiddenUserGUID").val();
        object.Code = Expert.Evaluation.evalCode;
        object.Number = Expert.Evaluation.evalNumber;
        object.Evaluator = evaluator;
        Expert.Ajax.Post('/Evaluation/GetEvaluationDegree', object, function (data) {
            console.log(data);
            if (data != null && data.Valid == true && data.result != null && data.result.id > 0) {
                $("#txtDegree").val(data.result.degree);
                $("#txtDegreeNotes").val(data.result.degreeNotes);
                $('#selectDegreeReason').val(data.result.degreeReason);

            }
        });
    },
    SaveEvaluation: function () {
        $('#btnSaveEvaluation').prop('disabled', true);
        valid = $('form#formEvaluation')[0].checkValidity();
        console.log(valid);
        if (valid) {
            var object = new Object();
            object.Degree = $("#txtDegree").val();// + " " + $("#txtLastName").val();
            object.ApplicationFormGUID = $("#hiddenUserGUID").val();
            object.DegreeReason = Expert.Helper.GetControlSelectedId("selectDegreeReason");
            object.DegreeNotes = $("#txtDegreeNotes").val();
            object.Code = Expert.Evaluation.evalCode;
            object.Number = Expert.Evaluation.evalNumber;
            console.log(object);
            Expert.Ajax.Post('/Evaluation/SaveEvaluationDegree', object, function (data) {
                $('#btnSaveEvaluation').removeAttr("disabled");
                if (data == null || data.Valid == false) {

                    swal({
                        title: 'التقييم',
                        text: 'لم يتم الحفظ',
                        showCancelButton: false,
                        showConfirmButton: true,
                        confirmButtonText: 'Ok',
                        dangerMode: false,
                    }, function () {
                        // window.location = '/Evaluation/List';
                        //location.reload();
                    });

                }
                else {
                    $('#EvalActionModal').modal('hide');
                    $("#btn" + Expert.Evaluation.evalCode + Expert.Evaluation.evalNumber + "").html(object.Degree);
                }
            });
        }
        else {
            $('.invalid-feedback').show();
            $('#btnSaveEvaluation').removeAttr("disabled");
        }
    },
    FinishEvaluation: function () {
        var object = new Object();
        object.ApplicationFormGUID = $("#hiddenUserGUID").val();
        swal({
            title: "إنهاء التقييم",
            text: "سيتم إنهاء التقييم",
            icon: "question",
            showCancelButton: true,
            cancelButtonText: 'إلغاء',
            confirmButtonText: 'متأكد',
            customClass: { confirmButton: "btn btn-primary fs-md-4", cancelButton: "btn btn-secondary fs-md-4" },
            closeOnConfirm: false,
            closeOnCancel: false
        }, function (isConfirm) {
            if (isConfirm) {
                Expert.Ajax.Post('/Evaluation/FinishEvaluationDegree', object, function (data) {
                    if (data == null || data.Valid == false) {
                        swal({
                            title: 'التقييم',
                            text: 'يجب إنهاء التقييم لكل المعايير',
                            showCancelButton: false,
                            showConfirmButton: true,
                            confirmButtonText: 'Ok',
                            dangerMode: false,
                        }, function () {
                        });
                    }
                    else {
                        swal({
                            title: 'التقييم',
                            text: 'تم إنهاء التقييم',
                            showCancelButton: false,
                            showConfirmButton: true,
                            confirmButtonText: 'Ok',
                            dangerMode: false,
                        }, function () {
                            window.location = '/home';
                        });
                    }
                });

            } else {
                swal("إلغاء", "لم يتم إنهاء التقييم، يمكنك المتابعة ", "success");
            }
        });


      
       
    },


}