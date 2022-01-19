using Microsoft.AspNetCore.Mvc;

namespace WebApi.Api.Controllers
{
    [ApiController]// nhớ thêm

    public class CategoryController : ControllerBase
    {
       public string Welcome()
        {
            return "Welcome!";
        }
    }
}
