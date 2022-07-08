using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepOnCodingAcademy.DataAccess.Models
{
    public interface IQuestionRepository
    {
        Question GetQuestion(int Id);
        IEnumerable<Question> GetAllQuestions();
    }
}
