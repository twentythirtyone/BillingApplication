const getUserExpenses = async (token) => {
    try {
        const apiUrl = 
        (process.env.BACKEND_HOST && process.env.BACKEND_PORT)
            ? `${process.env.BACKEND_HOST}:${process.env.BACKEND_PORT}`
            : 'http://localhost:5183';

        const response = await fetch(`${apiUrl}/subscribers/expenses/month/current`, {
            method: 'GET',
            headers: {
                'Accept': '*/*',
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
        });

        if (!response.ok) {
            throw new Error(`Ошибка: ${response.status}`);
        }

        const data = await response.json();
        return data;
    } catch (error) {
        console.error("Failed to fetch user expenses:", error);
    }
};
export default getUserExpenses;
