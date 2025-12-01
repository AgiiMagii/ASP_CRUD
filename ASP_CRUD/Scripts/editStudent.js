
let nameInput = document.getElementById('nameInput');

function validateName() {
    let name = nameInput.value.trim();
    if (name != null || name != "") {
        if (name.length < 3 || name.length > 50) {
            alert("Name must be between 3 and 50 characters.");
            name = name.substring(0, 50);

        }
        if (isDigit(name)) {
            alert("Name cannot contain digits.");
            name = name.substring(0, name.length - 1);
        }
    }
    nameInput.value = name;
}
function isDigit(str) {
    if (!str.match(/\d/)) {
        return false;
    }
    return true;
}
    