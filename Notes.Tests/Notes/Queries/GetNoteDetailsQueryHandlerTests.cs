using AutoMapper;
using Notes.Persistence;
using Notes.Tests.Common;
using Notes.Application.Notes.Queries.GetNoteDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;

namespace Notes.Tests.Notes.Queries
{
    [Collection("QueryCollection")]
    public class GetNoteDetailsQueryHandlerTests
    {
        private readonly NotesDbContext Context;
        private readonly IMapper Mapper;

        public GetNoteDetailsQueryHandlerTests(QueryTestFixture fixture)
        {
            Context = fixture.Context;
            Mapper = fixture.Mapper;
        }

        [Fact]
        public async Task GetNoteDetailsQueryHandler_Success()
        {
            // Arrange
            var handler = new GetNoteDetailsQueryHandler(Context, Mapper);
            // Act
            var result = await handler.Handle(
                new GetNoteDetailsQuery
                {
                    UserId = NotesContextFactory.UserBId,
                    Id = Guid.Parse("168EF99A-C9F1-4861-860F-CB0F985E2083")
                },
                CancellationToken.None);

            // Assert
            result.ShouldBeOfType<NoteDetailsVm>();
            result.Title.ShouldBe("Title2");
            result.CreationDate.ShouldBe(DateTime.Today);
        }
    }
}
