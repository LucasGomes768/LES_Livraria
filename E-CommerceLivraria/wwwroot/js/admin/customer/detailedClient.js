function showPasswordChange() {
    document.getElementById("divPassChangePopUp").style.display = "block"
}

function hidePasswordChange() {
    document.getElementById("divPassChangePopUp").style.display = "none"
}

function revealPassword(fieldName) {
    var x = document.getElementById(fieldName);

    if (x.type === "password") {
        x.type = "text"
    } else {
        x.type = "password"
    }
}