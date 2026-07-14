import {
    loadExpenses,
    loadCategories,
    loadPaymentMethods,
    saveExpense
} from "./expenses.js";

import {
    renderExpenses,
    renderCategories,
    renderPaymentMethods,
    showLoading,
    hideLoading,
    showError,
    getExpenseFormData,
    resetExpenseForm
} from "./ui.js";

async function initializePage() {
    try {
        showLoading();

        const [
            expenses,
            categories,
            paymentMethods
        ] = await Promise.all([
            loadExpenses(),
            loadCategories(),
            loadPaymentMethods()
        ]);

        renderExpenses(expenses);
        renderCategories(categories);
        renderPaymentMethods(paymentMethods);

        const expenseForm = document.querySelector("#expense-form");

        expenseForm.addEventListener(
            "submit",
            handleExpenseFormSubmit
        );

    } catch (error) {
        console.error(error);

        showError("Non è stato possibile caricare i dati.");
    } finally {
        hideLoading();
    }
}

async function handleExpenseFormSubmit(event) {
    event.preventDefault();

    try {
        const expense = getExpenseFormData();

        await saveExpense(expense);

        resetExpenseForm();

        const expenses = await loadExpenses();
        renderExpenses(expenses);
    } catch (error) {
        console.error(error);
        showError("Non è stato possibile salvare la spesa.");
    }
}

initializePage();