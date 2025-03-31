Expert.Attachment =
{

    createXMLHttp: function () {
        if (typeof XMLHttpRequest != "undefined") {
            return new XMLHttpRequest();
        } else if (window.ActiveXObject) {
            var aVersions = ["MSXML2.XMLHttp.5.0", "MSXML2.XMLHttp.4.0", "MSXML2.XMLHttp.3.0", "MSXML2.XMLHttp", "Microsoft.XMLHttp"];
            for (var i = 0; i < aVersions.length; i++) {
                try {
                    var oXmlHttp = new ActiveXObject(aVersions[i]);
                    return oXmlHttp;
                } catch (oError) {
                    console.log(oError);
                }
            }
        }

    },
    AttachChange: function (Code) {
        var FileAttach = document.getElementById('AttachmentFile_' + Code);
        $('#AttachmentName_' + Code).text(FileAttach.files.item(0).name);
    },
    Save: function (Code,id) {
        //$("#divloader").show();
        var fileInput = document.getElementById('AttachmentFile_' + Code);
        if (fileInput.files.length == 0) return;
        var param = "";
        param = $("#hiddenUserGUID").val() + "#" + Code + "#" + id;
        var formData = new FormData();
        if (fileInput.files[0].size > 11000000) {
            fileInput.value = "";
            swal('خطأ', 'حجم الملف يتجاوز الحد المسموح به، يرجى إضافة ملف أخر', 'error');
        }
        else {
            console.log(fileInput);
            formData.append(param, fileInput.files[0]);
            var xmlobj = Expert.Attachment.createXMLHttp();
            xmlobj.onreadystatechange = function () {
                if (xmlobj.readyState == 4) {
                    if (xmlobj.status == 200) {
                        var src = JSON.parse(xmlobj.responseText);
                        if (src.Entity != null && src.Entity != undefined) {
                            console.log(src);
                            $('#AttachmentName_' + Code).attr('href', 'Uploads/' + src.Entity.name)
                            $('#AttachmentName_' + Code).text(src.Entity.displayName);
                            fileInput.value = "";
                            //$("#divloader").hide();
                            //.Attachment.DrawTable('Add', src.Entity, Code)
                            //var object = new Object();
                            //object.Name = src.Entity.name;
                            //object.IsMain = ismain;
                            //.Attachment.AttachmentArray.push(object);
                            //$('#Attachment_Name_' + Code).text("");
                            //$('#Attachment_Notes_' + Code).val("")
                            //$('#Attachment_File_' + Code).val(null);
                            //$('#Attachment_Loading_' + Code).hide();
                        }
                    }
                    else {
                        swal('خطأ', 'لا يمكن رفع هذا الملف ، يرجى إضافة ملف أخر', 'error');
                        fileInput.value = "";
                        console.log("File not Saved");
                        console.log(xmlobj);
                        $("#divloader").hide();
                    }
                }
            };
            xmlobj.open("post", "/ApplicationForm/SaveAttach", true);
            xmlobj.send(formData);
        }
    },
    checkAnswer: function (radio, code) {
        var value = radio.value;
        var cntrl = $("#divEvalAttach_" + code);
        if (value == 1) {
            cntrl.show();
        }
        else {
            cntrl.hide();
        }
    },
}