using Notes.Persistence;

namespace Notes.Tests.Common
{
    public class TestCommandBase : IDisposable
    {
        protected readonly NotesDbContext Context;
        public TestCommandBase()
        {
            Context = NotesContextFactory.Create();
        }

        // метод для удаления БД
        public void Dispose()
        {
            NotesContextFactory.Destroy(Context);
        }
    }
}
