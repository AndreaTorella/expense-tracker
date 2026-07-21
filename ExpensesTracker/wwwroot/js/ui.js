let loadingRequests = 0;

export function renderExpenses(expenses) {
    const tableBody = document.querySelector("#expenses-table-body");

    tableBody.innerHTML = "";

    for (const expense of expenses) {
        const row = document.createElement("tr");

        row.innerHTML = `
        <td>${formatDate(expense.date)}</td>
        <td>
            <div class="expense-title">${expense.title}</div>
            <div class="expense-meta">ID ${expense.id}</div>
        </td>
        <td><span class="category-pill">${expense.categoryName}</span></td>
        <td><span class="payment-pill">${expense.paymentMethodName}</span></td>
        <td class="text-end expense-amount">${formatAmount(expense.amount)}</td>
        <td>
            <div class="row-actions">
            <button
                type="button"
                class="btn btn-outline-secondary btn-sm edit-expense-button"
                data-expense-id="${expense.id}">
                <i class="bi bi-pencil-square" aria-hidden="true"></i>
                <span class="visually-hidden">Modifica spesa</span>
            </button>

            <button
                type="button"
                class="btn btn-outline-danger btn-sm delete-expense-button"
                data-expense-id="${expense.id}">
                <i class="bi bi-trash3" aria-hidden="true"></i>
                <span class="visually-hidden">Elimina spesa</span>
            </button>
            </div>
        </td>
        `;

        tableBody.appendChild(row);
    }
}

export function renderCategories(categories) {
    appendSelectOptions("#category", categories);
}

export function renderFilterCategories(categories) {
    appendSelectOptions("#filter-category", categories);
}

export function renderPaymentMethods(paymentMethods) {
    appendSelectOptions("#payment-method", paymentMethods);
}

export function renderFilterPaymentMethods(paymentMethods) {
    appendSelectOptions("#filter-payment-method", paymentMethods);
}

export function getExpenseFilters() {
    return {
        Search: document.querySelector("#filter-search").value.trim(),
        FromDate: document.querySelector("#filter-from-date").value,
        ToDate: document.querySelector("#filter-to-date").value,
        CategoryId: document.querySelector("#filter-category").value,
        PaymentMethodId: document.querySelector("#filter-payment-method").value
    };
}

export function getExpenseSorting() {
    const [expenseSortBy, sortDirection] =
        document.querySelector("#expenses-sort").value.split(":");

    return {
        ExpenseSortBy: expenseSortBy,
        SortDirection: sortDirection
    };
}

export function updateExpensesSummary(expenses) {
    const summary = document.querySelector("#expenses-summary");
    const count = expenses.length;

    summary.textContent = count === 1
        ? "1 spesa trovata"
        : `${count} spese trovate`;
}

export function getExpenseFormData() {
    const titleInput = document.querySelector("#title");
    const amountInput = document.querySelector("#amount");
    const dateInput = document.querySelector("#date");
    const categorySelect = document.querySelector("#category");
    const paymentMethodSelect = document.querySelector("#payment-method");

    return {
        title: titleInput.value.trim(),
        amount: Number(amountInput.value),
        date: `${dateInput.value}T00:00:00`,
        categoryId: Number(categorySelect.value),
        paymentMethodId: Number(paymentMethodSelect.value)
    };
}

export function setDefaultExpenseDate() {
    const dateInput = document.querySelector("#date");

    if (!dateInput.value) {
        dateInput.value = new Date().toISOString().split("T")[0];
    }
}

export function resetExpenseForm() {
    const form = document.querySelector("#expense-form");
    form.reset();
}

export function hideExpenseModal() {
    const modalElement = document.querySelector("#expense-modal");

    if (!modalElement || !window.bootstrap) {
        return;
    }

    const modal = window.bootstrap.Modal.getInstance(modalElement);

    if (modal) {
        modal.hide();
    }
}

export function showLoading() {
    const loadingOverlay = getLoadingOverlay();

    loadingRequests += 1;

    loadingOverlay.hidden = false;
    document.body.classList.add("app-is-loading");
    document.body.setAttribute("aria-busy", "true");
}

export function hideLoading() {
    const loadingOverlay = getLoadingOverlay();

    loadingRequests = Math.max(loadingRequests - 1, 0);

    if (loadingRequests > 0) {
        return;
    }

    loadingOverlay.hidden = true;
    document.body.classList.remove("app-is-loading");
    document.body.removeAttribute("aria-busy");
}

export function showError(message) {
    const errorMessage = document.querySelector("#error-message");

    errorMessage.textContent = message;
    errorMessage.hidden = false;
    showToast(message, "error");
}

export function showToast(message, type = "success") {
    const toastContainer = document.querySelector("#toast-container");

    if (!toastContainer || !window.bootstrap) {
        return;
    }

    const isError = type === "error";
    const toastElement = document.createElement("div");

    toastElement.className = `toast app-toast ${isError ? "app-toast-error" : "app-toast-success"}`;
    toastElement.setAttribute("role", "status");
    toastElement.setAttribute("aria-live", isError ? "assertive" : "polite");
    toastElement.setAttribute("aria-atomic", "true");
    toastElement.innerHTML = `
        <div class="toast-body">
            <i class="bi ${isError ? "bi-exclamation-circle-fill" : "bi-check-circle-fill"}" aria-hidden="true"></i>
            <span></span>
            <button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Chiudi"></button>
        </div>
    `;

    toastElement.querySelector("span").textContent = message;
    toastContainer.appendChild(toastElement);

    toastElement.addEventListener("hidden.bs.toast", () => {
        toastElement.remove();
    });

    window.bootstrap.Toast.getOrCreateInstance(toastElement, {
        delay: 5000
    }).show();
}

export function confirmExpenseDeletion() {
    const modalElement = document.querySelector("#delete-expense-modal");
    const confirmButton = document.querySelector("#confirm-delete-expense-button");

    if (!modalElement || !confirmButton || !window.bootstrap) {
        return Promise.resolve(false);
    }

    return new Promise((resolve) => {
        const modal = window.bootstrap.Modal.getOrCreateInstance(modalElement);
        let confirmed = false;

        const handleConfirmation = () => {
            confirmed = true;
            modal.hide();
        };

        const handleModalHidden = () => {
            confirmButton.removeEventListener("click", handleConfirmation);
            resolve(confirmed);
        };

        confirmButton.addEventListener("click", handleConfirmation);
        modalElement.addEventListener("hidden.bs.modal", handleModalHidden, { once: true });
        modal.show();
    });
}

function formatDate(dateValue) {
    const date = new Date(dateValue);

    return date.toLocaleDateString("it-IT");
}

function appendSelectOptions(selector, options) {
    const select = document.querySelector(selector);

    for (const optionData of options) {
        const option = document.createElement("option");

        option.value = optionData.id;
        option.textContent = optionData.name;

        select.appendChild(option);
    }
}

function formatAmount(amount) {
    return amount.toLocaleString("it-IT", {
        style: "currency",
        currency: "EUR"
    });
}

function getLoadingOverlay() {
    let loadingOverlay = document.querySelector("#app-loading-overlay");

    if (loadingOverlay) {
        return loadingOverlay;
    }

    loadingOverlay = document.createElement("div");
    loadingOverlay.id = "app-loading-overlay";
    loadingOverlay.className = "loading-overlay";
    loadingOverlay.setAttribute("role", "status");
    loadingOverlay.setAttribute("aria-live", "polite");
    loadingOverlay.hidden = true;
    loadingOverlay.innerHTML = `
        <div class="loading-spinner" aria-hidden="true">
            <span></span>
            <span></span>
            <span></span>
            <span></span>
            <span></span>
            <span></span>
            <span></span>
            <span></span>
            <span></span>
            <span></span>
            <span></span>
            <span></span>
        </div>
        <span class="visually-hidden">Operazione in corso...</span>
    `;

    document.body.appendChild(loadingOverlay);

    return loadingOverlay;
}
