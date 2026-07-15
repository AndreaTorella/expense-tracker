import {
    loadExpenses,
    loadCategories,
    loadPaymentMethods,
    saveExpense,
    removeExpense
} from "./expenses.js";

import {
    renderExpenses,
    renderCategories,
    renderPaymentMethods,
    showLoading,
    hideLoading,
    showError,
    getExpenseFormData,
    resetExpenseForm,
    setDefaultExpenseDate,
    hideExpenseModal
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
        setDefaultExpenseDate();

        const expenseForm = document.querySelector("#expense-form");

        expenseForm.addEventListener(
            "submit",
            handleExpenseFormSubmit
        );

        const tableBody = document.querySelector("#expenses-table-body");

        tableBody.addEventListener(
            "click",
            handleExpensesTableClick
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
    showLoading();

    try {
        const expense = getExpenseFormData();

        await saveExpense(expense);

        resetExpenseForm();
        setDefaultExpenseDate();
        hideExpenseModal();

        const expenses = await loadExpenses();
        renderExpenses(expenses);
    } catch (error) {
        console.error(error);
        showError("Non è stato possibile salvare la spesa.");
    } finally {
        hideLoading();
    }
}

async function handleExpensesTableClick(event) {
    const deleteButton = event.target.closest(".delete-expense-button");

    if (!deleteButton) {
        return;
    }

    const expenseId = Number(deleteButton.dataset.expenseId);

    const confirmed = window.confirm(
        "Sei sicuro di voler eliminare questa spesa?"
    );

    if (!confirmed) {
        return;
    }

    try {
        showLoading();

        await removeExpense(expenseId);

        const expenses = await loadExpenses();
        renderExpenses(expenses);
    } catch (error) {
        console.error(error);
        showError("Non è stato possibile eliminare la spesa.");
    } finally {
        hideLoading();
    }
}

initializePage();
