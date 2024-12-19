using MediatR;
using Microsoft.AspNetCore.Identity;
using MyBlog.Domain.Entites.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Application.Features.Commands.Users
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommandRequest, UpdateUserCommandResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public UpdateUserCommandHandler(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<UpdateUserCommandResponse> Handle(UpdateUserCommandRequest request, CancellationToken cancellationToken)
        {
            var response = new UpdateUserCommandResponse();

            var user = await _userManager.FindByIdAsync(request.UserId);

            if (user == null)
            {
                response.Successed = false;
                response.Message = "Kullanıcı bulunamadı.";
                return response;
            }

            user.UserName = request.UserName ?? user.UserName;
            user.FirstName = request.FirstName ?? user.FirstName;
            user.LastName = request.LastName ?? user.LastName;
            user.Email = request.Email ?? user.Email;

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                response.Successed = false;
                response.Message = "Kullanıcı güncellenirken hata oluştu:";
                foreach (var error in updateResult.Errors)
                {
                    response.Message += $"{error.Code} - {error.Description}<br>";
                }
                return response;
            }

            if (!string.IsNullOrEmpty(request.RoleName))
            {
                var currentRoles = await _userManager.GetRolesAsync(user);

                // Mevcut rolleri kaldır
                foreach (var role in currentRoles)
                {
                    await _userManager.RemoveFromRoleAsync(user, role);
                }

                // Yeni rolü ata
                var roleExists = await _roleManager.RoleExistsAsync(request.RoleName);
                if (roleExists)
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, request.RoleName);
                    if (!roleResult.Succeeded)
                    {
                        response.Successed = false;
                        response.Message = "Rol atanırken hata oluştu:";
                        foreach (var error in roleResult.Errors)
                        {
                            response.Message += $"{error.Code} - {error.Description}<br>";
                        }
                        return response;
                    }
                }
                else
                {
                    response.Successed = false;
                    response.Message = $"Role '{request.RoleName}' bulunamadı.";
                    return response;
                }
            }

            response.Successed = true;
            response.Message = "Kullanıcı başarıyla güncellendi.";
            return response;
        }
    }
}
