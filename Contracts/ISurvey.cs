using SurveyShrike.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyShrike.Contracts
{
    public interface ISurvey
    {
        bool AddSurvey(SurveyDeails newSurvey);

        List<BaseModel> LoadSurvey();

        SurveyDeails SurveyDetails(int suveyId);

        bool Save(UserSurveyModel userServeyDetails);
    }
}
