using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.Data;

/// Experience = Job / Employment
/// 
public class ExperienceHandler
{
    private CurrentUser user;

    public ExperienceHandler(CurrentUser user)
    {
        this.user = user;
    }  

    public List<Experience> LoadExperiences(ref Database db, string cvId)
    {
        List<Experience> exp = new List<Experience>();

        var allExperiences = db.Query("SELECT * FROM CVsExperiences WHERE UserId = @0 AND CVId = @1 ORDER BY SubId", user.GetId(), Convert.ToInt32(cvId));
        
        foreach (var row in allExperiences)
        {
            string subId = row.SubId.ToString();
            string startDate = row.FromStart.Trim();
            string endDate = row.ToEnd.Trim();
            string position = row.Position.Trim();
            string company = row.Company.Trim();
            string summary = row.Summary.Trim();
                
            Experience temp = new Experience(subId, startDate, endDate, position, company, summary);
            exp.Add(temp);
        }

        return exp;
    }

    public void UpdateExperiences(ref Database db, string cvId, List<Experience> experiences)
    {   
        var alreadyInDBCount = db.QueryValue("SELECT COUNT(Id) FROM CVsExperiences WHERE UserId = @0 AND CVId = @1", user.GetId(), Convert.ToInt32(cvId));

        for (int i = 0; i < experiences.Count; i++)
        {
            string id = experiences[i].Id; 
            string startDate = Truncate(experiences[i].StartDate,100);
            string endDate = Truncate(experiences[i].EndDate,100);
            string position = Truncate(experiences[i].Position,100);
            string company = Truncate(experiences[i].Company,100);
            string summary = Truncate(experiences[i].Summary,2000);           

            if (i < alreadyInDBCount)
            {
                db.Execute("UPDATE CVsExperiences SET FromStart = @0, ToEnd = @1, Position = @2, Company = @3, Summary = @4 WHERE UserId = @5 AND CVId = @6 AND SubId = @7", startDate, endDate, position, company, summary, user.GetId(), Convert.ToInt32(cvId), Convert.ToInt32(id));
            }
            else
            {
                db.Execute("INSERT INTO CVsExperiences(UserId, CVId, SubId, FromStart, ToEnd, Position, Company, Summary) VALUES (@0,@1,@2,@3,@4,@5,@6,@7)", user.GetId(), Convert.ToInt32(cvId), Convert.ToInt32(id), startDate, endDate, position, company, summary);
            }        
        }
    }

    public List<Experience> DeleteExperiences(ref Database db, string cvId, string[] experiencesToBeDeleted)
    {
        foreach (var id in experiencesToBeDeleted)
        {
            db.Execute("DELETE FROM CVsExperiences WHERE UserId = @0 AND CVId = @1 AND SubId = @2", user.GetId(), Convert.ToInt32(cvId), Convert.ToInt32(id));
        }

        //Renumber everything
        var sqlIDs = db.Query("SELECT Id FROM CVsExperiences WHERE UserId = @0 AND CVId = @1", user.GetId(), Convert.ToInt32(cvId));
        for (int i = 0; i < sqlIDs.Count(); i++)
        {
            db.Execute("UPDATE CVsExperiences SET SubId = @0 WHERE UserId = @1 AND Id = @2", i+1, user.GetId(), sqlIDs.ElementAt(i).Id);
        }

        //Reload
        List<Experience> experiences = LoadExperiences(ref db, cvId);
        return experiences;
    }    

    private string Truncate(string str, int maxLength)
    {        
        if (str.Length > maxLength)
        {
            str = str.Substring(0, maxLength);
        }        

        return str;
    }              
}
