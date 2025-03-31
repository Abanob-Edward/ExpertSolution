'use strict';

(function ($) {
    $('#addRowComputer').click(function () {
        const rowCount = $('#ComputerSkills-table tbody tr').length + 1;
        var ComputerSkill = "<option  value=0 >--اختر--</option>";
        var object = new Object();
        object.TableName = "lookupComputerSkill";
        Expert.Ajax.Post('ApplicationForm/GetLookup', object, function (data) {
            if (data != null && data.Valid) {
                $.each(data.result, function (e, entity) {
                    ComputerSkill += "<option  value=" + entity.id + " >" + entity.name + "</option>";

                });
            }
        });

        var evaloption = "<option  value=0 >--اختر--</option>";
        object = new Object();
        object.TableName = "lookupLevel";
        Expert.Ajax.Post('ApplicationForm/GetLookup', object, function (data) {
            if (data != null && data.Valid) {
                $.each(data.result, function (e, entity) {
                        evaloption += "<option  value=" + entity.id + " >" + entity.name + "</option>";

                });
            }
        });

        var object = new Object();
        object.Number = rowCount;
        object.TableName = "ApplicationFormComputerSkills";
        object.UserGUID = $("#hiddenUserGUID").val();
        Expert.Ajax.Post('ApplicationForm/CreateTableRow', object, function (data) {
            if (data != null && data.Valid) {
                const newRow = `
          <tr  data-id="${data.id}">
              <td>${String(rowCount).padStart(2, '0')}</td>
              <td>    
               <select class="form-select" id="selectComputerSkill" onchange="SaveTableValue(this,'ApplicationFormComputerSkills','ComputerSkillId');" required style="width:200px">
                               ${ComputerSkill}
                     </select>

               </td>
               <td>    
                  <select class="form-select" id="selectlvlComputer" required style="width:200px" onchange="SaveTableValue(this,'ApplicationFormComputerSkills','LevelId');" >
                            ${evaloption}
                     </select>

               </td>
              <td>
             <input type="text" class="invoive-form-control" onchange="SaveTableValue(this,'ApplicationFormComputerSkills','Notes');"  />
              </td>
               
              <td class="text-center">
                  <button type="button" class="remove-row"><iconify-icon icon="ic:twotone-close" class="text-danger-main text-xl"></iconify-icon></button>
              </td>
          </tr>
      `;
                $('#ComputerSkills-table tbody').append(newRow);
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
        object.TableName = "ApplicationFormComputerSkills";
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
        $('#ComputerSkills-table tbody tr').each(function (index) {
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