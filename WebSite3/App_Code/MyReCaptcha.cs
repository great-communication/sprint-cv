using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// Invisible ReCaptcha Server Side Validation

public class MyReCaptcha
{
    public bool Success { get; set; }    

    public MyReCaptcha()
    {        
                     
    }

    public static bool Validate(string encodedResponse)
    {
        if (string.IsNullOrEmpty(encodedResponse))
            return false;

        var secret = "6LdHc10UAAAAAEcZOhcaG6td_fLwwmfTdNxybp_6";
        var client = new System.Net.WebClient();
        var googleReply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, encodedResponse));
        var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        var reCaptcha = serializer.Deserialize<MyReCaptcha>(googleReply);

        return reCaptcha.Success;
    }   
}