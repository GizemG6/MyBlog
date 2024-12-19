using MediatR;
using Microsoft.AspNetCore.Identity;
using MyBlog.Application.Interfaces;
using MyBlog.Domain.Entites;
using MyBlog.Domain.Entites.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Application.Features.Commands.Users
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public CreateUserCommandHandler(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
            // Kullanıcı oluşturuluyor
            var user = new AppUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = request.UserName,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email
            };

            // Kullanıcı veritabanına ekleniyor
            IdentityResult result = await _userManager.CreateAsync(user, request.Password);

            var response = new CreateUserCommandResponse();

            if (result.Succeeded)
            {
                // Rol kontrol ediliyor
                var roleExist = await _roleManager.RoleExistsAsync(request.RoleName);
                if (roleExist)
                {
                    // Rol atanıyor
                    IdentityResult roleResult = await _userManager.AddToRoleAsync(user, request.RoleName);
                    if (roleResult.Succeeded)
                    {
                        response.Successed = true;
                        response.Message = "Kullanıcı başarıyla oluşturulmuş ve role atanmıştır.";
                    }
                    else
                    {
                        response.Successed = false;
                        response.Message = "Kullanıcı oluşturuldu fakat role atanamadı.";
                        foreach (var error in roleResult.Errors)
                        {
                            response.Message += $"{error.Code} - {error.Description}<br>";
                        }
                    }
                }
                else
                {
                    response.Successed = false;
                    response.Message = $"Role '{request.RoleName}' does not exist.";
                }
            }
            else
            {
                // Kullanıcı oluşturulamadıysa hata mesajları ekleniyor
                response.Successed = false;
                response.Message = "Kullanıcı oluşturulamadı. Hatalar:";
                foreach (var error in result.Errors)
                {
                    response.Message += $"{error.Code} - {error.Description}<br>";
                }
            }

            return response;
        }

    }
}
