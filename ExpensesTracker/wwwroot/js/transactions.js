import {
    getTransactions,
    getTransactionById,
    getCategories,
    getPaymentMethods,
    createTransaction,
    updateTransaction,
    deleteTransaction as deleteTransactionApi
} from "./api.js";

export async function loadTransactions(filters = {}) {
    return await getTransactions(filters);
}

export async function loadTransactionById(id) {
    return await getTransactionById(id);
}

export async function loadCategories() {
    return await getCategories();
}

export async function loadPaymentMethods() {
    return await getPaymentMethods();
}

export async function saveTransaction(transaction) {
    return await createTransaction(transaction);
}

export async function modifyTransaction(id, transactionData) {
    return await updateTransaction(id, transactionData);
}

export async function removeTransaction(id) {
    await deleteTransactionApi(id);
}
