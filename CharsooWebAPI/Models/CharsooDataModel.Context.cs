﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CharsooWebAPI.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class charsoog_DBEntities : DbContext
    {
        public charsoog_DBEntities()
            : base("name=charsoog_DBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Puzzle> Puzzles { get; set; }
        public virtual DbSet<PlayerInfo> PlayerInfoes { get; set; }
        public virtual DbSet<LogIn> LogIns { get; set; }
        public virtual DbSet<Purchase> Purchases { get; set; }
        public virtual DbSet<UserPuzzle> UserPuzzles { get; set; }
        public virtual DbSet<PlayPuzzle> PlayPuzzles { get; set; }
        public virtual DbSet<PuzzleRate> PuzzleRates { get; set; }
        public virtual DbSet<PushID> PushIDs { get; set; }
        public virtual DbSet<Reward> Rewards { get; set; }
    }
}
