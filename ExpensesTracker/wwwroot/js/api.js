const baseUrl = "/api";

export async function getExpenses() {
    const response = await fetch(`${baseUrl}/Expenses`);

    if (!response.ok) {
        throw new Error(
            `Errore durante il caricamento delle spese. Status: ${response.status}`
        );
    }

    return await response.json();
}

export async function getCategories() {
    const response = await fetch(`${baseUrl}/Categories`);

    if (!response.ok) {
        throw new Error(
            `Errore durante il caricamento delle categorie. Status: ${response.status}`
        );
    }

    return await response.json();
}

export async function getPaymentMethods() {
    const response = await fetch(`${baseUrl}/PaymentMethods`);

    if (!response.ok) {
        throw new Error(
            `Errore durante il caricamento dei metodi di pagamento. Status: ${response.status}`
        );
    }

    return await response.json();
}

export async function createExpense(expense) {
    const response = await fetch(`${baseUrl}/Expenses`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(expense)
    });

    if (!response.ok) {
        throw new Error(
            `Errore durante il salvataggio della spesa. Status: ${response.status}`
        );
    }

    return await response.json();
}

export async function updateExpense(id, expenseData) {
    const response = await fetch(`${baseUrl}/Expenses/${id}`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(expenseData)
    });

    if (!response.ok) {
        throw new Error(
            `Errore durante la modifica della spesa. Status: ${response.status}`
        );
    }

    return await response.json();
}

export async function deleteExpense(id) {
    const response = await fetch(`${baseUrl}/Expenses/${id}`, {
        method: "DELETE"
    });

    if (!response.ok) {
        throw new Error(
            `Errore durante l'eliminazione della spesa. Status: ${response.status}`
        );
    }
}