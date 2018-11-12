﻿using CardioCarta.Controllers;
using CardioCarta.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System;
using System.Threading;
using System.Threading.Tasks;

[assembly: OwinStartupAttribute(typeof(CardioCarta.Startup))]
namespace CardioCarta
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRoles();
            Thread downloadAirly = new Thread(new ThreadStart(AirlyApi.AirlyTrigger));
            downloadAirly.Start();
            GC.KeepAlive(downloadAirly);
        }

        private void CreateRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            if (!roleManager.RoleExists("Patient"))
            {
                var role = new IdentityRole();
                role.Name = "Patient";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Doctor"))
            {
                var role = new IdentityRole();
                role.Name = "Doctor";
                roleManager.Create(role);
            }
        }
    }
}
