using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepOnCodingAcademy.DataAccess.Models
{
    public class QuestionRepository : IQuestionRepository
    {
        private List<Question> _questionList;
        private readonly AppDbContext context;

        public QuestionRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Question GetQuestion(int Id)
        {
            return context.Questions.Find(Id);
        }

        public IEnumerable<Question> GetAllQuestions()
        {
            return context.Questions;
        }


    }
}
