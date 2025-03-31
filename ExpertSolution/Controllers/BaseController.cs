using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Globalization;
using System.Web;

namespace ExpertSolution.Controllers;

public class BaseController : Controller
{
    public BaseController()
    {
       
    }
    public UserMainInformation UserInfo
    {
        get
        {
            UserMainInformation? entity = new UserMainInformation();
            var cookie = Request.Cookies["ExpertUserInfo"];
            if (cookie != null)
            {
                string? decode = HttpUtility.UrlDecode(cookie);
                var obj = JsonConvert.DeserializeObject<UserMainInformation>(decode);
                entity = obj;
            }
            return entity == null ? new UserMainInformation() : entity;
        }
    }
}