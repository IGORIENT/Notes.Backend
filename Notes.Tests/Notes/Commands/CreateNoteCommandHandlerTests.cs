using Microsoft.EntityFrameworkCore;
using Notes.Application.Notes.Commands.CreateNote;
using Notes.Tests.Common;

namespace Notes.Tests.Notes.Commands
{
    public class CreateNoteCommandHandlerTests : TestCommandBase
    {
        [Fact] //означает, что данный метод должен быть запущен во время прогона тестов.
        public async Task CreateNoteCommandHandler_Success()
        {
            // Метод тестов обычно делится на 3 части

            // 1. Arrange - подготовка данных
            var handler = new CreateNoteCommandHandler(Context);  //тут контекст - это БД в ОЗУ с 4-мя заметками.
            var noteName = "note name";
            var noteDetails = "note details";



            // 2. Act -выполнение логики
            var noteId = await handler.Handle(
                new CreateNoteCommand
                {
                    Title = noteName,
                    Details = noteDetails,
                    UserId = NotesContextFactory.UserAId
                },
                CancellationToken.None);



            // 3. Assert - проверка результатов

            Assert.NotNull(
                await Context.Notes.SingleOrDefaultAsync(note => 
                    note.Id == noteId && note.Title == noteName && 
                    note.Details == noteDetails));

        }
    }
}
