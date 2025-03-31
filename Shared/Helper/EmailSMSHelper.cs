using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Helper
{
    public class EmailSMSHelper
    {

        public static string ReplaceTemplateFields(UserMainInformation model, string fileContents,string link ="")
        {
            if (!string.IsNullOrEmpty(fileContents))
            {
                fileContents = fileContents.Contains("##ContactFullName##") ? fileContents.Replace("##ContactFullName##", model.ContactFullName) : fileContents;
                fileContents = fileContents.Contains("##Email##") ? fileContents.Replace("##Email##", model.Email) : fileContents;
                fileContents = fileContents.Contains("##Password##") ? fileContents.Replace("##Password##", model.Password) : fileContents;
                fileContents = fileContents.Contains("##OrganizationName##") ? fileContents.Replace("##OrganizationName##", model.ContactFullName) : fileContents;
                fileContents = fileContents.Contains("##Link##") ? fileContents.Replace("##Link##", link) : fileContents;

            }

            return fileContents;

        }
    }
}
