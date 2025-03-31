
Expert.Account =
{
  
    Save: function () {
        $('#submitBtn').prop('disabled', true);
        var valid = $('form')[0].checkValidity();
        if (grecaptcha.getResponse()) {
            $('#recaptcha').hide();
            console.log(valid);
            if (valid && $("#txtPassword").val() == $("#txtRePassword").val()) {
                var object = new Object();
                object.ContactFullName = $("#txtFirstName").val() + " " + $("#txtLastName").val();
                //object.LastName = $("#txtLastName").val();
                //object.Position = $("#txtPosition").val();
                //object.OrganizationName = $("#txtOrganizationName").val();
                //object.CountryId = Expert.Helper.GetControlSelectedId("selectCountryId");
                object.Phone = $("#phone").val();
                object.Email = $("#txtEmail").val();
                object.Password = $("#txtPassword").val();
                console.log(object);
                Expert.Ajax.Post('AccountSave', object, function (data) {
                    if (data != null && data.Valid && data.Id > 0) {
                        console.log(data);
                        swal({
                            title: 'تم إنشاء الحساب بنجاح',
                            text: 'يرجى الدخول فى النظام، للإستكمال البيانات المطلوبة',
                            showCancelButton: false,
                            showConfirmButton: true,
                            confirmButtonText: 'Ok',
                            dangerMode: false,
                        }, function () {
                            window.location = '/account/login';
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
        }
        else {
            $('#submitBtn').removeAttr("disabled");

            $('#recaptcha').show();
        }

    },
    Login: function () {
        //$('form')[0].valid();
        //swal('خطأ', 'يرجى التأكد من اسم المستخدم وكلمة المرور', 'error');
        //$('form')[0].reportValidity();
        var valid = $('form')[0].checkValidity();
        if (valid) {
            var object = new Object();
            object.Email = $("#txtEmail").val();
            object.Password = $("#txtPassword").val();
            console.log(object);
            Expert.Ajax.Post('LoginByUserNamePassword', object, function (data) {
                console.log(data);
                if (data != null && data.Valid) {
                    console.log('Valid');
                    window.location = '/Home';
                }
                else {
                    console.log('Not Valid');
                    swal('خطأ', 'يرجى التأكد من اسم المستخدم وكلمة المرور', 'error');
                }
            });
        }
    },
    UpdatePassword: function () {

        var valid = $('form')[0].checkValidity();

        if (valid && $("#txtPassword").val() == $("#txtRePassword").val()) {
            var object = new Object();
            object.OldPassword = $("#txtOldPassword").val();
            object.Password = $("#txtPassword").val();
            console.log(object);
            var parameters = { OldPassword: object.OldPassword, NewPassword: object.Password };
            Expert.Ajax.Post('UpdatePassword', parameters, function (data) {
                if (data != null && data.Valid) {
                    console.log(data);
                    swal({
                        title: 'تم تغيير كلمة المرور بنجاح',
                        text: '',
                        showCancelButton: false,
                        showConfirmButton: true,
                        confirmButtonText: 'Ok',
                        dangerMode: false,
                    }, function () {
                        window.location = '/home';
                    });
                }
                else {
                    swal('خطأ', 'كلمة المرور القديمة غير صحيحة', 'error');
                }
            });
        }


    },
    ForgetPassword: function () {
        var valid = $('form')[0].checkValidity();
        if (valid) {


            var object = new Object();
            object.Email = $("#txtEmail").val();
            console.log(object);
            Expert.Ajax.Post('ReterivePasswordByEmail', object, function (data) {
                if (data != null && data.Valid) {
                    //window.location = '/ApplicationForm';
                    swal('تم', 'يرجى مراجعة البريد الإلكترونى لإستعادة كلمة المرور', 'success');

                }
                else {
                    swal('خطأ', 'هذا البريد الإلكترونى غير مسجل من قبل', 'error');
                }
            });
        }
    },
    UpdatePasswordByActivateKey: function () {

        var valid = $('form')[0].checkValidity();

        if (valid && $("#txtPassword").val() == $("#txtRePassword").val()) {
            var object = new Object();
            object.ActivateKey = $("#txtPassword").attr("date-ActivateKey");;
            object.Password = $("#txtPassword").val();
            var parameters = { ActivateKey: object.ActivateKey, NewPassword: object.Password };
            Expert.Ajax.Post('../UpdatePasswordByActivateKey', parameters, function (data) {
                if (data != null && data.Valid) {
                    swal({
                        title: 'تم تغيير كلمة المرور بنجاح',
                        text: '',
                        showCancelButton: false,
                        showConfirmButton: true,
                        confirmButtonText: 'Ok',
                        dangerMode: false,
                    }, function () {
                        window.location = '/account/login';
                    });
                }
                else {
                    swal('خطأ', 'كلمة المرور القديمة غير صحيحة', 'error');
                }
            });
        }


    },
    
}