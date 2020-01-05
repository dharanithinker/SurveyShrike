using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyShrike.Models
{
    public class SurveyDeails
    {
        public int Id { get; set; }
        public string SurveyName { get; set; }
        public List<QuestionModel> Questions { get; set; }
        public string CreatedBy { get; set; }
    }
}
