using Microsoft.AspNet.Identity;
using SurveyShrike.Contracts;
using SurveyShrike.Models;
using SurveyShrike.Services;
using SurveyShrike.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SurveyShrike.Controllers.Api
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class SurveyController : ApiController
    {
        /// <summary>
        /// Add Survey only Admin Can Access
        /// </summary>
        /// <param name="newSurvey"></param>
        /// <returns></returns>
        [AuthorizeRoles(Helper.ADMIN_ROLE)]
        [HttpPost]
        public IHttpActionResult AddSurvey(SurveyDeails newSurvey)
        {
            newSurvey.CreatedBy = RequestContext.Principal.Identity.GetUserId();
            ISurvey service = new SurveyService();
            bool status = service.AddSurvey(newSurvey);
            if (status)
                return Json(new { Status = true, Message = "Added Successfully" });
            else
                return Json(new { Status = false, Message = "Adding survey has problem" });
        }

        /// <summary>
        /// Get Survey - Admin, User Can Access
        /// </summary>
        /// <returns></returns>
        [AuthorizeRoles(Helper.ADMIN_ROLE, Helper.USER_ROLE)]
        [HttpGet]
        public IHttpActionResult Surveys()
        {
            ISurvey service = new SurveyService();
            List<BaseModel> surveyDeails = service.LoadSurvey();
            if (surveyDeails.Count > 0)
                return Json(new { Status = true, data = surveyDeails, Message = "" });
            else
                return Json(new { Status = false, data = "", Message = "Loading surveys has problem" });
        }

        /// <summary>
        ///  Get Survey Questions based on SurveyId - Admin, User Can Access
        /// </summary>
        /// <param name="surveyId"></param>
        /// <returns></returns>
        [AuthorizeRoles(Helper.ADMIN_ROLE, Helper.USER_ROLE)]
        [HttpGet]
        public IHttpActionResult SurveyDetails(int surveyId)
        {
            ISurvey service = new SurveyService();
            SurveyDeails surveyDeails = service.SurveyDetails(surveyId);
            if (surveyDeails.Id != 0)
                return Json(new { Status = true, data = surveyDeails, Message = "" });
            else
                return Json(new { Status = true, data = "", Message = "Loading survey details has problem" });

        }


        /// <summary>
        /// Save user survey details
        /// </summary>
        /// <param name="userSurvey"></param>
        /// <returns></returns>
        [AuthorizeRoles(Helper.ADMIN_ROLE, Helper.USER_ROLE)]
        [HttpPost]
        public IHttpActionResult Save(UserSurveyModel userSurvey)
        {
            userSurvey.SubmittedBy = RequestContext.Principal.Identity.GetUserId();
            ISurvey service = new SurveyService();
            bool status = service.Save(userSurvey);
            if (status)
                return Json(new { Status = true, Message = "Added Successfully" });
            else
                return Json(new { Status = false, Message = "Adding user survey has problem" });
        }
    }
}
