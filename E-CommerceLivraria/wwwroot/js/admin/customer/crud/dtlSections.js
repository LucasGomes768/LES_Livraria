document.addEventListener("DOMContentLoaded", function () {
    showDataSection(undefined)
})

function showDataSection(triggerBtn) {
    const btns = document.getElementsByClassName('dataSectionBtns');
    const extraInfoSections = document.getElementsByClassName('extraInfoSections');

    for (let btn of btns) {
        btn.disabled = false;
    }

    for (let section of extraInfoSections) {
        section.style.display = 'none';
    }

    if (!triggerBtn) return
    triggerBtn.disabled = true;
    document.getElementById(triggerBtn.value).style.display = 'flex';
}

function showAddSection(triggerBtn) {
    const adds = document.getElementsByClassName('addSection');
    const addBtns = document.getElementsByClassName('addSectionBtn');

    for (let add of adds) {
        add.style.display = 'none'
    }

    for (let btn of addBtns) {
        btn.disabled = false;
    }

    console.log(triggerBtn.value)
    triggerBtn.disabled = true;

    const sectionDiv = document.getElementById(triggerBtn.value);
    sectionDiv.style.display = 'flex';
}