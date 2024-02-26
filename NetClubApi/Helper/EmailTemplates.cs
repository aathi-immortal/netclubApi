


    public static class EmailTemplates
    {
        public const string ProfessionalEmailTemplate = @"
      <!DOCTYPE html>
<html lang=""en"">

<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>MeterPro</title>

    <style>
        body {
            background-color: #add8e6;
            color: #000;
            font-family: Arial, sans-serif;
             margin: 0;
            padding: 0;
        }

        .container {

            max-width: 600px;
            margin: auto;
            padding: 20px;
            background-color: #fff;
            color: #000;
            border-radius: 10px;
        }

        .header {
            text-align: center;
            background-color: #bfd9f5;
            padding:0.1em 0;
        }

        .header img {
            width: 100px;
        }

        .header .date {
            color: #bbb;
            font-size: 14px;
        }

       

        .fare {
            text-align: center;
            font-size: 48px;
            font-weight: bold;
            margin-bottom: 30px;
        }
        
        .fare-breakdown p {
            width:500px !important;    
            display: flex;
            justify-content: space-between;
            margin-bottom: 10px;
        }

        .driver-info {
            text-align: center;
            margin-bottom: 30px;
        }

        .driver-info h3 {
            margin-bottom: 10px;
        }

        .driver-info p {
            margin-bottom: 20px;
        }

        .driver-info a {
            display: inline-block;
            color: #fff;
            background-color: #007bff;
            padding: 10px 20px;
            border-radius: 5px;
            text-decoration: none;
        }

        .map img {
            width: 100%;
            border-radius: 10px;
        }

        .footer-links {
            display: flex;
            justify-content: space-between;
            font-size: 14px;
            margin-top: 30px;
        }

        .footer-links a {
            color: #bbb;
            text-decoration: none;
        }

        /* New section */
        .new-section {
            background-color: #add8e6;
            /* Light blue background color */
            padding: 20px;
            margin-top: 20px;
            border-radius: 10px;
        }

        .thankBox{
            max-width: 400px;
            padding-left: 1em;
            margin: auto;
        }
        .thanks-message .thanks{
            font-size: 2em;
            text-align: start;
            font-weight: 300;
            padding-top: 0.3em;
        }
        .thanks-message p{
            font-size: 1.2em;
            word-wrap: break-word;
            text-align: start;
        }
        .thanks-message{
            background-color: #bfd9f5;
            height: fit-content;
        }
        .carBG{
            max-width: 85%;
            text-align: start;
        }
        .carBG img{
            max-width: fit-content;
            width: 100%;
        }


        .receipt {
            width: 100%;
            max-width: 400px;
            margin: auto;
        }

        .receiptHeader {
            font-weight: bold;
            margin-bottom: 20px;
        }

        .journey-details .time {
            font-weight: bold;
            margin-top: 10px;
        }

        hr {
            border-top-color: black;
        }

        .line {
            height: 50px;
            border-left: 2px solid black;
            margin: 10px 0;
        }

        .stop {
            text-align: center;
        }

        .receipt {
            display: flex;
            flex-direction: column;
            align-items: center;
        }

        .journey-details {
            position: relative;
            padding-left: 63px;
        }

        .journey-details::before {
            content: """";
            position: absolute;
            top: 10%;
            bottom: 44%;
            left: 14.5px;
            width: 1px;
            background-color: black;
        }

        .time {
            position: relative;
            margin-left: -35px;
        }

        .time::before {
            content: """";
            position: absolute;
            left: -18px;
            top: 50%;
            width: 10px;
            height: 10px;
            background-color: black;
            border-radius: 10%;
            transform: translateY(-50%);
        }

        .map-image {
            text-align: center;

        }
        .map-image  img{
            width: 90%;   
        }
       table {
        border-top: 1px solid #444;
        border-bottom: 1px solid #444;
        width: 100%;
        border-collapse: collapse;
        }

    td {
      padding: 8px;
    }

    td:first-child {
      width: 50%;
      font-size: 1.2em;
      font-weight: 400;
    }
    td:last-child {
      width: 50%;
      text-align: right;
    }
    </style>

    
</head>

<body>
    <div class=""container"">
        <div class=""header"">

            <h2>MeterPro</h2>



        </div>

        <div class=""thanks-message"">
            <div class=""thankBox"">
                <p class=""thanks"">Thanks for riding, {cabDriverName}</p>
                <div class=""carBG"">
                    <p>We hope you enjoyed your ride this {noon}.</p>
                    <img src=""https://firebasestorage.googleapis.com/v0/b/website-325dd.appspot.com/o/Car.png?alt=media&token=7a101dec-b55c-4364-bf66-3fe081df0478"" alt=""carImage"">
                </div>
            </div>
        </div>

        <p class=""fare"">${totalCharge}</p>
        <table>
            <tr>
                <td>Trip charge</td>
                <td>${tripCharge}</td>
            </tr>
            <tr>
                <td>SubTotal</td>
                <td>${subTotal}</td>
            </tr>
            <tr>
                <td>Rounding off</td>
                <td>$0.00</td>
            </tr>
            <tr>
                <td>Booking fee</td>
                <td>$0.00</td>
            </tr>
            <tr>
                <td>Promotion</td>
                <td>$0.0</td>
            </tr>
        </table>
        <div class=""driver-info"">
            <h3>You rode with {cabDriverName}</h3>
            <p>Cab Number: {cabNumber}</p>
            <a href=""#"">Rate or tip</a>
        </div>

        <div class=""receipt"">
            <div class=""receiptHeader"">MeterPro cab | {distanceTraveled} Miles |  {travelTime}</div>
            <div class=""journey-details"">
                <div class=""time"">{startTime}</div>
                <address>
                    {fromAddress}
                </address>
                <div class=""time"">{endTime}</div>
                <address>
                    {toAddress}
                </address>
            </div>
        </div>
        <div class=""map-image"">
            <img src=""cid:MapImage"" alt=""Map Image"">
        </div>
    </div>
</body>

</html>

    ";
    }
