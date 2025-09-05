using System;
using MediatR;
namespace Notes.Application.Notes.Commands.DeleteCommand
{
    public class DeleteNoteCommand :IRequest
    {
        public Guid UserId {  get; set; } //Id пользователя

        public Guid Id { get; set; } //Id заметки
    }
}
