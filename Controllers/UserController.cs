﻿using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PelicanManagementUi.Models.ViewModels.Common.Pagination;
using PelicanManagementUi.Models.ViewModels.User;
using PelicanManagementUi.WebServices.Interfaces;
using System.Drawing.Printing;
using System.Reflection;
using System.Security.Claims;

namespace PelicanManagementUi.Controllers
{
    [Authorize]
    public class UserController : Controller
    {

        private readonly IExternalServices _service;
        private readonly INotyfService _toastNotification;

        public UserController(IExternalServices externalServices, INotyfService notyfService)
        {
            _service = externalServices;
            _toastNotification = notyfService;

        }
        [HttpGet]
        public async Task<IActionResult> List(PaginationViewModel model)
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var users = await _service.GetUserList(model, token);

            var paginationModel = new PaginationMetadata<UsersListViewModel>
            {
                Data = users.Data,
                CurrentPage = model.PageNumber,
                PageSize = model.PageSize,
                TotalCount = users.TotalCount.Value
            };
            return View(paginationModel);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var userDetail = await _service.GetUser(id, token);
            return View(userDetail.Data);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var rolesList = await _service.GetRolesList(token);
            return View(rolesList.Data);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddUserViewModel model)
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var userDetail = await _service.AddUser(model, token);
            if (!userDetail.IsSuccessFull.Value)
            {
                _toastNotification.Error(userDetail.Message);
                return RedirectToAction("Detail", "User");
            }
            _toastNotification.Success(userDetail.Message);
            return RedirectToAction("List", "User");

        }

        [HttpPost]
        public async Task<IActionResult> GetRolePermissions(Guid roleID)
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var userDetail = await _service.GetRolePermissions(roleID, token);
            return Json(userDetail.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateUserViewModel model)
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var userDetail = await _service.UpdateUser(model, token);
            if (!userDetail.IsSuccessFull.Value)
            {
                _toastNotification.Error(userDetail.Message);
                return RedirectToAction("Detail", "User", new { id = model.UserId });
            }
            _toastNotification.Success(userDetail.Message);
            return RedirectToAction("Detail", "User", new { id = model.UserId });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var result = await _service.DeleteUser(id, token);
            return Json(result);
        }

        [HttpPut]
        public async Task<IActionResult> ToggleActiveStatus(Guid id)
        {
            var token = HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
            var result = await _service.ToggleActiveStatus(id, token);
            return Json(result);
        }




    }
}
