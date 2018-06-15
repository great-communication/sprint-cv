    function styleCV(theme, cv) {
        switch (theme) {
            case 'Default':
                return defaultStyle(cv);                    
            case 'Corporate':
                return corporate(cv);                                    
        }
    }   
        

    function defaultStyle(cv) { 
        //Markup
        var output = "" +
                "<div id='cv-container' style='padding:30px 70px; margin:20px 0; border-radius: 1px;'>" +            
                    "<h3 style='font-weight: bold; color: #222222; font-size: 28px;'>" + cv.firstName + "<br />" +
                    "" + cv.lastName + "</h3>" +
                    "<div>" +  cv.email + "</div>" +
                    "<hr>" +
                    "<div style='overflow: hidden;'>" +
                    "<div style='display: inline-block; white-space: pre;'><span style='font-weight: bold;'>Address: </span><br />" + cv.address + "</div>" +
                    "<div style='float: right; text-align: right; padding: 10px; display: inline-block;'>"+
                        "<div><span style='font-weight: bold;'>Website:</span> " + cv.website + "</div>" +
                        "<div><span style='font-weight: bold;'>Phone:</span> " + cv.phoneNumber + "</div>" +
                    "</div>" +                        
                "</div>" +
                    "<hr>" +
                    "<div>" +
                    "<div style='display: inline-block; width: 40%; vertical-align: top;'>" +
                        "<h4 style='color:#222222; display:inline;'>Bio</h4>" + 
                    "</div>" +
                        "<div style='display: inline-block; width: 60%; vertical-align: top;'>" +
                        "<div>" + cv.summary + "</div>" +
                        "</div>" +                     
                    "</div>" +
                "<hr>" +
                "<div style='display: inline-block; width: 40%; vertical-align: top;'>" +
                    "<h4 style='color:#222222; display: inline;'>Experience</h4>" + 
                "</div>" + 
                "<div style='display: inline-block; width: 60%; vertical-align: top;'>";

        for (let i = 0; i < cv.experiences.length; i++)
        {
            output += "" +
                "<div style='font-weight: bold; display: inline-block; width: auto; vertical-align: top;'>" + cv.experiences[i].position + " at " + cv.experiences[i].company + "</div>" +
                "<div style='font-weight: bold; display: inline-block; float: right; vertical-align: top;'>" + cv.experiences[i].startDate + " – " + cv.experiences[i].endDate + "</div>" +
                "<div style='margin-bottom: 20px;' >" + cv.experiences[i].summary + "</div>";
        }   

        output += "</div>" +
            "<hr>" +
            "<div style='display: inline-block; width: 40%; vertical-align: top;'>" +
                "<h4 style='color:#222222; display: inline;'>Education</h4>" +
            "</div>" +
            "<div style='display: inline-block; width: 60%; vertical-align: top;'>";     

        for (let i = 0; i < cv.educations.length; i++) {
            output += "" +
                "<div style='font-weight: bold; display: inline-block; width: auto; vertical-align: top;'>" + cv.educations[i].position + " at " + cv.educations[i].company + "</div>" +
                "<div style='font-weight: bold; display: inline-block; float: right; vertical-align: top;'>" + cv.educations[i].startDate + " – " + cv.educations[i].endDate + "</div>" +
                "<div style='margin-bottom: 20px;'>" + cv.educations[i].summary + "</div>";
        }   

        output += "</div>" +
            "</div>";

        return output;  
    }


    function corporate(cv) {
        //Markup
        var output = "<div id='cv-container' style='padding:30px 70px; margin:20px 0; border-radius: 1px;'>" +
                     "<div id='content'>" +
                        "<h1 style='text-transform: uppercase; font-size: 24px;'>Curriculum vitae</h1>" +                
                        "<h2 style='text-transform: uppercase; font-size: 16px;'>Personal details</h2>" +
                        "<table id='personal-details' style='width:80%;'>" +
                            "<tr><td style='width:30%; vertical-align:top;'><div style='font-weight: bold;'>Name:</div></td><td style='width:70%;'><div>" + cv.firstName + " " + cv.lastName+"</div></td></tr>" +
                            "<tr><td style='width:30%; vertical-align:top;'><div style='font-weight: bold;'>Email:</div></td><td style='width:70%;'><div>" + cv.email+"</div></td></tr>" +
                            "" +
                            "<tr><td style='width:30%; vertical-align:top;'><div style='font-weight: bold;'>Phone number:</div></td><td style='width:70%;'><div>" + cv.phoneNumber+"</div></td></tr>" +
                            "<tr><td style='width:30%; vertical-align:top;'><div style='font-weight: bold;'>Email:</div></td><td style='width:70%;'><div>" + cv.website+"</div></td></tr>" +
                                "" +
                            "<tr><td style='width:30%; vertical-align:top;'><div style='font-weight: bold;'>Adress:</div></td><td style='width:70%;'><div>" + cv.address +"</div></td></tr>" +
                        "</table>" +
                        "" +
                        "<h2 style='text-transform: uppercase; font-size: 16px;'>Summary</h2>" +
                        "<table style='width:80%;'>" +
                            "<tr><td style='width:30%; vertical-align:top;'><div></div></td><td style='width:70%;'><div>" + cv.summary +"</div></td></tr>" +
                        "</table>" +
                        ""+
                        "<h2 style='text-transform: uppercase; font-size: 16px;'>Education</h2>" +
                            "<table style='width:80%;'>";

        for (let i = 0; i < cv.educations.length; i++) {
            output += "<tr><td style='width:30%; vertical-align:top;'><div>" + cv.educations[i].startDate + " - " + cv.educations[i].endDate+"</div></td>" +
                "<td style='width:70%;'><div>" + cv.educations[i].position+"</div>" +
                "<div>" + cv.educations[i].company + "</div><div>" + cv.educations[i].summary+"</div></td></tr>";               
        }

        output +="</table>" +
                "" +
                "<h2 style='text-transform: uppercase; font-size: 16px;'>Experience</h2>" +
                    "<table style='width:80%;'>";

        for (let i = 0; i < cv.experiences.length; i++) {
            output += "<tr><td style='width:30%; vertical-align:top;'><div>" + cv.experiences[i].startDate + "-" + cv.experiences[i].endDate+"</div></td>" +
                "<td style='width:70%;'><div>" + cv.experiences[i].position+"</div>" +
                "<div>" + cv.experiences[i].company + "</div><div>" + cv.experiences[i].summary+"</div></td></tr>";
        }

        output += "</table>" +
            "</div>" +
            "</div>";    

        return output;
    }