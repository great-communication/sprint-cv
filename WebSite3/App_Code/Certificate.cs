using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.Data;
using System.Globalization;

/// <summary>
/// Summary description for Certificate
/// </summary>
public class Certificate
{
    private CurrentUser user;
    private Database db;
    public string Id { get; set; }
    public string Type { get; set; }
    public DateTime Created { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool Complete { get; set; }    
    public List<int> scores;
    public int TotalScore { get; set; }

    public Certificate(CurrentUser user)
    {
        this.user = user;
        db = Database.Open("StarterSite");
        Id = Type = FirstName = LastName = "";
        TotalScore = 0;
        Complete = false;
        scores = new List<int>();
    }

    public void CreateNew(string type, List<int> scores)
    {
        //CreateNewTables();  //Only done once. Move this to somewhere else...              

        if (!user.HasTooManyCertificates()) {
            Type = type;
            this.scores = scores;
            TotalScore = CalculateTotalScore();
            db.Execute("INSERT INTO Certificates(UserId, Type, Created, FirstName, LastName, Complete, ScoreQ1, ScoreQ2, ScoreQ3, ScoreQ4, ScoreQ5, ScoreQ6, ScoreQ7, ScoreQ8, ScoreQ9, ScoreQ10) VALUES (@0,@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@13,@14,@15)", user.GetId(), type, DateTime.Now, FirstName, LastName, Complete, scores[0], scores[1], scores[2], scores[3], scores[4], scores[5], scores[6], scores[7], scores[8], scores[9]);
            Id = db.QueryValue("SELECT TOP 1 Id FROM Certificates WHERE UserId = @0 ORDER BY Id DESC", user.GetId()).ToString();
        }
    }

    public void Load(string id)
    {
        //Load basic data
        var query = db.QuerySingle("SELECT * FROM Certificates WHERE UserId = @0 AND Id = @1", user.GetId(), Convert.ToInt32(id));
        Id = query.id.ToString();
        Type = query.Type.Trim();
        Created = query.Created;
        FirstName = query.FirstName.Trim();
        LastName = query.LastName.Trim();
        Complete = query.Complete;
        scores.Add(query.ScoreQ1);
        scores.Add(query.ScoreQ2);
        scores.Add(query.ScoreQ3);
        scores.Add(query.ScoreQ4);
        scores.Add(query.ScoreQ5);
        scores.Add(query.ScoreQ6);
        scores.Add(query.ScoreQ7);
        scores.Add(query.ScoreQ8);
        scores.Add(query.ScoreQ9);
        scores.Add(query.ScoreQ10);
        TotalScore = CalculateTotalScore();
    }

    public void Update(string id, string firstName, string lastName, bool complete)
    {
        if(id != "") {
            FirstName = Truncate(firstName, 100);
            LastName = Truncate(lastName, 100);
            Complete = complete;
            db.Execute("UPDATE Certificates SET FirstName = @0, LastName = @1, Complete = @2 WHERE UserId = @3 AND Id = @4", FirstName, LastName, Complete, user.GetId(), Convert.ToInt32(id));
        }
    }

    public void Delete(string id)
    {
        db.Execute("DELETE FROM Certificates WHERE UserId = @0 AND Id = @1", user.GetId(), Convert.ToInt32(id));       
    }   

    private void CreateNewTables()
    {
        db.Execute("CREATE TABLE Certificates(Id int PRIMARY KEY IDENTITY(1,1) NOT NULL, UserId int NOT NULL, Type nchar(100), Created datetime NOT NULL, FirstName nchar(100) NOT NULL, LastName nchar(100) NOT NULL, Complete bit NOT NULL, ScoreQ1 int NOT NULL, ScoreQ2 int NOT NULL, ScoreQ3 int NOT NULL, ScoreQ4 int NOT NULL, ScoreQ5 int NOT NULL, ScoreQ6 int NOT NULL, ScoreQ7 int NOT NULL, ScoreQ8 int NOT NULL, ScoreQ9 int NOT NULL, ScoreQ10 int NOT NULL);");
        
    }

    private string Truncate(string str, int maxLength)
    {
        if (str.Length > maxLength)
        {
            str = str.Substring(0, maxLength);
        }

        return str;
    }

    private int CalculateTotalScore()
    {
        int bonusMultiplier = 1;
        if(Type == "WebDevCertificate")
        {
            bonusMultiplier = 10;
        }

        int tot = 0;
        for (int i = 0; i < scores.Count; i++)
        {
            if (i == 8)
            {
                tot += scores[i]* bonusMultiplier;
            }
            else
            {
                tot += scores[i];
            }            
        }

        return tot;
    }

    public string GetMonthFromDate()
    {
        CultureInfo ci = new CultureInfo("en-US");
        string month = Created.ToString("MMMM", ci).ToUpper();
        return month;
    }
}