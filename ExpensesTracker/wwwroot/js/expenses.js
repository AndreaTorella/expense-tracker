import {
    getExpenses,
    getExpenseById,
    getCategories,
    getPaymentMethods,
    createExpense,
    updateExpense,
    deleteExpense as deleteExpenseApi
} from "./api.js";

export async function loadExpenses() {
    return await getExpenses();
}

export async function loadExpenseById(id) {
    return await getExpenseById(id);
}

export async function loadCategories() {
    return await getCategories();
}

export async function loadPaymentMethods() {
    return await getPaymentMethods();
}

export async function saveExpense(expense) {
    return await createExpense(expense);
}

export async function modifyExpense(id, expenseData) {
    return await updateExpense(id, expenseData);
}

export async function removeExpense(id) {
    await deleteExpenseApi(id);
}

// filterExpensesByMonth
// calculateTotal
// sortExpenses
// createExpense