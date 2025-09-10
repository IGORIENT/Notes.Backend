using FluentValidation;

namespace Notes.Application.Notes.Queries.GetNoteDetails
{
    public class GetNoteDetailsQueryValidator : AbstractValidator<GetNoteDetailsQuery>
    {
        public GetNoteDetailsQueryValidator()
        {
            RuleFor(Note => Note.Id).NotEqual(Guid.Empty);
            RuleFor(Note => Note.UserId).NotEqual(Guid.Empty);
        }
    }
}
