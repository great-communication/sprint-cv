$(function () {
    $('#accordion-experience').accordion({
        heightStyle: 'content',
        collapsible: true,
        active: accordionIndexExperience,
        activate: function (event, ui) {
            var currentAccordionIndex = $("#accordion-experience").accordion("option", "active");
            $('#experience-accordion-index').val(currentAccordionIndex);
        }
    });
    $('#add-accordion-experience').click(function () {
        if (accordionSectionCountExperience < 50) {
            addNewExperienceSection();
        } else {
            alert("Can not add more than 50 experiences.");
        }
    });

    for (let i = 0; i < accordionSectionCountExperience; i++) {
        addScriptForNewExperienceSection(i + 1);
    }
});

function addNewExperienceSection() {
    accordionSectionCountExperience++;
    idCountExperience++;
    $('#experience-count').val(accordionSectionCountExperience);
    var newSection = getNewExperienceSection(idCountExperience, accordionSectionCountExperience);
    $('#accordion-experience').append(newSection);
    addScriptForNewExperienceSection(idCountExperience);
    $('#accordion-experience').accordion('refresh');
    $('.datepicker').datepicker({
        dateFormat: 'yy-mm-dd',
        changeMonth: true,
        changeYear: true
    });
    $('#save-reminder-experience').html('Not saved!');
}

function deleteExperienceSection(id) {
    $('#headline-experience-' + id).remove();
    $('#section-experience-' + id).remove();
    var toBeDeleted = $('#experience-to-be-deleted').val();
    $('#experience-to-be-deleted').val(toBeDeleted + " " + id);
    $('#save-reminder-experience').html('Not saved!');
    //Rename all experience elements with higher id
    var idPrefixes = ["start-date-experience-", "end-date-experience-", "position-experience-", "company-experience-", "experience-experience-"];

    var nameCount = 1;
    for (let i = 1; i <= idCountExperience; i++) {
        if ($('#headline-position-experience-' + i).length) { //If id number exists
            for (j = 0; j < idPrefixes.length; j++) {
                $('#' + idPrefixes[j] + i).attr("name", idPrefixes[j] + (nameCount));
            }
            nameCount++;
        }
    }
    accordionSectionCountExperience--;
    $('#experience-count').val(accordionSectionCountExperience);
}

function getNewExperienceSection(id, name) {
    var newSection = "<h3 id='headline-experience-" + id + "'>" +
        "<span id='headline-position-experience-" + id + "' class='same-row'>Position</span><span> – </span>" +
        "<span id='headline-company-experience-" + id + "' class='same-row'>Company</span>" +
        "<span id='delete-experience-" + id + "' class='ui-icon ui-icon-circle-close float-right same-row'></span>" +
        "<span id='move-down-experience-" + id + "' class='ui-icon ui-icon-circle-arrow-s float-right same-row'></span>" +
        "<span id='move-up-experience-" + id + "' class='ui-icon ui-icon-circle-arrow-n float-right same-row'></span>" +
        "</h3>" +
        "<div id='section-experience-" + id + "'>" +
        "<table>" +
        "<tbody>" +
        "<tr><td><label> From:</label></td><td><input id='start-date-experience-" + id + "' class='datepicker' type='text' name='start-date-experience-" + name + "' value='' /></td></tr>" +
        "<tr><td><label> To:</label></td><td><input id='end-date-experience-" + id + "' class='datepicker' type='text' name='end-date-experience-" + name + "' value='' /></td></tr>" +
        "<tr><td><label> Position:</label></td><td><input id='position-experience-" + id + "' type='text' name='position-experience-" + name + "' value='' /></td></tr>" +
        "<tr><td><label> Company:</label></td><td><input id='company-experience-" + id + "' type='text' name='company-experience-" + name + "' value='' /></td></tr>" +
        "<tr><td><label> Description: </label></td><td><textarea id='experience-experience-" + id + "' class='textarea-small' rows='3' cols='5' name='experience-experience-" + name + "'></textarea></td></tr>" +
        " </tbody>" +
        "</table>" +
        "</div>";
    return newSection;
}

function addScriptForNewExperienceSection(id) {
    var script = "";

    //Change warning and
    //copy 'positon' and 'company' to headline
    var idPrefixesForHeadlines = ["position-experience-", "company-experience-"];

    for (let i = 0; i < idPrefixesForHeadlines.length; i++) {
        script += "" +
            "$('#" + idPrefixesForHeadlines[i] + id + "').on('change paste keyup', function () {" +
            "var text = $('#" + idPrefixesForHeadlines[i] + id + "').val();" +
            "$('#headline-" + idPrefixesForHeadlines[i] + id + "').text(text);" +
            "$('#save-reminder-experience').html('Not saved!');" +
            "});";
    }

    var idPrefixes = ["start-date-experience-", "end-date-experience-", "experience-experience-"];

    for (let i = 0; i < idPrefixes.length; i++) {
        script += "" +
            "$('#" + idPrefixes[i] + id + "').on('change paste keyup', function () {" +
            "$('#save-reminder-experience').html('Not saved!');" +
            "});";
    }

    //Client side validation
    script += "" +
        "$('#start-date-experience-" + id + "').attr('maxlength', '100');" +
        "$('#end-date-experience-" + id + "').attr('maxlength', '100');" +
        "$('#position-experience-" + id + "').attr('maxlength', '100');" +
        "$('#company-experience-" + id + "').attr('maxlength', '100');" +
        "$('#experience-experience-" + id + "').attr('maxlength', '2000');";

    //Delete button
    script += "" +
        "$('#delete-experience-" + id + "').click(function(event) {" +
        "var position = $('#position-experience-" + id + "').val();" +
        "var company = $('#company-experience-" + id + "').val();" +
        "if (confirm('Are you sure you want to delete this experience ' + position +' ' + company +'?')) {" +
        "deleteExperienceSection(" + id + ");" +
        "}" +
        "event.stopPropagation();" +
        "});";

    //Move down button
    script += "" +
        "$('#move-down-experience-" + id + "').click(function(event) {" +
        "moveExperience('" + id + "'," + true + ");" +
        "event.stopPropagation();" +
        "});";

    //Move up button
    script += "" +
        "$('#move-up-experience-" + id + "').click(function(event) {" +
        "moveExperience('" + id + "'," + false + ");" +
        "event.stopPropagation();" +
        "});";

    $('<script>')
        .attr('type', 'text/javascript')
        .text(script)
        .appendTo('head');
}

function moveExperience(id, isMovingDown) {
    var nextId = findNextExperienceId(id, isMovingDown);

    if (nextId != -1) {
        var idPrefixesForElements = ["headline-position-experience-", "headline-company-experience-", "experience-experience-"];
        var idPrefixesForInputs = ["start-date-experience-", "end-date-experience-", "position-experience-", "company-experience-"];
        var idData = ["", "", "", "", "", "", ""];
        var nextIdData = ["", "", "", "", "", "", ""];

        //Copy data
        for (let i = 0; i < idPrefixesForElements.length; i++) {
            idData[i] = $('#' + idPrefixesForElements[i] + id).html(); 
            nextIdData[i] = $('#' + idPrefixesForElements[i] + nextId).html();
        }
        for (let i = 0; i < idPrefixesForInputs.length; i++) {
            var arrIndex = i + idPrefixesForElements.length;
            idData[arrIndex] = $('#' + idPrefixesForInputs[i] + id).val();
            nextIdData[arrIndex] = $('#' + idPrefixesForInputs[i] + nextId).val();
        }        

        //Paste data
        for (let i = 0; i < idPrefixesForElements.length; i++) {
            $('#' + idPrefixesForElements[i] + id).html(nextIdData[i]);
            $('#' + idPrefixesForElements[i] + nextId).html(idData[i]);            
        }
        for (let i = 0; i < idPrefixesForInputs.length; i++) {
            var arrIndex = i + idPrefixesForElements.length;
            $('#' + idPrefixesForInputs[i] + id).val(nextIdData[arrIndex]);
            $('#' + idPrefixesForInputs[i] + nextId).val(idData[arrIndex]);
        }

        $('#save-reminder-experience').html('Not saved!');
    }
}

function findNextExperienceId(id, isMovingDown) {

    var returnValue = -1; //-1 not found
    var intId = parseInt(id);

    if (isMovingDown) {
        for (let i = intId + 1; i <= idCountExperience; i++) {
            if ($('#headline-experience-' + i).length) { //Check if id exists
                returnValue = i;
                break;
            }
        }
    } else {
        for (let i = id - 1; i > 0; i--) {
            if ($('#headline-experience-' + i).length) {
                returnValue = i;
                break;
            }
        }
    }

    return returnValue;
}