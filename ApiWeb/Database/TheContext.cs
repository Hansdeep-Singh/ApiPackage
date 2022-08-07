﻿using ApiWeb.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiWeb.Database
{
    public class TheContext : DbContext
    {
        public TheContext() { }
        public TheContext(DbContextOptions<TheContext> options)
           : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<TheToken> TheTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {

        }


    }
}
