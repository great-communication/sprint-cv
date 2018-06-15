using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Education
/// </summary>
public class Education
{
    public string Id { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public string Degree { get; set; }
    public string School { get; set; }
    public string Summary { get; set; }

    public Education(string id, string startDate, string endDate, string degree, string school, string summary)
    {
        Id = id;
        StartDate = startDate;
        EndDate = endDate;
        Degree = degree;
        School = school;
        Summary = summary;
    }
}