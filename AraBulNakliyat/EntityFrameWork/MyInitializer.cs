using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AraBulNakliyat.DataAccessLayer.EntityFrameWork;
using AraBulNakliyat.Entities;

namespace AraBulNakliyat.DataAccessLayer.EntityFrameWork
{
   public class MyInitializer : CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            // Adding admin user
            AraBulUser admin = new AraBulUser()
            {
                Name = "Nebi",
                Surname = "Kiramali",
                Email = "nebi@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = true,
                UserName = "nebikiramanli",
                ProfileImageFilename = "user_boy.png",
                Password = "123456",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUserName = "nebikiramanli"
            };
            // Adding standart user
            AraBulUser standartUser = new AraBulUser()
            {
                Name = "furkan",
                Surname = "odeyen",
                Email = "odeyen@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false,
                UserName = "furkanodeyen",
                ProfileImageFilename = "user_boy.png",
                Password = "654321",
                CreatedOn = DateTime.Now.AddHours(1),
                ModifiedOn = DateTime.Now.AddMinutes(65),
                ModifiedUserName = "nebikiramanli"
            };
            context.AraBulUsers.Add(admin);
            context.AraBulUsers.Add(standartUser);

            for (int i = 0; i < 8; i++)
            {
                AraBulUser user = new AraBulUser()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    ProfileImageFilename = "user_boy.png",
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = true,
                    IsAdmin = false,
                    UserName = $"user{i}",
                    Password = "123",
                    CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedUserName = $"user{i}"
                };
                context.AraBulUsers.Add(user);
            }


            context.SaveChanges();
            // user list for usıng

            List<AraBulUser> usersList = context.AraBulUsers.ToList();
            //adding fake Categories
            for (int i = 0; i < 10; i++)
            {
                City category = new City()
                {
                    Title = FakeData.PlaceData.GetStreetName(),
                    Description = FakeData.PlaceData.GetAddress(),
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ModifiedUserName = "nebikiramanli"
                };
                context.Cities.Add(category);

                //Adding fake Notices
                for (int j = 0; j < FakeData.NumberData.GetNumber(5,9); j++)
                {
                    AraBulUser owner = usersList[FakeData.NumberData.GetNumber(0, usersList.Count - 1)];

                    Notice notice = new Notice()
                    {
                        Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 25)),
                        Text = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
                        City = category,
                        IsDraft = false,

                        LikeCount = FakeData.NumberData.GetNumber(1, 9),

                        Owner = owner,
                        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),

                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1),DateTime.Now),

                        ModifiedUserName = owner.UserName
                    };
                    category.Notices.Add(notice);


                    // Adding Fake Comments 
                    for (int k = 0; k <FakeData.NumberData.GetNumber(3,5); k++)
                    {
                        AraBulUser comment_owner = usersList[FakeData.NumberData.GetNumber(0, usersList.Count - 1)];
                        Comment comment = new Comment()
                        {
                            Text = FakeData.TextData.GetSentence(),
                            Notice = notice,
                            Owner = comment_owner,

                            CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),

                            ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),

                            ModifiedUserName = comment_owner.UserName
                        };
                        notice.Comments.Add(comment);
                    }


                    //Adding Fake Like
                  
                    for (int m = 0; m < notice.LikeCount; m++)
                    {
                        Liked liked = new Liked()
                        {
                            Notice = notice,
                            LikedUser = usersList[m]
                        };
                        notice.Likes.Add(liked);
                    }
                }

            }

            context.SaveChanges();
        }


    }
}
