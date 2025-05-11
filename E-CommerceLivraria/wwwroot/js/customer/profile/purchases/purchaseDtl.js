function showOneSection(section) {
    const sections = document.getElementsByClassName("dataSectionJs")

    for (let i = 0; i < sections.length; i++) {
        sections.item(i).style.display = "none"
    }

    document.getElementById(section).style.display = "block"
}