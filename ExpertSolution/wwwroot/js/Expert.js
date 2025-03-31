Expert.Ajax.Post = function (url, parameters, callBack) {
    //console.log("URL = " + url);
    //console.log(parameters);
    //console.log(callBack);
    if (url == undefined || url == 'undefined')
    {
        window.location = ""; 
        return false;
    }

    //$("#divloader").show();
    $.ajax({
        url: url, type: "POST", async: false, dataType: "JSON", data: parameters,
        beforeSend: function () {
            $("#divloader").removeAttr("style");
            //$("#divloader").attr("style", "display: block;");
            //$("#divloader").show();
        },
        success: function (result) {
            //console.log(result);
            if (result.IsAuthorized || result.isAuthorized) {
                if (callBack != null)
                    callBack(result);
            }
        },
        complete: function (data) {
            $("#divloader").hide();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(jqXHR);
            console.log("In Post");
            console.log("textStatus = " + textStatus);
            console.log("errorThrown = " + errorThrown);
            console.log("parameters = " + JSON.stringify(parameters));
            console.log("URL = " + url);
        }
    });
};

Expert.Ajax.Get = function (url, parameters, error, callBack) {
    $("#object_loader").show();
    $.ajax({
        url: url, type: "GET", dataType: "JSON", data: parameters,
        success: function (result) {
            if (result.IsAuthorized) {
                if (result.IsValid) {
                    if (callBack != null)
                        callBack(result);
                }
                else
                    swal(Error, result.Error, 'warning');
            }
            $("#object_loader").hide();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $("#object_loader").hide(); console.log(jqXHR); console.log("In GET"); console.log("textStatus = " + textStatus); console.log("errorThrown = " + errorThrown); console.log("parameters = " + JSON.stringify(parameters)); console.log("URL = " + url);
        }
    });
};

Expert.Helper.GetControlSelectedId = function (controlId) {
    if (!controlId) {
        console.log("Control ID is invalid");
        return 0;
    }

    var selectedValue = $("#" + controlId).val();
    console.log("Control ID:", controlId, "Selected Value:", selectedValue);

    if (!selectedValue) {
        console.log("Selected value is invalid, returning 0");
        return 0;
    }

    return selectedValue;
};