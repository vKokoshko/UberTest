var Mode = { CREATE: 1, EDIT: 2 };
var entries = JSON.parse(modelCalendarEntriesJson.replace(/(&quot\;)/g, "\""));
var selectedTimes = [];

$(document).ready(function () {
    $('#btnAddCourse').on('click', { mode: Mode.CREATE }, OpenModalForm);
    var listItemsCourse = $('.listItemCourse');
    listItemsCourse.on('click', { mode: Mode.EDIT }, OpenModalForm);
    listItemsCourse.on('mouseover', HighlightCourse);
    listItemsCourse.on('mouseout', HighlightCourseOff);
    InitializeCalendar("tblCalendar");
    $("#modalCourseEditor").on('hidden.bs.modal', ClearModalFormFields);
});

function HighlightCourse() {
    var element = $(this);
    var courseEntries = $.grep(entries, function (n, i) { return n.Course == element.attr('tag') });
    for (var i = 0; i < courseEntries.length; i++) {
        var cls = courseEntries[i].Day + "-" + courseEntries[i].Hour;
        $('#tblCalendar .' + cls).addClass('bg-green');
    }
}

function HighlightCourseOff() {
    var element = $(this);
    var courseEntries = $.grep(entries, function (n, i) { return n.Course == element.attr('tag') });
    for (var i = 0; i < courseEntries.length; i++) {
        var cls = courseEntries[i].Day + "-" + courseEntries[i].Hour;
        $('#tblCalendar .' + cls).removeClass('bg-green');
    }
}

function InitializeCalendar(calendarId) {
    for (var i = 0; i < entries.length; i++) {
        var cls = entries[i].Day + "-" + entries[i].Hour;
        $('#' + calendarId + ' .' + cls).addClass('occupied-cell').attr('tag', entries[i].Course);
    }
    var cells = $('.occupied-cell');
    cells.off();
    cells.on('mouseover', ShowTooltip);
    cells.on('mouseout', HideTooltip);
}

function ShowTooltip() {
    var tooltip = $('#tooltip');
    tooltip.appendTo(this);
    var course = $(this).attr('tag');
    if (course !== undefined) {
        tooltip.html(course);
        tooltip.removeClass('hidden');
    }
}

function HideTooltip() {
    var tooltip = $('#tooltip');
    tooltip.html('');
    tooltip.addClass('hidden');
}

function OpenModalForm(event) {
    var element = $(this);
    InitializeCalendar("tblCalendarModal");
    switch (event.data.mode) {
        case Mode.CREATE:
            $('#modalCourseEditor .modal-title').html('Create course');
            break;
        case Mode.EDIT:
            $('#modalCourseEditor .modal-title').html('Edit course');
            $('#CourseId').val(element.attr('id'));
            $('#CourseName').val(element.attr('tag'));
            $('#CourseValue').val(element.attr('value'));
            var courseEntries = $.grep(entries, function (n, i) { return n.Course == element.attr('tag') });
            for (var i = 0; i < courseEntries.length; i++) {
                var cell = $($('#tblCalendarModal .' + courseEntries[i].Day + "-" + courseEntries[i].Hour)[0]);
                cell.removeClass('occupied-cell').removeAttr('tag');
                var entry = {
                    Day: cell.index(),
                    Hour: cell.parent('tr').index()
                }
                selectedTimes.push(entry);
                cell.addClass('bg-green');
            }
            $('#SelectedTimesJson').val(JSON.stringify(selectedTimes));
            $('#btnDeleteCourse').removeClass('hidden');
            $('#btnDeleteCourse').click(DeleteCourse);
            break;
    };
    var freeCells = $('#tblCalendarModal .calendar-cell:not(.occupied-cell)');
    freeCells.off();
    freeCells.on('click', SelectTimeCell);
    $('#modalCourseEditor').modal();
}

function ClearModalFormFields() {
    $('#CourseId').val('');
    $('#CourseName').val('');
    $('#CourseValue').val(0);
    selectedTimes = [];
    $('#SelectedTimesJson').val('');
    $('#tblCalendarModal .calendar-cell').removeClass('bg-green');
    $('#btnDeleteCourse').addClass('hidden');
    $('.field-validation-error').html('');
}

function SelectTimeCell() {
    var entry = {
        Day: $(this).index(),
        Hour: $(this).parent('tr').index()
    }
    var index = selectedTimes.findIndex(i => i.Day === entry.Day && i.Hour === entry.Hour);
    if (index != -1) {
        selectedTimes.splice(index, 1);
        $(this).removeClass('bg-green');
    } else {
        selectedTimes.push(entry);
        $(this).addClass('bg-green');
    }
    $('#SelectedTimesJson').val(JSON.stringify(selectedTimes));
}

function DeleteCourse() {
    var result = confirm("Are you sure?");
    if (result) {
        var createform = document.createElement('form');
        createform.setAttribute("action", "/Home/DeleteCourse");
        createform.setAttribute("method", "post");

        var inputelement = document.createElement('input');
        inputelement.setAttribute("type", "hidden");
        inputelement.setAttribute("name", "courseId");
        inputelement.setAttribute("value", $("#CourseId").val());
        createform.appendChild(inputelement);

        $(document.body).append(createform);
        $(createform).submit();
    }
}