using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using RestSharp;
using System.Web;

namespace ExpertSolution.Extensions;

public static class HtmlExtensions
{
    public static UserMainInformation GetUserinfo(this IHtmlHelper helper)
    {
        UserMainInformation? entity = new UserMainInformation();

        var cookie = helper.ViewContext.HttpContext.Request.Cookies["ExpertUserInfo"];
        if (cookie != null)
        {
            string? decode = HttpUtility.UrlDecode(cookie);
            var obj = JsonConvert.DeserializeObject<UserMainInformation>(decode);
            entity = obj;
        }
        return entity == null ? new UserMainInformation() : entity;
    }


}
