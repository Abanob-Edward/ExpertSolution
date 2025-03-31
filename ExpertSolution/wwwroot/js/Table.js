
function SaveTableValue(sel, tableName, columName) {
    var tr = $(sel).closest('tr');
    //console.log(tr.data("id"));
    //77console.log(sel.value);
    var object = new Object();
    object.Id = tr.data("id");
    object.TableName = tableName;//"ApplicationFormSocialLinks";
    //object.UserGUID = $("#hiddenUserGUID").val();
    object.ColumName = columName;//"SocialCode";
    object.ColumValue = sel.value;
    console.log(object);
    Expert.Ajax.Post('ApplicationForm/UpdateTableField', object, function (data) {
        if (data != null && data.Valid) {
           
        }
        else {
            //swal('خطأ', 'حدث خطأ ما، يرجى المحاولة مرة أخرى', 'error');
        }
    });

}
