using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersManagementUi.Enums;
using UsersManagementUi.ViewModels.Common.Pagination;
using UsersManagementUi.ViewModels.Management;
using UsersManagementUi.ViewModels.Role;
using UsersManagementUi.WebServices.Implementation;
using UsersManagementUi.WebServices.Interfaces;
using System.Security.Claims;

namespace UsersManagementUi.Controllers
{
    [Authorize]
    public class ManagementController : Controller
    {
        private readonly IManagementService _managementService;
        private readonly INotyfService _toastNotification;

        public ManagementController(IManagementService managementService, INotyfService notyfService)
        {
            _managementService = managementService;
            _toastNotification = notyfService;
        }

        [HttpGet]
        public async Task<IActionResult> List(PaginationViewModel model, UserType type)
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var users = await _managementService.GetUsersList(model, token,type);
            if (users.Status == "Unauthorized")
            {
                _toastNotification.Information(users.Message);
                return RedirectToAction("SignOut", "Account");
            }
            if (!users.IsSuccessFull.Value)
            {
                _toastNotification.Error(users.Message);
                return RedirectToAction("index", "Home");
            }
            var paginationModel = new PaginationMetadata<UsersListViewModel>
            {
                Data = users.Data,
                CurrentPage = model.PageNumber,
                PageSize = model.PageSize,
                TotalCount = users.TotalCount.Value
            };
            ViewData["Type"] = (int)type;
            return View(paginationModel);
        }

        [HttpGet]
        public async Task<IActionResult> ClinicList(PaginationViewModel model)
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var users = await _managementService.GetClinicUsersList(model, token);
            if (users.Status == "Unauthorized")
            {
                _toastNotification.Information(users.Message);
                return RedirectToAction("SignOut", "Account");
            }
            if (!users.IsSuccessFull.Value)
            {
                _toastNotification.Error(users.Message);
                return RedirectToAction("index", "Home");
            }
            var paginationModel = new PaginationMetadata<ClinicUserListViewModel>
            {
                Data = users.Data,
                CurrentPage = model.PageNumber,
                PageSize = model.PageSize,
                TotalCount = users.TotalCount.Value
            };
            return View(paginationModel);
        }
        [HttpGet]
        public async Task<IActionResult> HisNovinList(PaginationViewModel model)
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var users = await _managementService.GetHisNovinUsersList(model, token);
            if (users.Status == "Unauthorized")
            {
                _toastNotification.Information(users.Message);
                return RedirectToAction("SignOut", "Account");
            }
            if (!users.IsSuccessFull.Value)
            {
                _toastNotification.Error(users.Message);
                return RedirectToAction("index", "Home");
            }
            var paginationModel = new PaginationMetadata<HisNovinUserListViewModel>
            {
                Data = users.Data,
                CurrentPage = model.PageNumber,
                PageSize = model.PageSize,
                TotalCount = users.TotalCount.Value
            };
            return View(paginationModel);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(string username)
        {
            var paginationModel = new PaginationViewModel();
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var userDetail = await _managementService.GetUser(username, token);
            if (userDetail.Status == "Unauthorized")
            {
                _toastNotification.Information(userDetail.Message);
                return RedirectToAction("SignOut", "Account");
            }
            if (!userDetail.IsSuccessFull.Value)
            {
                _toastNotification.Error(userDetail.Message);
                return RedirectToAction("List", "Management", new { model = paginationModel, type=2 });
            }
            return View(userDetail.Data);
        }


        [HttpGet]
        public async Task<IActionResult> TeriageUserDetail(string username)
        {
            var paginationModel = new PaginationViewModel();

            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var userDetail = await _managementService.GetTeriageUser(username, token);
            if (userDetail.Status == "Unauthorized")
            {
                _toastNotification.Information(userDetail.Message);
                return RedirectToAction("SignOut", "Account");
            }
            if (!userDetail.IsSuccessFull.Value)
            {
                _toastNotification.Error(userDetail.Message);
                return RedirectToAction("List", "Management", new { model = paginationModel, type = 1 });
            }
            return View(userDetail.Data);
        }


        [HttpGet]
        public async Task<IActionResult> ClinicUserDetail(string userId)
        {
            var paginationModel = new PaginationViewModel();

            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var userDetail = await _managementService.GetClinicUser(userId, token);
            if (userDetail.Status == "Unauthorized")
            {
                _toastNotification.Information(userDetail.Message);
                return RedirectToAction("SignOut", "Account");
            }
            if (!userDetail.IsSuccessFull.Value)
            {
                _toastNotification.Error(userDetail.Message);
                return RedirectToAction("List", "ClinicList", new { model = paginationModel});
            }
            return View(userDetail.Data);
        }

        [HttpGet]
        public async Task<IActionResult> HisNovinUserDetail(string userId)
        {
            var paginationModel = new PaginationViewModel();

            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var userDetail = await _managementService.GetHisNovinUser(userId, token);
            if (userDetail.Status == "Unauthorized")
            {
                _toastNotification.Information(userDetail.Message);
                return RedirectToAction("SignOut", "Account");
            }
            if (!userDetail.IsSuccessFull.Value)
            {
                _toastNotification.Error(userDetail.Message);
                return RedirectToAction("HisNovinList", "Management", new { model = paginationModel });
            }
            return View(userDetail.Data);
        }

        public async Task<IActionResult> Add()
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var userDetail = await _managementService.GetPermissionsAndUnits(token);
            if (userDetail.Status == "Unauthorized")
            {
                _toastNotification.Information(userDetail.Message);
                return RedirectToAction("SignOut", "Account");
            }
            return View(userDetail.Data);
        }
        public async Task<IActionResult> AddTeriageUser()
        {
            return View();
        }
        public async Task<IActionResult> AddClinicUser()
        {
            return View();
        }
        public async Task<IActionResult> AddHisNovinUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddIdentityUserViewModel model, UserType type)
        {
            var paginationModel = new PaginationViewModel();
            
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var result = await _managementService.AddUser(model, token,type);
            if (result.Status == "Unauthorized")
            {
                _toastNotification.Information(result.Message);
                return RedirectToAction("SignOut", "Account");
            }
            if (!result.IsSuccessFull.Value)
            {
                _toastNotification.Error(result.Message);
                return RedirectToAction("Add", "Management");
            }
            _toastNotification.Success(result.Message);
            return RedirectToAction("List", "Management", new { model = paginationModel,type = type });
        }

        [HttpPost]
        public async Task<IActionResult> AddClinicUser(AddClinicUserViewModel model)
        {
            var paginationModel = new PaginationViewModel();

            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var result = await _managementService.AddClinicUser(model, token);
            if (result.Status == "Unauthorized")
            {
                _toastNotification.Information(result.Message);
                return RedirectToAction("SignOut", "Account");
            }
            if (!result.IsSuccessFull.Value)
            {
                _toastNotification.Error(result.Message);
                return RedirectToAction("AddClinicUser", "Management");
            }
            _toastNotification.Success(result.Message);
            return RedirectToAction("ClinicList", "Management", new { model = paginationModel });
        }

        [HttpPost]
        public async Task<IActionResult> AddHisNovinUser(AddHisNovinUserViewModel model)
        {
            var paginationModel = new PaginationViewModel();

            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var result = await _managementService.AddHisNovinUser(model, token);
            if (result.Status == "Unauthorized")
            {
                _toastNotification.Information(result.Message);
                return RedirectToAction("SignOut", "Account");
            }
            if (!result.IsSuccessFull.Value)
            {
                _toastNotification.Error(result.Message);
                return RedirectToAction("AddHisNovinUser", "Management");
            }
            _toastNotification.Success(result.Message);
            return RedirectToAction("HisNovinList", "Management", new { model = paginationModel });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var result = await _managementService.DeleteUser(id, token);
            return Json(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteClinicUser(string id)
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var result = await _managementService.DeleteClinicUser(id, token);
            return Json(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteHisNovinUser(string id)
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var result = await _managementService.DeleteHisNovinUser(id, token);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdatePelicanUserViewModel model,int type)
        {
            var paginationModel = new PaginationViewModel();

            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var result = await _managementService.UpdateUser(model, token);
            if (result.Status == "Unauthorized")
            {
                _toastNotification.Information(result.Message);

                return RedirectToAction("SignOut", "Account");
            }
            if (!result.IsSuccessFull.Value)
            {
                _toastNotification.Error(result.Message);
                return RedirectToAction("List", "Management", new { model = paginationModel, type= type });
            }
            _toastNotification.Success(result.Message);
            return RedirectToAction("List", "Management", new { model = paginationModel,type = type });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateClinicUser(UpdateClinicUserViewModel model, int type)
        {
            var paginationModel = new PaginationViewModel();

            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var result = await _managementService.UpdateClinicUser(model, token);
            if (result.Status == "Unauthorized")
            {
                _toastNotification.Information(result.Message);

                return RedirectToAction("SignOut", "Account");
            }
            if (!result.IsSuccessFull.Value)
            {
                _toastNotification.Error(result.Message);
                return RedirectToAction("ClinicList", "Management", new { model = paginationModel });
            }
            _toastNotification.Success(result.Message);
            return RedirectToAction("ClinicList", "Management", new { model = paginationModel });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateHisNovinUser(UpdateHisNovinUserViewModel model, int type)
        {
            var paginationModel = new PaginationViewModel();
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var result = await _managementService.UpdateHisNovinUser(model, token);
            if (result.Status == "Unauthorized")
            {
                _toastNotification.Information(result.Message);

                return RedirectToAction("SignOut", "Account");
            }
            if (!result.IsSuccessFull.Value)
            {
                _toastNotification.Error(result.Message);
                return RedirectToAction("HisNovinList", "Management", new { model = paginationModel });
            }
            _toastNotification.Success(result.Message);
            return RedirectToAction("HisNovinList", "Management", new { model = paginationModel });
        }

    }
}
