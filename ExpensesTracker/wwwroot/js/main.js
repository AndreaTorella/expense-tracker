import {
    loadExpenses,
    loadCategories,
    loadPaymentMethods,
    saveExpense,
    loadExpenseById,
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

let editingExpenseId = null;

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

        const expenseForm =
            document.querySelector("#expense-form");

        const tableBody =
            document.querySelector("#expenses-table-body");

        const newExpenseButton =
            document.querySelector("#new-expense-button");

        expenseForm.addEventListener(
            "submit",
            handleExpenseFormSubmit
        );

        tableBody.addEventListener(
            "click",
            handleExpensesTableClick
        );

        newExpenseButton.addEventListener("click", () => {
            editingExpenseId = null;

            resetExpenseForm();
            setDefaultExpenseDate();

            const modalTitle =
                document.querySelector("#expense-modal-title");

            modalTitle.textContent = "Nuova spesa";
        });
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

        if (editingExpenseId === null) {
            await saveExpense(expense);
        } else {
            await modifyExpense(editingExpenseId, expense)
        }

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

        const expenseTobeUpdated = await loadExpenseById(expenseId);
        editingExpenseId = expenseId; //Usato per distinguere save/update nel submit

        populateExpenseForm(expenseTobeUpdated);

        const modalElement = document.getElementById("expense-modal");
        const modal = bootstrap.Modal.getOrCreateInstance(modalElement);

        modal.show();
    } catch (error) {
        console.error(error);
        showError("Non è stato possibile modificare la spesa.");
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

function populateExpenseForm(expense) {
    document.querySelector("#title").value = expense.title;
    document.querySelector("#amount").value = expense.amount;
    document.querySelector("#date").value =
        expense.date.split("T")[0];

    document.querySelector("#category").value =
        String(expense.categoryId);

    document.querySelector("#payment-method").value =
        String(expense.paymentMethodId);
}

initializePage();
