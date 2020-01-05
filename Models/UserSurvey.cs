using System.Collections.Generic;

namespace SurveyShrike.Models
{
    public class UserSurveyModel
    {
        public int SurveyId { get; set; }
        public List<QuestionAnswer> UserValues { get; set; }
        public string SubmittedBy { get; set; }
    }
}
