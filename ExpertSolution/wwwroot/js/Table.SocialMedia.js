'use strict';



(function ($) {
    $('#addRowSocial').click(function () {
        const rowCount = $('#Social-table tbody tr').length + 1;
        var socialoption = "<option  value=0 >--اختر--</option>";
        var object = new Object();
        object.TableName = "lookupSocial";
        Expert.Ajax.Post('ApplicationForm/GetLookup', object, function (data) {
            if (data != null && data.Valid) {
                $.each(data.result, function (e, entity) {
                        socialoption += "<option  value=" + entity.id + " >" + entity.name + "</option>";

                });
            }
        });

        var object = new Object();
        object.Number = rowCount;
        object.TableName = "ApplicationFormSocialLinks";
        object.UserGUID = $("#hiddenUserGUID").val();
        Expert.Ajax.Post('ApplicationForm/CreateTableRow', object, function (data) {
            if (data != null && data.Valid) {
                const newRow = `
          <tr data-id="${data.id}">
              <td>${String(rowCount).padStart(2, '0')}</td>
              <td>   
              <select class="form-select" id="selectLangId" onchange="SaveTableValue(this,'ApplicationFormSocialLinks','SocialId');" required style="width:200px">
                               ${socialoption}
                     </select>
               </td>
              <td>
             <input type="text" onchange="SaveTableValue(this,'ApplicationFormSocialLinks','Link');" class="invoive-form-control" />
              </td>
               
              <td class="text-center">
                  <button type="button" class="remove-row"><iconify-icon icon="ic:twotone-close" class="text-danger-main text-xl"></iconify-icon></button>
              </td>
          </tr>`;
                $('#Social-table tbody').append(newRow);
            }
            else {
                swal('خطأ', 'حدث خطأ ما، يرجى المحاولة مرة أخرى', 'error');
            }
        });



    });

    $(document).on('click', '.remove-row', function () {
        var tr = $(this).closest('tr');
        var object = new Object();
        object.Id = tr.data("id");
        object.TableName = "ApplicationFormSocialLinks";
        Expert.Ajax.Post('ApplicationForm/DeleteTableRow', object, function (data) {
            if (data != null && data.Valid) {
                tr.remove();
                updateRowNumbers();
            }
            else {
                swal('خطأ', 'حدث خطأ ما، يرجى المحاولة مرة أخرى', 'error');
            }
        });

        
    });
   
    function updateRowNumbers() {
        $('#Social-table tbody tr').each(function (index) {
            $(this).find('td:first').text(String(index + 1).padStart(2, '0'));
        });
    }

    // Make table cells editable on click
    $('.editable').click(function () {
        const cell = $(this);
        const originalText = cell.text().substring(1); // Remove the leading ':'
        const input = $('<input type="text" class="invoive-form-control" />').val(originalText);

        cell.empty().append(input);

        input.focus().select();

        input.blur(function () {
            const newText = input.val();
            cell.text(' ' + newText);
        });

        input.keypress(function (e) {
            if (e.which == 13) { // Enter key
                const newText = input.val();
                cell.text(':' + newText);
            }
        });
    });
})(jQuery);