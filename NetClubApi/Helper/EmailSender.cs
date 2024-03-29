using MailKit.Net.Smtp;

using MimeKit;
using NetClubApi.Helper;


public interface IEmailSender
{
<<<<<<< HEAD
    public Task<string> SendEmailAsync(string email, String url, string subject);
    public Task<String> ClubInvitation(string email,string url );
    public Task<String> LeagueInvitation(string email,string url );
=======
    public Task<string> SendEmailAsync(string email, String url,string subject,string httmlTemplate);
    public Task<String> ClubInvitation(string email, String url);
    public Task<String> LeagueInvitation(string email, String url);
>>>>>>> 8ff77041da275b91087241d51571b3ec7a5ab60a
}

public class EmailSender : IEmailSender
{
    public async Task<string> ClubInvitation(string email, string url)
    {
<<<<<<< HEAD
        return await SendEmailAsync(email, url, "clubInvitation");
=======
        return await SendEmailAsync(email, url,"clubInvitation", ClubInvitationEmail.ProfessionalEmailTemplate);
>>>>>>> 8ff77041da275b91087241d51571b3ec7a5ab60a
    }

    public async Task<string> LeagueInvitation(string email, string url)
    {
<<<<<<< HEAD
        return await SendEmailAsync(email, url,"leagueInvitation");
    }

    public async Task<string> SendEmailAsync(string email, String url,string subject)
=======
        return await SendEmailAsync(email, url, "leagueInvitation", LeagueInvitationEmail.ProfessionalEmailTemplate);
    }

    public async Task<string> SendEmailAsync(string email, String url,string subject,string htmlTemplate)
>>>>>>> 8ff77041da275b91087241d51571b3ec7a5ab60a
    {
        


        var senderEmail = "aathisnr123@gmail.com";
        var password = "ynne zgez irkp jguw";

        try
        {
            // Load the HTML template
            

            htmlTemplate =  htmlTemplate.Replace("url_placeholder", url);



            
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("Your Company", senderEmail));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = subject;


            var multipart = new Multipart("mixed");
            multipart.Add(new TextPart("html") { Text = htmlTemplate });
            

            // Set the multipart as the body of the message
            message.Body = multipart;

            // Send the email using MailKit
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Connect("smtp.gmail.com", 465, true);
                smtpClient.Authenticate(senderEmail, password);
                await smtpClient.SendAsync(message);
                smtpClient.Disconnect(true);
            }

            return "Email sent successfully";
        }
        catch (Exception ex)
        {
            return $"Error sending email: {ex}";
            throw;
        }
    }

    //private string getTravelTime(Trip tripDetail)
    //{
    //    TimeSpan difference = (tripDetail.endTime.Value - tripDetail.startTime.Value);
    //    int totalMinutes = (int)difference.TotalMinutes;
    //    int hours = totalMinutes / 60;
    //    int remainingMinutes = totalMinutes % 60;
    //    String result = remainingMinutes + " min";
    //    if (hours == 0)
    //        return result;
    //    return hours + " hr : " + result;

    //}

    //private string getTheNoon(int hour)
    //{
    //    if (hour >= 5 && hour < 12)
    //    {
    //        return "Morning";
    //    }
    //    else if (hour >= 12 && hour < 17)
    //    {
    //        return "Afternoon";
    //    }
    //    else if (hour >= 17 && hour < 20)
    //    {
    //        return "Evening";
    //    }
    //    else
    //    {
    //        return "Night";
    //    }
    //}



    //private Cab getCab(int? cabId)
    //{

    //    return _meterproDbContext.Cab.FirstOrDefault(cab => cab.id == cabId);

    //}

    //private User_detail? getCabDriver(int? userId)
    //{
    //    try
    //    {

    //        var user = _meterproDbContext.User_detail.FirstOrDefault(user => user.id == userId);

    //        return user;
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}


}
