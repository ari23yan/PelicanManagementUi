//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.AspNetCore.Mvc;
//using Lakoode.Repository.Interfaces.Account;
//using Lakoode.Models.Dtos.Common;

//namespace Lakoode.HttpHandler
//{
//    public class PermissionCheckerAttribute : AuthorizeAttribute, IAuthorizationFilter
//    {
//        private IUserRepository _userService;

//        public void OnAuthorization(AuthorizationFilterContext context)
//        {

//            if (context.HttpContext.User.Identity.IsAuthenticated)
//            {
//                var userId = context.HttpContext.User.GetUserId();
//                _userService = (IUserRepository)context.HttpContext.RequestServices.GetService(typeof(IUserRepository));

//                if (!_userService.ChekUserIsAdmin(userId))
//                {
//                    context.Result = new JsonResult(new
//                    {
//                        message = "User is Not Admin"
//                    })
//                    {
//                        StatusCode = 401 // Unauthorized
//                    };

//                }
//            }
//            else
//            {
//                context.Result = new JsonResult(new
//                {
//                    message = "User is not Unauthorized"
//                })
//                {
//                    StatusCode = 401 // Unauthorized
//                };
//            }
//        }
//    }
//}
