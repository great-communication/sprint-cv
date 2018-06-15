using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.WebData;
using WebMatrix.Data;

/// <summary>
/// Summary description for User
/// </summary>
public class CurrentUser
{
    private int id;
    private int maxCVs;
    private int maxCertificates;

    public CurrentUser()
    {
        id = WebSecurity.CurrentUserId;
        maxCVs = 5;
        maxCertificates = 5;
    }    

    public int GetId()
    {
        return id;
    }

    public string GetCVTableContent()
    {
        var tableContent = "";
        
        if (HasCV())
        {  
            var db = Database.Open("StarterSite");
            var allCVs = db.Query("SELECT Id, DocumentName, Created FROM CVs WHERE UserId = @0 ORDER BY Created ASC", id);


            foreach (var row in allCVs)
            {
                var Id = row.Id;
                var DocumentName = row.DocumentName.Trim();                
                var Created = row.Created.ToString("yyyy-MM-dd hh:mm");                

                tableContent += "" + 
                    "<tr>" +
                        "<td><a id=\"name-cv-"+Id+"\" class=\"download\" href=\"#\">"+DocumentName+"</a></td>" +
                        "<td>"+Created+"</td>" +
                        "<td><span id=\"edit-cv-" + Id + "\" class=\"edit document-options link\">Edit</span><span id=\"delete-cv-" + Id+"\" class=\"delete document-options link\">Delete</span></td>" +
                        "<td></td>" +
                    "</tr>";
            }            
        }
        else
        {
            tableContent = "" +
            "<tr>" +
                "<td> You currently don't have any CVs.</td><td></td>" +
            "</tr>";
        }
        
        return tableContent;
    }

    public string GetCertificateTableContent()
    {
        var tableContent = "";

        if (HasCertificate())
        {
            var db = Database.Open("StarterSite");
            var allCertificates = db.Query("SELECT Id, Type, Created, ScoreQ1, ScoreQ2, ScoreQ3, ScoreQ4, ScoreQ5, ScoreQ6, ScoreQ7, ScoreQ8, ScoreQ9, ScoreQ10 FROM Certificates WHERE UserId = @0 ORDER BY Created ASC", id);


            foreach (var row in allCertificates)
            {
                var Id = row.Id;
                var Type = row.Type.Trim();
                var Created = row.Created.ToString("yyyy-MM-dd hh:mm");
                var bonusMultiplier = 1;
                var maxScore = 100;
                if(Type == "WebDevCertificate")
                {
                    bonusMultiplier = 10;
                }

                var totalScore = row.ScoreQ1 + row.ScoreQ2 + row.ScoreQ3 + row.ScoreQ4 + row.ScoreQ5 + row.ScoreQ6 + row.ScoreQ7 + row.ScoreQ8 + (row.ScoreQ9*bonusMultiplier) + row.ScoreQ10;
                if (totalScore > maxScore)
                {
                    totalScore = maxScore;
                }

                tableContent += "" +
                    "<tr>" +
                        "<td><a id=\"name-certificate-" + Id + "\" class=\"download\" href=\"#\">" + Type + "</a></td>" +
                        "<td>" + Created + "</td>" +
                        "<td>" + totalScore + "</td>" +
                        "<td><span id=\"delete-certificate-" + Id + "\" class=\"delete document-options link\">Delete</span></td>" +
                        
                    "</tr>";
            }
        }
        else
        {
            tableContent = "" +
            "<tr>" +
                "<td> You currently don't have any Certificates.</td><td></td>" +
            "</tr>";
        }

        return tableContent;
    }

    public bool DoesTableExist(string tableName)
    {        
        var db = Database.Open("StarterSite");
        var count = db.QueryValue("SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '"+tableName+"'");
        var hasTable = count.Equals(1);
        return hasTable;
    }    

    public bool HasCV()
    {
        var hasCV = false;

        if (DoesTableExist("CVs"))
        {
            var db = Database.Open("StarterSite");
            var count = db.QueryValue("SELECT COUNT(Id) FROM CVs WHERE UserId = @0", id);
            if (count > 0)
            {
                hasCV = true;
            }
        }
        
        return hasCV;        
    }

    public bool HasTooManyCVs()
    {
        var hasTooManyCVs = false;

        if (DoesTableExist("CVs")) {
            var db = Database.Open("StarterSite");
            var count = db.QueryValue("SELECT COUNT(Id) FROM CVs WHERE UserId = @0", id);
            if (count >= maxCVs)
            {
                hasTooManyCVs = true;
            }
        }
       
        return hasTooManyCVs;        
    }

    public bool HasCertificate()
    {
        var hasCertificate = false;

        if (DoesTableExist("Certificates"))
        {
            var db = Database.Open("StarterSite");
            var count = db.QueryValue("SELECT COUNT(Id) FROM Certificates WHERE UserId = @0", id);
            if (count > 0)
            {
                hasCertificate = true;
            }
        }

        return hasCertificate;
    }

    public bool HasTooManyCertificates()
    {
        var hasTooManyCertificates = false;

        if (DoesTableExist("Certificates"))
        {
            var db = Database.Open("StarterSite");
            var count = db.QueryValue("SELECT COUNT(Id) FROM Certificates WHERE UserId = @0", id);
            if (count >= maxCertificates)
            {
                hasTooManyCertificates = true;
            }
        }

        return hasTooManyCertificates;
    }
}

  