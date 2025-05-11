const uri = 'api/Flats';
const districtsUri = 'api/Districts';

let flats = [];
let districts = [];
let modal;

document.addEventListener('DOMContentLoaded', () => {
    modal = new bootstrap.Modal(document.getElementById('flatModal'));
    loadInitialData();
});

function loadInitialData() {
    fetch(districtsUri)
        .then(response => response.json())
        .then(data => {
            districts = data;
            populateDistrictSelect();  
            getFlats();
        })
        .catch(error => console.error('Не вдалося завантажити райони.', error));
}

function populateDistrictSelect() {
    const select = document.getElementById('districtId');
    if (!select) {
        return;
    }
    select.innerHTML = ''; 
    districts.forEach(d => {
        const option = document.createElement('option');
        option.value = d.id;
        option.textContent = d.dsName || d.DsName;
        select.appendChild(option);
    });
}

function getFlats() {
    fetch(uri)
        .then(response => response.json())
        .then(data => {
            flats = data;
            displayFlats(data);
        })
        .catch(error => console.error('Помилка отримання квартир.', error));
}

function displayFlats(data) {
    const tBody = document.getElementById('flats');
    tBody.innerHTML = '';

    flats = data;

    data.forEach(flat => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${flat.flAddr}</td>
            <td>${flat.flArea}</td>
            <td>${flat.flRooms}</td>
            <td>${flat.flPrice}</td>
            <td>${getDistrictName(flat.districtId)}</td>
            <td>
                <button class="btn btn-outline-primary btn-sm" onclick="showEditForm(${flat.id})">Редагувати</button>
                <button class="btn btn-outline-secondary btn-sm" onclick="viewFlatDetails(${flat.id})">Деталі</button>
                <button class="btn btn-danger btn-sm" onclick="deleteFlat(${flat.id})">Видалити</button>
            </td>
        `;
        tBody.appendChild(row);
    });
}

function getDistrictName(districtId) {
    const parsedId = parseInt(districtId);
    if (isNaN(parsedId)) {
        return 'Не вказано';
    }

    const district = districts.find(d => d.id === parsedId);
    return district ? (district.dsName || district.DsName) : 'Не вказано';
}

function deleteFlat(id) {
    if (confirm("Ви впевнені, що хочете видалити квартиру?")) {
        fetch(`${uri}/${id}`, { method: 'DELETE' })
            .then(() => getFlats())
            .catch(error => console.error('Не вдалося видалити квартиру.', error));
    }
}

function showAddForm() {
    document.getElementById('flatForm').reset();
    document.getElementById('flatId').value = '';
    document.getElementById('flatModalLabel').textContent = 'Додати квартиру';

    populateDistrictSelect(); 
    modal.show();
}

function showEditForm(id) {
    const flat = flats.find(f => f.id === id);
    if (flat) {
        populateDistrictSelect();

        document.getElementById('flatId').value = flat.id;
        document.getElementById('flAddr').value = flat.flAddr;
        document.getElementById('flArea').value = flat.flArea;
        document.getElementById('flRooms').value = flat.flRooms;
        document.getElementById('flPrice').value = flat.flPrice;
        document.getElementById('districtId').value = flat.districtId;

        document.getElementById('flatModalLabel').textContent = 'Редагувати квартиру';
        modal.show();
    }
}

function submitFlat(event) {
    event.preventDefault();
    const id = document.getElementById('flatId').value;
    const flAddr = document.getElementById('flAddr').value;
    const flArea = parseFloat(document.getElementById('flArea').value);
    const flRooms = parseInt(document.getElementById('flRooms').value);
    const flPrice = parseFloat(document.getElementById('flPrice').value);
    const districtId = parseInt(document.getElementById('districtId').value);

    if (!flAddr || isNaN(flArea) || flArea <= 0 ||
        isNaN(flRooms) || flRooms <= 0 ||
        isNaN(flPrice) || flPrice <= 0 ||
        isNaN(districtId) || districtId <= 0) {
        showError("Будь ласка, заповніть всі поля правильно (усі числові значення повинні бути більше 0).");
        return;
    }

    const flat = { flAddr, flArea, flRooms, flPrice, DsId: districtId };
    const method = id ? 'PUT' : 'POST';
    const url = id ? `${uri}/${id}` : uri;
    if (id) flat.id = parseInt(id);

    fetch(url, {
        method: method,
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(flat)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error(`Помилка на сервері: ${response.statusText}`);
            }

            if (response.status === 204) {
                return null; 
            }

            return response.json();
        })
        .then(data => {
            modal.hide();
            getFlats();
        })
        .catch(error => {
            console.error('Помилка збереження:', error);
            showError("Сталася помилка при збереженні даних. Перевірте консоль для деталей.");
        });
}

function showError(message) {
    alert(message); 
}

function viewFlatDetails(id) {
    alert(`Детальна інформація про квартиру ID = ${id}`);
}










