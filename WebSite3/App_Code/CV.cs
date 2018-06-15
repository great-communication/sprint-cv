using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.Data;
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for CV
/// </summary>
public class CV
{
    private CurrentUser user;
    private Database db;
    private ExperienceHandler experienceHandler;
    private EducationHandler educationHandler;    
    public string Id { get; set; }
    public string DocumentName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Website { get; set; }
    public string Address { get; set; }
    public string Summary { get; set; }
    public string Theme { get; set; }
    public bool Complete { get; set; }    
    public List<Experience> experiences;
    public List<Education> educations;       

    public CV(CurrentUser user)
    {
        this.user = user;
        experienceHandler = new ExperienceHandler(user);
        educationHandler = new EducationHandler(user);
        db = Database.Open("StarterSite");        
        Id = FirstName = LastName = Email = PhoneNumber = Website = Address = Summary = "";
        Theme = "Default";
        experiences = new List<Experience>();
        educations = new List<Education>();
        DocumentName = "(Untitled)"; 
        Complete = false;
    }  

    public void CreateNewCV()
    { 
        //CreateNewTables();  //Only done once. Move this to somewhere else...              

        db.Execute("INSERT INTO CVs(UserId, DocumentName, Created, FirstName, LastName, Email, PhoneNumber, Website, Address, Summary, Theme, Complete) VALUES (@0,@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11)", user.GetId(), DocumentName, DateTime.Now, FirstName, LastName, Email, PhoneNumber, Website, Address, Summary, Theme, Complete);
        Id = db.QueryValue("SELECT TOP 1 Id FROM CVs WHERE UserId = @0 ORDER BY Id DESC", user.GetId()).ToString();  
    }

    public void LoadCV(string id)
    {          
        //Load basic data
        var query = db.QuerySingle("SELECT * FROM CVs WHERE UserId = @0 AND Id = @1", user.GetId(), Convert.ToInt32(id));
        Id = query.id.ToString();
        DocumentName = query.DocumentName.Trim();
        FirstName = query.FirstName.Trim();
        LastName = query.LastName.Trim();
        Email = query.Email.Trim();
        PhoneNumber = query.PhoneNumber.Trim();
        Website = query.Website.Trim();
        Address = query.Address.Trim();
        Summary = query.Summary.Trim();
        Theme = query.Theme.Trim();
        Complete = query.Complete;

        //Load experiences        
        experiences = experienceHandler.LoadExperiences(ref db, id);        

        //Load educations        
        educations = educationHandler.LoadEducations(ref db, id);        
    }

    public void UpdateCV(string id, string documentName, string firstName, string lastName, string email, string phoneNumber, string website, string address, string summary, string theme, string complete, List<Experience> experiences, List<Education> educations)
    {
        //Update CV   
        Id = id;
        DocumentName = Truncate(documentName, 20);
        FirstName = Truncate(firstName, 100);
        LastName = Truncate(lastName, 100);
        Email = Truncate(email, 100);
        PhoneNumber = Truncate(phoneNumber, 100);
        Website = Truncate(website, 100);
        Address = Truncate(address, 1000);
        Summary = Truncate(summary, 2000);
        Theme = Truncate(theme, 100);
        Complete = Convert.ToBoolean(complete);

        db.Execute("UPDATE CVs SET DocumentName = @0, FirstName = @1, LastName = @2, Email = @3, PhoneNumber = @4, Website = @5, Address = @6, Summary = @7, Theme = @8, Complete = @9 WHERE UserId = @10 AND Id = @11", DocumentName, FirstName, LastName, Email, PhoneNumber, Website, Address, Summary, Theme, Complete, user.GetId(), Convert.ToInt32(Id));

        //Update experiences        
        this.experiences = experiences;
        experienceHandler.UpdateExperiences(ref db, id, experiences);        

        //Update educations  
        this.educations = educations;
        educationHandler.UpdateEducations(ref db, id, educations);        
    }

    public void DeleteCV(string id)
    {        
        db.Execute("DELETE FROM CVs WHERE UserId = @0 AND Id = @1", user.GetId(), Convert.ToInt32(id));
        db.Execute("DELETE FROM CVsEducations WHERE UserId = @0 AND CVId = @1", user.GetId(), Convert.ToInt32(id));
        db.Execute("DELETE FROM CVsExperiences WHERE UserId = @0 AND CVId = @1", user.GetId(), Convert.ToInt32(id));
    }

    public void DeleteExperiences(string id, string[] experiencesToBeDeleted)
    {
        experiences = experienceHandler.DeleteExperiences(ref db, id, experiencesToBeDeleted);        
    }

    public void DeleteEducations(string id, string[] educationsToBeDeleted)
    {
        educations = educationHandler.DeleteEducations(ref db, id, educationsToBeDeleted);        
    }

    private void CreateNewTables()
    {
        db.Execute("CREATE TABLE CVs(Id int PRIMARY KEY IDENTITY(1,1) NOT NULL, UserId int NOT NULL, DocumentName nchar(100) NOT NULL, Created datetime NOT NULL, FirstName nchar(100) NOT NULL, LastName nchar(100) NOT NULL, Email nchar(100) NOT NULL, PhoneNumber nchar(100) NOT NULL, Website nchar(100) NOT NULL, Address ntext NOT NULL, Summary ntext NOT NULL, Theme nchar(100) NOT NULL, Complete bit NOT NULL);");
        db.Execute("CREATE TABLE CVsExperiences(Id int PRIMARY KEY IDENTITY(1,1) NOT NULL, UserId int NOT NULL, CVId int NOT NULL, SubId int NOT NULL, FromStart nchar(100) NOT NULL, ToEnd nchar(100) NOT NULL, Position nchar(100) NOT NULL, Company nchar(100) NOT NULL, Summary ntext NOT NULL);");
        db.Execute("CREATE TABLE CVsEducations(Id int PRIMARY KEY IDENTITY(1,1) NOT NULL, UserId int NOT NULL, CVId int NOT NULL, SubId int NOT NULL, FromStart nchar(100) NOT NULL, ToEnd nchar(100) NOT NULL, Degree nchar(100) NOT NULL, School nchar(100) NOT NULL, Summary ntext NOT NULL);");
    }

    private string Truncate(string str, int maxLength)
    {        
        if (str.Length > maxLength)
        {
            str = str.Substring(0, maxLength);
        }        

        return str;
    }    

    public string ReplaceLineBreaks(string s)
    {      
        //Replaces line breaks with <br>
        return Regex.Replace(s, @"\r\n?|\n", "<br>");        
    }    
}