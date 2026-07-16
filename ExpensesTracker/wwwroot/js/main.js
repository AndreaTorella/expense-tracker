import {
    loadExpenses,
    loadCategories,
    loadPaymentMethods,
    saveExpense,
    modifyExpense,
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
    const updateButton = event.target.closest(".edit-expense-button");
    const deleteButton = event.target.closest(".delete-expense-button");

    if (updateButton) {
        await handleEditExpense(updateButton);
        return;
    }

    if (deleteButton) {
        await handleDeleteExpense(deleteButton);
    }
}

async function handleEditExpense(updateButton) {
    const expenseId = Number(updateButton.dataset.expenseId);

    try {
        showLoading();

        const expenseTobeUpdated = await getExpenseById(expenseId);
        populateExpenseForm(expenseTobeUpdated);

        const modalElement = document.getElementById("expense-modal");
        const modal = bootstrap.Modal.getOrCreateInstance(modalElement);

        modal.show();
        renderExpenses(expenses);
    } catch (error) {
        console.error(error);
        showError("Non è stato possibile eliminare la spesa.");
    } finally {
        hideLoading();
    }

}

async function handleDeleteExpense(deleteButton) {
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

export function populateExpenseForm(expense) {
    const titleInput = document.querySelector("#title");
    const amountInput = document.querySelector("#amount");
    const dateInput = document.querySelector("#date");
    const categorySelect = document.querySelector("#category");
    const paymentMethodSelect = document.querySelector("#payment-method");

    titleInput.value = expense.title;
    amountInput.value = expense.amount;
    dateInput.value = expense.date.split("T")[0];

    selectOptionByText(categorySelect, expense.categoryName);
    selectOptionByText(
        paymentMethodSelect,
        expense.paymentMethodName
    );
}

function selectOptionByText(selectElement, text) {
    const matchingOption = Array.from(selectElement.options)
        .find(option => option.textContent.trim() === text);

    if (!matchingOption) {
        selectElement.value = "";
        return;
    }

    selectElement.value = matchingOption.value;
}

initializePage();
