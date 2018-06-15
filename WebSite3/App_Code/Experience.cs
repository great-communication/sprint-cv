using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Experience
/// </summary>
public class Experience
{
    public string Id { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public string Position { get; set; }
    public string Company { get; set; }
    public string Summary { get; set; }

    public Experience(string id, string startDate, string endDate, string position, string company, string experience)
    {
        Id = id;
        StartDate = startDate;
        EndDate = endDate;
        Position = position;
        Company = company;
        Summary = experience;
    }
}