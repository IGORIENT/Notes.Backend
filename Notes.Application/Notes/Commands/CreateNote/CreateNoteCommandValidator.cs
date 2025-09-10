using FluentValidation;

namespace Notes.Application.Notes.Commands.CreateNote
{
    public class CreateNoteCommandValidator : AbstractValidator<CreateNoteCommand>
    {
        public CreateNoteCommandValidator() 
        { 
            RuleFor(createNoteCommand => 
                createNoteCommand.Title).NotEmpty().MaximumLength(250); //Длина заголовка заметки д.б. не более 250 символов (не д.б. пусой)
            RuleFor(createNoteCommand => 
                createNoteCommand.UserId).NotEqual(Guid.Empty); //Id пользователя не должен быть пустым Guid-ом
        }

    }
}
