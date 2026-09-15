import { clearToken, getToken, redirectToLogin } from "./auth.js";

const baseUrl = "/api";

async function apiFetch(url, options = {}) {
    const headers = new Headers(options.headers);
    const token = getToken();

    if (token) {
        headers.set("Authorization", `Bearer ${token}`);
    }

    const response = await fetch(url, { ...options, headers });

    if (response.status === 401) {
        clearToken();
        redirectToLogin();
        throw new Error("Sessione scaduta o non autorizzata.");
    }

    return response;
}

export async function getTransactions(filters = {}) {
    const queryParams = new URLSearchParams();

    Object.entries(filters).forEach(([key, value]) => {
        if (value !== null &&
            value !== undefined &&
            value !== "") {
            queryParams.set(key, value);
        }
    });

    const queryString = queryParams.toString();

    const url = queryString
        ? `${baseUrl}/Transactions?${queryString}`
        : `${baseUrl}/Transactions`;

    const response = await apiFetch(url);

    if (!response.ok) {
        throw new Error(
            `Errore durante il caricamento dei movimenti. Status: ${response.status}`
        );
    }

    return await response.json();
}

export async function getTransactionById(transactionId) {
    const response = await apiFetch(`${baseUrl}/Transactions/${transactionId}`);

    if (!response.ok) {
        throw new Error(
            `Errore durante il caricamento del movimento. Status: ${response.status}`
        );
    }

    return await response.json();
}

export async function getCategories() {
    const response = await apiFetch(`${baseUrl}/Categories`);

    if (!response.ok) {
        throw new Error(
            `Errore durante il caricamento delle categorie. Status: ${response.status}`
        );
    }

    return await response.json();
}

export async function createCategory(category) {
    const response = await apiFetch(`${baseUrl}/Categories`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(category)
    });

    if (!response.ok) {
        throw new Error(
            `Errore durante il salvataggio della categoria. Status: ${response.status}`
        );
    }

    return await response.json();
}

export async function deleteCategory(id) {
    const response = await apiFetch(`${baseUrl}/Categories/${id}`, {
        method: "DELETE"
    });

    if (!response.ok) {
        throw new Error(
            `Errore durante l'eliminazione della categoria. Status: ${response.status}`
        );
    }
}

export async function getPaymentMethods() {
    const response = await apiFetch(`${baseUrl}/PaymentMethods`);

    if (!response.ok) {
        throw new Error(
            `Errore durante il caricamento dei metodi di pagamento. Status: ${response.status}`
        );
    }

    return await response.json();
}

export async function createTransaction(transaction) {
    const response = await apiFetch(`${baseUrl}/Transactions`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(transaction)
    });

    if (!response.ok) {
        throw new Error(
            `Errore durante il salvataggio del movimento. Status: ${response.status}`
        );
    }

    return await response.json();
}

export async function updateTransaction(id, transactionData) {
    const response = await apiFetch(`${baseUrl}/Transactions/${id}`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(transactionData)
    });

    if (!response.ok) {
        throw new Error(
            `Errore durante la modifica del movimento. Status: ${response.status}`
        );
    }

    return await response.json();
}

export async function deleteTransaction(id) {
    const response = await apiFetch(`${baseUrl}/Transactions/${id}`, {
        method: "DELETE"
    });

    if (!response.ok) {
        throw new Error(
            `Errore durante l'eliminazione del movimento. Status: ${response.status}`
        );
    }
}

export async function getDashboardSummary(year, month) {
    const params = new URLSearchParams({
        year,
        month
    });

    const response = await apiFetch(`${baseUrl}/DashboardSummary?${params}`);

    if (!response.ok) {
        throw new Error(
            `Errore dashboard. Status: ${response.status}`
        );
    }

    return await response.json();
}
