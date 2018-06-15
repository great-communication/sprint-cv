$(function () {
    $('#accordion-education').accordion({
        heightStyle: 'content',
        collapsible: true,
        active: accordionIndexEducation,
        activate: function (event, ui) {
            var currentAccordionIndex = $("#accordion-education").accordion("option", "active");
            $('#education-accordion-index').val(currentAccordionIndex);
        }
    });
    $('#add-accordion-education').click(function () {
        if (accordionSectionCountEducation < 50) {
            addNewEducationSection();
        } else {
            alert("Can not add more than 50 experiences.");
        }
    });

    for (let i = 0; i < accordionSectionCountEducation; i++) {
        addScriptForNewEducationSection(i + 1);
    }
});

function addNewEducationSection() {
    accordionSectionCountEducation++;
    idCountEducation++;
    $('#education-count').val(accordionSectionCountEducation);
    var newSection = getNewEducationSection(idCountEducation, accordionSectionCountEducation);
    $('#accordion-education').append(newSection);
    addScriptForNewEducationSection(idCountEducation);
    $('#accordion-education').accordion('refresh');
    $('.datepicker').datepicker({
        dateFormat: 'yy-mm-dd',
        changeMonth: true,
        changeYear: true
    });
    $('#save-reminder-education').html('Not saved!');
}

function deleteEducationSection(id) {
    $('#headline-education-' + id).remove();
    $('#section-education-' + id).remove();
    var toBeDeleted = $('#education-to-be-deleted').val();
    $('#education-to-be-deleted').val(toBeDeleted + " " + id);
    $('#save-reminder-education').html('Not saved!');
    //Rename all education elements with higher id
    var idPrefixes = ["start-date-education-", "end-date-education-", "position-education-", "company-education-", "education-education-"];

    var nameCount = 1;
    for (let i = 1; i <= idCountEducation; i++) {
        if ($('#headline-position-education-' + i).length) { //If id number exists
            for (j = 0; j < idPrefixes.length; j++) {
                $('#' + idPrefixes[j] + i).attr("name", idPrefixes[j] + (nameCount));
            }
            nameCount++;
        }
    }
    accordionSectionCountEducation--;
    $('#education-count').val(accordionSectionCountEducation);
}

function getNewEducationSection(id, name) {
    var newSection = "<h3 id='headline-education-" + id + "'>" +
        "<span id='headline-position-education-" + id + "' class='same-row'>Degree/Course</span><span> – </span>" +
        "<span id='headline-company-education-" + id + "' class='same-row'>Institution</span>" +
        "<span id='delete-education-" + id + "' class='ui-icon ui-icon-circle-close float-right same-row'></span>" +
        "<span id='move-down-education-" + id + "' class='ui-icon ui-icon-circle-arrow-s float-right same-row'></span>" +
        "<span id='move-up-education-" + id + "' class='ui-icon ui-icon-circle-arrow-n float-right same-row'></span>" +
        "</h3>" +
        "<div id='section-education-" + id + "'>" +
        "<table>" +
        "<tbody>" +
        "<tr><td><label> From:</label></td><td><input id='start-date-education-" + id + "' class='datepicker' type='text' name='start-date-education-" + name + "' value='' /></td></tr>" +
        "<tr><td><label> To:</label></td><td><input id='end-date-education-" + id + "' class='datepicker' type='text' name='end-date-education-" + name + "' value='' /></td></tr>" +
        "<tr><td><label> Degree/Course:</label></td><td><input id='position-education-" + id + "' type='text' name='position-education-" + name + "' value='' /></td></tr>" +
        "<tr><td><label> Institution:</label></td><td><input id='company-education-" + id + "' type='text' name='company-education-" + name + "' value='' /></td></tr>" +
        "<tr><td><label> Description: </label></td><td><textarea id='education-education-" + id + "' class='textarea-small' rows='3' cols='5' name='education-education-" + name + "'></textarea></td></tr>" +
        " </tbody>" +
        "</table>" +
        "</div>";
    return newSection;
}

function addScriptForNewEducationSection(id) {
    var script = "";

    //Change warning and
    //copy 'positon' and 'company' to headline
    var idPrefixesForHeadlines = ["position-education-", "company-education-"];

    for (let i = 0; i < idPrefixesForHeadlines.length; i++) {
        script += "" +
            "$('#" + idPrefixesForHeadlines[i] + id + "').on('change paste keyup', function () {" +
            "var text = $('#" + idPrefixesForHeadlines[i] + id + "').val();" +
            "$('#headline-" + idPrefixesForHeadlines[i] + id + "').text(text);" +
            "$('#save-reminder-education').html('Not saved!');" +
            "});";
    }

    var idPrefixes = ["start-date-education-", "end-date-education-", "education-education-"];

    for (let i = 0; i < idPrefixes.length; i++) {
        script += "" +
            "$('#" + idPrefixes[i] + id + "').on('change paste keyup', function () {" +
            "$('#save-reminder-education').html('Not saved!');" +
            "});";
    }

    //Client side validation
    script += "" +
        "$('#start-date-education-" + id + "').attr('maxlength', '100');" +
        "$('#end-date-education-" + id + "').attr('maxlength', '100');" +
        "$('#position-education-" + id + "').attr('maxlength', '100');" +
        "$('#company-education-" + id + "').attr('maxlength', '100');" +
        "$('#education-education-" + id + "').attr('maxlength', '2000');";

    //Delete button
    script += "" +
        "$('#delete-education-" + id + "').click(function(event) {" +
        "var position = $('#position-education-" + id + "').val();" +
        "var company = $('#company-education-" + id + "').val();" +
        "if (confirm('Are you sure you want to delete this education ' + position +' ' + company +'?')) {" +
        "deleteEducationSection(" + id + ");" +
        "}" +
        "event.stopPropagation();" +
        "});";

    //Move down button
    script += "" +
        "$('#move-down-education-" + id + "').click(function(event) {" +
        "moveEducation('" + id + "'," + true + ");" +
        "event.stopPropagation();" +
        "});";

    //Move up button
    script += "" +
        "$('#move-up-education-" + id + "').click(function(event) {" +
        "moveEducation('" + id + "'," + false + ");" +
        "event.stopPropagation();" +
        "});";

    $('<script>')
        .attr('type', 'text/javascript')
        .text(script)
        .appendTo('head');
}

function moveEducation(id, isMovingDown) {
    var nextId = findNextEducationId(id, isMovingDown);

    if (nextId != -1) {
        var idPrefixesForElements = ["headline-position-education-", "headline-company-education-", "education-education-"];
        var idPrefixesForInputs = ["start-date-education-", "end-date-education-", "position-education-", "company-education-"];
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

        $('#save-reminder-education').html('Not saved!');
    }
}

function findNextEducationId(id, isMovingDown) {

    var returnValue = -1; //-1 not found
    var intId = parseInt(id);

    if (isMovingDown) {
        for (let i = intId + 1; i <= idCountEducation; i++) {
            if ($('#headline-education-' + i).length) { //Check if id exists
                returnValue = i;
                break;
            }
        }
    } else {
        for (let i = id - 1; i > 0; i--) {
            if ($('#headline-education-' + i).length) {
                returnValue = i;
                break;
            }
        }
    }

    return returnValue;
}