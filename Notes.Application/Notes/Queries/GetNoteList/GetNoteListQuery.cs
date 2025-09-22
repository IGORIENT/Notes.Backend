using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Application.Notes.Queries.GetNoteList
{
    public class GetNoteListQuery : IRequest<NoteListVm>  // запрос на получение списка заметок конкретного юзера. Результатом выполнения запроса будет вью модель
    {
        public Guid UserId { get; set; }

    }
}
