using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyShrike.Models
{
    public class QuestionModel : BaseModel
    {
        public BaseModel QustionType { get; set; } // Id-1,Text-SingleLine
        public List<OptionModel> Options { get; set; }
        public bool IsMandatory { get; set; }
    }
}
