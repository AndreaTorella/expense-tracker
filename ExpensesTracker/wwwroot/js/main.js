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
    renderFilterCategories,
    renderPaymentMethods,
    renderFilterPaymentMethods,
    showLoading,
    hideLoading,
    showError,
    getExpenseFormData,
    resetExpenseForm,
    setDefaultExpenseDate,
    hideExpenseModal,
    showToast,
    confirmExpenseDeletion,
    getExpenseFilters,
    getExpenseSorting,
    updateExpensesSummary
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
        renderFilterCategories(categories);
        renderPaymentMethods(paymentMethods);
        renderFilterPaymentMethods(paymentMethods);
        updateExpensesSummary(expenses);
        setDefaultExpenseDate();

        const expenseForm =
            document.querySelector("#expense-form");

        const tableBody =
            document.querySelector("#expenses-table-body");

        const newExpenseButton =
            document.querySelector("#new-expense-button");

        const filtersForm = document.querySelector("#filters-form");

        const resetFiltersButton =
            document.querySelector("#reset-filters-button");

        const expensesSort = document.querySelector("#expenses-sort");

        expenseForm.addEventListener(
            "submit",
            handleExpenseFormSubmit
        );

        tableBody.addEventListener(
            "click",
            handleExpensesTableClick
        );

        filtersForm.addEventListener("submit", handleFiltersSubmit);
        resetFiltersButton.addEventListener("click", handleFiltersReset);
        expensesSort.addEventListener("change", handleSortChange);

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

        const isNewExpense = editingExpenseId === null;

        if (isNewExpense) {
            await saveExpense(expense);
        } else {
            await modifyExpense(editingExpenseId, expense);
        }

        editingExpenseId = null;
        resetExpenseForm();
        setDefaultExpenseDate();
        hideExpenseModal();

        await refreshExpenses();
        showToast(isNewExpense ? "Spesa aggiunta correttamente" : "Spesa modificata");
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

        const expenseToBeUpdated =
            await loadExpenseById(expenseId);

        editingExpenseId = expenseId;

        populateExpenseForm(expenseToBeUpdated);

        const modalTitle = document.querySelector("#expense-modal-title");

        modalTitle.textContent = "Modifica spesa";

        const modalElement = document.querySelector("#expense-modal");

        const modal = bootstrap.Modal.getOrCreateInstance(modalElement);

        modal.show();
    } catch (error) {
        console.error(error);
        showError(
            "Non è stato possibile caricare la spesa da modificare."
        );
    } finally {
        hideLoading();
    }
}

async function handleDeleteExpense(deleteButton) {
    const expenseId = Number(deleteButton.dataset.expenseId);

    const confirmed = await confirmExpenseDeletion();

    if (!confirmed) {
        return;
    }

    try {
        showLoading();

        await removeExpense(expenseId);

        await refreshExpenses();
        showToast("Spesa eliminata");
    } catch (error) {
        console.error(error);
        showError("Non è stato possibile eliminare la spesa.");
    } finally {
        hideLoading();
    }
}

async function handleFiltersSubmit(event) {
    event.preventDefault();

    const filters = getExpenseFilters();

    if (filters.FromDate && filters.ToDate && filters.FromDate > filters.ToDate) {
        showError("La data iniziale non può essere successiva alla data finale.");
        return;
    }

    try {
        showLoading();
        await refreshExpenses(filters);
    } catch (error) {
        console.error(error);
        showError("Non è stato possibile applicare i filtri alle spese.");
    } finally {
        hideLoading();
    }
}

async function handleFiltersReset() {
    document.querySelector("#filters-form").reset();

    try {
        showLoading();
        await refreshExpenses({});
    } catch (error) {
        console.error(error);
        showError("Non è stato possibile ripristinare l'elenco delle spese.");
    } finally {
        hideLoading();
    }
}

async function handleSortChange() {
    try {
        showLoading();
        await refreshExpenses();
    } catch (error) {
        console.error(error);
        showError("Non è stato possibile ordinare le spese.");
    } finally {
        hideLoading();
    }
}

async function refreshExpenses(filters = getExpenseFilters()) {
    const expenses = await loadExpenses({
        ...filters,
        ...getExpenseSorting()
    });

    renderExpenses(expenses);
    updateExpensesSummary(expenses);
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
