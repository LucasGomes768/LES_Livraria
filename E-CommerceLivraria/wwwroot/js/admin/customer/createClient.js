function revealPassword(fieldName) {
    var x = document.getElementById(fieldName);

    if (x.type === "password") {
        x.type = "text"
    } else {
        x.type = "password"
    }
}