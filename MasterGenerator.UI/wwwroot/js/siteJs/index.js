
function loadpage() {
    $("#refreshgred").hide();
    $("#loader").show();
}

function valueAccessorFn(field, data, column) {
    // debugger

    var value = data[field];
    value = 'CS ID- ' + value;
    return value;
}
function ChangeIcon(e) {
    var i = e.children[0];
    var c = i.attributes['1']
    var id = i.attributes['0'];
    var btnid = "#" + id.ownerElement.id;
    var className = c.ownerElement.className;
    if (className == 'metismenu-state-icon pe-7s-angle-up caret-left') {
        $(btnid).removeClass("metismenu-state-icon pe-7s-angle-up caret-left").addClass("metismenu-state-icon pe-7s-angle-down caret-left");
    }
    else {
        $(btnid).removeClass("metismenu-state-icon pe-7s-angle-down caret-left").addClass("metismenu-state-icon pe-7s-angle-up caret-left");
    }

}
