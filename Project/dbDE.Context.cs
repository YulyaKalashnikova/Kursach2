﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Project
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class dbData : DbContext
    {
        private static dbData _context;

        public static User s_user;

        public static dbData GetContext()
        {
            if (_context == null)
                _context = new dbData();

            return _context;
        }

        public dbData()
            : base("name=dbData")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Condition> Conditions { get; set; }
        public virtual DbSet<Enterprise> Enterprises { get; set; }
        public virtual DbSet<ListOfThing> ListOfThings { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Thing> Things { get; set; }
        public virtual DbSet<TypeOfThing> TypeOfThings { get; set; }
        public virtual DbSet<Unit> Units { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
