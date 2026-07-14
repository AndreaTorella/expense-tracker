import {
    getExpenses,
    getCategories,
    getPaymentMethods,
    createExpense,
    deleteExpense as deleteExpenseApi
} from "./api.js";

export async function loadExpenses() {
    return await getExpenses();
}

export async function loadCategories() {
    return await getCategories();
}

export async function loadPaymentMethods() {
    return await getPaymentMethods();
}

export async function saveExpense(expense) {
    return await createExpense(expense)
}

export async function removeExpense(id) {
    await deleteExpenseApi(id);
}

// filterExpensesByMonth
// calculateTotal
// sortExpenses
// createExpense
// updateExpense
// deleteExpense