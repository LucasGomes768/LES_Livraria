window.addEventListener("DOMContentLoaded", () => {
    showOneSection("dadosGerais");
    showItemSection("purchaseItems");
})

function showOneSection(section) {
    const sections = document.getElementsByClassName("dataSectionJs")

    for (let i = 0; i < sections.length; i++) {
        sections.item(i).style.display = "none"
    }

    document.getElementById(section).style.display = "block"
}
function showItemSection(section) {
    const sections = document.getElementsByClassName("items");
    const buttons = document.getElementsByClassName("itemsSelectBtn");

    for (let i = 0; i < sections.length; i++) {
        sections.item(i).style.display = "none";
    }

    document.getElementById(section).style.display = "flex";
}

function updateStatusAll(id, newStatus) {
    const request = new XMLHttpRequest()
    request.open('PUT', '/AdmPurchases/UpdatePurchaseStatus', false)
    request.setRequestHeader('Content-Type', 'application/json')

    const data = {
        PrcId: id,
        StcId: null,
        Status: newStatus
    }

    try {
        request.send(JSON.stringify(data));

        if (request.status === 200) {
            alert("Status da compra atualizado")
            window.location.reload()
        } else {
            alert('Erro ao atualizar a compra: ' + request.statusText)
        }

    } catch (error) {
        alert('Falha na comunicação: ' + error.message)
        console.error(error)
    }
}

function updateItemStatus(prcId, stcId, newStatus) {
    const request = new XMLHttpRequest()
    request.open('PUT', '/AdmPurchases/UpdatePurchaseItemStatus', false)
    request.setRequestHeader('Content-Type', 'application/json')

    const data = {
        PrcId: prcId,
        StcId: stcId,
        Status: newStatus
    }

    try {
        request.send(JSON.stringify(data));

        if (request.status === 200) {
            alert("Status do item atualizado")
            window.location.reload()
        } else {
            alert('Erro ao atualizar a compra: ' + request.statusText)
        }

    } catch (error) {
        alert('Falha na comunicação: ' + error.message)
    }
}