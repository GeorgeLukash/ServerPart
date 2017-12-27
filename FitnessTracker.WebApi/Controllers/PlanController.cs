using FitnessTracker.DataModel.Exercises;
using FitnessTracker.DataModel.Plan;
using FitnessTracker.Operations.Abstraction;
using FitnessTracker.WebApi.Filters;
using FitnessTracker.WebApi.Providers.Abstraction;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace FitnessTracker.WebApi.Controllers
{
    [RoutePrefix("user")]
    public class PlanController : ApiControllerBase
    {
        private readonly IMyPlanOperations _planOperations;

        public PlanController(IMyPlanOperations planOper, ICurrentUserProvider currentUserProvider) : base(currentUserProvider)
        {
            _planOperations = planOper;
        }

        [Route("recommends")]
        [AuthorizeIfTokenValid]
        [HttpGet]
        public IHttpActionResult GetRecommendPlans(string query)
        {
            try
            {
                return Ok(_planOperations.GetRecommends(query));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
                throw;
            }
        }

        [Route("apply/{id:int}")]
        [AuthorizeIfTokenValid]
        [HttpGet]
        public IHttpActionResult Apply(int id)
        {
            try
            {
                _planOperations.ApplyToRecommend(id, _currentUserProvider.CurrentUserId);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
                throw;
            }
        }

        [Route("unfollow/{id:int}")]
        [AuthorizeIfTokenValid]
        [HttpGet]
        public IHttpActionResult Unfollow(int id)
        {
            try
            {
                _planOperations.Unfollow(id, _currentUserProvider.CurrentUserId);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
                throw;
            }
        }

        [Route("my_recommended")]
        [AuthorizeIfTokenValid]
        [HttpGet]
        public IHttpActionResult MyRecommends()
        {
            try
            {
                return Ok(_planOperations.GetMyRecommendedPlans(_currentUserProvider.CurrentUserId));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
                throw;
            }
        }

        [Route("plans")]
        [AuthorizeIfTokenValid]
        [HttpGet]
        public IHttpActionResult GetPlans()
        {
            try
            {
                return Ok(_planOperations.GetPlans(_currentUserProvider.CurrentUserId));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
                throw;
            }
        }

        [Route("plan")]
        [AuthorizeIfTokenValid]
        [HttpPost]
        public IHttpActionResult AddPlan(CreatePlanModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _planOperations.CreatePlan(model, _currentUserProvider.CurrentUserId);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
                throw;
            }
        }

        [Route("exercise")]
        [AuthorizeIfTokenValid]
        [HttpPost]
        public IHttpActionResult AddExercise(PostExerciseModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _planOperations.AddExercise(model, _currentUserProvider.CurrentUserId);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
                throw;
            }
        }

        [Route("exercises")]
        [AuthorizeIfTokenValid]
        [HttpPost]
        public IHttpActionResult AddExercise(IList<PostExerciseModel> model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _planOperations.AddExercise(model, _currentUserProvider.CurrentUserId);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
                throw;
            }
        }

        [Route("exercise")]
        [AuthorizeIfTokenValid]
        [HttpPut]
        public IHttpActionResult UpdateExercise(UpdateExerciseModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _planOperations.UpdateExercise(model, _currentUserProvider.CurrentUserId);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
                throw;
            }
        }

        [Route("exercise/{plan:int}/{id:int}")]
        [HttpDelete]
        [AuthorizeIfTokenValid]
        public IHttpActionResult DeleteExercise(int plan, int id)
        {
            try
            {
                _planOperations.DeleteExercise(plan, id, _currentUserProvider.CurrentUserId);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
                throw;
            }
        }
    }
}