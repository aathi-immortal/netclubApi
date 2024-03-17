﻿
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NetClubApi.Comman;
using NetClubApi.Helper;
using NetClubApi.Model;

namespace NetClubApi.Modules.UserModule
{
    public interface IUserDataAccess
    {
        public Task<UserModel> AuthenticateUser(UserLogin user_login);
        public Task<UserModel> RegisterUser(UserModel user);
    }
    public class UserDataAccess : IUserDataAccess
    {

        private readonly IHelper _helper;
        private readonly NetClubDbContext _netClubDbContext;
        public UserDataAccess(IHelper helper, NetClubDbContext netClubDbContext)
        {
            _helper = helper;
            _netClubDbContext = netClubDbContext;
        }
        public async Task<UserModel> AuthenticateUser(UserLogin user_login)
        {
                UserModel user = new UserModel();
            user.Message = [];
            try
            {
                user.Email = user_login.email;
                using (SqlConnection myCon = sqlHelper.GetConnection())
                {
                    myCon.Open();
                    string sql1 = $@"select * from [dbo].[User_detail] where Email='{user_login.email}'";
                    using (SqlCommand myCommand = new SqlCommand(sql1, myCon))
                    {
                        SqlDataReader reader = myCommand.ExecuteReader();
                        if (reader.HasRows)
                        {
                            user.Password = _helper.EncodeBase64(user_login.password.ToString());
                            while (reader.Read())
                            {
                                if (((string)reader["Password"]).CompareTo(user.Password) == 0)
                                {
                                    user.Id = (int)reader["Id"];
                                    user.First_name = (string)reader["first_name"];
                                    user.Last_name = (string)reader["last_name"];
                                    user.User_name = (string)reader["user_name"];
                                    user.gender = (string)reader["gender"];
                                    //user.date_of_birth = Convert.ToDateTime(reader["date_of_birth"]);
                                    user.date_of_birth = (DateTime)reader["date_of_birth"];
                                    user.Message.Add("valid password");
                                    user.IsSuccess = true;
                                }
                                else
                                {
                                    user.Message.Add("Invalid Password");
                                    user.IsSuccess = false;
                                }
                            }
                        }
                        else
                        {
                            reader.Close();
                            user.Message.Add("Invalid Userid");
                            user.IsSuccess = false;
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
            return user;
        }

        public async Task<UserModel> RegisterUser(UserModel user)
        {
            try
            {
                using (SqlConnection myCon = sqlHelper.GetConnection())
                {
                    myCon.Open();
                    string sql1 = $@"select Email,Password from [dbo].[User_detail] where Email='{user.Email}';";
                    using (SqlCommand myCommand = new SqlCommand(sql1, myCon))
                    {
                        SqlDataReader reader = myCommand.ExecuteReader();
                        if (reader.HasRows)
                        {
                            user.Message.Add("user already register");
                            user.IsSuccess = false;
                        }
                        else
                        {
                            reader.Close();
                            Console.WriteLine(user.date_of_birth);
                            user.Password = _helper.EncodeBase64(user.Password);
                            string sql2 = $@"INSERT INTO [dbo].[User_detail] (user_name, first_name, last_name, password, email,phone_number,gender,date_of_birth)
                                   VALUES ('{user.User_name}','{user.First_name}','{user.Last_name}',    '{user.Password}','{user.Email}','{user.Phone_number}','{user.gender}','{user.date_of_birth}')";

                            string insertSql = @"INSERT INTO [dbo].[User_detail] (user_name, first_name, last_name, password, email,phone_number,gender,date_of_birth)
                                   VALUES (@user_name,@first_name,@last_name,    @password,@email,@phone_number,@gender,@date_of_birth)";
                            using (SqlCommand insertCommand = new SqlCommand(insertSql, myCon))
                            {
                                // Add parameters
                                insertCommand.Parameters.AddWithValue("@user_name", user.User_name);
                                insertCommand.Parameters.AddWithValue("@first_name", user.First_name);
                                insertCommand.Parameters.AddWithValue("@last_name", user.Last_name);
                                insertCommand.Parameters.AddWithValue("@password", user.Password);
                                insertCommand.Parameters.AddWithValue("@email", user.Email); 
                                insertCommand.Parameters.AddWithValue("@phone_number", user.Phone_number);
                                insertCommand.Parameters.AddWithValue("@gender", user.gender);
                                insertCommand.Parameters.AddWithValue("@date_of_birth", user.date_of_birth);
                                insertCommand.ExecuteNonQuery();
                            }
                           //     using (SqlCommand myCommand1 = new SqlCommand(sql2, myCon))
                            //{
                              //  myCommand1.ExecuteNonQuery();
                            //}
                            user.Message.Add("registered Successfully");
                            user.IsSuccess = true;
                        }
                        myCon.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return user;
        }
    }
}
