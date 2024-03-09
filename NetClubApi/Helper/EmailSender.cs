using MailKit.Net.Smtp;

using MimeKit;


public interface IEmailSender
{
    public Task<string> SendEmailAsync(string email, String url);

}

public class EmailSender : IEmailSender
{

    
    
    public async Task<string> SendEmailAsync(string email, String url)
    {
        


        var senderEmail = "aathisnr123@gmail.com";
        var password = "ynne zgez irkp jguw";

        try
        {
            // Load the HTML template
            string htmlTemplate = EmailTemplates.ProfessionalEmailTemplate;

            htmlTemplate =  htmlTemplate.Replace("url_placeholder", url);

            



            // Replace placeholders with actual values

            // Embedding Images in C# Code
            //var mapImagePart = new MimePart("image", "png")
            //{
            //    ContentDisposition = new ContentDisposition(ContentDisposition.Inline),
            //    ContentTransferEncoding = ContentEncoding.Base64,
            //};

            //var carImagePart = new MimePart("image", "png")
            //{
            //    ContentDisposition = new ContentDisposition(ContentDisposition.Inline),
            //    ContentTransferEncoding = ContentEncoding.Base64,
            //};


            //byte[] mapImageBytes;
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    await emailModel.formFile.CopyToAsync(ms);
            //    mapImageBytes = ms.ToArray();
            //}
            //mapImagePart.Content = new MimeContent(new MemoryStream(mapImageBytes));
            //mapImagePart.ContentId = "MapImage";




            //htmlTemplate = htmlTemplate

            //            .Replace("src=\"cid:MapImage\"", $"src=\"cid:{mapImagePart.ContentId}\"");


            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("Your Company", senderEmail));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = "vanakkam";


            var multipart = new Multipart("mixed");
            multipart.Add(new TextPart("html") { Text = htmlTemplate });
            //multipart.Add(mapImagePart);
            //multipart.Add(carImagePart);

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
