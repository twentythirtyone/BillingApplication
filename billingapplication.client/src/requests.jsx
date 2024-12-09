import getUserExpenses from './main-page/expences.jsx'

export const getTariff = async (token) => {
    try {
        const response = await fetch('https://localhost:7262/tariff', {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`,
            },
        });

        if (!response.ok) {
            throw new Error(`Ошибка: ${response.status}`);
        }

        const data = await response.json();
        return data;
    } catch (error) {
        console.error('Failed to get tariff data', error);
    }
};

export const fetchExpenses = async () => {
    const token = localStorage.getItem('token');
    if (token) {
        try {
            const expenses = await getUserExpenses(token);
            return expenses;
        } catch (error) {
            console.error("Failed to fetch user expenses:", error);
        }
    }
};

export const changeTariff = async (tariffId) => {
    const token = localStorage.getItem('token');
    const response = await fetch(`https://localhost:7262/subscribers/tariff/change/${tariffId}`, {
        method: 'POST',
        headers: {
            Authorization: `Bearer ${token}`,
        },
    });
    if (!response.ok) {
        throw new Error('Ошибка при смене тарифа');
    }
    return response.json();
};

export const payTariff = async (tariffId) => {
    const token = localStorage.getItem('token');
    const response = await fetch(`https://localhost:7262/subscribers/tariff/pay`, {
        method: 'POST',
        headers: {
            Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({ tariffId }),
    });
    if (!response.ok) {
        throw new Error('Ошибка при оплате тарифа');
    }
    return response.json();
};
