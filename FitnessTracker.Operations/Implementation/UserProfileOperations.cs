using FitnessTracker.DataAccess;
using FitnessTracker.DataAccess.Entity;
using FitnessTracker.DataModel;
using FitnessTracker.Operations.Abstraction;
using System;
using System.IO;
using System.Linq;

namespace FitnessTracker.Operations.Implementation
{
    public class UserProfileOperations : IUserProfileOperations
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserProfileOperations(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public UserProfileModel GetProfile(int currUserId)
        {
            var user = _unitOfWork.Repository<UserEntity>().Include(x => x.Profile).FirstOrDefault(x => x.Id == currUserId);

            return new UserProfileModel
            {
                Age = user.Profile.Age,
                Sex = user.Profile.Sex,
                Height = user.Profile.Height,
                Weight = user.Profile.Weight,
                Firstname = user.Firstname,
                Lastname = user.Lastname,                
                Image = user.Image,
                Email = user.Email
            };
        }

        public void UpdateProfile(UserProfileModel model, int currUserId)
        {
            var user = _unitOfWork.Repository<UserProfileEntity>().GetById(currUserId);

            user.Age = model.Age;
            user.Height = model.Height;
            user.Weight = model.Weight;
            _unitOfWork.SaveChanges();
        }

        public void UpdateProfileImage(PostImageModel model, int currUserId)
        {
            var bytes = Convert.FromBase64String(model.Base64);
            var filename = $"{Guid.NewGuid().ToString()}.png";

            var exists = Directory.Exists(model.Path);
            if (!exists)
            {
                Directory.CreateDirectory(model.Path);
            }
            using (var imageFile = new FileStream(model.Path + "/" + filename, FileMode.Create))
            {
                imageFile.Write(bytes, 0, bytes.Length);
                imageFile.Flush();
            }

            _unitOfWork.Repository<UserEntity>().GetById(currUserId).Image = $"{model.ImageUrl}/{filename}";
            _unitOfWork.SaveChanges();
        }
    }
}