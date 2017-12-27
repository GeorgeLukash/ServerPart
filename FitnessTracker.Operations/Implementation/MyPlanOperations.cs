using FitnessTracker.DataAccess;
using FitnessTracker.DataAccess.Entity;
using FitnessTracker.DataModel.Exercises;
using FitnessTracker.DataModel.Plan;
using FitnessTracker.Operations.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using FitnessTracker.DataModel;
using FitnessTracker.DataModel.Enums;

namespace FitnessTracker.Operations.Implementation
{
    public class MyPlanOperations : IMyPlanOperations
    {
        private readonly IUnitOfWork _unitOfWork;

        public MyPlanOperations(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public void AddExercise(PostExerciseModel model, int currUserId)
        {
            var user = _unitOfWork.Repository<UserEntity>().GetById(currUserId);
            var block = _unitOfWork.Repository<BlockExersiceEntity>().GetById(model.BlockId);

            if (user != null)
            {
                _unitOfWork.Repository<ExerciseEntity>().Insert(new ExerciseEntity
                {
                    Amount = model.Amount,
                    Distance = model.Distance,
                    Name = model.Name,
                    Time = model.Time,
                    Weight = model.Weight,
                    CreatedAt = DateTimeOffset.Now,
                    Block = block
                });
                _unitOfWork.SaveChanges();
            }
        }

        public void AddExercise(IList<PostExerciseModel> model, int currUserId)
        {
            var user = _unitOfWork.Repository<UserEntity>().GetById(currUserId);
            var block = _unitOfWork.Repository<BlockExersiceEntity>().GetById(model[0].BlockId);

            if (user != null)
            {
                foreach (var item in model)
                {
                    _unitOfWork.Repository<ExerciseEntity>().Insert(new ExerciseEntity
                    {
                        Amount = item.Amount,
                        Distance = item.Distance,
                        Time = item.Time,
                        Weight = item.Weight,
                        CreatedAt = DateTimeOffset.Now,
                        Block = block
                    });
                }
                _unitOfWork.SaveChanges();
            }
        }

        public int CreatePlan(CreatePlanModel model, int currUserId)
        {
            var user = _unitOfWork.Repository<UserEntity>().GetById(currUserId);

            var entityToInsert = new PlanEntity
            {
                Blocks = new List<BlockExersiceEntity>(),
                Name = model.Name,
                Owner = user,
                Type = model.Type
            };

            _unitOfWork.Repository<PlanEntity>().Insert(entityToInsert);
            _unitOfWork.SaveChanges();

            return entityToInsert.Id;
        }

        public void DeleteExercise(int blockId, int exerciseId, int currUserId)
        {
            var exercise = _unitOfWork.Repository<ExerciseEntity>()
                .Include(x => x.Block, x => x.Block.Plan.Owner)
                .FirstOrDefault(x => x.Block.Plan.Owner.Id == currUserId
                  && x.Id == exerciseId && x.Block.Id == blockId);

            if (exercise != null)
            {
                _unitOfWork.Repository<ExerciseEntity>().Delete(exercise);
                _unitOfWork.SaveChanges();
            }
        }

        public ICollection<MyPlanModel> GetPlans(int currUserId)
        {
            return _unitOfWork.Repository<PlanEntity>()
                .Include(x => x.Owner)
                .Where(x => x.Owner.Id == currUserId)
                .Select(x => new MyPlanModel
                {
                    Name = x.Name,
                    Id = x.Id
                }).ToList();
        }

        public ICollection<RecommendedPlanModel> GetRecommends(string query)
        {
            return _unitOfWork.Repository<PlanEntity>()
                 .Include(x => x.Owner)
                 .Where(x => x.Owner.Role == (int)UserType.Coach && (query == null || x.Type.StartsWith(query)))
                 .Select(x => new RecommendedPlanModel
                 {
                     Firstname = x.Owner.Firstname,
                     Id = x.Id,
                     Image = x.Owner.Image,
                     Lastname = x.Owner.Lastname,
                     OwnerId = x.Owner.Id,
                     Name = x.Name,
                     Type = x.Type,
                     Followers = x.Followers.Count
                 }).OrderByDescending(x => x.Followers).ToList();
        }

        public void ApplyToRecommend(int id, int userId)
        {
            var plan = _unitOfWork.Repository<PlanEntity>().GetById(id);
            var user = _unitOfWork.Repository<UserEntity>().GetById(userId);

            plan.Followers.Add(user);
            _unitOfWork.SaveChanges();
        }

        public ICollection<MyPlanModel> GetMyRecommendedPlans(int userId)
        {
            return _unitOfWork.Repository<UserEntity>().Include(x => x.Followed).FirstOrDefault(x => x.Id == userId)
                .Followed.Select(x => new MyPlanModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Type = x.Type
                }).ToList();
        }

        public void UpdateExercise(UpdateExerciseModel model, int currUserId)
        {
            var exercise = _unitOfWork.Repository<ExerciseEntity>().GetById(model.Id);

            if (exercise != null)
            {
                exercise.Amount = model.Amount;
                exercise.Distance = model.Distance;
                exercise.Time = model.Time;
                exercise.Weight = model.Weight;
                _unitOfWork.SaveChanges();
            }
        }

        public void Unfollow(int id, int currUserId)
        {
            var user = _unitOfWork.Repository<UserEntity>().Include(x => x.Followed).FirstOrDefault(x => x.Id == currUserId);

            user.Followed.Remove(user.Followed.FirstOrDefault(x => x.Id == id));
            _unitOfWork.SaveChanges();
        }
    }
}