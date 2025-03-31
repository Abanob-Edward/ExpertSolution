$(document).ready(function () {
});

Expert.ApplicationManage =
{
    appGUID: "",
    appStatus: "",
    ShowActionModal: function (guid, status, groupId) {
        Expert.ApplicationManage.appGUID = guid;
        Expert.ApplicationManage.appStatus = status;
        $('#ActionModal').modal('show');
        $("#selectStatus").val(status);
        $("#selectEvaluatorGroupId").val(groupId);
    },
    ChangeStatus: function () {
        var object = new Object();
        object.UserGUID = Expert.ApplicationManage.appGUID;
        object.Status = Expert.Helper.GetControlSelectedId("selectStatus");;
        Expert.Ajax.Post('ApplicationForm/ApplicationFormChangeStatus', object, function (data) {
            if (data != null && data.Valid) {
                console.log(data);
                swal({
                    title: "حالة التسجيل",
                    text: "تم التعديل بنجاح",
                    showCancelButton: false,
                    showConfirmButton: true,
                    confirmButtonText: 'Ok',
                    dangerMode: false,
                }, function () {
                    location.reload();
                });
            }
            else {
                swal('خطأ', 'كلمة المرور القديمة غير صحيحة', 'error');
            }

        });
    },
    ChangeGroup: function () {
        var object = new Object();
        object.UserGUID = Expert.ApplicationManage.appGUID;
         object.EvaluatorGroupId = Expert.Helper.GetControlSelectedId("selectEvaluatorGroupId");;
         Expert.Ajax.Post('ApplicationForm/ChangeGroup', object, function (data) {
            if (data != null && data.Valid) {
                console.log(data);
                swal({
                    title: "مجموعة التقيم ",
                    text: "تم التعديل بنجاح",
                    showCancelButton: false,
                    showConfirmButton: true,
                    confirmButtonText: 'Ok',
                    dangerMode: false,
                }, function () {
                    location.reload();
                });
            }
            else {
                swal('خطأ', 'كلمة المرور القديمة غير صحيحة', 'error');
            }

        });
    }
}