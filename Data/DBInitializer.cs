using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeviantMusicCore.Data;
using DeviantMusicCore.Logic;
using DeviantMusicCore.Models;

namespace DeviantMusicCore.Data
{
    public class DBInitializer : IDBInitializer
    {
        private readonly DeviantContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DBInitializer(DeviantContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public async void Initialize()    
        {
            //Exit if role already exists
            if (_db.Roles.Any(r => r.Name == Roles.Master)) return;

            if (_db.Roles.Any(r => r.Name == Roles.Admin)) return;

            if (_db.Roles.Any(r => r.Name == Roles.SuperAdmin)) return;

            if (_db.Roles.Any(r => r.Name == Roles.Member)) return;

            //Create Admin role
            _roleManager.CreateAsync(new IdentityRole(Roles.Master)).GetAwaiter().GetResult();

            _roleManager.CreateAsync(new IdentityRole(Roles.Admin)).GetAwaiter().GetResult();

            _roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin)).GetAwaiter().GetResult();

            _roleManager.CreateAsync(new IdentityRole(Roles.Member)).GetAwaiter().GetResult();


            //Create Admin user
            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "azra3l",
                Email = "azra3l@gmail.com",
                EmailConfirmed = true,
                Name = "Michael Azra3l",
                StageName = "AZRA3L",
                Designation = "Developer and Producer",
                IsTeam = true,
                IsArtiste = true
            },"P@ssword1").GetAwaiter().GetResult();

            //Assign role to Admin user
            await _userManager.AddToRoleAsync(await _userManager.FindByNameAsync("azra3l"),Roles.Master);

             _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "superadmin1",
                Email = "superadmin@gmail.com",
                Name = "Test Super Admin",
                Designation = "Super Admin",
                EmailConfirmed = true,
                IsTeam = true
            },"P@ssword1").GetAwaiter().GetResult();

            //Assign role to Admin user
            await _userManager.AddToRoleAsync(await _userManager.FindByNameAsync("superadmin1"),Roles.SuperAdmin);

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "admin1",
                Email = "admin@gmail.com",
                Name = "Test Admin",
                Designation = "Admin",
                EmailConfirmed = true,
                IsTeam = true
            },"P@ssword1").GetAwaiter().GetResult();

            //Assign role to Admin user
            await _userManager.AddToRoleAsync(await _userManager.FindByNameAsync("admin1"),Roles.Admin);

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "member1",
                Email = "member@gmail.com",
                Name = "Test Member",
                Designation = "Member",
                EmailConfirmed = true,
                IsTeam = true
            },"P@ssword1").GetAwaiter().GetResult();

            //Assign role to Admin user
            await _userManager.AddToRoleAsync(await _userManager.FindByNameAsync("member1"),Roles.Member);

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "user1",
                Email = "user@gmail.com",
                Name = "Test Artiste",
                StageName = "TestArtiste",
                Designation = "Artiste",
                EmailConfirmed = true,
                IsArtiste = true
            },"P@ssword1").GetAwaiter().GetResult();
                       

        }
    }
}
