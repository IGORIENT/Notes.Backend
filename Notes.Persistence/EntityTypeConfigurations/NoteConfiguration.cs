using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notes.Domain;

namespace Notes.Persistence.EntityTypeConfigurations
{
    public class NoteConfiguration : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder.HasKey(note => note.Id); // первичный ключ
            builder.HasIndex(note => note.Id).IsUnique(); //создаем индекс по полю Id
            builder.Property(note => note.Title).HasMaxLength(250);
        }
    }
}
