namespace CardStatApp.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using CardStatApp.Models;

    public partial class CardStatsContext : DbContext
    {
        public CardStatsContext()
            : base("name=CardStatsContext")
        {
        }

        public virtual DbSet<CardModel> Cards { get; set; }
        public virtual DbSet<ActionModel> Actions { get; set; }
        public virtual DbSet<HandModel> Hands { get; set; }
        public virtual DbSet<HandHistoryModel> HandHistories { get; set; }
        public virtual DbSet<PlayerActionModel> PlayerActions { get; set; }
        public virtual DbSet<PlayerModel> Players { get; set; }
        public virtual DbSet<SeatModel> Seats { get; set; }
        public virtual DbSet<TourneyResultModel> TourneyResults { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TourneyResultModel>()
                .Property(e => e.SiteName)
                .IsUnicode(false);

            modelBuilder.Entity<TourneyResultModel>()
                .Property(e => e.TourneyNo)
                .IsUnicode(false);

            modelBuilder.Entity<TourneyResultModel>()
                .Property(e => e.GameType)
                .IsUnicode(false);

            modelBuilder.Entity<TourneyResultModel>()
                .Property(e => e.BuyIn)
                .IsUnicode(false);

            modelBuilder.Entity<TourneyResultModel>()
                .Property(e => e.StartTime)
                .IsUnicode(false);

            modelBuilder.Entity<HandHistoryModel>()
             .Property(e => e.HandNo)
             .IsUnicode(false);

            modelBuilder.Entity<HandHistoryModel>()
                .Property(e => e.TourneyNo)
                .IsUnicode(false);

            modelBuilder.Entity<HandHistoryModel>()
           .Property(e => e.HoleCard1)
           .IsUnicode(false);

            modelBuilder.Entity<HandHistoryModel>()
           .Property(e => e.HoleCard2)
           .IsUnicode(false);

            modelBuilder.Entity<HandHistoryModel>()
                .Property(e => e.Board)
                .IsUnicode(false);
        }
    }
}
