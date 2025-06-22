function invertPopUpDisplay(id) {
    const element = document.getElementById(id)

    element.style.display = element.style.display != 'flex' ? 'flex' : 'none'
}