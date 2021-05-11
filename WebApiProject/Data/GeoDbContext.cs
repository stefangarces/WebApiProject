﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using WebApiProject.Models;
using WebApiProject.Models.V2;

namespace WebApiProject.Data
{
    public class GeoDbContext : IdentityDbContext<MyUser>
    {
        public GeoDbContext(DbContextOptions<GeoDbContext> options)
            : base(options)
        {

        }
        public DbSet<GeoMessage> GeoMessages { get; set; }
        public DbSet<GeoMessageV2> GeoMessagesV2 { get; set; }
        public DbSet<MyUser> Users { get; set; }
        public DbSet<Token> Tokens { get; set; }

        public async Task Seed(UserManager<MyUser> userManager)
        {
            await Database.EnsureDeletedAsync();
            await Database.EnsureCreatedAsync();

            var user = new MyUser { UserName = "FirstUser", FirstName = "John", LastName = "Userman" };
            await userManager.CreateAsync(user);

            var geoMessage = new GeoMessage
            {
                Message = "GetTest",
                Latitude = 80.5,
                Longitude = 90.3
            };

            var token = new Token { Key = new Guid(), User = user };

            await AddAsync(token);
            await AddAsync(geoMessage);
            await SaveChangesAsync();
        }
    }
}
