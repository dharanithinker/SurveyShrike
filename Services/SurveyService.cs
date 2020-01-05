using SurveyShrike.Contracts;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SurveyShrike.Models;

namespace SurveyShrike.Services
{
    public class SurveyService : ISurvey
    {
        public bool AddSurvey(SurveyDeails surveyDetails)
        {
            bool status = false;
            try
            {
                using (SurveyShrikeEntities context = new SurveyShrikeEntities())
                {
                    Survey survey = new Survey();
                    survey.Name = surveyDetails.SurveyName;
                    survey.CreatedBy = surveyDetails.CreatedBy;
                    survey.CreatedDate = DateTime.UtcNow;
                    context.Surveys.Add(survey);
                    context.SaveChanges();
                    if (survey.SurveyId > 0)
                    {
                        AddQuestions(surveyDetails, context);
                        status = true;
                    }
                }
                return status;
            }
            catch (Exception ex)
            {
                return status;
            }
        }
        private static void AddQuestions(SurveyDeails surveyDetails, SurveyShrikeEntities context)
        {
            foreach (var currentQuestion in surveyDetails.Questions)
            {
                Question question = new Question();
                question.Description = currentQuestion.Text;
                question.TypeID = currentQuestion.QustionType.Id;
                question.IsMandatory = currentQuestion.IsMandatory;
                context.Questions.Add(question);
                context.SaveChanges();

                SurveyQuestionMapping surveyQuestionMapping = new SurveyQuestionMapping();
                surveyQuestionMapping.SurveyId = surveyDetails.Id;
                surveyQuestionMapping.QuestionId = currentQuestion.Id;
                context.SurveyQuestionMappings.Add(surveyQuestionMapping);
                context.SaveChanges();

                if (question.QuestionId > 0)
                {
                    AddOptions(context, currentQuestion, question);
                }
            }
        }
        private static void AddOptions(SurveyShrikeEntities context, QuestionModel currentQuestion, Question question)
        {
            foreach (var currentOption in currentQuestion.Options)
            {
                Option option = new Option();
                option.QuestionId = question.QuestionId;
                option.Description = currentOption.Text;
                context.Options.Add(option);
                context.SaveChanges();
                if (option.OptionId > 0)
                {
                    OptionDetail optionDetail = new OptionDetail();
                    optionDetail.OptionId = option.OptionId;
                    optionDetail.PlaceHolder = !String.IsNullOrEmpty(currentOption.PlaceHolder) ? currentOption.PlaceHolder : "";
                    optionDetail.MinLength = currentOption.MinLength;
                    optionDetail.MaxLength = currentOption.MaxLength;
                    optionDetail.RegexPattern = !String.IsNullOrEmpty(currentOption.RegexPattern) ? currentOption.RegexPattern : null;
                    context.OptionDetails.Add(optionDetail);
                    context.SaveChanges();
                }
            }
        }
        public List<BaseModel> LoadSurvey()
        {
            List<BaseModel> surveyDetails = new List<BaseModel>();
            try
            {
                using (SurveyShrikeEntities context = new SurveyShrikeEntities())
                {
                    surveyDetails = context.Surveys.Where(x => x.IsActive == true).Select(y => new BaseModel()
                    {
                        Id = y.SurveyId,
                        Text = y.Name
                    }).ToList();
                    return surveyDetails;
                }
            }
            catch (Exception ex)
            {
                return surveyDetails;
            }
        }
        public SurveyDeails SurveyDetails(int suveyId)
        {
            SurveyDeails surveyQuestionDetails = new SurveyDeails();
            try
            {
                using (SurveyShrikeEntities context = new SurveyShrikeEntities())
                {
                    var query = from surveyQuestionMapping in context.SurveyQuestionMappings                                                                
                                join questions in context.Questions on surveyQuestionMapping.QuestionId equals questions.QuestionId
                                join questionType in context.QuestionTypes on questions.TypeID equals questionType.QuestionTypeId
                                join options in context.Options on surveyQuestionMapping.QuestionId equals options.QuestionId
                                where surveyQuestionMapping.SurveyId == suveyId & questions.IsActive == true & options.IsActive == true
                                select new { surveyQuestionMapping.SurveyId, Question = questions, Option = options };
                    return surveyQuestionDetails;
                }
            }
            catch (Exception ex)
            {
                return surveyQuestionDetails;
            }
        }
        public bool Save(UserSurveyModel userServeyDetails)
        {
            bool status = false;
            try
            {
                using (SurveyShrikeEntities context = new SurveyShrikeEntities())
                {
                    UserSurvey userSurvey = new UserSurvey();
                    userSurvey.SurveyId = userServeyDetails.SurveyId;
                    userSurvey.SubmittedBy = userServeyDetails.SubmittedBy;
                    userSurvey.SubmittedOn = DateTime.UtcNow;
                    context.UserSurveys.Add(userSurvey);
                    context.SaveChanges();
                    if (userSurvey.SurveyId > 0)
                    {
                        foreach (var currentUserSurvey in userServeyDetails.UserValues)
                        {
                            UserSurveyDetail userSurveyDetail = new UserSurveyDetail();
                            userSurveyDetail.QuestionId = currentUserSurvey.QuestionId;
                            userSurveyDetail.SystemValue = currentUserSurvey.SystemValue;
                            userSurveyDetail.CustomValue = currentUserSurvey.CustomValue;
                            context.UserSurveyDetails.Add(userSurveyDetail);
                            context.SaveChanges();
                        }
                    }
                }
                return status;
            }
            catch (Exception ex)
            {
                return status;
            }
        }
    }
}
