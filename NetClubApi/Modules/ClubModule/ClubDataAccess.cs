using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NetClubApi.Helper;
using NetClubApi.Model;
using NetClubApi.Model.ResponseModel;
using System.Data.SqlTypes;

namespace NetClubApi.Modules.ClubModule
{

    public interface IClubDataAccess
    {
        public Task<List<ClubRegistration>> getCreatedClub(int id);
        public Task<Club> getClubDetails(int club_id);

        public Task<string> CreateClub(Club club, int id);
        public Task<List<ClubRegistration>> getRegisteredClub(int id);
        public Task<string> ClubRegistration(Club code, int user_id);
        public Task<List<RegisterClubModel>> getRegisteredClubModel(int user_id);
        public Task<string> getClubLabel(int club_id);
    }

    public class ClubDataAccess : IClubDataAccess
    {
        private readonly NetClubDbContext _netClubDbContext;

        public ClubDataAccess(NetClubDbContext netClubDbContext)
        {
            _netClubDbContext = netClubDbContext;


        }

        //return the list of clubs created by the user
        public async Task<List<ClubRegistration>> getCreatedClub(int id)
        {
            List<ClubRegistration> clubs = new();
            try
            {

                //get the list of clubs created by the user using user id and role
                clubs = await _netClubDbContext.club_registration.Where(clubs => clubs.user_id == id && clubs.isadmin).ToListAsync();


                return clubs;



            }
            catch (Exception)
            {
                throw;
            }


        }

        public async Task<Club> getClubDetails(int club_id)
        {
            try
            {
                Console.WriteLine(club_id + 1);
                // Retrieve a single club entity with the specified club_id
                var clubs = await _netClubDbContext.club.FirstOrDefaultAsync(user => 
                          user.Id == club_id
                    );
                Console.WriteLine(clubs);
                return clubs;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> CreateClub(Club club, int userId)
        {

            try
            {
                club.created_date = DateTime.Now;
                club.club_label = await GenerateUniqueLabel();


                var registerclub = await _netClubDbContext.club.AddAsync
(club);
                await _netClubDbContext.SaveChangesAsync();
                ClubRegistration clubRegistration = new();

                clubRegistration.user_id = userId;
                clubRegistration.club_id = registerclub.Entity.Id;

                clubRegistration.isadmin = true;
                await _netClubDbContext.AddAsync(clubRegistration);
                await _netClubDbContext.SaveChangesAsync();



                return "Club created Successfully";
            }
            catch (Exception)
            {
                throw;
            }


        }
        public async Task<List<RegisterClubModel>> getRegisteredClubModel(int user_id) 
        {
            List<RegisterClubModel> registerClub= new List<RegisterClubModel>();
            try
            {
                using (SqlConnection myCon = sqlHelper.GetConnection())
                {
                    myCon.Open();
                    string sql2 = $@"
select [dbo].[club].id,[dbo].[club].club_name,[dbo].[club].created_by,[dbo].[club_registration].join_date,[dbo].[club].club_label from [dbo].[club_registration] inner join [dbo].[club] on [dbo].[club_registration].club_id=[dbo].[club].id where [dbo].[club_registration].user_id={user_id} and [dbo].[club_registration].isadmin=0";
                    using (SqlCommand myCommand = new SqlCommand(sql2, myCon))
                    {
                        SqlDataReader reader = myCommand.ExecuteReader();
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {
                                RegisterClubModel club = new RegisterClubModel
                                 {
                                     id = (int)reader["id"],
                                     club_name = (string)reader["club_name"],
                                     created_by = (string)reader["created_by"],
                                     join_date= $"{(DateTime)reader["join_date"]}",
                                    club_label = (string)reader["club_label"]
                                 };
                                registerClub.Add(club);
                            }
                        }
                        else
                        {
                            reader.Close();
                        }
                        myCon.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            return registerClub;
        }

        public async Task<List<ClubRegistration>> getRegisteredClub(int id)
        {
            List<ClubRegistration> clubs = new();
            try
            {


                clubs = await _netClubDbContext.club_registration.Where(clubs => clubs.user_id == id && !clubs.isadmin).ToListAsync();


                return clubs;



            }
            catch (Exception)
            {
                throw;
            }


        }

        public async Task<string> ClubRegistration(Club code, int user_id)
        {
            try
            {
                int club_id1 = 0;
                using (SqlConnection myCon = sqlHelper.GetConnection())
                {
                    myCon.Open();
                    string sql2 = $@"select [dbo].[club].Id from [dbo].[club] where [dbo].[club].club_label={code}";
                    using (SqlCommand myCommand = new SqlCommand(sql2, myCon))
                    {
                        SqlDataReader reader = myCommand.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                club_id1 = (int)reader["Id"];
                                string sql3 = $@"select [dbo].[club_registration].club_id,[dbo].[club_registration].user_id from [dbo].[club_registration] where [dbo].[club_registration].club_id={club_id1} and [dbo].[club_registration].user_id={user_id}";
                                using (SqlCommand myCommand1 =new SqlCommand(sql3, myCon))
                                {
                                    SqlDataReader reader1 = myCommand1.ExecuteReader();
                                    if (reader1.HasRows)
                                    {
                                        return "already register in this club";
                                    }
                                    else
                                    {
                                        string insertSql = $@"insert into [dbo].[club_registration](user_id,club_id,isadmin,join_date)  values ('{user_id}','{club_id1}','{0}','{DateTime.Now}')";
                                        return "Club registered";
                                    }
                                }
                            }
                        }
                        else
                        {
                            return "club not found";
                            reader.Close();
                        }
                        myCon.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            /*
            var club = await _netClubDbContext.club.FirstOrDefaultAsync(club => club.club_label == code.club_label);

            if (club == default)
                return "club not found";
            var club_id = club.Id;
            if (await IsAlreadyRegister(club_id, user_id))
                return "already register in this club";
            else
            {
                ClubRegistration clubRegistration = new();
                clubRegistration.user_id = user_id;
                clubRegistration.club_id = club_id;

                clubRegistration.isadmin = false;
                //clubRegistration.league_played = 0;

                clubRegistration.join_date = DateTime.Now;
                await _netClubDbContext.club_registration.AddAsync(clubRegistration);
                await _netClubDbContext.SaveChangesAsync();
                return "you registered to the club";
            }*/
            return "Club registered";
        }

        private async Task<bool> IsAlreadyRegister(int club_id, int user_id)
        {
            var club = await _netClubDbContext.club_registration.FirstOrDefaultAsync(club => club.Id == club_id && club.user_id == user_id);
            if (club == default)
                return false;
            return true;
        }

        private async Task<string> GenerateUniqueLabel()
        {
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            int labelLength = 6;
            Random random = new Random();
            string randomString;
            do
            {
                randomString = new string(Enumerable.Range(0, labelLength)
                    .Select(_ => characters[random.Next(characters.Length)])
                    .ToArray());
            } while (await IsStringExistsInDatabase(randomString));

            return randomString;
        }

        private async Task<bool> IsStringExistsInDatabase(string randomString)
        {
            var club = await _netClubDbContext.club.FirstOrDefaultAsync(club => club.club_label == randomString);
            if (club != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<string> getClubLabel(int club_id)
        {
            var club = await _netClubDbContext.club.FirstOrDefaultAsync(club => club.Id== club_id);
           return club.club_label;
        }
    }
}
