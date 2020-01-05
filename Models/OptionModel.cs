namespace SurveyShrike.Models
{
    public class OptionModel : BaseModel
    {
        public string PlaceHolder { get; set; }
        public int MinLength { get; set; }
        public int MaxLength { get; set; }
        public string RegexPattern { get; set; }
    }
}