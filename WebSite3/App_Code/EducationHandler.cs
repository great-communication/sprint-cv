using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.Data;

/// Education
/// 
public class EducationHandler
{
    private CurrentUser user;    

    public EducationHandler(CurrentUser user)
    {
        this.user = user;        
    }

    public List<Education> LoadEducations(ref Database db, string cvId)
    {
        List<Education> edu = new List<Education>();

        var allEducations = db.Query("SELECT * FROM CVsEducations WHERE UserId = @0 AND CVId = @1 ORDER BY SubId", user.GetId(), Convert.ToInt32(cvId));
        
        foreach (var row in allEducations)
        {
            string subId = row.SubId.ToString();
            string startDate = row.FromStart.Trim();
            string endDate = row.ToEnd.Trim();
            string degree = row.Degree.Trim();
            string school = row.School.Trim();
            string summary = row.Summary.Trim();
            
            //Properly done...            
            Education temp = new Education(subId, startDate, endDate, degree, school, summary);
            edu.Add(temp);
        }

        return edu;
    }

    public void UpdateEducations(ref Database db, string cvId, List<Education> educations)
    {        
        var alreadyInDBCount = db.QueryValue("SELECT COUNT(Id) FROM CVsEducations WHERE CVId = @0", Convert.ToInt32(cvId));

        for (int i = 0; i < educations.Count; i++)
        {
            string id = educations[i].Id;
            string startDate = Truncate(educations[i].StartDate, 100);
            string endDate = Truncate(educations[i].EndDate, 100);
            string degree = Truncate(educations[i].Degree, 100);
            string school = Truncate(educations[i].School, 100);
            string summary = Truncate(educations[i].Summary, 2000);          

            if (i < alreadyInDBCount)
            {
                db.Execute("UPDATE CVsEducations SET FromStart = @0, ToEnd = @1, Degree = @2, School = @3, Summary = @4 WHERE UserId = @5 AND CVId = @6 AND SubId = @7", startDate, endDate, degree, school, summary, user.GetId(), Convert.ToInt32(cvId), Convert.ToInt32(id));
            }
            else
            {
                db.Execute("INSERT INTO CVsEducations(UserId, CVId, SubId, FromStart, ToEnd, Degree, School, Summary) VALUES (@0,@1,@2,@3,@4,@5,@6,@7)", user.GetId(), Convert.ToInt32(cvId), Convert.ToInt32(id), startDate, endDate, degree, school, summary);
            }            
        }
    }

    public List<Education> DeleteEducations(ref Database db, string cvId, string[] educationsToBeDeleted)
    {
        foreach (var id in educationsToBeDeleted)
        {
            db.Execute("DELETE FROM CVsEducations WHERE UserId = @0 AND CVId = @1 AND SubId = @2", user.GetId(), Convert.ToInt32(cvId), Convert.ToInt32(id));
        }

        //Renumber everything
        var sqlIDs = db.Query("SELECT Id FROM CVsEducations WHERE UserId = @0 AND CVId = @1", user.GetId(), Convert.ToInt32(cvId));
        for (int i = 0; i < sqlIDs.Count(); i++)
        {
            db.Execute("UPDATE CVsEducations SET SubId = @0 WHERE UserId = @1 AND Id = @2", i + 1, user.GetId(), sqlIDs.ElementAt(i).Id);
        }

        //Reload
        List<Education> educations = LoadEducations(ref db, cvId);
        return educations;
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