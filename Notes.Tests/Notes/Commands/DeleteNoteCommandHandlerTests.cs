using Notes.Tests.Common;
using Notes.Application.Notes.Commands.DeleteCommand;
using Notes.Application.Common.Exceptions;
using Notes.Application.Notes.Commands.CreateNote;

namespace Notes.Tests.Notes.Commands
{
    // Тест для удаления замтки
    public class DeleteNoteCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task DeleteNoteCommandHandler_Success()
        {
            // Arrange 
            var handler = new DeleteNoteCommandHandler(Context);


            // Act
            await handler.Handle(new DeleteNoteCommand
            {
                Id = NotesContextFactory.NoteIdFordelete,
                UserId = NotesContextFactory.UserAId
            },
            CancellationToken.None);

            // Assert
            Assert.Null(Context.Notes.SingleOrDefault(note =>
                note.Id == NotesContextFactory.NoteIdFordelete));
        }


        [Fact]
        public async Task DeleteNotCommandHandler_FailOnWrongId()
        {
            // Arrange
            var handler = new DeleteNoteCommandHandler(Context);

            // Act
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>  // для проверки, что асинхронный код выбрасывает исключение определённого типа
                await handler.Handle(
                    new DeleteNoteCommand
                    {
                        Id = Guid.NewGuid(),
                        UserId = NotesContextFactory.UserAId,
                    },
                    CancellationToken.None));
        }


        [Fact]
        public async Task DeleteNoteCommandHandler_FailOnWrongUserId()
        {
            // Arrange
            var deleteHandler = new DeleteNoteCommandHandler(Context);
            var createHandler = new CreateNoteCommandHandler(Context);
            var noteId = await createHandler.Handle(
                new CreateNoteCommand
                {
                    Title = "NoteTitle",
                    UserId = NotesContextFactory.UserAId  //тут указали что UserId = A
                }, CancellationToken.None);


            // Act
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await deleteHandler.Handle(
                    new DeleteNoteCommand
                    {
                        // метод Handle возвращает Guid созданной заметки
                        Id = noteId,  
                        UserId = NotesContextFactory.UserBId  // а тут хотим удалить эту заметку, но UserId = B
                    }, CancellationToken.None));
        }


    }
}
