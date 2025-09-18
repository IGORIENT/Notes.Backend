using Microsoft.EntityFrameworkCore;
using Notes.Application.Interfaces;
using Notes.Domain;
using Notes.Persistence.EntityTypeConfigurations;


namespace Notes.Persistence
{
    public class NotesDbContext : DbContext, INotesDbContext
    {
        public DbSet<Note> Notes { get; set;}

        //конструктор, в него подается один параметр - опции: провайдет, имя БД и др.)
        public NotesDbContext(DbContextOptions<NotesDbContext> options) : base(options) { }


        // Здесь описывается, как класс Note мапится на таблицу в БД.
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new NoteConfiguration());
            base.OnModelCreating(builder);
        }

    }
}
