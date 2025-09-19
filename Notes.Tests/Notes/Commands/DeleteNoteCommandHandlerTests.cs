using Notes.Tests.Common;
using Notes.Application.Notes.Commands.DeleteCommand;
using Notes.Application.Common.Exceptions;

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
    }
}
