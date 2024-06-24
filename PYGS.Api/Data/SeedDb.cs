using Microsoft.EntityFrameworkCore;
using PYGS.Api.Helpers;
using PYGS.Shared.Entities;
using PYGS.Shared.Enums;

namespace PYGS.Api.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            await CheckCategoriesAsync();
            await CheckCiudadAsync();
            await CheckRolesAsync();
            await CheckUserAsync("2672152", "Christian", "Morales", "christian@yopmail.com", "0981612950", "Judas Tadeo y Victor Alfiere", UserType.Admin);
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }

        private async Task CheckCiudadAsync()
        {
            if (!_context.Ciudades.Any())
            {
                _context.Ciudades.Add(new Ciudad { Name = "Asuncion" });
                _context.Ciudades.Add(new Ciudad { Name = "Lambare" });


                await _context.SaveChangesAsync();
            }
        }

        private async Task<User> CheckUserAsync(string document, string firstName, string lastName, string email, string phone, string address, UserType userType)
        {
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {

                var ciudad = await _context.Ciudades.FirstOrDefaultAsync(x => x.Name == "Lambare");
                if (ciudad == null)
                {
                    ciudad = await _context.Ciudades.FirstOrDefaultAsync();
                }

                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    Ciudad = ciudad,
                    UserType = userType,
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());

                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);

            }

            return user;
        }

        private async Task CheckCategoriesAsync()
        {
            if (!_context.Categorias.Any())
            {
                _context.Categorias.Add(new Categoria { Name = "Maternidad" });
                _context.Categorias.Add(new Categoria { Name = "Terminacion" });


                await _context.SaveChangesAsync();
            }
        }

    }
}
