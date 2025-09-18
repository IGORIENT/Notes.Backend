using System;
using Microsoft.EntityFrameworkCore;
using Notes.Domain;
using Notes.Persistence;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Notes.Tests.Common
{
    public class NotesContextFactory
    {
        public static Guid UserAId = Guid.NewGuid();
        public static Guid UserBId = Guid.NewGuid();

        public static Guid NoteIdFordelete = Guid.NewGuid();
        public static Guid NoteIdForUpdate = Guid.NewGuid();

        public static NotesDbContext Create()
        {
            //задаются опции для создания БД
            // 1. провайдер EF Core InMemory - чтобы создать базу в ОЗУ (м.б. другой провайдет, PostgreSQL, SQLite, SQL server)
            // 2. имя БД - случайный Guid 
            var options = new DbContextOptionsBuilder<NotesDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options; 
            
            // тут уже создается экзмепляр контекста 
            var context = new NotesDbContext(options);
            
            // контекст проверяет, создана ли БД и все ее таблицы.
            context.Database.EnsureCreated();

            // данные, с которыми будут работать тесты Update, Delete, GetList;
            context.Notes.AddRange(
                new Note
                {
                    CreationDate = DateTime.Today,
                    Details = "Details1",
                    EditDate = null,
                    Id = Guid.Parse("{189A4EDB-C6CC-4049-9350-C892D9F4421B}"),
                    Title = "Title1",
                    UserId = UserAId,
                },
                new Note
                {
                    CreationDate = DateTime.Today,
                    Details = "Details2",
                    EditDate = null,
                    Id = Guid.Parse("{168EF99A-C9F1-4861-860F-CB0F985E2083}"),
                    Title = "Title2",
                    UserId = UserBId,
                },
                new Note
                {
                    CreationDate = DateTime.Today,
                    Details = "Details3",
                    EditDate = null,
                    Id = NoteIdFordelete,
                    Title = "Title3",
                    UserId = UserAId,
                },
                 new Note
                 {
                     CreationDate = DateTime.Today,
                     Details = "Details4",
                     EditDate = null,
                     Id = NoteIdForUpdate,
                     Title = "Title4",
                     UserId = UserBId,
                 }
            );
            context.SaveChanges();
            return context;
        }

        public static void Destroy(NotesDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
